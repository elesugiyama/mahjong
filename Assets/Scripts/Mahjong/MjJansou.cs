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
// jansou.j
//-*****************
public partial class MahjongBase : SceneBase {
/*****************************************************************************
* 麻雀大会 for PS2
* "jansou.c" 雀荘処理
*****************************************************************************/
//#include "MahJongRally.h"								// Module interface definitions

/*********************************************************************
	雀荘メイン
*********************************************************************/
public void	_initTableData(/*MahJongRally * pMe*/)
{
	byte	i;
	for (i = 0; i < MJDefine.MAX_TABLE; i++) {
		gsTableData[i].byTableNo		=	i;
		gsTableData[i].byDrawRenchan	=	0;
	}
}

public void	setFreeMentsu(/*MahJongRally * pMe,*/ byte[] pbyMentsu)	//BYTE *pbyMentsu
{
	byte[]	byBuf= new byte [MJDefine.MAX_COMP_CHARACTER];
	int		l_iMax;
	int		i, j, P= 0;
	byte	byMem;

	l_iMax	=	IsGameClear() ? MJDefine.MAX_COMP_CHARACTER : MJDefine.MAX_NORMAL_CHARACTER;

	for (i = 0; i < MJDefine.MAX_COMP_CHARACTER; i++)
		byBuf[i]	=	(byte)i;

	for (i = 0; i < 3; i++) {
		byMem	=	(byte)mj_getrand((ushort)(l_iMax - i));

		for (j = 0; ; j++) {
			if (byBuf[j] != MJDefine.NONE) {
				if (byMem == 0)
					break;
				--byMem;
			}
		}
		pbyMentsu[P++]	=	byBuf[j];		//*pbyMentsu
//		++pbyMentsu;
		byBuf[j]	=	MJDefine.NONE;
	}
}

public bool game_free(/*MahJongRally * pMe*/)
{
	byte[]	byMentsu= new byte [MJDefine.MAX_TABLE_MEMBER - 1];

	gpsTableData	=	gsTableData[0];		//&gsTableData[0];

	setFreeMentsu(byMentsu);											/* 面子セット */
#if false //-*todo:保留
	gsTableData[0].sMemData[0].byMember	=	MJ_CHAR_SILHOUETTE;			/*	プレイヤーでも指導モードがあるのでキャラをセットする	*/
#endif //-*todo;保留

	gsTableData[0].sMemData[1].byMember	=	(byte)sel_buf[0];
	gsTableData[0].sMemData[2].byMember	=	(byte)sel_buf[1];
	gsTableData[0].sMemData[3].byMember	=	(byte)sel_buf[2];

	gsTableData[0].sMemData[0].NickName= MJDefine.D_PLAYER_DEFAULT_NICNAME;			//STRNCPY(gsTableData[0].sMemData[0].NickName, D_PLAYER_DEFAULT_NICNAME, D_NICK_NAME_MAX + 1  );		//tamaki 20051118 add
	gsTableData[0].sMemData[1].NickName= MJDefine.CharNickNameTable[ sel_buf[0] ];	//STRNCPY(gsTableData[0].sMemData[1].NickName, CharNickNameTable[ sel_buf[0] ],D_NICK_NAME_MAX + 1 );	//tamaki 20051118 add
	gsTableData[0].sMemData[2].NickName= MJDefine.CharNickNameTable[ sel_buf[1] ];	//STRNCPY(gsTableData[0].sMemData[2].NickName, CharNickNameTable[ sel_buf[1] ],D_NICK_NAME_MAX + 1 );	//tamaki 20051118 add
	gsTableData[0].sMemData[3].NickName= MJDefine.CharNickNameTable[ sel_buf[2] ];	//STRNCPY(gsTableData[0].sMemData[3].NickName, CharNickNameTable[ sel_buf[2] ],D_NICK_NAME_MAX + 1 );	//tamaki 20051118 add

	//< 2006/03/12 差し馬削除
	//ClearSashiuma(gpsTableData);

	/* 面子セットしたら戻れない */
	SetupGame();

	///////////////////////////////////////////////////////
	// ホーム（メインメニュー）でフェイドアウトしている
	// のをここでフェイドインする
	///////////////////////////////////////////////////////

	SetRuleData((byte)MJDefine.GAMEID_FREE);										/*	ルール設定	*/
	return true;
}

public bool game_wireless(/*MahJongRally * pMe*/)
{
	gpsTableData	=	gsTableData[0];		//&gsTableData[0];

	gsTableData[0].sMemData[0].byMember	=	0;
	gsTableData[0].sMemData[1].byMember	=	1;
	gsTableData[0].sMemData[2].byMember	=	2;
	gsTableData[0].sMemData[3].byMember	=	3;

	/* 20051112 add */
	//< 2006/03/12 差し馬削除
	//ClearSashiuma(pMe, gpsTableData);

	/* 面子セットしたら戻れない */
	SetupGame();
	SetRuleData((byte)MJDefine.GAMEID_FREE);										/*	ルール設定	*/
	return true;
}

/****************************************
	サバイバル面子
****************************************/
public void SurvivalMemberDrop(/*MahJongRally * pMe,*/ byte chr_no)
{
	int i;

	for(i = 0; i < MJDefine.MAX_COMP_CHARACTER;i++)
	{
		if(MJDefine.SurvivalMentsuTable[i] == chr_no)
		{
			SubMj.SurvivalMentsuFlg[i] = (byte)SURVIVAL.DROP;
			break;
		}
	}
}

/****************************************
	サバイバル面子
****************************************/
public byte SurvivalNewMember(/*MahJongRally * pMe*/)
{
	int i;

	for(i = 0; i < MJDefine.MAX_COMP_CHARACTER;i++)
	{
		if(SubMj.SurvivalMentsuFlg[i] == (byte)SURVIVAL.NONE)
		{
			SubMj.SurvivalMentsuFlg[i] = (byte)SURVIVAL.MEM;
			return(MJDefine.SurvivalMentsuTable[i]);
		}
	}
	return(MJDefine.NONE);
}

/****************************************
	サバイバル面子
****************************************/
public byte SurvivalMemberClear(/*MahJongRally * pMe*/)
{
	int i;

	for(i = 0; i < MJDefine.MAX_COMP_CHARACTER;i++)
	{
		if(SubMj.SurvivalMentsuFlg[i] == (byte)SURVIVAL.MEM)
		{
			SubMj.SurvivalMentsuFlg[i] = (byte)SURVIVAL.NONE;
		}
	}
	return(MJDefine.NONE);
}

public bool game_survival(/*MahJongRally * pMe*/)
{
	int	i;

	gpsTableData	=	gsTableData[0];		//&gsTableData[0];

	if(SubMj.survival_stage_no == 0)
	{
		for(i = 0; i < MJDefine.MAX_COMP_CHARACTER;i++)
		{
			SubMj.SurvivalMentsuFlg[i] = (byte)SURVIVAL.NONE;
		}
	}

	//< 2006/03/12 差し馬削除
	//ClearSashiuma(gpsTableData);

	/* 面子セットしたら戻れない */
	SetupGame();

	///////////////////////////////////////////////////////
	// ホーム（メインメニュー）でフェイドアウトしている
	// のをここでフェイドインする
	///////////////////////////////////////////////////////

	SetRuleData((byte)MJDefine.GAMEID_FREE);										/*	ルール設定	*/
	return true;
}
/*************************END OF FILE*****************************************/



//-*********************jansou.j
}
