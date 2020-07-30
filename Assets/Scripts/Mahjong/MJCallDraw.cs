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
/// 鳴きテンパイノーテン宣言の描画関連
/// 元ソース「MJ_TalkDraw.j」から必要部分移植
/// todo:アニメーション追加するならココで処理する
/// </summary>
public class MJCallDraw : MonoBehaviour {
	

	[SerializeField]
	private GameObject[] m_callObjs = new GameObject[(int)CALLDRAW.MAX];	//-*CALLTYPEの順番と画像を連動
	
	/// <summary>
	/// 宣言描画初期化
  	/// <param name="assort">ボタン管理番号</param>
  	/// <param name="handNo">何番目</param>
  	/// <param name="selectF">ボタン機能フラグ</param>
	/// </summary>
	public void InitCallDraw()
	{
		for(int a=0;a<m_callObjs.Length;a++){
			m_callObjs[a].SetActive(false);
		}
	}
	public void setCall(int	msgnum)
	{
		if(msgnum >= m_callObjs.Length)return;

		// ポンやロンなどの共有会話の表示
		for(int a=0;a<m_callObjs.Length;a++){
			if(a == msgnum){
				m_callObjs[a].SetActive(true);
			}else{
				m_callObjs[a].SetActive(false);
			}
		}

	}



	// Use this for initialization
	void Start () {}
	// Update is called once per frame
	void Update () {}


}
