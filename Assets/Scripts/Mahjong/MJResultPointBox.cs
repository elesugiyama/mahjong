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
/// リザルト精算画面
/// </summary>
public class MJResultPointBox : MonoBehaviour {

	private const int PLAYER = 0;
	private const int ENEMY = 1;
	private const int RATE = 100;
	//-*[東南西北]家画像
	private String[] HOUSE_IMAGE_NAME = new String[]{
		"B000",
		"B002",
	};

	[Header("家")]
	[SerializeField]
	private Image[] m_imgHouse = new Image[MJDefine.MEMBER_NUM_MAX];	//-*[東南西北]家画像

	[Header("現在点")]
	[SerializeField]
	private Text[] m_txtNowPoint = new Text[MJDefine.MEMBER_NUM_MAX];	//-*今の点

	[Header("変動点")]
	[SerializeField]
	private Text[] m_txtMovePoint = new Text[MJDefine.MEMBER_NUM_MAX];	//-*加減する点


	
	private int[] m_house = new int[MJDefine.MEMBER_NUM_MAX];	//-*[東南西北]家内部値
	private int[] m_pointNow = new int[MJDefine.MEMBER_NUM_MAX];	//-*今の点の内部値
	private int[] m_pointMove = new int[MJDefine.MEMBER_NUM_MAX];	//-*加減する点の内部値



	public void InitResultPoint(int[] NowPoint)
	{
		// m_house[PLAYER] = 0;
		// m_house[ENEMY] = 0;
		int myNowPoint   = NowPoint[PLAYER]*RATE;
		int yourNowPoint = NowPoint[ENEMY]*RATE;
		m_txtNowPoint[PLAYER].text = myNowPoint.ToString();
		m_txtNowPoint[ENEMY].text =  yourNowPoint.ToString();
		m_txtMovePoint[PLAYER].text = "";
		m_txtMovePoint[ENEMY].text = "";

	}
	public void SetResultPointBox(int myHouse,int[] NowPoint,int[] MovePoint)
	{
		String mark = "";
		int num = 0;
		String houseName = null;
		String imageName = null;
		Sprite spriteImage = null;

		InitResultPoint( NowPoint );	//-*初期化

		//-*データ格納
		m_house[PLAYER] = myHouse;
		m_pointNow[PLAYER] = NowPoint[PLAYER]*RATE;
		m_pointMove[PLAYER] = MovePoint[PLAYER]*RATE;
		m_house[ENEMY] = ((myHouse+ 1) & 0x01);
		m_pointNow[ENEMY] = NowPoint[ENEMY]*RATE;
		m_pointMove[ENEMY] = MovePoint[ENEMY]*RATE;
		//-*表示(自分)
		m_txtNowPoint[PLAYER].text = m_pointNow[PLAYER].ToString();
		mark = (m_pointMove[PLAYER] < 0)?"－":"＋";
		num = Math.Abs(m_pointMove[PLAYER]);	//-*符号は別付けなので消す
		m_txtMovePoint[PLAYER].text = mark+num.ToString();
		if(m_imgHouse[PLAYER] == null)return;
		houseName = HOUSE_IMAGE_NAME[m_house[PLAYER]];
		imageName = String.Concat(Dir.MJ_COMMON_DIRECTORY, houseName);
		spriteImage = Resources.Load<Sprite>(imageName);
		if(spriteImage == null){
			Debug.LogError("//-*TileImageSet:NullErr:"+imageName+" m_house["+PLAYER+"]:"+m_house[PLAYER]+"...");
			return;
		}
		m_imgHouse[PLAYER].sprite = spriteImage;

		//-*表示(相手)
		m_txtNowPoint[ENEMY].text = m_pointNow[ENEMY].ToString();
		mark = (m_pointMove[ENEMY] < 0)?"－":"＋";
		num = Math.Abs(m_pointMove[ENEMY]);	//-*符号は別付けなので消す
		m_txtMovePoint[ENEMY].text = mark+num.ToString();

		if(m_imgHouse[ENEMY] == null)return;
		houseName = HOUSE_IMAGE_NAME[m_house[ENEMY]];
		imageName = String.Concat(Dir.MJ_COMMON_DIRECTORY, houseName);
		spriteImage = Resources.Load<Sprite>(imageName);
		if(spriteImage == null){
			Debug.LogError("//-*TileImageSet:NullErr:"+imageName+" m_house["+ENEMY+"]:"+m_house[ENEMY]+"...");
			return;
		}
		m_imgHouse[ENEMY].sprite = spriteImage;

	}

	public void UpdateResultPointBox(int[] NowPoint)
	{
		//-*データ格納
		m_pointNow[PLAYER] = NowPoint[PLAYER]*RATE;
		m_pointNow[ENEMY] = NowPoint[ENEMY]*RATE;
		//-*表示(自分)
		m_txtNowPoint[PLAYER].text = m_pointNow[PLAYER].ToString();
		//-*表示(相手)
		m_txtNowPoint[ENEMY].text = m_pointNow[ENEMY].ToString();
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
