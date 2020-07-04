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
// mjsub.j
//-*****************
public partial class MahjongBase : SceneBase {

//#include "MahJongRally.h"								// Module interface definitions

public bool _No_Paidisp(/*MahJongRally * pMe*/)
{
#if true //-*todo:
	// if (IsGameAutoPlay()) {
	if(((sGameData.byFlags & (byte)GAMEFLAG.AUTO) != 0))
	{
		return (false);
	}
#endif //-*todo:
	return (SubMj.CompVSCompMode);
}

/**************************************************************************************************
**	初期化
**************************************************************************************************/
public void InitMJKResult(/*MahJongRally * pMe*/)
{
	int		i;

	for (i = 0; i < MJDefine.MAX_TABLE_MEMBER; i++) {	//4
		gMJKResult.sMemResult[i].wFlag	=	0;
		gMJKResult.sMemResult[i].iPoint	=	0;
	}
}

/**************************************************************************************************
**	鳴いた
**************************************************************************************************/
public void ResultSetNaki(/*MahJongRally * pMe,*/ int iOrder)
{
	short	wTmp;
	int		i;

	gMJKResult.sMemResult[iOrder].wFlag |= (byte)RESF.NAKI;					/*	鳴いた	*/
	wTmp	=	0;
	for (i = 0; i < MJDefine.MAX_TABLE_MEMBER; i++) {
		wTmp	|=	(short)(gMJKResult.sMemResult[i].wFlag & (byte)RESF.N_FIRST);
	}
	if (wTmp == 0) {
		gMJKResult.sMemResult[iOrder].wFlag |= (byte)RESF.N_FIRST;			/*	最初に鳴いた	*/
	}
}

/**************************************************************************************************
**	リーチ
**************************************************************************************************/
public void ResultSetRichi(/*MahJongRally * pMe,*/ int iOrder)			/*	リーチした	*/
{
	short	wTmp;
	int		i;

	gMJKResult.sMemResult[iOrder].wFlag |= (byte)RESF.RICH;					/*	リーチした	*/
	wTmp	=	0;
	for (i = 0; i < MJDefine.MAX_TABLE_MEMBER; i++) {
		wTmp	|=	(short)(gMJKResult.sMemResult[i].wFlag & (byte)RESF.R_FIRST);
	}
	if (wTmp == 0) {
		gMJKResult.sMemResult[iOrder].wFlag |= (byte)RESF.R_FIRST;			/*	最初にリーチした	*/
	}
}

/**************************************************************************************************
**	上がった
**************************************************************************************************/
public void ResultSetWin(/*MahJongRally * pMe,*/ int iOrder, int iOrder2, short iPoint)				/*	上がった	*/
{
	int	i;

	gMJKResult.sMemResult[iOrder].wFlag |= (short)RESF.AGARI;					/*	上がった	*/
	gMJKResult.sMemResult[iOrder].iPoint	=	iPoint;

	for (i = 0; i < gMJKResult.byYakuCnt; i++) {
		if (gMJKResult.sYaku[i].name == (byte)YK.IPPAT) {
			gMJKResult.sMemResult[iOrder].wFlag |= (short)RESF.IPPATU;			/*	一発	*/
		}
	}

	if (gMJKResult.byYakuman != 0 || gMJKResult.byHan >= 13) {
		gMJKResult.sMemResult[iOrder].wFlag |= (short)RESF.YAKUMAN;			/*	役満		*/
	}
	else {
		if (gMJKResult.byHan >= 5 ||
			(gMJKResult.byHan == 4 && gMJKResult.byFu >= 40) ||
			(gMJKResult.byHan == 3 && gMJKResult.byFu >= 80) ||
			(gMJKResult.byHan == 2 && gMJKResult.byFu >= 130)) {
			gMJKResult.sMemResult[iOrder].wFlag |= (short)RESF.MANGAN;			/*	満貫		*/
		}
	}
	if (iOrder2 >= 0) {													/*	ロン上がりの時	*/
		gMJKResult.sMemResult[iOrder].wFlag 	|= (short)RESF.RON;			/*	振り込んだ	*/
		gMJKResult.sMemResult[iOrder2].iPoint	=	(short)(-iPoint);
	}
}

/**************************************************************************************************
**	流局
**************************************************************************************************/
public void ResultSetTanpai(/*MahJongRally * pMe,*/ int iOrder)										/*	流局時テンパイ	*/
{
	gMJKResult.sMemResult[iOrder].wFlag |= (short)RESF.TENPAI;					/*	テンパイ	*/
}



/**************************************END OF FILE**********************************************/
//-*********************mjsub.j
}
