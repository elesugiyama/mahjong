using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.InteropServices;
using UnityEngine;

public class DontDestroyData : MonoBehaviour {

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}
	void Awake() {

#if UNITY_SWITCH && !UNITY_EDITOR
		FsInit();

		InitPersonalData();
		InitSlotData();
		// スロットデータの読み込み
		FileReadSaveData();
#else
		// スロットデータの読み込み
		FileReadSaveData();
#endif		
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
	private static int m_advScoNo;
	public static int AdvScoNo {
		get { return m_advScoNo; }
		set { m_advScoNo = value; }
	}

	/// <summary>
	/// シナリオ：次シナリオ番号
	/// </summary>
	private static int m_advNextScoNo;
	public static int AdvNextScoNo {
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
	/// シナリオ：CG獲得状態:保存用
	/// </summary>
	private static int m_flagGallery;
	public static int FlagGallery {
		get { return m_flagGallery; }
		set { m_flagGallery = value; }
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
	/// チャレンジ：フラグ(T:チャレンジモード	F:シナリオモード)
	/// </summary>
	private bool m_isMjChallenge;
	public bool IsMjChallenge {
		get {return m_isMjChallenge;}
		set {m_isMjChallenge = value;}
	}
	/// <summary>
	/// チャレンジ：進行度
	/// </summary>
	private static int m_challengeProgress;
	public static int ChallengeProgress {
		get {return m_challengeProgress;}
		set {m_challengeProgress = value;}
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
	private static int m_vol		=  0;
	public static int SOUND_VOLUME{
		get {return m_vol;}
		set {m_vol = value;}
	}
	/// <summary>
	/// シナリオ速度
	/// </summary>
	private static int m_scoSpd		=  0;
	public static int SCO_SPEED{
		get {return m_scoSpd;}
		set {m_scoSpd = value;}
	}
	/// <summary>
	/// シナリオ自動
	/// </summary>
	private static int m_scoAuto		=  0;
	public static int SCO_AUTO{
		get {return m_scoAuto;}
		set {
			m_scoAuto = value;
			//-*todo:「0」がシナリオオートOFF前提(マジックナンバー注意)
			m_isScoAuto = (m_scoAuto == 0)?false:true;
		}
	}
	private static bool m_isScoAuto = false;
	public static bool IS_SCO_AUTO{
		get{return m_isScoAuto;}
	}
#endregion //-*OPTION



#region	SAVELOAD

	///////////////////////////////////////////////////////////////////
	/// 構造体

	// システムデータ
	[System.Serializable] public class PersonalData{
		public int vol;			// 音量（0～４）
		public int msg;			// 文字速度（0～４）
		public int auto;		// オート速度（0～４）
		public bool btn_skip;	// スキップの有無
		public int cg;			// CGフラグ
	}

	// スロットデータ
	[System.Serializable] public class SlotData{
		public bool active;					// 使用可能なデータが入っているかどうか
		public string realTime;				// ゲームを保存した 年/月/日/時/分
		public long[] ev_flag;				// イベントフラグ
		public int cg;						// CGフラグ
		#region MJ
		public int scoFileNo;				//-*読み込むシナリオ番号
		public int challengeProgress;		//-*チャレンジの進行度
		#endregion	//-*MJ
	}

	///////////////////////////////////////////////////////////////////	/// 変数
	public static PersonalData personalData;	// システムデータ
	public static SlotData[] slotData;			// スロットデータ
	public static SlotData currentData;			// 現在のデータ
	///////////////////////////////////////////////////////////////////
	/// 定数

	// 各種最大値
	public const int MAX_FILES		= 2;	// セーブファイル
	public const int MAX_SLOT		= 8;	// スロット
	public const int MAX_EV	 		= 4;	// イベントフラグ
	public const int MAX_CG	 		= 12;	// CGフラグ
	public const int MAX_SYSTEM 	= 4;	// 各種システムデータ
	public const int MAX_DAY		= 7;	// ゲーム内日数
	public const int MAX_TIMEZONE	= 3;	// 時間帯
	public const int MAX_LOVE		= 20;	// 好感度
	static string[] FILE_KEY = {"pelsonalData", "slotData"};

	// 初期値の設定
	void InitPersonalData()
	{
		if(personalData!=null) return;

		// システムデータの初期値
		personalData = new PersonalData();
		personalData.vol		= 2;
		personalData.msg		= 2;
		personalData.auto		= 2;
		personalData.btn_skip	= false;
		personalData.cg			= 0;
		
	}

	// 初期値の設定
	void InitSlotData()
	{
		Debug.Log("//-*InitSlotData()");
		if(slotData!=null) return;

		int i, k;

		// スロットデータの初期値
		slotData = new SlotData[8];
		for(i=0; i<MAX_SLOT; i++)
		{
			slotData[i] = new SlotData();
			slotData[i].active		= false;
			slotData[i].realTime	= "";
			slotData[i].ev_flag		= new long[MAX_EV];
			for(k=0; k<MAX_EV; k++)	slotData[i].ev_flag[k] = 0;
			slotData[i].cg			= 0;
			#region MJ
			slotData[i].scoFileNo = 0;
			slotData[i].challengeProgress = 0;
			#endregion	//-*MJ
		}

	}

	public static void InitCurrentData()
	{
		// 現在の状態を初期化
		if(currentData == null)
		{
			currentData = new SlotData();
		}
		currentData.active		= true;
		currentData.realTime	= "";
		currentData.ev_flag		= new long[4];
		for(int i=0; i<MAX_EV; i++)	currentData.ev_flag[i] = 0;
		currentData.cg			= 0;
		#region MJ
		currentData.scoFileNo = 0;
		currentData.challengeProgress = 0;
		#endregion	//-*MJ
	}

	///////////////////////////////////////////////////////////////////
	/// データセット
	public static void Save(int id)
	{
		if(id<0 && id>=MAX_SLOT) return;
Debug.Log("//-*id:"+id+"...slotData"+slotData.Length);
		slotData[id].active			= true;
Debug.Log("//-*1");
		// 現在時刻を取得して保存する
		slotData[id].realTime	= System.DateTime.Now.ToString("yyyy/MM/dd HH:mm");
Debug.Log("//-*2...."+slotData[id].ev_flag.Length+"...."+currentData.ev_flag.Length);

		for(int i=0; i<MAX_EV; i++){
			Debug.Log("//-*2_1..MAX("+MAX_EV+") No."+i);
			slotData[id].ev_flag[i] = currentData.ev_flag[i];
		}
Debug.Log("//-*3");
		#region MJ
		currentData.cg = FlagGallery;
		slotData[id].challengeProgress	= ChallengeProgress;
		Debug.Log("//-*SAVE:::FlagGallery("+currentData.cg+") ChallengeProgress("+slotData[id].challengeProgress+")");
		#endregion	//-*MJ
		slotData[id].cg					= currentData.cg;
		slotData[id].scoFileNo			= AdvNextScoNo;//-*AdvScoNo;

	}

	public static bool Load(int id)
	{
		if(id<0 && id>=MAX_SLOT) return false;
		Debug.Log("//-*LOAD::["+id+"]::slotData."+slotData.Length);

		if(slotData[id].active==false)	return false;

		currentData.realTime	= "";
		Debug.Log("//-*LOAD::::ev_flag");
		for(int i=0; i<MAX_EV; i++)	currentData.ev_flag[i] = slotData[id].ev_flag[i];
		Debug.Log("//-*LOAD::::cg");
		currentData.cg				= slotData[id].cg;
		#region MJ
		FlagGallery = currentData.cg;
		ChallengeProgress = currentData.challengeProgress;

		Debug.Log("//-*LOAD::::FlagGallery("+FlagGallery+") ChallengeProgress("+ChallengeProgress+")");
		#endregion	//-*MJ
		return true;
	}

	///////////////////////////////////////////////////////////////////
	/// ファイル操作

	// システムデータをファイルに書き込む
	public static void FileWritePersonalData()
	{
		// 2020/09/04 harashima
//#if UNITY_SWITCH
#if UNITY_SWITCH && !UNITY_EDITOR
		#region MJ
		SetSaveOptionData();
		Debug.Log("//-**********"+personalData.vol);
		#endregion //-*MJ
		FsSave();
#else
		if(personalData == null)return;
		DataSerialize.Save<PersonalData>(FILE_KEY[0], personalData);
#endif
	}

	// スロットデータをファイルに書き込む
	public static void FileWriteSlotData()
	{
		// 2020/09/04 harashima
//#if UNITY_SWITCH
#region MJ
		// SetSaveSlotData(currentData);
		Save(0);
#endregion //-*MJ
#if UNITY_SWITCH && !UNITY_EDITOR
		FsSave();
#else
		DataSerialize.Save<SlotData[]>(FILE_KEY[1], slotData);
#endif
	}

	/// 全てのデータをファイルから読み込む
	void FileReadSaveData()
	{

		// 2020/09/04 harashima
//#if UNITY_SWITCH
#if UNITY_SWITCH && !UNITY_EDITOR
#region MJ
		InitCurrentData();
#endregion //-*MJ
		FsLoad();
		Load(0);
#else
		personalData = DataSerialize.Load<PersonalData>(FILE_KEY[0]);

		slotData = DataSerialize.Load<SlotData[]>(FILE_KEY[1]);
#endif

		if(personalData==null) InitPersonalData();
#region MJ
		SetLoadOptionData(personalData);
#endregion //-*MJ

		if(slotData==null) InitSlotData();
#region MJ
		SetLoadSlotData(slotData[0]);
#endregion //-*MJ

		
		// slotData[0].active      = true;
		// slotData[0].family      = "葛城";
		// slotData[0].first       = "祐介";
		// slotData[0].toDay       = 4;
		// slotData[0].time_zone   = 2;
		// slotData[0].miu_love    = 20;
		// slotData[0].juri_love   = 0;
		// slotData[0].ikumi_love  = 0;
		// slotData[0].serika_love = 0;
		// slotData[0].realTime    = "";
		// // slotData[0].ev_flag		= new long[4];
		// for(int i=0; i<MAX_EV; i++)	slotData[0].ev_flag[i] = 0;
		// slotData[0].cg			= 0;

	}

	// PlayerPrefs バイナリ保存
	private class DataSerialize {
		// <!!> T is any struct or class marked with [Serializable]
		public static void Save<T> (string prefKey, T serializableObject) {
			MemoryStream memoryStream = new MemoryStream ();
			BinaryFormatter bf = new BinaryFormatter ();
			bf.Serialize (memoryStream, serializableObject);
			string tmp = System.Convert.ToBase64String (memoryStream.ToArray ());
			PlayerPrefs.SetString ( prefKey, tmp );
		}

		public static T Load<T> (string prefKey) {
			if (!PlayerPrefs.HasKey(prefKey)) return default(T);
			BinaryFormatter bf = new BinaryFormatter();
			string serializedData = PlayerPrefs.GetString(prefKey);
			MemoryStream dataStream = new MemoryStream(System.Convert.FromBase64String(serializedData));
			T deserializedObject = (T)bf.Deserialize(dataStream);
			return deserializedObject;
		}
	}



#if UNITY_SWITCH

	private static System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

	private static nn.account.Uid userId;
	private const string mountName = "MySave";
	private const string fileName = "MySaveData";
	private static string filePath;
	private static nn.fs.FileHandle fileHandle = new nn.fs.FileHandle();

	private static nn.hid.NpadState npadState;
	private static nn.hid.NpadId[] npadIds = { nn.hid.NpadId.Handheld, nn.hid.NpadId.No1 };
	private const int saveDataVersion = 1;
	// private const int saveDataSize = 512;
	// private const int saveDataSize = 1024;
	private const int saveDataSize = 2048;
	private static int counter = 0;
	private static int saveData = 0;
	private static int loadData = 0;

	void FsInit()
	{
		nn.account.Account.Initialize();
		nn.account.UserHandle userHandle = new nn.account.UserHandle();

		// 2020/09/04 harashima
		// OpenPreselectedUserは廃止になったので、
		// ドキュメントに従ってTryOpenPreselectedUserに変更
		//nn.account.Account.OpenPreselectedUser(ref userHandle);
		nn.account.Account.TryOpenPreselectedUser(ref userHandle);
		nn.account.Account.GetUserId(ref userId, userHandle);

		Debug.Log("userId___:"+userId);
		#if false	//-*
		nn.Result result = nn.fs.SaveData.Mount(mountName, userId);
		#else
		//-*デバッグ用
		nn.Result result = nn.fs.SaveData.MountForDebug(mountName);
		#endif
		result.abortUnlessSuccess();

		filePath = string.Format("{0}:/{1}", mountName, fileName);
Debug.Log("//-************************/"+nn.fs.SaveData.IsExisting(userId));
Debug.Log("//-*filePath：："+filePath);

	}


	private static void FsSave()
	{
Debug.Log("//-*FsSave:Start******,._.,");

		byte[] data;
		using (MemoryStream stream = new MemoryStream(saveDataSize))
		{
			BinaryFormatter bf = new BinaryFormatter ();
			bf.Serialize (stream, personalData);
			bf.Serialize (stream, slotData);
Debug.Log("//-*FsSave:slotData:"+slotData.Length);
			stream.Close();
			data = stream.GetBuffer();
			Debug.Assert(data.Length == saveDataSize);
		}

// #if UNITY_SWITCH
		// Nintendo Switch Guideline 0080
		UnityEngine.Switch.Notification.EnterExitRequestHandlingSection();
// #endif

		nn.Result result = nn.fs.File.Delete(filePath);
		if (!nn.fs.FileSystem.ResultPathNotFound.Includes(result))
		{
			result.abortUnlessSuccess();
		}

		result = nn.fs.File.Create(filePath, saveDataSize);
		result.abortUnlessSuccess();

		result = nn.fs.File.Open(ref fileHandle, filePath, nn.fs.OpenFileMode.Write);
		result.abortUnlessSuccess();

		result = nn.fs.File.Write(fileHandle, 0, data, data.LongLength, nn.fs.WriteOption.Flush);
		result.abortUnlessSuccess();

		nn.fs.File.Close(fileHandle);
		// 2020/09/04 harashima 
		//result = nn.fs.SaveData.Commit(mountName);
		result = nn.fs.FileSystem.Commit(mountName);
		result.abortUnlessSuccess();

// #if UNITY_SWITCH
		// Nintendo Switch Guideline 0080
		UnityEngine.Switch.Notification.LeaveExitRequestHandlingSection();
// #endif

		// UnityEngine.Debug.LogError("FsSave");
Debug.Log("//-*FsSave:End******(-_-)");

	}

	private static void FsLoad()
	{
Debug.Log("//-*FsLoad:Start******,._.,");
		nn.fs.EntryType entryType = 0;
		nn.Result result = nn.fs.FileSystem.GetEntryType(ref entryType, filePath);
		if (nn.fs.FileSystem.ResultPathNotFound.Includes(result)) { return; }
		result.abortUnlessSuccess();

		result = nn.fs.File.Open(ref fileHandle, filePath, nn.fs.OpenFileMode.Read);
		result.abortUnlessSuccess();

		long fileSize = 0;
		result = nn.fs.File.GetSize(ref fileSize, fileHandle);
		result.abortUnlessSuccess();

		byte[] data = new byte[fileSize];
		result = nn.fs.File.Read(fileHandle, 0, data, fileSize);
		result.abortUnlessSuccess();

		nn.fs.File.Close(fileHandle);


		try
		{
			using (MemoryStream stream = new MemoryStream(data))
			{
				BinaryFormatter bf = new BinaryFormatter();
				personalData = (PersonalData) bf.Deserialize(stream);
				slotData = (SlotData[]) bf.Deserialize(stream);
			}

		}
		catch
		{
			UnityEngine.Debug.LogError("ロード失敗");
		}
Debug.Log("//-*FsLoad:End******(-_-)");
	}

#region  MJ	
private void SetLoadOptionData(PersonalData data){
	SOUND_VOLUME = data.vol;
	SCO_SPEED = data.msg;
	SCO_AUTO = (data.btn_skip)?1:0;
	Debug.Log("//-*(-_-)人(＾－＾)"+data.cg);
}
private static void SetSaveOptionData(){
	personalData.vol = SOUND_VOLUME;
	personalData.msg = SCO_SPEED;
	personalData.btn_skip = (SCO_AUTO==0 )?false:true;
}

private static void SetLoadSlotData(SlotData data){
	AdvScoNo = data.scoFileNo;
	AdvNextScoNo = AdvScoNo;
	FlagGallery = data.cg;
	Debug.Log("FlagGallery:"+FlagGallery+"::::AdvScoNo"+AdvScoNo);
}
private static void SetSaveSlotData(SlotData data){
	slotData[0] = data;
	Debug.Log("slotData[0]:cg("+slotData[0].cg+") Time = "+slotData[0].realTime);
}
public void SetFlagGallery(int a)
{
	Debug.Log("No."+a+"***Befor:"+FlagGallery);
	FlagGallery|=(1<<a);                   // ギャラリー用CGフラグのセット
	Debug.Log("(1<<a):"+(1<<a)+"***After:"+FlagGallery);
}
public bool CheckFlagGallery(int a)
{
	bool b = ((FlagGallery&(1<<a))!=0);
	Debug.Log("//-****FlagGallery["+FlagGallery+"]**a["+a+"]***("+(1<<a)+")***"+b);
	return ((FlagGallery&(1<<a))!=0);              // ギャラリー用CGフラグのチェック
}
#endregion //-*MJ
#endif

//-*********
#endregion	//-*SAVELOAD
}
