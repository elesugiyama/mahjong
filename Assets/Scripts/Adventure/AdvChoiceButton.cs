using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

/// <summary>
/// 選択肢ボタン
/// </summary>
public class AdvChoiceButton : ButtonCtl {

#if false
//-*サンプル
	[SerializeField]
	private spriteUINumS m_spriteUINumber;
#endif
	[SerializeField]
	private Text m_labelText;
	private int m_buttonNo;
	
	//-*選択肢で飛ぶシナリオ番号
	private string m_advNextScoNo;

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}
	public void SetLabel(int btNo, string text, string scoNo)
	{
#if false
//-*サンプル
		string fileName = "N10"+a;
		m_spriteUINumber.init(fileName);
#endif
		m_buttonNo = btNo;
		m_labelText.text = ""+(text);
		m_advNextScoNo = scoNo;
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
		if (m_OnPointerClickCallbackString != null)
		{
			m_OnPointerClickCallbackString(m_advNextScoNo);
		}
    }
}
