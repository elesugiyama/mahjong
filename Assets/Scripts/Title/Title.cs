using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : SceneBase {

//-----------------------------------------------
// タイトルメニューの定義
//-----------------------------------------------
	private enum TITLEMODE{ // menu処理全体
		tMODE_INIT,			// 0:タイトル初期化:画像等
		tMODE_TOP00,		// 1:サウンドデバイスの初期化とcielロゴ表示
		tMODE_LOGO0,		// 2:ロゴ1
		tMODE_TOP01,		// 3:待ち
		tMODE_LOGO1,		// 4:ロゴ2
		tMODE_TOP02,		// 5:待ち
		tMODE_PRESSKEY_TITLE,	// 6:PRESS ANY KEY
		tMODE_MAIN,			// 7:タイトル及びタイトルメニュー
		tMODE_STORY,		// 8:ストーリーモードメニュー
		tMODE_SPECIAL,		// 9:スペシャルメニュー
		tMODE_PROFILE,		// 10:スペシャル1:プロフィール
		tMODE_END,			// 11:終了
		tMODE_RECONF,		// 12:終了確認
		tMODE_SUPPORT,		// 110517:[DOCOMO/アプリストア]サポートの連絡先表示のみ
		tMODE_SOUNDTEST,	// サウンドテスト用
		tMODE_FLAGSWITCH,	// デバッグ用フラグスイッチ
		tMODE_ERRPROC,		// 終了確認
#region UNITY_ORIGINAL
		tMODE_NEXT_SCENE,
#endregion //-*UNITY_ORIGINAL
#region KEY_TEST
		tMODE_OPTION,
#endregion //-*KEY_TEST
		tMODE_MAX,		//*最大数
	};

	[SerializeField]
	private GameObject m_TitleLogo = null;

	[SerializeField]
	private GameObject m_TitleBtnModeSelect = null;
	[SerializeField]
	private ButtonCtl m_BtnSelSco = null;
	[SerializeField]
	private ButtonCtl m_BtnSelChallenge = null;
	[SerializeField]
	private ButtonCtl m_BtnSelGallery = null;
	[SerializeField]
	private ButtonCtl m_BtnSelOption = null;

	[SerializeField]
	private GameObject m_TitleBtnScoModeSelect = null;
	[SerializeField]
	private ButtonCtl m_BtnScoSelNew = null;
	[SerializeField]
	private ButtonCtl m_BtnScoSelContinue = null;
	[SerializeField]
	private ButtonCtl m_BtnScoSelBack = null;

	private TITLEMODE m_Mode = TITLEMODE.tMODE_MAX;	//-*モード

#if SUGI_DEB
	[Header("デバッグ用")]
	[SerializeField]
	private ButtonCtl m_BtnDebSelStage = null;
	[SerializeField]
	private ButtonCtl m_BtnDebInGame = null;	
#endif //-*SUGI_DEB

#region GAME_PAD
	private int m_menuNo = 0;
	private enum TITLE_SELECT { //-*タイトルメニュー
		SEL_STORY = 0,
		SEL_CHALLENGE,
		SEL_GALLERY,
		SEL_OPTION,
		MAX,
	};
	private enum SCO_SELECT { // シナリオメニュー
		SEL_NEW_GAME = 0,
		SEL_CONTINUE,
		SEL_BACK,
		MAX,
	};
	[SerializeField]
	private GameObject m_titleCursol=null;

	[SerializeField]
	private List<GameObject> m_titleMenuObj = new List<GameObject>();		//-*TITLE_SELECTと連動させること
	[SerializeField]
	private List<GameObject> m_titleScoMenuObj = new List<GameObject>();	//-*SCO_SELECTと連動させること
#endregion //-*GAME_PAD


	// Use this for initialization
	protected override void Start () {}
	protected override void Awake()
	{
		base.Awake();
		m_Mode = TITLEMODE.tMODE_INIT;
		if(m_BtnSelSco != null) m_BtnSelSco.SetOnPointerClickCallback(InitModeSelSco);
		// if(m_BtnSelChallenge != null) m_BtnSelChallenge.SetOnPointerClickCallback();

		// if(m_BtnScoSelNew != null) m_BtnScoSelNew.SetOnPointerClickCallback();
		// if(m_BtnScoSelContinue != null) m_BtnScoSelContinue.SetOnPointerClickCallback();
		if(m_BtnScoSelBack != null) m_BtnScoSelBack.SetOnPointerClickCallback(InitScoSelTitle);

#if SUGI_DEB
		if(m_BtnDebSelStage != null) m_BtnDebSelStage.SetOnPointerClickCallback(DebBtnSelSco);
		if(m_BtnDebInGame != null) m_BtnDebInGame.SetOnPointerClickCallback(DebBtnInGame);
#endif //-*SUGI_DEB
			DebLog("//-*awake");

	}
	// Update is called once per frame
	protected override void Update () {
		base.Update();
		switch(m_Mode){
		case TITLEMODE.tMODE_INIT:
			DebLog("//-*tMODE_INIT");
			m_Mode = ModeSet(TITLEMODE.tMODE_PRESSKEY_TITLE);
			
			break;
		case TITLEMODE.tMODE_PRESSKEY_TITLE:
			DebLog("//-*tMODE_PRESSKEY_TITLE");
			m_Mode = ModeSet(TITLEMODE.tMODE_MAIN);
			break;
		case TITLEMODE.tMODE_MAIN:
			m_Mode = UpdateModeMain();
			break;
		case TITLEMODE.tMODE_STORY:
			m_Mode = UpdateModeStory();
			break;
		case TITLEMODE.tMODE_NEXT_SCENE:
			UpdateNextScene();
			break;
#region KEY_TEST
		case TITLEMODE.tMODE_OPTION:
			if(!m_keepData.IsOptionOpen){
				m_Mode = TITLEMODE.tMODE_MAIN;
			}
			break;
#endregion //-*KEY_TEST
		default:
			break;
		}
	}

	private TITLEMODE UpdateModeMain()
	{
#region GAME_PAD
		//-************
		//-*キー入力テスト
		//-************
		if(IsKeyAxisButton(KEY_NAME.UP)){
			m_menuNo--;
			if(m_menuNo < (int)TITLE_SELECT.SEL_STORY){
				m_menuNo = (int)TITLE_SELECT.SEL_OPTION;
			}else if(m_menuNo >= (int)TITLE_SELECT.MAX){
				m_menuNo = (int)TITLE_SELECT.SEL_STORY;
			}
			//-*カーソル移動
			m_titleCursol.transform.SetParent(m_titleMenuObj[m_menuNo].transform);
			m_titleCursol.transform.localPosition = new Vector3(-60.0f,0.0f,0.0f);
		}else
		if(IsKeyAxisButton(KEY_NAME.DOWN)){
			m_menuNo++;
			if(m_menuNo < (int)TITLE_SELECT.SEL_STORY){
				m_menuNo = (int)TITLE_SELECT.SEL_OPTION;
			}else if(m_menuNo >= (int)TITLE_SELECT.MAX){
				m_menuNo = (int)TITLE_SELECT.SEL_STORY;
			}
			//-*カーソル移動
			m_titleCursol.transform.SetParent(m_titleMenuObj[m_menuNo].transform);
			m_titleCursol.transform.localPosition = new Vector3(-60.0f,0.0f,0.0f);
		}
		else if(IsKeyBtnPress(KEY_NAME.SELECT,false)){
			switch(m_menuNo)
			{
			case (int)TITLE_SELECT.SEL_STORY:
				m_keyWaitTImeMax = KEY_WAIT;
				InitModeSelSco();
				return TITLEMODE.tMODE_STORY;
			case (int)TITLE_SELECT.SEL_CHALLENGE:
				m_nextSceneName = SceneNameDic[SCENE_NAME.CHALLENGE];
				return TITLEMODE.tMODE_NEXT_SCENE;
			case (int)TITLE_SELECT.SEL_GALLERY:
				m_nextSceneName = SceneNameDic[SCENE_NAME.GALLERY];
				return TITLEMODE.tMODE_NEXT_SCENE;
			case (int)TITLE_SELECT.SEL_OPTION:
				OpenOption();
				return TITLEMODE.tMODE_OPTION;
			}
		}

#endregion //-*GAME_PAD
#region BUTTON_PUSH
		if(m_BtnSelSco.ISPUSH){
#region KEY_TEST
			m_menuNo = (int)TITLE_SELECT.SEL_STORY;
			m_titleCursol.transform.SetParent(m_titleMenuObj[m_menuNo].transform);
			m_titleCursol.transform.localPosition = new Vector3(-60.0f,0.0f,0.0f);
#endregion //-*KEY_TEST
			return TITLEMODE.tMODE_STORY;
		}
		if(m_BtnSelChallenge.ISPUSH){
#region KEY_TEST
			m_menuNo = (int)TITLE_SELECT.SEL_CHALLENGE;
			m_titleCursol.transform.SetParent(m_titleMenuObj[m_menuNo].transform);
			m_titleCursol.transform.localPosition = new Vector3(-60.0f,0.0f,0.0f);
#endregion //-*KEY_TEST
			m_nextSceneName = SceneNameDic[SCENE_NAME.CHALLENGE];
			return TITLEMODE.tMODE_NEXT_SCENE;
		}
		if(m_BtnSelGallery.ISPUSH){
#region KEY_TEST
			m_menuNo = (int)TITLE_SELECT.SEL_GALLERY;
			m_titleCursol.transform.SetParent(m_titleMenuObj[m_menuNo].transform);
			m_titleCursol.transform.localPosition = new Vector3(-60.0f,0.0f,0.0f);
#endregion //-*KEY_TEST
			m_nextSceneName = SceneNameDic[SCENE_NAME.GALLERY];
			return TITLEMODE.tMODE_NEXT_SCENE;
		}
		if(m_BtnSelOption.ISPUSH){
#region KEY_TEST
			m_menuNo = (int)TITLE_SELECT.SEL_OPTION;
			m_titleCursol.transform.SetParent(m_titleMenuObj[m_menuNo].transform);
			m_titleCursol.transform.localPosition = new Vector3(-60.0f,0.0f,0.0f);
#endregion //-*KEY_TEST
			// m_nextSceneName = SceneNameDic[SCENE_NAME.OPTION];
			// return TITLEMODE.tMODE_NEXT_SCENE;
			OpenOption();
		}
#endregion //-*BUTTON_PUSH

		return TITLEMODE.tMODE_MAIN;
	}
	private TITLEMODE UpdateModeStory()
	{

#region GAME_PAD
		//-************
		//-*ゲームパッド入力
		//-************
		if(IsKeyAxisButton(KEY_NAME.UP)){
			m_menuNo--;
			//-*todo:セーブの有無チェック
			if(m_menuNo == (int)SCO_SELECT.SEL_CONTINUE){
			//-*なければ飛ばす
				m_menuNo--;
			}
			if(m_menuNo < (int)SCO_SELECT.SEL_NEW_GAME){
				m_menuNo = (int)SCO_SELECT.SEL_BACK;
			}else if(m_menuNo >= (int)SCO_SELECT.MAX){
				m_menuNo = (int)SCO_SELECT.SEL_NEW_GAME;
			}
			m_titleCursol.transform.SetParent(m_titleScoMenuObj[m_menuNo].transform);
			m_titleCursol.transform.localPosition = new Vector3(-60.0f,0.0f,0.0f);
		}else
		if(IsKeyAxisButton(KEY_NAME.DOWN)){
			m_menuNo++;
			//-*todo:セーブの有無チェック
			if(m_menuNo == (int)SCO_SELECT.SEL_CONTINUE){
			//-*なければ飛ばす
				m_menuNo++;
			}
			if(m_menuNo < (int)SCO_SELECT.SEL_NEW_GAME){
				m_menuNo = (int)SCO_SELECT.SEL_BACK;
			}else if(m_menuNo >= (int)SCO_SELECT.MAX){
				m_menuNo = (int)SCO_SELECT.SEL_NEW_GAME;
			}
			m_titleCursol.transform.SetParent(m_titleScoMenuObj[m_menuNo].transform);
			m_titleCursol.transform.localPosition = new Vector3(-60.0f,0.0f,0.0f);
		}
		else
		if(IsKeyBtnPress(KEY_NAME.BACK,false)){
			DebLog("戻る！");
			InitScoSelTitle();
			return TITLEMODE.tMODE_MAIN;
		}
		else
		if(IsKeyBtnPress(KEY_NAME.SELECT,false)){
			switch(m_menuNo)
			{
			case (int)SCO_SELECT.SEL_NEW_GAME:
				m_nextSceneName = SceneNameDic[SCENE_NAME.ADVENTURE];
				return TITLEMODE.tMODE_NEXT_SCENE;
			case (int)SCO_SELECT.SEL_CONTINUE:
				return TITLEMODE.tMODE_MAIN;
			case (int)SCO_SELECT.SEL_BACK:
				InitScoSelTitle();
				return TITLEMODE.tMODE_MAIN;
			}
		}

#endregion //-*GAME_PAD
#region BUTTON_PUSH
		if(m_BtnScoSelNew.ISPUSH){
			m_nextSceneName = SceneNameDic[SCENE_NAME.ADVENTURE];
			return TITLEMODE.tMODE_NEXT_SCENE;
		}
		if(m_BtnScoSelContinue.ISPUSH){
			return TITLEMODE.tMODE_MAIN;
		}
		if(m_BtnScoSelBack.ISPUSH){
			return TITLEMODE.tMODE_MAIN;
		}
#endregion //-*BUTTON_PUSH
		return TITLEMODE.tMODE_STORY;
	}

	private void UpdateNextScene()
	{
		SceneChange();
	}

	// //---------------------------------------------------------
	// /// <summary>
	// /// クリック
	// /// </summary>
	// //---------------------------------------------------------
	public void InitModeSelSco()
	{
		m_TitleBtnModeSelect.SetActive(false);
		m_TitleBtnScoModeSelect.SetActive(true);
		m_BtnScoSelContinue.SetInteractable(false);
#region KEY_TEST
		m_menuNo = (int)TITLE_SELECT.SEL_STORY;
		m_titleCursol.transform.SetParent(m_titleScoMenuObj[m_menuNo].transform);
		m_titleCursol.transform.localPosition = new Vector3(-60.0f,0.0f,0.0f);
#endregion //-*KEY_TEST

	}
	public void InitScoSelTitle()
	{
		m_TitleBtnModeSelect.SetActive(true);
		m_TitleBtnScoModeSelect.SetActive(false);
#region KEY_TEST
		m_menuNo = (int)SCO_SELECT.SEL_NEW_GAME;
		m_titleCursol.transform.SetParent(m_titleMenuObj[m_menuNo].transform);
		m_titleCursol.transform.localPosition = new Vector3(-60.0f,0.0f,0.0f);
#endregion //-*KEY_TEST
	}

#if SUGI_DEB
	public void DebBtnSelSco()
	{
		DebLog("//-*ButtonTest2:");
		SceneChange("SelectStage");
	}

	public void DebBtnInGame()
	{
		DebLog("//-*ButtonTest2:");
		SceneChange("InGame");
	}
#endif //-*SUGI_DEB
}
