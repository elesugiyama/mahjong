using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Const;
using AdventureDefine;

public class AdventureChara : MonoBehaviour {

	void Start () {}
	void Update () {}

	[SerializeField]
	private Image m_chara;
	[SerializeField]
	private GameObject m_faceObj;
	[SerializeField]
	private Image m_face;

	private Sprite[] m_faces; // 分割した表情画像を格納する配列

	/// <summary>
	/// キャラ画像設定
	/// </summary>
	public bool ImageCharaSet(String charaNo)
	{
		if(m_chara == null || m_face == null || charaNo == null) return false;
		//-*画像名連結
		String imageName = String.Concat(Dir.ADV_IMAGE_DIRECTORY, Dir.IMAGE_CHARA_BASE_NAME,charaNo);
		Debug.Log("//-*ImageCharaSet:"+imageName);
		var spriteImage = Resources.Load<Sprite>(imageName);
		if(spriteImage == null) return false;
		m_chara.GetComponent<Image>().sprite = spriteImage;
		return true;
	}

	/// <summary>
	/// 顔画像設定
	/// 表情1枚絵を分割したものを纏めて格納するので画像側の設定に注意
	/// </summary>
	public bool ImageFaceSet(String faceNo)
	{
		if(m_chara == null || m_face == null || faceNo == null) return false;
		//-*画像名連結
		String imageName = String.Concat(Dir.ADV_IMAGE_DIRECTORY, Dir.IMAGE_CHARA_BASE_NAME, faceNo,Dir.IMAGE_CHARA_FACE_BASE_NAME);
		Debug.Log("//-*ImageFaceSet:"+imageName);
		m_faces = Resources.LoadAll<Sprite>(imageName);
		if(m_faces.Length <= 0){
		//-*表情差分無し
			m_faceObj.SetActive(false);
			return false;
		}
		m_faceObj.SetActive(true);
		m_face.GetComponent<Image>().sprite = m_faces[0];
		return true;
	}

	/// <summary>
	/// 表情変更
	/// </summary>
	public bool ImageFaceChangeExpression(int faceNo)
	{
		if(faceNo >= m_faces.Length) return false;
		Debug.Log("//-*ImageFaceChangeExpression:"+faceNo);
		m_face.GetComponent<Image>().sprite = m_faces[faceNo];
		return true;
	}
	
	
}
