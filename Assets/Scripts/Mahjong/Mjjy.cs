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
// Mjjy.j
//-*****************
public partial class MahjongBase : SceneBase {
/*
**		麻雀大会３
**		役判定アルゴ
*/

//#include "MahJongRally.h"								// Module interface definitions


public void wrthan(/*MahJongRally * pMe,*/ int yn, int hn)
{
	YAKU_T	p;

	gMJKResult.byHan	+=	(byte)hn;
	p					=	gMJKResult.sYaku[gMJKResult.byYakuCnt++];
	p.name				=	(byte)yn;
	p.factor			=	(byte)hn;
}

public void wrtyaku(/*MahJongRally * pMe,*/ int yn, int hn)
{
	SubMj.yhan+= hn;
	wrthan(yn,hn);
}

public void wrtyman(/*MahJongRally * pMe,*/ int yn, int fc)
{
	if(gMJKResult.byYakuman == 0){
		gMJKResult.byYakuman	+=	4;
		gMJKResult.byHan		=	0;
		gMJKResult.byYakuCnt	=	0;
	}
	wrthan(yn, fc);
}

public void wrtyks(/*MahJongRally * pMe,*/ int yn, int hn)
{
	if (gpsPlayerWork.bFmenzen != 0) {
		hn--;
	}
	wrtyaku(yn,hn);
}

public int jyman_a(/*MahJongRally * pMe*/)
{
	int i;

	if( (Status&(byte)ST.ONE_JUN) == 0 && (Status&(byte)ST.RINSH) == 0){
		if( (Status&(byte)ST.RON) == 0) {
			if(Order==gpsTableData.byOya) {
				wrtyman((int)YK.TENHO, 4);											/*	天和	*/
			}
			else {
				wrtyman((int)YK.CHIHO, 4);											/*	地和	*/
			}
		}
		else if ((Status & (byte)ST.CHANK) == 0) {									/*	国士暗貫ロンの対策	*/
			if (Rultbl[(int)RL.RENHO] != 0) {
				i=gpsTableData.byOya;
				while(i!=Order)
					if(i==Odrbuf){
						wrtyman((int)YK.RENHO, 4);									/*	人和	*/
						break;
					} else
						i=(i+1)&3;
			}
		}
	}
	if(Rultbl[(int)RL.PAREN] != 0 && gpsTableData.byParen>=8 && Order==gpsTableData.byOya)
		wrtyman((int)YK.PAREN, 4);													/*	八連荘	*/

	if(gpsPlayerWork.bFkokus != 0) {											/*	国士無双	*/
#if	Rule_2P
		if( gpsPlayerWork.byTenpai== 13)	// 純正国士無双
			wrtyman((int)YK.JKOKUS, 8);
		else
			wrtyman((int)YK.KOKUS, 4);
#else
		wrtyman(YK_KOKUS, Rultbl[RL_DBYMAN] != 0 && gpsPlayerWork.byTenpai == 13 ? 8 : 4);
#endif
		return 1;
	}
	return 0;
}

public int jryuiso(/*MahJongRally * pMe*/)
{
	byte[]	p;
	int		p0= 0, q0= 0;
	byte[]	q;
	byte[]	rd={0x12,0x52,0x53,0x54,0x56,0x58,0x76,0xFF};
	q=rd;
	while(q[++q0]<(byte)(sMntData.byJanto|0x40));	//(++q<(BYTE)(sMntData.byJanto|0x40));
	if(q[q0]!=(byte)(sMntData.byJanto|0x40))
		return 0;

	p=sMntData.byMnt;
	q=rd;	q0= 0;
	do {
		while(q[q0]<p[p0]) {
			q0++;	//q++;
		}
		if(q[q0]!=p[p0])
			return 0;
	} while(++p0< 4);		//(++p<sMntData.byMnt+4);
	wrtyman((int)YK.RYUIS, 4);														/*	緑一色	*/
	return 1;
}

public int jchinroto(/*MahJongRally * pMe*/)
{
	byte[]	p;
	int		p0= 0;
	if(sMntData.byMnt[0]<0x40 || sMntData.byMnt[3]>0x70 || sMntData.byJanto>0x30 || (sMntData.byJanto&7)!=1)
		return 0;

	p=sMntData.byMnt;
	do {
		if((p[p0]&7)!=1)
			return 0;
	} while(++p0< 4);		//(++p<sMntData.byMnt+4);
	wrtyman((int)YK.CHINR, 4);														/*	清老頭	*/
	return 1;
}

public int jkouiso(/*MahJongRally * pMe*/)
{
	byte[]	rd={0x11,0x15,0x17,0x19,0x37,0xFF};
	//tamaki BYTE*	p;
	byte[]	q;
	int		i, p0= 0;
	byte	byData;

	if (Rultbl[(int)RL.HUNDRED_MAN] == 0)
		return 0;

	if(sMntData.byMnt[0]<(0x40 | 0x11) || sMntData.byMnt[3]>(0x37 | 0x40))
		return 0;

	q=rd;
	while (q[p0] != 0xFF && q[p0] != sMntData.byJanto) {		//(*q != 0xFF && *q != sMntData.byJanto)
		p0++;		//++q;
	}
	if (q[p0] == 0xFF)
		return (0);

	for (i = 0; i < 4; i++) {
		byData	=	(byte)(sMntData.byMnt[i] & 0x3F);
		q=rd;	p0= 0;
		while (byData > q[p0]) {		//(byData > *q)
			p0++;		//++q;
		}
		if (q[p0] != byData)			//(*q != byData)
			return (0);
	}
	wrtyman((int)YK.BENIK, 4);														/*	紅孔雀	*/
	return 1;
}

public int	jaonodoumon(/*MahJongRally * pMe*/)
{
	byte[]	rd={0x22,0x24,0x28,0x31,0x32,0x33,0x34,0xFF};
	//tamaki BYTE*	p;
	byte[]	q;
	int		i, p0= 0;
	byte	byData;

	if (Rultbl[(int)RL.HUNDRED_MAN] == 0)
		return 0;

	if(sMntData.byMnt[0]<(0x40 | 0x22) || sMntData.byMnt[3]>(0x34 | 0x40))
		return 0;

	q=rd;
	while (q[p0] != 0xFF && q[p0] != sMntData.byJanto) {	//(*q != 0xFF && *q != sMntData.byJanto)
		p0++;		//++q;
	}
	if (q[p0] == 0xFF)
		return (0);

	for (i = 0; i < 4; i++) {
		byData	=	(byte)(sMntData.byMnt[i] & 0x3F);
		q=rd;	p0= 0;
		while (byData > q[p0]) {		//(byData > *q)
			p0++;		//++q;
		}
		if (q[p0] != byData)			//(q != byData)
			return (0);
	}
	wrtyman((int)YK.DOMON, 4);														/*	青の桐門	*/
	return 1;
}

public int jhyakumansub(/*MahJongRally * pMe,*/ byte byData)
{
	for (int i = 0; i < gpsPlayerWork.byFhcnt; i++) {
		if ((gpsPlayerWork.byFrhai[i] & 0x0F) == byData) {
			switch (gpsPlayerWork.byFrhai[i] / 0x40) {
			  case 1:													/*	刻子	*/
				return (3);
//				break;
			  case 2:													/*	カン子	*/
			  case 3:													/*	カン子	*/
				return (4);
//				break;
			}
		}
	}
	return (3);
}

public int jhyakuman(/*MahJongRally * pMe*/)
{
	byte	byData;
	byte	byData1;
	int		i;
	int		iTotal;

	if (Rultbl[(int)RL.HUNDRED_MAN] == 0)
		return 0;

	if (!(sMntData.byJanto >= 0x01 && sMntData.byJanto <= 0x09))
		return (0);

	iTotal	=	sMntData.byJanto*2;
	for (i = 0; i < 4; i++) {
		byData	=	sMntData.byMnt[i];
		byData1	=	(byte)(byData & 0x3F);
		if (byData1 < 0x01 || byData1 > 0x09)
			return (0);

		byData1	=	(byte)(byData & 0x0F);
		switch (byData / 0x40) {
		  case 0:																/*	順子	*/
			iTotal	+=	byData1;
			iTotal	+=	byData1 + 1;
			iTotal	+=	byData1 + 2;
			break;
		  case 1:																/*	刻子	*/
			iTotal	+=	byData1 * jhyakumansub(byData1);
			break;
		}
	}
	if (iTotal < 100)
		return (0);

	wrtyman((int)YK.HUNDRED_MAN, 4);														/*	百万石	*/
	return 1;
}

public int jsharin(/*MahJongRally * pMe*/)
{
	if(gpsPlayerWork.byFhcnt != 0 || Rultbl[(int)RL.DAISH] == 0)
		return 0;
	for(int x=0x22; x<=0x28; x++)
		if(cntbuf[x]!=2)
			return 0;

	wrtyman((int)YK.SHARI, 4);														/*	大車輪	*/
	return 4;
}

public int jchuren(/*MahJongRally * pMe*/)
{
	int x;

	if(gpsPlayerWork.byFhcnt != 0 || SubMj.hikihai>0x30)
		return 0;
	if(cntbuf[x=(SubMj.hikihai&0x30)+1]<3 || cntbuf[x+8]<3)
		return 0;

	while(cntbuf[++x] != 0)
		;
	if((x&0x0F)!=10)
		return 0;

#if	Rule_2P
	if( gpsPlayerWork.byTenpai== 9)		// 純正九連宝燈
		wrtyman((int)YK.JCHURE, 8);
	else
		wrtyman((int)YK.CHURE, 4);
#else
	wrtyman(YK_CHURE, Rultbl[RL_DBYMAN] != 0 && gpsPlayerWork.byTenpai == 9 ? 8 : 4);		/*	九連宝燈	*/
#endif
	return 1;
}

//	四暗刻 チェック
public void jsuanko(/*MahJongRally * pMe*/)
{
	int f;

	if (gpsPlayerWork.bFmenzen != 0 || sMntData.byMnt[0]<0x30)
		return;

	if(sMntData.byJanto== SubMj.hikihai) {
		f= 8;		//四暗刻単騎
	} else if((Status & (byte)ST.RON) != 0) {
		return;
	} else {
		f= 4;
		SubMj.suanflg=1;
	}

#if	Rule_2P
	if( f== 4)
		wrtyman((int)YK.SUANK, f);		//四暗刻
	if( f== 8)
		wrtyman((int)YK.TSUANK, f);		//四暗刻単騎
#else
	wrtyman(YK_SUANK, Rultbl[RL_DBYMAN] != 0 ? f : 4);								/*	四暗刻	*/
#endif
}

public void jyman_b(/*MahJongRally * pMe*/)
{
	if(sMntData.byMnt[0]>0x70 && sMntData.byJanto>0x30)								/*	字一色	*/
		wrtyman((int)YK.TSUIS, 4);

	if (sMntData.byMnt[1] == 0x75) {
		if(SubMj.Pao3 != 0)
			SubMj.Paoflg=SubMj.Pao3;
		wrtyman((int)YK.DAISA,4);													/*	大三元	*/
	} else if(sMntData.byMnt[0]==0x71 && sMntData.byMnt[3]==0x74){
		if(SubMj.Pao4 != 0)
			SubMj.Paoflg=SubMj.Pao4;

		wrtyman((int)YK.DAISU, Rultbl[(int)RL.DBYMAN] != 0 ? 8 : 4);						/*	大四喜	*/
	}
	else if(sMntData.byJanto>=0x31 && sMntData.byJanto<=0x34 &&
		  (sMntData.byMnt[0]>=0x71 && sMntData.byMnt[2]<=0x74 || sMntData.byMnt[1]>=0x71 && sMntData.byMnt[3]<=0x74)) {
		wrtyman((int)YK.SHOSU, 4);													/*	小四喜	*/
	}
	else if(Rultbl[(int)RL.SANRE] != 0 && sMntData.byMnt[0]>=0x40 && sMntData.byMnt[3]<=0x70 && (byte)(sMntData.byMnt[0]+3)==sMntData.byMnt[3]) {
		wrtyman((int)YK.SUREN, 4);													/*	四連刻	*/
	}
//> 2006/03/23 役満判定不具合
//	else {		//0422
		jsharin();
		jchinroto();
		jryuiso();
		jchuren();
		jkouiso();
		jhyakuman();
		jaonodoumon();
//	}			//0422
//< 2006/03/23 役満判定不具合

	if(gpsPlayerWork.byKancnt == 4)
		wrtyman((int)YK.SUKAN, 4);													/*	四カンツ	*/

	jsuanko();
}

public void jhonroto(/*MahJongRally * pMe*/)
{
	byte[]	p;
	int		p0= 0;
	if(sMntData.byJanto<0x30 && (sMntData.byJanto&7)!=1)
		return;
	p=sMntData.byMnt;
	do {
		if(p[p0]>0x70)
			break;
		else if((p[p0]&7)!=1)
			return;
	} while(++p0< 4);		//(++p<sMntData.byMnt+4);
	wrtyaku((int)YK.HONRO, 2);														/*	混老頭	*/
}

public void jchanta(/*MahJongRally * pMe*/)
{
	byte[]	p;
	bool	flag;
	int		p0= 0;

	if(sMntData.byJanto<0x30 && (sMntData.byJanto&7)!=1)
		return;
	flag= sMntData.byJanto>= 0x30;
	p= sMntData.byMnt;
	do {
		if(p[p0]>0x70){
			flag=true;		//flag=1;
			break;
		} else if((p[p0]&0x47)!=7 && (p[p0]&7)!=1)
			return;
	} while(++p0< 4);		//(++p<sMntData.byMnt+4);

	if(flag)
		wrtyks((int)YK.CHANT, 2);													/*	チャンタ	*/
	else
		wrtyks((int)YK.JUNCH, 3);													/*	純チャン	*/
}

public void jpinho(/*MahJongRally * pMe,*/ bool fSet)
{
	byte[] p;
	int flag, p0= 0;

	if( (Status&(byte)ST.RON) == 0 && Rultbl[(int)RL.PINHO] == 0)
		return;

	if(Ff[sMntData.byJanto] != 0)
		return;

	flag=0;
	if((SubMj.hikihai&0x0F)<7){
		p=sMntData.byMnt;
		do {
			if(p[p0]==SubMj.hikihai){
				flag++;
				break;
			}
		} while(++p0< 4);		//(++p<sMntData.byMnt+4);
	}
	if( flag == 0 && (SubMj.hikihai&0x0f)>3) {
		p=sMntData.byMnt;
		p0= 0;
		do {
			if(p[p0]==(byte)(SubMj.hikihai-2)) {
				flag++;
				break;
			}
		} while(++p0< 4);		//(++p<sMntData.byMnt+4);
	}
	if(flag != 0){
		if (fSet)
			wrtyaku((int)YK.PINHO,1);		/*	平和	*/
		SubMj.pinho=1;
	}
}

public void jsanshiki(/*MahJongRally * pMe*/)
{
	if((byte)(sMntData.byMnt[0]+0x10)==sMntData.byMnt[1] && ((byte)(sMntData.byMnt[1]+0x10)==sMntData.byMnt[2] || (byte)(sMntData.byMnt[1]+0x10)==sMntData.byMnt[3])
	|| (byte)(sMntData.byMnt[2]+0x10)==sMntData.byMnt[3] && ((byte)(sMntData.byMnt[0]+0x10)==sMntData.byMnt[2] || (byte)(sMntData.byMnt[1]+0x10)==sMntData.byMnt[2]))
		wrtyks((int)YK.SANSH, 2);													/*	三色同順	*/
	else
		if((byte)(sMntData.byMnt[0]+3)==sMntData.byMnt[1] && ((byte)(sMntData.byMnt[1]+3)==sMntData.byMnt[2] || (byte)(sMntData.byMnt[1]+3)==sMntData.byMnt[3])
		|| (byte)(sMntData.byMnt[2]+3)==sMntData.byMnt[3] && ((byte)(sMntData.byMnt[0]+3)==sMntData.byMnt[2] || (byte)(sMntData.byMnt[1]+3)==sMntData.byMnt[2]))
			wrtyks((int)YK.ITSU, 2);													/*	一気通貫	*/
}

public void jipeko(/*MahJongRally * pMe*/)
{
	byte[]	p;
	int		p0= 0;
	p=sMntData.byMnt;
	do {
		if(p[p0]==(p[p0+1])){
			wrtyaku((int)YK.IPEKO, 1);												/*	一盃口	*/
			return;
		}
	} while(++p0< 3);		//(++p<sMntData.byMnt+3);
}

public void jsananko(/*MahJongRally * pMe*/)
{
	byte[] p;
	int i, flag1=0, flag2=0, p0=0;

	if(sMntData.byMnt[0]>0x30)
		flag1=1;

	for(i=gpsPlayerWork.byFhcnt, p= gpsPlayerWork.byFrhai; i != 0; i--, p0++)	//(i=gpsPlayerWork.byFhcnt, p= gpsPlayerWork.byFrhai; i; i--, p++)
		if((byte)(p[p0]&0x40) != 0 && --flag1<0)
			return;
		else if(p[p0]==sMntData.byMnt[0])
			flag2=1;

	if(flag1==0 && SubMj.hikihai!=sMntData.byJanto && 
		(flag2 != 0 || 
		(SubMj.hikihai < sMntData.byMnt[0]) || 
		SubMj.hikihai>(byte)(sMntData.byMnt[0]+2) ) ) {
		SubMj.sananflg= 1;
		if((Status&(byte)ST.RON) != 0){
			return;
		}
	}
	wrtyaku((int)YK.SANAN, 2);														/*	三暗刻	*/
}

public void jdopon(/*MahJongRally * pMe*/)
{
	if(sMntData.byMnt[0]<0x50 && (byte)(sMntData.byMnt[0]+0x10)==sMntData.byMnt[1]
	&& ((byte)(sMntData.byMnt[1]+0x10)==sMntData.byMnt[2] || (byte)(sMntData.byMnt[1]+0x10)==sMntData.byMnt[3])
	|| sMntData.byMnt[3]<0x70 && (byte)(sMntData.byMnt[2]+0x10)==sMntData.byMnt[3]
	&& ((byte)(sMntData.byMnt[0]+0x10)==sMntData.byMnt[2] || (byte)(sMntData.byMnt[1]+0x10)==sMntData.byMnt[2]))
		wrtyaku((int)YK.DOPON, 2);													/*	三色同刻	*/
	else
		if(Rultbl[(int)RL.SANRE] != 0 && sMntData.byMnt[1]<0x70 && ((byte)(sMntData.byMnt[1]+2)== sMntData.byMnt[3]
		|| (byte)(sMntData.byMnt[0]+2)==sMntData.byMnt[2]))
			wrtyaku((int)YK.SANRE, 2);													/*	三連刻	*/
}

public void jyaku_a(/*MahJongRally * pMe*/)
{
	SubMj.pinho= 0;
	if(sMntData.byMnt[0]>=0x40){
		jhonroto();
		wrtyaku((int)YK.TOITO, 2);													/*	対々和	*/
	} else {
		jchanta();
		if(sMntData.byMnt[3]<0x30 && gpsPlayerWork.bFmenzen == 0){
			jpinho(true);
			//> 2006/04/15 DS依存バグNo.4
			// 同じ順子が４組できる場合に、二盃口役が付かないようになっている
			if((sMntData.byMnt[0]==sMntData.byMnt[1]) && (sMntData.byMnt[2]==sMntData.byMnt[3])){	//0422
//			if(sMntData.byMnt[0]==sMntData.byMnt[1] && sMntData.byMnt[2]==sMntData.byMnt[3] && sMntData.byMnt[1]!=sMntData.byMnt[2]){
			//< 2006/04/15 DS依存バグNo.4
				wrtyaku((int)YK.RYANP, 3);											/*	二盃口	*/
				return;
			}
		}
		if(sMntData.byMnt[2]<0x30){
			if(sMntData.byMnt[1]==sMntData.byMnt[2] && (sMntData.byMnt[0]==sMntData.byMnt[1] || sMntData.byMnt[2]==sMntData.byMnt[3])){
				if(Rultbl[(int)RL.SANJUN] != 0) {
					wrtyks((int)YK.DOSHU, 2);										/*	一色三順	*/
				}
				return;
			}
			jsanshiki();
		}
		if(sMntData.byMnt[1]<0x30){
			if (gpsPlayerWork.bFmenzen == 0)
				jipeko();
			return;
		}
	}
	jsananko();
	jdopon();
}

public void jhoniso(/*MahJongRally * pMe*/)
{
	byte[] p;
	byte x;
	int flag=0, p0= 0;

	p=sMntData.byMnt;

	if((x=(byte)(sMntData.byJanto&0x30))==0x30){
		flag++;
		x=(byte)(p[p0++]&0x30);
	}
	do {
		if(p[p0]>0x70){
			flag++;
			break;
		} else
			if((byte)(p[p0]&0x30)!=x)
				return;
	} while(++p0< 4);		//(++p<sMntData.byMnt+4);

	if(flag != 0)
		wrtyks((int)YK.HONIS, 3);													/*	混一色	*/
	else
		wrtyks((int)YK.CHINI, 6);													/*	清一色	*/
}

public void jtanyo(/*MahJongRally * pMe*/)
{
	byte[]	p;
	int		p0= 0;
	if(sMntData.byJanto>0x30 || (sMntData.byJanto&7)==1) {
		return;
	}
	p=sMntData.byMnt;
	do {
		if(p[p0]>0x70 || (p[p0]&0x47)==7 || (p[p0]&7)==1) {
			return;
		}
	} while(++p0< 4);		//(++p<sMntData.byMnt+4);
	if (Rultbl[(int)RL.KUITA] != 0) {												/*	食い断有り	*/
		wrtyaku((int)YK.TANYO, 1);													/*	断ヤオ	*/
	}
	else if (gpsPlayerWork.bFmenzen == 0) {
		wrtyaku((int)YK.TANYO, 1);													/*	断ヤオ	*/
	}
}

public void jfanpai(/*MahJongRally * pMe*/)
{
	byte[]	p;
	int h, fc=0, p0= 0;

	p=sMntData.byMnt;
	do {
		if((h=Ff[p[p0]&0x3F]) != 0) {
			fc+=h;
		}
	} while(++p0< 4);		//(++p<sMntData.byMnt+4);

	if(fc != 0)
		wrtyaku((int)YK.FANPA, fc);													/*	飜牌	*/
}

public void jyaku_b(/*MahJongRally * pMe*/)
{
	jhoniso();
	jtanyo();
	if(sMntData.byMnt[2]>=0x75 && sMntData.byJanto>=0x35) {
		wrtyaku((int)YK.SHOSA, 2);													/*	小三元	*/
	}
	if(gpsPlayerWork.byKancnt >= 3) {
		wrtyaku((int)YK.SANKA, 2);													/*	三カン子	*/
	}
	jfanpai();
}

public void jhadaka(/*MahJongRally * pMe*/)
{
	if (Rultbl[(int)RL.KINCH] == 0)
		return;
	if (gpsPlayerWork.byFhcnt != 4)
		return;

	wrtyaku((int)YK.HADAK, 1);														/*	裸単騎	*/
}

public void jyaku_c(/*MahJongRally * pMe*/)
{
	int n;

	if(gpsPlayerWork.bFrich != 0){
		if(gpsPlayerWork.bFwrich != 0)
			wrtyaku((int)YK.WRICH, 2);												/*	ダブル立直	*/
		else
			wrtyaku((int)YK.RICHI, 1);												/*	立直	*/

		if(Rultbl[(int)RL.IPPAT] != 0 && gpsPlayerWork.bFippat != 0 && (Status&(byte)ST.RINSH) == 0)
			wrthan((int)YK.IPPAT, 1);												/*	一発	*/
	}
	jhadaka();
	if((Status&(byte)ST.RINSH) != 0) {
		if (Rultbl[(int)RL.FIVE_PIN] != 0 && Sthai == 0x25)
			wrtyaku((int)YK.FIVE_PIN, 2);												/*	五筒開花	*/
		else
			wrtyaku((int)YK.RINSH, 1);												/*	嶺山開花	*/
	} else if((Status&(byte)ST.CHANK) != 0) {
		if (Rultbl[(int)RL.FIVE_PIN] != 0 && Sthai == 0x12)
			wrtyaku((int)YK.TWO_SO, 2);													/*	二索搶カン	*/
		else
			wrtyaku((int)YK.CHANK, 1);												/*	搶カン	*/
	} else if(Bpcnt+ Kancnt== MJDefine.PAI_MAX) {	//122x
		if ((Status&(byte)ST.RON) != 0)
			wrtyaku((int)YK.HOTEI, 1);												/*	河底	*/
		else {
			if (Rultbl[(int)RL.FIVE_PIN] != 0 && Sthai == 0x21)
				wrtyaku((int)YK.ONE_PIN, 2);											/*	一筒ラオ月	*/
			else
				wrtyaku((int)YK.HAITE, 1);											/*	海底	*/
		}
	}

	if(gpsPlayerWork.bFmenzen == 0 && (Status&(byte)ST.RON) == 0)
		wrtyaku((int)YK.MENZE, 1);													/*	面前清ツモ	*/

	if((n=cntdora_jy()) != 0)
		wrthan((int)YK.DORA, n);														/*	ドラ	*/
}

public void calfu(/*MahJongRally * pMe*/)
{
	byte[]	p, q;
	int		iCnt, p0= 0, q0= 0;
	int		i,f;
	byte	x;
	byte	byPushFu;
	byte[]	fbuf= new byte[5];


	gMJKResult.byFu= (byte)((Status&(byte)ST.RON) != 0 && gpsPlayerWork.bFmenzen == 0 ? 15 : 10);
	for (iCnt = 0; iCnt < 5; iCnt++) {
		sMntData.byFuData[iCnt]	=	0;
	}
	for (iCnt = 0; iCnt < 4; iCnt++) {
		sMntData.byType[iCnt]	=	MJDefine.NONE;
	}

	sMntData.byMachiMnt		=	MJDefine.NONE;								/*	待ち牌を使用する面子番号	*/
	sMntData.byMachi		=	SubMj.hikihai;							/*	待ち牌	*/
	if (gMJKResult.byFu != 10) {
		sMntData.byFuFlag	|=	(byte)MNTFLAG.MENZEN;
	}

	byPushFu	=	gMJKResult.byFu;


	p=sMntData.byMnt;
	for (iCnt = 0; iCnt < 4; iCnt++, p0++) {	//p++				/*	面子の符計算	*/
		if((x=p[p0])<0x40) {
			continue;
		}
		x&=0x3F;
		f=2;
		i=gpsPlayerWork.byFhcnt;
		q= gpsPlayerWork.byFrhai;	q0= 0;
		sMntData.byType[iCnt]	=	0;								/*	暗刻	*/
		while(i-- != 0) {
			if(x==(byte)(q[q0]&0x3F) && q[q0]>0x40){
				if(q[q0]>0xC0) {										/*	明カン				*/
					f=4;
					sMntData.byType[iCnt]	=	3;
				}
				else if(q[q0]>0x80) {									/*	暗カン				*/
					f=8;
					sMntData.byType[iCnt]	=	2;
				}
				else {												/*	明刻				*/
					f=1;
					sMntData.byType[iCnt]	=	1;
				}
				break;
			}
			else {
				q0++;		//q++;
			}
		}
		if (SubMj.hikihai == x) {
			sMntData.byMachiMnt		=	(byte)iCnt;
		}
		if(x>0x30 || (x&7)==1) {									/*	１、９、字牌		*/
			f*=2;
		}
		gMJKResult.byFu+=(byte)f;
		sMntData.byFuData[iCnt]	=	(byte)(f * 2);					/*	面子の符			*/
	}
	if(sMntData.byJanto==SubMj.hikihai) {									/*	単騎待ち			*/
		sMntData.byMachiMnt		=	4;
		gMJKResult.byFu++;
		sMntData.byFuFlag	|=	(byte)MNTFLAG.MACHI;						/*	待ち符				*/
	}
	else {
		p= gpsPlayerWork.byFrhai;	p0= 0;
		q=fbuf;	q0= 0;
		if((i=gpsPlayerWork.byFhcnt) != 0) {
			do {
				q[q0++]=p[p0++];		//q++=p++;
			} while(--i != 0);
			pai_sort(fbuf,gpsPlayerWork.byFhcnt);
		}
		q[q0]=0;		//*q=0;
		q=fbuf;		q0= 0;
		i=(int)ST.RON;
		p=sMntData.byMnt;	p0= 0;
		for (iCnt = 0; iCnt < 4; iCnt++, p0++) {	//p++			/*	待ち符計算	*/
			if((x=p[p0])>0x40 || x> SubMj.hikihai){
				if((Status&i) != 0) {
					gMJKResult.byFu--;
					if(SubMj.hikihai>0x30 || (SubMj.hikihai&7)==1) {
						gMJKResult.byFu--;
					}
				}
				break;
			}
			if(x==q[q0]) {
				q0++;		//q++;
			}
			else if(x==SubMj.hikihai){
				if ( SubMj.pinho == 0 || (x&7)!=7) {
					sMntData.byMachiMnt		=	(byte)iCnt;
					if((x&7)==7) {									/*	ペンチャン？		*/
						gMJKResult.byFu++;
						sMntData.byFuFlag	|=	(byte)MNTFLAG.MACHI;		/*	待ち符				*/
					}
					break;
				}
			}
			else if((x+=2)>SubMj.hikihai) {								/*	カンチャン？		*/
				if( SubMj.pinho == 0) {
					sMntData.byMachiMnt		=	(byte)iCnt;
					gMJKResult.byFu++;
					sMntData.byFuFlag	|=	(byte)MNTFLAG.MACHI;			/*	待ち符				*/
					break;
				}
			}
			else if(x==SubMj.hikihai){
				if( SubMj.pinho == 0 || (x&7) != 3) {
					sMntData.byMachiMnt		=	(byte)iCnt;
					if((x&7) == 3){									/*	ペンチャン？		*/
						gMJKResult.byFu++;
						sMntData.byFuFlag	|=	(byte)MNTFLAG.MACHI;		/*	待ち符				*/
						break;
					}
				}
				i=0;
			}
		}
		//  2006/01/14
		//国士無双では符なしなのでここで引っかかるので削除
		//ASSERT(sMntData.byMachiMnt != NONE);
		// ASSERT(iCnt < 4);
		//if( iCnt < 4 )	TRACE("*** ERROR ASSERT(iCnt < 4) ***");
	}
	if(SubMj.pinho != 0) {
		gMJKResult.byFu		=	byPushFu;
		sMntData.byFuFlag	=	(byte)(sMntData.byFuFlag&~(byte)MNTFLAG.MACHI);
		return;
	}

	if( (Status&(byte)ST.RON) == 0) {							/*	ツモ				*/
		gMJKResult.byFu++;
		sMntData.byFuFlag	|=	(byte)MNTFLAG.TSUMO;			/*	ツモ				*/
	}
	else {
		//> 2006/03/20 符無し状態での聴牌によるメモリ破壊を防ぐ
		if( sMntData.byMachiMnt < 5 )			//0422 xxxx
		//< 2006/03/20 符無し状態での聴牌によるメモリ破壊を防ぐ
		{
			if (sMntData.byFuData[sMntData.byMachiMnt] != 0) {
				//> 2006/03/20 符無し状態での聴牌によるメモリ破壊を防ぐ
				if( sMntData.byMachiMnt < 4 )	//0422 xxxx
				//< 2006/03/20 符無し状態での聴牌によるメモリ破壊を防ぐ
				{
					if (sMntData.byType[sMntData.byMachiMnt] == 0) {			/*	暗刻を明刻にする	*/
						sMntData.byType[sMntData.byMachiMnt]	=	1;
					}
				}
				sMntData.byFuData[sMntData.byMachiMnt]	-=	2;
				if(SubMj.hikihai>0x30 || (SubMj.hikihai&7)==1) {
					sMntData.byFuData[sMntData.byMachiMnt]	-=	2;
				}
			}
		}
	}
	if (Ff[sMntData.byJanto] != 0) {
		if (Ff[sMntData.byJanto] != 0) {
			++gMJKResult.byFu;							/*	雀頭が役牌			*/
			sMntData.byFuData[4]	=	2;				/*	ツモ				*/
		}
	}
}

public void clrytbl(/*MahJongRally * pMe*/)
{
	gMJKResult.byYakuman	=	0;
	SubMj.yhan				=	0;
	gMJKResult.byHan		=	0;
	gMJKResult.byFu			=	0;
	gMJKResult.byYakuCnt	=	0;
}

public void trfmnt(/*MahJongRally * pMe*/)
{
	byte[] p, q;
	int i, p0= 0, q0= 0;

	if((i=gpsPlayerWork.byFhcnt) != 0) {
		p=gpsPlayerWork.byFrhai;
		q=sMntData.byMnt;	q0= 3;
		do {
			if((q[q0]=p[p0])>=0x40)
				q[q0]=(byte)((q[q0]&0x3F)|0x40);
			p0++; q0--;		//p++; q--;
		} while(--i != 0);
	}
	pai_sort(sMntData.byMnt, 4);
}

public void jhoniso7(/*MahJongRally * pMe*/)		//0422	関数内
{
	byte[]	p;
	byte x;
	int		p0;

	p= SubMj.chitoibuf;	p0= 1;	x=(byte)(SubMj.chitoibuf[0]&0x30);	//p=chitoibuf+1;

	//> 2006/04/05 七対子・混一色状態で混一色が無視される
	//if(p[p0]>0x30) {
	//	wrtyaku(YK_HONIS, 3);													/*	混一色	*/
	//}
	{
		byte ishoniso_ = 0x01;
		byte headtype_ = (byte)(SubMj.chitoibuf[0]&0x30);		//headtype_ = *chitoibuf&0x30;
		for(p0= 0, p=SubMj.chitoibuf; p0< 7; p0++) {			//(p=chitoibuf; p<chitoibuf+7; p++)
			byte type_=(byte)(p[p0]&0x30);				//type_=*p&0x30;
			if(type_==0x30) {
				// 字牌だったら終了
				break;
			}
			if(headtype_!=type_) {
				// 先頭対子と違う種類の牌が出たら混一色フラグOFFして終了
				ishoniso_ = 0x00;
				break;
			}
		}
		if( ishoniso_  != 0 ) {					//0422
			wrtyaku((int)YK.HONIS, 3);
		}
	}
	//< 2006/04/05 七対子・混一色状態で混一色が無視される

//#if	1	//	Ver 1.3	BugFix	2005/12/05
	for(p0= 0, p=SubMj.chitoibuf; p0< 7; p0++) {		//(p=chitoibuf; p<chitoibuf+7; p++)
		if(p[p0]>0x30) {						//(*p>0x30)
			wrtyaku((int)YK.HONRO, 2);											/*	混老頭	*/
			break;
		}
		if((p[p0]&7)!=1){						//((*p&7)!=1)
			break;
		}
	}
//#else	//	Ver 1.3	BugFix	2005/12/05
//	for(p=chitoibuf; p<chitoibuf+7; p++)
//		if((*p&7)!=1){
//			if(*p>0x30) {
//				wrtyaku(pMe,YK_HONRO, 2);											/*	混老頭	*/
//			}
//			break;
//		}
//#endif	//	Ver 1.3	BugFix	2005/12/05
}

public int jtanyo7(/*MahJongRally * pMe*/)
{
	byte[]	p;
	int		p0= 0;
	for(p=SubMj.chitoibuf; p0< 7; p0++) {		//(p=chitoibuf; p<chitoibuf+7; p++)
		if(p[p0]>0x30 || (p[p0]&7)==1) {
			return 0;
		}
	}
	wrtyaku((int)YK.TANYO, 1);														/*	断ヤオ	*/
	return 1;
}

/************************************************
		役判定
		  ↓
       上がれる時に必ず呼ばれる
	（PLAYER COM ﾄﾞﾁﾗﾃﾞﾓ ）
	ロン上がりの時：２回
	ツモ上がりの時：１回
************************************************/
public int jchitoi(/*MahJongRally * pMe*/)
{
	if(SubMj.chitoibuf[0]>0x30){
		wrtyman((int)YK.TSUIS, 4);													/*	字一色	*/
		return 1;
	}
	if(gMJKResult.byYakuman != 0) {
		return 1;
	}
	wrtyaku((int)YK.CHITO, 2);														/*	七対子	*/
	if( ((SubMj.chitoibuf[0]^SubMj.chitoibuf[6])&0x30) == 0) {
		wrtyaku((int)YK.CHINI, 6);													/*	清一色	*/
	}
	else if( jtanyo7() == 0) {
		jhoniso7();
	}
	return 0;
}
public PLST[]	jyaku_jy_pailist= new PLST [15];//0505mt
public bool	jyaku_jy ( /*MahJongRally * pMe*/ )/*1995.4.25, 5.15*/
{
	int m;
	int hanstr, fustr;
	byte	jntstr1, mntstr1;
	byte	jntstr2, mntstr2;
//0505mt	PLST	pailist[]= new PLST [15];
	for( int i= 0; i< 15; i++)
		jyaku_jy_pailist[i].clear();	//0505mt pailist[i]= new PLST();

//#if	1	//	Ver 1.3	BugFix	2005/12/15
	int	nCheckHan	=	1;
//#endif	//	Ver 1.3	BugFix	2005/12/15

	clrytbl();
	SubMj.Paoflg=(byte)0xFF;		//-1;

	sMntData.byFuFlag	=	0;

	SubMj.hikihai= hotpai= Sthai;
	SubMj.pinho	=	0;

//#if	1	//	Ver 1.3	BugFix	2005/12/15
	mnta_mnt( );
//#endif	//	Ver 1.3	BugFix	2005/12/15

	jpinho(false);
	if(jyman_a() != 0) {
		calfu();
		return (true);
	}
	setcnt_plst();
	cntbuf[SubMj.hikihai= hotpai= Sthai]++;
	if(!mnta_mnt()){
		byte[] _hikihai= new byte [1];
		_hikihai[0]= SubMj.hikihai;
		newlst_plst(jyaku_jy_pailist);//0505mt
		if( jtnp7_jtnp(jyaku_jy_pailist, _hikihai) == 0) {//0505mt
			SubMj.hikihai= _hikihai[0];
			return (false);
		}
		SubMj.hikihai= _hikihai[0];
		if(jchitoi() != 0) {
			sMntData.byFuFlag	|=	(byte)MNTFLAG.CHITOI;
			return (true);
		}
	} else {
		trfmnt();
		jyman_b();
		if(gMJKResult.byYakuman != 0) {
			calfu();
			return (true);
		}
		m=0;
		jntstr1=sMntData.byJanto; mntstr1=sMntData.byMnt[1];
		clrytbl();
		jyaku_a();
		calfu();
		hanstr=gMJKResult.byHan;
		fustr=gMJKResult.byFu;
		mntb_mnt();
		trfmnt();
		jntstr2=sMntData.byJanto; mntstr2=sMntData.byMnt[1];
		if(sMntData.byJanto!=jntstr1 || sMntData.byMnt[1]!=mntstr1){
			clrytbl();
			jyaku_a();
			if(gMJKResult.byHan>=hanstr){
				calfu();
				if(gMJKResult.byHan>hanstr || gMJKResult.byFu>fustr) {
					m=1; hanstr=gMJKResult.byHan; fustr=gMJKResult.byFu;	}
			}
		}
		mntc_mnt();
		trfmnt();
		if(!(sMntData.byJanto==jntstr1 && sMntData.byMnt[1]==mntstr1 ||
						 sMntData.byJanto==jntstr2 && sMntData.byMnt[1]==mntstr2)){
			clrytbl();
			jyaku_a();
			if(gMJKResult.byHan>hanstr){
				calfu();
				if(gMJKResult.byHan>hanstr || gMJKResult.byFu>fustr)
					m= 2;
			}
		}
		if(m!=2){
			if(m==0)
				mnta_mnt();
			else
				mntb_mnt();

			trfmnt();
			clrytbl();
			jyaku_a();
			calfu();
		}
		jyaku_b();
	}
	jyaku_c();

//#if	1	//	Ver 1.3	BugFix	2005/12/05
	if(Ryansh){
		++nCheckHan;
	}
#if	Rule_2P
	else{
		if( Order== 0){		//プレイヤー？
			if(pRyansh){
				nCheckHan++;
			}
		}
	}
#endif

	if (sRuleSubData.byRuleNo == (byte)RULETYPE.BASHIBARI) {			/*	場縛りの時	*/
		if (gpsTableData.byRenchan >= 10)
			++nCheckHan;
		if (gpsTableData.byRenchan >= 15)
			++nCheckHan;
	}

	return (SubMj.yhan>=nCheckHan);
//#else	//	Ver 1.3	BugFix	2005/12/05
//	if(!Ryansh) {
//		yhan++;
//	}
//	if (sRuleSubData.byRuleNo == RULETYPE_BASHIBARI) {			/*	場縛りの時	*/
//		if (gpsTableData.byRenchan >= 10){
//			++yhan;
//		}
//		if (gpsTableData.byRenchan >= 15){
//			++yhan;
//		}
//	}
//	return (yhan>=2);
//#endif	//	Ver 1.3	BugFix	2005/12/05
}

/*****************************
	 流局時に流し満貫を判定
	   流局時 ４回呼ばれる
              Return    A
*****************************/
public bool jyaoch_jy ( /*MahJongRally * pMe*/ )/*1995.4.25*/
{
	byte[]	p;
	byte x;
	int		i, p0= 0;

	if( Rultbl[(byte)RL.YAOCHU] == 0 || gpsPlayerWork.bFnagas != 0)
		return (false);
	i=gpsPlayerWork.byShcnt;
	p=gpsPlayerWork.bySthai;
	do {
		if((x=p[p0++])<0x30 && (x&7)!=1)
			return (false);
	} while(--i != 0);

	gMJKResult.byYakuman		=	0;
	gMJKResult.byFu				=	40;
	gMJKResult.sYaku[0].factor	=	gMJKResult.byHan	=	4;
	gMJKResult.byYakuCnt		=	1;
	gMJKResult.sYaku[0].name	=	(byte)YK.YAOCH;									/*	ヤオ九振切	*/
	SubMj.Paoflg=(byte)0xFF;		//-1;

	return (true);
}

/************************************************/
/*		     十三不搭			*/
/************************************************/
public bool jshisa_jy ( /*MahJongRally * pMe*/ )/*1995.4.25, 5.19*/
{
	byte[] p,q;
	int i,n, p0=0, q0= 0;
	byte[] buf= new byte [14];

	if(Rultbl[(int)RL.SHISAN] == 0 || (Status&(byte)ST.RINSH) != 0) {
		return (false);
	}
	p	=	gpsPlayerWork.byTehai;
	q	=	buf;
	for(i=0; i<13; i++) {
		q[q0++]=p[p0++];		//*q++=*p++;
	}
	q[q0]= hotpai;				//*q=hotpai;
	pai_sort(buf,14);
	n=0;
	for(p0= 0, i=0,p=buf; i<13; i++, p0++) {		//(i=0,p=buf; i<13; i++,p++)
		if(p[p0]==(p[p0+1]) && n++ != 0) {			//１つの対子
			return (false);
		}
	}
	if(n==0) {
		return (false);
	}

	p=buf;	p0= 0;
	do {		//嵌張(カンチャン)Ｘ
		if(p[p0]!=(p[p0+1]) && (byte)(p[p0]+2)>=(p[p0+1])) {		//if(*p!=*(p+1) && (BYTE)(*p+2)>=*(p+1)) {
			return (false);
		}
	} while(p[++p0]<0x30);		//(*++p<0x30);
	gMJKResult.sYaku[0].factor	=	gMJKResult.byFu=40;
	gMJKResult.byYakuman			=	0;
	switch(Rultbl[(int)RL.SHISAN]){
	  case 1:
		gMJKResult.sYaku[0].factor	=	gMJKResult.byHan=4;
		gMJKResult.byYakuman			=	0;
		break;
	  case 2:
		gMJKResult.sYaku[0].factor	=	gMJKResult.byHan=8;
		gMJKResult.byYakuman			=	0;
		break;
	  case 3:
		gMJKResult.sYaku[0].factor	=	gMJKResult.byHan=4;
		gMJKResult.byYakuman			=	4;
		break;
	}
	gMJKResult.byYakuCnt			=	1;
	gMJKResult.sYaku[0].name	=	(byte)YK.SHISA;					/*	十三不搭	*/
	SubMj.Paoflg=(byte)0xFF;		//-1;
	return (true);
}

/************************************************/
/*	  	９種９牌の判定時		*/
/************************************************/
public int cntyao_jy ( /*MahJongRally * pMe*/ )/*1995.4.25*/
{
	byte[] p, q;
	int i, n, p0= 0, q0= 0;
	byte[] buf= new byte[15];

	if((Status&(byte)ST.RINSH) != 0)
		return 0;
	p	=	gpsPlayerWork.byTehai;
	q	=	buf;
	for(i=0; i<13; i++)
		q[q0++]=p[p0++];		//*q++=*p++;
	q[q0++]=hotpai;				//*q++=hotpai;
	q[q0]=0;					//*q=0;
	pai_sort(buf,14);
	n=0;
	p=buf;	p0= 0;
	while(p[p0] != 0){		//while(*p)
		if(p[p0]!=(p[p0+1]) && (p[p0]>0x30 || (p[p0]&7)==1))		//if(*p!=*(p+1) && (*p>0x30 || (*p&7)==1))
			n++;
		p0++;		//p++;
	}
	return n;
}

public void evaly_jy (/*MahJongRally * pMe,*/ int x)/*1995.4.25, 6.15, 7.28*/
{
	int yhanmax, ryhan;
	byte jntstr1, mntstr1;
	byte jntstr2, mntstr2;

	clrytbl();
	cntbuf[SubMj.hikihai=(byte)x]++;
	SubMj.Rict= SubMj.Ricr= SubMj.Hant= SubMj.Hanr= 0;
	SubMj.sananflg= SubMj.suanflg= 0;
	mnta_mnt();
	trfmnt();
	jyman_b();
	if(gMJKResult.byYakuman != 0){
		SubMj.Hant=13;
		if( SubMj.suanflg == 0 || gMJKResult.byHan>=8)
			SubMj.Ricr=SubMj.Rict=SubMj.Hanr=SubMj.Hant;
	}
	if(SubMj.Hanr==0){
		jntstr1=sMntData.byJanto; mntstr1=sMntData.byMnt[1];
		clrytbl();
		jyaku_a();
		yhanmax=SubMj.yhan;
		SubMj.Hanr=gMJKResult.byHan;
		if(SubMj.Hant<gMJKResult.byHan) {
			SubMj.Hant=gMJKResult.byHan;
		}
		if(SubMj.sananflg != 0) {
			SubMj.Hanr-=2;
		}
		mntb_mnt();
		trfmnt();
		jntstr2=sMntData.byJanto; mntstr2=sMntData.byMnt[1];
		if(sMntData.byJanto!=jntstr1 || sMntData.byMnt[1]!=mntstr1){
			clrytbl();
			jyaku_a();
			if(yhanmax<SubMj.yhan) {
				yhanmax=SubMj.yhan;
			}
			if(SubMj.Hanr<gMJKResult.byHan) {
				SubMj.Hanr=gMJKResult.byHan;
			}
			if(SubMj.Hant<gMJKResult.byHan) {
				SubMj.Hant=gMJKResult.byHan;
			}
		}
		mntc_mnt();
		trfmnt();
		if(!(sMntData.byJanto==jntstr1 && sMntData.byMnt[1]==mntstr1 ||
						 sMntData.byJanto==jntstr2 && sMntData.byMnt[1]==mntstr2)){
			clrytbl();
			jyaku_a();
			if(yhanmax<SubMj.yhan) {
				yhanmax=SubMj.yhan;
			}
			if(SubMj.Hanr<gMJKResult.byHan) {
				SubMj.Hanr=gMJKResult.byHan;
			}
			if(SubMj.Hant<gMJKResult.byHan) {
				SubMj.Hant=gMJKResult.byHan;
			}
		}
		SubMj.yhan=yhanmax;
		gMJKResult.byHan=(byte)(SubMj.Dorasum+SubMj.Doracnt[x]);
		jyaku_b();
		SubMj.Hanr+=gMJKResult.byHan;
		SubMj.Hant+=gMJKResult.byHan;
		if (gpsPlayerWork.bFmenzen == 0) {
			SubMj.Hant++;
		}
		if(!Ryansh)
			SubMj.yhan++;

		if(SubMj.Ricflg != 0){
			SubMj.Rict=(byte)(SubMj.Hant+1);
			SubMj.Ricr=(byte)(SubMj.Hanr+1);
			ryhan=SubMj.yhan+1;
			if( (Status&(byte)ST.ONE_JUN) == 0){
				ryhan++;
				SubMj.Rict++;
				SubMj.Ricr++;
			}
			if(ryhan==1) {
				SubMj.Ricr=1;
			}
		}
		if(SubMj.yhan==0) {
			SubMj.Hant=SubMj.Hanr=0;
		}
		else if(SubMj.yhan==1) {
			SubMj.Hanr=0;
			if (gpsPlayerWork.bFmenzen != 0) {
				SubMj.Hant=0;
			}
		}
	}
	cntbuf[x]--;
}

public void evaly7_jy (/*MahJongRally * pMe,*/ int x)/*1995.4.25, 6.15, 7.28*/
{
	SubMj.Rict=SubMj.Ricr=SubMj.Hant=SubMj.Hanr=0;
	clrytbl();
	if(jchitoi() != 0) {
		SubMj.Ricr=SubMj.Rict=SubMj.Hant=SubMj.Hanr=13;
	}
	else {
		gMJKResult.byHan+=(byte)(SubMj.Dorasum+SubMj.Doracnt[x]);
		SubMj.Hanr=gMJKResult.byHan;
		SubMj.Hant=(byte)(gMJKResult.byHan+1);
		if(SubMj.Ricflg != 0){
			SubMj.Rict=(byte)(SubMj.Hant+1);
			SubMj.Ricr=(byte)(SubMj.Hanr+1);
			if( (Status&(int)ST.ONE_JUN) == 0){
				SubMj.Rict++;
				SubMj.Ricr++;
			}
		}
	}
}

//-*********************Mjjy.j
}