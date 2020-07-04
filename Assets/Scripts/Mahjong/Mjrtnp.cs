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
// mjrtnp.j
//-*****************
public partial class MahjongBase : SceneBase {
/*
**		麻雀大会３
**		リーチ用聴牌チェック
*/

//#include "MahJongRally.h"								// Module interface definitions

	public enum __Check{
		Mnt = 0,
		Jnt = 1
	}

public int _func(int x, int n, int mc, int nfunc)
{
	int		ret= 0;
	switch(nfunc) {
		case (int)__Check.Mnt:	ret= CheckMnt(x,n,mc);		break;
		case (int)__Check.Jnt:	ret= CheckJnt(x,n,mc);		break;
	}
	return ret;
}
public int amentsu(PLST p, int n, int mc, int nfunc)		//amentsu(PLST p[], int n, int mc, int (func)(int x, int n, int mc))
{
	int x, r;

	if(p.min>0x30){
		if(cntbuf[p.min]>=4)
			return 0;
		++cntbuf[p.min];
		r=_func( p.min, n, mc, nfunc);
		--cntbuf[p.min];
		return r;
	}
	x=p.min; r=0;
	if((x&0x0F)>1 && cntbuf[x-1]<4){
		++cntbuf[x-1];
		r=_func( x-1, n, mc, nfunc);
		--cntbuf[x-1];
		if(r != 0)
			return 1;
	}
	do
		if(cntbuf[x]<4){
			++cntbuf[x];
			r=_func( p.min, n, mc, nfunc);
			--cntbuf[x];
			if(r != 0)
				return 1;
		}
	while(++x<=p.max);
	if((x&0x0F)<=9 && cntbuf[x]<4){
		++cntbuf[x];
		r=_func( p.min, n, mc, nfunc);
		--cntbuf[x];
	}
	return r;
}

public bool rtnp(/*MahJongRally * pMe,*/ PLST[] p)
{
	byte[]	Div3tbl={0,0,0,1,1,1,2,2,2,3,3,3,4,4,4,5};	//[16]
	byte[]	Mod3tbl={0,1,2,0,1,2,0,1,2,0,1,2,0,1,2,0};	//[16]
	int mc, im;
	int jflag=0;
	int mx=0;
	int mz=0;
	int c1=0, p0= 0;

	while(p[p0].min != 0){
		im=Div3tbl[p[p0].num];
		switch(Mod3tbl[p[p0].num]){
		  case 0:
			if((mc=CountMnt( p[p0].min, p[p0].num))+1<im)
				return (false);
			if(mc==4)
				return (true);
			if(mc<im) {
				if((mz++) != 0) {
					return (false);
				}
				else if( jflag == 0 && CheckJnt( p[p0].min, 1, im) != 0) {
					jflag=1;
				}
				else if( mx == 0 && amentsu( p[p0], 1, im, (int)__Check.Mnt) != 0) {
					mx=1;
				}
				else {
					return (false);
				}
			}
			break;
		  case 1:
			if( mz == 0 && CheckMnt( p[p0].min, 1, im) != 0){
				if(im==4) {
					return (true);
				}
				mz=c1=1;
			}
			else if( mx == 0 && jflag == 0 && (c1 != 0 && CheckMnt( p[p0].min, 1, im) != 0 || amentsu( p[p0], 0, im+1, (int)__Check.Jnt) != 0)) {
				mx=jflag=1;
			}
			else {
				return (false);
			}
			break;
		  case 2:
			if((mc=CountMnt( p[p0].min, p[p0].num))+1<im) {
				return (false);
			}
			if(mc==4) {
				return (true);
			}
			if(mc==im && jflag == 0 && CheckJnt( p[p0].min, 0, im+1) != 0) {
				jflag=1;
			}
			else if(mc==im && mx == 0 && amentsu( p[p0], 0, im+1, (int)__Check.Mnt) != 0) {
				mx=1;
			}
			else if( mz == 0 && mx == 0 && jflag == 0 && amentsu( p[p0], 1, im+1, (int)__Check.Jnt) != 0) {
				mz=mx=jflag=1;
			}
			else {
				return (false);
			}
			break;
		}
		p0++;		//++p;
	}
	return (true);
}

public bool	rtnp7(PLST[] p)
{
	int x, p0= 0;
	int triple=0, single=0;

	if(gpsPlayerWork.byFhcnt != 0) {
		return (false);
	}
	for(; (x=p[p0].min) != 0; p0++) {		//p++
		do {
			while(cntbuf[x]==0) {
				x++;
			}
			switch(cntbuf[x]){
			  case 4:
				return (false);
			  case 3:
				if((triple++) != 0) {
					return (false);
				}
				break;
			  case 1:
				if(++single>2 || single==2 && triple != 0) {
					return (false);
				}
				break;
			  default:
				break;
			}
		} while(++x<=p[p0].max);
	}
	return (true);
}

public bool rtnp_rtnp(PLST[] p)/*1995.6.15, 7.28*/
{
	if (rtnp( p)) {
		return (true);
	}
	return (rtnp7( p));
}

/**************************************END OF FILE**********************************************/
//-*********************mjrtnp.j
}
