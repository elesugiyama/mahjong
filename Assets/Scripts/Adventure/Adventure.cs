using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Const;
using AdventureDefine;

public class Adventure : SceneBase {
//-*仮データ用
	private const int STAGEMAXNUM_TEST = 10;
//-----------------------------------------------
// ステージセレクトメニューの定義
//-----------------------------------------------
	private enum ADVENTUREMODE {
	// シナリオmode
		aMODE_INIT,				// 0
		aMODE_TEXT_DRAW,		// 4
		aMODE_PAGE_WAIT,		// 5
		aMODE_SAVE, // 22
		aMODE_ASC_CONTINUE, // 23
		aMODE_DIALOG_TRIAL, // 24
		//-*************
		aMODE_CHOICE_WAIT,		//-*選択肢待ち
		aMODE_EFFECT_SET,		//-*エフェクト開始
		aMODE_EFFECT_WAIT,		//-*エフェクト終了待ち
		aMODE_EFFECT_ERR,		//-*エラー

		aMODE_MAX,		//*最大数
	};
	private enum SCENARIOEFFECT {
	//-*演出
		aEFFCT_PARSE,			// 1
		aEFFCT_FLASH,			// 2
		aEFFCT_QUAKE,			// 3
		aEFFCT_CHOISE,			// 6
		aEFFCT_WAIT,				// 7
		aEFFCT_WAITe,			// 8
		aEFFCT_WAIT_BLACK,		// 9
		aEFFCT_DISPWAIT,			// 10
		aEFFCT_GAMEOVER,			// 11
		aEFFCT_GAMECLEAR00,		// 12
		aEFFCT_GAMECLEAR01,		// 13
		aEFFCT_GAMECLEAR02,		// 14
		aEFFCT_GAMECLEAR03,		// 15
		aEFFCT_BLACKOUT,			// 16
		aEFFCT_BLACKOUT_S,		// 17
		aEFFCT_BLACKIN,			// 18
		aEFFCT_WHITEOUT,			// 19
		aEFFCT_WHITEIN,			// 20
		aEFFCT_JUMPWAIT,			// 21
	};
	//-*選択肢
	[SerializeField]
	private GameObject m_choiceObjBase;
	//-*選択肢ボタン
	private List<GameObject> m_choiceButton;

	//-*ページ送り
	[SerializeField]
	private GameObject m_nextButton;	//-*ページ送りボタン
	[SerializeField]
	private GameObject m_pageWaitIcon;	//-*ページ送り待ちアイコン

	//-*背景
	[SerializeField]
	private GameObject m_bgObjBase;
	private GameObject m_bgObj;
	private AdventureBg m_bgScript;
	//-*キャラクター
	[SerializeField]
	private GameObject m_charaObjBase;
	private GameObject m_charaObj;
	private AdventureChara m_CharaScript;

	// [SerializeField]
	// private Image m_effectFade;

	[SerializeField]
	private spriteUINumS test;

	private ADVENTUREMODE m_Mode = ADVENTUREMODE.aMODE_MAX;	//-*モード
	private ADVENTUREMODE m_ModeNext = ADVENTUREMODE.aMODE_MAX;	//-*次のモード
	private ScreenEffect.EFFECTTYPE m_EffectType = ScreenEffect.EFFECTTYPE.NONE;	//-*演出
	public Canvas UICanvas;     //UIを表示するキャンバス


	//-********************
	//-*シナリオテキスト関連
	//-*todo:独立させるかも
	//-********************
	//-*読み込んだテキスト出力用
	[SerializeField]
	private Text[] m_textLine;	//-*1行目
	List<string> m_textData = new List<string>();         //空のListを作成する
	
	//-*発言者名
	[SerializeField]
	private GameObject m_SpeakerNames;	//-*名前が画像

	//-*現在の読み込み行番号
	private int m_textLineNum;
	private int m_pageLineNum;
	//-*ページ区切りまで読んだか
	private bool m_isPageWait = false;
	//-*選択肢選択待ち
	private bool m_isChoiceWait = false;
	//-*演出待ち
	private bool m_isEffectWait = false;
	//-*フェードイン
 	private bool m_effectFadeIn = false;

	// Use this for initialization
	protected override void Start () {
		Debug.Log("//-*start");
#if true
		//-*シナリオデバッグ
		m_gameData.AdvNextScoNo = "13";
#endif
		m_Mode = ADVENTUREMODE.aMODE_INIT;
		StartCoroutine("UpdateAdventure");
	}
	
	// Update is called once per frame
	protected override void Update () {}

	private IEnumerator Init(){
		//-*背景
		if(m_bgObj == null){
			GameObject objBG = (GameObject)Resources.Load("Prefabs/adventureBG");
			m_bgObj = (GameObject)Instantiate(objBG, Vector3.zero, Quaternion.identity);
			m_bgObj.transform.SetParent(m_bgObjBase.transform, false);
			m_bgScript = m_bgObj.GetComponent<AdventureBg>();
			m_bgObjBase.SetActive(false);
		}
		// yield return new WaitForSeconds(1);

		//-*キャラクターtest
		if(m_charaObj == null){
	        GameObject objCh = (GameObject)Resources.Load("Prefabs/adventureChara");
			m_charaObj = (GameObject)Instantiate(objCh, Vector3.zero, Quaternion.identity);
			m_charaObj.transform.SetParent(m_charaObjBase.transform, false);
			m_CharaScript = m_charaObj.GetComponent<AdventureChara>();
			m_charaObjBase.SetActive(false);
		}

		LoadScenarioFile();

		//-*ページ送りボタン
		m_nextButton.GetComponent<ButtonCtl>().SetOnPointerClickCallback(PushNextPageButton);
		
		Debug.Log("//-*INIT");
		yield break;
	}

	private IEnumerator UpdateAdventure(){
		while(true){
			switch(m_Mode){
			case ADVENTUREMODE.aMODE_INIT:
				Debug.Log("//-*aMODE_INIT B");
				yield return StartCoroutine("Init");
				Debug.Log("//-*aMODE_INIT A");
				m_Mode = ModeSet(ADVENTUREMODE.aMODE_TEXT_DRAW);
				break;
			case ADVENTUREMODE.aMODE_TEXT_DRAW:
				ScenarioReadUpdate();
				if( m_isPageWait ) {
					PageWaitInit();
					m_Mode = ModeSet(ADVENTUREMODE.aMODE_PAGE_WAIT);
				}else
				if( m_isChoiceWait ){
					m_Mode = ModeSet(ADVENTUREMODE.aMODE_CHOICE_WAIT);
				}
				if( m_isEffectWait ) {
					m_Mode = ModeSet(ADVENTUREMODE.aMODE_EFFECT_WAIT);
				}
				break;
			case ADVENTUREMODE.aMODE_PAGE_WAIT:
				if( !m_isPageWait ) {
					PageWaitFin();
					m_Mode = ModeSet(ADVENTUREMODE.aMODE_TEXT_DRAW);
				}
				break;
			case ADVENTUREMODE.aMODE_CHOICE_WAIT:
				if( !m_isChoiceWait ) {
					ChoiceWaitFin();
					m_Mode = ModeSet(ADVENTUREMODE.aMODE_EFFECT_WAIT);
					m_ModeNext = ModeSet(ADVENTUREMODE.aMODE_INIT);

				}
				break;
			case ADVENTUREMODE.aMODE_EFFECT_WAIT:
				yield return StartCoroutine(m_screenEffect.FadeOutStart(false));
				yield return StartCoroutine(m_screenEffect.FadeInStart(false));
				m_Mode = ModeSet(m_ModeNext);
				m_ModeNext = ModeSet(ADVENTUREMODE.aMODE_MAX);
				break;
			case ADVENTUREMODE.aMODE_EFFECT_ERR:	//-*シナリオエラー
			default:
				Debug.Log("//-*Err:"+m_Mode);

				break;
			}
			yield return null;
		}
	}


	private IEnumerator EffectUpdate(){
		StartCoroutine(m_screenEffect.FadeInStart(false));
		yield break;
	}
	// public void ButtonSelectStage(int a)
	// {
	// 	Debug.Log("//-*ButtonTest:"+a);
	// 	SceneManager.LoadScene ("Title");
	// }


	/// <summary>
	/// シナリオファイルの読み込みと中身格納
	/// </summary>
	public bool LoadScenarioFile() {
		m_gameData.AdvScoNo = m_gameData.AdvNextScoNo;
		if(string.IsNullOrEmpty(m_gameData.AdvScoNo))
		{
			m_gameData.AdvScoNo = "0";
		}
		String scenarioNo = m_gameData.AdvScoNo;

		if( String.IsNullOrEmpty(scenarioNo) )return false;
//-*************ファイル読み込み
		//-*シナリオファイル名
		String scenarioName = String.Concat(Dir.ADV_SCENARIO_DIRECTORY, Dir.SCENARIO_BASE_NAME, scenarioNo, Dir.SCENARIO_EXTENSION);
		Debug.Log("//-*scenarioName:"+scenarioName);

		//-*ファイル読み込み
		System.IO.StreamReader file = new System.IO.StreamReader(scenarioName);  

		//-*ファイルを1行ごとに格納
		int counter = 0;  
		string line;
		if(m_textData != null){
			m_textData.Clear();
			m_textData = null;
		}
		m_textData = new List<string>();         //空のListを作成する
		while((line = file.ReadLine()) != null)  
		{  
			m_textData.Add(line);  
			counter++;  
		} 
//-*************
		m_isPageWait = false;
		m_textLineNum = 0;
		PageInit();


		//-*デバッグ用
		Debug.Log("//-*text:"+m_textData.Count);
		int a =0;
		foreach(string item in m_textData) {
			Debug.Log("//-*"+a+":"+item);
			a++;
		}


		return true;
	}
	
	/// <summary>
	/// シナリオコマンド処理
	/// true:ページ区切りまで処理した
	/// </summary>
	private void ScenarioReadUpdate()
	{
		//-*
		if(m_isPageWait)return;
		string scoLineText = m_textData[m_textLineNum];
		var cmd = ScenarioRead(scoLineText);
		Debug.Log("//-********("+cmd+")");
		switch(cmd){
		case AdvDefine.CMD_TYPE.CMD_PAGEEND:
		// ページ区切り
			// PageInit();
			m_isPageWait = true;
			break;
		case AdvDefine.CMD_TYPE.CMD_SPEAKER_DEL:
		// 発言者名:消す
			m_SpeakerNames.SetActive(false);
			break;
		case AdvDefine.CMD_TYPE.CMD_SPEAKER_MOE:
		// 発言者名:萌(萌日記)
			m_SpeakerNames.SetActive(true);
			break;
		case AdvDefine.CMD_TYPE.CMD_BG:
		// 背景
			m_bgObjBase.SetActive(true);
			if(m_bgScript != null){
				m_gameData.AdvBgNo = GetScoCmdNo(scoLineText,cmd);
				m_bgScript.ImageChange( m_gameData.AdvBgNo );
				// m_bgScript.ImageChange("adventure/moema_bg00");
			}
			break;
		case AdvDefine.CMD_TYPE.CMD_EV_BG:
		// イベント背景
			m_bgObjBase.SetActive(true);
			if(m_bgScript != null){
				m_gameData.AdvBgNo = GetScoCmdNo(scoLineText,cmd);
				m_bgScript.ImageChange( m_gameData.AdvBgNo, true );
				// m_bgScript.ImageChange("adventure/moema_bg00");
			}
			break;
		case AdvDefine.CMD_TYPE.CMD_BGM:
		// BGM再生
			string name = String.Concat(Dir.SOUND_BGM_BASE_NAME, GetScoCmdNo(scoLineText,cmd),".ogg");
			m_gameData.SoundCtl.PlayBgm( name );
			break;
		case AdvDefine.CMD_TYPE.CMD_BGM_END:
		// BGM終了:未実装
			break;
		case AdvDefine.CMD_TYPE.CMD_CH:
		// キャラ
			Debug.Log("//-*CMD_TYPE.CMD_CH");
			m_charaObjBase.SetActive(true);
			if(m_CharaScript != null){
				var imageName = GetScoCmdNo(scoLineText, AdvDefine.CMD_TYPE.CMD_CH);;
				m_CharaScript.ImageCharaSet(imageName);
				m_CharaScript.ImageFaceSet(imageName);
			}
			break;
		case AdvDefine.CMD_TYPE.CMD_CH_FACE:
		// 表情
			if(m_CharaScript != null){
				int imageName = -1;
				if(int.TryParse( GetScoCmdNo(scoLineText, AdvDefine.CMD_TYPE.CMD_CH_FACE), out imageName) ){
					if(imageName < 0)return;
					m_CharaScript.ImageFaceChangeExpression(imageName);
				}
			}
			break;
		case AdvDefine.CMD_TYPE.CMD_GO_BATTLE:
			SetMahjongInGameData(scoLineText,AdvDefine.CMD_TYPE.CMD_GO_BATTLE);
		// InGame(麻雀)へ
			break;
		case AdvDefine.CMD_TYPE.CMD_CHOICE_START:
		// 選択肢開始
			ChoiceWaitInit();
			SetChoiceSentence();
			break;
		case AdvDefine.CMD_TYPE.CMD_FADE_IN:
		case AdvDefine.CMD_TYPE.CMD_FADE_OUT:
		// フェードインフェードアウト
			ChoiceWaitInit();
			SetChoiceSentence();
			break;
		case AdvDefine.CMD_TYPE.CMD_SCO_END:
		// シナリオ終了
			break;
		case AdvDefine.CMD_TYPE.NO_CMD_SENTENCE:
		// シナリオ本文
			Debug.Log("//-********m_pageLineNum("+m_pageLineNum+")→MAX："+m_textLine.Length);
			if(m_pageLineNum < m_textLine.Length){
				m_textLine[m_pageLineNum].text = m_textData[m_textLineNum];
				Debug.Log("//-*("+m_pageLineNum+"):"+m_textLine[m_pageLineNum].text);
				m_pageLineNum++;
			}
			break;
		//-*********ココで読み込んだらエラーなコマンド
		case AdvDefine.CMD_TYPE.CMD_CHOICE:
		case AdvDefine.CMD_TYPE.CMD_CHOICE_END:
		// 選択肢
		// 選択肢終了
			m_Mode = ModeSet(ADVENTUREMODE.aMODE_EFFECT_ERR);
			break;
		default:
			m_Mode = ModeSet(ADVENTUREMODE.aMODE_EFFECT_WAIT);
			break;
		}
		m_textLineNum++;
	} 
#region PAGE
	/// <summary>
	/// ページの初期化
	/// </summary>
	private void PageInit()
	{
		//-*ページ文の消去
		for(m_pageLineNum = 0;m_pageLineNum<m_textLine.Length;m_pageLineNum++){
			m_textLine[m_pageLineNum].text = string.Empty;
		}
		m_pageLineNum = 0;
		m_isPageWait = false;
	}
	/// <summary>
	/// ページ送り待機時の初期化
	/// </summary>
	private void PageWaitInit()
	{
		//-*点灯
		m_nextButton.SetActive(true);	//-*ページ送りボタン
		m_pageWaitIcon.SetActive(true);	//-*ページ送り待ちアイコン
	}
	/// <summary>
	/// ページ送り待機時の終了処理
	/// </summary>
	private void PageWaitFin()
	{
		//-*消灯
		m_nextButton.SetActive(false);		//-*ページ送りボタン
		m_pageWaitIcon.SetActive(false);	//-*ページ送り待ちアイコン
	}

	/// <summary>
	/// ページ送りボタン押下処理
	/// </summary>
	public void PushNextPageButton()
	{
		Debug.Log("//-*PushNextPageButton:");
		PageInit();
		// SceneManager.LoadScene ("SelectStage");
	}
#endregion //-*PAGE

#region CHOICE
	/// <summary>
	/// 選択肢待機時の初期化
	/// </summary>
	private void ChoiceWaitInit()
	{
		//-*消灯
		m_isPageWait = false;
		m_nextButton.SetActive(false);	//-*ページ送りボタン

		//-*点灯
		m_isChoiceWait = true;
		m_choiceObjBase.SetActive(true);

		m_pageWaitIcon.SetActive(true);	//-*ページ送り待ちアイコン
	}
	/// <summary>
	/// 選択肢待機時の終了処理
	/// </summary>
	private void ChoiceWaitFin()
	{
		//-*消灯
		m_nextButton.SetActive(false);		//-*ページ送りボタン
		m_pageWaitIcon.SetActive(false);	//-*ページ送り待ちアイコン
		for(int btNo=0;btNo<m_choiceButton.Count;btNo++){
		//-*選択肢ボタンの消去
			Destroy(m_choiceButton[btNo]);
		}
		m_choiceObjBase.SetActive(false);	//-*選択肢ベース
	}

	/// <summary>
	/// 選択肢ボタン押下処理
	/// </summary>
	public void PushChoiceButton(string a)
	{
		Debug.Log("//-*PushChoiceButton:"+a);
		m_gameData.AdvNextScoNo = a;
		m_isChoiceWait = false;
		// SceneManager.LoadScene ("SelectStage");
	}
#endregion //-*CHOICE

#region EFFECT
	/// <summary>
	/// フェードの初期化
	/// </summary>
	private void FadeInit()
	{
		m_isEffectWait = false;

		// //-*ページ文の消去
		// for(m_pageLineNum = 0;m_pageLineNum<m_textLine.Length;m_pageLineNum++){
		// 	m_textLine[m_pageLineNum].text = string.Empty;
		// }
		// m_pageLineNum = 0;
		// m_isPageWait = false;

	}
#endregion	//-*EFFECT
	/// <summary>
	/// シナリオテキストを1行読み込み
	/// シナリオコマンドか本文かの判別
	/// </summary>
	private AdvDefine.CMD_TYPE ScenarioRead(string lineText)
	{
		foreach(KeyValuePair<AdvDefine.CMD_TYPE, string> cmd in AdvDefine.CmdDir){
		//-*シナリオコマンドの判別
			if( CheckScoCmd(lineText, cmd.Value) ){
				return cmd.Key;	//-*一致したコマンドを返す
			}
		}
		//-*一致しなかったら本文
		return AdvDefine.CMD_TYPE.NO_CMD_SENTENCE;
	}

	/// <summary>
	/// シナリオコマンドチェック
	/// sco:シナリオ文, cmd:コマンド, isExactmatch:(true)完全一致判定させる
	/// </summary>
	// private string GetScoCmdNo( string scoLineText, AdvDefine.CMD_TYPE scoCmdType)
	public bool CheckScoCmd(string sco, string cmd, bool isExactmatch = false)
	{
		// Debug.Log("//-*Sco:"+sco+"___Cmd:"+cmd);
		if( string.IsNullOrEmpty(sco) || string.IsNullOrEmpty(cmd) )return false;
		if(isExactmatch){
			return sco.Equals(cmd);
		}else{
			return sco.Contains(cmd);
		}
	}

	/// <summary>
	/// シナリオコマンド用処理：コマンド以降の番号取得
	/// </summary>
	private string GetScoCmdNo( string scoLineText, AdvDefine.CMD_TYPE scoCmdType)
	{
		//-*番号の抜き出し
		var no = scoLineText.Replace(AdvDefine.CmdDir[scoCmdType],"");
		int temp = -1;
		//-*不正な番号
		if(!int.TryParse(no, out temp)) return "err:NotNumber:"+no;	//-*番号ではない
		if(temp < 0) return "err:IllegalNumber:"+temp;	//-*番号が負数

		//-*背景番号が10未満なら文字追加
		if(temp < 10)　no = "0"+no;

		Debug.Log("//-*"+scoCmdType+" = "+no);
		return no;
	}
	/// <summary>
	/// シナリオコマンド用処理：コマンド以降の文取得
	/// </summary>
	private string GetScoCmdChoiceSentence( string scoLineText, AdvDefine.CMD_TYPE scoCmdType)
	{
		//-*コマンド以降の文取得
		var sentence = scoLineText.Replace(AdvDefine.CmdDir[scoCmdType],"");
		//-*todo:何か処理が有れば
		return sentence;
	}

#region ExclusiveScenarioCommand
//-*「萌え日記麻雀」のシナリオコマンドに対応した処理：他ゲーでも利用できるかも？
//-*todo:シナリオコマンド処理専用ソースに分けた方が良いかも
	/// <summary>
	/// シナリオコマンド用処理：選択肢格納
	/// </summary>
	private void SetChoiceSentence()
	{
		int choiceNo = 0;

		if(m_choiceButton != null){
			m_choiceButton.Clear();
			m_choiceButton = null;
		}
		m_choiceButton = new List<GameObject>();         //空のListを作成する

		m_textLineNum++;	//-*Start行なので1行進める
		while( !CheckScoCmd( m_textData[m_textLineNum], AdvDefine.CmdDir[AdvDefine.CMD_TYPE.CMD_CHOICE_END],true ) )
		{//-*選択肢終了コマンドが出るまで
			//-*選択肢コマンド
			if( CheckScoCmd( m_textData[m_textLineNum], AdvDefine.CmdDir[AdvDefine.CMD_TYPE.CMD_CHOICE]) ) {
				var sentence = m_textData[m_textLineNum].Replace(AdvDefine.CmdDir[AdvDefine.CMD_TYPE.CMD_CHOICE],"");
				var choiceAndScoNo = sentence.Split(AdvDefine.SCO_CMD_SPLIT);
				//-*選択肢ボタン生成
		        var obj = (GameObject)Resources.Load("Prefabs/advChoice_button");
				var btnObj = (GameObject)Instantiate(obj,Vector3.zero,Quaternion.identity);
				btnObj.transform.SetParent(m_choiceObjBase.transform, false);
				m_choiceButton.Add( btnObj );
				var btnCtl = m_choiceButton[choiceNo].GetComponent<AdvChoiceButton>();
				btnCtl.SetOnPointerClickCallback(PushChoiceButton);
				btnCtl.SetLabel(choiceNo, choiceAndScoNo[0], choiceAndScoNo[1]);
				choiceNo++;
			}
			m_textLineNum++;
		}
	}

	/// <summary>
	/// 麻雀ルール格納
	/// </summary>
	private void SetMahjongInGameData( string scoLineText, AdvDefine.CMD_TYPE scoCmdType)
	{
		Debug.Log("//-*SetMahjongInGameData("+scoLineText+","+scoCmdType+")..."+m_gameData);
		if(m_gameData == null)return;
		var stageAndRule = GetScoCmdNo(scoLineText, scoCmdType).Split(AdvDefine.SCO_CMD_SPLIT);
		int stNo = -1;
		int ruleNo = -1;
		if(stageAndRule != null ){
			if(!string.IsNullOrEmpty(stageAndRule[0]) && int.TryParse(stageAndRule[0], out stNo)){
				m_gameData.BattleStage = stNo;
			}
			if(!string.IsNullOrEmpty(stageAndRule[1]) && int.TryParse(stageAndRule[0], out ruleNo)){
				m_gameData.BattleRule = ruleNo;
			}
		}
		Debug.Log("//-*(stNo:"+stNo+", ruleNo:"+ruleNo+")");
		// Const.MahjongInGameData
	}
#endregion	//-*ExclusiveScenarioCommand
}
