using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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

/// <summary>
/// 鳴き系メニュー選択肢ボタン
/// </summary>
public class CallMenuButton : ButtonCtl {

#region CONSTANT
	/// <summary>
	/// 画像名:参照(元ソース)MJ_GameMain.j[gMenuStr_res]
	/// </summary>
	private string[] CALL_TYPE = new string[]{
		"",						//-*00:未使用(-1)
		"lovema_button_naki01",	//-*01:D_WIN_10:ツモ
		"lovema_button_naki04",	//-*02:D_WIN_13:ロン
		"lovema_button_naki03",	//-*03:D_WIN_12:リーチ
		"lovema_button_naki05",	//-*04:D_WIN_14:チー
		"lovema_button_naki06",	//-*05:D_WIN_15:ポン
		"lovema_button_naki07",	//-*06:D_WIN_16:カン
		"lovema_button_naki02",	//-*07:D_WIN_11:たおす(九種九牌)
		"lovema_button_naki07",	//-*08:D_WIN_16:カン("ﾁｬﾝｶﾝ")
		"lovema_button_naki09",	//-*09:D_WIN_17:パス
		"lovema_button_naki08",	//-*10:D_WIN_18:捨牌(リーチ、ロン、ツモしない用)
	};

		private const string PUSH_IMAGE = "_push";
#endregion //-*CONSTANT

#region VARIABLE
#if false
//-*サンプル
	[SerializeField]
	private spriteUINumS m_spriteUINumber;
#endif
	[SerializeField]
	private Image m_callImage;

	private Sprite m_PushImage;
	//-*ボタン管理番号
	private int m_buttonNo;
	//-*画像名
	private string m_imageName;
#endregion //-*VARIABLE

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}

	/// <summary>
	/// 鳴き系メニュー画像
	/// </summary>
  	/// <param name="btNo">ボタン管理番号</param>
  	/// <param name="type">メニュー</param>
	public void SetLabel(int btNo, byte type)
	{
#if false
//-*サンプル
		string fileName = "N10"+a;
		m_spriteUINumber.init(fileName);
#endif
		m_buttonNo = btNo;
		//-******
		if(type < CALL_TYPE.Length){
			m_imageName = String.Concat(Dir.MJ_CALL_DIRECTORY, CALL_TYPE[type]);
			var spriteImage = Resources.Load<Sprite>(m_imageName);
			if(spriteImage == null){
				Debug.LogError("//-*CallMenuBtnImageSet:NullErr:["+btNo+"]"+m_imageName+" type:"+type);
				return;
			}
			m_callImage.sprite = spriteImage;			

		}else{
			Debug.LogError("//-*CallMenuType:NotFoundErr:["+btNo+"] type:"+type);
		}
		//-******
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
