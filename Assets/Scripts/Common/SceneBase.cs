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

	public DontDestroyData m_keeoData;

	private Const.Err.ERRTYPE m_errType;

	//-*シーン変更
	public bool m_isSceneChange = false;

	protected virtual void Start () {}
	protected virtual void Update () {}
	protected virtual void Awake() {
		Debug.Log("//-*Awake:SceneBase"+(m_keeoData!=null));
		
		var ObjList = FindObjectsOfType<DontDestroyData>();
		DontDestroyData gameData = null;
		if(ObjList.Length <= 0){
		//-*初回
	        var objBG = (GameObject)Resources.Load("Prefabs/DontDestroyObj");
			var tempObj = (GameObject)Instantiate(objBG, Vector3.zero, Quaternion.identity);
			gameData = tempObj.GetComponent<DontDestroyData>();
		}else{
		//-*既にある
			gameData = ObjList[0];
		}
		m_keeoData = gameData;		
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
		m_isSceneChange = false;
		SceneManager.LoadScene (nextScene);
		StopSound(SoundManagerCtrl.SOUNDTYPE.TYPE_ALL);
	}
#region SOUND
	public void PlaySound(string fileName, SoundManagerCtrl.SOUNDTYPE type)
	{
		switch(type)
		{
			case SoundManagerCtrl.SOUNDTYPE.TYPE_BGM:
				m_keeoData.SoundCtl.PlayBgm( fileName );
				break;
			case SoundManagerCtrl.SOUNDTYPE.TYPE_SE:
				m_keeoData.SoundCtl.PlaySe( fileName );
				break;
			case SoundManagerCtrl.SOUNDTYPE.TYPE_VOICE:
				// m_gameData.SoundCtl.PlayVoice( fileName );	//-*未実装
				break;
			default:
				break;
		}
	}

	public void StopSound(SoundManagerCtrl.SOUNDTYPE type)
	{
		switch(type)
		{
			case SoundManagerCtrl.SOUNDTYPE.TYPE_BGM:
				m_keeoData.SoundCtl.StopBgm();
				break;
			case SoundManagerCtrl.SOUNDTYPE.TYPE_SE:
				m_keeoData.SoundCtl.StopSe();
				break;
			case SoundManagerCtrl.SOUNDTYPE.TYPE_VOICE:
				// m_gameData.SoundCtl.PlayVoice( fileName );	//-*未実装
				break;
			case SoundManagerCtrl.SOUNDTYPE.TYPE_ALL:
				m_keeoData.SoundCtl.StopAll();
				break;
			default:
				break;
		}
	}
#endregion	//-*SOUND
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
	/// <summary>
	/// デバッグログ
	/// </summary>
	public void DevLog(string log)
	{
#if true
		Debug.Log(log);
#endif		
	}
	public void DevLogError(string log)
	{
#if true
		Debug.LogError(log);
#endif		
	}	
}
