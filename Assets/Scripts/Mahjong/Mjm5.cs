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
// mjm5.j
//-*****************
public partial class MahjongBase : SceneBase {
/*
**		麻雀大会３
**		麻雀ゲームシステムのチェック
*/

//#include "MahJongRally.h"								// Module interface definitions


/*****************************
	聴牌チェック
*****************************/
public int chkkokushi( /*MahJongRally * pMe*/ )
{
	byte[]	p;
	byte	x	=	0;
	int		cnt	=	0, p0= 0;
	byte[]	kokushi	= new byte[]{
		0x01,0x09,0x11,0x19,0x21,0x29,0x31,0x32,0x33,0x34,0x35,0x36,0x37
	};

	for(p=kokushi; p0< 13; ++p0) {		//(p=kokushi; p<kokushi+13; ++p)
		switch(cntbuf[p[p0]]) {
			case 0:
				if(x != 0)
					return 0;
				x=p[p0];
				goto case 1;
			case 1:
				break;
			case 2:
				if(cnt != 0)
					return 0;
				++cnt;
				break;
			default:
				return 0;
		}
	}
	if(x == 0) {
		gpsPlayerWork.bFkokus	=	0xFF;		//国士無双
		_MEMCPY(gpsPlayerWork.byMchai, kokushi, kokushi.Length);	//MEMCPY(gpsPlayerWork.byMchai[0], kokushi, sizeof(kokushi));
		return 13;
	}
	if(cnt != 0){
		gpsPlayerWork.bFkokus		=	0xFF;	//国士無双
		gpsPlayerWork.byMchai[0]	=	x;
		return 1;
	}
	return 0;
}

public int chkfrt( /*MahJongRally * pMe,*/ byte x )
{
	byte[]	p;
	int i, p0= 0;

	i	=	gpsPlayerWork.byShcnt;
	p	=	gpsPlayerWork.bySthai;
	while(i-- != 0) {
		if(x==p[p0++]) {
			return 1;
		}
	}
	return 0;
}

/*	テンパイチェック	*/
public PLST[]	chktnp_m5_pailist = new PLST [15];//0505mt
public bool chktnp_m5 ( /*MahJongRally * pMe*/ ) // 1995.4.25, 5.2
{
	int		n;
	byte[]	p;	int	p0= 0;
//0505mt	PLST	pailist[]= new PLST [15];

	for( int i= 0; i< 15; i++)
		chktnp_m5_pailist[i].clear();	//0505mt pailist[i]= new PLST();
	gpsPlayerWork.bFkokus	=	0x00;
	gpsPlayerWork.byTenpai	=	0;
	setcnt_plst();
	newlst_plst( chktnp_m5_pailist);//0505mt

	//聴牌チェック
	if( (n= jtnp_jtnp( chktnp_m5_pailist, gpsPlayerWork.byMchai)) == 0 && ((gpsPlayerWork.byFhcnt != 0) ||	//0505mt	//!(n=jtnp_jtnp( pailist[0], gpsPlayerWork.byMchai))
		//チートイテンパイチェック
		(n= jtnp7_jtnp( chktnp_m5_pailist, gpsPlayerWork.byMchai)) == 0 && (n=chkkokushi()) == 0)) {		//0505mt

		return (false);
	}
	gpsPlayerWork.byTenpai	=	(byte)n;
	p	=	gpsPlayerWork.byMchai;
	while(n-- != 0) {
		if(chkfrt( p[p0++]) != 0) {
			gpsPlayerWork.bFfurit	=	0xFF;		//フリテン
			break;
		}
	}
	return (true);
}

/*****************************
	ピンチチェック
*****************************/
public bool chkpin_m5 ( /*MahJongRally * pMe*/ )/*1995.6.2, 6.8*/
{
	int		i;
	int		iDlgID;
	int		iOrder = 0;	//w4
	byte	ucPai;
	byte	bflag;
	byte	st0;//st1;
	int		Order_back;

	Order_back = Order;

	Order = Odrbuf;
	setwp_m2(Order);
	st0 = gpsPlayerWork.bFippat;

	/* BUGFIX 1996.5.30. */
	if (( Status & (byte)ST.NAKI ) == 0)
		bflag = 4;
	else {
	/*** ナキのビットが立っている時 ***/
		/*** 一発ルールが無しなら ***/
		if ( Rultbl[ (int)RL.IPPAT ] == 0 )
			bflag = 3;
		else {
		/*** 一発ルールが有りなら ***/
			iOrder = 3;
			bflag = 1;
		}
	}
	#if true //-*todo:変換有ってるかな？
	// Status &= ~ST_NAKI;
	Status = (byte)(Status & ~(byte)ST.NAKI);
	#endif //-*todo:変換有ってるかな？

	while ( bflag != 0 ) {

		if ( bflag == 1 )		/* 一発フラグを全員消す */
			bflag = 3;

		if ( bflag == 3 ) {		/* 一発フラグを全員消す */
			for (i = 0; i < MJDefine.MAX_TABLE_MEMBER; i++) {
				gsPlayerWork[i].bFippat			=	0;
				gsPlayerWork[i].byIppatuClear	=	0;
			}
			bflag = 4;
		}
		if ( bflag == 4 ) {
			gpsPlayerWork.bFippat	=	st0;
			if ( st0 != 0 )		/* 一発である */
				bflag = 41;
			else				/* 一発ではない */
				bflag = 9;
		}
		if ( bflag == 41 ) {					/* リーチが掛けられる時 */
			ResultSetRichi( Order);				/*	リーチフラグセット	*/

//			Richi++;//0603 No.1090,1104
			Richi= 0;		//リーチしてる人を数える//0603 No.1090,1104
			for (i = 0; i < MJDefine.MAX_TABLE_MEMBER; i++)//0603 No.1090,1104
				if(( gMJKResult.sMemResult[i].wFlag & (byte)RESF.RICH)!= 0)	/*	リーチした	*///0603 No.1090,1104
					Richi++;//0603 No.1090,1104

			gpsTableData.byRibo++;				/* １０００点引く */
			gpsTableData.sMemData[Order].byRibo_stack++;
			if(gpsTableData.byRibo> 99)	gpsTableData.byRibo = 99;
#if	Rule_2P
			gpsTableData.sMemData[Order].nPoint		-=	MJDefine.ReachPoint;
			gpsTableData.sMemData[Order].nOldPoint	-=	MJDefine.ReachPoint;
#else
			gpsTableData.sMemData[Order].nPoint	-=	10;
#endif

			gpsPlayerWork.bFrich	=	0xff;	/* １巡目ならダブリーフラグをたてる */
			if ( (Status & (byte)ST.ONE_JUN) == 0 )
				gpsPlayerWork.bFwrich	=	0xff;

			bflag = 5;
		}
		if ( bflag == 5 ) {		/* リー棒の表示 */
			//://disp_ribou ( Order );
			SubMj.RiboDispFlg[Order] = 1;
			//://te_prt_all( 0 );
			if (Order == (byte)MJ_POS.A) {
				//://PadShock(10);
			}
			if (Richi < 4 )
				bflag = 51;
			else {				/* リーチが４人なら流れる */
				/* 四人リーチのとき最後のリーチを書けた人以外の人(67) */
				#if true //-*todo:
				// MjDialog(  OtherOne( Order ), Order, DIAL_TO_4CYA_REACH );
				MjDialog(  (int)OtherOne( Order ), Order, (int)DIAL.TO_4CYA_REACH );
				#endif //-*todo:
				Pinch = (byte)SP.FOUR_RIC;
				return (true);
			}
		}
		if ( bflag == 51 ) {
			if ( gpsPlayerWork.bFwrich == 0 )	/* ダブリーではない */
				bflag = 52;
			else {					/* ダブリーである */
				/* ダブリーをされたとき(30) */
				#if true //-*todo:
				// MjDialog(  OtherOne( Order ), Order, DIAL_TO_WREACH );
				MjDialog(  (int)OtherOne( Order ), Order, (int)DIAL.TO_WREACH );
				#endif //-*todo:
				bflag = 7;
			}
		}

		if( bflag == 52 ) {
			if ( SubMj.Lastric != 0 )
				bflag = 6;
			else {
				if ( Order != 0 )
					bflag = 7;
				else {
					/* ﾌﾟﾚｲﾔｰにリーチされたとき(31) */
				#if true //-*todo:
					// MjDialog(  OtherOne( Order ), Order, DIAL_TO_PLAY_WREACH );
					MjDialog(  (int)OtherOne( Order ), Order, (int)DIAL.TO_PLAY_WREACH );
				#endif //-*todo:
					bflag = 9;
				}
			}
		}
		if ( bflag == 6 ) {
			/* デフォルトは親の時追っかけ立直された */
			iDlgID	=	(int)DIAL.TO_KO_OIREACH;
			if ( SubMj.Lastric == gpsTableData.byOya)		/* 親の時 */
				bflag = 61;
			else {									/* 子の時 */
				iDlgID = (int)DIAL.TO_OYA_OIREACH;
				/* デフォルトは相手が親で追っかけ立直された */
				if ( Order != gpsTableData.byOya)
					/* 普通の追っかけ立直された */
					iDlgID = (int)DIAL.TO_OTHER_OIREACH;
			}
			MjDialog( SubMj.Lastric, Order, iDlgID );
			if ( Richi != 3 ) {
				bflag = 7;
			} else {
				iOrder	=	3;
				bflag = 62;
				while (--iOrder >=0) { //0603mt No.1104 // 3人目の立直で配列[-1]を参照する不具合 while (iOrder-- >=0) {
					if ( gsPlayerWork[iOrder].bFrich == 0 ) {
						/* デフォルトは親の時３人立直 */
						if ( iOrder != gpsTableData.byOya){
							/* 子の時３人立直 */
							iDlgID = (int)DIAL.D_3REACH_KO;
						}else{
							iDlgID = (int)DIAL.D_3REACH_OYA;
						}
						/* 他家３人にリーチされたとき(35～36) */
//						MjDialog(  iOrder, Order, iDlgID );
						break;
					}
				}
				bflag = 7;
			}
		}

		if( bflag == 7 ) {
			SubMj.Lastric = Order;
			bflag = 9;
		}

		/* 四風子連打判定 */
		if( bflag == 9 ) {
			/* １巡目ではない → 四風子連打は不成立 */
			if(( (byte)ST.ONE_JUN & Status ) == 0 ) {
#if	Rule_2P
				if(( byte )Bpcnt < 80 )
#else
				if(( BYTE )Bpcnt < 56 )
#endif
				{	/* まだ全員ツモっていない → 四風子連打は未成立 */
					bflag = 11;
				} else {			/* それ以外 → １巡目、全員が牌を切った */
					Status |= (byte)ST.ONE_JUN;
//m					ucPai	=	gsPlayerWork[0].bySthai[0];
//m					/* 風牌以外を切っているなら不成立 */
//m					if((ucPai >= 0x31 ) && (ucPai <= 0x34 ) )
//m						if(    ucPai == gsPlayerWork[1].bySthai[0]
//m							&& ucPai == gsPlayerWork[2].bySthai[0]
//m							&& ucPai == gsPlayerWork[3].bySthai[0]) {
//m								Pinch = SP_4FON;
//m								return (TRUE);
//m						}
				}
			}
			bflag = 11;
		}

		/* 四開槓判定 */
		if( bflag == 11 ) {
			/* ステータスフラグの四開槓ビットは立っている */
			if(( Status & (byte)ST.FOUR_KAN ) != 0 ) {
				bflag = 13;
			} else {
				/* 槓の数が４つ未満 */
				if( Kancnt < 4 )
					bflag = 13;
				/* 槓の数が４つ以上 */
				else {
					/* 四ｶﾝ流れ成立のとき(68) */
					MjDialog(  (int)Order , MJDefine.NONE, (int)DIAL.D_4KAIKAN );
					Pinch = (byte)SP.FOUR_KAN;
					return (true);
				}
			}
		}

		/* 流局時の判定 */
		if( bflag == 13 ) {
			if( Bpcnt + Kancnt < MJDefine.PAI_MAX )	/* 流局ではない */		//122x
				bflag = 0;
			else {							/* 流局 → 流し満貫判定 */
				iOrder	= 3;
				bflag = 14;
				//> 2006/03/26 流し満貫対策 => 流局扱いにする
				Pinch = (byte)SP.HOAN;		//0422
				//> 2006/03/26 流し満貫対策
			}
		}

		/* 流し満貫判定 */
		while( bflag == 14 ) {
			//Order = iOrder;
			setwp_m2(iOrder);

			/* 流し満貫チェック */
			if(jyaoch_jy()) {
				MjDialog(  iOrder , MJDefine.NONE, (int)DIAL.NAGASHIMAN );
				return (true);			/* 流し満貫成立 */
			} else {
				if(--iOrder < 0 ) {		/* 全員とも流し満貫ではない */
					Pinch = (byte)SP.HOAN;
					return (true);
				}
			}
		}
	}

	Order = (byte)Order_back;

	return (false);
}

/*****************************
	待ち牌と捨て牌の照合（ＣＯＭ用）
*****************************/
public bool	chkmoh_m5 ( /*MahJongRally * pMe*/ )/*1995.4.25*/
{
	int		iMachi, p0= 0;
	byte	ucPai;
	byte[]	pucMachi;

	iMachi = gpsPlayerWork.byTenpai;
	if ( iMachi == 0 )
		return (false);

	ucPai = gpsPlayerWork.byHkhai;
	pucMachi	=	gpsPlayerWork.byMchai;
	while (iMachi-- != 0) {
		if (ucPai == pucMachi[p0])
			return (true);

		++p0;		//++pucMachi;
	}
	return (false);
}

public bool chkron_m5 ( /*MahJongRally * pMe*/ )/*1995.6.1*/
{
	if ( gpsPlayerWork.bFfurit == 0 ) {
		gpsPlayerWork.byHkhai	=	Sthai;
		if (chkmoh_m5()) {
			Status |= (byte)ST.RON;
			if (!jyaku_jy ()) {
				gpsPlayerWork.bFfurit	=	0xFF;	//フリテン
				return (false);
			}
			return (true);
		}
	}
	return (false);
}
public byte	ronho_m5 ( int bakan_flg) {	return ronho_m5((byte)bakan_flg);	}
public byte ronho_m5 ( /*MahJongRally * pMe,*/ byte bakan_flg)/*1995.6.1*/
{
//://	BYTE	bflag;
	bool	fRon;
	byte	ret;
	int		ronho_m5_Order_bak= Order;

	Order = ronho_m5_iOrder;
	setwp_m2(Order);

	if( is_reentry == false ){		//0422
	#if true //-*todo:
		DebLog(( "ronho_m5:order="+Order+",ronorder="+Ronodr+",flag="+ronho_m5_bflag+",result="+gNakiResult[0]+":"+gNakiResult[1]+":"+gNakiResult[2]+":"+gNakiResult[3]));
	#endif //-*todo: 
	}

	while( ronho_m5_bflag != 0 ) {
		switch(ronho_m5_bflag) {
			case	1:
				Roncnt = 0;
				Hanetbl[0] = 0;
				Hanetbl[1] = 0;
				Hanetbl[2] = 0;
				Hanetbl[3] = 0;
				ronho_m5_iOrder = (byte)(Odrbuf + (4-1));
#if	Rule_2P
				ronho_m5_iOrder&= 1;
#else
				ronho_m5_iOrder&= 3;
#endif
				ronho_m5_bflag = 2;
				break;

			case	2:
				Order = ronho_m5_iOrder;
				setwp_m2(Order);

				/* 1996.6.6.KOKUSHI_CHANKAN作成挿入 国士チャンカン用 */
				fRon = false;
				if( bakan_flg != 0 ) {		/*ヤオチュウのアンカンから呼ばれた時*/
					if ( gpsPlayerWork.bFkokus != 0 )
						fRon = chkron_m5();		/*ロン上がり判定*/
				} else
					fRon = chkron_m5();/*ロン上がり判定*/

				/* 1996.6.6.KOKUSHI_CHANKAN作成挿入 国士チャンカン用 END */

#if true //-*todo:通信
//-*移植
					if( !fRon )
						/* ロンできない場合 */
						ronho_m5_bflag = 7;
					else {
						// ロンできる場合
						if( Order == game_player  ) {
							/* プレーヤーの場合 */
#if	Rule_2P
							//ロン禁止（プレイヤーのみロン禁止）
							if(( m_keepData.mah_limit_num== (int)MAH.LIM04)||( m_keepData.mah_limit_num== (int)MAH.LIM05 )){
								ronho_m5_bflag = 7;
							}else{
								ronho_m5_bflag = 3;
							}
#endif
						} else	/* COMの場合 */
							ronho_m5_bflag = 5;
					}

#else //-*todo:通信
				if( !IS_NETWORK_MODE ) {
					if( !fRon )
						/* ロンできない場合 */
						ronho_m5_bflag = 7;
					else {
						// ロンできる場合
						if( Order == game_player  ) {
							/* プレーヤーの場合 */
	#if	Rule_2P
							//ロン禁止（プレイヤーのみロン禁止）
							if(( myCanvas.mah_limit_num== MAH_LIM04)||( myCanvas.mah_limit_num== MAH_LIM05 )){
								ronho_m5_bflag = 7;
							}else{
								ronho_m5_bflag = 3;
							}
	#else
							ronho_m5_bflag = 3;
	#endif
						} else	/* COMの場合 */
							ronho_m5_bflag = 5;
					}
				} else {
					if( !fRon ) {
						// ロンできない場合
						ronho_m5_bflag = 7;
					} else {
						// ロンできる場合
						// プレイヤーについて
						if( Order == game_player ) {
							//> 2006/03/02 No.126 再接続時振聴チェック
							if(  is_reentry ) {
								ronho_m5_bflag = 6;
							} else {
							//< 2006/03/02 No.126 振聴チェック
								// 代打ちでツモ切りはロンをスルー
	#if	Rule_2P
								//ロン禁止（プレイヤーのみロン禁止）
								if(( m_keepData.mah_limit_num== (int)MAH.LIM04)||( m_keepData.mah_limit_num== (int)MAH.LIM05 ))
									ronho_m5_bflag = 7;
								else
									ronho_m5_bflag = 3;
	#endif
								switch( m_DaiuchiFlag ) {
									case D_ONLINE_OPERATOR_MANUAL :
	#if	Rule_2P
										//ロン禁止（プレイヤーのみロン禁止）
										if(( myCanvas.mah_limit_num== MAH_LIM04)||( myCanvas.mah_limit_num== MAH_LIM05 ))
											ronho_m5_bflag = 7;
										else
											ronho_m5_bflag = 3;
	#else
										ronho_m5_bflag = 3;
	#endif
										break;
									case D_ONLINE_OPERATOR_PASS   : {
										//ronho_m5_bflag = 6;
										if( gsPlayerWork[ Order ].bFrich != 0 &&
											gsPlayerWork[ Order ].bFfurit != 0xff ) {
											ronho_m5_bflag = 5;
										} else
											ronho_m5_bflag = 6;
										break;
									}
									case D_ONLINE_OPERATOR_AI     : {
										//ronho_m5_bflag = 5;
										if( gsPlayerWork[ Order ].bFfurit != 0xff ) {
											ronho_m5_bflag = 5;
										} else
											ronho_m5_bflag = 6;
										break;
									}
								}
							}
						} else {
							ronho_m5_bflag = 6;
							if( (byte)gNetTable_NetChar[ Order ].Flag != (byte)0xFF ) {
								if( gNetTable_NetChar[ Order ].AIFlag == 1 )
									ronho_m5_bflag = 5;

								if( gNetTable_NetChar[ Order ].AIFlag == 0 &&
									gsPlayerWork[ Order ].bFrich != 0 )
									ronho_m5_bflag = 5;

								//// プレイヤーが使用していて、AIフラグがオフならロンをスルー
								//if( gNetTable_NetChar[ Order ].AIFlag == 0 &&
								//	gsPlayerWork[ Order ].bFrich == 0 )// 立直している時は和了る
								//{
								//	ronho_m5_bflag = 6;
								//}
							}
							//> 2006/03/30 COMがロン和了りしない
							else
								ronho_m5_bflag = 5;		//0422

							//< 2006/03/30 COMがロン和了りしない

							//> 2006/03/14 振聴和了り再発 No.126
							// 振聴は和了らない
							if( gsPlayerWork[ Order ].bFfurit == 0xff )
								ronho_m5_bflag = 6;
							//> 2006/03/14 振聴和了り再発 No.126
						}
					}
				}
#endif //-*todo:通信
				break;

			case	3:
				// 2006/02/15 reentry_m1_bflag = 6 で立てたフラグを消してしまうので、コメントアウトしてみる
				//if( Roncnt != 0 )
				//{
				//	clropt_opt();
				//}
				ronho_m5_bflag = 4;
				pshopt_opt( (int)OP.RON);
				keyin_m6_bflag = 0;
				break;

			case	4:
				//tamaki TRACE("ronho_m5 : Order = %d\n",Order);
				//tamaki TRACE("Optcnt = %d     Optstk[0] = %d\n",Optcnt,Optstk[0]);

				// ＰＬＡＹＥＲのナキのキー入力処理
				is_ronchk_menu_open = 1;
				ret	= keyin_m6();

				if( keyin_m6_bflag == 0xFF ) {
					if( ret != (byte)OP.RON ) {
						Optno[Order] = 0xFF;
						switch( ret ) {
							case (byte)OP.PON :
							case (byte)OP.CHI :
							case (byte)OP.KAN :
								player_pon_flg[Order] = ret;
							break;
						}

						//if(ret == OP_PON || ret == OP_CHI || ret == OP_KAN)
						//{
						//	player_pon_flg[Order] = ret;
						//}
						ronho_m5_bflag = 6;
					} else
						ronho_m5_bflag = 5;
				}
				break;

			case	5:
				if( (sGameData.byGameMode != MJDefine.GAMEMODE_NET_FREE)&&
					(sGameData.byGameMode != MJDefine.GAMEMODE_NET_RESERVE) )
					MjEvent( (byte)GEV.NAKI_RON,Order);

				Ronodr		=	Order;
				Hanetbl[( byte )Ronodr] = 0xff;
				Roncnt++;

				/* 行動結果にロンを設定	2005/11/07add */
				gNakiResult[Ronodr] = (byte)OP.RON;
				#if true //-*todo:
				DebLog(("SET RON NakiResult["+Ronodr+"]:"+gNakiResult[Ronodr]));
				#endif	//-*todo:
				ronho_m5_bflag = 7;
				break;

			case	6:
				gpsPlayerWork.bFfurit	=	0xff;		//フリテン
				ronho_m5_bflag = 7;
				break;

			case	7:
				ronho_m5_iOrder	= (byte)(Order + (4 - 1));
#if	Rule_2P
				ronho_m5_iOrder&= 1;
#else
				ronho_m5_iOrder&= 3;
#endif

				if( ronho_m5_iOrder != Odrbuf ) {
					ronho_m5_bflag = 2;
				} else {
					if( Roncnt == 0 ) {
						Order = (byte)ronho_m5_Order_bak;
						setwp_m2(Order);
						return (0);
					}
					ronho_m5_bflag = 8;
				}
				break;

			case	8:
				/** 三家和ではない **/
				if( Roncnt != 3 )
					ronho_m5_bflag = 9;
				/** 三家和である **/
				else {
					/* トリロンダブロン処理 1996.6.10._3CHAHO  & 1996.6.11.RULE_FIX 三家和(無･有)の修正 */
					if(( Rultbl[(int)RL.THREE_CHAHO] == 0 )||(Rultbl[(int)RL.TWO_CHAHO] == 0)){
						Pinch = (byte)SP.THREE_CHAN;
					}

					/* トリロンダブロン処理 1996.6.10._3CHAHO  & 1996.6.11.RULE_FIX 三家和(無･有)の修正 END */
					return (1);
				}
				goto case 9;
			case	9:
				/** 二家和ではない **/
				if( Roncnt != 2 )
					ronho_m5_bflag = 12;
				/** 二家和である **/
				else {
#if	Rule_2P
					ronho_m5_iOrder	=	1;
#else
					ronho_m5_iOrder	=	3;
#endif
					ronho_m5_bflag = 10;
				}
				break;

			/** 二家和の判定 **/
			case	10:
				if( ronho_m5_iOrder == Ronodr )
					ronho_m5_bflag = 11;
				else {
					if( Hanetbl[ronho_m5_iOrder] == 0 )
						ronho_m5_bflag = 11;
					else
						ronho_m5_bflag = 12;
				}
				break;

			case	11:
				if( --ronho_m5_iOrder >= 0 )
					ronho_m5_bflag = 10;
				else
					ronho_m5_bflag = 12;
				break;

			/** 上がりの判定 **/
			case	12:
				Order = Ronodr;
				setwp_m2(Order);
				jyaku_jy ();	//tamaki 200511/18 del
				ronho_m5_bflag = 0;
				return 1;
		}

		Order = (byte)ronho_m5_Order_bak;
		setwp_m2(Order);
		return(2);
	}
	return(2);	//warning
}
/**************************************END OF FILE**********************************************/

//-*********************mjm5.j
}
