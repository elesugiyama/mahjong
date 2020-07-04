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
// mjycalc.j
//-*****************
public partial class MahjongBase : SceneBase {
	/*
**		麻雀大会３
**		局および半荘終了時の関数群
*/

//#include "MahJongRally.h"								// Module interface definitions

/*********************************************************************
	麻雀対局共通・単純処理
*********************************************************************/
/*****************************
	指定ポジションの点数を返す
*****************************/
public short get_mjpoint( short pos ) {	return get_mjpoint((ushort) pos );	}
public short get_mjpoint( /*MahJongRally * pMe,*/ ushort pos )
{
	if( pos > 3 )	return( 0 );

	return (gpsTableData.sMemData[pos].nPoint);
}

/*****************************
	指定ポジションに点数をセットする
*****************************/
public void set_mjpoint( /*MahJongRally * pMe,*/ ushort pos, short score )
{
	gpsTableData.sMemData[pos].nPoint	=	score;
}

/*****************************
	指定牌の枚数(手牌内)チェック
	(WP_HKHAIは探しにいかない)
		code = 牌コード
*****************************/
public ushort psearch_count ( /*MahJongRally * pMe,*/ byte who, byte code )
{
	ushort		i;
	//tamaki USHORT	kcnt;
	ushort		cnt;
	ushort		mx;				/* 手牌数 */
	PLAYERWORK	w;		//*w;
	byte		ucPai;

	w = gsPlayerWork[who];
	cnt = 0;
	if ( who == Odrbuf ) {
		mx = (ushort)w.byThcnt;
	}
	else {
		mx = (ushort)(13 - w.byFhcnt * 3);
	}
	for ( i = 0; i < mx; i++ ) {				/* ないていない牌の中を検索 */
		if (  w.byTehai[i] == code ) {
			cnt++;
		}
	}
	for ( i = 0; i < w.byFhcnt; i++ ) {	/* ないている牌を検索 */
		ucPai	=	w.byFrhai[i];
		if ( ucPai < 0x40 ) {	/* 順子に含まれるなら＋１ */
			if ( ( ucPai ==   code		 ) ||
				 ( ucPai == ( code - 1 ) ) ||
				 ( ucPai == ( code - 2 ) )	  ) {
				cnt++;
			}
		}
		else if ( ucPai < 0x80 ) {/* 刻子に含まれるなら＋３ */
			if ( ( ucPai & 0x3f ) == code ) {
				cnt += 3;
			}
		}
		else {/* カン子に含まれるなら＋４ */
			if (( w.byFrhai[i] & 0x3f )  == code ) {
				cnt += 4;
			}
		}
	}
	return	( cnt );
}

/*****************************
	各局の上がりの処理(役表示含む)
*****************************/
public void kyoku_result ( /*MahJongRally * pMe,*/ byte cnt )
{
	short	i, l;
	short 	buf;
	int		iPoint;

	SubMj._byRobiCount	=	0;

	gMJKResult.byYakuCnt	= (byte)(gMJKResult.byYakuCnt	% ( MJDefine.MAX_YAKU + 1 ));	/* 保険：new */

	/* 上がり点数初期化 */
	Total_point 		= 0;
	SubMj.Tsumopoint_from_oya = 0;
	SubMj.Tsumopoint_from_ko	= 0;

	gMJKResult.byFu	=	(byte)(((gMJKResult.byFu + 4) / 5) * 10);
	if (gMJKResult.byFu != 0)
		if ( Rultbl[(int)RL.NAKIPN] != 0 )		/* 鳴き平和形を３０符扱いにするなら */
			if ( ( gMJKResult.byFu == 20 ) && ( ( Status & (byte)ST.RON ) != 0 ) )
				gMJKResult.byFu = 30;

	cal_agari_point();			/* 点数計算 */

	yaku_number_sort();			//役番号のソート

	/** ここから暗くしたまま **/
	if ( Order == gpsTableData.byKamicha_dori )	/* 1996.7.28.BUGFIX */
		iPoint	=	(int)((Total_point - gpsTableData.byRenchan * sRuleSubData.byRenchanRate * 30 ) * 10);
	else
		iPoint	=	(int)(Total_point * 10);
	/* BUGFIX END */

	gMJKResult.nOyaPoint	*=	10;
	gMJKResult.nKoPoint		*=	10;
	gMJKResult.nTotalPoint	*=	10;
	gMJKResult.byRenchan	=	gpsTableData.byRenchan;

	Total_point 		/= 10;
	SubMj.Tsumopoint_from_oya /= 10;
	SubMj.Tsumopoint_from_ko	/= 10;
	SubMj.Wareme_point		/= 10;

	/* 包責任払い */
	if ( SubMj.Paoflg != 0xff ) {						/* パオの時 */
		if (( Status & (byte)ST.RON ) == 0 ) {		/* パオ－ツモの場合 */
			gMJKResult.nKoPoint	=	gMJKResult.nOyaPoint	=	0;
			l = 0xff;
		} else {
			if ( SubMj.Paoflg == Odrbuf )				/* パオがそのまま振った時 */
				l = 0xff;
			else {								/* パオと振った者が折半するとき */
				l = 0;
				for ( i = 0; i < MJDefine.MAX_TABLE_MEMBER2; i++ ) {		//MAX_TABLE_MEMBER
					buf = get_mjpoint(   i );
					if ( i == Order ) {			/* 上がった方はそのままもらう */
						gpsTableData.sMemData[i].nMovePoint	+=	(short)Total_point;
						KyokuKekka[i] = (byte)KEKKA.RON;	/* もらい */
					} else if ( i == SubMj.Paoflg ) {	/* パオの面子なら */
						gpsTableData.sMemData[i].nMovePoint	+=	(short)-( ( Total_point - gpsTableData.byRenchan * sRuleSubData.byRenchanRate * 3 ) / 2 );
										/* 1996.7.15.MENU *//* 積み棒以外の点数の半分 */
					} else if ( i == Odrbuf ) {	/* 振った面子 */

						gpsTableData.sMemData[i].nMovePoint	+=	(short)-( ( Total_point + gpsTableData.byRenchan * sRuleSubData.byRenchanRate * 3 ) / 2 );
												/* 1996.7.15.MENU *//* 積み棒以外の点数の半分と積み棒 */

						KyokuKekka[i] = (byte)KEKKA.HOUJU;	/* 払い */
					} else						/* ツモられた時は、関係ない人は何もない */
						gpsTableData.sMemData[i].nMovePoint	=	0;

					buf	+=	gpsTableData.sMemData[i].nMovePoint;
					set_mjpoint(   (ushort)i, buf );
				}
			}
			gMJKResult.nKoPoint	=	gMJKResult.nOyaPoint	=	gMJKResult.nTotalPoint/2;
		}
		if ( l == 0xff ) {							/* パオが全部払うとき */
			ResultSetWin( Order, Odrbuf, (short)Total_point);	/*	上がり、振り込みデータセット	*/
			for ( i = 0; i < MJDefine.MAX_TABLE_MEMBER2; i++ ) {	//MAX_TABLE_MEMBER
				buf = get_mjpoint(   i );
				if ( i == Order ) {					/* 上がった方はそのままもらう */
					gpsTableData.sMemData[i].nMovePoint	=	(short)Total_point;
					KyokuKekka[i] = (byte)KEKKA.TSUMO;	/* もらい */
				} else if ( i == SubMj.Paoflg ) {			/* パオの面子なら */
					gpsTableData.sMemData[i].nMovePoint	=	(short)-Total_point;
					KyokuKekka[i] = (byte)KEKKA.HOUJU;	/* 払い */
				} else								/* ツモられた時は、関係ない人は何もない */
					gpsTableData.sMemData[i].nMovePoint	=	0;

				buf	+=	gpsTableData.sMemData[i].nMovePoint;
				set_mjpoint(   (ushort)i, buf );
			}
		}
	} else if (( Status & (byte)ST.RINFR ) != 0 ) {			/* Statusの大明ｶﾝﾌﾗｸﾞを見る */
		/* 大明カン責任払い */
		ResultSetWin( Order, Odrbuf, (short)Total_point);	/*	上がり、振り込みデータセット	*/
		for ( i = 0; i < MJDefine.MAX_TABLE_MEMBER2; i++ ) {		/* 大明ｶﾝﾂﾓの時はﾂﾓ上がり */	//MAX_TABLE_MEMBER
			buf = get_mjpoint(   i );
			if ( i == Order ) {						/* 上がった方はそのままもらう */
				gpsTableData.sMemData[i].nMovePoint	=	(short)Total_point;
				KyokuKekka[i] = (byte)KEKKA.TSUMO;		/* もらい */
			} else if ( i == Odrbuf ) {				/* 責任払いの面子なら */
				gpsTableData.sMemData[i].nMovePoint	=	(short)-Total_point;
			} else									/* ツモられた時は、関係ない人は何もない */
				gpsTableData.sMemData[i].nMovePoint	=	0;

			buf	+=	gpsTableData.sMemData[i].nMovePoint;
			set_mjpoint(   (ushort)i, buf );
		}
		gMJKResult.nKoPoint	=	gMJKResult.nOyaPoint	=	0;
	} else {
		/* 通常 */
		/** ツモの場合 **/
		ResultSetWin( Order, -1, (short)Total_point);		/*	上がり、振り込みデータセット	*/
		if ( ( Status & (byte)ST.RON ) == 0 ) {
#if	Rule_2P
			SubMj.Tsumopoint_from_ko= Total_point;
#endif
			if (gpsTableData.byRibo != 0) {					/* 立直棒 */
				SubMj._byRobiCount	+=	gpsTableData.byRibo;
				Total_point += (gpsTableData.byRibo * 10);
			}
			if ( Order == gpsTableData.byOya ) {
				/* ツモ－親の場合 */
				for ( i = 0; i < MJDefine.MAX_TABLE_MEMBER2; i++ ) {	//MAX_TABLE_MEMBER
					buf = get_mjpoint(   i );
					if ( i != gpsTableData.byOya) {
						if ( i == SubMj.Wareme ){
							buf -= (short)SubMj.Wareme_point;
						}else{
							buf -= (short)SubMj.Tsumopoint_from_ko;
						}
					} else
						buf += (short)Total_point;

					set_mjpoint(   (ushort)i, buf );
				}
				for ( i = 0; i < MJDefine.MAX_TABLE_MEMBER2; i++ )	//MAX_TABLE_MEMBER
					if ( i != gpsTableData.byOya) {
						if ( i == SubMj.Wareme )
							gpsTableData.sMemData[i].nMovePoint	+=	(short)( -SubMj.Wareme_point );
						else
							gpsTableData.sMemData[i].nMovePoint	+=	(short)( -SubMj.Tsumopoint_from_ko );
					}

				KyokuKekka[gpsTableData.byOya] = (byte)KEKKA.TSUMO;	/* もらい */
				gpsTableData.sMemData[gpsTableData.byOya].nMovePoint	+=	(short)Total_point;
			} else if ( Order != gpsTableData.byOya) {
				/* ツモ－子の場合 */
#if	Rule_2P
//				Tsumopoint_from_oya*= 2;
				SubMj.Tsumopoint_from_oya= SubMj.Tsumopoint_from_ko;
#endif
				for ( i = 0; i < MJDefine.MAX_TABLE_MEMBER2; i++ ) {	//MAX_TABLE_MEMBER
					buf = get_mjpoint(   i );
					if ( i == Order )
						buf += (short)Total_point;
					else
						if ( i == gpsTableData.byOya)
							buf -= (short)SubMj.Tsumopoint_from_oya;
						else {
							if ( i == SubMj.Wareme ){
								buf -= (short)SubMj.Wareme_point;
							}else{
								buf -= (short)SubMj.Tsumopoint_from_ko;
							}
						}
					set_mjpoint(   (ushort)i, buf );
				}
				for ( i = 0; i < MJDefine.MAX_TABLE_MEMBER2; i++ )	//MAX_TABLE_MEMBER
					if ( i != Order ) {
						if ( i == gpsTableData.byOya)
							gpsTableData.sMemData[i].nMovePoint	+=	(short)( -SubMj.Tsumopoint_from_oya );
						else {
							if ( i == SubMj.Wareme )
								gpsTableData.sMemData[i].nMovePoint	+=	(short)( -SubMj.Wareme_point );
							else
								gpsTableData.sMemData[i].nMovePoint	+=	(short)(-SubMj.Tsumopoint_from_ko);
						}
					}

				KyokuKekka[Order] = (byte)KEKKA.TSUMO;	/* もらい */
				gpsTableData.sMemData[Order].nMovePoint	=	(short)Total_point;
			}
		} else {									/* 振った側から引く */
			/** ロンの場合 **/
			ResultSetWin( Order, Odrbuf, (short)Total_point);		/*	上がり、振り込みデータセット	*/

			buf  = get_mjpoint((short)Odrbuf );
			buf -= (short)Total_point;
			set_mjpoint((ushort)Odrbuf, (short)buf );
			gpsTableData.sMemData[Odrbuf].nMovePoint	+=	(short)(-Total_point);	/* リー棒を足す前に引かれる点数をキープ */
			if ( Order == gpsTableData.byKamicha_dori )		/* リー棒は上家取り */
				if (gpsTableData.byRibo != 0) {
					SubMj._byRobiCount	+=	gpsTableData.byRibo;
					Total_point = Total_point + gpsTableData.byRibo * 10;
				}

			buf  = get_mjpoint((short)Order );
			buf += (short)Total_point;
			set_mjpoint((ushort)Order, (short)buf );

			gpsTableData.sMemData[Order].nMovePoint	+=	(short)Total_point;
			KyokuKekka[Order] = (byte)KEKKA.RON;		/* もらい */
			KyokuKekka[Odrbuf] = (byte)KEKKA.HOUJU;	/* 払い */
		}
	}
	/* ダブロン時に最初だけ出すようにする */
	if( cnt == 0 )
		Dialog_Result();						/*------- 1996.7.5.DIALOG -------*/
}

/*****************************
	点数計算
*****************************/
/*	端数計算	*/
static int cal_agariE( int pnt) {
	return( pnt % 10)!= 0 ? ((pnt/ 10)+ 1)* 10 : pnt;
}
public void cal_agari_point ( /*MahJongRally * pMe*/ )
{
//	SHORT	i, l;
	short	l;
	short	fucnt = 0;
	short	pntcnt = 0;
	short	dum_pnt = 0;
	byte	chitoi_flag = 0;

#if	Rule_2P
	long	from_oya= 0;
	long	from_ko= 0;
#endif

	/* 符の計算(表示前までfucntには実際の10分の1の値が入っている) */
	fucnt = (short)(gMJKResult.byFu / 10);
	if ( ( gMJKResult.byFu % 10 ) != 0 ) 						/* 符跳ね */
		fucnt++;

	//> 2006/04/05 満貫以上だと七対子の符０として表示される	//0422
	for ( l = 0; l < gMJKResult.byYakuCnt; l++ )
		if ( gMJKResult.sYaku[l].name == (byte)YK.CHITO ) {			/* チートイの判定 */
			chitoi_flag = 1;
			break;
		}
	//< 2006/04/05 満貫以上だと七対子の符０として表示される

	/** 点数計算 **/
	SubMj.Mangan = 0;
	/* 単純に役満ではない時 */
	if ( gMJKResult.byYakuman == 0 ) {
		if ( gMJKResult.byHan >= 5 ) {							/* 満貫 */
			SubMj.Mangan++;
			if ( gMJKResult.byHan >= 6 ) {						/* 跳満 */
				SubMj.Mangan++;
				if ( gMJKResult.byHan >= 8 ) {					/* 倍満 */
					SubMj.Mangan++;
					if ( gMJKResult.byHan >= 11 ) {				/* 三倍満 */
						SubMj.Mangan += 2;
						if ( gMJKResult.byHan >= 13 )			/* 数え役満 */
							SubMj.Mangan += 2;
					}
				}
			}
		} else if ( (( gMJKResult.byHan == 4 ) && ( fucnt >= 4 )) ||	/* ４飜以下で満貫の時 */
				  (( gMJKResult.byHan == 3 ) && ( fucnt >= 7 )) ||
				  (( gMJKResult.byHan == 2 ) && ( fucnt >= 13 ))   ) {
			SubMj.Mangan++;
		} else {	/* 満貫以上ではないときの点数計算 */
			//> 2006/04/05 満貫以上だと七対子の符０として表示される		//0422
			//for ( l = 0; l < gMJKResult.byYakuCnt; l++ ) {
			//	if ( gMJKResult.sYaku[l].name == YK_CHITO ) {			/* チートイの判定 */
			//		chitoi_flag = 1;
			//		break;
			//	}
			//}
			//< 2006/04/05 満貫以上だと七対子の符０として表示される

			/* 基本の点数を計算(Total_pointには基本点数が入っているが、最終的には合計得点が入る) */
			/* ばんばんの分 */
			if ( chitoi_flag != 0)
				Total_point = 10;					/* 2.5(25符) * 2 * 2 */
			else
				Total_point = fucnt * 2 * 2;

			/* 飜数の分だけ×２ */
			for ( l = 0; l < gMJKResult.byHan; l++ )
				Total_point *= 2;

			if ( ( Status & (byte)ST.RON ) != 0 ) {		/* ロン上がりの時の場ゾロを含めた点数計算 */
				if ( Order == gpsTableData.byOya)
					Total_point *= 6;				/* 場ゾロ×1.5 */
				else
					Total_point *= 4;				/* 場ゾロ×1 */

				pntcnt = (short)(Total_point % 10);	/* 端数があるなら切り上げ */
				if ( pntcnt != 0 ) {
					dum_pnt 	= (short)(Total_point / 10);
					Total_point = ( dum_pnt + 1 ) * 10;
				}
			} else {								/* ツモ上がりの時の場ゾロを含めた点数計算 */
				if ( Order == gpsTableData.byOya)
					SubMj.Tsumopoint_from_ko	= Total_point * 2;
				else {
					SubMj.Tsumopoint_from_oya = Total_point * 2;
					SubMj.Tsumopoint_from_ko	= Total_point * 1;
				}
#if	Rule_2P
//				from_oya= Tsumopoint_from_oya;
//				from_ko= Tsumopoint_from_ko;
#else
				/* 子からの点数 */
				pntcnt = (short)(Tsumopoint_from_ko % 10);		/* 端数があるなら切り上げ */
				if ( pntcnt != 0 ) {
					dum_pnt = (short)(Tsumopoint_from_ko / 10);
					Tsumopoint_from_ko = ( dum_pnt + 1 ) * 10;
				}
				if ( Tsumopoint_from_oya != 0 ) {				/* 親からの点数があるなら */
					pntcnt = (short)(Tsumopoint_from_oya % 10);	/* 端数があるなら切り上げ */
					if ( pntcnt != 0 ) {
						dum_pnt = (short)(Tsumopoint_from_oya / 10);
						Tsumopoint_from_oya = ( dum_pnt + 1 ) * 10;
					}
				}
#endif
			}
		}
	}

	if ( gMJKResult.byYakuman != 0 ) {										/* 役満の時の点数計算 */
		if ( Rultbl[(int)RL.DBYMAN] == 0 )										/* ダブル役満無し */
			if ( gMJKResult.byHan > 4 )
				gMJKResult.byYakuman	= gMJKResult.byHan = 4;				/* ４倍満扱いにする */

		if ( ( Status & (byte)ST.RON ) != 0 ) {		/* ロン上がりの時 */
			if ( Order == gpsTableData.byOya)
				Total_point = gMJKResult.byHan * 400 * 3;
			else
				Total_point = gMJKResult.byHan * 400 * 2;
		} else {								/* ツモ上がりの時 */
			if ( Order == gpsTableData.byOya)
				SubMj.Tsumopoint_from_ko	= gMJKResult.byHan * 400;
			else {
				SubMj.Tsumopoint_from_oya = gMJKResult.byHan * 400;
				SubMj.Tsumopoint_from_ko	= gMJKResult.byHan * 200;
			}
		}
	} else if ( SubMj.Mangan != 0 ) {					/* 満貫以上の時の点数計算 */
		if ( ( Status & (byte)ST.RON ) != 0 ) {		/* ロン上がりの時 */
			if ( Order == gpsTableData.byOya)
				Total_point = ( SubMj.Mangan + 1 ) * 200 * 3;
			else
				Total_point = ( SubMj.Mangan + 1 ) * 200 * 2;
		} else {								/* ツモ上がりの時 */
			if ( Order == gpsTableData.byOya)
				SubMj.Tsumopoint_from_ko = ( SubMj.Mangan + 1 ) * 200;
			/** 子の時 **/
			else {
				SubMj.Tsumopoint_from_oya = ( SubMj.Mangan + 1 ) * 200;
				SubMj.Tsumopoint_from_ko	= ( SubMj.Mangan + 1 ) * 100;
			}
		}
	}

	from_oya= SubMj.Tsumopoint_from_oya;
	from_ko= SubMj.Tsumopoint_from_ko;

	/* ツモ上がりなら各家のマイナス分を合計 */
	gMJKResult.nOyaPoint	=	gMJKResult.nKoPoint	=	0;
	if ( ( Status & (byte)ST.RON ) == 0 ) {
#if	Rule_2P
		if ( Order == gpsTableData.byOya)
			Total_point = cal_agariE((int)(from_ko * 3));
		else
			Total_point = cal_agariE((int)(from_ko * 2 + from_oya));

		gMJKResult.nOyaPoint	=	cal_agariE((int)from_oya* 2);
		gMJKResult.nKoPoint		=	cal_agariE((int)from_ko* 3);

	#if	__MJ_CHECK
Debug.Log("-----------------------");
Debug.Log("Total_point: "+ Total_point);
Debug.Log("from_oya: "+ from_oya);
Debug.Log("from_ko: "+ from_ko);
Debug.Log("gMJKResult.nOyaPoint: "+ gMJKResult.nOyaPoint);
Debug.Log("gMJKResult.nKoPoint: "+ gMJKResult.nKoPoint);
Debug.Log("-----------------------");
	#endif
#else
		if ( Order == gpsTableData.byOya)
			Total_point = Tsumopoint_from_ko * 3;
		else
			Total_point = Tsumopoint_from_ko * 2 + Tsumopoint_from_oya;

		gMJKResult.nOyaPoint	=	(int)Tsumopoint_from_oya;
		gMJKResult.nKoPoint		=	(int)Tsumopoint_from_ko;
#endif
	}
	gMJKResult.nTotalPoint	=	(int)Total_point;

	/* 割れ目有りなら */
	if ( Rultbl[(int)RL.WAREM] != 0 ) {
		if ( Order == SubMj.Wareme ) {				/* 割れ目なら全部２倍 */
			SubMj.Tsumopoint_from_oya *= 2;
			SubMj.Tsumopoint_from_ko *= 2;
			Total_point *= 2;
		} else {								/* 割れ目じゃない */
			if (( Status & (byte)ST.RON ) != 0 ) {	/* ロン上がりの時 */
				if ( Odrbuf == SubMj.Wareme )			/* 振り込んだ相手が割れ目 */
					Total_point *= 2;
				/*else {} 上がった側も振った側も割れ目じゃない*/
			} else {							/* ツモ上がりの時 */
				if ( Order == gpsTableData.byOya) {
					Total_point  = Total_point / 3 * 4;
					SubMj.Wareme_point = SubMj.Tsumopoint_from_ko * 2;
				} else {
					if ( SubMj.Wareme == gpsTableData.byOya) {
						Total_point 		+= SubMj.Tsumopoint_from_oya;
						SubMj.Tsumopoint_from_oya *= 2;
						SubMj.Wareme_point		 = SubMj.Tsumopoint_from_oya;
					} else {
						Total_point  += SubMj.Tsumopoint_from_ko;
						SubMj.Wareme_point = SubMj.Tsumopoint_from_ko * 2;
					}
				}
			}
		}
	}

	/* 積み棒の分の計算 */
	if ( Order == gpsTableData.byKamicha_dori )		/* 積み棒は上家どり */
		if (gpsTableData.byRenchan != 0) {
			Total_point += ( gpsTableData.byRenchan * sRuleSubData.byRenchanRate * 30 );	/* 1996.7.15.MENU */
			if (( Status & (byte)ST.RON ) == 0 ) {		/* ツモなら */
				SubMj.Tsumopoint_from_ko += ( gpsTableData.byRenchan * sRuleSubData.byRenchanRate * 10 );
													/* 1996.7.15.MENU */
				if ( Order != gpsTableData.byOya)	/* ツモったのが親でなければ */
					SubMj.Tsumopoint_from_oya += ( gpsTableData.byRenchan * sRuleSubData.byRenchanRate * 10 );
													/* 1996.7.15.MENU */
				if ( Order != SubMj.Wareme )
					SubMj.Wareme_point += ( gpsTableData.byRenchan * sRuleSubData.byRenchanRate * 10 );
													/* 1996.7.15.MENU */
			}
		}

	/* リー棒の分の計算は別でやる */

	/* 表示用の符の計算(修正) */
	gMJKResult.byFu = (byte)( ( chitoi_flag != 0) ? 25 : ( fucnt * 10 ) );
}

/*****************************
	役番号のソート
*****************************/
public void yaku_number_sort ( /*MahJongRally * pMe*/ )
{
#if	__YakuNameSort

	#if false //-*todo:MahjongDeffine.csに移植
	// char yakuData[] = {
	// #include "../../YamaSortData.j"
	// };
	#endif //-*todo:MahjongDeffine.csに移植
	int		i, j, k= 0;
	bool[]	yakuBuf= new bool [gMJKResult.byYakuCnt];
	YAKU_T[]	tempYAKU= new YAKU_T[gMJKResult.byYakuCnt];

	//初期化
	for( i= 0; i< gMJKResult.byYakuCnt; i++ ) {
		yakuBuf[i]= false;
		tempYAKU[i]= new YAKU_T();
		tempYAKU[i].name= gMJKResult.sYaku[i].name;
		tempYAKU[i].factor= gMJKResult.sYaku[i].factor;
	}

	//ソート
	for( j= 0; j< MJDefine.yakuData.Length; j++ )
		for( i= 0; i< gMJKResult.byYakuCnt; i++ )
			if( tempYAKU[i].name== (byte)MJDefine.yakuData[j]) {
				yakuBuf[i]= true;
				gMJKResult.sYaku[k].name= tempYAKU[i].name;
				gMJKResult.sYaku[k].factor= tempYAKU[i].factor;
				k++;
				break;
			}

	//未登録チェック
	if( gMJKResult.byYakuCnt!= k) {
	#if _DEBUG
	Debug.out("役名未登録: "+ (int)(gMJKResult.byYakuCnt- k));
	#endif
		for( i= 0; i< gMJKResult.byYakuCnt; i++ )
			if( !yakuBuf[i]) {
				yakuBuf[i]= true;
				gMJKResult.sYaku[k].name= tempYAKU[i].name;
				gMJKResult.sYaku[k].factor= tempYAKU[i].factor;
				k++;
				break;
			}
	}
#else
	for ( int i = 0; i < gMJKResult.byYakuCnt - 1; i++ )
		for ( int l = (short)(i + 1); l < gMJKResult.byYakuCnt; l++ ) {
			if ( gMJKResult.sYaku[i].name > gMJKResult.sYaku[l].name ) {
				byte	yakudammy			=	gMJKResult.sYaku[i].name;
				gMJKResult.sYaku[i].name	=	gMJKResult.sYaku[l].name;
				gMJKResult.sYaku[l].name	=	yakudammy;
				yakudammy					=	gMJKResult.sYaku[i].factor;
				gMJKResult.sYaku[i].factor	=	gMJKResult.sYaku[l].factor;
				gMJKResult.sYaku[l].factor	=	yakudammy;
			}
		}
#endif
}

/*****************************
	ノーテン罰符の計算
*****************************/
public void cal_noten_bappu ( /*MahJongRally * pMe*/ )
{
	byte[]	tenpai_mentsu = new byte[MJDefine.MAX_TABLE_MEMBER];
	byte	tenpai_all = 0;
	byte	bappu_plus = 0;
	byte	bappu_minus = 0;
	short	i;
	short	buf;

	/* 旧ポイントをキープ */
	for ( i = 0; i < MJDefine.MAX_TABLE_MEMBER; i++ ) {
//m		gpsTableData.sMemData[i].nOldPoint	=	gpsTableData.sMemData[i].nPoint;
		tenpai_mentsu[i] = 0;
	}
	for ( i = 0; i < MJDefine.MAX_TABLE_MEMBER2; i++ ) {		//MAX_TABLE_MEMBER
		gMJKResult.byYakuCnt	=	0;
		if ( gsPlayerWork[i].byTenpai != 0	) {
			++tenpai_mentsu[i];
			++tenpai_all;
		}
	}
//#if 0
//	for ( i = 0; i < MAX_TABLE_MEMBER; i++ ) {
//		tenpai_all += tenpai_mentsu[i];
//	}
//#endif

#if	Rule_2P
#else
	if ( tenpai_all == 1 ) {									/*	１人テンパイ	*/
		for ( i = 0; i < MAX_TABLE_MEMBER; i ++ )
			if ( tenpai_mentsu[i] != 0 )
				break;
		/* 一人聴牌で(62) */
		MjDialog( (BYTE)i, NONE, DIAL_TENPAI_ONLY );
	} else if ( tenpai_all == 3 ) {								/*	３人テンパイ	*/
		for ( i = 0; i < MAX_TABLE_MEMBER; i ++ )
			if ( tenpai_mentsu[i] == 0 )
				break;
		/* 一人不聴で(63) */
		MjDialog( (BYTE)i, NONE, DIAL_NOTEN_ONLY );
	} else if ( tenpai_all == 4 ) {								/*	全員テンパイ	*/
		/* 全員聴牌で親が(64) */
		MjDialog( gpsTableData.byOya, NONE, DIAL_TENPAI_ALL );
	}
#endif

	switch ( tenpai_all ) {
#if	Rule_2P
	  case 1:
		bappu_plus	= 15;
		bappu_minus = 15;
		break;
#else
	  case 1:
		bappu_plus	= 30;
		bappu_minus = 10;
		break;
	  case 2:
		bappu_plus	= 15;
		bappu_minus = 15;
		break;
	  case 3:
		bappu_plus	= 10;
		bappu_minus = 30;
		break;
	  default:
		break;
#endif
	}
	switch ( tenpai_all ) {
	  case 1:
	  case 2:
	  case 3:
		for ( i = 0; i < MJDefine.MAX_TABLE_MEMBER; i++ ) {
			buf = get_mjpoint(   i );
			if ( tenpai_mentsu[i] != 0) {
				gpsTableData.sMemData[i].nMovePoint	=	(short)bappu_plus;
				buf += bappu_plus;
				//://set_mjkao_act( i, HYOUJOU_LAUGH );
			} else {
				gpsTableData.sMemData[i].nMovePoint	=	(short)(bappu_minus * -1);
				buf -= bappu_minus;
				//://set_mjkao_act( i, HYOUJOU_SAD );
			}
			set_mjpoint(   (ushort)i, buf );
		}
		break;
	  case 0:		/* "全員ノーテン" */
	  case 4:		/* "全員テンパイ" */
	  default:
		break;
	}
}

/*****************************
	チップの計算
*****************************/
public void cal_tip ( /*MahJongRally * pMe*/ )
{
	ushort	i, l;
	ushort	k_cnt;		/* 裏どらを何枚検索するか */
	ushort	d_cnt;		/* 裏どらの数 */
	ushort	tr_cnt; 	/* 鳥の数 */
	ushort	ps_cnt; 	/* ピストルの数 */
	ushort	kg_cnt; 	/* 籠の数 */
	byte	mekuri;		/* めくられる牌のbipaiのｵﾌｾｯﾄ */
	byte	n_cnt;		/* めくった牌が手牌に何枚あるか */
	int		iCUra, iCAlice, iCIppatu, iCTori;

	iCUra	=	iCAlice	=	iCIppatu	=	iCTori	=	0;
	setwp_m2( Order);
	if ( gMJKResult.byYakuman < 4 ) {/* 役満は[裏ドラ賞･一発賞･鳥撃ち]をつけない */
		/* 一発賞 */
		if (Rultbl[(int)RL.IPPAT] != 0 && Rultbl[(int)RL.IPSHO] != 0) {					/* 一発賞のルールが有り */
			for ( i = 0; i < gMJKResult.byYakuCnt; i++ ) {
				if ( gMJKResult.sYaku[i].name == (byte)YK.IPPAT ) {					/* 役番号テーブルに一発がある */
					if (( Status & (byte)ST.RON ) == 0 ) {			/* ツモの時 */
						for ( l = 0; l < MJDefine.MAX_TABLE_MEMBER; l++ ) {
							if ( l != Order ) {					/* 他家から２ずつ */
								gpsTableData.sMemData[l].nMoveChip	-=	2;
							}
							else {
								gpsTableData.sMemData[l].nMoveChip	+=	2*3;				/* もらう */
							}
						}
						iCIppatu	=	2*3;
					}
					else {/* ロンの時 */
						gpsTableData.sMemData[Odrbuf].nMoveChip	-=	2;					/* 当った方 */
						gpsTableData.sMemData[Order].nMoveChip	+=	2;					/* 上がった方 */
						iCIppatu	=	2;
					}
					break;										/* 役の一発が見つかったのでブレーク */
				}
			}
		}
		/* 裏ドラ賞 */
		if (( Rultbl[(int)RL.URA] & Rultbl[(int)RL.RDSHO] ) != 0 ) {	/* 裏ドラ有りで裏ドラ賞も有り */
			if ( gsPlayerWork[Order].bFrich != 0 ) {
				if (( Rultbl[(int)RL.KANUR] & Rultbl[(int)RL.KAN] ) != 0 ) {	/* カン裏も有りか？(ちなみにカンドラ無しならカンウラも無し) */
					k_cnt = (ushort)Kancnt;
				}
				else {
					k_cnt = 0;
				}
				d_cnt = 0;
				for ( i = 0; i < k_cnt + 1; i++ ) {
					SubMj.Uradora_rec[i] = (byte)d_cnt;
					/* １枚ずつ裏がのったかどうかカウント */
					d_cnt += psearch_count (   Order, Ura[i] );
					if ( Ura[i] == gpsPlayerWork.byHkhai) {				/* 上がった牌も調べる */
						d_cnt++;
					}
					SubMj.Uradora_rec[i] = (byte)(d_cnt - SubMj.Uradora_rec[i]);
				}
				if ( d_cnt != 0 ) {
					if (( Status & (byte)ST.RON ) == 0 ) {					/* ツモの時 */
						for ( l = 0; l < MJDefine.MAX_TABLE_MEMBER; l++ ) {
							if ( l != Order ) {						/* 他家から裏ドラののった数ずつ */
								gpsTableData.sMemData[l].nMoveChip	-=	(short)d_cnt;
							}
							else {
								gpsTableData.sMemData[l].nMoveChip	+=	(short)(d_cnt*3);				/* もらう */
							}
						}
						iCUra	=	d_cnt*3;
					}
					else {/* ロンの時 */
						gpsTableData.sMemData[Odrbuf].nMoveChip	-=	(short)d_cnt;					/* 当った方 */
						gpsTableData.sMemData[Order].nMoveChip	+=	(short)d_cnt;					/* 上がった方 */
						iCUra	=	d_cnt;
					}
					SubMj.tip_ura_cnt = (byte)(k_cnt+1);
				}
			}
		}
		SubMj.tip_tori_buf[0] = 0;
		SubMj.tip_tori_buf[1] = 0;
		SubMj.tip_tori_buf[2] = 0;
		/* 鳥撃ち */
		if (( Rultbl[(int)RL.TORIU] != 0 ) && (( Status & (byte)ST.RON ) != 0 )) {	/* 鳥撃ち有りで、ロンの時 */
			tr_cnt = psearch_count((byte)Order, (byte)0x11 );				/* 手牌の中の一索の数 */
			if ( gpsPlayerWork.byHkhai == 0x11 ) {							/* 上がった牌も調べる */
				tr_cnt++;
			}
			if ( tr_cnt != 0 ) {											/* 上がった手牌に一索がある時 */
				ps_cnt = psearch_count ((byte)Odrbuf, (byte)0x27 );			/* 相手の手牌の七筒の数 */
				kg_cnt = psearch_count ((byte)Order,  (byte)0x18 );			/* 手牌の中の八索の数 */
				if ( gpsPlayerWork.byHkhai == 0x18 ) {						/* 上がった牌も調べる */
					kg_cnt++;
				}
				if ( ( ps_cnt == 0 ) ||
					 (( ps_cnt != 0 ) && ( kg_cnt != 0 )) ){				/* 鳥撃ちでピストルがないか、あっても籠がある */
					gpsTableData.sMemData[Odrbuf].nMoveChip	-=	(short)tr_cnt;		/* 当った方 */
					gpsTableData.sMemData[Order].nMoveChip	+=	(short)tr_cnt;		/* 上がった方 */
					iCTori	=	tr_cnt;
				}
				SubMj.tip_tori_buf[0] = (byte)tr_cnt;
				SubMj.tip_tori_buf[1] = (byte)ps_cnt;
				SubMj.tip_tori_buf[2] = (byte)kg_cnt;
			}
		}
		_MEMSET(SubMj.tip_alice_hai,0,SubMj.tip_alice_hai.Length);
		_MEMSET(SubMj.tip_alice_cnt,0,SubMj.tip_alice_cnt.Length);
		if ( ( Rultbl[(int)RL.ALICE] != 0 ) &&
			 ( gpsPlayerWork.bFrich != 0 )  ){			/* アリス */
			if ( Rultbl[(int)RL.KAN] != 0 ) {				/* カンドラ有りのとき */
				mekuri = (byte)(128 - Kancnt * 2);		/* 128 → ﾄﾞﾗの隣 */
			}
			else {										/* カンドラ無しのとき */
				mekuri = 128;
			}
			iCAlice = 0;
//			alice_start_disp ( ( mekuri - Bpcnt ) / 2 );
			l = 0;
			if ( mekuri > Bpcnt ) {
				while ( mekuri > Bpcnt ) {
					n_cnt = (byte)psearch_count(  Order, Bipai[mekuri] );
					if ( gpsPlayerWork.byHkhai == Bipai[mekuri] ) {
						n_cnt++;
					}
					SubMj.tip_alice_hai[l] = Bipai[mekuri];
					if (( Status & (byte)ST.RON ) == 0 ) {	/* ツモの時 */
						for ( i = 0; i < MJDefine.MAX_TABLE_MEMBER; i++ ) {
							if ( i == Order ) {
								gpsTableData.sMemData[i].nMoveChip	+=	(short)( n_cnt * 3 );
							}
							else {
								gpsTableData.sMemData[i].nMoveChip	-=	n_cnt;
							}
						}
						iCAlice	+=	n_cnt*3;
					}
					else {/* ロンの時 */
						gpsTableData.sMemData[Order].nMoveChip	+=	n_cnt;
						gpsTableData.sMemData[Odrbuf].nMoveChip	-=	n_cnt;
						iCAlice	+=	n_cnt;
					}
					SubMj.tip_alice_cnt[l] = n_cnt;
					if ( n_cnt == 0 ) {
						break;
					}
					mekuri -= 2;
					l++;
				}
			}
		}
	}

	if ((sRuleSubData.byFlag & (byte)RULESUBFLAG.CHIP) != 0) {
		if ((iCTori + iCAlice + iCIppatu + iCUra) > 0) {
		}
	}
	/* 役満賞 */
	if ( Rultbl[(int)RL.YMSHO] != 0 ) {
		if ( gMJKResult.byYakuman != 0 || gMJKResult.byHan >= 13) {				/* 役満なら */
			if ( gMJKResult.sYaku[0].name != (byte)YK.SHISA ) {				/* 十三不搭でないなら */
				if (( Status & (byte)ST.RON ) == 0 ) {		/* ノーマルな役満ツモの時 */
					for ( l = 0; l < MJDefine.MAX_TABLE_MEMBER; l++ ) {
						if ( l != Order ) {/* 他家から５ずつ */
							gpsTableData.sMemData[l].nMoveChip	-=	5;
						}
						else {
							gpsTableData.sMemData[l].nMoveChip	+=	5*3;/* もらう */
						}
					}
				}
				else {									/* ロンの時 */
					gpsTableData.sMemData[Odrbuf].nMoveChip	-=	15;				/* 当った方 */
					gpsTableData.sMemData[Order].nMoveChip	+=	15;				/* 上がった方 */
				}
			}
		}
	}

	//> 2006/02/26 No.135
	//  ここでチップの和を取っているので、チップ獲得別表示を行うには別途処理が必要
	_dispChipData();
#if _DEBUG
	TRACE("CHIP:Ippatu=%d,Ura=%d,Tori=%d,Alice=%d",iCIppatu,iCUra,iCTori,iCAlice );
	TRACE("MOVE CHIP :%d,%d,%d,%d",
		gpsTableData.sMemData[0].nMoveChip,gpsTableData.sMemData[1].nMoveChip,
		gpsTableData.sMemData[2].nMoveChip,gpsTableData.sMemData[3].nMoveChip
		);
#endif
	//< 2006/02/26

	SubMj.tip_buf[0] = (byte)iCIppatu;
	SubMj.tip_buf[1] = (byte)iCUra;
	SubMj.tip_buf[2] = (byte)iCTori;
	SubMj.tip_buf[3] = (byte)iCAlice;
}

/*****************************
	次局のために変数を増減
		mode = 0:流局
			   1:アガリ
*****************************/
public void _setresultRyuukyoku(/*MahJongRally * pMe*/)
{
	gMJKResult.byYakuCnt	=	0;
	for ( int i = 0; i < MJDefine.MAX_TABLE_MEMBER; i++ )
		if ( gsPlayerWork[i].byTenpai != 0	)	//*ウキウキ:覚書：0が自分、1が相手
			ResultSetTanpai( i);									/*	テンパイデータセット	*/

	gpsTableData.byRenchan++;
}

public void set_for_nextplay( /*MahJongRally * pMe,*/ byte mode )
{
	short	i;

	SubMj.set_for_nextplay_fRenchan	=	false;
	Totsunyu_flg = 0;
	SubMj.kyoku_end_mode = mode;

	SubMj.agari_end_chk = 0;
	if( mode == 0 ){	/*** 流局の場合 ***/
		if ((sRuleSubData.byFlag & (byte)RULESUBFLAG.RENCHAN) != 0)
			_setresultRyuukyoku();
		else if ( Pinch != (byte)SP.HOAN ) {							/** 荒牌以外の場合 **/
			if ( Rultbl[(int)RL.NAGARE] == 1 ) {						/* 輪荘→積み棒増えて、親流れ */
				_setresultRyuukyoku();
				Totsunyu_flg = 0xFF;
			} else if ( Rultbl[(int)RL.NAGARE] == 2 )				/* 連荘→積み棒増えて、親連荘 */
				_setresultRyuukyoku();
			/* Rultbl[RL_NAGARE] == 0 流局→積み棒増えずにやり直し(何もせず) */
		} else {												/** 荒牌 **/
			if ( Rultbl[(int)RL.NANBA] == 0 ) {						/* 全て不聴流れのルールの時 */
				if ( gsPlayerWork[gpsTableData.byOya].byTenpai == 0 )	/* 親がノーテンの時 */
					Totsunyu_flg = 0xFF;
			} else if ( Rultbl[(int)RL.NANBA] == 1 ) {				/* 南場は流れないルールの時 */
				if ( ( gpsTableData.byKyoku < 4 )	&&
				 ( gsPlayerWork[gpsTableData.byOya].byTenpai == 0 ) )	/* 東場で親が不聴の時は流れる */
					Totsunyu_flg = 0xFF;
			} else if ( Rultbl[(int)RL.NANBA] == 2 )					/* 上がりのみ連荘の時 */
				Totsunyu_flg = 0xFF;

			_setresultRyuukyoku();								/* 通常流局は必ず連荘棒が増える */
		}
		gpsTableData.byParen	= 0;
	} else {		/** アガリの場合 **/
		if ( Rultbl[(int)RL.WINEND] != 0 )				/* 和了終了有りなら */
			if ( check_win_end() == true )
				Totsunyu_flg = 0xFF;

		if ( Rultbl[(int)RL.TWO_CHAHO] != 0 ) {				/* ダブロンあり：親が上がりで連荘 */
			if ( Hanetbl[gpsTableData.byOya] == 0 ) {
				//> 2006/03/15 不具合No.183 場縛り時に子の和了りで積み棒がクリアされる
				if (sRuleSubData.byRuleNo != (byte)RULETYPE.BASHIBARI)			/*	場縛りの時	*/
					gpsTableData.byRenchan	= 0;	/* 連荘(積み棒)クリア */

				// gpsTableData.byRenchan	= 0;	/* 連荘(積み棒)クリア */
				//< 2006/03/15 不具合No.183
				gpsTableData.byParen	= 0;	//FALSE;
				Ryansh = false;
				intwrk_m2e();					//リャンハン縛りチェック
				Totsunyu_flg = 0xFF;
			} else {
				gpsTableData.byRenchan++;
				gpsTableData.byParen++;
			}
		} else {										/* ダブロン無し：自分が上がっている時のみ連荘 */
			if ( gpsTableData.byKamicha_dori != gpsTableData.byOya) {
				if (sRuleSubData.byRuleNo != (byte)RULETYPE.BASHIBARI)			/*	場縛りの時	*/
					gpsTableData.byRenchan	= 0;	/* 連荘(積み棒)クリア */

				gpsTableData.byParen	= 0;	//FALSE;
				Ryansh = false;
				intwrk_m2e();					//リャンハン縛りチェック
				Totsunyu_flg = 0xFF;
			} else {
				gpsTableData.byRenchan++;
				if(gpsTableData.byRenchan>99)gpsTableData.byRenchan = 99;
				gpsTableData.byParen++;
			}
		}
		if ( gMJKResult.sYaku[0].name == (byte)YK.SHISA )		/* 1996.7.28.BUGFIX 十三不搭の場合八連荘は途切れる */
			gpsTableData.byParen	= 0;	/* BUGFIX END */
#if	Rule_2P
#else
		for ( i = 0; i < MAX_TABLE_MEMBER; i ++ )		/* リーチの記録を消す (無･有に関わらず) */
			gpsTableData.sMemData[i].byRibo_stack = 0;
#endif

		gpsTableData.byRibo	= 0;
		if ((sRuleSubData.byFlag & (byte)RULESUBFLAG.RENCHAN) != 0)
			SubMj.set_for_nextplay_fRenchan	=	true;
	}
}

/*********************************************************************
	半荘終了処理
*********************************************************************/
/*****************************
	順位決定
		mode = 0:半荘ポイントを格納しない
			   1:半荘ポイントを格納する
*****************************/
public void rank_sort ( /*MahJongRally * pMe,*/ ushort mode )
{
	short	i, l;
	short	point_ones;		/* 調べてる点 */
	short	point_other;	/* まわしてる(他３)点 */
	byte	rank_counter;	/* ランク(0-3) */
	byte	man_a;			/* 調べてる人 */
	byte	man_b;			/* まわしてる(他３)人 */

	for ( i = 0, man_a = gpsTableData.byChicha; i < MJDefine.MAX_TABLE_MEMBER; i++ ) {
		point_ones = get_mjpoint( (ushort)man_a );
		rank_counter = 0;
		for ( l = 0, man_b = gpsTableData.byChicha; l < MJDefine.MAX_TABLE_MEMBER; l++ ) {
			if ( man_a != man_b ) {
				point_other = get_mjpoint( (ushort)man_b );
				if ( ( short )point_ones < ( short )point_other ) {
					rank_counter++;
				}
				else if ( point_ones == point_other ) {
					if ( i > l ) {
						rank_counter++;
					}
				}
			}
			man_b = (byte)(( man_b + 1 ) % MJDefine.MAX_TABLE_MEMBER);
		}
		gpsTableData.sMemData[man_a].byRank = rank_counter;
		if( mode == 1 )
			gpsTableData.sMemData[man_a].nPnt	=	point_ones;

		man_a = (byte)(( man_a + 1 ) % MJDefine.MAX_TABLE_MEMBER);
	}
}

/*****************************
	半荘終了立直棒戻しの処理
*****************************/
public void ribo_return( /*MahJongRally * pMe*/ )
{
	short	i;
	short	buf;
	short	ribo_buf;

	if ( Rultbl[(int)RL.RIBO] != 0 ) {							/* 立直棒戻し有り */
		for ( i = 0; i < MJDefine.MAX_TABLE_MEMBER; i ++ ) {			/* リーチを欠けた面子を記録する */
			buf = get_mjpoint(   i );
			buf += (short)( gpsTableData.sMemData[i].byRibo_stack * 10 );
			set_mjpoint(   (ushort)i, buf );
		}
	} else {												/* 立直棒戻し無し */
		ushort	top = (ushort)0xFFFF;	//w4

		rank_sort ((ushort) 0 );							/* 先に一度ランク決定 */
		for ( i = 0, ribo_buf = 0; i < MJDefine.MAX_TABLE_MEMBER; i ++ ) {
			if( gpsTableData.sMemData[i].byRank == 0 ){
				top = (ushort)i;
			}
			ribo_buf += (short)( gpsTableData.sMemData[i].byRibo_stack * 10 );
		}

		if ( top == 0xFFFF ) {
		#if false //-*todo:
			ITRACE(("ERROR"));
		#endif //-*todo:
		} else {
			buf  = get_mjpoint(   top );
			buf += ribo_buf;
			set_mjpoint(   top, buf );
		}
	}
}

//-*********************mjycalc.j
}
