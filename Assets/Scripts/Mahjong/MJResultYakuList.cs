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
/// 麻雀牌
/// </summary>
public class MJResultYakuList : MonoBehaviour {
	
	[Header("役画像")]
	[SerializeField]
	private Image m_yakuImage;

	[Header("飜")]
	[SerializeField]
	private Text m_yakuFactor;

	public void SetImage(int no)
	{
		if(no >= (int)YK.MAX||  m_yakuImage == null)return;
		String yakuName = String.Concat( String.Format("{0:D2}", no) );
		String imageName = String.Concat(Dir.MJ_YAKU_DIRECTORY, Dir.IMAGE_YAKU_BASE_NAME,yakuName);
		var spriteImage = Resources.Load<Sprite>(imageName);
		if(spriteImage == null){
			Debug.LogError("//-*TileImageSet:NullErr:"+imageName+" no:"+no+"("+String.Format("{0:D2}", no)+")");
			return;
		}
		m_yakuImage.sprite = spriteImage;
	}

	public void SetFactor(int factor)
	{
		m_yakuFactor.text = factor.ToString()+"飜";
	}


	// Use this for initialization
	void Start () {}
	// Update is called once per frame
	void Update () {}

}
