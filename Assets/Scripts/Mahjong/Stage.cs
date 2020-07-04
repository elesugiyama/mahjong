using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Const;
//-********
using MahjongDeffine;
using MJDefsHeader;
using GameDefsHeader;
using MJDialogHeader;
//-********


public class Stage : MonoBehaviour {

	[Header("麻雀牌(プレイヤー)")]
	//-手牌
	[SerializeField]
	private GameObject m_myHandTilesBase;
	[SerializeField]
	private List<GameObject> m_myHandTiles = new List<GameObject>();
	//-*ツモ牌
	[SerializeField]
	private GameObject m_myDrawTileBase;
	[SerializeField]
	private GameObject m_myDrawTile;

	[Header("麻雀牌(相手)")]
	//-手牌
	[SerializeField]
	private GameObject m_yourHandTilesBase;
	[SerializeField]
	private List<GameObject> m_yourHandTiles = new List<GameObject>();
	//-*ツモ牌
	[SerializeField]
	private GameObject m_yourDrawTileBase;
	[SerializeField]
	private GameObject m_yourDrawTile;


	//-*内部処理用
	private List<MJTIle> m_mjTiles = new List<MJTIle>();

	// Use this for initialization
	void Start () {
		Init();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	private void Init(){
		//-*牌リスト作成
		if(m_mjTiles != null)
		{
			m_mjTiles.Clear();
			m_mjTiles = null;
		}
		m_mjTiles = new List<MJTIle>();
		for(TILE_LIST no=TILE_LIST.CHARACTERS_1; no<TILE_LIST.TILES_MAX; no++){
			for(int num=0; num<MJDefine.ONE_TILES_NUM_MAX; num++){
			//-*4個作成
				var temp = new MJTIle();
				temp.Init_BOTU(no);
				m_mjTiles.Add(temp);
			}
		}
	}

}
