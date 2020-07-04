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
// mjplst.j
//-*****************
public partial class MahjongBase : SceneBase {
/*
**		麻雀大会３
**		牌のカウント
*/

//#include "MahJongRally.h"								// Module interface definitions

/*****************************
	スタート時のパイのカウント
*****************************/
public void setcnt_plst ( /*MahJongRally * pMe*/ )/*1995.4.25, 5.2	正常動作*/
{
	int	n;

	_MEMSET(cntbuf, 0, cntbuf.Length);
	for ( n = 0; n < gpsPlayerWork.byThcnt; n++ ) { /* 手牌の数だけ存在する牌(フーロしていない牌)をカウントする */
		cntbuf[gpsPlayerWork.byTehai[n]]++;
	}
}

/*****************************
*****************************/
public void newlst_plst ( /*MahJongRally * pMe,*/ PLST[] q )/*1995.4.19, 5.1	正常動作*/
{
	int x=1, q0= 0;

	do {
		if(cntbuf[x] != 0){
			q[q0].max=q[q0].min=x;
			q[q0].num=cntbuf[x];
			while(cntbuf[++x] != 0 || cntbuf[++x] != 0) {
				q[q0].num+=cntbuf[(q[q0].max=x)];
			}
			++q0;		//++q;
		}
	} while(++x<=0x29);
	for(x=0x31; x<=0x37; x++) {
		if(cntbuf[x] != 0){
			q[q0].min=q[q0].max=x;
			q[q0].num=cntbuf[x];
			++q0;		//++q;
		}
	}
	q[q0].min=0;
	q[q0].max=0;
	q[q0].num=0;
}

/**************************************END OF FILE**********************************************/
//-*********************mjplst.j
}
