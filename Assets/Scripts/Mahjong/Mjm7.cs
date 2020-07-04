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
// Mjm7.j
//-*****************
public partial class MahjongBase : SceneBase {
/*
**		麻雀大会３
**		局終了時の処理
*/

//#include "MahJongRally.h"								// Module interface definitions

//		/*****************************
//			アガリの表示
//		*****************************/
//		_STATIC	VOID	_playYakumanSE(VOID)
//		{
//			BYTE	byBGM;
//
//			byBGM	=	GetNowBGM();
//
//			while (!StopBGM(32)) {											/*	BGMをHOMEへ	*/
//				g_WaitCounter(1);
//			}
//			PlaySE(SENO_YAKUMAN);								/*	役満SE		*/
//			g_DrawAll();
//			while (GetSEStatus()) {
//				if (InpPad() != 0) {
//					break;
//				}
//			}
//			if (byBGM != 0xFF) {
//				PlayBGM(byBGM);
//			}
//		}
//
public void dhora_m7 ( /*MahJongRally * pMe*/ )/** Agari Display **/
{
	byte		i;
	/* トリロンダブロン処理 1996.6.10._3CHAHO */
	byte	sto;
	byte	d_opn_flg;/* 裏ドラのオープンフラグ */

	if (SubMj.CompVSCompMode)
		return;

	d_opn_flg = 0;/* 裏ドラのオープンフラグをクリア */
	if ( gMJKResult.sYaku[0].name != (byte)YK.YAOCH ) {		/* 上り役が流し満貫じゃなかたら */
		if ( Roncnt > 1 ) {								/* ロンの面子が複数いる時 */
			disp_sutehai_flush( true ,true);	//( ON ,TRUE);

			sto = Order;
			for ( i = 0; i < 4; i++ )
				/* 上がりの面子の手牌をオープン */
				if ( Hanetbl[i] != 0 ) {
					Order	=	i;
					setwp_m2( Order);
					if ( gpsPlayerWork.bFrich != 0 )
						d_opn_flg |= 0xff;

					jyaku_jy ();	/* YAKUMAN_BUG 1996.9.19 */
//m					if( ( gMJKResult.byYakuman != 0 ) ||
//m						( gMJKResult.byHan > 12 )     ) {}
				}

			/* 手牌ポインタを戻しておく */
			Order = sto;
			setwp_m2( Order);
			if ( d_opn_flg != 0 )
				disp_dora((ushort) 2 );							/* 裏ドラの表示 */
		} else {	/* 単独のロンの時 */
			if (( Status & (byte)ST.RON ) != 0 )
				disp_sutehai_flush( true ,true);	//( ON ,TRUE);

			//if( ( gMJKResult.byYakuman != 0 ) ||
			//	( gMJKResult.byHan > 12 )     ) {
			//		//://_playYakumanSE();						/*	役満SE		*/
			//}
			if ( gpsPlayerWork.bFrich != 0 )
				disp_dora((ushort) 2 );							/* 裏ドラの表示 */
		}
	}
}

//		#define		DISP_SP_X	( 128 )
//		#define		DISP_SP_Y	( 108 )
/*****************************
	特別流局表示
*****************************/
public void _dpinchComp(/*MahJongRally * pMe*/ )
{
	if( Pinch == (byte)SP.HOAN && Rultbl[ (int)RL.NOTEN ] != 0)		/* 流局 */
		cal_noten_bappu ();									/* 罰符計算 */
}

public void _dpinchUser(/*MahJongRally * pMe*/ )
{
	short		i;

	/* ９種９牌オープン */
	if( Pinch == (byte)SP.NINE_HAI ) {
		MjEvent( (byte)GEV.SYS_9SYU9HAI,Order);
		//://PlaySE(SENO_TEHAIOPEN);
		//://disp_tehai( PAI_FRONT, Order, 1 );
		/* 九種九牌で倒牌した人が( 65 ) */
		MjDialog(  Order, MJDefine.NONE, (byte)DIAL.D_9SYU9HAI );
	} else if( Pinch == (byte)SP.FOUR_FON ) {
		/* ４風連打 */
		MjEvent( (int)GEV.SYS_4FURENDA,0);
		/* 四風連打の最後の人(69) */
		MjDialog(  Order, MJDefine.NONE, (byte)DIAL.D_4FURENDA );

	} else if( Pinch == (byte)SP.FOUR_KAN ) {
		/* 四開槓 */
		MjEvent( (int)GEV.SYS_4KAIKAN,0);
		// 最後の人
		// MjDialog(  Order, NONE, DIAL_4KAIKAN );
	}

	//> 2006/03/26 流し満貫対策		//0422
	for( i = 0; i < MJDefine.MAX_TABLE_MEMBER2; ++i ) {	//MAX_TABLE_MEMBER
		is_yaoch[ i ]    = 0x00;
		yaoch_point[ i ] = 0;
	}
	//< 2006/03/26 流し満貫対策

	if( Pinch == (byte)SP.HOAN ) {
		//> 2006/03/26 流し満貫対策		//0422
		if( gMJKResult.sYaku[0].name == (byte)YK.YAOCH ) {
			/*---------------------------
			- 流し満貫成立者チェック・加算得点計算
			----------------------------*/
#if	Rule_2P
			int	chkHane= 0;
#endif
			for( i = 0; i < MJDefine.MAX_TABLE_MEMBER2; ++i ) {		//MAX_TABLE_MEMBER
				setwp_m2( i );
				if( jyaoch_jy() ) {
					gsPlayerWork[i].byTenpai = 0;
					is_yaoch[ i ]    = 0x01;
#if	Rule_2P
					Pinch= (byte)SP.YAOCHU;
					if( i == gpsTableData.byOya ) {
						chkHane|= 0x01;
						yaoch_point[ i ] += 120;
						yaoch_point[ (i + 1) & 1 ] -= 120;
					} else {
						chkHane|= 0x02;
						yaoch_point[ i ] += 80;
						yaoch_point[ (i + 1) & 1 ] -= ( ((i + 1) & 1) == gpsTableData.byOya ) ? (short)80 : (short)80;
					}
#else
					if( i == gpsTableData.byOya ) {
						yaoch_point[ i ] += 120;
						yaoch_point[ (i + 1) & 3 ] -= 40;
						yaoch_point[ (i + 2) & 3 ] -= 40;
						yaoch_point[ (i + 3) & 3 ] -= 40;

						DebLog(("YAOCH1 %3d %3d %3d %3d",
							yaoch_point[ 0 ],yaoch_point[ 1 ],yaoch_point[ 2 ],yaoch_point[ 3 ]));
					} else {
						yaoch_point[ i ] += 80;
						yaoch_point[ (i + 1) & 3 ] -= ( ((i + 1) & 3) == gpsTableData.byOya ) ? 40 : 20;
						yaoch_point[ (i + 2) & 3 ] -= ( ((i + 2) & 3) == gpsTableData.byOya ) ? 40 : 20;
						yaoch_point[ (i + 3) & 3 ] -= ( ((i + 3) & 3) == gpsTableData.byOya ) ? 40 : 20;
						DebLog(("YAOCH2 %3d %3d %3d %3d",
							yaoch_point[ 0 ],yaoch_point[ 1 ],yaoch_point[ 2 ],yaoch_point[ 3 ]));
					}
#endif
				}
			}
#if	Rule_2P
			//流し満貫時に場のリー棒を加算する
			if( chkHane!= 0) {
				for( i = 0; i < MJDefine.MAX_TABLE_MEMBER2; ++i ) {
					setwp_m2( i );
					if( jyaoch_jy() ) {
						if( i == gpsTableData.byOya ) {	//親
							//親 流し満貫
							yaoch_point[ i ]+= (short)(gpsTableData.byRibo* 10+ gpsTableData.byRenchan* 3);
							yaoch_point[ i^ 1 ]-= (short)(gpsTableData.byRenchan* 3);		//子から連荘棒を引く
						} else							//子
							if(( chkHane & 0x01)== 0) {	//親子 流し満貫?
								//子 流し満貫
								yaoch_point[ i ]+= (short)(gpsTableData.byRibo* 10+ gpsTableData.byRenchan* 3);
								yaoch_point[ i^ 1 ]-= (short)(gpsTableData.byRenchan* 3);	//親から連荘棒を引く
							}
					}
				}
				gpsTableData.byRibo= 0;		//場のリーボーの数
			}
#endif
			#if false //-*todo:
			DebLog(("YAOCH3 %3d %3d %3d %3d",
				yaoch_point[ 0 ],yaoch_point[ 1 ],yaoch_point[ 2 ],yaoch_point[ 3 ]));
			#endif //-*todo:

			//---------------------------
			// 流し満貫得点セット
			//---------------------------
			// 旧ポイントをキープ
//m			for ( i = 0; i < MAX_TABLE_MEMBER2; i++ )	//MAX_TABLE_MEMBER
//m				gpsTableData.sMemData[i].nOldPoint	= gpsTableData.sMemData[i].nPoint;

			// 得点をセット
			for ( i = 0; i < MJDefine.MAX_TABLE_MEMBER2; i++ ) {	//MAX_TABLE_MEMBER
				short buf_ = get_mjpoint( i );
				gpsTableData.sMemData[i].nMovePoint = yaoch_point[ i ];
				buf_ += yaoch_point[ i ];
				set_mjpoint( (ushort)i, buf_ );
			}
		} else {
			//< 2006/03/26 流し満貫対策
			/* 流局 */
			MjEvent((byte)GEV.SYS_RYUKYOKU, Order);

			for ( i = 0; i < MJDefine.MAX_TABLE_MEMBER; i++ ) {
				/* 聴牌者オープン */
				gpsPlayerWork	=	gsPlayerWork[i];	//&gsPlayerWork[i];
				gNextTimeOut = result_wait_base;
			}
			if( Rultbl[ (int)RL.NOTEN ] != 0 )		//0422
				cal_noten_bappu ();						/* 罰符計算 */
		}
	} else {
		if( Pinch == (byte)SP.THREE_CHAN ) { 						/* トリロン流れ */
			MjEvent( (byte)GEV.SYS_TORIRON,Order);
			for( i = 0; i < MJDefine.MAX_TABLE_MEMBER; i++ )
				if( Hanetbl[ i ] != 0 ) {
					gpsPlayerWork	=	gsPlayerWork[i];	//&gsPlayerWork[i];
					if(gpsPlayerWork.byTenpai != 0) {
					}
				}
		} else if( Pinch == (byte)SP.FOUR_RIC ) { 				/* ４家立直 */
			for( i = 0; i < MJDefine.MAX_TABLE_MEMBER; i++ ) {
				MjEvent( (byte)GEV.SYS_4CYA_REACH,Order);
				//://PlaySE(SENO_TEHAIOPEN);
				//://disp_tehai( PAI_FRONT, i, 0 );		/* ツモ牌無し */
			}
		}
	}
}

public void dpinch_m7 ( /*MahJongRally * pMe*/ )
{
	ushort		i=0;

/* 1.流局     */
/* 2.四開カン */
/* 3.四風連打 */
/* 4.トリロン */
/* 5.四家立直 */
/* 6.九種九牌 */

	if( Pinch == (byte)SP.AGARI )
		return;		//for Safe

	/* 保留立直棒の記憶 */
	for( i = 0; i < MJDefine.MAX_TABLE_MEMBER; i ++ ) {
#if	Rule_2P
#else
		if( gsPlayerWork[i].bFrich != 0 )
			gpsTableData.sMemData[i].byRibo_stack++;
#endif
		KyokuKekka[ i ] = (byte)KEKKA.NAGARE;		/* 結果流局 */
	}

	if (!SubMj.CompVSCompMode)
		_dpinchUser();
	else
		_dpinchComp();

	/* 2.四開カンはプレート表示のみ */
}

/*****************************
	和了終了の判定処理
        winend_f = TRUE :上がり止めがあった
                   FALSE:上がり止めがなかった
*****************************/
public bool check_win_end ( /*MahJongRally * pMe*/ )
{
	bool	winend_f = false;	/* 上がり止めフラグ */
	short	last_p;				/* オーラスの人の点 */

	if ( chkdbn_game() == false ){										/* ドボンがなかった */
		if ( gpsTableData.byKyoku == Kyoend - 1 ) {						/* オーラスである */
			if ( Order == gpsTableData.byOya ) {						/* 親が上がっている */
				rank_sort ( (ushort) 0 );
				if ( gpsTableData.sMemData[Order].byRank == 0 ) {		/* トップである */
					last_p  = get_mjpoint(  Order );
					if ( last_p >= sRuleSubData.nToplin) {				/* 西入点数を超えている	*/
						if ( Order == game_player ) {					/* プレイヤーなら選択	*/
							//://if (!SelectAgariyame()) {				/*	やめるか？			*/
							//://	winend_f = TRUE;
							//://}
							SubMj.agari_end_chk = 1;
						} else {											/* ＣＯＭはトップなら必ず止める */
							winend_f = true;
						}
					}
				}
			}
		}
	}

	return	( winend_f );
}

/**************************************END OF FILE**************************************/


//-*********************Mjm7.j
}
