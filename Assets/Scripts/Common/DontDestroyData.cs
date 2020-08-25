using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyData : MonoBehaviour {

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {
		// if (Input.GetKey (KeyCode.LeftArrow)) {
		// 	Debug.Log("//-*********");
		// }
	#region OPTION
	#if false //-*todo:作り方模索中
		UpdateOption();
	#endif
	#endregion //-*OPTION
	}
	void Awake() {
		// シーンまたぎに登録
		int numMusicPlayers = FindObjectsOfType<DontDestroyData>().Length;
		if (numMusicPlayers > 1)
		{
			Destroy(this);
		}
		else
		{
	        DontDestroyOnLoad(this);
		} 

		m_isOptionOpne = false;
	}

#region MOEMAHJONG
/// 萌え日記麻雀関連

	/// <summary>
	/// シナリオ：現在のシナリオ番号
	/// </summary>
	private int m_advScoNo;
	public int AdvScoNo {
		get { return m_advScoNo; }
		set { m_advScoNo = value; }
	}

	/// <summary>
	/// シナリオ：次シナリオ番号
	/// </summary>
	private int m_advNextScoNo;
	public int AdvNextScoNo {
		get { return m_advNextScoNo; }
		set { m_advNextScoNo = value; }
	}

	/// <summary>
	/// シナリオ：現在の背景番号
	/// </summary>
	private string m_advBgNo;
	public string AdvBgNo {
		get { return m_advBgNo; }
		set { m_advBgNo = value; }
	}

	/// <summary>
	/// 麻雀：ステージ番号
	/// </summary>
	private int m_mjStage;
	public int MjStage {
		get { return m_mjStage; }
		set { m_mjStage = value; }
	}

	/// <summary>
	/// 麻雀：モード
	/// </summary>
	private int m_mjMode;
	public int MjMode {
		get { return m_mjMode; }
		set { m_mjMode = value; }
	}


	/// <summary>
	/// 麻雀：フラグ
	/// </summary>
	private int m_mjFlags;
	public int MjFlags {
		get {return m_mjFlags;}
		set {m_mjFlags = value;}
	}
#endregion	//-*MAHJONG
#region MYCANVAS
	/// <summary>
	/// 麻雀：麻雀制限ルール変数
	/// </summary>
	private int m_mah_limit_num = -1;	// 麻雀制限ルール変数
	public int mah_limit_num{
		get {return m_mah_limit_num;}
		set {m_mah_limit_num = value;}
	}

	/// <summary>
	/// 麻雀：バトル数
	/// </summary>
	private int m_num_battle = -1;	// バトル数
	public int num_battle{
		get {return m_num_battle;}
		set {m_num_battle = value;}
	}

	/// <summary>
	/// 麻雀：バトル数
	/// </summary>
	private int m_battle_bgm = 0;	// バトル数
	public int battle_bgm{
		get {return m_battle_bgm;}
		set {m_battle_bgm = value;}
	}

	/// <summary>
	/// 麻雀：0:勝利/1:敗北/デフォルト:-1
	/// </summary>
	private int m_flag_res_battle	= -1;	// 0:勝利/1:敗北/デフォルト:-1
	public int flag_res_battle{
		get {return m_flag_res_battle;}
		set {m_flag_res_battle = value;}
	}

	/// <summary>
	/// 麻雀：敵の強さ度
	/// </summary>
	private int m_level_cpu		=  0;	// 敵の強さ度
	public int level_cpu{
//-*SUGI_DEB***************************
#if SUGI_DEB //-*todo:注デバッグ中	
		get {return 6;}
#else
		get {return m_level_cpu;}
#endif //-*todo:注デバッグ中
//-****************************SUGI_DEB

		set {m_level_cpu = value;}
	}
#endregion //-*MYCANVAS
	/// <summary>
	/// サウンド
	/// </summary>
	[SerializeField]
	private SoundManagerCtrl m_SoundCtl;
	public SoundManagerCtrl SoundCtl {
		get { return m_SoundCtl; }
	}

	public void test()
	{
		m_SoundCtl.StopBgm();
	}
#region OPTION
	[SerializeField]
	private GameObject m_optionBox;
	[SerializeField]
	private ButtonCtl m_BtnOptBack = null;
	private bool m_isOptionOpne = false;

	public bool IsOptionOpen
	{
		get{ return m_isOptionOpne;}
		set{ m_isOptionOpne = value;}
	}
	public void UpdateOption(){
		bool isOptBoxActve = m_optionBox.activeSelf;
		if(isOptBoxActve != m_isOptionOpne){
			m_optionBox.SetActive(m_isOptionOpne);
			isOptBoxActve = m_optionBox.activeSelf;
		}
		if(!isOptBoxActve)return;
		if(m_BtnOptBack.ISPUSH){
			m_isOptionOpne = false;
		}
		// Debug.Log("UpdateOption()");
	}
	/// <summary>
	/// 音量
	/// </summary>
	private int m_vol		=  0;
	public int SOUND_VOLUME{
		get {return m_vol;}
		set {m_vol = value;}
	}
	/// <summary>
	/// シナリオ速度
	/// </summary>
	private int m_scoSpd		=  0;
	public int SCO_SPEED{
		get {return m_scoSpd;}
		set {m_scoSpd = value;}
	}
	/// <summary>
	/// シナリオ自動
	/// </summary>
	private int m_scoAuto		=  0;
	public int SCO_AUTO{
		get {return m_scoAuto;}
		set {
			m_scoAuto = value;
			//-*todo:「0」がシナリオオートOFF前提(マジックナンバー注意)
			m_isScoAuto = (m_scoAuto == 0)?false:true;
		}
	}
	private bool m_isScoAuto = false;
	public bool IS_SCO_AUTO{
		get{return m_isScoAuto;}
	}
#endregion //-*OPTION

}
