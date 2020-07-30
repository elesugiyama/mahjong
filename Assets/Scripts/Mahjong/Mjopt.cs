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
// mjopt.j
//-*****************
public partial class MahjongBase : SceneBase {
/*
**		麻雀大会３
**		オプションウィンドウ
*/
//#include "MahJongRally.h"								// Module interface definitions

/*********************************************************************
	定義・変数
*********************************************************************/

/*****************************
	オプションスタックの初期化
*****************************/
public void clropt_opt ( /*MahJongRally * pMe*/ )
{
	Optno[Order]	  = (byte)OP.TSUMO;			/* オプション種類 */
	Optcnt		= 0;				/* オプション数 */
	Optcsr		= 0;				/* カーソル位置 */
	// オプションスタック初期化
	_MEMSET( Optstk, (int)OP.PASS, MJDefine.D_OPT_STK_MAX );		//_MEMSET( Optstk, OP_PASS, sizeof(BYTE) * 5 );
	// motohashi
	// メニューカーソルの初期化。
	menu_csr = 0;
#region UNITY_ORIGINAL
	ResetCallMenu();
#endregion //-*UNITY_ORIGINAL
}

/*****************************
	オプション項目の登録
		Accum_a = OP_(オプションデファインが入ってくる)
*****************************/
public void pshopt_opt (/*MahJongRally * pMe,*/ int iMode)/*1995.5.31*/
{
	byte	st1;		/* バックアップ */
	//BYTE	bflag;
	int		iTmp;

	{
		//tamaki　追加	はじめ
		//> 2006/02/16 バグ No.81
		// 鳴き無しでもロン・自摸カン・自摸・立直を可能にする
		#if false //-*todo:通信
		if( IS_NETWORK_MODE && gMyTable_NakiNashi == 1 ) {
			switch( iMode ) {
				case OP_TSUMO	:
				case OP_RON 	:
				case OP_RICHI	:	break;
				case OP_KAN 	:
				case OP_CHANKAN : {
					if( reentry_m1_bflag != 8 )
						return;
				}
				break;
				default : return;
			}
		}
		#endif //-*todo:通信
		//< 2006/02/16 バグ No.81

		// メニューに何か追加される場合は最初にパスを入れる
		if(Optcnt== 0) {
			Optstk[Optcnt] = (byte)OP.TAPAI;
			Optcnt++;
		}
		//tamaki　追加　おわり
		iTmp	=	Optcnt;
		if ( iTmp != 0 ) {

			/* ソートして登録してるようだ */
			for (; ; ) {						/* オプションがある */
				if ( iMode <= Optstk[iTmp - 1] )
					break;
				else {
					st1				=	Optstk[iTmp - 1 ];
					Optstk[iTmp]	=	st1;
					if (--iTmp == 0)
						break;
				}
			}
		}
		Optstk[iTmp]	=	(byte)iMode;
		if ( iMode == (int)OP.RON )
			menu_csr = (short)Optcnt;	// 2006/02/16 No.101
		++Optcnt;
	}
}

/**************************************END OF FILE**********************************************/
//-*********************mjopt.j
}
