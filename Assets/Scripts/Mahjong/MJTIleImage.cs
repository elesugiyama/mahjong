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
public class MJTIleImage : MonoBehaviour {
	// [Header("")]
	[SerializeField]
	private GameObject m_tileImageBase;
	[SerializeField]
	private Image m_tileImage;

	private const int TILES_NO_MAX = 9;	//-*タイプの総数	//-*todo：ここじゃないかも
	#if false//-*todo:作ってみたけど...
	
	//-*リスト内番号
	private TILE_LIST m_listNo;
	public TILE_LIST ListNo{
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
	#endif //-*todo:作ってみたけど...
	public void Init()
	{
		m_tileImageBase.SetActive(false);
	}

	public void SetState(TILE_STATE state, TILE_TYPE type, int no)
	{
		switch(state)
		{
		case TILE_STATE.NO_USE:
		//-*山に有る間は表示なし
			m_tileImageBase.SetActive(false);
			break;
		default:
			SetImage(state,no);
			break;
		}
	}
	public void SetImage(TILE_STATE state, int no)
	{
		if(m_tileImage == null)return;
		String tileName = String.Concat( (int)state, String.Format("{0:D2}", no) );
		String imageName = String.Concat(Dir.MJ_TILE_DIRECTORY, Dir.IMAGE_TILE_BASE_NAME,tileName);
		var spriteImage = Resources.Load<Sprite>(imageName);
		if(spriteImage == null){
			Debug.LogError("//-*TileImageSet:NullErr:"+imageName+" type:"+state+"("+(int)state+") no:"+no+"("+String.Format("{0:D2}", no)+")");
			return;
		}
		m_tileImageBase.SetActive(true);
		m_tileImage.sprite = spriteImage;
	}



	// Use this for initialization
	void Start () {}
	// Update is called once per frame
	void Update () {}


}
