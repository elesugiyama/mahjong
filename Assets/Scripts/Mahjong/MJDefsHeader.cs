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

/// <summary>
/// 麻雀大会３
/// 麻雀ゲームデファイン
/// </summary>
namespace MJDefsHeader {

	/// <summary>
	/// 対局ポジション
	/// A(自分):B(下家):C(対面):D(上家)
	/// </summary>
	public enum MJ_POS : byte{
		A = 0,
		B,
		C,
		D,
		MAX,
	};

	/// <summary>
	/// 捨て牌表示用
	/// </summary>
	public enum SUTEHAI{
		NO_USE = 0,	//-*todo:数合わせ用不要なら詰める
		ON,			//-*捨て牌通常表示
		OFF,		//-*捨て牌非表示
		REACH,		//-*捨て牌立直表示
	};

	/// <summary>
	/// 危険牌
	/// </summary>
	public enum DANGER_MODE{
		NON	= 0,		//-*危険
		SUJI = 1,		//-*スジ(相手の河)
		ONE_CHANCE = 2,	//*１チャンス(すべての河+自分の手牌)
		KABE = 3,		//*壁(すべての河+自分の手牌)
		FOUR_JIPAI = 4,	//*字牌４枚見え(すべての河+自分の手牌)
		GENBUTU = 5,	//*現物(相手の河)
		MAX_DANGER_MODE = 6,
	};

	/// <summary>
	/// ルール種類
	/// </summary>
	public enum RULETYPE{
		NORMAL = 0,		//*標準
		TONPUU = 1,		//*東風
		HOKKAIDOU = 2,	//*北海道
		BUNYA = 3,		//*ブン屋
		BASHIBARI = 4,	//*場縛り
		KYOUGI = 5,		//*競技
		INFURE = 6,		//*インフレ
		FREE = 7,		//*自由設定
		FREEMODE = 8,	//*フリー対戦
		SURVIVAL = 9,	//*サバイバル
		WIRELESS = 10,	//*通信対戦
	};

	/// <summary>
	/// ルールデファイン 接頭語：RL_
	/// </summary>
	public enum RL{	
		KUITA = ( 0 ),		//*食い断ヤオ
		NAKIPN = ( 1 ),		//*ナキ平和
		PINHO = ( 2 ),		//*平和ツモ
		NOTEN = ( 3 ),		//*ノーテン罰符
		NANBA = ( 4 ),		//*流局設定
		URA = ( 5 ),		//*裏ドラ
		KAN = ( 6 ),		//*カンドラ
		KANUR = ( 7 ),		//*カンウラ
		IPPAT = ( 8 ),		//*一発賞
		DOBON = ( 9 ),		//*ドボン
		POINT = ( 10 ),		//*配給原点
		RETPOINT = ( 11 ),	//*返し点
		KAZE = ( 12 ),		//*場風
		SHANY = ( 13 ),		//*西入
		UMA = ( 14 ),		//*馬
		WINEND = ( 15 ),	//*和了止め
		NAGARE = ( 16 ),	//*途中流局
		RYANSH = ( 17 ),	//*二ﾊﾝ縛り
		KOKCHA = ( 18 ),	//*国士搶ｶﾝ
		RANKAN = ( 19 ),	//*リーチ後のアンカン
		RIBO = ( 20 ),		//*リー棒の戻り
		YAKI = ( 21 ),		//*焼き鳥
		WAREM = ( 22 ),		//*割れ目
		PAREN = ( 23 ),		//*八連荘
		IPSHO = ( 24 ),		//*一発賞
		RDSHO = ( 25 ),		//*裏ドラ賞
		YMSHO = ( 26 ),		//*役満賞
		TORIU = ( 27 ),		//*鳥撃ち
		ALICE = ( 28 ),		//*アリス
		KINCH = ( 29 ),		//*金鶏独立
		FIVE_PIN = ( 30 ),	//*五筒開花
		SANRE = ( 31 ),		//*三連刻
		SANJUN = ( 32 ),	//*一色三順
		YAOCHU = ( 33 ),	//*流し満貫
		SHISAN = ( 34 ),	//*十三不搭
		RENHO = ( 35 ),		//*人和
		DAISH = ( 36 ),		//*大車輪
		HUNDRED_MAN = ( 37 ),//*百万石
		DBYMAN = ( 38 ),	//*ダブル
		TWO_CHAHO = ( 39 ),	//*二家和
		THREE_CHAHO = ( 40 ),//*三家和
		RLSIZE = ( 41 ),	//*ルールテーブルのサイズ 
	};

	/// <summary>
	/// ルール変動最大値 接頭語：RULE_MAX_
	/// </summary>
	public enum RL_MAX{
		DEFAULT,	// 有り無し
		NANBA,		// 連荘
		DOBON,		// ドボン
		POINT,		// 配給原点
		RETPOINT,	// 返し点
		KAZE,		// 場風
		SHANY,		// 西入
		UMA,		// 馬
		NAGARE,		// 途中流局
		YAKI,		// 焼き鳥
		SHISAN,		// 十三不塔
	};

	/// <summary>
	/// 対局ステータスデファイン 接頭語：ST_
	/// </summary>
	public enum ST : byte{
		ONE_JUN		 = ( 0x01 ),	//*１順目ではない
		RON			 = ( 0x02 ),		//*ロン
		NAKI		 = ( 0x04 ),		//*フーロ
		RINSH		 = ( 0x08 ),		//*リンシャン
		RINFR		 = ( 0x10 ),		//*カン責任払い
		FOUR_KAN	 = ( 0x20 ),	//*四開カン
		CHANK		 = ( 0x40 ),		//*チャンカン
		SRICHI		 = ( 0x80 ),		//*リーチ
	};

	/// <summary>
	/// 対局オプションデファイン 接頭語：OP_
	/// </summary>
	public enum OP{
		PASS = ( 0 ),		//*パス
		NONE = ( 1 ),
		TSUMO = ( 1 ),		//*ツモ
		RON = ( 2 ),		//*ロン
		RICHI = ( 3 ),		//*リーチ
		CHI = ( 4 ),		//*チー
		PON = ( 5 ),		//*ポン
		KAN = ( 6 ),		//*カン
		TAOPAI = ( 7 ),		//*たおす 九種九牌
		CHANKAN = ( 8 ),	// 加槓(CHAKAN)なのに槍槓(CHANKAN)としているので、注意
		TAPAI = ( 9 ),
		THREE_CHAHO = ( 10 ),// 三家和の追加(例外的処理)
#region UNITY_ORIGINAL
		//-***鳴きメニュー用追加
		SUTE_HAI = (100),	//-*捨牌
#endregion //-*UNITY_ORIGINAL

	};

	/// <summary>
	/// 役デファイン 接頭語：YK_
	/// </summary>
	public enum YK{
		MENZE = ( 0 ),		//*面前清ツモ
		PINHO = ( 1 ),		//*平和
		TANYO = ( 2 ),		//*断ヤオ
		IPEKO = ( 3 ),		//*一盃口
		FANPA = ( 4 ),		//*飜牌 役牌
		RICHI = ( 5 ),		//*立直 リーチ
		HAITE = ( 6 ),		//*海底
		HOTEI = ( 7 ),		//*河底
		RINSH = ( 8 ),		//*嶺山開花
		CHANK = ( 9 ),		//*搶カン 槍槓
		DOSHU = ( 10 ),		//*一色三順
		CHANT = ( 11 ),		//*チャンタ
		SANSH = ( 12 ),		//*三色同順
		DOPON = ( 13 ),		//*三色同刻
		HONRO = ( 14 ),		//*混老頭
		TOITO = ( 15 ),		//*対々和
		SANKA = ( 16 ),		//*三槓子 三カン子
		ITSU = ( 17 ),		//*一気通貫
		RYANP = ( 18 ),		//*二盃口
		SANAN = ( 19 ),		//*三暗刻
		SHOSA = ( 20 ),		//*小三元
		CHITO = ( 21 ),		//*七対子
		WRICH = ( 22 ),		//*ダブル立直 リーチ
		HONIS = ( 23 ),		//*混一色
		JUNCH = ( 24 ),		//*純チャン
		CHINI = ( 25 ),		//*清一色
		YAOCH = ( 26 ),		//*ヤオ九振切
		SHISA = ( 27 ),		//*十三不搭
		TENHO = ( 28 ),		//*天和
		CHIHO = ( 29 ),		//*地和
		RENHO = ( 30 ),		//*人和
		DAISA = ( 31 ),		//*大三元
		SUKAN = ( 32 ),		//*四カン子 四槓子
		RYUIS = ( 33 ),		//*緑一色
		CHINR = ( 34 ),		//*清老頭
		TSUIS = ( 35 ),		//*字一色
		SHOSU = ( 36 ),		//*小四喜
		SUANK = ( 37 ),		//*四暗刻
		DAISU = ( 38 ),		//*大四喜
		KOKUS = ( 39 ),		//*国士無双
		CHURE = ( 40 ),		//*九連宝燈
		SANRE = ( 41 ),		//*三連刻
		SUREN = ( 42 ),		//*四連刻
		HADAK = ( 43 ),		//*裸単騎（金鶏独立）
		SHARI = ( 44 ),		//*大車輪
		ONE_PIN = ( 45 ),	//*一筒ラオ月
		//*欠番	46
		FIVE_PIN = ( 47 ),	//*五筒開花
		TWO_SO = ( 48 ),	//*二索搶カン
		PAREN = ( 49 ),		//*八連荘
		IPPAT = ( 50 ),		//*一発
		DORA = ( 51 ),		//*ドラ
		/** 1996.6.5. NEW **/
		HUNDRED_MAN = ( 52 ),//*百万石
		BENIK = ( 53 ),	//*紅孔雀
		DOMON = ( 54 ),	//*青の洞門

#if	Rule_2P
		//2008.1.7 NEW
		TSUANK = ( 55 ),	// 四暗刻単騎
		JCHURE = ( 56 ),	// 純正九連宝燈
		JKOKUS = ( 57 ),	// 純正国士無双
		MAX = ( 58 ),	// max
#else
		MAX = ( 55 ),	// max
#endif
	};

	public enum YKC{
		RICHI = ( 0 ),			//*立直 リーチ
		WRICH = ( 1 ),			//*ダブル立直 リーチ
		IPPAT = ( 2 ),			//*一発
		MENZE = ( 3 ),			//*面前清ツモ
		TANYO = ( 4 ),			//*断ヤオ
		PINHO = ( 5 ),			//*平和
		IPEKO = ( 6 ),			//*一盃口
		SANSH = ( 7 ),			//*三色同順
		ITSU = ( 8 ),			//*一気通貫
		TOITO = ( 9 ),			//*対々和
		DOPON = ( 10 ),			//*三色同刻
		SANKA = ( 11 ),			//*三カン子
		SANAN = ( 12 ),			//*三暗刻
		CHANT = ( 13 ),			//*チャンタ
		CHITO = ( 14 ),			//*七対子
		SHOSA = ( 15 ),			//*小三元
		HONRO = ( 16 ),			//*混老頭
		DOSHU = ( 17 ),			//*一色三順
		SSHAR = ( 18 ),			//*小車輪
		SANRE = ( 19 ),			//*三連刻
		HONIS = ( 20 ),			//*混一色
		RYANP = ( 21 ),			//*二盃口
		CHINP = ( 22 ),			//*清盃口
		JUNCH = ( 23 ),			//*純チャン
		CHINI = ( 24 ),			//*清一色
		CHANK = ( 25 ),			//*搶カン
		KINKE = ( 26 ),			//*裸単騎（金鶏独立）
		FIVE_PIN = ( 27 ),		//*五筒開花
		ONE_PIN = ( 28 ),		//*一筒ラオ月
		TWO_SO = ( 29 ),		//*二索搶カン
		HAITE = ( 30 ),			//*海底
		HOTEI = ( 31 ),			//*河底
		RINSH = ( 32 ),			//*嶺山開花
		FANPA = ( 33 ),			//*飜牌
		DORA = ( 34 ),			//*ドラ
		YAOCH = ( 35 ),			//*ヤオ九振切
		SHISA = ( 36 ),			//*十三不搭
		TENHO = ( 37 ),			//*天和
		CHIHO = ( 38 ),			//*地和
		RENHO = ( 39 ),			//*人和
		KOKUS = ( 40 ),			//*国士無双
		CHURE = ( 41 ),			//*九連宝燈
		SUANK = ( 42 ),			//*四暗刻
		DAISU = ( 43 ),			//*大四喜
		SHOSU = ( 44 ),			//*小四喜
		DAISA = ( 45 ),			//*大三元
		RYUIS = ( 46 ),			//*緑一色
		TSUIS = ( 47 ),			//*字一色
		CHINR = ( 48 ),			//*清老頭
		SUKAN = ( 49 ),			//*四カン子
		HUNDRED_MAN = ( 50 ),	//*百万石
		SHARI = ( 51 ),			//*大車輪
		SUREN = ( 52 ),			//*四連刻
		BENIK = ( 53 ),			//*紅孔雀
		DOMON = ( 54 ),			//*青の洞門
		PAREN = ( 55 ),			//*八連荘
	};

	/// <summary>
	/// 特殊処理デファイン（前ソースではピンチ） 接頭語：SP_
	/// </summary>
	public enum SP :byte{
		AGARI = ( 0 ),		//*和了
		HOAN = ( 1 ),		//*流局
		FOUR_KAN = ( 2 ),	//*四開カン
		FOUR_FON = ( 3 ),	//*四風連打
		THREE_CHAN = ( 4 ),	//*トリロン
		FOUR_RIC = ( 5 ),	//*四家立直
		NINE_HAI = ( 6 ),	//*九種九牌
		YAOCHU = ( 7 ),		//*流し満貫
	};

	/// <summary>
	/// comput_m1の戻り 接頭語：COMPUT_RET
	/// </summary>
	public enum COMPUT_RET{
		NON = (0),		//*牌を切る
		TSUMO = (1),	//*ツモ
		KAN = (2),		//*カン
		ELSE = (3),		//*その他(９種９牌
	};

	/// <summary>
	/// フラグ 接頭語：TBLMEMF_
	/// </summary>
	[System.Flags]
	public enum TBLMEMF{
		TORI = (1<<0),		//*焼き鳥フラグ
		DOBON = (1<<1),		//*箱割れ
		TORIOLD = (1<<2),	//*局開始時の焼き鳥状態
	};

	/// <summary>
	/// フラグ 接頭語：TBLF_
	/// </summary>
	[System.Flags]
	public enum TBLF{
		USER = (1<<0),			//*ユーザ参加ゲーム
		ACTIVE = (1<<1),		//*使用中
		FINISH = (1<<2),		//*ゲーム終了
		NEXTBA_VOICE = (1<<3),	//*次の場に移ったメッセージを言った
	};

	/// <summary>
	/// フラグ 接頭語：RULESUBFLAG_
	/// </summary>
	[System.Flags]
	public enum RULESUBFLAG{
		CHIP = (1<<0),		//*	チップ処理あり
		RENCHAN = (1<<1),	//*	流局時必ず連荘
//		SASHIUMA = (1<<2),	//*	差し馬可能
	};


	/// <summary>
	/// フラグ 接頭語：RESF_
	/// </summary>
	[System.Flags]
	public enum RESF{
		NAKI = (1<<0),				//*鳴いた
		N_FIRST = (1<<1),			//*最初に鳴いた
		TENPAI = (1<<2),			//*流局時テンパイ
		RON = (1<<3),				//*ロン上がり
		RICH = (1<<5),				//*リーチした
		R_FIRST = (1<<6),			//*最初にリーチした

		AGARI = (1<<8),				//*上がった
		YAKUMAN = (1<<9),			//*役満
		MANGAN = (1<<10),			//*満貫
		IPPATU = (1<<11),			//*一発
	};

	/// <summary>
	/// 局の結果 接頭語：KEKKA_
	/// </summary>
	public enum KEKKA:byte{
		RON = ( 0 ),		//* ロンアガリ */
		TSUMO = ( 1 ),		//* ツモアガリ */
		HOUJU = ( 2 ),		//* 放銃       */
		NAGARE = ( 3 ),		//* 流局       */
		KEKKA_NONE = ( (int)MJDefine.NONE ),	//* 無し       */
	};


		/******************************************************************************
		**	面子データ
		******************************************************************************/
	/// <summary>
	/// 面子データ 接頭語：MNTFLAG_
	/// </summary>
	[System.Flags]
	public enum MNTFLAG{
		//*	byFuFlag
		TSUMO = (1<<0),	//*	ツモ
		MACHI = (1<<1),	//*	待ち
		MENZEN = (1<<2),//*	面前ロン
		CHITOI = (1<<3),//*	七対子
	};

	public enum SURVIVAL{
		NONE = (0),
		MEM = (1),
		DROP = (2),
	};

	//* サバイバル成績
	public enum WANTYPE{
		REV = 0,
		FRONT = 1,
		NON = 2,
	};
	/*****************************END OF FILE************************************/
}

/// <summary>
/// 麻雀大会 for PS2
/// "gamedefs.h" ゲームデファイン
/// </summary>
namespace GameDefsHeader{


	/// <summary>
	/// ゲームモード
	/// </summary>
	//*	byFlags
	[System.Flags]
	public enum GAMEFLAG{
		CLEAR = (1<<0),		//*	ゲームをクリアしている
		AUTO = (1<<1),		//*	０人プレイ
		DEBUG = (1<<7),		//*	☆デバッグ
		GAMEOVER = (1<<6),	//*	ゲームオーバー
	};



	/*****************************
		プレート類デファイン
	*****************************/
	/*******************
		JPL_ = 状況プレート
			_KYO_ = 対局
			_KAI_ = 大会
			_MEN_ = メニュー
		SPL_ = 選択プレート
			_BIG_ = 大
			_SML_ = 小
			ルールはルールデファインの番号使用
	*******************/

	// 選択ダイアログの戻り値など
	/// <summary>
	/// todo:不要？接頭語：KAKUSHI_
	/// </summary>
	//*	byFlags
	[System.Flags]
	public enum KAKUSHI {
		CHR11 = (1<<0),		//*海原匠
		CHR12 = (1<<1),		//*シブサワコウ
		CHR13 = (1<<2),		//*磯部慎吾
		CHR10 = (1<<3),		//*水島リヨ子
		CHR06 = (1<<4),		//*藤波泰男
	};

	/***********	ゲーム用グローバルコンストテーブル	スタート	*************/

	// 選択ダイアログの戻り値など
	/// <summary>
	/// 麻雀牌のテーブル概念化(自家) 接頭語：PAI_
	/// </summary>
	public enum PAI {
	// 萬子
		M1 = (0x01),
		M2 = (0x02),
		M3 = (0x03),
		M4 = (0x04),
		M5 = (0x05),
		M6 = (0x06),
		M7 = (0x07),
		M8 = (0x08),
		M9 = (0x09),
	// 索子
		S1 = (0x11),
		S2 = (0x12),
		S3 = (0x13),
		S4 = (0x14),
		S5 = (0x15),
		S6 = (0x16),
		S7 = (0x17),
		S8 = (0x18),
		S9 = (0x19),
	// 筒子
		P1 = (0x21),
		P2 = (0x22),
		P3 = (0x23),
		P4 = (0x24),
		P5 = (0x25),
		P6 = (0x26),
		P7 = (0x27),
		P8 = (0x28),
		P9 = (0x29),
	// 字牌
		TON = (0x31),
		NAN = (0x32),
		SYA = (0x33),
		PEI = (0x34),
		HAKU = (0x35),
		HATSU = (0x36),
		CHUN = (0x37),

	//#define PAI_D_P5	0x38
	// 裏牌
		URA = (0x38),
	};


	// フーロ時(それぞれ萬子・索子・筒子・字牌の順)
	// チー  ：先頭の牌コード
	// ポン  ：0x4? 0x5? 0x6? 0x7?
	// 暗カン：0x8? 0x9? 0xA? 0xB?
	// 明カン：0xC? 0xD? 0xE? 0xF?

	/*********************************************************************
		固定ルールセット
	食断  鳴平	平ﾂﾓ  ﾉｰ罰	流局	裏ド  ｶﾝド	ｶﾝ裏  一発	ドボ
	持点  返点	場風  西入	うま	和了  途流	二縛  国搶	立暗
	棒戻  焼鳥	割目  八連	一賞	裏賞  役賞	鳥撃  ｱﾘス	金鶏
	五筒  三連	一色  流満	十三	人和  車輪	百万  ﾀﾞﾌﾞ	二和
	三和
	*********************************************************************/
	/// <summary>
	/// メニュー番号 接頭語：D_RULE_CHK_
	/// </summary>
	public enum D_RULE_CHK {
		POSITION1 = 0,	// 表示上　ルール１番目
		POSITION2 = 1,	// 表示上　ルール２番目
		POSITION3 = 2,	// 表示上　ルール３番目
		POSITION4 = 3,	// 表示上　ルール４番目
		POSITION5 = 4,	// 表示上　ルール５番目
		POSITION6 = 5,	// 表示上　ルール６番目
		POSITION7 = 6,	// 表示上　ルール７番目
		RULE_MAX = 7,	// 選択MAX

		PASTPAGE = 7,	// 表示上　前ページ
		NEXTPAGE = 8,	// 表示上　次ページ
		BACK = 9,		// 表示上　戻る
		SET = 10,		// 表示上　決定
		MAX = 11,		// 選択MAX
	};


	/// <summary>
	/// メニュー番号 接頭語：RV_
	/// </summary>
	public enum RV{
		RV_ARI = 0,			//;有り
		RV_NASHI = 1,		//;無し
		RV_20PU = 2,		//;２０符
		RV_30PU = 3,		//;３０符
		RV_TENPAI = 4,		//;聴牌
		RV_NANBA = 5,		//;南場
		RV_AGARI = 6,		//;和了
		RV_0 = 7,			//;０
		RV_10 = 8,			//;１０
		RV_15 = 9,			//;１５
		RV_20 = 10,			//;２０
		RV_30 = 11,			//;３０
		RV_16000 = 12,		//;16000
		RV_24000 = 13,		//;24000
		RV_25000 = 14,		//;25000
		RV_26000 = 15,		//;26000
		RV_27000 = 16,		//;27000
		RV_28000 = 17,		//;28000
		RV_29000 = 18,		//;29000
		RV_30000 = 19,		//;30000
		RV_30100 = 20,		//;30100
		RV_33100 = 21,		//;33100
		RV_35100 = 22,		//;35100
		RV_TONNAN =23,		//;東南
		RV_TONPU = 24,		//;東風
		RV_TONTON = 25,		//;東東
		RV_MANGAN = 26,		//;満貫
		RV_BAIMAN = 27,		//;倍満
		RV_YAKUMAN = 28,	//;役満
		RV_RONCHAN = 29,	//;輪荘
		RV_RENCHAN = 30,	//;連荘
		RV_URA10 = 31,		//;裏１０
		RV_URA15 = 32,		//;裏１５
		RV_URA20 = 33,		//;裏２０
		RV_0_5 = 34,		//;0-5
		RV_0_10 = 35,		//;0-10
		RV_0_20 = 36,		//;0-20
		RV_0_30 = 37,		//;0-30
		RV_5_10 = 38,		//;5-10
		RV_10_20 = 39,		//;10-20
		RV_10_30 = 40,		//;10-30
		RV_20_30 = 41,		//;20-30
		RV_20000 = 42,		//;20000
		RV_SPACE = 43,		//;無地
	};

	/// <summary>
	/// メニュー 接頭語：MENU_
	/// </summary>
	public enum MENU_SEL{
		NO_SEL = 0,		//非選択
		SEL = 1,		//選択
		TOUCH = 2,		//タッチ
		KETTEI = 3,		//決定
		DISABLE = 4,	//無効
	};

	/*****************************END OF FILE************************************/
	
}