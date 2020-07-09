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

/// <summary>
/// 各プレイヤーの状態
/// </summary>
public class PLAYERWORK  {
	// public class PLAYERWORK {
	public byte[]	byTehai = new byte [14];	/* 手牌(０～１２のＭＡＸ１３牌)             */
										/* １３ = ツモ牌（あやしい）                */
	public byte	byHkhai;			 		/* 現時点での捨てられた牌	（ツモ牌では？）*/
	public byte	byThcnt;					/* 手牌の数                                 */
	public byte	byFhcnt;					/* フーロ牌の数(暗カン含)                   */
	public byte	byShcnt;					/* 捨て牌の数                               */
	public byte	byShptr;					/* 何個目の捨て牌か                         */
/* 以下のフラグ (00:OFF) (FF:ON) */
	public byte	bFfurit;					/* フリテンフラグ(フリテンでＯＮ)           */
	public byte	bFrich;						/* リーチフラグ(リーチでＯＮ)               */
	public byte	bFwrich;					/* ダブリーフラグ(ダブリーでＯＮ)           */
	public byte	bFippat;					/* 一発フラグ(一発でＯＮ)                   */
	public byte	bFmenzen;					/* 面前フラグ（鳴くとＯＮ）                 */
	public byte	bFnagas;					/* 流し満貫フラグ(崩れるとＯＮ)             */
	public byte	bFkokus;					/* 国士フラグ(聴牌るとＯＮ)                 */
	public byte	bFsthai;
	public byte	byTenpai;					/* 聴牌（待ちの数）                         */
	public byte	byKancnt;					/* カンの数                                 */
	public byte	byStatus;					/* ステータス                               */
	public byte	byApflg;					/* オートプレイ                             */
	public byte	byKflg;
	public byte	byTkhai;					//:// 手牌表示フラグ（00:byThcntの数だけ表示 FF:byThcntの数表示+byHkhai（ツモ牌）を表示）
	public byte	byPlflg;					/* プレイヤーフラグ(プレイヤーでＯＮ)       */
	public byte[]	byFrhai = new byte [4];	/* フーロ牌（ＭＡＸで４個分）               */
	public byte[]	byFrhaiBefore = new byte [4];	//-*todo:前回のフーロ牌の数(GameObject生成するか確認用)
	public byte[]	byFrpos = new byte [4];	/* フーロ牌のポジション                     */
										/* ポンカンの場合：フーロ者から見て(0:上家)(1:対面)(2:下家) */
										/* チーの場合：順子の(0-2)番目の牌を一番左に持ってきて横にする */
	public byte[]	byMchai =  new byte [13];	/* 待ち牌(４４～５６のＭＡＸ１３牌)         */
	public byte[]	bySthai = new byte [MJDefine.SUTEHAIMAX];	/* 捨て牌(５７～７８のＭＡＸ２４牌)         */
	public byte	byOrder;					/* 予約                                     */

	public byte	byIppatuClear;				/*	一発クリア	後で削除					*/
	public PARA	sParamData;					/*	キャラクターパラメータ					*/

	public void copy( PLAYERWORK ob ) {
		int i;
		for(i = 0; i < 14; i++){
			byTehai[i]= ob.byTehai[i];
		}
		byHkhai= ob.byHkhai;
		byThcnt= ob.byThcnt;
		byFhcnt= ob.byFhcnt;
		byShcnt= ob.byShcnt;
		byShptr= ob.byShptr;

		bFfurit= ob.bFfurit;
		bFrich= ob.bFrich;
		bFwrich= ob.bFwrich;
		bFippat= ob.bFippat;
		bFmenzen= ob.bFmenzen;
		bFnagas= ob.bFnagas;
		bFkokus= ob.bFkokus;
		bFsthai= ob.bFsthai;
		byTenpai= ob.byTenpai;
		byKancnt= ob.byKancnt;
		byStatus= ob.byStatus;
		byApflg= ob.byApflg;
		byKflg= ob.byKflg;
		byTkhai= ob.byTkhai;
		byPlflg= ob.byPlflg;


		for(i = 0; i < 4; i++)	byFrhai[i]= ob.byFrhai[i];
		for(i = 0; i < 4; i++)	byFrhaiBefore[i]= ob.byFrhaiBefore[i];//-*todo:前回のフーロ牌の数(GameObject生成するか確認用)
		for(i = 0; i < 4; i++)	byFrpos[i]= ob.byFrpos[i];

		for(i = 0; i < 13; i++)	byMchai[i]= ob.byMchai[i];
		for(i = 0; i < (24); i++)	bySthai[i]= ob.bySthai[i];
		byOrder= ob.byOrder;

		byIppatuClear= ob.byIppatuClear;
		sParamData.copy( (PARA)ob.sParamData );
	}

	public PLAYERWORK clone() {
		int	i;

		PLAYERWORK ret = new PLAYERWORK();

		for(i = 0; i < 14; i++)	ret.byTehai[i]= this.byTehai[i];

		ret.byHkhai= this.byHkhai;
		ret.byThcnt= this.byThcnt;
		ret.byFhcnt= this.byFhcnt;
		ret.byShcnt= this.byShcnt;
		ret.byShptr= this.byShptr;

		ret.bFfurit= this.bFfurit;
		ret.bFrich= this.bFrich;
		ret.bFwrich= this.bFwrich;
		ret.bFippat= this.bFippat;
		ret.bFmenzen= this.bFmenzen;
		ret.bFnagas= this.bFnagas;
		ret.bFkokus= this.bFkokus;
		ret.bFsthai= this.bFsthai;
		ret.byTenpai= this.byTenpai;
		ret.byKancnt= this.byKancnt;
		ret.byStatus= this.byStatus;
		ret.byApflg= this.byApflg;
		ret.byKflg= this.byKflg;
		ret.byTkhai= this.byTkhai;
		ret.byPlflg= this.byPlflg;
		for(i = 0; i < 4; i++)	ret.byFrhai[i]= this.byFrhai[i];
		for(i = 0; i < 4; i++)	ret.byFrhaiBefore[i]= this.byFrhaiBefore[i];//-*todo:前回のフーロ牌の数(GameObject生成するか確認用)
		for(i = 0; i < 4; i++)	ret.byFrpos[i]= this.byFrpos[i];

		for(i = 0; i < 13; i++)	ret.byMchai[i]= this.byMchai[i];
		for(i = 0; i < MJDefine.SUTEHAIMAX; i++)	ret.bySthai[i]= this.bySthai[i];
		ret.byOrder= this.byOrder;

		ret.byIppatuClear= this.byIppatuClear;
		ret.sParamData= (PARA)this.sParamData.clone();

		return (ret);
	}
	// Use this for initialization
	void Start () {}
	// Update is called once per frame
	void Update () {}


}
