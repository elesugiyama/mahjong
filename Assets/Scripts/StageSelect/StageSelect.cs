using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelect : SceneBase {
//-*仮データ用
	private const int STAGEMAXNUM_TEST = 100;

//-----------------------------------------------
// ステージセレクトメニューの定義
//-----------------------------------------------
	private enum STAGESELECTMODE{ // menu処理全体
		mMODE_INIT,			// 0:タイトル初期化:画像等
		mMODE_TOP00,		// 1:サウンドデバイスの初期化とcielロゴ表示
		mMODE_LOGO0,		// 2:ロゴ1
		mMODE_TOP01,		// 3:待ち
		mMODE_LOGO1,		// 4:ロゴ2
		mMODE_TOP02,		// 5:待ち
		mMODE_PRESSKEY_TITLE,	// 6:PRESS ANY KEY
		mMODE_MAIN,			// 7:タイトル及びタイトルメニュー
		mMODE_STORY,		// 8:ストーリーモードメニュー
		mMODE_SPECIAL,		// 9:スペシャルメニュー
		mMODE_PROFILE,		// 10:スペシャル1:プロフィール
		mMODE_END,			// 11:終了
		mMODE_RECONF,		// 12:終了確認
		mMODE_SUPPORT,		// 110517:[DOCOMO/アプリストア]サポートの連絡先表示のみ

		mMODE_SOUNDTEST,	// サウンドテスト用
		mMODE_FLAGSWITCH,	// デバッグ用フラグスイッチ
		mMODE_ERRPROC,		// 終了確認
		mMODE_MAX,		//*最大数
	};

	[SerializeField]
	private GameObject[] m_StageButton;

	[SerializeField]
	private GameObject m_ScrollViewBase;

	[SerializeField]
	private spriteUINumS test;

	private STAGESELECTMODE m_Mode = STAGESELECTMODE.mMODE_MAX;	//-*モード
	public Canvas UICanvas;     //UIを表示するキャンバス
	// Use this for initialization
	protected override void Start () {
		m_Mode = STAGESELECTMODE.mMODE_INIT;

        GameObject obj = (GameObject)Resources.Load("Prefabs/stageSelect_button");

		m_StageButton = new GameObject[STAGEMAXNUM_TEST];

        // プレハブを元にオブジェクトを生成する
		for(int i = 0;i < STAGEMAXNUM_TEST;i++){
			m_StageButton[i] = (GameObject)Instantiate(obj,
								Vector3.zero,
								Quaternion.identity);
			//-*スクロールビューに登録
			m_StageButton[i].transform.SetParent(m_ScrollViewBase.transform, false);
			//-*ボタン設定
			var BCtl = m_StageButton[i].GetComponents<StageButton>();
			if(BCtl != null){
				BCtl[0].SetOnPointerClickCallback(ButtonSelectStageTest);
				BCtl[0].SetLabel(i);
			}
    	}
	}
	
	// Update is called once per frame
	protected override void Update () {
		switch(m_Mode){
		case STAGESELECTMODE.mMODE_INIT:
			DevLog("//-*mMODE_INIT");
			m_Mode = ModeSet(STAGESELECTMODE.mMODE_PRESSKEY_TITLE);
			
			break;
		case STAGESELECTMODE.mMODE_PRESSKEY_TITLE:
			DevLog("//-*mMODE_PRESSKEY_TITLE");
			
			break;
		default:
			DevLog("//-*Err:"+m_Mode);

			break;
		}
	}

	public void ButtonSelectStageTest(int a)
	{
		DevLog("//-*ButtonTest:"+a);
		m_keeoData.AdvNextScoNo = a;
		SceneChange("Adventure");
	}
}
