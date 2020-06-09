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
	/// 画像差し替え
	/// </summary>
	public bool ImageChange(String bgNo, bool isEventBg = false)
	{
		if(m_bg == null || bgNo == null) return false;
		String imageName = null;
		if(isEventBg){
			imageName = String.Concat(Dir.ADV_IMAGE_DIRECTORY, Dir.IMAGE_BGEV_BASE_NAME, bgNo);
		}else{
			imageName = String.Concat(Dir.ADV_IMAGE_DIRECTORY, Dir.IMAGE_BG_BASE_NAME, bgNo);
		}
		Debug.Log("//-*ImageChange(BG):"+imageName);
		var spriteImage = Resources.Load<Sprite>(imageName);
		m_bg.GetComponent<Image>().sprite = spriteImage;
		return true;
	}

}
