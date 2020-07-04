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
// Mjeval.j
//-*****************
public partial class MahjongBase : SceneBase {
/*
**		麻雀大会３
**		アルゴ － 手の内の評価・打牌思考ルーチン
*/

//#include "MahJongRally.h"								// Module interface definitions


public int evalfuro(/*MahJongRally * pMe*/)
{
	int val=0, i, x;

	for(i=0; i<gpsPlayerWork.byFhcnt; i++)
		if((x=gpsPlayerWork.byFrhai[i])>0x40)
			val+=SubMj.Kv[x&0x3F]*MJDefine.MENTSU;
		else
			val+=SubMj.Sv[x]*MJDefine.MENTSU;
	return val;
}

public int evltat(/*MahJongRally * pMe,*/ int eps, int same, int k, int val)
{
	int paix=eps;
	int pcsum=0;
	int valsum=0;
	int k2;
	int v, vs;

	SubMj.waste-=2; SubMj.g_mc++; SubMj.level--;
	same++;
	k=k*(SubMj.depth-SubMj.level);
	vs=0;
	do {
		if(SubMj.pc[paix] != 0){
			cntbuf[paix]++;
			if(CheckMnt(SubMj.pmin1, SubMj.waste, SubMj.g_mc) != 0 && (v=EvalMnt(SubMj.pmin1, SubMj.waste, SubMj.g_mc))>val){
				SubMj.tatflg[paix]=1;
				k2=SubMj.ppx[paix]*SubMj.pc[paix]*k/same/8;
				if(SubMj.spc[paix] != 0 && gpsTableData.psParam.chParaFuriten != 0)
					k2/=4;
				vs+=(v-val)*k2;
				pcsum+=k2;
				if(SubMj.level != 0){
					SubMj.pc[paix]--;
					valsum+=evltat(paix, same, k2, v);
					SubMj.pc[paix]++;
				}
			}
			cntbuf[paix]--;
		}
		same=1;
	} while(++paix<=SubMj.pmax1);
	SubMj.waste+=2;SubMj. g_mc--; SubMj.level++;
	return pcsum != 0 ? valsum*8/pcsum+vs/SubMj.level : 0;
}

public int calcbonus(/*MahJongRally * pMe,*/ int min, int max)
{
	int x, val=0;

	for(x=min; x<=max; x++)
		if(cntbuf[x]==2){
			val+=SubMj.gYaku.chitoi;
			if(SubMj.pc[x] != 0 && Ff[x] != 0 && (SubMj.gYaku.fanpai.p != 0 || SubMj.gYaku.honiso.p != 0 || SubMj.gYaku.toitoi.p != 0))
				val+=128;
		}
	return val;
}

public int evlpai(/*MahJongRally * pMe,*/ int val)
{
	int paix, paiy;
	int pcsum;
	int valsum=0;
	int v;

	SubMj.waste--; SubMj.g_mc++;
	paix=SubMj.pmin2;
	do
		if( SubMj.tatflg[paix] == 0 && SubMj.pc[paix] != 0){
			cntbuf[paix]++;
			SubMj.pc[paix]--;
			pcsum=0;
			paiy=paix;
			do
				if( SubMj.tatflg[paiy] == 0 && SubMj.pc[paiy] != 0){
					cntbuf[paiy]++;
					if(CheckMnt(SubMj.pmin2, SubMj.waste, SubMj.g_mc) != 0 && (v=EvalMnt(SubMj.pmin2, SubMj.waste, SubMj.g_mc))>val){
						v=(v-val)*SubMj.ppx[paiy]*SubMj.pc[paiy];
						if(paix!=paiy)
							v*=2;
						if(SubMj.spc[paiy] != 0 && gpsTableData.psParam.chParaFuriten != 0)
							v/=4;
						pcsum+=v;
					}
					cntbuf[paiy]--;
				}
			while(++paiy<=SubMj.pmax2);
			SubMj.pc[paix]++;
			cntbuf[paix]--;
			valsum+=pcsum*SubMj.ppx[paix]*SubMj.pc[paix];
		}
	while(++paix<=SubMj.pmax2);
	SubMj.waste++; SubMj.g_mc--;
	return (int)((long)valsum*SubMj.alone/16384);
}

public int evlsub(/*MahJongRally * pMe,*/ int min, int max, int val)
{
	int x, lvlmax, m_vmax=0;

	if( SubMj.waste == 0)
		return 0;
	while( cntbuf[min] == 0)
		min++;
	while( cntbuf[max] == 0)
		max--;
	SubMj.pmin1=SubMj.pmin2=min;
	SubMj.pmax1=SubMj.pmax2=max;
	if(min<0x30){
		if((min&=0x0F)>=2){
			--SubMj.pmin1;
			--SubMj.pmin2;
			if(min>=3)
				--SubMj.pmin2;
		}
		if((max&=0x0F)<=8){
			++SubMj.pmax1;
			++SubMj.pmax2;
			if(max<=7)
				++SubMj.pmax2;
		}
	}
	for(x=SubMj.pmin2; x<=SubMj.pmax2; x++)
		SubMj.tatflg[x]=0;
	if((lvlmax=SubMj.waste/2) != 0)
		for(SubMj.depth=1; SubMj.depth<=lvlmax; SubMj.depth++){
			SubMj.level=SubMj.depth;
			if((x=evltat(SubMj.pmin1, 0, 8, val))>m_vmax)
				m_vmax=x;
			else
				break;
		}
	return m_vmax+evlpai(val);
}

public void evlmj(/*MahJongRally * pMe,*/ int min, int max, int num)
{
	int x, v;

	SubMj.mval=SubMj.jval=0;
	if(num==0)
		return;
	while( cntbuf[min] == 0)
		min++;
	while( cntbuf[max] == 0)
		max--;
	SubMj.waste=num-3*(SubMj.g_mc=CountMnt(min, num));
	v=EvalMnt(min, SubMj.waste, SubMj.g_mc);
	SubMj.mval=v*MJDefine.MENTSU+evlsub(min, max, v);
	if((num-=2)<0){
		if(min<0x30){
			if((min&0x0F)==1 && cntbuf[min+3] != 0 || (min&0x0F)==9 && cntbuf[min-3] != 0)
				SubMj.mval-=12;
			else if((min&0x0F)==2 && cntbuf[min+3] != 0 || (min&0x0F)==8 && cntbuf[min-3] != 0)
				SubMj.mval-=3;
		} else if(min>0x30 && Ff[min] != 0 && SubMj.pc[min]>=2){
			if(SubMj.gYaku.fanpai.p==4)
				SubMj.mval+=64;
			else if(SubMj.gYaku.shosan != 0)
				SubMj.mval+=SubMj.gYaku.shosan*16;
			else if(gpsPlayerWork.bFmenzen == 0 && SubMj.gYaku.fanpai.p == 0 && SubMj.gYaku.fanpai.h+1>=gpsTableData.psParam.chParaFanpai2)
				SubMj.mval+=12;
		}
		return;
	}
	x=min;
	do
		if(cntbuf[x]>=2){
			cntbuf[x]-=2;
			SubMj.waste=num-3*(SubMj.g_mc=CountMnt(min, num));
			v=EvalMnt(min, SubMj.waste, SubMj.g_mc);
			if((v=v*MJDefine.MENTSU+SubMj.Jv[x]*SubMj.janto+evlsub(min, max, v))>SubMj.jval)
				SubMj.jval=v;
			cntbuf[x]+=2;
		}
	while(++x<=max);
	v=calcbonus(min, max);
	if((SubMj.jval-=SubMj.mval)<0)
		SubMj.jval=0;
	else
		SubMj.jval+=v;
	SubMj.mval+=v;
}

public void calcblock(/*MahJongRally * pMe,*/ PLST[] pailist)
{
	PLST[] pp= pailist;		//xxxx
	int[] bp= SubMj.blockval;		//xxxx
	int sum, p0= 0;

	sum=evalfuro();
	SubMj.maxblock=0; SubMj.maxjval=SubMj.nextjval=0;
	SubMj.jantoflag= gpsTableData.psParam.chParaJanto != 0 ? 0 : 1;
	for(; pp[p0].min != 0; p0++){		//(pp=pailist, bp=blockval; pp.min; bp++, pp++)
		evlmj(pp[p0].min, pp[p0].max, pp[p0].num);
		sum+=bp[p0]=SubMj.mval;
		if(SubMj.jval>SubMj.maxjval) {
			SubMj.maxblock=bp[p0]; SubMj.nextjval=SubMj.maxjval; SubMj.maxjval=SubMj.jval; SubMj.jantoflag=1;	}
		else if(SubMj.jval>SubMj.nextjval)
			SubMj.nextjval=SubMj.jval;
	}
	for(p0=0, pp=pailist, bp=SubMj.blockval; pp[p0].min != 0; p0++)		//(pp=pailist, bp=blockval; pp.min; bp++, pp++)
		bp[p0]=sum-bp[p0];
}

public void calcsthai(/*MahJongRally * pMe,*/ PLST[] pailist)
{
	int		paix, v, v0= 0, p0= 0;
	int[]		bp=SubMj.blockval;		//xxxx
	PAIVAL[]	vp=SubMj.gPaival;		//xxxx

	for(; (paix=vp[v0].paicode) != 0; v0++){		//(; paix=vp.paicode; vp++)
		if(paix>pailist[p0].max) {
			p0++; }				//pailist++; bp++;
		SubMj.spc[paix]++;
		cntbuf[paix]--;
		evlmj(pailist[p0].min, pailist[p0].max, pailist[p0].num-1);
		SubMj.mval+=bp[p0];
		v= (bp[p0]==SubMj.maxblock) ? SubMj.nextjval : SubMj.maxjval;
		vp[v0].val=SubMj.mval+(SubMj.jval>v ? SubMj.jval : v);
		if(paix>0x30 || (paix&7)==1)
			vp[v0].val++;
		SubMj.spc[paix]--;
		cntbuf[paix]++;
	}
}

public void calckokushi(/*MahJongRally * pMe*/)
{
	int x, j=0, p0= 0;
	PAIVAL[] p= SubMj.gPaival;		//xxxx

	for(; (x=p[p0].paicode) != 0; p0++)		//(p=gPaival; x=p.paicode; p++)
		if(x<0x30 && (x&7)!=1 || cntbuf[x]>=2 && (j++) != 0)
			p[p0].val=256;
}

public void getjanto(/*MahJongRally * pMe,*/ PLST[] pailist)
{
	int v, val, p0= 0;
	int[] alonetbl= new int[]{	//[18]
		  0, 12, 21, 26, 30, 33, 35, 36, 37,
		 38, 39, 40, 41, 42, 43, 44, 45, 45
	};
//0505mt 	BYTE	Paitbl[] = {		//[ MAX_PAI_KIND ]
//0505mt 		0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
//0505mt 		0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19,
//0505mt 		0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29,
//0505mt 		0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0
//0505mt 	};
	byte[]	p= Paitbl;		//xxxx

	SubMj.alone=alonetbl[SubMj.Tleft]*(gpsTableData.psParam.chParaAlone+4);
	if( gpsTableData.psParam.chParaJanto == 0){
		SubMj.janto=0;
		return;
	}
	for(; (v=p[p0]) != 0; p0++)			//(p=Paitbl; v=p; p++)
		SubMj.Kv[v]=SubMj.Sv[v]=4;
	val=0;
	for( p0= 0; pailist[p0].min != 0; p0++){		//(; pailist.min; pailist++)
		SubMj.waste=pailist[p0].num-3*(SubMj.g_mc=CountMnt(pailist[p0].min, pailist[p0].num));
		v=EvalMnt(pailist[p0].min, SubMj.waste, SubMj.g_mc);
		val+=v*MJDefine.MENTSU+evlsub(pailist[p0].min, pailist[p0].max, v);
	}
	v=(val+gpsPlayerWork.byFhcnt*4*MJDefine.MENTSU)/8;
	SubMj.janto=8+(int)((long)v*v/512);
}

public void	tval_eval ( /*MahJongRally * pMe,*/ PLST[] pailist )/*1995.6.16, 7.17, 8.8*/
{
	if(SubMj.gYaku.kokushi != 0){
		calckokushi();
		return;
	}
	getjanto(pailist);
	setmval_ey();
	calcblock(pailist);
	calcsthai(pailist);
}

public int	evlval_eval ( /*MahJongRally * pMe,*/ PLST[] pailist )/*1995.4.27, 6.12, 7.17, 8.8*/
{
	getjanto(pailist);
	setmval_ey();
	return evalfuro()+EvalTehai(pailist);

}
public int	EvalTehai(/*MahJongRally * pMe,*/ PLST[] pailist)
{
	int		sum=0, jmax=0, p0= 0;

	for(; pailist[p0].min != 0; p0++){		//(; pailist.min; pailist++)
		evlmj(pailist[p0].min, pailist[p0].max, pailist[p0].num);
		sum+=SubMj.mval;
		if(SubMj.jval>jmax)
			jmax=SubMj.jval;
	}
	return sum+jmax;
}

/**************************************END OF FILE**********************************************/

//-*********************Mjeval.j
}