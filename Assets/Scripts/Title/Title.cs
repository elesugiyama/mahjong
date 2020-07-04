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
		tMODE_MAX,		//*最大数
	};

	[SerializeField]
	private Image m_TitleLogo = null;

	[SerializeField]
	private ButtonCtl m_TitleButton = null;

	private TITLEMODE m_Mode = TITLEMODE.tMODE_MAX;	//-*モード

	// Use this for initialization
	protected override void Start () {}
	protected override void Awake()
	{
		base.Awake();
		m_Mode = TITLEMODE.tMODE_INIT;
		m_TitleButton.SetOnPointerClickCallback(ButtonTest);
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
			
			break;
		default:
			break;
		}
	}

	public void ButtonTest()
	{
		DebLog("//-*ButtonTest2:");
		SceneChange("SelectStage");
	}
}
