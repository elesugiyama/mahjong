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
// mjm3.j
//-*****************
public partial class MahjongBase : SceneBase {
/*
**		麻雀大会３
**		捨牌処理
*/

//#include "MahJongRally.h"								// Module interface definitions

/*****************************
	捨て牌の表示
*****************************/
public void dtapai_m3 ( /*MahJongRally * pMe*/ )
{
	set_sutehai_rec ( (ushort) 0, (ushort)Order );		/* for PlayStation */		//://捨て牌表示用バッファへの登録
	//://disp_sutehai ( Order );				/* 捨て牌の表示 */
}

/*****************************
	捨牌内部処理
*****************************/
public void tapai_m3 ( /*MahJongRally * pMe*/ )/*1995.6.1*/
{
	bool	fRichi;
	//tamaki BYTE	bflag;

	if (( Status & ( (byte)ST.RINSH | (byte)ST.RINFR )) != 0 ) {
		disp_dora((ushort) 0 );				/* 王牌の正式表示 */
		if ( Dorakan != 0) {
			/**** new_dial(暗カン除く) ****/
			if( !AnkanToDora )
				/* カンした牌がどらになった人へ(5) */
				MjDialog(  OtherOne( Order ), Order, (byte)DIAL.KANMAKE_DORA );
			AnkanToDora = false;
		}
		Status |= (byte)ST.NAKI;
	}
	#if true //-*todo:描き方有ってる？
	Status = (byte)(Status & (~((byte)ST.RINSH | (byte)ST.RINFR)));
	#endif //-*todo:描き方有ってる？
	Sthai = gpsPlayerWork.byHkhai;
	++gpsPlayerWork.byShptr;
	gpsPlayerWork.bySthai[gpsPlayerWork.byShcnt]	=	Sthai;
	++gpsPlayerWork.byShcnt;

	decpc_pcnt ( Sthai);
	if (gpsPlayerWork.bFippat != 0) {
		/*	リーチ打牌	*/
		//://MjSoundSEPlay( SENO_RICHI );
		fRichi	=	true;
	} else{
		/*	通常打牌	*/
		//://MjSoundSEPlay( SENO_DAHAI );
		fRichi	=	false;
	}
	/* 捨て牌の実表示 */
	dtapai_m3 ();

	if ( gpsPlayerWork.bFrich == 0 ){
		gpsPlayerWork.bFfurit	=	0;
	}
	/*	聴牌チェック					*/
	chktnp_m5 ();
	//://te_prt_all( 0 );								/*	捨牌・ソート後の手牌を描く		*/

	//://手牌に表示のときにツモ牌は表示しない
	gpsPlayerWork.byTkhai = 0;

//://	if (fRichi) {									/*	リーチ打牌は聞こえる様にする	*/
//://		{
//://			int	iWait;
//://
//://			for (iWait = 0; iWait < 10; iWait++) {
//://				g_DrawAll();
//://			}
//://		}
//://	}

	newanp_pcnt ( Sthai, -1);
	/* 1996.7.5.DIALOG 現在の局の序盤･中盤･終盤のいずれかをいれる*/
	if( Bpcnt < MJDefine.EARLSTG )
		Bp_now = 0;
	else {
		if ( Bpcnt < MJDefine.LATESTG )
			Bp_now = 1;
		else
			Bp_now = 2;
	}
	/* 1996.7.5.DIALOG END */
}

/*****************************
	捨て牌の消去
*****************************/
public void etapai_m3 ( /*MahJongRally * pMe*/ )
{
	setwp_m2(Odrbuf);
	if( check_chankan_disp() == true ){		/* 1996.6.17. */
		//://te_prt_all ( 0 );
	} else{
		--gpsPlayerWork.byShptr;			/* 河に並ぶ捨て牌の数 */
		set_sutehai_rec ( (ushort) 1, (ushort)Odrbuf );		/* for PlayStation */
		//://disp_sutehai ( Odrbuf );			/* 捨て牌の消去 */
	}
}

/*****************************
	捨牌の消去＋内部処理
******************************/
public void erssh_m3 ( /*MahJongRally * pMe*/ )
{
	etapai_m3 ();		/* 消される側の牌を消す */
	setwp_m2(Order);
}

/**************************************END OF FILE**********************************************/

//-*********************mjm3.j
}
