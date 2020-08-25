using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Const;

//-********
using MahjongDeffine;
using MJDefsHeader;
using GameDefsHeader;
using MJDialogHeader;
//-********

namespace MahjongDeffine {
	public class MJDefine{
		public const int APFTBL_COUNT = 56;		//-*apftblの添え字用
		public const int MEMBER_NUM_MAX = 2;	//-*〇人対戦
		public const int TYPE_TILES_NO_MAX = 9;	//-*牌1種の数
		public const int ONE_TILES_NUM_MAX = 4;	//-*牌1個の数
		public const int TILES_NUM_MAX = (int)TILE_LIST.TILES_MAX*ONE_TILES_NUM_MAX;	//-*牌の総数
		public const int TO_DRAW_POS = 13;	//-*ツモ牌置き場
		public const int HAND_NUM_MAX = 14;	//-*ツモ牌置き場

#region FAME_MSGDEFINE_H
// 定数定義
		public const int HOUSE_MAX_NUM = 4;
		public const int RULE_MAX_NUM = 41;
		public const int HAI_MAX_NUM = 136;
		public const int YAKU_MAX_NUM = 12;
		public const int TEHAI_MAX_NUM = 14;
		public const int FURO_MAX_NUM = 4;
		public const int MATI_MAX_NUM = 13;
		public const int SUTE_MAX_NUM = 24;
#endregion //-*FAME_MSGDEFINE_H

#region MJDEF_H
// フーロ牌用
public const byte D_MINKAN	= 0xC0;
public const byte D_ANKAN	= 0x80;
public const byte D_PON		= 0x40;
public const byte D_CHI		= 0x00;	// チーのdefineは存在しない。

public const byte D_MANZU	= 0x11;	// 萬子
public const byte D_SOUZU	= 0x10;	// 索子
public const byte D_PINZU	= 0x01;	// 筒子
public const byte D_JIHAI	= 0x00;	// 字牌
	/****************************************************/
	/*				ニックネーム長					*/
	/****************************************************/
	public const int D_NICK_NAME_MAX = 10;											// ニックネーム最大長
	public const string D_PLAYER_DEFAULT_NICNAME = "プレイヤー";

	/****************************************************/
	/*				対話テーブル数						*/
	/****************************************************/
	public const int D_TALK_TABLE_MAX = 10;							// トーク用情報テーブル数;
	public const int D_CHARCTER_TALK_TABLE_SIZE = (18+1);			// セリフ+NULL
	/****************************************************/
	/*				タイムアウト時間					*/
	/****************************************************/
	public const int D_MEMBER_WAIT_TIMEOUT	 = (30*1000);
	public const int D_TSUMO_WAIT_TIMEOUT	 = (5*1000);
	public const int D_NAKI_WAIT_TIMEOUT	 = (5*1000);
	public const int D_NEXT_WAIT_TIMEOUT	 = (5*1000);
	public const int D_NEXT_WAIT_TIMEOUTS	 = ((3*1000)/ 2);
	public const int D_SEND_KYOKU_READY_REPORT_WAIT = (5*1000);

//-------------------------------------------------------------------------
// サーバー共通定義
//-------------------------------------------------------------------------
	public const int MATCHID_INVALID  = (0xFFFF);
//0427mt ->
	public const int D_REENTRY_INTERVAL	= 2000;  //0511mt
	public const int D_REENTRY_TRY_MAX  = 15;  //0511mt
//0427mt <-

// 鳴き待ち間隔
	public const int D_TALK_WAIT = (15*2);		//2000

	public const int D_TALK_WAIT_PON	= (15*2);		//2000
	public const int D_TALK_WAIT_CHI	= (15*2);		//2000
	public const int D_TALK_WAIT_TSUMO	= (15*2);		//2000
	public const int D_TALK_WAIT_RICHI	= (15*2);		//2000
	public const int D_TALK_WAIT_KAN	= (15*2);		//2000
	public const int D_TALK_WAIT_RON	= (15*2);		//2000

	public const int D_OPT_STK_MAX = 5;	// 鳴き選択メニュー表示最大数
#endregion	//-*MJDEF_H


#region MJ3HEADER
// 麻雀大会３ 麻雀ゲームデファイン(mj_defs.h)
	#if	Rule_2P
		public const int SUTEHAIMAX = (24+ 8);	//捨て牌表示用
	#else
		public const int SUTEHAIMAX	= (24);		//捨て牌表示用
	#endif
		public const int D_MAX_NAKI_OPT_COUNT = 9;	// 打牌は除く

		/*******************************************
				その他
		*******************************************/
		public const int EARLSTG = ( 76 );		//* 早い立直
		public const int LATESTG = ( 100 );		//* 遅い立直
		public const int MAX_PAI_KIND = ( 9 + 9 + 9 + 4 + 3 );	//* 麻雀牌の種類

		/*******************************************
				上がり点数
		*******************************************/
		public const int MAX_YAKU = (12);	//* 同時発生役数


		public const int HAI_TYPE_MAX = (7);
		public const int SURVIVAL_MAX_STAGE = (13);

		//-*基本デファイン(defs.h)
		//xxxx	#define ETRACE DBGPRINTF
		public const byte NONE = ( 0xFF );		//* no use UTINY value
		public const ushort WORDMAX = ( 0xFFFF );
#endregion	//-*MJ3HEADER

#region MAHJONG_H
		public const int __startWait00 = (8);	//配牌開始時のウェイト
		public const int ReachPoint = 10;		//リーチ代 (x100)
#endregion //-*MAHJONG_H

#region GAMEDEFSHEADER
		//*****************************
		//	キャラクター
		//*****************************
		//** 記録用キャラクター番号 **//

		public const int MAX_ENTRY_STAGE = 12;	//*	シーズンエントリ数
		public const int MAX_STAGE = 5;			//*	ステージ数
		public const int MAX_TABLE_MEMBER = 4;	//*	ゲームの人数
	#if	Rule_2P
		public const int MAX_TABLE_MEMBER2 = 2;	///ゲームの人数
	#else
		public const int MAX_TABLE_MEMBER2 = 4;	///ゲームの人数
	#endif
		public const int MAX_TABLE = (MAX_ENTRY_STAGE/MAX_TABLE_MEMBER);		//*	卓数

		public const int MAX_NORMAL_CHARACTER = (11);	//*	通常キャラ数
		public const int MAX_SPECAIL_CHARACTER = (5);	//*	特殊キャラ数
		public const int MAX_COMP_CHARACTER = (MAX_NORMAL_CHARACTER+MAX_SPECAIL_CHARACTER);
		public const int MAX_CHARACTER = (MAX_COMP_CHARACTER+1);
		//*	コンピュータキャラ数
		public const int USER_CHARACTERNO = (MAX_CHARACTER-1);		//*	ユーザキャラの番号

		public const int MAX_TALKDATA = 50;
		public const int MAX_TALKKIND = 326;
		//-*todo:不要？
		//-*public const int MAX_TALKSTRING = ((((MAX_TALKKIND*MAX_TALKDATA)+(SECTOR_SIZE-1))/SECTOR_SIZE)*SECTOR_SIZE);

		public const int GAMEID_STAGE = 0;
		public const int GAMEID_FINAL = (GAMEID_STAGE+MAX_STAGE);
		public const int GAMEID_FREE = (GAMEID_FINAL+1);

		// ボタンが決定状態で待つカウント数
	#if MODE_IS_FRAME
		public const int OK_COUNTER_NUM = (8);
	#else	// MODE_IS_FRAME==0
		public const int OK_COUNTER_NUM = (30);
	#endif	// MODE_IS_FRAME==0

		/*****************************
			キャラクターデータ
		*****************************/
		public const int NAME_MAX = (10);	//*	名前文字バイト数
		public const int NAME_ZENKAKU_MAX = (NAME_MAX / 2);	//*	名前文字全角数

		/*	bySeasonMode	*/
		/*	byGameMode	*/
		public const int GAMEMODE_NONE = (0);	//  ゲームモード無し	//0422
		public const int GAMEMODE_FREE = (5);	//*	フリーゲーム
		public const int GAMEMODE_SURVIVAL = (9);	//*	サバイバル
		//*	2005/11/07 add
		public const int GAMEMODE_NET_FREE = (11);	//*	通信対戦
		public const int GAMEMODE_NET_RESERVE = (12);	//*	通信対戦

		//-*todo:不要？
		// public const int IsFreeGameMode()		(sGameData.byGameMode == GAMEMODE_FREE)

		public const int PAI_MAX = (122);	//ツモ牌数 (河底 ハイテイ)

		public const int D_RULE_PAGE_MAX = 6;	// ルール画面数
		public const int RSEL_MAX = 8;			// ルール選択数
		public const int MENU_SEL_MAX = 20;
		public const int MENU_SEL_DATA_MAX = 3;
#endregion //-*GAMEDEFSHEADER
#region MJ_GAME_H
		public const int TSUMO_ADD		= 3;
#endregion //-*MJ_GAME_H
#region MJEVAL_J
		public const int MENTSU		=  64;
#endregion //-*MJEVAL_J

#region MJEMNT_J
		public const int FAULT		=  -1000;
#endregion //-*MJEMNT_J

#region GAMEDEFS_JAVA

		// 牌データテーブル概念化Init
		public static PAI[] gPaiData = new PAI[]
		{
		// 萬子
			PAI.M1,PAI.M2,PAI.M3,PAI.M4,PAI.M5,PAI.M6,PAI.M7,PAI.M8,PAI.M9,
		// 索子
			PAI.S1,PAI.S2,PAI.S3,PAI.S4,PAI.S5,PAI.S6,PAI.S7,PAI.S8,PAI.S9,
		// 筒子
			PAI.P1,PAI.P2,PAI.P3,PAI.P4,PAI.P5,PAI.P6,PAI.P7,PAI.P8,PAI.P9,
		// 字牌
			PAI.TON,PAI.NAN,PAI.SYA,PAI.PEI,PAI.HAKU,PAI.HATSU,PAI.CHUN,
		// 裏牌
			PAI.URA,
		};


	/// <summary>
	/// <para> 固定ルールセット </para>
	/// <para> 食断  鳴平	平ﾂﾓ  ﾉｰ罰	流局	裏ド  ｶﾝド	ｶﾝ裏  一発	ドボ </para>
	/// <para>持点  返点	場風  西入	うま	和了  途流	二縛  国搶	立暗</para>
	/// <para>棒戻  焼鳥	割目  八連	一賞	裏賞  役賞	鳥撃  ｱﾘス	金鶏</para>
	/// <para>五筒  三連	一色  流満	十三	人和  車輪	百万  ﾀﾞﾌﾞ	二和</para>
	/// <para>三和</para>
	/// </summary>
		public static byte[][] byRuleData = new byte[][]{		//static	const BYTE	byRuleData[11][ 41 ] =		//0422
	//#if 0
	//	{	/*	デバッグ用	*/
	//		0x01,0x01,0x01,0x01,0x00, 0x01,0x01,0x01,0x01,0x01,
	//		0x01,0x00,0x01,0x00,0x00, 0x00,0x02,0x01,0x00,0x01,
	//		0x00,0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,0x00,
	//		0x00,0x00,0x00,0x01,0x01, 0x01,0x00,0x00,0x01,0x00,
	//		0x00,0x00
	//	},
	//#else
		new byte[]{	/*	標準ルール	*/
			0x01,0x01,0x01,0x01,0x00, 0x01,0x01,0x01,0x01,0x01,
			0x01,0x00,0x01,0x00,0x00, 0x00,0x02,0x01,0x00,0x01,
			0x00,0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,0x00,
			0x00,0x00,0x00,0x01,0x01, 0x01,0x00,0x00,0x01,0x00,
			0x00,0x00
		},
	//#endif
		new byte[]{	/*	東風ルール	*/
			NONE,NONE,NONE,NONE,0x02, NONE,NONE,NONE,NONE,NONE,
			0x01,0x00,0x02,0x00,0x06, NONE,NONE,NONE,NONE,NONE,
			NONE,0x01,NONE,NONE,NONE, NONE,NONE,0x01,NONE,NONE,
			NONE,NONE,NONE,NONE,NONE, NONE,NONE,NONE,NONE,NONE,
			NONE,NONE
		},
		new byte[]{	/*	北海道ルール	*/
			NONE,NONE,NONE,NONE,NONE, NONE,NONE,NONE,NONE,0x04,
			0x07,0x01,0x00,0x00,NONE, NONE,NONE,NONE,NONE,NONE,
			NONE,NONE,NONE,NONE,NONE, NONE,NONE,NONE,NONE,NONE,
			NONE,NONE,NONE,NONE,NONE, NONE,NONE,NONE,NONE,NONE,
			NONE,NONE
		},
		new byte[]{	/*	ブン屋ルール	*/
			NONE,NONE,NONE,NONE,NONE, NONE,NONE,NONE,NONE,0x00,
			0x00,0x00,0x01,0x00,0x07, NONE,NONE,0x00,NONE,NONE,
			NONE,0x00,NONE,NONE,NONE, NONE,NONE,NONE,NONE,NONE,
			NONE,NONE,NONE,NONE,NONE, NONE,NONE,NONE,NONE,NONE,
			NONE,NONE
		},
		new byte[]{	/*	場縛りルール	*/
			NONE,NONE,NONE,NONE,NONE, NONE,NONE,NONE,NONE,NONE,
			NONE,NONE,0x01,NONE,NONE, NONE,NONE,0x01,NONE,NONE,
			NONE,NONE,NONE,NONE,NONE, NONE,NONE,NONE,NONE,NONE,
			NONE,NONE,NONE,NONE,NONE, NONE,NONE,NONE,NONE,NONE,
			NONE,NONE
		},
		new byte[]{	/*	競技ルール	*/
			0x01,0x01,0x01,0x01,0x00, 0x00,0x00,0x00,0x00,0x00,
			0x06,0x00,0x01,0x00,0x00, 0x00,0x02,0x01,0x00,0x01,
			0x00,0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,0x00,
			0x00,0x00,0x00,0x01,0x00, 0x00,0x00,0x00,0x00,0x00,
			0x00,0x00
		},
		new byte[]{	/*	インフレルール	*/
			NONE,NONE,NONE,NONE,NONE, 0x01,0x01,0x01,0x01,0x04,
			0x00,0x00,NONE,NONE,0x08, NONE,NONE,NONE,NONE,NONE,
			NONE,0x06,0x01,NONE,0x01, 0x01,0x01,0x01,0x01,NONE,
			NONE,NONE,NONE,NONE,NONE, NONE,NONE,NONE,NONE,NONE,
			NONE,NONE
		},
		new byte[]{	/*	自由設定	*/
			NONE,NONE,NONE,NONE,NONE, NONE,NONE,NONE,NONE,NONE,
			NONE,NONE,NONE,NONE,NONE, NONE,NONE,NONE,NONE,NONE,
			NONE,NONE,NONE,NONE,NONE, NONE,NONE,NONE,NONE,NONE,
			NONE,NONE,NONE,NONE,NONE, NONE,NONE,NONE,NONE,NONE,
			NONE,NONE
		},
		new byte[]{	/*	フリールール	*/
			NONE,NONE,NONE,NONE,NONE, NONE,NONE,NONE,NONE,NONE,
			NONE,NONE,NONE,NONE,NONE, NONE,NONE,NONE,NONE,NONE,
			NONE,NONE,NONE,NONE,0x00, 0x00,0x00,0x00,0x00,NONE,
			NONE,NONE,NONE,NONE,NONE, NONE,NONE,NONE,NONE,NONE,
			NONE,NONE
		},
		new byte[]{	/*	サバイバルルール	*/
			NONE,NONE,NONE,NONE,0x02, NONE,NONE,NONE,NONE,NONE,
			0x01,0x00,0x02,0x00,0x06, NONE,NONE,NONE,NONE,NONE,
			NONE,NONE,NONE,NONE,NONE, NONE,NONE,NONE,NONE,NONE,		//2006.2.19 焼き鳥無しに変更。
			NONE,NONE,NONE,NONE,NONE, NONE,NONE,NONE,NONE,NONE,
			NONE,NONE
		},
		new byte[]{	/*	通信対戦ルール	携帯用*/
			0x01,0x01,0x01,0x01,0x00, 0x01,0x01,0x01,0x01,0x01,
			0x01,0x00,0x02,0x00,0x00, 0x00,0x02,0x01,0x00,0x01,
			0x00,0x00,0x00,0x00,0x00, 0x00,0x00,0x00,0x00,0x00,
			0x00,0x00,0x00,0x01,0x01, 0x01,0x00,0x00,0x01,0x00,
			0x00,0x00
		}
	};


	static byte[] urasho_sub = {2,1,1,0,0};	//static	const BYTE	urasho_sub[5] = {2,1,1,0,0};

	static byte[] csel_face = {		//static	const BYTE	csel_face[MAX_COMP_CHARACTER+1] =
		0,		/*水木優		  */
		1,		/*高沢渚		  */
		2,		/*東海林ワタミ	  */
		3,		/*鳥居美砂		  */
		4,		/*瀬川舞		  */
		5,		/*ビル・フｨｯシｬ-  */
		15,		/*滝川奈月		  */
		7,		/*コ-ラス・スミス */
		8,		/*岩波時典		  */
		9,		/*沖田ジｮ-ジ	  */
		14,		/*天海レナ　	  */
		/* 以下隠し */
		11,		/*海原匠		  */
		12,		/*シブサワコウ	  */
		13,		/*磯部慎吾		  */
		10,		/*水島リヨ子	  */
		6,		/*藤波泰男		  */
		0xff,
	};

		public static byte[][] rule_val_tbl = new byte[][]{	//static	const BYTE	rule_val_tbl[42][10] =
			new byte[]{	(byte)RV.RV_NASHI,	(byte)RV.RV_ARI,	0xFF,},											/* 食い断ヤオ			  */
			new byte[]{	(byte)RV.RV_20PU,	(byte)RV.RV_30PU,	0xFF,},											/* ナキ平和 			  */
			new byte[]{	(byte)RV.RV_NASHI,	(byte)RV.RV_ARI,	0xFF,},											/* 平和ツモ 			  */
			new byte[]{	(byte)RV.RV_NASHI,	(byte)RV.RV_ARI,	0xFF,},											/* ノーテン罰符 		  */
			new byte[]{	(byte)RV.RV_TENPAI,	(byte)RV.RV_NANBA,	(byte)RV.RV_AGARI,	0xFF,},						/* 流局設定 			  */
			new byte[]{	(byte)RV.RV_NASHI,	(byte)RV.RV_ARI,	0xFF,},											/* 裏ドラ				  */
			new byte[]{	(byte)RV.RV_NASHI,	(byte)RV.RV_ARI,	0xFF,},											/* カンドラ 			  */
			new byte[]{	(byte)RV.RV_NASHI,	(byte)RV.RV_ARI,	0xFF,},											/* カンウラ 			  */
			new byte[]{	(byte)RV.RV_NASHI,	(byte)RV.RV_ARI,	0xFF,},											/* 一発賞				  */
			new byte[]{	(byte)RV.RV_NASHI,	(byte)RV.RV_0,		(byte)RV.RV_10,		(byte)RV.RV_20,		(byte)RV.RV_30,	0xFF,},		/* ドボン				  */
			new byte[]{	(byte)RV.RV_24000,	(byte)RV.RV_25000,	(byte)RV.RV_26000,	(byte)RV.RV_27000,
						(byte)RV.RV_28000,	(byte)RV.RV_29000,	(byte)RV.RV_30000,	(byte)RV.RV_16000,	0xFF,},	/* 配給原点 			  */
			new byte[]{	(byte)RV.RV_30000,	(byte)RV.RV_20000,	0xFF,},											/* 返し点				  */
			new byte[]{	(byte)RV.RV_TONTON,	(byte)RV.RV_TONNAN,	(byte)RV.RV_TONPU,	0xFF,},						/* 場風 				  */
			new byte[]{	(byte)RV.RV_NASHI,	(byte)RV.RV_30000,	(byte)RV.RV_30100,
						(byte)RV.RV_33100,	(byte)RV.RV_35100,	0xFF,},											/* 西入 				  */

			new byte[]{	(byte)RV.RV_NASHI,	(byte)RV.RV_0_5,		(byte)RV.RV_0_10,	(byte)RV.RV_0_20,	(byte)RV.RV_0_30,
						(byte)RV.RV_5_10,	(byte)RV.RV_10_20,	(byte)RV.RV_10_30,	(byte)RV.RV_20_30,	0xFF,},	/* 馬					  */
			new byte[]{	(byte)RV.RV_NASHI,	(byte)RV.RV_ARI,	0xFF,},											/* 和了止め 			  */
			new byte[]{	(byte)RV.RV_NASHI,	(byte)RV.RV_RONCHAN,	(byte)RV.RV_RENCHAN,	0xFF,},				/* 途中流局 			  */
			new byte[]{	(byte)RV.RV_NASHI,	(byte)RV.RV_ARI,	0xFF,},											/* 二ﾊﾝ縛り 			  */
			new byte[]{	(byte)RV.RV_NASHI,	(byte)RV.RV_ARI,	0xFF,},											/* 国士搶ｶﾝ 			  */
			new byte[]{	(byte)RV.RV_NASHI,	(byte)RV.RV_ARI,	0xFF,},											/* リーチ後のアンカン	  */
			new byte[]{	(byte)RV.RV_NASHI,	(byte)RV.RV_ARI,	0xFF,},											/* リー棒の戻り 		  */
			new byte[]{	(byte)RV.RV_NASHI,	(byte)RV.RV_10,		(byte)RV.RV_15,		(byte)RV.RV_20,
						(byte)RV.RV_URA10,	(byte)RV.RV_URA15,	(byte)RV.RV_URA20,	0xFF,},						/* 焼き鳥				  */
			new byte[]{	(byte)RV.RV_NASHI,	(byte)RV.RV_ARI,	0xFF,},											/* 割れ目				  */
			new byte[]{	(byte)RV.RV_NASHI,	(byte)RV.RV_ARI,	0xFF,},											/* 八連荘				  */
			new byte[]{	(byte)RV.RV_NASHI,	(byte)RV.RV_ARI,	0xFF,},											/* 一発賞				  */
			new byte[]{	(byte)RV.RV_NASHI,	(byte)RV.RV_ARI,	0xFF,},											/* 裏ドラ賞 			  */
			new byte[]{	(byte)RV.RV_NASHI,	(byte)RV.RV_ARI,	0xFF,},											/* 役満賞				  */
			new byte[]{	(byte)RV.RV_NASHI,	(byte)RV.RV_ARI,	0xFF,},											/* 鳥撃ち				  */

			new byte[]{	(byte)RV.RV_NASHI,	(byte)RV.RV_ARI,	0xFF,},											/* アリス				  */
			new byte[]{	(byte)RV.RV_NASHI,	(byte)RV.RV_ARI,	0xFF,},											/* 金鶏独立 			  */
			new byte[]{	(byte)RV.RV_NASHI,	(byte)RV.RV_ARI,	0xFF,},											/* 五筒開花 			  */
			new byte[]{	(byte)RV.RV_NASHI,	(byte)RV.RV_ARI,	0xFF,},											/* 三連刻				  */
			new byte[]{	(byte)RV.RV_NASHI,	(byte)RV.RV_ARI,	0xFF,},											/* 一色三順 			  */
			new byte[]{	(byte)RV.RV_NASHI,	(byte)RV.RV_ARI,	0xFF,},											/* 流し満貫 			  */
			new byte[]{	(byte)RV.RV_NASHI,	(byte)RV.RV_MANGAN,	(byte)RV.RV_BAIMAN,	(byte)RV.RV_YAKUMAN,	0xFF,},				/* 十三不搭 			  */
			new byte[]{	(byte)RV.RV_NASHI,	(byte)RV.RV_ARI,	0xFF,},											/* 人和 				  */
			new byte[]{	(byte)RV.RV_NASHI,	(byte)RV.RV_ARI,	0xFF,},											/* 大車輪				  */
			new byte[]{	(byte)RV.RV_NASHI,	(byte)RV.RV_ARI,	0xFF,},											/* 百万石				  */
			new byte[]{	(byte)RV.RV_NASHI,	(byte)RV.RV_ARI,	0xFF,},											/* ダブル				  */
			new byte[]{	(byte)RV.RV_NASHI,	(byte)RV.RV_ARI,	0xFF,},											/* 二家和				  */
			new byte[]{	(byte)RV.RV_NASHI,	(byte)RV.RV_ARI,	0xFF,},											/* 三家和				  */
			new byte[]{	(byte)RV.RV_SPACE,	(byte)RV.RV_SPACE,	0xFF,},											/* 標準に戻す			  */
		};


	public static byte[]	SurvivalMentsuTable = {	//static const BYTE	SurvivalMentsuTable[MAX_COMP_CHARACTER] =
		0,		/*水木優		  */
		1,		/*高沢渚		  */
		2,		/*東海林ワタミ	  */
		3,		/*鳥居美砂		  */
		9,		/*沖田ジｮ-ジ	  */
		10,		/*水島リヨ子	  */
		5,		/*ビル・フｨｯシｬ-  */
		4,		/*瀬川舞		  */
		8,		/*岩波時典		  */
		14,		/*天海レナ　	  */
		7,		/*コ-ラス・スミス */
		6,		/*藤波泰男		  */
		15,		/*滝川奈月		  */
		11,		/*海原匠		  */
		12,		/*シブサワコウ	  */
		13,		/*磯部慎吾		  */
	};

	// キャラクターの名前
	public static String[]	CharNickNameTable =	//static const char	CharNickNameTable[][D_NICK_NAME_MAX+1] =
	{
		"COM 00",			//"水木",
		"COM 01",			//"高沢",
		"COM 02",			//"東海林",
		"COM 03",			//"鳥居",
		"COM 04",			//"瀬川",
		"COM 05",			//"ビル",
		"COM 06",			//"藤波",
		"COM 07",			//"コーラス",
		"COM 08",			//"岩波",
		"COM 09",			//"沖田",
		"COM 10",			//"水島",
		"COM 11",			//"海原",
		"COM 12",			//"シブサワ",
		"COM 13",			//"磯部",
		"COM 14",			//"天海",
		"COM 15",			//"滝川"
	};

#endregion	//-*GAMEDEFS_JAVA
#region MJ_CHAR_J

public static PARA[] sCharaMJParam= new PARA[]{
		/* name                        rr ud ki in yk or ft fi fv f 1 2 hon toi tan k  a t s j k f s r a */
#if	_MONITOR_CODE2
/*C*/	new PARA (/*水木優          */ -2, 0,-1, 0,-2,-2,-4,-4,-6,1,3,2,0,0,4,1,0,0,5,-2,0,0,0,1,1,1,1,0,0,0),
/*D*/	new PARA (/*高沢渚          */ -1, 0,-1, 0,-2,-2,-4,-4,-6,1,3,2,0,0,4,1,0,0,5,-2,0,0,0,0,1,1,1,1,0,0),
/*C*/	new PARA (/*東海林ワタミ    */ -1, 0, 0, 0,-2,-2,-4,-4,-6,1,3,2,0,0,4,1,0,0,5,-2,0,0,0,1,1,1,1,1,0,0),
/*A*/	new PARA (/*鳥居美砂        */ -3, 0, 0, 0,-2,-3,-4,-4,-6,1,3,2,0,0,4,1,0,0,5,-2,0,0,0,1,1,1,1,1,0,0),
/*A*/	new PARA (/*瀬川舞          */ -3, 0, 0, 0,-2,-3,-4,-4,-6,1,3,2,0,0,4,1,0,0,5,-2,0,0,0,1,1,1,1,1,0,0),

/*B*/	new PARA (/*ビル・フｨｯシｬ-  */ -2, 0, 0, 0,-2,-2,-4,-4,-6,1,3,2,0,0,4,1,0,0,5,-2,0,0,0,1,1,1,1,1,0,0),
/*E*/	new PARA (/*藤波泰男        */ -3, 0,-1, 0,-2,-2,-4,-4,-6,1,3,2,0,0,4,1,0,0,5,-2,0,0,0,1,1,1,1,1,0,0),
/*B*/	new PARA (/*コ-ラス・スミス */ -2, 0, 0, 0,-2,-2,-4,-4, 6,1,3,2,0,0,4,1,0,0,5,-2,0,0,0,1,1,1,1,1,0,0),
/*A*/	new PARA (/*岩波時典        */ -3, 0, 0, 0,-2,-3,-4,-4,-6,1,2,2,0,0,4,1,0,0,5,-2,0,0,0,1,1,1,1,0,0,0),
/*D*/	new PARA (/*沖田ジｮ-ジ      */ -1, 0,-1, 0,-2,-2,-4,-4,-6,1,3,2,0,0,4,1,0,0,5,-2,0,0,0,1,1,1,1,1,0,0),

/*E*/	new PARA (/*水島リヨ子      */ -3, 0,-2, 0,-2,-4,-4,-4,-6,1,3,2,0,0,4,1,0,0,5,-2,0,0,0,0,0,0,0,0,0,0),
/*S*/	new PARA (/*海原匠          */  4, 5, 0, 0, 0, 1, 1, 0,-1,2,2,1,1,4,1,4,1,2,3, 0,0,0,1,1,1,1,1,0,0,0),
/*S*/	new PARA (/*シブサワコウ    */  4, 5, 0, 0, 0, 1, 1, 1, 1,2,1,1,1,3,1,4,3,1,4,-1,0,0,1,1,1,1,1,1,0,0),
/*SS*/	new PARA (/*磯部慎吾        */  5, 5, 0, 0, 0, 2, 2, 2, 2,2,2,1,1,3,1,4,1,4,4, 0,0,0,1,1,1,1,1,1,0,0),
/*E*/	new PARA (/*天海レナ　      */ -3, 0,-2, 0,-2,-4,-4,-4,-6,1,3,2,0,0,4,1,0,0,5,-2,0,0,0,0,0,0,0,0,0,0),
/*A*/	new PARA (/*滝川奈月        */ -3, 0, 0, 0,-2,-3,-4,-4,-6,1,3,2,0,0,4,1,0,0,5,-2,0,0,0,1,1,1,1,1,0,0),
#else
		/* name                        rr ud ki in yk or ft fi fv f 1 2 hon toi tan k  a t s j k f s r a */
/*C*/	new PARA (/*水木優          */ -2, 0,-1, 0,-1,-2,-2,-2,-3,2,2,1,1,1,1,2,1,1,4,-1,0,0,0,1,1,1,1,0,0,0),
/*D*/	new PARA (/*高沢渚          */ -1, 0,-1, 0,-2, 1,-1,-1,-2,3,2,2,1,2,1,1,1,1,2,-1,0,0,1,0,1,1,1,1,0,0),
/*C*/	new PARA (/*東海林ワタミ    */ -1, 1, 0, 0, 0,-2,-1,-1,-1,2,2,2,1,2,1,1,1,1,2, 0,0,0,1,1,1,1,1,1,0,0),
/*A*/	new PARA (/*鳥居美砂        */  3, 2, 0, 0, 0, 3, 2, 2, 1,3,3,1,1,2,1,2,2,2,1, 0,0,0,1,1,1,1,1,1,0,0),
/*A*/	new PARA (/*瀬川舞          */  3, 3, 0, 0, 0, 3, 1, 1, 1,3,3,1,1,2,1,2,1,3,1, 0,0,0,1,1,1,1,1,1,0,0),

/*B*/	new PARA (/*ビル・フｨｯシｬ-  */  2, 1, 0, 0, 0, 2, 0, 0, 2,3,3,1,1,3,1,1,0,0,1, 0,0,0,1,1,1,1,1,1,0,0),
/*E*/	new PARA (/*藤波泰男        */ -3, 0,-1, 0,-1, 2,-1,-1,-2,3,2,2,1,2,1,1,1,1,1,-2,0,0,1,1,1,1,1,1,0,0),
/*B*/	new PARA (/*コ-ラス・スミス */  2, 2, 0, 0, 0, 2, 2, 2, 2,4,3,2,1,2,1,3,1,3,2, 0,0,0,1,1,1,1,1,1,0,0),
/*A*/	new PARA (/*岩波時典        */  3, 3, 0, 0,-1, 3,-1,-1, 0,2,2,2,1,2,1,2,1,1,2, 0,0,0,1,1,1,1,1,0,0,0),
/*D*/	new PARA (/*沖田ジｮ-ジ      */ -2, 0,-1, 0, 0,-1,-2,-2,-2,3,2,2,1,1,1,2,1,1,3,-2,0,0,1,1,1,1,1,1,0,0),

/*E*/	new PARA (/*水島リヨ子      */ -3, 0,-2, 0,-2,-4,-3,-3,-4,1,3,2,1,2,1,1,1,1,4,-2,0,0,0,0,0,0,0,0,0,0),
/*S*/	new PARA (/*海原匠          */  4, 5, 0, 0, 0, 1, 1, 0,-1,2,2,1,1,4,1,3,1,2,3, 0,0,0,1,1,1,1,1,0,0,0),
/*S*/	new PARA (/*シブサワコウ    */  4, 5, 0, 0, 0, 1, 1, 1, 1,2,1,1,1,3,1,4,3,1,4,-1,0,0,1,1,1,1,1,1,0,0),
/*SS*/	new PARA (/*磯部慎吾        */  5, 5, 0, 0, 0, 2, 2, 2, 2,2,2,1,1,3,1,4,1,4,4, 0,0,0,1,1,1,1,1,1,0,0),
/*E*/	new PARA (/*天海レナ　      */ -3, 0,-2, 0,-2,-4,-3,-3,-4,1,3,2,1,2,1,1,1,1,4,-2,0,0,0,0,0,0,0,0,0,0),
/*A*/	new PARA (/*滝川奈月        */  3, 3, 0, 0, 0, 3, 1, 1, 1,3,3,1,1,2,1,2,1,3,1, 0,0,0,1,1,1,1,1,1,0,0),
};

public static PARA[] sCharaMJParamTest= new PARA[]{

#region UNITY_ORIGINAL
		new PARA (/*全部マイナス    */  -7, -7, -7, -7, -7, -7, -7, -7, -7, -7, -7, -7, -7, -7, -7, -7, -7, -7, -7, -7, -7, -7, -7, -7, -7, -7, -7, -7, -7, -7),
		new PARA (/*全部プラス      */   7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7),
#endregion
#endif
};

#endregion //-*MJ_CHAR_J

#region YAMADATA_J
	public static byte[][] tumiData = new byte[][]{
	//-*todo:char -> byte に変換した　必要ないかも
// #include "../../YamaData.j"
	// //		0x01 ～ 0x09	マンズ
	// //		0x11 ～ 0x19	ソウズ
	// //		0x21 ～ 0x29	ピンズ
	// //		0x31 ～ 0x37	字牌
	// //	tsumikomi()
	// /*
	// 	tsumikomiNum
	// ."D:\Mobile\JPhone\Game\lv_mah01\src\Game.java"
	// ・ 2992,29  :  static int tsumikomiNum = 9;
	// */
		new byte[]{	//	00:基本(Yamadata00.j)
			0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
			0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19,
			0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29,
			0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37,
			0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
			0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19,
			0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29,
			0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37,
			0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
			0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19,
			0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29,
			0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37,
			0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
			0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19,
			0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29,
			0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37,
		},
		new byte[]{	//	01:七対子(Yamadata01.j)
			//配牌
			0x01, 0x01, 0x03, 0x03,		//自分
			0x11, 0x12, 0x13, 0x14,		//相手
			0x15, 0x16, 0x17, 0x18,
			0x01, 0x02, 0x03, 0x04,

			0x05, 0x05, 0x07, 0x07,		//自分
			0x05, 0x06, 0x07, 0x08,		//相手
			0x01, 0x02, 0x03, 0x04,
			0x05, 0x06, 0x07, 0x08,

			0x02, 0x02, 0x04, 0x04,		//自分
			0x11, 0x12, 0x13, 0x14,		//相手
			0x15, 0x16, 0x17, 0x18,
			0x06, 0x06, 0x08, 0x08,

			0x19,						//自分
			0x09,						//相手
			0x09,
			0x09,
			//未使用牌
			0x09, 0x19, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18,
			0x19, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19,
			0x21, 0x22, 0x23, 0x24, 0x25, 0x26,
			//ツモ牌
			0x27, 0x28, 0x29, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27,
			0x28, 0x29, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28,
			0x29, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29,
			0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x31, 0x32, 0x33,
			0x34, 0x35, 0x36, 0x37,
			//王牌
			0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37,
			0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37,
			//	  ↑ ドラ
		},
		new byte[]{	//	02:七対子 2(Yamadata02.j)
			0x01, 0x01, 0x03, 0x03,
			0x11, 0x12, 0x13, 0x14,
			0x15, 0x16, 0x17, 0x18,
			0x34, 0x35, 0x36, 0x37,

			0x05, 0x05, 0x07, 0x07,
			0x13, 0x14, 0x15, 0x16,
			0x31, 0x32, 0x33, 0x34,
			0x28, 0x29, 0x32, 0x33,

			0x02, 0x02, 0x04, 0x04,
			0x11, 0x12, 0x13, 0x14,
			0x15, 0x16, 0x17, 0x18,
			0x35, 0x36, 0x37, 0x31,

			0x23, 0x24, 0x25, 0x26,

			0x27, 0x28, 0x29, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27,
			0x19, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19,
			0x21, 0x22, 0x23, 0x24, 0x25, 0x26,

			0x05, 0x06, 0x07, 0x08,
			0x01, 0x02, 0x03, 0x04,
			0x05, 0x06, 0x07, 0x08,
			0x01, 0x02, 0x03, 0x04,
			0x06, 0x06, 0x08, 0x08,
			0x09, 0x09, 0x09, 0x09,
			0x28, 0x29, 0x21, 0x22,
			0x27, 0x28, 0x17, 0x18,
			0x29, 0x21, 0x22, 0x23,
			0x24, 0x25, 0x26, 0x27,
			0x19, 0x11, 0x12, 0x19,

			0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37,
			0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37,
		},
		new byte[]{	//	03:国士無双１(Yamadata03.j)
			//配牌
			0x01, 0x09, 0x11, 0x19,		//自分
			0x11, 0x12, 0x13, 0x14,		//相手
			0x15, 0x16, 0x17, 0x18,
			0x01, 0x02, 0x03, 0x04,

			0x21, 0x29, 0x31, 0x32,		//自分
			0x05, 0x06, 0x07, 0x08,		//相手
			0x01, 0x02, 0x03, 0x04,
			0x05, 0x06, 0x07, 0x08,

			0x33, 0x34, 0x35, 0x36,		//自分
			0x03, 0x12, 0x13, 0x14,		//相手
			0x15, 0x16, 0x17, 0x18,
			0x06, 0x06, 0x08, 0x08,

			0x37,						//自分
			0x09,						//相手
			0x09,
			0x09,
			//未使用牌
			0x01, 0x19, 0x22, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18,
			0x19, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x03,
			0x21, 0x22, 0x23, 0x24, 0x25, 0x26,
			//ツモ牌
			0x21, 0x32, 0x29, 0x37, 0x11, 0x23, 0x24, 0x25, 0x26, 0x27,
			0x28, 0x29, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28,
			0x29, 0x05, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x05,
			0x07, 0x28, 0x02, 0x02, 0x04, 0x04, 0x27, 0x31, 0x32, 0x33,
			0x34, 0x35, 0x36, 0x37,
			//王牌
			0x31, 0x07, 0x33, 0x34, 0x35, 0x36, 0x19,
			0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37,
			//	  ↑ ドラ
		},
		new byte[]{	//	04:九連宝燈１(Yamadata04.j)
			//配牌
			0x01, 0x01, 0x01, 0x02,		//自分
			0x11, 0x12, 0x13, 0x14,		//相手
			0x15, 0x16, 0x17, 0x18,
			0x03, 0x02, 0x03, 0x04,

			0x03, 0x04, 0x05, 0x06,		//自分
			0x05, 0x07, 0x07, 0x02,		//相手
			0x01, 0x02, 0x03, 0x04,
			0x05, 0x06, 0x07, 0x08,

			0x07, 0x08, 0x09, 0x09,		//自分
			0x11, 0x12, 0x13, 0x14,		//相手
			0x15, 0x16, 0x17, 0x18,
			0x12, 0x13, 0x08, 0x08,

			0x09,						//自分
			0x11,						//相手
			0x19,
			0x19,
			//未使用牌
			0x27, 0x28, 0x29, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27,
			0x19, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19,
			0x21, 0x22, 0x23, 0x24, 0x25, 0x26,
			//ツモ牌
			0x09, 0x04, 0x05, 0x06, 0x06, 0x14, 0x15, 0x16, 0x17, 0x18,
			0x28, 0x29, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28,
			0x29, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29,
			0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0x31, 0x32, 0x33,
			0x34, 0x35, 0x36, 0x37,
			//王牌
			0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37,
			0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37,
			//	  ↑ ドラ
		},
		new byte[]{	//	05:国士無双２（相手　中待ち、流し満貫確認可）(Yamadata05.j)
			//配牌
			0x01, 0x09, 0x11, 0x19,		//自分
			0x31, 0x32, 0x33, 0x34,		//相手
			0x15, 0x16, 0x17, 0x18,
			0x01, 0x02, 0x03, 0x04,

			0x21, 0x29, 0x31, 0x32,		//自分
			0x31, 0x32, 0x33, 0x34,		//相手
			0x01, 0x02, 0x03, 0x04,
			0x05, 0x06, 0x07, 0x08,

			0x33, 0x34, 0x35, 0x36,		//自分
			0x31, 0x32, 0x33, 0x34,		//相手
			0x15, 0x16, 0x17, 0x18,
			0x06, 0x06, 0x08, 0x08,

			0x37,						//自分
			0x37,						//相手
			0x23,
			0x24,
			//未使用牌
			0x25, 0x26, 0x22, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18,
			0x27, 0x28, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x03,
			0x37, 0x22, 0x23, 0x24, 0x25, 0x26,
			//ツモ牌
			0x21, 0x12, 0x29, 0x21, 0x11, 0x09, 0x09, 0x01, 0x19, 0x19,
			0x11, 0x29, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28,
			0x29, 0x05, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x05,
			0x07, 0x28, 0x02, 0x02, 0x35, 0x35, 0x27, 0x11, 0x12, 0x13,
			0x14, 0x35, 0x36, 0x09,
			//王牌
			0x03, 0x07, 0x13, 0x14, 0x04, 0x36, 0x19,
			0x05, 0x06, 0x07, 0x08, 0x04, 0x36, 0x37,
			//	  ↑ ドラ
		},
		new byte[]{	//	06:四槓流れ(Yamadata06.j)
			//配牌
			0x01, 0x01, 0x03, 0x03,		//自分
			0x11, 0x12, 0x13, 0x14,		//相手
			0x11, 0x12, 0x13, 0x14,	
			0x15, 0x16, 0x17, 0x18,

			0x01, 0x02, 0x03, 0x01,		//自分
			0x37, 0x37, 0x37, 0x37,		//相手
			0x05, 0x05, 0x07, 0x04,
			0x05, 0x06, 0x07, 0x08,

			0x02, 0x02, 0x02, 0x03,		//自分
			0x07, 0x04, 0x04, 0x04,		//相手
			0x15, 0x16, 0x17, 0x18,
			0x06, 0x06, 0x08, 0x08,

			0x09,						//自分
			0x19,						//相手
			0x19,
			0x11,
			//未使用牌
			0x27, 0x28, 0x29, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27,
			0x28, 0x29, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28,
			0x21, 0x22, 0x23, 0x24, 0x25, 0x26,
			//ツモ牌
			0x09, 0x09, 0x09, 0x12, 0x13, 0x12, 0x13, 0x18, 0x19, 0x18,
			0x19, 0x11, 0x14, 0x15, 0x14, 0x15, 0x16, 0x17, 0x16, 0x17,
			0x29, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29,
			0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x05, 0x31, 0x32, 0x33,
			0x34, 0x35, 0x36, 0x06,
			//王牌
			0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x07,
			0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x08,
			//	  ↑ ドラ
		},
		new byte[]{	//	07:混一、対々、三暗、三槓、混老、小三、役牌、ドラ（＋立直、一発）(Yamadata07.j)
			//配牌
			0x31, 0x31, 0x31, 0x31,		//自分
			0x11, 0x12, 0x13, 0x14,		//相手
			0x15, 0x16, 0x17, 0x18,
			0x01, 0x02, 0x03, 0x04,

			0x21, 0x21, 0x21, 0x21,		//自分
			0x05, 0x06, 0x07, 0x08,		//相手
			0x01, 0x02, 0x03, 0x04,
			0x05, 0x06, 0x07, 0x08,

			0x35, 0x35, 0x35, 0x35,		//自分
			0x11, 0x12, 0x13, 0x14,		//相手
			0x15, 0x16, 0x17, 0x18,
			0x06, 0x06, 0x08, 0x08,

			0x36,						//自分
			0x09,						//相手
			0x09,
			0x09,
			//未使用牌
			0x09, 0x19, 0x11, 0x12, 0x13, 0x04, 0x15, 0x16, 0x07, 0x18,
			0x19, 0x11, 0x12, 0x13, 0x04, 0x15, 0x16, 0x07, 0x18, 0x19,
			0x02, 0x22, 0x23, 0x24, 0x25, 0x26,
			//ツモ牌
			0x37, 0x28, 0x33, 0x02, 0x33, 0x33, 0x24, 0x25, 0x26, 0x27,
			0x28, 0x33, 0x14, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28,
			0x17, 0x14, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x03,
			0x01, 0x32, 0x22, 0x32, 0x17, 0x05, 0x27, 0x01, 0x32, 0x23,
			0x32, 0x03, 0x19, 0x37,
			//王牌
			0x29, 0x34, 0x29, 0x34, 0x37, 0x37, 0x36,
			0x29, 0x34, 0x29, 0x34, 0x05, 0x36, 0x36,
			//	  ↑ ドラ
		},
		new byte[]{	//	08:百万石？、一色四巡？(Yamadata08.j)
			//配牌
			0x07, 0x07, 0x07, 0x07,		//自分
			0x11, 0x12, 0x13, 0x14,		//相手
			0x15, 0x16, 0x17, 0x18,
			0x31, 0x32, 0x33, 0x34,

			0x08, 0x08, 0x08, 0x08,		//自分
			0x37, 0x36, 0x37, 0x03,		//相手
			0x31, 0x32, 0x33, 0x34,
			0x17, 0x11, 0x13, 0x03,

			0x09, 0x09, 0x09, 0x09,		//自分
			0x11, 0x16, 0x13, 0x14,		//相手
			0x15, 0x12, 0x17, 0x18,
			0x19, 0x36, 0x15, 0x36,

			0x06,						//自分
			0x35,						//相手
			0x35,
			0x19,
			//未使用牌
			0x19, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19,
			0x27, 0x28, 0x29, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27,
			0x21, 0x22, 0x23, 0x24, 0x25, 0x26,
			//ツモ牌
			0x06, 0x16, 0x01, 0x12, 0x01, 0x14, 0x05, 0x05, 0x06, 0x18,
			0x28, 0x29, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28,
			0x29, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29,
			0x31, 0x32, 0x33, 0x34, 0x35, 0x06, 0x37, 0x31, 0x32, 0x33,
			0x34, 0x35, 0x36, 0x37,
			//王牌
			0x01, 0x02, 0x03, 0x02, 0x04, 0x04, 0x05,
			0x01, 0x02, 0x03, 0x02, 0x04, 0x04, 0x05,
			//	  ↑ ドラ
		},
		new byte[]{	//	09:流し満貫（両方　和了できない）(Yamadata09.j)
			//配牌
			0x15, 0x16, 0x17, 0x18,		//自分
			0x04, 0x02, 0x03, 0x04,		//相手
			0x12, 0x05, 0x22, 0x23, 
			0x22, 0x25, 0x26, 0x27, 

			0x23, 0x26, 0x27, 0x28,		//自分
			0x05, 0x06, 0x07, 0x08,		//相手
			0x31, 0x32, 0x33, 0x34,
			0x12, 0x13, 0x03, 0x02,

			0x15, 0x16, 0x17, 0x18,		//自分
			0x06, 0x23, 0x08, 0x08,		//相手
			0x07, 0x28, 0x02, 0x02, 
			0x25, 0x14, 0x27, 0x24, 

			0x24,						//自分
			0x24,						//相手
			0x06,
			0x37,
			//未使用牌
			0x25, 0x26, 0x22, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18,
			0x27, 0x28, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x03,
			0x37, 0x22, 0x23, 0x24, 0x25, 0x26,
			//ツモ牌
			0x21, 0x11, 0x29, 0x21, 0x11, 0x09, 0x09, 0x01, 0x19, 0x19,
			0x29, 0x29, 0x21, 0x37, 0x21, 0x11, 0x09, 0x29, 0x31, 0x32,
			0x01, 0x09, 0x11, 0x19, 0x31, 0x32, 0x33, 0x34, 0x36, 0x36,
			0x33, 0x34, 0x35, 0x36, 0x31, 0x32, 0x33, 0x34, 0x01, 0x01,
			0x35, 0x35, 0x36, 0x35,
			//王牌
			0x03, 0x07, 0x13, 0x14, 0x04, 0x28, 0x19,
			0x05, 0x06, 0x07, 0x08, 0x04, 0x05, 0x37,
			//	  ↑ ドラ
		},
		new byte[]{	//	10:双方リーチ（両方　和了できないで流局）(Yamadata10.j)
			//配牌
			0x15, 0x16, 0x17, 0x18,		//自分
			0x04, 0x02, 0x03, 0x04,		//相手
			0x12, 0x05, 0x22, 0x23, 
			0x22, 0x25, 0x26, 0x27, 

			0x23, 0x26, 0x27, 0x28,		//自分
			0x05, 0x06, 0x07, 0x08,		//相手
			0x31, 0x32, 0x33, 0x34,
			0x12, 0x13, 0x03, 0x02,

			0x15, 0x16, 0x17, 0x18,		//自分
			0x06, 0x23, 0x08, 0x08,		//相手
			0x07, 0x28, 0x02, 0x02, 
			0x25, 0x14, 0x27, 0x24, 

			0x25,						//自分
			0x24,						//相手
			0x06,
			0x37,
			//未使用牌
			0x24, 0x26, 0x22, 0x11, 0x13, 0x14, 0x15, 0x16, 0x17, 0x19,
			0x27, 0x28, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x19, 0x03,
			0x37, 0x22, 0x23, 0x24, 0x25, 0x26,
			//ツモ牌
			0x21, 0x11, 0x29, 0x21, 0x12, 0x09, 0x09, 0x01, 0x18, 0x18,
			0x29, 0x29, 0x21, 0x37, 0x21, 0x11, 0x09, 0x29, 0x31, 0x32,
			0x01, 0x09, 0x11, 0x19, 0x31, 0x32, 0x33, 0x34, 0x36, 0x36,
			0x33, 0x34, 0x35, 0x36, 0x31, 0x32, 0x33, 0x34, 0x01, 0x01,
			0x35, 0x35, 0x36, 0x35,
			//王牌
			0x03, 0x07, 0x13, 0x14, 0x04, 0x28, 0x19,
			0x05, 0x06, 0x07, 0x08, 0x04, 0x05, 0x37,
			//	  ↑ ドラ
		},
		new byte[]{	//	11:親が２巡目　1500点和了(Yamadata11.j)
			//配牌
			0x15, 0x16, 0x17, 0x18,		//自分
			0x04, 0x02, 0x03, 0x14,		//相手
			0x12, 0x05, 0x22, 0x23, 
			0x22, 0x25, 0x26, 0x27, 

			0x23, 0x26, 0x27, 0x28,		//自分
			0x05, 0x06, 0x07, 0x08,		//相手
			0x31, 0x32, 0x33, 0x34,
			0x12, 0x13, 0x03, 0x02,

			0x15, 0x19, 0x17, 0x18,		//自分
			0x06, 0x23, 0x09, 0x08,		//相手
			0x07, 0x28, 0x02, 0x02, 
			0x25, 0x14, 0x27, 0x24, 

			0x21,						//自分
			0x26,						//相手
			0x06,
			0x37,
			//未使用牌
			0x24, 0x26, 0x29, 0x11, 0x13, 0x04, 0x15, 0x16, 0x17, 0x16,
			0x27, 0x28, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x19, 0x03,
			0x37, 0x22, 0x23, 0x24, 0x25, 0x25,
			//ツモ牌
			0x24, 0x11, 0x22, 0x21, 0x12, 0x08, 0x09, 0x01, 0x18, 0x18,
			0x29, 0x29, 0x21, 0x37, 0x21, 0x11, 0x09, 0x29, 0x31, 0x32,
			0x01, 0x09, 0x11, 0x19, 0x31, 0x32, 0x33, 0x34, 0x36, 0x36,
			0x33, 0x34, 0x35, 0x36, 0x31, 0x32, 0x33, 0x34, 0x01, 0x01,
			0x35, 0x35, 0x36, 0x35,
			//王牌
			0x03, 0x07, 0x13, 0x14, 0x04, 0x28, 0x19,
			0x05, 0x06, 0x07, 0x08, 0x04, 0x05, 0x37,
			//	  ↑ ドラ
		},
		new byte[]{	//	12:子が２巡目和了(西暗刻）(Yamadata12.j)
			//配牌
			0x15, 0x16, 0x17, 0x18,		//自分
			0x33, 0x33, 0x33, 0x04,		//相手
			0x12, 0x05, 0x22, 0x23, 
			0x22, 0x25, 0x26, 0x27, 

			0x23, 0x26, 0x27, 0x28,		//自分
			0x05, 0x06, 0x07, 0x08,		//相手
			0x31, 0x32, 0x33, 0x34,
			0x12, 0x13, 0x03, 0x02,

			0x11, 0x16, 0x19, 0x18,		//自分
			0x06, 0x23, 0x08, 0x08,		//相手
			0x07, 0x28, 0x02, 0x02, 
			0x25, 0x14, 0x27, 0x24, 

			0x24,						//自分
			0x21,						//相手
			0x06,
			0x37,
			//未使用牌
			0x25, 0x26, 0x24, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18,
			0x27, 0x28, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x03,
			0x37, 0x22, 0x23, 0x24, 0x25, 0x26,
			//ツモ牌
			0x21, 0x32, 0x29, 0x22, 0x11, 0x09, 0x09, 0x01, 0x17, 0x19,
			0x29, 0x29, 0x21, 0x37, 0x21, 0x11, 0x09, 0x29, 0x31, 0x15,
			0x01, 0x09, 0x11, 0x19, 0x31, 0x32, 0x02, 0x34, 0x36, 0x36,
			0x04, 0x34, 0x35, 0x36, 0x31, 0x32, 0x03, 0x34, 0x01, 0x01,
			0x35, 0x35, 0x36, 0x35,
			//王牌
			0x03, 0x07, 0x13, 0x14, 0x04, 0x28, 0x19,
			0x05, 0x06, 0x07, 0x08, 0x04, 0x05, 0x37,
			//	  ↑ ドラ
		},
		new byte[]{	//	13:親子ともに九種九牌(Yamadata13.j)
			//配牌
			0x01, 0x12, 0x11, 0x13,		//自分
			0x11, 0x09, 0x19, 0x14,		//相手
			0x11, 0x19, 0x17, 0x18,
			0x01, 0x19, 0x21, 0x04,

			0x21, 0x29, 0x31, 0x32,		//自分
			0x35, 0x36, 0x37, 0x08,		//相手
			0x31, 0x32, 0x33, 0x04,
			0x31, 0x32, 0x33, 0x08,

			0x33, 0x32, 0x35, 0x36,		//自分
			0x01, 0x29, 0x21, 0x34,		//相手
			0x35, 0x36, 0x37, 0x34,
			0x35, 0x36, 0x34, 0x33,

			0x37,						//自分
			0x09,						//相手
			0x09,
			0x09,
			//未使用牌
			0x03, 0x15, 0x22, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18,
			0x02, 0x16, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x03,
			0x03, 0x22, 0x23, 0x24, 0x25, 0x26,
			//ツモ牌
			0x21, 0x34, 0x12, 0x37, 0x11, 0x23, 0x24, 0x25, 0x26, 0x27,
			0x28, 0x29, 0x13, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28,
			0x29, 0x05, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x05,
			0x07, 0x28, 0x02, 0x02, 0x04, 0x04, 0x27, 0x05, 0x06, 0x07,
			0x14, 0x05, 0x06, 0x07,
			//王牌
			0x31, 0x07, 0x03, 0x18, 0x15, 0x16, 0x19,
			0x01, 0x02, 0x06, 0x06, 0x08, 0x08, 0x17,
			//	  ↑ ドラ
		},
		new byte[]{	//14:子がチー（４筒切り）(Yamadata14.j)
			//配牌
			0x15, 0x16, 0x17, 0x18,		//自分
			0x04, 0x02, 0x03, 0x14,		//相手
			0x12, 0x05, 0x22, 0x23, 
			0x22, 0x25, 0x26, 0x27, 

			0x23, 0x26, 0x27, 0x28,		//自分
			0x05, 0x06, 0x07, 0x08,		//相手
			0x31, 0x32, 0x33, 0x34,
			0x12, 0x13, 0x03, 0x02,

			0x15, 0x19, 0x17, 0x18,		//自分
			0x06, 0x23, 0x09, 0x08,		//相手
			0x07, 0x28, 0x02, 0x02, 
			0x25, 0x14, 0x27, 0x24, 

			0x21,						//自分
			0x25,						//相手
			0x06,
			0x37,
			//未使用牌
			0x24, 0x26, 0x29, 0x11, 0x13, 0x04, 0x15, 0x16, 0x17, 0x16,
			0x27, 0x28, 0x12, 0x13, 0x14, 0x15, 0x16, 0x21, 0x19, 0x03,
			0x37, 0x22, 0x23, 0x24, 0x25, 0x26,
			//ツモ牌
			0x24, 0x11, 0x17, 0x22, 0x12, 0x08, 0x09, 0x01, 0x18, 0x18,
			0x29, 0x29, 0x21, 0x37, 0x21, 0x11, 0x09, 0x29, 0x31, 0x32,
			0x01, 0x09, 0x11, 0x19, 0x31, 0x32, 0x33, 0x34, 0x36, 0x36,
			0x33, 0x34, 0x35, 0x36, 0x31, 0x32, 0x33, 0x34, 0x01, 0x01,
			0x35, 0x35, 0x36, 0x35,
			//王牌
			0x03, 0x07, 0x13, 0x14, 0x04, 0x28, 0x19,
			0x05, 0x06, 0x07, 0x08, 0x04, 0x05, 0x37,
			//	  ↑ ドラ
		},
		new byte[]{	//15:子が２巡目和了(南が暗刻）(Yamadata15.j)
			//配牌
			0x15, 0x16, 0x17, 0x18,		//自分
			0x32, 0x32, 0x32, 0x04,		//相手
			0x12, 0x05, 0x22, 0x23, 
			0x22, 0x25, 0x26, 0x27, 

			0x23, 0x26, 0x27, 0x28,		//自分
			0x05, 0x06, 0x07, 0x08,		//相手
			0x31, 0x32, 0x33, 0x34,
			0x12, 0x13, 0x03, 0x02,

			0x11, 0x16, 0x19, 0x18,		//自分
			0x06, 0x23, 0x08, 0x08,		//相手
			0x07, 0x28, 0x02, 0x02, 
			0x25, 0x14, 0x27, 0x24, 

			0x24,						//自分
			0x21,						//相手
			0x06,
			0x37,
			//未使用牌
			0x25, 0x26, 0x24, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18,
			0x27, 0x28, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x03,
			0x37, 0x22, 0x23, 0x24, 0x25, 0x26,
			//ツモ牌
			0x21, 0x33, 0x29, 0x22, 0x11, 0x09, 0x09, 0x01, 0x17, 0x19,
			0x29, 0x29, 0x21, 0x37, 0x21, 0x11, 0x09, 0x29, 0x31, 0x15,
			0x01, 0x09, 0x11, 0x19, 0x31, 0x33, 0x02, 0x34, 0x36, 0x36,
			0x04, 0x34, 0x35, 0x36, 0x31, 0x33, 0x03, 0x34, 0x01, 0x01,
			0x35, 0x35, 0x36, 0x35,
			//王牌
			0x03, 0x07, 0x13, 0x14, 0x04, 0x28, 0x19,
			0x05, 0x06, 0x07, 0x08, 0x04, 0x05, 0x37,
			//	  ↑ ドラ
		},
		new byte[]{	//16:緑一色(發無し）(Yamadata16.j)
			//配牌
			0x16, 0x16, 0x16, 0x18,		//自分
			0x32, 0x32, 0x32, 0x04,		//相手
			0x16, 0x05, 0x22, 0x23, 
			0x22, 0x25, 0x26, 0x27, 

			0x18, 0x12, 0x12, 0x12,		//自分
			0x05, 0x06, 0x07, 0x08,		//相手
			0x31, 0x32, 0x33, 0x34,
			0x26, 0x19, 0x03, 0x02,

			0x13, 0x12, 0x13, 0x18,		//自分
			0x06, 0x23, 0x08, 0x08,		//相手
			0x07, 0x28, 0x02, 0x02, 
			0x25, 0x24, 0x27, 0x24, 

			0x14,						//自分
			0x21,						//相手
			0x06,
			0x37,
			//未使用牌
			0x25, 0x26, 0x24, 0x27, 0x11, 0x22, 0x15, 0x15, 0x17, 0x18,
			0x27, 0x28, 0x28, 0x33, 0x29, 0x15, 0x17, 0x17, 0x23, 0x03,
			0x37, 0x22, 0x23, 0x24, 0x25, 0x26,
			//ツモ牌
			0x14, 0x13, 0x14, 0x14, 0x13, 0x09, 0x09, 0x01, 0x17, 0x19,
			0x29, 0x29, 0x21, 0x37, 0x21, 0x11, 0x09, 0x29, 0x31, 0x15,
			0x01, 0x09, 0x11, 0x19, 0x31, 0x33, 0x02, 0x34, 0x36, 0x36,
			0x04, 0x34, 0x35, 0x36, 0x31, 0x33, 0x03, 0x34, 0x01, 0x01,
			0x35, 0x35, 0x36, 0x35,
			//王牌
			0x03, 0x07, 0x11, 0x21, 0x04, 0x28, 0x19,
			0x05, 0x06, 0x07, 0x08, 0x04, 0x05, 0x37,
			//	  ↑ ドラ
		},
		new byte[]{	//17:子が九種九牌(Yamadata17.j)
			//配牌
			0x22, 0x12, 0x11, 0x13,		//自分
			0x11, 0x09, 0x19, 0x14,		//相手
			0x11, 0x19, 0x17, 0x18,
			0x01, 0x19, 0x21, 0x04,

			0x23, 0x35, 0x12, 0x33,		//自分
			0x35, 0x36, 0x37, 0x08,		//相手
			0x31, 0x32, 0x33, 0x04,
			0x31, 0x32, 0x33, 0x08,

			0x14, 0x15, 0x17, 0x17,		//自分
			0x01, 0x29, 0x21, 0x34,		//相手
			0x35, 0x36, 0x37, 0x34,
			0x35, 0x36, 0x34, 0x33,

			0x22,						//自分
			0x09,						//相手
			0x09,
			0x09,
			//未使用牌
			0x03, 0x15, 0x37, 0x31, 0x32, 0x13, 0x32, 0x24, 0x35, 0x18,
			0x02, 0x16, 0x12, 0x13, 0x14, 0x15, 0x16, 0x16, 0x18, 0x03,
			0x03, 0x01, 0x21, 0x29, 0x11, 0x26,
			//ツモ牌
			0x21, 0x22, 0x12, 0x37, 0x25, 0x23, 0x24, 0x25, 0x26, 0x27,
			0x28, 0x29, 0x13, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28,
			0x29, 0x05, 0x34, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x05,
			0x07, 0x28, 0x02, 0x02, 0x04, 0x04, 0x27, 0x05, 0x06, 0x07,
			0x14, 0x05, 0x06, 0x07,
			//王牌
			0x31, 0x07, 0x03, 0x18, 0x15, 0x16, 0x19,
			0x01, 0x02, 0x06, 0x06, 0x08, 0x08, 0x17,
			//	  ↑ ドラ
		},
		new byte[]{	//18:裏データ確認用（三人目四人目が配牌でテンパイ）(Yamadata18.j)
			//配牌
			0x15, 0x12, 0x16, 0x13,		//自分
			0x11, 0x06, 0x02, 0x14,		//相手
			0x01, 0x11, 0x21, 0x31,
			0x01, 0x02, 0x03, 0x04,

			0x17, 0x03, 0x18, 0x32,		//自分
			0x15, 0x16, 0x17, 0x08,		//相手
			0x09, 0x19, 0x29, 0x32,
			0x05, 0x06, 0x07, 0x08,

			0x33, 0x32, 0x35, 0x36,		//自分
			0x06, 0x29, 0x21, 0x18,		//相手
			0x35, 0x36, 0x37, 0x34,
			0x01, 0x01, 0x09, 0x09,

			0x37,						//自分
			0x08,						//相手
			0x33,
			0x09,
			//未使用牌
			0x03, 0x19, 0x22, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18,
			0x19, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x03,
			0x21, 0x22, 0x23, 0x24, 0x25, 0x26,
			//ツモ牌
			0x21, 0x34, 0x12, 0x37, 0x11, 0x23, 0x24, 0x25, 0x26, 0x27,
			0x28, 0x29, 0x13, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28,
			0x29, 0x05, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x05,
			0x07, 0x28, 0x02, 0x02, 0x04, 0x04, 0x27, 0x31, 0x32, 0x08,
			0x14, 0x05, 0x06, 0x07,
			//王牌
			0x04, 0x07, 0x33, 0x34, 0x35, 0x36, 0x19,
			0x31, 0x31, 0x33, 0x34, 0x35, 0x36, 0x37,
			//	  ↑ ドラ
		},
		new byte[]{	//19:親はリーチで子は九種九牌(Yamadata19.j)
			//配牌
			0x22, 0x12, 0x11, 0x13,		//自分
			0x11, 0x09, 0x19, 0x14,		//相手
			0x11, 0x19, 0x17, 0x18,
			0x01, 0x19, 0x21, 0x04,

			0x23, 0x24, 0x12, 0x13,		//自分
			0x35, 0x36, 0x37, 0x08,		//相手
			0x31, 0x32, 0x33, 0x04,
			0x31, 0x32, 0x33, 0x08,

			0x14, 0x15, 0x16, 0x17,		//自分
			0x01, 0x29, 0x21, 0x34,		//相手
			0x35, 0x36, 0x37, 0x34,
			0x35, 0x36, 0x34, 0x33,

			0x22,						//自分
			0x09,						//相手
			0x09,
			0x09,
			//未使用牌
			0x03, 0x15, 0x37, 0x31, 0x32, 0x33, 0x32, 0x35, 0x35, 0x18,
			0x02, 0x16, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x03,
			0x03, 0x01, 0x21, 0x29, 0x11, 0x26,
			//ツモ牌
			0x21, 0x22, 0x12, 0x37, 0x25, 0x23, 0x24, 0x25, 0x26, 0x27,
			0x28, 0x29, 0x13, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28,
			0x29, 0x05, 0x34, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x05,
			0x07, 0x28, 0x02, 0x02, 0x04, 0x04, 0x27, 0x05, 0x06, 0x07,
			0x14, 0x05, 0x06, 0x07,
			//王牌
			0x31, 0x07, 0x03, 0x18, 0x15, 0x16, 0x19,
			0x01, 0x02, 0x06, 0x06, 0x08, 0x08, 0x17,
			//	  ↑ ドラ
		},
		new byte[]{	//20:子が２巡目和了(東が暗刻）(Yamadata20.j)
			//配牌
			0x15, 0x16, 0x17, 0x18,		//自分
			0x31, 0x31, 0x31, 0x04,		//相手
			0x12, 0x05, 0x22, 0x23, 
			0x22, 0x25, 0x26, 0x27, 

			0x23, 0x26, 0x27, 0x28,		//自分
			0x05, 0x06, 0x07, 0x08,		//相手
			0x32, 0x32, 0x33, 0x34,
			0x12, 0x13, 0x03, 0x02,

			0x11, 0x16, 0x19, 0x18,		//自分
			0x06, 0x23, 0x08, 0x08,		//相手
			0x07, 0x28, 0x02, 0x02, 
			0x25, 0x14, 0x27, 0x24, 

			0x24,						//自分
			0x21,						//相手
			0x06,
			0x37,
			//未使用牌
			0x25, 0x26, 0x24, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18,
			0x27, 0x28, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x03,
			0x37, 0x22, 0x23, 0x24, 0x25, 0x26,
			//ツモ牌
			0x21, 0x33, 0x29, 0x22, 0x11, 0x09, 0x09, 0x01, 0x17, 0x19,
			0x29, 0x29, 0x21, 0x37, 0x21, 0x11, 0x09, 0x29, 0x32, 0x15,
			0x01, 0x09, 0x11, 0x19, 0x32, 0x33, 0x02, 0x34, 0x36, 0x36,
			0x04, 0x34, 0x35, 0x36, 0x31, 0x33, 0x03, 0x34, 0x01, 0x01,
			0x35, 0x35, 0x36, 0x35,
			//王牌
			0x03, 0x07, 0x13, 0x14, 0x04, 0x28, 0x19,
			0x05, 0x06, 0x07, 0x08, 0x04, 0x05, 0x37,
			//	  ↑ ドラ
		},
		new byte[]{	//21:親が２巡目和了（東暗刻）(Yamadata21.j)
			//配牌
			0x31, 0x31, 0x31, 0x18,		//自分
			0x04, 0x02, 0x03, 0x14,		//相手
			0x12, 0x05, 0x22, 0x23, 
			0x22, 0x25, 0x26, 0x27, 

			0x23, 0x26, 0x27, 0x28,		//自分
			0x05, 0x06, 0x07, 0x08,		//相手
			0x31, 0x32, 0x33, 0x34,
			0x12, 0x13, 0x03, 0x02,

			0x15, 0x19, 0x17, 0x18,		//自分
			0x06, 0x23, 0x09, 0x08,		//相手
			0x07, 0x28, 0x02, 0x02, 
			0x25, 0x14, 0x27, 0x24, 

			0x21,						//自分
			0x26,						//相手
			0x06,
			0x37,
			//未使用牌
			0x24, 0x26, 0x29, 0x11, 0x13, 0x04, 0x15, 0x16, 0x17, 0x16,
			0x27, 0x28, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x19, 0x03,
			0x37, 0x22, 0x23, 0x24, 0x25, 0x25,
			//ツモ牌
			0x24, 0x11, 0x22, 0x21, 0x12, 0x08, 0x09, 0x01, 0x18, 0x18,
			0x29, 0x29, 0x21, 0x37, 0x21, 0x11, 0x09, 0x29, 0x15, 0x32,
			0x01, 0x09, 0x11, 0x19, 0x16, 0x32, 0x33, 0x34, 0x36, 0x36,
			0x33, 0x34, 0x35, 0x36, 0x17, 0x32, 0x33, 0x34, 0x01, 0x01,
			0x35, 0x35, 0x36, 0x35,
			//王牌
			0x03, 0x07, 0x13, 0x14, 0x04, 0x28, 0x19,
			0x05, 0x06, 0x07, 0x08, 0x04, 0x05, 0x37,
			//	  ↑ ドラ
		},
		new byte[]{	//22:裏データ確認用（三人目四人目が配牌でテンパイ）(Yamadata22.j)
			//配牌
			0x15, 0x12, 0x16, 0x13,		//自分
			0x11, 0x06, 0x02, 0x14,		//相手
			0x01, 0x02, 0x03, 0x32,
			0x31, 0x31, 0x31, 0x04,

			0x17, 0x03, 0x18, 0x29,		//自分
			0x15, 0x16, 0x17, 0x08,		//相手
			0x18, 0x19, 0x32, 0x32,
			0x05, 0x06, 0x07, 0x08,

			0x33, 0x32, 0x35, 0x36,		//自分
			0x06, 0x29, 0x21, 0x18,		//相手
			0x17, 0x36, 0x36, 0x22,
			0x01, 0x01, 0x09, 0x09,

			0x37,						//自分
			0x08,						//相手
			0x23,
			0x09,
			//未使用牌
			0x21, 0x19, 0x34, 0x12, 0x13, 0x14, 0x15, 0x16, 0x35, 0x09,
			0x19, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x03,
			0x37, 0x22, 0x33, 0x24, 0x25, 0x26,
			//ツモ牌
			0x12, 0x34, 0x21, 0x21, 0x11, 0x24, 0x24, 0x25, 0x26, 0x27,
			0x28, 0x29, 0x13, 0x22, 0x23, 0x23, 0x25, 0x26, 0x27, 0x28,
			0x29, 0x05, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x05,
			0x07, 0x28, 0x11, 0x02, 0x04, 0x04, 0x27, 0x03, 0x31, 0x08,
			0x14, 0x05, 0x06, 0x07,
			//王牌
			0x04, 0x07, 0x33, 0x34, 0x35, 0x37, 0x19,
			0x01, 0x02, 0x33, 0x34, 0x35, 0x36, 0x37,
			//	  ↑ ドラ
		},
		new byte[]{	//23:裏データ確認用（こちらが捨牌後、相手が牌を取ったところで停止）(Yamadata23.j)// 思考が止まるバグの確認用:敵一巡目
			//配牌
			0x07, 0x08, 0x08, 0x09,		//自分
			0x01, 0x01, 0x01, 0x08,		//相手
			0x15, 0x16, 0x17, 0x18,
			0x03, 0x02, 0x03, 0x04,

			0x09, 0x13, 0x14, 0x15,		//自分
			0x09, 0x09, 0x15, 0x25,		//相手
			0x12, 0x02, 0x03, 0x04,
			0x05, 0x06, 0x07, 0x04,

			0x22, 0x23, 0x24, 0x33,		//自分
			0x32, 0x32, 0x32, 0x37,		//相手
			0x06, 0x16, 0x17, 0x18,
			0x12, 0x13, 0x14, 0x08,

			0x33,						//自分
			0x37,						//相手
			0x19,
			0x19,
			//未使用牌
			0x27, 0x28, 0x29, 0x21, 0x07, 0x01, 0x02, 0x02, 0x26, 0x27,
			0x19, 0x11, 0x12, 0x13, 0x14, 0x07, 0x16, 0x17, 0x18, 0x19,
			0x21, 0x22, 0x23, 0x24, 0x25, 0x26,
			//ツモ牌
			0x35, 0x37, 0x05, 0x06, 0x06, 0x14, 0x15, 0x16, 0x17, 0x18,
			0x28, 0x29, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28,
			0x29, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29,
			0x05, 0x11, 0x03, 0x34, 0x35, 0x11, 0x13, 0x31, 0x12, 0x11,
			0x34, 0x35, 0x36, 0x05,
			//王牌
			0x31, 0x31, 0x36, 0x34, 0x33, 0x36, 0x04,
			0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37,
			//	  ↑ ドラ
		},
		new byte[]{	//24:(Yamadata24.j)// カン後のリンシャンツモ確認用
			//配牌
			0x06, 0x06, 0x16, 0x17,		//自分
			0x01, 0x02, 0x03, 0x04,		//相手
			0x15, 0x08, 0x09, 0x18,
			0x01, 0x05, 0x03, 0x08,

			0x18, 0x27, 0x29, 0x36,		//自分
			0x05, 0x06, 0x12, 0x13,		//相手
			0x15, 0x02, 0x03, 0x04,
			0x35, 0x07, 0x07, 0x09,

			0x36, 0x36, 0x19, 0x19,		//自分
			0x14, 0x21, 0x22, 0x23,		//相手
			0x08, 0x16, 0x17, 0x09,
			0x12, 0x25, 0x33, 0x08,

			0x24,						//自分
			0x19,						//相手
			0x32,
			0x33,
			//未使用牌
			0x13, 0x14, 0x14, 0x09, 0x07, 0x16, 0x02, 0x02, 0x26, 0x27,
			0x37, 0x15, 0x12, 0x13, 0x06, 0x07, 0x16, 0x17, 0x18, 0x19,
			0x32, 0x32, 0x37, 0x24, 0x25, 0x26,
			//ツモ牌
			0x11, 0x31, 0x32, 0x04, 0x33, 0x14, 0x36, 0x37, 0x17, 0x18,
			0x28, 0x29, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28,
			0x29, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x35, 0x29,
			0x05, 0x11, 0x03, 0x34, 0x22, 0x11, 0x13, 0x01, 0x12, 0x11,
			0x34, 0x15, 0x35, 0x05,
			//王牌
			0x31, 0x31, 0x35, 0x34, 0x01, 0x28, 0x04,
			0x31, 0x21, 0x33, 0x34, 0x23, 0x28, 0x37,
			//	  ↑ ドラ
		},
		new byte[]{	//25:最初にリーチをせずカン後 ウィンドウが出ないバージョン(Yamadata25.j)// カン後のリンシャンツモ確認用
			//配牌
			0x06, 0x06, 0x16, 0x17,		//自分
			0x01, 0x02, 0x03, 0x04,		//相手
			0x15, 0x08, 0x09, 0x18,
			0x01, 0x05, 0x03, 0x08,

			0x18, 0x27, 0x29, 0x36,		//自分
			0x05, 0x06, 0x12, 0x13,		//相手
			0x15, 0x02, 0x03, 0x04,
			0x35, 0x07, 0x07, 0x09,

			0x36, 0x36, 0x19, 0x19,		//自分
			0x14, 0x21, 0x22, 0x23,		//相手
			0x28, 0x16, 0x17, 0x09,
			0x12, 0x25, 0x33, 0x08,

			0x19,						//自分
			0x36,						//相手
			0x32,
			0x33,
			//未使用牌
			0x28, 0x14, 0x14, 0x09, 0x07, 0x16, 0x02, 0x02, 0x26, 0x27,
			0x37, 0x15, 0x12, 0x13, 0x13, 0x07, 0x16, 0x17, 0x18, 0x24,
			0x32, 0x32, 0x37, 0x24, 0x25, 0x26,
			//ツモ牌
			0x11, 0x31, 0x32, 0x04, 0x33, 0x14, 0x06, 0x37, 0x17, 0x18,
			0x08, 0x29, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x19,
			0x29, 0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x35, 0x29,
			0x05, 0x11, 0x03, 0x34, 0x22, 0x11, 0x13, 0x01, 0x12, 0x11,
			0x34, 0x15, 0x35, 0x05,
			//王牌
			0x31, 0x31, 0x35, 0x34, 0x01, 0x28, 0x04,
			0x31, 0x21, 0x33, 0x34, 0x23, 0x28, 0x37,
			//	  ↑ ドラ
		},
		new byte[]{	// 26:7の変形版:三槓後リンシャンツモ(Yamadata26.j)// カン後のリンシャンツモ確認用
			//配牌
			0x31, 0x31, 0x31, 0x31,		//自分
			0x11, 0x12, 0x13, 0x14,		//相手
			0x15, 0x16, 0x17, 0x18,
			0x01, 0x02, 0x03, 0x04,

			0x21, 0x21, 0x21, 0x21,		//自分
			0x05, 0x06, 0x07, 0x08,		//相手
			0x01, 0x02, 0x03, 0x04,
			0x05, 0x06, 0x07, 0x08,

			0x35, 0x35, 0x35, 0x35,		//自分
			0x11, 0x12, 0x13, 0x14,		//相手
			0x15, 0x16, 0x17, 0x18,
			0x06, 0x06, 0x08, 0x08,
			0x36,						//自分
			0x09,						//相手
			0x09,
			0x09,
			//未使用牌
			0x09, 0x19, 0x11, 0x12, 0x13, 0x04, 0x15, 0x16, 0x07, 0x18,
			0x19, 0x11, 0x12, 0x13, 0x04, 0x15, 0x16, 0x07, 0x18, 0x19,
			0x02, 0x22, 0x23, 0x24, 0x25, 0x26,
			//ツモ牌
			0x34, 0x28, 0x33, 0x02, 0x33, 0x33, 0x24, 0x25, 0x26, 0x27,
			0x28, 0x33, 0x14, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28,
			0x17, 0x14, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x03,
			0x01, 0x32, 0x22, 0x32, 0x17, 0x05, 0x27, 0x01, 0x32, 0x23,
			0x32, 0x03, 0x19, 0x37,
			//王牌
			0x29, 0x37, 0x29, 0x34, 0x37, 0x34, 0x36,
			0x29, 0x37, 0x29, 0x34, 0x05, 0x36, 0x36,
			//	  ↑ ドラ
		},
		new byte[]{	//27:(Yamadata27.j)// 加カンで一瞬出るメニューのバグ確認用
			// 7の変形版:対々・ドラ8・役牌4・混老頭・混一色・三槓子の数え役満、又は四槓子
			// 2,3番目の加カン時にメニューウィンドウが出る不具合の確認用

			// 南・八筒・一索・五萬は捨ててください
			// 加カンにするために、最初はポンしてください
			// 最後に發を捨てて北をツモると数え役満
			0x31, 0x31, 0x31, 0x28,		// 自分
			0x31, 0x12, 0x13, 0x14,		// 相手
			0x15, 0x16, 0x17, 0x18,
			0x01, 0x02, 0x03, 0x04,

			0x21, 0x21, 0x21, 0x05,		// 自分
			0x21, 0x33, 0x33, 0x08,		// 相手
			0x01, 0x02, 0x03, 0x04,
			0x05, 0x06, 0x07, 0x08,

			0x35, 0x35, 0x35, 0x11,		// 自分
			0x35, 0x12, 0x13, 0x14,		// 相手
			0x15, 0x16, 0x17, 0x18,
			0x06, 0x06, 0x08, 0x08,
			0x36,						// 自分
			0x09,						// 相手
			0x09,
			0x09,

			//未使用牌
			0x09, 0x19, 0x11, 0x12, 0x13, 0x04, 0x15, 0x16, 0x07, 0x18,
			0x19, 0x11, 0x12, 0x13, 0x04, 0x15, 0x16, 0x07, 0x18, 0x19,
			0x02, 0x22, 0x23, 0x26, 0x25, 0x24,
			//ツモ牌
			0x32, 0x07, 0x28, 0x06, 0x02, 0x34, 0x25, 0x34, 0x26, 0x27,
			0x11, 0x33, 0x14, 0x22, 0x23, 0x33, 0x24, 0x26, 0x27, 0x28,
			0x17, 0x14, 0x22, 0x24, 0x23, 0x25, 0x26, 0x27, 0x28, 0x03,
			0x01, 0x32, 0x22, 0x32, 0x17, 0x05, 0x27, 0x01, 0x32, 0x23,
			0x34, 0x03, 0x19, 0x37,
			//王牌
			0x29, 0x29, 0x37, 0x37, 0x25, 0x34, 0x24, // ここ
			0x29, 0x37, 0x29, 0x36, 0x05, 0x36, 0x36,
		},
#region UNITY_ORIGINAL
		new byte[]{	//28:
			0x22, 0x22, 0x23, 0x23,		// 自分
			0x08, 0x08, 0x08, 0x08,		// 相手
			0x21, 0x37, 0x21, 0x32,	
			0x01, 0x31, 0x31, 0x31,

			0x24, 0x24, 0x25, 0x25,		// 自分
			0x36, 0x07, 0x18, 0x24,		// 相手
			0x11, 0x02, 0x03, 0x04,
			0x02, 0x03, 0x04, 0x17,			

			0x26, 0x26, 0x27, 0x27,		// 自分
			0x19, 0x12, 0x15, 0x14,		// 相手
			0x12, 0x15, 0x07, 0x18,
			0x33, 0x13, 0x33, 0x32,
			0x28,						// 自分
			0x11,						// 相手
			0x21,
			0x05,

			//未使用牌
			0x19, 0x05, 0x09, 0x12, 0x13, 0x04, 0x13, 0x16, 0x36, 0x18,
			0x06, 0x11, 0x12, 0x13, 0x04, 0x15, 0x16, 0x06, 0x18, 0x06,
			0x02, 0x37, 0x09, 0x17, 0x34, 0x34,
			//ツモ牌
			0x33, 0x35, 0x31, 0x35, 0x28, 0x28, 0x28, 0x25, 0x06, 0x27,
			0x11, 0x11, 0x14, 0x05, 0x23, 0x33, 0x24, 0x26, 0x27, 0x19,
			0x17, 0x14, 0x22, 0x16, 0x23, 0x34, 0x26, 0x35, 0x01, 0x03,
			0x01, 0x16, 0x22, 0x32, 0x07, 0x32, 0x15, 0x01, 0x37, 0x09,
			0x35, 0x03, 0x19, 0x07,
			//王牌
			0x29, 0x29, 0x14, 0x09, 0x25, 0x34, 0x05, // ここ
			0x29, 0x21, 0x29, 0x02, 0x17, 0x36, 0x36,
		},
#endregion //-*UNITY_ORIGINAL


	};
#endregion //-*YAMADATA_J

#region YAMASORTDATA_J
	public static YK[] yakuData = new YK[]{
	//＜通常時＞
		YK.RICHI,		/* 立直 リーチ*/
		YK.WRICH,		/* ダブル立直 リーチ*/
		YK.IPPAT,		/* 一発       */
		YK.MENZE,		/* 面前清ツモ */
		YK.HONIS,		/* 混一色     */
		YK.CHINI,		/* 清一色     */
		YK.HONRO,		/* 混老頭     */
		YK.TOITO,		/* 対々和     */
		YK.SANAN,		/* 三暗刻     */
		YK.SANKA,		/* 三カン子   */
		YK.SHOSA,		/* 小三元     */
		YK.DOPON,		/* 三色同刻   */
		YK.PINHO,		/* 平和       */
		YK.TANYO,		/* 断ヤオ     */
		YK.CHANT,		/* チャンタ   */
		YK.JUNCH,		/* 純チャン   */
		YK.CHITO,		/* 七対子     */
		YK.SANSH,		/* 三色同順   */
		YK.ITSU,		/* 一気通貫   */
		YK.IPEKO,		/* 一盃口     */
		YK.RYANP,		/* 二盃口     */
		YK.CHANK,		/* 搶カン 槍槓     */
		YK.HAITE,		/* 海底       */
		YK.HOTEI,		/* 河底       */
		YK.RINSH,		/* 嶺山開花   */
		YK.FANPA,		/* 飜牌       */
		YK.DORA,		/* ドラ       */

	//	＜役満時＞
		YK.TENHO,		/* 天和       */
		YK.CHIHO,		/* 地和       */
		YK.RENHO,		/* 人和       */
		YK.TSUANK,		// 四暗刻単騎
		YK.SUANK,		/* 四暗刻     */
		YK.SUKAN,		/* 四カン子 四槓子*/
		YK.DAISU,		/* 大四喜     */
		YK.SHOSU,		/* 小四喜     */
		YK.DAISA,		/* 大三元     */
		YK.TSUIS,		/* 字一色     */
		YK.RYUIS,		/* 緑一色     */
		YK.CHINR,		/* 清老頭     */
		YK.JKOKUS,		// 純正国士無双
		YK.KOKUS,		/* 国士無双   */
		YK.JCHURE,		// 純正九連宝燈
		YK.CHURE,		/* 九連宝燈   */
	};
#endregion //-*YAMASORTDATA_J
}
	
#region GAMEDEFS_JAVA
		public class GAMEDATA {
			public byte	byGameMode;							/*	ゲームモード				*/
			public byte	byRuleNo;							/*	ルール番号					*/
			public byte	byFlags;							/*	フラグ						*/
			public byte	byOptionFlags;						/*	オプションフラグ			*/
			public byte    byGuide;
		//} GAMEDATA	__attribute__((aligned(16)));
		};

#endregion	//-*GAMEDEFS_JAVA

#region MJDEF_H
	/// <summary>
	/// ゲーム画面モード番号
	/// </summary>
	public enum INMJMODE:int{
		D_LOGO_MODE,						// ロゴ　描画中
		D_TITLE_MODE,						// タイトル　表示中
		D_DEBUG_MODE,						// デバッグモード。

		D_ASK_RETRY_CONNECT_MODE,			// 通信　再接続ダイアログ	2006/02/18 要望No.41

		D_NET_MENU,							// 通信  メニュー
		D_NET_RETRY_CONNECT_MODE,			// 通信　再接続描画。
		D_NET_RETRY_CONNECT_WAIT_MODE,		// 通信　再接続待ち。
		D_NET_RETRY_CONNECT_READY_MODE,
		D_NET_FREE_ENTRY_START_MODE,		// 通信　フリー対戦　エントリーMPS接続開始
		D_NET_FREE_ENTRY_WAIT_MODE,			// 通信　フリー対戦　エントリーMPS接続待ち
		D_NET_FREE_ENTRY_REQ_MODE,			// 通信　フリー対戦　エントリー要求送信
		D_NET_FREE_ENTRY_RECV_MODE,			// 通信　フリー対戦　エントリー応答待ち

		D_NET_FREE_LOGIN_START_MODE,		// 通信　フリー対戦　ログインMPS接続開始
		D_NET_FREE_LOGIN_WAIT_MODE,			// 通信　フリー対戦　ログインMPS接続待ち
		D_NET_FREE_LOGIN_REQ_MODE,			// 通信　フリー対戦　ログイン要求送信
		D_NET_FREE_LOGIN_RECV_MODE,			// 通信　フリー対戦　ログイン応答待ち

		D_NET_IMAGE_DOWNLOAD_MODE,			// 通信  顔イメージダウンロード

		D_NET_FREE_GAME_ENTRY_WAIT_MODE,	// 通信　フリー対戦　参加登録待ち中
		D_NET_FREE_MEMBER_WAIT_MODE,		// 通信　フリー対戦　参加者待ち中
		D_NET_FREE_MEMBER_WAIT_TO_MODE,		// 通信　フリー対戦　参加者待ちタイムアウト
		D_NET_FREE_GAME_START_WAIT_MODE,	// 通信　フリー対戦　対戦開始待ち
		D_NET_FREE_KYOKU_START_WAIT_MODE,	// 通信　フリー対戦　対局開始待ち（ゲーム開始時）
		D_NET_FREE_KYOKU_START_WAIT_MODE2,	// 通信　フリー対戦　対局開始待ち（次局開始時）

		D_NET_MATCHING_TIMEOUT_MODE,		// 通信　マッチングタイムアウト	2006/02/08 要望No.35


		D_NET_RE_CONNECT_MODE,				// 通信　再接続処理モード。

		// 2005/11/28 motohashi
		D_NET_TABLE_NUMBER_INPUT_MODE,		// 通信　卓指定対戦　卓番号入力。
		D_NET_RESERVE_GAME_ENTRY_WAIT_MODE,	// 通信　卓指定対戦　参加登録中

		D_NET_FREE_GAME_REINIT_MODE,		// 通信　フリー対戦　対戦再初期化中
		D_NET_FREE_GAME_MODE,				// 通信　フリー対戦　対戦中

		D_NET_RESERVE_GAME_MODE,			// 通信　卓指定対戦　対戦中

		D_NET_RANK_DRAW_MODE,				// 通信　段位認定戦
		D_NET_RANK_RESULT_DRAW_MODE,		// 通信　段位認定戦結果
		D_NET_RANK_STATUS_DRAW_MODE,		// 通信　段位情報表示 2006/02/17 要望No.13
		D_NET_RANKING_DRAW_MODE,			// 通信　総合順位描画。

		D_NET_NEXT_RANK_DRAW_MODE,			// 通信  次の段位までのポイント表示

		D_NET_DISCONNECT_MODE,				// 通信　接続が切れた・再接続成功。

		D_COM_MENU,							// COMメニュー
		D_OPTION_SET_MODE,					// オプション設定モード。
		D_FREE_RULE_MODE,					// フリー対戦　ルール設定中
		D_FREE_RULE_EDIT_MODE,				// フリー対戦　ルール変更中
		D_FREE_NPC_SELLECT_MODE,			// フリー対戦　対戦相手設定中
		D_FREE_NPC_CHK_MODE,				// フリー対戦　対戦相手確認中

		D_FREE_GAME_REINIT_MODE,			// フリー対戦　対戦再初期化中
		D_FREE_GAME_MODE,					// フリー対戦　対戦中

		D_SURVIVAL_LOAD_MODE,				// サバイバル対戦　セーブデータからの分岐。
		D_SURIVIVAL_OPENING_MODE,			// サバイバル対戦　オープニング
		D_SURIVIVAL_RULE_CHK_MODE,			// サバイバル対戦　ルールチェック
		D_SURIVIVAL_OPENING_NO2_MODE,		// サバイバル対戦　オープニング（ルール確認後）
		D_SURIVIVAL_COMMENT_MODE,			// サバイバル対戦　コメント
		D_SURVIVAL_KYOKU_LOAD_MODE,
		D_SURIVIVAL_RESULT_MODE,			// サバイバル対戦　結果
		D_SURVIVAL_TOCYU_SAVE_MODE,			// サバイバル対戦　途中結果のセーブ。
		D_SURIVIVAL_SAVE_MODE,				// サバイバル対戦　保存
		D_SURIVIVAL_GET_CHAR_MODE,			// サバイバル対戦　新キャラ取得

		D_QUERY_NET_CONNECT_MODE,			// ネットワーク接続を許可しますか?
		D_ASK_NET_CONNECT_MODE,				// このアプリをご利用いただくには．．．
		D_NET_ERR_MODE,						// 通信プロトコルエラー
		D_CONN_ERR_MODE,					// 接続できませんでした
		D_TIMER_DISCONNECT_MODE,			// 無操作強制切断
		D_USER_DISCONNECT_MODE,				// ユーザ操作切断（ダイアログ表示）	// 2006/02/04 No.68
		D_ERR_MODE,							// エラー
		D_FATAL_ERR_MODE,					// 致命的なエラー
		D_MEMORY_ERR_MODE,					// メモリエラー
		D_EFS_ERR_MODE,
		D_BEFORE_ERR_MODE,					// BREW3.1対策。空回し。
		D_AFTER_ERR_MODE,					// BREW3.1対策。空回し。
		D_GO_SITE_MODE,						// サイトへ
		D_GAME_END_MODE,					// ゲームを終了するダイアログを表示
		D_SUBMIT_ERR_MODE,					// 認証エラー状態。
		D_MODE_MAX,
		D_GAME_RESULT_MODE,					// ゲームの清算画面(最終結果・馬・焼き鳥・ドボン・チップ)
	};

	/// <summary>
	///	// 鳴き無し
	/// </summary>
	public enum D_OPTION_NAKINASHI
	{
		OFF = 0,
		ON,
	};

	public enum D_SELECT_OPT
	{
		DAIUCHI = 0,
		SETSUDAN,
		NAKI_NASHI,
		MAX,
	};

	/// <summary>
	///	和了画面シーケンス番号
	/// </summary>
	public enum SEQ : byte{
		AG_DCLR,
		AG_DORA,
		AG_UCLR,
		AG_LIGHT,
		AG_YAKU,
		AG_YAKUMAN,
		AG_IPPATU,
		AG_URADORA,
		AG_TORIUCHI,
		AG_ALICE,
		AG_TIPTOTAL,		//10
		AG_NEXT_RESULT,
		AG_RYUKYOKU,
		AG_TENPAI,
		AG_SCORE0,
		AG_SCORE1,
		AG_SCORE2,
		AG_SCORE3,
		AG_END,
		AG_END2,
		AG_END3,			//20
		AG_END4,
		AG_END5,
		AG_END6,
		END_FADE,
		END_POINT1,
		END_POINT2,
		END_POINT3,
		END_POINT4,	// 2005/11/24 追加。
		END_UMA1,
		END_UMA2,			//30
		END_UMA3,
		END_UMA4,	// 2005/11/24 追加。
		END_DOBON1,
		END_DOBON2,
		END_DOBON3,
		END_DOBON4,	// 2005/11/24 追加。
		END_YAKITORI1,
		END_YAKITORI2,
		END_YAKITORI3,
		END_YAKITORI4,		//30	// 2005/11/24 追加。
		END_TIP1,
		END_TIP2,
		END_TIP3,
		END_TIP4,		// 2005/11/24 追加。
		AG_RANKING1,
		AG_RANKING2,
		END_MENU,

	};

	public enum GEV{
		NAKI_PON		 = 1,						// ポン
		NAKI_KAN		 = 2,						// カン
		NAKI_CHI		 = 3,						// チー
		NAKI_RICHI		 = 4,						// リーチ
		NAKI_RON		 = 5,						// ロン
		NAKI_TUMO		 = 6,						// ツモ
		SYS_RYUKYOKU	 = 109,						// 流局
		SYS_9SYU9HAI	 = 110,						// 九種九牌
		SYS_4FURENDA	 = 111,						// 四風連打
		SYS_TORIRON		 = 112,						// トリロン
		SYS_4CYA_REACH	 = 113,						// 四家立直
		SYS_4KAIKAN		 = 114,						// 四開槓
		// 2006/2/9 追記
		SYS_TENPAI		 = 115,						// テンパイ。		
	};

	/// <summary>
	///	家の番号
	/// </summary>
	public enum D_CYA
	{
		D_JICYA,
		D_SHIMOCYA,
		D_TOICYA,
		D_KAMICYA,
		D_MENTSU_COUNT,
	};

	/// <summary>
	///	キャラクターの顔アイコン・◇◇キャラクター選択◇◇
	/// </summary>
	public enum MJ_CHARCTER
	{
		CHARCTER_0 = 0,		// 0キャラクター
		CHARCTER_1,			// 1
		CHARCTER_2,			// 2
		CHARCTER_3,			// 3
		CHARCTER_4,			// 4
		CHARCTER_5,			// 5
		CHARCTER_6,			// 6
		CHARCTER_7,			// 7
		CHARCTER_8,			// 8
		CHARCTER_9,			// 9
		CHARCTER_10,			// 10
		CHARCTER_11,			// 11
		CHARCTER_12,			// 12
		CHARCTER_13,			// 13
		CHARCTER_14,			// 14
		CHARCTER_15,			// 15
		CHAR_NO_DATA,		// キャラクターの？ボックス。
		CHAR_SILHOUETTE,		// キャラクターシルエット。
		CHAR_SELECT_ICON,	// キャラクター選択アイコン。
		CHAR_SELECT_OK,		// キャラクター選択OK。
		CHAR_COM,			// COM表示
		CHARACTER_MAX_COUNT,
	};

#endregion	//-*MJDEF_H

#region MJ_GAME_H

//=============================================================================
//	オプション用定義
//=============================================================================
	/// <summary>
	/// opt_game[] に対するindex
	/// </summary>
	public enum GOPT {
		BGM = 		0,		/* ＢＧＭ				*/
		SE	 = 		1,		/* ＳＥ					*/
		TLK = 		2,		/* 対戦中台詞			*/
		DNG = 		3,		/* 危険牌チェック		*/
		AGR = 		4,		/* アガリ牌チェック		*/
		AUT = 		5,		/* 聴牌オート			*/
		TIM = 		6,		/* 制限時間				*/
	};

	/// <summary>
	///	opt_game[] に入っている値
	/// </summary>
	public enum GOPT_BGM {

		ON = 0,		/* ＯＮ	*/
		OFF = 1,		/* ＯＦＦ	*/
	};
	/// <summary>
	///	opt_game[] に入っている値
	/// </summary>
	public enum GOPT_SE {
		ON	= 0,		/* ＯＮ	*/
		OFF	= 1,		/* ＯＦＦ	*/
	};
	/// <summary>
	///	opt_game[] に入っている値
	/// </summary>
	public enum GOPT_TLK {
		ON = 1,		/* ＯＮ	*/
		OFF = 0,		/* ＯＦＦ	*/
	};
	/// <summary>
	///	opt_game[] に入っている値
	/// </summary>
	public enum GOPT_AUT {
		ON = 0,		/* ＯＮ	*/
		OFF = 1,		/* ＯＦＦ	*/
	};

	/// <summary>
	///	opt_game[] に入っている値
	/// </summary>
	public enum GOPT_TIM {
		TIM_OFF = 0,		/* 無し	*/
		TIM_5 = 1,		/* ５秒	*/
		TIM_10 = 2,		/* １０秒	*/
		TIM_15 = 3,		/* １５秒	*/
	};
#endregion //-*MJ_GAME_H


#region MJCallDraw

	/// <summary>
	/// MJCallDrawの画像と連動してるので注意
	/// </summary>
	public enum CALLDRAW{
		RICHI,						// リーチ
		PON,						// ポン
		CHI,						// チー
		KAN,						// カン
		TUMO,						// ツモ
		RON,						// ロン

//-***todo:順番注意
		TENPAI,		//-*テンパイ
		NOTEN,		//-*ノーテン
		NAGASHI,		//-*流し満貫
		RYUKYOKU,	//-*流局
//-***todo:順番注意
		MAX,
	};
#endregion //-*MJCallDraw

	/// <summary>
	///	麻雀制限ルール ヘッダー
	///	0・ノーマル
	///	1・鳴き禁止
	///	2・点数ハンデ戦（自分15000点、相手35000万点）
	///	3・二翻縛り
	///	4・ロン・リーチ禁止
	///	5・ロン禁止
	///	6・リーチ禁止
	/// </summary>
	public enum MAH{
		LIM00, //  0:ノーマル戦
		LIM01, //  1:鳴き禁止
		LIM02, //  2:点数ハンデ戦[-10000点/15000vs35000]
		LIM03, //  3:二翻縛り
		LIM04, //  4:ロン・リーチ禁止
		LIM05, //  5:ロン禁止
		LIM06, //  6:リーチ禁止
		LIM07, //  7:点数ハンデ戦[-5000点/20000vs30000]
		//********ウキウキ:ルール追加********
		LIM08, //* 8:点数ハンデ戦[-15000点/10000vs40000]
		//***********************************
		LIM_MAX,
	};

	public enum MAHJONGMODE {
		mMODE_READY,			//準備中
		mMODE_INIT,			//
		mMODE_GAME,			//
		mMODE_EXIT,			//終了
		mMODE_MAX,
	};

	/// <summary>
	/// 牌状態
	/// </summary>
	public enum TILE_STATE{
		HAND,		//-*手牌(自分)
		MY_DISCARDED,	//-*捨て牌(自分)
		RIICHI,			//-*リーチ
		YOUR_DISCARDED,	//-*捨て牌(相手)
		NO_USE,		//-*山
	};

	/// <summary>
	/// 牌種類
	/// </summary>
	public enum TILE_TYPE{
		CHARACTERS = 0,	//-*萬子
		CIRCLES,	//-*筒子
		BAMBOOS,	//-*索子
		HONOURS,	//-*字牌
		TYPE_MAX,	//-*総数
	};

	/// <summary>
	/// 牌リスト(画像番号と連動中)
	/// todo：処理方法によっては番号ズレるかも
	/// </summary>
	public enum TILE_LIST{
	//-*萬子
		CHARACTERS_1 = 0,
		CHARACTERS_2,
		CHARACTERS_3,
		CHARACTERS_4,
		CHARACTERS_5,
		CHARACTERS_6,
		CHARACTERS_7,
		CHARACTERS_8,
		CHARACTERS_9 = 8,

		//-*索子
		BAMBOOS_1 = 9,	//-*todo:10にするかも
		BAMBOOS_2,
		BAMBOOS_3,
		BAMBOOS_4,
		BAMBOOS_5,
		BAMBOOS_6,
		BAMBOOS_7,
		BAMBOOS_8,
		BAMBOOS_9 = 17,	//-*todo:18にするかも

		//-*筒子
		CIRCLES_1 = 18,	//-*todo:20にするかも
		CIRCLES_2,
		CIRCLES_3,
		CIRCLES_4,
		CIRCLES_5,
		CIRCLES_6,
		CIRCLES_7,
		CIRCLES_8,
		CIRCLES_9 = 26,	//-*todo:28にするかも

		//-*字牌
		HONOURS_EAST = 27,	//-*東	//-*todo:30にするかも
		HONOURS_SOUTH,		//-*南
		HONOURS_WEST,		//-*西
		HONOURS_NORTH,		//-*北
		HONOURS_WHITE,		//-*白
		HONOURS_GREEN,		//-*発
		HONOURS_RED = 33,	//-*中
		BACK = 34,			//-*裏	//-*todo:38にするかも
		TILES_MAX = BACK,	//-*総数
	};


	/// <summary>
	/// 個人手牌ワークデファイン(byte)
	/// 接頭語：WP_
	/// </summary>
	public  class PARA {
		public int	chParaRichi;				//*立直
		public int	chParaUra;					//*裏ドラ
		public int	chParaKish;					//*イーシャンテン
		public int	chParaIshnum;				//*イーシャンテンの待ち数
		public int	chParaYaku;					//*役
		public int	chParaOri;					//*降り
		public int	chParaFurotnp;				//*？
		public int	chParaFuroish;				//*鳴いてイーシャンテン
		public int	chParaFuroval;				//*鳴いてイーシャンテンでない
		public int	chParaFanpaih;				//*役牌
		public int	chParaFanpai1;
		public int	chParaFanpai2;
		public int	chParaHonisop;				//*ホンイツ
		public int	chParaHonisoh;
		public int	chParaToitoip;				//*トイトイ
		public int	chParaToitoih;
		public int	chParaTanyop;				//*タンヤオ
		public int	chParaTanyoh;
		public int	chParaKan;					//*カン(好き 0,5)
		public int	chParaAlone;				//*カンチャン、ペンチャン嫌い
		public int	chParaTako;					//*タコ
		public int	chParaSpecial;				//*未使用
		public int	chParaJanto;				//*雀頭
		public int	chParaKeiten;				//*形式テンパイ
		public int	chParaFuriten;				//*フリテン	//xxxx	未使用
		public int	chParaStrat;				//*？(0,1)
		public int	chParaRon;					//*ロン（上がりやすさ）
		public int	chParaAnpai;				//*アンパイ
		public int	chParaRsv1;					//*未使用
		public int	chParaRsv2;					//*未使用

		public PARA () {}
		public PARA (	int chParaRichi,	int chParaUra,		int chParaKish,		int chParaIshnum,	int chParaYaku,
				int chParaOri,		int chParaFurotnp,	int chParaFuroish,	int chParaFuroval,	int chParaFanpaih,
				int chParaFanpai1,	int chParaFanpai2,	int chParaHonisop,	int chParaHonisoh,	int chParaToitoip,
				int chParaToitoih,	int chParaTanyop,	int chParaTanyoh,	int chParaKan,		int chParaAlone,
				int chParaTako,		int chParaSpecial,	int chParaJanto,	int chParaKeiten,	int chParaFuriten,
				int chParaStrat,	int chParaRon,		int chParaAnpai,	int chParaRsv1,		int chParaRsv2 ) {
			this.chParaRichi= chParaRichi;
			this.chParaUra= chParaUra;
			this.chParaKish= chParaKish;
			this.chParaIshnum= chParaIshnum;
			this.chParaYaku= chParaYaku;
			this.chParaOri= chParaOri;
			this.chParaFurotnp= chParaFurotnp;
			this.chParaFuroish= chParaFuroish;
			this.chParaFuroval= chParaFuroval;
			this.chParaFanpaih= chParaFanpaih;
			this.chParaFanpai1= chParaFanpai1;
			this.chParaFanpai2= chParaFanpai2;
			this.chParaHonisop= chParaHonisop;
			this.chParaHonisoh= chParaHonisoh;
			this.chParaToitoip= chParaToitoip;
			this.chParaToitoih= chParaToitoih;
			this.chParaTanyop= chParaTanyop;
			this.chParaTanyoh= chParaTanyoh;
			this.chParaKan= chParaKan;
			this.chParaAlone= chParaAlone;
			this.chParaTako= chParaTako;
			this.chParaSpecial= chParaSpecial;
			this.chParaJanto= chParaJanto;
			this.chParaKeiten= chParaKeiten;
			this.chParaFuriten= chParaFuriten;
			this.chParaStrat= chParaStrat;
			this.chParaRon= chParaRon;
			this.chParaAnpai= chParaAnpai;
			this.chParaRsv1= chParaRsv1;
			this.chParaRsv2= chParaRsv2;
		}

		public void copy(PARA ob) {
			chParaRichi= ob.chParaRichi;
			chParaUra= ob.chParaUra;
			chParaKish= ob.chParaKish;
			chParaIshnum= ob.chParaIshnum;
			chParaYaku= ob.chParaYaku;
			chParaOri= ob.chParaOri;
			chParaFurotnp= ob.chParaFurotnp;
			chParaFuroish= ob.chParaFuroish;
			chParaFuroval= ob.chParaFuroval;
			chParaFanpaih= ob.chParaFanpaih;
			chParaFanpai1= ob.chParaFanpai1;
			chParaFanpai2= ob.chParaFanpai2;
			chParaHonisop= ob.chParaHonisop;
			chParaHonisoh= ob.chParaHonisoh;
			chParaToitoip= ob.chParaToitoip;
			chParaToitoih= ob.chParaToitoih;
			chParaTanyop= ob.chParaTanyop;
			chParaTanyoh= ob.chParaTanyoh;
			chParaKan= ob.chParaKan;
			chParaAlone= ob.chParaAlone;
			chParaTako= ob.chParaTako;
			chParaSpecial= ob.chParaSpecial;
			chParaJanto= ob.chParaJanto;
			chParaKeiten= ob.chParaKeiten;
			chParaFuriten= ob.chParaFuriten;
			chParaStrat= ob.chParaStrat;
			chParaRon= ob.chParaRon;
			chParaAnpai= ob.chParaAnpai;
			chParaRsv1= ob.chParaRsv1;
			chParaRsv2= ob.chParaRsv2;
		}

		public PARA clone () {
			PARA ret = new PARA();

			ret.chParaRichi= this.chParaRichi;
			ret.chParaUra= this.chParaUra;
			ret.chParaKish= this.chParaKish;
			ret.chParaIshnum= this.chParaIshnum;
			ret.chParaYaku= this.chParaYaku;
			ret.chParaOri= this.chParaOri;
			ret.chParaFurotnp= this.chParaFurotnp;
			ret.chParaFuroish= this.chParaFuroish;
			ret.chParaFuroval= this.chParaFuroval;
			ret.chParaFanpaih= this.chParaFanpaih;
			ret.chParaFanpai1= this.chParaFanpai1;
			ret.chParaFanpai2= this.chParaFanpai2;
			ret.chParaHonisop= this.chParaHonisop;
			ret.chParaHonisoh= this.chParaHonisoh;
			ret.chParaToitoip= this.chParaToitoip;
			ret.chParaToitoih= this.chParaToitoih;
			ret.chParaTanyop= this.chParaTanyop;
			ret.chParaTanyoh= this.chParaTanyoh;
			ret.chParaKan= this.chParaKan;
			ret.chParaAlone= this.chParaAlone;
			ret.chParaTako= this.chParaTako;
			ret.chParaSpecial= this.chParaSpecial;
			ret.chParaJanto= this.chParaJanto;
			ret.chParaKeiten= this.chParaKeiten;
			ret.chParaFuriten= this.chParaFuriten;
			ret.chParaStrat= this.chParaStrat;
			ret.chParaRon= this.chParaRon;
			ret.chParaAnpai= this.chParaAnpai;
			ret.chParaRsv1= this.chParaRsv1;
			ret.chParaRsv2= this.chParaRsv2;

			return (ret);
		}

		public bool check()
		{
			return true;
		}

		public void clear()
		{
			chParaRichi= 0;
			chParaUra= 0;
			chParaKish= 0;
			chParaIshnum= 0;
			chParaYaku= 0;
			chParaOri= 0;
			chParaFurotnp= 0;
			chParaFuroish= 0;
			chParaFuroval= 0;
			chParaFanpaih= 0;
			chParaFanpai1= 0;
			chParaFanpai2= 0;
			chParaHonisop= 0;
			chParaHonisoh= 0;
			chParaToitoip= 0;
			chParaToitoih= 0;
			chParaTanyop= 0;
			chParaTanyoh= 0;
			chParaKan= 0;
			chParaAlone= 0;
			chParaTako= 0;
			chParaSpecial= 0;
			chParaJanto= 0;
			chParaKeiten= 0;
			chParaFuriten= 0;
			chParaStrat= 0;
			chParaRon= 0;
			chParaAnpai= 0;
			chParaRsv1= 0;
			chParaRsv2= 0;
		}
	};

	/// <summary>
	/// 対局データ
	/// </summary>
	public class TABLEMEM {
		public byte	byRank;										/*	順位	*/
		public byte	byMember;									/*	メンバー	*/
		public string	NickName;	//char	NickName[D_NICK_NAME_MAX + 1];	/*  名前		*/	//tamaki 20051118 add
		public short	nPoint;										/*	点数	*/
		public short	nMovePoint;									/*	移動点数	*/
		public short	nOldPoint;									/*	直前の点数	(代入してるだけ→表示用に変更)	*/
		public short	nChip;										/*	チップ	*/
		public short	nMoveChip;									/*	移動チップ	*/
		public short	nOldChip;									/*	直前のチップ	*/
		public byte	byMemFlags;									/*	フラグ	*/
		public byte	byRibo_stack;								/*	リー棒戻(無･有)→流れたリー棒の各家の数 */
		public short	nPnt;										/*	プラス、マイナス情報	*/
		public short	nMovePnt;									/*	移動ポイント			*/
		public byte	byTotalRank;								/*	順位（チップ計算後）	*/

		public TABLEMEM clone() {
			TABLEMEM ret = new TABLEMEM();

			ret.byRank= this.byRank;
			ret.byMember= this.byMember;
			ret.NickName= this.NickName;
			ret.nPoint= this.nPoint;
			ret.nMovePoint= this.nMovePoint;
			ret.nOldPoint= this.nOldPoint;
			ret.nChip= this.nChip;
			ret.nMoveChip= this.nMoveChip;
			ret.nOldChip= this.nOldChip;
			ret.byMemFlags= this.byMemFlags;
			ret.byRibo_stack= this.byRibo_stack;
			ret.nPnt= this.nPnt;
			ret.nMovePnt= this.nMovePnt;
			ret.byTotalRank= this.byTotalRank;

			return (ret);
		}
		public void clear()
		{
			byRank= 0;
			byMember= 0;
			NickName= "";
			nPoint= 0;
			nMovePoint= 0;
			nOldPoint= 0;
			nChip= 0;
			nMoveChip= 0;
			nOldChip= 0;
			byMemFlags= 0;
			byRibo_stack= 0;
			nPnt= 0;
			nMovePnt= 0;
			byTotalRank= 0;
		}
		public bool check()
		{
			if( NickName == null )     { return false; }
			if( byRank < 0 || byRank > 3 )     { return false; }
			if( byMember < 0 || byMember > 17 ){ return false; }
			if( byTotalRank < 0 || byTotalRank > 3 ){ return false; }
			return true;
		}
	};

	public class TABLEDATA {
		public byte byTableNo;									/*	卓番号					*/
		public byte byFlags;									/*	フラグ					*/
		public byte byChicha;									/*	起家					*/
		public byte byKyoku;									/*	局数					*/
		public byte byRibo;										/*	場にあるリー棒			*/
		public byte byRenchan;									/*	連荘数					*/
		public byte byDrawRenchan;								/*	連荘数(表示用)			*/
		public byte byKamicha_dori; 							/*	上家取					*/
		public byte byOya;										/*	現在の親				*/
		public byte byOLDTop;									/*	前局までのトップ		*/
		public byte byParen;									/*	八連荘					*/
		public PARA	psParam;
		public TABLEMEM[]	sMemData = new TABLEMEM [4];
		public TABLEDATA clone() {
			int	i;
			TABLEDATA ret = new TABLEDATA();
			ret.byTableNo= this.byTableNo;
			ret.byFlags= this.byFlags;
			ret.byChicha= this.byChicha;
			ret.byKyoku= this.byKyoku;
			ret.byRibo= this.byRibo;
			ret.byRenchan= this.byRenchan;
			ret.byDrawRenchan= this.byDrawRenchan;
			ret.byKamicha_dori= this.byKamicha_dori;
			ret.byOya= this.byOya;
			ret.byOLDTop= this.byOLDTop;
			ret.byParen= this.byParen;
			ret.psParam= (PARA)this.psParam.clone();
			for(i = 0; i < 4; i++)	ret.sMemData[i]= (TABLEMEM)this.sMemData[i].clone();
			return (ret);
		}
		public void clear()
		{
			byTableNo= 0;
			byFlags= 0;
			byChicha= 0;
			byKyoku= 0;
			byRibo= 0;
			byRenchan= 0;
			byDrawRenchan= 0;
			byKamicha_dori= 0;
			byOya= 0;
			byOLDTop= 0;
			byParen= 0;
			psParam.clear();
			sMemData[0].clear();
			sMemData[1].clear();
			sMemData[2].clear();
			sMemData[3].clear();
		}
		public bool check()
		{
			// if( psParam.check() == false     ) return false;
			// if( sMemData[0].byMember !=	17 ) return false;
			// if( sMemData[0].NickName.compareTo( "プレイヤー" ) != 0 ) return false;	
			// if( sMemData[1].check() == false ) return false;
			// if( sMemData[2].check() == false ) return false;
			// if( sMemData[3].check() == false ) return false;
			return true;
		}
	};

/*** 牌の扱い ***/
/* 萬子：0x01～0x09                                 */
/* 索子：0x10～0x19                                 */
/* 筒子：0x21～0x29                                 */
/* 字牌：0x31～0x37：（東・南・西・北・白・発・中） */
	/* フーロ時(それぞれ萬子・索子・筒子・字牌の順で) */
		/* チー　：先頭の牌コード      */
		/* ポン　：0x4? 0x5? 0x6? 0x7? */
		/* 暗カン：0x8? 0x9? 0xA? 0xB? */
		/* 明カン：0xC? 0xD? 0xE? 0xF? */


	public class DANGERDAT {
		public byte	byTehai;
		public byte	byDanger;
	};



	public class YAKUMEM {
		public int	p;
		public int	h;
		public void clear()
		{
			p = 0;
			h = 0;
		}
	};

	public class YAKU {
		public YAKUMEM	honiso;
		public YAKUMEM	toitoi;
		public YAKUMEM	tanyo;
		public YAKUMEM	fanpai;
		public int	shosan;
		public int	chitoi;
		public int	kokushi;
		public int	honsort;

		public YAKU () {
			honiso= new YAKUMEM();
			toitoi= new YAKUMEM();
			tanyo= new YAKUMEM();
			fanpai= new YAKUMEM();
		}
		//0511mt
		public void clear()
		{
			honiso.clear();
			toitoi.clear();
			toitoi.clear();
			tanyo.clear();
			fanpai.clear();
			shosan = 0;
			chitoi = 0;
			kokushi = 0;
			honsort = 0;
		}
	};



	public class RULESUBDATA {
		public byte	byRuleNo;									/*	ルール番号			*/
		public byte	byFlag;										/*	フラグ				*/
		public byte	byRenchanRate;								/*	連荘レート			*/
		public byte	byChipRate;									/*	チップのレ－ト		*/
		public short nToplin;									/*	西入り点数			*/
	};


	/// <summary>
	/// キャラクターパラメーターデファイン 接頭語：CP_
	/// </summary>
	public class PAIVAL {
		public int tnpval;
		public int ricval;
		public int tnpnum;
		public int ricnum;
		public int ishval;
		public int ishnum;
		public int val;
		public int paicode;
		public void clear(){
			tnpval = 0;
			ricval = 0;
			tnpnum = 0;
			ricnum = 0;
			ishval = 0;
			ishnum = 0;
			val = 0;
			paicode = 0;
		}
	};

	public class PLST {
		public int min;
		public int max;
		public int num;
		public void clear(){ min = max = num = 0; }
	};

	/// <summary>
	/// 上がり点数
	/// </summary>
	public class MEM_RESULT {
		public short	wFlag;
		public short	iPoint;

		public MEM_RESULT clone() {
			MEM_RESULT ret = new MEM_RESULT();

			ret.wFlag= this.wFlag;
			ret.iPoint= this.iPoint;

			return (ret);
		}
	};

	public class YAKU_T {
		public byte	name;
		public byte	factor;

		public YAKU_T clone() {
			YAKU_T ret = new YAKU_T();
			ret.name= this.name;
			ret.factor= this.factor;
			return (ret);
		}
	};

	public class MJK_RESULT {
		public MEM_RESULT[] sMemResult = new MEM_RESULT [MJDefine.MAX_TABLE_MEMBER];
		public byte		byYakuman;
		public byte		byHan;
		public byte		byFu;
		public byte		byYakuCnt;
		public YAKU_T[]	sYaku = new YAKU_T [MJDefine.MAX_YAKU];

		public byte		byRenchan;
		public int			nOyaPoint;
		public int			nKoPoint;
		public int			nTotalPoint;

		public MJK_RESULT clone() {
			int		i;
			MJK_RESULT ret = new MJK_RESULT();

			for(i = 0; i < MJDefine.MAX_TABLE_MEMBER; i++)	ret.sMemResult[i]= (MEM_RESULT)this.sMemResult[i].clone();
			ret.byYakuman= this.byYakuman;
			ret.byHan= this.byHan;
			ret.byFu= this.byFu;
			ret.byYakuCnt= this.byYakuCnt;
			for(i = 0; i <  MJDefine.MAX_YAKU; i++)	ret.sYaku[i]= (YAKU_T)this.sYaku[i].clone();

			ret.byRenchan= this.byRenchan;
			ret.nOyaPoint= this.nOyaPoint;
			ret.nKoPoint= this.nKoPoint;
			ret.nTotalPoint= this.nTotalPoint;

			return (ret);
		}
	};

	/// <summary>
	/// 面子データ
	/// </summary>
	public class MNTDATA {
		public byte	byJanto;					/*	雀頭		*/
		public byte[]	byMnt = new byte [4];		/*	面子データ	*/
		/*	符表示用	*/
		public byte[]	byType = new byte [4];		/*	0=暗刻,1=明刻,2=暗カン,3=明カン,0xFF	*/
		public byte[]	byFuData = new byte [5];	/*	符データ	*/
		public byte	byFuFlag;
		public byte	byMachi;
		public byte	byMachiMnt;

		public MNTDATA clone() {
			int		i;
			MNTDATA ret = new MNTDATA();

			ret.byJanto= this.byJanto;
			for(i = 0; i < 4; i++)	ret.byMnt[i]= this.byMnt[i];
			for(i = 0; i < 4; i++)	ret.byType[i]= this.byType[i];
			for(i = 0; i < 5; i++)	ret.byFuData[i]= this.byFuData[i];
			ret.byFuFlag= this.byFuFlag;
			ret.byMachi= this.byMachi;
			ret.byMachiMnt= this.byMachiMnt;

			return (ret);
		}
	};

	public class CmdHistory_		//struct _CmdHistory_
	{
		public char SuteHouseID; // 捨家ID
		public char ActHouseID; // 対応家ID
		public char HaiState; // 牌状態
		public char ChiState; // チー状態
		public char SuteHai; // 捨牌
	};

//-*************ここまで

	/// <summary>
	/// Structure
	/// typedef struct _SubMj_Tbl
	/// </summary>
	public class SubMj_Tbl 		//typedef struct _SubMj_Tbl
	{
		/****************************************/
		/*		EVENT.c用　ワークＲＡＭ			*/
		/****************************************/
		public int			event_sts;
		public int			event_wait;
		public char			event_alpha;
		public int			event_txt_no;

		public int			event_u_sts;
		public int			event_u_wait;
		public char			event_u_alpha_bg1;
		public char			event_u_alpha_bg2;
		public short			event_u_spx1;
		public short			event_u_spy1;
		public short			event_u_spx2;
		public short			event_u_spy2;
		public char			event_mbri;

		public int			stage;
		public short			sv_get_hai;
		public int			item_get_flg;

		/****************************************/
		/*		MJ以下用　ワークＲＡＭ			*/
		/****************************************/

	//	BYTE			_byEndingMode;
		public byte[]			SurvivalMentsuFlg = new byte [MJDefine.MAX_COMP_CHARACTER];
		public byte			survival_stage_no;
		public byte			survival_win_count;
		public byte			survival_first_clear;

		public byte			Pao3;
		public byte			Pao4;
		public bool			sayTOpao3;							/* DIALOG パオの相手に喋ったかﾌﾗｸﾞ */
		public bool			sayTOpao4;							/* DIALOG パオの相手に喋ったかﾌﾗｸﾞ */
		public byte			Paoflg;
		public byte			Wareme;								/* ワレ目フラグ */
		public byte			Opnflg;								/* オープンモードフラグ */
		public byte			Lastric;
		public byte			Mangan;

		public byte[]			RiboDispFlg = new byte [MJDefine.MAX_TABLE_MEMBER];		/* リーチ棒表示フラグ */

		public int				Tnpval;								/*00A7*/	/* mjetnp2,mjthink */
		public int				Ricval;								/*00AA*/	/* mjetnp2,mjthink */
		public int				Tnpmax;								/*00AD*/	/* mjthink */
		public byte			Ishnum;								/*00B0*/	/* mjthink */
		public byte			Ishval;								/*00B2*/	/* mjthink */
		public byte			Hannec;								/*00BA*/	/* mjthink */
		public int				jantoflag;
		public YAKU			gYaku;
		public PAIVAL[]		gPaival = new PAIVAL [15];
		public byte[]			chitoibuf = new byte [7];

		public byte			Hant;								/*008E*/	/* mjetnp */
		public byte			Hanr;								/*008F*/	/* mjetnp */
		public byte			Rict;								/*0090*/	/* mjetnp */
		public byte			Ricr;								/*0091*/	/* mjetnp */
		public byte			Ricflg;								/*0092*/	/* mjthink */
		public byte			Dorasum;							/*0093*/	/* mjetnp,mjey,mjthink */
		public byte			Kannum;

	//	WORD			link_max;

		/* 通信用 */
		/* 親 */
		//WL_RESP			wlResp[3];					/* 子から親へのレスポンスデータ */
		//WL_RESP			wlChild;					/* 子から親へのレスポンスデータ */
		//WL_RESULT		wlResult;					/* 親から子への局結果データ */
		//BYTE			ChildRicbuf[3][14];

		public bool			CompVSCompMode;					/*	コンピュータ同士の対局	*/

		/*		牌のカウント	*/
		public byte[,]			apftbl = new byte[4,MJDefine.APFTBL_COUNT];
		public byte[]			risk = new byte [4];
		public byte[]			tenpai = new byte [4];
		public int				rkr;
		public int[]			rk = new int [4];
		public int[]			rksum = new int [4];
		public int[]			rkrsum = new int [4];
		public int[]			rabove = new int [40];
		public int[]			rbelow= new int [42];
		public int[]			rcenter= new int [56];
		public int				daisangen;
		public int				daisanpai;

		public byte[]			Uradora_rec = new byte [5]; 		/* 各裏ドラののってる数 */
		public byte			Ppr;					/*00C0*/	/* mjetnp */
		public byte			Pprr;					/*00C1*/	/* mjetnp */
		public byte[]			Paicnt = new byte [0x38];			/* 0841 */			/* mjey,mjthink */
		public byte[]			ppx = new byte [0x38];				/*:09C9-0A00*/		/* mjetnp,mjeval */
		public byte[]			spc = new byte [0x38];				/*:0A01-0A38*/		/* mjetnp,mjetnp2,mjeval */
		public byte[]			kdv = new byte [0x38];				/*:0A39-0A70*/		/* mjey */
		public byte[]			jdv = new byte [0x38];				/*:0A71-0AA6*/		/* mjey */
		public byte[]			sdv = new byte [0x38];				/*0AA9-0AE0*/		/* mjey */
		public byte[]			pntnec = new byte [4];				/*0AE5-0AE8*/	/* mjetnp */
		public byte[]			pntsuf = new byte [4];				/*0AE9-0AEC*/	/* mjetnp */
		public byte[]			notflg = new byte [4];				/*0AED-0AF0*/	/* mjthink */
		public short			keiten;					/*0AF1*/			/* mjetnp2 */
		public short			ricrisk;				/*0AF3-0AF4*/		/* mjetnp2 */
		public byte			kish;					/*0AF5*/			/* mjthink */
		public byte			ishnec;					/*0AF6*/			/* mjthink */
		public short[]			riskval = new short [0x38];			/*0AF7-0B2E*/		/* mjthink */
		public byte[]			safeval = new byte [0x38];

		public byte[]			Doracnt = new byte [0x38];			/*各牌が何回ドラにカウントされているか:0879-08B0	34種類の牌が何回ドラに数えられるか*/
		public byte[]			pc = new byte [0x38];				/*0991*/	/* ちなみに pc[] の使用されてるとこは MJETNP,MJETNP2,MJEVAL,MJEY,MJPCNT,MJTHINK */

		// Main.cで使用
	//	UINT4			random;
	//	UINT4			sys_cnt;				// 0初期化必要。

		// Graph.cで使用
	//	short			scr_pos[]= new short [16];
	//	short			fade_lv_m;
	//	short			fade_lv_s;

	//	UINT4			g_buf[]= new UINT4 [16];
	//	short			fade_lv_ma;
	//	short			fade_lv_sa;
	//	short			scr_posa[]= new short [16];

		//tamaki char			obj_pri[2][PRI_MAX];

		//mjthink.c
		public PAIVAL			sp;			//*sp;
		public int				janto;
		public int				furono;
		public int				result;
		public int				tmax;
		public int				imax;
		public int				vmax;
		public int				tmax1;
		public int				imax1;
		public int				vmax1;
		public int				tnpmaxval1;
		public int				furohan;

		public byte			Tleft;				/*00C2*/

		public byte[]			mp;			//*mp;
		public int				mp_p;			//mp のポインタのかわり
		public byte[]			mjmnt_min= new byte [15];
		public byte[]			mjmnt_num= new byte [15];

		public byte			_byRobiCount;
	//m	BYTE			Order_now;									/* 今誰の手順か（０～３） */
		public byte			kyoku_end_mode;								/* 0:流局 1:和了 */

		public byte[]			tip_buf= new byte[4];
		public byte			tip_ura_cnt;
		public byte[]			tip_tori_buf = new byte [3];
		public byte[]			tip_alice_hai = new byte [128];
		public byte[]			tip_alice_cnt= new byte [128];
		public byte			tip_alice_disp;
		public byte			tip_alice_total;
		public byte			tip_total;
		public byte			agari_score_yofs;
		public byte			agari_next_btn;								/* 0:通常 1:半透明 */
		public byte			agari_end_chk;								/* あがりやめチェック 0:しない 1:する */

		public bool			set_for_nextplay_fRenchan;


		public DANGERDAT[]		sDangerDat = new DANGERDAT [14];			/* 危険牌 */

		public byte			_byClearIppatu;

		public byte[]			App = new byte[56];	//*App;	/*00BD*/	// = apftbl[][]= new BYTE [4][56];

		public long			mj_wait;

		public byte			hikihai;
		public int				yhan;
		public int				pinho;
		public int				suanflg;
		public int				sananflg;

		//mjemnt.c
		public int				g_tnpnum;
		public int				g_ricnum;
		public int				g_maxnum;
		public int[]			g_pnttbl = new int [20];
		public int[]			g_rictbl = new int [20];
		public byte[]			g_paibuf = new byte [35];
		public int				g_msum;
		public int				g_tsum;
		public char[]			g_mctbl = new char [15];
		public char[]			g_wctbl = new char [15];
		public byte[]			g_apfsav = new byte [6];
		public int				p_apfsav;		// = *g_apfsav	//*p_apfsav;


		public byte[]			Kv = new byte [0x38];	/*05FC-0633*/
		public byte[]			Jv = new byte [0x38];	/*0634-066B*/
		public byte[]			Sv = new byte [0x38];	/*066C-06A3*/

		//mjeval.c
		//int				janto;
		public int				alone;
		public int				pmin1;
		public int				pmin2;
		public int				pmax1;
		public int				pmax2;
		public int				mval;
		public int				jval;
		public int				g_mc;
		public int				level;
		public int				depth;
		public int				waste;
		public byte[]			tatflg = new byte [56];		//unsigned char [56];
		public int[]			blockval = new int [15];
		public int				maxblock;		//*maxblock;
		public int				maxjval;
		public int				nextjval;

		//mjey.c
		public int				tanyoval;
		public int				honisoval;
		public int				toitoival;
		public int				normalval;
		public int[]			sortnum = new int [4];

		//mjjtnp.c
		public byte[]			g_mpp;		//*g_mpp;
		public int				g_mpp_p;		//g_mpp のポインタのかわり

		public byte			dobon_buf;
		public short			hanchan_result_top;

		//mj.c
		public bool			lgame_init_flg;
		public byte			agari_sts_p;
		public byte[]			agari_sts_c = new byte [3];
		public byte			agari_sts_tmp;
		public byte			mj_sts_tmp;
		//> 2006/03/12 差し馬削除
		//BYTE			sashiuma_event_flg;
		//BYTE			sashiuma_event_chara;
		//> 2006/03/12 差し馬削除
		public byte			result_disp_no;
		public byte			wl_start;
		public byte			sho_sts;
		public bool			fDrawPai;
		public bool			fDrawFu;
		public bool			fChitoi;

		/*********************************************************************
			局終了処理
		*********************************************************************/
		public long			Wareme_point;
		public long			Tsumopoint_from_oya;
		public long			Tsumopoint_from_ko;

#if false	//-*todo:描画関連は保留
	// 	/*********************************************************************
	// 		描画用処理
	// 	*********************************************************************/
		byte			wanpai_xpos;
		byte			wanpai_ypos;

		// 自家
		_IImage			m_pIPaiJicya;		//*m_pIPaiJicya;		// IImage インターフェースポインタテーブル
	#if	Rule_2P
		SpriteInfo		m_spPaiJicya;
	#else
		SpriteInfo		m_spPaiJicya[]= new SpriteInfo [D_SPRITE_PAI_COUNT];		//

		// 自家SMALL
		_IImage			m_pIPaiJicyaSmall;	//*m_pIPaiJicyaSmall;	// IImage インターフェースポインタテーブル
		SpriteInfo		m_spPaiJicyaSmall[]= new SpriteInfo [D_SPRITE_PAI_COUNT];	// 小さめ用

		// 上家
		_IImage			m_pIPaiKamicya;		//*m_pIPaiKamicya;		// IImage インターフェースポインタテーブル
		SpriteInfo		m_spPaiKamicya[]= new SpriteInfo [D_SPRITE_PAI_COUNT];		// 1

		// 対家
		_IImage			m_pIPaiToicya;		//*m_pIPaiToicya;		// IImage インターフェースポインタテーブル
		SpriteInfo		m_spPaiToicya[]= new SpriteInfo [D_SPRITE_PAI_COUNT];		// 2

		// 下家
		_IImage			m_pIPaiShimocya;	//*m_pIPaiShimocya;		// IImage インターフェースポインタテーブル
		SpriteInfo		m_spPaiShimocya[]= new SpriteInfo [D_SPRITE_PAI_COUNT];	// 3
	#endif
		// --- 捨て牌用 ---。
		// -- 自家。
		_IImage			m_pISutePaiJicya;	//*m_pISutePaiJicya;
	#if	Rule_2P
		SpriteInfo		m_spSutePaiJicya;
	#else
		SpriteInfo		m_spSutePaiJicya[]= new SpriteInfo [D_SPRITE_PAI_COUNT];
	#endif

		// -- 下家。
		_IImage			m_pISutePaiShimocya;	//*m_pISutePaiShimocya;
	#if	Rule_2P
		SpriteInfo		m_spSutePaiShimocya;
	#else
		SpriteInfo		m_spSutePaiShimocya[]= new SpriteInfo [D_SPRITE_PAI_COUNT];
	#endif

		// -- 対家。
		_IImage			m_pISutePaiToicya;		//*m_pISutePaiToicya;
	#if	Rule_2P
		SpriteInfo		m_spSutePaiToicya;
	#else
		SpriteInfo		m_spSutePaiToicya[]= new SpriteInfo [D_SPRITE_PAI_COUNT];
	#endif

	#if	Rule_2P
	#else
		// -- 上家。
		_IImage			m_pISutePaiKamicya;		//*m_pISutePaiKamicya;
		SpriteInfo		m_spSutePaiKamicya[]= new SpriteInfo [D_SPRITE_PAI_COUNT];
	#endif
#endif //-*todo:描画関連は保留
		public SubMj_Tbl () {}
	};

}
//-*******todo:以下は不要確定次第消す	


// public enum HONOURS{
// 	EAST = 0,	//-*東
// 	SOUTH,		//-*南
// 	WEST,		//-*西
// 	NORTH,		//-*北
// 	WHITE,		//-*白
// 	GREEN,		//-*発
// 	RED,		//-*中
// };
