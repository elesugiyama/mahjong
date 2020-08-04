using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
//-********
using GalleryDeffine;
using Const;
//-********


public class GalleryButton : ButtonCtl {

#if false
//-*サンプル
	[SerializeField]
	private spriteUINumS m_spriteUINumber;
#endif
	[SerializeField]
	private Image m_thumbnail;
	private int m_buttonNo;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void SetImage(int no, Sprite img)
	{
		m_buttonNo = no;
		if(img == null || m_thumbnail == null){
			Debug.LogError("//-*TileImageSet:"+no+"NullErr:"+img+":::m_thumbnail:"+m_thumbnail);
			return;
		}
		m_thumbnail.sprite = img;
	}

	//---------------------------------------------------------
	/// <summary>
	/// クリック
	/// </summary>
	//---------------------------------------------------------
	public override void OnPointerClick(PointerEventData eventData)
    {
		if(target == null){
			Debug.Log("//-*Button:target is null");
			return;
		}
        target.OnPointerClick(eventData);
		// コールバック
		if (m_OnPointerClickCallbackInt != null)
		{
			m_OnPointerClickCallbackInt(m_buttonNo);
		}
    }
}
