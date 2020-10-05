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

	// Use this for initialization
	void Start () {//-*todo:awakeにするかも
		InitAllBase();
		InitTUmikomi();
		InitThinkPara();
		InitAddDora();
		InitLuckPara();

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
		//-*追加ドラBox
		case FUNCTION_LIST.ADD_DORA:
			UpdateAddDoraBox();
			break;
		//-*運Box
		case FUNCTION_LIST.LUCK:
			UpdateLuckBox();
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

#region ALLBASE
//-***大元Box
	[Header("Box外")]
	[SerializeField]
	private GameObject m_btnOnOff;
	[SerializeField]
	private GameObject m_btnReset;

	[Header("Box内")]
	[SerializeField]
	private GameObject m_box;
	[SerializeField]
	private List<GameObject> m_btnDebList = new List<GameObject>();

	//-****変数・定数
	private FUNCTION_LIST m_useDebNo;	//-*デバッグ機能番号
	private FUNCTION_LIST m_useNextDebNo;	//-*デバッグ機能番号
	private bool[] m_isEffect = new bool[(int)FUNCTION_LIST.MAX];
	private const int PAI_MAX = 34;

	public enum FUNCTION_LIST : int
	{//-*デバッグ機能
		OPEN_PAI = 0,
		TUMIKOMI,
		INITGAME,
		GET_SCORE,
		THINK,
		ADD_DORA,
		LUCK,
		INITHAVEHAND,
		TUMOPOSTEST,
		MAX,
		DEFFO = 99,	//-*デバッグBox大元：ラベル未使用にも代用
	}

	/// <summary>
	/// デバッグリスト
	/// </summary>
	private static Dictionary<FUNCTION_LIST,string> FunctionDic = new Dictionary<FUNCTION_LIST,string>()
	{
		{ FUNCTION_LIST.OPEN_PAI,		"相手の牌："},
		{ FUNCTION_LIST.TUMIKOMI,		"積み込み"	},
		{ FUNCTION_LIST.INITGAME,		"積込実行"	},
		{ FUNCTION_LIST.GET_SCORE,		"点移動ﾅｼ："},
		{ FUNCTION_LIST.THINK,			"思考"		},
		{ FUNCTION_LIST.ADD_DORA,		"ドラ指定"	},
		{ FUNCTION_LIST.LUCK,			"運 "		},
		{ FUNCTION_LIST.INITHAVEHAND,	"手牌で初期化"},
		{ FUNCTION_LIST.TUMOPOSTEST,	"ﾂﾓ場所変："},
		{ FUNCTION_LIST.MAX,			"" },			//-*追加するならこれより↑に
		{ FUNCTION_LIST.DEFFO,			"" },
	};

	/// <summary>
	/// ラベルにON/OFFが要るか[true:必要]
	/// 専用BOXがあるものはON/OFF不要
	/// </summary>
	private static Dictionary<FUNCTION_LIST,bool> IsFunctionChangeLabelDic = new Dictionary<FUNCTION_LIST,bool>()
	{
		{ FUNCTION_LIST.OPEN_PAI,	true},
		{ FUNCTION_LIST.TUMIKOMI,	false},
		{ FUNCTION_LIST.INITGAME,	false},
		{ FUNCTION_LIST.GET_SCORE,	true},
		{ FUNCTION_LIST.THINK,		false},
		{ FUNCTION_LIST.ADD_DORA,	false},
		{ FUNCTION_LIST.LUCK,		false},
		{ FUNCTION_LIST.INITHAVEHAND,false},
		{ FUNCTION_LIST.TUMOPOSTEST,true},
		{ FUNCTION_LIST.MAX,		false},			//-*追加するならこれより↑に
		{ FUNCTION_LIST.DEFFO,		false},
	};
	private void InitAllBase()
	{//-*初期化
		//-*デバッグ設定On/OFFボタン
		var DebBCtl = m_btnOnOff.GetComponent<DebButtonBase>();
		if(DebBCtl != null){
			DebBCtl.SetOnPointerClickCallback(ButtonDebBoxOnOff);
			DebBCtl.InitLabel(0, FunctionDic[FUNCTION_LIST.DEFFO]);
			DebBCtl.ChangeOnOff(!m_box.activeSelf);
		}
		//-*Resetボタン(積み込み｢同｣+新規局へ)
		DebBCtl = null;
		DebBCtl = m_btnReset.GetComponent<DebButtonBase>();
		if(DebBCtl != null){
			DebBCtl.SetOnPointerClickCallback(ButtonRestart);
			DebBCtl.IsChangeLabel = false;
		}

		DebBCtl = null;
		for(int a = 0;a<m_btnDebList.Count;a++){
			DebBCtl = m_btnDebList[a].GetComponent<DebButtonBase>();
			if(DebBCtl != null){
				DebBCtl.SetOnPointerClickCallback(ButtonSelectStageTest);
				DebBCtl.IsChangeLabel = IsFunctionChangeLabelDic[(FUNCTION_LIST)a];
				DebBCtl.InitLabel(a, FunctionDic[(FUNCTION_LIST)a]);
			}
		}
		//-*各種フラグ
		for(int a=0;a<(int)FUNCTION_LIST.MAX;a++){
			if(a== (int)FUNCTION_LIST.THINK)continue;	//-*例外
			m_isEffect[a] = false;
		}
	}

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
		case FUNCTION_LIST.ADD_DORA:
			m_DoraBoxBase.SetActive(true);
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

	public bool GetDebugFlag(FUNCTION_LIST no, bool isOneTime = false)
	{
		bool debFlag = m_isEffect[(int)no];
		if(isOneTime){
			m_isEffect[(int)no] = false;
			var DebBCtl = m_btnOnOff.GetComponent<DebButtonBase>();
		}
		return debFlag;
	}
	public void SetDebugFlagOFF(FUNCTION_LIST no)
	{
		m_isEffect[(int)no] = false;
	}
	private void ResetDebBox()
	{
		m_useDebNo = m_useNextDebNo = FUNCTION_LIST.DEFFO;
		m_tumikomiBase.SetActive(false);
		m_thinkBase.SetActive(false);
		m_DoraBoxBase.SetActive(false);
		m_luckBoxBase.SetActive(false);
	}
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
	/// やり直すボタン(積み込み｢同｣+新規局へ)
	/// </summary>
	public void ButtonRestart(int a,bool flag)
	{
		//-*自分を登録
		ButtonSelectStageTest(a,flag);
		m_tumikomiNo = (m_tumikomiList.Length + (int)TUMIKOMI_PLUS.SAME_KYOKU);
		//-*新規局ON
		ButtonSelectStageTest((int)FUNCTION_LIST.INITGAME,true);
		//-*積み込みBox内ボタンの更新
		ButtonTumikomiNo(m_tumikomiNo);
	}

#endregion	//-*ALLBASE

#region TUMIKOMI
//-***積み込みBox
	[Header("積み込みBox")]
	[SerializeField]
	private GameObject m_tumikomiBase;
	[SerializeField]
	private GameObject m_tumikomiListBase;
	[SerializeField]
	private List<GameObject> m_btnTumikomiList = new List<GameObject>();
	[SerializeField]
	private GameObject m_btnTumikomiClose;

	//-*変数定数
	private byte [][] m_tumikomiList = MJDefine.tumiData;
	private int m_tumikomiNo = -1;
	private byte[]	m_tumikomiSame= new byte [ 136 ];	//-*全牌の並び同じ局を繰り返す用

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

	private void InitTUmikomi()
	{//-*初期化
		//-***積み込みBox
		var DebBCtl = m_btnTumikomiClose.GetComponent<DebButtonBase>();
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
#endregion	//-*TUMIKOMI

#region THINKPARA
//-***思考パラメータ
	[Header("思考操作Box")]
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

	//-*変数定数
	private int[] m_paraThinkValList = new int[THINK_PARA_NAME.Length];

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

	private void InitThinkPara()
	{//-*初期化
		var DebBCtl = m_btnThinkClose.GetComponent<DebButtonBase>();
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
	}

	private void UpdateThinkBox()
	{
		// if(m_useNextDebNo == FUNCTION_LIST.THINK)return;
		int paraListNo = 0;
		for(int a=0;a<m_btnThinkListP1.Count;a++,paraListNo++){
			m_paraThinkValList[paraListNo] = m_btnThinkListP1[a].VAL_NO;
		}
		for(int a=0;a<m_btnThinkListP2.Count;a++,paraListNo++){
			m_paraThinkValList[paraListNo] = m_btnThinkListP2[a].VAL_NO;
		}
		if(m_btnThinkNext.ISPUSH || m_btnThinkNext.ISPUSH){
			m_thinkListBase[0].SetActive(!m_thinkListBase[0].activeSelf);
			m_thinkListBase[1].SetActive(!m_thinkListBase[1].activeSelf);
		}
	}

	public int[] GetDebThinkPara()
	{
		return m_paraThinkValList;
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
	/// <summary>
	/// 思考BOXのCloseボタン
	/// </summary>
	public void ButtonThinkBoxOff()
	{
		m_thinkBase.SetActive(false);
		m_isEffect[(int)FUNCTION_LIST.THINK] = true;
		m_useNextDebNo = FUNCTION_LIST.DEFFO;
	}

#endregion	//-*THINKPARA

#region ADDDORA
//-***ドラBox
	[Header("ドラ追加Box")]
	[SerializeField]
	private GameObject m_DoraBoxBase;
	[SerializeField]
	private GameObject m_DoraListBase;
	[SerializeField]
	private List<GameObject> m_btnDoraList = new List<GameObject>();
	[SerializeField]
	private GameObject m_btnDoraClose;

	//-*変数定数
	private List<byte> m_addDoraList = new List<byte>();

	private void InitAddDora()
	{//-*初期化

		var DebBCtl = m_btnDoraClose.GetComponent<DebButtonBase>();
		if(DebBCtl != null){
			DebBCtl.SetOnPointerClickCallback(ButtonDoraBoxOff);
			DebBCtl.InitLabel(0,"閉じる",false);
			// DebBCtl.ChangeOnOff(!m_box.activeSelf);
		}


		//-*追加ドラボタン群
		if(m_btnDoraList != null)
		{
			foreach(var dora in m_btnDoraList){
				UnityEngine.Object.Destroy(dora);
			}
			m_btnDoraList.Clear();
			m_btnDoraList = null;
		}
		m_btnDoraList = new List<GameObject>();
		var objDora = (GameObject)Resources.Load("Prefabs/Debug/DebTileBaseDoraSel");

		// プレハブを元にオブジェクトを生成する
		int AddDdoraPlusBtnNum = 5;	//-*一括ボタンの追加
		for(int i = 0;i < (int)TILE_LIST.TILES_MAX+AddDdoraPlusBtnNum;i++){
			var instantObj = (GameObject)Instantiate(objDora,
								Vector3.zero,
								Quaternion.identity);
			if(instantObj == null){
				Debug.LogError("//-*Make TileBase Err:(デバッグ用ドラボタン)No."+i);
				return;
			}
			instantObj.name = "TUmikomi"+String.Format("{0:00}", i);
			
			//-*スクロールビューに登録
			instantObj.transform.SetParent(m_DoraListBase.transform, false);
			m_btnDoraList.Add(instantObj);
			if(i >= (int)TILE_LIST.TILES_MAX){
			//-*一括ボタンの位置調整
				int hierarchyPos = 9+10*(i-(int)TILE_LIST.TILES_MAX);
				instantObj.transform.SetSiblingIndex(hierarchyPos);
			}

			//-*ボタン設定
			var debMjTile = m_btnDoraList[i].GetComponent<DebDoraTIle>();
			if(debMjTile != null){
				var pai = debMjTile.GetDicPaiListValue(TILE_LIST.BACK);
				debMjTile.DebDoraSelTypeAll = -1;
				if(i < (int)TILE_LIST.TILES_MAX){
					pai = debMjTile.GetDicPaiListValue((TILE_LIST)i);
					debMjTile.useText(false);
				}else{
					debMjTile.DebDoraSelTypeAll = (i-(int)TILE_LIST.TILES_MAX);
					switch(debMjTile.DebDoraSelTypeAll){
					case 0:
						debMjTile.useText(true,"萬子");
						break;
					case 1:
						debMjTile.useText(true,"索子");
						break;
					case 2:
						debMjTile.useText(true,"筒子");
						break;
					case 3:
						debMjTile.useText(true,"字牌");
						break;
					default:
						debMjTile.useText(true,"ALL");
						break;
					}
				}
				debMjTile.set(TILE_STATE.MY_DISCARDED,pai);
				debMjTile.SetDebClickCallback(ButtonDoraAddNo);
			}
		}
	}

	private void UpdateAddDoraBox()
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
	}
	/// <summary>
	/// ドラBOXのCloseボタン
	/// </summary>
	public void ButtonDoraBoxOff()
	{
		m_DoraBoxBase.SetActive(false);
		m_useNextDebNo = FUNCTION_LIST.DEFFO;
	}

	/// <summary>
	/// 追加ドラ選択ボタン
	/// </summary>
	public void ButtonDoraAddNo(int paiNo,int allType, bool isOn)
	{
		byte pai = (byte)paiNo;
		byte startNo = 0;
		byte endNo = 0;
		int onOffStart = 0;
		int onOffNum = 9;
		if(isOn){
			if(allType != -1){
			//-*一括
				switch(allType){
				case 0://-*萬子
					startNo = 0x01;
					endNo = 0x09;
					onOffStart = (allType*9);
					break;
				case 1://-*索子
					startNo = 0x11;
					endNo = 0x19;
					onOffStart = (allType*9);
					break;
				case 2://-*筒子
					startNo = 0x21;
					endNo = 0x29;
					onOffStart = (allType*9);
					break;
				case 3://-*字牌
					startNo = 0x31;
					endNo = 0x38;
					onOffStart = (allType*9);
					onOffNum = 8;
					break;
				case 4://-*全部
					startNo = 0x01;
					endNo = 0x38;
					onOffNum = m_btnDoraList.Count;
					onOffStart = 0;
					break;
				default:
					break;

				}
				//-*ドラリスト一括追加
				for(byte lumpPai = startNo;lumpPai<=endNo;lumpPai++){
					if(!m_addDoraList.Contains((byte)lumpPai))
					{
						m_addDoraList.Add((byte)lumpPai);
					}
				}
				//-*ドラボタントグルON
				for(int btnListNo=0;btnListNo<onOffNum;btnListNo++){
					var debMjTile = m_btnDoraList[onOffStart+btnListNo].GetComponent<DebDoraTIle>();
					debMjTile.SetToggleOnOff(true);
				}
			}else{
				if(!m_addDoraList.Contains((byte)pai))
				{
					m_addDoraList.Add((byte)pai);
				}
			}
		}else{
			if(allType != -1){
			//-*一括
				switch(allType){
				case 0://-*萬子
					startNo = 0x01;
					endNo = 0x09;
					onOffStart = (allType*9);
					break;
				case 1://-*索子
					startNo = 0x11;
					endNo = 0x19;
					onOffStart = (allType*9);
					break;
				case 2://-*筒子
					startNo = 0x21;
					endNo = 0x29;
					onOffStart = (allType*9);
					break;
				case 3://-*字牌
					startNo = 0x31;
					endNo = 0x38;
					onOffStart = (allType*9);
					onOffNum = 8;
					break;
				case 4://-*全部
					startNo = 0x01;
					endNo = 0x38;
					onOffNum = m_btnDoraList.Count;
					onOffStart = 0;
					break;
				default:
					break;

				}
				//-*ドラリスト一括追加
				for(byte lumpPai = startNo;lumpPai<=endNo;lumpPai++){
					if(m_addDoraList.Contains((byte)lumpPai))
					{
						m_addDoraList.Remove((byte)lumpPai);
					}
				}
				//-*ドラボタントグルON
				for(int btnListNo=0;btnListNo<onOffNum;btnListNo++){
					var debMjTile = m_btnDoraList[onOffStart+btnListNo].GetComponent<DebDoraTIle>();
					debMjTile.SetToggleOnOff(false);
				}
			}else{
				if(m_addDoraList.Contains((byte)pai))
				{
					m_addDoraList.Remove((byte)pai);
				}
			}
		}
		m_addDoraList.Sort();
		Debug.Log("//-*****ButtonDoraAddNo(int "+paiNo+"("+pai+"),int "+allType+", bool "+isOn+")*****");
		for(int listNo=0;listNo<m_addDoraList.Count;listNo++){
			Debug.Log("//-*No."+listNo+":"+m_addDoraList[listNo]);
		}
	}
	public List<byte> GetDebAddDora()
	{
		return m_addDoraList;
	}

#endregion	//-*ADDDORA

#region LUCKPARA
//-***運
	[Header("運Box")]
	[SerializeField]
	private GameObject m_luckBoxBase;
	[SerializeField]
	private List<DebLuckBox> m_luckListBase = new List<DebLuckBox>();
	[SerializeField]
	private GameObject m_btnLuckClose;
	//-*変数定数
	private int[] luck_paraValList = new int[2];	//-*自分と相手の運
	private string[] LUCK_NAME = {"自分","相手"};

	private void InitLuckPara()
	{//-*初期化
		var DebBCtl = m_btnLuckClose.GetComponent<DebButtonBase>();
		if(DebBCtl != null){
			DebBCtl.SetOnPointerClickCallback(ButtonLuckBoxOff);
			DebBCtl.InitLabel(0,"閉じる",false);
			// DebBCtl.ChangeOnOff(!m_box.activeSelf);
		}

		for(int a=0;a<m_luckListBase.Count;a++){
			m_luckListBase[a].SetParaName(a,LUCK_NAME[a]+"の運");
			m_luckListBase[a].SetValue(0);
			m_luckListBase[a].SetSlideValue(0,10);
		}
	}
	/// <summary>
	/// 運BOXのCloseボタン
	/// </summary>
	public void ButtonLuckBoxOff()
	{
		m_luckBoxBase.SetActive(false);
		m_isEffect[(int)FUNCTION_LIST.LUCK] = true;
		m_useNextDebNo = FUNCTION_LIST.DEFFO;
	}
	private void UpdateLuckBox()
	{
		// if(m_useNextDebNo == FUNCTION_LIST.THINK)return;
		if(m_useNextDebNo == FUNCTION_LIST.TUMIKOMI)return;
		switch(m_useNextDebNo){
		//-*積み込みBox
		case FUNCTION_LIST.LUCK:
			m_luckBoxBase.SetActive(true);
			// MahjongBase.DebugOpenPai = m_isEffect;
			break;

		//-*大元/単体機能
		case FUNCTION_LIST.OPEN_PAI:
		case FUNCTION_LIST.INITGAME:
		case FUNCTION_LIST.GET_SCORE:
		default:
			break;
		}

		for(int a=0,paraListNo=0;a<m_luckListBase.Count;a++,paraListNo++){
			luck_paraValList[paraListNo] = m_luckListBase[a].VAL_NO;
		}
	}

	public int[] GetDebLuckPara()
	{
		return luck_paraValList;
	}
#endregion	//-*LUCKPARA

#region TUMOPOSCHANGE
//-*自摸場所の変更
	private List<byte> tpc_myStartHand = new List<byte>();	//-*現在の自分の手牌
	private List<byte> tpc_yourStartHand = new List<byte>();//-*現在の相手の手配
	private List<byte> tpc_paiLineBase = new List<byte>();//-*局の牌並び(保存したら変えない)
	private List<byte> tpc_paiLineBefor = new List<byte>();//-*局の牌並び(変更前)
	private List<byte> tpc_paiLineAfter = new List<byte>();//-*局の牌並び(変更後)

	
	/// <summary>
	/// 牌の並び格納
	/// </summary>
	public void DebSetTPCPaiLine(byte[] paiLine, byte Bpcnt)
	{
		string text = "";
		//-*初期化
		tpc_paiLineBase.Clear();
		tpc_paiLineBase = tpc_paiLineBefor = tpc_paiLineAfter = paiLine.ToList();

		text = "Num:"+tpc_paiLineBase.Count+" _LINE:";
		for(int a =0;a<tpc_paiLineBase.Count;a++){
			text += " ,"+a+"["+tpc_paiLineBase[a]+"]";
		}
		Debug.Log(text);
		
		text = "tpc_paiLineBase["+Bpcnt+"]:"+tpc_paiLineBase[Bpcnt];
		Debug.Log(text);

	}
	
	/// <summary>
	/// 今の手牌を配牌場所にセット
	/// </summary>
	public void DebSetTPCNowHand(PLAYERWORK[] pWork)
	{
		string text = "";
		//-*初期化
		tpc_myStartHand.Clear();
		tpc_yourStartHand.Clear();


		tpc_myStartHand = pWork[0].byTehai.ToList();
		text = "Num:"+tpc_myStartHand.Count+" NowMyHand:";
		for(int a =0;a<tpc_myStartHand.Count;a++){
			text += " ,"+a+"["+tpc_myStartHand[a]+"]";
		}
		Debug.Log(text);

		tpc_yourStartHand = pWork[1].byTehai.ToList();
		text = "Num:"+tpc_yourStartHand.Count+" NowYourHand:";
		for(int a =0;a<tpc_yourStartHand.Count;a++){
			text += " ,"+a+"["+tpc_yourStartHand[a]+"]";
		}
		Debug.Log(text);

		var test1 = pWork[2].byTehai.ToList();
		text = "Num:"+test1.Count+" TEST1:";
		for(int a =0;a<test1.Count;a++){
			text += " ,"+a+"["+test1[a]+"]";
		}
		Debug.Log(text);
		var test2 = pWork[3].byTehai.ToList();
		text = "Num:"+test2.Count+" TEST2:";
		for(int a =0;a<test2.Count;a++){
			text += " ,"+a+"["+test2[a]+"]";
		}
		Debug.Log(text);


		//-*手牌を牌列と入れ替え
		int kubaru = 0;
		int hito = 0;
		int no=0;
#if false
		for(kubaru=0;kubaru<3;kubaru++){
			for(hito = 0;hito<4;hito++){
				for( no=0;no<4;no++){
					if((kubaru*4)+no >= 13) break;
					tpc_paiLineAfter[(kubaru*16)+(hito*4)+no] = pWork[hito].byTehai[(kubaru*4)+no];
				}
			}
		}
		Debug.Log("//-********Affter****("+pWork[0].byTehai.Length+")**("+kubaru+","+hito+","+no+")**");
		
		for(hito = 0;hito<4;hito++){
				tpc_paiLineAfter[(kubaru*4)+hito] = pWork[hito].byTehai[12];
		}
#else
		int paiCnt = 0;
		for(kubaru=0;kubaru<3;kubaru++){
			for(hito = 0;hito<4;hito++){
				for( no=0;no<4;no++){
					if((kubaru*4)+no >= 13) break;
					tpc_paiLineAfter[paiCnt] = pWork[hito].byTehai[(kubaru*4)+no];
					paiCnt++;
				}
			}
		}
		Debug.Log("//-********Affter****("+pWork[0].byTehai.Length+")**("+kubaru+","+hito+","+no+")**");
		for(hito = 0;hito<4;hito++){
			tpc_paiLineAfter[paiCnt] = pWork[hito].byTehai[12];
			paiCnt++;
		}
#endif
		CheckTileLine(tpc_paiLineAfter.ToArray());
		DebSetTPCPaiLine(tpc_paiLineAfter.ToArray(),0);
	}
	public byte[] DebGetTPCPaiLine()
	{

		return tpc_paiLineAfter.ToArray();
	}

	/// <summary>
	/// 牌数の確認
	/// </summary>
	public bool CheckTileLine(byte[] paiLineArray)
	{
		List<byte> paiList = paiLineArray.ToList();
		bool isSafe = true;
		//-*enum PAI を順番に
		foreach (var i in Enum.GetValues(typeof(PAI)).Cast<PAI>())
		{
			int paiNum = paiList.Count(n => n==(byte)i);
			// Debug.Log("//-*牌名："+i+"["+(byte)i+"("+(int)i+")]___枚数："+paiNum);
			if(paiNum != MJDefine.ONE_TILES_NUM_MAX && i != PAI.URA){
			//-*1牌が4枚でなければエラー
				Debug.LogError("//-*牌名："+i+"["+(byte)i+"("+(int)i+")]___枚数："+paiNum);
				isSafe = false;
			}
		}
		return isSafe;
	}

#endregion	//-*TUMOPOSCHANGE
#endif //-*todo:注デバッグ中
//-****************************SUGI_DEB

}
