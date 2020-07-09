using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
public class MJTIle : MonoBehaviour {
	[Header("Image")]
	[SerializeField]
	private MJTIleImage m_tileImage;

	//-*リスト内番号
	private PAI m_listNo;
	public PAI ListNo{
		get {return m_listNo;}
	}
	//-*牌種類
	private TILE_TYPE m_tileType;
	public TILE_TYPE TileType{
		get {return m_tileType;}
	}
	//-*牌数字
	private int m_no;
	public int No{
		get {return m_no;}
	}

	//-*牌状態
	private TILE_STATE m_tileState;
	public TILE_STATE TileState{
		get {return m_tileState;}
		set {m_tileState = value;}
	}


	/// <summary>
	/// 牌画像と内部番号の連結
	/// </summary>
	public Dictionary<PAI, TILE_LIST> PAI_LIST = new Dictionary<PAI,TILE_LIST>()
	{
	//-*萬子
		{ PAI.M1,TILE_LIST.CHARACTERS_1 },		//
		{ PAI.M2,TILE_LIST.CHARACTERS_2 },		//
		{ PAI.M3,TILE_LIST.CHARACTERS_3 },		//
		{ PAI.M4,TILE_LIST.CHARACTERS_4 },		//
		{ PAI.M5,TILE_LIST.CHARACTERS_5 },		//
		{ PAI.M6,TILE_LIST.CHARACTERS_6 },		//
		{ PAI.M7,TILE_LIST.CHARACTERS_7 },		//
		{ PAI.M8,TILE_LIST.CHARACTERS_8 },		//
		{ PAI.M9,TILE_LIST.CHARACTERS_9 },		//
	//-*索子
		{ PAI.S1,TILE_LIST.BAMBOOS_1 },		//
		{ PAI.S2,TILE_LIST.BAMBOOS_2 },		//
		{ PAI.S3,TILE_LIST.BAMBOOS_3 },		//
		{ PAI.S4,TILE_LIST.BAMBOOS_4 },		//
		{ PAI.S5,TILE_LIST.BAMBOOS_5 },		//
		{ PAI.S6,TILE_LIST.BAMBOOS_6 },		//
		{ PAI.S7,TILE_LIST.BAMBOOS_7 },		//
		{ PAI.S8,TILE_LIST.BAMBOOS_8 },		//
		{ PAI.S9,TILE_LIST.BAMBOOS_9 },		//
	//-*筒子
		{ PAI.P1,TILE_LIST.CIRCLES_1 },		//
		{ PAI.P2,TILE_LIST.CIRCLES_2 },		//
		{ PAI.P3,TILE_LIST.CIRCLES_3 },		//
		{ PAI.P4,TILE_LIST.CIRCLES_4 },		//
		{ PAI.P5,TILE_LIST.CIRCLES_5 },		//
		{ PAI.P6,TILE_LIST.CIRCLES_6 },		//
		{ PAI.P7,TILE_LIST.CIRCLES_7 },		//
		{ PAI.P8,TILE_LIST.CIRCLES_8 },		//
		{ PAI.P9,TILE_LIST.CIRCLES_9 },		//
	//-*字牌
		{ PAI.TON,  TILE_LIST.HONOURS_EAST  },	//
		{ PAI.NAN,  TILE_LIST.HONOURS_SOUTH },	//
		{ PAI.SYA,  TILE_LIST.HONOURS_WEST  },	//
		{ PAI.PEI,  TILE_LIST.HONOURS_NORTH },	//
		{ PAI.HAKU, TILE_LIST.HONOURS_WHITE },	//
		{ PAI.HATSU,TILE_LIST.HONOURS_GREEN },	//
		{ PAI.CHUN, TILE_LIST.HONOURS_RED   },	//
	//-*裏向き
		{ PAI.URA,TILE_LIST.BACK },			//
	};


	public void Init()
	{
		set(TILE_STATE.NO_USE,PAI.M1);
		m_tileImage.Init();
	}
	public void set(TILE_STATE state,PAI no)
	{
		//-******
		if( PAI_LIST.ContainsKey(no) )	{
			TILE_LIST tNo = TILE_LIST.BACK;
			m_tileState = state;
			PAI_LIST.TryGetValue(no,out tNo);
			m_no = (int)tNo;
			m_tileImage.SetState(m_tileState,m_tileType,m_no);
		}
		//-******
	}






	// Use this for initialization
	void Start () {}
	// Update is called once per frame
	void Update () {}


}
