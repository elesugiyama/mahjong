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
// mjm1.j
//-*****************
public partial class MahjongBase : SceneBase {
/*
**		麻雀大会３
**		麻雀ゲームメインループ
*/

//#include "MahJongRally.h"								// Module interface definitions

//extern	void MainBGMStop(void);

public bool SomeyaCheck( /*MahJongRally * pMe*/ )
{
	short	i;
	/* 捨てている色。萬･索･筒 ０:捨 １:無捨 */
	byte[]	iro = { 1, 1, 1 };	//[ 3 ]
	byte	some;											/* 染めている色 */
	byte	st;
	bool	joken = true;
	byte	byPai;

	if (_No_Paidisp()) {									/*	コンピュータ同士の対局はメッセージは出さない	*/
		return (false);
	}

	some = MJDefine. NONE;
	for ( i = 0; i < gpsPlayerWork.byShcnt; i++ ) {
		byPai	=	gpsPlayerWork.bySthai[i];
		if ( byPai < 0x30 ) {			/* 数牌 */
			st = (byte)(( byPai & 0x30 ) >> 4);	/* code >> 4 */
			iro[ st ] = 0;									/* この柄捨てた！ */
			/* 全ての色を捨てている */
			if ( ( iro[ 0 ] | iro[ 1 ] | iro[ 2 ] ) == 0 ) {
				return	( false );
			}
		}
	}
	if ( ( iro[ 0 ] + iro[ 1 ] + iro[ 2 ] ) == 1 ) {		/* ２種類捨てている */
		for ( i = 0; i < 3; i++ ) {
			if ( iro[ i ] != 0 ) {
				some = (byte)(i << 4);								/* 染めてる柄を << 4 */
				break;
			}
		}
	}
	for ( i = 0; i < 3; i++ ) {			/* ３順目まで字牌がない */
		if ( gpsPlayerWork.bySthai[i] > 0x30 ) {
			joken = false;				/* 字牌だ！はい消えた！ */
			break;
		}
	}
	if ( joken == true ) {
		if ( gpsPlayerWork.byFhcnt == 0 ) {	/* ないていなければ染め屋 */
			return	( true );
		}
		else {								/* ないているならば */
			for ( i = 0; i < gpsPlayerWork.byFhcnt; i++ ) {
				/* 数牌ならチェック */
				if ( ( gpsPlayerWork.byFrhai[i] % 0x40 ) < 0x30 ) {
					if ( some == MJDefine.NONE ) {	/* 染め色未確定(１種色しか捨ててない) */
						/* ここで取敢えず染め色確定(１度しかこない) */
						some = (byte)(gpsPlayerWork.byFrhai[i] & 0x30);
						/* 色が捨てられてる１種の色じゃないか？ */
						if ( iro[ some >> 4 ] == 0 ) {
							return	( false );
						}

					}
					else {					/* 染め色確定済み */
						if ( some != ( gpsPlayerWork.byFrhai[i] & 0x30 ) ) {
							return	( false );
						}
					}
				}
			}
			return	( true );
		}
	}
	return	( false );
}

/*****************************
	毎局の処理(捨牌に対する言葉)
*****************************/
public void caution_m1 ( /*MahJongRally * pMe*/ )/*1995.7.12*/
{
	byte	bflag;
	int		iShCnt= 0;
	int		iDlgID= 0;
	byte	ucPai;
	bool	fNagas	=	false;

	if ( gpsPlayerWork.bFrich != 0 ) {		/* 立直中→何もしてない */
		return;
	}
	/* 立直してない */
	ucPai	= Sthai;
	bflag = 1;
	if ( ucPai < 0x30 ) {			/* 捨牌が数牌 */
		ucPai &= 7;
		if ( --ucPai != 0 ) {		/* 捨牌が中チャン牌(流し満貫じゃない) */
			fNagas = (gpsPlayerWork.bFnagas == 0);		/* 捨牌前のヤオ九振切フラグを入れておく */
			gpsPlayerWork.bFnagas = 0xFF;	/* ヤオ九振切終了判定 */
		}
	}
	if (_No_Paidisp()) {			/*	コンピュータ同士の対局はメッセージは出さないが流しマンガンのチェックはする	*/
		return;
	}

	while ( bflag != 0 ) {
		/*** ヤオ九振切突っ込みチェック(１２捨牌) ***/
		if ( bflag == 1 ) {
			iShCnt	=	gpsPlayerWork.byShcnt;
			if ( iShCnt != 12 )
				bflag = 2;
			else {									/* １２個目の捨牌 */
				/* 1996.8.11.DIAL_FIX */

				//> 2006/04/15 DS依存バグNo.3
				if ( Rultbl[(int)RL.YAOCHU] != 0 && ( gpsPlayerWork.bFnagas == 0 ) && ( gpsPlayerWork.bFmenzen == 0 ) ) {	//0422
				//< 2006/04/15 DS依存バグNo.3
					iDlgID = (int)DIAL.NAGASHI;			/* ヤオ九振切らしい(Go Talk) */
					bflag	= 8;
				} else								/* この関数終わり */
					bflag = 0;
			}
		}
		/*** ヤオ九振切終了嘆きチェック ***/
		if ( bflag == 2 ) {
////> w4 2006/02/25 チェック
//			iShCnt	=	gpsPlayerWork.byShcnt;
////> w4チェック
			if (iShCnt < 12)						/* １１以下 */
				bflag = 3;
			else {									/* １３以上 */
				//> 2006/04/15 DS依存バグNo.3
				// ルール設定でヤオ九振切を「なし」にしていても、ヤオ九振切関連セリフが出る
				if ( Rultbl[(int)RL.YAOCHU] != 0 && fNagas ) {	//0422	/* ヤオ九振切が終わった時 */
				//> 2006/04/15 DS依存バグNo.3
					/* 流し満貫が消えたとき(23) */
					MjDialog( Order, MJDefine.NONE, (byte)DIAL.NAGASHI_END );
				}
				bflag = 0;							/* この関数終わり */
			}
		}
		/*** 分岐 ***/
		if ( bflag == 3 ) {
			if ( iShCnt != 8 )
				bflag = 5;
			else									/* ８個目の捨牌なら */
				bflag = 4;
		}
		/*** 国士無双やってそうチェック ***/
		while ( bflag == 4 ) {
			iShCnt--;								/* １個づつチェック */
			ucPai	=	gpsPlayerWork.bySthai[iShCnt];
			if (ucPai >= 0x30 )						/* 字牌を捨ててる */
				bflag = 0;							/* 関数終了 */
			else {									/* 数牌 */
				ucPai	&=	7;
				if ( --ucPai == 0 )					/* 一九牌捨ててる */
					bflag = 0;						/* 関数終了 */
				else
					if ( iShCnt == 0) {				/* 中張牌かつ検索が最初の捨牌まできた */
						iDlgID	= (int)DIAL.KOKUSHI;		/* 国士無双に見える(Go_Talk) */
						bflag = 8;
						/* 1996.8.11.DIAL_FIX */
						if (gpsPlayerWork.byFhcnt != 0)	/* あら鳴いてたのね！やっぱだめ */
							bflag = 0;
					}
			}
		}
		/*** 分岐 ***/
		if ( bflag == 5 ) {
			if ( iShCnt != 10 )						/* 関数終了 */
				bflag = 0;
			else									/* １０個目の捨牌 */
				bflag = 6;
		}
		/*** 染め屋チェックの？？？ ***/
		if ( bflag == 6 )
			bflag = 7;

		/*** 染め屋チェック ***/
		/*----------------- 1996.8.11.DIAL_FIX */
		if ( bflag == 7 ) {
			bflag = 0;
			if ( Order == (byte)MJ_POS.A ) {			/* プレイヤーである */
				if ( SomeyaCheck() == true ) {	/* 染めている */
					iDlgID = (byte)DIAL.SOMEYA;
					bflag = 8;
				}
			}
		}
		/*** さあ喋るがよい ***/
		if ( bflag == 8 ) {
			/* ただしプレイヤーに対してだけね */
			if ( Order == (byte)MJ_POS.A ) {		/* 1996.8.11.DIAL_FIX */
				/* 他家の捨て牌を指摘（20:染屋,21:国士,22:流し満貫）*/
				MjDialog( OtherOne( Order ), (byte)Order, (byte)iDlgID );
			}
			bflag = 0;
		}
	}
}

/*****************************
	ＣＯＭのツモ手順
	  牌を１個積もってきたあとの処理

		戻り	0= COMPUT_RET_NON(0)	- 牌を切る
				1= COMPUT_RET_TSUMO(1)	- ツモ
				2= COMPUT_RET_KAN(2)	- カン（四槓）
				3= COMPUT_RET_ELSE(3)	- その他（９種９牌）
				4=						- プレーヤー入力待ち
*****************************/
public int comput_m1 (/*MahJongRally * pMe,*/ bool fGuide)
{
//	BYTE	bySutehaiPos	=	NONE;
	int		iDlgID;
	int		iTmp;
	int		iRet;

#if DEBUG		//_DEBUG
	if( comput_m1_bflag_old != comput_m1_bflag ) {	//&& is_reentry == false )
Debug.Log("comput_m1_bflag= "+comput_m1_bflag);		//DebLog(( "comput_m1 flag=%d", comput_m1_bflag ));
		comput_m1_bflag_old = comput_m1_bflag;
	}
#endif
	gpsPlayerWork	=	gsPlayerWork[Order];	//xxxx	なぜか gpsPlayerWorkが設定されてない
	iRet	=	(int)COMPUT_RET.NON;
	//while ( comput_m1_bflag != 0 )
	//{
//#ifdef	__LOGIC_CHECK
//Debug.out("comput_m1_bflag ", comput_m1_bflag);		//aaaa
//#endif
		switch ( comput_m1_bflag ) {
		  case 100:
			//	comput に飛ぶ場合 bflag = 100
			ankan_m6_bflag = 0;
#if DEBUG
//			Debug.out("player= "+ Integer.toString(game_player)+	", order= "+ Integer.toString(Order)+	", ai= "+ Integer.toString(gNetTable_NetChar[Order].AIFlag)		);	//DebLog(( "player=%d,order=%d,ai=%d",game_player,Order,gNetTable_NetChar[Order].AIFlag ));
#endif
#if false //-*todo:通信
			if( IS_NETWORK_MODE ) {
				// 再接続モード時は思考しない。
				// AIの設定がツモ切りになっているプレイヤーの場合も思考無し
				if( is_reentry == true ) {
					// 再接続中。
					//> 2006/03/02 バグ 126 振聴チェック
					chktnp_m5();
					//< 2006/03/02 バグ 126

					gThinkResult = OP_TAPAI;		//0422
				} else if( gNetTable_NetChar[Order].Flag != 0xFF ) {
					if( Order == game_player ) {
						switch( gDaiuchiFlag ) {
							case D_ONLINE_OPERATOR_AI :
								gThinkResult = think_think();
								break;
							case D_ONLINE_OPERATOR_PASS :
								gThinkResult = OP_TAPAI;		//0422
								break;
							case D_ONLINE_OPERATOR_MANUAL :
								ASSERT( !"player in com think" );
								gThinkResult = OP_TAPAI;		//0422
								break;
						}
					} else {
						if( gNetTable_NetChar[Order].AIFlag == 0 ) {
							if( gsPlayerWork[ Order ].bFrich != 0 && gsPlayerWork[ Order ].bFfurit == 0 ) {
								gThinkResult = think_think();
								switch( gThinkResult ) {
									case OP_TSUMO :		break;
									default : gThinkResult = OP_TAPAI;		//0422
								}
							} else
								gThinkResult = OP_TAPAI;		//0422
						} else
							gThinkResult = think_think();
					}
				} else
					gThinkResult = think_think();
			} else {
				gThinkResult = think_think();		/*スタンドアローン時は全てこちら*/

				// デバッグ用 みんなパスさせる。
				//gThinkResult = OP_PASS;
			}
#else
			gThinkResult = think_think();		/*スタンドアローン時は全てこちら*/
#endif //-*todo:通信

			hotpai	=	Sthai;
//#ifdef DEBUG
//Debug.out(	"CPU-"+ Integer.toString(Order)+
//				"-Think Result  ThinkResult : "+ Integer.toString(gThinkResult)+
//				" sute :"+ Integer.toString(Sthai)	);
//	DebLog(("CPU-%d-Think Result  ThinkResult : %d sute :0x%02x\n",Order,gThinkResult,Sthai));
//#endif
			comput_m1_bflag= 110;
#if false //-*todo:通信
			if( IS_NETWORK_MODE ) {
				// 通信対戦時
				// 捨牌報告
				SutehaiReportSnd((int)Order,(int)gThinkResult,(int)Sthai);
//				if( Order == game_player )
				{
					if( is_reentry == false ) {
						updateDaiuchiFlag();
						updateSetsudanFlag();
						// 2006/02/24 状態変更時に他のＰＣへ状態変更通知が届かない
						//CharaChangeReport( );
					}
				}
			}
#endif //-*todo:通信
			break;
		  case 110:
			//ここで捨て牌決定通知を待つ。それがくるまで状態変化なし
			//内容：Order、思考結果、牌
#if false //-*todo:通信
			if( ! IS_NETWORK_MODE ) {
				// モード以降
				comput_m1_bflag=120;
			} else {
				// 通信対戦時
				// 捨牌報告待ち
				if( is_reentry == true ) {
//0426mt >
					// 再接続時処理
					//struct_hai_sutehai_notify rbuf_;		//struct_hai_sutehai_notify *rbuf_;
					//CmdHistory_ cmd_;						//CmdHistory_ *cmd_;

					//cmd_  = stReEntry_mycommand[ stReEntry_mycommand_count];	//cmd_  = stReEntry_mycommand + stReEntry_mycommand_count;
					//rbuf_ = (m_brewMps.m_data.msg.Pai.hai_sutehai_notify);

//0501mt 					m_brewMps.m_data.msg.Pai.hai_sutehai_notify.HaiState 	= stReEntry_mycommand[ stReEntry_mycommand_count].HaiState;
//0501mt 					m_brewMps.m_data.msg.Pai.hai_sutehai_notify.SuteHai		= stReEntry_mycommand[ stReEntry_mycommand_count].SuteHai;
//0501mt 					m_brewMps.m_data.msg.Pai.hai_sutehai_notify.SuteHouseID	= stReEntry_mycommand[ stReEntry_mycommand_count].SuteHouseID;

//0426mt <
//0501mt 			SutehaiNotifyRcv( );
					SutehaiNotifyRcv(	(char)0,//stReEntry_mycommand[ stReEntry_mycommand_count].KyokuStatus,
						stReEntry_mycommand[ stReEntry_mycommand_count].SuteHouseID,
						stReEntry_mycommand[ stReEntry_mycommand_count].HaiState,
						stReEntry_mycommand[ stReEntry_mycommand_count].SuteHai,
						m_DmyAIFlag
					);

					++(stReEntry_mycommand_count);

					//> 2006/03/29 No.211 再接続時にロン牌が出ると見逃す
					// 再接続終了
					if( stReEntry_mycommand_count == stReEntry_mycommand_count_max ) {
						DebLog(("---------- reentry done  ----------"));

						gsPlayerWork[ game_player ].byPlflg = 1;
						is_reentry = false;
					}
					//< 2006/03/29 No.211 再接続時にロン牌が出ると見逃す
				}
			}
#else
			// モード以降
			comput_m1_bflag=120;
#endif //-*todo:通信

			break;

		  case 120:
			//思考結果の処理
#if	__MJ_CHECK
Debug.Log( "comput_m1= "+ (int)comput_m1_bflag+ ", order= "+ (int)Order+ ", think= "+ (int)gThinkResult );
#endif
#if true //-*todo:
			DebLog(( "comput_m1="+comput_m1_bflag+",order="+Order+",think="+gThinkResult));
#endif //-*todo:
			switch ( gThinkResult ) {
			  case (int)OP.TAOPAI:
				// 牌を倒す（９種９牌）< bflag = 0 >
				Pinch = (int)SP.NINE_HAI;
				comput_m1_bflag=0;
				return (int)(COMPUT_RET.ELSE);

			  case (int)OP.TSUMO:
				// ツモっちゃった（あがり）< bflag = 0 >
				MjEvent((byte)GEV.NAKI_TUMO,Order);
				comput_m1_bflag=0;
				return (int)(COMPUT_RET.TSUMO);

			  default:
				//局終了以外の思考の場合
				gpsPlayerWork.bFippat		=	0;
				gpsPlayerWork.byIppatuClear	=	0;

				//局終了以外の思考の場合
				switch ( gThinkResult ) {
				  case (int)OP.KAN:									/* 暗カン */
					if (fGuide)									/*	指導モードの時は抜ける	*/
						return (int)(COMPUT_RET.KAN);
					comput_m1_bflag = 15;
					ankan_m6_bflag=0;
					return(4);
//					break;
				  case (int)OP.CHANKAN:								/* ポン→明カン */
					if (fGuide)									/*	指導モードの時は抜ける	*/
						return (int)(COMPUT_RET.KAN);

					comput_m1_bflag = 14;
					chakan_m6_bflag = 0;
					return(4);
//					break;
				  case (int)OP.RICHI:								/* 立直 */
					if (fGuide) {								/*	指導モードの時は抜ける	*/
						comput_m1_bflag	=	0;
						break;
					}
					Status	&= (byte)(~ST.RINSH);						/* 1996.9.6. BUG_FIX  */
					if ( (Status & (byte)ST.ONE_JUN) != 0 ) {			/* ダブル立直じゃない */
						if ( Richi == 0 ) {						/* １人目の立直 */
							iDlgID	= (int)DIAL.REACH_NORMAL;
							if (( byte )Bpcnt >= MJDefine.EARLSTG )		/* 序盤 */
								++iDlgID;
						} else {
							/* デフォルトを四人目のリーチ */
							iDlgID	= (int)DIAL.D_4CYA_REACH/*68*/;
							if ( Richi != 3 ) {					/* ４人目じゃない */
								if (( byte )Richi < 2 )			/* 追っかけ */
									iDlgID	= (int)DIAL.REACH_OI;
								else							/* ３人目立直 */
									iDlgID	= (int)DIAL.REACH_3CHA;
							}
						}
					} else			/* ダブル立直 */
						iDlgID	= (int)DIAL.REACH_W;

					/* 自分がリーチするとき(24～29,66) */
					MjDialog( (byte)Order, MJDefine.NONE, (ushort)iDlgID);
//					gpsPlayerWork	=	gsPlayerWork[Order];	//xxxx	なぜか gpsPlayerWorkが設定されてない
					gpsPlayerWork.bFrich	=	0xFF;
					gpsPlayerWork.bFippat	=	0xFF;
					gpsPlayerWork.bFsthai	=	0xFF;
					/*break; defaultに突入させる */

					MjEvent((byte)GEV.NAKI_RICHI,Order);
					goto default;
				  default:
					comput_m1_bflag = 6;
					break;
				}
				break;//局終了以外の思考の場合
			}
			break;//思考結果の処理

		  case 6:		/* 牌を捨てる処理開始 */
			// ツモ切りモード
			//	指導モードの時は抜ける
			if (fGuide) {
				comput_m1_bflag	=	0;
				break;
			}
			Csr = Thcnt;

			//捨牌とツモ牌の判定
			if ( Sthai == gpsPlayerWork.byHkhai) {
				//同じ場合
				comput_m1_bflag = 8;
				break;
			} else {
				//異なる場合
				comput_m1_iThcnt	= Thcnt;
				comput_m1_bflag = 7;
			}
			goto case 7;
			case 7:	{ //ツモ牌と異なる牌を捨てる場合
			#if true //-*todo:
				DebLog(( "comput_m1_iThcnt="+comput_m1_iThcnt+",sthai="+Sthai+",hkhai="+gpsPlayerWork.byHkhai));
			#endif //-*todo:	
				while ( comput_m1_bflag == 7 ) {
					comput_m1_iThcnt--;
					if( comput_m1_iThcnt == 0xFF ) {
//						PLAYERWORK pw_ = gpsPlayerWork;
//						TRACE( "FATAL ERROR:comput_m1 mode=7" );
//						TRACE( "0x%08x : %02x,%02x,%02x,%02x,%02x,%02x,%02x,%02x,%02x,%02x,%02x,%02x,%02x,%02x",
//						pw_,
//						pw_.byTehai[0],pw_.byTehai[1],pw_.byTehai[2],pw_.byTehai[3],
//						pw_.byTehai[4],pw_.byTehai[5],pw_.byTehai[6],pw_.byTehai[7],
//						pw_.byTehai[8],pw_.byTehai[9],pw_.byTehai[10],pw_.byTehai[11],
//						pw_.byTehai[12],pw_.byTehai[13] );
					#if false //-*todo:
						ASSERT( !"comput_m1 mode=7" );
					#endif //-*todo:	
						break;
					}
					if ( Sthai == gpsPlayerWork.byTehai[comput_m1_iThcnt]) {
						gpsPlayerWork.byTehai[comput_m1_iThcnt]	=	gpsPlayerWork.byHkhai;
						gpsPlayerWork.byHkhai			=	Sthai;
						thsort_m2();
						break;
					}
				}
				comput_m1_bflag = 8;
			}
			goto case 8;
		  case 8:	//捨牌判定
			if ( SubMj.Doracnt[( byte )Sthai] == 0 ) {
			#if true //-*todo:描画
				paintF= true; //0507mt
			#endif //-*todo:描画
				comput_m1_bflag = 11;
				break;
			} else {
				if ( gpsPlayerWork.bFrich != 0 ) {
					comput_m1_bflag = 11;
					break;
				} else {
					comput_m1_iThcnt = 3;
					comput_m1_bflag = 9;
				}
			}
			goto case 9;
		  case 9:
			if(gsPlayerWork[comput_m1_iThcnt].bFrich == 0 )
				comput_m1_bflag = 10;
			else {
				setapp_pcnt(comput_m1_iThcnt);
				iTmp	= SubMj.App[Sthai];		//		*( App + Sthai );
				iTmp	&= 8;

				if( iTmp == 0 )
					comput_m1_bflag = 10;
				else {
					/* 他家の立直中、非安牌ドラをきるとき(19) */
					MjDialog( Order, MJDefine.NONE, (byte)DIAL.DORAKIRI );
					comput_m1_bflag = 11;
					break;
				}
			}
			goto case 10;
		  case 10:
			if ( comput_m1_iThcnt > 0 ) {
				comput_m1_iThcnt--;
				comput_m1_bflag = 9;
				break;
			} else
				comput_m1_bflag = 11;
//			break;
			goto case 11;
		  case 11:
			if ( Order == 0 ) {
				comput_m1_bflag = 13;
				break;
			} else {
				if ( SubMj.Opnflg != 0 ) {
					comput_m1_bflag = 13;
					break;
				}
//#if 0
//				else if ( Csr == Thcnt ) {
//					comput_m1_bflag = 12;
//				}
//#endif
				else
					comput_m1_bflag = 12;
			}
//			break;
			goto case 12;
		  case 12:
		  case 13:
			gpsPlayerWork.byTkhai = 0;			//://手牌に表示のときにツモ牌は表示しない
			comput_m1_bflag = 0;			/* ブレイク */
			#if false //-*サウンド
			SeOnPlay(SE_TAHAI,BANK_SE);
			#endif //-*サウンド
			break;

		  case 14:
			  // ＣＯＭの加槓処理
			switch(chakan_m6()) {
				case	1:
					comput_m1_bflag = 100;
					iRet	=	(int)COMPUT_RET.NON;
					break;
				case	0:
					iRet	=	(int)COMPUT_RET.KAN;
					//iRet	=	COMPUT_RET_NON;
					comput_m1_bflag = 0;
					break;
				default:
					return(4);
			}
			break;

		  case 15:
			  // ＣＯＭの暗槓処理
			switch (ankan_m6()) {
				case	1:
					comput_m1_bflag = 100;							/*	カン成功	*/
					break;
				case	0:
					iRet	=	(int)COMPUT_RET.KAN;
					comput_m1_bflag = 0;
					break;
				default:
					return(4);
//					break;
			}
			break;

		  default:
			break;
		}
	//}
	return (iRet);
}

public void check_urayakitori( /*MahJongRally * pMe*/ )
{
	short	i;
	short	clear_cnt;

	if ( (Rultbl[ (int)RL.YAKI ] >= 4) && (Rultbl[ (int)RL.YAKI ] <= 6) ) {	/* 裏 */
		for( i = 0, clear_cnt = 0; i < MJDefine.MAX_TABLE_MEMBER; i++ )
			if(( gpsTableData.sMemData[i].byMemFlags & (byte)TBLMEMF.TORI) == 0 )
				clear_cnt++;

		if( clear_cnt == 4 )
			for( i = 0; i < MJDefine.MAX_TABLE_MEMBER; i++ )
				gpsTableData.sMemData[i].byMemFlags	|=	(byte)TBLMEMF.TORI;
	}
}

public void _reentry_sub1(/*MahJongRally * pMe*/)
{
	if (!SubMj.CompVSCompMode)
		Dialog_KyokuStart();		/* DIALOG */
}

public void _dispChipData(/*MahJongRally * pMe*/)				/*	チップ表示× →チップ計算	*/
{
	if ((sRuleSubData.byFlag & (byte)RULESUBFLAG.CHIP) != 0)
		for ( int i = 0; i < MJDefine.MAX_TABLE_MEMBER; i++ )
			gpsTableData.sMemData[i].nChip	+= gpsTableData.sMemData[i].nMoveChip;
}

public void _agari_func(/*MahJongRally * pMe,*/ int talkcounter)
{
	kyoku_result ( (byte)talkcounter );	/* 役表示・移動点セット */

	if((sRuleSubData.byFlag & (byte)RULESUBFLAG.CHIP) != 0)
		cal_tip();		/* 1996.6.18.TIP チップの計算 */
	#if true //-*todo:変換有ってるかな
	gpsTableData.sMemData[Order].byMemFlags = (byte)(gpsTableData.sMemData[Order].byMemFlags & ~(byte)(TBLMEMF.TORI));
	#endif //-*todo:変換有ってるかな
}

public bool _checkRichiYaku(/*MahJongRally * pMe*/)
{
	if(Rultbl[(int)RL.URA] != 0)
		/*	裏ドラルール有り	*/
		if( Check_Yakunumber((int)YK.RICHI) || Check_Yakunumber((int)YK.WRICH) )
			/*	リーチしたなら	*/
			return (true);

	return (false);
}
//	局ループ
public void reentry_m1 ( /*MahJongRally * pMe*/ )
{
	ushort	i, l;
	byte	talkcounter = 0;
	byte	ret;
//	BYTE	tmp;

#if DEBUG
if(selFlg){
	int a=0;
	Debug.Log("//-*"+selFlg);
}
	if( reentry_m1_bflag_dbg != reentry_m1_bflag ) {	// && !is_reentry )
		// 状態遷移があったらデバッグ出力
//		DebLog(( "reentry_m1(): reentry_m1_bflag=%d,ply1=%d,ply2=%d,com=%d,order=%d,me=%d,status=%04x", reentry_m1_bflag, before_richi_m4_bflag, after_richi_m4_bflag, comput_m1_bflag, Order, game_player, Status ));
//		Debug.out("reentry_m1(): reentry_m1_bflag= "+ Integer.toString(reentry_m1_bflag)+	",ply1= "+ Integer.toString(before_richi_m4_bflag)+		",ply2= "+ Integer.toString(after_richi_m4_bflag)+		",com= "+ Integer.toString(comput_m1_bflag)+	",order= "+ Integer.toString(Order)+	",me= "+ Integer.toString(game_player)+		",status= "+ Integer.toString(Status)	);
//		if( is_reentry == true )		//( is_reentry != 0 )	//0422
//			DebLog(("reentry_m1(): reentry_m1_bflag=%d,ply1=%d,ply2=%d,com=%d,order=%d,me=%d,status=%04x\n", reentry_m1_bflag, before_richi_m4_bflag, after_richi_m4_bflag, comput_m1_bflag, Order, game_player, Status));

		reentry_m1_bflag_dbg = reentry_m1_bflag;
	#if	__MJ_CHECK
	Debug.Log("reentry_m1_bflag: "+ (int)reentry_m1_bflag);
	#endif
	}
#endif
// DebLog("//-*reentry_m1():Order("+Order+"):reentry_m1_bflag("+reentry_m1_bflag+")");
		switch ( reentry_m1_bflag ) {
			case ( 1 ):		 /*** 牌をツモる ***/
			#if false //-*todo:サウンド
				SeOnPlay(SE_PAI1,BANK_SE);
			#endif //-*todo:サウンド
				setwp_m2(Order);
				shuhai_m2();
				before_richi_m4_bflag = 100;
				after_richi_m4_bflag = 100;
				DebLog(("PLAYER"+Order+"  tsumo:"+gpsPlayerWork.byHkhai));
				reentry_m1_bflag = 2;
//-*SUGI_DEB***************************
#if SUGI_DEB //-*todo:注デバッグ中	
string tehaiList = "PLAYER("+Order+"){"+Thcnt+":"+gpsPlayerWork.byThcnt+"}:";
for(int a=0;a<gpsPlayerWork.byTehai.Length;a++)
{
	// if(gpsPlayerWork.byThcnt-1 < a)gpsPlayerWork.byTehai[a] = 0x40;
	tehaiList += "["+gpsPlayerWork.byTehai[a]+"] ";
}
DebLogError((""+tehaiList));
#endif //-*todo:注デバッグ中
//-****************************SUGI_DEB

				// １巡に１回のみチャットは送信可能にする。
				// TRUE . 送信可。

				//> 2006/03/31 通信対戦時のみチャット送信可能フラグを復帰
				#if false //-*todo:通信　描画
				if( IS_NETWORK_MODE && m_DaiuchiFlag == D_ONLINE_OPERATOR_MANUAL )		//0422
					m_ChatSendFlag = true;
				paintF= true;		//1 → 2
				#endif //-*todo:通信　描画
				break;

			case ( 2 ): {
					byte daiuchi_ = 0;
				#if false //-*todo:通信
					if( IS_NETWORK_MODE && game_player == Order && gDaiuchiFlag == D_ONLINE_OPERATOR_AI )
						daiuchi_ = 2;
				#endif //-*todo:通信

					/*** ロンの状態ではない→ロンのフラグを消す ***/
//m					Order_now = Order;
				#if true //-*todo:
					// Status &= ~ST_RON;
					Status &= (byte)~ST.RON;
				#endif //-*todo:

				#if false //-*todo:通信

					// 通信対戦時、自分の番が来るまでツモ切り・ＡＩ選択不可
					if( IS_NETWORK_MODE && Order == game_player ) {
						mythink_select_ok_flag = 1;
						mypinch_hitter_select_ok_flag = 1;
					}
				#endif //-*todo:通信
//-*SUGI_DEB***************************
#if SUGI_DEB //-*todo:注デバッグ中
// gpsPlayerWork.byPlflg = 0;	//-*オートプレイ
#endif //-*todo:注デバッグ中
//-****************************SUGI_DEB
					//プレーヤー／COM判定
					if ( gpsPlayerWork.byPlflg == 0 || is_reentry == true || daiuchi_ == 2 ) {	// いれ込み。
						// COMか再接続の時の処理
						comput_m1_bflag = 100;
						reentry_m1_bflag = 3;
						#if true //-*todo:描画
						paintF= true; //0507mt // 再接続時は描画しないので、ここでフラグを立ててもOK
						#endif //-*todo:描画
						DebLog(("Order["+Order+"] = COM"));
					} else {
						// プレイヤー処理
						reentry_m1_bflag = 8;
						hide_hkhai = 0;
						#if true //-*todo:描画
						paintF= true;		//2 → 8
						#endif //-*todo:描画
						DebLog(("Order["+Order+"] = PLAYER"));
					}
					break;
				}
			case ( 8 ): {
					#if false //-*通信
					// 自摸タイムアウトチェック
					if( IS_NETWORK_MODE ) {
						if(gTsumoTimeOut >= 0)
							gTsumoTimeOut -= uptimems_perfrm;

						if(gTsumoTimeOut <= 0) {
							gAutoFlag = 0x01;
							gTsumoTimeOut = 0;
						}

						if( gDaiuchiFlag == D_ONLINE_OPERATOR_PASS ) {
							gAutoFlag = 0x01;
							gTsumoTimeOut = 0;
						}
					}
					#endif //-*通信
					// 	プレイヤーの手順処理
					ret = player_m4();

				#if true //-*todo:通信
					switch ( ret ) {
						case 0:
							/* このツモで何か起こらない→牌を切る処理 */
							reentry_m1_bflag = 4;
							break;
						case 1:
							/* このツモで何か起こった→ブレイク */
							reentry_m1_bflag = 5;
							break;
					}
				#else //-*todo:通信
					// タイムアウトすると自動的に選択している状態で牌を動かす
					if ( !IS_NETWORK_MODE ) {
						switch ( ret ) {
							case 0:
								/* このツモで何か起こらない→牌を切る処理 */
								reentry_m1_bflag = 4;
								break;
							case 1:
								/* このツモで何か起こった→ブレイク */
								reentry_m1_bflag = 5;
								break;
						}
					} else {
						// 他の家の報告待ちへ
						if ( ret == 0 || ret == 1 )
							reentry_m1_bflag = 14;
					}
				#endif //-*通信
				}
				break;
			case 14 :
				// ツモで何か発生（カン、ツモ、倒す（九種九牌））
				// 捨牌対応報告を待つ
				break;
			case ( 3 ):
				//他家の行動は下記関数内で行う
				//関数の階層があっていないが既存のロジック優先のため
				// 捨牌報告受信待ちを行う

				ret	= (byte)comput_m1(false);

				if(comput_m1_bflag==0) {
					if(ret != 4) {
						/* ＣＯＭの手順 */
						hotpai	=	Sthai;
						if( ret == (byte)COMPUT_RET.NON )
							/* このツモで何か起こらない→牌を切る処理 */
							reentry_m1_bflag = 4;
						else
							/* このツモで何か起こった→ブレイク */
							reentry_m1_bflag = 5;
						#if true //-*todo:描画
							paintF= true;		//3 → 4 or 5
						#endif //-*todo:
					}	//> 2006/03/08 加槓・搶槓処理
					else // カンの場合は捨牌対応報告待ちへ
					{}
					//< 2006/03/08 加槓・搶槓処理
				}
				break;
			case ( 4 ):		 /*** 何かを切る時 ***/
				// 家が牌を切ったときに入る。
				tapai_m3();						/* 打牌処理 */
				#if true //-*todo:描画
				paintF= true; //0507mt
				#endif //-*todo:
				/* 各家の行動内容をクリア */
				for( i= 0; i< 4; i++)
					gNakiResult[i]=0;

				// ツモ順移動
				Odrbuf = Order;
#if	Rule_2P
				Order = (byte)(( Order + 1 ) & 1);
#else
				Order = (BYTE)(( Order + 1 ) & 3);
#endif
				gpsPlayerWork	=	gsPlayerWork[Order];		//xxxx
				#if true //-*todo:
				DebLog(("Odrbuf = "+Odrbuf+" : Order = "+Order));
				#endif //-*todo:
				chknaki_m6();		//鳴けるかどうかのチェック

				//TRACE( "odr=%d,odrbuf=%d,naki=[%d][%d][%d][%d]",
				//	Order,Odrbuf,
				//	gNakiResult[0],gNakiResult[1],gNakiResult[2],gNakiResult[3] );

				//TRACE( "pon=%d:ponodr=%d:[%d][%d][%d][%d]",
				//			Ponflg,Ponodr,gPonChk[0],gPonChk[1],gPonChk[2],gPonChk[3] );
				//TRACE( "chi=%d:[%d][%d][%d][%d]",
				//			Chiflg,gChiChk[0],gChiChk[1],gChiChk[2],gChiChk[3]
				//			);

				// ロンチェックフラグ初期化
				ronho_m5_bflag = 1;

				player_pon_flg[Order] = 0;

				Ronodr=0xff;

				reentry_m1_bflag = 7;
				is_ronchk_menu_open = 0;
				break;

			case ( 7 ):
//				paintF= true;		//aaaa
				/*** ロンのチェック ***/
				//０：ロンできる人なし
				//１：ロンできる人あり
				//２：処理中
				if( is_reentry == false ) {
				#if false //-*todo:通信
					if( IS_NETWORK_MODE ) {
						// 鳴きタイムアウト
						if(gNakiTimeOut >= 0)
							gNakiTimeOut -= uptimems_perfrm;

						if(gNakiTimeOut <= 0) {
							gAutoFlag = 0x01;
							gNakiTimeOut = 0;
						}

						if( gDaiuchiFlag == D_ONLINE_OPERATOR_PASS ) {
							gAutoFlag = 0x01;
							gNakiTimeOut = 0;
						}
					}
				#endif //-*todo:通信

					ret = ronho_m5(0);

				#if false //-*todo:通信
					if( IS_NETWORK_MODE ) {
						// 状態によってCOMの捨牌対応を上書き変化させる。
						for( int member_ = 0; member_ < HOUSE_MAX_NUM; ++member_ ) {
							if( member_ == game_player )
								continue;

							//if( gNetTable_NetChar[ member_].AIFlag == 1 )
							//{
							//	gNakiResult[member_] = OP_PASS;
							//}
						}
					}
				#endif //-*todo:通信
				} else {
					//> 2006/03/02 No.126 振聴チェック
					ret = ronho_m5(0);
//					ret = 0;
					//< 2006/03/02 No.126 振聴チェック
				}

				if(ret!=2) {
					// ロン判定結果を保存
					gRonChkResult = ret;

				#if false //-*todo:通信
					if( IS_NETWORK_MODE ) {
						// 通信対戦の場合
						// 捨牌対応報告を送信する
						SutehaiActReportSnd();
						gNakiTimeOut = 0;
					}
				#endif //-*todo:通信

					reentry_m1_bflag = 10;
				}
				break;

			case (10):
				/* ロン判定後処理		*/
			#if true //-*todo:通信
				// COM対戦時
				switch(gRonChkResult) {
					case	0:
						/* ロンがいない */
						reentry_m1_bflag=13;
						break;
					case	1:
						/* ロンがいた */
						reentry_m1_bflag = 5;
						break;
					default:
						/* プレーヤーの入力待ち */
						break;
				}
			#else //-*todo:通信
				if( ! IS_NETWORK_MODE ) {
					// COM対戦時
					switch(gRonChkResult) {
						case	0:
							/* ロンがいない */
							reentry_m1_bflag=13;
							break;
						case	1:
							/* ロンがいた */
							reentry_m1_bflag = 5;
							break;
						default:
							/* プレーヤーの入力待ち */
							break;
					}
				} else {
					// 通信対戦時
					// 捨牌対応報告受信待ち
					if( is_reentry == true ) {
//0426 mt ->
						//struct_hai_sutehai_act_notify rbuf_;
						//CmdHistory_ cmd_;
						//cmd_  = stReEntry_mycommand[ stReEntry_mycommand_count];	//cmd_  = stReEntry_mycommand + stReEntry_mycommand_count;
						//rbuf_ = (m_brewMps.m_data.msg.Pai.hai_sutehai_act_notify); //0425 mt
//0425 mt ->
//0501mt 						m_brewMps.m_data.msg.Pai.hai_sutehai_act_notify.Act			= stReEntry_mycommand[ stReEntry_mycommand_count].HaiState;
//0501mt 						m_brewMps.m_data.msg.Pai.hai_sutehai_act_notify.ActedHouseID	= stReEntry_mycommand[ stReEntry_mycommand_count].SuteHouseID;
//0501mt 						m_brewMps.m_data.msg.Pai.hai_sutehai_act_notify.ActHouseID	= stReEntry_mycommand[ stReEntry_mycommand_count].ActHouseID;
//0501mt 						m_brewMps.m_data.msg.Pai.hai_sutehai_act_notify.ChiNo			= stReEntry_mycommand[ stReEntry_mycommand_count].ChiState;
//0501mt 						m_brewMps.m_data.msg.Pai.hai_sutehai_act_notify.SuteHai		= stReEntry_mycommand[ stReEntry_mycommand_count].SuteHai;
//0501mt //0425 mt <-
//0501mt 						SutehaiActNotifyRcv( );
						SutehaiActNotifyRcv(
							(char)0,
							stReEntry_mycommand[ stReEntry_mycommand_count].HaiState,
							stReEntry_mycommand[ stReEntry_mycommand_count].ActHouseID,
							stReEntry_mycommand[ stReEntry_mycommand_count].SuteHouseID,
							stReEntry_mycommand[ stReEntry_mycommand_count].ChiState,
							stReEntry_mycommand[ stReEntry_mycommand_count].SuteHai
						);
						++(stReEntry_mycommand_count);

						//> 2006/03/29 No.211 再接続時にロン牌が出ると見逃す
						// 再接続終了
						if( stReEntry_mycommand_count == stReEntry_mycommand_count_max ) {
							DebLog(("---------- reentry done ----------"));

							gsPlayerWork[ game_player ].byPlflg = 1;
							is_reentry = false;
						}
						//> 2006/03/29 No.211 再接続時にロン牌が出ると見逃す
					}
				}
			#endif //-*todo:通信
				break;

			case (13):
//				paintF= true;		//aaaa
				//上がり以外の終了判定
				//TRUE	：終了
				//FALSE	：継続
				ret = (byte)(chkpin_m5() ? 1 : 0);

				if(ret == 0) {		//(ret == FALSE)
					//setwp_m2(Order);
					setwp_m2(Odrbuf);
					caution_m1();
					keyin_m6_bflag = 0;
					naki_m6_bflag = 0;
					reentry_m1_bflag = 6;		//://鳴きチェックへ
					#if true //-*todo:描画
					paintF= true;		//13 → 6
					#endif //-*todo:描画
				} else
					reentry_m1_bflag = 5;
				break;

			case ( 6 ):
				// 鳴きチェック（プレーヤーの入力待ち
				if( is_ronchk_menu_open == 1 ) {
					gAutoFlag = 0x01;
//					gNakiTimeOut = 0;
					//メニュークリア
					clropt_opt();
					is_ronchk_menu_open = 0;
				}
			#if true
				if( gMyTable_NakiNashi == 1 && Optcnt == 0 ) {
					//メニュークリア
					clropt_opt();
					gAutoFlag = 0x01;
				}
			#endif
			#if false //-*todo:通信
				// 鳴きタイムアウト
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

				//０：処理終了
				//FF：処理中
				// ロンチェックでメニューを開いていたら、ここはパス
				ret = naki_m6_jdg();

			#if false //-*todo:通信
				if( IS_NETWORK_MODE ) {
					// 状態によってCOMの捨牌対応を上書き変化させる。
					int member_ = 0;
					for( member_ = 0; member_ < HOUSE_MAX_NUM; ++member_ ) {
						if( member_ == game_player )
							continue;

						// 他のメンバーのフラグがツモ切り状態だったら哭きは全てパス
						if( gNetTable_NetChar[ member_].AIFlag == 0x00 )		//0422
							gNakiResult[member_] = OP_PASS;
					}
				}
			#endif //-*todo:通信

				//> 2006/03/16 不具合 No.186 通信対戦後にCOM対戦を行うと順番が飛ばされる。
				//if( ret == 0 || Optcnt == 0  )
				//> 2006/04/04 ロンと同時に鳴きが可能な場合に鳴きを選択しても鳴けない、COM対戦だとポン選択で飛ばされる
				if( ret == 0 )//|| ( Optcnt == 0 && Order == game_player ) )	//0422
				//> 2006/04/04 ロンと同時に鳴きが可能な場合に鳴きを選択しても鳴けない
				//> 2006/03/16 不具合 No.186 通信対戦後にCOM対戦を行うと順番が飛ばされる。
				{
					#if true //-*todo:
					if(naki_m6_iNakiMode != 0)
						DebLog(("naki_m6_iNakiMode = "+naki_m6_iNakiMode+" : Order = "+Order));
					#endif //-*todo:

					gAutoFlag = 0x00;

				#if false //-*todo:通信
					if( IS_NETWORK_MODE ) {
						//// 通信対戦の場合
						//TRACE("NAKI chk");
						//ここで捨牌対応報告を送信する
						SutehaiActReportSnd();
						gNakiTimeOut = 0;
					}
				#endif //-*todo:通信

					// 状態を鳴き判定へ
					reentry_m1_bflag = 15;
				}
				break;

			case (15):
				#if true //-*todo:通信
					// COM対戦時
					naki_m6_iNakiMode=0;		//チーポンカン設定初期化

					for( i= 0; i< MJDefine.MAX_TABLE_MEMBER2; i++) {	//4
						byte	tmp= gNakiResult[i];		//各家の状態取得
						/* ロン以外の場合 */
						//カン：６
						//ポン：５
						//チー：４
						//より大きな数字を保持していく
						if(naki_m6_iNakiMode< tmp) {
							naki_m6_iNakiMode= tmp;
							Order= (byte)i;
							setwp_m2(Order);
						}
					}
					reentry_m1_bflag=11;
				#else //-*todo:通信
				if( !IS_NETWORK_MODE ) {
					// COM対戦時
					naki_m6_iNakiMode=0;		//チーポンカン設定初期化

					for( i= 0; i< MAX_TABLE_MEMBER2; i++) {	//4
						BYTE	tmp= gNakiResult[i];		//各家の状態取得
						/* ロン以外の場合 */
						//カン：６
						//ポン：５
						//チー：４
						//より大きな数字を保持していく
						if(naki_m6_iNakiMode< tmp) {
							naki_m6_iNakiMode= tmp;
							Order= (BYTE)i;
							setwp_m2(Order);
						}
					}
					reentry_m1_bflag=11;
				} else {
					// 通信対戦時
					// 捨牌対応報告待ち状態
					if( is_reentry == true ) {
//0426 mt ->
//						struct_hai_sutehai_act_notify rbuf_;
//						CmdHistory_ cmd_;
//0425 mt ->
//						cmd_ = stReEntry_mycommand[ stReEntry_mycommand_count];		//cmd_ = stReEntry_mycommand + stReEntry_mycommand_count;

//						rbuf_ = (m_brewMps.m_data.msg.Pai.hai_sutehai_act_notify);

//0501mt 						m_brewMps.m_data.msg.Pai.hai_sutehai_act_notify.Act			= stReEntry_mycommand[ stReEntry_mycommand_count].HaiState;
//0501mt 						m_brewMps.m_data.msg.Pai.hai_sutehai_act_notify.ActedHouseID = stReEntry_mycommand[ stReEntry_mycommand_count].SuteHouseID;
//0501mt 						m_brewMps.m_data.msg.Pai.hai_sutehai_act_notify.ActHouseID	= stReEntry_mycommand[ stReEntry_mycommand_count].ActHouseID;
//0501mt 						m_brewMps.m_data.msg.Pai.hai_sutehai_act_notify.ChiNo		= stReEntry_mycommand[ stReEntry_mycommand_count].ChiState;
//0501mt 						m_brewMps.m_data.msg.Pai.hai_sutehai_act_notify.SuteHai		= stReEntry_mycommand[ stReEntry_mycommand_count].SuteHai;
//0501mt //0425 mt <-
//0501mt 						SutehaiActNotifyRcv( );
//0501mt //0426 mt <-

						SutehaiActNotifyRcv(
							(char)0,
							stReEntry_mycommand[ stReEntry_mycommand_count].HaiState,
							stReEntry_mycommand[ stReEntry_mycommand_count].ActHouseID,
							stReEntry_mycommand[ stReEntry_mycommand_count].SuteHouseID,
							stReEntry_mycommand[ stReEntry_mycommand_count].ChiState,
							stReEntry_mycommand[ stReEntry_mycommand_count].SuteHai
						);

						++(stReEntry_mycommand_count);

						//> 2006/03/29 No.211 再接続時にロン牌が出ると見逃す
						// 再接続終了
						if( stReEntry_mycommand_count == stReEntry_mycommand_count_max ) {
							DebLog(("---------- reentry done ----------"));

							gsPlayerWork[ game_player ].byPlflg = 1;
							is_reentry = false;
						}
						//> 2006/03/29 No.211 再接続時にロン牌が出ると見逃す
					}
				}
				#endif //-*todo:通信
				break;

			case (11):
				/* 鳴きの処理 */
				naki_m6_main();

				reentry_m1_bflag = 12;
				#if true //-*todo:描画
				paintF= true;		//11 → 12
				#endif //-*描画
				break;

			case (12): {
					/* 鳴きの後処理 */
					Optno[0] = (byte)OP.TSUMO;
					Optno[1] = (byte)OP.TSUMO;
					Optno[2] = (byte)OP.TSUMO;
					Optno[3] = (byte)OP.TSUMO;

					hide_hkhai = 0;

					// 特別流局ででは無い場合
					switch( Pinch ) {
						case (byte)SP.HOAN :
						case (byte)SP.FOUR_KAN :
						case (byte)SP.FOUR_FON :
						case (byte)SP.FOUR_RIC :
						case (byte)SP.NINE_HAI : {
							// ここに入るのはおかしいらしい
							reentry_m1_bflag = 5;
						}
						break;
						case (byte)SP.AGARI :
						case (byte)SP.THREE_CHAN : //三家和の処理はロンチェック側にあるのでスルー
						default : {
							#if true //-*todo:描画
							paintF= true;		//12 → 1 or 2
							#endif //-*todo:描画
							/* ナキも入っていないし、リンシャンでもない */
							if (( Status & ( (byte)ST.NAKI | (byte)ST.RINSH )) != 0 ) {
								/* メニューオプションクリア*/
								clropt_opt();

								reentry_m1_bflag = 2;

								before_richi_m4_bflag = 100;
								after_richi_m4_bflag = 100;
							} else {
								/* メニューオプションクリア*/
								clropt_opt();
								reentry_m1_bflag = 1;
							}
						}
						break;
					}
				}
				break;

			case ( 5 ): {	 /*** 局終了 ***/
				#if true //-*todo:描画
				paintF= true;		//5
				#endif //-*todo:描画
				if( is_reentry == true ) {		//再接続フラグ
				#if true //-*todo:
					DebLog(("---------- reentry retry ----------"));
				#endif //-*todo:

					reentry_try_count = 0;
					reentry_time	   = 0;
					is_reentry = false;

					mj_sts = 99;
				#if false //-*todo:通信保留

					// 再接続関連開放
//					SAFE_FREE(stReEntry);

					//// 再接続に失敗した事を表示
					ReConnectWaitDrawMode = MSGTYPE_ENTRY_MATCH_RETRY_RESPONSE_NG;

					// 再接続挑戦
					modeChange( D_NET_RETRY_CONNECT_WAIT_MODE );
				#endif //-*todo:通信保留
					reentry_m1_bflag = 0x0;
					return;
				}

				#if false //-*todo:通信
				// 局終了をサーバーへ送信
				if( IS_NETWORK_MODE )
					SendKyokuEndNotify( );
				#endif //-*todo:通信

				/* メニューオプションクリア*/
				clropt_opt();

				if ( Pinch != (byte)SP.AGARI ) {
					/*** 上がり以外処理 ***/
					gpsTableData.byKamicha_dori = 0xFF;
					dpinch_m7();						/* 特別流局処理 */
					set_for_nextplay((byte) 0 );		/* new (in mjycalc.c) */
				} else {
					/*** 上がりの処理 ***/
//					talkcounter = 0;							//0422
//					gpsTableData.byKamicha_dori = Order;		//0422
					for ( l = 0; l < MJDefine.MAX_TABLE_MEMBER; l++ ) {
						/* 旧ポイントをキープ */
//m						gpsTableData.sMemData[l].nOldPoint	=	gpsTableData.sMemData[l].nPoint;
						gpsTableData.sMemData[l].nOldChip	=	gpsTableData.sMemData[l].nChip;		/* tipnew */
					}

					{
						talkcounter = 0;
						gpsTableData.byKamicha_dori = Order;

						//://_reentry_agari_eff(Rultbl[ RL_2CHAHO ] != 0 && Roncnt >= 2);		/*	上がり効果	*/
						if ( Rultbl[ (int)RL.TWO_CHAHO ] != 0 && Roncnt >= 2 ) {
							/* ダブロンありなら */
							dhora_m7();							/* 手牌表示 */

							if(Rultbl[(int)RL.URA] != 0) {			/*	裏ドラルール有り	*/
#if	Rule_2P
								i = l = (ushort)(Order & 0x01);
#else
								i = l = (USHORT)(Order & 0x03);
#endif
								do {
									if( Hanetbl[ i ] != 0 ) {
										Order = (byte)i;
										setwp_m2(Order);
										jyaku_jy();
										if(_checkRichiYaku()) {
											//_dispUraDora();
										}
									}
									i = (ushort)((i+1) & 0x03);
								} while ( i != l );
							}

#if	Rule_2P
							i = l = (ushort)(Order & 0x01);
#else
							i = l = (USHORT)(Order & 0x03);
#endif
							do {
								if( Hanetbl[ i ] != 0 ) {
									Order = (byte)i;
									setwp_m2(Order);
									jyaku_jy ();
									_agari_func(talkcounter);
									result_order[talkcounter] = Order;
									gMJKResultSub[talkcounter]= (MJK_RESULT)gMJKResult.clone();	//MEMCPY(gMJKResultSub[talkcounter],gMJKResult,sizeof(gMJKResult));
									sMntDataSub[talkcounter]= (MNTDATA)sMntData.clone();		//MEMCPY(sMntDataSub[talkcounter],sMntData,sizeof(sMntData));
									talkcounter ++;
								}
#if	Rule_2P
								i = (ushort)((i+1) & 0x01);
#else
								i = (USHORT)((i+1) & 0x03);
#endif
							} while ( i != l );
						} else {
							/* 頭ハネの時 */
							Roncnt = 1;
							Hanetbl[ Order ] = 0xff;
							dhora_m7();					/* 手牌表示 */

							if(_checkRichiYaku()) {
								//_dispUraDora();
							}
							_agari_func(talkcounter);

							result_order[talkcounter] = Order;
							gMJKResultSub[talkcounter]= (MJK_RESULT)gMJKResult.clone();		//MEMCPY(gMJKResultSub[talkcounter],gMJKResult,sizeof(gMJKResult));
							sMntDataSub[talkcounter]= (MNTDATA)sMntData.clone();			//MEMCPY(sMntDataSub[talkcounter],sMntData,sizeof(sMntData));
						}
					}

					check_urayakitori();
					set_for_nextplay((byte) 1 );			/* new (in mjycalc.c) */
				}

				//20051121 add
#if _DEBUG // 得点デバッグ表示
				if(Pinch!=SP_AGARI)
					TRACE("RYUUKOKU : pinch%d", Pinch );
				else
					if(Ronodr!=0xff)
						TRACE("RON	:AgariHouse:%d	FurikomiHouse:%d",Order,Odrbuf);
					else
						TRACE("TSUMO	:AgariHouse:%d",Order);

				if( Roncnt == 1 ) {
					TRACE("YAKUMAN		:%d",gMJKResult.byYakuman);
					TRACE("YAKU			:%d",gMJKResult.byHan);
					TRACE("FU			:%d",gMJKResult.byFu);
					TRACE("YAKUSUU		:%d",gMJKResult.byYakuCnt);

					for(i=0;i<gMJKResult.byYakuCnt;i++)
						TRACE("NAME		:%d	YAKUSUU	:%d",gMJKResult.sYaku[i].name,gMJKResult.sYaku[i].factor);
				} else {
					for( int dagari_ = 0; dagari_ < talkcounter; ++ dagari_ ) {
						TRACE("DAGARI       :%d",result_order[dagari_]);
						TRACE("YAKUMAN		:%d",gMJKResultSub[dagari_].byYakuman);
						TRACE("YAKU			:%d",gMJKResultSub[dagari_].byHan);
						TRACE("FU			:%d",gMJKResultSub[dagari_].byFu);
						TRACE("YAKUSUU		:%d",gMJKResultSub[dagari_].byYakuCnt);

						for(i=0;i<gMJKResult.byYakuCnt;i++) {
							TRACE("NAME		:%d	YAKUSUU	:%d",
								gMJKResultSub[dagari_].sYaku[i].name,
								gMJKResultSub[dagari_].sYaku[i].factor);
						}
					}
				}
				for( i= 0; i< MAX_TABLE_MEMBER; i++) {	//4
					TRACE("SeisanPoint	:%d",gpsTableData.sMemData[i].nMovePoint);
					TRACE("SeisanPointGO:%d",gpsTableData.sMemData[i].nPoint);
				}
#endif
				reentry_m1_bflag = 0xFF;
			}
			break;
		}
//	return;
}

//-*********************mjm1.j
}
