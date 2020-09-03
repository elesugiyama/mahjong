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
// Mjthink.j
//-*****************
public partial class MahjongBase : SceneBase {
/*
**		麻雀大会３
**		ＣＰＵアルゴメイン
*/

//#include "MahJongRally.h"								// Module interface definitions

#if	_MONITOR_CODE2
int	_DebugThink	=	0;
#endif

//extern PRIVATE	BOOL j4kan_m4 (MahJongRally * pMe);

/************************************************/
/*		９種９牌の思考			*/
/************************************************/
public bool think_9shu9hai ( /*MahJongRally * pMe*/ )/*CODE_00008: 1995.4.26, 6.28*/
{
	int n;

	n = cntyao_jy();
	if(n<9 || n>=10 && gpsTableData.psParam.chParaYaku==0 && Order!=gpsTableData.byOya) {
		return (false);
	}
	return (true);
}

/************************************************/
/*		   チャカンの思考に関係		*/
/************************************************/
public bool chakan_think ( /*MahJongRally * pMe*/ )/*CODE_00014: 1995.6.29*/
{
	int i;
	int	iMax;

	iMax	=	gpsPlayerWork.byFhcnt;						//://プレイヤーのフーロ牌の数
	for(i=0; i<iMax; i++) {
		if(gpsPlayerWork.byFrhai[i] == (Sthai|0x40)) {		//://捨て牌と同じ牌をポンしているか
#if	_MONITOR_CODE2
			if (_DebugThink == 2) {
				return (true);
			}
#endif
			if(( Richi == 0 || gpsTableData.psParam.chParaKan>=4) && (gpsTableData.psParam.chParaKan>=1 && SubMj.tmax != 0 || gpsTableData.psParam.chParaKan>=2)) {
				return (true);
			}
			else {
				return (false);
			}
		}
	}
	return (false);
}

/************************************************/
/*		暗カンの思考に関係		*/
/************************************************/
static PLAYERWORK ankan_think_pailist_sBuf;
public PLST[]	ankan_think_pailist= new PLST [15];
public bool ankan_think ( /*MahJongRally * pMe*/ )/*CODE_00011:*/
{
//	PLAYERWORK ankan_think_pailist_sBuf;//	PLAYERWORK sBuf;
//0505mt	PLST	pailist[]= new PLST [15];
	for( int i= 0; i< 15; i++)	ankan_think_pailist[i].clear(); //0505mt pailist[i]= new PLST();
	if(cntbuf[Sthai]!=4) {
		return (false);
	}

#if	_MONITOR_CODE2
	if (_DebugThink == 2) {
		return (true);
	}
#endif
	if(SubMj.tmax != 0){
		ankan_think_pailist_sBuf.copy( (PLAYERWORK)gsPlayerWork[Order] );//ankan_think_pailist_sBuf= (PLAYERWORK)gsPlayerWork[Order].clone();	//MEMCPY(sBuf, gsPlayerWork[Order], sizeof(sBuf));
		cntbuf[Sthai]=SubMj.Paicnt[Sthai]=0;
		gpsPlayerWork.byFrhai[gpsPlayerWork.byFhcnt]	=	(byte)(Sthai|0x80);
		gpsPlayerWork.byFhcnt++;
		gpsPlayerWork.byThcnt	-=	3;
		gpsPlayerWork.bFmenzen	=	0xFF;
		newlst_plst( ankan_think_pailist);//0505mt
		initppr_pcnt();
		evltnp_etnp( ankan_think_pailist);//0505mt
		gsPlayerWork[Order].copy( (PLAYERWORK)ankan_think_pailist_sBuf );//gsPlayerWork[Order]= (PLAYERWORK)ankan_think_pailist_sBuf.clone();	//MEMCPY(gsPlayerWork[Order], sBuf, sizeof(sBuf));
		SubMj.Paicnt[Sthai]=4;
		if((SubMj.Tnpval>=SubMj.tmax || SubMj.Ricval>=SubMj.tmax) && (Richi != 0 && SubMj.riskval[Sthai] != 0 || gpsTableData.psParam.chParaKan>=1)) {
			return (true);
		}
		else {
			return (false);
		}
	}
	if(gpsTableData.psParam.chParaKan>=3 && ( Richi == 0 || gpsTableData.psParam.chParaKan>=4)) {
		if(Ff[Sthai] != 0) {
			return (true);
		}
		else if(gpsTableData.psParam.chParaKan>=5){
			if(Sthai>0x30) {
				return (true);
			}
			if((Sthai&0x0F)>=3 && cntbuf[Sthai-2] != 0) {
				return (false);
			}
			if((Sthai&0x0F)>=2 && cntbuf[Sthai-1] != 0) {
				return (false);
			}
			if((Sthai&0x0F)<=8 && cntbuf[Sthai+1] != 0) {
				return (false);
			}
			if((Sthai&0x0F)<=7 && cntbuf[Sthai+2] != 0) {
				return (false);
			}
			return (true);
		}
	}
	return (false);
}

/************************************************/
/*			 通常思考			*/
/*		Sthai <- 捨て牌			*/
/************************************************/
public PLST[] think_think_pailist= new PLST [15];
public int think_think ( /*MahJongRally * pMe*/ )/*1995.6.23*2*/
{
	//BYTE	st0;
//0505mt	PLST	pailist[]= new PLST [15];
	int		iThcnt;
	bool	fTnp;
#if	__comEnfeeble	//COM思考の弱体化
	int		NokoriPai= MJDefine.PAI_MAX- (Bpcnt+ Kancnt);	// 残り牌取得

	#if DEBUG
	Debug.Log("//-*com think_think NokoriPai("+NokoriPai+") = "+MJDefine.PAI_MAX+" - (Bpcnt("+Bpcnt+") + Kancnt("+Kancnt+"))["+(Bpcnt+ Kancnt)+"]" );
	#endif
	if((NokoriPai>= tumoPassMin) && (NokoriPai<= tumoPassMax)) {
	// #if DEBUG
	// Debug.Log("com think_think pass: "+ NokoriPai);
	// #endif
		return ((int )OP.TAPAI);
	} else {
	// #if DEBUG
	// Debug.Log("com think_think true: "+ NokoriPai);
	// #endif
	}
#endif

	for( int i= 0; i< 15; i++)
		think_think_pailist[i].clear();	//0505mt pailist[i]= new PLST();
#if	_MONITOR_CODE2
	if (_DebugThink == 1)
		return (OP_TAPAI);			/*	暗カンしない	*/
#endif
	if (( Status & (byte)ST.NAKI ) == 0 ) {/*	ナキフラグは立っているか*/
		if ( gpsPlayerWork.byTenpai != 0 ) {/*	テンパイしている*/
			if (chkmoh_m5()) {		/*		待ち牌との照合*/
				/*	→上がりである	*/
				if (jyaku_jy())		/*	役判定*/
					return (int)(OP.TSUMO);
			}
		}
		if (( Status & (byte)( ST.ONE_JUN | ST.RINSH )) == 0 )	/*	１順目である */
			if ( Rultbl[ (int)RL.SHISAN ] != 0 )
				if (jshisa_jy())		/* 十三不搭チェック */
					return (int)(OP.TSUMO);

		if ( gpsPlayerWork.bFrich != 0 ) {	/*	リーチしている	*/
			if(j4kan_m4()) {			//カン４つチェック
				Sthai = gpsPlayerWork.byHkhai;
				if (chkrk_m4())				/*	リーチ後の暗カン判定*/
					return (int)(OP.KAN);		/*	する*/
			}
			return (int)(OP.TAPAI);			/*	暗カンしない	*/
		}
	}

	setcnt_plst ();						/*	wpの指す手牌の牌カウントを行う*/
	cntbuf[gpsPlayerWork.byHkhai]++;	/*	現時点で捨てられた牌,又はツモった牌*/
	calcpcs_pcnt();

	/*	Not１巡目 or リンシャン → つまりは１巡目で９種９牌の権利はあるか*/
	if (( Status & (byte)( ST.ONE_JUN | ST.RINSH )) == 0 )
		if (think_9shu9hai())			/*	９種９牌のチェック		*/
			return (byte)(OP.TAOPAI);

	/** 手役チェックのための初期化 **/
	initey_etnp();
	newlst_plst ( think_think_pailist);	//0505mt

	/** 手役チェック **/
	chkyaku_ey ( think_think_pailist);	//0505mt

	/** リーチ出来る環境のチェック **/
	SubMj.Ricflg = (byte)(jrichi_m4() ? 1 : 0);

	/** 捨て牌決定 **/
	thinksub( think_think_pailist);		//0505mt
//#ifdef DEBUG
//	Debug.out("CPU-"+ Integer.toString(Order)+	" tsumo: "+ Integer.toString(gpsPlayerWork.byHkhai)+	", hai:"+ Integer.toString(Bpcnt + Kancnt)	);	//ITRACE(("CPU-%d tsumo:%02x,hai:%3d",Order,gpsPlayerWork.byHkhai, Bpcnt + Kancnt));
//#endif
	//TRACE("%02x,%02x,%02x,%02x,%02x,%02x,%02x,%02x,%02x,%02x,%02x,%02x,%02x,%02x,%02x\n",

	Sthai	=	(byte)SubMj.sp.paicode;
	hotpai	=	(byte)SubMj.sp.paicode;

	if (( Status & (byte)ST.NAKI ) == 0 )
		if (!(Rultbl[(int)RL.NAGARE] != 0 && Kancnt >= 4) && ((Bpcnt + Kancnt) < 122) ) {
			if (ankan_think())
				return (int)(OP.KAN);
			else
				if (chakan_think())
					return (int)(OP.CHANKAN);
		}

	if (SubMj.result == (int)OP.RICHI ) {
		WplayTOWbuf();
		Csr = Thcnt;
		if ( Sthai != gpsPlayerWork.byHkhai) {
//#if 0 // > 2006/02/04 No.704
//			iThcnt = Thcnt;
//			while ( 1 ) {
//				iThcnt--;
//				if ( Sthai == gpsPlayerWork.byTehai[iThcnt]) {
//					Csr = iThcnt;
//					gpsPlayerWork.byTehai[Csr]	=	gpsPlayerWork.byHkhai;
//					gpsPlayerWork.byHkhai		=	Sthai;
//					thsort_m2 ();
//					break;
//				}
//			}
//#else
			for ( iThcnt = Thcnt ; --iThcnt >= 0; ) {
				if ( Sthai == gpsPlayerWork.byTehai[iThcnt]) {
					Csr = (byte)iThcnt;
					gpsPlayerWork.byTehai[Csr]	=	gpsPlayerWork.byHkhai;
					gpsPlayerWork.byHkhai		=	Sthai;
					thsort_m2 ();
					break;
				}
			}
			if ( iThcnt < 0 ) {
				#if true //-*todo:
				DebLog(("No.704: think_think()"));
				#endif //-*todo:
				while ( true );
			}
//#endif // < 2006/02/04 No.704
		}
		fTnp	=	chktnp_m5 ();
		WbufTOWplay();
		if (!fTnp)
			return (int)(OP.TAPAI);
		else
			return (int)(OP.RICHI);
	}
	return (int)(OP.TAPAI);
}

/****************************************/
/*	   フーロの思考						*/
/****************************************/
public int getish(/*MahJongRally * pMe,*/ int val)
{
	return (int)((long)val*SubMj.kish/256);
}

public int getrisk(/*MahJongRally * pMe,*/ int x, int n)
{
	return SubMj.riskval[x]/(4+n/16);
}

public int	checkfuro(/*MahJongRally * pMe,*/ byte furo)
{
	int flag=0;
	int han=SubMj.gYaku.fanpai.h;

	if(SubMj.gYaku.fanpai.p != 0 && (gpsPlayerWork.bFmenzen != 0 || han>=gpsTableData.psParam.chParaFanpaih ||
			Ff[furo&0x3F] != 0 && (han>=gpsTableData.psParam.chParaFanpai1 ||
			SubMj.pc[hotpai]==0 && han>=gpsTableData.psParam.chParaFanpai2)))
		flag=1;
	if(SubMj.gYaku.honiso.p>=3){
		if(hotpai>0x30 || (hotpai&0x30)==SubMj.gYaku.honsort){
			flag=1;
			han+=SubMj.gYaku.honiso.h;
		}
		else if(SubMj.gYaku.honiso.p==4) {
			return 0;
		}
	}
	if(SubMj.gYaku.toitoi.p>=3){
		if(furo>=0x40){
			flag=1;
			han+=SubMj.gYaku.toitoi.h;
		} else if(SubMj.gYaku.toitoi.p==4)
			return 0;
	}
	if(!Ryansh && Rultbl[(int)RL.KUITA] != 0 && SubMj.gYaku.tanyo.p>=3)
		if(hotpai<0x30 && (furo&7)!=1 && (furo&0x4F)!=7){
			flag=1;
			han+=SubMj.gYaku.tanyo.h;
		} else if(SubMj.gYaku.tanyo.p==4)
			return 0;
	if(SubMj.gYaku.shosan>=2)
		han+=2;
	return (flag != 0 ? han : 0);
}
public PLST[] simfuro_pailist = new PLST [15];
public bool simfuro(/*MahJongRally * pMe,*/ byte furo)
{
	int h;
//0505mt	PLST pailist[]= new PLST [15];
	for( int i= 0; i< 15; i++)	simfuro_pailist[i].clear();//0505mt pailist[i]= new PLST();
	sTmpPlayerWork.copy( (PLAYERWORK)gsPlayerWork[Order] );//sTmpPlayerWork= (PLAYERWORK)gsPlayerWork[Order].clone();	//MEMCPY(sTmpPlayerWork, gsPlayerWork[Order], sizeof(sTmpPlayerWork));
	gpsPlayerWork.byFrhai[gpsPlayerWork.byFhcnt]	=	furo;
	++gpsPlayerWork.byFhcnt;
	gpsPlayerWork.byThcnt	-=	3;
	gpsPlayerWork.bFmenzen	=	0xFF;
	SubMj.Dorasum+=SubMj.Doracnt[hotpai];

	newlst_plst( simfuro_pailist);//0505mt
	thinksub( simfuro_pailist);//0505mt
	gsPlayerWork[Order].copy( (PLAYERWORK)sTmpPlayerWork );//gsPlayerWork[Order]= (PLAYERWORK)sTmpPlayerWork.clone();	//MEMCPY(gsPlayerWork[Order], sTmpPlayerWork, sizeof(sTmpPlayerWork));
	SubMj.Dorasum-=SubMj.Doracnt[hotpai];
	h=checkfuro( furo);
	if(SubMj.tmax<SubMj.tmax1 || SubMj.tmax==SubMj.tmax1 && (SubMj.imax<SubMj.imax1 || SubMj.imax==SubMj.imax1 && (SubMj.vmax<=SubMj.vmax1 || h == 0)) || hotpai==SubMj.sp.paicode)
		return (false);
	SubMj.tmax1=SubMj.tmax; SubMj.imax1=SubMj.imax; SubMj.vmax1=SubMj.vmax;
	SubMj.tnpmaxval1=SubMj.Tnpmax;
	Sthai=(byte)SubMj.sp.paicode;
	SubMj.furohan=h;
	SubMj.janto=SubMj.jantoflag;
	return (true);
}
static PLAYERWORK tminkan_sBuf;
public PLST[] tminkan_pailist= new PLST [15];//0505mt
public byte tminkan(/*MahJongRally * pMe,*/ int ponflg)
{
	int		curval;
//0505mt	PLST	pailist[]= new PLST [15];
//	PLAYERWORK	tminkan_sBuf;//	PLAYERWORK	sBuf;
	for( int i= 0; i< 15; i++)	tminkan_pailist[i].clear();//0505mt pailist[i]= new PLST();
	if(Rultbl[(int)RL.NAGARE] == 0 && Kancnt>=4)
		return (byte)(OP.NONE);

	if(ponflg!=2 || gpsTableData.psParam.chParaKan == 0)
		return (byte)(OP.NONE);

	SubMj.Ricflg	=	(byte)(jrichi_m4() ? 1 : 0);
	newlst_plst( tminkan_pailist);//0505mt
	initppr_pcnt();
	evltnp_etnp( tminkan_pailist);//0505mt
	if((curval=SubMj.Tnpval)==0){
		if(gpsPlayerWork.bFmenzen != 0 && hotpai>=0x30 &&
		(gpsTableData.psParam.chParaKan>=4 || gpsTableData.psParam.chParaKan>=3 && Ff[hotpai] != 0))
			return (byte)(OP.KAN);

		return (byte)(OP.NONE);
	}
	tminkan_sBuf.copy( (PLAYERWORK)gsPlayerWork[Order]);//tminkan_sBuf= (PLAYERWORK)gsPlayerWork[Order].clone();		//MEMCPY(sBuf, gsPlayerWork[Order], sizeof(sBuf));
	cntbuf[hotpai]=SubMj.Paicnt[hotpai]=0;

	gpsPlayerWork.byFrhai[gpsPlayerWork.byFhcnt]	=	(byte)(hotpai | 0xC0);
	gpsPlayerWork.byFhcnt++;
	gpsPlayerWork.byThcnt	-=	3;
	gpsPlayerWork.bFmenzen	=	0xFF;
	SubMj.Dorasum+=SubMj.Doracnt[hotpai];
	SubMj.Ricflg	=	0;
	newlst_plst( tminkan_pailist);//0505mt
	initppr_pcnt();
	evltnp_etnp( tminkan_pailist);//0505mt
	gsPlayerWork[Order].copy( (PLAYERWORK)tminkan_sBuf );// 0529mt gsPlayerWork[Order] = (PLAYERWORK)tminkan_sBuf.clone();		//MEMCPY(gsPlayerWork[Order], sBuf, sizeof(sBuf));
	SubMj.Paicnt[hotpai]=3;
	return((byte)((curval<=SubMj.Tnpval) ? OP.KAN : OP.NONE));
//#if 0
//	if(curval<=ten_tnpval)
//		act.type=ACT_KAN;
//	return act;
//#endif
}
public PLST[] tfuro_think_pailist= new PLST [15];//0505mt
public byte	tfuro_think (/*MahJongRally * pMe,*/ int iPon, int iChi)
{
	byte	one, two;
	int		j, curval;
//0505mt	PLST	pailist[]= new PLST [15];
	for( int i= 0; i< 15; i++)
		tfuro_think_pailist[i].clear();//0505mt pailist[i]= new PLST();
	int[] k={4,7,11,19,32,54,90,152,255};	//[9]
//	PRIVATE int k[9]={4,7,11,19,32,54,90,152,255};

#if	__comEnfeeble	//COM思考の弱体化
	int		NokoriPai= MJDefine.PAI_MAX- (Bpcnt+ Kancnt);	// 残り牌取得

	if((NokoriPai>= nakiPassMin) && (NokoriPai<= nakiPassMax)) {
	#if DEBUG
		Debug.Log("com tfuro_think pass: "+ NokoriPai);
	#endif
		return (byte)(OP.NONE);
	} else {
	#if DEBUG
	Debug.Log("com tfuro_think true: "+ NokoriPai);
	#endif
	}
#endif

#if	_MONITOR_CODE2
	if (_DebugThink == 1)
		return (FALSE);			/*	暗カンしない	*/
#endif

#if _DEVELOP
	// -- for debug COM鳴かない
//	return FALSE;
#endif

//	hotpai	=	Sthai;
	setcnt_plst();
	calcpcs_pcnt();
	initey_etnp();
	if(iPon == 2 && hotpai >=0x30)
		return (tminkan( iPon));

	SubMj.tmax1=SubMj.imax1=SubMj.vmax1=0;
	SubMj.furono=-1;

	newlst_plst( tfuro_think_pailist);//0505mt
	SubMj.pc[hotpai]++;
	chkyaku_ey( tfuro_think_pailist);//0505mt
	if(SubMj.gYaku.kokushi != 0)
		return (byte)(OP.NONE);

	SubMj.pc[hotpai]--;
	SubMj.Ricflg	=0;
/* --------------- PON --------------- */
	if(iPon != 0){
		cntbuf[hotpai]-=2;
		SubMj.Paicnt[hotpai]-=2;
		if(simfuro( (byte)(hotpai|0x40))) {
			SubMj.furono=3;
		}
		cntbuf[hotpai]+=2;
		SubMj.Paicnt[hotpai]+=2;
	}
/* --------------- CHI --------------- */
	if(iChi != 0) {
		for(j=0; j<3; j++) {
			if (Chipsf[j] != 0) {
				one = Chips1[j];
				two = Chips2[j];
				cntbuf[gpsPlayerWork.byTehai[one]]--;
				SubMj.Paicnt[gpsPlayerWork.byTehai[one]]--;
				cntbuf[gpsPlayerWork.byTehai[two]]--;
				SubMj.Paicnt[gpsPlayerWork.byTehai[two]]--;
				if(simfuro( (byte)(hotpai+j-2))) {
					SubMj.furono=j;
				}
				cntbuf[gpsPlayerWork.byTehai[one]]++;
				SubMj.Paicnt[gpsPlayerWork.byTehai[one]]++;
				cntbuf[gpsPlayerWork.byTehai[two]]++;
				SubMj.Paicnt[gpsPlayerWork.byTehai[two]]++;
			}
		}
	}
/* -------------- PASS -------------- */
	if(SubMj.furono<0)
		return (tminkan( iPon));

	newlst_plst( tfuro_think_pailist);//0505mt
	SubMj.Ricflg	=	(byte)(jrichi_m4() ? 1 : 0);
	initppr_pcnt();
	evltnp_etnp( tfuro_think_pailist);//0505mt				//	evltnp_etnp(pailist);
	SubMj.Tnpmax	=	SubMj.tnpmaxval1;
	curval=SubMj.Tnpval;
	evlish_etnp( true);
/* -------------- JUDGE -------------- */
	j=0;
	if(curval != 0 || SubMj.tmax1 != 0) {
		if(SubMj.tmax1>curval+getish( SubMj.Ishval)+k[gpsTableData.psParam.chParaFurotnp+4])
			j=1;
	} else if(SubMj.Ishnum>=SubMj.ishnec || SubMj.imax1 != 0){
		curval=4*k[gpsTableData.psParam.chParaFuroish+4];
		if(hotpai<0x30 && (hotpai&7)!=1)
			curval*=2;
		if((SubMj.janto != 0 || Ff[hotpai] != 0) && SubMj.imax1>SubMj.Ishval+k[gpsTableData.psParam.chParaFurotnp+4] && (gpsPlayerWork.bFmenzen != 0 || SubMj.imax1>=curval))
			j=1;
	} else if(Ff[hotpai] != 0){
		if(SubMj.furohan>=SubMj.Hannec)
			j=1;
		else
			return (byte)(OP.NONE);
	} else if(SubMj.janto != 0){
		curval=SubMj.Ishval/4+2*(evlval_eval( tfuro_think_pailist)+170+gpsTableData.psParam.chParaFuroval*4);//0505mt
		if(SubMj.vmax1>curval && SubMj.furohan>=SubMj.Hannec)
			j=1;
	}
	if( j == 0)
		return (tminkan( iPon));

	return ((byte)((SubMj.furono==3) ? OP.PON : OP.CHI));
}

/************************************************/
/*		  （初期化関連）		*/
/************************************************/
public void thinksub( /*MahJongRally * pMe,*/ PLST[] pailist )/*1995.6.23, 8.1*/
{
	int		v;
	PAIVAL[]	p;	int		p0= 0;
	PLST[]	q;	int		q0= 0;

	SubMj.tmax=SubMj.imax=SubMj.result=0; SubMj.vmax=-20000;
	for( int i= 0; i< 15; i++)	SubMj.gPaival[i].clear();//0508mt = new PAIVAL();		//MEMSET(gPaival, 0, sizeof(gPaival));
	for(p=SubMj.gPaival, q=pailist; (v=q[q0].min) != 0; q0++) {		//q++
		do {
			while(cntbuf[v]==0) {
				v++;
			}
			p[p0].paicode=v;
			++p0;		//++p;
		} while(++v<=q[q0].max);
	}

	initppr_pcnt();
	ttnp_etnp( pailist);
	if(SubMj.Tleft != 0)
		tish_etnp( pailist);
	if(SubMj.Tnpmax != 0){
		if( SubMj.Tleft == 0 && SubMj.notflg[Order] != 0) {
			for(p0= 0, p=SubMj.gPaival; p[p0].paicode != 0; p0++) {	//p++
				p[p0].tnpval= p[p0].tnpval != 0 ? 0 : 1024;
			}
		}
		for(p0= 0, p=SubMj.gPaival; p[p0].paicode != 0; p0++) {		//p++
			if((v=p[p0].tnpval+getish( p[p0].ishval)-getrisk( p[p0].paicode, p[p0].tnpnum))>SubMj.tmax) {
				SubMj.tmax=v;
				SubMj.sp=p[p0];
				SubMj.result=(int)OP.TAPAI;
			}
			if(SubMj.Ricflg != 0 && (v=p[p0].ricval-getrisk( p[p0].paicode, p[p0].ricnum))>SubMj.tmax) {
				SubMj.tmax=v;
				SubMj.sp=p[p0];
				SubMj.result= (int)OP.RICHI;
			}
		}
		if(SubMj.tmax != 0)
			return;
	}
	if(SubMj.Tleft != 0 || gpsTableData.psParam.chParaOri<=-5)
		tval_eval( pailist);

	if(gpsTableData.psParam.chParaTako != 0) {
		for(p0= 0, p=SubMj.gPaival; p[p0].paicode != 0; p0++) {	//p++
			p[p0].val+= mj_getrand((ushort)gpsTableData.psParam.chParaTako);
		}
	}
	for(p0= 0, p=SubMj.gPaival; p[p0].paicode != 0; p0++)		//p++
		if((v=p[p0].ishval-2*getrisk( p[p0].paicode, p[p0].ishnum)+SubMj.safeval[p[p0].paicode])>SubMj.imax || SubMj.imax>0 && v==SubMj.imax && p[p0].val>SubMj.sp.val) {
			SubMj.imax=v; SubMj.sp=p[p0];	}
	if( SubMj.imax == 0 || SubMj.sp.ishnum<SubMj.ishnec){
		SubMj.imax=0;
		for(p0= 0, p=SubMj.gPaival; p[p0].paicode != 0; p0++)		//p++
		{
			if((v=p[p0].ishval/4+p[p0].val*2-getrisk( p[p0].paicode, p[p0].ishnum))>SubMj.vmax) {
				SubMj.vmax=v;
				SubMj.sp=p[p0];
			}
		}
	}
}

/************************************************/
/*		Wbuf <-	Wplay			*/
/************************************************/
public void WplayTOWbuf ( /*MahJongRally * pMe*/ )/*CODE_00049: 1995.4.27, 6.29*/
{
//	sTmpPlayerWork= (PLAYERWORK)gpsPlayerWork.clone();	//MEMCPY(sTmpPlayerWork, gpsPlayerWork, sizeof(sTmpPlayerWork));
	sTmpPlayerWork.copy((PLAYERWORK)gpsPlayerWork);
}

/************************************************/
/*		Wbuf . Wplay			*/
/************************************************/
public void WbufTOWplay ( /*MahJongRally * pMe*/ )/*CODE_00050: 1995.4.27, 6.29*/
{
//	gpsPlayerWork= (PLAYERWORK)sTmpPlayerWork.clone();	//MEMCPY(gpsPlayerWork, sTmpPlayerWork, sizeof(sTmpPlayerWork));
	gpsPlayerWork.copy((PLAYERWORK)sTmpPlayerWork);
	Thcnt = gpsPlayerWork.byThcnt;
}

/**************************************END OF FILE**********************************************/

//-*********************Mjthink.j
}
