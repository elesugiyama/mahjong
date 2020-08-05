using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using OptionDeffine;

public class Option : SceneBase {

//-----------------------------------------------
// タイトルメニューの定義
//-----------------------------------------------
	private enum OPTIONMODE{ // menu処理全体
		tMODE_INIT,			// 0:タイトル初期化:画像等
		tMODE_MAIN_INIT,			// 7:タイトル及びタイトルメニュー(設定画面)
		tMODE_MAIN,			// 7:タイトル及びタイトルメニュー(設定画面)
		tMODE_HELP_INIT,	// 7:タイトル及びタイトルメニュー(ヘルプ)
		tMODE_HELP,			// 7:タイトル及びタイトルメニュー(ヘルプ)
#region UNITY_ORIGINAL
		tMODE_NEXT_SCENE,
#endregion //-*UNITY_ORIGINAL

		tMODE_MAX,		//*最大数
	};
	[SerializeField]
	private ButtonCtl m_BtnOptBack;

	[Header ("各種設定")]
	[SerializeField]
	private GameObject m_optionBox;
	[SerializeField]
	private GameObject m_settingBox;
	[SerializeField]
	private GameObject m_helpBox;

	[SerializeField]
	private List<OptionButton> m_volBtnList = new List<OptionButton>();
	[SerializeField]
	private List<OptionButton> m_scoSpdBtnList = new List<OptionButton>();
	[SerializeField]
	private List<OptionButton> m_scoAutoBtnList = new List<OptionButton>();

	[SerializeField]
	private ButtonCtl m_btnOptHelp;

	[Header ("ヘルプ")]
	[SerializeField]
	private Text m_helpStr;
	[SerializeField]
	private Text m_helpPageStr;
	[SerializeField]
	private ButtonCtl m_btnHelpNextPage;
	[SerializeField]
	private ButtonCtl m_btnHelpPrevPage;

	//-*変数
	private bool m_isOptionOpne = false;
	private OPTIONMODE m_Mode = OPTIONMODE.tMODE_MAX;	//-*モード
	private int m_helpPageNo = 0;


	protected override void Start () {}
	protected override void Awake()
	{
		base.Awake();
		m_Mode = OPTIONMODE.tMODE_INIT;
		int btNo = 0;
		for(btNo = 0;btNo<m_volBtnList.Count;btNo++){
			if(m_volBtnList[btNo] == null) continue;
			m_volBtnList[btNo].SetOnPointerClickCallback(ButtonVol);
			m_volBtnList[btNo].SetLabel(btNo);
			m_volBtnList[btNo].SetActive( (btNo == m_keepData.SOUND_VOLUME) );
		}
		for(btNo = 0;btNo<m_scoSpdBtnList.Count;btNo++){
			if(m_scoSpdBtnList[btNo] == null) continue;
			m_scoSpdBtnList[btNo].SetOnPointerClickCallback(ButtonScoSpd);
			m_scoSpdBtnList[btNo].SetLabel(btNo);
			m_scoSpdBtnList[btNo].SetActive( (btNo == m_keepData.SCO_SPEED) );
		}
		for(btNo = 0;btNo<m_scoAutoBtnList.Count;btNo++){
			if(m_scoAutoBtnList[btNo] == null) continue;
			m_scoAutoBtnList[btNo].SetOnPointerClickCallback(ButtonScoAuto);
			m_scoAutoBtnList[btNo].SetLabel(btNo);
			m_scoAutoBtnList[btNo].SetActive( (btNo == m_keepData.SCO_AUTO) );
		}
		
		

#if SUGI_DEB
		if(m_BtnDebSelStage != null) m_BtnDebSelStage.SetOnPointerClickCallback(DebBtnSelSco);
		if(m_BtnDebInGame != null) m_BtnDebInGame.SetOnPointerClickCallback(DebBtnInGame);
#endif //-*SUGI_DEB
			DebLog("//-*awake");

	}
	// Update is called once per frame
	protected override void Update () {
		switch(m_Mode){
		case OPTIONMODE.tMODE_INIT:
			DebLog("//-*tMODE_INIT");
			m_Mode = ModeSet(OPTIONMODE.tMODE_MAIN);
			break;
		case OPTIONMODE.tMODE_MAIN:
			m_Mode = UpdateOption();
			break;
		case OPTIONMODE.tMODE_HELP_INIT:
			m_settingBox.SetActive(false);
			m_helpBox.SetActive(true);
			m_helpPageNo = 0;
			SetHelpStr();
			m_Mode = ModeSet(OPTIONMODE.tMODE_HELP);
			break;
		case OPTIONMODE.tMODE_HELP:
			m_Mode = UpdateOptionHelp();
			break;
		case OPTIONMODE.tMODE_NEXT_SCENE:
			UpdateNextScene();
			break;
		default:
			break;

		}
	}

	private void UpdateNextScene()
	{
		m_isOptionOpne = false;
		CloseOption();
	}
	private OPTIONMODE UpdateOption(){

		if(m_btnOptHelp.ISPUSH){
			return OPTIONMODE.tMODE_HELP_INIT;
		}
		else if(m_BtnOptBack.ISPUSH){
			return OPTIONMODE.tMODE_NEXT_SCENE;
		}
		return OPTIONMODE.tMODE_MAIN;
	}
	private OPTIONMODE UpdateOptionHelp(){

		if(m_btnHelpNextPage.ISPUSH){
			m_helpPageNo++;
			m_helpPageNo = System.Math.Min(m_helpPageNo, OptDef.HELP_STR.Length-1);
			SetHelpStr();
		}
		else if(m_btnHelpPrevPage.ISPUSH){
			m_helpPageNo--;
			m_helpPageNo = System.Math.Max(m_helpPageNo, 0);
			SetHelpStr();
		}
		else if(m_BtnOptBack.ISPUSH){
			m_settingBox.SetActive(true);
			m_helpBox.SetActive(false);
			return OPTIONMODE.tMODE_MAIN;
		}
		return OPTIONMODE.tMODE_HELP;
	}

	private void SetHelpStr(){
		string str = "";
		
		for(int line = 0;line<OptDef.HELP_STR[m_helpPageNo].Length;line++)
		{
			str += (OptDef.HELP_STR[m_helpPageNo][line]+"\n");
		}
		m_helpStr.text = str;
		m_helpPageStr.text = (m_helpPageNo+1)+"/"+OptDef.HELP_STR.Length;
	}
	// //---------------------------------------------------------
	// /// <summary>
	// /// クリック
	// /// </summary>
	// //---------------------------------------------------------
	public void ButtonVol(int no)
	{
		DebLog("//-*ButtonVol:"+no);
		if(m_keepData == null) return;
		m_keepData.SOUND_VOLUME = no;
		for(int btNo = 0;btNo<m_volBtnList.Count;btNo++){
			if(m_volBtnList[btNo] == null) continue;
			m_volBtnList[btNo].SetActive( (btNo == m_keepData.SOUND_VOLUME) );
		}
	}
	public void ButtonScoSpd(int no)
	{
		DebLog("//-*ButtonScoSpd:"+no);
		if(m_keepData == null) return;
		m_keepData.SCO_SPEED = no;
		for(int btNo = 0;btNo<m_scoSpdBtnList.Count;btNo++){
			if(m_scoSpdBtnList[btNo] == null) continue;
			m_scoSpdBtnList[btNo].SetActive( (btNo == m_keepData.SCO_SPEED) );
		}
	}
	public void ButtonScoAuto(int no)
	{
		DebLog("//-*ButtonScoAuto:"+no);
		if(m_keepData == null) return;
		m_keepData.SCO_AUTO = no;
		for(int btNo = 0;btNo<m_scoAutoBtnList.Count;btNo++){
			if(m_scoAutoBtnList[btNo] == null) continue;
			m_scoAutoBtnList[btNo].SetActive( (btNo == m_keepData.SCO_SPEED) );
		}
	}

#if SUGI_DEB

	[Header("デバッグ用")]
	[SerializeField]
	private ButtonCtl m_BtnDebSelStage = null;
	[SerializeField]
	private ButtonCtl m_BtnDebInGame = null;	
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
