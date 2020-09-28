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
// Mjgame.j
//-*****************
public partial class MahjongBase : SceneBase {
/*
**		麻雀大会３
**		半荘処理
*/

//#include "MahJongRally.h"								// Module interface definitions

/*****************************
	ドボンチェック
        dobon_f = FALSE:ドボンがなかった
                  TRUE :ドボンがあった
*****************************/
public bool chkdbn_game ( /*MahJongRally * pMe*/ )
{
	bool	dobon_f = false;
	//tamaki int		iPoint;

	if( Rultbl[ (int)RL.DOBON ] != 0  )		/* ドボン有りなら */
		for(int i = 0; i < MJDefine.MAX_TABLE_MEMBER; i++)
			if (gpsTableData.sMemData[i].nPoint < 0) {
				dobon_f = true;
				break;
			}

	return	( dobon_f );
}

/*****************************
	半荘終了チェック
        end_f = FALSE:継続
                TRUE :終了
*****************************/
public bool chkend_game ( /*MahJongRally * pMe*/ )
{
	short	point, i;
	bool	end_f = false;

	if( chkdbn_game() == true ) {	/* ドボンで終了 */
		end_f = true;
		#if true //-*todo:
		DebLog(("DOBON END"));
		#endif//-*todo:
	} else {
		if( !( gpsTableData.byKyoku < Kyoend ) ) {			/* オーラス */
			if( Rultbl[ (int)RL.KAZE ] == 2 ) {	/* 東風 */
				end_f = true;
				#if true //-*todo:
				DebLog(("TONPU TONBA END"));
				#endif //-*todo:
			} else {	/* 東風ではない */
				if( Rultbl[ (int)RL.SHANY ] > 0 ) {	/* 西入が「有り」 */
					for( i = 0; i < MJDefine.MAX_TABLE_MEMBER; i ++ ) {
						point = get_mjpoint( i );
						if( point >= sRuleSubData.nToplin) {	/* 西入得点以上である */
							end_f = true;
							#if true //-*todo:
							DebLog(("chkend_game:TONNAN SYAIRI OK NANBA END"));
							#endif
							break;
						}
					}
					if( end_f == false ) {
						Kyoend += 4;						/* 西入処理 */
						#if true //-*todo:
						DebLog(("chkend_game:SYANYU"));
						#endif
					}
				} else {	/* 西入がない	*/
					end_f = true;
					#if true //-*todo:
					DebLog(("chkend_game:TONNAN NANBA END"));
					#endif
				}
			}
		}
	}

#if	Rule_2P
//********ウキウキ:表情操作********
	#if true //-*todo:描画関連
	if( end_f) {
		if( gpsTableData.sMemData[0].nPoint>= gpsTableData.sMemData[1].nPoint){
			m_keepData.flag_res_battle= 0;	// 0:勝利/1:敗北
			// FaceNum = (D_FACE_12-D_FACE_00);
		}else{
			m_keepData.flag_res_battle= 1;	// 0:勝利/1:敗北
			// FaceNum = (D_FACE_15-D_FACE_00);
		}
	}
	#endif //-*todo:描画関連
//*********************************
#endif

	return	( end_f );
}

/*****************************
	半荘初期化
*****************************/
public void inthanc_game ( /*MahJongRally * pMe*/ )
{
	short	i;
	short	nPointE;
	ushort[]	Shatbl = { 0, 300, 301, 331, 351 };							//Shatbl[ 5 ]
	short[]	Spnttbl	=	{ 240, 250, 260, 270, 280, 290, 300, 160 };		//Spnttbl[8]

	sRuleSubData.nToplin	= (short)Shatbl[ Rultbl[ (int)RL.SHANY ] ];				/* 西入得点 */

	gpsTableData.byKyoku	= 0;			/* 局のナンバー */
	gpsTableData.byRibo		= 0;			/* 場のリーボーの数 */
	gpsTableData.byRenchan	= 0;			/* 連チャン回数 */
	gpsTableData.byParen	= 0;			/* 八連荘フラグ */

//#ifdef _DEBUG
// -for debug 落ちバグ検証
//		gpsTableData->byParen	= 36;		/* 八連荘フラグ */
//		gpsTableData->byKyoku = 45;			/* 局のナンバー */
//		gpsTableData->byRibo =10;
//		gpsTableData->byRenchan=36;
//#endif

	/* 配給原点セット */
	nPointE	=	Spnttbl[ Rultbl[ (int)RL.POINT ] ];
	for(i = 0; i < MJDefine.MAX_TABLE_MEMBER; i++) {
		gpsTableData.sMemData[i].nPoint	=	nPointE;
		gpsTableData.sMemData[i].nChip	=	0;
	}

#if	Rule_2P
	#if true //-*todo:↓変換済
	//点数ハンデ戦（自分15000点、相手35000点）
	if( m_keepData.mah_limit_num == (int)MAH.LIM02) {
		gpsTableData.sMemData[0].nPoint=	150;
		gpsTableData.sMemData[1].nPoint=	350;
	}
	//点数ハンデ戦（自分20000点、相手30000点）
	if( m_keepData.mah_limit_num == (int)MAH.LIM07) {
		gpsTableData.sMemData[0].nPoint=	200;
		gpsTableData.sMemData[1].nPoint=	300;
	}
	//***********ウキウキ:ルール追加************
	//点数ハンデ戦（自分10000点、相手40000点）
	if( m_keepData.mah_limit_num == (int)MAH.LIM08) {
		gpsTableData.sMemData[0].nPoint=	100;
		gpsTableData.sMemData[1].nPoint=	400;
	}
	//******************************************
	#else //-*todo↓元を残しとく
	//点数ハンデ戦（自分15000点、相手35000点）
	if( myCanvas.mah_limit_num== MAH_LIM02) {
		gpsTableData.sMemData[0].nPoint=	150;
		gpsTableData.sMemData[1].nPoint=	350;
	}
	//点数ハンデ戦（自分20000点、相手30000点）
	if( myCanvas.mah_limit_num== MAH_LIM07) {
		gpsTableData.sMemData[0].nPoint=	200;
		gpsTableData.sMemData[1].nPoint=	300;
	}
	//***********ウキウキ:ルール追加************
	//点数ハンデ戦（自分10000点、相手40000点）
	if( myCanvas.mah_limit_num== MAH_LIM08) {
		gpsTableData.sMemData[0].nPoint=	100;
		gpsTableData.sMemData[1].nPoint=	400;
	}
	//******************************************
	#endif //-*todo:元を残しとく
#endif

	//tamaki gpsTableData.byChicha	= mj_rand () % 4;			/* 起家決め */
#if _DEBUG
	// デバッグ用に常にプレイヤーから始める。
	gpsTableData.byChicha	= 0;
#else //-*DEBUG
	#if	Rule_2P
	//	gpsTableData.byChicha	= (BYTE)mj_getrand(2);
	gpsTableData.byChicha	= 0;
	#else
	gpsTableData.byChicha	= (BYTE)mj_getrand(MAX_TABLE_MEMBER);	//4	/* 起家決め */
	#endif
#endif

	gpsTableData.byOya	= gpsTableData.byChicha;				/* 起親は親 */

	if( Rultbl[ (int)RL.KAZE ] < 2 )
		Kyoend = 8;						/* 東東・東南 */
	else
		Kyoend = 4;						/* 東風 */

	Totsunyu_flg = 0xFF;				/* 1996.7.5.DIALOG */
	for( i = 0; i < MJDefine.MAX_TABLE_MEMBER; i ++ ){
		/* 焼き鳥のリセット */
		gpsTableData.sMemData[i].byMemFlags	=	(byte)TBLMEMF.TORI;
	}
	gpsTableData.byOLDTop	=	0xFF;
}

public void _setTableMemberInfo(/*MahJongRally * pMe*/)
{
	int		i;
	byte	byMem;

	for (i = (int)MJ_POS.B; i <= (int)MJ_POS.D; i++) {
		byMem	=	gsTableData[0].sMemData[i].byMember;
	}
}

public void _setTableData(/*MahJongRally * pMe*/)
{
	int	i;

	gsTableData[0].byFlags	=	(byte)TBLF.USER;
	gsTableData[1].byFlags	=	0;
	gsTableData[2].byFlags	=	0;

	switch (sGameData.byGameMode)
	{
	  case MJDefine.GAMEMODE_SURVIVAL:							/*	サバイバル		*/
		gsTableData[0].sMemData[0].byMember	=	(byte)MJ_CHARCTER.CHAR_SILHOUETTE;
		/*	プレイヤーでも指導モードがあるのでキャラをセットする	*/
		gsTableData[0].sMemData[0].NickName= MJDefine.D_PLAYER_DEFAULT_NICNAME;		//STRNCPY(gsTableData[0].sMemData[0].NickName, D_PLAYER_DEFAULT_NICNAME, D_NICK_NAME_MAX + 1  );
		//tamaki 20051118 add
		for(i = 1; i < MJDefine.MAX_TABLE_MEMBER;i++)
		{
			gsTableData[0].sMemData[i].byMember	= SurvivalNewMember();
			gsTableData[0].sMemData[i].NickName= MJDefine.CharNickNameTable[ gsTableData[0].sMemData[i].byMember];	//STRCPY(gsTableData[0].sMemData[i].NickName, CharNickNameTable[ gsTableData[0].sMemData[i].byMember ] );
			//tamaki 20051118 add
		}
		goto case MJDefine.GAMEMODE_FREE;
	  case MJDefine.GAMEMODE_FREE:								/*	フリーゲーム		*/
		gsTableData[0].byFlags	|=	(byte)TBLF.ACTIVE;
		gsTableData[1].byFlags	|=	(byte)TBLF.FINISH;
		gsTableData[2].byFlags	|=	(byte)TBLF.FINISH;
		gpsTableData	=	gsTableData[0];		//&gsTableData[0];
		inthanc_game ();		/* 半荘のイニット */
		break;
	  case MJDefine.GAMEMODE_NET_FREE:							/* 通信　フリー対戦	*/
	  case MJDefine.GAMEMODE_NET_RESERVE:						/* 通信　卓指定対戦	*/
		gsTableData[0].byFlags	|=	(byte)TBLF.ACTIVE;
		gsTableData[1].byFlags	|=	(byte)TBLF.FINISH;
		gsTableData[2].byFlags	|=	(byte)TBLF.FINISH;
		gpsTableData	=	gsTableData[0];		//&gsTableData[0];
		inthanc_game ();		/* 半荘のイニット */
		break;
	}
	gpsTableData	=	gsTableData[0];			//&gsTableData[0];
}

public void SetupGame(/*MahJongRally * pMe*/)
{
	gsTableData[0].byFlags	=	gsTableData[1].byFlags	=	gsTableData[2].byFlags	=	0;
}

/************************* END OF FILE **********************************/

//-*********************Mjgame.j
}
