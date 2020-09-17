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
// MJ_GameCommand.j
//-*****************
public partial class MahjongBase : SceneBase {
/**
 * @file  MJ_Draw.c
 *
 * @brief ゲーム描画実装
 */
//#include "MahJongRally.h"

public void NetActTimeOutJdg( /*MahJongRally * pMe*/ )
{
#if	Rule_2P
#else
	if( !IS_NETWORK_MODE )	return;
#endif

	// 結果タイムアウト
	if( gNextTimeOut > 0 )
		gNextTimeOut -= (long)uptimems_perfrm;

	if( gNextTimeOut <= 0) {
		gAutoFlag = 0x01;
		gNextTimeOut = 0;
	}
}

/****************************************************************/
/*		メッセージ表示											*/
/****************************************************************/
/*	IN)		nothing												*/
/*	OUT)	nothing												*/
/****************************************************************/
public void	DispInfo( /*MahJongRally * pMe*/ )
{
	if( menu_mode != 0) {
		#if false //-*todo:サウンド
		SeOnPlay( SE_TAHAI, SEQ_SE);
		#endif //-*todo:サウンド
		menu_mode = 9;
		m_haiCsrNo = -1;
		chi_hai_csr = 0;
	}
}
/****************************************************************/
/*		メニュー選択											*/
/****************************************************************/
/*	IN)		nothing												*/
/*	OUT)	手牌番号(未決定:-1)									*/
/****************************************************************/
public short TsumoMenuSel( /*MahJongRally * pMe*/ )
{

	short no, max, dat = 0;

	max = (short)Optcnt;						//選択できるオプションのマックス数設定
	no = menu_csr;								//現状選択しているオプション番号設定

	//オプションがない場合は処理なしで返す
	if( Optcnt == 0 ){
		return (-1);
	}
#region UNITY_ORIGINAL
		if(m_callMenuSelF || gAutoFlag != 0) {
			//-*DebLog(("menu_csr(TsumoMenuSel) = "+menu_csr));
			no = m_callMenuCsrNo;
			m_callMenuSelF = false;
			menu_csr = no;
			selFlg = true;
			// > 2006/02/26 要望No.107
			if ( (menu_csr == 0) || (Optstk[no] == (byte)OP.TSUMO) ){
				hai_csr = (short)gpsPlayerWork.byThcnt;	// 牌カーソルを初期値に戻す
			}
			// ResetCallMenu();	//-*メニューボタンの削除
			return (short)(Optstk[no]);
		}
#endregion //-*UNITY_ORIGINAL
	do{
		#if false //-*todo:キー操作
		if( D_ONEPUSH_INPUT_DOWN ) {
			dat = 1;
		} else {
			if( D_ONEPUSH_INPUT_UP ){
				dat = -1;
			}else{
				break;
			}
		}
		#else
#region GAME_PAD
	if(IsKeyAxisButton(KEY_NAME.DOWN,true)){
		dat = 1;
	}else{
		if( IsKeyAxisButton(KEY_NAME.UP,true) ){
			dat = -1;
			//TRACE("hai_csr = %d\n",hai_csr);
		}else{
			break;
		}
	}
#endregion //-*GAME_PAD
		#endif //-*todo:キー操作
//		do{
			no += dat;
			if( no < 0) {
				no = (short)(max-1);
			} else{
				if( no >= max){
					no = 0;
				}
			}
//		}while( 0 );

#if true //-*todo:不要
		menu_csr = no;
		// > 2006/02/26 要望No.107
		if ( (menu_csr == 0) || (Optstk[no] == (byte)OP.TSUMO) )
			hai_csr = (short)gpsPlayerWork.byThcnt;	// 牌カーソルを初期値に戻す
#endif //-*todo:不要
#if false //-*tod:サウンド
		// < 2006/02/26 要望No.107
		SeOnPlay( SE_ATACK, SEQ_SE);
#endif //-*tod:サウンド
	}while(false);		//0

	return((short)Optstk[no]);	//決定ではなくて自動にする

}

/****************************************************************/
/*		メニュー選択											*/
/****************************************************************/
/*	IN)		nothing												*/
/*	OUT)	手牌番号(未決定:-1)									*/
/****************************************************************/
public short MenuSel( /*MahJongRally * pMe*/ )
{
	short no, max, dat=0;

	// 2006.2.19 No115
	if( Optcnt == 0 )
		return( -1);
	// 2006.2.19 No115

	max = (short)Optcnt;
	no = menu_csr;

	#if false//-*todo:キー操作
	if( ( D_ONEPUSH_INPUT_SELECT ) || gAutoFlag != 0){
		return (short)(Optstk[no]);
	}
	#else //-*todo:キー操作
#region GAME_PAD
	if(IsKeyBtnPress(KEY_NAME.SELECT,true) || gAutoFlag != 0) {
		return (short)(Optstk[no]);
	}
#endregion //-*GAME_PAD

#region UNITY_ORIGINAL
		if(m_callMenuSelF || gAutoFlag != 0) {
			//-*DebLog(("menu_csr(MenuSel) = "+hai_csr));
			no = m_callMenuCsrNo;
			m_callMenuSelF = false;
			menu_csr = no;
			ResetCallMenu();	//-*メニューボタンの削除
			// selFlg = true;
			selFlg = true;
			if ( (menu_csr == 0) || (Optstk[no] == (byte)OP.TSUMO) ){	// 2006/02/26 要望No.107
				hai_csr = (short)gpsPlayerWork.byThcnt;	//	牌カーソルを初期値に戻す。
			}
			return (short)(Optstk[no]);
		}
#endregion //-*UNITY_ORIGINAL
	#endif //-*todo:キー操作
	do{
		#if false//-*todo:キー操作
		if( D_ONEPUSH_INPUT_DOWN ){
			dat = 1;
			//TRACE("hai_csr = %d\n",hai_csr);
		}else{
			if( D_ONEPUSH_INPUT_UP ){
				dat = -1;
				//TRACE("hai_csr = %d\n",hai_csr);
			}else{
				break;
			}
		}
		#else
#region GAME_PAD
	if(menu_mode_sub == 0){
		if(IsKeyAxisButton(KEY_NAME.DOWN,true)){
			dat = 1;
		}else{
			if( IsKeyAxisButton(KEY_NAME.UP,true) ){
				dat = -1;
				//TRACE("hai_csr = %d\n",hai_csr);
			}else{
				break;
			}
		}
	}
#endregion //-*GAME_PAD
		#endif //-*todo:キー操作
//		do{
			no += dat;
			if( no < 0)
				no = (short)(max-1);
			else
				if( no >= max)
					no = 0;

		//}while( !MenuChk( no));

		menu_csr = no;
//		if(menu_csr == 0)
#if true //-*todo:不要
		if ( (menu_csr == 0) || (Optstk[no] == (byte)OP.TSUMO) )	// 2006/02/26 要望No.107
			hai_csr = (short)gpsPlayerWork.byThcnt;	//	牌カーソルを初期値に戻す。
#endif //-*todo:不要

#if false //-*todo:サウンド
		SeOnPlay( SE_ATACK, SEQ_SE);
#endif //-*todo:サウンド
		//-*DebLog(("menu_csr = "+menu_csr));
	}while(false);	//0

	if(Optstk[no]!= (byte)OP.TAPAI/* && selFlg*/)
		return (short)(Optstk[no]);

	return( -1);			//決定で移動
	// return(Optstk[no]);	//決定ではなくて自動にする
}

/****************************************************************/
/*		手牌パッド選択											*/
/****************************************************************/
/*	IN)		nothing												*/
/*	OUT)	手牌番号(未決定:-1)									*/
/****************************************************************/
public short HaiSelPD( /*MahJongRally * pMe*/)
{
	short no, add=0;
	play_wk.byThcnt = gsPlayerWork[game_player].byThcnt;

	no = hai_csr;
	//if( pd_trg & GAME_OK_BTN) return( no);
	#if true //-*todo:キー操作
		// スタンドアローン。

#region UNITY_ORIGINAL
#if true
		if(m_haiSelF || gAutoFlag != 0) {
			//-*DebLog(("hai_csr = "+hai_csr));
			no = m_haiCsrNo;
			selFlg = true;
			m_haiSelF = false;
			// if(test)return (no);
			if(( (hai_on & (1 << no)) == 0)){
				if(hai_csr != no && hai_csr != 14){
					hai_csr = no;
				}else{
					return (no);
				}
			}
		}
#else
		if(m_haiSelF || gAutoFlag != 0) {
			//-*DebLog(("hai_csr = "+hai_csr));
			no = m_haiCsrNo;
			selFlg = true;
			if(( (hai_on & (1 << no)) == 0)){
				m_haiSelF = false;
				hai_csr = no;
				return (no);
			}
		}
#endif
#endregion //-*UNITY_ORIGINAL
	#else //-*todo:通信キー操作

	// ツモった牌を捨てる時の決定キー。
	//if( WhichDrawMode == D_DRAW_NETWORK_MODE )
	if( IS_NETWORK_MODE ) {
		// 通信対戦時はチャットメニューとの兼ね合いを避けるためにフラグ管理。
		if( ((D_ONEPUSH_INPUT_SELECT) && ChatFlag==OFF) || gAutoFlag != 0) {
			//-*DebLog(("hai_csr = %d\n",hai_csr));
			ResetCountDown();
			return (no);
		}
	} else{
		// スタンドアローン。
		if( (D_ONEPUSH_INPUT_SELECT) || gAutoFlag != 0) {
			//-*DebLog(("hai_csr = %d\n",hai_csr));
			return (no);
		}
	}
	#endif //-*todo:保留通信キー操作
	do{
	#if true //-*todo:キー操作
#region GAME_PAD
	if(IsKeyBtnPress(KEY_NAME.SELECT,true) || gAutoFlag != 0){
		selFlg = true;
		return (no);
	}
		// if( D_ONEPUSH_INPUT_RIGHT ){
		// 	add = 1;
		// }else{
		// 	if( D_ONEPUSH_INPUT_LEFT ){
		// 		add = -1;
		// 	}else{
		// 		break;
		// 	}
		// }
	if(IsKeyAxisButton(KEY_NAME.RIGHT,true)){
		add = 1;
	}else{
		if( IsKeyAxisButton(KEY_NAME.LEFT,true) ){
			add = -1;
			//TRACE("hai_csr = %d\n",hai_csr);
		}else{
			break;
		}
	}

#endregion //-*GAME_PAD

		do{

			no += add;
			if( no < 0){
				no = (short)play_wk.byThcnt;
			}else{
				if( no > play_wk.byThcnt){
					no = 0;
				}
			}
		} while( (hai_on & (1 << no)) != 0);
	#endif //-*todo:キー操作

	#if false //-*todo:サウンド
		SeOnPlay( SE_ATACK, SEQ_SE);
	#endif //-*todo:サウンド
		hai_csr = no;
	}while(false);	//0
//-****************************
	// if( IS_NETWORK_MODE ) {
	// 	// 通信対戦時はチャットメニューとの兼ね合いを避けるためにフラグ管理。
	// 	if( ((D_ONEPUSH_INPUT_SELECT) && ChatFlag==OFF) || gAutoFlag != 0) {
	// 		DTRACE(("hai_csr = %d\n",hai_csr));
	// 		ResetCountDown();
	// 		return (no);
	// 	}
	// } else{
	// 	// スタンドアローン。
	// 	if( (D_ONEPUSH_INPUT_SELECT) || gAutoFlag != 0) {
	// 		DTRACE(("hai_csr = %d\n",hai_csr));
	// 		return (no);
	// 	}
	// }
	// do{
	// 	if( D_ONEPUSH_INPUT_RIGHT ){
	// 		add = 1;
	// 	}else{
	// 		if( D_ONEPUSH_INPUT_LEFT ){
	// 			add = -1;
	// 		}else{
	// 			break;
	// 		}
	// 	}
	// 	do{
	// 		no += add;
	// 		if( no < 0){
	// 			no = (SINT2)play_wk.byThcnt;
	// 		}else{
	// 			if( no > play_wk.byThcnt){
	// 				no = 0;
	// 			}
	// 		}
	// 	} while( (hai_on & (1 << no)) != 0);

	// 	hai_csr = no;
	// }while(false);	//0
//-****************************
	return( -1);
}


/****************************************************************/
/*		カウントダウンカウンタリセット							*/
/****************************************************************/
/*	IN)															*/
/*	OUT)														*/
/****************************************************************/
public void ResetCountDown()
{
	gTsumoTimeOut = 0;
	gNakiTimeOut = 0;
	gNextTimeOut = 0;
}

/****************************************************************/
/*		チー選択牌取得											*/
/****************************************************************/
/*	IN)		no:		選択番号									*/
/*	OUT)	選択肢数											*/
/****************************************************************/
public short GetChiHai( /*MahJongRally * pMe,*/ int no)
{
	short	l_ChiPai;

	if( Chipsf[no] == 0) return( 0);

	l_ChiPai = (short)((1 << Chips1[no]) | (1 << Chips2[no]));

	return( l_ChiPai );
}

/****************************************************************/
/*		カン選択牌取得											*/
/****************************************************************/
/*	IN)		no:		選択番号									*/
/*	OUT)	選択肢数											*/
/****************************************************************/
public short GetKanHai( /*MahJongRally * pMe,*/ int no)
{
	short dat;
	int pos;

	pos = (int)Kanpos[no];
	switch( Kanflg[no]){
		case 1:
			dat = (short)(0x1 << pos);
			break;

		case 3:
			dat = (short)((0x7 << pos) | (1 << play_wk.byThcnt));
			break;

		case 4:
			dat = (short)(0xF << pos);
			break;

		default:
			dat = 0;
			break;
	}

	return( dat);
}

/****************************************************************/
/*		選択可能牌設定											*/
/****************************************************************/
/*	IN)		type:	状況										*/
/*	OUT)	選択肢数											*/
/****************************************************************/
public short SetHaiOn( /*MahJongRally * pMe,*/ short type)
{
	short dat, cnt;
	int i;

	hai_on = 0;
	cnt = 0;
	do{
		if( type != (short)OP.TAPAI){
//			if( game_menu != 0) break;
			type += (short)((menu_csr == (short)Optcnt) ? (short)OP.TAPAI : Optstk[menu_csr]);
		}

		switch( type){
			case (short)OP.RICHI:
				for( i = 13; i >= 0; i--) {
					hai_on <<= 1;
					if( Ricbuf[i] != 0) {
						hai_on |= 1;
						NakiSelNo = (byte)i;
						cnt++;
					}
				}
				break;

			case (short)OP.CHI:
			case (short)OP.CHI + 64:
				for( i = 2; i >= 0; i--){
					dat = GetChiHai( i );
					if( dat != 0){
						hai_on |= dat;
						NakiSelNo = (byte)i;
						cnt++;
					}
				}
				break;

			case (short)OP.PON:
			case (short)OP.PON + 64:
				hai_on = (short)(0x3 << Ponpos);
				break;

			case (short)OP.KAN:
				i = 0;
				//> 2006/03/14
//				do{
				//> 2006/03/14
				for( i = SubMj.Kannum-1; i >= 0; --i )
				{
					dat = GetKanHai( i );
					if( dat != 0){
						hai_on |= dat;
						NakiSelNo = (byte)i;
						cnt++;
					}
				//> 2006/03/14
					//i++;
				//> 2006/03/14
				}
				//> 2006/03/14
//				}while( i < (int)Kannum);
				//> 2006/03/14
				break;

			case (short)OP.KAN + 64:
				hai_on = (short)(0x7 << Ponpos);
				break;

			case (short)OP.TAPAI:
				if( play_wk.bFrich != 0){
					hai_on = (short)(1 << play_wk.byThcnt);
					break;
				}
				goto default;
			default:
				continue;
		}
		hai_on ^= 0x3FFF;
	}while(false);	//0

	return( cnt);
}

/****************************************************************/
/*		手牌鳴き選択											*/
/****************************************************************/
/*	IN)		tbl:	有効フラグ格納テーブル						*/
/*			max:	有効範囲									*/
/*	OUT)	フラグ(決定:0 未決定:-1 変動も無し:-2)				*/
/****************************************************************/
//xxxxSINT2	NakiSelPD( /*MahJongRally * pMe,*/ UINT1 tbl[], SINT2 max)
//xxxx{
//xxxx	SINT2 no;
//xxxx
//xxxx	if( ( D_ONEPUSH_INPUT_SELECT ) || gAutoFlag != 0)
//xxxx	{
//xxxx		return (0);
//xxxx	}
//xxxx
//xxxx	no = (SINT2)NakiSelNo;
//xxxx	do{
//xxxx		if( D_ONEPUSH_INPUT_RIGHT )
//xxxx		{
//xxxx			if( (--no) < 0)
//xxxx			{
//xxxx				no += max;
//xxxx			}
//xxxx		}else if( D_ONEPUSH_INPUT_LEFT )
//xxxx		{
//xxxx			if( (++no) == max)
//xxxx			{
//xxxx				no = 0;
//xxxx			}
//xxxx		}else
//xxxx		{
//xxxx			return( -2);
//xxxx		}
//xxxx	}while( tbl[no] == 0);
//xxxx
//xxxx	if( NakiSelNo != no)
//xxxx	{
//xxxx		NakiSelNo = (BYTE)no;
//xxxx		SeOnPlay( SE_ATACK, SEQ_SE);
//xxxx	}
//xxxx	return( -1);
//xxxx}

//> 2006/03/14 No.179 複数槓選択時に左カーソルが効かない
public short NakiSelPD_R( /*MahJongRally * pMe,*/ byte[] tbl, short max)
{
	short no;
#if true //-*todo:保留 キー操作
#region GAME_PAD
	if(IsKeyBtnPress(KEY_NAME.SELECT,true) || gAutoFlag != 0) {
		return (0);
	}
	no = (short)NakiSelNo;
	do{
		if(IsKeyAxisButton(KEY_NAME.RIGHT,true)){
			if( (++no) == max)
			{
				no = 0;
			}
		}else if( IsKeyAxisButton(KEY_NAME.LEFT,true) ){
			if( (--no) < 0)
			{
				no += max;
			}
		}else
		{
			return( -2);
		}
	}while( tbl[no] == 0);

	if( NakiSelNo != no)
	{
		NakiSelNo = (byte)no;
		#if false	//-*todo:サウンド
		SeOnPlay( SE_ATACK, SEQ_SE);
		#endif	//-*todo:サウンド
	}
#endregion //-*GAME_PAD
		return (-1);
#else  //-*todo:保留 キー操作
	if( ( D_ONEPUSH_INPUT_SELECT ) || gAutoFlag != 0 )
	{
		return (0);
	}

	no = (short)NakiSelNo;
	do{
		if( D_ONEPUSH_INPUT_RIGHT )
		{
			if( (++no) == max)
			{
				no = 0;
			}
		}else if( D_ONEPUSH_INPUT_LEFT )
		{
			if( (--no) < 0)
			{
				no += max;
			}
		}else
		{
			return( -2);
		}
	}while( tbl[no] == 0);

	if( NakiSelNo != no)
	{
		NakiSelNo = (byte)no;
		SeOnPlay( SE_ATACK, SEQ_SE);
	}
	return( -1);
#endif //-*todo:保留 キー操作

}
//> 2006/03/14 No.179 複数槓選択時に左カーソルが効かない

/****************************************************************/
/*		メニュー選択											*/
/****************************************************************/
/*	IN)		menu:	キャンセル時のメニュー番号					*/
/*	OUT)	手牌番号(未決定:-1)									*/
/****************************************************************/
public short	HaiSelM( /*MahJongRally * pMe,*/ short menu)
{
	short	no= -1, old;

#if DEBUG
if( menu_mode_sub_keep!= menu_mode_sub) {
	menu_mode_sub_keep= menu_mode_sub;
#if	__MJ_CHECK
Debug.Log("menu_mode_sub: "+ menu_mode_sub);
#endif
}
#endif
#if true //-*移植中
	// no = HaiSelPD(true);
	no = HaiSelPD();

	if( no < 0) {
		do {
			old = menu_csr;
			int	ret= MenuSel();
menu_ret = (short)ret;
			if(selFlg) {
				//ツモ ロン 九種九牌 次の決定をスキップする
				if((ret== (int)OP.TSUMO) || (ret== (int)OP.RON) || (ret== (int)OP.PON) || (ret== (int)OP.TAOPAI))
					return hai_csr;
				//明カン 次の決定をスキップする
				if((ret== (int)OP.KAN) && (menu_type== 1))
					return hai_csr;
			}
			if( old == menu_csr)
				break;

			menu_mode = menu;
		}while(false);	//0
	}
	if( menu_mode_sub== 0) {
		//上下だけ
		if( (selFlg) || gAutoFlag != 0){
			menu_mode_sub= 1;
		}
	}
#else //-*
#if	Rule_2P
	#if false //-*todo:保留キー操作
	if( menu_mode_sub== 0) {
		//上下だけ
		if( (D_ONEPUSH_INPUT_SELECT) || gAutoFlag != 0){
			menu_mode_sub= 1;
		}
		KeyInfo.trigger&= ~(KEY_SELECT2 | KEY_RIGHT2 | KEY_LEFT2);
	} else {
		//左右決定だけ
		KeyInfo.trigger&= ~(KEY_UP2 | KEY_DOWN2);
	}
	#endif //-*todo:保留キー操作
#endif

//#if	Rule_2P
#if		false
	if( menu_mode_sub== 0) {
		if( (D_ONEPUSH_INPUT_SELECT) || gAutoFlag != 0)
			menu_mode_sub= 1;
		do {
			old = menu_csr;
			MenuSel();
			if( old == menu_csr)	break;

			menu_mode = menu;
		}while(false);
	} else
		no = HaiSelPD();
#else
	no = HaiSelPD();

	if( no < 0) {
		do {
			old = menu_csr;
			int	ret= MenuSel();
			if(selFlg) {
				//ツモ ロン 九種九牌 次の決定をスキップする
				if((ret== (int)OP.TSUMO) || (ret== (int)OP.RON) || (ret== (int)OP.PON) || (ret== (int)OP.TAOPAI))
					return hai_csr;
				//明カン 次の決定をスキップする
				if((ret== (int)OP.KAN) && (menu_type== 1))
					return hai_csr;
			}
			if( old == menu_csr)
				break;

			menu_mode = menu;
		}while(false);	//0
	}
#endif
#endif //-*

	return( no);
}

#if DEBUG
static short menu_mode_keep= 99;
static short menu_mode_sub_keep= 99;
#endif
#if	Rule_2P
static short menu_mode_sub;
static bool _menuFlg= false;
static bool selFlg;
#endif

/****************************************************************/
/*		ツモ選択												*/
/****************************************************************/
/*	IN)		nothing												*/
/*	OUT)	コマンド番号(未決定:-1)								*/
/****************************************************************/
public short TehaiSel( /*MahJongRally * pMe*/ )
{
	short no;
	int i;

	do{

#if DEBUG
if( menu_mode_keep!= menu_mode) {
	menu_mode_keep= menu_mode;
#if	__MJ_CHECK
Debug.Log("menu_mode 1: "+ menu_mode);
#endif
}
#endif

#if	Rule_2P

	selFlg = false;
	#if false //-*todo:キー操作
	selFlg= D_ONEPUSH_INPUT_SELECT;
	if( Optcnt != 0 ) {
		if( menu_mode_sub== 0) {
			if( selFlg || gAutoFlag != 0)
				menu_mode_sub= 1;
			KeyInfo.trigger&= ~(KEY_SELECT2 | KEY_RIGHT2 | KEY_LEFT2);
		} else
			KeyInfo.trigger&= ~(KEY_UP2 | KEY_DOWN2);
	}
	#else
#region GAME_PAD
	if(IsKeyBtnPress(KEY_NAME.SELECT,true)){
		selFlg = true;
	}
	if( Optcnt != 0 ) {
		if( menu_mode_sub== 0) {
			if( selFlg || gAutoFlag != 0){
				menu_mode_sub= 1;
			}
			// KeyInfo.trigger&= ~(KEY_SELECT2 | KEY_RIGHT2 | KEY_LEFT2);
		} else{
			// KeyInfo.trigger&= ~(KEY_UP2 | KEY_DOWN2);
		}
	}
#endregion //-*GAME_PAD

	#endif //-*todo:キー操作
#endif

		switch( menu_mode) {
			case -1:	//画面構成前
				return( -1);

//==========================================================
			case 0: //準備
#if	Rule_2P
				menu_csr = 0;
				hai_up = 0;
				menu_mode_sub= 0;
#endif
//				paintF= true;
				// 通信用設定
	#if false //-*todo:通信
				if( (sGameData.byGameMode == GAMEMODE_NET_FREE)||
					(sGameData.byGameMode == GAMEMODE_NET_RESERVE) ) {
					//時間設定
					gTsumoTimeOut = (int)( tsumo_wait_param * ( 20 - gMyTable_Rank )) + tsumo_wait_base;
					gAutoFlag = 0x00;
				}
	#endif //-*todo:通信

				i = Optcnt;
				hai_csr = (short)play_wk.byThcnt;
				if( (opt_game[(int)GOPT.AUT] == (byte)GOPT_AUT.ON) && play_wk.bFrich != 0 && ( i == 0)) {
					NakiSelNo = (byte)hai_csr;
	#if false //-*todo:サウンド
					SeOnPlay( SE_TAHAI, SEQ_SE);
	#endif //-*todo:サウンド
					return( (byte)OP.TAPAI);
				}

//				game_wait = 60;
//				game_menu = 0;
				if( i != 0 && ( first_ron == 0)){
					do{
						i--;
						if( Optstk[i] == (byte)OP.TSUMO ){
							first_ron = 1;
							break;
						}

					}while( i != 0);

					if( first_ron != 0){
						menu_csr = (short)(Optcnt-1);
						menu_mode = 3;
						first_ron = 0;
						break;
					}
				}

#if	Rule_2P
#else
				menu_csr = 0;
				hai_up = 0;
//#if	Rule_2P
//				menu_mode_sub= 0;
//#endif
#endif
				goto case 1;
			case 1: //再準備
				#if true //-*todo:描画
				paintF= true;
				#endif //-*todo:描画
				if(Optcnt==0) {
					menu_mode = 4;
					SetHaiOn( (short)OP.TAPAI );

					// > 2006.05.12 No.1055 暫定対処
					hai_on = (short)(0xffff << (play_wk.byThcnt+1));
					hai_csr = (short)play_wk.byThcnt;
					// < 2006.05.12 No.1055

					continue;
				} else {
//					game_menu = 0;
					menu_mode = 4;

					// > 2006.05.12 No.1055 暫定対処
					hai_on = (short)(0xffff << (play_wk.byThcnt+1));
					hai_csr = (short)play_wk.byThcnt;
					// < 2006.05.12 No.1055

					continue;
				}

//==========================================================
			case 2:	//捨牌選択
				// > 2006.05.12 No.1055 暫定対処
				hai_on = (short)(0xffff << (play_wk.byThcnt+1));
				// < 2006.05.12 No.1055

#if		false
				if(( Optcnt != 0 ) && ( == 0)) {
					if( (D_ONEPUSH_INPUT_SELECT) || gAutoFlag != 0)
						menu_mode_sub= 1;

					no = TsumoMenuSel();
					if(no != menu_ret)
						menu_mode = 4;

					hai_up = 0;
					continue;
				} else {
					no = HaiSelPD();

					hai_up = (short)(1 << hai_csr);

					if( no < 0)
						continue;
				}
#else
				no = HaiSelPD();

				hai_up = (short)(1 << hai_csr);

				if( no < 0) {
					no = TsumoMenuSel();

					if(no != menu_ret) {
						menu_mode = 4;
						continue;
					}
					hai_up = 0;
					continue;
				}
#endif
#if false	//-*todo:サウンド
				SeOnPlay( SE_TAHAI, SEQ_SE);
#endif //-*todo:サウンド
				NakiSelNo = (byte)no;
				menu_ret = (short)OP.TAPAI;
				menu_mode = 9;
				continue;

//==========================================================
			case 3:
#if true	//-*todo:描画
				paintF= true;		//鳴きメニューからカーソル位置を戻す時
#endif //-*todo:描画
				hai_up = 0;
				menu_mode++;
				continue;

//==========================================================
			case 4:	// メニュー選択
				no = TsumoMenuSel();
				menu_ret = no;
				SetHaiOn( (short)0 );

				if((menu_ret == -1) && ( Optcnt == 0))
					no = (short)OP.TAPAI;

				do{
					switch( no){
						case -1:
							break;
						case (short)OP.TAPAI:
							menu_mode = 2;
							break;
						case (short)OP.RICHI:
							menu_mode++;	//5
							hai_csr = (short)NakiSelNo;
							break;
						case (short)OP.KAN:
							menu_mode = 6;
							break;
						default:
							menu_mode = 7;
							break;
					}
				} while(false);	//0
				break;
//==========================================================
			case 5: //リーチ捨牌
				hai_up = (short)(1 << hai_csr);
				no = HaiSelM( (short)3 );	//menu_mode
				if( no < 0)		break;

				NakiSelNo = (byte)no;

#if	Rule_2P
				if( menu_mode != 0)
					menu_mode = 9;
#else
				DispInfo();	//9
#endif
				break;

//==========================================================
			case 6: //カン選択
				hai_csr = (short)Kanpos[NakiSelNo];
				no = HaiSelM( (short)3 );	//menu_mode
//				//-*DebLog(("KAN1 no=%d,hai_csr=%d,NakiSelNo=%d,Kanpos[NakiSelNo]=%d",no,hai_csr,NakiSelNo,Kanpos[NakiSelNo]));

				i = 0;
				do{
					hai_up = GetKanHai( i );
					if(( hai_up & (1 << hai_csr)) != 0)
						break;
					i++;
				}while( i < (int)SubMj.Kannum);
				//> 2006/03/14 No.179 複数槓選択時に左カーソルが効かない
				//NakiSelNo = i;
				//< 2006/03/14 No.179 複数槓選択時に左カーソルが効かない
//				//-*DebLog(("KAN2 no=%d,hai_csr=%d,NakiSelNo=%d,Kanpos[NakiSelNo]=%d",no,hai_csr,NakiSelNo,Kanpos[NakiSelNo]));

				if( no < 0){
					i = (int)SubMj.Kannum;
					if( i == 0)		i++;
					//> 2006/03/14 No.179 複数槓選択時に左カーソルが効かない
					// 何故か逆周りに動くので、その逆に動く関数を作って対応
					no = NakiSelPD_R( Kanflg, (short)i);
					//> 2006/03/14 No.179 複数槓選択時に左カーソルが効かない
					if( no == -2)	break;

					hai_up = GetKanHai( (int)NakiSelNo);
				}
				if( no >= 0)	DispInfo();	//9
				break;

//==========================================================
			case 7: //ツモ選択
				no = HaiSelM( (short)1);	//menu_mode
				if( no < 0)
					break;

				if( menu_ret!= (short)OP.TAOPAI)		/* たおす 九種九牌 */
					DispInfo();	//9
				else	//九種九牌時に、"牌を捨てる"音を鳴らさない
					if( menu_mode != 0)
						menu_mode = 9;

#if true	//-*todo:
				//-*DebLog(("TSUMO SELECT"));
#endif	//-*todo:
				break;

//==========================================================
			case 9:	//終了
				hai_up = 0;
				hai_on = 0;
//				tsumo_ofs = TSUMO_ADD;
				menu_mode++;		//10
				continue;

			case 10:
				menu_mode = 0;
				gTsumoTimeOut = 0;
				gAutoFlag = 0x00;
				menu_mode_sub= 0;
				return( menu_ret);
		}
//		game_wait--;
	}while(false);	//0

//	if( ( game_wait == 0) && (menu_mode != 9)){}
#region  UNITY_ORIGINAL
	if( Optcnt != 0 ) {
		if( menu_mode_sub== 0) {
			if( selFlg || gAutoFlag != 0){
				menu_mode_sub= 1;
			}
		}
	}
#endregion //-*UNITY_ORIGINAL
	return( -1);
}

/****************************************************************/
/*		鳴き選択												*/
/****************************************************************/
/*	IN)		nothing												*/
/*	OUT)	コマンド番号(未決定:-1)								*/
/****************************************************************/
public short NakiSel( /*MahJongRally * pMe*/ )
{
//	SINT2 no, cnt;
//	int i, sel1, sel2;
	short no, index;

#if true	//-*todo:キー操作
	// selFlg = false;
#else //-*todo:キー操作
	selFlg= D_ONEPUSH_INPUT_SELECT;
#endif 	//-*todo:キー操作

	do{
#if DEBUG
if( menu_mode_keep!= menu_mode) {
	menu_mode_keep= menu_mode;
#if	__MJ_CHECK
Debug.Log("menu_mode 2: "+ menu_mode);
#endif
}
#endif
		switch( menu_mode){
			case -1:	//画面構成前
				continue;

//==========================================================
			case 0: //準備
				// 通信用設定

#if false	//-*todo:通信
				if( (sGameData.byGameMode == GAMEMODE_NET_FREE)||
					(sGameData.byGameMode == GAMEMODE_NET_RESERVE) ) {
					//時間設定
					gNakiTimeOut = (int)( naki_wait_param * ( 20 - gMyTable_Rank )) + naki_wait_base;
					gAutoFlag = 0x00;
				}
//				game_wait = 60;
#endif 	//-*todo:通信

				// > 2006/02/16 No.101
				menu_csr = 0;
				// 選択肢にロンがあればカーソル初期値を合わせる
				{
					byte	i;
					for ( i = 0 ; i < Optcnt ; i++ ) {
						if ( Optstk[i] == (byte)OP.RON ) {
							menu_csr = (short)i;
							break;
						}
					}
				}
				// < 2006/02/16 No.101
//				game_menu = 0;
				hai_up = 0;
				goto case 1;
			case 1:	//再準備
				menu_mode = 2;
#region UNITY_ORIGINAL
menu_mode_sub = 0;
#endregion //-*UNITY_ORIGINAL
				menu_ret = -1;
				hai_up = 0;
				break;

//==========================================================
			case 2:	//メニュー選択
				no = MenuSel();
				menu_ret = no;
				SetHaiOn( (short)64 );

				// 2006.2.19 No115
				if((menu_ret == -1) && ( Optcnt == 0))
					no = (short)OP.TAPAI;

				// 2006.2.19 No115

				switch( no){
					case (short)OP.TAPAI:
#if false	//-*todo:サウンド
						SeOnPlay( SE_KIUTI, SEQ_SE);
#endif //-*todo:サウンド
						menu_mode = 9;
						goto case -1;
					case -1:
						break;

					case (short)OP.CHI:
						hai_csr = 14;
						hai_up = GetChiHai( (int)NakiSelNo );
						menu_mode++;	//3
						break;

					case (short)OP.PON:
					case (short)OP.KAN:
						hai_up = (short)(0x3FFF ^ hai_on);
						menu_mode = 4;
						break;

					default:
						hai_up = (short)(0x3FFF ^ hai_on);
					goto case (short)OP.RON;
					case (short)OP.RON:
						menu_mode = 5;
						break;
				}
				break;

//==========================================================
			case 3: //チー選択
				no = HaiSelM( (short)1);	//menu_mode

				// 2006/02/06 BugNo 30
				hai_csr = 14;

				for( index = 0; index <= 2; ++index )
					if( Chipsf[index] == 255 ) {
						chi_csr = index;
						chi_min = index;
						break;
					}

				for( index = 0; index <= 2; ++index )
					if( Chipsf[index] == 255 ) {
						Chipsf[index] = 254;
						chi_max = index;
					}

				index = chi_csr;
#if true	//-*todo:キー操作
				#if true
				if(no != -1 && m_haiCsrNo != -1){
					if(chi_hai_csr < m_haiCsrNo){
						if( chi_min <= chi_csr && chi_csr < chi_max  ){
							++chi_csr;
						}else{
							// chi_csr = chi_min;	//-*カーソルループは無し
						}
					}else if(chi_hai_csr > m_haiCsrNo){
						if( chi_min < chi_csr && chi_csr <= chi_max  ){
							--chi_csr;
						}else{
							// chi_csr = chi_max;	//-*カーソルループは無し
						}
					}
					// chi_hai_csr = m_haiCsrNo;
					chi_hai_csr = m_haiCsrNo;
				}
				#endif
#region GAME_PAD
	if(IsKeyAxisButton(KEY_NAME.RIGHT,true)){
		// menu_mode_sub = 0;
		if( chi_min <= chi_csr && chi_csr < chi_max  ){
			++chi_csr;
		}else{
			chi_csr = chi_min;
		}
	}else　if( IsKeyAxisButton(KEY_NAME.LEFT,true) ){
		// menu_mode_sub = 0;
		if( chi_min < chi_csr && chi_csr <= chi_max  ){
			--chi_csr;
		}else{
			chi_csr = chi_max;
		}
	}

#endregion //-*GAME_PAD
#else	//-*todo:キー操作
				if( D_ONEPUSH_INPUT_RIGHT ) {
					if( chi_min <= chi_csr && chi_csr < chi_max  )
						++chi_csr;
					else
						chi_csr = chi_min;
				} else if( D_ONEPUSH_INPUT_LEFT ) {
					if( chi_min < chi_csr && chi_csr <= chi_max  )
						--chi_csr;
					else
						chi_csr = chi_max;
				}
#endif 	//-*todo:キー操作

				if( Chipsf[chi_csr] != 0 ) {
					hai_up = 0;
					hai_up = (short)(hai_up | (short)(1 << Chips1[chi_csr]));
					hai_up = (short)(hai_up | (short)(1 << Chips2[chi_csr]));

					NakiSelNo = (byte)chi_csr;
				} else {
					chi_csr = index;
					NakiSelNo = (byte)chi_csr;
				}
#region UNITY_ORIGINAL

	#if false//-*todo:キー操作
				if( chi_hai_csr == m_haiCsrNo)
				{
					 DispInfo();	//9
				}
	#else
				// 2006/02/06 BugNo 30
				if( no >= 0) DispInfo();	//9
	#endif //-*todo:キー操作

#endregion //-*UNITY_ORIGINAL
				break;

//==========================================================
			case 4: //ポン、カン選択
				no = HaiSelM( (short)1);	//menu_mode
				if( no < 0)		break;
				DispInfo();	//9
				break;

//==========================================================
			case 5: //ロン選択
				no = HaiSelM( (short)1);	//menu_mode
				if( no < 0)		break;
				DispInfo();	//9
				break;

//==========================================================
			case 9:	//終了
				hai_up = 0;
				hai_on = 0;
				menu_mode++;
				continue;
			case 10:
				menu_mode = 0;
//				menu_mode_sub= 0;
				gNakiTimeOut = 0;
				gAutoFlag = 0x00;

				return( menu_ret);
		}

	}while(false);	//0

	return( -1);
}
//-*********************MJ_GameCommand.j
}
