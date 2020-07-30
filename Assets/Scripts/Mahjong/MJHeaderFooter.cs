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
/// ヘッダーフッター
/// </summary>
public class MJHeaderFooter : MonoBehaviour {
	private const int PLAYER = 0;
	private const int ENEMY = 1;
	private const int RATE = 100;
	//-*[東南西北]家//-*todo:画像にするなら画像名に流用
	private String[] HOUSE_NAME = new String[]{
		"東","南","西","北",
	};
	//-*ルール//-*todo:画像にするなら画像名に流用
	private String[] RULE_NAME = new String[]{
	//-*MahjongDeffine.cs内「enum MAH」と連動
		"ノーマル戦",		//-*LIM00, //  0:ノーマル戦
		"鳴き禁止",			//-*LIM01, //  1:鳴き禁止
		"点数ハンデ戦",		//-*LIM02, //  2:点数ハンデ戦[-10000点/15000vs35000]
		"二翻縛り",			//-*LIM03, //  3:二翻縛り
		"ロン・リーチ禁止",	//-*LIM04, //  4:ロン・リーチ禁止
		"ロン禁止",			//-*LIM05, //  5:ロン禁止
		"リーチ禁止",		//-*LIM06, //  6:リーチ禁止
		"点数ハンデ戦",		//-*LIM07, //  7:点数ハンデ戦[-5000点/20000vs30000]
		//********ウキウキ:ルール追加********
		"点数ハンデ戦",		//-*LIM08, //* 8:点数ハンデ戦[-15000点/10000vs40000]
		//***********************************
		"",					//-*LIM_MAX,
	};
	[Header("Header")]
	[SerializeField]
	private Text m_pointEne;	//-*相手の得点
	[SerializeField]
	private Text m_round;	//-*場(東南西北)
	[SerializeField]
	private Text m_riboCnt;	//-*リーチ棒数
	[SerializeField]
	private Text m_baCnt;	//-*場数

	[Header("Footer")]
	[SerializeField]
	private Text m_pointMy;	//-*自分の得点
	[SerializeField]
	private Text m_house;	//-*自分の家(東南西北)
	[SerializeField]
	private Text m_rule;	//-*ルール

	


	public void InitHF()
	{
		//-*ヘッダー
		m_pointEne.text = "";
		m_round.text = "";
		m_riboCnt.text = "";
		m_baCnt.text = "";
		//-*フッター
		m_pointMy.text = "";
		m_house.text = "";
		m_rule.text = "";
	}
	public void UpdateHF(int House,int Round,int RoundCnt,int Renchan,int[] Point,int RuleNo,int RiboCnt)
	{
		int myP = Point[PLAYER]*RATE;
		int yourP = Point[ENEMY]*RATE;
		int rule = (RuleNo<0)?0:RuleNo;
		int roundCnt = RoundCnt+1;	//-*内部値は0から始まってる
		//-*ヘッダー
		m_pointEne.text = yourP.ToString();
		m_round.text = HOUSE_NAME[Round]+roundCnt.ToString();
		m_riboCnt.text = "x"+RiboCnt.ToString();
		m_baCnt.text = "x"+Renchan.ToString();
		//-*フッター
		m_pointMy.text = myP.ToString();
		m_house.text = HOUSE_NAME[House];
		m_rule.text = RULE_NAME[rule];
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
