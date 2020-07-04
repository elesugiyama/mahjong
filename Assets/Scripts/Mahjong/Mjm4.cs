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
// mjm4.j
//-*****************
public partial class MahjongBase : SceneBase {
/*
**		麻雀大会３
**		プレイヤーがらみの処理
*/
//#include "MahJongRally.h"

public const int OPCHECK	=	1;
//extern SHORT	CommandCatch(/*MahJongRally * pMe,*/ BYTE iOrder);

public void SetUserSthai(/*MahJongRally * pMe,*/ byte byPai)
{
	hotpai	=	Sthai	=	byPai;
}

/*****************************
	プレイヤーの手順処理
*****************************/
public void __killPai(/*MahJongRally * pMe,*/ int iPos)
{
	/*	1999/10/04	M.Fukuda	捨牌効果	*/
	byte	ucData;
	if (iPos != Thcnt) {
		ucData	=	gpsPlayerWork.byTehai[iPos];
	}
	else {
		ucData	=	gpsPlayerWork.byHkhai;
	}

	SetUserSthai( ucData);
	if (iPos != Thcnt) {
		ucData						=	gpsPlayerWork.byTehai[iPos];
		gpsPlayerWork.byTehai[iPos]	=	gpsPlayerWork.byHkhai;
		gpsPlayerWork.byHkhai		=	ucData;
		thsort_m2();
	}
}

/*****************************
	カン４つチェック
	  Return	(0:カンNO)
*****************************/
public bool j4kan_m4 (/*MahJongRally * pMe*/)/*1995.2.6*/
{
	if ( (Bpcnt+Kancnt) == MJDefine.PAI_MAX)		//122x
		/* ハイテイはカンできない */
		return (false);

	if (Kancnt < 4)
		/* カンの数が４より少なければカンできる */
		return (true);
	else
		// 槓４つで槓を不可能に
		//else if ( Kancnt == 5 ) {/* カンの数が５になったら無条件でこれ以上カンできない */
		return (false);

//xxxx	if((Status & ST_4KAN) != 0) {/* ステータスの４カンフラグが立っていればカンできない */
//xxxx		return (false);
//xxxx	}

//xxxx	return true;
}

public byte after_richi_m4 (/*MahJongRally * pMe*/)
{
	byte	i;
	short	cmd;

	Csr = Thcnt;

	while( after_richi_m4_bflag != 0 ) {
		if(after_richi_m4_bflag == 100) {
			ankan_m6_bflag = 0;
			clropt_opt();

			if(!chkmoh_m5()) {	/*上がりでないならA=ff,上がりならA=HKHAI(Z=0)*/
				after_richi_m4_bflag = 1;
				/** ツモった牌であがれないとき → 四カンチェック **/
				if(j4kan_m4()) {
					/** リーチ後のカンチェック **/
					if(chkrk_m4()) {
						pshopt_opt( (int)OP.KAN);
						for(i = 0; i < gpsPlayerWork.byThcnt; i++)
							if(gpsPlayerWork.byTehai[i] == gpsPlayerWork.byHkhai)
								break;

						Kanflg[0] = 3;
						Kanflg[1] = 0;
						Kanflg[2] = 0;
						Kanpos[0] = i;
						SubMj.Kannum = 1;
					}
				}
			} else {
				if(jyaku_jy ()) {		/* あがれる状態 */
					/* 安国寺 ANKOKU_FIX 1996.9.5 */
					gpsPlayerWork.bFfurit	=	0xff;		/*上がれる牌ならフリテンフラグが立つ*/
					pshopt_opt( (int)OP.TSUMO);
				} else
					/* 安国寺 ANKOKU_FIX 1996.9.5 */
					gpsPlayerWork.bFfurit	=	0xff;		/*上がれる牌ならフリテンフラグが立つ*/
			}
			after_richi_m4_bflag = 2;
			SetUserSthai( gpsPlayerWork.byHkhai);			//://????????
		}

		if(after_richi_m4_bflag == 2) {
			// ツモ切り
			#if true //-*todo:通信
				cmd = CommandCatch( Order);
			#else //-*todo:通信
			if( IS_NETWORK_MODE && gDaiuchiFlag == D_ONLINE_OPERATOR_PASS ) {
				NakiSelNo = Thcnt;
				cmd = OP_TAPAI;
				//> ツモ切りでもリーチ時ならツモ和了りできるように修正
				for( i = 0; i < Optcnt; ++i )
					if( Optstk[i] == OP_TSUMO ) {
						cmd = OP_TSUMO;
						break;
					}

				//> ツモ切りでもツモ和了りできるように修正
			} else{
				cmd = CommandCatch( Order);
			}
			#endif //-*todo:通信
//			cmd = TehaiSel();
			if(cmd != -1) {
				after_richi_m4_bflag = 0;		/** break **/
				switch(cmd) {
					case (short)OP.TAPAI:
						if(NakiSelNo != Thcnt)
							after_richi_m4_bflag = 2;		/** break **/
						else {
							__killPai( Thcnt);
							gpsPlayerWork.bFippat		=	0;
							gpsPlayerWork.byIppatuClear	=	0;
						}
						break;

					case (short)OP.KAN:		/* カン */
						after_richi_m4_bflag = 3;
						ankan_m6_bflag = 0;
						break;

					case (short)OP.TSUMO:		/* ツモった */
						MjEvent( (byte)GEV.NAKI_TUMO,Order);
						//return (1);
						break;

					case (short)OP.TAOPAI:		/* 牌を倒す */
						//return (1);
						break;
				}
				#if false //-*todo:通信
				//20051205 add
				if( IS_NETWORK_MODE ) {
					SutehaiReportSnd( Order,cmd,Sthai);

					if(  is_reentry == false ) {
						// オプションの更新
						updateDaiuchiFlag();
						updateSetsudanFlag();
					}

					gTsumoTimeOut = 0;
				}
				#endif //-*todo:通信
				hai_csr = (short)gpsPlayerWork.byThcnt;	//20051117 add 牌カーソルを初期値にする

				if( (cmd==(short)OP.TAOPAI)||(cmd==(short)OP.TSUMO) )
					return(1);

			} else
				return(2);
		}

		if(after_richi_m4_bflag == 3) {
			switch(rankan_m6()) {
				case	1:
					after_richi_m4_bflag = 100;
					Csr = Thcnt;
					return(2);
				case	0:
					after_richi_m4_bflag = 0;		/** break **/
					return (0);
				default:
					return(2);
			}
		}
	}
	return (0);
}
/*****************************
	リーチのチェック
*****************************/
public bool	chkric_m4 (/*MahJongRally * pMe*/)
{
	byte	bflag;
	byte	Crp = 0;
	byte	Crf = 0;
	int		i;

	_MEMSET(Ricbuf, 0, Ricbuf.Length);
	if (!jrichi_m4())
		return (false);

	sTmpPlayerWork= (PLAYERWORK)gsPlayerWork[Order].clone();	//MEMCPY(sTmpPlayerWork, gsPlayerWork[Order], sizeof(sTmpPlayerWork));
	gpsPlayerWork	=	sTmpPlayerWork;				//&sTmpPlayerWork;
	Crp = Thcnt;
	bflag = 2;
	for (i = 0; i < 14; i++) {
		_MEMCPY(sTmpPlayerWork.byTehai, gsPlayerWork[Order].byTehai, 14);
		sTmpPlayerWork.byTehai[i]	=	gsPlayerWork[Order].byHkhai;
		thsort_m2();
		chktnp_m5();
		Ricbuf[i] = sTmpPlayerWork.byTenpai;
		Crf	|=		Ricbuf[i];
	}
	gpsPlayerWork	=	gsPlayerWork[Order];		//&gsPlayerWork[Order];

	return (Crf != 0);
}
/*****************************
	カン出来るかのチェック
*****************************/
public bool _chkan_m4(/*MahJongRally * pMe,*/ byte ubPai)
{
	int	i;
	for (i = 0; i < gsPlayerWork[Order].byFhcnt; i++) {
		if (gsPlayerWork[Order].byFrhai[i] == (ubPai | 0x40)) {		/*	ポンしている牌を調べる	*/
			return (true);
		}
	}
	return (false);
}
public bool chkkan_m4 (/*MahJongRally * pMe*/)/*1995.6.8*/
{
	int		i, iPos = 0;	//w4
	byte	ucPai;
	int		iCnt;
	byte	ucBkPai;
	int		iThcnt;

	//> 2006/04/15 DS依存バグNo.2
	// iThcnt		=	gsPlayerWork[Order].byThcnt;
	// ucBkPai	=	0xFF;
	SubMj.Kannum	=	0;		//0422
	// iCnt	=	0;
	//for (i = 0; i < iThcnt + 1; i++) {
	//	if (i < iThcnt) {
	//		ucPai	=	gpsPlayerWork->byTehai[i];				/*	調べる牌				*/
	//	}
	//	else {
	//		ucPai	=	0xFF;
	//	}
	//	if (_chkan_m4(pMe, ucPai)) {
	//		Kanflg[Kannum] = 1;								/*	ポンしている牌を調べる	*/
	//		Kanpos[Kannum] = (BYTE)i;
	//		++Kannum;
	//		iCnt	=	0;
	//		ucBkPai	=	0xFF;
	//	}
	//}
	//< 2006/04/15 DS依存バグNo.2

	iThcnt		=	gsPlayerWork[Order].byThcnt;
	ucBkPai	=	0xFF;
	iCnt	=	0;
	for (i = 0; i < iThcnt + 1; i++) {
		if (i < iThcnt)
			ucPai	=	gpsPlayerWork.byTehai[i];			/*	調べる牌				*/
		else
			ucPai	=	0xFF;

		if (ucBkPai != ucPai) {								/*	前と同じか？			*/
			if (iCnt == 3) {								/*	３牌同じ牌がある		*/
				if (ucBkPai == gpsPlayerWork.byHkhai) {		/*	ツモてきた牌と比べる	*/
					Kanflg[SubMj.Kannum] = 3;						/*	４牌あるのでカンできる	*/
					Kanpos[SubMj.Kannum] = (byte)iPos;
					++SubMj.Kannum;
				}
			}
			iCnt	=	0;									/*	CNT をクリア	*/
			iPos	=	i;									/*	先頭の位置		*/
			ucBkPai	=	ucPai;
		}
		//> 2006/04/15 DS依存バグNo.2
		//●プレイヤーの行動で「手牌での加槓」と 「手牌＋ツモ牌での暗槓」が
		//  可能な状況なのに「手牌での加槓」しかできない場合がある。
		//  具体的には、プレイヤーの手牌の並びが、暗槓できる３個の牌の右隣に
		//  加槓できる牌があるという場合。
		if (_chkan_m4(ucPai)) {				//0422
			Kanflg[SubMj.Kannum] = (byte)1;		//0422
			Kanpos[SubMj.Kannum] = (byte)i;		//0422
			SubMj.Kannum++;						//0422
			iCnt = 0;						//0422
			ucBkPai = 0xFF;					//0422
		}
		else {
//		if (ucBkPai == ucPai) {				//0422
		//> 2006/04/15 2006/04/15 DS依存バグNo.2
			++iCnt;
			if (iCnt >= 4) {								/*	４牌同じ牌がある		*/
				Kanflg[SubMj.Kannum] = 4;
				Kanpos[SubMj.Kannum] = (byte)iPos;
				++SubMj.Kannum;
			}
		}
	}

	if (_chkan_m4( gpsPlayerWork.byHkhai)) {
		Kanflg[SubMj.Kannum] = 1;								/*	ポンしている牌を調べる	*/
		Kanpos[SubMj.Kannum] = (byte)iThcnt;
		++SubMj.Kannum;
	}

	return (SubMj.Kannum != 0);
}

public int GuideCursolPos(/*MahJongRally * pMe*/)
{
	int		i;

	if (IsGuideMode()) {
		switch (comput_m1( true)) {
		  case (int)COMPUT_RET.NON:						/*	捨てる牌の場所にセット	*/
			if (Sthai == gpsPlayerWork.byHkhai)		/*	ツモ牌か？	*/
				break;

			for (i = 0; i < gpsPlayerWork.byThcnt; i++)
				if (gpsPlayerWork.byTehai[i] == Sthai)
					return (i);
			break;
		  case (int)COMPUT_RET.KAN:						/*	カンする牌にセット	*/
			if (hotpai == gpsPlayerWork.byHkhai)	/*	ツモ牌か？	*/
				break;
			for (i = 0; i < gpsPlayerWork.byThcnt; i++)
				if (gpsPlayerWork.byTehai[i] == hotpai)
					return (i);
			break;
		  case (int)COMPUT_RET.TSUMO:					/*	ツモ牌にカーソルをセット	*/
		  case (int)COMPUT_RET.ELSE:
			break;
		}
		hotpai	=	Sthai	=	gpsPlayerWork.byHkhai;
	}
	return (Thcnt);
}

/*****************************
	プレイヤーのリーチ前の手順処理（未完成）
*****************************/
public bool _chkTsumo(/*MahJongRally * pMe*/)
{
	if(gpsPlayerWork.byTenpai != 0 ) {
		if(chkmoh_m5()) {
			if (jyaku_jy ()) {
				pshopt_opt( (int)OP.TSUMO);
				return (true);
			}
		}
	}
	return (false);
}

public byte before_richi_m4 (/*MahJongRally * pMe*/)
{
	short	cmd;

	if(before_richi_m4_bflag != 0) {
		if(before_richi_m4_bflag == 100) {
			Csr = (byte)GuideCursolPos();				/* 初期カーソル位置は手牌カウント */
			Pinch	=	0;
			clropt_opt();						/* オプションの初期化 */

			//20051205
			if((Status & (byte)ST.RINSH) != 0 && _chkTsumo()) {
				/*	リンシャンの時はツモチェックをする	*/

				//> 2006/04/15 DS依存バグNo.1
				//●プレイヤーが嶺上牌でツモあがりが可能な時、「リーチ」や「カン」が
				//  できる状態であっても、「ツモ」しかできない。
				//  （ツモあがりできない時は、リーチやカンが可能）
				if(chkric_m4())
					/* 立直のチェック */
					pshopt_opt((int)OP.RICHI);

				if(j4kan_m4())
					/* ４個カンのチェック */
					if(chkkan_m4())
						/* カンできるかのチェック */
						pshopt_opt((int)OP.KAN);

				//< 2006/04/15 DS依存バグNo.1

				before_richi_m4_bflag = 5;
			} else if((Status & (byte)ST.NAKI) != 0) {
				before_richi_m4_bflag = 5;
			} else {
				before_richi_m4_bflag = 1;

				//> 2006/04/03 不具合？No.125 国士無双・天和（地和）で和了っていようとも「たおす」がしたい！
				/** １順目か？ **/
				if((Status & (byte)(ST.ONE_JUN | ST.RINSH)) == 0) {		//0422
					/** 九種九牌チェック **/
					if(cntyao_jy () >= 9)						//0422
						pshopt_opt((int)OP.TAOPAI);					//0422
				}
				//< 2006/04/03 不具合？No.125

				if(_chkTsumo())
					before_richi_m4_bflag = 3;
			}
		}

		if(before_richi_m4_bflag == 1) {
			/** １順目か？ **/
			if((Status & (byte)(ST.ONE_JUN | ST.RINSH)) == 0) {
				/** 十三チェック **/
				/* 1996.6.11.RULE_FIX 十三不搭(無･満･倍･役)の修正 */
				if(Rultbl[ (int)RL.SHISAN ] != 0)
					if(jshisa_jy ())
						pshopt_opt( (int)OP.TSUMO);

				//> 2006/04/03 不具合？125 国士無双・天和（地和）で和了っていようとも「たおす」がしたい！
				///* 1996.6.11.RULE_FIX 十三不搭(無･満･倍･役)の修正 END */
				///** 九種九牌チェック **/
				//if(cntyao_jy () >= 9)			//0422
				//{
				//	pshopt_opt(OP_TAOPAI);		//0422
				//}
				//< 2006/04/03 不具合？125
			}
			before_richi_m4_bflag = 3;
		}

		if(before_richi_m4_bflag == 3) {
			if(chkric_m4())
				/* 立直のチェック */
				pshopt_opt( (int)OP.RICHI);

			if(j4kan_m4())
				/* ４個カンのチェック */
				if(chkkan_m4())
					/* カンできるかのチェック */
					pshopt_opt( (int)OP.KAN);

			before_richi_m4_bflag = 5;
		}

		if(before_richi_m4_bflag == 5) {
			#if true //-*todo:通信
				cmd = CommandCatch( Order);
			#else //-*todo:通信
			// ツモ切り
			if( IS_NETWORK_MODE && gDaiuchiFlag == D_ONLINE_OPERATOR_PASS ) {
				NakiSelNo = Thcnt;
				cmd = OP_TAPAI;
			} else{
				cmd = CommandCatch( Order);
			}
			#endif //-*todo:通信
//			cmd = TehaiSel();
//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
//自動でツモ切りモード
//			cmd = OP_TAPAI;
//			NakiSelNo = gpsPlayerWork.byThcnt;
//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
			if(cmd != -1) {
				//TRACE("プレーヤー動作決定　コマンド：%d 牌位置：%d\n",cmd,NakiSelNo);
				//ここで捨牌報告を送信する
				before_richi_m4_bflag = 0;		/** break **/
				switch(cmd) {
					case (short)OP.TAPAI:
						__killPai( NakiSelNo);
						break;

					case (short)OP.RICHI:
						gpsPlayerWork.bFrich		=	0xff;
						if((Status & (byte)ST.ONE_JUN) == 0)
							gpsPlayerWork.bFwrich	=	0xff;
						gpsPlayerWork.bFippat		=	0xff;
						/* BUGFIX stop */
						__killPai( NakiSelNo);

						MjEvent( (byte)GEV.NAKI_RICHI,Order);
						break;

					case (short)OP.KAN:		/* カン */
						if(Kanpos[NakiSelNo] >= Thcnt)
							hotpai	=	Sthai	=	gpsPlayerWork.byHkhai;
						else
							hotpai	=	Sthai	=	gpsPlayerWork.byTehai[Kanpos[NakiSelNo]];

						if (Kanflg[NakiSelNo] - 1 != 0) {
							// 暗槓処理フラグをセット
							before_richi_m4_bflag = 6;
							ankan_m6_bflag = 0;
						} else {
							// 搶槓処理フラグをセット
							before_richi_m4_bflag = 7;
							chakan_m6_bflag = 0;

							//> 2006/03/09 No.165 加槓の際に牌が重複して表示される
							 hide_hkhai = 1;
							//< 2006/03/09 No.165
						}
						break;
					case (short)OP.TAOPAI:		/* 牌を倒す */
						Pinch = (byte)SP.NINE_HAI;
						break;
					case (short)OP.TSUMO:		/* ツモった */
						MjEvent( (byte)GEV.NAKI_TUMO,Order);
						break;
				}
				#if false //-*todo:通信
				if( IS_NETWORK_MODE ) {
					if(before_richi_m4_bflag==7) // 搶槓処理
						cmd = OP_CHANKAN;
					SutehaiReportSnd( Order,cmd,Sthai);

					if(  is_reentry == false ) {
						// オプションの更新
						updateDaiuchiFlag();
						updateSetsudanFlag();
					}
					gTsumoTimeOut = 0;
				}
				#endif //-*todo:通信

				hai_csr = (short)gpsPlayerWork.byThcnt;	//20051117 add 牌カーソルを初期値にする

				if( (cmd==(short)OP.TAOPAI) || (cmd==(short)OP.TSUMO) )
					return(1);

				#if false //-*todo:通信
				if( IS_NETWORK_MODE ){
					if( cmd==(short)OP.KAN || cmd==(short)OP.CHANKAN ){
						return(1);
					}
				}
				#endif //-*todo:通信
			} else
				return(2);
		}

		//-----------------------------------------
		// 暗槓フラグが立っている場合の処理
		//-----------------------------------------
		if(before_richi_m4_bflag == 6) {
			switch(ankan_m6()) {
				case	0:
					before_richi_m4_bflag = 0;		/** break **/
					return (1);
				case	1:
					before_richi_m4_bflag = 100;
					return (2);
				default:
					return (2);
			}
		}

		//-----------------------------------------
		// 搶槓フラグが立っている場合の処理
		//-----------------------------------------
		if(before_richi_m4_bflag == 7) {
			switch(chakan_m6()) {
				case	1:
					before_richi_m4_bflag = 100;
					return (2);
//					break;
				case	0:
					before_richi_m4_bflag = 0;		/** break **/
					return (1);
//					break;
				default:
					return (2);
//					break;
			}
		}
	}

	return (0);
}

public byte player_m4 (/*MahJongRally * pMe*/)
{
	byte	fRet;
	if (gpsPlayerWork.bFrich == 0)
	{
		/* リーチしていない */
		fRet = before_richi_m4();
	}else{
		/* リーチしている */
		fRet = after_richi_m4();
	}
	return (fRet);
}

/*****************************
	リーチ後のカンのチェック
*****************************/
public bool chkrk_m4 (/*MahJongRally * pMe*/)/*1995.4.25, 6.6*/
{
	byte	ucPai;
	int		iThcnt;
	int		iMachi;

	//if((Bpcnt + Kancnt) > 121 )
	//{
	//	return (FALSE);
	//}

	/* 1996.6.11.RULE_FIX 立直暗ｶﾝ(無･有)の修正 */
	if (Rultbl[ (int)RL.RANKAN ] != 0) {/* 基本的にこの if ～ elseが増えただけ */
		ucPai	=	gpsPlayerWork.byHkhai;
		iThcnt = Thcnt;

		while (iThcnt-- != 0) {
			if (ucPai == gpsPlayerWork.byTehai[iThcnt])
				break;
		}
		if (iThcnt < 0)
			return (false);

		iThcnt -= 2;
		if (iThcnt < 0)
			return (false);

		if (ucPai != gpsPlayerWork.byTehai[iThcnt])
			return (false);

		sTmpPlayerWork= (PLAYERWORK)gpsPlayerWork.clone();		//MEMCPY(sTmpPlayerWork, gpsPlayerWork, sizeof(sTmpPlayerWork));
		Thcnt -= 3;
		for (; ;) {
			sTmpPlayerWork.byTehai[iThcnt]	=	sTmpPlayerWork.byTehai[iThcnt + 3];
			if (++iThcnt >= Thcnt) {
				++sTmpPlayerWork.byFhcnt;
				gpsPlayerWork			=	sTmpPlayerWork;
				gpsPlayerWork.byThcnt	=	Thcnt;
				chktnp_m5();
				setwp_m2( Order);
				if (gpsPlayerWork.byTenpai != sTmpPlayerWork.byTenpai) {
					return (false);
				}
				break;
			}
		}
		iMachi = 0;
		iThcnt = gpsPlayerWork.byTenpai;
		while (iThcnt-- != 0) {
			if (gpsPlayerWork.byMchai[iMachi] != sTmpPlayerWork.byMchai[iMachi])
				return (false);

			++iMachi;
		}
		return (true);
	}
	return (false);
	/* 1996.6.11.RULE_FIX 立直暗ｶﾝ(無･有)の修正 END */
}

/*****************************
	リーチ出来るかのチェック
*****************************/
public bool jrichi_m4 (/*MahJongRally * pMe*/)	/*1995.4.27, 6.29*/
{
#if	Rule_2P
	//リーチ禁止
	if( Order== 0)		//プレイヤー？
		if(( m_keepData.mah_limit_num== (int)MAH.LIM04 )||( m_keepData.mah_limit_num== (int)MAH.LIM06 )){
			return (false);
		}
#endif

	if (gpsPlayerWork.bFmenzen != 0)
		return (false);

#if	Rule_2P
	if (Bpcnt + Kancnt >= 119+ 2)
#else
	if (Bpcnt + Kancnt >= 119)
#endif
		return (false);

	if (Rultbl[(int)RL.DOBON] != 0)
		//1000点あるか？
		if (gpsTableData.sMemData[Order].nPoint < 10)
			return (false);

	return (true);
}

//-*********************mjm4.j
}
