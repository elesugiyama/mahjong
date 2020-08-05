using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class OptionButton : ButtonCtl {
	[SerializeField]
	private GameObject m_objActive;
	[SerializeField]
	private GameObject m_objNotActive;

	private int m_buttonNo;
	private bool m_isActive;

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}
	public void SetLabel(int a)
	{
		m_buttonNo = a;
		// this.transform.localPosition = aaaa;
		// SetPos(pos);
	}
	public void SetActive(bool isActive)
	{	

		if(m_isActive == isActive)return;
		m_isActive = isActive;
		m_objActive.SetActive(isActive);
		m_objNotActive.SetActive(!isActive);

		// this.transform.localPosition = aaaa;
		// SetPos(pos);
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
