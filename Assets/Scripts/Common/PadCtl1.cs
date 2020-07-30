using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

/// <summary>
/// ゲームパッド、キー入力用コントローラー
/// </summary>
public class PadCtl1 : MonoBehaviour
	//, 
	// IPointerDownHandler, 
	// IPointerUpHandler, 
	// IBeginDragHandler, 
	// IDragHandler, 
	// IEndDragHandler
{
#if false
	bool m_JoyStickKey;


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
    }

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}

	public virtual void SetPos(Vector3 pos)
	{
		this.transform.localPosition = pos;
	}

#region JOY_STICK


#endregion //-*JOY_STICK
#endif
	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}
}
