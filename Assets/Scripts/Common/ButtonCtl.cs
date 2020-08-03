using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

/// <summary>
/// ボタンコントローラー
/// </summary>
public class ButtonCtl : MonoBehaviour, IPointerClickHandler
{
	public Button target;
	private bool isPush = false;
	public bool ISPUSH {
		get{
			bool isTemp = isPush;
			isPush = false;
			return isTemp;
		}
	}
	//---------------------------------------------------------
	/// <summary>
	/// クリックコールバックのセット
	/// </summary>
	//---------------------------------------------------------
	// コールバック
	public Action m_OnPointerClickCallback = null;
	public void SetOnPointerClickCallback(Action a_OnPointerClickCallback)
	{
		m_OnPointerClickCallback = a_OnPointerClickCallback;
	}
	public Action<int> m_OnPointerClickCallbackInt = null;
	public void SetOnPointerClickCallback(Action<int> a_OnPointerClickCallback)
	{
		m_OnPointerClickCallbackInt = a_OnPointerClickCallback;
	}
	public Action<string> m_OnPointerClickCallbackString = null;
	public void SetOnPointerClickCallback(Action<string> a_OnPointerClickCallback)
	{
		m_OnPointerClickCallbackString = a_OnPointerClickCallback;
	}
	public Action<int,bool> m_OnPointerClickCallbackIntBool = null;
	public void SetOnPointerClickCallback(Action<int,bool> a_OnPointerClickCallback)
	{
		m_OnPointerClickCallbackIntBool = a_OnPointerClickCallback;
	}
	//---------------------------------------------------------
	/// <summary>
	/// クリック
	/// </summary>
	//---------------------------------------------------------
	public virtual void OnPointerClick(PointerEventData eventData)
    {
		if(target == null){
			Debug.Log("//-*Button:target is null");
			return;
		}
        target.OnPointerClick(eventData);
		isPush = true;
		// コールバック
		if (m_OnPointerClickCallback != null)
		{
			m_OnPointerClickCallback();
		}
    }

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}

	public virtual void SetPos(Vector3 pos)
	{
		this.transform.localPosition = pos;
	}
	public virtual void SetInteractable(bool isInteractable)
	{
		target.interactable = isInteractable;
	}
}
