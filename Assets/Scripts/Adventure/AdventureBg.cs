using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Const;
using AdventureDefine;

public class AdventureBg : MonoBehaviour {
	void Start () {}
	void Update () {}


	[SerializeField]
	private Image m_bg;

	/// <summary>
	/// 画像差し替え:背景(萌日記)
	/// </summary>
	public bool SetImageBg(String bgName)
	{
		if(m_bg == null || bgName == null) return false;
		String imageName = null;
		imageName = String.Concat(Dir.ADV_IMAGE_DIRECTORY, Dir.IMAGE_BG_BASE_NAME, bgName);
		Debug.Log("//-*BGImage(BG):"+imageName);
		return ChangeImage(imageName);
	}

	/// <summary>
	/// 画像差し替え:イベント背景(萌日記)
	/// </summary>
	public bool SetImageEvBg(int bgNo)
	{
		if(m_bg == null || bgNo < 0 ) return false;
		String imageName = null;
		imageName = String.Concat(Dir.ADV_IMAGE_DIRECTORY, AdvDefine.CGList[bgNo]);
		Debug.Log("//-*BGImage(Ev):"+imageName);
		return ChangeImage(imageName);
	}

	private bool ChangeImage(string imageName)
	{
		var spriteImage = Resources.Load<Sprite>(imageName);
		if(spriteImage == null) return false;
		m_bg.GetComponent<Image>().sprite = spriteImage;
		return true;
	}

}
