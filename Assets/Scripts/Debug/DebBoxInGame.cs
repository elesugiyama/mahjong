using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Const;
//-********
using MahjongDeffine;
using MJDefsHeader;
using GameDefsHeader;
using MJDialogHeader;
//-********

/// <summary>
/// 麻雀牌
/// </summary>
public class DebBoxInGame : MonoBehaviour {
//-*SUGI_DEB***************************
#if SUGI_DEB //-*todo:注デバッグ中

	public enum FUNCTION_LIST : int
	{//-*デバッグ機能
		OPEN_PAI = 0,
		TUMIKOMI,
		INITGAME,
		GET_SCORE,
		THINK,
		MAX,
		DEFFO = 99,	//-*デバッグBox大元：ラベル未使用にも代用
	}

	/// <summary>
	/// デバッグリスト
	/// </summary>
	private static Dictionary<FUNCTION_LIST,string> FunctionDic = new Dictionary<FUNCTION_LIST,string>()
	{
		{ FUNCTION_LIST.OPEN_PAI,	"相手の牌：" },
		{ FUNCTION_LIST.TUMIKOMI,	"積み込み：" },
		{ FUNCTION_LIST.INITGAME,	"新規局へ：" },
		{ FUNCTION_LIST.GET_SCORE,	"点移動ﾅｼ：" },
		{ FUNCTION_LIST.THINK,		"思考　　：" },
		{ FUNCTION_LIST.MAX,		"" },			//-*追加するならこれより↑に
		{ FUNCTION_LIST.DEFFO,		"" },
	};


	private enum TUMIKOMI_PLUS
	{
		SAME_KYOKU,	//-*同じ局
		//-*以下固定
		DEL,
		MAX,
	}
	/// <summary>
	/// 追加積み込みボタンリスト
	/// </summary>
	private static Dictionary<TUMIKOMI_PLUS,string> TumikomiPlusDic = new Dictionary<TUMIKOMI_PLUS,string>()
	{
		{ TUMIKOMI_PLUS.SAME_KYOKU,	"同" },
		{ TUMIKOMI_PLUS.DEL,		"DEL" },
		{ TUMIKOMI_PLUS.MAX,		"" },			//-*追加するならこれより↑に
	};


	private static string[] THINK_PARA_NAME = {
		"chParaRichi;",				//*立直
		"chParaUra;",					//*裏ドラ
		"chParaKish;",					//*イーシャンテン
		"chParaIshnum;",				//*イーシャンテンの待ち数
		"chParaYaku;",					//*役
		"chParaOri;",					//*降り
		"chParaFurotnp;",				//*？
		"chParaFuroish;",				//*鳴いてイーシャンテン
		"chParaFuroval;",				//*鳴いてイーシャンテンでない
		"chParaFanpaih;",				//*役牌
		"chParaFanpai1;",
		"chParaFanpai2;",
		"chParaHonisop;",			//*ホンイツ
		"chParaHonisoh;",
		"chParaToitoip;",				//*トイトイ
		"chParaToitoih;",
		"chParaTanyop;",				//*タンヤオ
		"chParaTanyoh;",
		"chParaKan;",					//*カン(好き 0,5)
		"chParaAlone;",				//*カンチャン、ペンチャン嫌い
		"chParaTako;",					//*タコ
		"chParaSpecial;",				//*未使用
		"chParaJanto;",				//*雀頭
		"chParaKeiten;",				//*形式テンパイ
		"chParaFuriten;",				//*フリテン	//xxxx	未使用
		"chParaStrat;",				//*？(0,1)
		"chParaRon;",					//*ロン（上がりやすさ）
		"chParaAnpai;",				//*アンパイ
		"chParaRsv1;",					//*未使用
		"chParaRsv2;",					//*未使用
	};

	private static int[][] THINK_PARA_MINMAX = {
		//-*最低値、最大値
		new int[]{-7, 7},		//-*"chParaRichi;",				//*立直
		new int[]{-1, 7},		//-*"chParaUra;",				//*裏ドラ
		new int[]{99,99},		//-*"chParaKish;",				//*イーシャンテン
		new int[]{99,99},		//-*"chParaIshnum;",			//*イーシャンテンの待ち数
		new int[]{-3, 0},		//-*"chParaYaku;",				//*役
		new int[]{-4, 4},		//-*"chParaOri;",				//*降り
		new int[]{-4, 4},		//-*"chParaFurotnp;",			//*？
		new int[]{-4, 4},		//-*"chParaFuroish;",			//*鳴いてイーシャンテン
		new int[]{99,99},		//-*"chParaFuroval;",			//*鳴いてイーシャンテンでない
		new int[]{ 0,99},		//-*"chParaFanpaih;",			//*役牌
		new int[]{ 0,99},		//-*"chParaFanpai1;",		
		new int[]{ 0,99},		//-*"chParaFanpai2;",		
		new int[]{ 0, 5},		//-*"chParaHonisop;",			//*ホンイツ
		new int[]{ 0,99},		//-*"chParaHonisoh;",		
		new int[]{ 0, 5},		//-*"chParaToitoip;",			//*トイトイ
		new int[]{ 0,99},		//-*"chParaToitoih;",		
		new int[]{ 0, 5},		//-*"chParaTanyop;",			//*タンヤオ
		new int[]{ 0,99},		//-*"chParaTanyoh;",		
		new int[]{ 0, 5},		//-*"chParaKan;",				//*カン(好き 0,5)
		new int[]{99,99},		//-*"chParaAlone;",				//*カンチャン、ペンチャン嫌い
		new int[]{ 0, 1},		//-*"chParaTako;",				//*タコ
		new int[]{ 0, 0},		//-*"chParaSpecial;",			//*未使用
		new int[]{ 0, 1},		//-*"chParaJanto;",				//*雀頭
		new int[]{ 0, 1},		//-*"chParaKeiten;",			//*形式テンパイ
		new int[]{ 0, 1},		//-*"chParaFuriten;",			//*フリテン	//xxxx	未使用
		new int[]{ 0, 1},		//-*"chParaStrat;",				//*？(0,1)
		new int[]{ 0, 1},		//-*"chParaRon;",				//*ロン（上がりやすさ）
		new int[]{ 0, 1},		//-*"chParaAnpai;",				//*アンパイ
		new int[]{ 0, 0},		//-*"chParaRsv1;",				//*未使用
		new int[]{ 0, 0},		//-*"chParaRsv2;",				//*未使用
		
	};

	[Header("Box外")]
	[SerializeField]
	private GameObject m_btnOnOff;

	[Header("Box内")]
	[SerializeField]
	private GameObject m_box;
	[SerializeField]
	private List<GameObject> m_btnDebList = new List<GameObject>();

	[SerializeField]
	private GameObject m_tumikomiBase;
	[SerializeField]
	private GameObject m_tumikomiListBase;
	[SerializeField]
	private List<GameObject> m_btnTumikomiList = new List<GameObject>();
	[SerializeField]
	private GameObject m_btnTumikomiClose;
	
	//-*思考操作
	[SerializeField]
	private GameObject m_thinkBase;
	[SerializeField]
	private List<GameObject> m_thinkListBase = new List<GameObject>();
	[SerializeField]
	private List<DebThinkBox> m_btnThinkListP1 = new List<DebThinkBox>();
	[SerializeField]
	private List<DebThinkBox> m_btnThinkListP2 = new List<DebThinkBox>();
	[SerializeField]
	private GameObject m_btnThinkClose;
	[SerializeField]
	private ButtonCtl m_btnThinkNext;
	[SerializeField]
	private ButtonCtl m_btnThinkPrev;


	//-****変数
	private FUNCTION_LIST m_useDebNo;	//-*デバッグ機能番号
	private FUNCTION_LIST m_useNextDebNo;	//-*デバッグ機能番号
	private bool[] m_isEffect = new bool[(int)FUNCTION_LIST.MAX];
	private byte [][] m_tumikomiList = MJDefine.tumiData;
	private int m_tumikomiNo = -1;
	private bool m_isInitKyoku = false;
	private byte[]	m_tumikomiSame= new byte [ 136 ];	//-*全牌の並び同じ局を繰り返す用

	//-*思考
	private int[] m_paraValList = new int[THINK_PARA_NAME.Length];

	// Use this for initialization
	void Start () {//-*todo:awakeにするかも
		//-***ボタン設定***
		//-***大元Box
		var DebBCtl = m_btnOnOff.GetComponent<DebButtonBase>();
		if(DebBCtl != null){
			DebBCtl.SetOnPointerClickCallback(ButtonDebBoxOnOff);
			DebBCtl.InitLabel(0, FunctionDic[FUNCTION_LIST.DEFFO]);
			DebBCtl.ChangeOnOff(!m_box.activeSelf);
		}

		DebBCtl = null;
		for(int a = 0;a<m_btnDebList.Count;a++){
			DebBCtl = m_btnDebList[a].GetComponent<DebButtonBase>();
			if(DebBCtl != null){
				DebBCtl.SetOnPointerClickCallback(ButtonSelectStageTest);
				DebBCtl.InitLabel(a, FunctionDic[(FUNCTION_LIST)a]);
			}
		}

		//-***積み込みBox
		DebBCtl = null;
		DebBCtl = m_btnTumikomiClose.GetComponent<DebButtonBase>();
		if(DebBCtl != null){
			DebBCtl.SetOnPointerClickCallback(ButtonTumikomiBoxOff);
			DebBCtl.InitLabel(0,"閉じる",false);
			// DebBCtl.ChangeOnOff(!m_box.activeSelf);
		}


		//-*積み込みボタン群
		if(m_btnTumikomiList != null)
		{
			foreach(var tumi in m_btnTumikomiList){
				UnityEngine.Object.Destroy(tumi);
			}
			m_btnTumikomiList.Clear();
			m_btnTumikomiList = null;
		}
		m_btnTumikomiList = new List<GameObject>();
		var obj = (GameObject)Resources.Load("Prefabs/Debug/DebTumikomiBtnBase");

        // プレハブを元にオブジェクトを生成する
		for(int i = 0;i < m_tumikomiList.Length+(int)TUMIKOMI_PLUS.MAX;i++){
			var instantObj = (GameObject)Instantiate(obj,
								Vector3.zero,
								Quaternion.identity);
			if(instantObj == null){
				Debug.LogError("//-*Make TileBase Err:(デバッグ用積み込みボタン)No."+i);
				return;
			}
			instantObj.name = "TUmikomi"+String.Format("{0:00}", i);
			
			//-*スクロールビューに登録
			instantObj.transform.SetParent(m_tumikomiListBase.transform, false);
			m_btnTumikomiList.Add(instantObj);

			//-*ボタン設定
			DebBCtl = m_btnTumikomiList[i].GetComponent<DebButtonBase>();
			if(DebBCtl != null){
				DebBCtl.SetOnPointerClickCallback(ButtonTumikomiNo);
				string labelText = i.ToString();
				if(i>=m_tumikomiList.Length){
					labelText = TumikomiPlusDic[(TUMIKOMI_PLUS)(i-m_tumikomiList.Length)];
				}
				DebBCtl.InitLabel(i, labelText,false);
			}
    	}

		//-***局初期化ボタン
		m_isInitKyoku = false;


		//-***思考パラメータ
		DebBCtl = null;
		DebBCtl = m_btnThinkClose.GetComponent<DebButtonBase>();
		if(DebBCtl != null){
			DebBCtl.SetOnPointerClickCallback(ButtonThinkBoxOff);
			DebBCtl.InitLabel(0,"決定",false);
			// DebBCtl.ChangeOnOff(!m_box.activeSelf);
		}


		int paraListNo = 0;
		for(int a=0;a<m_btnThinkListP1.Count;a++,paraListNo++){
			m_btnThinkListP1[a].SetParaName(paraListNo,THINK_PARA_NAME[paraListNo]);
			m_btnThinkListP1[a].SetValue(0);
			m_btnThinkListP1[a].SetSlideValue(THINK_PARA_MINMAX[paraListNo][0],THINK_PARA_MINMAX[paraListNo][1]);
		}
		for(int a=0;a<m_btnThinkListP2.Count;a++,paraListNo++){
			m_btnThinkListP2[a].SetParaName(paraListNo,THINK_PARA_NAME[paraListNo]);
			m_btnThinkListP2[a].SetValue(0);
			m_btnThinkListP2[a].SetSlideValue(THINK_PARA_MINMAX[paraListNo][0],THINK_PARA_MINMAX[paraListNo][1]);
		}

		//-*各種フラグ
		for(int a=0;a<(int)FUNCTION_LIST.MAX;a++){
			if(a== (int)FUNCTION_LIST.THINK)continue;	//-*例外
			m_isEffect[a] = false;
		}

		ResetDebBox();

	}
	// Update is called once per frame
	void Update () {
		switch(m_useDebNo){
		//-*積み込みBox
		case FUNCTION_LIST.TUMIKOMI:
			UpdateTmikomiBox();
			break;
		//-*思考Box
		case FUNCTION_LIST.THINK:
			UpdateThinkBox();
			break;
		//-*大元/単体機能
		case FUNCTION_LIST.OPEN_PAI:
		case FUNCTION_LIST.INITGAME:
		case FUNCTION_LIST.GET_SCORE:
		case FUNCTION_LIST.DEFFO:
		default:
			UpdateDebBaseBox();
			break;
		}
		m_useDebNo = m_useNextDebNo;
	}






//-*************独立BOX
	public void UpdateDebBaseBox()
	{
		if(m_useNextDebNo == FUNCTION_LIST.DEFFO)return;
		switch(m_useNextDebNo){
		//-*積み込みBox
		case FUNCTION_LIST.TUMIKOMI:
			m_tumikomiBase.SetActive(true);
			// MahjongBase.DebugOpenPai = m_isEffect;
			break;
		case FUNCTION_LIST.THINK:
			m_thinkBase.SetActive(true);
			// MahjongBase.DebugOpenPai = m_isEffect;
			break;

		//-*大元/単体機能
		case FUNCTION_LIST.OPEN_PAI:
		case FUNCTION_LIST.INITGAME:
		case FUNCTION_LIST.GET_SCORE:
		default:
			break;
		}
	}
	
	private void UpdateTmikomiBox()
	{
		if(m_useNextDebNo == FUNCTION_LIST.TUMIKOMI)return;
		switch(m_useNextDebNo){
		//-*積み込みBox
		case FUNCTION_LIST.TUMIKOMI:
			m_tumikomiBase.SetActive(true);
			// MahjongBase.DebugOpenPai = m_isEffect;
			break;

		//-*大元/単体機能
		case FUNCTION_LIST.OPEN_PAI:
		case FUNCTION_LIST.INITGAME:
		case FUNCTION_LIST.GET_SCORE:
		default:
			break;
		}
		Debug.Log("UpdateUmikomiBox()");
	}

	private void UpdateThinkBox()
	{
		// if(m_useNextDebNo == FUNCTION_LIST.THINK)return;
		int paraListNo = 0;
		for(int a=0;a<m_btnThinkListP1.Count;a++,paraListNo++){
			m_paraValList[paraListNo] = m_btnThinkListP1[a].VAL_NO;
		}
		for(int a=0;a<m_btnThinkListP2.Count;a++,paraListNo++){
			m_paraValList[paraListNo] = m_btnThinkListP2[a].VAL_NO;
		}
		if(m_btnThinkNext.ISPUSH || m_btnThinkNext.ISPUSH){
			m_thinkListBase[0].SetActive(!m_thinkListBase[0].activeSelf);
			m_thinkListBase[1].SetActive(!m_thinkListBase[1].activeSelf);
		}
	}
	public int[] GetDebPara()
	{
		return m_paraValList;
	}
//-*************

	public bool GetDebugFlag(FUNCTION_LIST no, bool isOneTime = false)
	{
		bool debFlag = m_isEffect[(int)no];
		if(isOneTime){
			m_isEffect[(int)no] = false;
			var DebBCtl = m_btnOnOff.GetComponent<DebButtonBase>();
			// if(DebBCtl != null)
			// {
			// 	DebBCtl.ChangeOnOff(true);
			// }
		}
		return debFlag;
	}
	public void SetDebugFlagOFF(FUNCTION_LIST no)
	{
		m_isEffect[(int)no] = false;
	}
	public int GetTumikomiNo()
	{
		return m_tumikomiNo;
	}
	public bool IsSameTumikomi()
	{
		return (m_tumikomiNo-m_tumikomiList.Length == (int)TUMIKOMI_PLUS.SAME_KYOKU);
	}
	public void SetTumikomiSameKyoku(byte[] haiLine)
	{
		// m_tumikomiSame = 
		// haiLine.CopyTo(m_tumikomiSame,0);
		// Array.Copy(haiLine,m_tumikomiSame,haiLine.Length);
		string text = "LINE:";
		for(int a =0;a<m_tumikomiSame.Length;a++){
			text += " ,"+a+":"+m_tumikomiSame[a];
			m_tumikomiSame[a] = haiLine[a];
		}
		Debug.Log(text);
	}
	public byte[] GetTumikomiSameKyoku()
	{
		return m_tumikomiSame;
	}

	private void ResetDebBox()
	{
		m_useDebNo = m_useNextDebNo = FUNCTION_LIST.DEFFO;
		m_tumikomiBase.SetActive(false);
		m_thinkBase.SetActive(false);
	}

	// //---------------------------------------------------------
	// /// <summary>
	// /// クリック
	// /// </summary>
	// //---------------------------------------------------------
	/// <summary>
	/// デバッグBOX本体のON/OFF
	/// </summary>
	public void ButtonDebBoxOnOff()
	{
		var BCtlOnOff = m_btnOnOff.GetComponent<DebButtonBase>();
		if(BCtlOnOff != null){
			BCtlOnOff.ChangeOnOff(m_box.activeSelf);
		}
		m_box.SetActive(!m_box.activeSelf);
		ResetDebBox();
	}
	/// <summary>
	/// 各種デバッグボタン
	/// </summary>
	public void ButtonSelectStageTest(int a,bool flag)
	{
		m_useNextDebNo = (FUNCTION_LIST)a;
		m_isEffect[a] = flag;
	}


	/// <summary>
	/// 積み込みBOXのCloseボタン
	/// </summary>
	public void ButtonTumikomiBoxOff()
	{
		m_tumikomiBase.SetActive(false);
		m_useNextDebNo = FUNCTION_LIST.DEFFO;
	}
	/// <summary>
	/// 積み込み番号ボタン
	/// </summary>
	public void ButtonTumikomiNo(int a)
	{
		m_tumikomiNo = -1;
		if(a < m_tumikomiList.Length+(int)TUMIKOMI_PLUS.DEL){
			m_tumikomiNo = a;
		}
		foreach(var obj in m_btnTumikomiList)
		{
			var btn = obj.GetComponent<DebButtonBase>();
			if(btn == null)continue;
			btn.ChangeButtonColor( (m_tumikomiNo==btn.ButtonNo) );
		}
		
	}

	//-***思考
	/// <summary>
	/// 思考BOXのCloseボタン
	/// </summary>
	public void ButtonThinkBoxOff()
	{
		m_thinkBase.SetActive(false);
		m_isEffect[(int)FUNCTION_LIST.THINK] = true;
		m_useNextDebNo = FUNCTION_LIST.DEFFO;
	}

#endif //-*todo:注デバッグ中
//-****************************SUGI_DEB

}
