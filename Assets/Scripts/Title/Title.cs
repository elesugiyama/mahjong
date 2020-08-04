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

	// Use this for initialization
	protected override void Start () {}
	protected override void Awake()
	{
		base.Awake();
		m_Mode = TITLEMODE.tMODE_INIT;
		if(m_BtnSelSco != null) m_BtnSelSco.SetOnPointerClickCallback(BtnModeSelSco);
		// if(m_BtnSelChallenge != null) m_BtnSelChallenge.SetOnPointerClickCallback();

		// if(m_BtnScoSelNew != null) m_BtnScoSelNew.SetOnPointerClickCallback();
		// if(m_BtnScoSelContinue != null) m_BtnScoSelContinue.SetOnPointerClickCallback();
		if(m_BtnScoSelBack != null) m_BtnScoSelBack.SetOnPointerClickCallback(BtnScoSelBack);

#if SUGI_DEB
		if(m_BtnDebSelStage != null) m_BtnDebSelStage.SetOnPointerClickCallback(DebBtnSelSco);
		if(m_BtnDebInGame != null) m_BtnDebInGame.SetOnPointerClickCallback(DebBtnInGame);
#endif //-*SUGI_DEB
			DebLog("//-*awake");

	}
	// Update is called once per frame
	protected override void Update () {
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
		default:
			break;
		}
	}

	private TITLEMODE UpdateModeMain()
	{
		if(m_BtnSelSco.ISPUSH){
			return TITLEMODE.tMODE_STORY;
		}
		if(m_BtnSelChallenge.ISPUSH){
			m_nextSceneName = SceneNameDic[SCENE_NAME.CHALLENGE];
			return TITLEMODE.tMODE_NEXT_SCENE;
		}
		if(m_BtnSelGallery.ISPUSH){
			m_nextSceneName = SceneNameDic[SCENE_NAME.GALLERY];
			return TITLEMODE.tMODE_NEXT_SCENE;
		}
		if(m_BtnSelOption.ISPUSH){
			// m_nextSceneName = SceneNameDic[SCENE_NAME.OPTION];
			// return TITLEMODE.tMODE_NEXT_SCENE;
		}
		return TITLEMODE.tMODE_MAIN;
	}
	private TITLEMODE UpdateModeStory()
	{
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
	public void BtnModeSelSco()
	{
		m_TitleBtnModeSelect.SetActive(false);
		m_TitleBtnScoModeSelect.SetActive(true);
		m_BtnScoSelContinue.SetInteractable(false);
	}
	public void BtnScoSelBack()
	{
		m_TitleBtnModeSelect.SetActive(true);
		m_TitleBtnScoModeSelect.SetActive(false);
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
