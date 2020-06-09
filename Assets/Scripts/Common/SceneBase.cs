using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Const;

/// <summary>
/// シーンベース
/// 継承して使用
/// </summary>
public class SceneBase : MonoBehaviour {
	[SerializeField]
	public ScreenEffect m_screenEffect;

	public GameObject m_dontDestroyObj;
	public DontDestroyData m_gameData;

	private Const.Err.ERRTYPE m_errType;

	protected virtual void Start () {}
	protected virtual void Update () {}
	protected virtual void Awake() {
		Debug.Log("//-*Awake:SceneBase"+(m_gameData!=null));
		//-*シーン遷移で消えないオブジェクト関連
		// int numMusicPlayers = FindObjectsOfType<DontDestroyData>().Length;
		// if (numMusicPlayers <= 0)
		// {
	    //     GameObject objBG = (GameObject)Resources.Load("Prefabs/DontDestroyObj");
		// 	m_dontDestroyObj = (GameObject)Instantiate(objBG, Vector3.zero, Quaternion.identity);
		// }
		// m_gameData = m_dontDestroyObj.GetComponent<DontDestroyData>();
		
		var ObjList = FindObjectsOfType<DontDestroyData>();
		DontDestroyData gameData = null;
		if(ObjList.Length <= 0){
		//-*初回
	        GameObject objBG = (GameObject)Resources.Load("Prefabs/DontDestroyObj");
			m_dontDestroyObj = (GameObject)Instantiate(objBG, Vector3.zero, Quaternion.identity);
			gameData = m_dontDestroyObj.GetComponent<DontDestroyData>();
		}else{
		//-*既にある
			gameData = ObjList[0];
		}
		m_gameData = gameData;		
	}

	/// <summary>
	/// モードチェンジ
	/// </summary>
	public virtual Type ModeSet<Type>(Type setMode)
	where Type : IComparable
	{
		//-*モードチェンジ
		//-*todo:
		return setMode;
	}
	public virtual Type ModeSet<Type>(Type setMode, Type nextMode)
	where Type : IComparable
	{
		//-*一致チェック(todo:エラーモードのチェックに使える？)
		if(setMode.Equals(nextMode)) return nextMode;
		Debug.Log("//-*ModeChange:"+setMode);
		return setMode;
	}

	/// <summary>
	/// シーンチェンジ
	/// </summary>
	public virtual void SceneChange(String nextScene)
	{
		SceneManager.LoadScene (nextScene);
	}
	
	/// <summary>
	/// エラーダイアログ：todo
	/// </summary>
	public void ErrDairog()
	{
		switch(m_errType)
		{
		default:
			break;
		}
	}

}
