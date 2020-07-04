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

	public void Init()
	{
		set(TILE_STATE.THE_WALL,PAI.M1);
		m_tileImage.Init();
	}
	public void set(TILE_STATE state,PAI no)
	{
		m_tileState = state;
		m_listNo = no;
		m_tileType = (TILE_TYPE)( (int)m_listNo / MJDefine.TYPE_TILES_NO_MAX );
		m_no = (int)m_listNo % MJDefine.TYPE_TILES_NO_MAX;
		m_tileImage.SetState(m_tileState,m_tileType,m_no);
	}




	public void Init_BOTU(TILE_LIST listNo)
	{
		Debug.LogError("(-_-)");
		// m_listNo = listNo;
		m_tileType = (TILE_TYPE)( (int)listNo / MJDefine.TYPE_TILES_NO_MAX );
		m_no = (int)listNo % MJDefine.TYPE_TILES_NO_MAX;
		m_tileState = TILE_STATE.THE_WALL;
		m_tileImage.Init();
	}
	public void SetState_BOTU(TILE_STATE state)
	{
		m_tileState = state;
		// m_tileImage.SetState(m_tileState,m_myHandTiles[no].ListNo);
	}

	// Use this for initialization
	void Start () {}
	// Update is called once per frame
	void Update () {}


}
