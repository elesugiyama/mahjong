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
// mjetnp.j
//-*****************
public partial class MahjongBase : SceneBase {
/*
**		麻雀大会３
**		アルゴ － 聴牌思考ルーチン
*/

//#include "MahJongRally.h"								// Module interface definitions

//_STATIC	BYTE*	sp=apfsav;

	public void discard(/*MahJongRally * pMe,*/ int x)
	{
		SubMj.spc[x]++;
		SubMj.Dorasum-=SubMj.Doracnt[x];
		if(x<0x30){
			if((x&0x0F)>=4){
				SubMj.g_apfsav[SubMj.p_apfsav++]=SubMj.App[x-3];		//p_apfsav++=App[x-3];
				SubMj.App[x-3]&=0xBF;
			}
			if((x&0x0F)<=6){
				SubMj.g_apfsav[SubMj.p_apfsav++]=SubMj.App[x+3];		//p_apfsav++=App[x+3];
				SubMj.App[x+3]&=0xEF;
			}
		}
	}

	public void restore(/*MahJongRally * pMe,*/ int x)
	{
		SubMj.spc[x]--;
		SubMj.Dorasum+=SubMj.Doracnt[x];
		if(x<0x30){
			if((x&0x0F)<=6) {
				SubMj.App[x+3]=SubMj.g_apfsav[--SubMj.p_apfsav];		//=--p_apfsav;
			}
			if((x&0x0F)>=4) {
				SubMj.App[x-3]=SubMj.g_apfsav[--SubMj.p_apfsav];		//=--p_apfsav;
			}
		}
	}

	public PLST[] ttnpsub_pbuf = new PLST [15];//0505mt
	public void ttnpsub(/*MahJongRally * pMe,*/ int x)
	{
	//0505mt	PLST pbuf[]= new PLST [15];
		for( int i= 0; i< 15; i++)	ttnpsub_pbuf[i].clear();//0505mt pbuf[i]= new PLST();

		cntbuf[x]--;
		discard(x);
		newlst_plst(ttnpsub_pbuf);//0505mt
		evltnp_etnp(ttnpsub_pbuf);//0505mt
		cntbuf[x]++;
		restore(x);
	}

	public void ttnp_etnp ( /*MahJongRally * pMe,*/ PLST[] pailist )/*1995.6.15, 7.19, 8.1, 8.15*/
	{
		int x, j= 0;
		PAIVAL[] q;	//xxxx

		SubMj.Tnpmax	=	0;
		if(rtnp_rtnp(pailist)) {
			for(q=SubMj.gPaival; (x=q[j].paicode) != 0; j++){		//xxxx	(q=SubMj.gPaival; x=q.paicode; q++)
				ttnpsub(x);
				q[j].tnpnum=SubMj.g_tnpnum;
				if((q[j].tnpval=SubMj.Tnpval)>SubMj.Tnpmax) {
					SubMj.Tnpmax=SubMj.Tnpval;
				}
				q[j].ricnum=SubMj.g_ricnum;
				if((q[j].ricval=SubMj.Ricval)>SubMj.Tnpmax) {
					SubMj.Tnpmax=SubMj.Ricval;
				}
			}
		}
	}

	public bool chkishan(/*MahJongRally * pMe,*/ PLST[] pailist)	//int chkishan
	{
		int x, mi=0, jf=0, j= 0;

		SubMj.g_msum=SubMj.g_tsum=0;
		for(; (x=pailist[j].min) != 0; j++, mi++){	//xxx	for(; x=pailist.min; pailist++, mi++)
			SubMj.g_msum+=(int)(SubMj.g_mctbl[mi]=(char)CountMnt(x, pailist[j].num));
			SubMj.g_wctbl[mi]=(char)(pailist[j].num-SubMj.g_mctbl[mi]*3);
			if(jf== 0 && SubMj.g_wctbl[mi]>=2 && CheckJnt(x, SubMj.g_wctbl[mi]-2, SubMj.g_mctbl[mi]+1) != 0)	//(!jf && SubMj.g_wctbl[mi]>=2 && CheckJnt(x, SubMj.g_wctbl[mi]-2, SubMj.g_mctbl[mi]+1))
				jf=1;
			do
				if(cntbuf[x]>=2)
					SubMj.g_tsum++;
			while(++x<=pailist[j].max);
		}
		if(gpsPlayerWork.byFhcnt != 0){
			SubMj.g_msum+=gpsPlayerWork.byFhcnt;
			SubMj.g_tsum=0;
		}
		return SubMj.g_tsum>=5 || SubMj.g_msum>=3 || SubMj.g_msum==2 && jf != 0;
	}

	public void initishan(/*MahJongRally * pMe,*/ PLST[] pailist)
	{
		int		x, min, max, p0= 0, p1= 0;
		byte[]	p = SubMj.g_paibuf;

		for(x=1; (min=pailist[p0].min) != 0; p0++) {		//xxxx	for(x=1; min=pailist.min; pailist++)
			if(min>0x30){
				if(SubMj.pc[min] != 0) {
					p[p1++]=(byte)min;	//p++=min;
				}
			}
			else {
				max=pailist[p0].max;
				if((min&0x0F)>=2 && (--min&0x0F)>=2) {
					min--;
				}
				if((max&0x0F)<=8 && (++max&0x0F)<=8) {
					max++;
				}
				if(x<min) {
					x=min;
				}
				do {
					if(SubMj.pc[x] != 0)
						p[p1++]=(byte)x;		//p++=x;
				} while(++x<=max);
			}
		}
		p[p1]=0;		//p=0;
	}

	public void tish_etnp ( /*MahJongRally * pMe,*/ PLST[] pailist )/*1995.6.15, 7.19, 7.31*/
	{
		int		x, m, t, mi=0, p0=0, q0=0;
		byte	pcstr;
		PAIVAL[] q=SubMj.gPaival;	//xxxx

		if(!chkishan(pailist))
			return;
		initishan(pailist);

		for(; (x=q[q0].paicode) != 0; q0++){		//(; x=q.paicode; q++)
			if(pailist[p0].max<x) {
				p0++;		//pailist++;
				mi++;
			}
			cntbuf[x]--;
			t=SubMj.g_tsum;	m=SubMj.g_msum;
			if(cntbuf[x]==1) {
				t--;
			}
			if(SubMj.g_wctbl[mi]<=0 || CheckMnt(pailist[p0].min, SubMj.g_wctbl[mi]-1, SubMj.g_mctbl[mi]) == 0) {
				m--;
			}
			if(m==SubMj.g_msum || t>=5 || m>=2 && CheckJnt(pailist[p0].min, SubMj.g_wctbl[mi], SubMj.g_mctbl[mi]) != 0){
				pcstr=SubMj.pc[x];
				SubMj.pc[x]=0;
				discard(x);
				evlish_etnp((t>=6 || m>=4) );
				q[q0].ishval=SubMj.Ishval;
				q[q0].ishnum=SubMj.Ishnum;
				SubMj.pc[x]=pcstr;
				restore(x);
			}
			cntbuf[x]++;
		}
	}

	public int maxtnp(/*MahJongRally * pMe,*/ PLST[] pailist, int hx)
	{
		int x, val=0, p0=0;

		SubMj.g_maxnum=0;
		for(; (x=pailist[p0].min) != 0; p0++)		//for(; x=pailist.min; pailist++)
			do {
				while(cntbuf[x]==0)
					x++;
				if(x!=hx){
					ttnpsub(x);
					if((SubMj.Tnpval-=SubMj.Tnpmax)>val) {
						val=SubMj.Tnpval; SubMj.g_maxnum=SubMj.g_tnpnum;	}
					if((SubMj.Ricval-=SubMj.Tnpmax)>val) {
						val=SubMj.Ricval; SubMj.g_maxnum=SubMj.g_ricnum;	}
				}
			} while(++x<=pailist[p0].max);
		return val;
	}
	public PLST[] evlish_etnp_pbuf= new PLST [15];//0505mt
	public void evlish_etnp (/*MahJongRally * pMe,*/ bool all)/*1995.6.15, 7.19*/
	{
	//0505mt	PLST pbuf[]= new PLST [15];
	//0505mt	BYTE	Paitbl[] = {	//[ MAX_PAI_KIND ]
	//0505mt		0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
	//0505mt		0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19,
	//0505mt		0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29,
	//0505mt		0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0
	//0505mt	};
		int x, j= 0;
		byte[]	p = all ? Paitbl : SubMj.g_paibuf;
		for( int i= 0; i< 15; i++)	evlish_etnp_pbuf[i].clear();//0505mt pbuf[i]= new PLST();

		SubMj.Ishval=SubMj.Ishnum=0;
		while((x=p[j++]) != 0)		//while(x=*p++)
			if(SubMj.pc[x] != 0){
				cntbuf[x]++;
				SubMj.pc[x]--;
				SubMj.Dorasum+=SubMj.Doracnt[x];
				newlst_plst(evlish_etnp_pbuf);//0505mt
				if(rtnp_rtnp(evlish_etnp_pbuf)){//0505mt
					SubMj.Ishval+=(byte)((int)((long)(maxtnp(evlish_etnp_pbuf, x)*SubMj.ppx[x]*(SubMj.pc[x]+1)/64)));//0505mt
					SubMj.Ishnum+=(byte)(SubMj.g_maxnum*SubMj.ppx[x]*(SubMj.pc[x]+1)/64);
				}
				cntbuf[x]--;
				SubMj.pc[x]++;
				SubMj.Dorasum-=SubMj.Doracnt[x];
			}
	}
	public byte[]	cntsave= new byte [56];//0505mt	//-*todo:元char型
//-*SUGI_DEB***************************
#if SUGI_DEB //-*todo:注デバッグ中
	//-*デバッグ
	/// <summary>
	/// bool isResut:リザルト用判定
	/// </summary>
	public int		cntdora_jy ( /*MahJongRally * pMe*/ bool isResut = false)/*1995.4.25, 5.19	正常動作*/
#else
	//-*元
	public int		cntdora_jy ( /*MahJongRally * pMe*/ )/*1995.4.25, 5.19	正常動作*/
#endif //-*todo:注デバッグ中
//-****************************SUGI_DEB
	{
		int		i, j = 0;
		byte[]	p;
	//0505mt	char	cntsave[]= new char [56];

		_MEMCPY(cntsave, cntbuf, cntsave.Length);		//MEMCPY(cntsave, cntbuf, sizeof cntsave);
		for(i=gpsPlayerWork.byFhcnt, p=gpsPlayerWork.byFrhai; i>0; i--, j++) {	//p++
			if(p[j]>0x80) {
				cntbuf[p[j]&0x3F]+=4;
			}
			else if(p[j]>0x40) {
				cntbuf[p[j]&0x3F]+=3;
			}
			else {
				cntbuf[p[j]]++; cntbuf[p[j]+1]++; cntbuf[p[j]+2]++;		//cntbuf[*p]++, cntbuf[*p+1]++, cntbuf[*p+2]++;
			}
		}
		i=0;
		for( j= 0; j< byDoraCnt; j++) {			//(p=Dora; p<Dora+byDoraCnt; p++)
			i+=cntbuf[Dora[j]];					//i+=cntbuf[p];
		}
		if(gpsPlayerWork.bFrich != 0) {
			for( j= 0; j< byUraDoraCnt; j++) {		//(p=Ura; p<Ura+byUraDoraCnt; p++)
				i+=cntbuf[Ura[j]];				//i+=cntbuf[p];
			}
		}

//-*SUGI_DEB***************************
#if SUGI_DEB //-*todo:注デバッグ中
		//-*追加ドラ
		if(isResut){
			// byte[] addDraTest = new byte[5];
			// addDraTest[0] = 0x21;
			// addDraTest[1] = 0x22;
			// addDraTest[2] = 0x23;
			// addDraTest[3] = 0x24;
			// addDraTest[4] = 0;
			// for( j= 0; j< addDraTest.Length; j++) {
			// 	i+=cntbuf[addDraTest[j]];
			// }
			if(m_DebBox != null){
				var debAddDora = m_DebBox.GetDebAddDora();
				foreach(byte no in debAddDora){
					i+=cntbuf[no];
				}
			}
		}
#endif //-*todo:注デバッグ中
//-****************************SUGI_DEB
		_MEMCPY(cntbuf, cntsave, cntsave.Length);		//MEMCPY(cntbuf, cntsave, sizeof( cntbuf));
		return i;
	}

	public void initey_etnp ( /*MahJongRally * pMe*/ )/*1995.4.26, 6.12, 6.28, 7.24	正常動作*/
	{
		int i, x, j = 0;
		int[] p;
		int[][] pntlst = new int[][]{	//[4][13]
			new int[]{10,20,39,77,80,120,120,160,160,160,240,240,320},
			new int[]{14,24,39,65,67, 91, 91,112,112,112,152,152,189},
			new int[]{20,28,39,55,56, 68, 68, 79, 79, 79, 97, 97,112},
			new int[]{25,31,39,49,49, 57, 57, 62, 62, 62, 71, 71, 78}
		};

		SubMj.Hannec=0;
		SubMj.p_apfsav= 0;		//p_apfsav=g_apfsav;
		p=pntlst[Mathf.Abs(-gpsTableData.psParam.chParaYaku) & 3];		//-gpsTableData.psParam.chParaYaku
		for(i=1; i<14; i++)
			SubMj.g_pnttbl[i]=p[j++];
		if(Order==gpsTableData.byOya)
			for(i=1; i<14; i++)
				SubMj.g_pnttbl[i]+=SubMj.g_pnttbl[i]/2;
		x= gpsTableData.byRibo* 10+ gpsTableData.byRenchan* 3;
		for(i=1; i<14; i++)
			SubMj.g_pnttbl[i]+=x;
		for(i=13; i<20; i++)
			SubMj.g_rictbl[i]=SubMj.g_pnttbl[i]=SubMj.g_pnttbl[13];
		if(byUraDoraCnt == 0)
			x=1;
		else if((x=gpsTableData.psParam.chParaUra+byUraDoraCnt+1)>8)
			x=8;
			if(0>x)	x=0;
			if(x>8)	x=8;	//xxxx
		for(i=1; i<13; i++, x++)
			SubMj.g_rictbl[i]=SubMj.g_pnttbl[i]+(SubMj.g_pnttbl[x]-SubMj.g_pnttbl[i])*3/8;
		if(SubMj.pntnec[Order] != 0){
			for(i=1; SubMj.g_rictbl[i]<SubMj.pntnec[Order]; i++)
				SubMj.g_rictbl[i]/=4;
			for(i=1; SubMj.g_pnttbl[i]<SubMj.pntnec[Order]; i++)
				SubMj.g_pnttbl[i]/=4;
			SubMj.Hannec=(byte)i;
		}
		if(SubMj.pntsuf[Order] != 0)
			for(i=1; i<20; i++){
				if(SubMj.g_pnttbl[i]>=SubMj.pntsuf[Order])
					SubMj.g_pnttbl[i]=(SubMj.g_pnttbl[i]+SubMj.pntsuf[Order])/2;
				if(SubMj.g_rictbl[i]>=SubMj.pntsuf[Order])
					SubMj.g_rictbl[i]=(SubMj.g_rictbl[i]+SubMj.pntsuf[Order])/2;
			}
		SubMj.Dorasum=(byte)cntdora_jy();
	}

	public void addtnp(/*MahJongRally * pMe,*/ int x, int ftflg)
	{
		int ppt= SubMj.pc[x]* SubMj.ppx[x];

		if(ftflg == 0)
			getppr_pcnt(x);
		if(SubMj.Hant != 0){
			SubMj.g_tnpnum+=ppt;
			SubMj.Tnpval+=SubMj.g_pnttbl[SubMj.Hant % 20]*ppt/4;		//0323a
			if(ftflg == 0 && SubMj.Hanr != 0){
				SubMj.g_tnpnum+=SubMj.Ppr;
				SubMj.Tnpval+=SubMj.g_pnttbl[SubMj.Hanr % 20]*SubMj.Ppr/4;	//0323a
			}
		}
		if(SubMj.Ricflg != 0){
			SubMj.g_ricnum+=ppt;
			SubMj.Ricval+=SubMj.g_rictbl[SubMj.Rict % 20]*ppt/4;		//0323a
			if(ftflg == 0 && SubMj.Ricr != 0){
				SubMj.g_ricnum+=SubMj.Pprr;
				SubMj.Ricval+=SubMj.g_rictbl[SubMj.Ricr % 20]*SubMj.Pprr/4;	//0323a
			}
		}
	}

	public void evltnp_etnp ( /*MahJongRally * pMe,*/ PLST[] pailist )/*1995.6.15, 7.17, 7.28*/
	{
		int		i, n, x, f;
		byte[]	mcbuf = new byte [10];

		SubMj.Tnpval=SubMj.Ricval=SubMj.g_tnpnum=SubMj.g_ricnum=0;
		if((n=jtnp_jtnp(pailist, mcbuf)) != 0){
			f=0;
			for(i=0; i<n; i++)
				f|=SubMj.spc[mcbuf[i]];
			for(i=0; i<n; i++){
				if(SubMj.pc[x=mcbuf[i]] != 0){
					evaly_jy((byte)x);
					addtnp(x, f);
				}
			}
		} else if(gpsPlayerWork.byFhcnt == 0 && jtnp7_jtnp(pailist, mcbuf) != 0){
			if(SubMj.pc[x=mcbuf[0]] != 0){
				evaly7_jy((byte)x);
				addtnp(x, SubMj.spc[x]);
			}
		} else
			return;
		SubMj.Tnpval/=2;
		if(SubMj.keiten != 0){
			SubMj.Tnpval+=SubMj.keiten;
			if(SubMj.g_tnpnum<48)
				SubMj.g_tnpnum=48;
		}
		if((SubMj.Ricval/=2)>0 && (SubMj.Ricval-=SubMj.ricrisk/SubMj.g_ricnum*16)<0) {
			SubMj.Ricval=0;
		}
	}

/**************************************END OF FILE**********************************************/
//-*********************mjetnp.j
}
