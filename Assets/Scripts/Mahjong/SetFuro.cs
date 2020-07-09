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

	/// <summary>
	/// 副露牌
	/// </summary>
public class SetFuro : MonoBehaviour {
	//-*定数
	private const int TILE_NUM_PONCHI = 3;
	private const int TILE_NUM_KAN = 4;
	private enum FUROTYPE{
		MINKAN,
		ANKAN,
		PON,
		CHI,
		MAX,
	}
	/// <summary>
	/// 鳴き牌の状態(使用画像)
	/// Unity側で設定したm_FuroHaiと連動しているので順番注意
	/// </summary>
	private TILE_STATE[][] FURO_TAILE_STATE = new TILE_STATE[][]
	{
		new TILE_STATE[]{TILE_STATE.MY_DISCARDED,	TILE_STATE.RIICHI,			TILE_STATE.RIICHI,			TILE_STATE.MY_DISCARDED,},	//-*明槓
		new TILE_STATE[]{TILE_STATE.MY_DISCARDED,	TILE_STATE.MY_DISCARDED,	TILE_STATE.MY_DISCARDED,	TILE_STATE.MY_DISCARDED,},	//-*暗槓
		new TILE_STATE[]{TILE_STATE.MY_DISCARDED,	TILE_STATE.MY_DISCARDED,	TILE_STATE.RIICHI,			TILE_STATE.NO_USE,		},	//-*ポン
		new TILE_STATE[]{TILE_STATE.MY_DISCARDED,	TILE_STATE.MY_DISCARDED,	TILE_STATE.RIICHI,			TILE_STATE.NO_USE,		},	//-*チー
		new TILE_STATE[]{TILE_STATE.NO_USE,			TILE_STATE.NO_USE,			TILE_STATE.NO_USE,			TILE_STATE.NO_USE,		},	//-*未使用
	};

	/// <summary>
	/// PALYERWORK.cs内の変数byFrposのチー用
	/// チーの場合：順子の(0-2)番目の牌を一番左に持ってきて横にする
	/// MJGameDraw.j内の関数MJ_FrJicyaDraw()参照
	/// </summary>
	private int[][] CHI_TILE_REVISE = new int[][]
	{
		new int[]{0,1,2,0},
		new int[]{1,0,2,0},
		new int[]{2,0,1,0},
	};

	[Header("麻雀牌(鳴き)")]
	//-手牌
	[SerializeField]
	private GameObject[] m_FuroHai = new GameObject[4];

	private byte[]	m_drawFuroHai = new byte [4];	//-*副露牌
	private FUROTYPE m_type = 0;	//-*副露種
	// Use this for initialization
	void Start () {}
	// Update is called once per frame
	void Update () {}

	/// <summary>
	/// 鳴き牌の使用画像セット
	/// Type(int):鳴き種　Bbit(int):フーロ牌情報取得
	/// </summary>
	public void SetFuroImage(int Type, int sbit, int frPos){
		m_type = FUROTYPE.MAX;
		switch(Type){
		case MJDefine.D_MINKAN:
			m_type = FUROTYPE.MINKAN;
			break;
		case MJDefine.D_ANKAN:
			m_type = FUROTYPE.ANKAN;
			break;
		case MJDefine.D_PON:
			m_type = FUROTYPE.PON;
			break;
		case MJDefine.D_CHI:
			m_type = FUROTYPE.CHI;
			break;
		default:
			break;
		}

		for(int no=0;no<m_FuroHai.Length;no++){
			if(m_FuroHai[no] == null) continue;
			var mjTile = m_FuroHai[no].GetComponent<MJTIle>();
			if(mjTile == null) continue;
			if(m_type == FUROTYPE.CHI){
			//-*チー
				mjTile.set(FURO_TAILE_STATE[(int)m_type][no],(PAI)sbit+CHI_TILE_REVISE[frPos][no]);
			}else{
			//-*カンポン
				mjTile.set(FURO_TAILE_STATE[(int)m_type][no],(PAI)sbit);
			}
		}
	}
}

