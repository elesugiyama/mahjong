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
// app_mode.j
//-*****************
public partial class MahjongBase : SceneBase {
//-------1---------2---------3---------4---------5---------6---------7---------8
//機能 : mode
//設計 :
//作成 :
//　　 :
//備考 :
//-------1---------2---------3---------4---------5---------6---------7---------8

//#include "MahJongRally.h"
//#include "MsgSystem.h"

//#ifdef _WIN32
//#include <stdio.h>
//#include <stdlib.h>
//#endif
//関数 : モード遷移
//引数 : pMe	: MahJongRally(IApplet)ポインター
//　　 : to_mode	: 遷移モード
//戻値 : なし
//作成 :
//備考 : なし
#if true //-*todo
// public void modeChange(/*MahJongRally *pMe,*/ int to_mode)
public void modeChange(/*MahJongRally *pMe,*/ INMJMODE to_mode)
{
#endif //-*todo:
	int		i;
#if true //-*todo:
	DebLog( ("######## MODE CHANGE ["+m_Mode+"]->["+to_mode+"] ########" ) );
#endif //-*todo:
	//「致命的エラー」に遷移後は、その他への遷移を禁止
	if( m_Mode == INMJMODE.D_FATAL_ERR_MODE ) {
		// ゲームモード初期化
		sGameData.byGameMode = MJDefine.GAMEMODE_NONE;

		if( (to_mode != INMJMODE.D_TITLE_MODE) )
			return;
	}
	//「エラー」に遷移後は、タイトルに戻るのみ
	if( m_Mode == INMJMODE.D_ERR_MODE ) {
		// ゲームモード初期化
		sGameData.byGameMode = MJDefine.GAMEMODE_NONE;

		if( (to_mode != INMJMODE.D_TITLE_MODE) && (to_mode != INMJMODE.D_FATAL_ERR_MODE) )
			return;
	}

	//「認証エラー」に遷移後は、タイトルに戻るのみ
	if( m_Mode == INMJMODE.D_SUBMIT_ERR_MODE ) {
		// ゲームモード初期化
		sGameData.byGameMode = MJDefine.GAMEMODE_NONE;

		if( (to_mode != INMJMODE.D_TITLE_MODE) && (to_mode != INMJMODE.D_FATAL_ERR_MODE) )
			return;
	}

	//「EFSエラー」に遷移後は、アプリ終了
	if( m_Mode == INMJMODE.D_EFS_ERR_MODE ) {
		// ゲームモード初期化
		sGameData.byGameMode = MJDefine.GAMEMODE_NONE;

		if( to_mode != INMJMODE.D_FATAL_ERR_MODE )
			return;
	}

	//
	if( m_Mode == INMJMODE.D_TIMER_DISCONNECT_MODE )
		if((to_mode != INMJMODE.D_TITLE_MODE)&&(to_mode != INMJMODE.D_ERR_MODE)&&(to_mode != INMJMODE.D_FATAL_ERR_MODE))
			return;

	//遷移処理
	switch(to_mode) {
//-**********todo:ここから必要物移植
		/************************************/
		/*	COM対戦	メニュー				*/
		/************************************/
		case (INMJMODE.D_COM_MENU): {
			/* ロジック				*/
			#if false //-*todo:保留要らない？
			ComMenuInit();
			#endif //-*todo:保留要らない？

			/* 初期化処理。			*/
//m			MJ_ComMenuDrawInit( );

			// 音楽変更＆再生
#if	Rule_2P
//			myCanvas.playBGM(SN_BGM06);	//麻雀1
#else
			// PlayMusic_BgmChange(D_SOUND_BGM_02);
			PlayMusic_BgmChange(D_SOUND_BGM_06);
#endif
			break;
		}
		/************************************/
		/*	COM対戦	フリー対戦				*/
		/************************************/
		case (INMJMODE.D_FREE_RULE_MODE): {
			// 描画前の初期化。
			#if false //-*todo:描画
			MJ_RuleSelectMainDrawInit( );
			#endif //-*todo:描画
			/* ロジック				*/
			RuleInit();
			break;
		}
		case (INMJMODE.D_FREE_RULE_EDIT_MODE): {
			/* ロジック				*/
			RuleEditInit();
			// ルール確認用のリソースのロード。
			#if false //-*todo:描画
			MJ_RuleSelectMainDrawInit( );
			#endif //-*todo:描画
			break;
		}
		case (INMJMODE.D_FREE_NPC_SELLECT_MODE): {
		#if false //-*todo:要らない気がする
			/* ロジック				*/
			CselInit();
			/* 描画					*/
			// motohashi -コメント外すの禁止-。
			//MJ_CharSelDrawInit( );
		#endif //-*todo:要らない気がする
			break;
		}
		case (INMJMODE.D_FREE_NPC_CHK_MODE): {
		#if false //-*todo:要らない気がする
			/* ロジック				*/
			CselSureInit();
			removeSoftkey();
			// 画像のロード。
			// motohashi -コメント外すの禁止-。
			// MJ_CharSelDrawInit( );
		#endif //-*todo:要らない気がする
			break;
		}

		/****************************/
		/*	ゲームメイン			*/
		/****************************/
		case (INMJMODE.D_FREE_GAME_MODE): {
			// ゲーム再初期化時に描画初期化。
//			IDISPLAY_ClearScreen( GETMYDISPLAY() );
		#if false //-*todo:要らない気がする
			removeSoftkey();//0512mt
		#endif //-*todo:要らない気がする
			/* ロジック　*/
			//おためし
			GameInit();
		#if false //-*todo:要らない気がする
			// --- キャラクター解放。
			MJ_SurvivalCharRelease( );

			// --- フロアーＢＧの解放。
			MJ_SurvivalFloorRelease( );

			// ロードしていた場合は先に解放した後に下で再取得。
			MJ_GameDrawFree( );

			// 画面初期化処理。
			MJ_GameDrawInit( );
		#endif //-*todo:要らない気がする

			// 音楽変更＆再生
#if false //-*todo:サウンド
	#if	Rule_2P
			{
				int[] BGM00 = {
					#if ASA
					SN_BGM06,	SN_BGM06,	SN_BGM06,
					SN_BGM07,	SN_BGM07,
					SN_BGM08,	SN_BGM08,
					#endif
					#if NATSUME
					SN_BGM06,	SN_BGM06,	SN_BGM06,
					SN_BGM07,	SN_BGM07,
					SN_BGM08,	SN_BGM08,
					#endif
					#if NOZOMI
					SN_BGM07,	SN_BGM07,	SN_BGM07,
					SN_BGM08,	SN_BGM08,
					SN_BGM09,	SN_BGM09,
					#endif
					#if AYANO
					SN_BGM07,	SN_BGM07,	SN_BGM07,
					SN_BGM08,	SN_BGM08,
					SN_BGM09,	SN_BGM09,
					#endif
				};

				if( myCanvas.num_battle>= 0)
					myCanvas.playBGM(BGM00[myCanvas.num_battle % BGM00.length]);		/*麻雀*/
				else
					myCanvas.playBGM(SN_BGM08);		/*麻雀*/
			}
	#endif
#endif //-*todo:サウンド
			break;
		}
//-**********todo:ここまで必要物移植

//-*****todo:以下不要？
#if false //-*todo:不要
		/************************************/
		/*	サウンドのボリューム調整。		*/
		/************************************/
		//case(D_SET_SOUND_VOLUME_MODE):
		//{
		//	break;
		//}
		/************************************/
		/*	ロゴ表示						*/
		/************************************/
		case (INMJMODE.D_LOGO_MODE): {
			// --- ロゴのロード。
//m			MJ_RogoDrawInit( );

			// マーキーのX座標 初期値をセット。
			MarqueeX = m_screenWidth;

			break;
		}

		/************************************/
		/*	定期的に通信を・・・			*/
		/************************************/
		case (INMJMODE.D_QUERY_NET_CONNECT_MODE): {
// 0428mt ->
			removeSoftkey();
#if	MPS
			m_AuthCheck = null;
			m_AuthCheck = new AuthCheck();
			m_AuthCheck.init();
			m_AuthCheck.start();
#endif
// 0428mt <-
			break;
		}
		/************************************/
		/*	タイトル表示
			D_OPTION_SOUND_DAIUCHI,	// 手を離した時のｱｸｼｮﾝ。
			D_OPTION_SETUDAN_CYU,	// 切断中のｱｸｼｮﾝ。
			D_OPTION_CHAR_TALK,		// ｷｬﾗｸﾀｰのｾﾘﾌ・ON・OFF
			D_OPTION_NAKI_NASHI,	// 鳴きなし。
		*/
		/************************************/
		case (INMJMODE.D_TITLE_MODE): {
			// ロジック
			MainMenuInit();

			// --- タイトルとbarのロード。
			MJ_TitleDrawInit( );

#if	Rule_2P
#else
			// 音楽変更＆再生
			PlayMusic_BgmChange(D_SOUND_TITLE_BGM);
#endif

			//コネクションを切断する
			MpsBrewLib_FreeMps();//0527mt MpsBrewLib_Release(/*m_brewMps*/);

//			m_brewMps.m_connectInit	  = false;

			//無操作監視タイマーリセット
//xxxx		ISHELL_SetTimerEx(GETMYSHELL(), TIME_WARN_DISCONNECT, idle_disconnect.timer_callback);

			// キー情報を初期化
			ClrKeyInfo();

			// ゲームモード初期化
			sGameData.byGameMode = MJDefine.GAMEMODE_NONE;

			break;
		}
		/************************************/
		/*	オプション設定					*/
		/************************************/
		case (INMJMODE.D_OPTION_SET_MODE): {
			// ルール確認用のリソースのロード。
			MJ_RuleSelectMainDrawInit( );

			// オプションファイルを読み込んで設定を反映させる
			//MJ_ReadFile( D_WORD_INFOFILE, f_info ); 	// 削除 2006/02/25 要望No.120


			// オプション情報保持
			PreFileOptionSetting= f_info.f_optionsetting;	//MEMCPY( PreFileOptionSetting, f_info.f_optionsetting, sizeof( FileOptionSetting ) );

			// 初期化。
			menu_csr = 0;

			// 代打ち初期化
			f_info.f_optionsetting.m_OptionSel[D_OPTION_SOUND_DAIUCHI] = D_ONLINE_OPERATOR_MANUAL;
			select_opt[D_SELECT_OPT_DAIUCHI] = D_ONLINE_OPERATOR_MANUAL;
			updateDaiuchiFlag( );

			// 鳴き無し初期化
			f_info.f_optionsetting.m_OptionSel[D_OPTION_NAKI_NASHI] = D_OPTION_NAKINASHI_OFF;
			select_opt[D_SELECT_OPT_NAKI_NASHI] = D_OPTION_NAKINASHI_OFF;
			updateNakiNashiFlag( );
			break;
		}
		/************************************/
		/*	デバッグモード					*/
		/************************************/
		case (INMJMODE.D_DEBUG_MODE): {
			// 音楽停止
			//PlayMusic_Stop( AppSound[BGM] );
			//PlayMusic_Stop( AppSound[BGM_02] );
			PlayMusic_2chStop( );

			// キャラクターのロード。
			MJ_SurvivalCharLoad( (uint16_t)CHAR_00 );

			// フロアーのロード。
			MJ_SurvivalFloorLoad( );	// デバッグ用。

			// 初期化。
//			SoundTestFlag	= 0;
//			w21s			= 0;
			m_SeOnNo		= 0;
			break;
		}
		/************************************/
		/*	ゲーム終了しますか？			*/
		/************************************/
		case INMJMODE.D_GAME_END_MODE:
			menu_csr = 1;
			break;

		/************************************/
		/*	通信対戦	再接続メニュー		*/
		/************************************/
		case (INMJMODE.D_NET_RETRY_CONNECT_MODE):
//m			break;
		/************************************/
		/*	通信対戦	再接続待ち			*/
		/************************************/
		case (INMJMODE.D_NET_RETRY_CONNECT_WAIT_MODE):
			break;

		/************************************/
		/*	通信対戦	メニュー			*/
		/************************************/
		case (INMJMODE.D_NET_MENU): {
			/* ロジック				*/
			NetMenuInit();

			////コネクションを切断する
			MpsBrewLib_Release(/*m_brewMps*/);//0604mt 通信対戦メニューでは通信を切断 //MpsBrewLib_Release(/*m_brewMps*/);

			/* 初期化処理。			*/
			MJ_NetMenuDrawInit( );
			break;
		}


		/************************************/
		/*			  卓指定対戦			*/
		/************************************/
		// --- テーブル番号入力
		case (INMJMODE.D_NET_TABLE_NUMBER_INPUT_MODE):
			for( i=0 ; i<3 ; i++ )
				menu_sel[i] = 0;	// 初期化。
			menu_csr = 0;			// メニューカーソル初期化。
			gSelectTakuNumber = 0;	// 卓指定番号初期化。
			gNetTable_MatchType = menu_sel[3];	// 半荘・東風選択 2006/02/20 No.712

			MarqueeDrawText= "";	//MEMSET( MarqueeDrawText, 0, sizeof(MarqueeDrawText));	// 描画配列初期化。
			break;

		// --- 通信 卓指定対戦 参加者待ち中
		case (INMJMODE.D_NET_RESERVE_GAME_ENTRY_WAIT_MODE): {
			MarqueeDrawText= "";	//MEMSET( MarqueeDrawText, 0, sizeof(MarqueeDrawText));	// 描画配列初期化。

			// 描画するほうのモードを。
			gRootMode = TABLE_MACHING_MODE;
			removeSoftkey();//0505mt

			// 対戦参加要求送信
			MatchEntryGroupRequestSnd( );
			break;
		}

		/****************************************/
		/* 通信対戦	  フリー・卓指定対戦 共用	*/
		/****************************************/
		case (INMJMODE.D_NET_FREE_GAME_ENTRY_WAIT_MODE): {
			// 描画するほうのモードを。
			gRootMode = AUTO_MACHING_MODE;
			removeSoftkey();//0505mt

			// 対戦参加要求送信
			MatchEntryRequestSnd();
			break;
		}

		case (INMJMODE.D_NET_FREE_MEMBER_WAIT_MODE):
			// マーキーの初期値をセット。
			MarqueeX = m_screenWidth;
			MarqueeDrawText= "";	//MEMSET( MarqueeDrawText, 0, sizeof(MarqueeDrawText));	// 描画配列初期化。

			//for( i=0 ; i<gNetTable_Membercnt ; i++ )
			//{
			//	// キャラクターのフラグを無効化。
			//	gNetTable_NetChar[i].Flag = 0x00;
			//}
			break;

		case (INMJMODE.D_NET_FREE_MEMBER_WAIT_TO_MODE):
//m			break;
		case (INMJMODE.D_NET_FREE_GAME_START_WAIT_MODE):
			break;

		case (INMJMODE.D_NET_FREE_KYOKU_START_WAIT_MODE):
		case (INMJMODE.D_NET_FREE_KYOKU_START_WAIT_MODE2): {
//			KyokuReadyReportSnd();
			break;
		}

		/************************************/
		/*	通信　フリー対戦ゲームメイン	*/
		/************************************/
		case (INMJMODE.D_NET_FREE_GAME_MODE): {
			/* ロジック				*/
			GameInit();

			// ロードしていた場合は先に解放した後に下で再取得。
			MJ_GameDrawFree( );

			// 画面初期化処理。
			MJ_GameDrawInit( );

#if	Rule_2P
#else
			// 音楽変更＆再生
			PlayMusic_BgmChange(D_SOUND_BGM_07);
#endif
			// 時間設定の初期化。
//			DrawTime		= 0xFF;

			break;
		}
		/************************************/
		/*	通信　段位認定戦描画			*/
		/************************************/
		case (INMJMODE.D_NET_RANK_DRAW_MODE): {
			RankDrawMainInit( false);

			if( gMyTable_RankPoint >= 40 && gMyTable_RankPoint <= 49
				//> 2006/03/01 雀聖で昇格戦が発生していた
				&& gMyTable_Rank < RANK_INDEX_MASTER) {
				//< 2006/03/01
				// 昇格戦1の発生。
				RankMode = D_RANKMODE_DRAW_SYOUKAKUSEN_1;
			} else if( gMyTable_RankPoint == 50
				//> 2006/03/01 雀聖で昇格戦が発生していた
					&& gMyTable_Rank < RANK_INDEX_MASTER ) {
				//> 2006/03/01
				// 昇格戦2の発生。
				RankMode = D_RANKMODE_DRAW_SYOUKAKUSEN_2;
			} else if( (gMyTable_RankPoint <=0 ) && ( gMyTable_Rank > RANK_INDEX_1KYU ) ) {
				// 降格戦の発生。
				RankMode = D_RANKMODE_DRAW_KOUKAKUSEN;
			} else {
				// 段位認定戦が発生しなかった。
				RankMode = D_RANKMODE_DONE;
				break;
			}
			break;
		}

		/************************************/
		/*	通信　段位認定戦結果 描画		*/
		/************************************/
		case (INMJMODE.D_NET_RANK_RESULT_DRAW_MODE): {
			// > 2006/02/05 No.400
			MJ_GameDrawFree( );
			MJ_IIMAGE_Release( pIBar );
			MJ_IIMAGE_Release( g_pIImage );	// タイトル解放。

			RankDrawMainInit( true);	// リソースロード。

			// 段位は上がった？ or 下がった？
			if( gMyTable_PreRank==gMyTable_Rank ) {
				// 段位は変わらなかった。
				RankMode = D_RANKMODE_DONE;
			} else if( gMyTable_Rank>gMyTable_PreRank ) {
				// 昇格した。
				RankMode = D_RANKMODE_SYOUKAKU;
			} else
				// 降格した。
				RankMode = D_RANKMODE_KOUKAKU;

			// < 2006/02/05 No.400
			break;
		}
		case INMJMODE.D_NET_NEXT_RANK_DRAW_MODE:
			break;

		// > 2006/02/17
		/************************************/
		/*	通信対戦 段位情報表示			*/
		/************************************/
		case INMJMODE.D_NET_RANK_STATUS_DRAW_MODE:
			MJ_IIMAGE_Release(pIBar);
			break;
		// < 2006/02/17

		/************************************/
		/*	通信対戦 総合順位描画			*/
		/************************************/
		case (INMJMODE.D_NET_RANKING_DRAW_MODE):
			MJ_IIMAGE_Release( pIBar );
			break;



		/************************************/
		/*	COM対戦	サバイバル対戦			*/
		/************************************/
		case (INMJMODE.D_SURVIVAL_LOAD_MODE): {
			// --- 初期化。
			menu_csr = 0;

			// 音楽変更＆再生
			//PlayMusic_BgmChange(D_SOUND_BGM_03);

			break;
		}

		// 途中結果のセーブ。
		case (INMJMODE.D_SURVIVAL_TOCYU_SAVE_MODE):
			break;

		// サバイバル戦オープニング１
		case (INMJMODE.D_SURIVIVAL_OPENING_MODE): {
			// 初期化。
			SurvivalOpInit();
			// キャラクターのロード。
			MJ_SurvivalCharLoad( (uint16_t)CHAR_16 );
			// フロアーのロード。
			MJ_SurvivalFloorLoad( );
			break;
		}
		// サバイバル戦ルール確認
		case (INMJMODE.D_SURIVIVAL_RULE_CHK_MODE): {
			// 初期化。
			RuleCheckInit();
			// 詳細ルールの初期化。
			// ルール確認用のリソースのロード。
			MJ_RuleSelectMainDrawInit( );
			break;
		}
		// サバイバル戦オープニング２
		case (INMJMODE.D_SURIVIVAL_OPENING_NO2_MODE):
			break;

		// サバイバル戦メンバーコメント
		case (INMJMODE.D_SURIVIVAL_COMMENT_MODE): {
			MJ_IIMAGE_Release( g_pIImage );

			// --- 秘書のキャラクター解放。
			MJ_SurvivalCharRelease( );
			// --- フロアーの解放。
//			MJ_SurvivalFloorRelease( );

			MJ_SurvivalFloorLoad( );
			// 初期化。
			SurvivalCommentInit();
			break;
		}
		case (INMJMODE.D_SURIVIVAL_RESULT_MODE): {
			// 初期化。
			SurvivalResultInit();
			// 音楽変更＆再生
#if	Rule_2P
//			myCanvas.playBGM(SN_BGM06);	//麻雀1
#else
			PlayMusic_BgmChange(D_SOUND_BGM_06);
#endif
			break;
		}
		case (INMJMODE.D_SURIVIVAL_GET_CHAR_MODE):
			/* ロジック				*/
			CharaGetInit();
			break;

		case (INMJMODE.D_SURIVIVAL_SAVE_MODE):
			break;


		// --- ゲームリザルト画面・描画 ---
//m		case (D_GAME_RESULT_MODE):
//m			break;

		/****************************/
		/*	通信：エントリ処理		*/
		/****************************/
		case (INMJMODE.D_NET_FREE_ENTRY_START_MODE):
			//サーバーに接続中です．．．
			//初期化処理未定
			removeSoftkey();//0505mt
			break;

		case (INMJMODE.D_NET_FREE_ENTRY_WAIT_MODE):
			//サーバーに接続中です．．．
			//初期化処理未定
//m			break;
		case (INMJMODE.D_NET_FREE_ENTRY_REQ_MODE):
			//サーバーに接続中です．．．
			//初期化処理未定
//m			break;
		case (INMJMODE.D_NET_FREE_ENTRY_RECV_MODE):
			//サーバーに接続中です．．．
			//初期化処理未定
			break;

		/****************************/
		/*	通信：ログイン処理		*/
		/****************************/
		case (INMJMODE.D_NET_FREE_LOGIN_START_MODE):
			//サーバーに接続しますか
			//初期化処理未定
//m			break;
		case (INMJMODE.D_NET_FREE_LOGIN_WAIT_MODE):
			//サーバーに接続中です．．．
			//初期化処理未定
//m			break;
		case (INMJMODE.D_NET_FREE_LOGIN_REQ_MODE):
			//ログイン要求送信
			//初期化処理未定
//m			break;
		case (INMJMODE.D_NET_FREE_LOGIN_RECV_MODE):
			//ログイン応答受信
			//初期化処理未定
			break;

		case (INMJMODE.D_NET_DISCONNECT_MODE):
			// ネットワーク対戦時に
			// 使用していたリソース郡を解放。
			MJ_GameDrawFree( );
			break;

		/****************************/
		/*	そのた					*/
		/****************************/
		//case (D_QUERY_NET_CONNECT_MODE):
		//{
		//	break;
		//}
		case (INMJMODE.D_ASK_NET_CONNECT_MODE):
			break;

		case (INMJMODE.D_CONN_ERR_MODE ) : {
			MPS_STAT mps_stat;

			mps_stat = MpsBrewLib_CheckState( /*(m_brewMps)*/ );
			if( mps_stat == MPS_UP ) {
				//MPS終了処理
				if( MpsBrewLib_Release(/*(m_brewMps)*/) != true ) {
					ETRACE("MpsBrewLib_Release error at App_Mode_Timer_Disconnect !");
					ASSERT( 0 );
				}
			}
			break;
		}
		case (INMJMODE.D_BEFORE_ERR_MODE) :
			DebLog(("D_BEFORE_ERR_MODE\n"));
			break;

		case (INMJMODE.D_MEMORY_ERR_MODE) :
			DebLog(("D_MEMORY_ERR_MODE\n"));
			break;

		case (INMJMODE.D_EFS_ERR_MODE) :
			DebLog(("D_EFS_ERR_MODE\n"));
			break;

		case (INMJMODE.D_AFTER_ERR_MODE) :
			DebLog(("D_AFTER_ERR_MODE\n"));
			break;

		case (INMJMODE.D_GO_SITE_MODE):
//			generalFlag	= 1;	// デフォルトは1
			break;

		case (INMJMODE.D_TIMER_DISCONNECT_MODE):
			//コネクションを切断する
			if( MpsBrewLib_Release(/*m_brewMps*/) != true)
				DebLog(("MpsBrewLib_Release Error\n"));
//			m_brewMps.m_connectInit	  = false;
			break;

		case (INMJMODE.D_ERR_MODE):
//m			break;
		case (INMJMODE.D_FATAL_ERR_MODE):
			break;
#endif //-*todo:不要

	}

//	m_PreMode = m_Mode;
	m_Mode	= to_mode;

	// メモリ確保サイズチェック
//0507mt	allocatedMemorySize();
//	return;
}

//関数 : エントリ処理　MPS接続
//引数 : pMe	: MahJongRally(IApplet)ポインタ
//戻値 : なし
//作成 :
//備考 :
// D_NET_FREE_ENTRY_START_MODE
public void App_Mode_Entry( /*MahJongRally *pMe*/ )
{
#if false //-*todo:要るかな？	
	if( (int)( System.currentTimeMillis()/1000 - m_PauseTime ) < 30 )	return;	//0516mt
// vappli add 2006/04/14 start
	//MPSライブラリ初期化
	if( MpsBrewLib_InitMps(/*(m_brewMps)*/) == false ) {
		//System.out.println("App_Mode_Entry:MpsBrewLib_InitMps:failed");
		modeChange( INMJMODE.D_NET_ERR_MODE );
		return;
	}
// vappli add 2006/04/14 end
// vappli add start
	modeChange(INMJMODE.D_NET_FREE_LOGIN_START_MODE);
// vappli add end
#endif //-*todo:要るかな？	
}

//関数 : エントリ処理　MPS接続待ち
//引数 : pMe	: MahJongRally(IApplet)ポインタ
//戻値 : なし
//作成 :
//備考 :
// D_NET_FREE_ENTRY_WAIT_MODE
public void App_Mode_Entry_Wait( /*MahJongRally *pMe*/ )
{
}

//関数 : エントリ要求送信
//引数 : pMe	: MahJongRally(IApplet)ポインタ
//戻値 : なし
//作成 :
//備考 :
// D_NET_FREE_ENTRY_REQ_MODE
public void App_Mode_Entry_Req( /*MahJongRally *pMe*/ )
{

}

//関数 : エントリ応答待ち
//引数 : pMe	: MahJongRally(IApplet)ポインタ
//戻値 : なし
//作成 :
//備考 :
// D_NET_FREE_ENTRY_RECV_MODE
public void App_Mode_Entry_Recv( /*MahJongRally *pMe*/ )
{

}

//関数 : ログイン処理　MPS接続
//引数 : pMe	: MahJongRally(IApplet)ポインタ
//戻値 : なし
//作成 :
//備考 :
// D_NET_FREE_LOGIN_START_MODE
public void App_Mode_Login( /*MahJongRally *pMe*/ )
{
#if false //-*todo:要るかな？	
	if( MpsBrewLib_CheckState(/*m_brewMps*/) != 1 ) {
		if( MpsBrewLib_Connect(/*(m_brewMps),*/ 0) == true )
			modeChange( INMJMODE.D_NET_FREE_LOGIN_WAIT_MODE );
		else
			modeChange( INMJMODE.D_CONN_ERR_MODE );
	}
// vappli add end

	//クリアキーでのキャンセルのみ対応
	if(( GetKeyTrg( _KEY_CLEAR ))!= 0) {
		//タイトルへ
		modeChange( INMJMODE.D_TITLE_MODE);
//		return;
	}
//	return;
#endif //-*todo:要るかな？	
}

//関数 : ログイン処理　MPS接続待ち
//引数 : pMe	: MahJongRally(IApplet)ポインタ
//戻値 : なし
//作成 :
//備考 :
// D_NET_FREE_LOGIN_WAIT_MODE
public void App_Mode_Login_Wait( /*MahJongRally *pMe*/ )
{
#if false //-*todo:要るかな？	
	MPS_STAT	mps_stat;		//MPSステータス

	//サウンド状態処理
	//ソフトキー処理
	//画面更新

	//MPS通信状態確認
	mps_stat = MpsBrewLib_CheckState(/*(m_brewMps)*/);
	if( mps_stat == MPS_UP )
		//ログイン要求送信モードへ遷移
		modeChange( INMJMODE.D_NET_FREE_LOGIN_REQ_MODE);
	else
		if( mps_stat == MPS_STAT_ERR ) {
			//接続できませんでした
			lasterror = LAST_ERR_NETWORK_CONNECT;
			modeChange( INMJMODE.D_CONN_ERR_MODE );
			return;
		}

	//クリアキーでのキャンセルのみ対応
	if(( GetKeyTrg( _KEY_CLEAR ))!= 0) {
		//タイトルへ
		modeChange( INMJMODE.D_TITLE_MODE);
//		return;
	}

//	return;
#endif //-*todo:要るかな？	
}

//関数 : ログイン要求送信
//引数 : pMe	: MahJongRally(IApplet)ポインタ
//戻値 : なし
//作成 :
//備考 :
// D_NET_FREE_LOGIN_REQ_MODE
public void App_Mode_Login_Req( /*MahJongRally *pMe*/ )
{
#if false //-*todo:要るかな？	
	bool		ret;

	//サウンド状態処理
	//ソフトキー処理
	//画面更新

	//サブスクライバＩＤを取得
	ret = GetSubscriberID();

#if	MPS
	String subid_ = "vapp_id=0x" + Integer.toHexString( m_PlayerID );
#else
	String subid_ = "vapp_id=0x0000";
#endif

	byte[] _SubscriberID = new byte [41]; //
	long _version; //
	uint8_t _survival; //
	_SubscriberID= subid_.getBytes();
	_version = KOEI_MJ_CLIENT_VERSION;
	_survival = (uint8_t)(( survival_win_count > 0 ) ? 1 : 0);

#if	MPS
	if( mpsSendMessage_Login_Request(
		_SubscriberID,
		_version,
		_survival
		) == false )
	{
		modeChange( 61 );
		return;
	}
#endif
	_SubscriberID = null;
//0508mt <-

	// メッセージタイプクリア
//0508mt 	m_brewMps.m_data.msg.msgType=0;			//0422

	//クリアキーでのキャンセルのみ対応
	if(( GetKeyTrg( _KEY_CLEAR ))!= 0)
	{
		modeChange( INMJMODE.D_TITLE_MODE);		//タイトルへ
		return;
	}
	#if true //-*todo:
	DebLog( ( "<< App_Mode_Login_Req subid="+appSystem.subscriberID ) );
	#endif //-*todo:
	//ログイン応答待ち
	modeChange( INMJMODE.D_NET_FREE_LOGIN_RECV_MODE );

	return;
#endif //-*todo:要るかな？	
}

//関数 : ログイン応答待ち
//引数 : pMe	: MahJongRally(IApplet)ポインタ
//戻値 : なし
//作成 :
//備考 :
// D_NET_FREE_LOGIN_RECV_MODE
public void App_Mode_Login_Recv( /*MahJongRally *pMe*/ )
{
#if false //-*todo:要るかな？	
	//サウンド状態処理
	//ソフトキー処理
	//画面更新

	//クリアキーでのキャンセルのみ対応
	if(( GetKeyTrg( _KEY_CLEAR ))!= 0)
	{
		//タイトルへ
		modeChange( INMJMODE.D_TITLE_MODE);
		return;
	}
	return;
#endif //-*todo:要るかな？	
}


//関数 : 接続失敗ロジック。
//引数 : pMe	: MahJongRally(IApplet)ポインタ
//戻値 : なし
//作成 :
//備考 :
// D_CONN_ERR_MODE
public INMJMODE App_Mode_Conn_Err_Main( /*MahJongRally *pMe*/ )
{
	long	error_type_ = (long)(lasterror & 0xffff0000);
	long	error_code_ = (long)(lasterror & 0x0000ffff);

#if	MPS
	if( ( error_type_ >> 16 ) == MSGTYPE_LOGIN_RESPONSE_NG ) // (long)m_brewMps.m_data.msg.User.login_response_ng.msgType )	//0422
	{
		switch( (int)error_code_ )
		{
			// 横八文字制限 縦四文字制限
			case LOGIN_ERR_UNREGISTERD	://( 1 )// 登録されていない
			case LOGIN_ERR_NOTACTIVE	://( 2 )// フラグがACTIVEでない
			{
				if( D_ONEPUSH_INPUT_SELECT )
				{
					if( menu_csr == 0 )
					{
						PlayMusic_2chStop();

						//0517mt
//m						String url_ = "rtws.koei.co.jp/vmto/vmto?uid=1&pid=P359&sid=B1G6&Act=Top";
						String url_ = "rtws.test.co.jp";
						if( GoHomePage( url_ ) == false )
//0517mt						if( GoHomePage( "url://rtws.koei.co.jp/vmto/vmto?uid=1&pid=P359&sid=B1G6&Act=Top" ) == FALSE )		//0422
						{
							// 待避処理
							return D_TITLE_MODE;//m_Mode;aaa
						}

						m_PlayerID = 0xffffffff;
						ssave00(true);
						//0517mt webアクセスを行った場合はplayerID更新の為、再度認証を行う
						removeSoftkey();//0531mt
						return 101;//0530mt No.1096 D_QUERY_NET_CONNECT_MODE;//0517mt return(D_TITLE_MODE);
					}
					else
					{
						return(D_TITLE_MODE);
					}
				}
				else if( D_ONEPUSH_INPUT_UP )
				{
					menu_csr = (UINT2)(menu_csr^1);	//!menu_csr;
				}
				else if( D_ONEPUSH_INPUT_DOWN )
				{
					menu_csr = (UINT2)(menu_csr^1);	//!menu_csr;
				}
				return D_CONN_ERR_MODE;
			}
//			break;
		}
	}
#endif
#if false //-*todo:
	// サウンド状態処理
	// ソフトキー処理
	if( D_ONEPUSH_INPUT_SELECT )
	{
		// タイトルへ
		// 決定キーサウンド再生。
		return(INMJMODE.D_TITLE_MODE);
	}
#endif //-*todo:

	return(INMJMODE.D_CONN_ERR_MODE);
}


//関数 : 接続失敗描画。
//引数 : pMe	: MahJongRally(IApplet)ポインタ
//戻値 : なし
//作成 :
//備考 :
// D_CONN_ERR_MODE
public void App_Mode_Conn_Err_Draw( /*MahJongRally *pMe*/ )
{
#if false //-*todo:	
	int x = 0;
	int y = 90;
	int i = 0;
	int l = 0;
	int offset = 19;

	// エラー文字列のまとめるのが無いので、敢えてハードコーディング
	long	error_type_ = (long)(lasterror & 0xffff0000);
	long	error_code_ = (long)(lasterror & 0x0000ffff);

	// 背景
	MJ_DrawMainBG( );

	#if	MPS
	switch( (int)(error_type_ >> 16) )
	{
		case  MSGTYPE_LOGIN_RESPONSE_NG :
		{
			switch( (int)error_code_ )
			{
			case LOGIN_ERR_UNREGISTERD	://( 1 )// 登録されていない
			case LOGIN_ERR_NOTACTIVE	://( 2 )// フラグがACTIVEでない
				l = 6; x = 0; y = ( m_screenHeight - offset * l ) / 2 ;
				MJ_DrawMenu( x, y , 13, l, NORMAL_MENU );
				y+=10;
				MJ_Msg_CenterLine( "登録が行われていません\0\0", x, y+(offset*i++), SYS_FONT_WHITE );	//(AECHAR*)
				MJ_Msg_CenterLine( "サイトへ移動します\0\0"	 , x, y+(offset*i++), SYS_FONT_WHITE );		//(AECHAR*)
				MJ_YesNo_Draw( 70, y+(offset*i++) + 10, 20);
				break;
			case LOGIN_ERR_TERMINALCODE ://( 3 )// 端末コードエラー
				l = 5; x = 10; y = ( m_screenHeight - offset * l ) / 2 ;		//0422
				MJ_DrawMenu( x, y , 12, l, NORMAL_MENU );
				y+=10;
				MJ_Msg_CenterLine( "ログインに失敗しました\0\0"  , x, y+(offset*i++), SYS_FONT_WHITE );		//0422
				MJ_Msg_CenterLine( "暫くしてから\0\0"  , x, y+(offset*i++), SYS_FONT_WHITE );			//0422
				MJ_Msg_CenterLine( "ログインしてください\0\0"  , x, y+(offset*i++), SYS_FONT_WHITE );		//0422
				MJ_IconDraw( D_TRIANGLE_RIGHT, 80, y+(offset*i) + 5, 0 );
				MJ_Msg_CenterLine( D_WORD_YES, 0, y+(offset*i)+ 5, SYS_FONT_WHITE );					//(AECHAR*)
				break;
			case LOGIN_ERR_DUPULICATE	://( 4 )// 重複している
				l = 5; x = 10; y = ( m_screenHeight - offset * l ) / 2 ;
				MJ_DrawMenu( x, y , 12, l, NORMAL_MENU );
				y+=10;
				MJ_Msg_CenterLine( "ログインに失敗しました\0\0"  , x, y+(offset*i++), SYS_FONT_WHITE );		//0422
				MJ_Msg_CenterLine( "暫くしてから\0\0"  , x, y+(offset*i++), SYS_FONT_WHITE );			//0422
				MJ_Msg_CenterLine( "ログインしてください\0\0"  , x, y+(offset*i++), SYS_FONT_WHITE );		//0422
				MJ_IconDraw( D_TRIANGLE_RIGHT, 80, y+(offset*i) + 5, 0 );
				MJ_Msg_CenterLine( D_WORD_YES, 0, y+(offset*i)+ 5, SYS_FONT_WHITE );					//(AECHAR*)
				break;
			case LOGIN_ERR_MAINTENANCE	://( 5 )// サーバーメンテナンス
				l = 10; x = 10; y = ( m_screenHeight - offset * l ) / 2 ;
				MJ_DrawMenu( x, y , 12, l, NORMAL_MENU );
				y+=10;
				MJ_Msg_CenterLine( "サーバーのメンテナン\0\0"    , x, y+(offset*i++), SYS_FONT_WHITE );	//(AECHAR*)
				MJ_Msg_CenterLine( "ス中です　メンテナン\0\0"    , x, y+(offset*i++), SYS_FONT_WHITE );	//(AECHAR*)
				MJ_Msg_CenterLine( "ス完了までは通信対戦\0\0"    , x, y+(offset*i++), SYS_FONT_WHITE );	//(AECHAR*)
				MJ_Msg_CenterLine( "を行うことができませ\0\0"    , x, y+(offset*i++), SYS_FONT_WHITE );	//(AECHAR*)
				MJ_Msg_CenterLine( "ん　詳しくはサイトを\0\0"    , x, y+(offset*i++), SYS_FONT_WHITE );	//(AECHAR*)
				MJ_Msg_CenterLine( "ご覧ください　　　　\0\0"    , x, y+(offset*i++), SYS_FONT_WHITE );	//(AECHAR*)
				MJ_IconDraw( D_TRIANGLE_RIGHT, 80, y+(offset*i) + 5, 0 );
				MJ_Msg_CenterLine( D_WORD_YES, 0, y+(offset*i)+ 5, SYS_FONT_WHITE );					//(AECHAR*)
				break;
			case LOGIN_ERR_MAXPLAYER	://( 6 )// プレイヤー数が最大である
				l = 4; x = 10; y = ( m_screenHeight - offset * l ) / 2 ;
				MJ_DrawMenu( x, y , 12, l, NORMAL_MENU );
				y+=10;
				MJ_Msg_CenterLine( "プレイヤー数が\0\0"  , x, y+(offset*i++), SYS_FONT_WHITE );			//(AECHAR*)
				MJ_Msg_CenterLine( "最大です\0\0"  , x, y+(offset*i++), SYS_FONT_WHITE );				//(AECHAR*)
				MJ_IconDraw( D_TRIANGLE_RIGHT, 80, y+(offset*i) + 5, 0 );
				MJ_Msg_CenterLine( D_WORD_YES, 0, y+(offset*i)+ 5, SYS_FONT_WHITE );					//(AECHAR*)
				break;
			case LOGIN_ERR_VERSION		://( 7 )// バージョン違い
				l = 4; x = 10; y = ( m_screenHeight - offset * l ) / 2 ;
				MJ_DrawMenu( x, y , 12, l, NORMAL_MENU );
				y+=10;
				//> 2006/03/01 要望130
				MJ_Msg_CenterLine( "アプリのバージョンア\0\0"  , x, y+(offset*i++), SYS_FONT_WHITE );	//(AECHAR*)
				MJ_Msg_CenterLine( "ップを行ってください\0\0"  , x, y+(offset*i++), SYS_FONT_WHITE );	//(AECHAR*)
				//< 2006/03/01 要望130
				MJ_IconDraw( D_TRIANGLE_RIGHT, 80, y+(offset*i) + 5, 0 );
				MJ_Msg_CenterLine( D_WORD_YES, 0, y+(offset*i)+ 5, SYS_FONT_WHITE );					//(AECHAR*)
				break;
			case LOGIN_ERR_DATABASE 	://( 8 )// データベースエラー
				l = 4; x = 10; y = ( m_screenHeight - offset * l ) / 2 ;
				MJ_DrawMenu( x, y , 12, l, NORMAL_MENU );
				y+=10;
				MJ_Msg_CenterLine( "データベースの操作に\0\0", x, y+(offset*i++), SYS_FONT_WHITE );		//(AECHAR*)
				MJ_Msg_CenterLine( "失敗しました\0\0"    , x, y+(offset*i++), SYS_FONT_WHITE );			//(AECHAR*)
				MJ_IconDraw( D_TRIANGLE_RIGHT, 80, y+(offset*i) + 5, 0 );
				MJ_Msg_CenterLine( D_WORD_YES, 0, y+(offset*i)+ 5, SYS_FONT_WHITE );					//(AECHAR*)
				break;
			case LOGIN_ERR_UNKNOWN		://( 9 )// 不明なエラー
				l = 3; x = 10; y = ( m_screenHeight - offset * l ) / 2 ;
				MJ_DrawMenu( x, y , 12, l, NORMAL_MENU );
				y+=10;
				MJ_Msg_CenterLine( "システムエラー\0\0", x, y+(offset*i++), SYS_FONT_WHITE );			//(AECHAR*)
				MJ_IconDraw( D_TRIANGLE_RIGHT, 80, y+(offset*i) + 5, 0 );
				MJ_Msg_CenterLine( D_WORD_YES, 0, y+(offset*i)+ 5, SYS_FONT_WHITE );					//(AECHAR*)
				break;
			default :// その他
				l = 5; x = 10; y = ( m_screenHeight - offset * l ) / 2 ;
				MJ_DrawMenu( x, y , 12, l, NORMAL_MENU );
				y+=10;
				MJ_Msg_CenterLine( "通信できません\0\0", x, y+(offset*i++), SYS_FONT_WHITE );			//(AECHAR*)
				MJ_Msg_CenterLine( "制限設定、電波状態\0\0", x, y+(offset*i++), SYS_FONT_WHITE );		//(AECHAR*)
				MJ_Msg_CenterLine( "などを確認して下さい\0\0", x, y+(offset*i++), SYS_FONT_WHITE );		//(AECHAR*)
				MJ_IconDraw( D_TRIANGLE_RIGHT, 80, y+(offset*i) + 5, 0 );
				MJ_Msg_CenterLine( D_WORD_YES, 0, y+(offset*i)+ 5, SYS_FONT_WHITE );					//(AECHAR*)
				break;
			}
		}
		break;
		default :
		{
			// その他のエラー
			l = 5; x = 10; y = ( m_screenHeight - offset * l ) / 2 ;
			MJ_DrawMenu( x, y , 12, l, NORMAL_MENU );
			y+=10;
			MJ_Msg_CenterLine( "通信できません\0\0", x, y+(offset*i++), SYS_FONT_WHITE );				//(AECHAR*)
			MJ_Msg_CenterLine( "制限設定、電波状態\0\0", x, y+(offset*i++), SYS_FONT_WHITE );			//(AECHAR*)
			MJ_Msg_CenterLine( "などを確認して下さい\0\0", x, y+(offset*i++), SYS_FONT_WHITE );			//(AECHAR*)

			MJ_IconDraw( D_TRIANGLE_RIGHT, 80, y+(offset*i) + 5, 0 );
			MJ_Msg_CenterLine( D_WORD_YES, 0, y+(offset*i)+ 5, SYS_FONT_WHITE );						//(AECHAR*)
		}
		break;
	}
	return;
	#endif
#endif	//-*todo:
}

//関数 : エラーコードセット
//引数 : pMe	: MahJongRally(IApplet)ポインタ
//戻値 : なし
//作成 :
//備考 :
void Set_ErrCode( /*MahJongRally *pMe,*/ int err )
{
	errCode = err;
	return;
}

//関数 : エラー選択ロジック。
//引数 : pMe	: MahJongRally(IApplet)ポインタ
//戻値 : なし
//作成 :
//備考 :
public void App_Mode_Err_Select( /*MahJongRally *pMe*/ )
{
	
	switch( errCode )
	{
#if false //-*todo:	
	case D_ERR_MODE_MEMORY:
		modeChange( INMJMODE.D_MEMORY_ERR_MODE );
		break;
	case D_ERR_MODE_FATAL:
		modeChange( INMJMODE.D_MEMORY_ERR_MODE );
		break;
#endif //-*todo:	
	default:
		modeChange( INMJMODE.D_MEMORY_ERR_MODE );
		break;
	}
	
	return;
}

/**
*  @brief エラー処理
*/
public INMJMODE App_Mode_Err_Main( /*MahJongRally *pMe*/ )
{
#if false //-*todo:	
	if( D_ONEPUSH_INPUT_SELECT )
#endif //-*todo:
	{
		// エラーをクリア
		lasterror = 0x00000000;
		// タイトルへ
		return(INMJMODE.D_TITLE_MODE);
	}
	return(INMJMODE.D_ERR_MODE);
}

/**
*  @brief エラー描画
*/
public void App_Mode_Err_Draw( /*MahJongRally *pMe*/ )
{
	int x = 0;
	int y = 90;
	int i = 0;
	int l = 0;
	int offset = 19;
#if false //-*todo:
	// 背景
	MJ_DrawMainBG( );

	switch( (int)lasterror )
	{
		case LAST_ERR_READ_MESSAGE :
		{
			l = 5; x = 10; y = ( m_screenHeight - offset * l ) / 2 ;
			MJ_DrawMenu( x, y , 12, l, NORMAL_MENU );
			y+=10;
			MJ_Msg_CenterLine( "メッセージの読み込みに\0\0", x, y+(offset*i++), SYS_FONT_WHITE );		//(AECHAR*)
			MJ_Msg_CenterLine( "失敗しました\0\0", x, y+(offset*i++), SYS_FONT_WHITE );					//(AECHAR*)
			MJ_Msg_CenterLine( "タイトルへ戻ります\0\0", x, y+(offset*i++), SYS_FONT_WHITE );			//(AECHAR*)

			MJ_IconDraw( D_TRIANGLE_RIGHT, 80, y+(offset*i) + 5, 0 );
			MJ_Msg_CenterLine( D_WORD_YES, 0, y+(offset*i++)+ 5, SYS_FONT_WHITE );						//(AECHAR*)
		}
		break;
		case LAST_ERR_CN_DOWN      :
		case LAST_ERR_CN_DISC      :
		case LAST_ERR_CN_ERR       :
		{
			l = 5; x = 10; y = ( m_screenHeight - offset * l ) / 2 ;
			MJ_DrawMenu( x, y , 12, l, NORMAL_MENU );
			y+=10;
			MJ_Msg_CenterLine( "サーバーからの応答が\0\0", x, y+(offset*i++), SYS_FONT_WHITE );			//(AECHAR*)
			MJ_Msg_CenterLine( "ありません\0\0", x, y+(offset*i++), SYS_FONT_WHITE );					//(AECHAR*)
			MJ_Msg_CenterLine( "タイトルへ戻ります\0\0", x, y+(offset*i++), SYS_FONT_WHITE );			//(AECHAR*)

			MJ_IconDraw( D_TRIANGLE_RIGHT, 80, y+(offset*i) + 5, 0 );
			MJ_Msg_CenterLine( D_WORD_YES, 0, y+(offset*i++)+ 5, SYS_FONT_WHITE );						//(AECHAR*)
		}
		break;
		case LAST_ERR_WRITE_OVERFLOW  :
		{
			l = 5; x = 10; y = ( m_screenHeight - offset * l ) / 2 ;
			MJ_DrawMenu( x, y , 12, l, NORMAL_MENU );
			y+=10;
			MJ_Msg_CenterLine( "メッセージの送信に\0\0", x, y+(offset*i++), SYS_FONT_WHITE );			//(AECHAR*)
			MJ_Msg_CenterLine( "失敗しました\0\0", x, y+(offset*i++), SYS_FONT_WHITE );					//(AECHAR*)
			MJ_Msg_CenterLine( "タイトルへ戻ります\0\0", x, y+(offset*i++), SYS_FONT_WHITE );			//(AECHAR*)

			MJ_IconDraw( D_TRIANGLE_RIGHT, 80, y+(offset*i) + 5, 0 );
			MJ_Msg_CenterLine( D_WORD_YES, 0, y+(offset*i++)+ 5, SYS_FONT_WHITE );						//(AECHAR*)
		}
		break;
		case LAST_ERR_READ_OVERFLOW   :
		{
			l = 5; x = 10; y = ( m_screenHeight - offset * l ) / 2 ;
			MJ_DrawMenu( x, y , 12, l, NORMAL_MENU );
			y+=10;
			MJ_Msg_CenterLine( "メッセージの受信に\0\0", x, y+(offset*i++), SYS_FONT_WHITE );			//(AECHAR*)
			MJ_Msg_CenterLine( "失敗しました\0\0", x, y+(offset*i++), SYS_FONT_WHITE );					//(AECHAR*)
			MJ_Msg_CenterLine( "タイトルへ戻ります\0\0", x, y+(offset*i++), SYS_FONT_WHITE );			//(AECHAR*)

			MJ_IconDraw( D_TRIANGLE_RIGHT, 80, y+(offset*i) + 5, 0 );
			MJ_Msg_CenterLine( D_WORD_YES, 0, y+(offset*i++)+ 5, SYS_FONT_WHITE );						//(AECHAR*)
		}
		break;
		case LAST_ERR_NETWORK_CONNECT :
		{
			l = 5; x = 10; y = ( m_screenHeight - offset * l ) / 2 ;
			MJ_DrawMenu( x, y , 12, l, NORMAL_MENU );
			y+=10;
			MJ_Msg_CenterLine( "通信できません\0\0", x, y+(offset*i++), SYS_FONT_WHITE );				//(AECHAR*)
			MJ_Msg_CenterLine( "制限設定、電波状態\0\0", x, y+(offset*i++), SYS_FONT_WHITE );			//(AECHAR*)
			MJ_Msg_CenterLine( "などを確認して下さい\0\0", x, y+(offset*i++), SYS_FONT_WHITE );			//(AECHAR*)
			MJ_IconDraw( D_TRIANGLE_RIGHT, 80, y+(offset*i) + 5, 0 );
			MJ_Msg_CenterLine( D_WORD_YES, 0, y+(offset*i)+ 5, SYS_FONT_WHITE );						//(AECHAR*)
		}
		break;
		default :
		{
			l = 5; x = 10; y = ( m_screenHeight - offset * l ) / 2 ;
			MJ_DrawMenu( x, y , 12, l, NORMAL_MENU );
			y+=10;
			MJ_Msg_CenterLine( "通信できません\0\0", x, y+(offset*i++), SYS_FONT_WHITE );				//(AECHAR*)
			MJ_Msg_CenterLine( "制限設定、電波状態\0\0", x, y+(offset*i++), SYS_FONT_WHITE );			//(AECHAR*)
			MJ_Msg_CenterLine( "などを確認して下さい\0\0", x, y+(offset*i++), SYS_FONT_WHITE );			//(AECHAR*)
			MJ_IconDraw( D_TRIANGLE_RIGHT, 80, y+(offset*i) + 5, 0 );
			MJ_Msg_CenterLine( D_WORD_YES, 0, y+(offset*i)+ 5, SYS_FONT_WHITE );						//(AECHAR*)
		}
		break;
	}
#endif //-*todo:
	// for debug
	return;
}

/**
*  @brief エラー処理
*/
public INMJMODE App_Submit_Err_Main( /*MahJongRally *pMe*/ )
{
#if false //-*todo:
	if( D_ONEPUSH_INPUT_SELECT )
#endif //-*todo:
	{
		// タイトルへ
		return(INMJMODE.D_TITLE_MODE);
	}
	return(INMJMODE.D_SUBMIT_ERR_MODE);
}

/**
*  @brief 認証エラー描画
*/
public void App_Submit_Err_Draw( /*MahJongRally *pMe*/ )
{
	int x = 0;
	int y = 90;
	int i = 0;
	int l = 0;
	int offset = 19;
#if false //-*todo:

	// 背景
	MJ_DrawMainBG( );

	l = 6; x = 0; y = ( m_screenHeight - offset * l ) / 2 ;
	MJ_DrawMenu( x, y , 13, l, NORMAL_MENU );
	y+=10;

	// > 2006/02/27 要望No.731
	switch ( submiterr ) {

	default:
		MJ_Msg_CenterLine( "認証処理エラー\0\0", x, y+(offset*i++), SYS_FONT_WHITE );			//(AECHAR*)
		MJ_Msg_CenterLine( "タイトルへ戻ります\0\0", x, y+(offset*i++), SYS_FONT_WHITE );		//(AECHAR*)
		break;
	}
	// < 2006/02/27 要望No.731

	MJ_IconDraw( D_TRIANGLE_RIGHT, 80, y+(offset*i) + 10, 0 );
	MJ_Msg_CenterLine( D_WORD_YES, 0, y+(offset*i)+ 10, SYS_FONT_WHITE );						//(AECHAR*)
#endif //-*todo:
	return;
}

/**
 * @brief プロトコルエラー処理
 */
public INMJMODE App_Mode_Protocol_Err_Main( /*MahJongRally *pMe*/ )
{	//0422	この関数内
	// エラー毎に処理が必要なら、ここを利用してください。
	long error_type_ = (long)(lasterror & 0xffff0000);
	long error_code_ = (long)(lasterror & 0x0000ffff);
#if false //-*todo:
	if ( D_ONEPUSH_INPUT_CLEAR )
	{
		// CLRキー → 切断
		MpsBrewLib_Release(/*m_brewMps*/);
//xxxx	ISHELL_SetTimerEx(GETMYSHELL(), TIME_WARN_DISCONNECT, idle_disconnect.timer_callback);
		modeChange(INMJMODE.D_USER_DISCONNECT_MODE);
	}
	else if( D_ONEPUSH_INPUT_SELECT )
	{
		// 通信対戦メニューへ
		return(INMJMODE.D_NET_MENU);
	}
#endif //-*todo:
	return(INMJMODE.D_NET_ERR_MODE);
}

/**
 * @brief プロトコルエラー描画
 */
public void App_Mode_Protocol_Err_Draw( /*MahJongRally *pMe*/ )
{
	int x = 0;
	int y = 90;
	int i = 0;
	int l = 0;
	int offset = 19;

	long error_type_ = (long)(lasterror & 0xffff0000);
//	long error_code_ = lasterror & 0x0000ffff;		//0422
#if false //-*todo:
	// 背景
	MJ_DrawMainBG( );

	#if	MPS
	switch( (int)(error_type_ >> 16) )
	{
		case MSGTYPE_ENTRY_MATCH_RESPONSE_NG :
		case MSGTYPE_ENTRY_GROUP_MATCH_RESPONSE_NG :											//0422
		{																						//0422
			l = 4; x = 10; y = ( m_screenHeight - offset * l ) / 2 ;							//0422
			MJ_DrawMenu( x, y , 12, l, NORMAL_MENU );											//0422
			y+=10;																				//0422

			MJ_Msg_CenterLine( "マッチング参加に\0\0", x, y+(offset*i++), SYS_FONT_WHITE );		//0422
			MJ_Msg_CenterLine( "失敗しました\0\0"	, x, y+(offset*i++), SYS_FONT_WHITE );		//0422

			MJ_IconDraw( D_TRIANGLE_RIGHT, 80, y+(offset*i) + 5, 0 );							//0422
			MJ_Msg_CenterLine( D_WORD_YES, 0, y+(offset*i)+ 5, SYS_FONT_WHITE );				//0422
		}																						//0422
		break;
		default :
		{
			// その他のエラー
			l = 5; x = 10; y = ( m_screenHeight - offset * l ) / 2 ;
			MJ_DrawMenu( x, y , 12, l, NORMAL_MENU );
			y+=10;
			MJ_Msg_CenterLine( "エラー\0\0", x, y+(offset*i++), SYS_FONT_WHITE );				//(AECHAR*)
			MJ_Msg_CenterLine( "通信対戦メニューに\0\0", x, y+(offset*i++), SYS_FONT_WHITE );	//(AECHAR*)
			MJ_Msg_CenterLine( "戻ります\0\0", x, y+(offset*i++), SYS_FONT_WHITE );				//(AECHAR*)

			MJ_IconDraw( D_TRIANGLE_RIGHT, 80, y+(offset*i) + 5, 0 );
			MJ_Msg_CenterLine( D_WORD_YES, 0, y+(offset*i)+ 5, SYS_FONT_WHITE );				//(AECHAR*)
		}
		break;
	}
	return;
	#endif
#endif //-*todo:
}

//-*********************app_mode.j
}