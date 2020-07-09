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
/// 王牌
/// </summary>
public class SetWanpai : MonoBehaviour {
	//-*******定数宣言********
	private const int MAX_DORA_NUM = 5;	//-*ドラ最大数(表示用)


	[Header("麻雀牌(ドラ)")]
	//-ドラ牌
	[SerializeField]
	private GameObject[] m_doraHai = new GameObject[MAX_DORA_NUM];


	// Use this for initialization
	void Start () {}
	// Update is called once per frame
	void Update () {}

	/// <summary>
	/// 王牌の初期化
	/// 全部裏牌にして位置も戻す
	/// </summary>
	public void Init(){
		for(int no=0;no<MAX_DORA_NUM;no++){
			GameObject hai = m_doraHai[no];
			
			//-*牌を裏返す(表示)
			if(hai == null) return;
			var mjTile = hai.GetComponent<MJTIle>();
			if(mjTile == null) return;
			mjTile.set(TILE_STATE.MY_DISCARDED,PAI.URA);
		}
	}
	/// <summary>
	/// ドラ牌の使用画像セット
	/// Bbit(int):ドラ牌情報取得：doraPos(int)表示場所
	/// </summary>
	public void Set(byte sbit, int doraPos){
		if(m_doraHai[doraPos] == null || doraPos >= MAX_DORA_NUM) return;
		var mjTile = m_doraHai[doraPos].GetComponent<MJTIle>();
		if(mjTile == null) return;
		mjTile.set(TILE_STATE.MY_DISCARDED,(PAI)sbit);
	}
}

