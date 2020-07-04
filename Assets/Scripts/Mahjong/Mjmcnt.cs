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
// Mjmcnt.j
//-*****************
public partial class MahjongBase : SceneBase {
/*
**		麻雀大会３
**		役判定時の面子カウント処理
*/
//#include "MahJongRally.h"								// Module interface definitions

public int cntmnt(/*MahJongRally * pMe,*/ int x, int n)
{
	byte[]	Div3tbl={0,0,0,1,1,1,2,2,2,3,3,3,4,4,4,5};	//[16]

	int sc, kc;

	if((n-=3)<0)
		return 0;
	do
		switch(cntbuf[x]){
		case 4:
			cntbuf[x]=1;
			kc=cntmnt( x, n);
			cntbuf[x]=4;
			return kc+1;
		case 3:
			if((kc=cntmnt( x+1,n))<Div3tbl[n] && cntbuf[x+1]==2 && cntbuf[x+2]>=2){
				--cntbuf[x]; --cntbuf[x+1]; --cntbuf[x+2];
				if((sc=cntmnt( x,n))>kc)
					kc=sc;
				++cntbuf[x]; ++cntbuf[x+1]; ++cntbuf[x+2];
			}
			return kc+1;
		default:
			if(cntbuf[x+1] != 0 && cntbuf[x+2] != 0){
				--cntbuf[x]; --cntbuf[x+1]; --cntbuf[x+2];
				sc=cntmnt( x,n);
				++cntbuf[x]; ++cntbuf[x+1]; ++cntbuf[x+2];
				if(sc<Div3tbl[n] && cntbuf[x+1]==3){
					n-=cntbuf[x];
					if((kc=cntmnt( x+2, n))>sc)
						return kc+1;
				}
				return sc+1;
			}
			goto case 0;
		case 0:
			break;
		}
	while((n-=cntbuf[x++])>=0);
	return 0;
}

public int cntjnt(/*MahJongRally * pMe,*/ int x, int n)
{
	byte[]	Div3tbl={0,0,0,1,1,1,2,2,2,3,3,3,4,4,4,5};	//[16]
	int sc, kc;

	if((n-=2)<0)
		return 0;
	while(n>=3){
		switch(cntbuf[x]){
		case 4:
			cntbuf[x]=1;
			kc=cntjnt( x, n-1);
			cntbuf[x]=4;
			if(kc<Div3tbl[n] && cntbuf[x+1] != 0 && cntbuf[x+2] != 0){
				cntbuf[x]=1; --cntbuf[x+1]; --cntbuf[x+2];
				if((sc=cntmnt( x, n-3)+1)>kc)
					kc=sc;
				cntbuf[x]=4; ++cntbuf[x+1]; ++cntbuf[x+2];
			}
			return kc+1;
		case 3:
			kc=cntjnt( x+1, n-1);
			if(kc<Div3tbl[n] && cntbuf[x+1] != 0 && cntbuf[x+2] != 0){
				cntbuf[x]=2; --cntbuf[x+1]; --cntbuf[x+2];
				if((sc=cntjnt( x, n-1))>kc)
					kc=sc;
				cntbuf[x]=3; ++cntbuf[x+1]; ++cntbuf[x+2];
			}
			return kc+1;
		case 2:
			kc=cntmnt( x+1, n);
			if(kc<Div3tbl[n] && cntbuf[x+1] != 0 && cntbuf[x+2] != 0){
				cntbuf[x]=1; --cntbuf[x+1]; --cntbuf[x+2];
				if((sc=cntjnt( x, n-1))>=kc)
					kc=sc;
				cntbuf[x]=2; ++cntbuf[x+1]; ++cntbuf[x+2];
			}
			return kc+1;
		case 1:
			if(cntbuf[x+1] != 0 && cntbuf[x+2] != 0){
				--cntbuf[x+1]; --cntbuf[x+2];
				sc=cntjnt( x+1, n-1);
				++cntbuf[x+1]; ++cntbuf[x+2];
				if(sc<Div3tbl[n])
					switch(cntbuf[x+1]){
					case 2:
						if((kc=cntmnt( x+2, n-1))>sc)
							sc=kc;
						break;
					case 3:
						if((kc=cntjnt( x+2, n-2))>sc)
							sc=kc;
						break;
					default:
						break;
					}
				return sc+1;
			}
			goto default;
		default:
			n-=cntbuf[x++];
			break;
		}
	}
	do
		if(cntbuf[x]>=2)
			return 1;
	while((n-=cntbuf[x++])>=0);
	return 0;
}

public int	CountMnt(/*MahJongRally * pMe,*/ int x, int n)
{
	return (x<0x30 ? cntmnt( x,n) : (cntbuf[x]>=3 ? 1 : 0));
}

public int	CountJnt(/*MahJongRally * pMe,*/ int x, int n)
{
	return (x<0x30 ? cntjnt( x,n) : (cntbuf[x]>=2 ? 1 : 0));
}

/**************************************END OF FILE**********************************************/

//-*********************Mjmcnt.j
}
