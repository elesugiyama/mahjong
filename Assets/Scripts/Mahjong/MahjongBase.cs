using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Const;
//-********
using MahjongDeffine;
using MJDefsHeader;
using GameDefsHeader;
using MJDialogHeader;
//-********


public partial class MahjongBase : SceneBase {

	private MAHJONGMODE a_Mode = MAHJONGMODE.mMODE_MAX;	//-*全体モード(todo:GAME_Jのamodeと統合した)
	private MAHJONGMODE a_ModeNext = MAHJONGMODE.mMODE_MAX;	//-*次のモード
	#if true //-*todo:麻雀内モード(型名変更予定)
	private INMJMODE				mode;			//Mode			// ゲームモード
	private INMJMODE				m_Mode;			//Mode			// ゲームモード
	#endif //-*todo:麻雀内モード
#region GAME_J
	private int amode = 0;	//-*動作モード	//-*todo:(a_Modeと統合した)
	private int phase = 0;	//-*動作フェーズ
	private bool paintF= false;	//ゲームメイン描画フラグ
	private bool paintE= false;	//ゲームメイン描画終了フラグ
	private int paintT= 0;			//初回描画フラグ

#endregion //-*GAME_J
	
	private ScreenEffect.EFFECTTYPE m_EffectType = ScreenEffect.EFFECTTYPE.NONE;	//-*演出
	public Canvas UICanvas;     //UIを表示するキャンバス

	[Header("麻雀牌(プレイヤー)")]
	//-手牌
	[SerializeField]
	private GameObject m_myHandTilesBase;
	[SerializeField]
	private List<GameObject> m_myHandTiles = new List<GameObject>();
	//-*捨て牌
	[SerializeField]
	private GameObject m_myDiscardedTilesBase;
	[SerializeField]
	private List<GameObject> m_myDiscardedTiles = new List<GameObject>();
	//-*副露牌
	[SerializeField]
	private GameObject m_myFuroTilesBase;
	[SerializeField]
	private List<GameObject> m_myFuroTilesTiles = new List<GameObject>();

	[Header("麻雀牌(相手)")]
	//-手牌
	[SerializeField]
	private GameObject m_yourHandTilesBase;
	[SerializeField]
	private List<GameObject> m_yourHandTiles = new List<GameObject>();
	//-*捨て牌
	[SerializeField]
	private GameObject m_yourDiscardedTilesBase;
	[SerializeField]
	private List<GameObject> m_yourDiscardedTiles = new List<GameObject>();
	//-*副露牌
	[SerializeField]
	private GameObject m_yourFuroTilesBase;
	[SerializeField]
	private List<GameObject> m_yourFuroTilesTiles = new List<GameObject>();

	[Header("その他")]
	//-*ヘッダーフッター
	[SerializeField]
	private MJHeaderFooter m_HeaderFooterBase;
	//-*王牌
	[SerializeField]
	private GameObject m_WanTilesBase;
	//-*鳴き系メニュー
	[SerializeField]
	private GameObject m_callMenuBase;
	[SerializeField]
	private List<GameObject> m_callMenus = new List<GameObject>();
	//-*鳴き宣言描画用
	[SerializeField]
	private List<MJCallDraw> m_callObjs = new List<MJCallDraw>();	//-*0:自分　1:相手

	//-*流局表示用
	[SerializeField]
	private MJCallDraw m_callObjRyukyoku;	//-*todo:検討中


	//-*リーチ棒(0：自分、1:相手)
	[SerializeField]
	private List<GameObject> m_ribos = new List<GameObject>();

	[Header("リザルト画面")]
	//-*リザルト画面
	[SerializeField]
	private GameObject m_resultBox;
	//-*ポイント変動画面
	[SerializeField]
	private GameObject m_resultPointBox;
	[SerializeField]
	private GameObject m_nextButton;	//-*ページ送りボタン


	//-*牌選択用(元キー操作)
	private short m_haiCsrNo = -1;
	private bool m_haiSelF = false;
	//-*鳴き系メニュー選択用(元キー操作)
	private short m_callMenuCsrNo = -1;
	private bool m_callMenuSelF = false;
	//-*「次へ」ボタン
	private bool m_nextBtnF = false;
	public bool nextBtnF{
		get{return m_nextBtnF;}
		set{m_nextBtnF = value;}
	}

#region GAME_PAD
	[Header("ゲームパッド関連")]
	[SerializeField]
	private GameObject m_PaiCursol=null;

	private int m_handTileCursolNo = 0;


#endregion //-*GAME_PAD	
//-*SUGI_DEB***************************
#if SUGI_DEB //-*todo:注デバッグ中

	[Header("デバッグ")]
	//-*デバッグ
	[SerializeField]
	private DebBoxInGame m_DebBox;
#endif //-*todo:注デバッグ中
//-****************************SUGI_DEB
	private int deb_tsumikomiNum = -1;
	//-*運処理
	private int deb_myLuckP = 0;		//-*自分の運
	private int deb_yourLuckP = 0;		//-*相手の運
	private bool deb_isLuckyTime = false;

//-*****ここまでUnity用

#if false //-*todo:作ってみたけど...
	//-*内部処理用
	private List<MJTIle> m_mjTiles = new List<MJTIle>();
	private List<Member> m_member = new List<Member>();
#endif //-*todo:作ってみたけど...

#region MJRALLY
	private int						m_OnePushKeyState;		//unsigned int	// キーステータス(ビットフラグ)
	private int						stick;	// キーコード(リピート可)						
	private int						strig;	// キーコード(リピート不可)
#if false //-*todo
	_static int						m_BgmOffFlag= SOUND_ON;			// BGM ON/OFF判定
	_static int						m_SeOffFlag= SOUND_ON;			// SE  ON/OFF判定
	_static BYTE					m_Se_Volume= SOUND_MAX;			// SE音量。
	_static int						m_PlayMusicNo;					// 再生中音楽番号
	_static int						m_SeOnNo;						// SE番号
#endif
	//認証関連
	private int						submiterr;		//PremiumError	// 認証エラーコード	2006/02/27 要望No.731
	// 機種依存
	private int						TextOffsetY;							// テキストYオフセット値
	private int						joyAUp;									// JOYA_2
	private int						joyADown;								// JOYA_8
	private int						joyARight;								// JOYA_6
	private int						joyALeft;								// JOYA_4
	private int						joyASelect;								// JOYA_5
	private int						GameEnd_Flag;
	private int						GameEnd_YesNo;
#if false //-*todo
	_static MJFileInfo				f_info;
	// オプション変更前ファイルデータ保持→[戻る]:キャンセルへ
	_static FileOptionSetting		PreFileOptionSetting;
#endif
	// --- オプション用フラグ・
	private byte	m_DaiuchiFlag;
	// --- ﾁｬｯﾄ用のフラグ。
	// 一巡に１回のみ送信可能。
	private  bool	m_ChatSendFlag;		//BYTE

	// リザルト画面時に使用。
	private  int[]		m_NowResultDisp = new int [4];
	private  int[]		m_NextResultDisp = new int [4];
	private  bool	m_ResultEndFlag;
	public  bool	ResultEndFlag {
		get{return m_ResultEndFlag;}
		set{m_ResultEndFlag = value;}
	}


#if false //-*todo:
	// --- 描画用スプライト宣言 ---
	// Popular.PNG
	// ポピュラー
	// バラバラメニュー[9枚]
	// アイコン：右・下[2枚]
	// はい・いいえ[1枚]
	// 黄色のバラバラメニュー[9枚]
	_static _IImage			m_pIPopular;		//*m_pIPopular;
	_static SpriteInfo		m_spPopular[]= new SpriteInfo [MAX_POPULAR_COUNT];
	// --- サバイバルモード キャラクタースプライト。 ---
	// char_00.png ～ char_16.png
	_static _IImage			m_pISurvival;		//*m_pISurvival;
	_static SpriteInfo		m_spSurvival;
	// --- サバイバルモード フロアー。 ---
	// BG_floor00.png
	_static _IImage			m_pIFloorBG;		//*m_pIFloorBG;
	_static SpriteInfo		m_spFloorBG;
	// --- メインBG ---。
	// MainBg.png。
	_static _IImage			m_pIMainBG[]= new _IImage [D_MAX_BG_COUNT];		//*m_pIMainBG
	_static SpriteInfo		m_spMainBG[]= new SpriteInfo [D_MAX_BG_COUNT];
	// --- ゲーム中に使用する画像 ---。
	// TakuSozai.PNG
	// taku.png
	// End.PNG
	// point.PNG
	_static _IImage			m_pIGame[]= new _IImage [D_GAME_MAX_SOZAI_COUNT];	//*m_pIGame
	_static SpriteInfo		m_spGame[]= new SpriteInfo [D_GAME_MAX];
	// --- キャラクター顔画像 ---。
	// character.PNG
	_static _IImage			m_pIChar;
	_static SpriteInfo		m_spChar[]= new SpriteInfo [MJ_CHARACTER_MAX_COUNT];
	// 和了した後の役一覧表。
	// YakuSozai.PNG
	_static _IImage			m_pIYakuBG;						// 役の画像。
	_static SpriteInfo		m_spYakuBG[]= new SpriteInfo [YK_MAX];		//55	// 役の画像分。
	// --- 汎用的に使用する ---。
	_static _IImage			m_pIImage;		//*m_pIImage;
	_static SpriteInfo		m_sprite[]= new SpriteInfo [20];
	// --- 汎用的に。
	_static _IImage			m_pIBar;		//*m_pIBar;
	_static SpriteInfo		m_option;
	// --- カウントダウン数値。
	_static _IImage			m_pICountDown;	//*m_pICountDown;
	_static SpriteInfo		m_spCount[]= new SpriteInfo [D_COUNT_DOWN_MAX];
	// --- モード。
	_static _IImage			m_pMode;		//*m_pMode;
	_static SpriteInfo		m_spMode[]= new SpriteInfo [D_MODE_MAX_COUNT];
	// --- マーク。
	_static _IImage			m_pMark;		//*m_pMark;
	_static SpriteInfo		m_spMark[]= new SpriteInfo [D_MAX_MARK_COUNT];
	// Barよりテキストのロード。
//	_static RESOURCE		resData;															// リソースデータ
	_static String			m_NumberText[]= new String [SYSNUMBER_RES_COUNT];		//AECHAR								//[][SYSFONT_SIZE_10]
	_static String			m_RuleDispText[]= new String [RULE_MAX_RES_COUNT];		//AECHAR								//[][SYSFONT_SIZE_20]
	_static String			m_FreeBattleText[]= new String [FREE_BATTLE_RES_CONT];	//AECHAR	// フリー対戦設定文字列格納	//[][SYSFONT_SIZE_20]
	_static String			m_FreeModeText[]= new String [FREE_MODE_RES_COUNT];		//AECHAR								//[][SYSFONT_SIZE_20]
	_static String			m_OptionWordText[]= new String [OPTION_WORD_RES_COUNT];	//AECHAR	// オプション用				//[][SYSFONT_SIZE_10]
#endif

	private int				m_screenWidth;				// デバイス幅
	private int				m_screenHeight;				// デバイス高さ
	private short 			m_scrollX;					// マップ描画位置 X座標
	private short 			m_scrollY;					// マップ描画位置 Y座標
	private char			m_Anim;						// カーソルアニメーション。
	private char			m_DrawMode;					// 切断時・再接続時で使用 20051206
	private char			m_RankMode;					// 段位認定戦のモード。昇格・降格・何も起きない。
	private int			m_WhichDrawMode;			// ゲーム中の描画するモード。NetWork or StandAllone //-*todo：元char型　不要？
	public int WhichDrawMode{
		get {return m_WhichDrawMode;}
		set {m_WhichDrawMode = value;}
	}
	private long			m_ConnectWaitTime;			// 再接続ダイアログの表示時間を管理。
	private long			m_DisConnectWaitTime;		// 切断ダイアログの表示時間を管理。
	private int				m_ReConnectWaitDrawMode;	// 再接続時の描画するモード管理。
#if false //-*todo 通信
	// ゲーム中のチャット用。
	private byte			m_ChatFlag;					// SoftKey1のフラグ。
	private byte			m_FirstChatNo;				// チャットの描画開始ＩＤ。
	private byte			m_ChatMemberID;				// 誰が喋っているのか？
	private String[]		ChatMsg= new String [MJDefine.D_CHAT_LIST_MAX];	//AECHAR [D_CHAT_MSG_MAX]	// チャットメッセージリスト
	private long[]			m_ChatWaitTime = new long [4];			
	private short			m_chat_menu_csr;			
#endif
#if false //-*todo
	private long[]			m_optionWaitTime = new long [MJDefine.D_MAX_OPTION_COUNT];	
#endif
	private bool			gGamePointFlag;
	/******************************************************************************/
	/*		マーキー用						*/
	/******************************************************************************/
	private int						marqueeX;			//マーキー表示X座標				
	private int						marqueeY;			//マーキー表示Y座標				
	private String					marqueeDrawText;	
	/******************************************************************************/
	/*		対話用							*/
	/******************************************************************************/
	private byte			gTalkFlag;					// 対話フラグ
	private byte			gTalkCnt;					// 話す家数(実は会話の番号？)
	private long[]			gTalkWait = new long [MJDefine.D_TALK_TABLE_MAX];	// 対話待ち時間
	private byte[]			gTalkHouse = new byte [MJDefine.D_TALK_TABLE_MAX];	// 話す家位置
	private byte[]			gTalkChar = new byte [MJDefine.D_TALK_TABLE_MAX];	// 話すキャラ
	private byte[]			gTalkType = new byte [MJDefine.D_TALK_TABLE_MAX];	// 台詞タイプ
	private byte			gTalkNo;											// 話す家

	/****************			ゲーム管理宣言					*******************/
	private int				m_paiData;	// 牌の番号
	
	/******************************************************************************/
	/*		通信用							*/
	/******************************************************************************/
	private string	gMyTable_Name;
	private byte	gMyTable_CharPicNo;
	private byte	gMyTable_CharThink;
	private int		gMyTable_Rank;
	private short	gMyTable_RankPoint;
	private short	gMyTable_AddRankPoint;
	private byte	gMyTable_NakiNashi;
	private byte gMyTable_AIFlag;
	private long gMyTable_total_point;
	private long[] gMyTable_pre_total_point_rank= new long [3];
	private long[] gMyTable_cur_total_point_rank= new long [3];
	private byte gMyTable_PreRank;
	private string[]    gMyTable_ChatMsgList= new String [15];
	private int				gNetTable_Membercnt;
#if false //-*todo:保留
	private CHAR_INFO_TBL[]	gNetTable_NetChar= new CHAR_INFO_TBL [4];
#endif //-*todo:保留
	private byte 		gNetTable_byConDisp;
	private short		gNetTable_MatchID;
	private byte 		gNetTable_MatchType;
	private string			gNetTable_Sysmes;

	private short		gTakuID;							/* 卓ID						*/	//w4 値範囲:0～999
	private int				gThinkResult;						/* 思考ルーチンの判断結果	*/
	private int				gRonChkResult;						/* ロンルーチンの判断結果	*/
	private int				gNakiChkResult;						/* 鳴きルーチンの判断結果	*/
	private byte[]			gPonChk= new byte [4];			/* ポン判断					*/
	private byte[]			gChiChk= new byte [4];			/* チー判断					*/
	private byte[]			gKanChk= new byte [4];			/* カン判断					*/
	private byte[]			gNakiResult= new byte [4];		/* 鳴き判断結果				*/

	private long			gMemberWaitTimeOut;					/* メンバ待ち時間			*/
	private long			gTsumoTimeOut;						/* ツモタイムアウト時間		*/
	private long			gNakiTimeOut;						/* 鳴きタイムアウト時間		*/
	private long			gNextTimeOut;						/* 次タイムアウト時間		*/
	private long			gLogoTimeOut;						// ロゴのタイムアウト時間。
	private byte			gAutoFlag;							/* 自動決定フラグ			*/


	//==================================================
	//制御変数(Game.j)
	//==================================================
	private bool m_isGameJ_app_main_done = false;
	public bool app_main_done
	{
		get{return m_isGameJ_app_main_done;}
		set{m_isGameJ_app_main_done = value;}
	}//0504mt

	private bool m_isGameJ_Ready; 		//準備中
	public bool ready
	{
		get{return m_isGameJ_Ready;}
		set{m_isGameJ_Ready = value;}
	}
	private bool m_isGameJ_Start; 		//起動処理中
	public bool start{
		get{return m_isGameJ_Start;}
		set{m_isGameJ_Start = value;}
	}

/**********		ここから麻雀用宣言　はじまり	**********/

	/****************************************/
	/*		GAMEMAIN.C用　ワークＲＡＭ		*/
	/****************************************/
	private int			game_player;			//　自分のプレーヤー番号
	//-*todo:↑元char型							//　通信時に０－３の値を設定
												//　スタンドアローン時は０固定です

	private PLAYERWORK		play_wk;		//*play_wk;
	private short			first_ron;

	private short			menu_mode;
	private short			menu_ret;
	private short			menu_csr;							//　選択しているメニュ位置

	private short			hai_csr;							//　選択している牌位置
	private short			hai_up;								//　鳴きで選択される牌（複数）
																//　通常の捨て牌選択時も使用
	private short			hai_on;								//　選択できない牌？意味不明20051025
	private short			hai_open;		//自分 下家 対家 上家

	private short			chi_csr;							// チー選択カーソル 2006/02/06 BugNo 30
#region UNITY_ORIGINAL
	private short			chi_hai_csr;	//-*チー用選択中の手牌番号
#endregion	//-*UNITY_ORIGINAL
	private short			chi_min;
	private short			chi_max;

	/****************************************/
	/*		MENU.c用　ワークＲＡＭ			*/
	/****************************************/
	private char[]			menu_sel = new char [MJDefine.MENU_SEL_MAX];				//

	private byte			select_rule;						// 選択したルール番号
	private int			rule_edit_page;						// 選択しているルールページ
	private int			next_f_cnt;

	private char			sel_cnt;							//選択しているキャラクター数
	private int[]			sel_buf = new int [3];			//選択しているキャラクター番号	//-*todo:元char型
	private char[]			sel_pos = new char [3];			//？？？

	private byte			drop_chr_no;
	private char			kakushi;							//隠しの有効無効判定
	private byte			kakushi_char;						//獲得している隠しキャラ番号
														//0x00011111 各ビットでキャラになっている

	/****************************************/
	/*		GAMESYS.c用　ワークＲＡＭ		*/
	/****************************************/

	/*** メインステータス	***/
	private byte			mj_sts;

	/*** ユーザルール ***/
	private byte[]			gbyUserRule = new byte [(int)RL.RLSIZE];	/*	ユーザルール	*/

	/*** ゲームデータ ***/
	private GAMEDATA		sGameData;							/*	ゲームデータ	*/

#if false//-*todo:
	/*** サバイバルのデータ ***/
	private RESULT_SV		sSurvivalData;
#endif
	///*** 通信対戦のデータ ***/
	private byte[]			Rultbl= new byte [ (int)RL.RLSIZE ];		//	対局用ルールテーブル

	private RULESUBDATA		sRuleSubData;						/*	ルール補助データ	*/
	private char[]			gszTsumibouStr = new char [8+1];	/*	積み棒数表示用		*/
	private char[]			gszRiboStr = new char [8+1];		/*	リーチ棒表示用		*/
	private char[]			gszNokoriStr = new char [8+1];		/*	山の残り牌数表示用	*/

	private TABLEDATA[]		gsTableData = new TABLEDATA [3];	//	対局データテーブル
	private TABLEDATA		gpsTableData;		//*gpsTableData;	//	アクティブ対局データ

	private byte[]			Bipai= new byte [ 136 ];			/* 全牌の並び */

	private PLAYERWORK[]		gsPlayerWork= new PLAYERWORK [MJDefine.MAX_TABLE_MEMBER];		//	プレーヤーテーブル
	private PLAYERWORK		gpsPlayerWork;		//*gpsPlayerWork;	//	アクティブプレーヤー
	private PLAYERWORK		sTmpPlayerWork;

	private byte[]			KyokuKekka= new byte [ MJDefine.MAX_TABLE_MEMBER ];		/* 局の結果格納 */
	private MJK_RESULT		gMJKResult;							/*	局の結果	*/
	private byte			Kyoend;								/* 終了局番号 */
	private long			Total_point;						/* アガリ点数 */

	private byte			Thcnt;								//	アクティブプレーヤーの手牌数
	private byte			Csr;								/* カーソル位置 */

	private byte			Order;								//	誰の手順か
	private byte			Odrbuf;								//	誰が牌を切ったところか
	private byte			Sthai;								//	捨牌
	private byte			hotpai;								//	危険牌
	private byte			Bpcnt;								//	牌番号
#if	Rule_2P
	private byte			BpcntKep;							//初期牌番号
#endif
#if	__comEnfeeble	//COM思考の弱体化
	private int				tumoPassMax;						//ツモ切り 開始残り牌数
	private int				tumoPassMin;						//ツモ切り 終了残り牌数
	private int				nakiPassMax= 50;	//未使用		//鳴き 開始残り牌数
	private int				nakiPassMin= 50;	//未使用		//鳴き 終了残り牌数
#endif
	private byte			Status;								/* 対局ステータス */
	private byte			Pinch;								/* ピンチ(特別流局) */
	private byte			Bp_now;								/* 1996.7.5.DIALOG 序盤・中盤・終盤 */
	private byte			Richi;								/* 立直人数 */

	private MNTDATA			sMntData;							/* 面子データ				*/
	private byte			Ponodr;								/* ポンする人				*/
	private byte			Chiodr;								/* チーする人				*/
	private byte			Ponflg;								/* 選択用ポンフラグ			*/
	private byte			Ponpos;								/* 選択用ポンポジション		*/
	private byte			Chiflg;								/* 選択用チーフラグ			*/
	private byte			Chinum;								/* 選択用チーポジション		*/
	private byte[]			Chips1 = new byte [3];				/* mjthink チー出来る面子の１番上の牌 */
	private byte[]			Chips2 = new byte [3];				/* mjthink チー出来る面子の１番上の牌 */
	private byte[]			Chipsf = new byte [3];				/* mjthink チー出来るか出来ないかフラグ */

	private byte			Roncnt;								// ロンできる人数
	private byte			Ronodr;								// ロンする人（単独）
	private byte[]			Hanetbl= new byte [4];			// ロンする人（ダブロン以上ありのときに判断）

	private byte			byDoraCnt;							/*	ドラの数	*/
	private byte			byUraDoraCnt;						/*	裏ドラの数	*/
	private byte			Kancnt;								/* カンの数 */
	private byte[]			Dora = new byte [5];				/*	表ドラ	*/
	private byte[]			Ura = new byte [5];				/*	裏ドラ	*/
	private byte[]			Hyouji_dora = new byte [5];		// 表示用ドラ牌
	private byte[]			Hyouji_ura = new byte [5];			// 表示用裏ドラ牌
	private bool			AnkanToDora;						/* 暗カンしたらドラになったフラグ */
	private byte			Dorakan;							/* カンしたらドラになったフラグ	*/
	private byte[]			WanPaiBuf = new byte [14];
				/* カン表示用のバッファ？？？*/

	private bool			Ryansh;								/* 2飜縛りフラグ */
	private bool			pRyansh;							// プレイヤー二飜縛りフラグ
	private byte[]			Ff= new byte [56];				//	ファン牌のカウント
	private byte			Totsunyu_flg;						/* DIALOG 局が進んだ時に立てる */
	private byte[]			cntbuf = new byte [256+2];			//牌の残り枚数	/*051F-0556*/
	private byte[]			Dicnum= new byte [2];				//	さいころ

	private byte			agari_sts;
	private byte			MjKyokuInit_bflag;

	private byte			reentry_m1_bflag;
#if DEBUG		//_DEBUG
	private byte			reentry_m1_bflag_dbg;
#endif

	private byte			comput_m1_bflag;
#if DEBUG		//_DEBUG
	private byte			comput_m1_bflag_old;
#endif
	private byte			comput_m1_iThcnt;

	private byte			haipai_m2_bflag;					// 配牌遷移状態
	private byte			haipai_m2_iOrder;					// 親家保持
	private byte			haipai_m2_iThNow;					// 家に配られている配数
	private byte			haipai_m2_iBipai;					// 卓上の配数

	private byte			before_richi_m4_bflag;
	private byte			after_richi_m4_bflag;

	private byte			ronho_m5_bflag;
#if	Rule_2P
	private byte			ronho_m5_iOrder= 0;
#else
	private byte			ronho_m5_iOrder;
#endif

	private byte			naki_m6_iNakiMode;
	private byte			naki_m6_iOrder;
	private byte			naki_m6_bflag;
	private byte			keyin_m6_bflag;
	private byte			ankan_m6_bflag;
	private byte			ankan_m6_fKan;
	private byte			chakan_m6_bflag;
	private byte			chakan_m6_ucPai;
	private int				chakan_m6_iFrhai;

	private byte			Optcnt;								/* メニュー数			*/
	private byte[]			Optstk = new byte [MJDefine.D_OPT_STK_MAX];	/* メニュー内容			*/
	private byte[]			Optno = new byte [4];				/*005E*/
	private byte			Optcsr;								/* メニューカーソル		*/
	private byte[]			opt_game = new byte [7];

	private byte[]			player_pon_flg = new byte [4];
	private byte			event_type;
	private byte			event_par;

	private MJK_RESULT[]	gMJKResultSub = new MJK_RESULT [3];		/*	局の結果	*/
	private byte[]			result_order= new byte [3];
	private MNTDATA[]		sMntDataSub = new MNTDATA [3];				/*	面子データ	*/

	private byte[]			Ricbuf = new byte [14];					/* リーチする際の待ちの数	*/
	private byte[]			Kanpos = new byte [3];
	private byte[]			Kanflg = new byte [3];
	private byte			NakiSelNo;
	private bool			Sute_flush_f;								/* 点滅実行フラグ */
	private byte			Sute_flush_no;								/* 点滅させるプレーヤー番号（０～３） */
	private byte			Sute_flush_pos;								/* 点滅させる場所	0:最後に捨てた捨て牌 1:最後にカンした牌 */

	/*** 搶カン管理 ***/
	private byte			Rec_chankan_man;							/* 搶カンされた人 */
	private byte			Rec_chankan_hai;							/* 搶カンされた牌 */
	private byte			Rec_chankan_no;								/* 搶カン牌のフーロ番号(0-3) */
	private ushort[]		Rec_minkan_pri = new ushort [MJDefine.MAX_TABLE_MEMBER];		/* カン牌プライオリティ履歴 */

	/*** 捨て牌管理 ***/
	private byte[,]			Rec_sute_pos = new byte [MJDefine.MAX_TABLE_MEMBER, MJDefine.SUTEHAIMAX];	/* 捨て牌表示管理バッファ */
	private bool[]			Rec_sute_reach = new bool [MJDefine.MAX_TABLE_MEMBER];			/* 立直捨て牌表示フラグ */

	public SubMj_Tbl		SubMj;		//*SubMj;		// 麻雀ロジック部分グローバル
														// 静的確保ではなく動的に変更
	private bool			tsumo_agari;				// ツモ和了フラグ 真:ツモ 偽:ロン	2006/02/24 要望No.113

	/**********		ここから麻雀用宣言　おわり		**********/
	[Header("元ソース")]

//0430mt
//0518mt	MjWeb			stWeb; 		//*stWeb; 					// Webから画像をダウンロードするための構造体
	public byte[]		 stWeb_pszRecv = new byte [2048];
	public long 		 stWeb_pszRecvLen;
	public int 		 stWeb_sStatus;
	public char[] 	 stWeb_byRequestNoStack = new char [4];
	public long      stWeb_sShowMessageTime;
	public char[]     stWeb_sMessagePhaseQueue = new char [(10)];
	public ulong		faceimageflags; 						 // サーバー顔画像の所持フラグ
	public ulong		lasterror;								 // エラーコード

	public bool			is_entry ;								 // エントリーサーバー接続フラグ
	public bool			is_game_result_wait;					 // 終了結果待ちうけフラグ


	public bool			is_reentry; 							 // 再接続フラグ
	public byte			reentry_try_count;						 // 再接続挑戦カウント
	public ulong		reentry_time;							 // 再接続間隔タイマー

	static CmdHistory_[] 	stReEntry_mycommand = new CmdHistory_ [136*3];
	static short		stReEntry_mycommand_count;
	static short		stReEntry_mycommand_count_max;
	static short		stReEntry_mymatch_id;

	public ulong		pre_uptimems;							 // GETUPTIMEMSを前に実行した時間
	public ulong		uptimems_perfrm;						 // GETUPTIMEMSで計った１フレームあたりの時間

	public ulong		version;								 // バージョンが違うと接続出来なくなります

	public byte			no_use_suvival_save_data;

	public short naki_wait_base;
	public short naki_wait_param;
	public short tsumo_wait_base;
	public short tsumo_wait_param;
#if true //-*todo:通信用だから不要？
	// public short result_wait_base= D_NEXT_WAIT_TIMEOUTS;
	public short result_wait_base= ((3*1000)/ 2);
#endif //-*todo:
	public short draw_com_character;	// マッチング画面でのCOMの描画フラグ
	public short is_ronchk_menu_open; // ロンチェックでメニューが開いたかどうか？

	public long  send_kyoku_ready_report_wait; // 局情報送信待ち
	public short  mythink_select_ok_flag;	   // 自分の思考方法選択変更許可フラグ
	public short  mypinch_hitter_select_ok_flag;// 切断時思考フラグ

	public short agari_end_check_wnd_on;

	public byte[]	 select_opt= new byte [(int)D_SELECT_OPT.MAX];	// 0 : 通信時操作 1: 鳴き無し
	public byte     hide_hkhai;

#if _DEBUG
	//// デバッグ用なので、後で消します
	//_static ulong		send_sthai_count;
	//_static ulong		send_sthaiact_count;
	//_static ulong		recv_sthai_count;
	//_static ulong		recv_sthaiact_count;

	_static ulong total_send_size;
	_static ulong total_recv_size;
	_static char	 dbgaddr[]= new char [32];
	_static short dbgport;
#endif
#if _DEVELOP
	_static short dbgon;
	_static short dbgselect;
	_static short dbgopt[]= new short [8];	// 0: 牌をあける 1:デバッグ情報表示(0表示しない)
#endif
	private int				errCode;

	private byte[]  is_yaoch = new byte [MJDefine.MAX_TABLE_MEMBER];		// 流し満貫フラグ
	private short[] yaoch_point = new short [MJDefine.MAX_TABLE_MEMBER];	// 流し満貫得点

#endregion //-*MJRALLY
#region MAHJONG_J
	//--------------------------------------------------
	// abstruct: 開始設定（起動時のみ呼ばれる）
	//--------------------------------------------------

	// ローカル変数 //0505mt test
	public static byte[]	Paitbl = {	//[ MAX_PAI_KIND ]
	0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
	0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19,
	0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29,
	0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0
	};
#endregion	//-*MAHJONG_J
	// Use this for initialization
	protected override void Start () {
		DebLog("//-*MJ_start");
		a_Mode = ModeSet(MAHJONGMODE.mMODE_READY);
		StartCoroutine("UpdateMahjong");
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
	}
	private void GameStart()
	{

#region MAHJONG_J
//-*****todo
		int	i, j;

		//-*ready(true);		//準備中
		ready = false;
		m_keepData.flag_res_battle = -1;	//-*勝敗フラグ初期化
		//--------------------------------------------------
		//変数
	#if false //-*todo
		pictMax= 1;		//ALL_PICT;		//画像オブジェクト数
		pict= new Image[pictMax];		//画像オブジェクト
	#endif
		//--------------------------------------------------
#if	_APP_MAIN	//{
		SubMj= new SubMj_Tbl();
//0518mt		stWeb    = new MjWeb();

		sGameData= new GAMEDATA();
		play_wk= new PLAYERWORK();
		play_wk.sParamData= new PARA();
		for( i= 0; i< MJDefine.MAX_TABLE_MEMBER; i++) {
			gsPlayerWork[i]= new PLAYERWORK();
			gsPlayerWork[i].sParamData= new PARA();
		}
		gpsPlayerWork= new PLAYERWORK();
		gpsPlayerWork.sParamData= new PARA();
		sTmpPlayerWork= new PLAYERWORK();
		sTmpPlayerWork.sParamData= new PARA();

		for( i= 0; i< 14; i++)
			SubMj.sDangerDat[i]= new DANGERDAT();

		SubMj.gYaku= new YAKU();
		for( i= 0; i< 3; i++) {
			gsTableData[i]= new TABLEDATA();
			gsTableData[i].psParam= new PARA();
			for( j= 0; j< MJDefine.MAX_TABLE_MEMBER; j++)
				gsTableData[i].sMemData[j]= new TABLEMEM();
		}
		gpsTableData= new TABLEDATA();
		gpsTableData.psParam= new PARA();
		for( j= 0; j< MJDefine.MAX_TABLE_MEMBER; j++)
			gpsTableData.sMemData[j]= new TABLEMEM();
		#if false //-*todo:
		f_info= new MJFileInfo();
		f_info.f_network= new FileNetWork();
		f_info.f_standalone= new FileStandAlone();
		f_info.f_optionsetting= new FileOptionSetting();
		f_info.f_survival_kyoku_data= new SurvivalKyokuData();
		f_info.f_survival_kyoku_data.tabledata= new TABLEDATA();
		f_info.f_survival_kyoku_data.tabledata.psParam= new PARA();
		for( j= 0; j< MAX_TABLE_MEMBER; j++)
			f_info.f_survival_kyoku_data.tabledata.sMemData[j]= new TABLEMEM();
		#endif //-*todo:
		sRuleSubData= new RULESUBDATA();

		for( i= 0; i< 15; i++)
		{
			#if true //-*todo:
			// gPaival[i]= new PAIVAL();
			SubMj.gPaival[i]= new PAIVAL();
			#endif //-*todo:
		}
		#if true //-*todo:
		// sp= new PAIVAL();
		SubMj.sp= new PAIVAL();
		#endif //-*todo:

		gMJKResult= new MJK_RESULT();
		for( i= 0; i< MJDefine.MAX_TABLE_MEMBER; i++)
			gMJKResult.sMemResult[i]= new MEM_RESULT();
		for( i= 0; i< MJDefine.MAX_YAKU; i++)
			gMJKResult.sYaku[i]= new YAKU_T();

		for( i= 0; i< 3; i++)
			gMJKResultSub[i]= new MJK_RESULT();

		sMntData= new MNTDATA();
		for( i= 0; i< 3; i++)
			sMntDataSub[i]= new MNTDATA();
		#if false //-*todo:
		sSurvivalData= new RESULT_SV();
		#endif //-*todo:

		for( i= 0; i< MJDefine.MAX_TABLE_MEMBER; i++) {	//4
			#if false //-*todo:通信相手関連だから要らない？
			gNetTable_NetChar[i]= new CHAR_INFO_TBL();
			gNetTable_NetChar[i].spriteChar= new SpriteInfo();
			#endif
		}

#if false //-*todo:キー操作
		KeyInfo= new KEYINFO();

		appSystem= new APPSYSTEM();
#endif //-*todo:キー操作

#if false //-*todo:描画
#if	Rule_2P
		spPaiJicya= new SpriteInfo();			//m_spPaiJicya
		spSutePaiJicya= new SpriteInfo();		//m_spSutePaiJicya
		spSutePaiShimocya= new SpriteInfo();	//m_spSutePaiShimocya
		spSutePaiToicya= new SpriteInfo();		//m_spSutePaiToicya
#else
		for( i= 0; i< D_SPRITE_PAI_COUNT; i++) {
			spPaiJicyaSmall[i]= new SpriteInfo();
			spPaiKamicya[i]= new SpriteInfo();
			spPaiToicya[i]= new SpriteInfo();
			spPaiShimocya[i]= new SpriteInfo();
			spSutePaiJicya[i]= new SpriteInfo();
			spSutePaiShimocya[i]= new SpriteInfo();
			spSutePaiToicya[i]= new SpriteInfo();
			spSutePaiKamicya[i]= new SpriteInfo();
		}
#endif

		for( i= 0; i< MAX_POPULAR_COUNT; i++)
			m_spPopular[i]= new SpriteInfo();

		m_spSurvival= new SpriteInfo();
		m_spFloorBG= new SpriteInfo();
		for( i= 0; i< D_MAX_BG_COUNT; i++)
			m_spMainBG[i]= new SpriteInfo();
		for( i= 0; i< D_GAME_MAX; i++)
			m_spGame[i]= new SpriteInfo();
		for( i= 0; i< MJ_CHARACTER_MAX_COUNT; i++)
			m_spChar[i]= new SpriteInfo();
		for( i= 0; i< YK_MAX; i++)		//55
			m_spYakuBG[i]= new SpriteInfo();
		for( i= 0; i< 20; i++)
			m_sprite[i]= new SpriteInfo();
		m_option= new SpriteInfo();
		for( i= 0; i< D_COUNT_DOWN_MAX; i++)
			m_spCount[i]= new SpriteInfo();
		for( i= 0; i< D_MODE_MAX_COUNT; i++)
			m_spMode[i]= new SpriteInfo();
		for( i= 0; i< D_MAX_MARK_COUNT; i++)
			m_spMark[i]= new SpriteInfo();
#endif //-*todo:描画

		for( i = 0; i < 136*3; i++)
			stReEntry_mycommand[i]= new CmdHistory_();

		for( i= 0; i< 15; i++) {
			ttnpsub_pbuf[i] = new PLST();
			evlish_etnp_pbuf[i]= new PLST();
			jyaku_jy_pailist[i] = new PLST();
			chktnp_m5_pailist[i] = new PLST();
			ankan_think_pailist[i] = new PLST();
			think_think_pailist[i] = new PLST();
			simfuro_pailist[i] = new PLST();
			tminkan_pailist[i] = new PLST();
			tfuro_think_pailist[i] = new PLST();
		}

		ankan_think_pailist_sBuf = new PLAYERWORK();
		ankan_think_pailist_sBuf.sParamData= new PARA();

		tminkan_sBuf = new PLAYERWORK();
		tminkan_sBuf.sParamData= new PARA();
#endif
		//--------------------------------------------------
	#if false //-*todo
		m_PauseTime=0;//0516mt
		gc();		//ガーベッジ
	#endif

		//ストレージのデータを読み込む

#if	SpriteUSE
		canvas.loadSprite(SPRITE_DATA);
		canvas.initSprite(false, false, 0, dataLoad20);
#endif
#if DEBUG
	#if false //-*todo
		Game.testStr1= "data read OK";
	#endif
#endif

		// サウンド
#if	NOSOUND
		jinit(dataSef00);
#endif
	#if false //-*todo
		Runtime info= Runtime.getRuntime();
		_totalMemory= info.totalMemory();		//最大メモリ

		gc();
	#endif
#if DEBUG
	#if false //-*todo
		Game.testStr1= "init OK";
//		Runtime info= Runtime.getRuntime();
		Game.testStr2= Long.toString(info.freeMemory())+ "/"+ Long.toString(_totalMemory);
Debug.mem("mem");
	#endif
#endif

//-*********
#endregion //-*MAHJONG_J
	}

#region APP_SYSTEM_J
	/// <summary>
	/// 初期化処理関数
	/// </summary>
	public bool MahJongRally_InitAppData(/*MahJongRally *pMe*/)
	{

	#if HEAP_CHECK
		mallocated = 0;
		peak_mallocated = 0;
	#endif // HEAP_CHECK
		//アプリケーション初期化
		if( MahJongRally_InitApp() == false) {
			return false;
		}

		#if false	//-*todo:
		modeChange(D_LOGO_MODE);
		#endif
		//データ初期化
		MahJongRally_InitData();

		return true;
	}
#endregion	//-*APP_SYSTEM_J

#region MJ_INIT_J
/*===========================================================================

	アプリケーション初期化処理関数

===========================================================================*/
public bool MahJongRally_InitApp(/*MahJongRally * pMe*/)
{
//xxxx	AEEDeviceInfo di;
	int i=0;
	int state		= 0;
	short	x	= 0;	//
	short	y	= 0;	//
	short	cx	= 0;	//
	short	cy	= 0;	//

#if false	//-*todo:
	//実機画面サイズ取得
//xxxx	ISHELL_GetDeviceInfo(GETMYSHELL(),&di);
	//画面サイズをインスタンスへ設定
	m_screenWidth	= 240;		//= di.cxScreen;
	m_screenHeight = 280;		//= di.cyScreen;
	app_main_rect.x = 0;
	app_main_rect.y = 0;
	app_main_rect.dx = m_screenWidth;
	app_main_rect.dy = m_screenHeight;

	/*========================
			牌以外のロード
	==========================*/
	{	// 役表示文字列のロード。
		int resNo = 0;

		// 数値文字列のロード。
		for( i=0 ; i<SYSNUMBER_RES_COUNT ; i++ ) {
			resNo = NumberTbl[i];
			ResAddTextBuff( (short)resNo, NumberText[i], sizeof(NumberText[i]) );		// 文字列のロード
		}

		for( i=0 ; i<RULE_MAX_RES_COUNT ; i++ ) {
			resNo = g_RuleResIdTbl[i];
			ResAddTextBuff( (short)resNo, RuleDispText[i], sizeof(RuleDispText[i]) );		// 文字列のロード
		}

		// フリー対戦設定用文字列のロード。
		for( i=0 ; i<FREE_BATTLE_RES_CONT ; i++ ) {
			resNo = g_freeBattleResIdTbl[i];
			ResAddTextBuff( (short)resNo, FreeBattleText[i], sizeof(FreeBattleText[i]) );	// 文字列のロード
		}

		// 自由ルール設定文字列格納。
		for( i=0 ; i<FREE_MODE_RES_COUNT ; i++ ) {
			resNo = g_FreeResIdTbl[i];
			ResAddTextBuff( (short)resNo, FreeModeText[i], sizeof(FreeModeText[i]) );		// 文字列の格納。
		}

		for( i=0 ; i<OPTION_WORD_RES_COUNT ; i++ ) {
			resNo = g_optionResidTbl[i];
			ResAddTextBuff( (short)resNo, OptionWordText[i], sizeof(OptionWordText[i]) );		// 文字列の格納。
		}
	}

	/*========================
			自牌のロード (hai_L.png → moema_majan01.png → ukima_majan01.png)//*ウキウキ
	==========================*/
	{	// 自家イメージデータ読み出し
		pIPaiJicya= _MJ_LoadResImage( pIPaiJicya, PAI_JICYA_LARGE );	//MJ_LoadResImage( pIPaiJicya, PAI_JICYA_LARGE );

#if	Rule_2P
		_MpsBrewLib_SetSpriteInfo( spPaiJicya, pIPaiJicya, 0, 0, 288, 58);
#else
		for( i=0 ; i<35 ; i++ ) {
			state = gPaiData[i];
//			MpsBrewLib_SetSpriteInfo( spPaiJicya[state], pIPaiJicya,0+(i*D_PAI_TATE_LARGE_SIZE_W),0,D_PAI_TATE_LARGE_SIZE_W,D_PAI_TATE_LARGE_SIZE_H);
			MpsBrewLib_SetSpriteInfo( spPaiJicya[state], pIPaiJicya,
				0+((i%18)*D_PAI_TATE_LARGE_SIZE_W),			//0+((i%18)*15),
				(i/18)*D_PAI_TATE_LARGE_SIZE_H,		//0,	//(i/18)*29,
				D_PAI_TATE_LARGE_SIZE_W,					//15
				D_PAI_TATE_LARGE_SIZE_H);					//24
		}
#endif
	}
#if	Rule_2P
#else
	/*========================
		小さい自牌のロード (hai_S1.png)
	==========================*/
	{	// 自家イメージデータ読み出し
		pIPaiJicyaSmall= _MJ_LoadResImage( pIPaiJicyaSmall, PAI_JICYA_SMALL );	//MJ_LoadResImage( pIPaiJicyaSmall, PAI_JICYA_SMALL );

		for( i=0 ; i<35 ; i++ ) {
			state = gPaiData[i];
			MpsBrewLib_SetSpriteInfo( spPaiJicyaSmall[state], pIPaiJicyaSmall,
									   0+(i*D_PAI_TATE_SMALL_SIZE_W),
									   0,
									   D_PAI_TATE_SMALL_SIZE_W,
									   D_PAI_TATE_SMALL_SIZE_H);
		}
	}
	/*========================
			上家牌のロード (hai_S2.png)
	==========================*/
	{	// 上家イメージデータ読み出し
		pIPaiKamicya= _MJ_LoadResImage( pIPaiKamicya, PAI_KAMICYA_SMALL );	//MJ_LoadResImage( pIPaiKamicya, PAI_KAMICYA_SMALL );

		// 牌のテーブル概念化
		for( i=0 ; i<35 ; i++ ) {
			state = gPaiData[i];
			MpsBrewLib_SetSpriteInfo( spPaiKamicya[state], pIPaiKamicya,	0+(i*D_PAI_YOKO_SMALL_SIZE_W),
																			0,
																			D_PAI_YOKO_SMALL_SIZE_W,
																			D_PAI_YOKO_SMALL_SIZE_H);
		}
	}
	/*========================
			対面牌のロード (hai_S3.png)
	==========================*/
	{	// 対家イメージデータ読み出し
		pIPaiToicya= _MJ_LoadResImage( pIPaiToicya, PAI_TOICYA_SMALL );	//MJ_LoadResImage( pIPaiToicya, PAI_TOICYA_SMALL );

		// 牌のテーブル概念化
		for( i=0 ; i<35 ; i++ ) {
			state = gPaiData[i];
			MpsBrewLib_SetSpriteInfo( spPaiToicya[state], pIPaiToicya, 0+(i*D_PAI_TATE_SMALL_SIZE_W),
																		0,
																		D_PAI_TATE_SMALL_SIZE_W,
																		D_PAI_TATE_SMALL_SIZE_H);
		}
	}
	/*========================
			下家牌のロード (hai_S4.png)
	==========================*/
	{	// 下家イメージデータ読み出し
		pIPaiShimocya= _MJ_LoadResImage( pIPaiShimocya, PAI_SHIMOCYA_SMALL );	//MJ_LoadResImage( pIPaiShimocya, PAI_SHIMOCYA_SMALL );

		// 牌のテーブル概念化
		for( i=0 ; i<35 ; i++ ) {
			state = gPaiData[i];
			MpsBrewLib_SetSpriteInfo( spPaiShimocya[state], pIPaiShimocya, 0+(i*D_PAI_YOKO_SMALL_SIZE_W),
																			0,
																			D_PAI_YOKO_SMALL_SIZE_W,
																			D_PAI_YOKO_SMALL_SIZE_H);
		}
	}
#endif
	/****************************************************************/
	/*					  捨て牌用リソースロード。					*/
	/****************************************************************/
	// case 自家。(Hai_L1 → moema_majan02 → ukima_majan02)	//*ウキウキ
	{	// 自家イメージデータ読み出し
		pISutePaiJicya= _MJ_LoadResImage( pISutePaiJicya, SUTEPAI_JICYA );	//MJ_LoadResImage( pISutePaiJicya, SUTEPAI_JICYA );

#if	Rule_2P
		_MpsBrewLib_SetSpriteInfo( spSutePaiJicya, pISutePaiJicya, 0, 0, 252, 50);
#else
		// 牌のテーブル概念化
		for( i=0 ; i<35 ; i++ ) {
			state = gPaiData[i];
//			MpsBrewLib_SetSpriteInfo( spSutePaiJicya[state], pISutePaiJicya, 0+(i*15),0,15,24);
			MpsBrewLib_SetSpriteInfo( spSutePaiJicya[state], pISutePaiJicya,
				0+((i%18)*14),			//0+((i%18)*15),
				(i/18)*25,		//0,	//(i/18)*25,
				14,						//15,
				25);					//24;
		}
#endif
	}
	// case 下家。(Hai_L2 → moema_majan03 → ukima_majan03)
	{	// 下家イメージデータ読み出し
		pISutePaiShimocya= _MJ_LoadResImage( pISutePaiShimocya, SUTEPAI_SHIMOCYA );	//MJ_LoadResImage( pISutePaiShimocya, SUTEPAI_SHIMOCYA );

#if	Rule_2P
	#if SOFTBANK // 080718 // 110513:After麻雀から#ifdef文移植
		_MpsBrewLib_SetSpriteInfo( spSutePaiShimocya, pISutePaiShimocya, 0, 0, 324, 40);
	#else // DOCOMO:牌2段→4段対応
		_MpsBrewLib_SetSpriteInfo( spSutePaiShimocya, pISutePaiShimocya, 0, 0, 162, 80);
	#endif
#else
		// 牌のテーブル概念化
		for( i=0 ; i<35 ; i++ ) {
			state = gPaiData[i];
//			MpsBrewLib_SetSpriteInfo( spSutePaiShimocya[state], pISutePaiShimocya, 0+(i*19),0,19,20);
			MpsBrewLib_SetSpriteInfo( spSutePaiShimocya[state], pISutePaiShimocya,
				0+((i%18)*18),			//0+((i%18)*19),
				(i/18)*20,		//0,
				18,						//19,
				20);
		}
#endif
	}
	// case 対家。(Hai_L3 → moema_majan04 → ukima_majan04)	//*ウキウキ
	{	// 対家イメージデータ読み出し
		pISutePaiToicya= _MJ_LoadResImage( pISutePaiToicya, SUTEPAI_TOICYA );	//MJ_LoadResImage( pISutePaiToicya, SUTEPAI_TOICYA );

#if	Rule_2P
		_MpsBrewLib_SetSpriteInfo( spSutePaiToicya, pISutePaiToicya, 0, 0, 252, 50);
#else
		// 牌のテーブル概念化
		for( i=0 ; i<35 ; i++ ) {
			state = gPaiData[i];
//			MpsBrewLib_SetSpriteInfo( spSutePaiToicya[state], pISutePaiToicya, 0+(i*15),0,15,24);
			MpsBrewLib_SetSpriteInfo( spSutePaiToicya[state], pISutePaiToicya,
				0+((i%18)*14),			//0+((i%18)*15),
				(i/18)*25,		//0,	//(i/18)*24,
				14,						//15,
				25);					//24;
		}
#endif
	}
#if	Rule_2P
#else
	// case 上家。
	{	// 対家イメージデータ読み出し
		pISutePaiKamicya= _MJ_LoadResImage( pISutePaiKamicya, SUTEPAI_KAMICYA );	//MJ_LoadResImage( pISutePaiKamicya, SUTEPAI_KAMICYA );

		// 牌のテーブル概念化
		for( i=0 ; i<35 ; i++ ) {
			state = gPaiData[i];
//			MpsBrewLib_SetSpriteInfo( spSutePaiKamicya[state], pISutePaiKamicya, 0+(i*19),0,19,20);
			MpsBrewLib_SetSpriteInfo( spSutePaiKamicya[state], pISutePaiKamicya,
				0+((i%18)*19),
				(i/18)*20,		//0,
				19,
				20);
		}
	}
#endif
#endif//-*todo:

#if false //-*todo
	// エントリーサーバー接続フラグ初期化
	is_entry = false;


	// 再接続情報のロード。
	if( MJ_ReadFile( D_WORD_INFOFILE, f_info) == TRUE )
		// 隠しキャラフラグのみは最初に更新しておく。
		kakushi_char = (BYTE)f_info.f_standalone.m_kakushi_char;

	//> バグNo.123 オプションの初期化 2006/02/22
	else {
		initData= true;
		MJ_FileCreate();		//初期値のセット
		// ファイルの読み込みに失敗した場合は初期値でファイルに書き込む
		InitUserRule(  );
//		{
//			uint8_t	i;
			for ( i = 0 ; i < RLSIZE ; i++ )
				f_info.f_rule[i] = gbyUserRule[i];
//		}

		if( MJ_WriteFile( D_WORD_INFOFILE, f_info) == FALSE ) {
			// 失敗したら、初期化失敗へ
			ITRACE(( "MJ_WriteFile default set failed" ));
			//> 2006/02/27 EFSチェック
//			return false;
			return true;
			//> 2006/02/27 EFSチェック
		}
	}
#endif//-*todo
	// ユーザールールが破壊されていないかチェックし、破壊されていたら修正
	checkUserRule();		//0422
#if false//-*todo:
	OptionSetData( MJDefine.D_OPTION_FLG_CHG );		//OptionSetData( D_OPTION_FLG_SET );
	// サバイバル勝利情報は通信時に送る必要があるので、ここでロード
	survival_win_count = (BYTE)f_info.f_standalone.m_SurvivalWinCount;
	kakushi_char = (BYTE)f_info.f_standalone.m_kakushi_char;
#endif
#if false//-*todo:
	if( f_info.f_survival_kyoku_data.byKyokuSave == 1 )
		if( f_info.f_survival_kyoku_data.tabledata.check() == false )
			// 局データが壊れていたら参照しない
			f_info.f_survival_kyoku_data.byKyokuSave = 0;
#endif


	//> バグNo.123 オプションの初期化 2006/02/22
	return true;
}
/*===========================================================================

	データ初期化処理関数

===========================================================================*/
public void MahJongRally_InitData(/*MahJongRally * pMe*/)
{
#if false//-*todo
	// ロゴのタイムアウト値設定。
	gLogoTimeOut = D_LOGO_WAIT_TIMEOUT;
	pre_uptimems = GETUPTIMEMS();

	//モード初期化
#endif 
}

#endregion //-*MJ_INIT_J



	private IEnumerator UpdateMahjong(){
		while(true){
#region KEY_TEST
			while(m_keepData.IsOptionOpen){
				yield return null;
			}
#endregion //-*KEY_TEST

			switch(a_Mode){
			case MAHJONGMODE.mMODE_READY:
				GameStart();
				a_Mode = ModeSet(MAHJONGMODE.mMODE_INIT);
				break;
			case MAHJONGMODE.mMODE_INIT:
#region UNITY_ORIGINAL
				//-*開始時一度だけの処理
				InitMakeHandTiles();
				InitNextPageBtn();
				//-*局ごとのリセット
				ResetDiscardedTiles();
				ResetFuroTiles();
				ResetWanTiles();
				ResetCallMenu();
				RestRebo();
				RestCallDraw();
				RestCallRyukyokuDraw();
				RestResultBoxDraw();
				RestResultPointDraw();
#endregion //-*UNITY_ORIGINAL
				MahJongRally_InitAppData();
				a_Mode = ModeSet(MAHJONGMODE.mMODE_GAME);
				#if true //-*todo:Appjから移植(app_main内)
				game_player = 0;
				sGameData.byGameMode = MJDefine.GAMEMODE_FREE;				/*	フリーゲーム		*/
				//ルール選択初期化へ移行
				mode = INMJMODE.D_FREE_RULE_MODE;
				// m_Mode = INMJMODE.D_FREE_RULE_MODE;
				modeChange(mode);
				#endif //-*todo:Appjから移植
				break;
			case MAHJONGMODE.mMODE_GAME:
				try {
					#if	_APP_MAIN
					app_main();
					//0511mt m_TerminalCount += INTERVAL;//0501mt
					#endif
				} catch (Exception e) {
					#if DEBUG
					//	Runtime info= Runtime.getRuntime();
					//	Debug.mem("mem");
					Debug.Log("app_main error:"+e);
					#if	_APP_MAIN
					Debug.Log( "app_main:" + e );
					#endif
					// e.printStackTrace();
					// __HALT;		/*HALT*/
					#endif
				}
				#if false //-*todo:不要？
				//空きが80%で gc()
				if( info.freeMemory()< (_totalMemory/10 * 2)) {
					#if DEBUG
					Debug.out("gc();");
					/*	Debug.mem("mem");	*/
					#endif
					gc();		//Main ループ
				}
				#endif //-*todo:不要？
				#if true //-*todo:描画
				if( paintF)		paintE= true;		//ゲームメイン描画終了フラグ
				#endif //-*todo:描画

				break;
			case MAHJONGMODE.mMODE_EXIT:
			default:
				if(m_keepData.IsMjChallenge){
				//-*チャレンジモード
					SceneChange("SelectStage");
				}else{
				//-*シナリオモード
					SceneChange("Adventure");
				}
				yield break;
			}
			yield return null;
		}

	}
#region MJWRULE_J
//#include "MahJongRally.h"

public void SetRuleData(/*MahJongRally * pMe,*/ byte byGameID)
{
	byte	byRuleType;
	byte[]	byRuleTable	=	{
		0x01, 0x02, 0x04, 0x80, 0x40, 0x01, 0xFF
	};

	sRuleSubData.byChipRate	=	5;
	byRuleType	=	byRuleTable[byGameID];
}
#endregion //-*MJWRULE_J
#region GAMEDEFS_H
	// #define	IsGameClear()			((sGameData.byFlags & GAMEFLAG_CLEAR) != 0)
	public bool IsGameClear(){	return ((sGameData.byFlags & (byte)GAMEFLAG.CLEAR) != 0);}
	public bool IsGameAutoPlay(){
		return	((sGameData.byFlags & (byte)GAMEFLAG.AUTO) != 0);
	}
#if false //-*todo:必要になったら考える
	// #define	SetGameClear()			(sGameData.byFlags |= GAMEFLAG_CLEAR)

	// #ifdef	_MONITOR_CODE
	// #define	IsGameDebugMode()		(TRUE)
	// #else
	// #define	IsGameDebugMode()		((sGameData.byFlags & GAMEFLAG_DEBUG) != 0)
	// #endif
	// #define	SetGameDebugMode()		(sGameData.byFlags |= GAMEFLAG_DEBUG)

	// #define	SetGameAutoPlay()		(sGameData.byFlags |= GAMEFLAG_AUTO)
#endif
#endregion //-*GAMEDEFS_H

#region MJ_DEFS_H
	public bool IsGuideMode(){ return (sGameData.byGuide != MJDefine.NONE);}
#endregion //-*MJ_DEFS_H


#region UNITY_ORIGINAL
	private void InitMakeHandTiles(){

		GameObject obj = (GameObject)Resources.Load("Prefabs/TileBaseHand");

		// m_StageButton = new GameObject[STAGEMAXNUM_TEST];
        // プレハブを元にオブジェクトを生成する(偶数自分:奇数相手)
		for(int i = 0;i < MJDefine.HAND_NUM_MAX*MJDefine.MEMBER_NUM_MAX;i++){
			int assort = (i%MJDefine.MEMBER_NUM_MAX);//-*(偶数自分:奇数相手)
			int handNo = (i/MJDefine.MEMBER_NUM_MAX);//-*番号
			GameObject instantObj = (GameObject)Instantiate(obj,
						Vector3.zero,
						Quaternion.identity);
			if(instantObj == null){
				DebLogError("//-*Make TileBase Err:("+assort+")No."+handNo);
				return;
			}
			instantObj.name = "HandTile"+String.Format("{0:D2}", handNo);
			var mjTile = new MJTIle();
			
			switch(assort){
			case 0:
				//-*自分
				instantObj.transform.SetParent(m_myHandTilesBase.transform, false);
				m_myHandTiles.Add(instantObj);
				mjTile = m_myHandTiles[handNo].GetComponent<MJTIle>();
				if(mjTile == null){
					DebLogError("//-*MJTIle noto found :AllNo."+i+", assort:"+assort+", HandNo:"+handNo);
					break;
				}
				mjTile.InitTileHand(assort,handNo,true);
				mjTile.SetOnPointerClickCallback(ButtonSelectTile);
				break;
			case 1:
				//-*相手
				instantObj.transform.SetParent(m_yourHandTilesBase.transform, false);
				m_yourHandTiles.Add(instantObj);
				mjTile = m_yourHandTiles[handNo].GetComponent<MJTIle>();
				if(mjTile == null){
					DebLogError("//-*MJTIle noto found :AllNo."+i+", assort:"+assort+", HandNo:"+handNo);
					break;
				}
				mjTile.InitTileHand(assort,handNo);
				break;
			default:
				DebLogError("//-*人数("+MJDefine.MEMBER_NUM_MAX+")に合わせてcase式増設の必要("+assort+"):No."+handNo);
				break;
			}
		

    	}

	}
	/// <summary>
	/// ヘッダーフッターの初期化
	/// </summary>
	private void InitHeaderFotter()
	{
		m_HeaderFooterBase.InitHF();
	}

	/// <summary>
	/// 次へボタンの初期化
	/// </summary>
	private void InitNextPageBtn()
	{
		//-*ページ送りボタン
		m_nextButton.GetComponent<ButtonCtl>().SetOnPointerClickCallback(ButtonGoToNext);
		SetNextBtnActive(false);
	}

	/// <summary>
	/// 捨て牌の初期化
	/// </summary>
	private void ResetDiscardedTiles()
	{
		//-*自分
		if(m_myDiscardedTiles != null)
		{
			foreach(var tile in m_myDiscardedTiles){
				UnityEngine.Object.Destroy(tile);
			}
			m_myDiscardedTiles.Clear();
			m_myDiscardedTiles = null;
		}
		m_myDiscardedTiles = new List<GameObject>();
		
		//-*相手
		if(m_yourDiscardedTiles != null)
		{
			foreach(var tile in m_yourDiscardedTiles){
				UnityEngine.Object.Destroy(tile);
			}
			m_yourDiscardedTiles.Clear();
			m_yourDiscardedTiles = null;
		}
		m_yourDiscardedTiles = new List<GameObject>();
	}
	/// <summary>
	/// 副露牌の初期化
	/// </summary>
	private void ResetFuroTiles()
	{
		//-*自分
		if(m_myFuroTilesTiles != null)
		{
			foreach(var furo in m_myFuroTilesTiles){
				UnityEngine.Object.Destroy(furo);
			}
			m_myFuroTilesTiles.Clear();
			m_myFuroTilesTiles = null;
		}
		m_myFuroTilesTiles = new List<GameObject>();

		//-*相手
		if(m_yourFuroTilesTiles != null)
		{
			foreach(var furo in m_yourFuroTilesTiles){
				UnityEngine.Object.Destroy(furo);
			}
			m_yourFuroTilesTiles.Clear();
			m_yourFuroTilesTiles = null;
		}
		m_yourFuroTilesTiles = new List<GameObject>();
	}

	/// <summary>
	/// 王牌の初期化
	/// </summary>
	private void ResetWanTiles(){
		if(m_WanTilesBase == null)return;
		m_WanTilesBase.GetComponent<SetWanpai>().Init();
	}

	/// <summary>
	/// リー棒表示初期化
	/// </summary>
	private void RestRebo()
	{
		for(int a=0;a<m_ribos.Count;a++){
		//-*リー棒表示消し
			DrawRibo(a,false);
		}
	}
	/// <summary>
	/// 鳴き系メニューの初期化
	/// </summary>
	private void ResetCallMenu(){
		if(m_callMenus != null)
		{
			foreach(var call in m_callMenus){
				UnityEngine.Object.Destroy(call);
			}
			m_callMenus.Clear();
			m_callMenus = null;
		}
		m_callMenus = new List<GameObject>();
	}
	/// <summary>
	/// 鳴き宣言の描画消し
	/// </summary>
	private void RestCallDraw()
	{
		for(int a=0;a<m_callObjs.Count;a++){
			m_callObjs[a].InitCallDraw();
		}
	}
	/// <summary>
	/// 鳴き宣言の描画消し
	/// </summary>
	private void RestCallRyukyokuDraw()
	{
		m_callObjRyukyoku.InitCallDraw();
	}
	/// <summary>
	/// リザルトの描画消し
	/// </summary>
	private void RestResultBoxDraw()
	{
		var resultBox = m_resultBox.GetComponent<MJResultBox>();
		if(resultBox == null)return;
		resultBox.InitResult();
		// resultBox.SetOnPointerClickCallback(ButtonGoToNext);
		m_resultBox.SetActive(false);
	}
	/// <summary>
	/// リザルト精算画面の描画消し
	/// </summary>
	private void RestResultPointDraw()
	{
		var resultPontBox = m_resultPointBox.GetComponent<MJResultPointBox>();
		if(resultPontBox == null)return;
		int[] nowPoint = new int[]{gsTableData[0].sMemData[0].nOldPoint,gsTableData[0].sMemData[1].nOldPoint};
		resultPontBox.InitResultPoint(nowPoint);
		// resultBox.SetOnPointerClickCallback(ButtonGoToNext);
		m_resultPointBox.SetActive(false);
	}
	/// <summary>
	/// ヘッダーフッターの更新
	/// </summary>
	private void UpdateHeaderFotter()
	{
		int		Kyoku 	= ((gpsTableData.byKyoku/ 4) & 0x01);						// 局(東南)//-*西北)。
		int		KyokuCnt= ((gpsTableData.byKyoku) & 0x01);						// 局数。
		int		Renchan = (byte)(gpsTableData.byDrawRenchan % 100);	// 何本場か
		int		House 	= (( gpsTableData.byOya+ 4- game_player) % 4) & 0x01;
		int		Ribo	= (byte)(	gpsTableData.sMemData[0].byRibo_stack+
									gpsTableData.sMemData[1].byRibo_stack);		//場にあるリー棒
		int[]	Point = new int[MJDefine.MAX_TABLE_MEMBER2];		// 現在の点数

		for( int i = 0; i < MJDefine.MAX_TABLE_MEMBER2; i++ ) {
			int Seki = ( game_player+ i) % 4;
			Point[i]		= gsTableData[0].sMemData[Seki].nOldPoint;		// 現在の点数
		}
		m_HeaderFooterBase.UpdateHF(House,Kyoku,KyokuCnt,Renchan,Point,m_keepData.mah_limit_num,Ribo);
	}
	/// <summary>
	/// 次へボタンの点灯消灯
	/// </summary>
	private void SetNextBtnActive(bool active)
	{
		//-*ページ送りボタン
		m_nextButton.SetActive(active);
	}

	//-*****キー操作関連
	/// <summary>
	/// 捨て牌選択
	/// </summary>
	public void ButtonSelectTile(int a)
	{
		DebLog("//-*ButtonSelectTile:"+a);
		m_haiCsrNo = (short)a;
		m_haiSelF = true;
	}

	/// <summary>
	/// 鳴き系メニュー選択
	/// </summary>
	public void ButtonSelectCall(int a)
	{
		DebLog("//-*ButtonSelectCall:"+a);
		m_callMenuCsrNo = (short)a;
		m_callMenuSelF = true;
	}

	/// <summary>
	/// 次へボタン
	/// </summary>
	public void ButtonGoToNext()
	{
		DebLog("//-*ButtonGoToNext()");
		nextBtnF = true;
	}


//-*SUGI_DEB***************************
#if SUGI_DEB //-*todo:注デバッグ中
	/// <summary>
	/// 牌数の確認
	/// </summary>
	public bool CheckTileLine(byte[] paiLineArray)
	{
		List<byte> paiList = paiLineArray.ToList();
		bool isSafe = true;
		//-*enum PAI を順番に
		foreach (var i in Enum.GetValues(typeof(PAI)).Cast<PAI>())
		{
			int paiNum = paiList.Count(n => n==(byte)i);
			// Debug.Log("//-*牌名："+i+"["+(byte)i+"("+(int)i+")]___枚数："+paiNum);
			if(paiNum != MJDefine.ONE_TILES_NUM_MAX && i != PAI.URA){
			//-*1牌が4枚でなければエラー
				Debug.LogError("//-*牌名："+i+"["+(byte)i+"("+(int)i+")]___枚数："+paiNum);
				isSafe = false;
			}
		}
		return isSafe;
	}
#endif //-*todo:注デバッグ中
//-****************************SUGI_DEB

#endregion //-*UNITY_ORIGINAL
}
