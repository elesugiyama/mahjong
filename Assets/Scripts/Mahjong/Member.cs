using System;
using System.Linq;
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

public class Member : MonoBehaviour {

	public byte[]	DrawTehai = new byte [14];	/* 手牌(０～１２のＭＡＸ１３牌)             */
	public void test(){
		
	}
	
#if false 
	public enum FLAGS{
		FURIT,					/* フリテンフラグ(フリテンでＯＮ)           */
		RICH,						/* リーチフラグ(リーチでＯＮ)               */
		WRICH,					/* ダブリーフラグ(ダブリーでＯＮ)           */
		IPPAT,					/* 一発フラグ(一発でＯＮ)                   */
		MENZEN,					/* 面前フラグ（鳴くとＯＮ）                 */
		NAGAS,					/* 流し満貫フラグ(崩れるとＯＮ)             */
		KOKUS,					/* 国士フラグ(聴牌るとＯＮ)                 */
		STHAI,
		MAX,
	}

	[SerializeField]
	private List<GameObject> m_myHandTileImage = new List<GameObject>();	//-*手牌(0～12のMAX13牌)表示用
	// private List<MJTIleImage> m_myHandTileImage = new List<MJTIleImage>();	//-*手牌(0～12のMAX13牌)表示用
	[SerializeField]
	private List<GameObject> m_myDiscardedTileImage = new List<GameObject>();	//-*捨て牌表示用
	// private List<MJTIleImage> m_myDiscardedTileImage = new List<MJTIleImage>();	//-*捨て牌表示用
	private MJTIle[] m_myHandTiles = new MJTIle[14];	//-*手牌(0～12のMAX13牌)内部処理用
	private List<MJTIle> m_myDiscardedTiles = new List<MJTIle>();	//-*捨て牌内部処理用
	
	//-*点棒
	private int m_points;
	public int Points{
		get{ return m_points;}
	}
	//-*聴牌（待ちの数）
	private int m_tenpai;
	public int Tenpai{
		get{return m_tenpai;}
	}
	//-*カンの数
	private int m_kancnt;
	public int KanCnt{
		get{return m_kancnt;}
	}
	//-*ステータス
	private int m_status;
	public int Status{
		get{return m_status;}
	}
	//-*オートプレイ
	//-*自分相手判別用に使用中：true(相手)
	private bool m_isApFlg;
	public bool IsApFlg{
		get{return m_isApFlg;}
	}

	private Dictionary<FLAGS,bool> m_flags = new Dictionary<FLAGS, bool>();

	//-*コンストラクタ
	public Member()
	{
		Init();
	}
	public void Init(){
		m_points = 0;
		m_tenpai = 0;
		m_kancnt = 0;
		m_status = 0;
		m_isApFlg = false;
		m_flags.Clear();
		m_flags = new Dictionary<FLAGS, bool>();
		for(FLAGS f = 0;f<FLAGS.MAX;f++){
			m_flags.Add(f,false);
		}
	}
	
	public void InitHand(){
		//-*手牌初期化
		if(m_myHandTileImage != null)
		{
			m_myHandTileImage.Clear();
			m_myHandTileImage = null;
		}
		m_myHandTileImage = new List<GameObject>();
		GameObject obj = (GameObject)Resources.Load("Prefabs/TileBaseHand");
		for(int no=0; no<MJDefine.HAND_NUM_MAX; no++){
			m_myHandTileImage.Add( (GameObject)Instantiate(obj,Vector3.zero,Quaternion.identity) );
			m_myHandTileImage[no].GetComponent<MJTIleImage>().Init();
		}

		//-*捨て牌初期化
		if(m_myDiscardedTileImage != null)
		{
			m_myDiscardedTileImage.Clear();
			m_myDiscardedTileImage = null;
		}
		m_myDiscardedTileImage = new List<GameObject>();
		
	}

	/// <summary>
	/// ツモ
	/// </summary>
	public void ToDraw(MJTIle tile)
	{
		m_myHandTiles[MJDefine.TO_DRAW_POS] = tile;
		//-*肺の状態
		if(m_isApFlg){
			m_myHandTiles[MJDefine.TO_DRAW_POS].SetState_BOTU(TILE_STATE.YOUR_HAND);	//-*裏向き
		}else{
			m_myHandTiles[MJDefine.TO_DRAW_POS].SetState_BOTU(TILE_STATE.MY_HAND);	//-表向き
		}
	}

	/// <summary>
	/// 理牌
	/// </summary>
	private void SortTiles()
	{
		m_myHandTiles.OrderBy(m_myHandTile => m_myHandTile.ListNo);
	}

	/// <summary>
	/// 手牌画像切り替え
	/// </summary>
	private void ImageSetHandTiles()
	{
		#if false //-*todo:作ってみたけど...
		for(int no=0; no<MJDefine.HAND_NUM_MAX; no++){
			var mjImage = m_myHandTileImage[no].GetComponent<MJTIleImage>();
			mjImage.SetState(m_myHandTiles[no].TileState,m_myHandTiles[no].ListNo);
			#if false //-*todo:作ってみたけど...
			mjImage.SetImage();
			#endif //-*todo:作ってみたけど...
		}		
		#endif //-*todo:作ってみたけど...
	}
#endif
}

