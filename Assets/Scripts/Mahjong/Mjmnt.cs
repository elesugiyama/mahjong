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
// Mjmnt.j
//-*****************
public partial class MahjongBase : SceneBase {
/*
**		麻雀大会３
**		役判定時の面子確定処理
*/
//#include "MahJongRally.h"								// Module interface definitions

public int mks(/*MahJongRally * pMe,*/ int x, int mc)
{
	int r;

	if(--mc<0)
		return 1;
	while( cntbuf[x] == 0)
		++x;
	if(cntbuf[x]>=3){
		cntbuf[x]-=3;
		r=mks( x, mc);
		cntbuf[x]+=3;
		if(r != 0){
			SubMj.mp[SubMj.mp_p++]=(byte)(x|0x40);		//	*mp++
			return 1;
		} else
			return 0;
	}
	if(x<0x30 && cntbuf[x+1] != 0 && cntbuf[x+2] != 0){
		--cntbuf[x]; --cntbuf[x+1]; --cntbuf[x+2];
		r=mks( x, mc);
		++cntbuf[x]; ++cntbuf[x+1]; ++cntbuf[x+2];
		if(r != 0){
			SubMj.mp[SubMj.mp_p++]=(byte)x;			//	*mp++
			return 1;
		}
	}
	return 0;
}

public int mkjs(/*MahJongRally * pMe,*/ int x, int mc)
{
	int r;

	if(--mc<0)
		return 1;
	while( cntbuf[x] == 0)
		++x;
	if(cntbuf[x]>=3){
		cntbuf[x]-=3;
		r=mkjs( x, mc);
		cntbuf[x]+=3;
		if(r != 0){
			SubMj.mp[SubMj.mp_p++]=(byte)(x|0x40);		//	*mp++
			return 1;
		}
	}
	if(cntbuf[x]>=2){
		cntbuf[x]-=2;
		r=mks( x, mc);
		cntbuf[x]+=2;
		if(r != 0){
			sMntData.byJanto=(byte)x;
			return 1;
		}
	}
	if(x<0x30 && cntbuf[x+1] != 0 && cntbuf[x+2] != 0){
		--cntbuf[x]; --cntbuf[x+1]; --cntbuf[x+2];
		r=mkjs( x, mc);
		++cntbuf[x]; ++cntbuf[x+1]; ++cntbuf[x+2];
		if(r != 0){
			SubMj.mp[SubMj.mp_p++]=(byte)x;		//	*mp++
			return 1;
		}
	}
	return 0;
}

public int msk(/*MahJongRally * pMe,*/ int x, int mc)
{
	int r;

	if(--mc<0)
		return 1;
	while( cntbuf[x] == 0)
		++x;
	if(x<0x30 && cntbuf[x+1] != 0 && cntbuf[x+2] != 0){
		--cntbuf[x]; --cntbuf[x+1]; --cntbuf[x+2];
		r=msk( x, mc);
		++cntbuf[x]; ++cntbuf[x+1]; ++cntbuf[x+2];
		if(r != 0){
			SubMj.mp[SubMj.mp_p++]=(byte)x;		//	*mp++
			return 1;
		}
	}
	if(cntbuf[x]>=3){
		cntbuf[x]-=3;
		r=msk( x, mc);
		cntbuf[x]+=3;
		if(r != 0){
			SubMj.mp[SubMj.mp_p++]=(byte)(x|0x40);		//	*mp++
			return 1;
		}
	}
	return 0;
}

public int mjsk(/*MahJongRally * pMe,*/ int x, int mc)
{
	int r;

	if(--mc<0)
		return 1;
	while( cntbuf[x] == 0)
		++x;
	if(cntbuf[x]>=2){
		cntbuf[x]-=2;
		r=msk( x, mc);
		cntbuf[x]+=2;
		if(r != 0){
			sMntData.byJanto=(byte)x;
			return 1;
		}
	}
	if(x<0x30 && cntbuf[x+1] != 0 && cntbuf[x+2] != 0){
		--cntbuf[x]; --cntbuf[x+1]; --cntbuf[x+2];
		r=mjsk( x, mc);
		++cntbuf[x]; ++cntbuf[x+1]; ++cntbuf[x+2];
		if(r != 0){
			SubMj.mp[SubMj.mp_p++]=(byte)x;		//	*mp++
			return 1;
		}
	}
	if(cntbuf[x]>=3){
		cntbuf[x]-=3;
		r=mjsk( x, mc);
		cntbuf[x]+=3;
		if(r != 0){
			SubMj.mp[SubMj.mp_p++]=(byte)(x|0x40);	//	*mp++
			return 1;
		}
	}
	return 0;
}

public int mskj(/*MahJongRally * pMe,*/ int x, int mc)
{
	int r;

	if(--mc<0)
		return 1;
	while( cntbuf[x] == 0)
		++x;
	if(x<0x30 && cntbuf[x+1] != 0 && cntbuf[x+2] != 0){
		--cntbuf[x]; --cntbuf[x+1]; --cntbuf[x+2];
		r=mskj( x, mc);
		++cntbuf[x]; ++cntbuf[x+1]; ++cntbuf[x+2];
		if(r != 0){
			SubMj.mp[SubMj.mp_p++]=(byte)x;	//	*mp++
			return 1;
		}
	}
	if(cntbuf[x]>=3){
		cntbuf[x]-=3;
		r=mskj( x, mc);
		cntbuf[x]+=3;
		if(r != 0){
			SubMj.mp[SubMj.mp_p++]=(byte)(x|0x40);	//	*mp++
			return 1;
		}
	}
	if(cntbuf[x]>=2){
		cntbuf[x]-=2;
		r=msk( x, mc);
		cntbuf[x]+=2;
		if(r != 0){
			sMntData.byJanto=(byte)x;
			return 1;
		}
	}
	return 0;
}

public void newlist(/*MahJongRally * pMe*/)
{
	int x=1, i=0;

	do {
		if(cntbuf[x] != 0) {
			SubMj.mjmnt_min[i]=(byte)x;
			SubMj.mjmnt_num[i]=cntbuf[x];
			while(cntbuf[++x] != 0) {
				SubMj.mjmnt_num[i]+=cntbuf[x];
			}
			++i;
		}
	} while(++x<=0x29)
		;
	for (x = 0x31; x <= 0x37; x++) {
		if(cntbuf[x] != 0) {
			SubMj.mjmnt_min[i]=(byte)x;
			SubMj.mjmnt_num[i]=cntbuf[x];
			++i;
		}
	}
	SubMj.mjmnt_num[i]=0;
	SubMj.mjmnt_min[i]=0;
}

public bool	mnta_mnt ( /*MahJongRally * pMe*/ )/*1995.4.24, 5.11*2, 5.12*/
{
	byte[]	Div3tbl={0,0,0,1,1,1,2,2,2,3,3,3,4,4,4,5};	//[16]
	byte[]	Mod3tbl={0,1,2,0,1,2,0,1,2,0,1,2,0,1,2,0};	//[16]
	int i, n;

	newlist();
	sMntData.byJanto=0;
	SubMj.mp=sMntData.byMnt;	SubMj.mp_p= 0;
	for(i=0; (n=SubMj.mjmnt_num[i]) != 0; i++) {
		switch(Mod3tbl[n]){
		  case 2:
			if(sMntData.byJanto != 0 || mkjs( SubMj.mjmnt_min[i], Div3tbl[n+1]) == 0) {
				return (false);
			}
			break;
		  case 0:
			if( mks( SubMj.mjmnt_min[i], Div3tbl[n]) == 0) {
				return (false);
			}
			break;
		  default:
			return (false);
		}
	}
	return (true);
}

public bool	mntb_mnt ( /*MahJongRally * pMe*/ )/*1995.5.11, 5.15, 5.16*/
{
	byte[]	Div3tbl={0,0,0,1,1,1,2,2,2,3,3,3,4,4,4,5};	//[16]
	byte[]	Mod3tbl={0,1,2,0,1,2,0,1,2,0,1,2,0,1,2,0};	//[16]
	int i, n;

	SubMj.mp=sMntData.byMnt;	SubMj.mp_p= 0;
	for(i=0; (n=SubMj.mjmnt_num[i]) != 0; i++)
		switch(Mod3tbl[n]){
		case 2:
			if( mjsk( SubMj.mjmnt_min[i], Div3tbl[n+1]) == 0)
				return false;	//return 0;
			break;
		case 0:
			if( msk( SubMj.mjmnt_min[i], Div3tbl[n]) == 0)
				return false;	//return 0;
			break;
		default:
			return false;	//return 0;
		}
	return true;	//return 1;
}

public bool	mntc_mnt ( /*MahJongRally * pMe*/ )/*1995.5.11, 5.15*2, 5.16*/
{
	byte[]	Div3tbl={0,0,0,1,1,1,2,2,2,3,3,3,4,4,4,5};	//[16]
	byte[]	Mod3tbl={0,1,2,0,1,2,0,1,2,0,1,2,0,1,2,0};	//[16]
	int i, n;

	SubMj.mp=sMntData.byMnt;	SubMj.mp_p= 0;
	for(i=0; (n=SubMj.mjmnt_num[i]) != 0; i++)
		switch(Mod3tbl[n]){
		case 2:
			if( mskj( SubMj.mjmnt_min[i], Div3tbl[n+1]) == 0)
				return false;	//return 0;
			break;
		case 0:
			if( msk( SubMj.mjmnt_min[i], Div3tbl[n]) == 0)
				return false;	//return 0;
			break;
		default:
			return false;	//return 0;
		}
	return true;	//return 1;
}
/**************************************END OF FILE**********************************************/

//-*********************Mjmnt.j
}