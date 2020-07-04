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
	[Header("暗転などの演出")]
	[SerializeField]
	public ScreenEffect m_screenEffect;

	public DontDestroyData m_keepData;

	private Const.Err.ERRTYPE m_errType;

	//-*シーン変更
	public bool m_isSceneChange = false;

	protected virtual void Start () {}
	protected virtual void Update () {}
	protected virtual void Awake() {
		Debug.Log("//-*Awake:SceneBase"+(m_keepData!=null));
		
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
		m_keepData = gameData;		
	}

	/// <summary>
	/// モードチェンジ
	/// </summary>
	public virtual Type ModeSet<Type>(Type setMode)
	where Type : IComparable
	{
		//-*モードチェンジ
		//-*todo:
		Debug.Log("//-*ModeChange:"+setMode);
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
				m_keepData.SoundCtl.PlayBgm( fileName );
				break;
			case SoundManagerCtrl.SOUNDTYPE.TYPE_SE:
				m_keepData.SoundCtl.PlaySe( fileName );
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
				m_keepData.SoundCtl.StopBgm();
				break;
			case SoundManagerCtrl.SOUNDTYPE.TYPE_SE:
				m_keepData.SoundCtl.StopSe();
				break;
			case SoundManagerCtrl.SOUNDTYPE.TYPE_VOICE:
				// m_gameData.SoundCtl.PlayVoice( fileName );	//-*未実装
				break;
			case SoundManagerCtrl.SOUNDTYPE.TYPE_ALL:
				m_keepData.SoundCtl.StopAll();
				break;
			default:
				break;
		}
	}
#endregion	//-*SOUND

#region COMMON_FUNCTION
//-*汎用機能
	public void _MEMSET( byte[] p0, int n0, int s0) {
		try {
			for( int i= 0; i< s0; i++)
				p0[i]= (byte)n0;
		} catch (Exception e) {
			DebLog(""+e);
		}
	}
	public void _MEMSET( int[] p0, int n0, int s0) {
		try {
			for( int i= 0; i< s0; i++)
				p0[i]= n0;
		} catch (Exception e) {
			DebLog(""+e);
		}
	}

	public void _MEMCPY<Type>( Type[] p0, Type[] p1, int s0)
	where Type : IComparable
	// public void _MEMCPY( char[] p0, char[] p1, int s0)
	{
		// System.arraycopy( p1, 0, p0, 0, s0 );
		System.Array.Copy( p1, 0, p0, 0, s0 );
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
	/// <summary>
	/// デバッグログ
	/// </summary>
	public void DebLog(string log)
	{
#if DEBUG
		Debug.Log(log);
#endif		
	}
	public void DebLogError(string log)
	{
#if DEBUG
		Debug.LogError(log);
#endif		
	}
#endregion	//-*COMMON_FUNCTION
}
