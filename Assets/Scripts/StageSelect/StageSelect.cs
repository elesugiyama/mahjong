using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//-********
using MahjongDeffine;
// using MJDefsHeader;
// using GameDefsHeader;
// using MJDialogHeader;
//-********
public class StageSelect : SceneBase {

	private const int STAGEMAXNUM = 8;
	/// <summary>
	///	チャレンジモードステージルール
	/// </summary>
	private MAH[] StageRule = { // 
	//	LIM00, //  0:ノーマル戦
	//	LIM01, //  1:鳴き禁止
	//	LIM02, //  2:点数ハンデ戦[-10000点/15000vs35000]
	//	LIM03, //  3:二翻縛り
	//	LIM04, //  4:ロン・リーチ禁止
	//	LIM05, //  5:ロン禁止
	//	LIM06, //  6:リーチ禁止
	//	LIM07, //  7:点数ハンデ戦[-5000点/20000vs30000]
	//	//********ウキウキ:ルール追加********
	//	LIM08, //* 8:点数ハンデ戦[-15000点/10000vs40000]
	//	//***********************************
		//-*各ステージルールの入れ替えを考慮
		MAH.LIM00,
		MAH.LIM01,
		MAH.LIM02,
		MAH.LIM03,
		MAH.LIM04,
		MAH.LIM05,
		MAH.LIM06,
		MAH.LIM07,
		MAH.LIM08,
	};
	private string[] RULE_NAME = new string[]{
	//-*MahjongDeffine.cs内「enum MAH」と連動
		"ノーマル戦",		//-*LIM00, //  0:ノーマル戦
		"鳴き禁止",			//-*LIM01, //  1:鳴き禁止
		"点数ハンデ戦",		//-*LIM02, //  2:点数ハンデ戦[-10000点/15000vs35000]
		"二翻縛り",			//-*LIM03, //  3:二翻縛り
		"ロン・リーチ禁止",	//-*LIM04, //  4:ロン・リーチ禁止
		"ロン禁止",			//-*LIM05, //  5:ロン禁止
		"リーチ禁止",		//-*LIM06, //  6:リーチ禁止
		"点数ハンデ戦",		//-*LIM07, //  7:点数ハンデ戦[-5000点/20000vs30000]
		"点数ハンデ戦",		//-*LIM08, //* 8:点数ハンデ戦[-15000点/10000vs40000]
		"",					//-*LIM_MAX,
	};

//-----------------------------------------------
// ステージセレクトメニューの定義
//-----------------------------------------------
	private enum STAGESELECTMODE{ // menu処理全体
		mMODE_INIT,			//-*初期化
		mMODE_UPDATE,		//
		tMODE_NEXT_SCENE,	//-*次シーンへ
		mMODE_MAX,		//*最大数
	};

	[SerializeField]
	private GameObject[] m_StageButton;

	[SerializeField]
	private GameObject m_ScrollViewBase;

	private STAGESELECTMODE m_Mode = STAGESELECTMODE.mMODE_MAX;	//-*モード
	private bool m_isStageSelect = false;
	private MAH m_ruleNo;
	public Canvas UICanvas;     //UIを表示するキャンバス
	
	// Use this for initialization
	protected override void Start () {
		m_Mode = STAGESELECTMODE.mMODE_INIT;

        GameObject obj = (GameObject)Resources.Load("Prefabs/stageSelect_button");

		m_StageButton = new GameObject[STAGEMAXNUM];

        // プレハブを元にオブジェクトを生成する
		for(int i = 0;i < STAGEMAXNUM;i++){
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
				BCtl[0].SetRuleLabel(RULE_NAME[i]);
				
			}
    	}
	}
	
	// Update is called once per frame
	protected override void Update () {
		switch(m_Mode){
		case STAGESELECTMODE.mMODE_INIT:
			DebLog("//-*mMODE_INIT");
			m_Mode = ModeSet(STAGESELECTMODE.mMODE_UPDATE);
			break;
		case STAGESELECTMODE.mMODE_UPDATE:
			m_Mode = UpdateSelectStage();
			break;
		case STAGESELECTMODE.tMODE_NEXT_SCENE:
			UpdateNextScene();
			break;
		default:
			DebLog("//-*Err:"+m_Mode);

			break;
		}
	}

	private STAGESELECTMODE UpdateSelectStage()
	{
		if(m_isStageSelect){
			return STAGESELECTMODE.tMODE_NEXT_SCENE;
		}
		return STAGESELECTMODE.mMODE_UPDATE;
	}
	private void UpdateNextScene()
	{
		m_keepData.mah_limit_num = (int)m_ruleNo;
		DebLog("//-*m_ruleNo:"+m_ruleNo);
		SceneChange();
	}

	// //---------------------------------------------------------
	// /// <summary>
	// /// クリック
	// /// </summary>
	// //---------------------------------------------------------
	public void ButtonSelectStageTest(int a)
	{
		DebLog("//-*ButtonTest:"+a);
		// m_keepData.AdvNextScoNo = a;
		m_ruleNo = StageRule[a];
		m_nextSceneName = SceneNameDic[SCENE_NAME.INGAME];
		m_isStageSelect = true;
		// SceneChange("InGame");
	}
}
