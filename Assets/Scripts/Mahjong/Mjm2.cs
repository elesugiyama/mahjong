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
// mjm2.j
//-*****************
public partial class MahjongBase : SceneBase {
/*
**		麻雀大会３
**		初期処理
*/

//#include "MahJongRally.h"								// Module interface definitions

/*****************************
	牌をツモる処理
*****************************/
/*	修正	*/
public void	shuhai_m2 ( /*MahJongRally * pMe*/ )	/*1995.5.8, 6.1*/
{
	//SHORT	bflag;
	int		iNo;

	/*** 1996.4.24.修正 BUGFIX ***/
	if( gpsPlayerWork.bFrich == 0 )
		gpsPlayerWork.bFfurit	=	0;
	Status &= (byte)~ST.NAKI;
	if (( Status & (byte)ST.RINSH ) != 0 ) {		/* 嶺山のツモ */
		iNo = 136 - Kancnt;
		iNo ^= 0x01;
	} else {								/* 通常 */
		iNo = Bpcnt;
		Bpcnt++;		//ツモ牌 + 1
		#if false //-*todo;萌日記には不要
		//*****ウキウキ:表情操作****
		if(faceChangeCnt > 0){
			faceChangeCnt--;
			if(faceChangeCnt <= 0){
				FaceChange();
			}
		}
		//**************************
		#endif //-*todo;萌日記には不要
	}
	gpsPlayerWork.byHkhai	=	Bipai[iNo];
	hotpai	=	Sthai	=	gpsPlayerWork.byHkhai;
	gpsPlayerWork.byTkhai = 0xFF;			//://手牌に表示のときにツモ牌も表示する
}

/*****************************
	手牌ポインターのセット
*****************************/
/*	修正	*/
public void	setwp_m2 (/*MahJongRally * pMe,*/ int iorder)/*1995.6.16*/
{
	/* アクティブプレーヤーポインタ変更 */
	gpsPlayerWork			=	gsPlayerWork[iorder];		//&gsPlayerWork[iorder];
	gpsTableData.psParam	=	gpsPlayerWork.sParamData;	//&gpsPlayerWork.sParamData;

	/* 手牌数設定 */
	Thcnt					=	gpsPlayerWork.byThcnt;

	Ff[0x31]= Ff[0x32]= Ff[0x33]= Ff[0x34]= 0;

	if ( Rultbl[ (int)RL.KAZE ] == 1 ) {
		/* 東南回しのとき */
		//> 2006/03/26
		Ff[  0x31 + (( gpsTableData.byKyoku / 4 ) & 3) ] = 1;
		//> 2006/03/26
	} else
		Ff[ 0x31 ]++;

#if	Rule_2P
	if( gpsTableData.byOya== Order)
		Ff[0x31]++;		//東
	else
		Ff[0x33]++;		//西
#else
	Ff[0x31+((Order-gpsTableData.byOya)&3)]++;
#endif

//	TRACE("player_work[%d]:addr=0x%08x,thcnt=%d", iorder, &gsPlayerWork[iorder],Thcnt );
}

/*****************************
	それぞれ手牌のソート
	  wp の指す手牌
*****************************/
public void	pai_sort(byte[] p, int n)		//xxxx	*p
{
	byte[]	q;
	byte	x;
	int		i, q0= 0;

	while(--n>0) {
		i	=	n;
		q	=	p;	q0= 0;
		while(i-- != 0) {
			if(q[q0]>q[q0+1]) {		//(*q>*(q+1))
				x=q[q0]; q[q0]=q[q0+1]; q[q0+1]=x;
			}
			q0++;		//*q++;
		}
	}
}

public void	thsort_m2 ( /*MahJongRally * pMe*/ )
{
	pai_sort(gpsPlayerWork.byTehai, Thcnt);
}

//-----------------------------------------------------------------------------
// 乱数取得
//-----------------------------------------------------------------------------
public int BrewSystem_Random()
{
	#if true //-*todo:
	// int		rnd= _random(0,1000);		//0
	int		rnd= UnityEngine.Random.Range(0,1000);		//0
	#endif //-*todo:
	return(rnd);
}

/*****************************
	乱数計算
*****************************/
public ushort mj_getrand(int uMax) {	return mj_getrand((ushort)uMax);	}
public ushort mj_getrand(ushort uMax)
{
	ushort	rand;
	ushort	ret;

	if (uMax == 0)
		return (0);

	rand = (ushort)BrewSystem_Random();

	ret = (ushort)(rand % uMax);

	return (ret);
}

/*****************************
	牌のかき混ぜ
	  0 ~ 135 の乱数 R を 136 回発生させ
	  Bipai[i]とBipai[R]を入れ替える
*****************************/
public void	shipai_m2 ( /*MahJongRally * pMe*/ )
{
	int		i, j, cnt;
	byte	x;

#if false //-*todo:
#if DEBUG
	_rand= myCanvas.haipai_num;
#endif
#endif //-*todo:

	for(i=0; i<MJDefine.MAX_PAI_KIND; i++)		//34
		Bipai[i]= Bipai[i+34]= Bipai[i+34*2]= Bipai[i+34*3]= Paitbl[i];

#if true //-*todo:
	// _MEMSET(cntbuf, 0, sizeof( cntbuf));
	_MEMSET(cntbuf, 0, cntbuf.Length);
#endif
	for(i=0; i<MJDefine.MAX_PAI_KIND; i++)
		cntbuf[Paitbl[i]]	=	4;

	for(cnt=0; cnt<10; cnt++)
		for(i=0; i<136; i++) {
			j			=	mj_getrand(136);
			x			=	Bipai[i];
			Bipai[i]	=	Bipai[j];
			Bipai[j]	=	x;
		}

	Dicnum[0]	=	(byte)mj_getrand(6);
	Dicnum[1]	=	(byte)mj_getrand(6);
//-*SUGI_DEB***************************
#if SUGI_DEB //-*todo:注デバッグ中
	if(m_DebBox != null && !m_DebBox.IsSameTumikomi()){
		m_DebBox.SetTumikomiSameKyoku(Bipai);
	}
#endif //-*todo:注デバッグ中
//-****************************SUGI_DEB

}

/****************************
	表示ドラのネクストを決定
****************************/
public byte	svdoranext ( byte hyouji_hai )
{
	switch ( hyouji_hai ) {
		case 0x37:
			return	0x35;
//			break;
		case 0x34:
			return	0x31;
//			break;
		default:
			if (( hyouji_hai & 0x0f ) == 9 ) {
				hyouji_hai -= 8;
				return hyouji_hai;
			}
			else {
				hyouji_hai++;
				return	hyouji_hai;
			}
	}
//	return	1;
}

/*****************************
	ドラ→ワークへ
*****************************/
public void	svdora_m2 ( /*MahJongRally * pMe*/ )
{
	short	i;

	for ( i = 0; i < 5; i++ ) {
		Hyouji_ura[i] = Bipai[131 - i * 2];
		Hyouji_dora[i] = Bipai[130 - i * 2];
		Ura[i] = svdoranext ( Hyouji_ura[i] );
		Dora[i] = svdoranext ( Hyouji_dora[i] );
	}
}

//-*SUGI_DEB***************************
#if SUGI_DEB //-*todo:注デバッグ中	
// #if __LOGIC_CHECK
/**
 * @brief 積み込み
 */
public void tsumikomi( /*MahJongRally * pMe*/ )
{
//-*SUGI_DEB***************************
#if SUGI_DEB //-*todo:注デバッグ中
	var tumiData = MJDefine.tumiData;
	if(m_DebBox != null && m_DebBox.GetTumikomiNo() >= 0){
		tsumikomiNum = m_DebBox.GetTumikomiNo();
		if(m_DebBox.IsSameTumikomi()){
		//-*TUMIKOMI_PLUS.SAME_KYOKU
			byte[] sameLine = m_DebBox.GetTumikomiSameKyoku();
			for( int i= 0; i< MJDefine.HAI_MAX_NUM; i++){
				Bipai[i]= sameLine[i];
			}
		}else if(tsumikomiNum < tumiData.Length){
			for( int i= 0; i< MJDefine.HAI_MAX_NUM; i++){
				Bipai[i]= tumiData[tsumikomiNum][i];
			}
		}
		Dicnum[0]= Dicnum[1]= 0;
	}
#else //-*todo:注デバッグ中
	#if true//-*todo:要るかな？
// 	char tumiData[][] = {
// #include "../../YamaData.j"
// 	};
	var tumiData = MJDefine.tumiData;

	if( tumiData.Length<= tsumikomiNum) {
		Debug.Log("積み込みデータエラー:"+tumiData.Length);
		return;
	}
	if( tumiData[tsumikomiNum].Length!= MJDefine.HAI_MAX_NUM) {
		Debug.Log("積み込みデータエラー 136:"+tumiData[tsumikomiNum].Length);
		return;
	}
	if( gpsTableData.byKyoku!= 0)			return;		//局数
	if( gpsTableData.byDrawRenchan!= 0)		return;		//何本場か
	#endif //-*todo:

	for( int i= 0; i< MJDefine.HAI_MAX_NUM; i++){
		Bipai[i]= tumiData[tsumikomiNum][i];
	}

	Dicnum[0]= Dicnum[1]= 0;
#endif //-*todo:注デバッグ中
//-****************************SUGI_DEB
}
// #endif
#endif //-*todo:注デバッグ中
//-****************************SUGI_DEB

/*****************************
	配牌
*****************************/
public void	haipai_m2 ( /*MahJongRally * pMe*/ )
{
	int		i;

	//DebLog(( "HAIPAI: 0x%04x:0x%04x:0x%04x", haipai_m2_bflag, haipai_m2_iBipai, haipai_m2_iOrder ));
#if false //-*todo
	if( myCanvas.effect_mode!= NOMAL_MODE)		//フェードイン中
		return;
#endif //-*todo
	switch(haipai_m2_bflag){
		case	0:
			haipai_m2_iBipai	=	0;
			haipai_m2_iOrder	=	gpsTableData.byOya;
			/*******	親から順に各家に４枚ずつ配る	*******/
			haipai_m2_bflag = 1;
			SubMj.mj_wait= MJDefine.__startWait00;
			break;
		case	1:
			// 牌パイ準備
			Order	=	haipai_m2_iOrder;
			setwp_m2(Order);
			haipai_m2_iThNow	=	Thcnt;
			Thcnt	= gpsPlayerWork.byThcnt;
			Thcnt	+= 4;
			gpsPlayerWork.byThcnt	=	Thcnt;
			haipai_m2_bflag = 2;
			#if true //-*todo:
			goto case 2;
			#endif //-*todo:
		case	2:
			for ( ; haipai_m2_iThNow < gpsPlayerWork.byTehai.Length ; haipai_m2_iThNow++ ) {	//*gpsPlayerWork.byTehai
				gpsPlayerWork.byTehai[haipai_m2_iThNow]	=	Bipai[( byte )haipai_m2_iBipai];

				haipai_m2_iBipai++;
				if ( (haipai_m2_iThNow + 1) >= Thcnt ) {
					//://MjSoundSEPlay( SENO_DAHAI );
					//://te_prt_all( 0 );	/* ４個づつの手牌表示 */
					gpsPlayerWork.byTkhai = 0;			//://手牌に表示のときにツモ牌は表示しない
					//://MjFixWait(1);
					haipai_m2_iOrder++;
					haipai_m2_iOrder &= 3;
					haipai_m2_bflag = 1;
					if ( haipai_m2_iBipai >= 12 * 4 ) {
						haipai_m2_bflag = 3;
						haipai_m2_iOrder = gpsTableData.byOya;
					}
					break;
				}
			}
#if DEBUG
			if ( haipai_m2_iThNow >= gpsPlayerWork.byTehai.Length ) {	//*gpsPlayerWork.byTehai
				#if true //-*todo:
				DebLog(("No.704: haipai_m2()"));
				#endif
				while ( true );
			}
#endif

			#if true //-*todo:描画関連は保留
			paintF= true;		//配牌時
			#endif //-*todo:描画関連は保留

			break;

		case	3:
			for( i= 0; i< 4; i++) {		//配牌処理の高速化 (4回分を１度に処理する)
				/*******	チョンチョン ちょんちょん		*******/
				Order = haipai_m2_iOrder;
				setwp_m2(Order);
				Thcnt = gpsPlayerWork.byThcnt;
				gpsPlayerWork.byTehai[Thcnt]	= Bipai[haipai_m2_iBipai];
				Thcnt++;
				gpsPlayerWork.byThcnt	=	Thcnt;
				haipai_m2_iBipai++;
				//://MjSoundSEPlay( SENO_DAHAI );
				//://te_prt_all( 0 );	/* ちょんちょんの手牌表示 */
				//://MjFixWait(1);
				haipai_m2_iOrder++;
				haipai_m2_iOrder &= 3;
				if ( haipai_m2_iBipai >= 13 * 4 ) {
#if	Rule_2P
					Bpcnt = BpcntKep = (byte)(haipai_m2_iBipai+ 26);	/*m*/
#else
					Bpcnt = haipai_m2_iBipai;
#endif
					haipai_m2_bflag = 4;
#if	Rule_2P
#else
					disp_dora ((USHORT) 0 );	/* 牌パイ終了後ドラオープン */
#endif
					haipai_m2_iOrder	=	gpsTableData.byOya;
					break;
				}
			}
			#if true //-*todo:描画関連は保留
			paintF= true;		//配牌時
			#endif //-*todo:描画関連は保留
			break;

		case	4:
			for( i= 0; i< 4; i++) {		//配牌処理の高速化
				/*	配り終わったらソートしてテンパイチェックを行う	*/
				Order	=	haipai_m2_iOrder;
				setwp_m2(Order);

				thsort_m2();
				chktnp_m5();

				haipai_m2_iOrder++;
				haipai_m2_iOrder &= 3;
				if ( haipai_m2_iOrder == gpsTableData.byOya ) {
#if	Rule_2P
					disp_dora ((ushort) 0 );	/* 牌パイ終了後ドラオープン */
#endif
					Order = gpsTableData.byOya;
					setwp_m2(Order);
					haipai_m2_bflag = 0xff;		//牌パイ終了
					break;
				}
			}
			break;
	}
}

/*****************************
	各ワークのイニット
*****************************/
//	リャンハン縛りチェック
public void	intwrk_m2e()
{
#if	Rule_2P
	//リャンハン以上縛り
	pRyansh = false;
	#if true //-*todo 
	// if( myCanvas.mah_limit_num== MAH_LIM03)
	if( m_keepData.mah_limit_num == (int)MAH.LIM03)
		pRyansh = true;
	#endif
#endif
}

public void	intwrk_m2 ( /*MahJongRally * pMe*/ )
{
	short	i;

	gpsTableData.byOya = (byte)(( gpsTableData.byKyoku + gpsTableData.byChicha) & 3);
	_MEMSET(Ff, 0, Ff.Length);
	Ff[0x35]	=	Ff[0x36]	=	Ff[0x37]	=	1;		/*	三元牌の役フラグｦセットする	*/

	SubMj.Pao3 = 0xFF;
	SubMj.Pao4 = 0xFF;
	SubMj.sayTOpao3 = false;		//0;	/* 1996.7.5.DIALOG */
	SubMj.sayTOpao4 = false;		//0;	/* 1996.7.5.DIALOG */
	AnkanToDora = false;			/* 暗カンしたらドラになったフラグ */
	SubMj.Wareme = 0xff;
	/* Haneflg = 0xff; */
	Pinch = (byte)SP.AGARI;
	Kancnt = 0;
	Status = 0;
	Richi = 0;
	SubMj.Lastric = 0;
	Ryansh = false;

	/* 1996.6.11.RULE_FIX 二ﾊﾝ縛り(無･有)の修正 */
	if ( Rultbl[ (int)RL.RYANSH ] != 0 )
		if ( gpsTableData.byRenchan >= 5 )		/*リャンハン縛りチェック*/
			Ryansh = true;

	intwrk_m2e();					//リャンハン縛りチェック

	/* 1996.6.11.RULE_FIX 二ﾊﾝ縛り(無･有)の修正 END */
	if ( Rultbl[(int)RL.WAREM] != 0 )	/*ワレメ決定*/
		SubMj.Wareme = (byte)(( gpsTableData.byOya + Dicnum[0] + Dicnum[1] + 1 ) & 3);

	gpsTableData.byDrawRenchan	=	gpsTableData.byRenchan;
	for ( i = 0; i < MJDefine.MAX_TABLE_MEMBER; i++ ) {
		gsPlayerWork[i].byThcnt			=	0;
		gsPlayerWork[i].byFhcnt			=	0;
		gsPlayerWork[i].byShcnt			=	0;
		gsPlayerWork[i].byShptr			=	0;

		gsPlayerWork[i].bFfurit			=	0;
		gsPlayerWork[i].bFrich			=	0;
		gsPlayerWork[i].bFwrich			=	0;
		gsPlayerWork[i].bFippat			=	0;
		gsPlayerWork[i].byIppatuClear	=	0;
		gsPlayerWork[i].bFmenzen		=	0;
		gsPlayerWork[i].bFnagas			=	0;

		gsPlayerWork[i].bFkokus			=	0;
		gsPlayerWork[i].bFsthai			=	0;
		gsPlayerWork[i].byTenpai		=	0;
		gsPlayerWork[i].byKancnt		=	0;
		gsPlayerWork[i].byStatus		=	0;
		gsPlayerWork[i].byApflg			=	0;
		gsPlayerWork[i].byKflg			=	0;

		gpsTableData.sMemData[i].nMovePoint	=	gpsTableData.sMemData[i].nOldPoint	=	0;
		gpsTableData.sMemData[i].nMoveChip	=	gpsTableData.sMemData[i].nOldChip	=	0;
		KyokuKekka[ i ] = (byte)KEKKA.KEKKA_NONE;
	}
}

public void	_resetResult(/*MahJongRally * pMe*/)
{
	for (int i = 0; i < MJDefine.MAX_TABLE_MEMBER; i++) {
		gMJKResult.sMemResult[i].wFlag	=	0;
		gMJKResult.sMemResult[i].iPoint	=	0;
	}
}

/*****************************
	局のイニット
*****************************/
public void	chipai_m2 ( /*MahJongRally * pMe*/ )
{
#if	__comEnfeeble	//COM思考の弱体化
	int[][] comEData = new int[][]{
		new int[]{	38,		24		},		//１戦目　：　38～24
		new int[]{	36,		26		},		//２戦目　：　36～26
		new int[]{	36,		28		},		//３戦目　：　36～28
		new int[]{	34,		24		},		//４戦目　：　34～24
		new int[]{	34,		28		},		//５戦目　：　34～28
		new int[]{	38,		28		},		//６戦目　：　38～28
		new int[]{	50,		50		},		//７戦目　：　無し
	};
#endif

	intwrk_m2 ();
	svdora_m2 ();

	Bpcnt = 0;

	_resetResult();
	//://MjFixWait(2);					//://ここのウェイトは必要あるのかな？
//	MjSoundSEPlay( SEMNO_F_GLASS );
	disp_dora ((ushort) 3 );			/* 配牌前の王牌表示 */		//:// 3 = 開始直後（何も開いてない）

	Dorakan		=	0;
	byDoraCnt	=	1;
	if (Rultbl[(int)RL.URA] != 0)
		byUraDoraCnt	=	1;
	else
		byUraDoraCnt	=	0;

	//://MjFixWait(5);

	//://disp_tableinfo(TRUE);			//://起家マークやダイスの表示

#if	__comEnfeeble	//COM思考の弱体化
	tumoPassMax=						//ツモ切り 開始残り牌数
	tumoPassMin= 50;					//ツモ切り 終了残り牌数
	if( m_keepData.level_cpu!= -1)		// バトル数
		if( comEData.Length> m_keepData.level_cpu) {
			tumoPassMax= comEData[m_keepData.level_cpu][0];
			tumoPassMin= comEData[m_keepData.level_cpu][1];
		}

#if DEBUG
	Debug.Log("COM tumoPass Max: "+ tumoPassMax+ ", Min:"+ tumoPassMin+ ", battle #"+ m_keepData.num_battle);
#endif
#endif

#if	_MONITOR_CODE2
	RenhouDebug();
#endif
}

//-*********************mjm2.j
}
