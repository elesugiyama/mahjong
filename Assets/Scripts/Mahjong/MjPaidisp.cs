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
// paidisp.j
//-*****************
public partial class MahjongBase : SceneBase {
/*
**		麻雀大会３
**		牌表示処理
*/

//#include "MahJongRally.h"								// Module interface definitions

/*********************************************************************
		王牌表示
*********************************************************************/
/*****************************
	ドラ（王牌）ベースの表示
		syori = 0:カンカウント通り
				1:明カン直後（牌ツモのみ・ドラはまだ開かない）
				2:オープン（局終了）
				3:開始直後（何も開いてない）
*****************************/

public void	disp_wanpai_base ( /*MahJongRally * pMe,*/ ushort syori )
{
	ushort	i;
	ushort	open_cnt;		/* 表向き牌数 */
	byte[]	byBuf= new byte [14];

	_MEMSET(byBuf, (int)WANTYPE.REV, byBuf.Length);

	/* 表向き牌数取得 */
	if( (syori==1) || (syori==3) )		/* 開始・明カン直後 */
	{
		open_cnt = (ushort)Kancnt;
	}else{								/* デフォルト＋カンカウント */
		open_cnt = (ushort)(Kancnt + 1);
	}
	/* カンで開く牌があるがカンドラ無しルールだ 1996.6.19. */
	if( ( open_cnt > 1 ) && ( Rultbl[ (int)RL.KAN ] == 0 ) )
		open_cnt = 1;

	for (i = 0; i < open_cnt; i++) {
		byBuf[8-(i*2)]	=	(byte)WANTYPE.FRONT;
		if(syori == 2){
			byBuf[8-(i*2)+1]	=	(byte)WANTYPE.FRONT;
		}
	}

	if (Kancnt >= 1) {
		byBuf[12]	=	(byte)WANTYPE.NON;
		if (Kancnt >= 2) {
			byBuf[13]	=	(byte)WANTYPE.NON;
			if (Kancnt >= 3) {
				byBuf[10]	=	(byte)WANTYPE.NON;
				if (Kancnt >= 4)
					byBuf[11]	=	(byte)WANTYPE.NON;
			}
		}
	}
	//://
	for( i = 0; i < byBuf.Length; i++)
		WanPaiBuf[i] = byBuf[i];
}

/***************************************************************/
/*	プライオリティオフセット								   */
/*	  252 - 271 (20) 王牌									   */
/*		 00→09 (10) 絵柄									   */
/*				 左から１～５列分順に						   */
/*				 ＭＡＸ：カン４・ドラオープン時 			   */
/*						 １０個の牌柄						   */
/*		 10→19 (10) ベース 								   */
/*				 左から５～７列分順に						   */
/*				 ＭＡＸ：カン４・ドラオープン時 			   */
/*						 １０個の表向き通常牌				   */
/***************************************************************/
/*****************************
	ドラ（王牌）の表示
		syori = 0:カンカウント通り
				1:明カン直後（牌ツモのみ・ドラはまだ開かない）
				2:オープン（局終了）
				3:開始直後（何も開いてない）
*****************************/
public void disp_dora ( /*MahJongRally * pMe,*/ ushort syori )
{
	/* オープン処理だが裏ドラなし 1996.6.19. */
	if ( (syori == 2) && ( Rultbl[ (int)RL.URA ] == 0 ) )
		syori = 0;

	/*** 王牌（ドラ）ベースの表示 ***/
	disp_wanpai_base ( syori );
}

/*********************************************************************
		捨て牌管理
*********************************************************************/
/*****************************
	搶カン表示のセット 1996.6.5.Chankan
*****************************/
public void set_chankan_disp( /*MahJongRally * pMe,*/ byte basyo, byte hai )
{
	short	i;
	byte	buf2;

	if (_No_Paidisp()) {										/*	COMP対局は表示しない	*/
		return;
	}
	Rec_chankan_man = basyo;
	Rec_chankan_hai = hai;
	for( i = 0; i < 4; i++ ){
		buf2 = gsPlayerWork[basyo].byFrhai[i];
		if( ( buf2 == ( Rec_chankan_hai + 0x80 ) ) ||
			( buf2 == ( Rec_chankan_hai + 0xC0 ) )	 ){	/* その牌が搶カン牌 */
			Rec_chankan_no = (byte)i;					/* 何個目のフーロが搶カンか */
			break;
		}
	}
}

/*****************************
	搶カン表示のリセット 1996.6.17.Chankan
*****************************/
public void reset_chankan_disp( /*MahJongRally * pMe*/ )
{
	Rec_chankan_man = MJDefine.NONE;
	Rec_chankan_hai = MJDefine.NONE;
	Rec_chankan_no	= MJDefine.NONE;
}

/*****************************
	搶カン表示がセットされてるかのチェック 1996.6.17.Chankan
*****************************/
public bool check_chankan_disp( /*MahJongRally * pMe*/ )
{
	return	( ( Rec_chankan_man != MJDefine.NONE ) ? true : false);
}
/*****************************
	捨て牌・搶カン表示管理バッファの初期化
	  毎局開始前に実行すること！
*****************************/
public void init_sutehai_rec( /*MahJongRally * pMe*/ )
{
	short i, j;

	for (i = 0; i < MJDefine.MAX_TABLE_MEMBER; i++) {	//4
		for (j = 0; j < MJDefine.SUTEHAIMAX; j++)
			Rec_sute_pos[i,j] = 0;

//		Rec_sute_pri[i] = (USHORT)WORDMAX;
		Rec_sute_reach[i] = false;
		/* 1996.6.5.Chankan */
		#if true //-*todo:short -> ushort に変更
		// Rec_minkan_pri[i]		=	(USHORT)WORDMAX;
		Rec_minkan_pri[i]		=	MJDefine.WORDMAX;
		#endif //-*todo:short -> ushort に変更
	}
	Sute_flush_f = false;
	Rec_chankan_man = MJDefine.NONE;
	Rec_chankan_hai = MJDefine.NONE;
	Rec_chankan_no = MJDefine.NONE;

#region UNITY_ORIGINAL
	ResetDiscardedTiles();
	ResetFuroTiles();
	ResetWanTiles();
#endregion //-*UNITY_ORIGINAL

}

/*****************************
	捨て牌表示管理バッファへの登録
		syori = (0:ON)(1:OFF)
*****************************/
public void set_sutehai_rec ( /*MahJongRally * pMe,*/ ushort syori, ushort basyo )
{
	byte	sutehai_cnt;
	byte	reach_f;

	if (_No_Paidisp()) {										/*	COMP対局は表示しない	*/
		return;
	}
	sutehai_cnt = gsPlayerWork[basyo].byShcnt;				/* 現捨て牌数 */
	reach_f = gsPlayerWork[basyo].bFrich;					/* 立直中なら0xFF */
	if( sutehai_cnt == 0 ) return;
	switch( syori ){
	  case	( 0 ):	/* 登録 */
		if( (Rec_sute_reach[basyo] == false) && (reach_f == 0xFF) ){
		/* 立直牌未表示かつ立直中なら */
			Rec_sute_pos[basyo,sutehai_cnt-1] = (byte)SUTEHAI.REACH;
			Rec_sute_reach[basyo] = true;
		}
		else{
			Rec_sute_pos[basyo,sutehai_cnt-1] = (byte)SUTEHAI.ON;
		}
		break;
	  case	( 1 ):	/* 消去 */
		if( Rec_sute_pos[basyo,sutehai_cnt-1] == (byte)SUTEHAI.REACH ){
			/* 鳴きが入った牌が立直捨て牌だった */
			Rec_sute_reach[basyo] = false;
		}
		Rec_sute_pos[basyo,sutehai_cnt-1] = (byte)SUTEHAI.OFF;
		break;
	  default:
		break;
	}
}



/*********************************************************************
	捨て牌表示
*********************************************************************/
public void disp_sutehai_flush( /*MahJongRally * pMe,*/ bool syori ,bool agari)
{
	bool	flush_f;		//*flush_f;
	byte	flush_no;		//*flush_no;
	byte	flush_pos;		//*flush_pos;
	byte	i;
	//if(sGameData.byGameMode == GAMEMODE_WIRELESS && Order != 0 && Order <= link_max){
	//}else
	{
		flush_f = Sute_flush_f;
		flush_no = Sute_flush_no;
		flush_pos = Sute_flush_pos;
	}
	if (_No_Paidisp())										/*	COMP対局は表示しない	*/
		return;

	if( ( syori == true ) && ( flush_f == false ) ){	//( syori == ON )
		if(agari){
			//if(sGameData.byGameMode == GAMEMODE_WIRELESS){
			//	for(i=0;i<3;i++){
			//	}
			//	Sute_flush_f = TRUE;
			//	Sute_flush_no = Odrbuf;
			//	if( Rec_chankan_man == NONE ){					/* 搶カンじゃなかった */
			//		Sute_flush_pos = 0;
			//	}else{
			//		Sute_flush_f = FALSE;
			//	}
			//}else
			{
				flush_no = Odrbuf;
			}
		}else{
			flush_no = (byte)((Odrbuf-Order)&0x03);
			flush_f = true;								/* フラグを立てる */
		}
		/* オンかつ未設定 */
		/* 1996.6.5.Chankan */
		if( Rec_chankan_man == MJDefine.NONE ){					/* 搶カンじゃなかった */
			flush_pos = 0;
		} else{
//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
			flush_f = false;
//@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
		}
	}
	else if( ( syori == false ) && ( flush_f == true ) ){	//( syori == OFF )
		flush_f = false;						/* フラグをおろす */
	}
}

//		/****************************END OF FILE************************************/

//-*********************paidisp.j
}
