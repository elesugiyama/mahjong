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
// app.j
//-*****************
public partial class MahjongBase : SceneBase {

	// 色定義
	static Vector3 COLOR_DEFAULT	= new Vector3(  0,	0,	 0);		// デフォルト背景色
	static Vector3 COLOR_TABLE		= new Vector3( 49, 101,	90);		// 卓
	static Vector3 COLOR_MENU		= new Vector3(123, 121,	74);		// メニュー画面背景
	public int		m_Mode_kep= -1;
	public long	worktime= 0;
#if false //-*todo:
	public AEERect app_main_rect = new AEERect();
#endif

	public void app_main( /*MahJongRally *pMe*/ )
	{

		#if DOCOMO // 080716:強制ロック
		if(AppMain.flagBattleEnd){
		AppMain._lock();
		}
		#endif

		int			ret;
		// int			mode;		//w4
		long	starttime= 0;
#if false //-*todo:多分要らない
		while ( app_main_done ){
			if( suspend != 0 || exit == true ){
				return;
			}
		}
#endif //-*todo:
		worktime= 0;	//0505mt

	//	app_main_done = false;//0507mt

		ready = (false);
#if false //-*todo:多分要らない
		UpdateKeyInfo();	// キー情報更新
#endif //-*todo:

		if( (int)m_Mode!= m_Mode_kep) {
#if true //-*todo:描画
			paintF= true;
#endif //-*todo:描画
			m_Mode_kep= (int)m_Mode;
	#if DEBUG		//_DEBUG
	//		Debug.out("m_Mode ", m_Mode);
	#endif
		}

		/************************/
		/*	処理時間測定		*/
		/************************/
	//xxxx	starttime = ISHELL_GetTimeMS(pIshell);
		#if true //-*todo:変換あってるかな？
		// starttime = System.currentTimeMillis();
		starttime = DateTime.Now.Ticks;
		#endif //-*todo

	#if	MPS
		// 通信時無操作タイマー
		if( IMps != null && IMps.getState() == MPSC.STATE_UP ) {
			if( starttime - m_LastKeyUpdate - 300000 > 0 ) {
				m_LastKeyUpdate = starttime;
				m_PauseTime = (int)(System.currentTimeMillis()/1000);
				modeChange( 58 );
			}
		} else
			Game.m_LastKeyUpdate = starttime;//0530mt

		// 画像ダウンロード
		MJWebExecute();
	#endif

		//タイマー処理が必要ない場合はこの行を削除してください。
	////	ISHELL_SetTimer( GETMYSHELL(), (TIMER_SEC/FPS), (PFNNOTIFY)app_main );
	//	ISHELL_SetTimer( GETMYSHELL(), (D_FLAME_TIME), (PFNNOTIFY)app_main );
		#if false //-*todo:要らない？
		if(m_Mode == D_BEFORE_ERR_MODE) {
			App_Mode_Err_Select();
			return;
		} else if( m_Mode == D_AFTER_ERR_MODE){
			//何もしない
			return;
		}
		#endif //-*todo:要らない？
		// フレームカウンタインクリメント
	//	m_frameCounter++;


		// msec/フレーム
		{
#if false //-*todo:旧方式
			long uptimems_ = GETUPTIMEMS();
			uptimems_perfrm = (ulong)uptimems_ - pre_uptimems;
			pre_uptimems = (ulong)uptimems_;
#else //-*todo:旧方式
#region UNITY_ORIGINAL
			uptimems_perfrm = (ulong)(1/Time.deltaTime);	//-*Unity
#endregion //-*UNITY_ORIGINAL
#endif //-*todo:旧方式
		}
		//メインループ
		switch(m_Mode) {
//-**********todo:ここから必要物移植
		/************************************/
		/*	COM対戦	メニュー				*/
		/************************************/
		case INMJMODE.D_COM_MENU:
		{
			sGameData.byGameMode = MJDefine.GAMEMODE_FREE;				/*	フリーゲーム		*/
			//ルール選択初期化へ移行
			mode = INMJMODE.D_FREE_RULE_MODE;
			modeChange(mode);
			break;
		}
		/************************************/
		/*	COM対戦	フリー対戦				*/
		/************************************/
		// ルール選択表示
		case INMJMODE.D_FREE_RULE_MODE:
		{
			/* ロジック				*/
			mode = INMJMODE.D_FREE_RULE_EDIT_MODE;
			modeChange(mode);
			break;
		}
		// ルール確認表示
		case INMJMODE.D_FREE_RULE_EDIT_MODE:
		{
			mode = INMJMODE.D_FREE_NPC_SELLECT_MODE;
			modeChange(mode);
			break;
		}
		// 対戦相手選択表示
		case INMJMODE.D_FREE_NPC_SELLECT_MODE:
		{
			sel_buf[0] = 0;
			sel_buf[1] = 1;
			sel_buf[2] = 2;
			mode = INMJMODE.D_FREE_NPC_CHK_MODE;
			modeChange(mode);
			break;
		}
		// 対戦相手確認表示
		case INMJMODE.D_FREE_NPC_CHK_MODE:
		{
			MjModeInit();
		    MjHanchanInit();
			mode =INMJMODE.D_FREE_GAME_MODE;
			modeChange(mode);
			break;
		}
		/****************************/
		/*	ゲームメイン			*/
		/****************************/
		// ゲーム表示
		case INMJMODE.D_FREE_GAME_MODE: {
			// 描画するモードを。
			m_WhichDrawMode = 0;//-*D_DRAW_STANDALONE;

			/* ロジック				*/
			MjMain();
			#if true //-*todo:描画
			/* 描画					*/
			if(m_Mode!= INMJMODE.D_GAME_RESULT_MODE){ // 110513:After麻雀から移植
				MJ_GameDraw();
			} else {
				MJ_GameResultDraw();
			}
			#endif //-*todo:描画

			if(m_Mode==INMJMODE.D_FREE_GAME_REINIT_MODE) {
				/* ロジック				*/
				//おためし
				GameReset();
				#if false //-*todo:サウンド
				// 音楽再生
				//PlayMusic_Play(m_PlayMusicNo,BGM,true);
				if( m_BgmOffFlag != SOUND_OFF ){
					PlayMusic_2chPlay();
				}
				#endif //-*todo:サウンド
				// ゲーム再初期化時に描画初期化。
				//igarashi 局終了時にホワイトアウトする問題対処。
	//				IDISPLAY_ClearScreen( GETMYDISPLAY() );
				m_Mode=INMJMODE.D_FREE_GAME_MODE;
			}
			break;
		}
#if true//-*todo:描画

	// --- ゲームリザルト画面・描画 ---
		case INMJMODE.D_GAME_RESULT_MODE: {
			MJ_GameResultDraw();

			#if true //-*todo:通信
				// スタンドアローン。
				if( m_Mode== INMJMODE.D_TITLE_MODE ) {
					// 解放処理。
					MJ_GameDrawFree();
					#if false //-*todo:不要
					//-*タイトル初期化かな
					MJ_TitleDrawInit();
					#endif //-*todo:不要
//-************
//-*todo:ここでシーンチェン(麻雀→シナリオ)
//-************
					break;
				}
			#else //-*todo:通信
			// 通信対戦時。
			if( WhichDrawMode == INMJMODE.D_DRAW_NETWORK_MODE ) {
				// > 2006/02/04 No.68
				#if false //-*todo:キー操作
				if ( D_ONEPUSH_INPUT_CLEAR ) {
					// 解放処理。
					MJ_GameDrawFree();

					//コネクションを切断する
					MpsBrewLib_Release(/*m_brewMps*/);

					//無操作監視タイマーリセット
	//xxxx			ISHELL_SetTimerEx(GETMYSHELL(), TIME_WARN_DISCONNECT, idle_disconnect.timer_callback);

					modeChange(INMJMODE.D_USER_DISCONNECT_MODE);
				}
				#endif //-*todo:キー操作
				if( m_Mode==INMJMODE.D_TITLE_MODE ) {
					// 解放処理。
					MJ_GameDrawFree();
					MJ_TitleDrawInit();

					// 通信対戦後は段位の結果描画を行う。
					modeChange( INMJMODE.D_NET_RANK_RESULT_DRAW_MODE);
					break;
				}
			} else {
				// スタンドアローン。
				if( m_Mode== D_TITLE_MODE ) {
					// 解放処理。
					MJ_GameDrawFree();
					MJ_TitleDrawInit();
					break;
				}
			}
			#endif //-*todo:通信

			// サバイバルの場合、このモードの後に
			// 『D_SURIVIVAL_RESULT_MODE』へ移行する。
			break;
		}
#endif //-*todo:描画
		}

//-**********todo:ここまで必要物移植

#if false //-*todo:不要
		case 101 :
			modeChange( 54 );// ＵＲＬジャンプ後,再認証対応
			break;
		/************************************/
		/*	ロゴ表示						*/
		/************************************/
		case D_LOGO_MODE: {
	//m		m_debugCnt = 0;

	#if	__JarDiet00
			mode = MJ_RogoDrawMain();
			MJ_RogoDraw();

			if( mode != D_LOGO_MODE ) {
				// barのアンロード。
				MJ_IIMAGE_Release( pIBar );
				// タイトル表示に移行
				modeChange( mode);

				// ロゴイメージ削除
				MJ_Sprite_Release(sprite[D_KOEI_MPS_LOGO]);
	//			break;
			}
	#else
			mode = D_TITLE_MODE;
			modeChange( mode);
	#endif
			break;
		}
		// 中断後、タイトルへ戻る処理
		case 100 :
			modeChange(1);
			break;

		/************************************/
		/*	定期的に通信を・・・			*/
		/************************************/
		case D_QUERY_NET_CONNECT_MODE:	//54
			mode = MJ_NetWorkConnect();
			MJ_NetWorkConnectDraw();

			if((mode != D_QUERY_NET_CONNECT_MODE) && (mode == D_TITLE_MODE)) {
				// タイトル表示に移行
				modeChange( mode);
				break;
			}
			break;
		/************************************/
		/*	タイトル表示					*/
		/************************************/
		case D_TITLE_MODE: {
	#if	true	//mm
			game_player = 0;
			mode = D_COM_MENU;
	#else
			mode = MainMenuMain();
			MJ_TitleDraw();
	#endif
			switch(mode) {
			// 認証なしのモード。
			case D_TITLE_MODE:
				break;
			case D_GAME_END_MODE:
				removeSoftkey();			// バグNo5000　修正　大塚
			case D_OPTION_SET_MODE:
				/* リソース開放処理	*/
				MJ_IIMAGE_Release( g_pIImage );
				// barのアンロード。
				MJ_IIMAGE_Release( pIBar );
				/* モード遷移 */
				modeChange(mode);

				MJ_Sprite_Release(sprite[D_TITLE]);
	#if _TITLE_ON_
				MJ_Sprite_Release(sprite[D_TITLE_MENU]);
	#else
			BYTE title_menu_cnt=0;
			for( title_menu_cnt=0 ; title_menu_cnt<2 ; title_menu_cnt++ )	//No1005 for( title_menu_cnt=0 ; title_menu_cnt<3 ; title_menu_cnt++ )
			{
				MJ_Sprite_Release(sprite[D_TITLE_TOGLE_NET+title_menu_cnt]);
			}
	#endif
				// バグNo1005 修正 大塚
				// ルールイメージ読み込み
				MJ_RuleSelectMainDrawInit();

				break;
			// 認証ありのモード。
			case D_NET_FREE_ENTRY_START_MODE:
			case D_COM_MENU:
			default:
				/* モード遷移 */
				modeChange(mode);
				break;
			}
			break;
		}
		// > 2006/02/18 要望No.41
		/************************************/
		/*	再接続ダイアログ				*/
		/************************************/
		case D_ASK_RETRY_CONNECT_MODE:
			{
				// ロジック
				mode = AskRetryConnectMain();

				if ( mode != D_ASK_RETRY_CONNECT_MODE ) {
					// リソース解放
					MJ_IIMAGE_Release(g_pIImage);
					MJ_IIMAGE_Release(pIBar);
					// モード遷移
					modeChange(mode);
				} else {
					// 描画
					AskRetryConnectDraw();
				}
			}
			break;
		// < 2006/02/18 要望No.41

		/************************************/
		/*	オプション設定					*/
		/************************************/
		case D_OPTION_SET_MODE:
		{

			// ロジック。
			mode = OptionSetMain();

			// 描画。
			OptionSetDraw();

			if(mode != D_OPTION_SET_MODE) {
				// リソース開放処理
				MJ_IIMAGE_Release( g_pIImage );	// ルール用。

				MJ_IIMAGE_Release( pIBar );		//

				// バグNo1005 修正 大塚 start
				// ルールイメージ解放
				for(int i=0 ; i<19 ; i++ )
					MJ_Sprite_Release(sprite[i]);

				// バグNo1005 修正 大塚 end

				// モード遷移
				modeChange(mode);
			}
			break;
		}
		/************************************/
		/*	ゲーム終了しますか？			*/
		/************************************/
		case D_GAME_END_MODE:
		case D_USER_DISCONNECT_MODE:
		{
			// --- ロジック。
			mode = GameEndMenuMain();

			// --- 描画。
			GameEndMenuDraw();

			if ( (mode != D_GAME_END_MODE) && (mode != D_USER_DISCONNECT_MODE) ) {	// 2006/02/04 No.68
				// リソース開放処理
				//IImageRelease();

				// リソース開放処理。
				MJ_IIMAGE_Release( g_pIImage );

				// barのアンロード。
				MJ_IIMAGE_Release( pIBar );

				// モード遷移
				modeChange(mode);
			}
			break;
		}
		/************************************/
		/*	COM対戦	メニュー				*/
		/************************************/
		case INMJMODE.D_COM_MENU:
		{
	#if	true	//mm
			sGameData.byGameMode = MJDefine.GAMEMODE_FREE;				/*	フリーゲーム		*/
			//ルール選択初期化へ移行
			mode = INMJMODE.D_FREE_RULE_MODE;
			modeChange(mode);
	#else
			/* ロジック				*/
			mode = ComMenuMain();

			/* 描画					*/
			MJ_ComMenuDraw( STANDALONE );

			if(mode != D_COM_MENU)
			{
				/* リソース開放処理	*/
				MJ_IIMAGE_Release( g_pIImage );

				// barのアンロード。
				MJ_IIMAGE_Release( pIBar );

				/* モード遷移 */
				modeChange(mode);
			}
	#endif
			break;
		}

		/************************************/
		/*	通信対戦	メニュー			*/
		/************************************/
		case D_NET_MENU:
		{
			// ロジック。
			// 0601mt 通信対戦封印 mode = NetMenuMain();
			// 0601mt 通信対戦封印
			// 0601mt 通信対戦封印 // 描画。
			// 0601mt 通信対戦封印 MJ_ComMenuDraw( NETWORK );		// ｽﾀﾝﾄﾞｱﾛﾝと通信用で兼ねている。

			mode = MJ_NetMenuDraw(  );//0601mt

			if(mode != D_NET_MENU)
			{
				// リソース開放処理。
				MJ_IIMAGE_Release( g_pIImage );

				// barのアンロード。
				MJ_IIMAGE_Release( pIBar );

				// モード遷移
				modeChange(mode);
			}
			break;
		}

		/************************************/
		/*	通信対戦	再接続メニュー		*/
		/************************************/
		case D_NET_RETRY_CONNECT_MODE:
		{
			// 再接続ロジック。
			mode = MJ_NetRetryConnectMain();

			if( mode != D_NET_RETRY_CONNECT_MODE )
			{
				modeChange( mode );
				break;
			}
			break;
		}

		// 再接続待ち中・・・描画。
		case D_NET_RETRY_CONNECT_WAIT_MODE:
		{
			mode = MJ_NetRetryConnectWaitMain();

			// ～受信中・・・描画。
			MJ_NetRetryConnectWaitDraw();

			if( mode != D_NET_RETRY_CONNECT_WAIT_MODE ) {
				MJ_IIMAGE_Release( pIGame[D_GAME_TAKU] );
				modeChange( mode );
			}
		}
		break;
		/************************************/
		/*			  卓指定対戦			*/
		/************************************/
		// テーブル番号入力
		case D_NET_TABLE_NUMBER_INPUT_MODE:
		{

			// --- 卓指定モードロジック ---。
			mode = TableSelectModeMain();

			// --- 卓指定モード描画 ---。
			//0511mt app_main_done = false;//0504mt 仮
			TableSelectModeDraw();
			//0511mt app_main_done = true;//0504mt 仮

			if(mode != D_NET_TABLE_NUMBER_INPUT_MODE)
				// モード遷移。
				modeChange(mode);
			break;
		}

		// 通信　卓指定対戦　参加者待ち中
		case D_NET_RESERVE_GAME_ENTRY_WAIT_MODE: {
			// 背景
			MJ_DrawMainBG();

			// 初期化時に『対戦参加要求』の送信を行っている。
			break;
		}

		/************************************/
		/*	通信対戦	フリー対戦			*/
		/************************************/
		case D_NET_FREE_GAME_ENTRY_WAIT_MODE:
		{
			// 背景
			MJ_DrawMainBG();

			// 初期化時に『対戦参加』要求の送信。

			////////////////////////////////////////////////////////
			// MpsBrewLib_Handle_Messageへ移動
			// メッセージ受信処理はMpsBrewLib_Handle_Messageで行なう
			// こっちでは主にメッセージ待ち中の画面描画処理など
			////////////////////////////////////////////////////////
			////////////////////////////////////////////////////////
			// 処理の順序はmodeChangeでapp_mode.c内の初期化処理→
			// モードが変更され、次のフレームでココに入る→
			// ココの処理を終えてからMPSのproceedが行なわれ→
			// MpsBrewLib_Handle_Messageの処理が実行される
			////////////////////////////////////////////////////////
			break;
		}
		// 通信　フリー対戦　参加者待ち中
		case D_NET_FREE_MEMBER_WAIT_MODE:
		{
			// > 2006/02/04 No.68
			if ( D_ONEPUSH_INPUT_CLEAR ) {
				//コネクションを切断する
				MpsBrewLib_Release(/*m_brewMps*/);

				//無操作監視タイマーリセット
	//			ISHELL_SetTimerEx(GETMYSHELL(), TIME_WARN_DISCONNECT, idle_disconnect.timer_callback);

				modeChange(INMJMODE.D_USER_DISCONNECT_MODE);
			} else
			// < 2006/02/04 No.68
			{
				gMemberWaitTimeOut -= uptimems_perfrm;//D_FLAME_TIME;
				if( gMemberWaitTimeOut < 0 )
				{
					gMemberWaitTimeOut = 0;
				}

				// 通信対戦時の参加者待ちうけ描画。
				//0511mt app_main_done = false;//0504mt 仮
				MJ_NetMatchModeDraw( gRootMode );
				//0511mt app_main_done = true;//0504mt 仮
			}
			break;
		}

		// 通信　マッチングタイムアウト
		case D_NET_MATCHING_TIMEOUT_MODE:
			{
				// > 2006/02/08 要望No.35
				MJ_Net_MatchingTimeoutDraw();	// 処理
				MJ_NetMatchingTimeoutMain();		// 描画
				// < 2006/02/08 要望No.35
			}
			break;

		// 通信　フリー対戦　対戦開始待ち
		case D_NET_FREE_GAME_START_WAIT_MODE:
		{
			// 背景
			MJ_DrawMainBG();

			if ( D_ONEPUSH_INPUT_CLEAR ) {
				//コネクションを切断する
				MpsBrewLib_Release(/*m_brewMps*/);

				//無操作監視タイマーリセット
	//			ISHELL_SetTimerEx(GETMYSHELL(), TIME_WARN_DISCONNECT, idle_disconnect.timer_callback);

				modeChange(INMJMODE.D_USER_DISCONNECT_MODE);
			}
			//実際の通信導入によりコメントアウト
			break;
		}

		// 通信　フリー対戦　対局開始待ち（ゲーム開始時）
		case D_NET_FREE_KYOKU_START_WAIT_MODE:
		{
			// 背景
			//MJ_DrawMainBG();
			//> 要望 No.33
			if ( D_ONEPUSH_INPUT_CLEAR ) {
				//コネクションを切断する
				MpsBrewLib_Release(/*m_brewMps*/);

				//無操作監視タイマーリセット
	//			ISHELL_SetTimerEx(GETMYSHELL(), TIME_WARN_DISCONNECT, idle_disconnect.timer_callback);

				modeChange(D_USER_DISCONNECT_MODE);
			}
			else
			{
				if( send_kyoku_ready_report_wait > 0 )
				{
					send_kyoku_ready_report_wait -= uptimems_perfrm;//D_FLAME_TIME;
					if( send_kyoku_ready_report_wait <= 0 )
					{
						KyokuReadyReportSnd();
						send_kyoku_ready_report_wait = 0;
					}
				}

				// 通信対戦時の参加者待ちうけ描画。
				//0511mt app_main_done = false;//0504mt 仮
				MJ_NetMatchModeDraw( gRootMode );
				//0511mt app_main_done = true;//0504mt 仮
				//< 要望 No.33
			}

			//実際の通信導入によりコメントアウト
			break;
		}

		// 通信　フリー対戦　対局開始待ち（次局開始時）
		case D_NET_FREE_KYOKU_START_WAIT_MODE2: {
			/* 描画					*/
			vscreen.setColor(MAKE_RGB( 49, 102,	90));
			vscreen.fillRect(0, 0, dispW, dispH);
	//m		MpsBrewLib_DrawSprite( spGame[D_GAME_JYANTAKU], 82, 97 );		//0, 0
			MpsBrewLib_DrawSprite( spGame[D_GAME_JYANTAKU], 0, 0 );
	//		MpsBrewLib_DrawSprite( spGame[D_GAME_JYANTAKU], 0+ 120* 0, 0+ 130* 0 );
	//		MpsBrewLib_DrawSprite( spGame[D_GAME_JYANTAKU], 0+ 120* 1, 0+ 130* 0 );
	//		MpsBrewLib_DrawSprite( spGame[D_GAME_JYANTAKU], 0+ 120* 0, 0+ 130* 1 );
	//		MpsBrewLib_DrawSprite( spGame[D_GAME_JYANTAKU], 0+ 120* 1, 0+ 130* 1 );
			MJ_GameSozaiDisp();
			//実際の通信導入によりコメントアウト
			break;
		}

		/************************************/
		/*	通信　フリー対戦ゲームメイン	*/
		/************************************/
		// ゲーム表示
		case D_NET_FREE_GAME_MODE: {
			// 描画するモードを。
			WhichDrawMode = D_DRAW_NETWORK_MODE;
			//0511mt app_main_done = false; //0507mt
			/* ロジック				*/
			MjMain();

			/* 描画					*/
			if(m_Mode!= D_GAME_RESULT_MODE){ // 110513:After麻雀から移植
				#if DOCOMO // 080714
				AppMain._lock();
				#endif
				MJ_GameDraw();
			} else {
				#if DOCOMO // 080714
				AppMain._lock();
				#endif
				MJ_GameResultDraw();
				#if DOCOMO // 080716
				AppMain.flagBattleEnd = true;
				#endif
			}

			// 再初期化処理はゲーム処理で条件分岐して行なって下さい igarashi
			if( m_Mode== D_TITLE_MODE ) {
				// 解放処理。
				MJ_GameDrawFree();
				break;
			} else if(m_Mode==D_FREE_GAME_REINIT_MODE) {
				// ロジック
				GameReset();

				// ゲーム再初期化時に描画初期化。
				//IDISPLAY_ClearScreen( GETMYDISPLAY() );

				mode=D_NET_FREE_KYOKU_START_WAIT_MODE2;

				//> 要望 No.33
				if( IS_NETWORK_MODE )
					KyokuReadyReportSnd();

				//< 要望 No.33

				// モード遷移
				modeChange(mode);
			}
			//0511mt app_main_done = true; //0507mt
			break;
		}

		/************************************/
		/*	通信対戦 段位認定戦				*/
		/************************************/
		case D_NET_RANK_DRAW_MODE:
		{

			if(RankMode == D_RANKMODE_DONE)
			{
				// リソースの解放。
				// barのアンロード。
				MJ_IIMAGE_Release( pIBar );

				modeChange( INMJMODE.D_NET_MENU );	// オート・卓指定の選択。
				break;
			}
			// --- ロジック。
			mode = RankDrawMain();

			// --- 段位描画。
			RankDraw(false);

			if ( mode != D_NET_RANK_DRAW_MODE )
			{
				// リソースの解放。
				MJ_IIMAGE_Release( pIBar );
				modeChange( mode );
				break;
			}

			break;
		}

		/************************************/
		/*	通信対戦 段位認定戦	結果		*/
		/************************************/
		case D_NET_RANK_RESULT_DRAW_MODE:
		{

			// --- ロジック。
			mode = MJ_RankWarResultMain();

			if( (mode!=D_NET_RANK_RESULT_DRAW_MODE) || (RankMode==D_RANKMODE_DONE) )	// 2006/02/05 No.400
			{
				// リソースの解放。
				MJ_IIMAGE_Release( pIBar );
				//modeChange( mode );
	//			modeChange( D_NET_RANKING_DRAW_MODE );
				modeChange(INMJMODE.D_NET_RANK_STATUS_DRAW_MODE);	// 段位情報表示 2006/02/17 要望No.13
				break;
			}

			// --- 昇格・降格 描画。
			MJ_RankWarResultDraw();


			break;
		}

		// > 2006/02/17 要望No.13
		/************************************/
		/*	通信対戦 段位情報表示			*/
		/************************************/
		case D_NET_RANK_STATUS_DRAW_MODE:
			{
				mode = MJ_NetDrawRankStatusMain();

				MJ_NetDrawRankStatus();

				if ( mode != D_NET_RANK_STATUS_DRAW_MODE ) {
					modeChange(mode);
				}
			}
			break;
		// < 2006/02/17 要望No.13

		/************************************/
		/*	通信対戦 総合順位描画			*/
		/************************************/
		case D_NET_RANKING_DRAW_MODE:
		{

			mode = MJ_NetDrawRankingMain();

			MJ_NetDrawRanking();

			if( mode != D_NET_RANKING_DRAW_MODE )
			{
				modeChange( mode );
			}

			break;
		}

		/************************************/
		/*	COM対戦	フリー対戦				*/
		/************************************/
		// ルール選択表示
		case D_FREE_RULE_MODE:
		{
	#if	true	//mm
		KeyInfo.trigger= KEY_SELECT2;
		/* ロジック				*/
		mode = RuleMain();
		modeChange(mode);
	#endif
	#if	false	//mm
			/* ロジック				*/
			mode = RuleMain();
			// 描画。
			MJ_RuleSelectMainDraw();

			if(mode != D_FREE_RULE_MODE)
			{
				// リソース開放処理
				MJ_IIMAGE_Release( g_pIImage );
				/* モード遷移 */
				modeChange(mode);
				break;
			}
	#endif
			break;
		}
		// ルール確認表示
		case D_FREE_RULE_EDIT_MODE:
		{
	#if	true	//mm
			mode = D_FREE_NPC_SELLECT_MODE;
			modeChange(mode);
	#else
			/* ロジック				*/
			mode = RuleEditMain();
			/* 描画					*/
			MJ_RuleCheckMainDraw( TRUE );

			if(mode != D_FREE_RULE_EDIT_MODE)
			{
				// リソース開放処理
				MJ_IIMAGE_Release( g_pIImage );

				/* モード遷移 */
				modeChange(mode);
				break;
			}
	#endif
			break;
		}
		// 対戦相手選択表示
		case D_FREE_NPC_SELLECT_MODE:
		{
	#if	true	//mm
			sel_buf[0] = 0;
			sel_buf[1] = 1;
			sel_buf[2] = 2;
			mode = D_FREE_NPC_CHK_MODE;
			modeChange(mode);
	#else
			/* ロジック				*/
			mode = CselMain();

			if( mode == D_FREE_NPC_SELLECT_MODE )
			{
				MJ_CharSelectDraw();
			}

			// 解放処理が先に来ること。
			if(mode != D_FREE_NPC_SELLECT_MODE)
			{
				/* モード遷移 */
				modeChange(mode);
				break;
			}
			// --- 描画 ---
			// 描画の順番を変えてはいけない。
			//MJ_CharSelectDraw();
	#endif
			break;
		}
		// 対戦相手確認表示
		case D_FREE_NPC_CHK_MODE:
		{
	#if	true	//mm
		KeyInfo.trigger= KEY_SELECT2;
	#endif
			/* ロジック				*/
			mode = CselSure();

			if( mode == D_FREE_NPC_CHK_MODE )
			{
				MJ_CharChoiceDraw();
			}

			if(mode != D_FREE_NPC_CHK_MODE)
			{
				// リソース開放処理。
				if (g_pIImage != NULL )
				{
					//IIMAGE_Release(pIImage);	// タイトルの解放。
					//pIImage = NULL;
				}
				/* モード遷移 */
				modeChange(mode);
			}
			break;
		}
		/************************************/
		/*	COM対戦	サバイバル対戦			*/
		/************************************/
		// サバイバル戦。セーブデータロード。
		case D_SURVIVAL_LOAD_MODE: {
			// --- 分岐ロジック。
			mode = MJ_SurvivalLoadMain();

			if( mode != D_SURVIVAL_LOAD_MODE )
				// モード遷移
				modeChange(mode);
			break;
		}
		case D_SURVIVAL_KYOKU_LOAD_MODE: {
			// --- 分岐ロジック。
			mode = MJ_SurvivalKyokuShowMain();

			if( mode != D_SURVIVAL_KYOKU_LOAD_MODE ) {
				// モード遷移
				modeChange(mode);
			}
			break;
		}
	//	break;
		// オープニング表示
		case D_SURIVIVAL_OPENING_MODE:
		// オープニング２表示
		case D_SURIVIVAL_OPENING_NO2_MODE: {
			/* ロジック				*/
			SurvivalOpMain();
			break;
		}

		// ルール確認表示
		case D_SURIVIVAL_RULE_CHK_MODE: {
			/* ロジック				*/
			RuleCheckMain();
			// 詳細ルールの描画。
			MJ_RuleCheckMainDraw( FALSE );
			break;
		}

		// コメント表示
		case D_SURIVIVAL_COMMENT_MODE: {
			/* ロジック				*/
			SurvivalCommentMain();
			break;
		}

		// サバイバル戦　途中結果のセーブ。
		case D_SURVIVAL_TOCYU_SAVE_MODE: {
			// セーブロジック。
			mode = MJ_SurvivalSaveMain();

			if( mode != D_SURVIVAL_TOCYU_SAVE_MODE ) {
				modeChange(mode);

				// No1101 修正開始　大塚
				if(mode == D_TITLE_MODE){
					// 解放処理。
					MJ_GameDrawFree();
					MJ_SurvivalCharRelease( );
					MJ_SurvivalFloorRelease( );
					MJ_TitleDrawInit();
				}
				// No1101 修正終了　大塚
				break;
			}
			// セーブ描画。
			MJ_SurvivalSaveDraw();

			break;
		}

		// 結果表示
		case D_SURIVIVAL_RESULT_MODE:
		{
			/* ロジック				*/
			SurvivalResultMain();
			break;
		}
		// キャラゲット表示
		case D_SURIVIVAL_GET_CHAR_MODE:
		{
			/* ロジック				*/
			CharaGetMain();
			break;
		}
		// データ保存
		case D_SURIVIVAL_SAVE_MODE:
		{
			DebLog(("<< Save Survival Data >>"));

			//モードをCOMメニュー初期化へ移行する
			//> 2006/03/02 バグ No.155
			f_info.f_survival_kyoku_data.byKyokuSave = 0;
			MJ_SurvivalGameKyokuSave();
			MJ_SurvivalGameSaveMain();
			MJ_WriteFile( D_WORD_INFOFILE, f_info);		//MJ_WriteFile( pMe, D_WORD_INFOFILE, &pMe->f_info, sizeof(MJFileInfo));
			//> 2006/03/02 バグ No.155
			fileOpen();ssave01(false);ssave03(false);fileClose();
			modeChange( INMJMODE.D_COM_MENU );
			break;
		}

		/****************************/
		/*	ゲームメイン			*/
		/****************************/
		// ゲーム表示
		case D_FREE_GAME_MODE: {
			// 描画するモードを。
			WhichDrawMode = D_DRAW_STANDALONE;

			/* ロジック				*/
			MjMain();

			/* 描画					*/
			if(m_Mode!= D_GAME_RESULT_MODE){ // 110513:After麻雀から移植
				#if DOCOMO // 080714　
				AppMain._lock();
				#endif
				MJ_GameDraw();
			} else {
				#if DOCOMO // 080714
				AppMain._lock();
				#endif
				MJ_GameResultDraw();
				#if DOCOMO // 080716
				AppMain.flagBattleEnd = true;
				#endif
			}

			if(m_Mode==D_FREE_GAME_REINIT_MODE) {
				/* ロジック				*/
				//おためし
				GameReset();

				// 音楽再生
				//PlayMusic_Play(m_PlayMusicNo,BGM,true);
				if( m_BgmOffFlag != SOUND_OFF )
					PlayMusic_2chPlay();

				// ゲーム再初期化時に描画初期化。
				//igarashi 局終了時にホワイトアウトする問題対処。
	//				IDISPLAY_ClearScreen( GETMYDISPLAY() );
				m_Mode=D_FREE_GAME_MODE;
			}
			break;
		}
#endif //-*todo:不要
#if false//-*todo:描画

		// --- ゲームリザルト画面・描画 ---
		case D_GAME_RESULT_MODE: {
			MJ_GameResultDraw();

			// 通信対戦時。
			if( WhichDrawMode == D_DRAW_NETWORK_MODE ) {
				// > 2006/02/04 No.68
				if ( D_ONEPUSH_INPUT_CLEAR ) {
					// 解放処理。
					MJ_GameDrawFree();

					//コネクションを切断する
					MpsBrewLib_Release(/*m_brewMps*/);

					//無操作監視タイマーリセット
	//xxxx			ISHELL_SetTimerEx(GETMYSHELL(), TIME_WARN_DISCONNECT, idle_disconnect.timer_callback);

					modeChange(INMJMODE.D_USER_DISCONNECT_MODE);
				}

				if( m_Mode==D_TITLE_MODE ) {
					// 解放処理。
					MJ_GameDrawFree();
					MJ_TitleDrawInit();

					// 通信対戦後は段位の結果描画を行う。
					modeChange( INMJMODE.D_NET_RANK_RESULT_DRAW_MODE);
					break;
				}
			} else {
				// スタンドアローン。
				if( m_Mode== D_TITLE_MODE ) {
					// 解放処理。
					MJ_GameDrawFree();
					MJ_TitleDrawInit();
					break;
				}
			}

			// サバイバルの場合、このモードの後に
			// 『D_SURIVIVAL_RESULT_MODE』へ移行する。
			break;
		}
#endif //-*todo:描画
#if false//-*todo:不要

		/****************************/
		/*	通信　遷移				*/
		/****************************/

		/****************************/
		/*	通信：エントリ処理		*/
		/****************************/
		case D_NET_FREE_ENTRY_START_MODE: {
			//サーバーに接続中です．．．
			App_Mode_Entry();
			MJ_NowConnectingDraw( 0 );
			break;
		}
		case D_NET_FREE_ENTRY_WAIT_MODE:
		{
			//サーバーに接続中です．．．
			App_Mode_Entry_Wait();
			MJ_NowConnectingDraw( 0 );
			break;
		}
		case D_NET_FREE_ENTRY_REQ_MODE:
		{
			//サーバーに接続中です．．．
			App_Mode_Entry_Req();
			MJ_NowConnectingDraw( 0 );
			break;
		}
		case D_NET_FREE_ENTRY_RECV_MODE:
		{
			//サーバーに接続中です．．．
			App_Mode_Entry_Recv();
			MJ_NowConnectingDraw( 0 );
			break;
		}
		/****************************/
		/*	通信：ログイン処理		*/
		/****************************/
		case D_NET_FREE_LOGIN_START_MODE:
		{
			//サーバーに接続しますか
			App_Mode_Login();
			MJ_NowConnectingDraw( 0 );
			break;
		}
		case D_NET_FREE_LOGIN_WAIT_MODE:
		{
			//サーバーに接続中です．．．
			App_Mode_Login_Wait();
			MJ_NowConnectingDraw( 0 );
			break;
		}
		case D_NET_FREE_LOGIN_REQ_MODE:
		{
			//ログイン要求送信
			App_Mode_Login_Req();
			MJ_NowConnectingDraw( 1 );
			break;
		}
		case D_NET_FREE_LOGIN_RECV_MODE:
		{
			//ログイン応答受信
			App_Mode_Login_Recv();
			MJ_NowConnectingDraw( 1 );
			break;
		}
		// 切断メッセージモード。
		case D_NET_DISCONNECT_MODE:
		{
			// ロジック。
			mode = MJ_Net_DisConnectMain();

			if ( mode != D_NET_DISCONNECT_MODE ) {
				modeChange( mode);
			} else {
				// 描画。
				MJ_Net_DisConnectDraw();
			}
			break;
		}
		case D_NET_ERR_MODE:
		{
			mode = App_Mode_Protocol_Err_Main();

			if ( mode != D_NET_ERR_MODE )
			{
				modeChange( mode);
			}
			else
			{
				App_Mode_Protocol_Err_Draw();
			}
		}
		break;
		/****************************/
		/*	そのた					*/
		/****************************/
		case D_CONN_ERR_MODE:
		{
			// 接続失敗ロジック。
			mode = App_Mode_Conn_Err_Main();

			//接続失敗描画。
			App_Mode_Conn_Err_Draw();

			if( mode != D_CONN_ERR_MODE )
			{
				modeChange( mode );
			}

			break;
		}
		case D_ERR_MODE :
		{
	//		ASSERT( !"D_ERR_MODE" );

			//接続失敗
			mode = App_Mode_Err_Main();

			//接続失敗描画。
			App_Mode_Err_Draw();

			if( mode != D_ERR_MODE )
			{
				modeChange( mode );
			}
			break;
		}
		case D_SUBMIT_ERR_MODE :
		{
	//xxxx		//認証失敗
	//xxxx		mode = App_Submit_Err_Main();
	//xxxx
	//xxxx		//認証失敗描画。
	//xxxx		App_Submit_Err_Draw();
	//xxxx
	//xxxx		if( mode != D_SUBMIT_ERR_MODE )
	//xxxx		{
	//xxxx			if(submiterr != PREMIUM_ERROR_NEVER)	// 2006/02/27 要望No.731
	//xxxx			{
	//xxxx				submiterr = PREMIUM_ERROR_NEVER;	// 2006/02/27 要望No.731
	//xxxx				modeChange( mode );
	//xxxx			}
	//xxxx			else
	//xxxx			{
	//xxxx				modeChange( mode );
	//xxxx				GoHomePage( HOMEPAGE_TOP_URL );
	//xxxx			}
	//xxxx		}
			break;
		}
		case D_TIMER_DISCONNECT_MODE:
		{
	m_DrawMode = 2;//0530mt
	mode = MJ_Net_DisConnectMain();//0530mt
	if ( mode != 35 ) {//0530mt
	modeChange( mode);//0530mt
	} else {//0530mt
		MJ_Net_DisConnectDraw();//0530mt
	}//0530mt
			break;
		}
		default:
			break;
		}
#endif //-*todo:不要

		// クリアキーが押された時。
		//if( D_ONEPUSH_INPUT_CLR )
		//{
			//modeChange( D_TITLE_MODE);
		//}
#if false //-*todo:不要
		if((m_Mode != D_LOGO_MODE) && (m_Mode != D_QUERY_NET_CONNECT_MODE)) {
			/* ＊キーが押下された場合 */
			if(D_ONEPUSH_INPUT_STAR) {
				DebLog(("Sound Change!"));
				/* BGMがONの場合 */
				if( (m_BgmOffFlag==SOUND_MAX) || (m_BgmOffFlag==SOUND_ON) ) {
					m_BgmOffFlag=SOUND_OFF;
					m_SeOffFlag	= SOUND_OFF;
					f_info.f_optionsetting.m_OptionSel[0] = SOUND_OFF;	// オプション設定用。

	//				if((AppSound[SOUND_CHANNEL_BGM1] != NULL) && (AppSound[SOUND_CHANNEL_BGM2] != NULL))
	//				{
						PlayMusic_2chStop();
	//				}
				} else if( m_BgmOffFlag==SOUND_OFF ) {
					m_BgmOffFlag=SOUND_MIN;
					m_SeOffFlag	= SOUND_MIN;
					f_info.f_optionsetting.m_OptionSel[0] = SOUND_MIN;	// オプション設定用。
					PlayMusic_2chPlay();
	//				if((AppSound[SOUND_CHANNEL_BGM1] != NULL) && (AppSound[SOUND_CHANNEL_BGM2] != NULL))
	//				{
						PlayMusic_2chChangeVolume( SOUND_VOLUME_MIN );
	//				}
				} else if( m_BgmOffFlag==SOUND_MIN ) {
					m_BgmOffFlag=SOUND_MID;
					m_SeOffFlag	= SOUND_MID;
					f_info.f_optionsetting.m_OptionSel[0] = SOUND_MID;	// オプション設定用。
	//				if((AppSound[SOUND_CHANNEL_BGM1] != NULL) && (AppSound[SOUND_CHANNEL_BGM2] != NULL))
	//				{
						PlayMusic_2chChangeVolume( SOUND_VOLUME_MID );
	//				}
				} else {
					m_BgmOffFlag=SOUND_MAX;
					m_SeOffFlag	= SOUND_MAX;
					f_info.f_optionsetting.m_OptionSel[0] = SOUND_MAX;	// オプション設定用。
	//				if((AppSound[SOUND_CHANNEL_BGM1] != NULL) && (AppSound[SOUND_CHANNEL_BGM2] != NULL))
	//				{
						PlayMusic_2chChangeVolume( SOUND_VOLUME_MAX );
	//				}
				}

				//// 通信対戦モードで無ければセーブデータ更新
				//if( !IS_NETWORK_MODE )
				//{
				//	MJ_WriteFile( D_WORD_INFOFILE, f_info);
				//}
			}
		}
#endif //-*todo:不要
#if false //-*todo:描画
		// 顔画像メッセージ表示
		MJWebDraw();
#endif //-*todo:描画
		// ネット接続中無操作警告表示 // 2006/02/06 No.26
	//0507mt	WarnIdleDraw();

	// デバッグ文字列画面表示
	#if _DEVELOP
		if( GetKeyTrg(KEY_0) ) {
			if( dbgon == 1 ) {
				if( GetKeyTrg(KEY_0) ) {
					dbgselect = 1;
					dbgopt[ dbgselect ]++;
					if( dbgopt[ dbgselect ] == 8 ) {
						dbgopt[ dbgselect ] = 0;
						dbgon = 0;
					}
				}
			} else
				dbgon = 1;
		}

		if( dbgon == 1 ) {
			// ゲーム時デバッグ表示メイン
	//		AEERect	dbgrect_;
			int dbg_x_=0;
			int dbg_y_=0;
			int dbg_offset_ = 18;
			char dbgstr_[]= new chr[64];	//dbgstr_[64]={0};

	//		IDISPLAY_SetColor(GETMYDISPLAY(),CLR_USER_TEXT,MAKE_RGB(255,0,0));

			switch( dbgopt[1] ) {
				case 0 : {
					SETAEERECT(&dbgrect_, 0, 0,pMe->m_screenWidth, dbg_offset_ * 9 );
					IDISPLAY_DrawRect(GETMYDISPLAY(), &dbgrect_, MAKE_RGB(0,0,0), MAKE_RGB(0,0,0), IDF_RECT_FILL );

					SPRINTF( dbgstr_,"Ff[%d] %d:%d:%d:%d:%d:%d:%d",
						Order,Ff[0x31],Ff[0x32],Ff[0x33],Ff[0x34],Ff[0x35],Ff[0x36],Ff[0x37] );
					MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					//--
					SPRINTF( dbgstr_,"JT %02x MNT %02x:%02x:%02x:%02x",
						sMntData.byJanto,sMntData.byMnt[0],sMntData.byMnt[1],sMntData.byMnt[2],sMntData.byMnt[3] );
					MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					SPRINTF( dbgstr_,"MT %02x:%02x TYP %02x:%02x:%02x:%02x",
						sMntData.byMachi,sMntData.byMachiMnt,sMntData.byType[0],
						sMntData.byType[1],sMntData.byType[2],sMntData.byType[3] );
					MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					//--
					SPRINTF( dbgstr_,"JT %02x MNT %02x:%02x:%02x:%02x",
						sMntDataSub[0].byJanto,sMntDataSub[0].byMnt[0],
						sMntDataSub[0].byMnt[1],sMntDataSub[0].byMnt[2],sMntDataSub[0].byMnt[3] );
					MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					SPRINTF( dbgstr_,"MT %02x:%02x TYP %02x:%02x:%02x:%02x",
						sMntDataSub[0].byMachi,sMntDataSub[0].byMachiMnt,sMntDataSub[0].byType[0],
						sMntDataSub[0].byType[1],sMntDataSub[0].byType[2],sMntDataSub[0].byType[3] );
					MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					//--
					SPRINTF( dbgstr_,"JT %02x MNT %02x:%02x:%02x:%02x",
						sMntDataSub[1].byJanto,sMntDataSub[1].byMnt[0],
						sMntDataSub[1].byMnt[1],sMntDataSub[1].byMnt[2],sMntDataSub[1].byMnt[3] );
					MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					SPRINTF( dbgstr_,"MT %02x:%02x TYP %02x:%02x:%02x:%02x",
						sMntDataSub[1].byMachi,sMntDataSub[1].byMachiMnt,sMntDataSub[1].byType[0],
						sMntDataSub[1].byType[1],sMntDataSub[1].byType[2],sMntDataSub[1].byType[3] );
					MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					//--
					SPRINTF( dbgstr_,"JT %02x MNT %02x:%02x:%02x:%02x",
						sMntDataSub[2].byJanto,sMntDataSub[2].byMnt[0],
						sMntDataSub[2].byMnt[1],sMntDataSub[2].byMnt[2],sMntDataSub[2].byMnt[3] );
					MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					SPRINTF( dbgstr_,"MT %02x:%02x TYP %02x:%02x:%02x:%02x",
						sMntDataSub[2].byMachi,sMntDataSub[2].byMachiMnt,sMntDataSub[2].byType[0],
						sMntDataSub[2].byType[1],sMntDataSub[2].byType[2],sMntDataSub[2].byType[3] );
					MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );
				}
				break;
				case 1 :
				{
	//				SETAEERECT(dbgrect_, 0, 0,m_screenWidth, dbg_offset_ * 5 );
	//				IDISPLAY_DrawRect(GETMYDISPLAY(), dbgrect_, MAKE_RGB(0,0,0), MAKE_RGB(0,0,0), IDF_RECT_FILL );

					SPRINTF( dbgstr_,"fps:%lu,MODE:%2d",
						uptimems_perfrm == 0 ? 0 : 1000 / uptimems_perfrm, m_Mode);
					MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					//SPRINTF( dbgstr_,"S %8d R %8d", total_send_size, total_recv_size );
					//MpsBrewLib_DrawString( dbgstr_, dbg_x_ + 10, dbg_y_ += dbg_offset_ );

					SPRINTF( dbgstr_,"M1:%3d:%3d ST %d%d%d%d%d%d%d%d", reentry_m1_bflag, comput_m1_bflag,
						(Status >> 0) & 0x01,(Status >> 1) & 0x01,(Status >> 2) & 0x01,(Status >> 3) & 0x01,
						(Status >> 4) & 0x01,(Status >> 5) & 0x01,(Status >> 6) & 0x01,(Status >> 7) & 0x01 );
					MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					//SPRINTF( dbgstr_,"P1:%d P2:%d C:%d CH:%d",
					//	before_richi_m4_bflag,after_richi_m4_bflag,comput_m1_bflag,chakan_m6_bflag );
					//dbg_y_ += dbg_offset_;
					//IDISPLAY_DrawText(GETMYDISPLAY(),AEE_FONT_BOLD,
					//	dbgstr_,WSTRLEN(dbgstr_),V_X,dbg_y_+V_Y,NULL,IDF_TEXT_TRANSPARENT);

					SPRINTF( dbgstr_,"ME:%d ODR:%d:%d THK:%d P%2d ",
						game_player, Order, Odrbuf, gThinkResult, Pinch );
					MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					SPRINTF( dbgstr_,"場 %d 局 %d LAST %3d 槓 %d",
						gsTableData[0].byKyoku & 0x0f, gsTableData[0].byKyoku, Bpcnt+Kancnt, Kancnt );
					MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					//SPRINTF( dbgstr_,"KK %2x:%2x:%2x:%2x:",
					//	gsPlayerWork[0].bFkokus,gsPlayerWork[1].bFkokus,
					//	gsPlayerWork[2].bFkokus,gsPlayerWork[3].bFkokus);
					//dbg_y_ += dbg_offset_;
					//IDISPLAY_DrawText(GETMYDISPLAY(),AEE_FONT_BOLD,
					//	dbgstr_,WSTRLEN(dbgstr_),0,dbg_y_+V_Y,NULL,IDF_TEXT_TRANSPARENT);

					//SPRINTF( dbgstr_,"KAN %d", Kancnt );
					//dbg_y_ += dbg_offset_;
					//IDISPLAY_DrawText(GETMYDISPLAY(),AEE_FONT_BOLD,
					//	dbgstr_,WSTRLEN(dbgstr_),0,dbg_y_+V_Y,NULL,IDF_TEXT_TRANSPARENT);

					//SPRINTF( dbgstr_,"AGARI %d ODBF:%d NF %02x", agari_sts,Odrbuf, Totsunyu_flg );
					//dbg_y_ += dbg_offset_;
					//IDISPLAY_DrawText(GETMYDISPLAY(),AEE_FONT_BOLD,
					//	dbgstr_,WSTRLEN(dbgstr_),0,dbg_y_+V_Y,NULL,IDF_TEXT_TRANSPARENT);

					//MpsBrewLib_DrawString( dbgstr_, dbg_x_ + 10, dbg_y_ += dbg_offset_ );

					//SPRINTF( dbgstr_,"RONCHK %d KEY 0x%02x",	gRonChkResult, keyin_m6_bflag );
					//MpsBrewLib_DrawString( dbgstr_, dbg_x_ + 10, dbg_y_ += dbg_offset_ );

					// メニューオプション
					//SPRINTF( dbgstr_,"OPTSTK[%d][%d][%d][%d][%d]",
					//	Optstk[0], Optstk[1], Optstk[2],Optstk[3], Optstk[4] );
					//MpsBrewLib_DrawString( dbgstr_, dbg_x_ + 10, dbg_y_ += dbg_offset_ );

					//SPRINTF( dbgstr_,"突入 0x%02x", Totsunyu_flg );
					//MpsBrewLib_DrawString( dbgstr_, dbg_x_ + 10, dbg_y_ += dbg_offset_ );

					// 待ち時間デバッグ表示
					//SPRINTF( dbgstr_,"NB:%4d NP:%4d",naki_wait_base,naki_wait_param );
					//MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					// ツモ時間デバッグ表示
					//SPRINTF( dbgstr_,"TB:%4d TP%4d",tsumo_wait_base,tsumo_wait_param);
					//MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					// タイムアウトデバッグ表示
					//SPRINTF( dbgstr_,"NAKI:%4d:TSUMO:%4d:",gNakiTimeOut,gTsumoTimeOut );
					//MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					//SPRINTF( dbgstr_,"NAKICHK:%d:NAKI:%d:%d:%d:%d",
					//	gNakiChkResult,gNakiResult[0],gNakiResult[1],gNakiResult[2],gNakiResult[3] );
					//MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );
				}
				break;
				case 3 : {
	//				SETAEERECT(dbgrect_, 0, 0,m_screenWidth, dbg_offset_ * 5 );
	//				IDISPLAY_DrawRect(GETMYDISPLAY(), dbgrect_, MAKE_RGB(0,0,0), MAKE_RGB(0,0,0), IDF_RECT_FILL );

					SPRINTF( dbgstr_,"親%2d:自%2d:起%2d:数%d",
						gsTableData[0].byOya,
						game_player,
						( gsTableData[0].byOya + 4 - game_player ) % 4,
						gNetTable.Membercnt );
					MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					SPRINTF( dbgstr_,"FLG %2d:%d,%2d:%d,%2d:%d,%2d:%d",
						gsTableData[0].sMemData[0].byMember,gsPlayerWork[0].byPlflg,
						gsTableData[0].sMemData[1].byMember,gsPlayerWork[1].byPlflg,
						gsTableData[0].sMemData[2].byMember,gsPlayerWork[2].byPlflg,
						gsTableData[0].sMemData[3].byMember,gsPlayerWork[3].byPlflg );
					MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					// オプションフラグ関連
					SPRINTF( dbgstr_,"MY AI %d NAKI %d", gDaiuchiFlag, gMyTable.AIFlag );
					MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					SPRINTF( dbgstr_,"AI %2d:%d,%2d:%d,%2d:%d,%2d:%d",
						gNetTable.NetChar[0].AIFlag,gNetTable.NetChar[0].Operator,
						gNetTable.NetChar[1].AIFlag,gNetTable.NetChar[1].Operator,
						gNetTable.NetChar[2].AIFlag,gNetTable.NetChar[2].Operator,
						gNetTable.NetChar[3].AIFlag,gNetTable.NetChar[3].Operator);
					MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );
				}
				break;
				case 4:
				{
	//				SETAEERECT(dbgrect_, 0, 0,m_screenWidth, dbg_offset_ * 3 );
	//				IDISPLAY_DrawRect(GETMYDISPLAY(), dbgrect_, MAKE_RGB(0,0,0), MAKE_RGB(0,0,0), IDF_RECT_FILL );

					SPRINTF( dbgstr_, "NAKI  %d:%d:%d:%d",
							gNakiResult[ 0 ],gNakiResult[ 1 ],
							gNakiResult[ 1 ],gNakiResult[ 3 ] );
					MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					SPRINTF( dbgstr_, "OPT %d",Optcnt );
					MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					//// ダブロン情報
					//MJK_RESULT *result_ = gMJKResult;
					//dbg_x_ = 0;

					//SETAEERECT(dbgrect_, 0, 0,m_screenWidth, dbg_offset_ * 14 );
					//IDISPLAY_DrawRect(GETMYDISPLAY(), dbgrect_, MAKE_RGB(0,0,0), MAKE_RGB(0,0,0), IDF_RECT_FILL );

					//SPRINTF( dbgstr_,"AGARI %d",Order );
					//MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					//SPRINTF( dbgstr_,"YAKUCNT %d REN %d",result_->byYakuCnt, result_->byRenchan );
					//MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					//SPRINTF( dbgstr_,"OYA %d KO %d TOTAL %d",
					//	result_->nOyaPoint, result_->nKoPoint, result_->nTotalPoint );
					//MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					//SPRINTF( dbgstr_,"RESULT1 %6d",result_->sMemResult[0].iPoint );
					//MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					//SPRINTF( dbgstr_,"RESULT2 %6d",result_->sMemResult[1].iPoint );
					//MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					//SPRINTF( dbgstr_,"RESULT3 %6d",result_->sMemResult[2].iPoint );
					//MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					//SPRINTF( dbgstr_,"RESULT4 %6d",result_->sMemResult[3].iPoint );
					//MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					//SPRINTF( dbgstr_,"HAN %d FU %d YAKUSU %d",
					//			result_->byHan, result_->byFu, result_->byYakuCnt );
					//MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					//SPRINTF( dbgstr_,"YAKU1 %02x:%02x:%02x:%02x:%02x:%02x",
					//			result_->sYaku[0].name,result_->sYaku[1].name,result_->sYaku[2].name,
					//			result_->sYaku[3].name,result_->sYaku[4].name,result_->sYaku[5].name );
					//MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					//SPRINTF( dbgstr_,"YAKU1 %02x:%02x:%02x:%02x:%02x:%02x",
					//			result_->sYaku[6].name,result_->sYaku[7].name,result_->sYaku[8].name,
					//			result_->sYaku[9].name,result_->sYaku[10].name,result_->sYaku[11].name );
					//MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );
				}
				break;
				case 5:
				{
	//				SETAEERECT(dbgrect_, 0, 0,m_screenWidth, dbg_offset_ * 6 );
	//				IDISPLAY_DrawRect(GETMYDISPLAY(), dbgrect_, MAKE_RGB(0,0,0), MAKE_RGB(0,0,0), IDF_RECT_FILL );

					// オプションフラグ関連
					SPRINTF( dbgstr_,"TALK %d SOUND %d:%d", opt_game[GOPT_TLK], m_BgmOffFlag, m_SeOffFlag );
					MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					if( gsPlayerWork[0] && gsPlayerWork[1] && gsPlayerWork[2] && gsPlayerWork[3] )
					{
						// 振聴フラグの確認
						SPRINTF( dbgstr_,"FURI %02x:%02x:%02x:%02x",
							gsPlayerWork[0].bFfurit,gsPlayerWork[1].bFfurit,gsPlayerWork[2].bFfurit,gsPlayerWork[3].bFfurit );
						MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

						// 聴牌待ち牌数
						SPRINTF( dbgstr_,"TENPAI[%d][%d][%d][%d]",
							gsPlayerWork[0].byTenpai,gsPlayerWork[1].byTenpai,gsPlayerWork[2].byTenpai,gsPlayerWork[3].byTenpai );
						MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

						// 流し満貫フラグ
						SPRINTF( dbgstr_,"NAGASI %2x:%2x:%2x:%2x:",
							gsPlayerWork[0].bFnagas,gsPlayerWork[1].bFnagas,
							gsPlayerWork[2].bFnagas,gsPlayerWork[3].bFnagas);
						MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

						// 鳥・ドボンフラグ
						SPRINTF( dbgstr_,"MEMFLAG %02x:%02x:%02x:%02x",
							gsTableData[0].sMemData[0].byMemFlags,gsTableData[0].sMemData[1].byMemFlags,
							gsTableData[0].sMemData[2].byMemFlags,gsTableData[0].sMemData[3].byMemFlags
							);
						MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );
					}

					//// 複数ロンが合った場合の結果
					//int i = 0;
					//dbg_x_ = 0;

					//SETAEERECT(dbgrect_, 0, 0,m_screenWidth, dbg_offset_ * 14 );
					//IDISPLAY_DrawRect(GETMYDISPLAY(), dbgrect_, MAKE_RGB(0,0,0), MAKE_RGB(0,0,0), IDF_RECT_FILL );

					//SPRINTF( dbgstr_,"RON %d %02x:%02x:%02x:%02x",
					//		Roncnt, Hanetbl[0],Hanetbl[1],Hanetbl[2],Hanetbl[3] );
					//MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					//for( i = 0; i < 4; ++i )
					//{
					//	MJK_RESULT *result_ = gMJKResultSub[i];

					//	SPRINTF( dbgstr_,"HAN %d FU %d YAKUSU %d",
					//			 result_->byHan, result_->byFu, result_->byYakuCnt );
					//	MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					//	SPRINTF( dbgstr_,"YAKU1 %02x:%02x:%02x:%02x:%02x:%02x",
					//			 result_->sYaku[0].name,result_->sYaku[1].name,result_->sYaku[2].name,
					//			 result_->sYaku[3].name,result_->sYaku[4].name,result_->sYaku[5].name );
					//	MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					//	SPRINTF( dbgstr_,"YAKU1 %02x:%02x:%02x:%02x:%02x:%02x",
					//			 result_->sYaku[6].name,result_->sYaku[7].name,result_->sYaku[8].name,
					//			 result_->sYaku[9].name,result_->sYaku[10].name,result_->sYaku[11].name );
					//	MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );
					//}
				}
				break;
				case 6 :
				{
	//				SETAEERECT(dbgrect_, 0, 0,m_screenWidth, dbg_offset_ * 9 );
	//				IDISPLAY_DrawRect(GETMYDISPLAY(), dbgrect_, MAKE_RGB(0,0,0), MAKE_RGB(0,0,0), IDF_RECT_FILL );

					// 和了り状態
					SPRINTF( dbgstr_,"STATUS %3d KAMICYA %02x",
						agari_sts,gsTableData[0].byKamicha_dori
					);
					MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					// 順位
					SPRINTF( dbgstr_,"RANK %d:%d:%d:%d",
						gsTableData[0].sMemData[0].byRank,
						gsTableData[0].sMemData[1].byRank,
						gsTableData[0].sMemData[2].byRank,
						gsTableData[0].sMemData[3].byRank
					);
					MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					// ポイント
					SPRINTF( dbgstr_,"持点 %4d:%4d:%4d:%4d",
						gsTableData[0].sMemData[0].nPoint,
						gsTableData[0].sMemData[1].nPoint,
						gsTableData[0].sMemData[2].nPoint,
						gsTableData[0].sMemData[3].nPoint
					);
					MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					// 移動ポイント
					SPRINTF( dbgstr_,"MOVE %4d:%4d:%4d:%4d",
						gsTableData[0].sMemData[0].nMovePoint,
						gsTableData[0].sMemData[1].nMovePoint,
						gsTableData[0].sMemData[2].nMovePoint,
						gsTableData[0].sMemData[3].nMovePoint
					);
					MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					// 操作前ポイント
					SPRINTF( dbgstr_,"OLD  %4d:%4d:%4d:%4d",
						gsTableData[0].sMemData[0].nOldPoint,
						gsTableData[0].sMemData[1].nOldPoint,
						gsTableData[0].sMemData[2].nOldPoint,
						gsTableData[0].sMemData[3].nOldPoint
					);
					MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					// 和了り状態ポイント遷移
					// ポイント
					SPRINTF( dbgstr_,"PNT  %4d:%4d:%4d:%4d",
						gsTableData[0].sMemData[0].nPnt,
						gsTableData[0].sMemData[1].nPnt,
						gsTableData[0].sMemData[2].nPnt,
						gsTableData[0].sMemData[3].nPnt
					);
					MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					// 移動ポイント
					SPRINTF( dbgstr_,"MOVE %4d:%4d:%4d:%4d",
						gsTableData[0].sMemData[0].nMovePnt,
						gsTableData[0].sMemData[1].nMovePnt,
						gsTableData[0].sMemData[2].nMovePnt,
						gsTableData[0].sMemData[3].nMovePnt
					);
					MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					// チップ獲得の確認
					SPRINTF( dbgstr_,"CHIP %4d:%4d:%4d:%4d",
						gsTableData[0].sMemData[0].nChip,gsTableData[0].sMemData[1].nChip,
						gsTableData[0].sMemData[2].nChip,gsTableData[0].sMemData[3].nChip );
					MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );
				}
				break;
				case 7 :
				{
					USHORT i=0;

	//				SETAEERECT(dbgrect_, 0, 0,m_screenWidth, dbg_offset_ * 15 );
	//				IDISPLAY_DrawRect(GETMYDISPLAY(), dbgrect_, MAKE_RGB(0,0,0), MAKE_RGB(0,0,0), IDF_RECT_FILL );

					// ユーザールールの表示
					SPRINTF( dbgstr_,"USER RULE");
					MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					for( i = 0; i < 5; ++i )
					{
						SPRINTF( dbgstr_,"%02x,%02x,%02x,%02x,%02x,%02x,%02x,%02x",
							gbyUserRule[i*8+0],gbyUserRule[i*8+1],gbyUserRule[i*8+2],
							gbyUserRule[i*8+3],gbyUserRule[i*8+4],gbyUserRule[i*8+5],
							gbyUserRule[i*8+6],gbyUserRule[i*8+7]
							);
						MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );
					}
					SPRINTF( dbgstr_,"%02x,%02x",
						gbyUserRule[i*10+0],gbyUserRule[i*10+1]	);
					MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					// ルールの表示
					SPRINTF( dbgstr_,"RULE");
					MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );

					for( i = 0; i < 5; ++i ) {
						SPRINTF( dbgstr_,"%02x,%02x,%02x,%02x,%02x,%02x,%02x,%02x",
							Rultbl[i*8+0],Rultbl[i*8+1],Rultbl[i*8+2],
							Rultbl[i*8+3],Rultbl[i*8+4],Rultbl[i*8+5],
							Rultbl[i*8+6],Rultbl[i*8+7]
							);
						MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );
					}
					SPRINTF( dbgstr_,"%02x,%02x", Rultbl[i*8+0],Rultbl[i*8+1]	);
					MpsBrewLib_DrawString( dbgstr_, dbg_x_, dbg_y_ += dbg_offset_ );
				}
				break;
			}

			{
				int connstat_ = MpsBrewLib_CheckState( /*m_brewMps*/ );
	//			SPRINTF( dbgstr_,"DBG%d:%d%d%d%d%d%d%d%d MPS:%d:%d:%d",
	//			dbgselect, dbgopt[ 0 ],dbgopt[ 1 ],dbgopt[ 2 ],dbgopt[ 3 ],
	//				dbgopt[ 4 ],dbgopt[ 5 ],dbgopt[ 6 ],dbgopt[ 7 ],
	//				connstat_, m_brewMps.m_entryFlg,m_brewMps.m_connectInit);
	//				MpsBrewLib_DrawString( dbgstr_, dbg_x_, 0 );

			}

	//		IDISPLAY_SetColor(GETMYDISPLAY(),CLR_USER_TEXT,MAKE_RGB(255,255,255));
		}
	#endif
#if false //-*todo:不要
		//リドロー
		//IDISPLAY_Update(GETMYDISPLAY());
		//画面を即時に更新する場合はTRUE、後で更新する場合はFALSEを設定してください。
	//	IDISPLAY_UpdateEx(GETMYDISPLAY(), FALSE);
		//MPS
		if(true != MpsBrewLib_Proceed() ) {}
	//m	app_main_done = true;//0507mt

		/************************/
		/*	処理時間測定		*/
		/************************/
	//xxxx	endtime = ISHELL_GetTimeMS(pIshell);

		worktime = System.currentTimeMillis() - starttime;

		// メモリ確保サイズチェック
	//0507mt	allocatedMemorySize();
#endif //-*todo:不要


	//	return;
	}
//-*********************app.j
}
