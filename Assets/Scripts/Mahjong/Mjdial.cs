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
// mjdial.j
//-*****************
public partial class MahjongBase : SceneBase {
/*
**		麻雀大会３
**		キャラクター会話
*/

//#include "MahJongRally.h"								// Module interface definitions

/*********************************************************************
*********************************************************************/
/*********************************************************************
	会話判定処理
*********************************************************************/
/*****************************
	各局開始時に喋る時専用(番号０６～１６)
*****************************/
public void	Dialog_KyokuStart(/*MahJongRally * pMe*/)
{
#if false //-*todo:要らない気がする
	short	opnt, kpnt;
	byte	cmpMan;
	byte	mesnum;
	byte	sayman;/* 1996.8.11.DIAL_FIX */

	if(Rultbl[ (int)RL.RYANSH ] != 0)
		if(gpsTableData.byRenchan == 5)		/* リャンシ突入時(70)*/
			MjDialog(gpsTableData.byOya, MJDefine.NONE, DIAL_RYANSHI);

	if(Rultbl[ RL_PAREN ] != 0)
		if(gpsTableData.byParen == 8)		/* 八連荘(71)*/
			MjDialog(gpsTableData.byOya, NONE, DIAL_PAREN);

	if((Totsunyu_flg != 0)&&(gpsTableData.byKyoku != 0)){
		if((gpsTableData.byKyoku % 4)== 0){			/* 各一局・風が変わるとき */
			if ((gpsTableData.byFlags & TBLF_NEXTBA_VOICE) == 0) {
				/* 東東回しも出る */
				if(gpsTableData.byKyoku == 4)		/* 南入時 */
					/* 南入時のトップが(72)*/
					MjDialog(Get_Rank(0), NONE, DIAL_NANNYU);
				else if((gpsTableData.byKyoku / 4) != 0)
					/* 西入以降の場風が変わるときラスが(73)*/
					MjDialog(Get_Rank(3), NONE, DIAL_SYANYU);

				gpsTableData.byFlags	|=	TBLF_NEXTBA_VOICE;
			}
		} else if(((gpsTableData.byKyoku / 4)!= 0)&&
				((gpsTableData.byKyoku % 4)== 3)){	/* オーラスの時(東風はなし)*/
			sayman = gpsTableData.byOya;/* 1996.8.11.DIAL_FIX */
			cmpMan = Get_Rank(0);
			kpnt = get_mjpoint((USHORT)cmpMan);
			if((Rultbl[ RL_SHANY ] > 0)&&
			   (kpnt < sRuleSubData.nToplin)){	/* 西入りありでトップ目無し */
				mesnum = DIAL_ALLLAST_NOTOP;
			}
			else {
				/* 親の点数 */
				opnt = get_mjpoint((USHORT)gpsTableData.byOya);
				if(gpsTableData.sMemData[gpsTableData.byOya].byRank == 0){			/* 親がトップのとき */
					cmpMan = Get_Rank(1);
				}
				else {								/* 子がトップの時 */
					cmpMan = Get_Rank(0);
				}
				/* 比較する点数を決定 */
				kpnt = get_mjpoint((USHORT)cmpMan);
				if(gpsTableData.sMemData[gpsTableData.byOya].byRank == 0){			/* 親がトップの時 */
					if((opnt - kpnt)< 80){	/* 僅差のトップ */
						mesnum = DIAL_ALLLAST_OYASMLTOP;
					}
					else {							/* 大差のトップ */
						mesnum = DIAL_ALLLAST_OYABIGTOP;
					}
				}
				else {								/* 子がトップの時 */
					if((kpnt - opnt)< 80){	/* トップと僅差 */
						mesnum = DIAL_ALLLAST_KOSMLTOP;
					}
					else {							/* トップと大差 */
						mesnum = DIAL_ALLLAST_KOBIGTOP;
						sayman = Get_Rank(0);/* 1996.8.11.DIAL_FIX */
					}
				}
			}
			MjDialog(sayman, NONE, mesnum);
		}
	}
#endif	//-*todo:
}

/*****************************
	頭はねで喋る時専用(番号８５と８６)
*****************************/
public bool	Dialog_Headcut(/*MahJongRally * pMe*/)
{
	byte	i;
	byte	rmem;	/* 上がれる人のカウント */

	rmem = 0xff;
	/* はねられた者の面子番号を探す */
	for(i = 0; i < MJDefine.MAX_TABLE_MEMBER; i ++){		/* Roncntには上がる人間の数しか入らない */
		if((Hanetbl[ i ] != 0)&&(i != Order)){
			rmem = i;
			break;
		}
	}
	if((rmem != 0xff)&&(Rultbl[ (int)RL.TWO_CHAHO ] == 0)){		/* 複数の上がりがあって、ダブロン無し */
//		MjDialog(Order, NONE, DIAL_DABURON_ATAMA);	/* 頭はねした者が */	// 2006/02/09 No.82
//		MjDialog(rmem, NONE, DIAL_DABURON_SHIRI);		/* 頭はねされた者が */	// 2006/02/09 No.82
		return	(true);						/* 頭はねが有りました */
	}
	return	(false);							/* ここまできたら頭はねはなかったことになる */
}

/*****************************
	フーロで喋る時専用(番号０６～１６)
*****************************/
public void	Dialog_Furo(/*MahJongRally * pMe,*/ byte byIppatuClear)
{
	int	iDlgID;

	if((SubMj.sayTOpao3 == false)&&(SubMj.Pao3 != 0xff)){	//(sayTOpao3 == 0)
		/* 大三元パオに他家から一言(82)*/
//		MjDialog(OtherTwo(Order, Pao3), NONE, DIAL_BECAME_PAO);	// 2006/02/09 No.82
//		sayTOpao3 = 0xff;	// 2006/02/09 No.82
	}
	else if((SubMj.sayTOpao4 == false)&&(SubMj.Pao4 != 0xff)){	//(sayTOpao4 == 0)
		/* 大四喜パオに他家から一言(82)*/
//		MjDialog(OtherTwo(Order, Pao4), NONE, DIAL_BECAME_PAO);	// 2006/02/09 No.82
//		sayTOpao4 = 0xff;	// 2006/02/09 No.82
	}
	/* 一発を消された人がいる */
	else if (byIppatuClear != 0xFF) {
		/* 他家の一発を消すとき(まだ消えてない)(16)*/
		MjDialog(Order, MJDefine.NONE, (byte)DIAL.IPPATSU_KESHI);

		/* 17：他キャラクターにやられた */
		iDlgID	= (int)DIAL.TO_IPPATSU_KESHI_C;
		if ( Order == 0 ) {										/* 18：プレイヤーにやられた */
			iDlgID	= (int)DIAL.TO_IPPATSU_KESHI_P;
		}
		/* 一発を消されたとき(17,18) */
		MjDialog(byIppatuClear, Order, iDlgID);
	}
	else {															/* 一発の人がいない時は */
		if(gpsPlayerWork.byFhcnt == 1){
			/* 自分の第１副露(6,7,8)(PERCENT 50)*/
			MjDialog(Order, MJDefine.NONE, (int)DIAL.FIRST_FURO +(Bp_now % 3));
			/* 他人の第１副露に(9,10,11)(PERCENT 33)*/
			MjDialog(Odrbuf, MJDefine.NONE, (int)DIAL.TO_FIRST_FURO +(Bp_now % 3));
		}
		else if(gpsPlayerWork.byFhcnt == 3){
			/* 他人の第３副露(12)*/
			MjDialog(Odrbuf, MJDefine.NONE, (byte)DIAL.TO_THIRD_FURO);
		}
		else if(gpsPlayerWork.byFhcnt == 4){
			/* 自分の第４副露(14,15)*/
			MjDialog(Order, MJDefine.NONE, ((Rultbl[ (int)RL.KINCH ]) != 0 ? (int)DIAL.FOUR_FURO_KINCHI : (int)DIAL.FOUR_FURO));
			/* 他人の第４副露(13)*/
			MjDialog(Odrbuf, MJDefine.NONE, (byte)DIAL.TO_FOUR_FURO);
		}
	}
}

/*****************************
	面子番号３人の組み合わせパターンを受け取りほかの面子番号を返す
  引数			   返り値：面子番号
 (whoA+whoB+whoC)(whodial)
  0+1+2 = 3   .   MJ_POS_D(0)
  0+1+3 = 4   .   MJ_POS_C(1)
  0+2+3 = 5   .   MJ_POS_B(2)
  1+2+3 = 6   .   MJ_POS_A(3)
*****************************/
public byte	OtherTree(byte whoA, byte whoB, byte whoC)
{
	byte[]	whodial = {	(byte)MJ_POS.D,		//[ 4 ]
						(byte)MJ_POS.C,
						(byte)MJ_POS.B,
						(byte)MJ_POS.A };
	byte	pat;

	pat = (byte)(whoA + whoB + whoC - 3);	/* テーブルの数あわせ */
	return	(whodial[ pat ]);
}

/*****************************
	超簡易点数計算
	ハンと符を受け取り、得点を返す(バンバンぬき)
*****************************/
public short Check_Machi_Sub(byte h, byte f)
{
	short	i;
	short	pnt;

	i = (short)(f / 10);
	if((f % 10) != 0) {
		i++;
	}
	f = (byte)(i * 10);
	if(h < 5){								/* 倍満までしか調べません */
		if(((f >= 40 )&&(h >= 4))||
		   ((f >= 70 )&&(h >= 3))||
		   ((f >= 130)&&(h >= 2))){
			return	(500);					/* 満貫 */
		}
		else {
			pnt = (short)f;
			for(i = 0; i < h; i ++){
				pnt *= 2;
			}
			return	(pnt);
		}
	}
	else if(h < 6){
		return	(500);
	}
	else if(h < 8){
		return	(750);
	}
	else if(h < 11){
		return	(1000);
	}
	else if(h < 12){
		return	(1500);
	}
	else {
		return	(2000);
	}
}



/*****************************
	その上がりが高めか安めかを判定役満および流し満貫･十三不搭除く
 返り値 ）高め安め無し	：０
		  高めでの上がり：１
		  安めでの上がり：２
*****************************/

public byte Check_Machi_HIGHandLOW(/*MahJongRally* pMe*/)
{
	byte	result_ = 0;	// 上がりの高・安フラグ（返り値）
	byte	fu_buf;			// for PlayStation

	fu_buf = gMJKResult.byFu;

	if ( Order != (byte)MJ_POS.A ) {
		if ( gpsPlayerWork.byTenpai > 1 ) {	// 単騎待ちではない
			PLAYERWORK	wpbackup;
			short		pnt_real, pnt_if;
			byte		byHotpaiBK, bySthaiBK, mcnt, i,byHkhaiBK;

			// 実際に上がった点数をキープ
			pnt_real = Check_Machi_Sub(gMJKResult.byHan, gMJKResult.byFu);

			// 役をバックアップ
			{
				wpbackup= (PLAYERWORK)gpsPlayerWork.clone();	//MEMCPY(wpbackup, gpsPlayerWork, sizeof(wpbackup));
				byHotpaiBK	=	hotpai;
				bySthaiBK	=	Sthai;
				byHkhaiBK = gpsPlayerWork.byHkhai;
			}

			// 待ちの数だけループする
			mcnt = gpsPlayerWork.byTenpai;
			for ( i = 0 ; i < mcnt ; i++ ) {
				// 上がり牌決定
				gpsPlayerWork.byHkhai	=	gpsPlayerWork.byMchai[i];
				hotpai = Sthai			=	gpsPlayerWork.byHkhai;
				if ( jyaku_jy() ) {
					// 上がれるなら
					gMJKResult.byFu = (byte)(((gMJKResult.byFu + 4) / 5) * 10);
					if ( gMJKResult.byYakuman == 0 )
						pnt_if = Check_Machi_Sub(gMJKResult.byHan, gMJKResult.byFu);
					else
						pnt_if = (byte)(2000 * gMJKResult.byYakuman / 4);

					if ( pnt_if < pnt_real ) {
						// 安めがある
						if ( (pnt_real - pnt_if) > 125 ) {
							// 高め上がり
							result_ = 1;
						}
						break;
					} else
					if ( pnt_if > pnt_real ) {
						// 高めがある
						if ( (pnt_if - pnt_real) > 125 ) {
							// 安め上がり
							result_ = 2;
						}
						break;
					}
				}
			}

			// 元の役に戻す
			{
				hotpai	=	byHotpaiBK;
				Sthai	=	bySthaiBK;
				gpsPlayerWork.byHkhai	= byHkhaiBK;
				gpsPlayerWork= (PLAYERWORK)wpbackup.clone();	//MEMCPY(gpsPlayerWork, wpbackup, sizeof(wpbackup));
				byHkhaiBK = gpsPlayerWork.byHkhai;

				jyaku_jy();
			}
		}
	}

	gMJKResult.byFu = fu_buf;


	return result_;
}



/*****************************
	役番号を受け取り上がりにその役があるかどうかを返す
*****************************/
public bool Check_Yakunumber(int ynum) {	return Check_Yakunumber((byte)ynum);	}
public bool Check_Yakunumber(/*MahJongRally * pMe,*/ byte ynum)
{
	for(int i = 0; i < gMJKResult.byYakuCnt; i++)
		if(gMJKResult.sYaku[i].name == ynum)
			return	(true);

	return	(false);
}

/*****************************
	上がりの結果で喋る時専用(37～61)
*****************************/
public void Dialog_Result(/*MahJongRally * pMe*/)
{
	short	pnt = 0;
	byte	dnum = 0;

	pnt =(short)((Total_point - gpsTableData.byRenchan * sRuleSubData.byRenchanRate * 30)/ 10);	/* 1996.7.15.MENU */
	/*** 役満の上がりの時 ***/
	if((gMJKResult.byYakuman != 0)||(gMJKResult.byHan > 12)){
		if((Status & (byte)ST.RON)!= 0){		/* ロン上がりのとき */
			if((SubMj.Paoflg != 0xff)&&(SubMj.Paoflg != Odrbuf)){		/* パオの者がいてほかの者が振ったとき */
				/* パオに他家から一言 */
				MjDialog(OtherTree(Order, Odrbuf, SubMj.Paoflg), MJDefine.NONE, (byte)DIAL.RON_PAO);
			}
			else {								/* 普通に役満をロン上がりしたとき */
				/* 役満を上がった者が(発声)*/
				MjDialog(Order, MJDefine.NONE, (byte)DIAL.TSUMO_YAKUMAN);
				/* 役満放銃の者が */
				MjDialog(Odrbuf, MJDefine.NONE, (byte)DIAL.HOUJU_YAKUMAN);
				/* 関係ない面子が */
				MjDialog(OtherTwo(Order, Odrbuf), MJDefine.NONE, (byte)DIAL.RON_YAKUMAN);
			}
		}
		else {									/* ツモあがり */
			if(SubMj.Paoflg != 0xff){				/* パオの者がいてその対象者がツモったとき */
				/* パオに他家から一言 */
				MjDialog(OtherTwo(Order, SubMj.Paoflg), MJDefine.NONE, (byte)DIAL.TSUMO_PAO);
			}
			else {								/* 普通に役満をロン上がりしたとき */
				if(Check_Yakunumber((int)YK.TENHO)){		/* 天和なら */
					/* 天和をツモった者が */
					MjDialog(Order, MJDefine.NONE, (byte)DIAL.TSUMO_TENCHIHO);
				}
				else if(Check_Yakunumber((int)YK.CHIHO)){	/* 地和なら */
					/* 地和をツモった者が */
					MjDialog(Order, MJDefine.NONE, (byte)DIAL.TSUMO_TENCHIHO);
				}
				else{
					/* 役満をツモった者が(発声)*/
					MjDialog(Order, MJDefine.NONE, (byte)DIAL.TSUMO_YAKUMAN);
				}
				/* ほかの面子が */
				MjDialog(OtherOne(Order), MJDefine.NONE, (byte)DIAL.TAKE_TSUMO_YAKUMAN);
			}
		}
	}
	/*** 役満以外の上がりで１６０００点以上の時 ***/
	else if(pnt >= 160){
		/* 発声 */
		MjDialog(Order, MJDefine.NONE, (byte)DIAL.TSUMO_BIG);
		if((Status & (byte)ST.RINFR)!= 0){					/* 大明カンの責任払い */
			/* 責任払いのキャラが */
			MjDialog(Odrbuf, MJDefine.NONE, (byte)DIAL.DAIMINKAN);
			/* 他家のでかい手ロンに(PERCENT 50)*/
			MjDialog(OtherTwo(Order, Odrbuf), MJDefine.NONE, (byte)DIAL.RON_BIG);
		}
		else if((Status & (byte)ST.RON)!= 0){				/* ロン上がりのとき */
			/* 他家のでかい手に振って */
			MjDialog(Odrbuf, MJDefine.NONE, (byte)DIAL.HOUJU_BIG);
			/* 他家のでかい手ロンに(PERCENT 50)*/
			MjDialog(OtherTwo(Order, Odrbuf), MJDefine.NONE, (byte)DIAL.RON_BIG);
		}
		else {												/* ツモ上がりの時 */
			/* 他家のでかい手ツモに */
			MjDialog(OtherOne(Order), MJDefine.NONE, (byte)DIAL.TAKE_TSUMO_BIG);
		}
	}
	/*** 流し満貫成立の時 ***/
	else if(Check_Yakunumber((int)YK.YAOCH)){
		/* 成立した者が */
//		MjDialog(Order, NONE, DIAL_NAGASHIMAN);	// 2006/02/09 No.82
		/* 他家から */
//		MjDialog(OtherOne(Order), NONE, DIAL_TO_NAGASHIMAN);	// 2006/02/09 No.82
	}
	/*** 十三不搭成立の時 ***/
	else if(Check_Yakunumber((int)YK.SHISA)){
		/* 成立した者が */
		MjDialog(Order, MJDefine.NONE, (byte)DIAL.TSUMO_SHISAN);
		/* 他家のでかい手ツモに */
		MjDialog(OtherOne(Order), MJDefine.NONE, (byte)DIAL.TAKE_TSUMO_BIG);
	}
	/*** 満貫以上の時 ***/
	else if(SubMj.Mangan != 0){
		Dialog_Headcut();
		if((Status & (byte)ST.RINFR)!= 0){					/* 大明カンの責任払い */
			/* 責任払いのキャラが */
			MjDialog(Odrbuf, MJDefine.NONE, (byte)DIAL.DAIMINKAN);
			/* 他家のでかい手ロンに(PERCENT 50)*/
			MjDialog(OtherTwo(Order, Odrbuf), MJDefine.NONE, (byte)DIAL.RON_BIG);
		}
		else {
			dnum = Check_Machi_HIGHandLOW();
			if(dnum != 0){
				/* 上がった者が */
				MjDialog(Order, MJDefine.NONE, ((dnum == 1)? (int)DIAL.TAKAME : (int)DIAL.YASUME));
			}
			if((Status & (byte)ST.RON)!= 0){				/* ロン上がりのとき */
				/* 他家のでかい手に振って */
				MjDialog(Odrbuf, MJDefine.NONE, (byte)DIAL.HOUJU_BIG);
				/* 他家のでかい手ロンに(PERCENT 50)*/
				MjDialog(OtherTwo(Order, Odrbuf), MJDefine.NONE, (byte)DIAL.RON_BIG);
			}
			else {											/* ツモ上がりの時 */
				/* 他家のでかい手ツモに */
				MjDialog(OtherOne(Order), MJDefine.NONE, (byte)DIAL.TAKE_TSUMO_BIG);
			}
		}
	}
	/*** それ以外の上がり ***/
	else if((Status & (byte)ST.RON)== 0){					/* ツモ上がりの時 */
		if((Status & (byte)ST.RINSH)!= 0){
			/* リンシャン上がりのとき */
			dnum = (int)DIAL.TSUMO_RINSHAN;
		}
		else if(Check_Yakunumber((int)YK.HAITE)){
			/* ハイテイで上がった時 */
			dnum = (int)DIAL.HAIHOUTEI;
		}
		else if(Check_Yakunumber((int)YK.IPPAT)){
			/* 一発で上がった時 */
			dnum = (int)DIAL.TSUMO_IPPATSU;
		}
		else if((dnum = Check_Machi_HIGHandLOW())!= 0){
			/* 高めあるいは安めで上がった時 */
			dnum =(byte)(((dnum == 1) ? (int)DIAL.TAKAME : (int)DIAL.YASUME));
		}
		else if(pnt >= 77){
			/* 16000未満7700以上 */
			dnum = (int)DIAL.TSUMO_MIDBIG;
		}
		else if(pnt >= 20){
			/* 7700未満2000以上 */
			dnum = (int)DIAL.TSUMO_MIDSMALL;
		}
		else {
			/* むちゃむちゃ安い時 */
			dnum = (int)DIAL.TSUMO_SMALL;
		}
		/* 上がった者が */
		MjDialog(Order, MJDefine.NONE, dnum);

		if((Status & (byte)ST.RINFR)!= 0){						/* 大明カンの責任払い */
			/* 責任払いのキャラが */
			MjDialog(Odrbuf, MJDefine.NONE, (byte)DIAL.DAIMINKAN);
		}
	}
	else if((Status & (byte)ST.RON)!= 0){							/* ロン上がりのとき */
		if(Dialog_Headcut()== false){						/*頭はねがなければ */
			/* 1996.8.11.DIAL_FIX */
			if(Check_Yakunumber((int)YK.HOTEI)){
				/* 河底上がりのとき */
				MjDialog(Order, MJDefine.NONE, (byte)DIAL.HAIHOUTEI);
			} else {
				dnum = Check_Machi_HIGHandLOW();
				if(dnum != 0)
					/* 上がった者が */
					MjDialog(Order, MJDefine.NONE, ((dnum == 1)? (int)DIAL.TAKAME : (int)DIAL.YASUME));
			}

			/* 振った時の振った側の台詞 */
			if(Check_Yakunumber((int)YK.HOTEI)){					/* ホーテイで振る */
				dnum = (int)DIAL.HOUJU_HOUTEI;
			} else if((Status & (byte)ST.CHANK)!= 0){
				dnum = (int)DIAL.HOUJU_CHANKAN;
			}
//#if 1	/*	宣言牌では言わない	*/
			else if (((gsPlayerWork[Order].bFrich & gsPlayerWork[Odrbuf].bFrich) != 0)
							&& (gsPlayerWork[Odrbuf].bFippat == 0))
//#else
//			else if((gsPlayerWork[Order].bFrich &
//						gsPlayerWork[Odrbuf].bFrich  )!= 0)
//#endif
			{												/* リーチ合戦で振る */
				dnum = (int)DIAL.HOUJU_REACHBATTLE;
			} else if(Check_Dora(Sthai)) {
				dnum = (int)DIAL.HOUJU_DORA;
			} else if (Rultbl[ (int)RL.DOBON ] != 0 && gpsTableData.sMemData[Odrbuf].nPoint < 0) {
				dnum	=	(int)DIAL.DOBON;
			} else if(gMJKResult.byHan >= 2) {
				dnum = (int)DIAL.HOUJU_MIDDLE;
			} else {
				dnum = (int)DIAL.HOUJU_SMALL;
			}
			MjDialog(Odrbuf, MJDefine.NONE, dnum);
		}
	}
}

/*********************************************************************
	判定補佐処理
*********************************************************************/
/*****************************
	面子番号を受け取りランダムでそれ以外のＣＯＭ面子番号を返す
*****************************/
public byte OtherOne(byte whose)
{
	/* ＣＯＭ面子の喋る相手順に 下家・対面・上家 */
	byte[][]	whodial = new byte[][]{
			new byte[]{ (byte)MJ_POS.C, (byte)MJ_POS.D },		//[ 3 ][ 2 ]
			new byte[]{ (byte)MJ_POS.D, (byte)MJ_POS.B },
			new byte[]{ (byte)MJ_POS.B, (byte)MJ_POS.C }
	};

	if(whose == (byte)MJ_POS.A){			/* ﾌﾟﾚｲﾔｰならほかの面子 */
		//return	(mj_rand()% 3 + 1);
		return((byte)(mj_getrand(3)+1));
	}
	else {/* COMなら */
		//return(whodial[ whose - 1 ][ mj_rand()% 2 ]);
		return(whodial[ whose - 1 ][ mj_getrand(2) ]);
	}
}

/*****************************
	面子番号２人の組み合わせパターンを受け取りほかのＣＯＭ面子番号を返す
	(各数値は面子番号です)
	引数			 返り値
   (whoA+whoB)	  (whodial)
	0 + 1(-1)	   2 or 3(.2)
	0 + 2(-1)	   1 or 3(.3)
	0 + 3(-1)	   1 or 2(.1)
	1 + 2			 3 only
	1 + 3			 2 only
	2 + 3			 1 only
*****************************/
public byte OtherTwo(byte whoA, byte whoB)
{
	byte[]	whodial = {		//[ 6 ]
		(byte)MJ_POS.C, (byte)MJ_POS.D, (byte)MJ_POS.B, (byte)MJ_POS.D, (byte)MJ_POS.C, (byte)MJ_POS.B
	};
	byte	pat;

	pat = (byte)(whoA + whoB);
	if((whoA == (byte)MJ_POS.A)||
		(whoB == (byte)MJ_POS.A)){	/* どちらかにﾌﾟﾚｲﾔｰが含まれるなら */
		pat --;							/* テーブルの数あわせ */
	}
	return	(whodial[ pat ]);
}

/*****************************
	牌がドラかどうかチェック
*****************************/
public bool Check_Dora(/*MahJongRally * pMe,*/ byte code)
{
	short	i;

	/* 1996.8.11.DIAL_FIX */
	/* カンドラなし */
	if(Rultbl[ (int)RL.KAN ] == 0){
		if(Dora[ 0 ] == code){
			return	(true);
		}
		else {
			return	(false);
		}
	}
	/* カンドラ有り(表ドラの数だけチェック)*/
	for(i = 0; i < Kancnt + 1; i ++){
		if(Dora[ i ] == code){
			return	(true);
		}
	}
	return	(false);
}

/*****************************
	順位で面子番号を返す
*****************************/
public byte Get_Rank(int rnum) {	return Get_Rank((byte)rnum);	}
public byte	Get_Rank(/*MahJongRally * pMe,*/ byte rnum)
{
	byte	pos;

	for(pos = 0; pos < MJDefine.MAX_TABLE_MEMBER; pos ++){
		if(gpsTableData.sMemData[pos].byRank == rnum){
			return	(pos);
		}
	}
	return	0;
}

/***************************END OF MJDIAL.C************************************/
//-*********************mjdial.j
}
