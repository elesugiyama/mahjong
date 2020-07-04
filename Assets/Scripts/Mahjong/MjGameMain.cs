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
// MJ_GameMain.j
//-*****************
public partial class MahjongBase : SceneBase {
/**
 * @file  MJ_GameMain.c
 *
 * @brief ゲームメイン実装
 */

//#include "MahJongRally.h"								// Module interface definitions

// 通信対戦中のキャラクターの顔チェンジ。

/****************************************************************/
/*		ゲームイベント設定										*/
/****************************************************************/
/*	IN)		type:	イベント番号								*/
/*			par:	パラメータ									*/
/*	OUT)	nothing												*/
/****************************************************************/
public void	GameEvent( /*MahJongRally * pMe,*/ short type, short par)
{
	int sts, no, w, ofs;
	int i;

	// 再接続中は無視
	if( is_reentry == true )
		return;

	do{
		switch( type){
			case (short)GEV.NAKI_PON:			//ポン
				if( is_reentry == false ) {
//					if( Order != game_player )
					{
						gTalkFlag=1;

						// スタンドアロン版。
						gTalkWait[gTalkCnt]  = MJDefine.D_TALK_WAIT_PON;
						gTalkHouse[gTalkCnt] = (byte)Order;
						gTalkChar[gTalkCnt]  = gpsTableData.sMemData[Order].byMember;
						gTalkType[gTalkCnt]  = (byte)85;
						gTalkCnt=(byte)((gTalkCnt+1) % MJDefine.D_TALK_TABLE_MAX);
					}
					#if false //-*todo:サウンド
					WaveSeOnPlay(Order,(byte)OP.PON);
					#endif //-*todo:サウンド
				}
				no = 3;
				ofs = 0;
				break;

			case (short)GEV.NAKI_KAN:			//カン
				{
//					if(Order != game_player )
					{
						gTalkFlag=1;

						gTalkWait[gTalkCnt]  = MJDefine.D_TALK_WAIT_KAN;
						gTalkHouse[gTalkCnt] = (byte)Order;
						gTalkChar[gTalkCnt]  = gpsTableData.sMemData[Order].byMember;
						gTalkType[gTalkCnt]  = (byte)87;

						gTalkCnt=(byte)((gTalkCnt+1) % MJDefine.D_TALK_TABLE_MAX);
					}
					#if false //-*todo:サウンド
					WaveSeOnPlay(Order,OP_KAN);
					#endif //-*todo:サウンド
				}
				no = 4;
				break;

			case (short)GEV.NAKI_CHI:			//チー
				{
//					if( Order != game_player )
					{
						gTalkFlag=1;

						gTalkWait[gTalkCnt]  = MJDefine.D_TALK_WAIT_CHI;
						gTalkHouse[gTalkCnt] = (byte)Order;
						gTalkChar[gTalkCnt]  = gpsTableData.sMemData[Order].byMember;
						gTalkType[gTalkCnt]  = (byte)86;

						gTalkCnt=(byte)((gTalkCnt+1) % MJDefine.D_TALK_TABLE_MAX);
					}
					#if false //-*todo:サウンド
					WaveSeOnPlay(Order,OP_CHI);
					#endif //-*todo:サウンド
				}

				no = 2;
				ofs = 0;
				break;

			case (short)GEV.NAKI_RICHI:		//リーチ
				{
//					if( Order != game_player )
					{
//						gTalkFlag=1;

						gTalkWait[gTalkCnt]  = MJDefine.D_TALK_WAIT_RICHI;
						gTalkHouse[gTalkCnt] = (byte)Order;
						gTalkChar[gTalkCnt]  = gpsTableData.sMemData[Order].byMember;
						gTalkType[gTalkCnt]  = (byte)84;

						gTalkCnt=(byte)((gTalkCnt+1) % MJDefine.D_TALK_TABLE_MAX);
					}
					// > 2006/02/23 リーチSEタイミング正規化
					if ( gTalkFlag== 0 ) {		//!gTalkFlag
						gTalkFlag = 1;
						#if false //-*todo:サウンド
						WaveSeOnPlay( Order, OP_RICHI);
						#endif //-*todo:サウンド
					}
					// < 2006/02/23
				}

				no = 5;
				w = 5;
				break;

			case (short)GEV.NAKI_RON: {		//ロン
//					if(Order != game_player )
					{
						gTalkFlag=1;

						gTalkWait[gTalkCnt]  = MJDefine.D_TALK_WAIT_RON;
						gTalkHouse[gTalkCnt] = (byte)Order;
						gTalkChar[gTalkCnt]  = gpsTableData.sMemData[Order].byMember;
						gTalkType[gTalkCnt]  = (byte)89;

						gTalkCnt=(byte)((gTalkCnt+1) % MJDefine.D_TALK_TABLE_MAX);
					}
					#if false //-*todo:サウンド
					// 音楽停止
					PlayMusic_2chStop(  );
					WaveSeOnPlay(Order,OP_RON);
					#endif //-*todo:サウンド
					tsumo_agari = false;	// ツモ和了フラグOFF
//					PlayMusic_PlaySE( D_SOUND_FRIKOMI_SE);	// 2006/02/24 要望No.113
				}
				sts = 2;
//m				no = 1;

				// 牌をあける
				no = ((par - game_player) & 0x3);
#if	Rule_2P
				hai_open|= (short)((8>> no) & 0x0F);		//ロン時 牌をあける
				#if false //-*todo:サウンド
				myCanvas.playSE(SN_SE05);		//手牌を倒す
				#endif //-*todo:サウンド
#else
//				hai_open = (1<<no)&0x0f;
				// ダブロン／トリロン対応（複数の家がオープンされる可能性があるため、フラグを論理和する） 2006/02/25 No.141
				hai_open |= (1<<no)&0x0f;		//ロン時 牌をあける
#endif
				break;

			case (short)GEV.NAKI_TUMO: {			//ツモ
//					if( Order != game_player  )
					{
						gTalkFlag=1;

						gTalkWait[gTalkCnt]  = MJDefine.D_TALK_WAIT_TSUMO;
						gTalkHouse[gTalkCnt] = (byte)Order;
						gTalkChar[gTalkCnt]  = gpsTableData.sMemData[Order].byMember;
						gTalkType[gTalkCnt]  = (byte)88;

						gTalkCnt=(byte)((gTalkCnt+1) % MJDefine.D_TALK_TABLE_MAX);
					}
					#if false //-*todo:サウンド
					// 音楽停止
					PlayMusic_2chStop(  );

					// 自摸発声
					WaveSeOnPlay(Order,OP_TSUMO);
					#endif //-*todo:サウンド
					tsumo_agari = false;	// ツモ和了フラグON 2006/02/24 要望No.113

					// 和了音楽再生
//					PlayMusic_PlaySE( D_SOUND_AGARI_SE);	// 2006/02/24 要望No.113
				}

				sts = 2;
				no = 0;

				// 牌をあける
				no = ((par - game_player) & 0x3);
#if	Rule_2P
				hai_open= (short)((8>> no) & 0x0F);		//ツモ時 牌をあける
				#if false //-*todo:サウンド
				myCanvas.playSE(SN_SE05);				//手牌を倒す
				#endif //-*todo:サウンド
#else
				hai_open = (short)((1<<no)&0x0f);		//ツモ時 牌をあける
#endif
				break;

			case (short)GEV.SYS_RYUKYOKU:		//流局
				no = 0;

				if( is_reentry == false )
				#if false //-*todo:サウンド
					// 音楽停止
					PlayMusic_2chStop(  );
				#endif //-*todo:サウンド
#if	Rule_2P
				for( i= 0; i< MJDefine.MAX_TABLE_MEMBER; i++ ) {
					no<<= 1;
					if(( gsPlayerWork[(i + game_player) & 0x3].byTenpai)!= 0)
						no|= 0x01;
				}
#else
				for( i = 3; i >= 0; i--) {
					no <<= 1;
					if(( gsPlayerWork[(i + game_player) & 0x3].byTenpai)!= 0)
						no |= 0x01;
				}
#endif
				//携帯用追加処理
				//九種九牌時にオープン牌を上書きするため保護処理を実装。
				//実装の仕方を検討していないため問題があるかも
				if(hai_open== 0) {
					hai_open= (short)no;	//流局テンパイ時 牌をあける
#if	Rule_2P
					if(no!= 0){
						#if false //-*todo:サウンド
						myCanvas.playSE(SN_SE05);		//手牌を倒す
						#endif //-*todo:サウンド
					}
#endif
				}
				continue;

			case (short)GEV.SYS_9SYU9HAI:		//九種九牌
				if( is_reentry == false ) {
					//> 2006/02/09 要望 No86
					// プレイヤーに喋らせる事により対応
					//if(Order != game_player )
					//< 2006/02/09
					{
						gTalkFlag=1;

						gTalkWait[gTalkCnt]= MJDefine.D_TALK_WAIT;	//2000;//(2000/D_FLAME_TIME);
						gTalkHouse[gTalkCnt] = (byte)Order;
						gTalkChar[gTalkCnt] = gpsTableData.sMemData[Order].byMember;
						gTalkType[gTalkCnt] = (byte)90;

						gTalkCnt=(byte)((gTalkCnt+1) % MJDefine.D_TALK_TABLE_MAX);
					}

					#if false //-*todo:サウンド
					// 音楽停止
					PlayMusic_2chStop(  );
					#endif //-*todo:サウンド
				}

				// 牌をあける
				no = 1 << ((par - game_player) & 0x3);
				no = ((par - game_player) & 0x3);
#if	Rule_2P
				hai_open= (short)((8>> no) & 0x0F);	//九種九牌時 牌をあける
				if( hai_open!= 0){
					#if false //-*todo:サウンド
					myCanvas.playSE(SN_SE05);		//手牌を倒す
					#endif //-*todo:サウンド
				}
#else
				hai_open = (SINT2)((1<<no)&0x0f);	//九種九牌時 牌をあける
#endif
				continue;

			case (short)GEV.SYS_TENPAI: {
				// テンパイ。
				if( is_reentry == false ) {
					gTalkFlag=1;

					gTalkWait[gTalkCnt]= MJDefine.D_TALK_WAIT;	//2000;//(2000/D_FLAME_TIME);
					gTalkHouse[gTalkCnt] = (byte)Order;
					gTalkChar[gTalkCnt] = gpsTableData.sMemData[Order].byMember;
					gTalkType[gTalkCnt] = (byte)93;

					gTalkCnt= (byte)((gTalkCnt+1) % MJDefine.D_TALK_TABLE_MAX);
					#if false //-*todo:サウンド
					// 音楽停止
					PlayMusic_2chStop(  );
					#endif //-*todo:サウンド
				}

				// 牌をあける
				no = 1 << ((par - game_player) & 0x3);
				no = ((par - game_player) & 0x3);
#if Rule_2P
				hai_open= (short)((8>> no) & 0x0F);		//テンパイ時 牌をあける
				#if false //-*todo:サウンド
				myCanvas.playSE(SN_SE05);				//手牌を倒す
				#endif //-*todo:サウンド
#else
				hai_open = (SINT2)((1<<no)&0x0f);		//テンパイ時 牌をあける
#endif
				continue;
			}
			case (short)GEV.SYS_4FURENDA:		//四風連打
			case (short)GEV.SYS_4KAIKAN:		//四開槓
				if( is_reentry == false ) {
					// プレイヤー以外ならしゃべらせる
					//> 2006/02/09 要望 No86
					// if(Order != game_player )
					//< 2006/02/09 要望 No86
					{
						gTalkFlag=1;

						gTalkWait[gTalkCnt]  = MJDefine.D_TALK_WAIT;	//2000;//(2000/D_FLAME_TIME);
						gTalkHouse[gTalkCnt] = Order;
						gTalkChar[gTalkCnt]  = gpsTableData.sMemData[Order].byMember;
						switch ( type )
						{
							case (short)GEV.SYS_4FURENDA : gTalkType[gTalkCnt] = (byte)91; break;
							case (short)GEV.SYS_4KAIKAN : gTalkType[gTalkCnt]	= (byte)92; break;
						}
						gTalkCnt=(byte)((gTalkCnt+1) % MJDefine.D_TALK_TABLE_MAX);
					}

					no = 0;

				#if false //-*todo:サウンド
					// 音楽停止
					PlayMusic_2chStop(  );
				#endif //-*todo:サウンド
				}
				continue;

			case (short)GEV.SYS_TORIRON:		//トリロン
				break;	// ロンした面子のオープンフラグは既に立っているはず 2006/02/25 No.141
			case (short)GEV.SYS_4CYA_REACH:	//四家立直
//m				no = 0x0F;
				// 牌オープンフラグ反映 2006/02/25 No.141
				hai_open = (short)0x0F;		//四家立直時 牌をあける
#if false //-*todo:サウンド
#if	Rule_2P
				myCanvas.playSE(SN_SE05);	//手牌を倒す
#endif
#endif //-*todo:サウンド
				continue;

			default:
				return;
		}
		return;
	}while(false);		//while(0);
}

/****************************************************************/
/*		ゲーム画面準備											*/
/****************************************************************/
/*	IN)		nothing												*/
/*	OUT)	nothing												*/
/****************************************************************/
public void	GameSetting( /*MahJongRally * pMe*/ )
{
	play_wk = (gsPlayerWork[game_player]);	//&(gsPlayerWork[game_player])

//	game_sts = 0;
	menu_mode = 0;
	hai_up = 0;
	hai_on = 0;

	chi_csr = 0;	// 2006/02/06 BugNo 30
	chi_min = 0;
	chi_max = 0;

	hai_csr = 14;	//20051117 add
	gTalkCnt = 0;
	gTalkNo = 0;
	gTalkFlag = 0;

	gTsumoTimeOut = 0;	//20051122 add
	gNakiTimeOut = 0;
	gNextTimeOut = 0;

	//gAutoFlag = MJ_Get_gAutoFlag(  );
	gAutoFlag = 0x00;

	hai_open = 0;		//初期化
#if _DEBUG
	/* デバック時は手牌がわかるように0x0Fにする*/
	if( dbgopt[0] == 0 )
		hai_open = 0x0F;		//デバック時
#endif
#if	__OPEN_PAI
	hai_open = 0x0F;	//aaaa
#endif
}

/****************************************************************/
/*		一局開始時初期化処理									*/
/****************************************************************/
/*	IN)		nothing												*/
/*	OUT)	nothing												*/
/****************************************************************/
public void	GameReset( /*MahJongRally * pMe*/ )
{
	//tamaki MjMain();
	GameSetting();
	first_ron = 0;
//	tsumo_ofs = TSUMO_ADD;
}

/****************************************************************/
/*		麻雀再開時初期化処理									*/
/****************************************************************/
/*	IN)		nothing												*/
/*	OUT)	nothing												*/
/****************************************************************/
public void	GameResume( /*MahJongRally * pMe*/ )
{
	GameSetting();
}

/****************************************************************/
/*		麻雀開始時初期化処理									*/
/****************************************************************/
/*	IN)		nothing												*/
/*	OUT)	nothing												*/
/****************************************************************/
void	GameLink( /*MahJongRally * pMe*/ )
{
	GameResume();
	first_ron = 0;
//	tsumo_ofs = TSUMO_ADD;
}

/****************************************************************/
/*		麻雀開始時初期化処理									*/
/****************************************************************/
/*	IN)		nothing												*/
/*	OUT)	nothing												*/
/****************************************************************/
public void	GameInit( /*MahJongRally * pMe*/ )
{
	GameResume();
	first_ron = 0;
	gGamePointFlag = true;	// 得点表示のフラグ初期化。

	// ここで行わないと、結果画面が表示されてしまう。
	agari_sts = 0;		//0422

	// １巡に１回しか送信出来ないようにチャットフラグを設定。
	// TRUE->送信可。FALSE->送信不可。
	#if false //-*todo:通信
	if( IS_NETWORK_MODE ){
		gChatSendFlag = true;
	}
	#endif //-*todo:通信

}

/*=================================== File End ===================================*/
//-*********************MJ_GameMain.j
}
