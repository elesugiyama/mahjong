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
// MJ_Rule.j
//-*****************
public partial class MahjongBase : SceneBase {

	/****************************************************************/
	/*		ルール設定初期化処理									*/
	/****************************************************************/
	/*	IN)		nothing												*/
	/*	OUT)	nothing												*/
	/****************************************************************/
	public void RuleInit(/*MahJongRally * pMe*/)
	{
#if false //-*todo
		menu_csr = 0;
		MenuSelClr(TRUE);
		menu_sel[menu_csr] = MJDefine.MENU_SEL;
#endif
	}

	/****************************************************************/
	/*		ルール設定カーソル移動									*/
	/****************************************************************/
	/*	IN)		nothing												*/
	/*	OUT)	nothing												*/
	/****************************************************************/
	public byte RuleMain(/*MahJongRally * pMe*/)
	{

		byte[,] csr_move_dat = new byte[,]{	//[9][4]
	//			U,	D,	L,	R
			{	6,	2,	1,	1,	},		// 0:
			{	7,	3,	0,	0,	},		// 1:
			{	0,	4,	3,	3,	},		// 2:
			{	1,	5,	2,	2,	},		// 3:
			{	2,	6,	5,	5,	},		// 4:
			{	3,	7,	4,	4,	},		// 5:
			{	4,	0,	7,	7,	},		// 6:
			{	5,	1,	6,	6,	},		// 7:
			{	7,	0,	8,	8,	},		// 8:
		};

		byte	move = 0xFF;
		int		i;
#if false //-*todo:
		if( MJDefine.D_ONEPUSH_INPUT_UP )
		{
			move = 0;
		}
		else if( D_ONEPUSH_INPUT_DOWN )
		{
			move = 1;
		}
		else if( D_ONEPUSH_INPUT_LEFT )
		{
			move = 2;
		}
		else if( D_ONEPUSH_INPUT_RIGHT )
		{
			move = 3;
		}
		if(move != 0xFF)
		{
			SeOnPlay( SE_ATACK, SEQ_SE);
			menu_csr = (UINT2)csr_move_dat[menu_csr][move];

			MenuSelClr(FALSE);
			menu_sel[menu_csr] = MENU_SEL;
		}
		for( i = 0; i < sizeof(menu_sel); i++ )
			if( menu_sel[i] == MENU_TOUCH )
				menu_sel[i] = MENU_SEL;

		//ソフトキー１で戻る
		if(D_ONEPUSH_INPUT_SOFT1)
			menu_csr = RSEL_MAX;
#endif //-*todo

		for( i = MJDefine.RSEL_MAX; i >= 0; i--) {
			#if false //-*todo:必要そう
			if( (D_ONEPUSH_INPUT_SELECT && i==menu_csr) || (D_ONEPUSH_INPUT_SOFT1 && i==menu_csr) ) {
			#endif //-*todo:
	#if	Rule_2P
	#else
				MenuKetteiSe();
	#endif
				menu_csr = (short)i;

				if(i==MJDefine.RSEL_MAX){
					//COMメニュー初期化へ移行
					#if false //-*todo:必要そう
					return(D_COM_MENU);
					#else
					return 1;		//-*とりあえず
					#endif //-*todo:
				} else {
					i = (int)menu_csr;
				#if false //-*todo:
					MenuSelClr(FALSE);
					menu_sel[i] = MENU_KETTEI;
					select_rule = (byte)i;
				#endif //-*todo
					//> 2006/03/06 要望 128 サバイバル戦を行うとフリー対戦で行ったルール設定が変更される。
					if( select_rule == (byte)RULETYPE.FREE ) {
						int	j;		//uint8_t	i;
						for ( j = 0 ; j < (int)RL.RLSIZE ; j++ ) {		//for ( i = 0 ; i < RLSIZE ; i++ )
							#if false //-*todo:
							gbyUserRule[j] = f_info.f_rule[j];	// 設定値
							#endif //-*todo:

						}
						checkUserRule();		//0422
					} else {
						// 他は初期化
						InitUserRule();
					}

					//< 2006/03/06 要望 128

					//ルール編集初期化へ移行
					//m_Mode = D_FREE_RULE_EDIT_INIT_MODE;
					#if false //-*todo:必要そう
					return(D_FREE_RULE_EDIT_MODE);
					#else
					return 1;		//-*とりあえず
					#endif //-*todo:

				}
			#if false //-*todo:必要そう
			}
			#endif //-*todo:
		}
		#if false //-*todo:必要そう
		return(D_FREE_RULE_MODE);
		#else
		return 1;		//-*とりあえず
		#endif //-*todo:
	}

	/****************************************************************/
	/*		ルール編集画面初期化									*/
	/****************************************************************/
	/*	IN)		nothing												*/
	/*	OUT)	nothing												*/
	/****************************************************************/
	public void RuleEditInit(/*MahJongRally * pMe*/)
	{
		#if false //-*todo
		menu_csr = 0;
		MenuSelClr(TRUE);

		menu_sel[menu_csr] = MENU_SEL;
		#endif //-*todo
		rule_edit_page = 0;

		RuleValInit((byte)select_rule);
		RuleValSet(true);
		#if false //-*todo
		RuleCsrInit();
		#endif //-*todo
	}
	/****************************************************************/
	/*		ルール編集画面処理										*/
	/****************************************************************/
	/*	IN)		nothing												*/
	/*	OUT)	nothing												*/
	/****************************************************************/
	public byte RuleEditMain(/*MahJongRally * pMe*/)
	{
		return 0;

#if false //-*todo:多分不要
		byte	mode=D_FREE_RULE_EDIT_MODE;

		switch(RuleEditMove(true))
		{
			case	1:
				//戻る
				next_f_cnt = 16;

				//モードをルール初期化へ移行する
				mode = D_FREE_RULE_MODE;

				break;
			case	2:
				//終了
				if(sGameData.byGameMode == GAMEMODE_FREE){
					//モードをキャラクター選択初期化へ移行する
					mode = D_FREE_NPC_SELLECT_MODE;
				}
				else if(sGameData.byGameMode == GAMEMODE_SURVIVAL)
				{
					//モードをゲーム初期化へ移行する
					mode = D_FREE_GAME_MODE;
				}
				else
				{
					//モードをゲーム初期化へ移行する
					mode = D_FREE_GAME_MODE;
				}
				break;
		}
		RuleValSet(true);
		return(mode);
#endif //-*todo
		//tamaki MenuCsrAnm();
	}
	/****************************************************************/
	/*		ルール編集カーソル移動									*/
	/****************************************************************/
	/*	IN)		edit	- TRUE:編集可 FALSE:編集不可				*/
	/*	OUT)	nothing												*/
	/****************************************************************/
	public int RuleEditMove(/*MahJongRally * pMe,*/ bool edit)
	{
		#if false //-*todo:要らない気がする
		byte	ex_btn = 0xFF;
		int	i;
		if( D_ONEPUSH_INPUT_DOWN)
		{
			SeOnPlay( SE_ATACK, SEQ_SE);
			do{
				menu_csr++;
				if(menu_csr >=D_RULE_CHK_MAX)
				{
					menu_csr = 0;
				}

				/* 戻るを読み飛ばす	*/
				if(menu_csr == D_RULE_CHK_BACK)
				{
					menu_csr++;
				}
			}while(menu_sel[menu_csr]==MENU_DISABLE);

			MenuSelClr(FALSE);
			menu_sel[menu_csr] = MENU_SEL;
		}
		else if( D_ONEPUSH_INPUT_UP )
		{
			SeOnPlay( SE_ATACK, SEQ_SE);
			do{
				if(menu_csr == 0)
				{
					menu_csr = D_RULE_CHK_SET;
				}
				else
				{
					menu_csr--;
				}

				/* 戻るを読み飛ばす	*/
				if(menu_csr == D_RULE_CHK_BACK)
				{
					menu_csr--;
				}
			}while(menu_sel[menu_csr]==MENU_DISABLE);

			MenuSelClr(FALSE);
			menu_sel[menu_csr] = MENU_SEL;
		}
		else if(menu_csr >= D_RULE_CHK_RULE_MAX)
		{
			if( D_ONEPUSH_INPUT_RIGHT )
			{
				SeOnPlay( SE_ATACK, SEQ_SE);
				do{
					menu_csr++;
					if(menu_csr >=D_RULE_CHK_MAX)
					{
						menu_csr = D_RULE_CHK_RULE_MAX;
					}

					/* 戻るを読み飛ばす	*/
					if(menu_csr == D_RULE_CHK_BACK)
					{
						menu_csr++;
					}
				}while(menu_sel[menu_csr]==MENU_DISABLE);

				MenuSelClr(FALSE);
				menu_sel[menu_csr] = MENU_SEL;
			}
			else if( D_ONEPUSH_INPUT_LEFT )
			{
				SeOnPlay( SE_ATACK, SEQ_SE);
				do{
					menu_csr--;
					if(menu_csr < D_RULE_CHK_RULE_MAX)
					{
						menu_csr = D_RULE_CHK_SET;
					}

					/* 戻るを読み飛ばす	*/
					if(menu_csr == D_RULE_CHK_BACK)
					{
						menu_csr--;
					}
				}while(menu_sel[menu_csr]==MENU_DISABLE);

				MenuSelClr(FALSE);
				menu_sel[menu_csr] = MENU_SEL;
			}
		}

		/* ルール設定項目 */
		for( i = 0; i < sizeof(menu_sel); i++ )
		{
			if( menu_sel[i] == MENU_TOUCH )
			{
				menu_sel[i] = MENU_SEL;
			}
		}

		//ソフトキー１で戻る
		if(D_ONEPUSH_INPUT_SOFT1)
		{
			if(m_Mode != D_SURIVIVAL_RULE_CHK_MODE)
				menu_csr = D_RULE_CHK_BACK;
		}

		//ルールの選択
		for( i = 0; i < D_RULE_CHK_RULE_MAX; i++ )
		{
			if(menu_sel[i]==MENU_DISABLE)continue;
			if(edit)
			{
				if( D_ONEPUSH_INPUT_LEFT && i==menu_csr)
				{
					SeOnPlay( SE_ATACK, SEQ_SE);

					DebLog(("rule_edit_no = %d bool = %d \n",rule_edit_page*D_RULE_CHK_RULE_MAX+i,FALSE));

					RuleValChange((BYTE)(rule_edit_page*D_RULE_CHK_RULE_MAX+i),FALSE);
				}
				else if( D_ONEPUSH_INPUT_RIGHT && i==menu_csr)
				{
					SeOnPlay( SE_ATACK, SEQ_SE);

					DebLog(("rule_edit_no = %d bool = %d \n",rule_edit_page*D_RULE_CHK_RULE_MAX+i,TRUE));

					RuleValChange((BYTE)(rule_edit_page*D_RULE_CHK_RULE_MAX+i),TRUE);
				}
			}
		}

		ex_btn = NONE;

		//ルール画面の選択
		for( i = D_RULE_CHK_RULE_MAX; i < D_RULE_CHK_MAX; i++ )
		{
			if(menu_sel[i]==MENU_DISABLE)continue;

			//メニュー選択中に決定キー（セレクト）が押された場合はメニュ処理
			if( (D_ONEPUSH_INPUT_SELECT && i==menu_csr) || (D_ONEPUSH_INPUT_SOFT1 && i==menu_csr && m_Mode != D_SURIVIVAL_RULE_CHK_MODE) )
			{
				MenuKetteiSe();
				i = (int)menu_csr;
				MenuSelClr(FALSE);
				menu_sel[i] = MENU_KETTEI;
				ex_btn = (BYTE)i;
			}
		}

		//ルール変更中に決定キー（セレクト）が押された場合は決定処理
		if(D_ONEPUSH_INPUT_SELECT && menu_csr<D_RULE_CHK_RULE_MAX)
		{
			i = D_RULE_CHK_SET;
			menu_csr = (UINT2)i;

			MenuSelClr(FALSE);
			menu_sel[i] = MENU_TOUCH;
		}

		/* ← */
		if(ex_btn == D_RULE_CHK_PASTPAGE )
		{

			if( rule_edit_page == 0 )
			{
				rule_edit_page = D_RULE_PAGE_MAX-1;
			}
			else
			{
				rule_edit_page--;
			}

			DebLog(("rule_edit_page=%d",rule_edit_page));

			if(menu_sel[D_RULE_CHK_BACK] == MENU_DISABLE)
			{
				MenuSelClr(TRUE);
				menu_sel[D_RULE_CHK_BACK] = MENU_DISABLE;
			}else{
				MenuSelClr(TRUE);
			}

			RuleValSet(edit);
			RuleCsrInit();
		}
		/* → */
		else if(ex_btn == D_RULE_CHK_NEXTPAGE )
		{
			rule_edit_page++;

			if( rule_edit_page >= D_RULE_PAGE_MAX )
			{
				rule_edit_page = 0;
			}

			DebLog(("rule_edit_page=%d",rule_edit_page));

			if(menu_sel[D_RULE_CHK_BACK] == MENU_DISABLE)
			{
				MenuSelClr(TRUE);
				menu_sel[D_RULE_CHK_BACK] = MENU_DISABLE;
			}else{
				MenuSelClr(TRUE);
			}

			RuleValSet(edit);
			RuleCsrInit();
			//RuleFactChr();
			//RuleText(menu_csr);
		}
		/* 戻る */
		else if(ex_btn == D_RULE_CHK_BACK )
		{
			if(edit&&menu_sel[D_RULE_CHK_BACK]!=MENU_DISABLE)
			{
				MenuSelClr(FALSE);
				menu_csr = D_RULE_CHK_BACK;
				menu_sel[menu_csr] = MENU_KETTEI;
				return(1);
			}
		}
		/* 終了 */
		else if(ex_btn == D_RULE_CHK_SET )
		{
			MenuKetteiSe();
			MenuSelClr(FALSE);

			menu_csr = D_RULE_CHK_SET;
			menu_sel[menu_csr] = MENU_KETTEI;

			for (i = 0; i < RLSIZE; i++)
			{									/*	ユーザルールにコピー	*/
				if (byRuleData[select_rule][i] == NONE)
				{
					gbyUserRule[i]			=	Rultbl[i];
					//> 2006/03/06 要望 128 サバイバル戦を行うとフリー対戦で行ったルール設定が変更される。
					if( sGameData.byGameMode == GAMEMODE_FREE && select_rule == RULETYPE_FREE )
					{
						f_info.f_rule[i]	=	Rultbl[i];	// ファイル情報にもコピー 2006/02/10 要望No.87
					}
				}
			}

			UpdataRule();		//0422

			// ファイル書き込み 2006/02/10 要望No.87
			MJ_WriteFile(D_WORD_INFOFILE, f_info);ssave00(true);//0514mt

			return(2);
		}
		#endif //-*todo
		return(0);
	}

	/****************************************************************/
	/*		ルール値初期化											*/
	/****************************************************************/
	/*	IN)		select_rule - ルール番号							*/
	/*	OUT)	nothing												*/
	/****************************************************************/
	public void RuleValInit(/*MahJongRally * pMe,*/ byte no)
	{
		SetupRule(no);
	}
	/****************************************************************/
	/*		ルール値初期化											*/
	/****************************************************************/
	/*	IN)		select_rule - ルール番号							*/
	/*	OUT)	nothing												*/
	/****************************************************************/
	public void RuleValInit2(/*MahJongRally * pMe,*/ byte no)
	{
		short	i;
#if true//-*todo:
		DebLog(("RuleValInit2"));
#endif //-*todo
		for ( i = 0; i < (short)RL.RLSIZE; i++ )
		{
			Rultbl[i] = MJDefine.byRuleData[0][i];                     
		}

		sRuleSubData.byRenchanRate	=	1;
		sRuleSubData.byFlag			=	0;
		switch (no) {
		case (byte)RULETYPE.BUNYA:								/*	ブンヤルール	*/
			sRuleSubData.byFlag			|=	(byte)RULESUBFLAG.RENCHAN;
			break;
		case (byte)RULETYPE.TONPUU:
			sRuleSubData.byRenchanRate	=	5;
			break;
		default:
			break;
		}

		sRuleSubData.byRuleNo		=	no;
		sGameData.byRuleNo			=	(byte)no;
		for ( i = 0; i < (short)RL.RLSIZE; i ++ )
		{
			/* i番目のﾙｰﾙが固定の時 */
			if ( MJDefine.byRuleData[no][i]!= MJDefine.NONE )
			{
				Rultbl[ i ] = MJDefine.byRuleData[no][i];
			}
		}

		UpdataRule();
	}

	/****************************************************************/
	/*		ルール値変更											*/
	/****************************************************************/
	public void RuleValChange(/*MahJongRally * pMe,*/ byte	idx,bool add)
	{
		int	i;

		if(idx == 41){		/* 標準に戻す */
			InitUserRule();
			RuleValInit2((byte)select_rule);
			#if false //-*todo:
			SeOnPlay( SE_KETTEI, SEQ_SE);
			i = (int)menu_csr;
			MenuSelClr(FALSE);
			menu_sel[i] = MENU_KETTEI;
			#endif //-*todo
			return;
		}

		if(add)
		{
			Rultbl[idx]++;
			if(MJDefine.rule_val_tbl[idx][Rultbl[idx]] == 0xFF)
			{
				Rultbl[idx] = 0;
			}
		}else
		{
			if(Rultbl[idx]==0)
			{
				i = 0;
				while(MJDefine.rule_val_tbl[idx][i]!=0xFF){
					i++;
				}
				i--;
				Rultbl[idx] = (byte)i;
			}
			else
			{
				Rultbl[idx]--;
			}
		}
		#if true //-*todo:
		DebLog(("Rultbl["+idx+"]="+Rultbl[idx]));
		#endif //-*todo
		if(menu_sel[(int)D_RULE_CHK.BACK] == (char)MENU_SEL.DISABLE)
		{
		#if false //-*todo:
			MenuSelClr(true);
		#endif //-*todo:
			menu_sel[(int)D_RULE_CHK.BACK] = (char)MENU_SEL.DISABLE;
		}
		else
		{
		#if false //-*todo:
			MenuSelClr(true);
		#endif //-*todo:
		}

		RuleValSet(true);
		#if false //-*todo
		RuleCsrInit();
		#endif //-*todo
	}
	/****************************************************************/
	/*		ルール編集値セット										*/
	/****************************************************************/
	public void RuleValSet(/*MahJongRally * pMe,*/ bool edit)
	{
		int	i;

		for( i = 0; i < (int)D_RULE_CHK.RULE_MAX; i++ )
		{
			if( (MJDefine.byRuleData[select_rule][(rule_edit_page*(int)D_RULE_CHK.RULE_MAX)+i] != MJDefine.NONE) || (!edit) ){
				menu_sel[i] = (char)MENU_SEL.DISABLE;
			}
			if (Rultbl[(int)RL.KAZE] == 2) {									/*	東風の時は西入り無し	*/
				Rultbl[(int)RL.SHANY]	=	0;
				if((rule_edit_page*(int)D_RULE_CHK.RULE_MAX)+i==(int)RL.SHANY)menu_sel[i] = (char)MENU_SEL.DISABLE;
			}
			if (Rultbl[(int)RL.KAN] == 0) {									/*	カンドラなしはカン裏無し	*/
				Rultbl[(int)RL.KANUR]	=	0;
				if((rule_edit_page*(int)D_RULE_CHK.RULE_MAX)+i==(int)RL.KANUR)menu_sel[i] = (char)MENU_SEL.DISABLE;
			}
			if (Rultbl[(int)RL.URA] == 0) {									/*	裏ドラなしは裏ドラ賞無し	*/
				Rultbl[(int)RL.RDSHO]	=	0;
				if((rule_edit_page*(int)D_RULE_CHK.RULE_MAX)+i==(int)RL.RDSHO)menu_sel[i] = (char)MENU_SEL.DISABLE;
				/*	裏ドラ無しはカン裏無し	*/
				Rultbl[(int)RL.KANUR]	=	0;
				if((rule_edit_page*(int)D_RULE_CHK.RULE_MAX)+i==(int)RL.KANUR)menu_sel[i] = (char)MENU_SEL.DISABLE;
			}
			if (Rultbl[(int)RL.IPPAT] == 0) {								/*	一発なしは一発賞無し	*/
				Rultbl[(int)RL.IPSHO]	=	0;
				if((rule_edit_page*(int)D_RULE_CHK.RULE_MAX)+i==(int)RL.IPSHO)menu_sel[i] = (char)MENU_SEL.DISABLE;
			}
			if (Rultbl[(int)RL.TWO_CHAHO] == 0) {								/*	二家和なしは三家和無し	*/
				Rultbl[(int)RL.THREE_CHAHO]	=	0;
				if((rule_edit_page*(int)D_RULE_CHK.RULE_MAX)+i==(int)RL.THREE_CHAHO)menu_sel[i] = (char)MENU_SEL.DISABLE;
			}
		}
	}

	/****************************************************************/
	/*		ルール確認画面初期化									*/
	/****************************************************************/
	/*	IN)		nothing												*/
	/*	OUT)	nothing												*/
	/****************************************************************/
	public void RuleCheckInit(/*MahJongRally * pMe*/)
	{
		menu_csr = 0;
		#if false //-*todo:
		MenuSelClr(true);
		#endif //-*todo:
		menu_sel[menu_csr] = (char)MENU_SEL.SEL;
		rule_edit_page = 0;
	//	kanji = 0;

		SelectRuleSet();

		//tamaki DemoInit();
		RuleValSet(false);
		#if false //-*todo:
		RuleCsrInit();
		#endif //-*todo:

		//tamaki ここ確認↓
		menu_sel[(int)D_RULE_CHK.BACK] = (char)MENU_SEL.DISABLE;
		next_f_cnt = 8;
	}

	/****************************************************************/
	/*		ルール確認画面処理										*/
	/****************************************************************/
	/*	IN)		nothing												*/
	/*	OUT)	nothing												*/
	/****************************************************************/
	public void RuleCheckMain(/*MahJongRally * pMe*/)
	{
		switch(RuleEditMove(true))
		{
			case	2:
				//終了
				switch(sGameData.byGameMode)
				{
					case	MJDefine.GAMEMODE_SURVIVAL:
						//サバイバル対戦初期化へ移行
						#if false //-*todo:
						modeChange(D_SURIVIVAL_OPENING_NO2_MODE);
						#endif //-*todo:
						break;
					default:
						//フリー対戦初期化へ移行なの？？？
						#if false //-*todo:
						modeChange(D_FREE_RULE_MODE);
						#endif //-*todo:
						break;
				}
				break;
		}
		RuleValSet(false);
	}

	/****************************************************************/
	/*		モードごとのルール設定初期化							*/
	/****************************************************************/
	/*	IN)		nothing												*/
	/*	OUT)	nothing												*/
	/****************************************************************/
	public void SelectRuleSet(/*MahJongRally * pMe*/)
	{
		if(sGameData.byGameMode == MJDefine.GAMEMODE_SURVIVAL){
			select_rule = (byte)RULETYPE.SURVIVAL;
		}else if(sGameData.byGameMode == MJDefine.GAMEMODE_FREE){
			return;
		}

		RuleValInit2((byte)select_rule);
	}

	/**		//0422	以下追加
	* @brief 全てのユーザールールの境界チェックと修正
	*/
	public void checkUserRule( /*MahJongRally * pMe*/ )
	{
		short no_ = 0;

		for( no_ = 0; no_ < (short)RL.RLSIZE; ++no_ ) {
			byte max_ = 0x00;
			switch( no_ ) {
				case (short)RL.KUITA	: max_ = (byte)RL_MAX.DEFAULT;	break;
				case (short)RL.NAKIPN	: max_ = (byte)RL_MAX.DEFAULT;	break;
				case (short)RL.PINHO	: max_ = (byte)RL_MAX.DEFAULT;	break;
				case (short)RL.NOTEN	: max_ = (byte)RL_MAX.DEFAULT;	break;
				case (short)RL.NANBA	: max_ = (byte)RL_MAX.NANBA  ;	break;
				case (short)RL.URA		: max_ = (byte)RL_MAX.DEFAULT;	break;
				case (short)RL.KAN		: max_ = (byte)RL_MAX.DEFAULT;	break;
				case (short)RL.KANUR	: max_ = (byte)RL_MAX.DEFAULT;	break;
				case (short)RL.IPPAT	: max_ = (byte)RL_MAX.DEFAULT;	break;
				case (short)RL.DOBON	: max_ = (byte)RL_MAX.DOBON  ;	break;
				case (short)RL.POINT	: max_ = (byte)RL_MAX.POINT  ;	break;
				case (short)RL.RETPOINT	: max_ = (byte)RL_MAX.RETPOINT;	break;
				case (short)RL.KAZE		: max_ = (byte)RL_MAX.KAZE   ;	break;
				case (short)RL.SHANY	: max_ = (byte)RL_MAX.SHANY  ;	break;
				case (short)RL.UMA		: max_ = (byte)RL_MAX.UMA    ;	break;
				case (short)RL.WINEND	: max_ = (byte)RL_MAX.DEFAULT;	break;
				case (short)RL.NAGARE	: max_ = (byte)RL_MAX.NAGARE ;	break;
				case (short)RL.RYANSH	: max_ = (byte)RL_MAX.DEFAULT;	break;
				case (short)RL.KOKCHA	: max_ = (byte)RL_MAX.DEFAULT;	break;
				case (short)RL.RANKAN	: max_ = (byte)RL_MAX.DEFAULT;	break;
				case (short)RL.RIBO		: max_ = (byte)RL_MAX.DEFAULT;	break;
				case (short)RL.YAKI		: max_ = (byte)RL_MAX.YAKI   ;	break;
				case (short)RL.WAREM	: max_ = (byte)RL_MAX.DEFAULT;	break;
				case (short)RL.PAREN	: max_ = (byte)RL_MAX.DEFAULT;	break;
				case (short)RL.IPSHO	: max_ = (byte)RL_MAX.DEFAULT;	break;
				case (short)RL.RDSHO	: max_ = (byte)RL_MAX.DEFAULT;	break;
				case (short)RL.YMSHO	: max_ = (byte)RL_MAX.DEFAULT;	break;
				case (short)RL.TORIU	: max_ = (byte)RL_MAX.DEFAULT;	break;
				case (short)RL.ALICE	: max_ = (byte)RL_MAX.DEFAULT;	break;
				case (short)RL.KINCH	: max_ = (byte)RL_MAX.DEFAULT;	break;
				case (short)RL.FIVE_PIN	: max_ = (byte)RL_MAX.DEFAULT;	break;
				case (short)RL.SANRE	: max_ = (byte)RL_MAX.DEFAULT;	break;
				case (short)RL.SANJUN	: max_ = (byte)RL_MAX.DEFAULT;	break;
				case (short)RL.YAOCHU	: max_ = (byte)RL_MAX.DEFAULT;	break;
				case (short)RL.SHISAN   	: max_ = (byte)RL_MAX.SHISAN ;	break;
				case (short)RL.RENHO		: max_ = (byte)RL_MAX.DEFAULT;	break;
				case (short)RL.DAISH		: max_ = (byte)RL_MAX.DEFAULT;	break;
				case (short)RL.HUNDRED_MAN	: max_ = (byte)RL_MAX.DEFAULT;	break;
				case (short)RL.DBYMAN		: max_ = (byte)RL_MAX.DEFAULT;	break;
				case (short)RL.TWO_CHAHO	: max_ = (byte)RL_MAX.DEFAULT;	break;
				case (short)RL.THREE_CHAHO	: max_ = (byte)RL_MAX.DEFAULT;	break;
				default          : max_ = 0x01;
				#if false //-*todo:
					ASSERT( !"checkUserRule" );
				#endif //-*todo:
					break;
			}

			if( gbyUserRule[no_] > max_ ) {
				#if true //-*todo:
				DebLog(("ERROR BAD Rule["+no_+"]:"+gbyUserRule[no_]+"->"+max_));
				#endif //-*todo:
				gbyUserRule[no_] = max_;
				#if false //-*todo
				f_info.f_rule[no_] = max_;
				#endif //-*todo
	//			ASSERT( !"checkRuleMax" );
			}
		}
	}


//-*mjrule.j************************
/*
**		麻雀大会３
**		ルール設定処理
*/

//#include "MahJongRally.h"								// Module interface definitions


/*****************************
	ルール・ロード
*****************************/
public void	ldrule_rule ( /*MahJongRally * pMe*/ )
{
	int	i;

	/* ユーザールールを対局ルールにセット */
	for ( i = 0; i < (int)RL.RLSIZE; i++ ) {
		Rultbl[i] = gbyUserRule[i];
	}
	/* チップフラグをセット */
	if (( Rultbl[ (int)RL.IPSHO ]  |
				Rultbl[ (int)RL.RDSHO ]  |
				Rultbl[ (int)RL.YMSHO ]  |
				Rultbl[ (int)RL.TORIU ]  |
				Rultbl[ (int)RL.ALICE ]    ) != 0) {
		sRuleSubData.byFlag	|=	(byte)RULESUBFLAG.CHIP;
	}
	else {
		#if true //-*todo:書き換え有ってるかな？
		sRuleSubData.byFlag	= (byte)(sRuleSubData.byFlag & ~(byte)RULESUBFLAG.CHIP);
		// sRuleSubData.byFlag	&=	~(byte)RULESUBFLAG.CHIP;
		#endif
	}
}


/*****************************
	 ルール・イニット
*****************************/
public void	InitUserRule ( /*MahJongRally * pMe*/ )
{
	/* ユーザールールに標準ルールを設定 */
	// _MEMCPY(gbyUserRule, MJDefine.byRuleData[0], sizeof(gbyUserRule));	//MEMCPY(&gbyUserRule[0], &byRuleData[0][0], sizeof(gbyUserRule));
	_MEMCPY(gbyUserRule, MJDefine.byRuleData[0], gbyUserRule.Length);	//MEMCPY(&gbyUserRule[0], &byRuleData[0][0], sizeof(gbyUserRule));
}

/******************************************************************************
	ルール変更による修正
******************************************************************************/
public void	UpdataRule(/*MahJongRally * pMe*/)
{
	/* チップフラグを設定 */
	if (( Rultbl[ (int)RL.IPSHO ] | Rultbl[ (int)RL.RDSHO ] |
				Rultbl[ (int)RL.YMSHO ] | Rultbl[ (int)RL.TORIU ] | Rultbl[ (int)RL.ALICE ] ) != 0) {
		sRuleSubData.byFlag	|=	(byte)RULESUBFLAG.CHIP;
		sRuleSubData.byChipRate	=	5;
//		sRuleSubData.byChipRate	=	1;
	}
	else {
		#if true //-*todo:書き換え有ってるかな？
		sRuleSubData.byFlag	= (byte)(sRuleSubData.byFlag & ~(byte)RULESUBFLAG.CHIP);
		// sRuleSubData.byFlag	&=	~RULESUBFLAG_CHIP;
		#endif
		sRuleSubData.byChipRate	=	0;
	}
}



/******************************************************************************
	ステージごとのルールセット
******************************************************************************/
public void	SetupRule(/*MahJongRally * pMe,*/ int iRuleNo)
{
	short	i;
#if true //-*todo:	
	DebLog(("SetupRule"));
#endif //-*todo
	ldrule_rule ();										/* ユーザールールをルールテーブルにロード */

	sRuleSubData.byRenchanRate	=	1;
	sRuleSubData.byFlag			=	0;
	switch (iRuleNo) {
	  case (int)RULETYPE.BUNYA:								/*	ブンヤルール	*/
		sRuleSubData.byFlag			|=	(byte)RULESUBFLAG.RENCHAN;
		break;
	  case (int)RULETYPE.TONPUU:
		sRuleSubData.byRenchanRate	=	5;
		break;
	  default:
		break;
	}

	sRuleSubData.byRuleNo		=	(byte)iRuleNo;
	sGameData.byRuleNo			=	(byte)iRuleNo;
	for ( i = 0; i < (short)RL.RLSIZE; i ++ )
	{
		/* i番目のﾙｰﾙが固定の時 */
		if ( MJDefine.byRuleData[iRuleNo][i] != MJDefine.NONE )
		{
			Rultbl[ i ] = MJDefine.byRuleData[iRuleNo][i];
		}
		//++pbyRuleTable;
	}

	//if (iRuleNo == RULETYPE_BUNYA || iRuleNo == RULETYPE_FREEMODE) {
	//	sRuleSubData.byFlag	&=	~RULESUBFLAG_SASHIUMA;
	//}
	//else {
	//	sRuleSubData.byFlag	|=	RULESUBFLAG_SASHIUMA;
	//}
	UpdataRule();
}

/**************************END OF FILE*******************************/

//-*********************mjrule.j
}
