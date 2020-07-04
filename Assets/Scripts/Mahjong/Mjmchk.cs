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
// mjmchk.j
//-*****************
public partial class MahjongBase : SceneBase {
/*
**		麻雀大会３
**		役判定時の面子チェック
*/

//#include "MahJongRally.h"								// Module interface definitions

/* プロトタイプ宣言 */
public int chkmnt(/*MahJongRally * pMe,*/ int x, int n, int mc)
{
	int r;

	if(--mc<0)
		return 1;
	do
		switch(cntbuf[x]){
		case 4:
			cntbuf[x]=1;
			r=chkmnt( x, n, mc);
			cntbuf[x]=4;
			return r;
		case 3:
			if(chkmnt( x+1, n, mc)!= 0)
				return 1;
			if(x>=0x30 || cntbuf[x+1]!=2 || cntbuf[x+2]<2)
				return 0;
			--cntbuf[x]; --cntbuf[x+1]; --cntbuf[x+2];
			r=chkmnt( x, n, mc);
			++cntbuf[x]; ++cntbuf[x+1]; ++cntbuf[x+2];
			return r;
		default:
			if(x<0x30 && cntbuf[x+1] != 0 && cntbuf[x+2] != 0){
				--cntbuf[x]; --cntbuf[x+1]; --cntbuf[x+2];
				r=chkmnt( x, n, mc);
				++cntbuf[x]; ++cntbuf[x+1]; ++cntbuf[x+2];
				return (r != 0 || (n-=cntbuf[x])>=0 && cntbuf[x+1]==3 && chkmnt( x+2, n, mc) != 0) ? 1 : 0;
			}
			goto case 0;
		case 0:
			break;
		}
	while((n-=cntbuf[x++])>=0);
	return 0;
}

public int chkjnt(/*MahJongRally * pMe,*/ int x, int n, int mc)
{
	int r;

	if(--mc<0)
		return 1;
	do
		switch(cntbuf[x]){
		case 4:
			cntbuf[x]=1;
			r=chkjnt( x, n, mc);
			cntbuf[x]=4;
			if( r == 0 && x<0x30 && cntbuf[x+1] != 0 && cntbuf[x+2] != 0){
				cntbuf[x]=1; --cntbuf[x+1]; --cntbuf[x+2];
				r=chkmnt( x, n, mc-1);
				cntbuf[x]=4; ++cntbuf[x+1]; ++cntbuf[x+2];
			}
			return r;
		case 3:
			if(chkjnt( x+1, n, mc) != 0)
				return 1;
			if(x<0x30 && cntbuf[x+1] != 0 && cntbuf[x+2] != 0){
				cntbuf[x]=2; --cntbuf[x+1]; --cntbuf[x+2];
				r=chkjnt( x, n, mc);
				cntbuf[x]=3; ++cntbuf[x+1]; ++cntbuf[x+2];
				return r;
			}
			return 0;
		case 2:
			if(chkmnt( x+1, n, mc) != 0)
				return 1;
			if(x<0x30 && cntbuf[x+1] != 0 && cntbuf[x+2] != 0){
				cntbuf[x]=1; --cntbuf[x+1]; --cntbuf[x+2];
				r=chkjnt( x, n, mc);
				cntbuf[x]=2; ++cntbuf[x+1]; ++cntbuf[x+2];
				return r;
			}
			return 0;
		case 1:
			if(x<0x30 && cntbuf[x+1] != 0 && cntbuf[x+2] != 0){
				--cntbuf[x+1]; --cntbuf[x+2];
				r=chkjnt( x+1, n, mc);
				++cntbuf[x+1]; ++cntbuf[x+2];
				return ( r != 0 || --n>=0 &&
					(cntbuf[x+1]==2 && chkmnt( x+2, n, mc) != 0 ||
					cntbuf[x+1]==3 && chkjnt( x+2, n, mc) != 0)) ? 1 : 0;
			}
			goto default;
		default:
			break;
		}
	while((n-=cntbuf[x++])>=0);
	return 0;
}

public int CheckJnt(/*MahJongRally * pMe,*/ int x, int n, int mc)
{
	return (x<0x30 ? chkjnt( x, n, mc) != 0 : (mc==0 || cntbuf[x]>=2)) ? 1 : 0;
}

public int CheckMnt(/*MahJongRally * pMe,*/ int x, int n, int mc)
{
	return (x<0x30 ? chkmnt( x, n, mc) != 0 : (mc==0 || cntbuf[x]>=3)) ? 1 : 0;
}
/**************************************END OF FILE**********************************************/
//-*********************mjmchk.j
}
