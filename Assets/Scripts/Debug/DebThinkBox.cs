using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Const;
//-********
using MahjongDeffine;
using MJDefsHeader;
using GameDefsHeader;
using MJDialogHeader;
//-********

public class DebThinkBox : MonoBehaviour {
//-*仮データ用
	private const int STAGEMAXNUM_TEST = 21;
	private static int THINK_PARA_NO_LIMIT = 99;

	[SerializeField]
	private Slider m_target;

	[SerializeField]
	private Text m_topicStr;
	[SerializeField]
	private Text m_topicVal;


	private int m_boxNo = 0;
	public int BOX_NO {
		get{return m_boxNo;}
	}
	private int m_valNo = 0;
	public int VAL_NO {
		get{return m_valNo;}
	}
	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}

	public void SetParaName(int no, string a)
	{
		m_boxNo = no;
		m_topicStr.text = a;
	}
	public void SetValue(int a)
	{
		m_valNo = a;
		m_topicVal.text = m_valNo.ToString();
	}
	public void SetSlideValue(int min, int max)
	{

		if(m_target == null)return;
		//-*初期値
		m_target.value = 0;
		m_target.minValue = -7;	//-*制限ない場合用に仮
		m_target.maxValue = 7;	//-*

		if(min != THINK_PARA_NO_LIMIT){
			m_target.minValue = min;
		}
		if(max != THINK_PARA_NO_LIMIT){
			m_target.maxValue = max;
		}
	}

	// //---------------------------------------------------------
	// /// <summary>
	// /// スライダー
	// /// </summary>
	// //---------------------------------------------------------
    public void SlideUpdate() {
        Debug.Log("Slide");
		SetValue((int)m_target.value);
    }


}
