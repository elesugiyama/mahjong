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
/// リザルト画面用ドラ表示
/// </summary>
public class SetResultDora : MonoBehaviour {
	//-*定数


	[Header("麻雀牌(ドラ)")]
	//-手牌
	[SerializeField]
	private GameObject[] m_DoraHai = new GameObject[5];

	// Use this for initialization
	void Start () {}
	// Update is called once per frame
	void Update () {}

	/// <summary>
	/// ドラ牌の使用画像セット
	/// </summary>
	public void SetDoraImage(byte[] list, byte cnt){
		for(int no=0;no<m_DoraHai.Length;no++){
			if(m_DoraHai[no] == null) continue;
			if(list[no]>=(byte)PAI.URA) continue;
			var mjTile = m_DoraHai[no].GetComponent<MJTIle>();
			if(mjTile == null) continue;
			//-*ドラ
			if(no < cnt){
				mjTile.set(TILE_STATE.HAND,(PAI)list[no]);
			}else{
				mjTile.set(TILE_STATE.NO_USE,(PAI)list[no]);
			}
		}
	}
}

