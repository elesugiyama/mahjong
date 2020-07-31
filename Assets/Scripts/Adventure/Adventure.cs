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
		aMODE_TEXT_READ,		// 4
		aMODE_TEXT_DRAW,		// 4
		aMODE_PAGE_WAIT,		// 5
		aMODE_SAVE, // 22
		aMODE_ASC_CONTINUE, // 23
		aMODE_DIALOG_TRIAL, // 24
		//-*************
		aMODE_CHOICE_WAIT,		//-*選択肢待ち
		aMODE_EFFECT_SET,		//-*エフェクト開始
		aMODE_EFFECT_WAIT,		//-*エフェクト終了待ち
		aMODE_GO_TO_BATTLE,		//-*麻雀へ
		aMODE_ENDING,			//-*ゲームクリア
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
	[Header("選択肢")]
	[SerializeField]
	private GameObject m_choiceObjBase;
	//-*選択肢ボタン
	private List<GameObject> m_choiceButton;

	[Header("ページ送り")]
	[SerializeField]
	private GameObject m_nextButton;	//-*ページ送りボタン
	[SerializeField]
	private GameObject m_pageWaitIcon;	//-*ページ送り待ちアイコン

	[Header("背景")]
	[SerializeField]
	private GameObject m_bgObjBase;
	private GameObject m_bgObj;
	private AdventureBg m_bgScript;
	
	[Header("キャラクター")]
	[SerializeField]
	private GameObject m_charaObjBase;
	private GameObject m_charaObj;
	private AdventureChara m_CharaScript;

	// [SerializeField]
	// private Image m_effectFade;


	private ADVENTUREMODE m_Mode = ADVENTUREMODE.aMODE_MAX;	//-*モード
	private ADVENTUREMODE m_ModeNext = ADVENTUREMODE.aMODE_MAX;	//-*次のモード
	private ScreenEffect.EFFECTTYPE m_EffectType = ScreenEffect.EFFECTTYPE.NONE;	//-*演出
	public Canvas UICanvas;     //UIを表示するキャンバス


	//-********************
	//-*シナリオテキスト関連
	//-*todo:独立させるかも
	//-********************
	//-*読み込んだテキスト出力用
	[Header("シナリオテキスト関連")]
	[SerializeField]
	private Text[] m_textLine;	//-*文の表示用
	private List<string> m_textLineData = new List<string>();	//-*1ページの本文
	private List<string> m_textData = new List<string>();		//-*シナリオ一式(コマンド含む)
	
	//-*発言者名
	[SerializeField]
	private GameObject m_SpeakerNames;	//-*名前が画像

	//-*現在の読み込み行番号
	private int m_scoLineNum;		//-*シナリオ内の〇行目
	private int m_pageLineCount;	//-*1ページの行総数
	private int m_pageLine;			//-*1ページの〇行目
	private int m_messageCount; 	//-*現在表示中の文字数
	private float m_messageDelayCount; //-*1文字ずつ表示用ディレイカウント
	private float m_pageDrawSpeed = 0.2f;	//一文字一文字の表示する速さ todo:速度を3つ定義(0.0f,0.1f,0.2f)して可変にする


#region FLAG
	//-*ページ区切りまで読んだか
	private bool m_isPageWait = false;
	private bool m_isPageDraw = false;
	//-*選択肢選択待ち
	private bool m_isChoiceWait = false;
	//-*演出待ち
	private bool m_isEffectWait = false;
	//-*次のシーン待ち
	private bool m_isNextSceneWait = false;
	//-*麻雀へ
	private bool m_isBattleSceneWait = false;
	//-*クリアフラグ
	private bool m_isEnding = false;
	//-*エラー
	private bool m_isException = false;

#endregion //-*FLAG


	//-*フェードイン
 	private bool m_effectFadeIn = false;


	// Use this for initialization
	protected override void Start () {
		DebLog("//-*ADV_start");
#if SUGI_DEB
//-*デバッグ
// m_gameData.AdvNextScoNo = "13";
m_isSceneChange = true;
#endif
		m_Mode = ModeSet(ADVENTUREMODE.aMODE_INIT);
		StartCoroutine("UpdateAdventure");
	}
	
	// Update is called once per frame
	protected override void Update () {}

	private IEnumerator Init(){
		InitWaitFlags();

		//-*背景
		if(m_bgObj == null){
			GameObject objBG = (GameObject)Resources.Load("Prefabs/adventureBG");
			m_bgObj = (GameObject)Instantiate(objBG, Vector3.zero, Quaternion.identity);
			m_bgObj.transform.SetParent(m_bgObjBase.transform, false);
			m_bgScript = m_bgObj.GetComponent<AdventureBg>();
			m_bgObjBase.SetActive(false);
		}
		// yield return new WaitForSeconds(1);

		//-*キャラクター
		if(m_charaObj == null){
	        GameObject objCh = (GameObject)Resources.Load("Prefabs/adventureChara");
			m_charaObj = (GameObject)Instantiate(objCh, Vector3.zero, Quaternion.identity);
			m_charaObj.transform.SetParent(m_charaObjBase.transform, false);
			m_CharaScript = m_charaObj.GetComponent<AdventureChara>();
			m_charaObjBase.SetActive(false);
		}

		yield return LoadScenarioFile();

		//-*ページ送りボタン
		m_nextButton.GetComponent<ButtonCtl>().SetOnPointerClickCallback(PushNextPageButton);
		
		//-*初回フェードイン
		if(m_isSceneChange) {
//-***
			// m_screenEffect.SetEffect(ScreenEffect.EFFECTTYPE.FADE_OUT_BLACK);
//-***
			m_screenEffect.SetEffect(ScreenEffect.EFFECTTYPE.FADE_IN_BLACK);
			ScenarioReadUpdate();
			m_isEffectWait = true;
			m_isSceneChange = false;
		}

		m_Mode = ModeSet(ADVENTUREMODE.aMODE_TEXT_READ);

#if SUGI_DEB
//-*デバッグ
		m_dev_ScoFinButton.GetComponent<ButtonCtl>().SetOnPointerClickCallback(DevPushButton);
#endif

		DebLog("//-*INIT");
		yield break;
	}
	/// <summary>
	/// 各waitフラグの初期化
	/// </summary>
	private void InitWaitFlags(){
		m_isNextSceneWait = false;
		m_isPageWait = false;
		m_isPageDraw = false;
		m_isChoiceWait = false;
		m_isEffectWait = false;
		m_isNextSceneWait = false;
		m_isException = false;
		m_isEnding = false;
	}

	private IEnumerator UpdateAdventure(){
		while(true){
			if(m_isException){
				m_Mode = ModeSet(ADVENTUREMODE.aMODE_EFFECT_ERR);
			}
			switch(m_Mode){
			case ADVENTUREMODE.aMODE_INIT:
				DebLog("//-*aMODE_INIT B");
				yield return StartCoroutine("Init");
				DebLog("//-*aMODE_INIT A");
				break;
			case ADVENTUREMODE.aMODE_TEXT_READ:
				while(true){
					ScenarioReadUpdate();	//-*シナリオ行読み込み
					//-*読み込み中断フラグ
					if( m_isPageWait ) {
						PageWaitInit();
						m_Mode = ModeSet(ADVENTUREMODE.aMODE_TEXT_DRAW);
						break;
					}else
					if( m_isChoiceWait ){
						m_Mode = ModeSet(ADVENTUREMODE.aMODE_CHOICE_WAIT);
						break;
					}else
					if( m_isEffectWait ) {
						m_Mode = ModeSet(ADVENTUREMODE.aMODE_EFFECT_WAIT);
						m_ModeNext = ModeSet(ADVENTUREMODE.aMODE_TEXT_READ);
						break;
					}else
					if( m_isNextSceneWait ) {
						// m_Mode = ModeSet(ADVENTUREMODE.aMODE_EFFECT_WAIT);
						m_Mode = ModeSet(ADVENTUREMODE.aMODE_INIT);
						m_ModeNext = ModeSet(ADVENTUREMODE.aMODE_INIT);
						break;
					}else
					if( m_isBattleSceneWait ) {
						m_Mode = ModeSet(ADVENTUREMODE.aMODE_EFFECT_WAIT);
						m_ModeNext = ModeSet(ADVENTUREMODE.aMODE_GO_TO_BATTLE);
						break;
					}else
					if( m_isEnding ) {
						m_Mode = ModeSet(ADVENTUREMODE.aMODE_ENDING);
						break;
					}
				}
				break;
			case ADVENTUREMODE.aMODE_TEXT_DRAW:
				yield return StartCoroutine(DrawPage());
				PageWait();
				m_Mode = ModeSet(ADVENTUREMODE.aMODE_PAGE_WAIT);
				break;
			case ADVENTUREMODE.aMODE_PAGE_WAIT:
				if( !m_isPageWait ) {
					PageInit();
					PageWaitFin();
					m_Mode = ModeSet(ADVENTUREMODE.aMODE_TEXT_READ);
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
				//-*演出終了待ち
				DebLog("//-*aMODE_EFFECT_WAIT:"+m_Mode+"->"+m_ModeNext);
				yield return StartCoroutine(m_screenEffect.EffectUpdate());
				m_Mode = ModeSet(m_ModeNext);
				m_ModeNext = ModeSet(ADVENTUREMODE.aMODE_MAX);
				m_isEffectWait = false;
				break;
			case ADVENTUREMODE.aMODE_GO_TO_BATTLE:
				SceneChange("InGame");
				break;
			case ADVENTUREMODE.aMODE_ENDING:
				SceneChange("Title");
				break;
			case ADVENTUREMODE.aMODE_EFFECT_ERR:	//-*シナリオエラー
			default:
				//-*todo:エラーダイアログ
				DebLogError("//-*Err:"+m_Mode);

				break;
			}
			yield return null;
		}
	}

	/// <summary>
	/// シナリオファイルの読み込みと中身格納
	/// </summary>
	private IEnumerator LoadScenarioFile() {
		m_keepData.AdvScoNo = m_keepData.AdvNextScoNo;
		if(m_keepData.AdvScoNo <= 0)
		// if(string.IsNullOrEmpty(m_gameData.AdvScoNo))
		{
			m_keepData.AdvScoNo = 0;
		}
		// int scenarioNo = m_gameData.AdvScoNo;
		var scenarioNo = (m_keepData.AdvScoNo).ToString();
		if( String.IsNullOrEmpty(scenarioNo) )yield break;
//-*************ファイル読み込み
		//-*シナリオファイル名
		String scenarioName = String.Concat(Dir.ADV_SCENARIO_DIRECTORY, Dir.SCENARIO_BASE_NAME, scenarioNo/*, Dir.SCENARIO_EXTENSION*/);
		DebLog("//-*scenarioName:"+scenarioName);
#if SUGI_DEB
		DevSetScoFileName(scenarioName);
#endif //-*SUGI_DEB
		//-*ファイル読み込み(todo:アセット化するなら別方法)
		System.IO.TextReader file;
		String jsonString = Resources.Load<TextAsset>(scenarioName).ToString(); 
		file = new System.IO.StringReader(jsonString);
	
	 //-******

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
		m_isPageDraw = false;
		m_scoLineNum = 0;
		PageInit();


		//-*デバッグ用
		DebLog("//-*text:"+m_textData.Count);
		int a =0;
		string devSco = "//-*****シナリオ("+scenarioName+")*****\n";
		foreach(string item in m_textData) {
			// DevLog("//-*"+a+":"+item);
			devSco += (a+":"+item+"\n");
			a++;
		}
		DebLog(devSco);


		yield break;
	}
	
	/// <summary>
	/// シナリオコマンド処理
	/// true:ページ区切りまで処理した
	/// </summary>
	private void ScenarioReadUpdate()
	{
		//-*
		if(m_isPageWait || m_isPageDraw)return;
		string scoLineText = m_textData[m_scoLineNum];
		var cmd = ScenarioRead(scoLineText);
		DebLog("//-********("+cmd+")");
		switch(cmd){
		case AdvDefine.CMD_TYPE.CMD_PAGEEND:
		// ページ区切り
			// PageInit();
			m_isPageWait = true;
			m_isPageDraw = true;
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
				var bgName = GetScoCmdNoString(scoLineText,cmd);
				m_bgScript.SetImageBg( bgName );
				// m_bgScript.ImageChange("adventure/moema_bg00");
			}
			break;
		case AdvDefine.CMD_TYPE.CMD_EV_BG:
		// イベント背景
			m_bgObjBase.SetActive(true);
			if(m_bgScript != null){
				var cgNo = GetScoCmdNoInteger(scoLineText,cmd);
				m_bgScript.SetImageEvBg( cgNo );
				// m_bgScript.ImageChange("adventure/moema_bg00");
			}
			//-*人物を非表示
			m_charaObjBase.SetActive(false);
			break;
		case AdvDefine.CMD_TYPE.CMD_BGM:
		// BGM再生
			string bgmNo = GetScoCmdNoString(scoLineText,cmd);
#if true
			bgmNo = "01";
#endif
			string bgmName = String.Concat(Dir.SOUND_BGM_BASE_NAME, bgmNo, ".ogg");
			PlaySound(bgmName, SoundManagerCtrl.SOUNDTYPE.TYPE_BGM);
			break;
		case AdvDefine.CMD_TYPE.CMD_BGM_END:
		// BGM終了
			StopSound(SoundManagerCtrl.SOUNDTYPE.TYPE_BGM);
			break;
		case AdvDefine.CMD_TYPE.CMD_CH:
		// キャラ
			DebLog("//-*CMD_TYPE.CMD_CH");
			m_charaObjBase.SetActive(true);
			if(m_CharaScript != null){
				var imageName = GetScoCmdNoString(scoLineText, AdvDefine.CMD_TYPE.CMD_CH);;
				m_CharaScript.ImageCharaSet(imageName);
				m_CharaScript.ImageFaceSet(imageName);
			}
			break;
		case AdvDefine.CMD_TYPE.CMD_CH_FACE:
		// 表情
			if(m_CharaScript != null){
				int imageName = -1;
				if(int.TryParse( GetScoCmdNoString(scoLineText, AdvDefine.CMD_TYPE.CMD_CH_FACE), out imageName) ){
					if(imageName < 0)return;
					m_CharaScript.ImageFaceChangeExpression(imageName);
				}
			}
			break;
		case AdvDefine.CMD_TYPE.CMD_CH_DEL:
			m_charaObjBase.SetActive(false);
			break;
		case AdvDefine.CMD_TYPE.CMD_GO_BATTLE:
			// InGame(麻雀)へ
			SetMahjongInGameData(scoLineText,AdvDefine.CMD_TYPE.CMD_GO_BATTLE);
			m_isBattleSceneWait = true;
			m_screenEffect.SetEffect(ScreenEffect.EFFECTTYPE.FADE_OUT_BLACK);
			//-*次番号のシナリオを格納
			m_keepData.AdvNextScoNo = (m_keepData.AdvScoNo+1);
			break;
		case AdvDefine.CMD_TYPE.CMD_CHOICE_START:
		// 選択肢開始
			ChoiceWaitInit();
			SetChoiceSentence();
			break;
		// フェードインフェードアウト
		case AdvDefine.CMD_TYPE.CMD_FADE_OUT_B:
			m_screenEffect.SetEffect(ScreenEffect.EFFECTTYPE.FADE_OUT_BLACK);
			m_isEffectWait = true;
			break;
		case AdvDefine.CMD_TYPE.CMD_FADE_IN_B:
			m_screenEffect.SetEffect(ScreenEffect.EFFECTTYPE.FADE_IN_BLACK);
			m_isEffectWait = true;
			break;
		case AdvDefine.CMD_TYPE.CMD_GAME_CLEAR:
		//-*ゲームクリア
			m_isEnding = true;
			break;
		case AdvDefine.CMD_TYPE.CMD_SCO_END:
		// シナリオ終了
			if( IsCmdFileEndHaveNextSco(scoLineText) ){
			//-*次のシナリオへ
				m_isNextSceneWait = true;
			}else if(m_isEnding){
			//-*ゲームクリア(不要？）
				m_isEnding = true;
			}else{
			//-*指定がない
				//-*他のコマンドで遷移してるはずだから来ない想定
				m_isException = true;
			}
			break;
		case AdvDefine.CMD_TYPE.NO_CMD_SENTENCE:
		// シナリオ本文
			DebLog("//-********m_pageLineNum("+m_pageLineCount+")→MAX："+m_textLine.Length);
			if(m_pageLineCount < m_textLine.Length){
				m_textLineData.Add(m_textData[m_scoLineNum]);
#if false
				m_textLine[m_pageLine].text = m_textData[m_textLineNum];		//-*直接
#endif
				DebLog("//-*("+m_pageLineCount+"):"+m_textLineData[m_pageLineCount]);
				m_pageLineCount++;
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
		m_scoLineNum++;
	} 
#region PAGE
	/// <summary>
	/// ページの初期化
	/// </summary>
	private void PageInit()
	{
		//-*ページ文の消去
		// for(m_pageLine = 0;m_pageLine < m_pageLineCount;m_pageLine++){
		for(m_pageLine = 0;m_pageLine < m_textLine.Length;m_pageLine++){
			if(m_textLine[m_pageLine] != null)
				m_textLine[m_pageLine].text = string.Empty;
		}
		if(m_textLineData != null){
			m_textLineData.Clear();
			m_textLineData = null;
		}
		m_textLineData = new List<string>();

		m_pageLine = 0;
		m_pageLineCount = 0;
		m_messageCount = 0;
		m_messageDelayCount = 0;
		m_isPageDraw = false;
		m_isPageWait = false;
	}
	/// <summary>
	/// ページ送り待機時の初期化
	/// </summary>
	private void PageWaitInit()
	{
		//-*点灯
		m_nextButton.SetActive(true);	//-*ページ送りボタン
	}
	private void PageWait()
	{
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
		DebLog("//-*PushNextPageButton:");
		// PageInit();
		switch(m_Mode){
			case ADVENTUREMODE.aMODE_TEXT_DRAW:
				m_isPageDraw = false;
				break;
			case ADVENTUREMODE.aMODE_PAGE_WAIT:
				m_isPageWait = false;
				break;
			default:
				break;
		}
		// SceneManager.LoadScene ("SelectStage");
	}


    private IEnumerator DrawPage()
    {
		m_messageCount = 0;
		m_messageDelayCount = 0;
        while (m_textLineData[m_pageLine].Length > m_messageCount)//文字をすべて表示していない場合ループ
        {
			if(!m_isPageDraw)
			{
				m_textLine[m_pageLine].text = m_textLineData[m_pageLine];
				m_messageCount = m_textLineData[m_pageLine].Length;
				break;
			}
			if(m_messageDelayCount > m_pageDrawSpeed)
	    	{
				m_textLine[m_pageLine].text += m_textLineData[m_pageLine][m_messageCount];//一文字追加
				m_messageCount++;//現在の文字数
				m_messageDelayCount = 0;
			}else{
				m_messageDelayCount+=Time.deltaTime;
			}
			
            yield return null;
            // yield return new WaitWhile(() => m_isPageDraw);
            // yield return new WaitForSeconds(novelSpeed);//任意の時間待つ
        }

        m_pageLine++; //次の会話文配列
		DebLog("//-*m_pageLine("+m_pageLine+") < m_textLineData.Count("+m_textLineData.Count+")");
        if (m_pageLine < m_textLineData.Count)//全ての会話を表示したか
        {
            yield return StartCoroutine(DrawPage());
        }
		yield break;
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
		DebLog("//-*PushChoiceButton:"+a);
		m_keepData.AdvNextScoNo = int.Parse(a);
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
		// OutputDevLog("//-*Sco:"+sco+"___Cmd:"+cmd);
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
	private int GetScoCmdNoInteger( string scoLineText, AdvDefine.CMD_TYPE scoCmdType)
	{
		var noStr = GetScoCmdNoString(scoLineText,scoCmdType);
		int no = -1;
		//-*不正な番号
		if(!int.TryParse(noStr, out no)) return -1;	//-*番号ではない

		DebLog("//-*"+scoCmdType+" = "+noStr);
		return no;
	}
	private string GetScoCmdNoString( string scoLineText, AdvDefine.CMD_TYPE scoCmdType)
	{
		//-*番号の抜き出し
		var noStr = scoLineText.Replace(AdvDefine.CmdDir[scoCmdType],"");
		int temp = -1;
		//-*不正な番号
		if(!int.TryParse(noStr, out temp)) return "err:NotNumber:"+noStr;	//-*番号ではない
		if(temp < 0) return "err:IllegalNumber:"+temp;	//-*番号が負数

		//-*背景番号が10未満なら文字追加
		if(temp < 10)　noStr = "0"+noStr;

		DebLog("//-*"+scoCmdType+" = "+noStr);
		return noStr;
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
	/// <summary>
	/// シナリオコマンド用処理：終了コマンド
	/// 次のシナリオ番号が有るか
	/// </summary>
	private bool IsCmdFileEndHaveNextSco( string scoLineText)
	{
		//-*コマンド以降の文取得
		var sentence = scoLineText.Replace(AdvDefine.CmdDir[AdvDefine.CMD_TYPE.CMD_SCO_END],"");
		var nextScoNo = sentence.Split(AdvDefine.SCO_CMD_SPLIT);
DebLog("//-*"+scoLineText+": sentence("+sentence+"): nextScoNo("+nextScoNo.Length+")");
		int scoNo = 0;
		foreach( var a in nextScoNo ){
			//-*次のシナリオ番号が有れば
			if( int.TryParse(a, out scoNo) ){
				m_keepData.AdvNextScoNo = scoNo;
				return true;
			}
		}
		return false;
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

		m_scoLineNum++;	//-*Start行なので1行進める
		while( !CheckScoCmd( m_textData[m_scoLineNum], AdvDefine.CmdDir[AdvDefine.CMD_TYPE.CMD_CHOICE_END],true ) )
		{//-*選択肢終了コマンドが出るまで
			//-*選択肢コマンド
			if( CheckScoCmd( m_textData[m_scoLineNum], AdvDefine.CmdDir[AdvDefine.CMD_TYPE.CMD_CHOICE]) ) {
				var sentence = m_textData[m_scoLineNum].Replace(AdvDefine.CmdDir[AdvDefine.CMD_TYPE.CMD_CHOICE],"");
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
			m_scoLineNum++;
		}
	}

	/// <summary>
	/// 麻雀ルール格納
	/// </summary>
	private void SetMahjongInGameData( string scoLineText, AdvDefine.CMD_TYPE scoCmdType)
	{
		DebLog("//-*SetMahjongInGameData("+scoLineText+","+scoCmdType+")..."+m_keepData);
		if(m_keepData == null)return;
		// var stageAndRule = GetScoCmdNoString(scoLineText, scoCmdType).Split(AdvDefine.SCO_CMD_SPLIT);
		var stageAndRule = GetScoCmdChoiceSentence(scoLineText, scoCmdType).Split(AdvDefine.SCO_CMD_SPLIT);
		
		int stNo = -1;
		int ruleNo = -1;
		if(stageAndRule != null ){
			if(!string.IsNullOrEmpty(stageAndRule[0]) && int.TryParse(stageAndRule[0], out stNo)){
				m_keepData.MjStage = stNo;
			}
			if(!string.IsNullOrEmpty(stageAndRule[1]) && int.TryParse(stageAndRule[1], out ruleNo)){
				m_keepData.mah_limit_num = ruleNo;
			}
		}
		DebLog("//-*(stNo:"+stNo+", ruleNo:"+ruleNo+")");
		// Const.MahjongInGameData
	}
#endregion	//-*ExclusiveScenarioCommand

#if SUGI_DEB
#region DEBUG
[Header("デバッグ用")]
	[SerializeField]
	private GameObject m_dev_ScoFinButton;	//-*シナリオ終了ボタン
	/// <summary>
	/// ページ送りボタン押下処理
	/// </summary>
	public void DevPushButton()
	{
		SceneChange("SelectStage");

		// SceneManager.LoadScene ("SelectStage");
	}

	[SerializeField]
	private Text m_dev_ScoFileName;	//-*シナリオファイル名
	public void DevSetScoFileName(string name)
	{

		m_dev_ScoFileName.text = name;
	}

#endregion	//-*SUGI_DEB
#endif
}
