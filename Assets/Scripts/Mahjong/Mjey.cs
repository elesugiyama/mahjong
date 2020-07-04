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
// Mjey.j
//-*****************
public partial class MahjongBase : SceneBase {
/*
**		麻雀大会３
**		アルゴ － 手の高さ・役の思考ルーチン
*/

//#include "MahJongRally.h"								// Module interface definitions

public void setbase(/*MahJongRally * pMe*/)
{
	int		x, p0= 0;
	byte[]	p;		//xxxx
	byte[]	vp;		//xxxx
	byte[][]	vt= new byte[][]{			//[4][13]
		new byte[]{4,8,16,31,31,31,31,31,31,31,31,31,31},
		new byte[]{4,7,11,19,31,31,31,31,31,31,31,31,31},
		new byte[]{4,6, 8,11,16,23,31,31,31,31,31,31,31},
		new byte[]{4,5, 6, 8,10,13,16,20,25,31,31,31,31}
	};
	byte[]	shuntsu= new byte[]{		//[22]
		0x01,0x02,0x03,0x04,0x05,0x06,0x07,
		0x11,0x12,0x13,0x14,0x15,0x16,0x17,
		0x21,0x22,0x23,0x24,0x25,0x26,0x27,0
	};
//0505mt	BYTE	Paitbl[] = {	//[ MAX_PAI_KIND ]
//0505mt		0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
//0505mt		0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19,
//0505mt		0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29,
//0505mt		0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0		//xxxx
//0505mt	};

	vp= gpsTableData.psParam.chParaYaku==0 ? vt[0] : vt[-gpsTableData.psParam.chParaYaku];
	for(p=Paitbl; (x=p[p0]) != 0; p0++){		//(p=Paitbl; x=p; p++)
		SubMj.Jv[x]=vp[SubMj.jdv[x]];
		SubMj.Kv[x]=vp[SubMj.kdv[x]+Ff[x]];
	}
	for(p0= 0, p=shuntsu; (x=p[p0]) != 0; p0++)		//(p=shuntsu; x=p; p++)
		SubMj.Sv[x]=vp[SubMj.sdv[x]];
	if(gpsPlayerWork.bFmenzen != 0)
		for(x=0x31; x<=0x37; x++)
			if(Ff[x] != 0)
				SubMj.Jv[x]/=2;
}

public bool checkkokushi(/*MahJongRally * pMe*/)		//PRIVATE int checkkokushi()
{
	int i, x, n=0, j=0;
	byte[]	yaochu= new byte[]{
		0x01,0x09,0x11,0x19,0x21,0x29,0x31,
		0x32,0x33,0x34,0x35,0x36,0x37
	};

	for(i=0; i<13; i++)
		if(SubMj.Paicnt[x=yaochu[i]]==0)
			return false;		//return 0;
		else if(cntbuf[x] != 0){
			n++;
			if(cntbuf[x]>=2)
				j=1;
		}
	return n+j>=10;
}

public void sethoniso(/*MahJongRally * pMe,*/ int val)
{
	int i, j;

	val=4-val;
	for(i=0; i<=0x20; i+=0x10)
		if(i!=SubMj.gYaku.honsort)
			for(j=i+1; j<=i+9; j++){
				SubMj.Jv[j]=(byte)(SubMj.Jv[j]*val/4);
				SubMj.Kv[j]=(byte)(SubMj.Kv[j]*val/4);
				SubMj.Sv[j]=(byte)(SubMj.Sv[j]*val/4);
			}
}

public void settoitoi(/*MahJongRally * pMe,*/ int val)
{
	byte[]	p;
	int		x, p0= 0;

	byte[]	shuntsu= new byte[]{		//[22]
		0x01,0x02,0x03,0x04,0x05,0x06,0x07,
		0x11,0x12,0x13,0x14,0x15,0x16,0x17,
		0x21,0x22,0x23,0x24,0x25,0x26,0x27,0
	};

	val=4-val;
	for(p=shuntsu; (x=p[p0]) != 0; p0++)		//(p=shuntsu; x=p; p++)
		SubMj.Sv[x]=(byte)(SubMj.Sv[x]*val/4);
}

public void settanyo(/*MahJongRally * pMe,*/ int val)
{
	int		i, p0;
	byte[]	p;
	byte[] yao1=new byte[]{
		0x01,0x09,0x11,0x19,0x21,0x29,0x31,
		0x32,0x33,0x34,0x35,0x36,0x37,0
	};
	byte[] yao2= new byte[]{
		0x01,0x07,0x11,0x17,0x21,0x27,0
	};

	val= val==4 ? 0 : 3;
	for(p0= 0, p=yao1; (i=p[p0]) != 0; p0++){		//(p=yao1; i=p; p++)
		SubMj.Jv[i]=(byte)(SubMj.Jv[i]*val/4);
		SubMj.Kv[i]=(byte)(SubMj.Kv[i]*val/4);
	}
	for(p0= 0, p=yao2; (i=p[p0]) != 0; p0++)					//(p=yao2; i=p; p++)
		SubMj.Sv[i]=(byte)(SubMj.Sv[i]*val/4);
}

public int getyakup(/*MahJongRally * pMe,*/ int val, int k)
{
	return (val=(val+k-SubMj.normalval)/(k/4))<=0 ? 0 : (val<=3 ? val : 3);
}

public void evalhoniso(/*MahJongRally * pMe,*/ PLST[] pailist)
{
	int i, x;

	if(SubMj.gYaku.honsort<0){
		for(x=0, i=1; i<3; i++)
			if(SubMj.sortnum[x]<SubMj.sortnum[i])
				x=i;
		if(SubMj.sortnum[x]+SubMj.sortnum[3]+gpsTableData.psParam.chParaHonisop+3<gpsPlayerWork.byThcnt)
			return;
		SubMj.gYaku.honsort=(short)(x*16);
	}
	SubMj.gYaku.honiso.h=2;
	for(i=0; i<byDoraCnt; i++)
		if((x=Dora[i])<0x30 && (x&0x30)!=SubMj.gYaku.honsort && (SubMj.gYaku.honiso.h-=cntbuf[x])<0)
			return;
	setbase();
	sethoniso(4);
	SubMj.gYaku.honiso.p=(short)getyakup(SubMj.honisoval=EvalTehai(pailist), 256);
}

public void evaltoitoi(/*MahJongRally * pMe,*/ PLST[] pailist)
{
	int i;

	SubMj.gYaku.toitoi.h=2;
	for(i=0; i<byDoraCnt; i++)
		if(cntbuf[Dora[i]]==1 && --SubMj.gYaku.toitoi.h<0)
			return;
	setbase();
	settoitoi(4);
	SubMj.gYaku.toitoi.p=(short)getyakup(SubMj.toitoival=EvalTehai(pailist), 256);
}

public void evaltanyo(/*MahJongRally * pMe,*/ PLST[] pailist)
{
	int i, x;

	SubMj.gYaku.tanyo.h=1;
	for(i=0; i<byDoraCnt; i++)
		if(((x=Dora[i])>=0x30 || (x&7)==1) && (SubMj.gYaku.tanyo.h-=cntbuf[x])<0)
			return;
	setbase();
	settanyo(4);
	SubMj.gYaku.tanyo.p=(short)getyakup(SubMj.tanyoval=EvalTehai(pailist), 128);
}

/************************************************/
/*		正しく動作している		*/
/************************************************/
public void setmval_ey ( /*MahJongRally * pMe*/ )/*1995.4.26, 6.14, 7.24*/
{
	int x;

	setbase();
	if(SubMj.gYaku.honiso.p != 0)
		sethoniso(SubMj.gYaku.honiso.p);
	if(SubMj.gYaku.toitoi.p != 0)
		settoitoi(SubMj.gYaku.toitoi.p);
	if(SubMj.gYaku.tanyo.p>=3)
		settanyo(SubMj.gYaku.tanyo.p);
	if(SubMj.gYaku.fanpai.p != 0)
		for(x=0x31; x<=0x37; x++)
			if(Ff[x] != 0)
				SubMj.Jv[x]=0;
}

/************************************************/
/*		ＣＯＭ用役チェック						*/
/*												*/
/*	ＣＯＭがツモるときに一回ずつ呼ばれる		*/
/*	( think_think ｶﾗ )							*/
/*	ＣＯＭがナける牌が捨てられたときにも		*/
/*	一回ずつ呼ばれる( tfuro_think ｶﾗ )			*/
/*	ＣＯＭがナいた直後にも一回ずつ呼ばれる		*/
/*	( この時テンパイだと呼ばれない )			*/
/************************************************/
public void chkyaku_ey ( /*MahJongRally * pMe,*/ PLST[] pailist )/*1995.6.12*/
{
	int i, x;
	int kc, kc1, fc, fc1, fc2, sc, sc1, sc2, tc;
	int yaochu;
	int hof, tyf, ttf;

	SubMj.gYaku.clear();//= new YAKU();		//MEMSET(&SubMj.gYaku, 0, sizeof SubMj.gYaku);
	SubMj.toitoival=SubMj.tanyoval=SubMj.honisoval=0;
	kc=kc1=fc=fc1=fc2=sc=sc1=sc2=tc=0;
	hof=tyf=ttf=0;
	SubMj.gYaku.honsort=-1;
	if(gpsPlayerWork.byFhcnt == 0 && checkkokushi()){
		SubMj.gYaku.kokushi=1;
		return;
	}
	if(SubMj.Tleft<2)
		return;
	for(i=0; i<gpsPlayerWork.byFhcnt; i++){
		if((x=gpsPlayerWork.byFrhai[i])>0x40){
			x&=0x3F;
			kc++;
			if(x>=0x31){
				tyf=1;
				fc+=Ff[x];
				if(x>=0x35)
					sc++;
			} else if((x&7)==1)
				tyf=1;
		} else {
			ttf=1;
			if((x&0x0F)==1 || (x&0x0F)==7)
				tyf=1;
		}
		if((x&0x3F)<0x30)
			if(SubMj.gYaku.honsort<0)
				SubMj.gYaku.honsort=(short)(x&0x30);
			else if(SubMj.gYaku.honsort!=(x&0x30))
				hof=1;
	}
	_MEMSET(SubMj.sortnum, 0, SubMj.sortnum.Length);
	yaochu=0;
	for(x=1; x<=0x37; x++)
		if(cntbuf[x] != 0){
			SubMj.sortnum[x/16]+=cntbuf[x];
			if(x>=0x31 || (x&7)==1)
				yaochu+=cntbuf[x];
			if(cntbuf[x]>=3){
				kc++;
				if(x>=0x31){
					tyf=1;
					fc+=Ff[x];
					if(x>=0x35)
						sc++;
				} else if((x&7)==1)
					tyf=1;
			} else if(cntbuf[x]==2){
				tc++;
				if(SubMj.pc[x]>=1){
					kc1++;
					fc1+=Ff[x];
					if(x>=0x35)
						sc1++;
				}
			} else if(cntbuf[x]==1 && SubMj.pc[x]>=2){
				fc2+=Ff[x];
				if(x>=0x35)
					sc2++;
			}
		}
	setbase();
	SubMj.normalval=EvalTehai(pailist);
	if( hof == 0 && gpsTableData.psParam.chParaHonisop != 0)
		evalhoniso(pailist);
	if( ttf == 0 && gpsTableData.psParam.chParaToitoip != 0 && kc>=1 && kc+kc1>=4)
		evaltoitoi(pailist);
	if( tyf == 0 && gpsTableData.psParam.chParaTanyop != 0 && yaochu-gpsTableData.psParam.chParaTanyop<4)
		evaltanyo(pailist);
	if(sc+sc1+sc2==3)
		SubMj.gYaku.shosan=(short)(6-sc1-sc2*2);
	if(gpsPlayerWork.byFhcnt == 0 && tc>=3 && SubMj.normalval<400)
		SubMj.gYaku.chitoi=(short)((tc-2)*16);
	if(fc+fc1>((Ryansh) ? 1 : 0))
		SubMj.gYaku.fanpai.p=1;
	SubMj.gYaku.fanpai.h=(short)(SubMj.Dorasum+fc+fc1);
	if(SubMj.gYaku.honiso.h+SubMj.gYaku.fanpai.h>=gpsTableData.psParam.chParaHonisoh && SubMj.gYaku.honiso.p+gpsTableData.psParam.chParaHonisop>=5)
		SubMj.gYaku.honiso.p=4;
	if(SubMj.gYaku.toitoi.h+SubMj.gYaku.fanpai.h>=gpsTableData.psParam.chParaToitoih && SubMj.gYaku.toitoi.p+gpsTableData.psParam.chParaToitoip>=5)
		SubMj.gYaku.toitoi.p=4;
	if( SubMj.gYaku.fanpai.p == 0 && Rultbl[ (int)RL.KUITA ] != 0 && !Ryansh &&
			SubMj.gYaku.tanyo.h-SubMj.gYaku.fanpai.h>=gpsTableData.psParam.chParaTanyoh && SubMj.gYaku.tanyo.p+gpsTableData.psParam.chParaTanyop>=5)
		SubMj.gYaku.tanyo.p=4;
	if( SubMj.gYaku.fanpai.p == 0 && gpsPlayerWork.bFmenzen != 0){
		for(i=3; i>=1; i--)
			if(SubMj.gYaku.honiso.p>=i){
				SubMj.gYaku.honiso.p=4;
				break;
			} else if(SubMj.gYaku.toitoi.p>=i){
				SubMj.gYaku.toitoi.p=4;
				break;
			} else if(!Ryansh && Rultbl[ (int)RL.KUITA ] != 0 && SubMj.gYaku.tanyo.p>=i){
				SubMj.gYaku.tanyo.p=4;
				break;
			}
		if(i==0 && fc+fc1+fc2>((Ryansh) ? 1 : 0))
			SubMj.gYaku.fanpai.p=4;
	}
}

//-*********************Mjey.j
}