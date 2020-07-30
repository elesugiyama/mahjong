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
// Mjm6.j
//-*****************
public partial class MahjongBase : SceneBase {
/*
**		麻雀大会３
**		鳴きの処理
*/

//#include "MahJongRally.h"								// Module interface definitions

//extern PRIVATE	BOOL j4kan_m4 (/*MahJongRally * pMe*/);/*1995.2.6*/

//extern SHORT	NakiCommandCatch(/*MahJongRally * pMe,*/ BYTE iOrder);

// #define	ClearIppatuMem()		_byClearIppatu = 0xFF
// #define	GetIppatuClear()		(_byClearIppatu)
// #define	SetIppatuClear(iOrder)	_byClearIppatu = (BYTE)iOrder

/************************************************/
public bool	chkchi_m6 ( /*MahJongRally * pMe*/ )/*1995.9.6*/
{
	short[] Chi_h1 = { -2, -1, 1 };		//[3]
	short[] Chi_h2 = { -1, 1, 2 };		//[3]
	byte[]	Chi_min = { 3, 2, 1 };		//[3]
	byte[]	Chi_max = { 10, 9, 8 };		//[3]

	byte	bflag;
	byte	ucPai = 0;	//w4
	int		iChipos;
	int		iThcnt = 0;	//w4

	Chiflg = 0;
#region UNITY_ORIGINAL
	iThcnt = gpsPlayerWork.byThcnt;
#endregion //-*UNITY_ORIGINAL	
	if (gpsPlayerWork.bFrich != 0 ) {
		return (false);
	} else if ( Thcnt < 4 ) {
		return (false);
	} else if (Sthai >= 0x30 ) {
		return (false);
	} else {
		iChipos	= 2;
		bflag = 1;
		while ( bflag != 8 ) {
			if ( bflag == 1 ) {
				ucPai	=	Sthai;
				ucPai	&=	0x0f;
				if (( ucPai < Chi_min[iChipos] ) || ( ucPai >= Chi_max[iChipos] )) {
					bflag = 6;
				}
				else {
					ucPai	= Sthai;
					ucPai	+= (byte)Chi_h1[iChipos];
					ucPai	&= 0x00ff;
					iThcnt	= Thcnt;
					iThcnt--;
					bflag = 2;
				}
			}
			while ( bflag == 2 ) {
				if ( ucPai == gpsPlayerWork.byTehai[iThcnt]) {
					bflag = 3;
//://@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
					if( iThcnt > 0 ){
						if( ucPai == gpsPlayerWork.byTehai[iThcnt-1] ){
							iThcnt--;
							bflag = 2;
						}
					}
//://@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
					break;
				}
				else {
					if ( --iThcnt < 0 ) {
						bflag = 6;
						break;
					}
				}
			}
			if ( bflag == 3 ) {
				Chips1[iChipos] = (byte)iThcnt;
				ucPai	= Sthai;
				ucPai	+= (byte)Chi_h2[iChipos];
				ucPai	&= 0x00ff;
				iThcnt	= 0;
				bflag = 4;
			}
			while ( bflag == 4 ) {
				if ( ucPai == gpsPlayerWork.byTehai[iThcnt]) {
					bflag = 5;
				}
				else {
					if ( ++iThcnt >= Thcnt ) {
						bflag = 6;
						break;
					}
				}
			}
			if ( bflag == 5 ) {
				Chips2[iChipos] = (byte)iThcnt;
				Chinum = (byte)iChipos;
				Chipsf[iChipos] = 0xFF;
				bflag = 7;
			}
			if ( bflag == 6 ) {
				Chipsf[iChipos] = 0x00;
				bflag = 7;
			}
			if ( bflag == 7 ) {
				Chiflg |= Chipsf[iChipos];
				if ( --iChipos >= 0 ) {
					bflag = 1;
				}
				else {
					bflag = 8;
					break;
				}
			}
		}
	}
	#if true //*todo:
	DebLog(("chiflg="+Chiflg+",["+Chipsf[0]+":"+Chipsf[1]+":"+Chipsf[2]+"]"));	//0422
	#endif //-*tpdp:
	return ( Chiflg != 0 );
}

public bool	chkpon_m6 ( /*MahJongRally * pMe*/ )
{
	byte	bflag;
	int		iThcnt;

	if ( gpsPlayerWork.bFrich != 0 ) {
		return (false);
	}

	iThcnt	= Thcnt;		//手牌数
#region UNITY_ORIGINAL
	iThcnt = gpsPlayerWork.byThcnt;
#endregion //-*UNITY_ORIGINAL
	if (Thcnt < 4 ) {
		return (false);
	}

	bflag = 1;
	while ( bflag != 0 ) {
		while ( bflag == 1 ) {
			iThcnt--;
			if ( Sthai == gpsPlayerWork.byTehai[iThcnt]) {
				bflag = 2;
			}
			else if (iThcnt >= 2 ) {
				bflag = 1;
			}
			else {
				return ( iThcnt == 2 );
			}
		}
		if ( bflag == 2 ) {
			iThcnt--;
			if ( Sthai != gpsPlayerWork.byTehai[iThcnt]) {
				return (false);
			}
			else {
				Ponpos = (byte)iThcnt;
				Ponflg++;
				if ( --iThcnt < 0 ) {
					bflag = 3;
				}
				else if ( Sthai != gpsPlayerWork.byTehai[iThcnt]) {
					bflag = 3;
				}
				else {
					Ponpos = (byte)iThcnt;
					Ponflg++;
					bflag = 3;
				}
			}
		}
		if ( bflag == 3 ) {
			//if(sGameData.byGameMode == GAMEMODE_WIRELESS && Order != 0 && Order <= link_max)
			//{
			//	//tamaki wlCommon.wlChildData[Order-1].wPonpos = Ponpos;
			//}
			#if true //-*todo:
			DebLog(("Ponflg = "+Ponflg));		//0422
			#endif //-*todo:
			return (true);
		}
	}

	// 本来ここには来ない
	return (true);
}
/************************************************
		鳴けるかどうかのチェック
	ポン	→	Ponflg = 1	Ponodr
	カン	→	Ponflg = 2	Ponodr
	チー	→	Chiflg = 0xff
	鳴けるのがプレイヤーの場合
	それぞれ pshopt_opt() を呼ぶ
************************************************/
// 未使用
//bool	chkopt_m6 ( /*MahJongRally * pMe*/ )/*1995.6.8*/
//{
//	BYTE	bflag;
//	int		iOrder;
//
//	Ponflg = 0;					//ポンフラグ初期化
//	Chiflg = 0;					//チーフラグ初期化
//	Ponodr = 0xff;				//ポンできる人初期化
//
//	clropt_opt();
//
//	if (Bpcnt + Kancnt >= PAI_MAX )		//122x
//		bflag = 0;
//	else {
//		bflag = 1;
//
//		iOrder = Odrbuf + 1;
//		iOrder &= 3;
//
//		/** Orderをｲﾝｸﾘﾒﾝﾄしてポンできるかをチェックするループ **/
//		while ( bflag == 1 ) {
//			//Order = iOrder;
//			setwp_m2( Order);
//
//			if(chkpon_m6()) {
//				/** 出来る **/
//				/** ..2 ..**/
//				Ponodr = iOrder;			//ポンできる人を設定
//
//				//iOrder = Odrbuf + 1;
//				//iOrder &= 3;
//				//if ( Ponodr == iOrder )
//				//{
//				//	chkchi_m6();
//				//}
//
//				//プレーヤーの場合　オプションに追加
//				if(gpsPlayerWork.byPlflg != 0) {
//					pshopt_opt( OP_PON);
//
//					if(Ponflg == 2)
//						pshopt_opt( OP_KAN);
//				}
//				break;
//			} else {
//				/** 出来ない **/
//				iOrder = (iOrder + 1);
//				iOrder &= 3;
//
//				if( iOrder == Odrbuf )
//					/** 一周したら終わり **/
//					break;
//			}
//		}
//	}
//	/** ..3 **/
//
//	//下家のチーチェック
//	iOrder	= Odrbuf + 1;
//	iOrder &= 3;
//	//Order = iOrder;
//
//	setwp_m2( Order);
//
//	if(chkchi_m6()) {
//		//プレーヤーの場合　オプションに追加
//		if(gpsPlayerWork.byPlflg != 0)
//			pshopt_opt( OP_CHI);
//	}
//
//	return (TRUE);
//}

/************************************************
		鳴けるかどうかのチェック
	ポン	→	Ponflg = 1	Ponodr
	カン	→	Ponflg = 2	Ponodr
	チー	→	Chiflg = 0xff
	鳴けるのがプレイヤーの場合
	それぞれ pshopt_opt() を呼ぶ
************************************************/
public bool	chknaki_m6 ( /*MahJongRally * pMe*/ )/*1995.6.8*/
{
	int		iOrder;
	int		i;
#if	Rule_2P
	int		_mask= 1;
#else
	int		_mask= 3;
#endif

	Ponflg = 0;					//ポンフラグ初期化
	Chiflg = 0;					//チーフラグ初期化
	Ponodr = 0xff;				//ポンできる人初期化

	clropt_opt();

	if (Bpcnt + Kancnt >= MJDefine.PAI_MAX )		//122x
		return(true);

	//> 2006/03/17 不具合No.191-2 .. 再現性は取れていないのですが、明らかに間違いなので修正
	// ポン出来ないにもかかわらずポン出来るようになってしまう
//	for(i=0;i<3;i++)
#if	Rule_2P
	for( i= 0; i< 1; i++)
#else
	for( i= 0; i< 4; i++)		//0422
#endif
	//< 2006/03/17 不具合No.191
	{
		/* ポン･チー判定テーブル初期化 */
		gPonChk[i]=0x00;
		gChiChk[i]=0x00;

		// 2006.2.19 No115
		gKanChk[i]=0x00;
		// 2006.2.19 No115
	}

	/* 下家からポン確認 */
	iOrder = Odrbuf + 1;
	iOrder &= _mask;

#if	Rule_2P
	for( i= 0; i< 1; i++)
#else
	for( i= 0; i< 3; i++)
#endif
	{
		//> No127. 2006/02/25 順番が飛ばされる。
		if( iOrder == game_player ) {
			// チェック対象がプレイヤーで且鳴き無しなら次へ
			if( gMyTable_NakiNashi == (byte)D_OPTION_NAKINASHI.ON ) {
				#if true	//-*todo:
				DebLog(( "player="+iOrder+":pon nashi"));
				#endif

				/* 家を進める */
				iOrder = (iOrder + 1);
				iOrder &= _mask;

				continue;
			}
		} else
			// チェック対象がプレイヤー以外でかつ切断時操作がツモ切りなら次へ
			//> 2006/03/16 No.186 通信対戦サスペンドからCOM対戦へ移ると順番飛ばしが発生
			#if false //-*todo:
			if( IS_NETWORK_MODE ){ 		//0422
			//> 2006/03/16 No.186
				if( gNetTable_NetChar[ iOrder ].Flag == 0x00 ){
					if( gNetTable_NetChar[iOrder].AIFlag == 0x00 ) {
						DebLog(( "ai="+iOrder+":pon nashi" ));

						/* 家を進める */
						iOrder = (iOrder + 1);
						iOrder &= _mask;

						continue;
					}
				}
			}
			#endif //-*todo:
		//< No127. 2006/02/25 順番が飛ばされる。

		/* アクティブプレーヤーを更新 */
		setwp_m2( iOrder);

		/* ポン確認 */
//		if( is_reentry == false )		//0422
//		{
//			DebLog(("chkpon order=%d,flag=%d,ai=%d",iOrder,gNetTable_NetChar[iOrder].Flag,gNetTable_NetChar[iOrder].AIFlag));	//0422
//		}
		if(chkpon_m6()) {	/* ポンできる場合 */
			Ponodr = (byte)iOrder;		//ポンできる人を設定
			gPonChk[iOrder]=Ponflg;		//ポンテーブル設定
			#if true //-*todo:
			DebLog(("chkpon is true"));
			#endif //-*todo:

			// 2006.2.19 No115
			if(Ponflg == 2)
				gKanChk[iOrder]=Ponflg;
			// 2006.2.19 No115

			//プレーヤーの場合かつ、オートツモでなければオプションに追加
			if( iOrder == game_player ) {
				//> No127. 2006/02/25 順番が飛ばされる。
				{
					pshopt_opt( (int)OP.PON);
					// 2006.2.19 No115
					// カンの場合。
					if( Ponflg == 2 )
						// 2006/03/08 5個目の槓が出来てかつ、落ちるのを修正
						if( j4kan_m4() )
							pshopt_opt( (int)OP.KAN);
					// 2006.2.19 No115
				}
				//< No127. 2006/02/25 順番が飛ばされる。
			}
		}

		/* 家を進める */
		iOrder = (iOrder + 1);
		iOrder &= _mask;
	}

	/* 下家のチー確認 */
	iOrder = Odrbuf + 1;
	iOrder &= _mask;

	//> 2006/03/27 No.198			//0422	以下関数内変更
	//> No127. 順番が飛ばされる。
	if( iOrder == game_player ) {
		// チェック対象がプレイヤーで且鳴き無しなら次へ
		if( gMyTable_NakiNashi == (byte)D_OPTION_NAKINASHI.ON ) {
			#if true //-*todo:
			DebLog(( "player="+iOrder+":chi nashi" ));
			#endif //-*todo:
			return (true);
		}

		#if false //-*todo:通信
		if( IS_NETWORK_MODE && gDaiuchiFlag == D_ONLINE_OPERATOR_PASS ) {
			DebLog(( "ai="+iOrder+":chi nashi" ));
			return (TRUE);
		}
		#endif //-*todo:通信
	} else
		// チェック対象がプレイヤー以外でかつ切断時操作がツモ切りなら次へ
		//> 2006/03/16 No.186 通信対戦サスペンドからCOM対戦へ移ると順番飛ばしが発生
		#if false //-*todo:通信
		if( IS_NETWORK_MODE ){
		//> 2006/03/16 No.186
			if( gNetTable_NetChar[ iOrder ].Flag == 0x00 ){
				if( gNetTable_NetChar[iOrder].AIFlag == 0x00 ) {
					DebLog(( "player="+iOrder+":chi nashi" ));
					return (TRUE);
				}
			}
		}
		#endif //-*todo:通信
	//< 2006/03/27 No.198

	/* アクティブプレーヤーを更新 */
	setwp_m2(iOrder);

	/* チー確認 */
	if( is_reentry == false ){
		#if false //-*todo:通信
		DebLog(("chkchi order="+iOrder+",flag="+SubMj.gNetTable_NetChar[iOrder].Flag+",ai="+gNetTable_NetChar[iOrder].AIFlag));
		#endif //-*todo:通信
		DebLog("is_reentry:todo:通信");
	}
	if(chkchi_m6()) {
		#if true //-*todo:
		DebLog(("chkchi is true"));
		#endif //-*todo:
		Chiodr = (byte)(iOrder);	//チーできる人を設定	//0422
		gChiChk[iOrder]=Chiflg;		//チーテーブル設定

		//プレーヤーの場合　オプションに追加
		if( Chiodr == game_player )
			//> No127. 2006/02/25 順番が飛ばされる。
			pshopt_opt((int)OP.CHI);
			//< No127. 2006/02/25 順番が飛ばされる。
	}
	return (true);
}

/*****************************
	ＰＬＡＹＥＲのナキのキー入力処理
*****************************/
public byte keyin_m6 ( /*MahJongRally * pMe*/ )/* BUG_FIX 1996.5.8. */
{
	//://BYTE	bflag;
	//://ULONG	key;
	short	cmd;
	//tamaki ULONG	pl_cmd;
//	BYTE	iOptNo;

	switch(keyin_m6_bflag) {
		case	0:
			/* OptnoがOP_TSUMOでない */
			//20051205 del
			//if (Optno[Order] != OP_TSUMO )
			//{
			//	keyin_m6_bflag = 0xff;
			//	TRACE("Optcnt = %d  Optstk = %d\n",Optcnt,Optstk[0]);
			//	iOptNo = Optno[Order];
			//	return (iOptNo);
			//}
			disp_sutehai_flush(  true ,false);		//(  ON ,FALSE);	/* 牌の点滅 */
			keyin_m6_bflag = 2;
			//tamaki wlCommon.wlNaki = Order;
			//tamaki wlCommon.wlSute = NONE;
			//tamaki wlCommon.seq_cnt++;
			break;

		case	2:
			cmd = NakiCommandCatch( Order);
//			cmd = NakiSel();
			if(cmd != -1) {
				//tamaki wlCommon.wlNaki = NONE;
				disp_sutehai_flush(  false ,false);	//(  OFF ,FALSE);
				keyin_m6_bflag = 0xff;				//://行動決定
				switch(cmd){
					case (short)OP.TAPAI:
						return (byte)(OP.NONE);
					case (short)OP.CHI:
						Chinum = NakiSelNo;
						return (byte)(OP.CHI);
					case (short)OP.PON:
						return (byte)(OP.PON);
					case (short)OP.KAN:
						return (byte)(OP.KAN);
					case (short)OP.RON:
						return (byte)(OP.RON);
					default:
						keyin_m6_bflag = 2;
						return (byte)(OP.NONE);					//://とりあえず何もしない
//						break;
				}

// 意図がわからないので削除
//				hai_csr = gpsPlayerWork.byThcnt;	//20051117 add 牌カーソルを初期値にする
			}
			break;
	}
	// ここには来ない
	return (byte)OP.NONE;
}

public int naki_Sm6 (/*MahJongRally * pMe,*/ int iPon, int iChi )
{	//0422	関数内大幅に変更
//://戻り値がNONEならプレーヤーの入力待ち中
	byte byRet = 0;
	if ( gpsPlayerWork.byPlflg == 0 )
	//> 2006/03/24 切断時鳴き無しにも関わらず鳴きが入る。
//	&&( IS_NETWORK_MODE && gDaiuchiFlag == D_ONLINE_OPERATOR_AI ) )	// 代打ちのAIﾌﾗｸﾞを追加。2005/2/5
	{
		//------------------------------
		// プレイヤー以外の処理
		//------------------------------
		#if true //-*todo:通信
			byRet = tfuro_think (iPon, iChi);		/*スタンドアローン時は全てこちら*/
		#else //-*todo:通信
		if( IS_NETWORK_MODE ) {
			// 再接続中は副露思考無し
			if( is_reentry == false && Order == 0x01 && gNetTable_NetChar[Order].AIFlag == 0x01 )		//( is_reentry == false && Order == gNetTable_NetChar[Order].AIFlag == 0x01 )
				byRet = tfuro_think(iPon, iChi);
		} else{
			byRet = tfuro_think (iPon, iChi);		/*スタンドアローン時は全てこちら*/
		}
		//byRet = tfuro_think (iPon, iChi);

		ITRACE(("tfuro_think byRet = %d",byRet));
		#endif //-*todo:通信
		if(byRet == (byte)OP.NONE)
			byRet	=	0x00;
	} else {
	//> 2006/03/24 切断時鳴き無しにも関わらず鳴きが入る。
	#if true //-*todo:
		DebLog(("naki_Sm6: "+player_pon_flg[Order]+" :P="+iPon+" : C="+iChi+" : Optno="+Optno[Order]));
	#endif //-*todo:
//-*SUGI_DEB***************************
#if SUGI_DEB //-*todo:注デバッグ中	
// gMyTable_NakiNashi = (byte)D_OPTION_NAKINASHI.ON;	//-*鳴き無し
#endif //-*todo:注デバッグ中
//-****************************SUGI_DEB
		//------------------------------
		// プレイヤーの処理
		//------------------------------
		if( Order == game_player && gMyTable_NakiNashi == (byte)D_OPTION_NAKINASHI.ON )
			// 鳴き無しならば何もさせない
			return 0x00;
		#if false //-*todo:通信
		//> 2006/03/24 切断時鳴き無しにも関わらず鳴きが入る。
		if( IS_NETWORK_MODE && gDaiuchiFlag == D_ONLINE_OPERATOR_AI ) {
			byRet = tfuro_think (iPon, iChi);
			ITRACE(("tfuro_think byRet = %d",byRet));

			if(byRet == OP_NONE)
				byRet	=	0x00;

			return byRet;
		}
		#endif //-*todo:通信
		//> 2006/03/24 切断時鳴き無しにも関わらず鳴きが入る。

		if(player_pon_flg[Order] != 0) {
			byRet = player_pon_flg[Order];
			player_pon_flg[Order] = 0;
			return(byRet);
		}
		byRet	=	keyin_m6();

		if( keyin_m6_bflag == 0xFF ) {
			if(byRet == (byte)OP.NONE)
				byRet = 0;
		} else {
			byRet = MJDefine.NONE;
			// 2006.2.19 No115
			if( Optcnt == 0 && gKanChk[game_player] != 0)
				byRet = 0;
			if( Optcnt == 0 && gPonChk[game_player] != 0)
				byRet = 0;
			if( Optcnt == 0 && gChiChk[game_player] != 0)
				byRet = 0;

			if( Optcnt != 0 ) {
				if( gMyTable_NakiNashi != 0 && gKanChk[game_player] != 0)
					byRet = 0;
				if( gMyTable_NakiNashi != 0 && gPonChk[game_player] != 0)
					byRet = 0;
				if( gMyTable_NakiNashi != 0 && gChiChk[game_player] != 0)
					byRet = 0;
			}
			// 2006.2.19 No115
		}
	}
	return ((int)byRet);
}

public void SetIppatuInfo(/*MahJongRally * pMe*/)
{
	int		i;
	//tamaki USHORT	uiTmp;

	/*	打牌後のメッセージの為 */
	for (i = MJDefine.MAX_TABLE_MEMBER-1; i >= 0; i--) {
		if ( Rultbl[ (int)RL.IPPAT ] != 0 ) {				/*	鳴きで一発が消える	*/
			if ( SubMj._byClearIppatu == 0xFF && gsPlayerWork[i].bFippat != 0) {	//0422
				SubMj._byClearIppatu = (byte)(i);	//SetIppatuClear(i);			//0422
//				break;
			}
//			gsPlayerWork[i].byIppatuClear	=	gsPlayerWork[i].bFippat;
		}
		gsPlayerWork[i].bFippat			=	0;
	}
}

public void furosub_m6 (/*MahJongRally * pMe,*/ byte uiPai)/* 1995.9.8*/
{
	int		iFhcnt;
	int		iOrder;

	/** フーロのカウントを増やす **/
	iFhcnt = gpsPlayerWork.byFhcnt;
	iFhcnt++;
	gpsPlayerWork.byFhcnt	=	(byte)iFhcnt;
	/** フーロした牌のコードをバッファに入れる **/
	gpsPlayerWork.byFrhai[iFhcnt - 1] = uiPai;
	/** 誰から鳴いたか（どの牌を鳴いたか）をバッファへ **/
	iOrder = Order + 4;
	iOrder -= Odrbuf;
	iOrder &= 3;
	gpsPlayerWork.byFrpos[iFhcnt - 1] = (byte)iOrder;
	#if false //-*todo:
	ITRACE(("NAKI %d %d %02X\n", iFhcnt, iOrder, uiPai));
//	TRACE("%d %d ", Order, Odrbuf);
	#endif //-*todo:
#if	_DEBUG_EMU
DebHaiDisp(uiPai);
#endif	//_DEBUG_EMU
//TRACE(" を鳴いた\n");

	/** 門前フラグをたてる **/
	gpsPlayerWork.bFmenzen	=	0xFF;
	/** ステータスフラグをたてる **/
	Status |= ((byte)ST.ONE_JUN | (byte)ST.NAKI);

	/** 鳴かれた側の流し満貫を消す **/
	setwp_m2( Odrbuf);
	gpsPlayerWork.bFnagas	=	0xFF;
	/** 手牌ポインタを鳴いた面子に戻す **/
	setwp_m2( Order);

/*	1999/09/27	M.Fukuda	1発は泣いた時点で消える	*/
	SetIppatuInfo();
}

public void chkpao_m6 ( /*MahJongRally * pMe*/ )
{
	int		iFhcnt;
	byte[]	pFrhai;
	int	p0= 0;
	byte	ucPai;
	bool[]	fChkPao= new bool [7];

	if ( hotpai < 0x31 ) {
		return;
	}
	for (iFhcnt = 0; iFhcnt < 7; iFhcnt++) {
		fChkPao[iFhcnt]	=	false;
	}

	iFhcnt	= gpsPlayerWork.byFhcnt;
	pFrhai	=	gpsPlayerWork.byFrhai;
	while (iFhcnt-- != 0) {
		ucPai	=	pFrhai[p0++];		//	*pFrhai++;
		ucPai &= 0x3f;
		if (ucPai >= 0x31 && ucPai <= 0x37) {
			fChkPao[ucPai - 0x31]	=	true;
		}
	}
	if ( hotpai >= 0x35 ) {
		if (fChkPao[4] && fChkPao[5] && fChkPao[6]) {
			SubMj.Pao3 = Odrbuf;
		}
	}
	else if (hotpai >= 0x31) {
		if (fChkPao[0] && fChkPao[1] && fChkPao[2] && fChkPao[3]) {
			SubMj.Pao4 = Odrbuf;
		}
	}
}

/*****************************
	ポン（全員）の処理
*****************************/
public void pon_m6 ( /*MahJongRally * pMe*/ )
{
	int		i;
	int		iCnt;

	SubMj._byClearIppatu = 0xFF;//ClearIppatuMem();	//0422
	#if false //-*todo:
	ITRACE(("PON %d %d %02X\n", Order, Ponpos, hotpai));
	#endif //-*todo:
	MjEvent( (byte)GEV.NAKI_PON,Order);
	iCnt	=	0;
	for (i = 0; i < Thcnt; i++) {
		if (gpsPlayerWork.byTehai[i] == hotpai) {
			gpsPlayerWork.byTehai[i] = 0x40;
			++iCnt;
			if (iCnt >= 2) {
				break;
			}
		}
	}
	thsort_m2();

	Thcnt	-=	3;
	gpsPlayerWork.byHkhai	=	gpsPlayerWork.byTehai[Thcnt];
	gpsPlayerWork.byThcnt	=	Thcnt;

	erssh_m3();	/* 捨て牌を消す */
	decpc_pcnt ( hotpai);
	decpc_pcnt ( hotpai);
	furosub_m6 ((byte)( hotpai | 0x40));
	//://MjSoundSEPlay( SENO_NAKI );
	//://te_prt_all( 1 );	/* フーロ後の表示・手牌カウント＋１ */
	gpsPlayerWork.byTkhai = 0xFF;			//://手牌に表示のときにツモ牌も表示する
	chkpao_m6();

	if ( Check_Dora(  hotpai ) == true ) {	/* 鳴いた牌がドラの時 */
		/** ドラをポンした人(0,1) **/
		MjDialog(  Order, MJDefine.NONE, (byte)DIAL.DORAPON + ( ( Bp_now / 2 ) % 2 ) );
		/** ドラをないた人へ、又はドラをなかした人へ（鳴かせた者以外が）喋る(2,3) **/
		//tamaki MjDialog(  OtherTwo( Order, Odrbuf ), NONE, DIAL_TO_DORAPON + ( mj_rand() % 2 ) );
		MjDialog(  OtherTwo( Order, Odrbuf ), MJDefine.NONE, (byte)DIAL.TO_DORAPON + mj_getrand(2) );
	}
	else {
		Dialog_Furo( SubMj._byClearIppatu);		/* フーロの個数によって喋る */		//0422
	}
}

/*****************************
	チー（全員）の処理（深い）
*****************************/
public void chi_s (/*MahJongRally * pMe,*/ int iPos)/*1995.9.8*/
{
	byte	ucTmp	=	gpsPlayerWork.byTehai[iPos];
	decpc_pcnt ( ucTmp);
	gpsPlayerWork.byTehai[iPos]	=	0x40;
}

/*****************************
	チー（全員）の処理（浅い）
*****************************/
public void chi_m6 ( /*MahJongRally * pMe*/ )/*1995.9.8*/
{
	byte	ucPai;
	//tamaki int		iFhcnt;

	//if(sGameData.byGameMode == GAMEMODE_WIRELESS && Order != 0 && Order <= link_max){
	//	//tamaki MI_CpuCopy8(	wlCommon.wlChildData[Order-1].Chips1,	Chips1,		sizeof(Chips1)	);
	//	//tamaki MI_CpuCopy8(	wlCommon.wlChildData[Order-1].Chips2,	Chips2,		sizeof(Chips2)	);
	//}
	SubMj._byClearIppatu = 0xFF;//ClearIppatuMem();	//0422
	MjEvent( (byte)GEV.NAKI_CHI,Order);
	chi_s( Chips1[Chinum]);
	chi_s( Chips2[Chinum]);
	thsort_m2();

	Thcnt	-=	3;
	gpsPlayerWork.byHkhai	=	gpsPlayerWork.byTehai[Thcnt];
	gpsPlayerWork.byThcnt	=	Thcnt;

	/* ここでの手牌表示はcut */
	erssh_m3();	/* 捨て牌を消す */
	ucPai	=	(byte)(hotpai + Chinum);
	ucPai -= ( 1 + 1 );
	furosub_m6( ucPai);			/* フーロによる数値変化 */
	gpsPlayerWork.byFrpos[gpsPlayerWork.byFhcnt - 1] = (byte)(2 - Chinum);

	gpsPlayerWork.byTkhai = 0xFF;			//://手牌に表示のときにツモ牌も表示する

	Dialog_Furo( SubMj._byClearIppatu);			//0422	/* 1996.7.5.DIALOG フーロの個数によって喋る */
}

/*****************************
	カン（すべて）の共通処理
		 会話用引き数
		 mode = 0：暗カン
			  = 1：明カン
			  = 2：チャカン
*****************************/
public bool	kansub_m6 ( /*MahJongRally * pMe,*/ byte mode )/*1995.6.1*/
{
	int		iKanCnt;

	SubMj._byClearIppatu = 0xFF;//ClearIppatuMem();	//0422
	/* 1996.8.11.DIAL_FIX */
	/* 鳴いた牌がドラの時 */
	if ( Check_Dora(  hotpai ) == true ){
		/* ドラのカンをされたとき */
		MjDialog(  OtherOne( Order ), MJDefine.NONE, (byte)DIAL.DORAKAN );
	}
	else if( mode == 1 ){					/* 明カンのみ */
		Dialog_Furo( (SubMj._byClearIppatu));						/* フーロの個数によって喋る */
	}

	Status |= (byte)ST.ONE_JUN;/* BUG_FIX 1996.6.8. */

	Kancnt++;					/* カンカウント＋１ */
//> 2006/04/15 DS依存バグNo.5
//●カンした時、ルールで槓ドラが無しであった場合でも、アルゴで使用している
// 「ドラを示す変数」の値を更新している
//  そのため、ＮＰＣは見えないはずの槓ドラをドラだと認識して行動している
//  // 影響範囲の大きい修正になるので、出来れば避けたい。
	// また、修正自体に変数・関数が追加されているのでどれくらい影響があるのか不明
	adddora_pcnt ( hotpai);
//> 2006/04/15 DS依存バグNo.5

	if (Rultbl[(int)RL.KAN] != 0) {
		++byDoraCnt;
		if (Rultbl[(int)RL.KANUR] != 0 && Rultbl[(int)RL.URA] != 0) {
			++byUraDoraCnt;
		}
		//> 2006/04/15 DS依存バグNo.5
		// adddora_pcnt (pMe, hotpai);
		//> 2006/04/15 DS依存バグNo.5
	}
	if ( Kancnt < 5 ) {
		iKanCnt	= gpsPlayerWork.byKancnt + 1;
		gpsPlayerWork.byKancnt	=	(byte)iKanCnt;
		if ( iKanCnt >= 4 ) {
			Status |= (byte)ST.FOUR_KAN;
		}
		chktnp_m5();
		Status &= (byte)~ST.RINFR;
		Status |= (byte)ST.RINSH;
		shuhai_m2();			/* 牌をツモる（王牌表示はここじゃないので注意） */
		return (true);
	}
	Pinch = (byte)SP.FOUR_KAN;
	return (false);
}

/*****************************
	ミンカン（全員）の処理
*****************************/
public void minkan_m6 ( /*MahJongRally * pMe*/ )
{
	int		i;
	bool	fRet;

	MjEvent( (byte)GEV.NAKI_KAN,Order);
	for (i = 0; i < Thcnt; i++) {
		if (gpsPlayerWork.byTehai[i] == hotpai) {
			gpsPlayerWork.byTehai[i]	=	0x40;
		}
	}
	thsort_m2();

	Thcnt	-=	3;
	gpsPlayerWork.byHkhai	= gpsPlayerWork.byTehai[Thcnt];
	gpsPlayerWork.byThcnt	=	Thcnt;

	erssh_m3();				/* 捨て牌を消す */
	clrpc_pcnt ( hotpai);
	furosub_m6((byte)( hotpai | 0xC0));

	gpsPlayerWork.byTkhai = 0;				//://手牌に表示のときにツモ牌は表示しない
	fRet	=	kansub_m6((byte)1 );		/* この中で嶺山牌をつもって表示 */
	disp_dora ((ushort) 1 );						/* 王牌のツモした時点（カンドラはオープンしてない） */
	if ( fRet ) {
		Status |= (byte)ST.RINFR;
	}
}

/*****************************
	鳴き（全員）の処理
*****************************/
#if DEBUG
static byte naki_m6_bflag_keep= 99;
#endif
public byte naki_m6_jdg( /*MahJongRally * pMe*/ )
{
#if DEBUG
if( naki_m6_bflag_keep!= naki_m6_bflag) {
	naki_m6_bflag_keep= naki_m6_bflag;
#if	__MJ_CHECK
Debug.Log("naki_m6_bflag 1: "+ (int)naki_m6_bflag);
#endif
}
#endif
	switch(naki_m6_bflag) {
		case	0:
//			Debug.out("naki_m6_jdg mode= "+ Integer.toString(naki_m6_bflag)+	",order= "+ Integer.toString(Order)+	",ponchk: "+ Integer.toString(gPonChk[0])+	", "+ Integer.toString(gPonChk[1])+		", "+ Integer.toString(gPonChk[2])+		", "+ Integer.toString(gPonChk[3])	);	//DebLog(( "naki_m6_jdg mode=%d,order=%d,ponchk:%d,%d,%d,%d",naki_m6_bflag,Order,gPonChk[0],gPonChk[1],gPonChk[2],gPonChk[3] ));

			/* ポンできる家の判定 */
			naki_m6_iNakiMode = 0;

			/*	ポン判定	*/
			if( Ponflg != 0 ) {
				/* ポンできる場合 */
				/** 捨て牌した次の者からチェック開始 **/
				naki_m6_iOrder = (byte)(Odrbuf + 1);
#if	Rule_2P
				naki_m6_iOrder&= 1;
#else
				naki_m6_iOrder&= 3;
#endif

				/** 捨て牌した者の下家以外がポンできる時 **/
				if( naki_m6_iOrder != Ponodr ) {
					/** それ以外がポンできるとき **/
					Order	=	Ponodr;
					setwp_m2( Order);
					#if false //-*todo:
					DebLog(("order change1 by pon:order=%d", Order));
					#endif

					/** ナキアルゴ又はプレイヤーナキ処理へ **/
					naki_m6_bflag	=	1;
					keyin_m6_bflag = 0;
					break;
				}
			}
			naki_m6_bflag	=	2;
			break;

		case	1:
//			Debug.out("1 naki_m6_jdg mode= "+ Integer.toString(naki_m6_bflag)+	",order= "+ Integer.toString(Order)	);	//DebLog(( "naki_m6_jdg mode=%d,order=%d,",naki_m6_bflag,Order ));
			/* ポンできる家が下家以外の場合 */
			naki_m6_iNakiMode = (byte)naki_Sm6(  Ponflg, 0);
			if( naki_m6_iNakiMode == MJDefine.NONE )
				return(MJDefine.NONE);

			/* 各家の行動保存 2005/11/07add*/
			if(gNakiResult[Order]==0)
				gNakiResult[Order]=naki_m6_iNakiMode;
//			else
//				DebLog(("RON already checked:order=%d",Order));

			//if( naki_m6_iNakiMode == 0 )
			////if( naki_m6_iNakiMode == OP_NONE )
			//{
			//	/** 鳴かないなら捨て牌した者の下家のナキチェックへ **/
			//	/** ただしポンチェックはしない **/
			//	Ponflg = 0;
			//}

			//ポンフラグは落として下家の確認へ
			Ponflg = 0;
			naki_m6_bflag	=	2;
			break;

		case	2:
//			Debug.out("2 naki_m6_jdg mode= "+ Integer.toString(naki_m6_bflag)+	",order= "+ Integer.toString(Order)	);	//DebLog(( "naki_m6_jdg mode=%d,order=%d,",naki_m6_bflag,Order ));

			/** 捨て牌した者の下家のナキチェック **/

			/* すでにポンする場合は処理せず */
			//ここでチーも確認するよう変更
			//if(naki_m6_iNakiMode == 0)
			{
#if	Rule_2P
				Order = (byte)((Odrbuf + 1) & 0x01);
#else
				Order = (byte)((Odrbuf + 1) & 0x03);
#endif
				setwp_m2( Order);
				#if true
				DebLog(("order change2 by pon:order="+Order));
				#endif

				if( Chiflg != 0 || Ponflg != 0) {
					/** 鳴けるならナキ処理を呼ぶ **/
					naki_m6_bflag	=	3;
					keyin_m6_bflag = 0;
					break;
				}
			}
			naki_m6_bflag	=	4;
			break;

		case	3:
//			Debug.out("naki_m6_jdg mode= "+ Integer.toString(naki_m6_bflag)+	",order= "+ Integer.toString(Order)+	",pon="+ Integer.toString(Ponflg)+	",chi="+ Integer.toString(Chiflg)	);	//DebLog(( "naki_m6_jdg mode=%d,order=%d,pon=%d,chi=%d",naki_m6_bflag,Order,Ponflg,Chiflg ));

			/* 下家が鳴ける場合 */
			naki_m6_iNakiMode = (byte)naki_Sm6( Ponflg, Chiflg);
			if( naki_m6_iNakiMode == MJDefine.NONE )
				return(MJDefine.NONE);

			/* 各家の行動保存 2005/11/07add*/
			if(gNakiResult[Order]==0){
				gNakiResult[Order]=naki_m6_iNakiMode;
			}else{
				#if true
				DebLog(("RON already checked:order="+Order));
				#endif
			}
			naki_m6_bflag	=	4;
			break;

		case	4:
//			Debug.out("naki_m6_jdg mode= "+ Integer.toString(naki_m6_bflag)+	",order= "+ Integer.toString(Order)	);	//DebLog(( "naki_m6_jdg mode=%d,order=%d,",naki_m6_bflag,Order ));
			return(0);
	}
	return(MJDefine.NONE);
}

/*****************************
	ナキの処理（本体）
*****************************/
public void naki_m6_main( /*MahJongRally * pMe*/ )
{
	if (naki_m6_iNakiMode != 0)
	{
		switch (naki_m6_iNakiMode)
		{
			case (byte)OP.CHI:							/* チーの時 */
			chi_m6();
			ResultSetNaki( Order);					/*	鳴いたフラグセット	*/
			break;

			case (byte)OP.KAN:							/* 明カンの時 */
			minkan_m6();						/* 明カンの時 */
			ResultSetNaki( Order);					/*	鳴いたフラグセット	*/
			break;

			case (byte)OP.PON:							/* ポンの時 */
			pon_m6();
			ResultSetNaki( Order);					/*	鳴いたフラグセット	*/
			break;

			default:
			#if false //-*todo:
			ITRACE(("NAKI ERROR %d \n", naki_m6_iNakiMode));
			#endif //-*todo:
			break;
		}
	}
	naki_m6_bflag	=	0xff;
}

public int chkpos_m6 ( /*MahJongRally * pMe*/ )/*1995.6.1, 6.7*/
{
	short	i;
	i = 0;
	while ( true ) {
		if ( hotpai == gpsPlayerWork.byTehai[i]) {
			break;
		}
		i++;
	}
	return (i);
}

/*****************************
	アンカン（全員）の処理
*****************************/
public byte ankan_m6 ( /*MahJongRally * pMe*/ )
{
	byte	ucPai;
	byte	ret;
	int		iTmp;
	int		iFhcnt;

	while(true) {
		switch(ankan_m6_bflag) {
			case	0:
				#if true //-*todo:
				DebLog(("ankan_m6 case0"));
				#endif //-*todo:
				MjEvent( (byte)GEV.NAKI_KAN,Order);

				ucPai	= gpsPlayerWork.byHkhai;
				iTmp	=	chkpos_m6();
				if (( byte )ucPai != hotpai ) {
					gpsPlayerWork.byTehai[iTmp]	=	ucPai;
					iTmp++;
				}
				gpsPlayerWork.byTehai[iTmp + 0]	=	0x38;
				gpsPlayerWork.byTehai[iTmp + 1]	=	0x38;
				gpsPlayerWork.byTehai[iTmp + 2]	=	0x38;
				thsort_m2();						/* ソート */
				Thcnt -= 3;							/* 手牌数－３ */
				gpsPlayerWork.byThcnt	=	Thcnt;

				/* ここでの手牌表示はcut */
				iFhcnt	= ++gpsPlayerWork.byFhcnt;
				--iFhcnt;

				ucPai	=	hotpai;
				clrpc_pcnt ( ucPai);
				ucPai	|=	0x80;
				gpsPlayerWork.byFrhai[iFhcnt] = ucPai;

				gpsPlayerWork.byTkhai = 0;				//://手牌に表示のときにツモ牌は表示しない

				/* 1996.6.11.RULE_FIX 国士搶ｶﾝ(無･有)の修正 */
				/* 1996.6.6.KOKUSHI_CHANKAN修正 */
				ankan_m6_fKan	=	1;					/*	分岐フラグに使ってます */
				ankan_m6_bflag	=	2;
				SetIppatuInfo();

				if ( Rultbl[(int)RL.KOKCHA] == 1 ) {
					/* 字牌か１・９ならロンチェックをする */
					if ((( hotpai & 0x07 )==0x01) || ( hotpai > 0x30 )) {
						clropt_opt();
						Odrbuf = Order;
						set_chankan_disp(  Odrbuf, hotpai );
						ankan_m6_bflag	=	1;
						ronho_m5_bflag = 1;
					} else {	//> 2006/03/10 国士搶槓対応
					#if false //-*todo:通信
						if( IS_NETWORK_MODE ) {
							SutehaiActReportSnd();
							ankan_m6_bflag	=	3;
							return (2);
						}
					#endif //-*todo:通信
					}
					//< 2006/03/10 国士搶槓対応
				}
				//> 2006/03/10 国士搶槓対応
				else
				{
					#if false //-*todo:通信
					if( IS_NETWORK_MODE )
					{
						SutehaiActReportSnd();
						ankan_m6_bflag	=	3;
						return (2);
					}
					#endif //-*todo:通信
				}
				//< 2006/03/10 国士搶槓対応
				break;
			case	1:
				DebLog(("ankan_m6 case1"));
			#if false //-*todo:通信

				//> 2006/03/16 バグ No.186 搶槓に対するタイマーが入っていない	//0422
				if( IS_NETWORK_MODE )
				{
					if(gNakiTimeOut >= 0)
					{
						gNakiTimeOut -= uptimems_perfrm;
					}
					if(gNakiTimeOut <= 0)
					{
						gAutoFlag = 0x01;
						gNakiTimeOut = 0;
					}

					if( gDaiuchiFlag== D_ONLINE_OPERATOR_PASS )
					{
						gAutoFlag = 0x01;
						gNakiTimeOut = 0;
					}
				}
			#endif //-*todo:通信
				//> 2006/03/16 バグ No.186

				ret = ronho_m5( 1);
				switch(ret)
				{
					case	0:
						//> 2006/03/10 国士搶槓対応
			#if true //-*todo:通信
							reset_chankan_disp();
							Order = Odrbuf;
							setwp_m2( Order);
							ankan_m6_bflag	=	2;			
			#else //-*todo:通信
						if( IS_NETWORK_MODE )
						{
							reset_chankan_disp();
							Order = Odrbuf;
							setwp_m2( Order);
							ankan_m6_bflag	=	3;
							SutehaiActReportSnd();
							return (2);
						}
						else
						//< 2006/03/10 国士搶槓対応
						{
							reset_chankan_disp();
							Order = Odrbuf;
							setwp_m2( Order);
							ankan_m6_bflag	=	2;
						}
			#endif //-*todo:通信
						break;
					case	1:
						//> 2006/03/10 国士搶槓対応
			#if true //-*todo:通信
							Status |= (byte)ST.CHANK;								/*	国士暗貫の為セットする	*/
							ankan_m6_fKan	=	0;
							ankan_m6_bflag	=	2;
			#else //-*todo:通信
						if( IS_NETWORK_MODE )
						{
							Status |= ST_CHANK;
							ankan_m6_fKan	=	0;
							ankan_m6_bflag	=	3;
							SutehaiActReportSnd();
							return (2);
						}
						else
						//< 2006/03/10 国士搶槓対応
						{
							Status |= ST_CHANK;								/*	国士暗貫の為セットする	*/
							ankan_m6_fKan	=	0;
							ankan_m6_bflag	=	2;
						}
			#endif //-*todo:通信
						break;
					default:
						return(2);
//						break;
				}
				break;
			case	2:
			#if true //-*todo:
				DebLog(("ankan_m6 case2"));
			#endif //-*todo:
				if( ankan_m6_fKan == 1)
				{
					kansub_m6((byte)0 );			/* この中で嶺山牌をつもって表示 */
					disp_dora((ushort) 0 );				/* 王牌表示(カウント通り) */

					/**** new_dial(暗カンオンリー) ****/
					if( Dorakan != 0) {
						/* カンした牌がどらになった人へ(5) */
						MjDialog(  OtherOne( Order ), Order, (byte)DIAL.KANMAKE_DORA );
						AnkanToDora = true;
					}
				}
				/* 1996.6.11.RULE_FIX 国士搶ｶﾝ(無･有)の修正 END */
				return (ankan_m6_fKan);
//				break;
			//> 2006/03/10 国士搶ｶﾝ(無･有)の修正
			case 3:
			{
				// ここで捨牌対応報告を待つ
				if(  is_reentry == true )
				{
					// 再接続処理
			#if false //-*todo:保留　通信？
					SutehaiActNotifyRcv(
						(char)0,
						stReEntry_mycommand[ stReEntry_mycommand_count].HaiState,
						stReEntry_mycommand[ stReEntry_mycommand_count].ActHouseID,
						stReEntry_mycommand[ stReEntry_mycommand_count].SuteHouseID,
						stReEntry_mycommand[ stReEntry_mycommand_count].ChiState,
						stReEntry_mycommand[ stReEntry_mycommand_count].SuteHai
					);
			#endif //-*todo:保留　通信？
					++( stReEntry_mycommand_count);

					//> 2006/03/29 No.211 再接続時にロン牌が出ると見逃す	//0422
					// 再接続終了
					if( stReEntry_mycommand_count == stReEntry_mycommand_count_max ) {
						#if true //-*todo:
						DebLog(("---------- reentry done ----------"));
						#endif //-*todo:

						gsPlayerWork[ game_player ].byPlflg = 1;
						is_reentry = false;
					}
					//> 2006/03/29 No.211 再接続時にロン牌が出ると見逃す
				}
				return (2);
			}
//			break;
			//< 2006/03/10 国士搶ｶﾝ(無･有)の修正
		}
	}
}

/*****************************
	リーチ後のアンカン（全員）の処理
*****************************/
public byte rankan_m6 ( /*MahJongRally * pMe*/ )/*1995.6.7*/
{
	hotpai = gpsPlayerWork.byHkhai;
	return (ankan_m6());
}

/*****************************
	ポンの後のカン（全員）の処理
*****************************/
public byte chakan_m6 ( /*MahJongRally * pMe*/ )
{
	bool	fRet;

	//while(true)
	//{
		switch(chakan_m6_bflag) {
			case	0:
				#if true //-*todo:
				DebLog(("chakan_m6 case0"));
				#endif //-*todo:
				MjEvent( (byte)GEV.NAKI_KAN,Order);

				if( gpsPlayerWork.byHkhai != hotpai ) {
					gpsPlayerWork.byTehai[chkpos_m6()] = gpsPlayerWork.byHkhai;
					thsort_m2();
				}
				chakan_m6_ucPai = (byte)(hotpai | 0x40);
				chakan_m6_bflag = 1;
				chakan_m6_iFrhai = 0;
//				break;
				goto case 1;
			case	1:
				#if true //-*todo:
				DebLog(("chakan_m6 case1"));
				#endif //-*todo:
				for( int i= 0; i< 4; i++) {
					if( chakan_m6_ucPai== gpsPlayerWork.byFrhai[i])
						chakan_m6_iFrhai= i;
				}
				if(chakan_m6_iFrhai < gpsPlayerWork.byFhcnt)
					chakan_m6_bflag = 2;
				else
					return(0);
				break;

			case	2:
				#if true //-*todo:
				DebLog(("chakan_m6 case2"));
				#endif //-*todo:
				if (chakan_m6_ucPai == gpsPlayerWork.byFrhai[chakan_m6_iFrhai]) {
					SetIppatuInfo();
					gpsPlayerWork.byFrhai[chakan_m6_iFrhai]	=	(byte)(hotpai | 0xC0);

					gpsPlayerWork.byTkhai = 0;				//://手牌に表示のときにツモ牌は表示しない
					Status |= (byte)ST.CHANK;
					Status &= (byte)~ST.RINSH;	/* BUG_FIX 1996.6.6. */
					clropt_opt();
					Odrbuf = Order;
					set_chankan_disp(  Odrbuf, hotpai );	/* 1996.6.5.Chankan  搶カン表示の為 */
					chakan_m6_bflag = 3;
					ronho_m5_bflag = 1;
				} else
					chakan_m6_bflag = 5;
				break;

			case	3:
				// 加槓に対するロンチェックを行う
				// 通信対戦時はここで捨牌対応報告を行ってから捨牌対応通知を待つ
				//> 2006/03/16 バグ No.186 搶槓に対するタイマーが入っていない	//0422
			#if false //-*todo:通信
				if( IS_NETWORK_MODE ) {
					if(gNakiTimeOut >= 0)
						gNakiTimeOut -= uptimems_perfrm;

					if(gNakiTimeOut <= 0) {
						gAutoFlag = 0x01;
						gNakiTimeOut = 0;
					}

					if( gDaiuchiFlag== D_ONLINE_OPERATOR_PASS ) {
						gAutoFlag = 0x01;
						gNakiTimeOut = 0;
					}
				}
			#endif //-*todo:通信
				//> 2006/03/16 バグ No.186

				switch(ronho_m5( 0)) {
					case	0: // ロンできるのがいない
						//> 2006/03/08 搶槓対応
						// chakan_m6_bflag = 4;
						// 捨牌対応報告を送信
					#if true //-*todo:通信
							//何も無い場合は嶺上処理へ
							chakan_m6_bflag = 4;
					#else //-*todo:通信
						if( IS_NETWORK_MODE ) {
							SutehaiActReportSnd();
							chakan_m6_bflag = 6;
						} else{
							//何も無い場合は嶺上処理へ
							chakan_m6_bflag = 4;
						}
						//> 2006/03/08 搶槓対応
					#endif //-*todo:通信
						break;
					case	1: // ロンできるのがいた！
						// 捨牌対応報告を送信
						//> 2006/03/08 搶槓対応
						#if true //-*todo:通信
							return (0);
						#else //-*todo:通信
						if( IS_NETWORK_MODE ) {
							SutehaiActReportSnd();
							chakan_m6_bflag = 6;	setwp_m2( Odrbuf );//0529mt
						} else
							return (0);
						#endif //-*todo:通信

						//return (0);
						//> 2006/03/08 搶槓対応
					default:
						break;
				}
				break;

			case	4:
				// 嶺上処理
				reset_chankan_disp();
				Status |= (byte)ST.RINSH;		/* BUG_FIX 1996.6.6. */
				Status &= (byte)~ST.CHANK;
				Order = Odrbuf;
				setwp_m2( Order);
				fRet	=	kansub_m6 ((byte)2 );	/* この中で嶺山牌をつもって表示 */
				disp_dora ((ushort) 1 );					/* 王牌のツモした時点（カンドラはオープンしてない） */

				//> 2006/03/09 No.165 加槓の際に牌が重複して表示される
				 hide_hkhai = 0;
				//< 2006/03/09 No.165 加槓の際に牌が重複して表示される

				if(fRet)
					// カンに成功
					return (1);
				else
					// カンに失敗
					return (0);
//				break;

			case	5:
				//TRACE("chakan_m6 case5");
				chakan_m6_iFrhai++;
				chakan_m6_bflag = 1;
				break;

			case 6: {
				// ここで捨牌対応報告を待つ
				if(  is_reentry == true ) {
			#if false //-*todo:保留　通信？

				 	SutehaiActNotifyRcv(
						(char)0,
						stReEntry_mycommand[ stReEntry_mycommand_count].HaiState,
						stReEntry_mycommand[ stReEntry_mycommand_count].ActHouseID,
						stReEntry_mycommand[ stReEntry_mycommand_count].SuteHouseID,
						stReEntry_mycommand[ stReEntry_mycommand_count].ChiState,
						stReEntry_mycommand[ stReEntry_mycommand_count].SuteHai
					);
			#endif //-*todo:保留　通信？

					++( stReEntry_mycommand_count);

					//> 2006/03/29 No.211 再接続時にロン牌が出ると見逃す		//0422
					// 再接続終了
					if( stReEntry_mycommand_count == stReEntry_mycommand_count_max ) {
						#if true //-*todo:
						DebLog(("---------- reentry done ----------"));
						#endif //-*todo:
						gsPlayerWork[ game_player ].byPlflg = 1;
						is_reentry = false;
					}
					//> 2006/03/29 No.211 再接続時にロン牌が出ると見逃す
				}
			}
//				break;
			goto default;
			default:
				break;
		}
	//}
	return(2);
}

//-*********************Mjm6.j
}
