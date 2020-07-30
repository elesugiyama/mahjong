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
// MJ_TalkDraw.j
//-*****************
public partial class MahjongBase : SceneBase {
#region UNITY_ORIGINAL
/// <summary>
/// 共用 キャラクターセリフ描画
/// D_COMMON_TALK
/// </summary>
public int GetDCommonTalk(){	return (gTalkType[gTalkNo] - 84);}
#endregion //-*UNITY_ORIGINAL

#if true //-*todo:描画関連検討中
//#include "MahJongRally.h"	// Module interface definitions

//#include	"K00_MES.h"
//#include	"K01_MES.h"
//#include	"K02_MES.h"
//#include	"K03_MES.h"
//#include	"K04_MES.h"
//#include	"K05_MES.h"
//#include	"K06_MES.h"
//#include	"K07_MES.h"
//#include	"K08_MES.h"
//#include	"K09_MES.h"
//#include	"K10_MES.h"
//#include	"K11_MES.h"
//#include	"K12_MES.h"
//#include	"K13_MES.h"
//#include	"K14_MES.h"
//#include	"K15_MES.h"
//#include	"C00_MES.h"

/****************************************************************/
/*							define								*/
/****************************************************************/

private ushort[]	baseX = {  35,  70, 35,	 5 };
private ushort[]	baseY = { 160, 100,	5, 100 };

/****************************************************************/
/*						プロトタイプ宣言						*/
/****************************************************************/

// キャラクターセリフ描画
//void MJ_CharTalkDraw( /*MahJongRally * pMe,*/ const char TALK_DATA[][3][D_CHARCTER_TALK_TABLE_SIZE] );
//void MJ_CommonTalkDraw( /*MahJongRally * pMe,*/ const char TALK_DATA[][9] );

/****************************************************************/
/*																*/
/*						対話描画								*/
/*																*/
/****************************************************************/
public void MJ_TalkDraw(/*MahJongRally * pMe*/)
{
#if true //-*todo:描画関連検討中
	#region UNITY_ORIGINAL
	if ( gTalkFlag == 1 ) {
		if ( gTalkWait[gTalkNo] > 0 ) {
			// どの家が喋っているか（席の家の人がしゃべっているか [0:自分]）
			int	seki = (gTalkHouse[gTalkNo] + 4 - game_player) % 4;

			gTalkWait[gTalkNo]--;	//gTalkWait[gTalkNo] -= uptimems_perfrm;

			SubMj.mj_wait = gTalkWait[gTalkNo];
			if(seki >= m_callObjs.Count && m_callObjs[seki] != null)return;
			m_callObjs[seki].setCall(GetDCommonTalk());
		} else {
			paintF= true;	paintT= 0;
				// > 2006/02/23 リーチSEタイミング正規化
			gTalkNo = (byte)((gTalkNo + 1) % MJDefine.D_TALK_TABLE_MAX);
			if ( gTalkCnt == gTalkNo ) {
				gTalkFlag = 0;
			}
			RestCallDraw();
		}
	}
	#endregion //-*UNITY_ORIGINAL
#else	
	// 途中キャンセル
	if ( D_ONEPUSH_INPUT_SELECT ) {
		//> 2006/03/03 No.159 立直時のメッセージが消えてしまう。
		switch( gTalkType[gTalkNo] ) {
			case 84 : // 立直
			case 85 : // ポン
			case 86 : // チー
			case 87 : // カン
			case 88 : // ツモ
			case 89 : // ロン
			case 90 : // 九種九牌
			case 91 : // 四風連打
			case 92 : // 四開槓
			case 93 : // テンパイ
				// キー入力を無視する
				break;
			default :
				SubMj.mj_wait = gTalkWait[gTalkNo] = paintT= 0;
			break;
		}
	}

	if ( gTalkFlag == 1 ) {
		if ( gTalkWait[gTalkNo] > 0 ) {
			gTalkWait[gTalkNo]--;	//gTalkWait[gTalkNo] -= uptimems_perfrm;

			SubMj.mj_wait = gTalkWait[gTalkNo];

			// TRACE("mj_wait=%d",mj_wait);

			// gTalkHouse[gTalkNo]で位置を決定すること
//			if( paintF)
			switch ( gTalkChar[gTalkNo] ) {
			case 0:
				MJ_CharTalkDraw(__TALK00_DATA);
				break;
			case 1:
				MJ_CharTalkDraw(__TALK01_DATA);
				break;
			case 2:
				MJ_CharTalkDraw(__TALK02_DATA);
				break;
			case 3:
				MJ_CharTalkDraw(__TALK03_DATA);
				break;
			case 4:
				MJ_CharTalkDraw(__TALK04_DATA);
				break;
			case 5:
				MJ_CharTalkDraw(__TALK05_DATA);
				break;
			case 6:
				MJ_CharTalkDraw(__TALK06_DATA);
				break;
			case 7:
				MJ_CharTalkDraw(__TALK07_DATA);
				break;
			case 8:
				MJ_CharTalkDraw(__TALK08_DATA);
				break;
			case 9:
				MJ_CharTalkDraw(__TALK09_DATA);
				break;
			case 10:
				MJ_CharTalkDraw(__TALK10_DATA);
				break;
			case 11:
				MJ_CharTalkDraw(__TALK11_DATA);
				break;
			case 12:
				MJ_CharTalkDraw(__TALK12_DATA);
				break;
			case 13:
				MJ_CharTalkDraw(__TALK13_DATA);
				break;
			case 14:
				MJ_CharTalkDraw(__TALK14_DATA);
				break;
			case 15:
				MJ_CharTalkDraw(__TALK15_DATA);
				break;
			default:
				MJ_CharTalkDraw(__TALK15_DATA);	// ダミーデータを渡しておく
				break;
			}
		} else {
			paintF= true;	paintT= 0;
				// > 2006/02/23 リーチSEタイミング正規化
			gTalkNo = (byte)((gTalkNo + 1) % MJDefine.D_TALK_TABLE_MAX);
			if ( gTalkCnt == gTalkNo ) {
				gTalkFlag = 0;
			} else
			if ( gTalkType[gTalkNo] == (byte)84 ) {
				#if false //-*todo:サウンド
				// 続くセリフが「リーチ」ならSE再生
				WaveSeOnPlay(Order, OP.RICHI);
				#endif //-*todo:サウンド
			}
			// < 2006/02/23
		}
	}
//	return;
#endif //-*todo:描画関連検討中

}

/********************************************************/
/*														*/
/*				キャラクターセリフ描画					*/
/*														*/
/********************************************************/
public void MJ_CharTalkDraw(/*MahJongRally * pMe,*/ String[][] TALK_DATA)		//[3][D_CHARCTER_TALK_TABLE_SIZE]
{
#if false //-*todo:描画関連検討中

	short[]	m_tbl = {
		0,		// (01) 自分がドラのポン序盤・中盤
		1,		// (02) 自分がドラのポン終盤
		2,		// (03) 他人がドラのポン鳴かせた者への非難
		3,		// (04) 他人がドラのポン鳴いた者への羨望、落胆
		4,		// (05) 他人がドラの槓
		5,		// (06) 他人が槓した牌がドラになる
		6,		// (07) 自分が第一副落・序盤
		7,		// (08) 自分が第一副落・中盤
		8,		// (09) 自分が第一副落・終盤
		9,		// (10) 他人の第一副落・序盤
		10,		// (11) 他人の第一副落・中盤
		11,		// (12) 他人の第一副落・終盤
		12,		// (13) 他人の第三副落
		13,		// (14) 他人の第四副落
		14,		// (15) 自分が第四副落・金ｹｲ独立設定時
		15,		// (16) 自分が第四副落・非設定時
		16,		// (17) 他人の一発を消すとき
		17,		// (18) 他人に一発を消された
		18,		// (19) プレイヤーに一発を消された
		19,		// (20) 他人の立直中ドラを切る時（完全安牌時は除く）
		-1,		// (21) 削る
		-1,		// (22) 削る
		-1,		// (23) 削る
		-1,		// (24) 削る
		20,		// (25) 自分がダブル立直
		21,		// (26) 自分が追っかけ立直
		22,		// (27) 自分が３人目立直
		23,		// (28) 自分がその他の立直・序盤
		24,		// (29) 自分がその他の立直・中盤
		25,		// (30) 自分がその他の立直・終盤
		26,		// (31) 他家にダブル立直された
		27,		// (32) プレイヤーに立直された
		28,		// (33) 削る
		28,		// (34) 削る
		28,		// (35) 追っかけ立直された・その他
		29,		// (36) 他の３人に立直された・親
		30,		// (37) 他の３人に立直された・子
		31,		// (38) 自分の自摸和立直一発
		32,		// (39) 自分の自摸和２０００未満
		33,		// (40) 自分の自摸和２０００以上７７００未満
		34,		// (41) 自分の自摸和７７００以上１６０００未満
		35,		// (42) 自分の自摸和16000～役満未満
		36,		// (43)自分の自摸和嶺上開花
		37,		// (44)自分の自摸和十三不塔
		38,		// (45)自分の自摸和天和・地和
		39,		// (46)自分の自摸和他の役満
		40,		// (47)他人のツモ満貫以上
		41,		// (48)他人のツモ役満
		42,		// (49)他人のツモ役満で責任払の者へ
		43,		// (50)他人の栄和満貫以上
		44,		// (51)他人の栄和役満
		45,		// (52)他人の栄和役満で責任払の者へ
		46,		// (53)自分の和了（ツモ＆栄和）高め
		47,		// (54)自分の和了（ツモ＆栄和）安目
		48,		// (55)海底・河底の和了
		49,		// (56)放銃一役
		50,		// (57)放銃二役以上満貫未満
		51,		// (58)放銃満貫以上
		52,		// (59)放銃役満
		53,		// (60)放銃ドラの放銃
		54,		// (61)放銃立直合戦に負けた場合
		55,		// (62)放銃搶槓
		56,		// (63)河底の放銃
		57,		// (64)明槓させた相手が嶺上開花
		58,		// (65)流局１人聴牌（罰符設定時のみ）
		59,		// (66)流局１人不聴（罰符設定時のみ）
		60,		// (67)流局全員聴牌（親の台詞）
		61,		// (68)九種九牌倒牌
		62,		// (69)４人立直（４人目）
		63,		// (70)４人立直（他の人）
		64,		// (71)四槓算了になる槓をした
		65,		// (72)四風子連打になる牌を捨てた
		66,		// (73)二役縛り突入時（親の台詞）
		67,		// (74)八連荘の対象局突入時親の台詞
		68,		// (75)南入時（その時点のトップの台詞）
		69,		// (76)西入・北入・返り東…時（その時点のラスの台詞）
		70,		// (77)トップ目がいないとき、親の台詞(オーラス局開始時）
		-1,		// (78)なし
		-1,		// (79)なし
		-1,		// (80)僅差のトップが親のとき、(オーラス局開始時）親の台詞
		-1,		// (81)大差のトップが親のとき、(オーラス局開始時）親の台詞
		-1,		// (82)僅差のトップが子のとき、(オーラス局開始時）親の台詞
		-1,		// (83)大差のトップが子のとき、トップの台詞(オーラス局開始時）
		-1,		// (84)プレイヤーからの差し馬申し込み
		-1,		// (85)なし
		-1,		// (86)なし
		-1,		// (87)なし
		-1,		// (88)なし
		-1,		// (89)なし
		-1,		// (90)なし
		-1,		// (91)なし
		-1,		// (92)なし
		-1,		// (93)なし
		-1,		// (94)なし
		-1,		// (95)なし
		-1,		// (96)なし
		-1,		// (97)包になった者への一言
		-1,		// (98)自分の流し満貫達成
		-1,		// (99)他人の流し満貫に一言
		-1,		// (100)ダブロンで頭はねした
		-1,		// (101)ダブロンで頭はねされた
		-1,		// (102)トリロンで頭はねした
		-1,		// (103)トリロンで頭はねされた
	};
	int	seki;
//	paintF= true;	//aaaa
	// どの家が喋っているか（席の家の人がしゃべっているか [0:自分]）
	seki = (gTalkHouse[gTalkNo] + 4 - game_player) % 4;

	ASSERT(seki >= 0);

	{
		int	msgnum = gTalkType[gTalkNo];

		if ( (msgnum < 77) && (opt_game[(int)GOPT.TLK] == (byte)GOPT_TLK.ON) ) {
			msgnum = m_tbl[msgnum];	// DS版よりセリフを削っているので、テーブルに則って番号変更

			if ( WhichDrawMode == D_DRAW_NETWORK_MODE ) {
				// ネットワークモード且つキャラクターが代打ちならセリフを描画しない
				SubMj.mj_wait = gTalkWait[gTalkNo] = paintT= 0;
			} else
			if ( msgnum >= 0 ) {
				// 会話ON時にも、通信対戦時のCOM（代理プレイヤー）は会話OFF
				int	yoffset = 0, i;

				for ( i = 0 ; i < 3 ; i++ ) {
					int	len = TALK_DATA[msgnum][i].Length;	//int	len = STRLEN(TALK_DATA[msgnum][i]);

					if ( len != 0 ) {
						yoffset++;
					} else
					if ( i == 0 )  {
						// セリフが削られていた場合
						SubMj.mj_wait = gTalkWait[gTalkNo] = paintT= 0;
					}
				}

				if ( yoffset != 0 ) {
					//0511mt if( paintT> 2)	return;		//aaaa
					paintF= true;	paintT++;
					SpriteInfo	sprite_ = null;
					String name_ = gpsTableData.sMemData[gTalkHouse[gTalkNo]].NickName;

					if( gTalkChar[gTalkNo] < MJDefine.MAX_COMP_CHARACTER )
						sprite_ = spChar[gTalkChar[gTalkNo]];

					// メニューフレーム描画
					MJ_DrawMenu(baseX[seki], baseY[seki], 8, yoffset + 3, YELLOW_MENU);

					// キャラクターの名前取得
					if ( WhichDrawMode == D_DRAW_NETWORK_MODE ) {
						// 通信対戦でプレイヤーの顔があれば入れ替え
						if ( (byte)gNetTable_NetChar[gTalkHouse[gTalkNo]].Flag != (byte)0xFF ) {
							sprite_ = gNetTable_NetChar[gTalkHouse[gTalkNo]].spriteChar;
							if( sprite_.pIImage == NULL )
								if( gNetTable_NetChar[gTalkHouse[gTalkNo]].CharPicNo < MJDefine.MAX_COMP_CHARACTER )
									sprite_ = spChar[gNetTable_NetChar[gTalkHouse[gTalkNo]].CharPicNo];
						}
					}

					// 顔描画
					DrawCharacterFace(sprite_, baseX[seki] + 5, baseY[seki] + 5);

					{
//						AECHAR	szText[27] = { 0 };

						// 名前描画
//						STREXPAND((const byte*)name_, D_NICK_NAME_MAX, szText, sizeof(szText));	// 2006/02/05 No.706
						MahJongRally_Msg_Line(name_, baseX[seki] + 50, baseY[seki] + 15, SYS_FONT_BLACK);	//szText

						// セリフ描画
						for ( i = 0 ; i < 3 ; i++ ) {
//							STREXPAND(TALK_DATA[msgnum][i], 20, szText, sizeof(szText));
							MahJongRally_Msg_Line(TALK_DATA[msgnum][i], baseX[seki] + 5, baseY[seki] + 50 + (i * 20), SYS_FONT_BLACK);	//szText
						}
					}
				} //0511mt else		paintT= 0;
			}
		} else
		if ( (msgnum > 83) && (msgnum < 97) )
			// ポンやロンなどの共有会話の表示
			MJ_CommonTalkDraw(__commonTalkTbl);
	}
//	return;
#endif //-*todo:描画関連検討中
}

/********************************************************/
/*				共用 キャラクターセリフ描画				*/
/********************************************************/
private byte	D_COMMON_TALK(){return (byte)(gTalkType[gTalkNo] - 84);}

public void MJ_CommonTalkDraw(/*MahJongRally* pMe,*/ String[] TALK_DATA)
{
	int	seki, yoffset = 0;
#if false //-*todo:描画関連検討中

//	paintF= true;		//aaaa
	// どの家が喋っているか
	seki = (gTalkHouse[gTalkNo] + 4 - game_player) % 4;

#if	Rule_2P
#else
	if ( TALK_DATA[D_COMMON_TALK].length() != 0 )	//if ( STRLEN(TALK_DATA[D_COMMON_TALK]) )
#endif
		yoffset++;

	ASSERT(yoffset);

	if ( yoffset != 0 ) {
		//0511mt  if( paintT> 2)	return;		//aaaa
		paintF= true;	paintT++;

		// メニューフレーム描画
#if	Rule_2P
	{
		int		scr= 0;
		int[]		nakiData00 = {
			D_NAKI01_01,		//*リーチ
			D_NAKI00_01,		//ポン
			D_NAKI00_00,		//チー
			D_NAKI00_02,		//カン
			D_NAKI01_00,		//ツモ
			D_NAKI01_02,		//*ロン
//			D_NAKI02_00,		//流局 ("九種九牌")
//			D_NAKI02_00,		//流局 ("四風連打")
//			D_NAKI02_00,		//流局 ("四開槓")
		};
//	"テンパイ",	"ノーテン",
		int[][]		nakiData10 = new int[][]{
			new int[]{
				D_FACE_11- D_FACE_00,		//キャラクター表情（自分がリーチした場合）
				D_FACE_10- D_FACE_00,		//キャラクター表情（自分が鳴いた場合）
				D_FACE_10- D_FACE_00,		//キャラクター表情（自分が鳴いた場合）
				D_FACE_10- D_FACE_00,		//キャラクター表情（自分が鳴いた場合）
				D_FACE_12- D_FACE_00,		//キャラクター表情（自分がロンした場合）
				D_FACE_12- D_FACE_00,		//キャラクター表情（自分がロンした場合）
//				0, 0, 0,
			},
			new int[]{
				D_FACE_13- D_FACE_00,		//キャラクター表情（相手がリーチした場合）
				D_FACE_14- D_FACE_00,		//キャラクター表情（相手が鳴いた場合）
				D_FACE_14- D_FACE_00,		//キャラクター表情（相手が鳴いた場合）
				D_FACE_14- D_FACE_00,		//キャラクター表情（相手が鳴いた場合）
				D_FACE_15- D_FACE_00,		//キャラクター表情（相手がロンした場合）
				D_FACE_15- D_FACE_00,		//キャラクター表情（相手がロンした場合）
//				0, 0, 0,
			}
		};

		if(( D_COMMON_TALK>= 0) && ( D_COMMON_TALK< nakiData00.Length)) {
			FaceNum= nakiData10[seki & 1][D_COMMON_TALK];	//キャラクター表情
			//*********ウキウキ:表情操作************
			faceChangeCnt = FACECHANGETIME;
			//**************************************
			if(( seki== 1) && (( D_COMMON_TALK== 0) || ( D_COMMON_TALK== 5))) {
				// アニメーション:com リーチ & ロン
				scr= ((int)gTalkWait[gTalkNo]* 40) % 120;
				mMpsBrewLib_DrawSprite( spGame[D_NAKI03_00], -120+ scr, 59 );		//スクロール背景
				mMpsBrewLib_DrawSprite( spGame[D_NAKI03_00],    0+ scr, 59 );
				mMpsBrewLib_DrawSprite( spGame[D_NAKI03_00],  120+ scr, 59 );
				//*******ウキウキ:表情操作**********
				//*mMpsBrewLib_DrawSprite( spGame[D_NAKI03_01],         0, 60 );	//キャラ画像
				mMpsBrewLib_DrawSprite( spGame[D_CHAR_REACH],         0, 60 );		//キャラ立ち絵画像
				//**********************************
				//D_TALK_WAIT	(15*2)
				scr= abs((int)(Math.sin(Math.toRadians((gTalkWait[gTalkNo]* 360)/ D_TALK_WAIT))* gTalkWait[gTalkNo]));
				if( D_COMMON_TALK== 0)
					mMpsBrewLib_DrawSprite( spGame[D_NAKI03_02], 120, 84- scr );	//リーチ
				else
					mMpsBrewLib_DrawSprite( spGame[D_NAKI03_03], 120, 84- scr );	//ロン！
			} else {
				if( seki== 0) {
					mMpsBrewLib_DrawSprite( spGame[nakiData00[D_COMMON_TALK]],
						40, 90+ (int)gTalkWait[gTalkNo]* 0 );
					//"リーチ" 表示後に捨て牌音
					if(D_COMMON_TALK== 0)
						if( gTalkWait[gTalkNo]== 0)
							SeOnPlay( SE_TAHAI, BANK_SE);
				} else
					mMpsBrewLib_DrawSprite( spGame[nakiData00[D_COMMON_TALK]],
						40, 90- (int)gTalkWait[gTalkNo]* 0 );
			}
		} else {
#if	DEBUG
DebLog("MJ_CommonTalkDraw none");
#endif
			gTalkWait[gTalkNo]= 0;
		}
	}
#else
		SpriteInfo	sprite_ = NULL;
		String name_  = gsTableData[0].sMemData[gTalkHouse[gTalkNo]].NickName;	// 名前		//xxxx	gsTableData

		if( gTalkChar[gTalkNo] < MAX_COMP_CHARACTER )
			sprite_ = spChar[gTalkChar[gTalkNo]];

		MJ_DrawMenu(baseX[seki], baseY[seki], 8, yoffset + 3, YELLOW_MENU);

		if ( WhichDrawMode == D_DRAW_NETWORK_MODE ) {
			// 通信対戦でプレイヤーの顔があれば入れ替え
			if ( (byte)gNetTable_NetChar[gTalkHouse[gTalkNo]].Flag != (byte)0xFF ) {
				sprite_ = gNetTable_NetChar[gTalkHouse[gTalkNo]].spriteChar;
				if( sprite_.pIImage == NULL )
					if( gNetTable_NetChar[gTalkHouse[gTalkNo]].CharPicNo < MAX_COMP_CHARACTER )
						sprite_ = spChar[gNetTable_NetChar[gTalkHouse[gTalkNo]].CharPicNo];
			}
		}

		// 顔描画
		DrawCharacterFace(sprite_, baseX[seki] + 5, baseY[seki] + 5);
		{
//			AECHAR	szText[16] = { 0 };

			// 名前描画
//			STREXPAND((const byte*)name_, D_NICK_NAME_MAX + 1, szText, sizeof(szText));	// 2006/02/05 No.706
			MahJongRally_Msg_Line(name_, baseX[seki] + 50, baseY[seki] + 15, SYS_FONT_BLACK);	//szText

			// セリフ描画
			{
//				int	len_ = TALK_DATA[D_COMMON_TALK].length();	//int	len_ = STRLEN(TALK_DATA[D_COMMON_TALK]);

//				STREXPAND((const byte*)TALK_DATA[D_COMMON_TALK], len_, szText, sizeof(szText));
				MahJongRally_Msg_Line(TALK_DATA[D_COMMON_TALK], baseX[seki] + 5, baseY[seki] + 50, SYS_FONT_BLACK);	//szText
			}
		}
#endif
	} else
		paintT= 0;
//	return;
#endif //-*todo:描画関連検討中
}

//**************ウキウキ:表情操作*****************
public void FaceChange(){
//*表情を戻す
#if false //-*todo:描画関連検討中
	//*通常時
	FaceNum = 0;
	if(gsPlayerWork[1].bFrich != 0){
	//*キャラがリーチ済なら喜び優先
		FaceNum = D_FACE_13- D_FACE_00;
	}else if(gsPlayerWork[0].bFrich != 0){
	//*プレイヤーがリーチ済なら悲しみ
		FaceNum = D_FACE_11- D_FACE_00;
	}
#endif //-*todo:描画関連検討中
}
//************************************************
#endif //-*todo:描画関連検討中
}
