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
// mjjtnp.j
//-*****************
public partial class MahjongBase : SceneBase {
/*
**		麻雀大会３
**		アルゴ － 聴牌チェック
*/

//#include "MahJongRally.h"								// Module interface definitions

	private const int __mentsu = 0;
	private const int __jmentsu = 1;

/*****************************
	チートイテンパイチェック(1995.5.9 OK!)
*****************************/
public int	jtnp7_jtnp ( /*MahJongRally * pMe,*/ PLST[] p, byte[] l_mp )/*1995.4.24, 5.2*/	//*l_mp
{
	int		x, p0= 0,q0= 0;
	byte[]	q = SubMj.chitoibuf;

	l_mp[0]= 0;
	for(; (x=p[p0].min) != 0; p0++)	//for(; x=p.min; p++)
		do {
			while(cntbuf[x]==0)
				x++;
			if(cntbuf[x]>2)
				return 0;
			else if(cntbuf[x]==1){
				if(l_mp[0] != 0)
					return 0;
				l_mp[0]= (byte)x;
			}
			q[q0]=(byte)x;q0++;			//*q++=x;
		} while(++x<=p[p0].max);	//(++x<=p->max);
	return 1;
}

/*****************************
	聴牌チェック
*****************************/
public int mentsu(/*MahJongRally * ,pMe*/ int x, int mc)
{
	int r;

	if(--mc<0)
		return 1;
	while( cntbuf[x]== 0)
		++x;
	if(cntbuf[x]>=3){
		cntbuf[x]-=3;
		r=mentsu(x, mc);
		cntbuf[x]+=3;
	} else if(x<0x30 && cntbuf[x] != 0 && cntbuf[x+1] != 0 && cntbuf[x+2] != 0){
		--cntbuf[x]; --cntbuf[x+1]; --cntbuf[x+2];
		r=mentsu(x, mc);
		++cntbuf[x]; ++cntbuf[x+1]; ++cntbuf[x+2];
	} else
		return 0;
	return r;
}

public int jmentsu(/*MahJongRally * pMe,*/ int x, int mc)
{
	int r;

	if(--mc<0)
		return 1;
	while( cntbuf[x] == 0)
		++x;
	if(cntbuf[x]>=3){
		cntbuf[x]-=3;
		if(jmentsu(x, mc) != 0){
			cntbuf[x]+=3;
			return 1;
		}
		++cntbuf[x];
		r=mentsu(x, mc);
		cntbuf[x]+=2;
		return r;
	}
	if(cntbuf[x]==2 && mentsu(x+1, mc) != 0)
		return 1;
	if(x<0x30 && cntbuf[x+1] != 0 && cntbuf[x+2] != 0){
		--cntbuf[x]; --cntbuf[x+1]; --cntbuf[x+2];
		r=jmentsu(x, mc);
		++cntbuf[x]; ++cntbuf[x+1]; ++cntbuf[x+2];
		return r;
	}
	return 0;
}

public int _func(int x, int mc, int nfunc)
{
	int		ret= 0;
	switch(nfunc) {
		case __mentsu:		ret= mentsu(x,mc);		break;
		case __jmentsu:		ret= jmentsu(x,mc);		break;
	}
	return ret;
}
public int amentsu( PLST p, int nfunc)		//static int amentsu(/*MahJongRally * pMe,*/ PLST p, int (*func)(/*MahJongRally * pMe,*/ int x, int mc))
{
	byte[]		Div3tbl={0,0,0,1,1,1,2,2,2,3,3,3,4,4,4,5};		//[16]
	//BYTE		Mod3tbl[]={0,1,2,0,1,2,0,1,2,0,1,2,0,1,2,0};		//[16]
	int			x, r= 0;
	int			mc= Div3tbl[p.num+2];

	if(p.min>0x30){
		if(cntbuf[p.min]>=4)
			return 0;
		++cntbuf[p.min];
		if((r=_func(p.min, mc, nfunc)) != 0)
			SubMj.g_mpp[SubMj.g_mpp_p++]=(byte)p.min;		//g_mpp++=p.min;
		--cntbuf[p.min];
		return r;
	}
	x=p.min; r=0;
	if((x&0x0F)>1 && cntbuf[x-1]<4){
		++cntbuf[x-1];
		if(_func(x-1, mc, nfunc) != 0) {
			SubMj.g_mpp[SubMj.g_mpp_p++]=(byte)(x-1); r=1;	}		//g_mpp++=x-1;
		--cntbuf[x-1];
	}
	do
		if(cntbuf[x]<4){
			++cntbuf[x];
			if(_func(p.min, mc, nfunc) != 0) {
				SubMj.g_mpp[SubMj.g_mpp_p++]=(byte)x; r=1;	}		//g_mpp++=x;
			--cntbuf[x];
		}
	while(++x<=p.max);
	if((x&0x0F)<=9 && cntbuf[x]<4){
		++cntbuf[x];
		if(_func(p.min, mc, nfunc) != 0) {
			SubMj.g_mpp[SubMj.g_mpp_p++]=(byte)x; r=1;	}		//g_mpp++=x;
		--cntbuf[x];
	}
	return r;
}

public int	jtnp_jtnp ( /*MahJongRally * pMe,*/ PLST[] p, byte[] l_mp )/*1995.4.25, 5.2, 5.10*/
{
	int j1=0, p0= 0;
	int j2=0;
	int jf1=0, jf2;

	byte[]	Div3tbl={0,0,0,1,1,1,2,2,2,3,3,3,4,4,4,5};	//[16]
	byte[]	Mod3tbl={0,1,2,0,1,2,0,1,2,0,1,2,0,1,2,0};	//[16]

	SubMj.g_mpp= l_mp;	SubMj.g_mpp_p= SubMj.g_mpp[0]= 0;	//(g_mpp=l_mp)=0;
	while(p[p0].min != 0){
		switch(Mod3tbl[p[p0].num]){
		case 0:
			if( mentsu(p[p0].min, Div3tbl[p[p0].num]) == 0)
				return 0;
			break;
		case 1:
			if(j1++ != 0 || j2 != 0 || amentsu(p[p0], __jmentsu) == 0)
				return 0;
			break;
		case 2:
			if(j1 != 0 || ++j2>2)
				return 0;
			if(j2==1){
				jf1=jmentsu(p[p0].min, Div3tbl[p[p0].num+1]);
				if( amentsu(p[p0], __mentsu) == 0 && jf1 == 0)
					return 0;
			} else {
				if((jf2=jmentsu(p[p0].min, Div3tbl[p[p0].num+1])) == 0) {
					SubMj.g_mpp=l_mp;		SubMj.g_mpp_p= 0;	}
				if(( jf1 == 0 || amentsu(p[p0], __mentsu) == 0) && jf2 == 0)
					return 0;
			}
			break;
		}
		p0++;		//++p;
	}
	return SubMj.g_mpp_p;		//g_mpp-l_mp;
}

/**************************************END OF FILE**********************************************/
//-*********************mjjtnp.j
}