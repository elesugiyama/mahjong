using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class DebButtonBase : ButtonCtl {
//-*SUGI_DEB***************************
#if SUGI_DEB //-*todo:注デバッグ中

	private const int OFF = 0;
	private const int ON = 1;
	private static string[] ONOFFF = {"OFF","ON"};

	[SerializeField]
	private Text m_labelText;

	[SerializeField]
	private GameObject m_buttonColorChoice;
	[SerializeField]
	private GameObject m_buttonColorNotChoice;
	private int m_buttonNo;
	public int ButtonNo{
		get{return m_buttonNo;}
	}
	private int m_buttonType;
	private bool m_isEffective;	//-*効果中か
	private string m_labelTextBase;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void InitLabel(int no,string text, bool isOnOff = true)
	{
		m_buttonNo = no;
		m_labelTextBase = text;
		if(isOnOff){
			ChangeOnOff(false);
		}else{
			m_labelText.text = m_labelTextBase;
		}
	}
	public void ChangeOnOff(bool isEffect)
	{
		m_isEffective = isEffect;
		int onoff_label = (m_isEffective)?ON:OFF;
		m_labelText.text = m_labelTextBase+ONOFFF[onoff_label];
	}
	public void SwichOnOff()
	{
		bool onOff = !m_isEffective;
		ChangeOnOff(onOff);
	}

	public void ChangeButtonColor(bool isUse = false)
	{//-*積み込みボタン用
		if(m_buttonColorChoice == null || m_buttonColorNotChoice == null)return;
		m_buttonColorChoice.SetActive(isUse);
		m_buttonColorNotChoice.SetActive(!isUse);
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
		if (m_OnPointerClickCallbackInt != null){
			m_OnPointerClickCallbackInt(m_buttonNo);
		}
		else if (m_OnPointerClickCallback != null){
			m_OnPointerClickCallback();
		}
		else if(m_OnPointerClickCallbackIntBool != null){
			SwichOnOff();
			m_OnPointerClickCallbackIntBool(m_buttonNo,m_isEffective);
		}
		
    }
#endif //-*todo:注デバッグ中
//-****************************SUGI_DEB
}
