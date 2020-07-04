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
// MJ_GameDraw.j
//-*****************
public partial class MahjongBase : SceneBase {
/**
 * @file  MJ_GameDraw.c
 *
 * @brief ゲーム描画実装
 */

//#include "MahJongRally.h"								// Module interface definitions


/****************************************************************/
/*		DEFINE 宣言												*/
/****************************************************************/
public const int D_YOUBOU_NO_80_		= 16;
public const int D_MARK_OPTION_BASE_X	= 23;
public const int D_MARK_OPTION_BASE_Y	= (225+(-2));//-*(225+_y_of02);
// 和了した後の役表示のベース座標とOFFSET値
public const int D_YAKU_DISP_BASE_X		 = 15;		//35
public const int D_YAKU_DISP_BASE_Y		 = 87;		//140
public const int D_YAKU_DISP_OFFSET_X	 = 112;		//90
public const int D_YAKU_DISP_OFFSET_Y	 = 12;		//20

// 清算時のポイント＆サシウマ・ＴＩＰ・ヤキトリの座標。
//public const int D_END_POINT_BASE_X		110
//public const int D_END_POINT_BASE_Y		40
//public const int D_END_POINT_BG_X		(D_END_POINT_BASE_X+60)
public const int D_END_POINT_BASE_X		 = 170;
public const int D_END_POINT_BASE_Y		 = 40;
public const int D_END_POINT_BG_X		 = (D_END_POINT_BASE_X-60);

// 符。翻。子 or 親。満貫 or 跳萬 or 倍萬 or 三倍萬 or 役満 or 数え役満の文字のOFFSET
public const int D_AGARI_TEXT_DISP_BASE_X	= 60;
public const int D_AGARI_TEXT_DISP_BASE_Y	= 220;	// 06/05/31 No.1099

// 本場の文字のOFFSET
public const int D_AGARI_HONBA_TEXT_DISP_BASE_Y	= 240;	// 06/05/31 No.1099

// 合計の文字のOFFSET
public const int D_AGARI_GOUKEI_TEXT_DISP_BASE_Y	= 260;	// 06/05/31 No.1099

public const int D_TOTAL_POINT_X		= 110;
public const int D_TOTAL_POINT_Y		= 75;

public const string D_WORD_RECONNECT	= "ゲームに";
public const string D_WORD_RECONNECT_	= "復帰しました";

public const string D_WORD_DISCONNECT	= "回線が不安定の";
public const string D_WORD_DISCONNECT_	= "為切断されました";


	/// <summary>
	/// @brief 顔の描画(見つからないときは影を表示)
	/// </summary>
public void DrawCharacterFace( /*MahJongRally * pMe,*/ int sprite_, int x_, int y_ )
// public void DrawCharacterFace( /*MahJongRally * pMe,*/ SpriteInfo sprite_, int x_, int y_ )
{
#if	Rule_2P
#else
	if( sprite_ == NULL )
		sprite_ = spChar[MJ_CHAR_SILHOUETTE];
	else
		if( sprite_.pIImage == NULL )
			sprite_ = spChar[MJ_CHAR_SILHOUETTE];

	MpsBrewLib_DrawSprite( sprite_, x_, y_ );
#endif
}

#if false //-*todo:描画保留
public void MJ_GameMarkDraw( SpriteInfo sprite_, int x_, int y_ )
{
	MpsBrewLib_DrawSprite( sprite_, x_, y_ + _y_of02);
}
#endif //-*todo:描画保留
/*===========================================================================
	ゲームの描画全般
===========================================================================*/
#if	Rule_2P
static int		FaceNum= 0;		//キャラクター表情
#endif
public void MJ_GameDraw( /*MahJongRally * pMe*/ )
{
	DebLog("//-*MJ:GameDraw.cs:MJ_GameDraw()");
	int		House=0;	// 誰から表示をするか
	// 対家位置描画
	House = ( game_player + 1) % (int)D_CYA.D_MENTSU_COUNT;
	MJ_ToicyaDraw( House );
	// 自家位置描画
	House = ( game_player + 0) % (int)D_CYA.D_MENTSU_COUNT;
	MJ_JicyaDraw( House );
#if false //-*todo:描画保留

	int		House=0;	// 誰から表示をするか
#if Rule_2P
	int[] _rule = {
		D_RULE00,		//ノーマル戦
		D_RULE01,		//鳴き禁止
		D_RULE02,		//点数ハンデ戦[-10000点/15000vs35000]
		D_RULE03,		//二翻縛り
		D_RULE04,		//ロン・リーチ禁止
		D_RULE05,		//ロン禁止
		D_RULE06,		//リーチ禁止
		D_RULE02,		//点数ハンデ戦[-5000点/20000vs30000]
		//******ウキウキ:ルール追加*******
		D_RULE02,		//点数ハンデ戦[-15000点/10000vs40000]
		//********************************
	};
#else
	MJ_GameCountDown();	//0507mt
#endif

	if(( agari_sts!= (byte)SEQ.AG_YAKU) && ( agari_sts!= (byte)SEQ.AG_NEXT_RESULT)) { //0511mt if( paintF)
	// 背景
	vscreen.setColor(MAKE_RGB( 49, 102,	90));
	vscreen.fillRect(0, 0, dispW, dispH);
//m	MpsBrewLib_DrawSprite( spGame[D_GAME_JYANTAKU], 82, 97 );		//0, 0

//zeniya
	// 背景の描画

	MpsBrewLib_DrawSprite( spGame[D_GAME_JYANTAKU], 0, 0);

	/*情報バー*/
	mMpsBrewLib_DrawSprite( spGame[D_SCORE_00], 0, 0 );
	mMpsBrewLib_DrawSprite( spGame[D_SCORE_10], 0, 240 );

	/*ルール表示*/
	if( myCanvas.mah_limit_num!= -1) {
		mMpsBrewLib_DrawSprite( spGame[D_SCORE_11], 125, 242 );		//ルール表示枠（右下ピンク）:そらみず
		mMpsBrewLib_DrawSprite( spGame[_rule[myCanvas.mah_limit_num]], 130, 244 );
	}

#if	Rule_2P
	//キャラクター表示
//***********ウキウキ:表情操作************
//*	mMpsBrewLib_DrawSprite( spGame[D_FACE_00], 167, 65 );
//*	if( FaceNum!= 0){
//*		mMpsBrewLib_DrawSprite( spGame[D_FACE_00+ FaceNum], 188, 89 );	//キャラクター表情:萌
//*	}
	mMpsBrewLib_DrawSprite( spGame[D_CHAR_FACE_00+ FaceNum], 167, 65 );	//*キャラ表情
	mMpsBrewLib_DrawSprite( spGame[D_CAHR_FRAME], 164, 62 );	//*キャラ表示用枠
//****************************************
	//鳴きメニュー選択中は、牌を上に上げない
	if( _menuFlg)
		hai_up = 0;

	// 対家位置描画
	House = ( game_player + 1) % D_MENTSU_COUNT;
	MJ_ToicyaDraw( House );
#else
	// 下家位置描画
	House = ( game_player + 1) % D_MENTSU_COUNT;
	MJ_ShimocyaDraw( House );

	// 対家位置描画
	House = ( game_player + 2) % D_MENTSU_COUNT;
	MJ_ToicyaDraw( House );

	// 上家位置描画
	House = ( game_player + 3) % D_MENTSU_COUNT;
	MJ_KamicyaDraw( House );
#endif

#if	__MJ_CHECK
	// アイコン描画
	{
		if ( gMyTable_NakiNashi == D_OPTION_NAKINASHI_ON )	//1
			// 鳴きなしマーク
			MJ_GameMarkDraw( SPMark[D_MARK_NAKI], D_MARK_OPTION_BASE_X+(19*0), D_MARK_OPTION_BASE_Y);

		if ( opt_game[GOPT_TLK] == GOPT_TLK_OFF || ( IS_NETWORK_MODE && gDaiuchiFlag != D_ONLINE_OPERATOR_MANUAL ) )
			// セリフ無しマーク
			MJ_GameMarkDraw( SPMark[D_MARK_SERIFU], D_MARK_OPTION_BASE_X+(19*1), D_MARK_OPTION_BASE_Y);

		if ( IS_NETWORK_MODE ) {
			if ( gDaiuchiFlag == D_ONLINE_OPERATOR_PASS )
				// 代打ち・ツモ切り - "ツモ切"
				MJ_GameMarkDraw( SPMark[D_MARK_TSUMOGIRI], D_MARK_OPTION_BASE_X+(19*2), D_MARK_OPTION_BASE_Y);
			else
				if ( gDaiuchiFlag == D_ONLINE_OPERATOR_AI )
					// 代打ち・AI - "AI"
					MJ_GameMarkDraw( SPMark[D_MARK_AI], D_MARK_OPTION_BASE_X+(19*2), D_MARK_OPTION_BASE_Y);

		}
	}
#endif

	// 自家位置描画
	House = ( game_player + 0) % D_MENTSU_COUNT;
	MJ_JicyaDraw( House );

	// 王牌の描画。
	MJ_GameWanpaiDisp( );

	// 影描画（重い。削除しても問題なし）
//m	MJ_EffectShadow();

	// 何局目・サイコロ・残り牌数・点棒
	MJ_GameSozaiDisp( );

#if	Rule_2P
	// リーチ棒
	if( RiboDispFlg[0]!= 0)
		mMpsBrewLib_DrawSprite( spGame[D_REACH_S], 10, 197);
	if( RiboDispFlg[1]!= 0)
		mMpsBrewLib_DrawSprite( spGame[D_REACH_S], 10,  55);
#endif

	// ゲーム中のメニュー描画。
	MJ_GameMenuDraw( );

	// > 2006/02/21 No.124
	// 流局を描画
	if ( agari_sts == SEQ_AG_RYUKYOKU )
		MJ_RyukyokuDraw();
	// < 2006/02/21 No.124
	}

	// 対戦者の対話描画
	// リーチ、ツモ、ポン等
	MJ_TalkDraw();

	{ //0511mt if( paintF) {
	// ゲーム中の得点描画。
	MJ_GamePointDraw( );

#if	__MJ_CHECK
	//ゲーム中にオプションモードの切り替えを行う。(描画)
	MJ_GameModeChangeDraw( );
#endif

	if( WhichDrawMode == D_DRAW_NETWORK_MODE ) {
		// ゲーム中のチャットメニュー描画。
		MJ_GameChatDraw( );

		// 各プレイヤーから送られてきたメッセージの描画。
		MJ_GameChatTalkDraw( );

		// カウントダウン描画
		MJ_GameCountDownDraw( );

		// クリアキー押されたら強制タイトル。
		if( D_ONEPUSH_INPUT_CLEAR ) {
			// > 2006/02/04 No.68
			modeChange( D_USER_DISCONNECT_MODE );

			//コネクションを切断する
			MpsBrewLib_Release(/*m_brewMps*/);

			//0516mt m_brewMps.m_connectInit	  = false;

			//無操作監視タイマーリセット
//			ISHELL_SetTimerEx(GETMYSHELL(), TIME_WARN_DISCONNECT, idle_disconnect.timer_callback);
			// < 2006/02/04 No.68
		}

	}

	// 結果の表示
	switch(agari_sts) {
		case (byte)SEQ.AG_END2 : {
//			String szText;		//AECHAR

			MJ_DrawMenu( 8,100,12,5,NORMAL_MENU );

//			STREXPAND((const byte*)"対局を続けますか？",20,szText,sizeof(szText));
			MJ_Msg_CenterLine(	"対局を続けますか？", 0, 109, SYS_FONT_WHITE );

			// はい・いいえの描画。
			MJ_YesNo_Draw( 75,140,20 );
		}
		break;
		// > 2006/02/21 No.124
//		case SEQ_AG_RYUKYOKU : {
//			// 流局を描画
//			MJ_RyukyokuDraw( );
//			return;
//		}
//		break;
//		// < 2006/02/21 No.124
		case (byte)SEQ.AG_TENPAI: {
			// 流局後のテンパイ or ノーテン 描画。
			// テンパイ描画。
			MJ_TenpaiDraw( );
			removeSoftkey();
			return;
		}
		case (byte)SEQ.AG_YAKU:
		case (byte)SEQ.AG_NEXT_RESULT: {
			// 上がり役描画
			MJ_PointDraw();
			removeSoftkey();
			return;
		}
		case (byte)SEQ.AG_SCORE0:
		case (byte)SEQ.AG_SCORE1:
		case (byte)SEQ.AG_SCORE2: {
			// 清算描画
			MJ_ResultDraw();
			removeSoftkey();
			return;
		}
		default:
			break;
	}



	if( WhichDrawMode == D_DRAW_NETWORK_MODE ) {
// 0502mt ->
		if( mj_sts == 3 ) {
			if( reentry_m1_bflag != 5 && reentry_m1_bflag != 0xFF )	{

				// バグ修正 No1108 大塚 start
		 		int num = -1;
				if( opt_game[GOPT_TLK]==GOPT_TLK_ON && gChatSendFlag ) {
		 			if(getSoftNum(SOFTKEY1_X) != D_SOFTKEY_TALK){
 						num = D_SOFTKEY_TALK;
 					}
 				}
 				else if(getSoftNum(SOFTKEY1_X) != D_SOFTKEY_DESIDE){
 						num = D_SOFTKEY_DESIDE;
 				}

 				if(num != -1){
 					removeSoftkey(SOFTKEY1_X);
		 			sMpsBrewLib_DrawSprite( num, SOFTKEY1_X);
 				}

				if(getSoftNum(SOFTKEY2_X) != D_SOFTKEY_MARK){
			 		removeSoftkey(SOFTKEY2_X);
  					sMpsBrewLib_DrawSprite(D_SOFTKEY_MARK, SOFTKEY2_X);
				}
			}
		}
//0502mt <-
	} else if( mj_sts == 3 )
		if( reentry_m1_bflag != 5 && reentry_m1_bflag != 0xFF ) {
			sMpsBrewLib_DrawSprite( D_SOFTKEY_DESIDE, SOFTKEY1_X);
			sMpsBrewLib_DrawSprite( D_SOFTKEY_MARK,   SOFTKEY2_X);
		}
	}
//	return;
#endif //-*todo:描画保留
}

/*===========================================================================
							ゲーム中の得点描画
===========================================================================*/
// 第１引数：たらい回しポインタ。
// 第２引数：
public const int D_MARK_BASE_X = 5;
public const int D_MARK_BASE_Y = 3;
public const int D_MARK_OFFSET_Y = (13+D_MARK_BASE_Y);
public void MJ_GamePointDraw( /*MahJongRally * pMe*/ )
{
	DebLog("//-*MJ:GameDraw.cs:MJ_GamePointDraw");
#if false //-*todo:描画保留

	short[]		MenuPosX= { 50-D_YOUBOU_NO_80_, 75-D_YOUBOU_NO_80_, 50-D_YOUBOU_NO_80_,	5};
	short[]		MenuPosY= {200,140, 20, 80};
	short[]		PointPosX= {100,125,100, 55+D_YOUBOU_NO_80_};
	short[]		PointPosY= {225,165, 45,105};
	short[]		FacePosX= { 55,80, 55, 10+D_YOUBOU_NO_80_};
	short[]		FacePosY= {204,144, 24, 84};
	short[]		NamePosX= {110,135,110, 65+D_YOUBOU_NO_80_};
	short[]		NamePosY= {205,145, 25, 85};
	int				i			= 0;
	int				Seki		= 0;
	string			name;		//char	*name	= 0;
	short			Len			= 0;
	short			posX		= 0;
//	String			szText;		//AECHAR
	SpriteInfo		sprite_;


	//if( !D_INPUT_SOFT2 ) return;
	//if(!(GetKeyLev2(_KEY_SOFT2))) return;
	if( gGamePointFlag == true )	return;

	// チャットの文字描画は行なわない。
	ChatFlag = 0;

	//	  2
	// 3	 1
	//	  0
	//	(自分)
	for ( i = 0 ; i < 4 ; i++ ) {
		sprite_ = null;
//		szText= "";		//MEMSET( szText, 0, sizeof(szText));

		// 描画の場所を取得
		Seki = (game_player + i) % 4;

		name  = gsTableData[0].sMemData[Seki].NickName;	// 名前					//gsTableData

		if ( gsTableData[0].sMemData[Seki].byMember < MAX_COMP_CHARACTER ) {	//gsTableData
			sprite_ = spChar[gsTableData[0].sMemData[Seki].byMember];			//gsTableData
		}

		if ( WhichDrawMode == D_DRAW_NETWORK_MODE ) {
			// 通信対戦でプレイヤーの顔があれば入れ替え
			if ( (byte)gNetTable_NetChar[Seki].Flag != (byte)0xFF ) {
				sprite_ = gNetTable_NetChar[Seki].spriteChar;
				if( sprite_.pIImage == NULL ) {
					if ( gNetTable_NetChar[Seki].CharPicNo < MAX_COMP_CHARACTER ) {
						sprite_ = spChar[gNetTable_NetChar[Seki].CharPicNo];
					}
				}
			}
		}

		// --- メニュー描画
		_MJ_DrawMenu(MenuPosX[i],MenuPosY[i],9,2, NORMAL_MENU);

		// --- 親マーク描画
		if ( Seki == gpsTableData.byOya )	// 2006/02/21 No.119
			MpsBrewLib_DrawSprite(SPMark[D_MARK_OYA], MenuPosX[i]+D_MARK_BASE_X, MenuPosY[i]+D_MARK_BASE_Y);

		// --- 焼き鳥マーク描画
		if ( Rultbl[RL_YAKI]!= 0 )	// 2006/02/21 要望No.99
			if (( gpsTableData.sMemData[Seki].byMemFlags & TBLMEMF_TORI ) != 0)
				MpsBrewLib_DrawSprite(SPMark[D_MARK_YAKITORI], MenuPosX[i]+D_MARK_BASE_X, MenuPosY[i]+D_MARK_OFFSET_Y+2);

		// --- ワレメマーク描画
		if ( Rultbl[RL_WAREM]!= 0 ) {
			// ワレメ決定	// ← 何でここで？
			Wareme = (BYTE)((gpsTableData.byOya + Dicnum[0] + Dicnum[1] + 1) & 3);
			if ( Seki == Wareme )	// 2006/02/21 No.119
				MpsBrewLib_DrawSprite(SPMark[D_MARK_WAREME], MenuPosX[i] + D_MARK_BASE_X, MenuPosY[i]+(D_MARK_OFFSET_Y*2)+1);
		}

		// --- キャラクターの名前描画
		Len = (int16_t)MJ_GetStrLen( name );	//(AECHAR*)						// 名前の長さ取得
		posX = (int16_t)(NamePosX[i]+((90-Len)/2));	//w21t ? (NamePosX[i]+((90-Len)/2)) : (NamePosX[i]+((80-Len)/2));	// 表示位置算出
//		STREXPAND((const byte*)name,STRLEN(name), szText, sizeof(szText));		//
		MahJongRally_Msg_Line( name, posX, NamePosY[i], SYS_FONT_WHITE );		//szText

		// キャラクターの顔描画
		DrawCharacterFace( sprite_, FacePosX[i],FacePosY[i]);

		// 通信対戦中にCOMを表す画像表示
		if( WhichDrawMode == D_DRAW_NETWORK_MODE ) {
			// 通信対戦でプレイヤーの顔があれば入れ替え
			BOOL is_com_ = false;		//short
			if( gNetTable_NetChar[Seki].Flag == 0 && gNetTable_NetChar[Seki].AIFlag == 1 ) {
				// 切断時・かつAIフラグが立っている
				is_com_ = true;
			} else if( gNetTable_NetChar[Seki].Flag == 1 && gNetTable_NetChar[Seki].Operator == 2 ) {
				// 通信中・かつAIフラグが立っている
				is_com_ = true;
			} else if( (byte)gNetTable_NetChar[Seki].Flag == (byte)0xFF ) {
				// 普通にCOM
				is_com_ = true;
			}

			if( is_com_ == true ) {
				sprite_ = spChar[MJ_CHAR_COM];
				MpsBrewLib_DrawSprite( sprite_, FacePosX[i],FacePosY[i] );
			}
		}

		// 点数
		DAgariNumDisp( (gsTableData[0].sMemData[Seki].nPoint) * 100, 0, PointPosX[i], PointPosY[i], 7, FALSE, SYS_FONT_WHITE  );	//gsTableData
	}
//	return;
#endif //-*todo:描画保留
}

/*===========================================================================
	リザルト画面表示 (清算)
===========================================================================*/
public void MJ_ResultDraw( /*MahJongRally * pMe*/ )
{
	DebLog("//-*MJ:GameDraw.cs:MJ_ResultDraw()");
#if false//-*todo:描画保留

#if	Rule_2P
	int		House = (( gpsTableData.byOya+ 4- game_player) % 4) & 0x01;
	int		ypos= 137;

	//点移動ナシはスキップ (結果だけ表示する)
//	if(( gsTableData[0].sMemData[0].nMovePoint | gsTableData[0].sMemData[1].nMovePoint)== 0)
//		return;

	//ワク
	mMpsBrewLib_DrawSprite( spGame[D_END03_00], 50, 100 );
	//家
	mMpsBrewLib_DrawSprite( spGame[D_END03_02+ ((House+ 1) & 0x01)],	60, 109 );
	mMpsBrewLib_DrawSprite( spGame[D_END03_02+ House],					60, 134 );

	//得点
	for( int i = 0; i < MAX_TABLE_MEMBER2; i++ ) {
		int		Seki= ( game_player+ i) % 4;
		int		Point		= gsTableData[0].sMemData[Seki].nOldPoint;		// 現在の点数
		int		MovePoint	= gsTableData[0].sMemData[Seki].nMovePoint;		// 移動点数

		// 現在の点数
		if( Point< 0)
			mMpsBrewLib_DrawSprite( spGame[D_END03__], 79- 7* 0, ypos );
		AgariNumDisp( abs(Point* 100),
			79- 7* 1, ypos, 7, 7, false, D_END03_03- D_END01_18 );

		if( gNakiTimeOut== 1) {
			// 移動点数
			if( MovePoint> 0) {
				//増
				mMpsBrewLib_DrawSprite( spGame[D_END03_33], 131, ypos );
				AgariNumDisp( abs(MovePoint* 100),			131- 7* 1, ypos, 7, 7, false, D_END03_13- D_END01_18 );
			} else {
				//減
				mMpsBrewLib_DrawSprite( spGame[D_END03_34], 131, ypos );
				AgariNumDisp( abs(MovePoint* 100),			131- 7* 1, ypos, 7, 7, false, D_END03_23- D_END01_18 );
			}
		}
		ypos-= 25;
	}
#else
	int			i			= 0;
	int			Seki		= 0;
	short		Point		= 0;
	short		MovePoint	= 0;

	int16_t		MenuPosX[]		= { 50, 95,50,	5};
	int16_t		MenuPosY[]		= {200,140,20, 80};
	int16_t		PointPosX[]		= { 90,135,90, 45};
	int16_t		PointPosY[]		= {225,165,45,105};
	int16_t		FacePosX[]		= { 55,100,55, 10};
	int16_t		FacePosY[]		= {204,144,24, 84};

	int16_t		MovePointPosX[]= { 90,135,90,45};
	int16_t		MovePointPosY[]= {205,145,25,85};

	// チャットの文字描画は行なわない。
	ChatFlag = 0;
	gGamePointFlag = TRUE;

	// リザルト画面。
	for( i=0 ; i<4 ; i++ ) {
		SpriteInfo sprite_ = NULL;

		// 描画の場所を取得。
		Seki = (game_player+ i) % 4;

		if( gsTableData[0].sMemData[Seki].byMember < MAX_COMP_CHARACTER )	//gsTableData
			sprite_ = spChar[ gsTableData[0].sMemData[Seki].byMember ];		//gsTableData

		if( WhichDrawMode == D_DRAW_NETWORK_MODE ) {
			// 通信対戦でプレイヤーの顔があれば入れ替え
			if( (byte)gNetTable_NetChar[Seki].Flag != (byte)0xFF ) {
				sprite_ = gNetTable_NetChar[Seki].spriteChar;
				if( sprite_.pIImage == NULL ) {
					if( gNetTable_NetChar[Seki].CharPicNo < MAX_COMP_CHARACTER )
						sprite_ = spChar[gNetTable_NetChar[Seki].CharPicNo];
				}
			}
		}

		Point		= gsTableData[0].sMemData[Seki].nPoint;		// 現在の点数	//gsTableData
		MovePoint	= gsTableData[0].sMemData[Seki].nMovePoint;	// 移動点数		//gsTableData

		// メニュー
		MJ_DrawMenu( MenuPosX[i],MenuPosY[i],7,2, NORMAL_MENU );	// メニュー描画。

		// 顔描画。
		DrawCharacterFace( sprite_, FacePosX[i],FacePosY[i]);

		// 移動得点描画。
		DAgariNumDisp( MovePoint*100, 3, MovePointPosX[i], MovePointPosY[i]+2, 7, TRUE, SYS_FONT_WHITE  );

		// 現在の点数描画。
		DAgariNumDisp( Point*100,	   0, PointPosX[i], PointPosY[i]+2, 7, FALSE, SYS_FONT_WHITE	);
	}
//	return;
#endif
#endif //-*todo:描画保留
}


/****************************************
	和了下画面数値表示（最大８桁）
****************************************/
// 第1引数：引き回し。
// 第2引数：得点。
// 第3引数：自家・下家・対家・上家
// 第4引数：x座標。
// 第5引数：y座標
// 第6引数：桁数
// 第7引数：『+』『-』を付ける
public byte	DAgariNumDisp( int	 val, int pos, int	 dx, int   dy, int place, bool Flag, int color) {	return DAgariNumDisp( (int)val, (byte)pos, (int)dx, (int)dy, (byte)place, (bool)Flag, (int)color);	}
public byte	DAgariNumDisp( short val, int pos, int	 dx, int   dy, int place, bool Flag, int color) {	return DAgariNumDisp( (int)val, (byte)pos, (int)dx, (int)dy, (byte)place, (bool)Flag, (int)color);	}
public byte	DAgariNumDisp( int	 val, int pos, short dx, short dy, int place, bool Flag, int color) {	return DAgariNumDisp( (int)val, (byte)pos, (int)dx, (int)dy, (byte)place, (bool)Flag, (int)color);	}
public byte	DAgariNumDisp( short val, int pos, short dx, short dy, int place, bool Flag, int color) {	return DAgariNumDisp( (int)val, (byte)pos, (int)dx, (int)dy, (byte)place, (bool)Flag, (int)color);	}
public byte	DAgariNumDisp( /*MahJongRally * pMe,*/
					   int	val,
					   byte pos,
					   int	dx,
					   int	dy,
					   byte place,
					   bool	Flag,
					   int color)
{
#if true//-*todo:描画保留
	DebLog("DAgariNumDisp(int "+val+", byte "+pos+", int "+dx+", int "+dy+", byte "+place+", bool "+Flag+",	int "+color+" )");
	return 0;
#else//-*todo:描画保留

	short	len;
	short	i;
	short	j = 0;
	byte	minus_flg;
	short	ret = 0;
	byte[]	num_buf= new byte [8];
	int		x = 0;
	int		y = 0;

	switch(pos) {
		case	D_JICYA:
			x = dx;
			y = dy;
			break;
		case	D_SHIMOCYA:
			x = dx;
			y = dy;
			break;
		case	D_TOICYA:
			x = dx;
			y = dy;
			break;
		case	D_KAMICYA:
			x = dx;
			y = dy;
			break;
	}

	// 移動ポイントが『－』なら
	if( val < 0 ) {
		minus_flg=1;	// フラグON
		val*=-1;		// －の値をプラスに変えておく。
	} else
		minus_flg=0;

	// --- 初期化 ---
	num_buf[7] =
	num_buf[6] =
	num_buf[5] =
	num_buf[4] =
	num_buf[3] =
	num_buf[2] =
	num_buf[1] =
	num_buf[0] = 0;

	// --- 配列代入 ---
	if(val != 0) {
		val = val%100000000;
		num_buf[7] = (byte)((val/10000000)&0xff);
		val = val%10000000;
		num_buf[6] = (byte)((val/1000000)&0xff);
		val = val%1000000;
		num_buf[5] = (byte)((val/100000)&0xff);
		val = val%100000;
		num_buf[4] = (byte)((val/10000)&0xff);
		val = val%10000;
		num_buf[3] = (byte)((val/1000)&0xff);
		val = val%1000;
		num_buf[2] = (byte)((val/100)&0xff);
		val = val%100;
		num_buf[1] = (byte)((val/10)&0xff);
		val = val%10;
		num_buf[0] = (byte)(val&0xff);
	}

	for( i = 7; i >0; i--) {
		if( num_buf[i] == 0 )
			num_buf[i] = 0xFF;
		else
			break;
	}
		len = (short)(i+1);

		// 表示開始。
		for( j=0 ; j<len ; j++ ) {
			if( num_buf[j] != 0xFF )
				MahJongRally_Msg_Line( NumberText[num_buf[j]], dx+((place-j)*12), dy,color );		// 数字の描画。
		}

		len = (short)(place-j);
		/****************************************************/
		/*													*/
		/*				+ or - の記号の描画					*/
		/*													*/
		/****************************************************/
		if( minus_flg!= 0 )
			// 点数が0点以下なら『-』の描画。
			MahJongRally_Msg_Line( NumberText[D_SYSFONT_PULS+minus_flg], x+(len*12), y,color );
		else
			if( Flag == true )
				// 『+』の描画。
				MahJongRally_Msg_Line( NumberText[D_SYSFONT_PULS], x+(len*12), y,color );

		return((byte)ret);
#endif //-*todo:描画保留
}

/*===========================================================================
	和了の役表示
===========================================================================*/
public void MJ_PointDraw( /*MahJongRally * pMe*/ )
{
	DebLog("//-*MJ:GameDraw.cs:MJ_PointDraw()");
#if false//-*todo:描画保留
	int		i			= 0;		// ループ用&OFFSET用。
	int		yakuCount	= 0;		// 和了した手が何翻か？
	int		yakuNumber	= 0;		// 役の番号。
	int		yakuFactor	= 0;		// 役の翻数。
	int		FU			= 0;		// 符。
	int		HAN			= 0;		// 翻。
	int		RENCYAN		= 0;		// 何本場？
	int		TotalPoint	= 0;		// 和了した点数は？
	int		iOrder		= Order;	// 現在のオーナーを取得。
	int		offSetX		= 0;		//
	int		offSetY		= 0;		//
//m	int		DoraY		= 20;		// 裏ドラが無かった場合は-10しとく。
	int		DoraY		= 0;
	int		cnt			= 0;
	int		pai			= 0;
	int 	posX	= 0;		//
	int 	posY	= 0;		//
	short 	SetY	= 0;
	short	Rich	= (short)gpsPlayerWork.bFrich;	// リーチした場合。
	string		name;			//char name[D_NICK_NAME_MAX+1]	= {0};
	//< 2006/03/23 流し満貫、十三不塔対策	//0422
	bool	 isSpecialYaku = false;
	bool	 isNagashi	   = false;
	//< 2006/03/23 流し満貫、十三不塔対策
	MJK_RESULT result_ = gMJKResult;
//	BOOL	yakumanFlag = TRUE;// 2006/03/23 混乱の元なので削除		//0422

	if( SubMj.result_disp_no < Roncnt && Rultbl[ (int)RL.TWO_CHAHO ] != 0) {
		// 二家和ありなら、個別に成績を表示
		iOrder	= result_order[ SubMj.result_disp_no ];
		result_ = gMJKResultSub[ SubMj.result_disp_no ];
	}

	// 役満の場合は、翻と符を表示させない。
	// yakumanFlagがFALSEの時が役満なので注意

#if false //-*todo 通信
	// チャットの文字描画は行なわない。
	ChatFlag = 0;
#endif //-*todo 通信
	yakuCount = result_.byYakuCnt;

	FU			= (int)result_.byFu;		// 符。
	HAN			= (int)result_.byHan;		// 翻。
	RENCYAN		= (int)result_.byRenchan;	// 何本場？
	TotalPoint	= (int)result_.nTotalPoint;	// 和了した得点。

	// (テスト)和了した後はメニューを初期化させる。
	Optcnt = 0;
	gGamePointFlag = true;

	//if (result_.byYakuman != 0)		//0422
	//	yakumanFlag = FALSE;

	//> 2006/03/23 流し満貫、十三不塔対策		//0422
	for (i = 0; i < yakuCount ; i++)
		switch( result_.sYaku[i].name ) {
			case (byte)YK.YAOCH : isSpecialYaku = isNagashi = true;	break;
			case (byte)YK.SHISA : isSpecialYaku = true;				break;
		}

	//< 2006/03/23 流し満貫、十三不塔対策
#if	Rule_2P
//zeniya
	MpsBrewLib_DrawSprite( spGame[D_GAME_JYANTAKU], 0, 0 );
//	MpsBrewLib_DrawSprite( spGame[D_GAME_JYANTAKU], 0+ 120* 0, 0+ 130* 0 );
//	MpsBrewLib_DrawSprite( spGame[D_GAME_JYANTAKU], 0+ 120* 1, 0+ 130* 0 );
//	MpsBrewLib_DrawSprite( spGame[D_GAME_JYANTAKU], 0+ 120* 0, 0+ 130* 1 );
//	MpsBrewLib_DrawSprite( spGame[D_GAME_JYANTAKU], 0+ 120* 1, 0+ 130* 1 );
//	for(int work_width=0; work_width<4; work_width++) {
//		for(int work_height=0; work_height<6; work_height++) {
//			mMpsBrewLib_DrawSprite( spGame[D_GAME_JYANTAKU], 0+(60*work_width), 0+(48*work_height) );
//		}
//	}
//	MpsBrewLib_DrawSprite( spGame[D_GAME_JYANTAKU], 0, 0);


	MpsBrewLib_DrawSprite( spGame[D_END00_00], 4, 9 );			//役とドラの表示ウィンドウ

	mMpsBrewLib_DrawSprite( spGame[D_END01_04], 46, 29 );		//局 - 場表示（東一局東家）
	mMpsBrewLib_DrawSprite( spGame[D_END01_05], 72, 29 );		//家 - 場表示（東一局東家）
	mMpsBrewLib_DrawSprite( spGame[D_END01_06], 18, 49 );		//1000点棒
	mMpsBrewLib_DrawSprite( spGame[D_END01_17], 53, 49 );		//100点棒

	mMpsBrewLib_DrawSprite( spGame[D_END01_32], 86, 192 );		//得点表示の枠
#else
	MJ_DrawMenu( 0, 0, 13, 17, NORMAL_MENU );
#endif

	/****************************************************************/
	/*						和了した牌の描画						*/
	/****************************************************************/
	{
		//> 2006/03/24 流し満貫対策		//0422
		if( isNagashi == false ) {
		//< 2006/03/24 流し満貫対策
			cnt		= gsPlayerWork[iOrder].byThcnt;	// 手牌の数を取得。
			for( i=0 ; i<cnt ; i++ ) {
				pai = gsPlayerWork[iOrder].byTehai[i];
#if	Rule_2P
				hMpsBrewLib_DrawSprite( spPaiJicya, pai,
					D_JICYA_TEHAI_X+(i*(D_PAI_TATE_LARGE_SIZE_W-1)), 226 );		//10
#else
				MpsBrewLib_DrawSprite( spPaiJicya[pai],
					D_JICYA_TEHAI_X+(i*(D_PAI_TATE_LARGE_SIZE_W-1)), 226 );		//10
#endif
			}
#if	Rule_2P
			hMpsBrewLib_DrawSprite( spPaiJicya, Sthai,
				D_JICYA_TEHAI_X-3+((1+cnt)*(D_PAI_TATE_LARGE_SIZE_W-1)), 226 );		//10
#else
			MpsBrewLib_DrawSprite( spPaiJicya[Sthai],
				D_JICYA_TEHAI_X-3+((1+cnt)*(D_PAI_TATE_LARGE_SIZE_W-1)), 226 );		//10
#endif
			MJ_FrJicyaDraw( iOrder, 265 );		//D_AGARI_FR_DRAW_BASE_X
		}
	}

#if	Rule_2P
#else
	/****************************************************************/
	/*					和了したキャラクターの描画					*/
	/****************************************************************/
	{
		SpriteInfo sprite_ = NULL;

		int			face_x_ = 25;
		int			face_y_ = 40;
		int16_t		Len		= 0;
		BYTE		base_x	= 90;		//w21t ? 90 : 92;
		BYTE		max_len	= 90;		//w21t ? 90 : 80;
//		String		szText;		//AECHAR	szText[D_NICK_NAME_MAX+1] ={0};

		if( gsTableData[0].sMemData[iOrder].byMember < MAX_COMP_CHARACTER )		//gsTableData
			sprite_  = spChar[ gsTableData[0].sMemData[iOrder].byMember ];		//gsTableData

		// 名前コピー。
		name= (gsTableData[0].sMemData[iOrder].NickName);	//gsTableData	//STRNCPY( name, (gsTableData.sMemData[iOrder].NickName), D_NICK_NAME_MAX + 1 );	//(const char *)

		if( IS_NETWORK_MODE ) {
			// 通信対戦でプレイヤーの顔があれば入れ替え
			if( (byte)gNetTable_NetChar[iOrder].Flag != (byte)0xFF ) {
				sprite_ = gNetTable_NetChar[iOrder].spriteChar;
				if( sprite_.pIImage == NULL )
					if( gNetTable_NetChar[iOrder].CharPicNo < MAX_COMP_CHARACTER )
						sprite_ = spChar[gNetTable_NetChar[iOrder].CharPicNo];
			}
		}

		// --- キャラクターの名前描画。
		Len	 = (int16_t)MJ_GetStrLen( name );		//(AECHAR*)								// 名前の長さ取得。
		posX = (base_x+(((max_len)-Len)/2));	//w21t ? (base_x+(((max_len)-Len)/2)) : (base_x+(((max_len)-Len)/2));	// 表示位置算出。
//		STREXPAND((const byte*)name,STRLEN(name), szText, sizeof(szText));			//
		MahJongRally_Msg_Line( name, posX, face_y_+13, SYS_FONT_WHITE );			//szText

		DrawCharacterFace( sprite_, face_x_, face_y_ );
	}
#endif

	/****************************************************************/
	/*						ドラの描画								*/
	/****************************************************************/
	if( result_.byYakuman == 0 ) {	//&& isSpecialYaku == FALSE ) //> 2006/03/23 流し満貫・十三不塔対策	//0422
#if	Rule_2P
//		for( i= 0; i< 5; i++ )
//			hMpsBrewLib_DrawSprite( spSutePaiJicya, PAI_URA, 139+ i* 15, 17 );
		for( i=0 ; i< byDoraCnt ; i++ )
			hMpsBrewLib_DrawSprite( spSutePaiJicya, Dora[i], 139+ i* 15, 17 );
#else
		for( i= 0; i< 5; i++ )
//			MpsBrewLib_DrawSprite( spSutePaiJicya[PAI_URA], D_WanBASE_X+ (13* i), D_WanBASE_Y+ (7* 1));
//			MpsBrewLib_DrawSprite( spPaiJicya[PAI_URA], 139+ i* 16, 17 );
			MpsBrewLib_DrawSprite( spSutePaiJicya[PAI_URA], 139+ i* 15, 17 );
		for( i=0 ; i<byDoraCnt ; i++ ) {
//m			MahJongRally_Msg_Line( NumberText[D_SYSFONT_DORA],35,90,SYS_FONT_WHITE );	// 『ドラ』文字の表示。
			MpsBrewLib_DrawSprite(
//				spPaiJicyaSmall[Dora[i]],
//				spPaiJicya[Dora[i]],
				spSutePaiJicya[Dora[i]],
				139+ i* 15,		//100+i*20,
				17				//90
			);
		}
#endif
		/****************************************************************/
		/*						ウラドラの描画							*/
		/****************************************************************/
		// リーチがされていなければ表示しない。
#if	Rule_2P
//		for( i= 0; i< 5; i++ )
//			hMpsBrewLib_DrawSprite( spSutePaiJicya, PAI_URA, 144+ i* 15, 48 );
		if( Rich != 0 ) {
			for( i=0 ; i<byUraDoraCnt ; i++ )
				hMpsBrewLib_DrawSprite( spSutePaiJicya, Ura[i], 144+ i* 15, 48 );
			DoraY = 0;
		}
#else
		for( i= 0; i< 5; i++ )
//			MpsBrewLib_DrawSprite( spPaiJicya[PAI_URA], 144+ i* 16, 48 );
			MpsBrewLib_DrawSprite( spSutePaiJicya[PAI_URA], 144+ i* 15, 48 );
		if( Rich != 0 )
			for( i=0 ; i<byUraDoraCnt ; i++ ) {
				//MahJongRally_Msg_Line( NumberText[D_SYSFONT_URADORA],35,100,SYS_FONT_WHITE );	// 『ドラ』文字の表示。
				//MpsBrewLib_DrawSprite( spPaiJicyaSmall[Ura[i]],100+i*20,100);
//m				MahJongRally_Msg_Line( NumberText[D_SYSFONT_URADORA],35,110,SYS_FONT_WHITE );	// 『ドラ』文字の表示。
				MpsBrewLib_DrawSprite(
//					spPaiJicyaSmall[Ura[i]],
//					spPaiJicya[Ura[i]],
					spSutePaiJicya[Ura[i]],
					144+ i* 15,		//100+i*20,
					48				//110
				);
				DoraY = 0;
			}
#endif
	}

	/****************************************************************/
	/*		満貫以上ならの描画										*/
	/****************************************************************/
#if	Rule_2P
	i= -1;
	if( result_.byYakuman!= 0)		// 役満
		i= ( gpsTableData.byOya == iOrder ) ? D_END02_05+ (TotalPoint / 48000)- 1 : D_END02_05+ (TotalPoint / 32000)- 1;
	else if( result_.byHan>= 13)	i= D_END02_04;		// 数え役満
	else if( result_.byHan>= 11)	i= D_END02_03;		// ３倍満
	else if( result_.byHan>= 8)		i= D_END02_02;		// 倍満
	else if( result_.byHan>= 6)		i= D_END02_01;		// 跳満
	else if((result_.byHan >=	5)						// 満貫
		|| (result_.byHan == 4 && result_.byFu >=  40)
		|| (result_.byHan == 3 && result_.byFu >=  70)
		|| (result_.byHan == 2 && result_.byFu >= 130))
									i= D_END02_00;

	if( i!= -1) {
		int j= D_END01_31;

		if( i>= D_END02_04)
			j= D_END01_30;
		mMpsBrewLib_DrawSprite( spGame[j],  4, 193 );	//ワク
		mMpsBrewLib_DrawSprite( spGame[i], 13, 198 );	//文字
	}
#else
	if (result_.byYakuman != 0) {	// 役満
		MahJongRally_Msg_Line( NumberText[D_SYSFONT_YAKUMAN],100, (D_AGARI_TEXT_DISP_BASE_Y+SetY)+10+ _y_of030,SYS_FONT_WHITE );
//		yakumanFlag = FALSE;
	} else if (result_.byHan >= 13) {	// 数え役満
		MahJongRally_Msg_Line( NumberText[D_SYSFONT_KAZOEYAKUMAN],120,(D_AGARI_HONBA_TEXT_DISP_BASE_Y+SetY)-(DoraY)+ _y_of031,SYS_FONT_WHITE  );
	} else if (result_.byHan >= 11) {	// ３倍満
		MahJongRally_Msg_Line( NumberText[D_SYSFONT_3BAIMAN],120,(D_AGARI_HONBA_TEXT_DISP_BASE_Y+SetY)-(DoraY)+ _y_of031,SYS_FONT_WHITE  );
	} else if (result_.byHan >=	8) {	// 倍満
		MahJongRally_Msg_Line( NumberText[D_SYSFONT_BAIMAN],120,(D_AGARI_HONBA_TEXT_DISP_BASE_Y+SetY)-(DoraY)+ _y_of031,SYS_FONT_WHITE	);
	} else if (result_.byHan >=	6) {	// 跳満
		MahJongRally_Msg_Line( NumberText[D_SYSFONT_HANEMAN],120,(D_AGARI_HONBA_TEXT_DISP_BASE_Y+SetY)-(DoraY)+ _y_of031,SYS_FONT_WHITE  );
	} else if ((result_.byHan >=	5)	// 満貫
			|| (result_.byHan == 4 && result_.byFu >=  40)
			|| (result_.byHan == 3 && result_.byFu >=  70)
			|| (result_.byHan == 2 && result_.byFu >= 130)) {
		MahJongRally_Msg_Line( NumberText[D_SYSFONT_MANGAN],120,(D_AGARI_HONBA_TEXT_DISP_BASE_Y+SetY)-(DoraY)+ _y_of031,SYS_FONT_WHITE	);
	}
#endif

	/****************************************************************/
	/*						和了した役の描画						*/
	/****************************************************************/
#if	Rule_2P
	yaku_number_sort();			//役番号のソート
#endif
	/*	役名を作成	*/
	for (i = 0; i < yakuCount ; i++) {
		yakuNumber = result_.sYaku[i].name;								// 役の名前取得。

		offSetX = i % 2;
		offSetY = i / 2;

		posX = offSetX != 0 ? D_YAKU_DISP_BASE_X+(offSetX*D_YAKU_DISP_OFFSET_X) : D_YAKU_DISP_BASE_X;
		posY = D_YAKU_DISP_BASE_Y+offSetY*D_YAKU_DISP_OFFSET_Y;

		// 和了した役の描画。
		if( result_.byYakuman == 0 && isSpecialYaku == false )		//0422
			MpsBrewLib_DrawSprite( spYakuBG[yakuNumber],posX,posY-(DoraY) );
		else
			MpsBrewLib_DrawSprite( spYakuBG[yakuNumber],posX,posY );

		// 役数・飜
		if( result_.byYakuman == 0 && isSpecialYaku == false ) {		//0422
			yakuFactor = result_.sYaku[i].factor;
//			MpsBrewLib_DrawSprite( spGame[D_BIG_NUMBER_0+yakuFactor],posX+59,(posY+2)-(DoraY));
			if( yakuFactor>= 10)
				mMpsBrewLib_DrawSprite( spGame[D_END01_18+ (yakuFactor / 10)], posX+ 59+ 20- 7, (posY)-(DoraY));
			mMpsBrewLib_DrawSprite( spGame[D_END01_18+ (yakuFactor % 10)], posX+ 59+ 20, (posY)-(DoraY));
			mMpsBrewLib_DrawSprite( spGame[D_END01_34], posX+ 59+ 27, (posY)-(DoraY));	//得点表示の"飜"（単位）
		}
	}
//	font= Font.getFont(Font.FACE_SYSTEM, Font.STYLE_PLAIN, Font.SIZE_SMALL);
//	vscreen.setFont(font);

		/****************************************************************/
		/*		符の描画												*/
		/****************************************************************/
	// 役満の場合は符と翻の描画を行わない。
	if(result_.byYakuman == 0 && isSpecialYaku == false ) {	//> 2006/03/23 流し満貫、十三不塔対策	//0422
		{		//0422
			{
				// 通常。
//m				AgariNumDisp( FU, 10, (D_AGARI_TEXT_DISP_BASE_Y+SetY)-(DoraY)+ _y_of030 , 3, 3, FALSE );
				i= AgariNumDisp( FU, 97- 7, 198, 3, 3, FALSE )+ 1;
				mMpsBrewLib_DrawSprite( spGame[ D_END01_33], 97- 7+ (i* 7), 198 );
//m				MahJongRally_Msg_Line( NumberText[D_SYSFONT_FU],60, (D_AGARI_TEXT_DISP_BASE_Y+SetY)-(DoraY)+ _y_of030, SYS_FONT_WHITE );	//
			}

			/****************************************************************/
			/*		翻の描画												*/
			/****************************************************************/
//m			AgariNumDisp( HAN, 70, (D_AGARI_TEXT_DISP_BASE_Y+SetY)-(DoraY)+ _y_of030 , 2, 2, FALSE );
			i= AgariNumDisp( HAN, 132, 198, 2, 2, FALSE )+ 2;
			mMpsBrewLib_DrawSprite( spGame[ D_END01_34], 132- 7+ (i* 7), 198 );
//m			MahJongRally_Msg_Line( NumberText[D_SYSFONT_HAN], 110, (D_AGARI_TEXT_DISP_BASE_Y+SetY)-(DoraY)+ _y_of030, SYS_FONT_WHITE );
		}
	}

#if	Rule_2P
#else
	/****************************************************************/
	/*		親か子か描画											*/
	/****************************************************************/
	// 余りに見通しが悪いのでついでに修正		//0422
	//> 2006/03/23 流し満貫、十三不塔対策
	{
		String text_ = NumberText[D_SYSFONT_KO];		//AECHAR *text_
		int posx_ = 140;
		int posy_ = (D_AGARI_TEXT_DISP_BASE_Y+SetY)-(DoraY);

		if( gpsTableData.byOya == iOrder )
			text_ = NumberText[D_SYSFONT_OYA];

		// 役満の場合は位置を変える
		if( result_.byYakuman != 0 ) {
			posx_ = 40;
			posy_ = (D_AGARI_TEXT_DISP_BASE_Y+SetY)+10;
		}

//m		MahJongRally_Msg_Line( text_, posx_, posy_+ _y_of030, SYS_FONT_WHITE );
	}
	//> 2006/03/23 流し満貫、十三不塔対策
#endif

	/****************************************************************/
	/*		本場の描画												*/
	/****************************************************************/
	// 何本場？
//m	if( result_.byYakuman == 0 ) {	//0422
		// 2桁対応 2006/02/26 No.724
//m		AgariNumDisp( RENCYAN, 10, (D_AGARI_HONBA_TEXT_DISP_BASE_Y+SetY)-(DoraY)+ _y_of031, 3, 3, FALSE );
		AgariNumDisp( RENCYAN, 70- 7* 2, 48- 1, 3, 3, false, D_END01_07- D_END01_18);
//m		MahJongRally_Msg_Line( NumberText[D_SYSFONT_HONBA],60,(D_AGARI_HONBA_TEXT_DISP_BASE_Y+SetY)-(DoraY)+ _y_of031,	SYS_FONT_WHITE	);	// 本場
//m	}

	//***************************************************************
	//		リー棒数の描画											*
	//***************************************************************
#if	Rule_2P
	AgariNumDisp( gpsTableData.sMemData[0].byRibopublicck+ gpsTableData.sMemData[1].byRibopublicck,
		35- 7* 2, 48- 1, 3, 3, false, D_END01_07- D_END01_18);
#else
	AgariNumDisp( gpsTableData.byRibo, 35- 7* 2, 48- 1, 3, 3, FALSE, D_END01_07- D_END01_18);
#endif

	/****************************************************************/
	/*		合計の描画												*/
	/****************************************************************/
	if( result_.byYakuman != 0 ) {	//&& isSpecialYaku == FALSE )	//0422
		// --- 通常。
//m		MahJongRally_Msg_Line( NumberText[D_SYSFONT_GOUKEI],30,(D_AGARI_GOUKEI_TEXT_DISP_BASE_Y+SetY)+ _y_of032,SYS_FONT_WHITE	);			// 合計
//m		AgariNumDisp( TotalPoint, 40, (D_AGARI_GOUKEI_TEXT_DISP_BASE_Y+SetY)+ _y_of032, 7, 8, FALSE );
		i= AgariNumDisp( TotalPoint, 167- 7* 3, 198, 7, 8, false )+ 5;
//m		MahJongRally_Msg_Line( NumberText[D_SYSFONT_TEN],150, (D_AGARI_GOUKEI_TEXT_DISP_BASE_Y+SetY)+ _y_of032,SYS_FONT_WHITE );			// 点
	} else {
		// --- 役満・特殊役なら。
//m		MahJongRally_Msg_Line( NumberText[D_SYSFONT_GOUKEI],30,(D_AGARI_GOUKEI_TEXT_DISP_BASE_Y+SetY)-(DoraY)+ _y_of032,SYS_FONT_WHITE	);	// 合計
//m		AgariNumDisp( TotalPoint, 40, (D_AGARI_GOUKEI_TEXT_DISP_BASE_Y+SetY)-(DoraY)+ _y_of032, 7, 8, FALSE );
		i= AgariNumDisp( TotalPoint, 167- 7* 3, 198, 7, 8, false )+ 5;
//m		MahJongRally_Msg_Line( NumberText[D_SYSFONT_TEN],150, (D_AGARI_GOUKEI_TEXT_DISP_BASE_Y+SetY)-(DoraY)+ _y_of032,SYS_FONT_WHITE );	// 点
	}
	mMpsBrewLib_DrawSprite( spGame[ D_END01_35], 167- 7* 6+ (i* 7), 198 );	//"点"

//	font= Font.getFont(Font.FACE_SYSTEM, Font.STYLE_PLAIN, Font.SIZE_MEDIUM);
//	vscreen.setFont(font);		//return;

	/****************************************************************/
	/*		？局？家												*/
	/****************************************************************/
	{
	int		Oya			= gpsTableData.byOya;		// 現在の親。
#if	Rule_2P
	int		House		= ( gpsTableData.byOya == iOrder ) ? D_END01_00 : D_END01_36;
#else
	int		House		= (Oya+ 4- game_player) % 4;
#endif
	int		Kyoku		= gpsTableData.byKyoku;		// 局数。

	mMpsBrewLib_DrawSprite( spGame[D_END01_00+ ((Kyoku/ 4) & 0x01)],	124- 105,	3+ 26 );	//2
	mMpsBrewLib_DrawSprite( spGame[D_END01_02+ (Kyoku & 0x01)],			137- 105,	3+ 26 );
	mMpsBrewLib_DrawSprite( spGame[House],								84- 25,		244- 215 );
	}

#if	Rule_2P
	/* ドラ枠の影 */
	mMpsBrewLib_DrawSprite( spGame[D_END01_28], 101, 13 );		//ドラの表示枠(牌の影消し用も兼ねているので牌より上に描画してください)
	mMpsBrewLib_DrawSprite( spGame[D_END01_29], 106, 44 );		//裏ドラの表示枠(牌の影消し用も兼ねているので牌より上に描画してください)
#endif
#endif //-*todo:描画保留

}

/****************************************
	和了上画面数値表示（最大８桁）
****************************************/
// 第１引数：
// 第２引数：点数など値。
// 第３引数：ｘ座標。
// 第４引数：ｙ座標。
// 第５引数：ループ回数。
// 第６引数：最大描画桁数。
// 第７引数：ゼロフラグ
public int AgariNumDisp( int val, int x, int y, int len, int place, bool zero_flg ) {
	return (AgariNumDisp( (int)val, (int)x, (int)y, (byte)len, (byte)place, (bool)zero_flg, 0));
}
public int AgariNumDisp( int val, int x, int y, int len, int place, bool zero_flg, int num ) {
	return (mAgariNumDisp( (int)val, (int)x, (int)y, (byte)len, (byte)place, (bool)zero_flg, num));
}
public int mAgariNumDisp( /*MahJongRally * pMe,*/ int val, int x, int y, byte len, byte place, bool zero_flg, int num )
{
#if true//-*todo:描画保留
DebLog("//-*MJ:GameDraw.cs:mAgariNumDisp( int "+val+", int "+x+", int "+y+", byte "+len+", byte "+place+", bool "+zero_flg+", int "+num+" )");
return 0;
#else //-*todo:描画保留

	byte	i  = 0;
	byte	j  = 0;
	byte[]	num_buf= new byte [8];

	num_buf[7] = 0;
	num_buf[6] = 0;
	num_buf[5] = 0;
	num_buf[4] = 0;
	num_buf[3] = 0;
	num_buf[2] = 0;
	num_buf[1] = 0;
	num_buf[0] = 0;

	if(val != 0) {
		val = val%100000000;
		num_buf[7] = (byte)((val/10000000)&0xff);
		val = val%10000000;
		num_buf[6] = (byte)((val/1000000)&0xff);
		val = val%1000000;
		num_buf[5] = (byte)((val/100000)&0xff);
		val = val%100000;
		num_buf[4] = (byte)((val/10000)&0xff);
		val = val%10000;
		num_buf[3] = (byte)((val/1000)&0xff);
		val = val%1000;
		num_buf[2] = (byte)((val/100)&0xff);
		val = val%100;
		num_buf[1] = (byte)((val/10)&0xff);
		val = val%10;
		num_buf[0] = (byte)(val&0xff);
	}

	if( !zero_flg ) {
		for( i = 7; i >0; i--) {
			if( num_buf[i] == 0 )
				num_buf[i] = 0x0a;		// アスキースペース。
			else
				break;
		}
		if(len == 0)
			len = (byte)(i + 1);
	}

	// 表示開始。
	for( j=0 ; j< len; j++ )
		if( num_buf[j] != 0x0a )
			// 数字の描画。
#if	Rule_2P
			mMpsBrewLib_DrawSprite( spGame[ num_buf[j]+ D_END01_18+ num],
				x+ (( place- j)* 7), y );
#else
			MahJongRally_Msg_Line( NumberText[num_buf[j]],
				x+((place-j)*12), y, SYS_FONT_WHITE );
#endif
	return len;
#endif //-*todo:描画保留

}

public const int D_SUTEPAI_OFFSET_H	 = 17;	//18
/*===========================================================================
								自家の描画全般
===========================================================================*/
public void MJ_JicyaDraw( /*MahJongRally * pMe,*/ int House )
{
	DebLog("//-*MJ:GameDraw.cs:MJ_JicyaDraw( int "+House+" )");
	int i		= 0;	// ループ
	int j		= 0;	// チーループ用。
	int x		= 0;	// 牌UP用
	int cnt		= 0;	// 現在の手牌数
	int sutecnt = 0;	// 捨て牌数。
	int pai		= 0;	// 描画する牌の番号
	int tsumopai= 0;	// ツモ牌を描画用番号取得。
	int paiupY	= 0;	// 牌UPするY座標
	int csr		= 0;	// 牌カーソル位置
	int Fr		= 0;	// フーロ牌チェック用
	int FrPos	= 0;	// 左0・中心1・右2．
	int Bbit	= 0;	// フーロ牌情報取得
	int Hbit	= 0;	// 上位ビット
	int SBit	= 0;	// 下位ビット
	int offsetX = 0;	// 捨て牌のオフセット値
	int offsetY = 0;	// 捨て牌のオフセット値
	int suteFlag= 0;	// リーチ or リーチ？
	int furoOffX			= 0;		// フーロ牌用のOFFset
	int furoOffBaseX		= 0;		// フーロ牌用のOFFset
	int	DrawCount	= 0;
	int	reached_add	= 0;
	int	reached_line= 0;



	/*===========================================================================
								手牌の描画
	===========================================================================*/
	cnt		= gsPlayerWork[House].byThcnt;							// 手牌の数を取得。
	DebLog("//-***************手牌************");
	for(int a=0;a<cnt;a++){
		var tehaiDrawTest = m_myHandTiles[a].GetComponent<MJTIle>();
		DebLog("//-*手牌["+a+"]:"+gsPlayerWork[House].byTehai[a]+"("+(PAI)gsPlayerWork[House].byTehai[a]+")");
		tehaiDrawTest.set(TILE_STATE.MY_HAND,(PAI)gsPlayerWork[House].byTehai[a]);
	}
#if false//-*todo:描画保留
	int i		= 0;	// ループ
	int j		= 0;	// チーループ用。
	int x		= 0;	// 牌UP用
	int cnt		= 0;	// 現在の手牌数
	int sutecnt = 0;	// 捨て牌数。
	int pai		= 0;	// 描画する牌の番号
	int tsumopai= 0;	// ツモ牌を描画用番号取得。
	int paiupY	= 0;	// 牌UPするY座標
	int csr		= 0;	// 牌カーソル位置
	int Fr		= 0;	// フーロ牌チェック用
	int FrPos	= 0;	// 左0・中心1・右2．
	int Bbit	= 0;	// フーロ牌情報取得
	int Hbit	= 0;	// 上位ビット
	int SBit	= 0;	// 下位ビット
	int offsetX = 0;	// 捨て牌のオフセット値
	int offsetY = 0;	// 捨て牌のオフセット値
	int suteFlag= 0;	// リーチ or リーチ？
	int furoOffX			= 0;		// フーロ牌用のOFFset
	int furoOffBaseX		= 0;		// フーロ牌用のOFFset
	int	DrawCount	= 0;
	int	reached_add	= 0;
	int	reached_line= 0;

	// 現在の牌カーソル値の取得
	csr = hai_csr;
	if( menu_csr>= D_OPT_STK_MAX)	menu_csr= 0;		//0507 No.1030

	/*===========================================================================
		捨て牌数の描画
	===========================================================================*/
	sutecnt = gsPlayerWork[House].byShcnt;							// 捨て牌数取得。

	for( i=0 ; i<sutecnt ; i++ ) {
		suteFlag	= Rec_sute_pos[House][i];						// 捨て牌がリーチかどうかをGET。
		pai			= gsPlayerWork[House].bySthai[i];				// 河の捨て牌取得。
		offsetX		= DrawCount%12;		//	%8						// 横の位置
		offsetY		= DrawCount/12;		//	/8						// 段の位置
		switch( suteFlag ) {
			case SUTEHAI_ON:
				// 捨て牌通常表示
#if	Rule_2P
				if(offsetY == reached_line)
					// リーチ牌を表示したラインならば
					hMpsBrewLib_DrawSprite( spSutePaiJicya, pai,
						(D_JICYA_RIVER_X+(offsetX*13)+reached_add),
						D_JICYA_RIVER_Y+(offsetY*D_SUTEPAI_OFFSET_H) );
				else
					// ちがうのであれば
					hMpsBrewLib_DrawSprite( spSutePaiJicya, pai,
						D_JICYA_RIVER_X+(offsetX*13),
						D_JICYA_RIVER_Y+(offsetY*D_SUTEPAI_OFFSET_H) );
#else
				if(offsetY == reached_line)
					// リーチ牌を表示したラインならば
					MpsBrewLib_DrawSprite( spSutePaiJicya[pai],
						(D_JICYA_RIVER_X+(offsetX*13)+reached_add),
						D_JICYA_RIVER_Y+(offsetY*D_SUTEPAI_OFFSET_H) );
				else
					// ちがうのであれば
					MpsBrewLib_DrawSprite( spSutePaiJicya[pai],
						D_JICYA_RIVER_X+(offsetX*13),
						D_JICYA_RIVER_Y+(offsetY*D_SUTEPAI_OFFSET_H) );
#endif
				DrawCount+=1;
				break;
//m			case SUTEHAI_OFF:
//m				break;
			case SUTEHAI_REACH:
				reached_add=4;
				reached_line = offsetY;

				// 捨て牌リーチ表示。
#if	Rule_2P
				hMpsBrewLib_DrawSprite_line4( spSutePaiShimocya, pai,
#else
				MpsBrewLib_DrawSprite( spSutePaiShimocya[pai],
#endif
					D_JICYA_RIVER_X+(offsetX*13),
					(D_JICYA_RIVER_Y+(offsetY*D_SUTEPAI_OFFSET_H))+4+1 );
				DrawCount+=1;
				break;
//m			default:
//m				break;
		}
	}

	/*===========================================================================
								手牌の描画
	===========================================================================*/
	cnt		= gsPlayerWork[House].byThcnt;							// 手牌の数を取得。

#if	Rule_2P
	for( j= 0; j< 2; j++ )
#endif
		for( i= 0; i< cnt; i++ ) {
			short icontype_ = D_TRIANGLE_DOWN;

			//> 2006/02/05 No19. メニューの値ではなくオプションスタックを見るように変更
			x = 0x00;
			if( hai_up == 0 ) {
				// 持ち上げ牌フラグが０の場合（アクションが無し）
				if( i == hai_csr ) {
					// パス、打牌時のみカーソルを採用
					switch( Optstk[ menu_csr ] ) {
						case OP_PASS:
						case OP_TAPAI:
							x = 0x01;
							break;
					}
				}
			} else
				// 立直、哭きはこちらにくる、こない場合は牌選択ルーチンがおかしい可能性あり
				//> 要望No.112 捨牌でツモ牌以外を選択後にツモを選択すると、カーソルが２個表示される
				if( Optstk[menu_csr] != OP_TSUMO ) {
					//< 要望No.112
					x = hai_up >> i;
					x = x & 0x1;
				}

			// 手牌の中身を取得(16進)
			pai = gsPlayerWork[House].byTehai[i];
			paiupY = 0;

			if( x!= 0 ) {
				// カーソル・牌UP/自ツモのみ。
				paiupY = 6;		// = 5;
#if	Rule_2P
				if( j== 0 )
#endif
					MJ_IconDraw( icontype_, (int16_t)((i*15)+11-1), 220+12, 0 );	//MJ_IconDraw( icontype_, (int16_t)((i*15)+11), 220, 0 );
			}

#if	Rule_2P
			//選択牌を先に表示する
			if((( j== 0) && ( paiupY!= 0)) || (( j== 1) && ( paiupY== 0)))
				hMpsBrewLib_DrawSprite( spPaiJicya, pai,
					D_JICYA_TEHAI_X+(i*(D_PAI_TATE_LARGE_SIZE_W-1)),
					D_JICYA_TEHAI0_Y-paiupY+ _y_of010);
#else
			//< 2006/02/05 No19.
			MpsBrewLib_DrawSprite( spPaiJicya[pai],
				D_JICYA_TEHAI_X+(i*(D_PAI_TATE_LARGE_SIZE_W-1)),
				D_JICYA_TEHAI0_Y-paiupY+ _y_of010);
#endif
		}

	/*===========================================================================
								ツモ牌の描画
	===========================================================================*/
	tsumopai = gsPlayerWork[House].byHkhai;		// ツモってきた牌を取得。

	//> 2006/03/08 バグNo.165
	if( hide_hkhai == 0 )
		if((tsumopai>=0x01) && (tsumopai<=0x37)) {
			int16_t icontype_ = D_TRIANGLE_DOWN;

			if(
				( ( csr== cnt ) && ( hai_up== 0 ) )
				|| (( hai_up & 0x2000 ) != 0)
				//> 2006/03/05 要望 No.107 ツモ和了り時にツモ牌にカーソルを当てる。
				|| Optstk[ menu_csr ] == OP_TSUMO
				//< 2006/03/05 要望 No.107
				//> 2006/03/05 バグ No.110 小明槓ツモ牌にカーソルを当てる
				//> 2006/03/08 バグNo.164
				//|| ( ( Optstk[ menu_csr ] == OP_KAN ) && (( 0x0001 << cnt ) == hai_up ) )
				|| ( ( Optstk[ menu_csr ] == OP_KAN ) && (( 0x0001 << cnt ) & hai_up ) != 0 )
				//< 2006/03/05 バグNo.110
				//> 2006/03/08 バグNo.164
				|| ( ( Optstk[ menu_csr ] == OP_RICHI ) && (( 0x0001 << cnt ) == hai_up ) )
				//> 2006/03/08 バグNo.164
			) {
				paiupY = 6;		// = 5;
				if( gsPlayerWork[House].byTkhai != 0 )
					MJ_IconDraw( icontype_,(int16_t)((cnt*15)+17),220+12- 1,0);	//MJ_IconDraw( icontype_,(int16_t)((cnt*15)+17),220,0);
			} else
				paiupY = 0;

#if	Rule_2P
			//鳴きメニュー選択中は、牌を上に上げない
			if( _menuFlg)
				paiupY= 0;
#endif

			if( gsPlayerWork[House].byTkhai != 0 )
				// ツモ牌
#if	Rule_2P
				hMpsBrewLib_DrawSprite( spPaiJicya, tsumopai,
					(D_JICYA_TEHAI_X+cnt*(D_PAI_TATE_LARGE_SIZE_W-1)+5+2),
					D_JICYA_TEHAI0_Y-paiupY+ _y_of010);
#else
				MpsBrewLib_DrawSprite( spPaiJicya[tsumopai],
					(D_JICYA_TEHAI_X+cnt*(D_PAI_TATE_LARGE_SIZE_W-1)+5+2),	//(D_JICYA_TEHAI_X+cnt*D_PAI_TATE_LARGE_SIZE_W+5),
					D_JICYA_TEHAI0_Y-paiupY+ _y_of010);
#endif
		}
		//< 2006/03/08 バグNo.165

#if _DEBUG
	if( dbgopt[1] == 2 ) {
		// ゲーム時デバッグ表示メイン
//		AEERect	dbgrect_;
		int dbg_x_=0;
		int dbg_y_=0;
		int dbg_offset_ = 20;
		char dbgstr_[64]={0};

//		IDISPLAY_SetColor(GETMYDISPLAY(),CLR_USER_TEXT,MAKE_RGB(255,255,0));
//		SETAEERECT(dbgrect_, 0, 0,m_screenWidth, dbg_offset_ * 5 );
//		IDISPLAY_DrawRect(GETMYDISPLAY(), dbgrect_, MAKE_RGB(0,0,0), MAKE_RGB(0,0,0), IDF_RECT_FILL );

		SPRINTF( dbgstr_,"手牌=%d,捨牌=%d 鳴位置=%d", cnt,sutecnt,NakiSelNo );
		MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_+= dbg_offset_ );

		SPRINTF( dbgstr_,"牌UP %d%d%d%d%d%d%d%d%d%d%d%d%d%d",
			0x01 & (hai_up >> 0), 0x01 & (hai_up >> 1),0x01 & (hai_up >> 2),0x01 & (hai_up >> 3),
			0x01 & (hai_up >> 4), 0x01 & (hai_up >> 5),0x01 & (hai_up >> 6),0x01 & (hai_up >> 7),
			0x01 & (hai_up >> 8), 0x01 & (hai_up >> 9),0x01 & (hai_up >> 10),0x01 & (hai_up >> 11),
			0x01 & (hai_up >> 12), 0x01 & (hai_up >> 13));
		MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_+= dbg_offset_ );

		SPRINTF( dbgstr_,"牌UP 0x%04x CSR %d:%d:%d", hai_up, hai_csr, menu_csr,Optstk[ menu_csr ]);
		MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_+= dbg_offset_ );

		SPRINTF( dbgstr_,"KAN[%d] %d:%d:%d %d:%d:%d",
			Kannum,Kanflg[0],Kanflg[1],Kanflg[2],Kanpos[0],Kanpos[1],Kanpos[2]);
		MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_+= dbg_offset_ );

//		IDISPLAY_SetColor(GETMYDISPLAY(),CLR_USER_TEXT,MAKE_RGB(255,255,255));
	}
#endif

	/*===========================================================================
								フーロ牌の描画
		注：ここの処理より下にコードを書く場合は
			if( Fr == 0 ) return;の解除を行うこと
	===========================================================================*/
#if	Rule_2P
	MJ_FrJicyaDraw( House, 247 );	//D_JICYA_TEHAI_Y
#else
	// フーロ牌があるかチェック
	Fr = gsPlayerWork[House].byFhcnt;

	if( Fr == 0 )	return;							// フーロ牌が無ければ。
	if( Fr > 4 )	Fr = 4;

	// フーロ牌の情報取得
	for( i= 0; i< Fr; i++ ) {
		Bbit = gsPlayerWork[House].byFrhai[i];		// フーロ牌情報取得。

		//Frに4以上が入る不具合発生のため暫定的にここで処理を入れる 20051202
		if(Bbit == 0)	return;

		FrPos= gsPlayerWork[House].byFrpos[i];		// 鳴き場所取得。

		Hbit = Bbit & 0xC0;							// 上位ビットの取得。
		switch( Hbit ) {
			// 明カン(自家)
			case D_MINKAN:
				SBit = Bbit & 0x3F;					// 下位ビットの取得
				furoOffX = 0;
				furoOffBaseX = 0;
				{
					if( FrPos==1) {
						// 済。
						// 横になる牌。
//m						MpsBrewLib_DrawSprite( spPaiShimocya[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+0,D_JICYA_TEHAI_Y-8+_y_of011);
//m						MpsBrewLib_DrawSprite( spPaiShimocya[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+0,D_JICYA_TEHAI_Y+3+_y_of011);
						MpsBrewLib_DrawSprite( spSutePaiShimocya[SBit],
							-(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+0,
							D_JICYA_TEHAI_Y-8+_y_of011+ _y_of012+ _y_of013);
						MpsBrewLib_DrawSprite( spSutePaiShimocya[SBit],
							-(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+0,
							D_JICYA_TEHAI_Y+3+_y_of011+ _y_of012);
						// 自家通常牌。
//m						MpsBrewLib_DrawSprite( spPaiJicyaSmall[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+13,D_JICYA_TEHAI_Y+_y_of011);
//m						MpsBrewLib_DrawSprite( spPaiJicyaSmall[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+24,D_JICYA_TEHAI_Y+_y_of011);
						MpsBrewLib_DrawSprite( spSutePaiJicya[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+13,D_JICYA_TEHAI_Y+_y_of011);
						MpsBrewLib_DrawSprite( spSutePaiJicya[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+24,D_JICYA_TEHAI_Y+_y_of011);
					}
					if( FrPos==2 ) {
						// 済。
						// 自家通常牌。
//m						MpsBrewLib_DrawSprite( spPaiJicyaSmall[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+0,D_JICYA_TEHAI_Y+_y_of011);
						MpsBrewLib_DrawSprite( spSutePaiJicya[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+0,D_JICYA_TEHAI_Y+_y_of011);
						// 横になる牌。
//m						MpsBrewLib_DrawSprite( spPaiShimocya[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+11,D_JICYA_TEHAI_Y-8+_y_of011);
//m						MpsBrewLib_DrawSprite( spPaiShimocya[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+11,D_JICYA_TEHAI_Y+3+_y_of011);
						MpsBrewLib_DrawSprite( spSutePaiShimocya[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+11,D_JICYA_TEHAI_Y-8+_y_of011+ _y_of012+ _y_of013);
						MpsBrewLib_DrawSprite( spSutePaiShimocya[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+11,D_JICYA_TEHAI_Y+3+_y_of011+ _y_of012);
						// 自家通常牌。
//m						MpsBrewLib_DrawSprite( spPaiJicyaSmall[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+25,D_JICYA_TEHAI_Y+_y_of011);
						MpsBrewLib_DrawSprite( spSutePaiJicya[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+25,D_JICYA_TEHAI_Y+_y_of011);
					}
					if( FrPos==3) {
						// 済。
						// 自家通常牌。
//m						MpsBrewLib_DrawSprite( spPaiJicyaSmall[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+0,D_JICYA_TEHAI_Y+_y_of011);
//m						MpsBrewLib_DrawSprite( spPaiJicyaSmall[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+11,D_JICYA_TEHAI_Y+_y_of011);
						MpsBrewLib_DrawSprite( spSutePaiJicya[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+0,D_JICYA_TEHAI_Y+_y_of011);
						MpsBrewLib_DrawSprite( spSutePaiJicya[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+11,D_JICYA_TEHAI_Y+_y_of011);
						// 横になる牌。
//m						MpsBrewLib_DrawSprite( spPaiShimocya[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+22,D_JICYA_TEHAI_Y-8+_y_of011);
//m						MpsBrewLib_DrawSprite( spPaiShimocya[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+22,D_JICYA_TEHAI_Y+3+_y_of011);
						MpsBrewLib_DrawSprite( spSutePaiShimocya[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+22,D_JICYA_TEHAI_Y-8+_y_of011+ _y_of012+ _y_of013);
						MpsBrewLib_DrawSprite( spSutePaiShimocya[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+22,D_JICYA_TEHAI_Y+3+_y_of011+ _y_of012);
					}
				}
				break;

			// 暗カン(両端を裏にして表示)
			case D_ANKAN:
				SBit = Bbit & 0x3F;					// 下位ビットの取得
				{	// 済。
//m					MpsBrewLib_DrawSprite( spPaiJicyaSmall[PAI_URA], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+(0*D_PAI_TATE_SMALL_SIZE_W)+0),D_JICYA_TEHAI_Y+_y_of011);
//m					MpsBrewLib_DrawSprite( spPaiJicyaSmall[PAI_URA], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+(2*D_PAI_TATE_SMALL_SIZE_W)+0)+3,D_JICYA_TEHAI_Y+_y_of011);
					MpsBrewLib_DrawSprite( spSutePaiJicya[PAI_URA], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+(0*D_PAI_TATE_SMALL_SIZE_W)+0),D_JICYA_TEHAI_Y+_y_of011);
					MpsBrewLib_DrawSprite( spSutePaiJicya[PAI_URA], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+(2*D_PAI_TATE_SMALL_SIZE_W)+0)+3,D_JICYA_TEHAI_Y+_y_of011);
//m					MpsBrewLib_DrawSprite( spPaiShimocya[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+(1*D_PAI_TATE_SMALL_SIZE_W)-0),D_JICYA_TEHAI_Y-8+_y_of011);
//m					MpsBrewLib_DrawSprite( spPaiShimocya[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+(1*D_PAI_TATE_SMALL_SIZE_W)-0),D_JICYA_TEHAI_Y+3+_y_of011);
					MpsBrewLib_DrawSprite( spSutePaiShimocya[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+(1*D_PAI_TATE_SMALL_SIZE_W)-0),D_JICYA_TEHAI_Y-8+_y_of011+ _y_of012+ _y_of013);
					MpsBrewLib_DrawSprite( spSutePaiShimocya[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+(1*D_PAI_TATE_SMALL_SIZE_W)-0),D_JICYA_TEHAI_Y+3+_y_of011+ _y_of012);
				}
				break;

			// ポン
			case D_PON:
				SBit = Bbit & 0x3F;					// 下位ビットの取得
				furoOffX = 0;

				for( j=1 ; j<=3 ; j++ ) {
					if( j == FrPos ) {
						// 左・中心・右のどこに置くか
//m						MpsBrewLib_DrawSprite( spPaiShimocya[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X+furoOffX, D_JICYA_TEHAI_Y+3+_y_of011);
						MpsBrewLib_DrawSprite( spSutePaiShimocya[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X+furoOffX, D_JICYA_TEHAI_Y+3+_y_of011+ _y_of012);
						furoOffX = furoOffX+ D_PAI_YOKO_SMALL_SIZE_W;
						continue;
					}
//m					MpsBrewLib_DrawSprite( spPaiJicyaSmall[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X+furoOffX, D_JICYA_TEHAI_Y+_y_of011);
					MpsBrewLib_DrawSprite( spSutePaiJicya[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X+furoOffX, D_JICYA_TEHAI_Y+_y_of011);
					furoOffX = furoOffX+ D_PAI_TATE_SMALL_SIZE_W;
				}
				break;

			// チー
			default:
				// チーの鳴き牌処理
				// MahJongRally_DispChiPai( i, Bbit, FrPos, House );
				switch( FrPos ) {
					case 0: {
						//MpsBrewLib_DrawSprite( spPaiShimocya[Bbit+0],	-(i*D_PAI_TOTAL_SIZE)+D_JI_FH_BASE_X+(0*D_PAI_TATE_SMALL_SIZE_W),D_JICYA_TEHAI_Y);
						//MpsBrewLib_DrawSprite( spPaiJicyaSmall[Bbit+1],-(i*D_PAI_TOTAL_SIZE)+D_JI_FH_BASE_X+(1*D_PAI_TATE_SMALL_SIZE_W),D_JICYA_TEHAI_Y);
						//MpsBrewLib_DrawSprite( spPaiJicyaSmall[Bbit+2],-(i*D_PAI_TOTAL_SIZE)+D_JI_FH_BASE_X+(2*D_PAI_TATE_SMALL_SIZE_W),D_JICYA_TEHAI_Y);
//m						MpsBrewLib_DrawSprite( spPaiShimocya[Bbit+0],	-(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X, D_JICYA_TEHAI_Y+3+_y_of011);
						MpsBrewLib_DrawSprite( spSutePaiShimocya[Bbit+0],	-(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X, D_JICYA_TEHAI_Y+3+_y_of011+ _y_of012);
//m						MpsBrewLib_DrawSprite( spPaiJicyaSmall[Bbit+1],-(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X+D_PAI_YOKO_SMALL_SIZE_W, D_JICYA_TEHAI_Y+_y_of011);
//m						MpsBrewLib_DrawSprite( spPaiJicyaSmall[Bbit+2],-(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X+D_PAI_YOKO_SMALL_SIZE_W+D_PAI_TATE_SMALL_SIZE_W, D_JICYA_TEHAI_Y+_y_of011);
						MpsBrewLib_DrawSprite( spSutePaiJicya[Bbit+1],-(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X+D_PAI_YOKO_SMALL_SIZE_W, D_JICYA_TEHAI_Y+_y_of011);
						MpsBrewLib_DrawSprite( spSutePaiJicya[Bbit+2],-(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X+D_PAI_YOKO_SMALL_SIZE_W+D_PAI_TATE_SMALL_SIZE_W, D_JICYA_TEHAI_Y+_y_of011);
						break;
					}
					case 1: {
//m						MpsBrewLib_DrawSprite( spPaiShimocya[Bbit+1],	-(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X, D_JICYA_TEHAI_Y+3+_y_of011);
						MpsBrewLib_DrawSprite( spSutePaiShimocya[Bbit+1],	-(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X, D_JICYA_TEHAI_Y+3+_y_of011+ _y_of012);
//m						MpsBrewLib_DrawSprite( spPaiJicyaSmall[Bbit+0],-(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X+D_PAI_YOKO_SMALL_SIZE_W, D_JICYA_TEHAI_Y+_y_of011);
//m						MpsBrewLib_DrawSprite( spPaiJicyaSmall[Bbit+2],-(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X+D_PAI_YOKO_SMALL_SIZE_W+D_PAI_TATE_SMALL_SIZE_W, D_JICYA_TEHAI_Y+_y_of011);
						MpsBrewLib_DrawSprite( spSutePaiJicya[Bbit+0],-(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X+D_PAI_YOKO_SMALL_SIZE_W, D_JICYA_TEHAI_Y+_y_of011);
						MpsBrewLib_DrawSprite( spSutePaiJicya[Bbit+2],-(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X+D_PAI_YOKO_SMALL_SIZE_W+D_PAI_TATE_SMALL_SIZE_W, D_JICYA_TEHAI_Y+_y_of011);
						break;
					}
					case 2: {
//m						MpsBrewLib_DrawSprite( spPaiShimocya[Bbit+2],	-(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X, D_JICYA_TEHAI_Y+3+_y_of011);
						MpsBrewLib_DrawSprite( spSutePaiShimocya[Bbit+2],	-(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X, D_JICYA_TEHAI_Y+3+_y_of011+ _y_of012);
						if( Bbit == 32 || Bbit == 31 )
							break;
						else
//m							MpsBrewLib_DrawSprite( spPaiJicyaSmall[Bbit+0],-(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X+D_PAI_YOKO_SMALL_SIZE_W, D_JICYA_TEHAI_Y+_y_of011);
							MpsBrewLib_DrawSprite( spSutePaiJicya[Bbit+0],-(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X+D_PAI_YOKO_SMALL_SIZE_W, D_JICYA_TEHAI_Y+_y_of011);
//m						MpsBrewLib_DrawSprite( spPaiJicyaSmall[Bbit+1],-(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X+D_PAI_YOKO_SMALL_SIZE_W+D_PAI_TATE_SMALL_SIZE_W, D_JICYA_TEHAI_Y+_y_of011);
						MpsBrewLib_DrawSprite( spSutePaiJicya[Bbit+1],-(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X+D_PAI_YOKO_SMALL_SIZE_W+D_PAI_TATE_SMALL_SIZE_W, D_JICYA_TEHAI_Y+_y_of011);
						break;
					}
//m					default:
//m						//DBGPRINTF("");
//m						break;
				}
				break;
		} // switch( Hbit )
	} // for
#endif
#endif //-*todo:描画保留

} // MJ_Draw_Jicya

#if	Rule_2P
#else
public const int D_SUTEPAI_OFFSET_SHIMOCYA	15
/*===========================================================================
								下家の描画全般
===========================================================================*/
public void MJ_ShimocyaDraw( /*MahJongRally * pMe,*/ int House )
{
	int i		= 0;	// ループ
	int j		= 0;	// チーループ用。
	int sutecnt	= 0;	// 捨て牌数。
	int pai		= 0;	// 牌番号取得用。
	int csr		= 0;	// 牌カーソル位置
	int Fr		= 0;	// フーロ牌チェック用
	int FrPos	= 0;	// 左0・中心1・右2．
	int Bbit	= 0;	// フーロ牌情報取得
	int Hbit	= 0;	// 上位ビット
	int SBit	= 0;	// 下位ビット
	int offsetX	= 0;	// 捨て牌のオフセット値
	int offsetY	= 0;	// 捨て牌のオフセット値
	int suteFlag= 0;	// リーチ or リーチ？
	SINT2 dx	= 0;
	SINT2 dy	= 0;
	SINT2 flg	= 0;
	SINT2 no	= 0;
	int furoOffY = 0;	// フーロ牌用のOFFset
	int furoOffBaseY = 0;	// フーロ牌用のOFFset
	int	DrawCount=0;

	int	reached_add	= -3;
	int reached_line= 10;

	// 現在の牌カーソル値の取得
	csr = hai_csr;

	/*===========================================================================
								捨て牌数の描画
	===========================================================================*/
	sutecnt		 = gsPlayerWork[House].byShcnt;				// 捨て牌数取得。
	DrawCount	 = sutecnt-1-getKakarenaihai( House );	// 鳴かれた牌数を取得。
	reached_line = getReachLine_Shimocya( House, D_SHIMOCYA );

	for( i=sutecnt-1 ; i>=0 ; i-- ) {
		suteFlag	= Rec_sute_pos[House][i];				// 捨て牌がリーチかどうかをGET。
		pai			= gsPlayerWork[House].bySthai[i];		// 河の捨て牌取得。
		offsetX		= DrawCount/12;			// /8			// 段の位置
		offsetY		= DrawCount%12;			// %8			// 横の位置

		switch( suteFlag ) {
			case SUTEHAI_ON: {
				// --- リーチしたラインならば。
				if( reached_line == offsetX ) {

					MpsBrewLib_DrawSprite( spSutePaiShimocya[pai], (D_SHIMOCYA_RIVER_X+(offsetX*19)),
																(D_SHIMOCYA_RIVER_Y-(offsetY*D_SUTEPAI_OFFSET_SHIMOCYA)+reached_add));

				} else {
					// --- 通常牌。
					MpsBrewLib_DrawSprite( spSutePaiShimocya[pai], D_SHIMOCYA_RIVER_X+(offsetX*19),
																D_SHIMOCYA_RIVER_Y-(offsetY*D_SUTEPAI_OFFSET_SHIMOCYA) );
				}

				DrawCount-=1;
				break;
			}
			case SUTEHAI_OFF:
				break;
			case SUTEHAI_REACH: {
				MpsBrewLib_DrawSprite( spSutePaiToicya[pai], D_SHIMOCYA_RIVER_X+(offsetX*19)+4,// +3は固定。下側に揃える為。
														  D_SHIMOCYA_RIVER_Y-(offsetY*D_SUTEPAI_OFFSET_SHIMOCYA)-4						 // -3は固定。そうしないと牌と重なる。
									 );
				DrawCount-=1;
				reached_add = 0;
				reached_line= 0;

				break;
			}
		}
	}

	/*===========================================================================
								手牌の描画
	===========================================================================*/

	///*===========================================================================
	//							ツモ牌の描画
	//===========================================================================*/
	dx = 0;
	dy = 0;

	i = gsPlayerWork[House].byThcnt;	// 手牌カウント。

	if(i!=0) {
		i = gsPlayerWork[House].byThcnt-1;	// 手牌カウント。
		flg = ((hai_open & 0x2) != 0) ? -1 : (SINT2)(gsPlayerWork[House].bFrich != 0 ? 1 : 0);	// リーチフラグ？
		// オープンフラグが立ってかつリーチの時は１。それ以外は０。オープンフラグが立っていないときは－1
		//do
		for(;i>=-1;i--) {
			if( i >= 0 )	no = (flg < 0) ? (SINT2)gsPlayerWork[House].byTehai[(i)] : PAI_URA;		//

			// --- 手牌分回したら
			if( i == -1 ) {
				if( gsPlayerWork[House].byTkhai == 0 )	//
					break;

				no = PAI_URA;	//gsPlayerWork[House].byHkhai;	// 現時点での捨てられた牌（ツモ牌）
				//if( flg > 0) 20051117 del
				if( flg < 0) {
					flg = 0;							//
					no = (SINT2)gsPlayerWork[House].byHkhai;	// 現時点での捨てられた牌（ツモ牌）
				}
				dy -= (gsPlayerWork[House].byThcnt*10)+20;
			}
			//pri = 25 - i;
			if( flg == 1 ) {
				MpsBrewLib_DrawSprite( spPaiShimocya[no],D_SHIMOCYA_TEHAI_X,D_SHIMOCYA_TEHAI_Y-(i*10)+dy);
			} else {
				// ここに入ると落ちる？
#if	__OPEN_PAI
				if( i>= 0)
					MpsBrewLib_DrawSprite( spPaiShimocya[gsPlayerWork[House].byTehai[i]],D_SHIMOCYA_TEHAI_X,D_SHIMOCYA_TEHAI_Y-(i*10)+dy);
				else
#endif
					MpsBrewLib_DrawSprite( spPaiShimocya[no],D_SHIMOCYA_TEHAI_X,D_SHIMOCYA_TEHAI_Y-(i*10)+dy);
			}
			//dy += 10;
		}
		//}while( (--i) >= 0);	// 手牌分ループ。
	}

	/*===========================================================================
								フーロ牌の描画
		注：ここの処理より下にコードを書く場合は
			if( Fr == 0 ) return;の解除を行うこと
	===========================================================================*/
	// フーロ牌があるかチェック
	Fr = gsPlayerWork[House].byFhcnt;

	if( Fr == 0 ) return;																	// フーロ牌が無ければ。
	if( Fr > 4 ) Fr = 4;

	// フーロ牌の情報取得
	for( i=0 ; i<Fr ; i++ )
	{
		Bbit = gsPlayerWork[House].byFrhai[i];												// フーロ牌情報取得。

		//Frに4以上が入る不具合発生のため暫定的にここで処理を入れる 20051202
		if(Bbit == 0) return;

		FrPos= gsPlayerWork[House].byFrpos[i];												// 鳴き場所取得。

		Hbit = Bbit & 0xC0;																	// 上位ビットの取得。
		switch( Hbit )
		{
			// 明カン(下家)
			case D_MINKAN:
			{

				SBit = Bbit & 0x3F;															// 下位ビットの取得

				furoOffY = 0;
				furoOffBaseY = 0;

				if( FrPos==1)
				{
					// 済。
					// 通常牌。
					MpsBrewLib_DrawSprite( spPaiShimocya[SBit],	D_SHIMOCYA_TEHAI_X, (D_SHIMO_FH_BASE_Y+(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)-furoOffY)-22);
					MpsBrewLib_DrawSprite( spPaiShimocya[SBit],	D_SHIMOCYA_TEHAI_X, (D_SHIMO_FH_BASE_Y+(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)-furoOffY)-11);

					// 横になる牌。
					MpsBrewLib_DrawSprite( spPaiToicya[SBit],	3+D_SHIMOCYA_TEHAI_X-11, D_SHIMO_FH_BASE_Y+((i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)-furoOffY)-0);
					MpsBrewLib_DrawSprite( spPaiToicya[SBit],	3+D_SHIMOCYA_TEHAI_X, D_SHIMO_FH_BASE_Y+((i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)-furoOffY)-0);
				}
				if( FrPos==2 )
				{
					// 済。
					// 通常牌。
					MpsBrewLib_DrawSprite( spPaiShimocya[SBit],	D_SHIMOCYA_TEHAI_X, (D_SHIMO_FH_BASE_Y+(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)-furoOffY)-26);
					// 横になる牌。
					MpsBrewLib_DrawSprite( spPaiToicya[SBit],	3+D_SHIMOCYA_TEHAI_X-11, D_SHIMO_FH_BASE_Y+((i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)-furoOffY)-15);
					MpsBrewLib_DrawSprite( spPaiToicya[SBit],	3+D_SHIMOCYA_TEHAI_X, D_SHIMO_FH_BASE_Y+((i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)-furoOffY)-15);
					// 通常牌。
					MpsBrewLib_DrawSprite( spPaiShimocya[SBit],	D_SHIMOCYA_TEHAI_X, (D_SHIMO_FH_BASE_Y+(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)-furoOffY)-0);
				}
				if( FrPos==3)
				{
					// 済。
					// 横になる牌
					MpsBrewLib_DrawSprite( spPaiToicya[SBit],	3+D_SHIMOCYA_TEHAI_X-11, D_SHIMO_FH_BASE_Y+((i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)-furoOffY)-22);
					MpsBrewLib_DrawSprite( spPaiToicya[SBit],	3+D_SHIMOCYA_TEHAI_X, D_SHIMO_FH_BASE_Y+((i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)-furoOffY)-22);

					// 通常牌
					MpsBrewLib_DrawSprite( spPaiShimocya[SBit],	D_SHIMOCYA_TEHAI_X, (D_SHIMO_FH_BASE_Y+(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)-furoOffY)-11);
					MpsBrewLib_DrawSprite( spPaiShimocya[SBit],	D_SHIMOCYA_TEHAI_X, (D_SHIMO_FH_BASE_Y+(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)-furoOffY)-0);
				}

				break;
			}

			// 暗カン(両端を裏にして表示)(下家)
			case D_ANKAN:
			{
				SBit = Bbit & 0x3F;															// 下位ビットの取得

				{
					// 済。
					MpsBrewLib_DrawSprite( spPaiShimocya[PAI_URA],	D_SHIMOCYA_TEHAI_X, D_SHIMO_FH_BASE_Y+(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)-(2*D_PAI_YOKO_SMALL_SIZE_W)+2);
					MpsBrewLib_DrawSprite( spPaiToicya[SBit],	D_SHIMOCYA_TEHAI_X-8, (D_SHIMO_FH_BASE_Y+(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)-(1*D_PAI_YOKO_SMALL_SIZE_W)+0));
					MpsBrewLib_DrawSprite( spPaiToicya[SBit],	D_SHIMOCYA_TEHAI_X+3, (D_SHIMO_FH_BASE_Y+(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)-(1*D_PAI_YOKO_SMALL_SIZE_W)+0));
					MpsBrewLib_DrawSprite( spPaiShimocya[PAI_URA],	D_SHIMOCYA_TEHAI_X, D_SHIMO_FH_BASE_Y+(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)-(0*D_PAI_YOKO_SMALL_SIZE_W)+0);
				}

				break;
			}

			// ポン
			case D_PON:
			{
				SBit = Bbit & 0x3F;															// 下位ビットの取得
				furoOffY = D_PAI_YOKO_SMALL_SIZE_W + D_PAI_TATE_SMALL_SIZE_W;

				for( j=3 ; j>=1 ; j-- )
				{
					if( j == FrPos )
					{
						// 左・中心・右のどこに置くか
						MpsBrewLib_DrawSprite( spPaiToicya[SBit],	3+D_SHIMOCYA_TEHAI_X, D_SHIMO_FH_BASE_Y+(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)-furoOffY);
						furoOffY = furoOffY - D_PAI_YOKO_SMALL_SIZE_W;
						continue;
					}
					MpsBrewLib_DrawSprite( spPaiShimocya[SBit],	D_SHIMOCYA_TEHAI_X, D_SHIMO_FH_BASE_Y+(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)-furoOffY);
					furoOffY = furoOffY - D_PAI_TATE_SMALL_SIZE_W;
				}
				break;
			}

			// チー
			default:
			{
				// 牌を横にする場所の決定
				switch( FrPos )
				{
					// 一番小さい数字
					case 0:
					{
						MpsBrewLib_DrawSprite( spPaiShimocya[Bbit+2],	D_SHIMOCYA_TEHAI_X, D_SHIMO_FH_BASE_Y+(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)-26);
						MpsBrewLib_DrawSprite( spPaiShimocya[Bbit+1],	D_SHIMOCYA_TEHAI_X, D_SHIMO_FH_BASE_Y+(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)-15);
						MpsBrewLib_DrawSprite( spPaiToicya[Bbit+0],	3+D_SHIMOCYA_TEHAI_X, D_SHIMO_FH_BASE_Y+(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)-4);
						break;
					}

					// 真ん中の数字
					case 1:
					{
						MpsBrewLib_DrawSprite( spPaiShimocya[Bbit+2],	D_SHIMOCYA_TEHAI_X, D_SHIMO_FH_BASE_Y+(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)-26);
						MpsBrewLib_DrawSprite( spPaiShimocya[Bbit+0],	D_SHIMOCYA_TEHAI_X, D_SHIMO_FH_BASE_Y+(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)-15);
						MpsBrewLib_DrawSprite( spPaiToicya[Bbit+1],	3+D_SHIMOCYA_TEHAI_X, D_SHIMO_FH_BASE_Y+(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)-4);
						break;
					}

					// 一番大きい数字
					case 2:
					{
						MpsBrewLib_DrawSprite( spPaiShimocya[Bbit+1],	D_SHIMOCYA_TEHAI_X, D_SHIMO_FH_BASE_Y+(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)-26);
						MpsBrewLib_DrawSprite( spPaiShimocya[Bbit+0],	D_SHIMOCYA_TEHAI_X, D_SHIMO_FH_BASE_Y+(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)-15);
						MpsBrewLib_DrawSprite( spPaiToicya[Bbit+2],	3+D_SHIMOCYA_TEHAI_X, D_SHIMO_FH_BASE_Y+(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)-4);
						break;
					}
				}
				break;
			}//default
		}//switch
	}//for
	return;
}
#endif

public const int D_SUTEPAI_OFFSET_TOICYA		= 13;		//15
public const int D_SUTEPAI_REACH_OFF			= (-4);	//(-3)
/*===========================================================================
								対家の描画全般
===========================================================================*/
public void MJ_ToicyaDraw( /*MahJongRally * pMe,*/ int House )
{
	DebLog("//-*MJ:GameDraw.cs:MJ_ToicyaDraw( int "+House+" )");
	
#if false//-*todo:描画保留
	int i		= 0;	// ループ
	int j		= 0;	// チーループ用。
	int suteCnt = 0;	// 捨て牌数。
	int pai		= 0;	// 描画する牌の番号
	int csr		= 0;	// 牌カーソル位置
	int Fr		= 0;	// フーロ牌チェック用
	int FrPos	= 0;	// 左0・中心1・右2．
	int Bbit	= 0;	// フーロ牌情報取得
	int Hbit	= 0;	// 上位ビット
	int SBit	= 0;	// 下位ビット
	int offsetX = 0;	// 捨て牌のオフセット値
	int offsetY = 0;	// 捨て牌のオフセット値
	int suteFlag= 0;	// リーチ or リーチ？
	int	flg		= 0;	// 牌オープン？
	int	dx		= 0;	//
	int	no		= 0;	//
	int furoOffX = 0;	// フーロ牌用のOFFset
	int furoOffBaseX = 0;	// フーロ牌用のOFFset
	int	DrawCount=0;
	int	reached_add= D_SUTEPAI_REACH_OFF;
	int	reached_line= 10;

	// 現在の牌カーソル値の取得
	csr = hai_csr;

	/*===========================================================================
								捨て牌数の描画
	===========================================================================*/
	suteCnt		 = gsPlayerWork[House].byShcnt;				// 捨て牌数取得。
#if	Rule_2P
	//4回に分けて描画する
	//0:上の列のリーチ牌
	//1:上の列
	//2:下の列のリーチ牌
	//3:下の列
	for( j= 0; j< 4; j++ ) {
		reached_add= D_SUTEPAI_REACH_OFF;
#endif
	DrawCount	 = suteCnt-1-getKakarenaihai( House );		// 鳴かれた牌数を取得。
	reached_line = getReachLine_Toicya( House, D_TOICYA );

	for( i= suteCnt- 1; i>= 0; i-- ) {
		suteFlag	= Rec_sute_pos[House][i];				// 捨て牌がリーチかどうかをGET。
		pai			= gsPlayerWork[House].bySthai[i];		// 河の捨て牌取得。
		offsetX		= DrawCount%12;		// %8				// 横の位置
		offsetY		= DrawCount/12;		// /8				// 段の位置
		switch( suteFlag ) {
			case (int)SUTEHAI.ON:
#if	Rule_2P
				// --- リーチしたラインならば。
				if((( j== 1) && ( offsetY!= 0)) || (( j== 3) && ( offsetY== 0))) {
					if( reached_line == offsetY )
						hMpsBrewLib_DrawSprite( spSutePaiToicya, pai,
							(D_TOICYA_RIVER_X-(offsetX*D_SUTEPAI_OFFSET_TOICYA))+ reached_add,
							(D_TOICYA_RIVER_Y-(offsetY*D_SUTEPAI_OFFSET_H)));
					else
						// --- 通常牌。
						hMpsBrewLib_DrawSprite( spSutePaiToicya, pai,
							D_TOICYA_RIVER_X-(offsetX*D_SUTEPAI_OFFSET_TOICYA),
							D_TOICYA_RIVER_Y-(offsetY*D_SUTEPAI_OFFSET_H));
				}
#else
				// --- リーチしたラインならば。
				if( reached_line == offsetY )
					MpsBrewLib_DrawSprite( spSutePaiToicya[pai],
						(D_TOICYA_RIVER_X-(offsetX*D_SUTEPAI_OFFSET_TOICYA))+ reached_add,
						(D_TOICYA_RIVER_Y-(offsetY*D_SUTEPAI_OFFSET_H)));
				else
					// --- 通常牌。
					MpsBrewLib_DrawSprite( spSutePaiToicya[pai],
						D_TOICYA_RIVER_X-(offsetX*D_SUTEPAI_OFFSET_TOICYA),
						D_TOICYA_RIVER_Y-(offsetY*D_SUTEPAI_OFFSET_H));
#endif
				DrawCount-= 1;
				break;
//m			case SUTEHAI_OFF:
//m				break;
			case (int)SUTEHAI.REACH:
				// リーチ牌
#if	Rule_2P
				if((( j== 0) && ( offsetY!= 0)) || (( j== 2) && ( offsetY== 0)))
					hMpsBrewLib_DrawSprite_line4( spSutePaiShimocya, pai,
#else
//m					MpsBrewLib_DrawSprite( spSutePaiKamicya[pai],
					MpsBrewLib_DrawSprite( spSutePaiShimocya[pai],
#endif
						D_TOICYA_RIVER_X-(offsetX*D_SUTEPAI_OFFSET_TOICYA)+ D_SUTEPAI_REACH_OFF,
						D_TOICYA_RIVER_Y-(offsetY*D_SUTEPAI_OFFSET_H));
				DrawCount-= 1;
				reached_add =
				reached_line= 0;
				break;
		}
	}
#if	Rule_2P
	}
#endif

	i = gsPlayerWork[House].byThcnt;	//手牌の数
	flg = (hai_open & 0x4) != 0 ? -1 :
		(gsPlayerWork[House].bFrich != 0 ? 1 : 0);

	do{
		if( i >= 0 )
			no = (flg < 0) ? gsPlayerWork[House].byTehai[(gsPlayerWork[House].byThcnt - i)] : PAI_URA;
		if( i == 0) {
			if( gsPlayerWork[House].byTkhai == 0)
				break;

			no = (int)PAI.URA;	//gsPlayerWork[House].byHkhai;
			if( flg < 0) {
				flg = 0;
				no = gsPlayerWork[House].byHkhai;	// 現時点での捨てられた牌（ツモ牌）
			}
			dx -= 5;
		}

		if( flg != 0 )
//m			MpsBrewLib_DrawSprite( spPaiToicya[no],
#if	Rule_2P
			hMpsBrewLib_DrawSprite( spSutePaiToicya, no,
#else
			MpsBrewLib_DrawSprite( spSutePaiToicya[no],
#endif
				D_TOICYA_TEHAI_X+(dx),
				D_TOICYA_TEHAI_Y );
		else {
#if	__OPEN_PAI
				if( i>= 0)
#if	Rule_2P
					hMpsBrewLib_DrawSprite( spSutePaiToicya, gsPlayerWork[House].byTehai[(gsPlayerWork[House].byThcnt - i)],
#else
//m					MpsBrewLib_DrawSprite( spPaiToicya[gsPlayerWork[House].byTehai[(gsPlayerWork[House].byThcnt - i)]],
					MpsBrewLib_DrawSprite( spSutePaiToicya[gsPlayerWork[House].byTehai[(gsPlayerWork[House].byThcnt - i)]],
#endif
						D_TOICYA_TEHAI_X+(dx),
						D_TOICYA_TEHAI_Y );
				else
#endif
#if	Rule_2P
					hMpsBrewLib_DrawSprite( spSutePaiToicya, no,
#else
//m					MpsBrewLib_DrawSprite( spPaiToicya[no],
					MpsBrewLib_DrawSprite( spSutePaiToicya[no],
#endif
						D_TOICYA_TEHAI_X+(dx),
						D_TOICYA_TEHAI_Y );
		}
		dx -= 13;		//m	dx -= 11;
	}while( (--i) >= 0);

	/*===========================================================================
			フーロ牌の描画
		注：ここの処理より下にコードを書く場合は
			if( Fr == 0 ) return;の解除を行うこと
	===========================================================================*/
	// フーロ牌があるかチェック
	Fr = gsPlayerWork[House].byFhcnt;

	if( Fr == 0 ) return;							// フーロ牌が無ければ。
	if( Fr > 4 ) Fr = 4;

	// フーロ牌の情報取得
	for( i=0 ; i<Fr ; i++ ) {
		Bbit = gsPlayerWork[House].byFrhai[i];		// フーロ牌情報取得。

		//Frに4以上が入る不具合発生のため暫定的にここで処理を入れる 20051202
		if(Bbit == 0)	return;

#if	Rule_2P
		FrPos= 2;
#else
		FrPos= gsPlayerWork[House].byFrpos[i];		// 鳴き場所取得。
#endif

		Hbit = Bbit & 0xC0;							// 上位ビットの取得。
		switch( Hbit ) {
			// 明カン(対家)
			case D_MINKAN: {
				// 下位ビットの取得
				SBit = Bbit & 0x3F;
				furoOffX = 0;
				furoOffBaseX = 0;
				{
					if( FrPos==1) {
						// 済。
						// -- 対家通常牌。
//m						MpsBrewLib_DrawSprite( spPaiToicya[SBit], (D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+furoOffX)+0, D_TOICYA_TEHAI_Y);
//m						MpsBrewLib_DrawSprite( spPaiToicya[SBit], (D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+furoOffX)+11, D_TOICYA_TEHAI_Y);
#if	Rule_2P
						hMpsBrewLib_DrawSprite( spSutePaiToicya, SBit,
#else
						MpsBrewLib_DrawSprite( spSutePaiToicya[SBit],
#endif
							(D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+furoOffX)+0,
							D_TOICYA_TEHAI_Y);
#if	Rule_2P
						hMpsBrewLib_DrawSprite( spSutePaiToicya, SBit,
#else
						MpsBrewLib_DrawSprite( spSutePaiToicya[SBit],
#endif
							(D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+furoOffX)+11+2,
							D_TOICYA_TEHAI_Y);
						// 横になる牌。
#if	Rule_2P
						hMpsBrewLib_DrawSprite_line4( spSutePaiShimocya, SBit, (D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+furoOffX)+22, D_TOICYA_TEHAI_Y);
						hMpsBrewLib_DrawSprite_line4( spSutePaiShimocya, SBit, (D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+furoOffX)+22, D_TOICYA_TEHAI_Y+11+2);
#else
//m						MpsBrewLib_DrawSprite( spPaiKamicya[SBit], (D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+furoOffX)+22, D_TOICYA_TEHAI_Y);
//m						MpsBrewLib_DrawSprite( spPaiKamicya[SBit], (D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+furoOffX)+22, D_TOICYA_TEHAI_Y+11);
						MpsBrewLib_DrawSprite( spSutePaiShimocya[SBit], (D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+furoOffX)+22, D_TOICYA_TEHAI_Y);
						MpsBrewLib_DrawSprite( spSutePaiShimocya[SBit], (D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+furoOffX)+22, D_TOICYA_TEHAI_Y+11+2);
#endif
					}
					if( FrPos==2 ) {
						// 済。
						// -- 対家通常牌。(右)	(影の問題で先に右から書く)
#if	Rule_2P
						hMpsBrewLib_DrawSprite( spSutePaiToicya, SBit, (D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+furoOffX)+24+2+4, D_TOICYA_TEHAI_Y);
#else
//m						MpsBrewLib_DrawSprite( spPaiToicya[SBit], (D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+furoOffX)+24, D_TOICYA_TEHAI_Y);
						MpsBrewLib_DrawSprite( spSutePaiToicya[SBit], (D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+furoOffX)+24, D_TOICYA_TEHAI_Y);
#endif

						// -- 対家通常牌。(中２つ)
#if	Rule_2P
						hMpsBrewLib_DrawSprite( spSutePaiToicya, SBit, (D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+furoOffX)+0, D_TOICYA_TEHAI_Y);
#else
//m						MpsBrewLib_DrawSprite( spPaiToicya[SBit], (D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+furoOffX)+0, D_TOICYA_TEHAI_Y);
						MpsBrewLib_DrawSprite( spSutePaiToicya[SBit], (D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+furoOffX)+0, D_TOICYA_TEHAI_Y);
#endif
						// 横になる牌。(左)
#if	Rule_2P
						hMpsBrewLib_DrawSprite_line4( spSutePaiShimocya, SBit, (D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+furoOffX)+11+2, D_TOICYA_TEHAI_Y);
						hMpsBrewLib_DrawSprite_line4( spSutePaiShimocya, SBit, (D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+furoOffX)+11+2, D_TOICYA_TEHAI_Y+11+2);
#else
//m						MpsBrewLib_DrawSprite( spPaiKamicya[SBit], (D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+furoOffX)+11, D_TOICYA_TEHAI_Y);
//m						MpsBrewLib_DrawSprite( spPaiKamicya[SBit], (D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+furoOffX)+11, D_TOICYA_TEHAI_Y+11);
						MpsBrewLib_DrawSprite( spSutePaiShimocya[SBit], (D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+furoOffX)+11, D_TOICYA_TEHAI_Y);
						MpsBrewLib_DrawSprite( spSutePaiShimocya[SBit], (D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+furoOffX)+11, D_TOICYA_TEHAI_Y+11+2);
#endif
					}
					if( FrPos==3) {
						// 済。
						// -- 横になる牌。
#if	Rule_2P
						hMpsBrewLib_DrawSprite_line4( spSutePaiShimocya, SBit, (D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+furoOffX)+0, D_TOICYA_TEHAI_Y);
						hMpsBrewLib_DrawSprite_line4( spSutePaiShimocya, SBit, (D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+furoOffX)+0, D_TOICYA_TEHAI_Y+11+2);
#else
//m						MpsBrewLib_DrawSprite( spPaiKamicya[SBit], (D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+furoOffX)+0, D_TOICYA_TEHAI_Y);
//m						MpsBrewLib_DrawSprite( spPaiKamicya[SBit], (D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+furoOffX)+0, D_TOICYA_TEHAI_Y+11);
						MpsBrewLib_DrawSprite( spSutePaiShimocya[SBit], (D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+furoOffX)+0, D_TOICYA_TEHAI_Y);
						MpsBrewLib_DrawSprite( spSutePaiShimocya[SBit], (D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+furoOffX)+0, D_TOICYA_TEHAI_Y+11+2);
#endif
						// -- 対家通常牌。
#if	Rule_2P
						hMpsBrewLib_DrawSprite( spSutePaiToicya, SBit, (D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+furoOffX)+25, D_TOICYA_TEHAI_Y);
						hMpsBrewLib_DrawSprite( spSutePaiToicya, SBit, (D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+furoOffX)+14, D_TOICYA_TEHAI_Y);
#else
//m						MpsBrewLib_DrawSprite( spPaiToicya[SBit], (D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+furoOffX)+25, D_TOICYA_TEHAI_Y);
//m						MpsBrewLib_DrawSprite( spPaiToicya[SBit], (D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+furoOffX)+14, D_TOICYA_TEHAI_Y);
						MpsBrewLib_DrawSprite( spSutePaiToicya[SBit], (D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+furoOffX)+25, D_TOICYA_TEHAI_Y);
						MpsBrewLib_DrawSprite( spSutePaiToicya[SBit], (D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+furoOffX)+14, D_TOICYA_TEHAI_Y);
#endif
					}
				}
				break;
			}

			// 暗カン(両端を裏にして表示)(対家)
			case D_ANKAN: {
				SBit = Bbit & 0x3F;					// 下位ビットの取得
				{
					// 済。
#if	Rule_2P
					hMpsBrewLib_DrawSprite( spSutePaiToicya, PAI_URA,	D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(0*D_PAI_TATE_SMALL_SIZE_W+furoOffBaseX)+0,D_TOICYA_TEHAI_Y);
					hMpsBrewLib_DrawSprite( spSutePaiToicya, PAI_URA,	D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(2*D_PAI_TATE_SMALL_SIZE_W+furoOffBaseX)+3,D_TOICYA_TEHAI_Y);
					hMpsBrewLib_DrawSprite_line4( spSutePaiShimocya, SBit,	D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(1*D_PAI_TATE_SMALL_SIZE_W+furoOffBaseX)-0,D_TOICYA_TEHAI_Y+0);
					hMpsBrewLib_DrawSprite_line4( spSutePaiShimocya, SBit,	D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(1*D_PAI_TATE_SMALL_SIZE_W+furoOffBaseX)-0,D_TOICYA_TEHAI_Y+11+2);
#else
//m					MpsBrewLib_DrawSprite( spPaiToicya[PAI_URA],	D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(0*D_PAI_TATE_SMALL_SIZE_W+furoOffBaseX)+0,D_TOICYA_TEHAI_Y);
//m					MpsBrewLib_DrawSprite( spPaiToicya[PAI_URA],	D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(2*D_PAI_TATE_SMALL_SIZE_W+furoOffBaseX)+3,D_TOICYA_TEHAI_Y);
					MpsBrewLib_DrawSprite( spSutePaiToicya[PAI_URA],	D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(0*D_PAI_TATE_SMALL_SIZE_W+furoOffBaseX)+0,D_TOICYA_TEHAI_Y);
					MpsBrewLib_DrawSprite( spSutePaiToicya[PAI_URA],	D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(2*D_PAI_TATE_SMALL_SIZE_W+furoOffBaseX)+3,D_TOICYA_TEHAI_Y);
//m					MpsBrewLib_DrawSprite( spPaiKamicya[SBit],		D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(1*D_PAI_TATE_SMALL_SIZE_W+furoOffBaseX)-0,D_TOICYA_TEHAI_Y+0);
//m					MpsBrewLib_DrawSprite( spPaiKamicya[SBit],		D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(1*D_PAI_TATE_SMALL_SIZE_W+furoOffBaseX)-0,D_TOICYA_TEHAI_Y+11);
					MpsBrewLib_DrawSprite( spSutePaiShimocya[SBit],		D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(1*D_PAI_TATE_SMALL_SIZE_W+furoOffBaseX)-0,D_TOICYA_TEHAI_Y+0);
					MpsBrewLib_DrawSprite( spSutePaiShimocya[SBit],		D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(1*D_PAI_TATE_SMALL_SIZE_W+furoOffBaseX)-0,D_TOICYA_TEHAI_Y+11+2);
#endif
				}
				break;
			}
			// ポン
			case D_PON: {
				SBit = Bbit & 0x3F;					// 下位ビットの取得
				furoOffX = 0;
#if	Rule_2P
				for( j=3 ; j>=1 ; j-- )
					if( j == FrPos ) {
						// 左・中心・右のどこに置くか (横)
						hMpsBrewLib_DrawSprite_line4( spSutePaiShimocya, SBit,
							D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+furoOffX,
							D_TOICYA_TEHAI_Y);
						furoOffX = furoOffX+ D_PAI_YOKO_SMALL_SIZE_W;
					} else
						furoOffX = furoOffX+ D_PAI_TATE_SMALL_SIZE_W;
				furoOffX = 0;
				for( j=3 ; j>=1 ; j-- )
					if( j == FrPos )
						furoOffX = furoOffX+ D_PAI_YOKO_SMALL_SIZE_W;
					else {
						hMpsBrewLib_DrawSprite( spSutePaiToicya, SBit,
							D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+furoOffX,
							D_TOICYA_TEHAI_Y);
						furoOffX = furoOffX+ D_PAI_TATE_SMALL_SIZE_W;
					}
#else
				for( j=3 ; j>=1 ; j-- ) {
					if( j == FrPos ) {
						// 左・中心・右のどこに置くか (横)
#if	Rule_2P
						hMpsBrewLib_DrawSprite_line4( spSutePaiShimocya, SBit,
#else
//m						MpsBrewLib_DrawSprite( spPaiKamicya[SBit],	D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+furoOffX,D_TOICYA_TEHAI_Y);
						MpsBrewLib_DrawSprite( spSutePaiShimocya[SBit],
#endif
							D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+furoOffX,
							D_TOICYA_TEHAI_Y);
						furoOffX = furoOffX+ D_PAI_YOKO_SMALL_SIZE_W;
						continue;
					}
//m					MpsBrewLib_DrawSprite( spPaiToicya[SBit],	D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+furoOffX,D_TOICYA_TEHAI_Y);
#if	Rule_2P
					hMpsBrewLib_DrawSprite( spSutePaiToicya, SBit,
#else
					MpsBrewLib_DrawSprite( spSutePaiToicya[SBit],
#endif
						D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+furoOffX,
						D_TOICYA_TEHAI_Y);
					furoOffX = furoOffX+ D_PAI_TATE_SMALL_SIZE_W;
				}
#endif
				break;
			}
			// チー
			default: {
				FrPos= gsPlayerWork[House].byFrpos[i];		// 鳴き場所取得。
				// 牌を横にする場所の決定
				switch( FrPos ) {
					// 一番小さい数字
					case 0: {
#if	Rule_2P
						hMpsBrewLib_DrawSprite_line4( spSutePaiShimocya, Bbit+0,	D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+2*D_PAI_TATE_SMALL_SIZE_W, D_TOICYA_TEHAI_Y);
						hMpsBrewLib_DrawSprite( spSutePaiToicya, Bbit+1,	D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_PAI_TATE_SMALL_SIZE_W, D_TOICYA_TEHAI_Y);
						hMpsBrewLib_DrawSprite( spSutePaiToicya, Bbit+2,	D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX), D_TOICYA_TEHAI_Y);
#else
//m						MpsBrewLib_DrawSprite( spPaiKamicya[Bbit+0],	D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+2*D_PAI_TATE_SMALL_SIZE_W, D_TOICYA_TEHAI_Y);
						MpsBrewLib_DrawSprite( spSutePaiShimocya[Bbit+0],	D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+2*D_PAI_TATE_SMALL_SIZE_W, D_TOICYA_TEHAI_Y);
//m						MpsBrewLib_DrawSprite( spPaiToicya[Bbit+1],	D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_PAI_TATE_SMALL_SIZE_W, D_TOICYA_TEHAI_Y);
//m						MpsBrewLib_DrawSprite( spPaiToicya[Bbit+2],	D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX), D_TOICYA_TEHAI_Y);
						MpsBrewLib_DrawSprite( spSutePaiToicya[Bbit+1],	D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_PAI_TATE_SMALL_SIZE_W, D_TOICYA_TEHAI_Y);
						MpsBrewLib_DrawSprite( spSutePaiToicya[Bbit+2],	D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX), D_TOICYA_TEHAI_Y);
#endif
						break;
					}
					// 真ん中の数字
					case 1: {
#if	Rule_2P
						hMpsBrewLib_DrawSprite_line4( spSutePaiShimocya, Bbit+1,	D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+2*D_PAI_TATE_SMALL_SIZE_W, D_TOICYA_TEHAI_Y);
						hMpsBrewLib_DrawSprite( spSutePaiToicya, Bbit+0,	D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_PAI_TATE_SMALL_SIZE_W, D_TOICYA_TEHAI_Y);
						hMpsBrewLib_DrawSprite( spSutePaiToicya, Bbit+2,	D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX), D_TOICYA_TEHAI_Y);
#else
//m						MpsBrewLib_DrawSprite( spPaiKamicya[Bbit+1],	D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+2*D_PAI_TATE_SMALL_SIZE_W, D_TOICYA_TEHAI_Y);
						MpsBrewLib_DrawSprite( spSutePaiShimocya[Bbit+1],	D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+2*D_PAI_TATE_SMALL_SIZE_W, D_TOICYA_TEHAI_Y);
//m						MpsBrewLib_DrawSprite( spPaiToicya[Bbit+0],	D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_PAI_TATE_SMALL_SIZE_W, D_TOICYA_TEHAI_Y);
//m						MpsBrewLib_DrawSprite( spPaiToicya[Bbit+2],	D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX), D_TOICYA_TEHAI_Y);
						MpsBrewLib_DrawSprite( spSutePaiToicya[Bbit+0],	D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_PAI_TATE_SMALL_SIZE_W, D_TOICYA_TEHAI_Y);
						MpsBrewLib_DrawSprite( spSutePaiToicya[Bbit+2],	D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX), D_TOICYA_TEHAI_Y);
#endif
						break;
					}
					// 一番大きい数字
					case 2: {
#if	Rule_2P
						hMpsBrewLib_DrawSprite_line4( spSutePaiShimocya, Bbit+2,	D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+2*D_PAI_TATE_SMALL_SIZE_W, D_TOICYA_TEHAI_Y);
						hMpsBrewLib_DrawSprite( spSutePaiToicya, Bbit+0,	D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_PAI_TATE_SMALL_SIZE_W, D_TOICYA_TEHAI_Y);
						hMpsBrewLib_DrawSprite( spSutePaiToicya, Bbit+1,	D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX), D_TOICYA_TEHAI_Y);
#else
//m						MpsBrewLib_DrawSprite( spPaiKamicya[Bbit+2],	D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+2*D_PAI_TATE_SMALL_SIZE_W, D_TOICYA_TEHAI_Y);
						MpsBrewLib_DrawSprite( spSutePaiShimocya[Bbit+2],	D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+2*D_PAI_TATE_SMALL_SIZE_W, D_TOICYA_TEHAI_Y);
//m						MpsBrewLib_DrawSprite( spPaiToicya[Bbit+0],	D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_PAI_TATE_SMALL_SIZE_W, D_TOICYA_TEHAI_Y);
//m						MpsBrewLib_DrawSprite( spPaiToicya[Bbit+1],	D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX), D_TOICYA_TEHAI_Y);
						MpsBrewLib_DrawSprite( spSutePaiToicya[Bbit+0],	D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_PAI_TATE_SMALL_SIZE_W, D_TOICYA_TEHAI_Y);
						MpsBrewLib_DrawSprite( spSutePaiToicya[Bbit+1],	D_TOI_FH_BASE_X+(i*D_PAI_TOTAL_SIZE+furoOffBaseX), D_TOICYA_TEHAI_Y);
#endif
						break;
					}
//m					default:
//m						break;
				}
				break;
			}//default
		}//switch
	}//for
//m	return;
#endif //-*todo:描画保留
}

#if	Rule_2P
#else
public const int D_SUTEPAI_OFFSET_KAMICYA_W	15
public const int D_SUTEPAI_OFFSET_KAMICYA_H	19
/*===========================================================================
								上家の描画全般
===========================================================================*/
public void MJ_KamicyaDraw( /*MahJongRally * pMe,*/ int House )
{
	int i		= 0;	// ループ
	int j		= 0;	// チーループ用。
	int suteCnt = 0;	// 捨て牌数。
	int pai		= 0;	// 牌番号取得用。
	int csr		= 0;	// 牌カーソル位置
	int Fr		= 0;	// フーロ牌チェック用
	int FrPos	= 0;	// 左0・中心1・右2．
	int Bbit	= 0;	// フーロ牌情報取得
	int Hbit	= 0;	// 上位ビット
	int SBit	= 0;	// 下位ビット
	int offsetX = 0;	// 捨て牌のオフセット値
	int offsetY = 0;	// 捨て牌のオフセット値
	int suteFlag= 0;	// リーチ or リーチ？
	int	flg		= 0;	// 牌オープン？
	int	dy		= 0;
	int	no		= 0;
	int furoOffY = 0;	// フーロ牌用のOFFset
	int furoOffBaseY = 0;	// フーロ牌用のOFFset
	short	DrawCount=0;
	int		reached_add	= 0;
	int		reached_line= 0;

	// 現在の牌カーソル値の取得
	csr = hai_csr;

	/*===========================================================================
								捨て牌数の描画
	===========================================================================*/
	suteCnt = gsPlayerWork[House].byShcnt;												// 捨て牌数取得。

	for( i=0 ; i<suteCnt ; i++ ) {
		suteFlag	= Rec_sute_pos[House][i];			// 捨て牌がリーチかどうかをGET。
		pai			= gsPlayerWork[House].bySthai[i];	// 河の捨て牌取得。
		offsetX		= DrawCount/12;			// /8		// 横の位置
		offsetY		= DrawCount%12;			// %8		// 段の位置

		switch( suteFlag ) {
			case SUTEHAI_ON: {
				// 通常牌
				if( reached_line == offsetX )
					MpsBrewLib_DrawSprite( spSutePaiKamicya[pai],D_KAMICYA_RIVER_X-(offsetX*D_SUTEPAI_OFFSET_KAMICYA_H),D_KAMICYA_RIVER_Y+reached_add+(offsetY*D_SUTEPAI_OFFSET_KAMICYA_W) );
				else
					MpsBrewLib_DrawSprite( spSutePaiKamicya[pai],D_KAMICYA_RIVER_X-(offsetX*D_SUTEPAI_OFFSET_KAMICYA_H),D_KAMICYA_RIVER_Y+(offsetY*D_SUTEPAI_OFFSET_KAMICYA_W) );
				DrawCount+=1;
				break;
			}
//m			case SUTEHAI_OFF:
//m				break;
			case SUTEHAI_REACH: {
				reached_add+=3;
				reached_line = offsetX;

				// リーチ牌
				MpsBrewLib_DrawSprite( spSutePaiJicya[pai],D_KAMICYA_RIVER_X-(offsetX*D_SUTEPAI_OFFSET_KAMICYA_H),D_KAMICYA_RIVER_Y+(offsetY*D_SUTEPAI_OFFSET_KAMICYA_W) );
				DrawCount+=1;
				break;
			}
		}
	}

	i = gsPlayerWork[House].byThcnt;	// 手牌カウント。
	flg = (hai_open & 0x8) != 0 ? -1 : (gsPlayerWork[House].bFrich != 0 ? 1 : 0);	// リーチフラグ？
	// オープンフラグが立ってかつリーチの時は１。それ以外は０。オープンフラグが立っていないときは－1
	do {
		if( i >= 0 )
			no = (flg < 0) ? gsPlayerWork[House].byTehai[(gsPlayerWork[House].byThcnt - i)] : PAI_URA;	//

		// --- 手牌分回したら
		if( i == 0 ) {
			if( gsPlayerWork[House].byTkhai == 0 )	//
				break;

			no = PAI_URA;		// gsPlayerWork[House].byHkhai;	// 現時点での捨てられた牌（ツモ牌）
			//if( flg > 0) 20051117 del
			if( flg < 0) {
				flg = 0;	//
				no = gsPlayerWork[House].byHkhai;	// 現時点での捨てられた牌（ツモ牌）
			}
			dy += 10;
		}
		//pri = 25 - i;
		if( flg != 0 ) {
			MpsBrewLib_DrawSprite( spPaiKamicya[no],D_KAMICYA_TEHAI_X,D_KAMICYA_TEHAI_Y+(dy));
		} else {
#if	__OPEN_PAI
				if( i>= 0)
					MpsBrewLib_DrawSprite( spPaiKamicya[gsPlayerWork[House].byTehai[(gsPlayerWork[House].byThcnt - i)]],D_KAMICYA_TEHAI_X,D_KAMICYA_TEHAI_Y+(dy));
				else
#endif
					MpsBrewLib_DrawSprite( spPaiKamicya[no],D_KAMICYA_TEHAI_X,D_KAMICYA_TEHAI_Y+(dy));
			j++;
		}
		dy += 10;
	}while( (--i) >= 0);	// 手牌分ループ。

	/*===========================================================================
								フーロ牌の描画
		注：ここの処理より下にコードを書く場合は
			if( Fr == 0 ) return;の解除を行うこと
	===========================================================================*/
	// フーロ牌があるかチェック
	Fr = gsPlayerWork[House].byFhcnt;

	if( Fr == 0 )	return;						// フーロ牌が無ければ。
	if( Fr > 4 )	Fr = 4;

	// フーロ牌の情報取得
	for( i=0 ; i<Fr ; i++ ) {
		Bbit = gsPlayerWork[House].byFrhai[i];	// フーロ牌情報取得。

		//Frに4以上が入る不具合発生のため暫定的にここで処理を入れる 20051202
		if(Bbit == 0) return;

		FrPos= gsPlayerWork[House].byFrpos[i];	// 鳴き場所取得。

		Hbit = Bbit & 0xC0;						// 上位ビットの取得。
		switch( Hbit ) {
			// 明カン(上家)
			case D_MINKAN: {
				SBit = Bbit & 0x3F;															// 下位ビットの取得
				furoOffY = 0;
				furoOffBaseY = 0;

				if( FrPos==1) {
					// 済。
					// --- 横になる牌。
					MpsBrewLib_DrawSprite( spPaiJicyaSmall[SBit], D_KAMICYA_TEHAI_X,-(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)+(furoOffY+D_KAMI_FH_BASE_Y)+0);
					MpsBrewLib_DrawSprite( spPaiJicyaSmall[SBit], D_KAMICYA_TEHAI_X+11,-(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)+(furoOffY+D_KAMI_FH_BASE_Y)+0);
					// -- 上家通常牌。
					MpsBrewLib_DrawSprite( spPaiKamicya[SBit],	D_KAMICYA_TEHAI_X,-(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)+(furoOffY+D_KAMI_FH_BASE_Y)+14);
					MpsBrewLib_DrawSprite( spPaiKamicya[SBit],	D_KAMICYA_TEHAI_X,-(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)+(furoOffY+D_KAMI_FH_BASE_Y)+24);
				}
				if( FrPos==2 ) {
					// 済。
					// -- 上家通常牌。
					MpsBrewLib_DrawSprite( spPaiKamicya[SBit],	D_KAMICYA_TEHAI_X,-(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)+(furoOffY+D_KAMI_FH_BASE_Y)+0);
					// --- 横になる牌。
					MpsBrewLib_DrawSprite( spPaiJicyaSmall[SBit], D_KAMICYA_TEHAI_X,-(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)+(furoOffY+D_KAMI_FH_BASE_Y)+11);
					MpsBrewLib_DrawSprite( spPaiJicyaSmall[SBit], D_KAMICYA_TEHAI_X+11,-(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)+(furoOffY+D_KAMI_FH_BASE_Y)+11);
					// -- 上家通常牌。
					MpsBrewLib_DrawSprite( spPaiKamicya[SBit],	D_KAMICYA_TEHAI_X,-(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)+(furoOffY+D_KAMI_FH_BASE_Y)+25);
				}
				if( FrPos==3) {
					// 済。
					// -- 上家通常牌。
					MpsBrewLib_DrawSprite( spPaiKamicya[SBit],	D_KAMICYA_TEHAI_X,-(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)+(furoOffY+D_KAMI_FH_BASE_Y)+0);
					MpsBrewLib_DrawSprite( spPaiKamicya[SBit],	D_KAMICYA_TEHAI_X,-(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)+(furoOffY+D_KAMI_FH_BASE_Y)+11);
					// --- 横になる牌。
					MpsBrewLib_DrawSprite( spPaiJicyaSmall[SBit], D_KAMICYA_TEHAI_X,-(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)+(furoOffY+D_KAMI_FH_BASE_Y)+22);
					MpsBrewLib_DrawSprite( spPaiJicyaSmall[SBit], D_KAMICYA_TEHAI_X+11,-(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)+(furoOffY+D_KAMI_FH_BASE_Y)+22);
				}
				break;
			}

			// 暗カン(両端を裏にして表示)(上家)
			case D_ANKAN: {
				SBit = Bbit & 0x3F;					// 下位ビットの取得
				furoOffBaseY=+furoOffBaseY + 3;		// 上家にだけは残しておく。
				{
					// 済。
					// 通常牌。
					MpsBrewLib_DrawSprite( spPaiKamicya[PAI_URA],D_KAMICYA_TEHAI_X, -(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)+(0*D_PAI_YOKO_SMALL_SIZE_W)+D_KAMI_FH_BASE_Y+0);
					// 横２段牌。
					MpsBrewLib_DrawSprite( spPaiJicyaSmall[SBit],D_KAMICYA_TEHAI_X+11, -(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)+(1*D_PAI_YOKO_SMALL_SIZE_W)+D_KAMI_FH_BASE_Y+0);
					MpsBrewLib_DrawSprite( spPaiJicyaSmall[SBit],D_KAMICYA_TEHAI_X+0, -(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)+(1*D_PAI_YOKO_SMALL_SIZE_W)+D_KAMI_FH_BASE_Y+0);
					// 通常牌。
					MpsBrewLib_DrawSprite( spPaiKamicya[PAI_URA],D_KAMICYA_TEHAI_X, -(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)+(2*D_PAI_YOKO_SMALL_SIZE_W)+D_KAMI_FH_BASE_Y+1);

				}
				break;
			}

			// ポン
			case D_PON: {
				SBit = Bbit & 0x3F;					// 下位ビットの取得
				furoOffY = 0;

				for( j=1 ; j<=3 ; j++ ) {
					if( j == FrPos ) {
						// 左・中心・右のどこに置くか
						MpsBrewLib_DrawSprite( spPaiJicyaSmall[SBit],D_KAMICYA_TEHAI_X, -(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)+furoOffY+D_KAMI_FH_BASE_Y);	// 240が適度
						furoOffY = furoOffY + D_PAI_YOKO_SMALL_SIZE_W;
						continue;
					}
					MpsBrewLib_DrawSprite( spPaiKamicya[SBit],	D_KAMICYA_TEHAI_X, -(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)+furoOffY+D_KAMI_FH_BASE_Y);
					furoOffY = furoOffY + D_PAI_TATE_SMALL_SIZE_W;
				}
				break;
			}

			// チー
			default: {
				switch( FrPos ) {
					case 0: {
						MpsBrewLib_DrawSprite( spPaiJicyaSmall[Bbit+0],D_KAMICYA_TEHAI_X, -(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)+D_KAMI_FH_BASE_Y);
						MpsBrewLib_DrawSprite( spPaiKamicya[Bbit+1],	D_KAMICYA_TEHAI_X, -(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)+15+D_KAMI_FH_BASE_Y);
						MpsBrewLib_DrawSprite( spPaiKamicya[Bbit+2],	D_KAMICYA_TEHAI_X, -(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)+26+D_KAMI_FH_BASE_Y);
						break;
					}
					case 1: {
						MpsBrewLib_DrawSprite( spPaiJicyaSmall[Bbit+1],D_KAMICYA_TEHAI_X, -(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)+D_KAMI_FH_BASE_Y);
						MpsBrewLib_DrawSprite( spPaiKamicya[Bbit+0],	D_KAMICYA_TEHAI_X, -(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)+15+D_KAMI_FH_BASE_Y);
						MpsBrewLib_DrawSprite( spPaiKamicya[Bbit+2],	D_KAMICYA_TEHAI_X, -(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)+26+D_KAMI_FH_BASE_Y);
						break;
					}
					case 2: {
						MpsBrewLib_DrawSprite( spPaiJicyaSmall[Bbit+2],D_KAMICYA_TEHAI_X, -(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)+D_KAMI_FH_BASE_Y);
						MpsBrewLib_DrawSprite( spPaiKamicya[Bbit+0],	D_KAMICYA_TEHAI_X, -(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)+15+D_KAMI_FH_BASE_Y);
						MpsBrewLib_DrawSprite( spPaiKamicya[Bbit+1],	D_KAMICYA_TEHAI_X, -(i*D_PAI_TOTAL_SIZE_Y+furoOffBaseY)+26+D_KAMI_FH_BASE_Y);
						break;
					}
				}
				break;
			} //default:
		} //switch
	} //for

//	return;
}
#endif

/*===========================================================================
							メニューの描画全般
===========================================================================*/
#if	Rule_2P
	public const int		D_MENU_BASE_X			= (185);
	public const int		D_MENU_BASE_Y			= (193);
#else
	public const int		D_MENU_BASE_X			(80)
	public const int		D_MENU_BASE_Y			(147 - 7)
	public const int		D_MENU_BASE_Y_W21_S		(D_MENU_BASE_Y + 2)
	public const int		D_MENU_BASE_Y_W21_T		(D_MENU_BASE_Y - 1)
#endif
public void MJ_GameMenuDraw( /*MahJongRally * pMe*/ )
{
	DebLog("//-*MJ:GameDraw.cs:MJ_GameMenuDraw()");
#if false//-*todo:描画保留
//	enum {
//		D_MENU_BASE_X		=	80, //160
//		D_MENU_BASE_Y		=	147 - 7,	// 2006/02/21 要望No.101
//		D_MENU_BASE_Y_W21_S	=	D_MENU_BASE_Y + 2,
//		D_MENU_BASE_Y_W21_T	=	D_MENU_BASE_Y - 1
//	};

	int		i			= 0;		// ループ
	byte	MenuNumber	= 0;		// メニュー番号がIn
	//> バグ No.67 要望 No.50 メニューの変更
	ushort	strNo		= 0;
	byte	offsetY_	= 0;
	String	charBuff= "";			//AECHAR	charBuff[10]={0};	// 読込んだ文字列.
	//< バグ No.67 要望 No.50 メニューの変更
	byte show_ = 0;

#if	Rule_2P
	_menuFlg= false;
#endif

	//> バグ No.67 要望 No.50 メニューの変更
//xxxx	SpriteInfo opt_spinfo_[]= new SpriteInfo [D_OPT_STK_MAX];
//xxxx	for( i = 0; i < D_OPT_STK_MAX; ++i )
//xxxx	{
//xxxx		opt_spinfo_[i] = NULL;
//xxxx	}
	//< バグ No.67 要望 No.50 メニューの変更

	// プレイヤーの入力待ち状態になるまでメニューを表示しない
	switch( reentry_m1_bflag ) {
		case 3 :	if( chakan_m6_bflag == 3 )		show_ = 1;
					if( ankan_m6_bflag == 1 &&
					( Status | (byte)ST.CHANK ) != 0 )	show_ = 1;		break;
		case 6 :	if( is_ronchk_menu_open == 0 )	show_ = 1;		break;
		case 7 :	if( ronho_m5_bflag == 4 )		show_ = 1;		break;
		case 8 :	show_ = 1;										break;
	}

	if( Optcnt == 0 )		return;		// メニューが零であれば。
	if( gTalkFlag != 0)		return;		// 対話があれば表示しない
	if( menu_mode_sub!= 0)	return;
	if( show_ == 0 )		return;

	// 通信対戦中手動でなければメニュー表示無し
	#if false //-*todo:通信
	if( IS_NETWORK_MODE )
		if( gDaiuchiFlag != D_ONLINE_OPERATOR_MANUAL )
			return;
	#endif //-*todo:通信
	gGamePointFlag = true;		//メニューが出た時に(ポン、チー)、得点表示を消す

#if	Rule_2P
	//鳴きコマンド 上部枠
	mMpsBrewLib_DrawSprite( spGame[ D_WIN_00], D_MENU_BASE_X, D_MENU_BASE_Y- (Optcnt* 16)- 5);
	//鳴きコマンド 下部枠
	mMpsBrewLib_DrawSprite( spGame[ D_WIN_01], D_MENU_BASE_X, D_MENU_BASE_Y);
#else
	MJ_DrawMenu( D_MENU_BASE_X, D_MENU_BASE_Y, 3, Optcnt, NORMAL_MENU );
#endif

	{
//		if( w21sa ) {
//			offsetY_ = D_MENU_BASE_Y_W21_S;
//		}else if( w21t )
//			offsetY_ = D_MENU_BASE_Y_W21_T;
//		else
			offsetY_ = D_MENU_BASE_Y;

		for( i=0 ; i< Optcnt ; i++ ) {
			int	j;

			MenuNumber = Optstk[i];
#if	Rule_2P
			if( reentry_m1_bflag == 8 && MenuNumber == (byte)OP.TAPAI )
				j= D_WIN_18;	//捨牌
			else {
				int[] menu_dat = new int[]{
					D_WIN_10,	D_WIN_13,	D_WIN_12,	D_WIN_14,		//"ツモ", "ロン", "リーチ", "チー",
					D_WIN_15,	D_WIN_16,	D_WIN_11,	D_WIN_16,		//"ポン", "カン", "たおす", "ﾁｬﾝｶﾝ",
					D_WIN_17,											//"パス",
				};
				j= menu_dat[gMenuStr_res[MenuNumber]];
			}
			mMpsBrewLib_DrawSprite( spGame[ j],
				D_MENU_BASE_X, D_MENU_BASE_Y- (Optcnt* 16)+ (i * 16));
#else
			//> 要望 No.93 プレイヤーのツモ選択メニューの「パス」を「捨牌」に
			if( reentry_m1_bflag == 8 && MenuNumber == OP_TAPAI )
				charBuff= "捨牌";		//STREXPAND((const byte*)"捨牌",5,charBuff,sizeof(charBuff));
			else {
				strNo = (ushort)gMenuStr_res[MenuNumber];
				ResAddTextBuff( strNo, charBuff, 20);			// 文字列のロード
			}
			//< 要望 No.93 プレイヤーのツモ選択メニューの「パス」を「捨牌」に
			MahJongRally_Msg_Line( charBuff, D_MENU_BASE_X+21, offsetY_ + 9 + (i * 16), SYS_FONT_WHITE );
#endif
		}
	}

	//カーソル
#if	Rule_2P
	mMpsBrewLib_DrawSprite( spGame[ D_WCURSOR], D_MENU_BASE_X+ 4, (D_MENU_BASE_Y- (Optcnt* 16))+ (menu_csr * 16)+ 3);
#else
	MJ_IconDraw( D_TRIANGLE_RIGHT, D_MENU_BASE_X+5, D_MENU_BASE_Y + 9 + (menu_csr * 16), 0 );
#endif
	//> バグ No.67 要望 No.50 メニューの変更

#if	Rule_2P
	_menuFlg= true;
#endif
#endif //-*todo:描画保留

}

/****************************************************************/
/*		王牌表示												*/
/****************************************************************/
#if	Rule_2P
static bool _wanFlg= false;
#endif
public const int		D_WanBASE_X			= 35;		//(93)
public const int		D_WanBASE_Y			= 113;		//(120)
public void MJ_GameWanpaiDisp( /*MahJongRally * pMe*/ )
{
	DebLog("//-*MJ:GameDraw.cs:MJ_GameWanpaiDisp()");
#if false //-*todo:描画保留
	short i=0;
	short	dx= D_WanBASE_X+ (13*2);
	short	dy=0;			// = wanpai_ypos + 5;
	byte	no=0;

#if	Rule_2P
	//カンの時につもる牌
	for( i= 0; i< 7; i++)	//下段
		hMpsBrewLib_DrawSprite( spSutePaiJicya, PAI.URA, D_WanBASE_X+ (13* i), D_WanBASE_Y+ (7* 1));
	for( i= 0; i< 2; i++)	//下段
		hMpsBrewLib_DrawSprite( spSutePaiJicya, PAI.URA, D_WanBASE_X+ (13* i), D_WanBASE_Y+ (7* 0));

//m	for( i= 0; i< 10; i+= 2)
	for( i= 8; i>= 0; i-=2) {
		dy = (short)(D_WanBASE_Y + (i%2)*10);
		switch(WanPaiBuf[i]) {
			case	(byte)WANTYPE.REV:	//裏向き
				no = (byte)PAI.URA;		//no = Bipai[PAI_MAX + i];	//122x
				break;
			case	(byte)WANTYPE.FRONT:
				if(( i&1 ) != 0 )
					no = (byte)PAI.URA;	//no = 0;			// ロンした時。
				else
					no = Bipai[(int)MJDefine.PAI_MAX + i];		//122x

				if( Rultbl[(int)RL.KANUR] != 0 || ((Rultbl[(int)RL.URA] != 0 ) && i/2 == 4)) {
					if((i&1) != 0 )
						dy += 20;	//dy -= buf1;	// ロンした時に。
					else
						dy += 0;					// 1枚目のドラ位置？(上段右から3個目)
				}
				break;
			case	(byte)WANTYPE.NON:		// カンした時。
				no = (byte)PAI.URA;
				break;
		}

		if(no != 0xFF) {
			if(i!= 8)
				hMpsBrewLib_DrawSprite( spSutePaiJicya, no, dx, dy );
			else {
				//配牌時 中央山 牌の回転アニメ
				switch(haipai_m2_bflag) {
					case 3:
						mMpsBrewLib_DrawSprite( spGame[D_PAI_A00], 61, 113- 4 );
						break;
					case 4:
						_wanFlg= true;
						mMpsBrewLib_DrawSprite( spGame[D_PAI_A01], 61, 113- 4 );
						break;
					default:
						if(_wanFlg) {
							_wanFlg= false;
							#if false //-*todo:サウンド
							myCanvas.playSE(SN_SE04);	//ドラ牌をめくる
							#endif //-*todo:サウンド
						}
						hMpsBrewLib_DrawSprite( spSutePaiJicya, no, dx, dy );
						break;
				}
			}
		}

		dx += 13;
	}

	/*上下をつなぐ影*/
	for( i= 0; i< 7; i++)
		mMpsBrewLib_DrawSprite( spGame[D_PSHADOW], D_WanBASE_X+ (13* i), D_WanBASE_Y+ (7* 0)+ 21);
#else
	for(i = 0;i <10;i+=2) {
		dy = (SINT2)(D_WanBASE_Y + (i%2)*10);
		switch(WanPaiBuf[i]) {
			case	0:	//裏向き
				no = PAI_URA;
				//no = Bipai[PAI_MAX + i];		//122x
				break;
			case	1:
				if(( i&1 ) != 0 )
					no = PAI_URA;	//no = 0;	// ロンした時。
				else
					no = Bipai[PAI_MAX + i];	//122x

				if( Rultbl[RL_KANUR] != 0 || ((Rultbl[RL_URA] != 0 ) && i/2 == 4)) {
					if((i&1) != 0 ) {
						//dy -= buf1;
						dy += 20;		// ロンした時に。
					} else
						dy += 0;		// 1枚目のドラ位置？(上段右から3個目)
				}
				break;
			case	2:		// カンした時。
				no = PAI_URA;
				break;
		}
		if(no != 0xFF)
			MpsBrewLib_DrawSprite( spPaiJicyaSmall[no], dx, dy );	//
		dx += 11;
	}
#endif
#endif //-*todo:描画保留

}

/************************************************************/
/*															*/
/*				ゲーム中に使用する画像のロード				*/
/*					   TakuSozai.PNG						*/
/*					   	 taku.png							*/
/*					   	  End.PNG							*/
/*					   	 point.PNG							*/
/*					   YakuSozai.PNG						*/
/************************************************************/
public void	MJ_GameDrawInit( /*MahJongRally * pMe*/ )
{
	DebLog("//-*MJ:GameDraw.cs:MJ_GameDrawInit");
#if false//-*todo:描画保留

	short i	= 0;
//********ウキウキ:表情操作*********
//*System.out.println("num_battle---------:"+myCanvas.num_battle);
//*System.out.println("level_cpu----------:"+myCanvas.level_cpu);
//*System.out.println("mah_limit_num------:"+myCanvas.mah_limit_num);
	//*キャラクターの表情
	pIGame[D_GAME_CHAR_FACE]= __MJ_LoadResImage( pIGame[D_GAME_CHAR_FACE], BATTLE_CHAR_00+(myCanvas.num_battle-1));
//*System.out.println("pIGame[D_GAME_CHAR_FACE("+D_GAME_CHAR_FACE+")]------:"+pIGame[D_GAME_CHAR_FACE]);
	_MpsBrewLib_SetSpriteInfo( spGame[D_CHAR_FACE_00], pIGame[D_GAME_CHAR_FACE], 0, 0, 180, 234);
	for( i= D_CHAR_FACE_00; i< D_CHAR_FACE_15+ 1; i++) {
		mMpsBrewLib_SetSpriteInfo( spGame[i] , spGame[D_CHAR_FACE_00].pIImage,
			g_Rect_CharFace[i- D_CHAR_FACE_00][0], g_Rect_CharFace[i- D_CHAR_FACE_00][1],
			g_Rect_CharFace[i- D_CHAR_FACE_00][2], g_Rect_CharFace[i- D_CHAR_FACE_00][3] );
	}

	//*リーチ時の立ち絵
//*	pIGame[D_GAME_CHAR_REACH]= __MJ_LoadResImage( pIGame[D_GAME_CHAR_REACH], BATTLE_CHAR_REACH_00+(myCanvas.num_battle-1));
	pIGame[D_GAME_CHAR_REACH]= __MJ_LoadResImage( pIGame[D_GAME_CHAR_REACH], BATTLE_CHAR_00+(myCanvas.num_battle-1));
	_MpsBrewLib_SetSpriteInfo( spGame[D_CHAR_REACH], pIGame[D_GAME_CHAR_REACH], 0, 0, 146, 164 );
	mMpsBrewLib_SetSpriteInfo( spGame[D_CHAR_REACH] , spGame[D_CHAR_REACH].pIImage,
			g_Rect_CharReach[ g_Rect_USE_CharReachNUM[(myCanvas.num_battle-1)] ][0], 
			g_Rect_CharReach[ g_Rect_USE_CharReachNUM[(myCanvas.num_battle-1)] ][1],
			g_Rect_CharReach[ g_Rect_USE_CharReachNUM[(myCanvas.num_battle-1)] ][2], 
			g_Rect_CharReach[ g_Rect_USE_CharReachNUM[(myCanvas.num_battle-1)] ][3] );

	//*キャラ表示用枠
//*	pIGame[D_GAME_CHAR_FRAME]= __MJ_LoadResImage( pIGame[D_GAME_CHAR_FRAME], BATTLE_CHAR_FRAME);
	pIGame[D_GAME_CHAR_FRAME]= __MJ_LoadResImage( pIGame[D_GAME_CHAR_FRAME], _TAKUSOZAI);  
	_MpsBrewLib_SetSpriteInfo( spGame[D_CAHR_FRAME], pIGame[D_GAME_CHAR_FRAME], 0, 0, 236, 210 );
//*	mMpsBrewLib_SetSpriteInfo( spGame[D_CAHR_FRAME] , spGame[D_CAHR_FRAME].pIImage,
//*			g_Rect_CharFrame[0], g_Rect_CharFrame[1],
//*			g_Rect_CharFrame[2], g_Rect_CharFrame[3] );
	mMpsBrewLib_SetSpriteInfo( spGame[D_CAHR_FRAME] , spGame[D_CAHR_FRAME].pIImage,
			g_Rect_TakuSozai[3][0], g_Rect_TakuSozai[3][1],
			g_Rect_TakuSozai[3][2], g_Rect_TakuSozai[3][3] );
//**********************************
#if	Rule_2P
#else
	/********************************************/
	/*				卓素材画像のロード			*/
	/*				  TakuSozai01~39.PNG		*/
	/********************************************/
	for( i=0 ; i<34 ; i++ ) {		//38
		pIGame[D_GAME_SOZAI]= __MJ_LoadResImage( pIGame[D_GAME_SOZAI], TAKUSOZAI+ i );	//MJ_LoadResImage( pIGame[D_GAME_SOZAI], TAKUSOZAI );
		_MpsBrewLib_SetSpriteInfo( spGame[i] , pIGame[D_GAME_SOZAI],
									0, 0,
									g_RectTakuSozai[i][2],
									g_RectTakuSozai[i][3] );
	}
#endif

//	pIGame[D_GAME_SOZAI]= _MJ_LoadResImage( pIGame[D_GAME_SOZAI], TAKUSOZAI );	//MJ_LoadResImage( pIGame[D_GAME_SOZAI], TAKUSOZAI );
//	for( i=0 ; i<38 ; i++ )
//	{
//
//		MpsBrewLib_SetSpriteInfo( spGame[i] , pIGame[D_GAME_SOZAI],
//									g_RectTakuSozai[i][0],
//									g_RectTakuSozai[i][1],
//									g_RectTakuSozai[i][2],
//									g_RectTakuSozai[i][3] );
//	}

	/********************************************/
	/*				卓素材画像のロード			*/
	/*				  ukima_majan11.png//*ウキウキ	(moema_majan06.PNG)			*/
	/********************************************/
	//m_pIGame
	pIGame[D_GAME_SOZAI]= __MJ_LoadResImage( pIGame[D_GAME_SOZAI], _TAKUSOZAI );
	_MpsBrewLib_SetSpriteInfo( spGame[D_SCORE_00], pIGame[D_GAME_SOZAI], 0, 0, 240, 178);
	for( i= D_SCORE_00; i< D_WCURSOR+ 1; i++) {
		mMpsBrewLib_SetSpriteInfo( spGame[i] , spGame[D_SCORE_00].pIImage,
			g_Rect_TakuSozai[i- D_SCORE_00][0], g_Rect_TakuSozai[i- D_SCORE_00][1],
			g_Rect_TakuSozai[i- D_SCORE_00][2], g_Rect_TakuSozai[i- D_SCORE_00][3] );
	}

	/********************************************/
	/*				 上がり表示のロード			*/
	/*				  ukima_majan08.png//*ウキウキ	(moema_majan08.png)			*/
	/*				  ukima_majan09.png//*ウキウキ	(moema_majan09.png)			*/
	/*				  ukima_majan11.png//*ウキウキ	(moema_majan11.png)			*/
	/*				  ukima_majan17.png//*ウキウキ	(moema_majan17.png)			*/
	/********************************************/
	//moema_majan08.png → ukima_majan08.png//*ウキウキ
	pIGame[D_GAME_END00]= __MJ_LoadResImage( pIGame[D_GAME_END00], _GAME_END00 );
	_MpsBrewLib_SetSpriteInfo( spGame[D_END00_00], pIGame[D_GAME_END00], 0, 0, 232, 177 );

	//moema_majan09.png → ukima_majan09.png//*ウキウキ
	pIGame[D_GAME_END01]= __MJ_LoadResImage( pIGame[D_GAME_END01], _GAME_END01 );
	_MpsBrewLib_SetSpriteInfo( spGame[D_END01_00], pIGame[D_GAME_END01], 0, 0, 230, 91 );
	for( i= D_END01_00; i< D_END01_36+ 1; i++) {
		mMpsBrewLib_SetSpriteInfo( spGame[i] , spGame[D_END01_00].pIImage,
			g_Rect_EndSozai[i- D_END01_00][0], g_Rect_EndSozai[i- D_END01_00][1],
			g_Rect_EndSozai[i- D_END01_00][2], g_Rect_EndSozai[i- D_END01_00][3] );
	}
	//moema_majan11.png → ukima_majan11.png//*ウキウキ
	pIGame[D_GAME_END02]= __MJ_LoadResImage( pIGame[D_GAME_END02], _GAME_END02 );
	_MpsBrewLib_SetSpriteInfo( spGame[D_END02_00], pIGame[D_GAME_END02], 0, 0, 61, 156 );
	for( i= D_END02_00; i< D_END02_11+ 1; i++) {
		mMpsBrewLib_SetSpriteInfo( spGame[i] , spGame[D_END02_00].pIImage,
			0, (i- D_END02_00)* 13, 61, 13);
	}
	//moema_majan17.png → ukima_majan17.png//*ウキウキ
	pIGame[D_GAME_END03]= __MJ_LoadResImage( pIGame[D_GAME_END03], _GAME_END03 );
	_MpsBrewLib_SetSpriteInfo( spGame[D_END03_00], pIGame[D_GAME_END03], 0, 0, 233, 60 );
	for( i= D_END03_00; i< D_END03_34+ 1; i++) {
		mMpsBrewLib_SetSpriteInfo( spGame[i] , spGame[D_END03_00].pIImage,
			g_Rect_ResultSozai[i- D_END03_00][0], g_Rect_ResultSozai[i- D_END03_00][1],
			g_Rect_ResultSozai[i- D_END03_00][2], g_Rect_ResultSozai[i- D_END03_00][3] );
	}

	/********************************************/
	/*				 卓画像のロード				*/
	/*				 ukima_majan05//*ウキウキ (moema_majan05) (taku.png)	*/
	/********************************************/
	// zeniya
	//pIGame[D_GAME_TAKU]= __MJ_LoadResImage( pIGame[D_GAME_TAKU], MAIN_BG_2 -22 );	//MJ_LoadResImage( pIGame[D_GAME_TAKU], TAKU );
//
//	pIGame[D_GAME_TAKU]= __MJ_LoadResImage( pIGame[D_GAME_TAKU], MAIN_BG_2 );	//MJ_LoadResImage( pIGame[D_GAME_TAKU], TAKU );

//m	_MpsBrewLib_SetSpriteInfo( spGame[D_GAME_JYANTAKU] , pIGame[D_GAME_TAKU], 0, 0, 240, 266 );

	//_MpsBrewLib_SetSpriteInfo( spGame[D_GAME_JYANTAKU] , pIGame[D_GAME_TAKU], 0, 0, 240, 257 );
	//_MpsBrewLib_SetSpriteInfo( spGame[D_GAME_JYANTAKU] , pIGame[D_GAME_TAKU], 151, 121, 60, 48);
//	MpsBrewLib_SetSpriteInfo( spGame[D_GAME_JYANTAKU] , pIGame[D_GAME_TAKU], 0, 0, 240, 266);

	pIGame[D_GAME_TAKU]= __MJ_LoadResImage( pIGame[D_GAME_TAKU], MAIN_BG_2 );	//MJ_LoadResImage( pIGame[D_GAME_TAKU], TAKU );
//m	_MpsBrewLib_SetSpriteInfo( spGame[D_GAME_JYANTAKU] , pIGame[D_GAME_TAKU], 0, 0, 240, 266 );
	_MpsBrewLib_SetSpriteInfo( spGame[D_GAME_JYANTAKU] , pIGame[D_GAME_TAKU], 0, 0, 240, 260 );

	/********************************************/
	/*			  清算時の画像ロード			*/
	/*				  point.PNG					*/
	/********************************************/
//m	pIGame[D_GAME_END_POINT]= _MJ_LoadResImage( pIGame[D_GAME_END_POINT], POINT_BG );	//MJ_LoadResImage( pIGame[D_GAME_END_POINT], POINT_BG );
//m	for( i=0 ; i<6 ; i++ )
//m		MpsBrewLib_SetSpriteInfo( spGame[D_GAME_END_POINT_BG+i] , pIGame[D_GAME_END_POINT], 0, 0+(i*14), 46, 14);

	/********************************************/
	/*				役画像のロード				*/
	/*				YakuSozai.PNG				*/
	/********************************************/
//	pIYakuBG= _MJ_LoadResImage( pIYakuBG, YAKU_SOZAI );	//MJ_LoadResImage( pIYakuBG, YAKU_SOZAI );
//	for( i=0 ; i<55 ; i++ )
//		MpsBrewLib_SetSpriteInfo( spYakuBG[i], pIYakuBG, 0, 0+(i*16), 80, 16 );
	{
		int		j= 0;
#if	Rule_2P
		pIYakuBG= _MJ_LoadResImage( pIYakuBG, YAKU_SOZAI+ 0 );
		for( i=0 ; i< 14 ; i++ )	MpsBrewLib_SetSpriteInfo( spYakuBG[j++], pIYakuBG, 0, 0+(i*12), 72, 12 );
		pIYakuBG= _MJ_LoadResImage( pIYakuBG, YAKU_SOZAI+ 1 );
		for( i=0 ; i< 14 ; i++ )	MpsBrewLib_SetSpriteInfo( spYakuBG[j++], pIYakuBG, 0, 0+(i*12), 72, 12 );
		pIYakuBG= _MJ_LoadResImage( pIYakuBG, YAKU_SOZAI+ 2 );
		for( i=0 ; i< 14 ; i++ )	MpsBrewLib_SetSpriteInfo( spYakuBG[j++], pIYakuBG, 0, 0+(i*12), 72, 12 );
		pIYakuBG= _MJ_LoadResImage( pIYakuBG, YAKU_SOZAI+ 3 );
		for( i=0 ; i< 13 ; i++ )	MpsBrewLib_SetSpriteInfo( spYakuBG[j++], pIYakuBG, 0, 0+(i*12), 72, 12 );
		pIYakuBG= _MJ_LoadResImage( pIYakuBG, YAKU_SOZAI+ 4 );
		for( i=0 ; i< 3 ; i++ )		MpsBrewLib_SetSpriteInfo( spYakuBG[j++], pIYakuBG, 0, 0+(i*12), 72, 12 );
#else
		pIYakuBG= _MJ_LoadResImage( pIYakuBG, YAKU_SOZAI+ 0 );
		for( i=0 ; i< 14 ; i++ )	MpsBrewLib_SetSpriteInfo( spYakuBG[j++], pIYakuBG, 0, 0+(i*16), 80, 16 );
		pIYakuBG= _MJ_LoadResImage( pIYakuBG, YAKU_SOZAI+ 1 );
		for( i=0 ; i< 14 ; i++ )	MpsBrewLib_SetSpriteInfo( spYakuBG[j++], pIYakuBG, 0, 0+(i*16), 80, 16 );
		pIYakuBG= _MJ_LoadResImage( pIYakuBG, YAKU_SOZAI+ 2 );
		for( i=0 ; i< 14 ; i++ )	MpsBrewLib_SetSpriteInfo( spYakuBG[j++], pIYakuBG, 0, 0+(i*16), 80, 16 );
		pIYakuBG= _MJ_LoadResImage( pIYakuBG, YAKU_SOZAI+ 3 );
		for( i=0 ; i< 13 ; i++ )	MpsBrewLib_SetSpriteInfo( spYakuBG[j++], pIYakuBG, 0, 0+(i*16), 80, 16 );
#endif
	}

	/********************************************/
	/*				鳴き演出のロード			*/
	/*				ukima_majan13.PNG//*ウキウキ(moema_majan13.PNG)			*/
	/*				ukima_majan14.PNG//*ウキウキ(moema_majan14.PNG)			*/
	/*				ukima_majan15.PNG//*ウキウキ(moema_majan15.PNG)			*/
	/********************************************/
	//moema_majan13.PNG → ukima_majan13.PNG//*ウキウキ
	pIGame[D_GAME_END04]= __MJ_LoadResImage( pIGame[D_GAME_END04], _GAME_END04 );
	_MpsBrewLib_SetSpriteInfo( spGame[D_NAKI00_00], pIGame[D_GAME_END04], 0, 0, 160, 240 );
	for( i= D_NAKI00_00; i< D_NAKI00_02+ 1; i++) {
		mMpsBrewLib_SetSpriteInfo( spGame[i] , spGame[D_NAKI00_00].pIImage,
			g_Rect_NakiSozai00[i- D_NAKI00_00][0], g_Rect_NakiSozai00[i- D_NAKI00_00][1],
			g_Rect_NakiSozai00[i- D_NAKI00_00][2], g_Rect_NakiSozai00[i- D_NAKI00_00][3] );
	}
	//moema_majan14.PNG → ukima_majan14.PNG//*ウキウキ
	pIGame[D_GAME_END05]= __MJ_LoadResImage( pIGame[D_GAME_END05], _GAME_END05 );
	_MpsBrewLib_SetSpriteInfo( spGame[D_NAKI01_00], pIGame[D_GAME_END05], 0, 0, 160, 240 );
	for( i= D_NAKI01_00; i< D_NAKI01_02+ 1; i++) {
		mMpsBrewLib_SetSpriteInfo( spGame[i] , spGame[D_NAKI01_00].pIImage,
			g_Rect_NakiSozai00[i- D_NAKI01_00][0], g_Rect_NakiSozai00[i- D_NAKI01_00][1],
			g_Rect_NakiSozai00[i- D_NAKI01_00][2], g_Rect_NakiSozai00[i- D_NAKI01_00][3] );
	}
	//moema_majan15.PNG → ukima_majan15.PNG//*ウキウキ
	pIGame[D_GAME_END06]= __MJ_LoadResImage( pIGame[D_GAME_END06], _GAME_END06 );
	_MpsBrewLib_SetSpriteInfo( spGame[D_NAKI02_00], pIGame[D_GAME_END06], 0, 0, 160, 140 );
	for( i= D_NAKI02_00; i< D_NAKI02_03+ 1; i++) {
		mMpsBrewLib_SetSpriteInfo( spGame[i] , spGame[D_NAKI02_00].pIImage,
			g_Rect_NakiSozai01[i- D_NAKI02_00][0], g_Rect_NakiSozai01[i- D_NAKI02_00][1],
			g_Rect_NakiSozai01[i- D_NAKI02_00][2], g_Rect_NakiSozai01[i- D_NAKI02_00][3] );
	}

	/********************************************/
	/*				カットインのロード			*/
	/*				ukima_majan12.PNG//*ウキウキ	(moema_majan12.PNG)			*/
	/********************************************/
	pIGame[D_GAME_END07]= __MJ_LoadResImage( pIGame[D_GAME_END07], _GAME_END07 );
	_MpsBrewLib_SetSpriteInfo( spGame[D_NAKI03_00], pIGame[D_GAME_END07], 0, 0, 240, 240 );
	for( i= D_NAKI03_00; i< D_NAKI03_03+ 1; i++) {
		mMpsBrewLib_SetSpriteInfo( spGame[i] , spGame[D_NAKI03_00].pIImage,
			g_Rect_NakiSozai02[i- D_NAKI03_00][0], g_Rect_NakiSozai02[i- D_NAKI03_00][1],
			g_Rect_NakiSozai02[i- D_NAKI03_00][2], g_Rect_NakiSozai02[i- D_NAKI03_00][3] );
	}

	/********************************************/
	/*				ルール表示のロード			*/
	/*				  ukima_majan07.PNG//*ウキウキ	(moema_majan07.PNG)			*/
	/********************************************/
	pIGame[D_GAME_END08]= __MJ_LoadResImage( pIGame[D_GAME_END08], _GAME_END08 );
	_MpsBrewLib_SetSpriteInfo( spGame[D_RULE00], pIGame[D_GAME_END08], 0, 0, 220, 234 );
	for( i= D_RULE00; i< D_RULE06+ 1; i++)
		mMpsBrewLib_SetSpriteInfo( spGame[i] , spGame[D_RULE00].pIImage,
			((i- D_RULE00)/ 18)* 110, ((i- D_RULE00)% 18)* 13, 110, 13);

	/********************************************/
	/*				勝利 敗北 のロード			*/
	/*				  ukima_majan16.PNG	//*ウキウキ(moema_majan16.PNG)			*/
	/********************************************/
	pIGame[D_GAME_END09]= __MJ_LoadResImage( pIGame[D_GAME_END09], _GAME_END09 );
	_MpsBrewLib_SetSpriteInfo( spGame[D_WIN00], pIGame[D_GAME_END09], 0, 0, 220, 160 );
	for( i= D_WIN00; i< D_LOSE00+ 1; i++)
		mMpsBrewLib_SetSpriteInfo( spGame[i] , spGame[D_WIN00].pIImage, 0, 80* (i- D_WIN00), 220, 80);

	gc();
#if DEBUG
Debug.mem("mem");
#endif
//	return;
#endif //-*todo:描画保留

}

/************************************************************/
/*			　ゲーム中に使用する画像のアンロード			*/
/*						TakuSozai.PNG						*/
/*						 taku.png							*/
/*						  End.PNG							*/
/************************************************************/
public void	MJ_GameDrawFree( /*MahJongRally * pMe*/ )
{
	DebLog("//-*MJ:GameDraw.cs:MJ_GameDrawFree()");

#if false //-*todo:描画保留
	short i=0;

	// ゲーム中に使用する素材。
	for( i=0 ; i<D_GAME_MAX_SOZAI_COUNT ; i++ )
		MJ_IIMAGE_Release( pIGame[i] );

	// ゲーム中に使用する素材。
	for( i= 0; i< D_GAME_MAX; i++)
		MJ_Sprite_Release(spGame[i]);

//m	for( i=0 ; i<38 ; i++ )
//m		MJ_Sprite_Release(spGame[i]);

//m	MJ_Sprite_Release(spGame[D_GAME_JYANTAKU]);

//m	for( i=0 ; i<6 ; i++ )
//m		MJ_Sprite_Release(spGame[D_GAME_END_POINT_BG+i]);

//	m_spFloorBG.pIImage=
//	m_spSurvival.pIImage= null;
//	for( i= 0 ; i< 20 ; i++ )	m_sprite[i].pIImage= null;

	MJ_Sprite_Release(m_spFloorBG);
	MJ_Sprite_Release(m_spSurvival);
	for( i= 0 ; i< 20 ; i++ )
		MJ_Sprite_Release(m_sprite[i]);

	// 役の画像の解放。
	MJ_IIMAGE_Release( pIYakuBG );

	// 役の画像の解放。
	int	j = 0;
	for( i=0 ; i< 14 ; i++ )	MJ_Sprite_Release( spYakuBG[j++]);
	for( i=0 ; i< 14 ; i++ )	MJ_Sprite_Release( spYakuBG[j++]);
	for( i=0 ; i< 14 ; i++ )	MJ_Sprite_Release( spYakuBG[j++]);
	for( i=0 ; i< 13 ; i++ )	MJ_Sprite_Release( spYakuBG[j++]);
//	return;

	gc();
#if DEBUG
Debug.mem("mem");
#endif
#endif //-*todo:描画保留

}

/********************************************************/
/*			何局目・サイコロ・残り牌数・点棒 表示		*/
/********************************************************/
public void MJ_GameSozaiDisp( /*MahJongRally * pMe*/ )
{
	DebLog("//-*MJ:GameDraw.cs:MJ_GameSozaiDisp()");
#if false //-*todo:描画保留
	
	// 2006/03/31 謎処理につき修正		//0422
//	short	i=0;
	int			House;
	byte		Kyoku		= gpsTableData.byKyoku;						// 局数。
	byte		Oya			= gpsTableData.byOya;						// 現在の親。
#if	Rule_2P
	byte		Ribo		= (byte)(	gpsTableData.sMemData[0].byRibo_stack+
										gpsTableData.sMemData[1].byRibo_stack);		//場にあるリー棒
#else
	byte		Ribo		= (byte)(gpsTableData.byRibo % 100);		// 場にあるリー棒	2006/02/26 No.725
#endif
	byte		DrawRenchan = (byte)(gpsTableData.byDrawRenchan % 100);	// 何本場か			2006/02/26 No.725
//m	BYTE		Chicha;			// 起家。
//m	BYTE		TableNo   =0;
//m	BYTE		D1		  =0;
//m	BYTE		D2		  =0;
//m	BYTE		NokoriPai =0;
//m	short	BA[]  = { D_TON,D_NAN,D_SYA,D_PEI };								//[16]
//m	short	MYHOUSE[] = {D_TON,D_PEI,D_SYA,D_NAN,D_TON,D_PEI,D_SYA,D_NAN};		//[8]

//m	Chicha		= gpsTableData.byChicha;					// 起家。
//m	TableNo		= gpsTableData.byTableNo;

	// 1000点棒の数の描画。
//m	if ( Ribo >= 10 ) MpsBrewLib_DrawSprite( spGame[D_SMALL_NUMBER_0 + (Ribo / 10)], 144, 144 );	// 数値：十の位	2006/02/26 No.725
//m	MpsBrewLib_DrawSprite( spGame[D_SMALL_NUMBER_0 + (Ribo % 10)], 144 + 5, 144 );	// 数値：一の位	2006/02/26 No.725
	if( Ribo>= 10 )
		mMpsBrewLib_DrawSprite( spGame[D_NUM_0+ (Ribo/ 10)], 185, 5 );
	mMpsBrewLib_DrawSprite( spGame[D_NUM_0+ (Ribo% 10)], 193, 5 );

	// 100点棒の数の描画。
//m	if ( DrawRenchan >= 10 ) MpsBrewLib_DrawSprite( spGame[D_SMALL_NUMBER_0 + (DrawRenchan / 10)], 144, 153 );	// 数値：十の位	2006/02/26 No.725
//m	MpsBrewLib_DrawSprite( spGame[D_SMALL_NUMBER_0 + (DrawRenchan % 10)], 144 + 5, 153 );	// 数値：一の位	2006/02/26 No.725
	if( DrawRenchan>= 10 )
		mMpsBrewLib_DrawSprite( spGame[D_NUM_0+ (DrawRenchan/ 10)], 220, 5 );
	mMpsBrewLib_DrawSprite( spGame[D_NUM_0+ (DrawRenchan% 10)], 228, 5 );

	// 2006/03/31  謎処理につき修正		//0422
	// --- 何局目かの描画。
	//i = (short)(Kyoku%4);
	//if( select_rule == RULETYPE_SURVIVAL ) {
	//	if( Kyoku > 2 ){ Kyoku = 2;	}
	//	if( i > 4 )	i=0;
	//}

// > 2006/02/04 No29
//m	if(Rultbl[ RL_KAZE ] != 0) {
//m		short ba_ = (short)(BA[( Kyoku / 4 ) & 0x03]);	//Kyoku & 0x0f;		//0422
//m		MpsBrewLib_DrawSprite( spGame[ba_],100-12,105);			// 東・南・西・北
//m	} else
//m		MpsBrewLib_DrawSprite( spGame[D_TON],100-12,105);		// 東（東東戦の時）
	mMpsBrewLib_DrawSprite( spGame[D_BA_TON+ ((Kyoku/ 4) & 0x01)], 124, 3 );

// < 2006/02/04 No29

	// 2006/03/31 謎処理につき修正
//	MpsBrewLib_DrawSprite( spGame[D_SUPER_BIG_NUMBER_1+i],115-15,105);	// 局数。
//	MpsBrewLib_DrawSprite( spGame[D_SUPER_BIG_NUMBER_1 + (Kyoku & 0x03)],115-15,105);	// 局数。
	mMpsBrewLib_DrawSprite( spGame[D_KYOKU_1+ (Kyoku & 0x01)], 137, 3 );

	// --- 残り牌数の描画。
#if	__MJ_CHECK
	{
	int NokoriPai = MJDefine.PAI_MAX - (Bpcnt+Kancnt);	// 残り牌取得
	if( NokoriPai>= 10 )
		mMpsBrewLib_DrawSprite( spGame[D_NUM_0+ (NokoriPai/ 10)], 100- 26+ 4, 5 );
	mMpsBrewLib_DrawSprite( spGame[D_NUM_0+ (NokoriPai% 10)], 100- 26+ 4+ 8, 5 );
	}
#endif
//m	if( D_BIG_NUMBER_9 >= NokoriPai/10 && D_BIG_NUMBER_0 <= NokoriPai/10 ) {	// 限界値チェック。
//m		MpsBrewLib_DrawSprite( spGame[NokoriPai/10],102,146);		// 下二桁。
//m		MpsBrewLib_DrawSprite( spGame[NokoriPai%10],109,146);		// 下一桁。
//m	}

	// --- サイコロの値を取得。
//m	D1 = Dicnum[0];
//m	D2 = Dicnum[1];

	/* 自分の家を取得 */
	House = (Oya+4-game_player) % 4;

	// --- 自分が何家かを描画。
	// -- これによりサイコロの描画を削除。
	// -- 起家が常に東になるように作らねばならない。
//m	MpsBrewLib_DrawSprite( spGame[MYHOUSE[ House ]],128,105);			// 東・南・西・北
//m	MpsBrewLib_DrawSprite( spGame[D_MARU_HOUSE],141,105);				// 家。
	mMpsBrewLib_DrawSprite( spGame[D_IE_TON+ (House & 0x01)], 84, 244 );

	//点数
	if( gpsTableData.sMemData[0].nOldPoint< 0)
		mMpsBrewLib_DrawSprite( spGame[D_NUM__], 51- 7* 6, 246 );
	AgariNumDisp( abs(gpsTableData.sMemData[0].nOldPoint* 100), 51- 7* 7, 246, 7, 7, false, D_NUM_0- D_END01_18 );

	if( gpsTableData.sMemData[1].nOldPoint< 0)
		mMpsBrewLib_DrawSprite( spGame[D_NUM__], 51- 7* 6, 5 );
	AgariNumDisp( abs(gpsTableData.sMemData[1].nOldPoint* 100), 51- 7* 7,   5, 7, 7, false, D_NUM_0- D_END01_18 );
//	return;
#endif //-*todo:描画保留
}

/********************************************************/
/*					フーロ牌数取得。					*/
/********************************************************/
public int	getKakarenaihai( /*MahJongRally * pMe,*/ int House )
{
#if true //-*todo:描画保留
	DebLog("//-*MJ:GameDraw.cs:getKakarenaihai( int "+House+")");
	return 0;
#else //-*todo:描画保留
	int			i		= 0;
	short	suteCnt = 0;
	short	suteFlag= 0;
	short	ret		= 0;

	suteCnt = (short)gsPlayerWork[House].byShcnt;					// 捨て牌数取得。

	for( i=suteCnt-1 ; i>=0 ; i-- ) {
		suteFlag	= (short)Rec_sute_pos[House,i];					// 捨て牌がリーチかどうかをGET。

		switch( suteFlag ) {
			case (short)SUTEHAI.OFF: {
				ret +=1;
				break;
			}
		}
	}

	return	ret;
#endif //-*todo:描画保留
}

/********************************************************/
/*						リーチライン。					*/
/********************************************************/
public int	getReachLine_Toicya( /*MahJongRally * pMe,*/ int House, int Define )
{
#if true //-*todo:描画保留
	DebLog("//-*MJ:GameDraw.cs:getReachLine_Toicya( int "+House+", int "+Define+")");
	return 0;
#else //-*todo:描画保留
	int		i			= 0;
	short	suteCnt		= 0;
	short	suteFlag	= 0;
	int	ret			=10;
	int	DrawCount	= 0;
	int	offsetX		= 0;
	int	offsetY		= 0;

	/*===========================================================================
								捨て牌数の取得。
	===========================================================================*/
	suteCnt = (short)gsPlayerWork[House].byShcnt;			// 捨て牌数取得。
	DrawCount = suteCnt-1-getKakarenaihai( House );			// 鳴かれた牌数を取得。

	for( i=suteCnt-1 ; i>=0 ; i-- ) {
		suteFlag	= (short)Rec_sute_pos[House][i];		// 捨て牌がリーチかどうかをGET。
		offsetX		= DrawCount%12;		// %8				// 横の位置
		offsetY		= DrawCount/12;		// /8				// 段の位置

		switch( suteFlag ) {
			case (short)SUTEHAI.ON:
				DrawCount-=1;
				break;
			case (short)SUTEHAI.OFF:
				break;
			case (short)SUTEHAI.REACH:
				if( Define == D_TOICYA )
					ret = offsetY;
				else
					if( Define == D_SHIMOCYA )
						ret = offsetX;		//ret;

				DrawCount-=1;
				break;
		}
	}

	return	ret;
#endif //-*todo:描画保留
}

/********************************************************/
/*						リーチライン。					*/
/********************************************************/
public int	getReachLine_Shimocya( /*MahJongRally * pMe,*/ int House, int Define )
{
#if true //-*todo:描画保留
	DebLog("//-*MJ:GameDraw.cs:getReachLine_Shimocya( int "+House+", int "+Define+")");
	return 0;
#else //-*todo:描画保留

	int			i		= 0;
	short	suteCnt		= 0;
	short	suteFlag	= 0;
	int		ret			=10;
	short	DrawCount	= 0;
	int		offsetX		= 0;
	int		offsetY		= 0;

	/*===========================================================================
								捨て牌数の取得。
	===========================================================================*/
	suteCnt = (short)gsPlayerWork[House].byShcnt;				// 捨て牌数取得。
	DrawCount = (short)(suteCnt-1-getKakarenaihai( House ));	// 鳴かれた牌数を取得。

	for( i=suteCnt-1 ; i>=0 ; i-- )
	{
		suteFlag	= (short)Rec_sute_pos[House][i];			// 捨て牌がリーチかどうかをGET。
		offsetX		= DrawCount/12;		// /8					// 横の位置
		offsetY		= DrawCount%12;		// %8					// 段の位置

		switch( suteFlag )
		{
			case (short)SUTEHAI.ON:
			{
				DrawCount-=1;
				break;
			}
			case (short)SUTEHAI.OFF:
			{
				break;
			}
			case (short)SUTEHAI.REACH:
			{
				if( Define == D_TOICYA )
				{
					ret = offsetY;
				}else if( Define == D_SHIMOCYA )
				{
					ret = offsetX;
//					ret;
				}

				DrawCount-=1;
				break;
			}
		}
	}

	return	ret;
#endif //-*todo:描画保留
}

/************************************************************/
/*	半荘終了後のゲームリザルト画面。						*/
/************************************************************/
public const int D_RANK_NUMBER_BASE_X		= 15;	// 順位
public const int D_RANK_NUMBER_BASE_Y		= 75;	//
public const int D_RANK_DRAW_FACE_BASE_X	= 30;	// 顔
public const int D_RANK_DRAW_FACE_BASE_Y	= 60;	//
public const int D_RANK_NAME_BASE_X			= 75;	// 名前
public const int D_RANK_NAME_BASE_Y			= 57;	//
public const int D_RANK_POINT_BASE_X		= 100;	// ポイント(総合)
public const int D_RANK_POINT_BASE_Y		= 100;	//
public const int D_RANK_KAKU_POINT_BASE_X	= 100; // 各ポイント
public const int D_RANK_KAKU_POINT_BASE_Y	= 100; //

// ドボン・差し馬・チップ・順位馬・焼き鳥
//public const int D_BASE_ALL_NPNT_X	110
//public const int D_BASE_ALL_NPNT_Y	75
public const int D_BASE_ALL_NPNT_X	 = 50;
public const int D_BASE_ALL_NPNT_Y	 = 75;

public void MJ_GameResultDraw(/*MahJongRally* pMe*/)
{

#if true //-*todo:描画保留
	DebLog("//-*MJ:GameDraw.cs:MJ_GameResultDraw( )");
	// ロジック
	MjEndMain();
#else //-*todo:描画保留

#if	Rule_2P
	// ロジック
	MjEndMain();

	//ゲームの描画全般
	MJ_GameDraw();

	if( m_keepData.flag_res_battle== 0)	// 0:勝利/1:敗北
		mMpsBrewLib_DrawSprite( spGame[ D_WIN00], 10, 90 );	//勝利
	else
		mMpsBrewLib_DrawSprite( spGame[D_LOSE00], 10, 90 );	//敗北
#else
	int point = 0;

	// ロジック
	MjEndMain();

	// メインBGの描画
	MJ_DrawMainBG();

	// 終了画像
	MJ_DrawMode( D_MODE_TAIKYOKU_END, 10);

	// 順位とキャラクターの描画
	for ( short	rank = 0 ; rank < 4 ; rank++ ) {
		for ( short cnt = 0 ; cnt < 4 ; cnt++ ) {
			short	ranking = (short)gpsTableData.sMemData[cnt].byRank;	// 順位

			if ( ranking == rank ) {
				SpriteInfo	sprite_ = NULL;
				String name;	//char* name = "";

				if( gpsTableData.sMemData[cnt].byMember < MAX_COMP_CHARACTER )
					sprite_ = spChar[gpsTableData.sMemData[cnt].byMember];

				name = gpsTableData.sMemData[cnt].NickName;
				if ( WhichDrawMode == D_DRAW_NETWORK_MODE ) {
					// 顔
					if ( (byte)gNetTable_NetChar[cnt].Flag != (byte)0xFF ) {	// 2006/02/06 No.705
						// 代打ちでなければ。
						sprite_ = gNetTable_NetChar[cnt].spriteChar;
						if ( sprite_.pIImage == null )	//( !sprite_.pIImage )
							if( gNetTable_NetChar[cnt].CharPicNo < MAX_COMP_CHARACTER )
								sprite_ = spChar[gNetTable_NetChar[cnt].CharPicNo];
						// 名前
						name = gNetTable_NetChar[cnt].Name;
					}
				}
				DrawCharacterFace(sprite_, D_RANK_DRAW_FACE_BASE_X, D_RANK_DRAW_FACE_BASE_Y + (point * 50));

				// 名前の描画
				MJ_DrawName(name, D_RANK_NAME_BASE_X, (int16_t)(D_RANK_NAME_BASE_Y+(point*50)));
				point++;
			}
		}

		MpsBrewLib_DrawSprite(spGame[1 + rank], D_RANK_NUMBER_BASE_X, D_RANK_NUMBER_BASE_Y + (rank * 50));
	}

	// 結果の表示。
	switch ( agari_sts ) {
		case SEQ_END_POINT2:
			// 最終得点表示
			MJ_SeqEndPoint2();
			break;
	//> 2006/03/12 差し馬削除
	//case SEQ_END_SASHIUMA2:
	//case SEQ_END_SASHIUMA3:
	//	// 差し馬
	//	MJ_SeqEndSashiuma2();
	//	break;
	//< 2006/03/12 差し馬削除
		case SEQ_END_TIP2:
		case SEQ_END_TIP3:
			// チップ
			MJ_SeqEndTip2();
			break;
		case SEQ_END_DOBON2:
		case SEQ_END_DOBON3:
			// ドボン
			MJ_SeqEndDobon2();
			break;
		case SEQ_END_UMA2:
		case SEQ_END_UMA3:
			// 順位馬
			MJ_SeqEndUma2();
			break;
		case SEQ_END_YAKITORI2:
		case SEQ_END_YAKITORI3:
			// 焼き鳥
			MJ_SeqEndYakitori2();
			break;
		case SEQ_AG_RANKING2:
			// サシウマ・ドボン・トリ清算結果表示
			MJ_SeqAgRanking2();
			break;
	}
//	return;
#endif
#endif //-*todo:描画保留

}

/************************************************************/
/*															*/
/*						最終得点表示						*/
/*															*/
/************************************************************/
public void MJ_SeqEndPoint2( /*MahJongRally * pMe*/ )
{
	DebLog("//-*MJ:GameDraw.cs:MJ_SeqEndPoint2( )");
#if false //-*todo:描画保留
	short		nPointE	= 0;	// 局終了後の得点。
	short	ranking	= 0;	// 順位。
	short	cnt		= 0;	// 。
	short	rank	= 0;	// 。
	int			point	= 0;	// 。

	for(rank=0; rank<4; rank++)
		for( cnt=0; cnt<4; cnt++) {
			ranking = (short)gpsTableData.sMemData[cnt].byRank;
			if(ranking==rank) {
				// 最終得点表示。
				nPointE = gpsTableData.sMemData[cnt].nPoint;

				DAgariNumDisp( nPointE*100, 2, 90, 75+(point*50), 7, false, SYS_FONT_WHITE );

				//『点』の描画。
				MahJongRally_Msg_Line( NumberText[D_SYSFONT_TEN], 186, 75+(point*50), SYS_FONT_WHITE );

				point++;
			}
		}

//	return;
#endif //-*todo:描画保留
}

/************************************************************/
/*															*/
/*					  チップ結果表示						*/
/*															*/
/************************************************************/
public void MJ_SeqEndTip2( /*MahJongRally * pMe*/ )
{
	DebLog("//-*MJ:GameDraw.cs:MJ_SeqEndTip2( )");
#if false //-*todo:描画保留
	short		nPnt		= 0;		// 局終了後の得点。
	short	ranking		= 0;		// 順位。
	short			cnt			= 0;		//
	short			rank		= 0;		//
	int			point		= 0;		//

	ResultEndFlag			= false;	//OFF;

	// ポイントの描画。
	MpsBrewLib_DrawSprite( spGame[D_GAME_END_POINT_BG],D_END_POINT_BASE_X,D_END_POINT_BASE_Y );

	// チップの描画。
	MpsBrewLib_DrawSprite( spGame[D_GAME_END_TIP],D_END_POINT_BG_X,D_END_POINT_BASE_Y );


 	for( rank=0 ; rank<4 ; rank++ )
	{
		for( cnt=0; cnt<4 ; cnt++ )
		{
			// ランキング。
			ranking = (short)gpsTableData.sMemData[cnt].byRank;
			if( ranking==rank )
			{

				// 結果の表示。
				switch(agari_sts)
				{
					case (byte)SEQ.END_TIP3:
					{
						NowResultDisp[cnt] = gpsTableData.sMemData[cnt].nPnt;	// 現在のポイント。
						gpsTableData.sMemData[cnt].nMovePnt = 0;				// 移動ポイントを0に。
						break;
					}
				}

				// ポイント。
				DAgariNumDisp( NowResultDisp[cnt], 2, D_TOTAL_POINT_X, D_TOTAL_POINT_Y+(point*50), 7, FALSE, SYS_FONT_WHITE  );

				// 順位馬。
				nPnt = gpsTableData.sMemData[cnt].nMovePnt;
				DAgariNumDisp( nPnt, 2, D_BASE_ALL_NPNT_X, D_BASE_ALL_NPNT_Y+(point*50), 7, FALSE, SYS_FONT_WHITE	);

				point++;
			}
		}
	}

	return;
#endif //-*todo:描画保留

}

/************************************************************/
/*															*/
/*					  ドボン結果表示						*/
/*															*/
/************************************************************/
public void MJ_SeqEndDobon2( /*MahJongRally * pMe*/ )
{
	DebLog("//-*MJ:GameDraw.cs:MJ_SeqEndDobon2( )");
#if false //-*todo:描画保留
	short		nPnt		= 0;		// 局終了後の得点。
	short	ranking		= 0;		// 順位。
	int			cnt			= 0;		//
	int			rank		= 0;		//
	int			point		= 0;		//

	ResultEndFlag			= false;	//OFF;

	// ポイントの描画。
	MpsBrewLib_DrawSprite( spGame[D_GAME_END_POINT_BG],D_END_POINT_BASE_X,D_END_POINT_BASE_Y );

	// ドボンの描画。
	MpsBrewLib_DrawSprite( spGame[D_GAME_END_DOBON],D_END_POINT_BG_X,D_END_POINT_BASE_Y );

 	for( rank=0 ; rank<4 ; rank++ )
	{
		for( cnt=0; cnt<4 ; cnt++ )
		{
			// ランキング。
			ranking = (short)gpsTableData.sMemData[cnt].byRank;
			if( ranking==rank )
			{

				// 結果の表示。
				switch(agari_sts)
				{
					case (byte)SEQ.END_DOBON3:
					{
						NowResultDisp[cnt] = gpsTableData.sMemData[cnt].nPnt;	// 現在のポイント。
						gpsTableData.sMemData[cnt].nMovePnt = 0;				// 移動ポイントを0に。
					}
				}

				// ポイント。
				DAgariNumDisp( NowResultDisp[cnt], 2, D_TOTAL_POINT_X, D_TOTAL_POINT_Y+(point*50), 7, FALSE, SYS_FONT_WHITE  );

				// 順位馬。
				nPnt = gpsTableData.sMemData[cnt].nMovePnt;
				DAgariNumDisp( nPnt, 2, D_BASE_ALL_NPNT_X, D_BASE_ALL_NPNT_Y+(point*50), 7, FALSE, SYS_FONT_WHITE	);

				point++;
			}
		}
	}

	return;
#endif //-*todo:描画保留
}

/************************************************************/
/*															*/
/*					  順位馬結果表示						*/
/*															*/
/************************************************************/
public void MJ_SeqEndUma2( /*MahJongRally * pMe*/ )
{
	DebLog("//-*MJ:GameDraw.cs:MJ_SeqEndUma2( )");
#if false //-*todo:描画保留	short		nPnt		= 0;		// 局終了後の得点。
	short	ranking		= 0;		// 順位。
	int			cnt			= 0;		//
	int			rank		= 0;		//
	int			point		= 0;		//

	ResultEndFlag			= false;	//OFF;

	// ポイントの描画。
	MpsBrewLib_DrawSprite( spGame[D_GAME_END_POINT_BG],D_END_POINT_BASE_X,D_END_POINT_BASE_Y );

	// 順位馬の描画。
	MpsBrewLib_DrawSprite( spGame[D_GAME_END_RANK_UMA],D_END_POINT_BG_X,D_END_POINT_BASE_Y );


 	for( rank=0 ; rank<4 ; rank++ )
	{
		for( cnt=0; cnt<4 ; cnt++ )
		{
			// ランキング。
			ranking = (short)gpsTableData.sMemData[cnt].byRank;
			if( ranking==rank )
			{

				// 結果の表示。
				switch(agari_sts)
				{
					case (byte)SEQ.END_UMA3:
					{
						NowResultDisp[cnt] = gpsTableData.sMemData[cnt].nPnt;	// 現在のポイント。
						gpsTableData.sMemData[cnt].nMovePnt = 0;				// 移動ポイントを0に。
					}
				}

				// ポイント。
				DAgariNumDisp( NowResultDisp[cnt], 2, D_TOTAL_POINT_X, D_TOTAL_POINT_Y+(point*50), 7, false, SYS_FONT_WHITE  );

				// 順位馬。
				nPnt = gpsTableData.sMemData[cnt].nMovePnt;
				DAgariNumDisp( nPnt, 2, D_BASE_ALL_NPNT_X, D_BASE_ALL_NPNT_Y+(point*50), 7, false, SYS_FONT_WHITE	);

				point++;
			}
		}
	}

	return;
#endif //-*todo:描画保留
}

/************************************************************/
/*															*/
/*					  焼き鳥結果表示						*/
/*															*/
/************************************************************/
public void MJ_SeqEndYakitori2( /*MahJongRally * pMe*/ )
{
	DebLog("//-*MJ:GameDraw.cs:MJ_SeqEndYakitori2( )");
#if false //-*todo:描画保留
	short		nPnt		= 0;		// 局終了後の得点。	short		nPnt		= 0;		// 局終了後の得点。
	short	ranking		= 0;		// 順位。
	int			cnt			= 0;		//
	int			rank		= 0;		//
	int			point		= 0;		//

	ResultEndFlag			= false;	//OFF;

	// ポイントの描画。
	MpsBrewLib_DrawSprite( spGame[D_GAME_END_POINT_BG],D_END_POINT_BASE_X,D_END_POINT_BASE_Y );

	// ヤキトリの描画。
	MpsBrewLib_DrawSprite( spGame[D_GAME_END_YAKITORI],D_END_POINT_BG_X,D_END_POINT_BASE_Y );

 	for( rank=0 ; rank<4 ; rank++ )
	{
		for( cnt=0; cnt<4 ; cnt++ )
		{
			// ランキング。
			ranking = (short)gpsTableData.sMemData[cnt].byRank;
			if( ranking==rank )
			{

				// 結果の表示。
				switch(agari_sts)
				{
					case (byte)SEQ.END_YAKITORI3:
					{
						NowResultDisp[cnt] = gpsTableData.sMemData[cnt].nPnt;	// 現在のポイント。
						gpsTableData.sMemData[cnt].nMovePnt = 0;				// 移動ポイントを0に。
					}
				}

				// ポイント。
				DAgariNumDisp( NowResultDisp[cnt], 2, D_TOTAL_POINT_X, D_TOTAL_POINT_Y+(point*50), 7, false, SYS_FONT_WHITE  );

				// 順位馬。
				nPnt = gpsTableData.sMemData[cnt].nMovePnt;
				DAgariNumDisp( nPnt, 2, D_BASE_ALL_NPNT_X, D_BASE_ALL_NPNT_Y+(point*50), 7, false, SYS_FONT_WHITE	);

				point++;
			}
		}
	}

	return;
#endif //-*todo:描画保留
}

/************************************************************/
/*															*/
/*			 サシウマ・ドボン・トリ清算結果表示				*/
/*															*/
/************************************************************/
public void MJ_SeqAgRanking2( /*MahJongRally * pMe*/ )
{
	DebLog("//-*MJ:GameDraw.cs:MJ_SeqAgRanking2( )");
#if false //-*todo:描画保留
//	int			i		= 0;
//	short	member	= 0;	// メンバー。
	short		nPnt	= 0;	// 局終了後の得点。
	short	ranking	= 0;	// 順位。
	int			cnt;
	int			rank=0;
	int			point=0;

	// ポイントの描画。
	//MpsBrewLib_DrawSprite( spGame[D_GAME_END_POINT_BG],D_END_POINT_BASE_X,D_END_POINT_BASE_Y );

	MpsBrewLib_DrawSprite( spGame[D_GAME_END_POINT_BG], D_END_POINT_BASE_X, D_END_POINT_BASE_Y );

	for(rank=0; rank<4; rank++)
	{
		for( cnt=0; cnt<4; cnt++)
		{
			ranking = (short)gpsTableData.sMemData[cnt].byRank;

			if(ranking==rank)
			{
				// 最終得点表示。
				nPnt = gpsTableData.sMemData[cnt].nPnt;					// 移動点数。
				DAgariNumDisp( nPnt, 2, D_END_POINT_BG_X, D_BASE_ALL_NPNT_Y+(point*50), 7, false, SYS_FONT_WHITE	);
				point++;
			}
		}
	}
	return;
#endif //-*todo:描画保留
}


/**
 * ゲーム中のチャット描画。
 *
 * @version 	2005.12.6
 *
 * @param		pMe		オブジェクトのポインタ。
 *
 * @deprecated	ゲーム中に一覧を表示してチャット文章を選択。
 *
 */
public void MJ_GameChatDraw( /*MahJongRally * pMe*/ )
{
	DebLog("//-*MJ:GameDraw.cs:MJ_GameChatDraw( )");
#if false //-*todo:描画保留
	short	i	 =0;	//

	// フラグが立っていなければ戻る。
	#if false //-*todo:通信
	if ( ChatFlag == 0 ) return;
	//> 2006/03/31 チャットメニューが出続ける不具合		//0422
	// 通信対戦時・手動モードで無ければ戻る
	if( IS_NETWORK_MODE && gDaiuchiFlag !=	D_ONLINE_OPERATOR_MANUAL )
	{
		return ;
	}
	//< 2006/03/31 チャットメニューが出続ける不具合

	// メニューの描画。
	_MJ_DrawMenu(50, 40, D_CHAT_LIST_SHOW_MAX, 11, NORMAL_MENU);

	// 文字列の描画。
	for( i = (short)FirstChatNo ; i < (D_CHAT_LIST_SHOW_MAX + (short)FirstChatNo) ; i++ ) {
		if( gMyTable_ChatMsgList[i].length() == 0 ) continue;		//if( WSTRLEN( gMyTable_ChatMsgList[i] ) == 0 ) continue;
		if( i > D_CHAT_MSG_MAX ) continue;
		MahJongRally_Msg_Line( gMyTable_ChatMsgList[i], 77, 69+(i-FirstChatNo)*20, SYS_FONT_WHITE );	//(AECHAR*)
	}

	// 文字選択アイコンの描画。
	MJ_IconDraw(D_TRIANGLE_RIGHT, 55, 69+(20*chat_menu_csr), 0);

	if ( FirstChatNo != 0 ) MJ_IconDraw(D_TRIANGLE_UP,	105, 50, 0);	// 上向きアイコン。
	if ( FirstChatNo != (D_CHAT_LIST_MAX - D_CHAT_LIST_SHOW_MAX) ) MJ_IconDraw(D_TRIANGLE_DOWN,105, 215, 0);	// 下向きアイコン。	// 2006/02/26 No.147

	#endif //-*todo:通信
#endif //-*todo:描画保留

}


/**
 * ゲーム中の各プレイヤーから
 * 送られてきたメッセージを描画。
 *
 * @version 	2005.12.6
 *
 * @param		pMe		オブジェクトのポインタ。
 *
 * @deprecated	ゲーム中にチャットを表示。
 *
 */
//
public void MJ_GameChatTalkDraw(/*MahJongRally* pMe*/)
{
	DebLog("//-*MJ:GameDraw.cs:MJ_GameChatTalkDraw( )");
#if false //-*todo:描画保留
	short[]	MenuPosX = {	50-8,	75-16,	50-8,	5};
	short[]	MenuPosY = { 200, 140,  20,	80};
	short[]	ChatPosX = { 100-8, 120-16, 100-8,  52};
	short[]	ChatPosY = { 225, 165,  45, 105};
	short[]	FacePosX = {	55-8,	80-16,	55-8,  10};
	short[]	FacePosY = { 204, 144,  24,	84};
	short[]	NamePosX = { 110-8, 135-16, 110-8,  65};
	short[]	NamePosY = { 205, 145,  25,	85};
	short				Len, posX, i;
	short				House;

	for ( i = 0 ; i < MJDefine.HOUSE_MAX_NUM ; i++ ) {
		String					name = null;
		SpriteInfo				sprite_= null;
//		String					szText;		//AECHAR	szText[32]={0};

		House = (short)((game_player + i) % (short)MJDefine.HOUSE_MAX_NUM);

		if ( ChatWaitTime[House] <= 0 ) {
			ChatMsg[House]= "";		//MEMSET(ChatMsg[House], 0, D_CHAT_MSG_MAX);		//sizeof( AECHAR ) * D_CHAT_MSG_MAX
			continue;
		}

		ChatWaitTime[House] -= (short)uptimems_perfrm;

		if( ChatMsg[House].length() == 0 ) continue;		//if( WSTRLEN( ChatMsg[House] ) == 0 ) continue;

		name = gsTableData[0].sMemData[House].NickName;	// 名前					//gsTableData
		if ( gsTableData[0].sMemData[House].byMember < MJDefine.MAX_COMP_CHARACTER ) {	//gsTableData
			sprite_ = spChar[gsTableData[0].sMemData[House].byMember];			//gsTableData
		}

		if ( (WhichDrawMode == D_DRAW_NETWORK_MODE) && ((byte)gNetTable_NetChar[House].Flag != (byte)0xFF) ) {
			// 通信対戦でプレイヤーの顔があれば入れ替え
			sprite_ = gNetTable_NetChar[House].spriteChar;
			if ( sprite_.pIImage== NULL && (gNetTable_NetChar[House].CharPicNo < MJDefine.MAX_COMP_CHARACTER) ) {	//!sprite_.pIImage
				sprite_ = spChar[gNetTable_NetChar[House].CharPicNo];
			}
		}

		// メニューの描画
		MJ_DrawMenu(MenuPosX[i], MenuPosY[i], 9, 2, NORMAL_MENU);

		// キャラクターのセリフ描画
		Len = (short)MJ_GetStrLen( ChatMsg[House]);		//(AECHAR*)	// セリフの長さ取得
		posX = (short)(ChatPosX[i]+((114+16-Len)/2));	//w21t ? (ChatPosX[i]+((114+16-Len)/2)) : (ChatPosX[i]+((108+16-Len)/2));

		MahJongRally_Msg_Line( ChatMsg[House], posX, ChatPosY[i], SYS_FONT_WHITE );

		Len = (short)MJ_GetStrLen( gsTableData[0].sMemData[House].NickName );	//(AECHAR*)	// 名前の長さ取得	//gsTableData
		posX = (short)(NamePosX[i]+((90+16-Len)/2));		//w21t ? (NamePosX[i]+((90+16-Len)/2)) : (NamePosX[i]+((80+16-Len)/2));	// 名前 描画位置決定
//		STREXPAND((const byte*)gsTableData[0].sMemData[House].NickName,D_NICK_NAME_MAX,szText,sizeof(szText));	//gsTableData
		MahJongRally_Msg_Line( gsTableData[0].sMemData[House].NickName, posX, NamePosY[i], SYS_FONT_WHITE );		// キャラクターの名前描画	//szText	//gsTableData

		// 顔描画
		DrawCharacterFace( sprite_, FacePosX[i],FacePosY[i] );

	}
	return;
#endif //-*todo:描画保留
}

/**
 *
 * ゲーム中にオプションモードの
 * 切り替えを行う。(ロジック)
 *
 * @param1 オブジェクトのポインタ。
 *
 *
 */
// D_OPTION_BGM=0,		// 音量。
// D_OPTION_DAIUCHI,	// 代打ち。
// D_OPTION_SETUDAN,	// 切断中。
// D_OPTION_TALK,		// セリフ。
// D_OPTION_NAKINASHI,	// 鳴きなし。
// D_MAX_OPTION_COUNT
public void MJ_GameModeChangeMain( /*MahJongRally * pMe*/ )
{
	DebLog("//-*MJ:GameDraw.cs:MJ_GameModeChangeMain( )");
#if false //-*todo:描画保留
	// --- 音量 ---。
	// f_info.f_optionsetting.m_OptionSel[D_OPTION_BGM]
	FileOptionSetting optset_ = f_info.f_optionsetting;

	if( OptionWaitTime[D_OPTION_DAIUCHI] > 0 ) OptionWaitTime[D_OPTION_DAIUCHI] -= (short)uptimems_perfrm;//0508mt
	if( OptionWaitTime[D_OPTION_SETUDAN] > 0 ) OptionWaitTime[D_OPTION_SETUDAN] -= (short)uptimems_perfrm;//0508mt
	if( OptionWaitTime[D_OPTION_CHAR_TALK] > 0 ) OptionWaitTime[D_OPTION_CHAR_TALK] -= (short)uptimems_perfrm;//0508mt
	if( OptionWaitTime[D_OPTION_NAKINASHI] > 0 ) OptionWaitTime[D_OPTION_NAKINASHI] -= (short)uptimems_perfrm;//0508mt

	// --- 代打ち。
	if( IS_NETWORK_MODE ) {
		if( (GetKeyTrg(_KEY_1))!= 0 ) {
			switch( select_opt[D_SELECT_OPT_DAIUCHI] ) {
				case D_ONLINE_OPERATOR_MANUAL : {
					select_opt[D_SELECT_OPT_DAIUCHI] = D_ONLINE_OPERATOR_PASS;
				}
				break;
				case D_ONLINE_OPERATOR_PASS : {
					select_opt[D_SELECT_OPT_DAIUCHI] = D_ONLINE_OPERATOR_AI;
				}
				break;
				case D_ONLINE_OPERATOR_AI : {
					select_opt[D_SELECT_OPT_DAIUCHI] = D_ONLINE_OPERATOR_MANUAL;
				}
				break;
				default : {
					select_opt[D_SELECT_OPT_DAIUCHI] = D_ONLINE_OPERATOR_MANUAL;
				}
				break;
			}

			mythink_select_ok_flag = 0;

			OptionWaitTime[D_OPTION_DAIUCHI]  = 2000;	// 代打ち。
			OptionWaitTime[D_OPTION_SETUDAN]  = 0;		// 切断中。
			OptionWaitTime[D_OPTION_TALK]	  = 0;		// セリフ。
			OptionWaitTime[D_OPTION_NAKINASHI]= 0;		// 鳴きなし。
		}
	}

	// --- 切断中 ---。
	if( IS_NETWORK_MODE ) {
		if( (GetKeyTrg(_KEY_3))!= 0 ) {
			{
				switch( select_opt[ D_SELECT_OPT_SETSUDAN ] ) {
					case  0 : select_opt[ D_SELECT_OPT_SETSUDAN ] = 1; break;
					case  1 : select_opt[ D_SELECT_OPT_SETSUDAN ] = 0; break;
					default : select_opt[ D_SELECT_OPT_SETSUDAN ] = 0; break;
				}

				// 2006/02/24 状態変更時に他のＰＣへ状態変更通知が届かない
				mypinch_hitter_select_ok_flag = 0;
			}

			OptionWaitTime[D_OPTION_DAIUCHI]  = 0;		// 代打ち。
			OptionWaitTime[D_OPTION_SETUDAN]  = 2000;	// 切断中。
			OptionWaitTime[D_OPTION_TALK]	  = 0;		// セリフ。
			OptionWaitTime[D_OPTION_NAKINASHI]= 0;		// 鳴きなし。
		}
	}

	// --- セリフ ---。
	if( !IS_NETWORK_MODE || ( IS_NETWORK_MODE && gDaiuchiFlag == D_ONLINE_OPERATOR_MANUAL ) ) {	//0504mt //0427mt
#if	__MJ_CHECK
/*
		if((GetKeyTrg(_KEY_7))!= 0) {
			switch( optset_.m_OptionSel[D_OPTION_CHAR_TALK] ) {
				case D_OPTION_CHAR_TALK_OFF :
					optset_.m_OptionSel[D_OPTION_CHAR_TALK] = D_OPTION_CHAR_TALK_ON;
					break;
				case D_OPTION_CHAR_TALK_ON	:
					optset_.m_OptionSel[D_OPTION_CHAR_TALK] = D_OPTION_CHAR_TALK_OFF;
					break;
				default :
					optset_.m_OptionSel[D_OPTION_CHAR_TALK] = D_OPTION_CHAR_TALK_OFF;
					break;
			}
			OptionSetData( D_OPTION_FLG_SET );

			//if( !IS_NETWORK_MODE )
			//	// セーブする。
			//	MJ_WriteFile( D_WORD_INFOFILE, f_info);

			OptionWaitTime[D_OPTION_DAIUCHI]  = 0;		// 代打ち。
			OptionWaitTime[D_OPTION_SETUDAN]  = 0;		// 切断中。
			OptionWaitTime[D_OPTION_TALK]	  = 2000;	// セリフ。
			OptionWaitTime[D_OPTION_NAKINASHI]= 0;		// 鳴きなし。

//			removeSoftkey();//0527mt No.1077	//No1007
		}
*/
#else
		optset_.m_OptionSel[D_OPTION_CHAR_TALK] = D_OPTION_CHAR_TALK_OFF;
#endif
	}
//zeniya
#if DEBUG
	if((GetKeyTrg(_KEY_7))!= 0)		// --- 牌をオープン デバック ---。
		hai_open^= 0xFF;
#endif
#if HEAP_DEBUG
	if(hai_open != 0xFF) {
		hai_open = 0xFF;
	}
#endif
#if	__MJ_CHECK
	// --- 鳴きなし デバック ---。
	if((GetKeyTrg(_KEY_7))!= 0) {
		if( Optcnt == 0 ) {
			switch( select_opt[D_SELECT_OPT_NAKI_NASHI] ) {
				case D_OPTION_NAKINASHI_OFF :
					select_opt[D_SELECT_OPT_NAKI_NASHI] = D_OPTION_NAKINASHI_ON;
					break;
				case D_OPTION_NAKINASHI_ON :
					select_opt[D_SELECT_OPT_NAKI_NASHI] = D_OPTION_NAKINASHI_OFF;
					break;
				default :
					select_opt[D_SELECT_OPT_NAKI_NASHI] = D_OPTION_NAKINASHI_OFF;
					break;
			}

			OptionWaitTime[D_OPTION_DAIUCHI]  = 0;		// 代打ち。
			OptionWaitTime[D_OPTION_SETUDAN]  = 0;		// 切断中。
			OptionWaitTime[D_OPTION_TALK]	  = 0;		// セリフ。
			OptionWaitTime[D_OPTION_NAKINASHI]= 2000;	// 鳴きなし。

			// フラグ更新
			updateNakiNashiFlag( );
		}
	}
#endif
#endif //-*todo:描画保留
}

/**
 * ゲーム中にオプションモードの
 * 切り替えを行う。(描画)
 *
 * @param1 オブジェクトのポインタ。
 */
public void MJ_GameModeChangeDraw( /*MahJongRally * pMe*/ )
{
	DebLog("//-*MJ:GameDraw.cs:MJ_GameModeChangeDraw( )");
#if false //-*todo:描画保留
#if	__MJ_CHECK
	String str;			//char* str=0;
//	String szText;		//AECHAR szText[13]={0};

	// --- 音量。
	// 中 -> 大 -> OFF -> 小 -> 中
	int x_ = 23;//0510mt
	int y_ = 200;//0510mt

	if( OptionWaitTime[D_OPTION_DAIUCHI] > 0 ) {
		// --- 代打ち。
		// OFF -> ツモ切り -> AI -> OFF
		switch( select_opt[ D_SELECT_OPT_DAIUCHI ] ) {
			case D_ONLINE_OPERATOR_MANUAL : str = "代打ちOFF";	break;
			case D_ONLINE_OPERATOR_PASS   : str = " ツモ切";	break;
			case D_ONLINE_OPERATOR_AI	  : str = "  ＡＩ";		break;
			default 					  : str = "";			break;
		}
		_MJ_DrawMenu( x_, y_, 2, 0, 1);//0510mt		STREXPAND((const byte*)str,STRLEN(str),szText,sizeof(szText));
		MahJongRally_Msg_Line(	str, x_+10-2, y_+2, ((255<<16)+(255<<8)+(255)) );	//0510mt		MJ_Msg_CenterLine(	str, 0, 270, SYS_FONT_WHITE );	//szText // 06/05/31 No.1102
//0510mt	OptionWaitTime[D_OPTION_DAIUCHI] -= (int32_t)uptimems_perfrm;
	} else if( OptionWaitTime[D_OPTION_SETUDAN] > 0 ) {
		switch( select_opt[ D_SELECT_OPT_SETSUDAN ] ) {
			case 0	: str = " ツモ切";		break;
			case 1	: str = "  ＡＩ";		break;
			default : str = "";				break;
		}
		_MJ_DrawMenu( x_, y_, 2, 0, 1);//0510mt		STREXPAND((const byte*)str,STRLEN(str),szText,sizeof(szText));
		MahJongRally_Msg_Line(	str, x_+10-2, y_+2, ((255<<16)+(255<<8)+(255)) );	//0510mt		MJ_Msg_CenterLine(	str, 0, 270, SYS_FONT_WHITE );	//szText // 06/05/31 No.1102
//0510mt	OptionWaitTime[D_OPTION_SETUDAN] -= (int32_t)uptimems_perfrm;
	} else if( OptionWaitTime[D_OPTION_TALK] > 0 ) {
		// --- セリフ。
		// ON -> OFF -> ON
		str = opt_game[GOPT_TLK]==GOPT_TLK_ON ? "セリフON" : "セリフOFF";

		_MJ_DrawMenu( x_, y_, 2, 0, 1);//0510mt		STREXPAND((const byte*)str,STRLEN(str),szText,sizeof(szText));
		MahJongRally_Msg_Line(	str, x_+(opt_game[GOPT_TLK]==GOPT_TLK_ON ? 10 : 10-2), y_+2, ((255<<16)+(255<<8)+(255)) );	//0510mt		MJ_Msg_CenterLine(	str, 0, 270, SYS_FONT_WHITE );	//szText // 06/05/31 No.1102
//0510mt	OptionWaitTime[D_OPTION_TALK] -= (int32_t)uptimems_perfrm;
	} else if( OptionWaitTime[D_OPTION_NAKINASHI] > 0 ) {
		switch( select_opt[D_SELECT_OPT_NAKI_NASHI] ) {
			case D_OPTION_NAKINASHI_OFF : str = "鳴きあり"; break;
			case D_OPTION_NAKINASHI_ON	: str = "鳴きなし"; break;
			default 					: str = ""; 		break;
		}
		_MJ_DrawMenu( x_, y_, 2, 0, 1);//0510mt		STREXPAND((const byte*)str,STRLEN(str),szText,sizeof(szText));
		MahJongRally_Msg_Line(	str, x_+10, y_+2, ((255<<16)+(255<<8)+(255)) );	//0510mt		MJ_Msg_CenterLine(	str, 0, 270, SYS_FONT_WHITE );	//szText
//0510mt	OptionWaitTime[D_OPTION_NAKINASHI] -= (int32_t)uptimems_perfrm;
	}
//	return;
#endif
#endif //-*todo:描画保留
}


/**
 *
 * ゲーム中のチャットロジック。
 *
 * @param1 オブジェクトのポインタ。
 *
 */
public void MJ_GameChatMain( /*MahJongRally * pMe*/ )
{

	DebLog("//-*MJ:GameDraw.cs:MJ_GameChatMain( )");
#if false //-*todo:描画保留
	// メニューがあればﾓﾄﾞﾙ。
	// 対話があればﾓﾄﾞﾙ。
	if( Optcnt != 0 || gTalkFlag != 0 )
	{
		ChatFlag = 0;
		return;
	}

	if( D_ONEPUSH_INPUT_SOFT1 )
	{
		// Flag管理
		// 0->OFF
		// 1->ON
		ChatFlag++;
		ChatFlag &=1;
	}

	if( ChatFlag == 0 ) return ;

	// --- ロジック --- 。
	if( D_ONEPUSH_INPUT_SELECT && gChatSendFlag ) {
		SeOnPlay( SE_KETTEI, 0 );

		MJ_Game_ChatMsg_Send_Request( gMyTable_ChatMsgList[chat_menu_csr + FirstChatNo] );

		// FlagOFFにする。
		ChatFlag = 0;
		gChatSendFlag = FALSE;
	} else
	if ( D_ONEPUSH_INPUT_UP ) {
		SeOnPlay(SE_ATACK, 0);
		chat_menu_csr--;

		if ( chat_menu_csr < 0 ) {
			// > 2006/02/08 要望No.82
			if ( FirstChatNo > 0 ) {
				chat_menu_csr = 0;
				FirstChatNo--;
			} else {
				// 上端から下端へ
				chat_menu_csr = D_CHAT_LIST_SHOW_MAX - 1;
				FirstChatNo = D_CHAT_LIST_MAX - D_CHAT_LIST_SHOW_MAX;	// 2006/02/26 No.147
			}
			// < 2006/02/08 要望No.82
		}

		DTRACE(("FirstChatNo = %d",FirstChatNo));
		DTRACE(("chat_menu_csr = %d",chat_menu_csr));
	} else
	if ( D_ONEPUSH_INPUT_DOWN ) {
		SeOnPlay(SE_ATACK, 0);
		chat_menu_csr++;
		if ( chat_menu_csr > (D_CHAT_LIST_SHOW_MAX - 1) ) {
			// > 2006/02/08 要望No.82
			if ( FirstChatNo < (D_CHAT_LIST_MAX - D_CHAT_LIST_SHOW_MAX) ) {	// 2006/02/26 No.147
				chat_menu_csr = D_CHAT_LIST_SHOW_MAX - 1;
				FirstChatNo++;
			} else {
				// 下端から上端へ
				chat_menu_csr = 0;
				FirstChatNo = 0;
			}
			// < 2006/02/08 要望No.82
		}
		DTRACE(("FirstChatNo = %d",FirstChatNo));
		DTRACE(("chat_menu_csr = %d",chat_menu_csr));
	}
//< 2006/02/06チャット変更

//	return;
#endif //-*todo:描画保留
}

/**
 * 和了時のフーロ牌描画。
 */
#if	Rule_2P
//public const int D_AGARI_FR_DRAW_BASE_X	226
#else
public const int D_AGARI_FR_DRAW_BASE_X	17
#endif
public void MJ_FrJicyaDraw( /*MahJongRally * pMe,*/ int House, int D_JICYA_TEHAI_Y )
{
	DebLog("//-*MJ:GameDraw.cs:MJ_FrJicyaDraw( int "+House+", int "+D_JICYA_TEHAI_Y+" )");
#if false //-*todo:描画保留
	int Bbit		= 0;	// フーロ牌情報取得
	int Fr			= 0;	// フーロ牌チェック用
	int FrPos		= 0;	// 左0・中心1・右2．
	int furoOffBaseX= 0;	// フーロ牌用のOFFset
	int furoOffX	= 0;	// フーロ牌用のOFFset
	int Hbit		= 0;	// 上位ビット
	int i			= 0;	// ループ
	int j			= 0;	// チーループ用。
	int SBit		= 0;	// 下位ビット

#if	Rule_2P
	// フーロ牌があるかチェック
	Fr = gsPlayerWork[House].byFhcnt;

	if( Fr == 0 )	return;							// フーロ牌が無ければ。
	if( Fr > 4 )	Fr = 4;

	// フーロ牌の情報取得
	for( i= 0; i< Fr; i++ ) {
		Bbit = gsPlayerWork[House].byFrhai[i];		// フーロ牌情報取得。

		//Frに4以上が入る不具合発生のため暫定的にここで処理を入れる 20051202
		if(Bbit == 0)	return;

#if	Rule_2P
		FrPos= 2;		//対面から
#else
		FrPos= gsPlayerWork[House].byFrpos[i];		// 鳴き場所取得。
#endif

		Hbit = Bbit & 0xC0;							// 上位ビットの取得。
		switch( Hbit ) {
			// 明カン(自家)
			case D_MINKAN:
				SBit = Bbit & 0x3F;					// 下位ビットの取得
				furoOffX = 0;
				furoOffBaseX = 0;
				{
					if( FrPos==1) {
						// 済。
						// 横になる牌。
						hMpsBrewLib_DrawSprite_line4( spSutePaiShimocya, SBit,
							-(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+0,
							D_JICYA_TEHAI_Y-8+_y_of011+ _y_of012+ _y_of013);
						hMpsBrewLib_DrawSprite_line4( spSutePaiShimocya, SBit,
							-(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+0,
							D_JICYA_TEHAI_Y+3+_y_of011+ _y_of012);
						// 自家通常牌。
						hMpsBrewLib_DrawSprite( spSutePaiJicya, SBit, -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+13,D_JICYA_TEHAI_Y+_y_of011);
						hMpsBrewLib_DrawSprite( spSutePaiJicya, SBit, -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+24,D_JICYA_TEHAI_Y+_y_of011);
					}
					if( FrPos==2 ) {
						// 済。
						// 自家通常牌。
						hMpsBrewLib_DrawSprite( spSutePaiJicya, SBit,
							-(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+0,
							D_JICYA_TEHAI_Y+_y_of011);
						// 横になる牌。
						hMpsBrewLib_DrawSprite_line4( spSutePaiShimocya, SBit,
							-(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+11+2,
							D_JICYA_TEHAI_Y-8+_y_of011+ _y_of012+ _y_of013);
						hMpsBrewLib_DrawSprite_line4( spSutePaiShimocya, SBit,
							-(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+11+2,
							D_JICYA_TEHAI_Y+3+_y_of011+ _y_of012);
						// 自家通常牌。
						hMpsBrewLib_DrawSprite( spSutePaiJicya, SBit,
							-(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+25+2+3,
							D_JICYA_TEHAI_Y+_y_of011);
					}
					if( FrPos==3) {
						// 済。
						// 自家通常牌。
						hMpsBrewLib_DrawSprite( spSutePaiJicya, SBit, -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+0,D_JICYA_TEHAI_Y+_y_of011);
						hMpsBrewLib_DrawSprite( spSutePaiJicya, SBit, -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+11,D_JICYA_TEHAI_Y+_y_of011);
						// 横になる牌。
						hMpsBrewLib_DrawSprite_line4( spSutePaiShimocya, SBit, -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+22,D_JICYA_TEHAI_Y-8+_y_of011+ _y_of012+ _y_of013);
						hMpsBrewLib_DrawSprite_line4( spSutePaiShimocya, SBit, -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+22,D_JICYA_TEHAI_Y+3+_y_of011+ _y_of012);
					}
				}
				break;

			// 暗カン(両端を裏にして表示)
			case D_ANKAN:
				SBit = Bbit & 0x3F;					// 下位ビットの取得
				{	// 済。
					hMpsBrewLib_DrawSprite( spSutePaiJicya, PAI_URA,
						-(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+(0*D_PAI_TATE_SMALL_SIZE_W)+0),
						D_JICYA_TEHAI_Y+_y_of011);
					hMpsBrewLib_DrawSprite( spSutePaiJicya, PAI_URA,
						-(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+(2*D_PAI_TATE_SMALL_SIZE_W)+0)+3+1,
						D_JICYA_TEHAI_Y+_y_of011);
					hMpsBrewLib_DrawSprite_line4( spSutePaiShimocya, SBit,
						-(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+(1*D_PAI_TATE_SMALL_SIZE_W)-0),
						D_JICYA_TEHAI_Y-8+_y_of011+ _y_of012+ _y_of013);
					hMpsBrewLib_DrawSprite_line4( spSutePaiShimocya, SBit,
						-(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+(1*D_PAI_TATE_SMALL_SIZE_W)-0),
						D_JICYA_TEHAI_Y+3+_y_of011+ _y_of012);
				}
				break;

			// ポン
			case D_PON:
				SBit = Bbit & 0x3F;					// 下位ビットの取得
				furoOffX = 0;

				for( j= 1; j<= 3; j++ ) {
					if( j == FrPos ) {
						// 左・中心・右のどこに置くか
						hMpsBrewLib_DrawSprite_line4( spSutePaiShimocya, SBit, -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X+furoOffX, D_JICYA_TEHAI_Y+3+_y_of011+ _y_of012);
						furoOffX = furoOffX+ D_PAI_YOKO_SMALL_SIZE_W;
						continue;
					}
					hMpsBrewLib_DrawSprite( spSutePaiJicya, SBit, -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X+furoOffX, D_JICYA_TEHAI_Y+_y_of011);
					furoOffX = furoOffX+ D_PAI_TATE_SMALL_SIZE_W;
				}
				break;

			// チー
			default:
				FrPos= gsPlayerWork[House].byFrpos[i];		// 鳴き場所取得。

				// チーの鳴き牌処理
				// MahJongRally_DispChiPai( i, Bbit, FrPos, House );
				switch( FrPos ) {
					case 0: {
						hMpsBrewLib_DrawSprite_line4( spSutePaiShimocya, Bbit+0, -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X, D_JICYA_TEHAI_Y+3+_y_of011+ _y_of012);
						hMpsBrewLib_DrawSprite( spSutePaiJicya, Bbit+1, -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X+D_PAI_YOKO_SMALL_SIZE_W, D_JICYA_TEHAI_Y+_y_of011);
						hMpsBrewLib_DrawSprite( spSutePaiJicya, Bbit+2, -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X+D_PAI_YOKO_SMALL_SIZE_W+D_PAI_TATE_SMALL_SIZE_W, D_JICYA_TEHAI_Y+_y_of011);
						break;
					}
					case 1: {
						hMpsBrewLib_DrawSprite_line4( spSutePaiShimocya, Bbit+1, -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X, D_JICYA_TEHAI_Y+3+_y_of011+ _y_of012);
						hMpsBrewLib_DrawSprite( spSutePaiJicya, Bbit+0, -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X+D_PAI_YOKO_SMALL_SIZE_W, D_JICYA_TEHAI_Y+_y_of011);
						hMpsBrewLib_DrawSprite( spSutePaiJicya, Bbit+2, -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X+D_PAI_YOKO_SMALL_SIZE_W+D_PAI_TATE_SMALL_SIZE_W, D_JICYA_TEHAI_Y+_y_of011);
						break;
					}
					case 2: {
						hMpsBrewLib_DrawSprite_line4( spSutePaiShimocya, Bbit+2, -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X, D_JICYA_TEHAI_Y+3+_y_of011+ _y_of012);
						if( Bbit == 32 || Bbit == 31 )
							break;
						else
							hMpsBrewLib_DrawSprite( spSutePaiJicya, Bbit+0, -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X+D_PAI_YOKO_SMALL_SIZE_W, D_JICYA_TEHAI_Y+_y_of011);
						hMpsBrewLib_DrawSprite( spSutePaiJicya, Bbit+1, -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X+D_PAI_YOKO_SMALL_SIZE_W+D_PAI_TATE_SMALL_SIZE_W, D_JICYA_TEHAI_Y+_y_of011);
						break;
					}
				}
				break;
		} // switch( Hbit )
	} // for
#else
	// フーロ牌があるかチェック
	Fr = gsPlayerWork[House].byFhcnt;

	if( Fr == 0 ) return;							// フーロ牌が無ければ。
	if( Fr > 4 ) Fr = 4;

	// フーロ牌の情報取得
	for( i= 0; i< Fr; i++ ) {
		Bbit = gsPlayerWork[House].byFrhai[i];		// フーロ牌情報取得。

		//Frに4以上が入る不具合発生のため暫定的にここで処理を入れる 20051202
		if(Bbit == 0)	return;

		FrPos= gsPlayerWork[House].byFrpos[i];		// 鳴き場所取得。

		Hbit = Bbit & 0xC0;							// 上位ビットの取得。
		switch( Hbit ) {
			// 明カン(自家)
			case D_MINKAN: {
				SBit = Bbit & 0x3F;					// 下位ビットの取得
				furoOffX = 0;
				furoOffBaseX = 0;
				{
					if( FrPos==1) {
						// 横になる牌。
						MpsBrewLib_DrawSprite( spPaiShimocya[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+0,D_AGARI_FR_DRAW_BASE_X-8);
						MpsBrewLib_DrawSprite( spPaiShimocya[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+0,D_AGARI_FR_DRAW_BASE_X+3);
						// 自家通常牌。
						MpsBrewLib_DrawSprite( spPaiJicyaSmall[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+13,D_AGARI_FR_DRAW_BASE_X);
						MpsBrewLib_DrawSprite( spPaiJicyaSmall[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+24,D_AGARI_FR_DRAW_BASE_X);
					}
					if( FrPos==2 ) {
						// 自家通常牌。
						MpsBrewLib_DrawSprite( spPaiJicyaSmall[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+0,D_AGARI_FR_DRAW_BASE_X);
						// 横になる牌。
						MpsBrewLib_DrawSprite( spPaiShimocya[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+11,D_AGARI_FR_DRAW_BASE_X-8);
						MpsBrewLib_DrawSprite( spPaiShimocya[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+11,D_AGARI_FR_DRAW_BASE_X+3);
						// 自家通常牌。
						MpsBrewLib_DrawSprite( spPaiJicyaSmall[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+24,D_AGARI_FR_DRAW_BASE_X);
					}
					if( FrPos==3) {
						// 自家通常牌。
						MpsBrewLib_DrawSprite( spPaiJicyaSmall[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+0,D_AGARI_FR_DRAW_BASE_X);
						MpsBrewLib_DrawSprite( spPaiJicyaSmall[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+11,D_AGARI_FR_DRAW_BASE_X);
						// 横になる牌。
						MpsBrewLib_DrawSprite( spPaiShimocya[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+22,D_AGARI_FR_DRAW_BASE_X-8);
						MpsBrewLib_DrawSprite( spPaiShimocya[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+furoOffX)+22,D_AGARI_FR_DRAW_BASE_X+3);
					}
				}
				break;
			}

			// 暗カン(両端を裏にして表示)
			case D_ANKAN: {
				SBit = Bbit & 0x3F;					// 下位ビットの取得

				{
					MpsBrewLib_DrawSprite( spPaiJicyaSmall[PAI_URA], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+(0*D_PAI_TATE_SMALL_SIZE_W)+0),D_AGARI_FR_DRAW_BASE_X);
					MpsBrewLib_DrawSprite( spPaiJicyaSmall[PAI_URA], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+(2*D_PAI_TATE_SMALL_SIZE_W)+0)+3,D_AGARI_FR_DRAW_BASE_X);
					MpsBrewLib_DrawSprite( spPaiShimocya[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+(1*D_PAI_TATE_SMALL_SIZE_W)-0),D_AGARI_FR_DRAW_BASE_X-8);
					MpsBrewLib_DrawSprite( spPaiShimocya[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+(D_JI_FH_BASE_X+(1*D_PAI_TATE_SMALL_SIZE_W)-0),D_AGARI_FR_DRAW_BASE_X+3);
				}
				break;
			}

			// ポン
			case D_PON: {
				SBit = Bbit & 0x3F;					// 下位ビットの取得
				furoOffX = 0;

				for( j=1 ; j<=3 ; j++ ) {
					if( j == FrPos ) {
						// 左・中心・右のどこに置くか
						MpsBrewLib_DrawSprite( spPaiShimocya[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X+furoOffX, D_AGARI_FR_DRAW_BASE_X+3);
						furoOffX = furoOffX+D_PAI_YOKO_SMALL_SIZE_W;
						continue;
					}
					MpsBrewLib_DrawSprite( spPaiJicyaSmall[SBit], -(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X+furoOffX, D_AGARI_FR_DRAW_BASE_X);
					furoOffX = furoOffX+D_PAI_TATE_SMALL_SIZE_W;
				}
				break;
			}

			// チー
			default: {
				// チーの鳴き牌処理
				switch( FrPos ) {
					case 0: {
						MpsBrewLib_DrawSprite( spPaiShimocya[Bbit+0],	-(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X, D_AGARI_FR_DRAW_BASE_X+3);
						MpsBrewLib_DrawSprite( spPaiJicyaSmall[Bbit+1],-(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X+D_PAI_YOKO_SMALL_SIZE_W, D_AGARI_FR_DRAW_BASE_X);
						MpsBrewLib_DrawSprite( spPaiJicyaSmall[Bbit+2],-(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X+D_PAI_YOKO_SMALL_SIZE_W+D_PAI_TATE_SMALL_SIZE_W, D_AGARI_FR_DRAW_BASE_X);
						break;
					}
					case 1: {
						MpsBrewLib_DrawSprite( spPaiShimocya[Bbit+1],	-(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X, D_AGARI_FR_DRAW_BASE_X+3);
						MpsBrewLib_DrawSprite( spPaiJicyaSmall[Bbit+0],-(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X+D_PAI_YOKO_SMALL_SIZE_W, D_AGARI_FR_DRAW_BASE_X);
						MpsBrewLib_DrawSprite( spPaiJicyaSmall[Bbit+2],-(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X+D_PAI_YOKO_SMALL_SIZE_W+D_PAI_TATE_SMALL_SIZE_W, D_AGARI_FR_DRAW_BASE_X);
						break;
					}
					case 2: {
						MpsBrewLib_DrawSprite( spPaiShimocya[Bbit+2],	-(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X, D_AGARI_FR_DRAW_BASE_X+3);
						if( Bbit == 32 || Bbit == 31 )
							break;
						else
							MpsBrewLib_DrawSprite( spPaiJicyaSmall[Bbit+0],-(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X+D_PAI_YOKO_SMALL_SIZE_W, D_AGARI_FR_DRAW_BASE_X);
						MpsBrewLib_DrawSprite( spPaiJicyaSmall[Bbit+1],-(i*D_PAI_TOTAL_SIZE+furoOffBaseX)+D_JI_FH_BASE_X+D_PAI_YOKO_SMALL_SIZE_W+D_PAI_TATE_SMALL_SIZE_W, D_AGARI_FR_DRAW_BASE_X);
						break;
					}
					default:
						//DBGPRINTF("");
						break;
				}
				break;
			}
		} // switch( Hbit )
	} //
//	return;
#endif
#endif //-*todo:描画保留
}

/*===========================================================================
	ゲーム後のテンパイ・ノーテン・流し満貫 描画
===========================================================================*/
// 第１引数：たらい回しポインタ。
// 第２引数：
public void MJ_TenpaiDraw(/*MahJongRally* pMe*/)
{
	DebLog("//-*MJ:GameDraw.cs:MJ_TenpaiDraw( )");
#if false //-*todo:描画保留
	short[]	MenuPosX	 = {  50,	75,  50,  20};
	short[]	MenuPosY	 = { 190, 135,	20,  78};
	short[]	FacePosX	  = {  55,	80,  55,  25};
	short[]	FacePosY	  = { 194, 139,  24,  82};
	short[]	TenpaiPosX = { 105, 130, 105,  75};
	short[]	TenpaiPosY = { 205, 150,	35,  93};
	short				Seki, i, Len, posX;
//	String					szText;		//AECHAR	szText[10+1];
	String					str;		//char	str[10+1];
	SpriteInfo				sprite_;
	byte					is_yaoch_ = 0x00;		//0422

	//	  2
	// 3	 1
	//	  0
	//	(自分)
	for( i = 0; i < MJDefine.MAX_TABLE_MEMBER2; ++i )	//0422	//MAX_TABLE_MEMBER
		if( is_yaoch[i] == 0x01 ) {				//0422
			is_yaoch_ = 0x01;					//0422
			break;
		}

	if( is_yaoch_ == 0x01 ) {
		for ( i = 0 ; i < MJDefine.MAX_TABLE_MEMBER2 ; i++ ) {	//MAX_TABLE_MEMBER (4)
			sprite_ = null;
//			MEMSET(szText, 0, sizeof(szText));

			// 描画の場所を取得。
			Seki = (short)((game_player+i) % 4);

			if( is_yaoch[Seki] == 0x01 ) {
#if	Rule_2P
				mMpsBrewLib_DrawSprite( spGame[D_NAKI02_03], 40,  177- i* 125 );	//流し満貫
//#if DEBUG
//Debug.out("流し満貫: "+ (int)i);
//#endif
#else
				if ( gsTableData[0].sMemData[Seki].byMember < MAX_COMP_CHARACTER )		//0422
					sprite_ = spChar[gsTableData[0].sMemData[Seki].byMember];			//0422

				if ( WhichDrawMode == D_DRAW_NETWORK_MODE )
					// 通信対戦でプレイヤーの顔があれば入れ替え
					if ( (byte)gNetTable_NetChar[Seki].Flag != (byte)0xFF ) {
						sprite_ = gNetTable_NetChar[Seki].spriteChar;
						if ( sprite_.pIImage == NULL )
							if ( gNetTable_NetChar[Seki].CharPicNo < MAX_COMP_CHARACTER )
								sprite_ = spChar[gNetTable_NetChar[Seki].CharPicNo];
					}

				// メニュー描画
				MJ_DrawMenu(MenuPosX[i], MenuPosY[i], 7, 2, NORMAL_MENU);

				// 幺九振切表示 (流し満貫)
				str= "幺九振切\0\0";	//MEMCPY(str, "幺九振切\0\0", sizeof("幺九振切\0\0"));
				Len = (int16_t)MJ_GetStrLen( str );	// 長さ取得

				posX = (short)(TenpaiPosX[i]+((90-Len)/2));	//w21t ? (TenpaiPosX[i]+((90-Len)/2)) : (TenpaiPosX[i]+((80-Len)/2));	// 表示位置算出
				MahJongRally_Msg_Line(str, posX, TenpaiPosY[i], SYS_FONT_WHITE);
				// キャラクターの顔描画
				DrawCharacterFace( sprite_, FacePosX[i],FacePosY[i]);
#endif
			}
		}
	} else {
#if	Rule_2P
		int	tenpai_all= 0;

		for ( i = 0 ; i < MJDefine.MAX_TABLE_MEMBER2; i++ )
			if ( gsPlayerWork[i].byTenpai!= 0 )
				tenpai_all++;

		if ( tenpai_all == 0 ) {	//全員ノーテン
			mMpsBrewLib_DrawSprite( spGame[D_NAKI02_02], 40,  52 );
			mMpsBrewLib_DrawSprite( spGame[D_NAKI02_02], 40, 173 );
		}
		if ( tenpai_all == 1 ) {	//1人テンパイ
			if ( gsPlayerWork[0].byTenpai!= 0 ) {
				mMpsBrewLib_DrawSprite( spGame[D_NAKI02_02], 40,  52 );
				mMpsBrewLib_DrawSprite( spGame[D_NAKI02_01], 40, 173 );
			} else {
				mMpsBrewLib_DrawSprite( spGame[D_NAKI02_01], 40,  52 );
				mMpsBrewLib_DrawSprite( spGame[D_NAKI02_02], 40, 173 );
			}
		}
		if ( tenpai_all == 2 ) {	//全員テンパイ
			mMpsBrewLib_DrawSprite( spGame[D_NAKI02_01], 40,  52 );
			mMpsBrewLib_DrawSprite( spGame[D_NAKI02_01], 40, 173 );
		}
#else
		for ( i = 0 ; i < MAX_TABLE_MEMBER ; i++ ) {	//4
			sprite_ = NULL;
//			MEMSET(szText, 0, sizeof(szText));

			// 描画の場所を取得。
			Seki = (short)((game_player+i) % 4);

			if ( gsTableData[0].sMemData[Seki].byMember < MAX_COMP_CHARACTER )		//0422
				sprite_ = spChar[gsTableData[0].sMemData[Seki].byMember];			//0422

			if ( WhichDrawMode == D_DRAW_NETWORK_MODE ) {
				// 通信対戦でプレイヤーの顔があれば入れ替え
				if ( (byte)gNetTable_NetChar[Seki].Flag != (byte)0xFF ) {
					sprite_ = gNetTable_NetChar[Seki].spriteChar;
					if ( sprite_.pIImage == NULL ) {
						if ( gNetTable_NetChar[Seki].CharPicNo < MAX_COMP_CHARACTER ) {
							sprite_ = spChar[gNetTable_NetChar[Seki].CharPicNo];
						}
					}
				}
			}

			// メニュー描画
			MJ_DrawMenu(MenuPosX[i], MenuPosY[i], 7, 2, NORMAL_MENU);

			// テンパイかノーテンかを描画
			if ( gsPlayerWork[Seki].byTenpai !=0 ) {
				str= "テンパイ\0\0";		//MEMCPY(str, "テンパイ\0\0", sizeof("テンパイ\0\0"));	// 2006/02/25 No.145
				Len = (int16_t)MJ_GetStrLen( str );	// 長さ取得
			}else {
				str= "ノーテン\0\0";		//MEMCPY(str, "ノーテン\0\0", sizeof("ノーテン\0\0"));	// 2006/02/25 No.145
				Len = (int16_t)MJ_GetStrLen( str);		// 長さ取得
			}

			posX = (short)(TenpaiPosX[i]+((90-Len)/2));		//w21t ? (TenpaiPosX[i]+((90-Len)/2)) : (TenpaiPosX[i]+((80-Len)/2));	// 表示位置算出

			MahJongRally_Msg_Line(str, posX, TenpaiPosY[i], SYS_FONT_WHITE);

			// キャラクターの顔描画
			DrawCharacterFace( sprite_, FacePosX[i],FacePosY[i]);
		}
#endif
	}
//	return;
#endif //-*todo:描画保留
}

/**
 * @brief 特別 流局(仮)描画
 * 流局 描画
 */
#if	Rule_2P
#else
	public const int		HEIGHT			30		// バーの高さ

	// 色
	public const int		COLOR_BACK		MAKE_RGB( 82,  48,	 0)		// 背景
	public const int		COLOR_FRAME		MAKE_RGB(255, 158,	33)		// 背景枠
	public const int		COLOR_TEXT		MAKE_RGB(255, 255, 255)		// 文字
	public const int		COLOR_SHADOW	MAKE_RGB( 49,  24,	 0)		// 文字影
public AEERect		MJ_RyukyokuDraw_rect_= new AEERect();	//0508mt
#endif
public void MJ_RyukyokuDraw(/*MahJongRally* pMe*/)
{
	DebLog("//-*MJ:GameDraw.cs:MJ_RyukyokuDraw( )");
#if false //-*todo:描画保留
#if	Rule_2P
	if( Pinch!= SP_YAOCHU)
		mMpsBrewLib_DrawSprite( spGame[D_NAKI02_00], 40, 90 );		//流局
#else
//	enum {
//		HEIGHT = 30,	// バーの高さ
//
//		// 色
//		COLOR_BACK		=	MAKE_RGB( 82,  48,	 0),	// 背景
//		COLOR_FRAME		=	MAKE_RGB(255, 158,	33),	// 背景枠
//		COLOR_TEXT		=	MAKE_RGB(255, 255, 255),	// 文字
//		COLOR_SHADOW	=	MAKE_RGB( 49,  24,	 0),	// 文字影
//	};
	int		y_ = ((m_screenHeight - HEIGHT - SOFTMENU_HEIGHT) / 2);

	// バー
	{
		AEERect		rect_= MJ_RyukyokuDraw_rect_;//new AEERect();//0508mt

		SETAEERECT(rect_, 0, y_, m_screenWidth, HEIGHT);
		IDISPLAY_DrawRect(GETMYDISPLAY(), rect_, COLOR_BACK, COLOR_BACK, IDF_RECT_FILL);

		SETAEERECT(rect_, 0, y_ - 3, m_screenWidth-1 , 3);
		IDISPLAY_DrawRect(GETMYDISPLAY(), rect_, COLOR_SHADOW, COLOR_FRAME, IDF_RECT_FILL | IDF_RECT_FRAME);

		SETAEERECT(rect_, 0, y_ + HEIGHT, m_screenWidth-1 , 3);
		IDISPLAY_DrawRect(GETMYDISPLAY(), rect_, COLOR_SHADOW, COLOR_FRAME, IDF_RECT_FILL | IDF_RECT_FRAME);
	}

	// 文字
	{
		final String		text[] = { "和了", "流局", "四開槓", "四風連打", "三家和", "四人立直", "九種九牌" };
//		String		szText;		//AECHAR	szText[20]={0};

//xxxx	y_ += SMALLFONT_OFFSET_MIDDLE(HEIGHT);

//		STREXPAND((const byte*)text[Pinch],STRLEN(text[Pinch]),szText,sizeof(szText));
		int textOffs = 8;
		MJ_Msg_CenterLine(	text[Pinch], 0, y_+4+textOffs,(RGBVAL)COLOR_SHADOW);	// 2006/02/24 要望No.118	//szText
		MJ_Msg_CenterLine(	text[Pinch], 0, y_+textOffs, (RGBVAL)COLOR_TEXT);	// 2006/02/24 要望No.118	//szText
	}

	return;
#endif
#endif //-*todo:描画保留
}

//0507mt ->
public int	draw_time_index_ = 0;
public int pre_draw_time_index_ = 0;
public void MJ_GameCountDown()
{
	DebLog("//-*MJ:GameDraw.cs:MJ_GameCountDown( )");
#if false //-*todo:描画保留
	if( !IS_NETWORK_MODE )	return;//0508mt

	int draw_time_ = 0;

	if( gTsumoTimeOut > 0 ) {
		draw_time_ =  (int)(( gTsumoTimeOut / 1000 ) + 1);
	} else
		if( gNakiTimeOut > 0 )
			draw_time_ =  (int)(( gNakiTimeOut / 1000 ) + 1);

	if( (m_DaiuchiFlag) != D_ONLINE_OPERATOR_MANUAL )
		return;

	switch( draw_time_ ) {
		case 0 : draw_time_index_ = 3;	break;
		case 1 : draw_time_index_ = 0;	break;
		case 2 : draw_time_index_ = 1;	break;
		case 3 : draw_time_index_ = 2;	break;
		default : break;
	}
	if( draw_time_index_ != pre_draw_time_index_ ) {
		pre_draw_time_index_ = draw_time_index_;
		paintF = true;
	}
#endif //-*todo:描画保留
}
/**
 * @brief カウントダウン描画
 */
public void MJ_GameCountDownDraw( /*MahJongRally *pMe*/)
{
	DebLog("//-*MJ:GameDraw.cs:MJ_GameCountDownDraw( )");
#if false //-*todo:描画保留
	if( draw_time_index_ < D_COUNT_DOWN_MAX )
		MpsBrewLib_DrawSprite( spCount[draw_time_index_], 180, 193 );

//	return;
#endif //-*todo:描画保留
}
//-*********************MJ_GameDraw.j
}
