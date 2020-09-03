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
	
	/// <summary>
	/// シーン名
	/// </summary>
	public enum SCENE_NAME{
		TITLE,
		ADVENTURE,
		INGAME,
		CHALLENGE,
		GALLERY,
		OPTION,
		DEB_TITLE,
		DEB_SCOSEL,
		MAX,
	}
	/// <summary>
	/// シーン変更用
	/// </summary>
	public static Dictionary<SCENE_NAME,string> SceneNameDic = new Dictionary<SCENE_NAME,string>()
	{
		{ SCENE_NAME.TITLE,		"Title" },
		{ SCENE_NAME.ADVENTURE,	"Adventure" },
		{ SCENE_NAME.INGAME,	"InGame" },
		{ SCENE_NAME.CHALLENGE,	"SelectStage" },
		{ SCENE_NAME.GALLERY,	"Gallery" },
		{ SCENE_NAME.OPTION,	"Option" },
		{ SCENE_NAME.DEB_TITLE,	"DEB_Title" },
		{ SCENE_NAME.DEB_SCOSEL,"DEB_SelectStage" },
		{ SCENE_NAME.MAX,		"" },
	};

	[Header("暗転などの演出")]
	[SerializeField]
	public ScreenEffect m_screenEffect;
	[Header("オプション展開中用")]
	[SerializeField]
	public GraphicRaycaster m_raycaster;
	public DontDestroyData m_keepData;

	private Const.Err.ERRTYPE m_errType;

	//-*シーン変更
	public bool m_isSceneChange = false;
	public SCENE_NAME m_nextScenes = SCENE_NAME.MAX;
	public string m_nextSceneName = SceneNameDic[SCENE_NAME.MAX];
	//-*オプション画面
	public bool m_isOpenOption = false;	//-*不要になるかな？

	protected virtual void Start () {}
	protected virtual void Update () {
		if(m_raycaster != null){
			if(m_keepData.IsOptionOpen){
				m_raycaster.enabled = false;
			}else{
				m_raycaster.enabled = true;
			}
		}
#region KEY_TEST
		StickAxisUpdate();
#endregion //-*KEY_TEST
	}
	protected virtual void Awake() {
		Debug.Log("//-*Awake:SceneBase"+(m_keepData!=null));
		m_nextSceneName = SceneNameDic[SCENE_NAME.MAX];

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
		m_nextSceneName = SceneNameDic[SCENE_NAME.MAX];	//-*シーン名は初期化しとく
		StopSound(SoundManagerCtrl.SOUNDTYPE.TYPE_ALL);
	}
	/// <summary>
	/// シーンチェンジ
	/// </summary>
	public virtual void SceneChange()
	{
		m_isSceneChange = false;
		Debug.Log("//-*m_nextSceneName:"+m_nextSceneName);

		if( !String.IsNullOrEmpty(m_nextSceneName) ){
			SceneManager.LoadScene (m_nextSceneName);
		}
		m_nextSceneName = SceneNameDic[SCENE_NAME.MAX];	//-*シーン名は初期化しとく
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

#region OPTION
	public void OpenOption(){
		if(m_keepData == null)return;
		m_keepData.IsOptionOpen = true;
		// if(m_mainSceneCanvas!= null)m_mainSceneCanvas.SetActive(false);
		SceneManager.LoadScene(SceneNameDic[SCENE_NAME.OPTION],LoadSceneMode.Additive);
	}
	public void CloseOption(){
		if(m_keepData == null)return;
		// if(m_mainSceneCanvas!= null)m_mainSceneCanvas.SetActive(true);
        StartCoroutine(CoUnload());
		m_keepData.IsOptionOpen = false;
	}
 
    IEnumerator CoUnload()
    {
        //SceneAをアンロード
        var op = SceneManager.UnloadSceneAsync("Option");
        yield return op;
 
       //アンロード後の処理を書く
 
        //必要に応じて不使用アセットをアンロードしてメモリを解放する
        //けっこう重い処理なので、別に管理するのも手
        yield return Resources.UnloadUnusedAssets();
     }

#endregion //-*OPTION

#region GAME_PAD
	public const float KEY_WAIT = 0.3f;	//-*キー入力後の受け付けない時間Delay
	public const float KEY_AXIS_REFUSE_INCLINE = 0.5f;	//-*傾きの受付拒否幅
	public Vector2 m_inputStick = Vector2.zero;
	public bool m_isInputStick
	{
		set{}
		get{return false;}

	}
	public float m_keyWaitTImeMax = 0.0f;	//-*キー入力後の受け付けない時間格納用
	public float m_keyWaitTIme = 0.0f;	//-*キー入力後の受け付けない時間更新用
	public enum KEY_NAME {
		UP = 0,
		DOWN,
		LEFT,
		RIGHT,
		AXIS_MAX,	//-*方向キーここまで
		SELECT,		//-*決定	//-* = AXIS_MAX,(代入式保留)
		BACK,		//-*戻る
		KEY_NAME_MAX,
	};
	public static Dictionary<KEY_NAME,string> GetPushKey = new Dictionary<KEY_NAME,string>()
	{
		{ KEY_NAME.BACK,	"joystick button 0" },	//-*戻るボタン
		{ KEY_NAME.SELECT,	"joystick button 1" },	//-*決定ボタン
	};

	/// <summary>
	/// ボタンが押されたか
	/// </summary>
	public bool IsKeyBtnPress(KEY_NAME keyName, bool isWait = true)
	{
		if(isWait && m_keyWaitTIme > 0 && keyName < KEY_NAME.AXIS_MAX)
			return false;
		if(Input.GetKeyDown(GetPushKey[keyName])){
			return true;
		}
		return false;
	}
	/// <summary>
	/// 方向キー入力
	/// </summary>
	public bool IsKeyAxisButton(KEY_NAME keyName, bool isWait = true)
	{
		bool isPress = false;
		//-*受付拒否(受付拒否時間中,方向キー以外,傾きが一定値無い)
		if(isWait && m_keyWaitTIme > 0)
			return false;
		if(keyName >= KEY_NAME.AXIS_MAX)
			return false;
		if( (keyName == KEY_NAME.UP || keyName == KEY_NAME.DOWN) && ( Mathf.Abs(m_inputStick.y) <= KEY_AXIS_REFUSE_INCLINE) )
			return false;
		if( (keyName == KEY_NAME.LEFT || keyName == KEY_NAME.RIGHT) && ( Mathf.Abs(m_inputStick.x) <= KEY_AXIS_REFUSE_INCLINE) )
			return false;
			
		
		int axisX = (m_inputStick.x > 0)? 1:-1;
		int axisY = (m_inputStick.y > 0)? 1:-1;
		switch(keyName){
			case KEY_NAME.UP:
				if(axisY == -1){
					isPress = true;
				}
				break;
			case KEY_NAME.DOWN:
				if(axisY == 1){
					isPress = true;
				}
				break;
			case KEY_NAME.LEFT:
				if(axisX == -1){
					isPress = true;
				}
				break;
			case KEY_NAME.RIGHT:
				if(axisX == 1){
					isPress = true;
				}
				break;
			default:
			break;
		}
		if(isWait){
			m_keyWaitTImeMax = KEY_WAIT;
		}
		return isPress;
	}
	public void StickAxisUpdate(){

		m_inputStick = Vector2.zero;

		if( m_inputStick == Vector2.zero ) {
			//外部コントローラー ジョイスティック
			var x = Input.GetAxis( "Horizontal" );
			var y = Input.GetAxis( "Vertical" );
			if( Mathf.Abs( x ) <= 0.1f ) x = 0;
			if( Mathf.Abs( y ) <= 0.1f ) y = 0;
			if( x != 0 || y != 0 ) {
				m_inputStick.x = x;
				m_inputStick.y = y;
				DebLog("//-****m_InputVector:"+m_inputStick);
			}
		}
		//-*キー入力受付拒否時間
		m_keyWaitTIme += Time.deltaTime;
		var isTime = m_keyWaitTImeMax - m_keyWaitTIme;
		
		if(isTime < 0){
			m_keyWaitTIme=0;
			m_keyWaitTImeMax = 0;
		}
	}


#endregion //-*GAME_PAD

}
