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
// Mjpcnt.j
//-*****************
public partial class MahjongBase : SceneBase {
/*
**		麻雀大会３
**		牌のカウント
*/

//#include "MahJongRally.h"								// Module interface definitions

public int getrisk(/*MahJongRally * pMe*/)
{
	int i, x, h=0, y=0, s=0;
	int[] sangentbl= new int [3];

	for(i=0; i<3; i++)
		sangentbl[i]=0;
	for(i=0; i<gpsPlayerWork.byFhcnt; i++){
		if((x=gpsPlayerWork.byFrhai[i])<0x40)
			h+=SubMj.Doracnt[x]+SubMj.Doracnt[x+1]+SubMj.Doracnt[x+2];
		else {
			h+=SubMj.Doracnt[x&0x3F]*(x<0x80 ? 3 : 4);
			y+=Ff[x&=0x3F];
			if(x>=0x35) {
				++s;
				sangentbl[x-0x35]=1;
			}
		}
	}
	if(s==3)
		return 13;
	if(s==2){
		for(x=0x35; sangentbl[x-0x35] != 0; x++);
		if(SubMj.Paicnt[x]>=3 && SubMj.apftbl[Order,x] != 0){
			SubMj.daisangen=Order;
			SubMj.daisanpai=x;
		}
	}
	if(gpsPlayerWork.bFrich != 0){
		y+=4;
		if(gpsPlayerWork.bFippat != 0 && Rultbl[(int)RL.IPPAT] != 0) {
			y++;
		}
		if(gpsPlayerWork.bFwrich != 0) {
			y++;
		}
	}
	if(y==0) {
		y=1;
	}
	return (y+=h)>13 ? 13 : y;
}

public void	initppr_pcnt ( /*MahJongRally * pMe*/ ) // 1995.6.23, 7.13
{

	byte[]	rbtbl={36,33,31,29,28,26,25,23,22,21,20,20,19,18,17,17,16,16};
	byte[]	rktbl={2,2,4,8,16,20,24,28,32,32,32,32,32,32,32,32,32,32};
	byte[]	vtbl={1,2,4,7,7,7,7,7,7,7,7,7,7};		//[13]

//0505mt	BYTE	Paitbl[] = {	//[ MAX_PAI_KIND ]
//0505mt		0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
//0505mt		0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19,
//0505mt		0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29,
//0505mt		0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0
//0505mt	};

	int x, y, k, v;
	byte[]	p= Paitbl;	int	p0= 0;

	if( gpsTableData.psParam.chParaRon == 0)
		return;
	while((x=p[p0++]) != 0) {
		SubMj.rcenter[x]=0;
		if(x<0x30){
		//-*check:字牌以外
			y=x&0x0F;
			if(y<7)
				SubMj.rabove[x]=SubMj.Paicnt[x+1]*SubMj.Paicnt[x+2]*vtbl[SubMj.sdv[x]]*2;
			else if(y==7)
				SubMj.rcenter[x]+=SubMj.Paicnt[x+1]*SubMj.Paicnt[x+2]*vtbl[SubMj.sdv[x]];
			if(y>3)
				SubMj.rbelow[x]=SubMj.Paicnt[x-2]*SubMj.Paicnt[x-1]*vtbl[SubMj.sdv[x-2]]*2;
			else if(y==3)
				SubMj.rcenter[x]+=SubMj.Paicnt[x-2]*SubMj.Paicnt[x-1]*vtbl[SubMj.sdv[x-2]];
			if(y>=2 && y<=8)
				SubMj.rcenter[x]+=SubMj.Paicnt[x-1]*SubMj.Paicnt[x+1]*vtbl[SubMj.sdv[x-1]];
		}
		if(SubMj.Paicnt[x]>=2){
			v=(SubMj.Paicnt[x]-1)*vtbl[SubMj.jdv[x]]*2;
			if(SubMj.Paicnt[x]>=3)
				v+=(SubMj.Paicnt[x]-2)*(SubMj.Paicnt[x]-1)*vtbl[SubMj.kdv[x]+Ff[x]];
			if(x>0x30)
				v*=3;
			SubMj.rcenter[x]+=v;
		}
		SubMj.rcenter[x]/=2;
	}
	SubMj.risk[Order]=(byte)getrisk();
	for(x=0; x<4; x++)
		SubMj.rk[x]=rktbl[(x==Order || SubMj.tenpai[x] != 0) ? SubMj.risk[x] : 1];
	SubMj.rkr=rktbl[SubMj.risk[Order]+3]-SubMj.rk[Order];
	SubMj.rk[gpsTableData.byOya]+=SubMj.rk[gpsTableData.byOya]/2;
	if(Order==gpsTableData.byOya)
		SubMj.rkr+=SubMj.rkr/2;
	k=rbtbl[SubMj.Tleft];
	for(x=v=0; x<4; x++)
		v+=(SubMj.rk[x]=SubMj.rk[x]*k/8);
	SubMj.rkr=SubMj.rkr*k/8;
	for(x=0; x<4; x++)
		if(x!=Order){
			SubMj.rksum[x]=60+240/(v-SubMj.rk[x]);
			SubMj.rkrsum[x]=60+240/(v+SubMj.rkr-SubMj.rk[x]);
		}
}

public void getppr_pcnt ( /*MahJongRally * pMe,*/ int x ) // 1995.6.15, 7.20
{
	int i, v, y;
	int rvsum, rvrsum = 0;	//w4
	int[] rv= new int [4];

	if( gpsTableData.psParam.chParaRon == 0) {
		SubMj.Ppr=24;
		SubMj.Pprr=18;
	} else {
		rvsum=0;
		for(i=0; i<4; i++)
			if(((y=SubMj.apftbl[i,x])&0x80) != 0) {
				v=SubMj.rcenter[x];
				if((y&0x40) != 0)
					v+=SubMj.rabove[x];
				if((y&0x10) != 0)
					v+=SubMj.rbelow[x];
				if(i==Order)
					rvrsum=v*SubMj.rkr;
				rvsum+=(rv[i]=v*SubMj.rk[i]);
			} else
				rv[i]=0;
		rvrsum+=rvsum;
		SubMj.Ppr=SubMj.Pprr=0;
		for(i=0; i<4; i++)
			if(i!=Order)
				if(gsPlayerWork[i].bFrich != 0) {
					SubMj.Ppr+=8; SubMj.Pprr+=8;	}
				else {
					#if true //-*todo
					// Ppr+=v=1000/((rvsum-rv[i])/16+rksum[i]);
					SubMj.Ppr+=(byte)(v=1000/((rvsum-rv[i])/16+SubMj.rksum[i]));
					#endif //-*todo:
//#if	1	//	Ver 1.3	BugFix	2005/12/15
					if (gsPlayerWork[i].sParamData.chParaOri <= 5)
//#else	//	Ver 1.3	BugFix	2005/12/15
//					if(paratbl[i].chParaOri<=-5)
//#endif	//	Ver 1.3	BugFix	2005/12/15
					#if true //-*todo
						// Pprr+=v;
						SubMj.Pprr=(byte)(SubMj.Pprr+v);
					#endif //-*todo:
					else
					#if true //-*todo:
						// Pprr+=1000/((rvrsum-rv[i])/16+rkrsum[i]);
						SubMj.Pprr+=(byte)(1000/((rvrsum-rv[i])/16+SubMj.rkrsum[i]));
					#endif //-*todo:
				}
	}
	SubMj.Ppr=(byte)(SubMj.Ppr*SubMj.pc[x]*SubMj.ppx[x]/8);
	SubMj.Pprr=(byte)(SubMj.Pprr*SubMj.pc[x]*SubMj.ppx[x]/8);
}


public short GetPoint(/*MahJongRally * pMe,*/ int odr)
{
	return (gpsTableData.sMemData[odr].nPoint);
}


public void _loadParam(/*MahJongRally * pMe,*/ int odr, TABLEMEM pTableMem)
{
#if true //-*キャラパラメーター
	int	iChara	=	pTableMem.byMember;

	//-*gsPlayerWork[odr].sParamData = GetCharacterparam(iChara);	//MEMCPY((gsPlayerWork[odr].sParamData), (GetCharacterparam(iChara)), sizeof(PARA));
	gsPlayerWork[odr].sParamData = MJDefine.sCharaMJParam[iChara];

	//-*SUGI_DEB***************************
	gsPlayerWork[odr].sParamData = new PARA (  0, 0, 0, 0, 0, 0, 0, 0, 0,0,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,0,0);
	#if SUGI_DEB //-*todo:注デバッグ中	if(m_DebBox != null && m_DebBox.GetTumikomiNo() >= 0){
		if(m_DebBox != null && m_DebBox.GetDebugFlag(DebBoxInGame.FUNCTION_LIST.THINK)){
			var debPara = m_DebBox.GetDebPara();
			if(debPara.Length >= 30){
				PARA temp = new PARA(
					debPara[0],debPara[1],debPara[2],debPara[3],debPara[4],debPara[5],debPara[6],debPara[7],debPara[8],debPara[9],
					debPara[10],debPara[11],debPara[12],debPara[13],debPara[14],debPara[15],debPara[16],debPara[17],debPara[18],debPara[19],
					debPara[20],debPara[21],debPara[22],debPara[23],debPara[24],debPara[25],debPara[26],debPara[27],debPara[28],debPara[29]
				);
				gsPlayerWork[odr].sParamData = temp;
				if(odr >= gsPlayerWork.Length-1)m_DebBox.SetDebugFlagOFF(DebBoxInGame.FUNCTION_LIST.THINK);
			}

		}
	#endif //-*todo:注デバッグ中
	//-****************************SUGI_DEB
#endif //-*todo:
}


public void _InitPara(/*MahJongRally * pMe,*/ int odr)
{
	int	x, y, kk, ps = 0, pn=0;	//w4
	//tamaki int	i;

	SubMj.tenpai[odr]=gsPlayerWork[odr].byTenpai;
	SubMj.notflg[odr]=SubMj.pntnec[odr]=SubMj.pntsuf[odr]=0;

	gpsTableData.psParam	=	gsPlayerWork[odr].sParamData;	//&gsPlayerWork[odr].sParamData;
	_loadParam( odr, gpsTableData.sMemData[odr]);		//xxxx	psParam を下で入れなおす
//	gpsTableData.psParam	=	gsPlayerWork[odr].sParamData;
	y = GetPoint( odr);
	if( gpsTableData.psParam.chParaStrat == 0 || y<sRuleSubData.nToplin)
		return;
	if(odr==Get_Rank( 0)) {												// トップ
		ps=y-GetPoint( Get_Rank( 1));									// ２位との差
	}
	else {
		for(x=0; Get_Rank( x) != odr; x++) {							// -8000点以内の相手
			if((ps=GetPoint( x)-y)<=80) {
				break;
			}
		}
	}
	ps+=1;
	if((kk=(ps+6)/(Kyoend-gpsTableData.byKyoku)/16)==0)
		return;
	if(Get_Rank( 0) == odr){
		if((gpsTableData.psParam.chParaOri+=kk)>5)
			gpsTableData.psParam.chParaOri=5;
		if((gpsTableData.psParam.chParaKan-=kk)<0)
			gpsTableData.psParam.chParaKan=0;
		if((gpsTableData.psParam.chParaRichi+=kk)>7)
			gpsTableData.psParam.chParaRichi=7;
		if(odr==gpsTableData.byOya){
			if((gpsTableData.psParam.chParaYaku+=kk)>0)
				gpsTableData.psParam.chParaYaku=0;
			if(Rultbl[(int)RL.NANBA] == 0 && (Rultbl[(int)RL.NOTEN] == 0 || kk>=3))
				SubMj.notflg[odr]=1;
		} else {
			gpsTableData.psParam.chParaHonisop=gpsTableData.psParam.chParaToitoip=0;
			if((gpsTableData.psParam.chParaYaku-=kk)<-3)
				gpsTableData.psParam.chParaYaku=-3;
			if((gpsTableData.psParam.chParaFanpaih-=kk)<0)
				gpsTableData.psParam.chParaFanpaih=0;
			if((gpsTableData.psParam.chParaFanpai1-=kk)<0)
				gpsTableData.psParam.chParaFanpai1=0;
			if((gpsTableData.psParam.chParaFanpai2-=kk)<0)
				gpsTableData.psParam.chParaFanpai2=0;
			if((gpsTableData.psParam.chParaTanyoh-=kk)<0)
				gpsTableData.psParam.chParaTanyoh=0;
			if((gpsTableData.psParam.chParaUra-=kk)<-1)
				gpsTableData.psParam.chParaUra=-1;
		}
	} else {
		gpsTableData.psParam.chParaHonisop=2;
		if((gpsTableData.psParam.chParaYaku+=kk)>0)
			gpsTableData.psParam.chParaYaku=0;
		if((gpsTableData.psParam.chParaOri-=kk)<-4)
			gpsTableData.psParam.chParaOri=-4;
		if((gpsTableData.psParam.chParaKan+=kk)>4)
			gpsTableData.psParam.chParaKan=4;
		if((gpsTableData.psParam.chParaRichi-=kk)<-7)
			gpsTableData.psParam.chParaRichi=-7;
		if((gpsTableData.psParam.chParaUra+=kk)>3)
			gpsTableData.psParam.chParaUra=7;
		if(((odr-gpsTableData.byOya)&3)>=Kyoend-gpsTableData.byKyoku){
			if((SubMj.pntnec[odr]=(byte)pn)>80)
				SubMj.pntnec[odr]=80;
			SubMj.pntsuf[odr]=(byte)ps;
		}
	}
	if( gpsTableData.psParam.chParaOri> 5)		gpsTableData.psParam.chParaOri= 5;		//xxxx
	if( gpsTableData.psParam.chParaOri< -4)		gpsTableData.psParam.chParaOri=-4;		//xxxx
	if( gpsTableData.psParam.chParaKan< 0)		gpsTableData.psParam.chParaKan= 0;		//xxxx
	if( gpsTableData.psParam.chParaKan> 4)		gpsTableData.psParam.chParaKan= 4;		//xxxx
	if( gpsTableData.psParam.chParaRichi> 7)	gpsTableData.psParam.chParaRichi= 7;	//xxxx
	if( gpsTableData.psParam.chParaRichi<-7)	gpsTableData.psParam.chParaRichi=-7;	//xxxx

	if( gpsTableData.psParam.chParaYaku> 0)		gpsTableData.psParam.chParaYaku= 0;		//xxxx
	if( gpsTableData.psParam.chParaYaku< -3)	gpsTableData.psParam.chParaYaku=-3;		//xxxx
	if( gpsTableData.psParam.chParaFanpaih< 0)	gpsTableData.psParam.chParaFanpaih= 0;	//xxxx
	if( gpsTableData.psParam.chParaFanpai1< 0)	gpsTableData.psParam.chParaFanpai1= 0;	//xxxx
	if( gpsTableData.psParam.chParaFanpai2< 0)	gpsTableData.psParam.chParaFanpai2= 0;	//xxxx
	if( gpsTableData.psParam.chParaTanyoh< 0)	gpsTableData.psParam.chParaTanyoh= 0;	//xxxx
	if( gpsTableData.psParam.chParaUra< -1)		gpsTableData.psParam.chParaUra= -1;		//xxxx
	if( gpsTableData.psParam.chParaUra> 3)		gpsTableData.psParam.chParaUra= 7;		//xxxx
}


public byte[] apfdata={
	0xCC,0xEE,0xFF,0xFF,0xFF,0xFF,0xFF,0xBB,0x99,
	0xCC,0xEE,0xFF,0xFF,0xFF,0xFF,0xFF,0xBB,0x99,
	0xCC,0xEE,0xFF,0xFF,0xFF,0xFF,0xFF,0xBB,0x99,
	0x88,0x88,0x88,0x88,0x88,0x88,0x88
};
public void intpc_pcnt ( /*MahJongRally * pMe*/ )
{
	int i, j;

//0505mt	BYTE	Paitbl[] = {	//[ MAX_PAI_KIND ]
//0505mt		0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
//0505mt		0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19,
//0505mt		0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29,
//0505mt		0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37,
//0505mt		0x00
//0505mt	};

	for(i=0; (j=Paitbl[i]) != 0; i++){
		SubMj.Paicnt[j]=4;
		SubMj.Doracnt[j]=0;
		SubMj.jdv[j]=SubMj.kdv[j]=0;
		if(j<0x30 && (j&0x0F)<=7)
			SubMj.sdv[j]=0;
		SubMj.apftbl[0,j]=SubMj.apftbl[1,j]=SubMj.apftbl[2,j]=SubMj.apftbl[3,j]=apfdata[i];
	}
	adddora_pcnt((byte)0x00);
	for(i=0; i<4; i++) {
		_InitPara( i);
	}
}


/*****************************
	牌のカウントをデクリメント
*****************************/
public void decpc_pcnt ( /*MahJongRally * pMe,*/ int x )
{
	SubMj.Paicnt[x]--;
}

/*****************************
	牌のカウントをクリア（カンの時）
*****************************/
public void clrpc_pcnt ( /*MahJongRally * pMe,*/ int x ) // 1995.6.1
{
	SubMj.Paicnt[x]=0;
}

/*****************************
	ドラを加える
*****************************/
public void adddora_pcnt (/*MahJongRally * pMe,*/ byte byPai) // 1995.6.1
{
	int x= Dora[Kancnt];

	SubMj.Doracnt[x]++;

	Dorakan = 0;
	if (byPai == x) {
		if ( Rultbl[(int)RL.KAN] != 0 ) {
			Dorakan	=	SubMj.Doracnt[x];
		}
	}

	SubMj.jdv[x]+=2;
	SubMj.kdv[x]+=3;
	if(x<0x30){
		if((x&0x0F)<=7) {
			SubMj.sdv[x]++;
		}
		if((x&0x0F)<=8 && (x&0x0F)>=2) {
			SubMj.sdv[x-1]++;
		}
		if((x&0x0F)>=3) {
			SubMj.sdv[x-2]++;
		}
	}
	--x;
//#if	1	//	Ver 1.3	BugFix	2005/11/29
	if ((x & 0xF0) == 0x30) {				/*	字牌	*/
		if ((x & 0x0F) == 0) {				/*	ドラが東	*/
			x	|=	0x04;
		}
		else if ((x & 0x0F) == 4) {			/*	ドラが白	*/
			x	|=	0x07;
		}
	}
	else if ((x & 0x0F) == 0) {				/*	ドラが１マン、１ピン、１ソウ	*/
		x	|=	0x09;
	}
//#else	//	Ver 1.3	BugFix	2005/11/29
//	if ((x & 0x0F) == 0) {				/*	ドラが１マン、１ピン、１ソウ、東	*/
//		if ((x & 0xF0) == 0x30) {		/*	東	*/
//			x	|=	0x07;
//		}
//		else {
//			x	|=	0x09;
//		}
//	}
//#endif	//	Ver 1.3	BugFix	2005/11/29
	SubMj.Paicnt[x]--;
}


/*****************************
	アンパイリスト？
*****************************/
public void newanp_pcnt ( /*MahJongRally * pMe,*/ int x, int csr )
{
	int i, mask;

	for(i=0; i<4; i++){
#if true //-*todo:
		// App=apftbl[i];
		for(int a=0;a<MJDefine.APFTBL_COUNT;a++){
			SubMj.App[a] = SubMj.apftbl[i,a];
		}
#endif	//-*todo:
		mask= (i==Order || gsPlayerWork[i].bFrich != 0) ? 0 : 0xF0;
		SubMj.App[x]&=(byte)(0|mask);
		if(x<0x30){
			if((x&0x0F)>=4)
				SubMj.App[x-3]&=(byte)(0xBB|mask);
			if((x&0x0F)<=6)
				SubMj.App[x+3]&=(byte)(0xEE|mask);
		}
	}
	if(csr!=gpsPlayerWork.byThcnt || Status != 0 & (byte)ST.SRICHI != 0) {
#if true //-*todo:
		for(int a=0;a<MJDefine.APFTBL_COUNT;a++){
			SubMj.App[a] = SubMj.apftbl[Order,a];
		}
		for(i=1; i<56; i++) {
			SubMj.App[i]|=(byte)(SubMj.App[i]/16);
		}

#else 	//-*todo:
		for(i=1, App=apftbl[Order]; i<56; i++) {
			SubMj.App[i]|=(byte)(SubMj.App[i]/16);
		}
#endif	//-*todo:
	}
	SubMj.tenpai[Order]=gpsPlayerWork.byTenpai;
}

/************************************************/
/*						*/
/************************************************/
public int howdanger(/*MahJongRally * pMe,*/ int x, byte f, int trap)
{
	byte[] vtbl={1,2,4,7,7,7,7,7,7,7,7,7,7};	//[13]

	int val=0, v;
	if((x< 3) || (x>=(0x38-2)))	return 0;	//xxxx	pc[] で範囲外に
	if( (f&8) == 0)
		return 0;
	v=0;
	if(SubMj.pc[x]>=2)
		v=SubMj.pc[x]*(SubMj.pc[x]-1)*(cntbuf[x]+1)*vtbl[SubMj.kdv[x]+Ff[x]];
	v+=SubMj.pc[x]*2*(cntbuf[x]+1)*vtbl[SubMj.jdv[x]];
	if(x>0x30)
		v*=2;
	val+=v;
	if((f&2) == 0) {
		v=SubMj.pc[x-1]*SubMj.pc[x+1]*(cntbuf[x]+1);
		val+=vtbl[SubMj.sdv[x-1]]*v;
	}
	if((f&1) == 0) {
		v=SubMj.pc[x-2]*SubMj.pc[x-1]*(cntbuf[x]+cntbuf[x-3]+1);
		if((x&0x0F)>3){
			v*=2;
			if( trap == 0)
				v*=2;
		}
		val+=vtbl[SubMj.sdv[x-2]]*v;
	}
	if((f&4) != 0) {
		v=SubMj.pc[x+1]*SubMj.pc[x+2]*(cntbuf[x]+cntbuf[x+3]+1);
		if((x&0x0F)<7){
			v*=2;
			if( trap== 0)
				v*=2;
		}
		val+=vtbl[SubMj.sdv[x]]*v;
	}
	if( trap == 0)
		val/=2;
	if((val/=4)>255)
		val=255;
	return val;
}

public int getchance(/*MahJongRally * pMe,*/ int x, byte f, int trap)
{
	int val=0, v;

	if( (f&8) == 0)
		return 0;
	v=0;
	if(SubMj.pc[x]>=2)
		v=SubMj.pc[x]*(SubMj.pc[x]-1)*(cntbuf[x]+1);
	v+=SubMj.pc[x]*2*(cntbuf[x]+1);
	if(x>0x30)
		v*=2;
	val+=v;
	if((f&2) != 0)
		val+=SubMj.pc[x-1]*SubMj.pc[x+1]*(cntbuf[x]+1);
	if((f&1) != 0) {
		v=SubMj.pc[x-2]*SubMj.pc[x-1]*(cntbuf[x]+cntbuf[x-3]+1);
		if((x&0x0F)>3){
			v*=2;
			if( trap == 0)
				v*=2;
		}
		val+=v;
	}
	if((f&4) != 0) {
		v=SubMj.pc[x+1]*SubMj.pc[x+2]*(cntbuf[x]+cntbuf[x+3]+1);
		if((x&0x0F)<7){
			v*=2;
			if( trap == 0)
				v*=2;
		}
		val+=v;
	}
//#if	1	//	Ver 1.3	BugFix	2005/12/15
	if(val > 255) {
		val=255;
	}
//#endif	//	Ver 1.3	BugFix	2005/12/15
	return val;
}

public void calcrisk(/*MahJongRally * pMe*/)
{
	byte[]	rktbl={2,2,4,8,16,20,24,28,32,32,32,32,32,32,32,32,32,32};

	int		x, v, trap, sum, m_base;
	long	vl;
	byte[]	p, q;
	int	p0= 0, q0= 0;
	int		odrstr;
	byte[]	paibuf= new byte [35];
	int[] k={4,6,8,11,16,23,32,45,64,91};	//[15]

	byte[] Paitbl = new byte[]{	//[ MAX_PAI_KIND ]
		0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
		0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19,
		0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29,
		0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0
	};

	for(p=Paitbl, q=paibuf; (x=p[p0]) != 0; p0++)	//p++
		if(cntbuf[x] != 0)
			q[q0++]=(byte)x;
//	q=0;
	for(p0= 0, p=paibuf; (x=p[p0]) != 0; p0++) {	//p++
		SubMj.riskval[x]=0;	SubMj.safeval[x]=0;	}
	if(gpsTableData.psParam.chParaOri<=-5)
		return;
//#if	1	//	Ver 1.3	BugFix	2005/12/15
	for(x=0, m_base=3; x<Kancnt; x++)
//#else	//	Ver 1.3	BugFix	2005/12/15
//	for(x=0, base=3; x<Dorakan; x++)
//#endif	//	Ver 1.3	BugFix	2005/12/15
		m_base+=SubMj.pc[ Dora[x]];
	m_base=m_base*k[gpsTableData.psParam.chParaOri+4]/4;
	if(gpsPlayerWork.bFmenzen != 0)
		m_base/=2;
	odrstr=Order;
	while((Order=(byte)((Order+1)&3))!=odrstr){
//#if	1	//	Ver 1.3	BugFix	2005/12/15
		trap=gsPlayerWork[Order].sParamData.chParaRon;
//#else	//	Ver 1.3	BugFix	2005/12/15
//		setwp_m2( Order);
//		trap=paratbl[Order].chParaRon;
//#endif	//	Ver 1.3	BugFix	2005/12/15
#if true //-*todo:
		// App = apftbl[Order];
		for(int a=0;a<MJDefine.APFTBL_COUNT;a++){
			SubMj.App[a] = SubMj.apftbl[Order,a];
		}
#endif	//-*todo:
		if(SubMj.tenpai[Order] != 0){
			sum=0;
			for(p0= 0, p=paibuf; (x=p[p0]) != 0; p0++)	//p++
				sum+=getchance( x,SubMj.App[x], trap);
			if((sum/=64)<1)
				sum=1;
			v=SubMj.risk[Order];
			if(Bpcnt+Kancnt== MJDefine.PAI_MAX)		//122x
				v++;
			v=m_base*rktbl[v]*2;
			if(Order==gpsTableData.byOya)
				v+=v/2;
			for(p0= 0, p=paibuf; (x=p[p0]) != 0; p0++) {	//p++
				if((vl=SubMj.riskval[x]+(long)v*howdanger( x, SubMj.App[x], trap)/sum)>16000L)
					vl=16000L;
				SubMj.riskval[x]=(short)vl;
			}
//#if	1	//	Ver 1.3	BugFix	2005/12/15
		} else if(gsPlayerWork[odrstr].sParamData.chParaAnpai != 0 && SubMj.Tleft>=4){
//#else	//	Ver 1.3	BugFix	2005/12/15
//		} else if(gpsTableData.psParam.chParaAnpai && Tleft>=4){
//#endif	//	Ver 1.3	BugFix	2005/12/15
			sum=0;
			for(p0= 0, p=paibuf; (x=p[p0]) != 0; p0++)	//p++
				sum+=getchance( x, (byte)(SubMj.App[x]/16), trap);
			v=SubMj.risk[Order];
			if(v<3 && gpsPlayerWork.bFmenzen == 0)
				v=3;
			v=rktbl[v]*m_base;
			if(Order==gpsTableData.byOya)
				v+=v/2;
			for(p0= 0, p=paibuf; (x=p[p0]) != 0; p0++)	//p++
				SubMj.safeval[x]+=(byte)((int)((long)v*howdanger( x, (byte)(SubMj.App[x]/16), trap)/sum));
		}
	}
	Order=(byte)odrstr;
	setwp_m2( Order);
	if(SubMj.daisangen>=0 && SubMj.daisangen!=Order && SubMj.pc[SubMj.daisanpai]>=2)
		SubMj.riskval[SubMj.daisanpai]+=(short)(k[gpsTableData.psParam.chParaOri+4]*200);
}

public void calcpcs_pcnt ( /*MahJongRally * pMe*/ ) // 1995.4.26, 6.14, 6.21, 6.28, 7.24
{
	int		x, q0= 0;
	byte[]	q;
//#if	1	//	Ver 1.3	BugFix	2005/12/15
	int[] keitbl={300,200,120,80,40,0,0,0,0,0,0,0,0,0,0,0,0,0};				//[18]
	int[] kishtbl={0,1,2,3,4,4,5,5,6,6,7,7,7,7,8,8,8,8};					//[18]
	int[] rrtbl={0,255,159,99,62,39,24,15,15,15,15,15,15,15,15,15,15,15};	//[18]
	byte[]	Paitbl = {		//[ MAX_PAI_KIND ]
		0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
		0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19,
		0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29,
		0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0
	};
//#else
//	int keitbl[]={300,200,120,80,40,0,0,0,0,0,0,0,0,0,0,0,0,0};				//[18]
//	int kishtbl[]={0,1,2,3,4,4,5,5,6,6,7,7,7,7,8,8,8,8};					//[18]
//	int rrtbl[]={0,255,159,99,62,39,24,15,15,15,15,15,15,15,15,15,15,15};	//[18]
//	BYTE	Paitbl[] = {		//[ MAX_PAI_KIND ]
//		0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09,
//		0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 0x18, 0x19,
//		0x21, 0x22, 0x23, 0x24, 0x25, 0x26, 0x27, 0x28, 0x29,
//		0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x37, 0
//	};
//#endif

//#if	1	//	Ver 1.3	BugFix	2005/12/15
	gpsTableData.psParam=(gsPlayerWork[Order].sParamData);
//#else	//	Ver 1.3	BugFix	2005/12/15
//	gpsTableData.psParam=paratbl+Order;
//#endif	//	Ver 1.3	BugFix	2005/12/15
	SubMj.Tleft= (byte)((MJDefine.PAI_MAX- Bpcnt- Kancnt)/ 4);		//122x	//-*check:([残り牌取得(牌数-牌番号[積み込み配列内の何番目か]-カン数)]/4)
	SubMj.keiten= (short)(gpsTableData.psParam.chParaKeiten != 0 && (Order==gpsTableData.byOya ? SubMj.notflg[Order] == 0 &&
		(Rultbl[(int)RL.NANBA] == 0 || Rultbl[(int)RL.NOTEN] != 0) : Rultbl[(int)RL.NOTEN] != 0) ? keitbl[SubMj.Tleft] : 0);
	SubMj.ricrisk=(short)(rrtbl[SubMj.Tleft]*(8+gpsTableData.psParam.chParaRichi)*(2+Richi));
	SubMj.kish=(byte)(kishtbl[SubMj.Tleft]*(24+gpsTableData.psParam.chParaKish));
	SubMj.ishnec=(byte)(18+4*(8+gpsTableData.psParam.chParaIshnum));
	for(q=Paitbl; (x=q[q0]) != 0; q0++) {	//q++
		byte[] pk={0,6,5,4,3};		//[5]
		SubMj.pc[x]=(byte)(SubMj.Paicnt[x]-cntbuf[x]);
		SubMj.ppx[x]=pk[SubMj.Paicnt[x]];
		SubMj.spc[x]=0;
	}
	for(q0= 0, q=gpsPlayerWork.bySthai, x=gpsPlayerWork.byShcnt; x>0; q0++, x--)	//q++
		SubMj.spc[q[q0]]++;
	x=Order;
	SubMj.daisangen=-1;
	for(Order=0; Order<4; Order++){
		setwp_m2( Order);
		SubMj.risk[Order]=(byte)getrisk();
	}
	Order=(byte)x;
	setwp_m2( Order);
	calcrisk();
	
#if true //-*todo:
		// App=apftbl[Order];
		for(int a=0;a<MJDefine.APFTBL_COUNT;a++){
			SubMj.App[a] = SubMj.apftbl[Order,a];
		}
#endif	//-*todo:
}

public void setapp_pcnt (/*MahJongRally * pMe,*/ int iOrder) // 1995.4.26, 6.21, 6.28
{
	
#if true //-*todo:
		// App = apftbl[iOrder];	//(BYTE *)
		for(int a=0;a<MJDefine.APFTBL_COUNT;a++){
			SubMj.App[a] = SubMj.apftbl[iOrder,a];
		}
#endif	//-*todo:
}

/**************************************END OF FILE**********************************************/


//-*********************Mjpcnt.j
}
