using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class StageButton : ButtonCtl {

#if false
//-*サンプル
	[SerializeField]
	private spriteUINumS m_spriteUINumber;
#endif
	[SerializeField]
	private Text m_labelText;
	private int m_buttonNo;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void SetLabel(int a)
	{
#if false
//-*サンプル
		string fileName = "N10"+a;
		m_spriteUINumber.init(fileName);
#endif
		m_buttonNo = a;
		m_labelText.text = ""+(a+1);
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
