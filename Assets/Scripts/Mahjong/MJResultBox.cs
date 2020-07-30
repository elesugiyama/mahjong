using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
/// リザルト画面
/// </summary>
public class MJResultBox : MonoBehaviour {

	[Header("役")]
	[SerializeField]
	private GameObject[] m_yakuList = new GameObject[MJDefine.MAX_YAKU];	//-役リスト最大12個

	[Header("ドラ")]
	[SerializeField]
	private GameObject m_doraSetBase;	//-ドラ
	[SerializeField]
	private GameObject m_doraUraSetBase;	//-*裏ドラ

	[SerializeField]
	private Text m_totalPoint;	//-*〇符〇翻〇点

	
	private MJK_RESULT m_result = null;	//-*リザルトデータ

	private byte[] m_doraList = new byte[4];
	private byte[] m_doraUraList = new byte[4];
	private byte m_doraCnt = 0;

	int		yakuCount	= 0;		// 和了した手が何翻か？
	int		yakuNumber	= 0;		// 役の番号。
	int		yakuFactor	= 0;		// 役の翻数。
	int		FU			= 0;		// 符。
	int		HAN			= 0;		// 翻。
	int		RENCYAN		= 0;		// 何本場？
	int		TotalPoint	= 0;		// 和了した点数は？
	int		iOrder		= -1;		// 現在のオーナーを取得。
	short	Rich		= 0;	// リーチした場合。

	public void InitResult()
	{
		//-*リザルトデータ
		if(m_result != null){
			m_result = null;
		}
		m_result = new MJK_RESULT();
		yakuCount	= 0;		// 和了した手が何翻か？
		yakuNumber	= 0;		// 役の番号。
		yakuFactor	= 0;		// 役の翻数。
		FU			= 0;		// 符。
		HAN			= 0;		// 翻。
		RENCYAN		= 0;		// 何本場？
		TotalPoint	= 0;		// 和了した点数は？
		iOrder		= -1;		// 現在のオーナーを取得。
		Rich		= 0;		// リーチした場合。		
		//-*ドラ
		m_doraList = new byte[4];
		m_doraUraList = new byte[4];
		m_doraCnt = 0;
		//-*役
		for(int i=0;i<MJDefine.MAX_YAKU;i++){
			m_yakuList[i].SetActive(false);
		}
		m_totalPoint.text = "";
	}
	public void SetResultBox(MJK_RESULT result, short rich, byte[] dora, byte[] ura,byte doraCnt,bool isSpecialYaku)
	{
		//-*リザルトデータ
		InitResult();	//-*初期化
		m_result = result;
		yakuCount = m_result.byYakuCnt;
		FU			= (int)m_result.byFu;		// 符。
		HAN			= (int)m_result.byHan;		// 翻。
		RENCYAN		= (int)m_result.byRenchan;	// 何本場？
		TotalPoint	= (int)m_result.nTotalPoint;	// 和了した得点。
		Rich		= rich;
		m_doraList = dora;
		m_doraUraList = ura;
		m_doraCnt = doraCnt;

		//-*ドラ,裏ドラ表示
		var setDora = m_doraSetBase.GetComponent<SetResultDora>();
		if(setDora != null){
			setDora.SetDoraImage(m_doraList,m_doraCnt);
		}

		var setUraDora = m_doraUraSetBase.GetComponent<SetResultDora>();
		if(setUraDora != null){
			setUraDora.SetDoraImage(m_doraUraList,m_doraCnt);
		}

		for(int i=0;i<MJDefine.MAX_YAKU;i++){
			if(i < yakuCount){
				m_yakuList[i].SetActive(true);
				var yakuList = m_yakuList[i].GetComponent<MJResultYakuList>();
				if(yakuList != null){
					yakuList.SetImage(m_result.sYaku[i].name);
					// 役数・飜
					if( m_result.byYakuman == 0 && isSpecialYaku == false ) {		//0422
						yakuFactor = m_result.sYaku[i].factor;
						yakuList.SetFactor(yakuFactor);
					}
				}
			}else{
				m_yakuList[i].SetActive(false);
			}
		}
		if(result.byYakuman == 0 && isSpecialYaku == false ) {	//> 2006/03/23 流し満貫、十三不塔対策	//0422
			m_totalPoint.alignment = TextAnchor.MiddleLeft;
			m_totalPoint.text = FU+"符　"+HAN+"翻　"+TotalPoint+"点";
		}else{
			m_totalPoint.alignment = TextAnchor.MiddleRight;
			m_totalPoint.text = TotalPoint+"点";
		}

	}



	// Use this for initialization
	void Start () {}
	// Update is called once per frame
	void Update () {}
	


	// //---------------------------------------------------------
	// /// <summary>
	// /// クリック
	// /// </summary>
	// //---------------------------------------------------------
	// public override void OnPointerClick(PointerEventData eventData)
    // {
	// 	if(target == null){
	// 		Debug.Log("//-*Button:target is null");
	// 		return;
	// 	}
    //     target.OnPointerClick(eventData);
	// 	// コールバック
	// 	if (m_OnPointerClickCallback != null)
	// 	{
	// 		m_OnPointerClickCallback();
	// 	}
    // }
}
