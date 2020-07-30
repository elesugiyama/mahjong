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


	//-*変数
	private FUNCTION_LIST m_useDebNo;	//-*デバッグ機能番号
	private FUNCTION_LIST m_useNextDebNo;	//-*デバッグ機能番号
	private bool[] m_isEffect = new bool[(int)FUNCTION_LIST.MAX];
	private byte [][] m_tumikomiList = MJDefine.tumiData;
	private int m_tumikomiNo = -1;
	private bool m_isInitKyoku = false;
	private byte[]	m_tumikomiSame= new byte [ 136 ];	//-*全牌の並び同じ局を繰り返す用

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
			DebBCtl.InitLabel(0,"Close",false);
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



		//-*各種フラグ
		for(int a=0;a<(int)FUNCTION_LIST.MAX;a++){
			m_isEffect[a] = false;
		}

		ResetDebBox();

	}
	// Update is called once per frame
	void Update () {
		switch(m_useDebNo){
		//-*積み込みBox
		case FUNCTION_LIST.TUMIKOMI:
			UpdateUmikomiBox();
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

		//-*大元/単体機能
		case FUNCTION_LIST.OPEN_PAI:
		case FUNCTION_LIST.INITGAME:
		case FUNCTION_LIST.GET_SCORE:
		default:
			break;
		}
	}
	
	private void UpdateUmikomiBox()
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
#endif //-*todo:注デバッグ中
//-****************************SUGI_DEB

}
