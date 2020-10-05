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

//-*****************
// mj.j
//-*****************
public partial class MahjongBase : SceneBase {
//#define AUTO_TEST
//#define WIN_TEST
//#define ONEGAME_TEST

//#include "MahJongRally.h"								// Module interface definitions

//extern void tsumikomi( /*MahJongRally **/ );
/****************************************
	「次へ」の受付
****************************************/
public bool NextButton(/*MahJongRally * pMe*/){
	//int	i;
#if AUTO_TEST
	return(true);
#endif
#if false //-*todo;キー操作
	// こちらしか通らないわけだが...
	if(( (GetKeyTrg(KEY_SELECT2))!= 0 | (uint32)gAutoFlag != 0 )) {	//( (GetKeyTrg(KEY_SELECT2)) || (uint32)gAutoFlag )
		SeOnPlay( SE_KETTEI, SEQ_SE);
		return (TRUE);
	} else
		return(FALSE);
#else
#region GAME_PAD
	if(IsKeyBtnPress(KEY_NAME.SELECT,true)){
		nextBtnF =true;
	}
#endregion //-*GAME_PAD

	if(nextBtnF){
	//-*次へボタンが押された
		nextBtnF = false;	//-*フラグOFF
		SetNextBtnActive(false);
		return true;	//-*todo:キー操作(保留
	}else{
		SetNextBtnActive(true);
		DebLog("//-*from.................");
	}
	return false;
#endif
}

/****************************************
	各ゲームモード開始時の初期化
****************************************/
public void	MjModeInit(/*MahJongRally * pMe*/)
{
	DebLog(("Init for Mode Init"));

	switch (sGameData.byGameMode) {
	  case MJDefine.GAMEMODE_SURVIVAL:			/*	サバイバル		*/
		SetupRule((int)RULETYPE.SURVIVAL);
		sel_buf[0] = (char)0;
		sel_buf[1] = (char)1;
		sel_buf[2] = (char)2;
		game_survival();
		break;
	  case MJDefine.GAMEMODE_FREE:				/*	フリー対局		*/
		game_free();
		break;
	  case MJDefine.GAMEMODE_NET_FREE:			/* 通信　フリー対戦	*/
	  case MJDefine.GAMEMODE_NET_RESERVE:		/* 通信　卓指定対戦	*/
		#if false //-*todo:ネット対戦
		SetupRule( (int)RULETYPE.WIRELESS);
		game_wireless();	// tamaki 20051121 del
		#endif //-*todo:ネット対戦
		break;
	}
//	_byEndingMode = 0;
	_initTableData();
	mj_sts = 1;

	init_sutehai_rec();					/* 捨て牌表示管理バッファの初期化 */
	chipai_m2();						/* 局のイニット */
	gsPlayerWork[0].byTkhai = 0;
	gsPlayerWork[1].byTkhai = 0;
	gsPlayerWork[2].byTkhai = 0;
	gsPlayerWork[3].byTkhai = 0;
}

#if	GAME_SWITCH
/****************************************
	半荘開始時の初期化
****************************************/
public void MjHanchanInit(/*MahJongRally * pMe*/)
{
	int	cnt;
	#if true //-*todo:
	DebLog(("Init for Hanchan Start"));
	#endif //-*todo
	_setTableData();													/*	対局者情報セット	*/
	gpsTableData	=	gsTableData[0];		//&gsTableData[0];

	if((gpsTableData.byFlags & (byte)TBLF.USER) != 0 && !IsGameAutoPlay()) {	//gpsTableData.byFlags
		//gsPlayerWork[0].byPlflg	=	0xFF;	/* オートモードＯＦＦ */
											/* 2005/11/11 chage		*/
		gsPlayerWork[game_player].byPlflg	=	0xFF;	/* オートモードＯＦＦ	*/
		SubMj.CompVSCompMode			=	false;
		sGameData.byGuide	=	0;
	} else {
		gsPlayerWork[0].byPlflg	=	0x00;	/* オートモードＯＮ */
		SubMj.CompVSCompMode			=	true;
	}

//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
#if AUTO_TEST
		gsPlayerWork[0].byPlflg	=	0x00;	/* オートモードＯＮ */			//テストでプレーヤーもオートモードにしてみる
#endif
//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@

	MjKyokuInit_bflag = 0;

	//20051116 add
	hai_csr = 14;

#if false //-*todo:通信
	/* ここは通信設定必要？		*/
	if( IS_NETWORK_MODE ) {
		for(cnt=0;cnt<4;cnt++)
			if(cnt!=game_player)
				gsPlayerWork[cnt].byPlflg	=	0;

		// 対戦の最初にフラグを初期化
		// 代打ちフラグ初期化
		select_opt[D_SELECT_OPT_DAIUCHI] = D_ONLINE_OPERATOR_MANUAL;
		updateDaiuchiFlag(  );

		// 切断時フラグ初期化
		select_opt[D_SELECT_OPT_SETSUDAN] = f_info.f_optionsetting.m_OptionSel[D_OPTION_SETUDAN_CYU];
		updateSetsudanFlag(  );

		// 鳴き無しフラグ初期化
		select_opt[D_SELECT_OPT_NAKI_NASHI] = D_OPTION_NAKINASHI_OFF;
#if	Rule_2P
		//鳴き禁止
		if(( myCanvas.mah_limit_num== MAH_LIM01)) {
			// 鳴きなし。ポン・チーを鳴かなくする。
			select_opt[D_SELECT_OPT_NAKI_NASHI] = D_OPTION_NAKINASHI_ON;
		}
#endif
		updateNakiNashiFlag(  );

		// その他オプションロード
		OptionSetData( D_OPTION_FLG_SET );
	} else {
		gsPlayerWork[1].byPlflg	=	0;
		gsPlayerWork[2].byPlflg	=	0;
		gsPlayerWork[3].byPlflg	=	0;
	}
#else //-*todo:通信
		gsPlayerWork[1].byPlflg	=	0;
		gsPlayerWork[2].byPlflg	=	0;
		gsPlayerWork[3].byPlflg	=	0;
#endif //-*todo:通信
	mj_sts = 2;
}

/****************************************
	局処理
****************************************/
public void MjKyokuMain(/*MahJongRally * pMe*/)
{
	reentry_m1();			/* 局ループ */
}

/****************************************
	局開始時の初期化
****************************************/
public void	MjKyokuInit(/*MahJongRally * pMe*/)
{
	byte i;

DebLogError(( "MjKyokuInit_bflag : flag="+MjKyokuInit_bflag ));
#if false //-*描画
#if	Rule_2P
	FaceNum= 0;			//キャラクター表情
	//*****ウキウキ:表情操作*****
	faceChangeCnt = 0;		//*表情操作用カウンター
	//***************************
#endif
#endif //-*描画

	switch( MjKyokuInit_bflag) {
		case	0:
			if (SubMj.CompVSCompMode)				/*	COMP VS COMP	*/
				SubMj.Opnflg	= 0xFF;
			else
				SubMj.Opnflg	= (byte)(IsGameAutoPlay() ? 0xFF : 0x00);

			for( i = 0; i < MJDefine.MAX_TABLE_MEMBER; i++)
				SubMj.RiboDispFlg[i] = 0;

			Order = 0;

			if( (sGameData.byGameMode != MJDefine.GAMEMODE_NET_FREE) &&
				(sGameData.byGameMode != MJDefine.GAMEMODE_NET_RESERVE) ) {
				shipai_m2();					/* 牌のかき混ぜ						*/
//-*SUGI_DEB***************************
#if SUGI_DEB //-*todo:注デバッグ中
// #if __LOGIC_CHECK
				tsumikomi();
// #endif
#endif //-*todo:注デバッグ中
//-****************************SUGI_DEB	
			}

			init_sutehai_rec();					/* 捨て牌表示管理バッファの初期化	*/
			InitMJKResult();					/* 局結果データの初期化				*/
			chipai_m2();						/* 局のイニット						*/

			MjKyokuInit_bflag = 1;
			haipai_m2_bflag = 0;
			agari_sts = 0;		//0422
			gsPlayerWork[0].byTkhai = 0;
			gsPlayerWork[1].byTkhai = 0;
			gsPlayerWork[2].byTkhai = 0;
			gsPlayerWork[3].byTkhai = 0;

#if	Rule_2P
      		//旧ポイントをキープ
			for ( i = 0; i < MJDefine.MAX_TABLE_MEMBER2; i++ )
				gpsTableData.sMemData[i].nOldPoint	= gpsTableData.sMemData[i].nPoint;
#endif
			goto case 1;
		case	1:
			MjKyokuInit_bflag = 2;
			goto case 2;
		case	2:
			haipai_m2();		//配牌
			if(haipai_m2_bflag == 0xff)
				MjKyokuInit_bflag = 3;
			break;

		case	3:
			intpc_pcnt();					/* アルゴ用ワーク初期化				*/
			Order = gpsTableData.byOya;		/* 局のスタート時は親のツモから		*/
			setwp_m2( Order);				/* 手牌ポインターセット				*/

			reentry_m1_bflag = 1;

			// 全てのフラグの初期化
			//> 2006/03/16 バグNo.186
			//> 通信対戦をサスペンドした後にフラグが初期化されなずに順番の飛ばしが発生する。
			comput_m1_bflag       = 0;		//0422
			keyin_m6_bflag        = 0;		//0422
			ronho_m5_bflag        = 0;		//0422
			before_richi_m4_bflag = 0;		//0422
			after_richi_m4_bflag  = 0;		//0422
			chakan_m6_bflag       = 0;		//0422
			ankan_m6_bflag        = 0;		//0422
			naki_m6_bflag         = 0;		//0422
			//< 2006/03/16 バグNo.186

			//< 2006/03/17 バグNo.191-1
			// 国士暗槓有りで最初に幺九牌の槓を行うと前の局の鳴き結果によりバグが発生する。
			gNakiResult[0] = gNakiResult[1] = gNakiResult[2] = gNakiResult[3] = (byte)OP.PASS;		//0422
			//< 2006/03/17 バグNo.191-1

			_reentry_sub1();		/*	開局前 DLG メッセージ処理	*/

			//焼き鳥フラグ→局開始時の焼き鳥状態（４人分）
			for(i = 0; i < MJDefine.MAX_TABLE_MEMBER; i++)
				if((gpsTableData.sMemData[i].byMemFlags & (byte)TBLMEMF.TORI) != 0){
					gpsTableData.sMemData[i].byMemFlags	|=	(byte)TBLMEMF.TORIOLD;
				}else{
					#if true //-*todo:
					// gpsTableData.sMemData[i].byMemFlags	&=	~TBLMEMF.TORIOLD;
					gpsTableData.sMemData[i].byMemFlags	=	(byte)(gpsTableData.sMemData[i].byMemFlags & ~(byte)TBLMEMF.TORIOLD);
					#endif //-*todo:
				}

			MjKyokuInit_bflag = 0;
//			game_event_flg = 0;

			Optno[0] = 0;
			Optno[1] = 0;
			Optno[2] = 0;
			Optno[3] = 0;
			player_pon_flg[0] = 0;
			player_pon_flg[1] = 0;
			player_pon_flg[2] = 0;
			player_pon_flg[3] = 0;
//			agari_sts = 0;		//0422

			// 局ごとの鳴き有無のオプションの初期化
			// 鳴き無し初期化
			select_opt[(int)D_SELECT_OPT.NAKI_NASHI] = (byte)D_OPTION_NAKINASHI.OFF;
#if	Rule_2P
			//鳴き禁止
			#if true //-*todo:
			// if( (myCanvas.mah_limit_num== MAH_LIM01) ) 
			if( (m_keepData.mah_limit_num== (int)MAH.LIM01) ) 
			{
				// 鳴きなし。ポン・チーを鳴かなくする。
				select_opt[(int)D_SELECT_OPT.NAKI_NASHI] = (byte)D_OPTION_NAKINASHI.ON;
			}
			#endif //-*todo:
#endif
			#if false //-*todo:保留
			updateNakiNashiFlag(  );
			#endif //-*todo:保留
			break;
		}
}
#endif

/*****************************
	半荘終了結果表示
*****************************/
public void hanchan_result ( /*MahJongRally * pMe*/ )
{
	short	i, buf;
	short	retpnt;				/* 返し点1996.7.15.MENU */

	short[]	RetSpnttable	=	new short[]{ 30, 20 };

	SubMj.dobon_buf = 0;

	/* リー棒戻し */
	ribo_return();

	/** 順位の決定 **/
	rank_sort ( (ushort) 1 );

	/* gpsTableData.sMemData[i].nPnt	に配給原点をいれる */
	retpnt = RetSpnttable[Rultbl[(int)RL.RETPOINT]];
	for( i = 0; i < MJDefine.MAX_TABLE_MEMBER; i++ )
		gpsTableData.sMemData[i].nMovePnt	=	0;

	/* ここから結果得点に変わる(以前は０表示内部１００００) */

	/*** トップ賞 ***/
	for( i = 0; i < MJDefine.MAX_TABLE_MEMBER; i++ ){
		gpsTableData.sMemData[i].nMovePnt	=	-1;
		buf = gpsTableData.sMemData[i].nPnt;
		if( buf < 0 ) {
			if ( Rultbl[(int)RL.DOBON] != 0 )
			{	
				gpsTableData.sMemData[i].byMemFlags	|=	(byte)TBLMEMF.DOBON;
			}
			#if true //-*todo:
			// (SubMj.dobon_buf) |= ( 1 << i );
			SubMj.dobon_buf |= (byte)( 1 << i );			/* ドボンならビットを立てておく */
			#endif //-*todo:
			gpsTableData.sMemData[i].nPnt	=	/* ５捨６入 */
				(short)(( buf / 10 ) + ( ( ( ( - buf ) % 10 ) >= 5 ) ? -1 : 0 ) - retpnt);
		} else {
			gpsTableData.sMemData[i].nPnt	=	/* ５捨６入 */
				(short)(( buf / 10 ) + ( ( ( buf % 10 ) >= 6 ) ? 1 : 0 ) - retpnt);
		}
		if( gpsTableData.sMemData[i].byRank == 0 )
			SubMj.hanchan_result_top = i;
	}
	for( i = 0, buf = 0; i < MJDefine.MAX_TABLE_MEMBER; i++ )
		if( i != SubMj.hanchan_result_top )
			buf += gpsTableData.sMemData[i].nPnt;

	gpsTableData.sMemData[SubMj.hanchan_result_top].nMovePnt	=	1;
	gpsTableData.sMemData[SubMj.hanchan_result_top].nPnt	=	(short)( buf * -1 );

#if _DEBUG
	TRACE( "TOP %d:%d,%d:%d,%d:%d,%d:%d",
		gpsTableData.sMemData[0].nPnt,gpsTableData.sMemData[0].nMovePnt,
		gpsTableData.sMemData[1].nPnt,gpsTableData.sMemData[1].nMovePnt,
		gpsTableData.sMemData[2].nPnt,gpsTableData.sMemData[2].nMovePnt,
		gpsTableData.sMemData[3].nPnt,gpsTableData.sMemData[3].nMovePnt
		);
#endif
}

/*****************************
	半荘終了結果表示（馬）
*****************************/
public bool hanchan_result_uma ( /*MahJongRally * pMe*/ )
{
	/* 馬払いテーブル１・４位 */
	byte[]		Umatbl_top = new byte[]{  0,  5, 10, 20, 30, 10, 20, 30, 30 };
	/* 馬払いテーブル２・３位 */
	byte[]		Umatbl_2nd = new byte[]{  0,  0,  0,  0,  0,  5, 10, 10, 20 };

	short	i, buf;
	bool	ret = false;

	/*** 馬 ***/
	if ( Rultbl[(int)RL.UMA] != 0 )
	{
		for( i = 0; i < MJDefine.MAX_TABLE_MEMBER; i++ )
		{
			if( gpsTableData.sMemData[i].byRank == 0 )
			{
				buf = (short)Umatbl_top[Rultbl[(int)RL.UMA]];
			}
			else if( gpsTableData.sMemData[i].byRank == 1 )
			{
				buf = (short)Umatbl_2nd[Rultbl[(int)RL.UMA]];
			}
			else if( gpsTableData.sMemData[i].byRank == 2 )
			{
				buf = (short)(Umatbl_2nd[Rultbl[(int)RL.UMA]] * -1);
			}
			else
			{
				buf = (short)(Umatbl_top[Rultbl[(int)RL.UMA]] * -1);
			}
			gpsTableData.sMemData[i].nMovePnt	=	buf;
			gpsTableData.sMemData[i].nPnt		+= buf;
		}
		ret = true;
#if _DEBUG
	TRACE( "UMA %d:%d,%d:%d,%d:%d,%d:%d",
		gpsTableData.sMemData[0].nPnt,gpsTableData.sMemData[0].nMovePnt,
		gpsTableData.sMemData[1].nPnt,gpsTableData.sMemData[1].nMovePnt,
		gpsTableData.sMemData[2].nPnt,gpsTableData.sMemData[2].nMovePnt,
		gpsTableData.sMemData[3].nPnt,gpsTableData.sMemData[3].nMovePnt
		);
#endif
	}
	return ret;
}
/*****************************
	半荘終了結果表示（ドボン）
*****************************/
public bool hanchan_result_dobon ( /*MahJongRally * pMe*/ )
{
	/* ドボン払いテーブル */
	byte[]		Dbntbl = new byte[]{ 0, 0, 10, 20, 30 };

	short	i, buf;
	bool	ret = false;
	/*** ドボン ***/
	if (Dbntbl[Rultbl[(int)RL.DOBON]] != 0) {
		if( SubMj.dobon_buf != 0 && gpsTableData.byKamicha_dori != 0xFF) {
			for( i = 0; i < MJDefine.MAX_TABLE_MEMBER; i++ ){
				gpsTableData.sMemData[i].nMovePnt	=	0;
			}
			for( i = 0; i < MJDefine.MAX_TABLE_MEMBER; i++ ){
				if( i == gpsTableData.byKamicha_dori ) continue;
				if(( SubMj.dobon_buf & (1 << i) ) != 0){
					buf = (short)Dbntbl[Rultbl[(int)RL.DOBON]];
					gpsTableData.sMemData[i].nPnt					-=	buf;
					gpsTableData.sMemData[gpsTableData.byKamicha_dori].nPnt		+=	buf;

					gpsTableData.sMemData[i].nMovePnt				=	(short)(-buf);
					gpsTableData.sMemData[gpsTableData.byKamicha_dori].nMovePnt	=	buf;
				}
			}
			ret = true;
		}
#if _DEBUG
	TRACE( "DOBON %d:%d,%d:%d,%d:%d,%d:%d",
		gpsTableData.sMemData[0].nPnt,gpsTableData.sMemData[0].nMovePnt,
		gpsTableData.sMemData[1].nPnt,gpsTableData.sMemData[1].nMovePnt,
		gpsTableData.sMemData[2].nPnt,gpsTableData.sMemData[2].nMovePnt,
		gpsTableData.sMemData[3].nPnt,gpsTableData.sMemData[3].nMovePnt
		);
#endif
	}
	return(ret);
}
/*****************************
	半荘終了結果表示（焼き鳥）
*****************************/
public bool hanchan_result_yakitori ( /*MahJongRally * pMe*/ )
{
	/* 焼き鳥払いテーブル */
	byte[]		Tortbl = new byte[]{ 0, 10, 15, 20, 10, 15, 20 };

	short	i, buf;
	byte	toriget = 0;
	bool	ret = false;

	/*** 焼き鳥 ***/
	if ( Rultbl[(int)RL.YAKI] != 0 ) {
		for( i = 0; i < MJDefine.MAX_TABLE_MEMBER; i++ ){
			if ((gpsTableData.sMemData[i].byMemFlags & (byte)TBLMEMF.TORI) == 0x00){		/* 焼き鳥クリア */
				toriget ++;
			}
		}
		if( (toriget != 0) && (toriget != MJDefine.MAX_TABLE_MEMBER) ){
			buf = (short)Tortbl[Rultbl[(int)RL.YAKI]];
			for( i = 0; i < MJDefine.MAX_TABLE_MEMBER; i++ ){
				if ((gpsTableData.sMemData[i].byMemFlags & (byte)TBLMEMF.TORI) != 0 ){	/* 焼き鳥払い */
					#if true //-*todo:
					// gpsTableData.sMemData[i].nPnt	-= buf * toriget;
					gpsTableData.sMemData[i].nPnt	= (short)(gpsTableData.sMemData[i].nPnt - (buf * toriget));
					#endif //-*todo:
					gpsTableData.sMemData[i].nMovePnt	=	(short)(-buf * toriget);
				}
				else{						/* 焼き鳥もらう */
					#if true //-*todo:
					// gpsTableData.sMemData[i].nPnt	+= buf * (MJDefine.MAX_TABLE_MEMBER - toriget);
					gpsTableData.sMemData[i].nPnt	= (short)(gpsTableData.sMemData[i].nPnt+(buf * (MJDefine.MAX_TABLE_MEMBER - toriget)));
					#endif //-*todo:
					gpsTableData.sMemData[i].nMovePnt	=	(short)(buf * (MJDefine.MAX_TABLE_MEMBER - toriget));
				}
			}
			ret = true;
		}
#if _DEBUG
	TRACE( "YAKI %d:%d,%d:%d,%d:%d,%d:%d",
		gpsTableData.sMemData[0].nPnt,gpsTableData.sMemData[0].nMovePnt,
		gpsTableData.sMemData[1].nPnt,gpsTableData.sMemData[1].nMovePnt,
		gpsTableData.sMemData[2].nPnt,gpsTableData.sMemData[2].nMovePnt,
		gpsTableData.sMemData[3].nPnt,gpsTableData.sMemData[3].nMovePnt
		);
#endif
	}
	return(ret);
}
/*****************************
	半荘終了結果表示（チップ）
*****************************/
public bool hanchan_result_tip ( /*MahJongRally * pMe*/ )
{
	short	i;
	bool	ret;
	/*	チップ	*/
	ret	=	false;
	if(sRuleSubData.byChipRate != 0) {
		for( i = 0; i < MJDefine.MAX_TABLE_MEMBER; i++ ) {
			gpsTableData.sMemData[i].nMovePnt	=	(short)(gpsTableData.sMemData[i].nChip * sRuleSubData.byChipRate);

			if(gpsTableData.sMemData[i].nMovePnt != 0) {
				ret	=	true;
				gpsTableData.sMemData[i].nPnt		+=	gpsTableData.sMemData[i].nMovePnt;
			}
		}
#if _DEBUG
	TRACE( "TIP %d:%d,%d:%d,%d:%d,%d:%d",
		gpsTableData.sMemData[0].nPnt,gpsTableData.sMemData[0].nMovePnt,
		gpsTableData.sMemData[1].nPnt,gpsTableData.sMemData[1].nMovePnt,
		gpsTableData.sMemData[2].nPnt,gpsTableData.sMemData[2].nMovePnt,
		gpsTableData.sMemData[3].nPnt,gpsTableData.sMemData[3].nMovePnt
		);
#endif
	}
	return(ret);
}


/****************************************
	半荘終了時の処理
****************************************/
public void MjHanchanEnd(/*MahJongRally * pMe*/)
{
	if ((gpsTableData.byFlags & (byte)TBLF.ACTIVE) != 0) {
		SubMj.CompVSCompMode	=	((gpsTableData.byFlags & (byte)TBLF.USER) == 0);
		hanchan_result();
	}
}


/****************************************
	和了画面初期化
****************************************/
public void AgariDispInit(/*MahJongRally * pMe*/)
{
#if	FAST_TEST
	agari_sts = SEQ_AG_END4;
	return;
#endif

	if ( SubMj.CompVSCompMode ) {
		agari_sts = (byte)SEQ.AG_END4;
		return;
	}

	if ( SubMj.kyoku_end_mode == 1 ) {
		agari_sts = (byte)SEQ.AG_DCLR;

		gMJKResult= (MJK_RESULT)gMJKResultSub[SubMj.result_disp_no].clone();	//MEMCPY(&gMJKResult,&gMJKResultSub[result_disp_no],sizeof(gMJKResult));
		sMntData= (MNTDATA)sMntDataSub[SubMj.result_disp_no].clone();			//MEMCPY(&sMntData,&sMntDataSub[result_disp_no],sizeof(sMntData));

#if	Rule_2P
	#if false //-*todo:サウンド
		myCanvas.playSE( (tsumo_agari) ? SN_SE12 : SN_SE13);	//和了時, 敵和了時
	#endif //-*todo:
#else
		// ジングルSE再生	2006/02/24 要望No.113
		PlayMusic_PlaySE( (tsumo_agari)? D_SOUND_AGARI_SE : D_SOUND_FRIKOMI_SE);
#endif
	} else {
		/// @notice 2回通るので削除
		MjEvent((int)GEV.SYS_RYUKYOKU,0);　

		if( Pinch == (byte)SP.AGARI )
			AgariDispMS();		//agari_sts = SEQ_AG_SCORE0;
		else {
			gNextTimeOut = result_wait_base;
			agari_sts = (byte)SEQ.AG_RYUKYOKU;
#if	Rule_2P
	#if false //-*todo:サウンド
			if(( is_yaoch[0]+ is_yaoch[1])!= 0) {
				if( is_yaoch[0]!= 0)
					myCanvas.playSE( SN_SE12);	//和了時
				else
					myCanvas.playSE( SN_SE13);	//敵和了時
			} else
				myCanvas.playSE( SN_SE14);		//流局時
	#endif //-*todo:サウンド
#else
			PlayMusic_PlaySE( D_SOUND_RIACH_NAGARE);	// 流局SE再生 2006/02/23 要望No.58
#endif
		}
	}

	SubMj.mj_wait = 0;
}

/****************************************
	チップ数字
****************************************/
private void TipNumDisp(byte val, byte x, byte y, byte len)
{
	int	i;
	byte[] num_buf= { 0,0,0,0,0,0,0,0 };	//BYTE num_buf[8];

	if(val < 0) {
		num_buf[7] =
		num_buf[6] =
		num_buf[5] =
		num_buf[4] =
		num_buf[3] =
		num_buf[2] =
		num_buf[1] =
		num_buf[0] = 0x0a;
	} else {
		num_buf[7] =
		num_buf[6] =
		num_buf[5] =
		num_buf[4] =
		num_buf[3] =
		num_buf[2] =
		num_buf[1] =
		num_buf[0] = 0;

		if(val != 0){
			val = (byte)(val%100000000);
			num_buf[7] = (byte)((val/10000000)&0xff);
			val = (byte)(val%10000000);
			num_buf[6] = (byte)((val/1000000)&0xff);
			val = (byte)(val%1000000);
			num_buf[5] = (byte)((val/100000)&0xff);
			val = (byte)(val%100000);
			num_buf[4] = (byte)((val/10000)&0xff);
			val = (byte)(val%10000);
			num_buf[3] = (byte)((val/1000)&0xff);
			val = (byte)(val%1000);
			num_buf[2] = (byte)((val/100)&0xff);
			val = (byte)(val%100);
			num_buf[1] = (byte)((val/10)&0xff);
			val = (byte)(val%10);
			num_buf[0] = (byte)(val&0xff);
		}
	}
	for( i = 7; i >0; i--)
		if( num_buf[i] == 0 )
			num_buf[i] = 0x0a;
		else
			break;

	if(len == 0)
		len = (byte)(i + 1);
#if true //-*todo
	//tamaki
	DebLog(("TipNumDisp "+val+", "+x+", "+y+", "+len));
#endif //-*todo
}

/****************************************
	和了画面メイン
****************************************/
public void AgariDispMS()
{
	agari_sts = (byte)SEQ.AG_SCORE0;
}
public bool AgariDispMain(/*MahJongRally * pMe*/)
{
	byte	i;
	bool isGameEnd = false;

	switch(agari_sts) {
		case	(byte)SEQ.AG_DCLR:
			agari_sts = (byte)SEQ.AG_DORA;
			goto case (byte)SEQ.AG_DORA;
		case	(byte)SEQ.AG_DORA:
			agari_sts = (byte)SEQ.AG_UCLR;
			goto case (byte)SEQ.AG_UCLR;
		case	(byte)SEQ.AG_UCLR:
			agari_sts = (byte)SEQ.AG_LIGHT;
			goto case (byte)SEQ.AG_LIGHT;
		case	(byte)SEQ.AG_LIGHT: {
			/************************/
			/*	この状態で役表示	*/
			/************************/
			#if true //-*todo:
			DebLog(("<<yaku printf>>"));
			#endif //-*todo:

			#if false //-*todo:通信
			// 通信用設定
			if( IS_NETWORK_MODE ) {
				//時間設定
				gNextTimeOut = 8000;//result_wait_base;;//D_NEXT_WAIT_TIMEOUT;
				gAutoFlag = 0x00;
			}
			#endif //-*todo:
			#if true //-*todo:描画
			paintF= true;		//役名表示
			#endif //-*todo:
			agari_sts = (byte)SEQ.AG_YAKU;
		}
		goto case (byte)SEQ.AG_YAKU;
		case	(byte)SEQ.AG_YAKU: {
			bool to_next_ = false;	//0;
			#if false //-*todo:通信
			if( IS_NETWORK_MODE ) {
				// ネットワークモード。
				//20051122 add
				NetActTimeOutJdg();
				if( gNextTimeOut <= 0 )
					to_next_ = TRUE;
			} else {
				to_next_= NextButton(  );
			}
			#else
				to_next_= NextButton(  );
			#endif //-*todo:

			if( to_next_ == true ) {
				//ここまでに役を表示する
				{
					SubMj.sho_sts = 0;

					if( Pinch != (byte)SP.AGARI ) {
						// 流局関連の場合はスコア表示に移る
						AgariDispMS();		//agari_sts = SEQ_AG_SCORE0;
					} else {
						if(gMJKResult.byYakuman !=0 && Rultbl[(int)RL.YMSHO] != 0) {
							// 役満賞がついているなら、役満賞へ
							agari_sts = (byte)SEQ.AG_YAKUMAN;
						} else {
							if(	  ( Rultbl[(byte)RL.IPSHO] != 0 && SubMj.tip_buf[0] != 0 )
								||( Rultbl[(byte)RL.RDSHO] != 0 && SubMj.tip_buf[1] != 0 )
								||( Rultbl[(byte)RL.TORIU] != 0 && SubMj.tip_tori_buf[0] != 0 )
								||( Rultbl[(byte)RL.ALICE] != 0 && SubMj.tip_alice_hai[0] != 0 )
							)
								agari_sts = (byte)SEQ.AG_IPPATU;
							else
								agari_sts = (byte)SEQ.AG_NEXT_RESULT;
						}
					}
				}
			}
		}
		break;
		case	(byte)SEQ.AG_YAKUMAN:
		case	(byte)SEQ.AG_IPPATU:
		case	(byte)SEQ.AG_URADORA:
		case	(byte)SEQ.AG_TORIUCHI:
		case	(byte)SEQ.AG_ALICE:
		case	(byte)SEQ.AG_TIPTOTAL:
		case	(byte)SEQ.AG_NEXT_RESULT:
			#if true //-*todo:描画
			paintF= true;		//役名表示後の盤面+４者の得点表示
			#endif //-*todo:描画
			SubMj.result_disp_no++;

			if( SubMj.result_disp_no < Roncnt && Rultbl[ (int)RL.TWO_CHAHO ] != 0) {
				mj_sts = 4;
				return false;
			}

			AgariDispMS();		//agari_sts = SEQ_AG_SCORE0;
			gNextTimeOut = result_wait_base;
			break;

		case (byte)SEQ.AG_RYUKYOKU : {
				byte nextflg_ = 0;
#if false //-*todo:通信
				if( !IS_NETWORK_MODE ) {
	#if	Rule_2P
					if( Pinch== SP_YAOCHU) {	//流し満貫
						gAutoFlag = 0x00;
						nextflg_ = 1;
					}
					NetActTimeOutJdg();
					if( gNextTimeOut <= 0 )
						nextflg_ = 1;
	#else
					if( NextButton() )
						nextflg_ = 1;
	#endif
				} else {
					NetActTimeOutJdg();
					if( gNextTimeOut <= 0 )
						nextflg_ = 1;
				}
#else //-*todo:通信

	#if	Rule_2P
					if( Pinch== (byte)SP.YAOCHU) {	//流し満貫
						gAutoFlag = 0x00;
						nextflg_ = 1;
					}
					NetActTimeOutJdg();
					if( gNextTimeOut <= 0 )
						nextflg_ = 1;
	#endif


#endif //-*todo:通信
				if( nextflg_ == 1 ) {
					// ｽﾀﾝﾄﾞｱﾛｰﾝ時はｴﾝﾀｰｷｰで移行。
					switch( Pinch ) {
						case (byte)SP.HOAN:		/* 流局     */
#if	Rule_2P
	#if false //-*todo:
//****ウキウキ:表情操作****
FaceNum = 0;		//*流局時は真顔に戻す
faceChangeCnt = 0;	//*カウンター初期化
//************************
	#endif //-*todo:
						goto case (byte)SP.YAOCHU;
						case (byte)SP.YAOCHU:		//流し満貫
#endif
							agari_sts = (byte)SEQ.AG_TENPAI;
//							gNextTimeOut = result_wait_base;
							gNextTimeOut = MJDefine.D_NEXT_WAIT_TIMEOUTS;
							break;
						case (byte)SP.FOUR_KAN:
						case (byte)SP.FOUR_FON:
						case (byte)SP.THREE_CHAN:
						case (byte)SP.FOUR_RIC:
						case (byte)SP.NINE_HAI:
							AgariDispMS();		//agari_sts = SEQ_AG_SCORE0;
							break;
					}
				}
			}
			break;
		case	(byte)SEQ.AG_TENPAI:	// 2006/2/9 add
			NetActTimeOutJdg();
#if false //-*todo:通信
			if( IS_NETWORK_MODE ) {
				if( gNextTimeOut <= 0	/*OptionWaitTime[D_TENPAI_TIME] <0*/ )
					AgariDispMS();		//agari_sts = SEQ_AG_SCORE0;
			} else {
#if	Rule_2P
				if( gNextTimeOut <= 0	/*OptionWaitTime[D_TENPAI_TIME] <0*/ )
					AgariDispMS();		//agari_sts = SEQ_AG_SCORE0;
#else
				if(NextButton())
					// ｽﾀﾝﾄﾞｱﾛｰﾝ時はｴﾝﾀｰｷｰで移行。
					AgariDispMS();		//agari_sts = SEQ_AG_SCORE0;
#endif
			}
#else


	#if	Rule_2P
			if( gNextTimeOut <= 0	/*OptionWaitTime[D_TENPAI_TIME] <0*/ ){
				AgariDispMS();		//agari_sts = SEQ_AG_SCORE0;
			}
	#endif	
#endif //-*todo:
			break;

		/*	清算描画	*/
		case	(byte)SEQ.AG_SCORE0:
#if	Rule_2P
//			hai_open= 0x0F;			//清算時 牌をあける
			gNakiTimeOut= 0;
			gNextTimeOut = MJDefine.D_NEXT_WAIT_TIMEOUTS;
			gAutoFlag = 0x00;

			//移動点数
			if(( gsTableData[0].sMemData[0].nMovePoint | gsTableData[0].sMemData[1].nMovePoint)== 0) {
				//点移動ナシはスキップ (結果だけ表示する)
				agari_sts = (byte)SEQ.AG_SCORE2;
				#if false //-*todo:サウンド
				myCanvas.playSE(SN_SE19);		//点数計算
				#endif //-*todo:サウンド
				break;
			}
#endif
			agari_sts = (byte)SEQ.AG_SCORE1;
			goto case (byte)SEQ.AG_SCORE1;
		case	(byte)SEQ.AG_SCORE1:
			/************************/
			/*	この状態で点表示	*/
			/************************/
			#if true //-*todo:
			DebLog(("<<tokutenn printf>>"));
			#endif //-*todo:

			// 通信用設定
			if( (sGameData.byGameMode == MJDefine.GAMEMODE_NET_FREE)|| (sGameData.byGameMode == MJDefine.GAMEMODE_NET_RESERVE) ) {
				//時間設定
				gNextTimeOut = result_wait_base;;	//D_NEXT_WAIT_TIMEOUT;
				gAutoFlag = 0x00;
				//gAutoFlag = MJ_Get_gAutoFlag(  );
			}
#if	Rule_2P
			NetActTimeOutJdg();

			if( gNextTimeOut <= 0 ) {
				gNextTimeOut = MJDefine.D_NEXT_WAIT_TIMEOUTS;
				gAutoFlag = 0x00;
				gNakiTimeOut++;
				if( gNakiTimeOut>= 2) {
					#if false //-*todo:サウンド
					myCanvas.playSE(SN_SE19);		//点数計算
					#endif //-*todo:サウンド
					agari_sts = (byte)SEQ.AG_SCORE2;
					for( i= 0; i< MJDefine.MAX_TABLE_MEMBER2; i++ ) {
						//旧ポイントをキープ
						gsTableData[0].sMemData[i].nOldPoint= (short)(gsTableData[0].sMemData[i].nPoint);
						if (( Pinch== (byte)SP.AGARI ) || ( Pinch== (byte)SP.YAOCHU ))
							gpsTableData.sMemData[i].byRibo_stack = 0;
					}
				}
			}
#else
			agari_sts = SEQ_AG_SCORE2;
#endif
			break;
		case	(byte)SEQ.AG_SCORE2:
#if false //-*todo:通信
			if( IS_NETWORK_MODE ) {
				NetActTimeOutJdg();
				// ネットワークモード。
				if( gNextTimeOut <= 0 )
					agari_sts = SEQ_AG_SCORE3;
			} else {
				// --- ノーマルモード。
				if(NextButton()){
					#if TRIAL // 一局で終了
					amode= MODE_EXIT;
					#if DOCOMO
					AppMain.playSE(SN_SE01);
					AppMain.flagBattleEnd = false; // 強制ロックを解除
					#else
					myCanvas.playSE(SN_SE01);
					myCanvas.flagBattleEnd = false; // 強制ロックを解除
					#endif
					#else
					agari_sts = SEQ_AG_SCORE3;
					#endif
				}
			}
#else
			// --- ノーマルモード。
			if(NextButton()){
			#if TRIAL // 一局で終了
				amode= MODE_EXIT;
				#if DOCOMO
				AppMain.playSE(SN_SE01);
				AppMain.flagBattleEnd = false; // 強制ロックを解除
				#else
				myCanvas.playSE(SN_SE01);
				myCanvas.flagBattleEnd = false; // 強制ロックを解除
				#endif
			#else
				agari_sts = (byte)SEQ.AG_SCORE3;
			#endif
			}

#endif //-*通信
			break;
		case (byte)SEQ.AG_SCORE3:
			agari_sts = (byte)SEQ.AG_END;
			goto case (byte)SEQ.AG_END;
		case (byte)SEQ.AG_END:
			// 和了り終了ありの場合で条件にあった場合、ダイアログを出して
			// 続行を確認
			if( SubMj.agari_end_chk != 0 ) {
				agari_sts = (byte)SEQ.AG_END2;
				menu_csr = 0;
			} else
				agari_sts = (byte)SEQ.AG_END3;
			break;
		case (byte)SEQ.AG_END2:
			// 和了り終了確認
			#if false //-*todo:キー操作
			if( D_ONEPUSH_INPUT_UP ) {
				menu_csr = (UINT2)(menu_csr^1);	//!menu_csr;
			} else if( D_ONEPUSH_INPUT_DOWN ) {
				menu_csr = (UINT2)(menu_csr^1);	//!menu_csr;
			} else if( D_ONEPUSH_INPUT_SELECT ) {
				if( menu_csr == 0 )
					Totsunyu_flg = 0xFF;
				agari_sts = SEQ_AG_END3;
				agari_end_chk = 0;
			}
			#endif //-*todo:キー操作
			break;

		case	(byte)SEQ.AG_END3:
			agari_sts = (byte)SEQ.AG_END4;
			goto case (byte)SEQ.AG_END4;
		case	(byte)SEQ.AG_END4:
			if( Totsunyu_flg != 0) {
				/* 次局に突入なら局番号を進める */
#if	Rule_2P
				if(( gpsTableData.byKyoku % MJDefine.MAX_TABLE_MEMBER)== 0)
					gpsTableData.byKyoku++;
				else
					gpsTableData.byKyoku+= (MJDefine.MAX_TABLE_MEMBER- 1);
#else
				gpsTableData.byKyoku++;
#endif
				#if true //-*todo:
				// gpsTableData.byFlags	&=	~TBLF.NEXTBA_VOICE;
				gpsTableData.byFlags	=	(byte)(gpsTableData.byFlags & ~(byte)TBLF.NEXTBA_VOICE);
				#endif //-*todo:
			}

			if( SubMj.set_for_nextplay_fRenchan )
				// set_for_nextplay_fRenchanが立っていたら局を終わらせる 99 が入っていたので、局
				gpsTableData.byKyoku = Kyoend;

			//if(sGameData.byGameMode == GAMEMODE_WIRELESS && game_player != 0)
			//{
			//	// ここには決して入ってこない
			//	return(FALSE);
			//}

			// 20060206 No.406 BEGIN
			//	 半荘終了判定結果をサーバーに送信
			isGameEnd = chkend_game();

#if false //-*todo:通信
			// 通信対戦の場合
			if( IS_NETWORK_MODE )
				// 対局結果報告
				KyokuEndResultReportSnd( isGameEnd);
			// 20060206 No.406 END
#endif //-*todo:通信
			if( isGameEnd == true ) {
				// 終了なら局番号を戻す（見た目に変化させない） 2006/02/07 No.308
				// 第一局で局終了になると0-1=255になり、画像参照先がおかしくなるので修正
				if( gpsTableData.byKyoku > 0 ) {
					//> 2006/04/16 No.
					// ドボン終了時は元に戻さない
					if( gpsTableData.byKyoku == Kyoend )		//0422
						gpsTableData.byKyoku--;
				} else{
					gpsTableData.byKyoku = 0;
				}

#if false //-todo:保留				
				if( sGameData.byGameMode != MJDefine.GAMEMODE_SURVIVAL ){		//0422
					// 設定ファイルの保存
					MJ_WriteFile( D_WORD_INFOFILE, f_info);ssave02(true);//0514mt			//0422
				}
#endif //-*todo:保留
				
#if false //-todo:通信
				// 局終了時
				if( IS_NETWORK_MODE ) {
					// 対戦結果待ちへ
					is_game_result_wait = true;
					agari_sts = SEQ_AG_END6;

					return false;
				} else {
					// 半荘終了時
					gpsTableData.byFlags	|=	TBLF_FINISH;
//					if (!CompVSCompMode) {}
					MjHanchanEnd();

					agari_sts = SEQ_END_FADE;
					mj_sts = 6;
					return true;
				}
#else
					// 半荘終了時
					gpsTableData.byFlags	|=	(byte)TBLF.FINISH;
					MjHanchanEnd();
					agari_sts = (byte)SEQ.END_FADE;
					mj_sts = 6;
					return true;
#endif //-*todo:通信
			} else {
				rank_sort( (ushort) 0);					/*	現在のトップ	*/
				gpsTableData.byOLDTop	=	Get_Rank( 0);

				if (!SubMj.CompVSCompMode) {
					if( sGameData.byGameMode == MJDefine.GAMEMODE_SURVIVAL ) {
						#if false //-*todo:セーブ保留
						// 局終了ごとのセーブ
						f_info.f_survival_kyoku_data.byKyokuSave = 1;
						MJ_SurvivalGameKyokuSave();
						MJ_SurvivalGameSaveMain();
						fileOpen();	ssave01(false);ssave02(false);ssave03(false);	fileClose();	//0514mt
						#endif //-*todo:セーブ保留
					} else{
						#if false //-*todo:セーブ保留
						ssave02(true);
						#endif //-*todo:セーブ保留
					}
					// 設定ファイルの保存
//#ifndef	SAVE_ALL
//					ssave03(true);		//MJ_WriteFile( D_WORD_INFOFILE, f_info);		//0422
//#endif
					init_sutehai_rec();						/* 捨て牌表示管理バッファの初期化 */
					chipai_m2();							/* 局のイニット */
#if _DEBUG
					TRACE("byChicha = %d: byOya = %d: byKyoku = %d \n",
						gpsTableData.byChicha,gpsTableData.byOya,gpsTableData.byKyoku);
#endif
					// ゲーム再初期化へ移行
					modeChange( INMJMODE.D_FREE_GAME_REINIT_MODE );
				}
				mj_sts = 2;
			}
			break;
		case (byte)SEQ.AG_END6 :
			if( is_game_result_wait == false ) {
				// 対戦結果を受信したら終了
				gpsTableData.byFlags	|=	(byte)TBLF.FINISH;
//				if (!CompVSCompMode) {}
				MjHanchanEnd();
				agari_sts = (byte)SEQ.END_FADE;
				mj_sts = 6;
				return true;
			}
			break;
	}
	return(false);
}

/****************************************
	順位を決める
****************************************/
public void RankSet(/*MahJongRally * pMe*/)
{
	byte	cnt;

	for(int i=0;i<MJDefine.MAX_TABLE_MEMBER;i++){
		cnt = 0;
		for(int j=0;j<MJDefine.MAX_TABLE_MEMBER;j++)
			if(gpsTableData.sMemData[i].nPnt < gpsTableData.sMemData[j].nPnt)
				cnt++;

		gpsTableData.sMemData[i].byTotalRank = cnt;
	}
}

/****************************************
	麻雀終了画面メイン
****************************************/
public bool MjEndMain(/*MahJongRally * pMe*/)
{
//	amode= MODE_EXIT;
	int	i=0;

	if(SubMj.CompVSCompMode)
		return(true);

	switch(agari_sts) {
		case	(byte)SEQ.END_FADE:
				agari_sts = (byte)SEQ.END_POINT1;
			goto case (byte)SEQ.END_POINT1;
		case	(byte)SEQ.END_POINT1:
				// 通信用設定
				if( (sGameData.byGameMode == MJDefine.GAMEMODE_NET_FREE)||
					(sGameData.byGameMode == MJDefine.GAMEMODE_NET_RESERVE) ) {
					//時間設定
					gNextTimeOut = result_wait_base;;//D_NEXT_WAIT_TIMEOUT;
					gAutoFlag = 0x00;
				}
				agari_sts = (byte)SEQ.END_POINT2;
			goto case (byte)SEQ.END_POINT2;
		case	(byte)SEQ.END_POINT2:
				//20051124 add
				NetActTimeOutJdg();

#if	Rule_2P
#else
				if(NextButton())
#endif
					agari_sts = (byte)SEQ.END_POINT3;
			goto case (byte)SEQ.END_POINT3;
		case	(byte)SEQ.END_POINT3:
				if(hanchan_result_uma()){
					agari_sts = (byte)SEQ.END_UMA1;
				}else{
					agari_sts = (byte)SEQ.END_UMA4;
				}
				break;

		/************************************************/
		/*												*/
		/*					順位馬 清算					*/
		/*												*/
		/************************************************/
		case	(byte)SEQ.END_UMA1:
				// 点数の代入。
				for( i=0 ; i<4 ; i++ ) {
					// 現在のポイント。
					m_NowResultDisp[i] = gpsTableData.sMemData[i].nPnt - gpsTableData.sMemData[i].nMovePnt;

					// NextまでNowを増やしていく。
					m_NextResultDisp[i]= gpsTableData.sMemData[i].nPnt;
				}

				// 通信用設定
				if( (sGameData.byGameMode == MJDefine.GAMEMODE_NET_FREE)||
					(sGameData.byGameMode == MJDefine.GAMEMODE_NET_RESERVE) ) {
					//時間設定
					gNextTimeOut = result_wait_base;;//D_NEXT_WAIT_TIMEOUT;
					gAutoFlag = 0x00;
				}
				agari_sts = (byte)SEQ.END_UMA2;
			goto case (byte)SEQ.END_UMA2;
		case	(byte)SEQ.END_UMA2:
				//20051124 add
				NetActTimeOutJdg();

				if(NextButton()) {
					// 通信用設定
					if( (sGameData.byGameMode == MJDefine.GAMEMODE_NET_FREE)||
						(sGameData.byGameMode == MJDefine.GAMEMODE_NET_RESERVE) ) {
						//時間設定
						gNextTimeOut = result_wait_base;;//D_NEXT_WAIT_TIMEOUT;
						gAutoFlag = 0x00;
					}

					agari_sts = (byte)SEQ.END_UMA3;
					m_ResultEndFlag = true;	//ON;
				}
				break;

		case	(byte)SEQ.END_UMA3:
				if( m_ResultEndFlag == false ) {	//OFF
					//20051124 add
					NetActTimeOutJdg();

					if(NextButton())
						agari_sts = (byte)SEQ.END_UMA4;
				}
				break;

		case	(byte)SEQ.END_UMA4:
				if(hanchan_result_dobon()){
					agari_sts = (byte)SEQ.END_DOBON1;
				}else{
					agari_sts = (byte)SEQ.END_DOBON4;
				}
				break;

		/************************************************/
		/*												*/
		/*					ドボン 清算					*/
		/*												*/
		/************************************************/
		case	(byte)SEQ.END_DOBON1:
				for( i=0 ; i<4 ; i++ ) {
					// 2006/2/5 add
					// 点数の代入。
					m_NowResultDisp[i] = gpsTableData.sMemData[i].nPnt - gpsTableData.sMemData[i].nMovePnt;	// 現在のポイント。
					m_NextResultDisp[i]= gpsTableData.sMemData[i].nPnt;										// NextまでNowを増やしていく。
				}
				#if false //-*todo:通信	
				// 通信用設定
				if( IS_NETWORK_MODE ) {
					//時間設定
					gNextTimeOut = result_wait_base;;//D_NEXT_WAIT_TIMEOUT;
					gAutoFlag = 0x00;
				}
				#endif //-*todo:通信
				agari_sts = (byte)SEQ.END_DOBON2;
			goto case	(byte)SEQ.END_DOBON2;
		case	(byte)SEQ.END_DOBON2:
				//20051124 add
				NetActTimeOutJdg();

				if(NextButton()) {
					// 通信用設定
				#if false //-*todo:通信	
					if( IS_NETWORK_MODE ) {
						//時間設定
						gNextTimeOut = result_wait_base;;//D_NEXT_WAIT_TIMEOUT;
						gAutoFlag = 0x00;
					}
				#endif //-*todo:通信
					agari_sts		= (byte)SEQ.END_DOBON3;
					m_ResultEndFlag = true;	//ON;
				}
				break;

		case	(byte)SEQ.END_DOBON3:
				if( m_ResultEndFlag == false ) {	//OFF
					//20051124 add
					NetActTimeOutJdg();

					if(NextButton())
						agari_sts = (byte)SEQ.END_DOBON4;
				}
				break;

		case	(byte)SEQ.END_DOBON4:
				if(hanchan_result_yakitori()){
					agari_sts = (byte)SEQ.END_YAKITORI1;
				}else{
					agari_sts = (byte)SEQ.END_YAKITORI4;
				}
				break;

		/************************************************/
		/*												*/
		/*				  ヤキトリ 清算					*/
		/*												*/
		/************************************************/
		case	(byte)SEQ.END_YAKITORI1:
				for( i=0 ; i<4 ; i++ ) {
					// 2006/2/5 add
					// 点数の代入。
					m_NowResultDisp[i] = gpsTableData.sMemData[i].nPnt - gpsTableData.sMemData[i].nMovePnt;	// 現在のポイント。
					m_NextResultDisp[i]= gpsTableData.sMemData[i].nPnt;										// NextまでNowを増やしていく。
				}
				#if false //-*todo:通信
				// 通信用設定
				if( IS_NETWORK_MODE ) {
					//時間設定
					gNextTimeOut = result_wait_base;;//D_NEXT_WAIT_TIMEOUT;
					gAutoFlag = 0x00;
				}
				#endif //-*todo:通信

				agari_sts = (byte)SEQ.END_YAKITORI2;
				goto case	(byte)SEQ.END_YAKITORI2;
		case	(byte)SEQ.END_YAKITORI2:

				//20051124 add
				NetActTimeOutJdg();

				if(NextButton()) {
				#if false //-*todo:通信
					// 通信用設定
					if( IS_NETWORK_MODE ) {
						//時間設定
						gNextTimeOut = result_wait_base;;//D_NEXT_WAIT_TIMEOUT;
						gAutoFlag = 0x00;
					}
				#endif //-*todo:通信
					agari_sts		= (byte)SEQ.END_YAKITORI3;
					m_ResultEndFlag = true;	//ON;
				}
				break;

		case	(byte)SEQ.END_YAKITORI3:
				if( m_ResultEndFlag == false ) {	//OFF
					//20051124 add
					NetActTimeOutJdg();

					if(NextButton())
						agari_sts = (byte)SEQ.END_YAKITORI4;
				}
				break;
		case	(byte)SEQ.END_YAKITORI4:
				//_dispChipData();
				if(hanchan_result_tip()){
					agari_sts = (byte)SEQ.END_TIP1;
				}else{
					agari_sts = (byte)SEQ.END_TIP4;
				}
				break;

		/************************************************/
		/*												*/
		/*				  チップ 清算					*/
		/*												*/
		/************************************************/
		case	(byte)SEQ.END_TIP1:
				for( i=0 ; i<4 ; i++ ) {
					// 2006/2/5 add
					// 点数の代入。
					m_NowResultDisp[i] = gpsTableData.sMemData[i].nPnt - gpsTableData.sMemData[i].nMovePnt;	// 現在のポイント。
					m_NextResultDisp[i]= gpsTableData.sMemData[i].nPnt;										// NextまでNowを増やしていく。
				}
				#if false//-*todo:通信
				// 通信用設定
				if( IS_NETWORK_MODE )
				//if( (sGameData.byGameMode == GAMEMODE_NET_FREE)||
				//	(sGameData.byGameMode == GAMEMODE_NET_RESERVE) )
				{
					//時間設定
					gNextTimeOut = result_wait_base;;//D_NEXT_WAIT_TIMEOUT;
					gAutoFlag = 0x00;
				}
				#endif //-*todo:通信
				agari_sts = (byte)SEQ.END_TIP2;
			goto case	(byte)SEQ.END_TIP2;
		case	(byte)SEQ.END_TIP2:

				//20051124 add
				NetActTimeOutJdg();

				if(NextButton()) {
					#if false //-*todo:通信
					// 通信用設定
					if( IS_NETWORK_MODE )
					//if( (sGameData.byGameMode == GAMEMODE_NET_FREE)||
					//	(sGameData.byGameMode == GAMEMODE_NET_RESERVE) )
					{
						//時間設定
						gNextTimeOut = result_wait_base;;//D_NEXT_WAIT_TIMEOUT;
						gAutoFlag = 0x00;
					}
					#endif //-*todo:通信
					agari_sts		= (byte)SEQ.END_TIP3;
					m_ResultEndFlag = true;	//ON;
				}
				break;

		case	(byte)SEQ.END_TIP3:
				if( m_ResultEndFlag == false ) {	//OFF
					//20051124 add
					NetActTimeOutJdg();

					if(NextButton()){
						agari_sts = (byte)SEQ.END_TIP4;
					}
				}
				break;

		case	(byte)SEQ.END_TIP4:
				agari_sts = (byte)SEQ.AG_RANKING1;
				break;

		case	(byte)SEQ.AG_RANKING1:
				RankSet();

				/****************************/
				/*	この状態で詳細点数表示	*/
				/****************************/
				#if true //-*todo:
				DebLog(("<<result ten printf>>"));
				#endif //-*todo:

				if(sGameData.byGameMode == MJDefine.GAMEMODE_SURVIVAL) {
#if false //-*todo:保留要らない気がする
					sSurvivalData.GameCnt++;
					switch(gpsTableData.sMemData[0].byTotalRank) {
						case	0:
							sSurvivalData.Rank1++;
							if(sSurvivalData.Best < survival_stage_no+1)	sSurvivalData.Best = (BYTE)(survival_stage_no+1);
							break;
						case	1:
							sSurvivalData.Rank2++;
							if(sSurvivalData.Best < survival_stage_no+1)	sSurvivalData.Best = (BYTE)(survival_stage_no+1);
							break;
						case	2:
							sSurvivalData.Rank3++;
							if(sSurvivalData.Best < survival_stage_no+1)	sSurvivalData.Best = (BYTE)(survival_stage_no+1);
							break;
						default:
							sSurvivalData.Rank4++;
							break;
					}

					//モード移行挿入
					//モードをサバイバル対戦結果へ移行する
					modeChange( D_SURIVIVAL_RESULT_MODE);
					//< 2006/03/11 差し馬削除
#endif //-*todo:保留要らない気がする
					break;
				} else {
					#if false //-*todo:通信
					// 通信用設定
					if( IS_NETWORK_MODE ) {
						//時間設定
						gNextTimeOut = result_wait_base;;//D_NEXT_WAIT_TIMEOUT;
						gAutoFlag = 0x00;
					}
					#endif //-*todo:通信
#if	Rule_2P
					gAutoFlag = 0x00;
#endif
					agari_sts = (byte)SEQ.AG_RANKING2;
				}
				break;

		case	(byte)SEQ.AG_RANKING2:
#if	Rule_2P
			if(NextButton()) {
				a_Mode= MAHJONGMODE.mMODE_EXIT;	//-*todo:旧(amod)
				#if DOCOMO // 080716
				AppMain.flagBattleEnd = false; // 強制ロックを解除 // 110513:After麻雀から移植
				#endif
//				MJ_GameDrawFree();		//02/05 ココで開放すると、画像がなくなる
			}
#else
			//20051124 add
			NetActTimeOutJdg();

			if(NextButton())
				modeChange( D_TITLE_MODE);
#endif
			break;

		case	(byte)SEQ.END_MENU:
			if(game_player != 0){
				break;
			}
#if false //-*todo:保留
	#if GAME_SWITCH
				ComEndMenuMain();
			break;
	#endif
#else //-*todo:保留
	break;
#endif //-*todo:保留
	}

	return(false);
}

/****************************************
	麻雀メイン
****************************************/
public void MjMain(/*MahJongRally * pMe*/)
{
#if GAME_SWITCH

	{
		//20051116 del
		if(SubMj.mj_wait > 0){
			SubMj.mj_wait--;	//mj_wait -= uptimems_perfrm;
			return;
		}
#if FAST_TEST
	do{
#endif
//#if	__LOGIC_CHECK
// DebLog("mj_sts "+mj_sts);		//aaaa
//#endif
		switch(mj_sts) {
			//0 と 7 は使われてない
			case	0:						//各ゲームモード開始時の初期化
				MjModeInit();
				mj_sts = 1;
				agari_sts = 0;
				goto case 1;
			case	1:						//半荘開始時の初期化
				MjHanchanInit();
				goto case 2;
			case	2:						//局開始時の初期化
				Optcnt = 0;					// ゲーム開始時にメニューを表示させないようにクリア。
				MjKyokuInit();

				if(MjKyokuInit_bflag == 0){
					mj_sts = 3;
				}
				break;
			case	3:						//局処理
//-*SUGI_DEB***************************
#if SUGI_DEB //-*todo:注デバッグ中
				if(m_DebBox != null && m_DebBox.GetDebugFlag(DebBoxInGame.FUNCTION_LIST.INITGAME,true)){
				//-*新規局へ
					mj_sts = 2;
					break;
				}
#endif //-*todo:注デバッグ中
//-****************************SUGI_DEB
				MjKyokuMain();
				if(reentry_m1_bflag== 0xFF) {
					mj_sts = 8;
					// オプションを0に。
					Optcnt = 0;	// ゲーム終了後にメニューを表示させないようにクリア。
					break;
				}

				// 2006/1/20
				// MJ_GameDraw.cに置いてあるロジックをこちらに移動。
				// 再接続時は行わない
				if( is_reentry == false ) {
					gGamePointFlag = false;
					#if false //-*todo:保留キー操作
					if( (GetKeyLev2(_KEY_SOFT2)) == 0){
						gGamePointFlag = true;
					}
					#endif //-*todo:保留キー操作

					#if false //-*todo:不要？
					// チャット
					if( opt_game[GOPT_TLK] == GOPT_TLK_ON ){
						//> 2006/03/31 チャットメニューが出続ける不具合
						if( IS_NETWORK_MODE && gChatSendFlag && gDaiuchiFlag ==  D_ONLINE_OPERATOR_MANUAL ){	//0422
						//< 2006/03/31 チャットメニューが出続ける不具合
							MJ_GameChatMain(  );
						}
					}
					#endif //-*todo:不要？

					#if false //-*todo:保留
					// モードの切り替え。
					MJ_GameModeChangeMain(  );
					#endif //-*todo:不要？

					#if false //-*todo:保留
					if( m_Mode == D_EFS_ERR_MODE ) {
						modeChange( D_EFS_ERR_MODE );
						return;
					}
					#endif //-*todo:不要？
				}
				break;

			case	8:
				SubMj.result_disp_no = 0;
				mj_sts = 4;
				goto case 4;
			case	4:						//局終了
#if DEBUG
Debug.Log("* Kyoku End *");
#endif
				AgariDispInit();
				mj_sts = 5;
				goto case 5;
			case	5:						//和了画面メイン
				if( AgariDispMain()) {
				#if false //-*todo:サウンド
					myCanvas.stopBGM();
					if( myCanvas.flag_res_battle== 0)	// 0:勝利/1:敗北
						myCanvas.playSE( SN_SE15);
					else
						myCanvas.playSE( SN_SE16);
				#endif //-*todo:サウンド
					modeChange( INMJMODE.D_GAME_RESULT_MODE );
				}
				break;

			case	6:						//半荘終了
				// モードチェンジ。
				// ここでリザルト描画モードへジャンプ。
				// MjEndMain();
				modeChange( INMJMODE.D_GAME_RESULT_MODE );
				break;

			case	7:						//半荘終了
				#if true //-*todo:
				DebLog(("### Hanchan End ###"));
				#endif //-*todo:
				break;
//-*SUGI_DEB***************************
#if SUGI_DEB //-*todo:仮作成中
			//-*運処理
			case 99://-*mj_sts
				if(m_DebBox != null && m_DebBox.GetDebugFlag(DebBoxInGame.FUNCTION_LIST.INITGAME,true)){
				//-*新規局へ
					mj_sts = 2;
					break;
				}
				//-*運処理
				// if((Bpcnt+Kancnt) == MJDefine.PAI_MAX){
				if(game_player == Order){
					deb_myLuckP = 3;
					deb_yourLuckP = 1;
					int deb_maxLuckP = (Math.Max(deb_myLuckP,deb_yourLuckP)*2);
					int deb_startBpcnt = 79;//-*開始時の数値
					gsPlayerWork[game_player].byPlflg = 1;	//-*手動プレイ
					if((Bpcnt+Kancnt) <= (deb_startBpcnt+ deb_maxLuckP)){
						Debug.Log("//-*"+game_player+","+Order+"ハイテイ！！！！！"+Math.Max(deb_myLuckP,deb_yourLuckP)+"！！！！！("+Bpcnt+"+"+Kancnt+") == "+MJDefine.PAI_MAX);
						gsPlayerWork[game_player].byPlflg = 0;	//-*オートプレイ
					}
					else{
						// mj_sts = 2;
						// break;
						m_DebBox.ButtonSelectStageTest((int)DebBoxInGame.FUNCTION_LIST.TUMOPOSTEST,true);
						m_DebBox.ButtonRestart(0,true);

					}
				}
				MjKyokuMain();
				if(!deb_isLuckyTime)
				{
					mj_sts = 2;
				}
				if(reentry_m1_bflag== 0xFF) {
					mj_sts = 8;
					// オプションを0に。
					Optcnt = 0;	// ゲーム終了後にメニューを表示させないようにクリア。
					break;
				}

				// 2006/1/20
				// MJ_GameDraw.cに置いてあるロジックをこちらに移動。
				// 再接続時は行わない
				if( is_reentry == false ) {
					gGamePointFlag = false;
				}
				break;
#endif //-*todo:注デバッグ中
//-****************************SUGI_DEB
		}
#if FAST_TEST
	} while( mj_sts== 3);
#endif
	}
#else
//	sGameData.byGameMode = GAMEMODE_WIRELESS;			/*	通信対戦		*/
//	MjWireless();
#endif
}

/****************************************************************/
/*		COMセリフ												*/
/****************************************************************/
public void MjDialog( int byPos, int byTarget, int tno ) {		MjDialog((byte)byPos, (byte)byTarget, (byte)tno);	}
public void MjDialog( char byPos, int byTarget, int tno ) {		MjDialog((byte)byPos, (byte)byTarget, (byte)tno);	}
public void MjDialog( char byPos, int byTarget, char tno ) {	MjDialog((byte)byPos, (byte)byTarget, (byte)tno);	}
public void MjDialog( /*MahJongRally * pMe,*/ byte byPos, byte byTarget, byte tno )
{
#if false //-*todo:
	ITRACE(( "MjDialog:order=%d,tgt=%d,no=%d",byPos,byTarget,tno ));
#endif //-*todo:
#if	FAST_TEST
	if(Optcnt == 0)		// メニューコマンドが表示されていないとき
		GameTalk( (byte)tno, (byte)byPos, (byte)byTarget);
#endif
}

/****************************************************************/
/*		イベント？												*/
/****************************************************************/
public void MjEvent( int type, int par ) {	MjEvent((byte)type, (byte)par);	}
public void MjEvent( /*MahJongRally * pMe,*/ 	byte type, byte par )
{
	#if true //-*todo:描画
	paintF= true;
	paintT= 0;		//イベント？
	#endif //-*todo:描画
	GameEvent(type,par);
//	game_event_flg = 1;
}

/****************************************
	プレイヤーの入力（ツモった後）
****************************************/
#if	Rule_2P
public int	menu_type;
#endif
public int	menu_mode_kep= -1;
public int	hai_up_kep= -1;
public short CommandCatch(/*MahJongRally * pMe,*/ byte iOrder)
{
#if	Rule_2P
	menu_type= 0;
#endif
	short	ret = TehaiSel();		//0422

	//プレイヤーの入力(ツモった後)
	#if true //-todo:
	//-*元
	if( menu_mode_kep!= menu_mode) {	menu_mode_kep= menu_mode;	paintF= true;		}
	if( hai_up_kep!= hai_up) {			hai_up_kep= hai_up;			paintF= true;		}
	#else
	if( menu_mode_kep!= menu_mode) {	menu_mode_kep= menu_mode;	}
	if( hai_up_kep!= hai_up) {			hai_up_kep= hai_up;			}
	#endif //-*todo:

	return(ret);			//0422
}
/****************************************
	プレイヤーの入力（鳴いた後）
****************************************/
public short NakiCommandCatch(/*MahJongRally * pMe,*/ byte iOrder)
{
#if	Rule_2P
	menu_type= 1;
#endif
	short	ret = (NakiSel());		//0422

	//プレイヤーの入力(鳴いた後)
	#if true //-todo:
	//-*元
	if( menu_mode_kep!= menu_mode) {	menu_mode_kep= menu_mode;	paintF= true;		}
	if( hai_up_kep!= hai_up) {			hai_up_kep= hai_up;			paintF= true;		}
	#else
	if( menu_mode_kep!= menu_mode) {	menu_mode_kep= menu_mode;		}
	if( hai_up_kep!= hai_up) {			hai_up_kep= hai_up;				}
	#endif //-*todo:

	return(ret);			//0422
}
//-*********************mj.j
}