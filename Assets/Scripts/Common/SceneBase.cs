using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Const;
#if N_SWITCH
#region GAME_PAD
using nn.hid;
#endregion //-*GAME_PAD
#endif //-*N_SWITCH

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
#if N_SWITCH
		if(UpdatePadState()){
			StickAxisUpdate();
		}
#else
		StickAxisUpdate();
#endif //-*N_SWITCH
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

//-*******
#region GAME_PAD
#if N_SWITCH
		Npad.Initialize();
		Npad.SetSupportedIdType(new NpadId[]{ NpadId.Handheld, NpadId.No1 });
		Npad.SetSupportedStyleSet(NpadStyle.FullKey | NpadStyle.Handheld | NpadStyle.JoyDual);
#endif //-*N_SWITCH
#endregion //-*GAME_PAD
//-*******			
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
	// [Header("ゲームパッド関連")]
    public NpadId npadId = NpadId.Invalid;
    public NpadStyle npadStyle = NpadStyle.Invalid;
    public NpadState npadState = new NpadState();

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
//-*******
		if(isWait && m_keyWaitTIme > 0 && keyName < KEY_NAME.AXIS_MAX)
			return false;

		bool isKeyPush = false;
#if N_SWITCH
		switch(keyName)
		{
			case KEY_NAME.SELECT:
				if(npadState.GetButtonUp(NpadButton.A)) isKeyPush = true;
				break;
			case KEY_NAME.BACK:
				if(npadState.GetButtonUp(NpadButton.B)) isKeyPush = true;
				break;
		}
#else
		if(Input.GetKeyDown(GetPushKey[keyName])){
			isKeyPush = true;
		}
#endif //-*N_SWITCH
		if(isWait && isKeyPush){
			m_keyWaitTImeMax = KEY_WAIT;
		}
//-*******
		return isKeyPush;
	}
	/// <summary>
	/// 方向キー入力
	/// </summary>
	public bool IsKeyAxisButton(KEY_NAME keyName, bool isWait = true)
	{
		bool isPress = false;
		//-*傾きの大きさ
		float stickMagnitudeX = Mathf.Abs(m_inputStick.x);
		float stickMagnitudeY = Mathf.Abs(m_inputStick.y);
		//-*受付拒否(受付拒否時間中,方向キー以外,傾きが一定値無い)
		if(isWait && m_keyWaitTIme > 0)
			return false;
		if(keyName >= KEY_NAME.AXIS_MAX)
			return false;
		if( (keyName == KEY_NAME.LEFT || keyName == KEY_NAME.RIGHT) && ( stickMagnitudeX <= KEY_AXIS_REFUSE_INCLINE) )
			return false;
		if( (keyName == KEY_NAME.UP || keyName == KEY_NAME.DOWN) && ( stickMagnitudeY <= KEY_AXIS_REFUSE_INCLINE) )
			return false;
			
		//-*上下左右の向き
#if N_SWITCH
		int axisX = (m_inputStick.x > 0)? 1:-1;
		int axisY = (m_inputStick.y > 0)? -1:1;
#else
		int axisX = (m_inputStick.x > 0)? 1:-1;
		int axisY = (m_inputStick.y > 0)? 1:-1;
#endif //-*N_SWITCH
		//-*上下左右の優先度
		bool isAxisX = (stickMagnitudeX > stickMagnitudeY)? true:false;	//-*傾きが同じなら縦(Y軸)優先

		switch(keyName){
			case KEY_NAME.UP:
				if(axisY == -1 && !isAxisX){
					isPress = true;
					Debug.Log("//-*上！！！！！！");
				}
				break;
			case KEY_NAME.DOWN:
				if(axisY == 1 && !isAxisX){
					isPress = true;
					Debug.Log("//-*下！！！！！！");
				}
				break;
			case KEY_NAME.LEFT:
				if(axisX == -1 && isAxisX){
					isPress = true;
					Debug.Log("//-*左！！！！！！");
				}
				break;
			case KEY_NAME.RIGHT:
				if(axisX == 1 && isAxisX){
					isPress = true;
					Debug.Log("//-*右！！！！！！");
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
#if N_SWITCH
	#if false //-*元
			AnalogStickState lStick = npadState.analogStickL;
			var x = lStick.x;
			var y = lStick.y;
	#else	//-*試作
			AnalogStickState lStick = npadState.analogStickL;
			var x = lStick.x;
			var y = lStick.y;
			if(x == 0 && y == 0){
				if(npadState.GetButtonUp(NpadButton.Right)){
					x = 1;
				}else
				if(npadState.GetButtonUp(NpadButton.Left)){
					x = -1;
				}else
				if(npadState.GetButtonUp(NpadButton.Up)){
					y = 1;
				}else
				if(npadState.GetButtonUp(NpadButton.Down)){
					y = -1;
				}
			}
	#endif
#else
			var x = Input.GetAxis( "Horizontal" );
			var y = Input.GetAxis( "Vertical" );
#endif
			if( Mathf.Abs( x ) <= KEY_AXIS_REFUSE_INCLINE ) x = 0;
			if( Mathf.Abs( y ) <= KEY_AXIS_REFUSE_INCLINE ) y = 0;
			// if( Mathf.Abs( x ) <= 0.1f ) x = 0;
			// if( Mathf.Abs( y ) <= 0.1f ) y = 0;
			if( x != 0 || y != 0 ) {
				m_inputStick.x = x;
				m_inputStick.y = y;
				// DebLog("//-****m_InputVector:"+m_inputStick+"::"+(m_keyWaitTImeMax - m_keyWaitTIme)+" = "+m_keyWaitTImeMax+" - "+m_keyWaitTIme);
			}
		}
		//-*キー入力受付拒否時間
		m_keyWaitTIme += Time.deltaTime;
		var isTime = m_keyWaitTImeMax - m_keyWaitTIme;
		
		if(isTime < 0/* || m_inputStick == Vector2.zero*/){
			// DebLog("//-*"+m_inputStick+":"+isTime+" = "+m_keyWaitTImeMax+" - "+m_keyWaitTIme);
			m_keyWaitTIme=0;
			m_keyWaitTImeMax = 0;
			isTime = 0;
		}
	}

    public bool UpdatePadState()
    {
#if !N_SWITCH
		return false;
#else
        NpadStyle handheldStyle = Npad.GetStyleSet(NpadId.Handheld);
        NpadState handheldState = npadState;
        if (handheldStyle != NpadStyle.None)
        {
            Npad.GetState(ref handheldState, NpadId.Handheld, handheldStyle);
            if (handheldState.buttons != NpadButton.None)
            {
                npadId = NpadId.Handheld;
                npadStyle = handheldStyle;
                npadState = handheldState;
                return true;
            }
        }

        NpadStyle no1Style = Npad.GetStyleSet(NpadId.No1);
        NpadState no1State = npadState;
        if (no1Style != NpadStyle.None)
        {
            Npad.GetState(ref no1State, NpadId.No1, no1Style);
            if (no1State.buttons != NpadButton.None)
            {
                npadId = NpadId.No1;
                npadStyle = no1Style;
                npadState = no1State;
                return true;
            }
        }

        if ((npadId == NpadId.Handheld) && (handheldStyle != NpadStyle.None))
        {
            npadId = NpadId.Handheld;
            npadStyle = handheldStyle;
            npadState = handheldState;
        }
        else if ((npadId == NpadId.No1) && (no1Style != NpadStyle.None))
        {
            npadId = NpadId.No1;
            npadStyle = no1Style;
            npadState = no1State;
        }
        else
        {
            npadId = NpadId.Invalid;
            npadStyle = NpadStyle.Invalid;
            npadState.Clear();
            return false;
        }
        return true;
#endif //-*N_SWITCH
    }

#endregion //-*GAME_PAD

}
