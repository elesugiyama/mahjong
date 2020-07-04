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
/// 麻雀大会３
/// 会話デファイン
/// </summary>
namespace MJDialogHeader {

/// <summary>
/// 対局会話
/// </summary>
	public enum DIAL : byte{
		DORAPON			= (0),			/*	ドラポン(0序中1終)					*/
		TO_DORAPON			= (2),			/*	ドラポンに(2序中3終)				*/
		DORAKAN			= (4),			/*	ドラカン							*/
		KANMAKE_DORA		= (5),			/*	カンしたらドラになった				*/
		FIRST_FURO			= (6),			/*	第１フーロ(6序7中8終)				*/
		TO_FIRST_FURO		= (9),			/*	第１フーロに(9序10中11終)			*/
		TO_THIRD_FURO		= (12),		/*	第３フーロに						*/
		TO_FOUR_FURO		= (13),		/*	第４フーロに						*/
		FOUR_FURO_KINCHI	= (14),		/*	第４フーロ(裸単騎有り)				*/
		FOUR_FURO			= (15),		/*	第４フーロ							*/
		IPPATSU_KESHI		= (16),		/*	一発消しフーロ						*/
		TO_IPPATSU_KESHI_C	= (17),		/*	キャラクターに一発消された			*/
		TO_IPPATSU_KESHI_P	= (18),		/*	プレイヤーに一発消された			*/
		DORAKIRI			= (19),		/*	立直中ドラ切り						*/
		SOMEYA				= (20),		/*	警告・染め屋						*/
		KOKUSHI			= (21),		/*	警告・国士無双						*/
		NAGASHI			= (22),		/*	警告・ヤオ九振切					*/
		NAGASHI_END		= (23),		/*	流し満貫終了						*/
		REACH_W			= (24),		/*	Ｗ立直								*/
		REACH_OI			= (25),		/*	追いかけ立直						*/
		REACH_3CHA			= (26),		/*	３人目立直							*/
		REACH_NORMAL		= (27),		/*	通常立直= (27序28中29終),				*/
		TO_WREACH			= (30),		/*	ダブル立直した人に					*/
		TO_PLAY_WREACH		= (31),		/*	プレイヤーダブル立直に				*/
		TO_KO_OIREACH		= (32),		/*	親の立直中、子の追っかけ立直に		*/
		TO_OYA_OIREACH		= (33),		/*	子の立直中、親の追っかけ立直に		*/
		TO_OTHER_OIREACH	= (34),		/*	子の立直中、子の追っかけ立直に		*/
		D_3REACH_OYA			= (35),		/*	３人立直で親だけ違う				*/
		D_3REACH_KO			= (36),		/*	３人立直で子だけ違う				*/
		TSUMO_IPPATSU		= (37),		/*	一発ツモ							*/
		TSUMO_SMALL		= (38),		/*	小さいツモ							*/
		TSUMO_MIDSMALL		= (39),		/*	そこそこ小さいツモ					*/
		TSUMO_MIDBIG		= (40),		/*	そこそこ大きいツモ					*/
		TSUMO_BIG			= (41),		/*	大きいツモ							*/
		TSUMO_RINSHAN		= (42),		/*	嶺山開花							*/
		TSUMO_SHISAN		= (43),		/*	十三不搭							*/
		TSUMO_TENCHIHO		= (44),		/*	天和・地和							*/
		TSUMO_YAKUMAN		= (45),		/*	その他役満							*/
		TAKE_TSUMO_BIG		= (46),		/*	でかいツモに						*/
		TAKE_TSUMO_YAKUMAN	= (47),		/*	役満ツモに							*/
		TSUMO_PAO			= (48),		/*	役満ツモでパオに					*/
		RON_BIG			= (49),		/*	でかい手ロンに						*/
		RON_YAKUMAN		= (50),		/*	役満ロンに							*/
		RON_PAO			= (51),		/*	ロンでパオに						*/
		TAKAME				= (52),		/*	高めアガリ							*/
		YASUME				= (53),		/*	安めアガリ							*/
		HAIHOUTEI			= (54),		/*	海底河底アガリ						*/
		HOUJU_SMALL		= (55),		/*	小さい手を放銃						*/
		HOUJU_MIDDLE		= (56),		/*	そこそこの手を放銃					*/
		HOUJU_BIG			= (57),		/*	でかい手を放銃						*/
		HOUJU_YAKUMAN		= (58),		/*	役満を放銃							*/
		HOUJU_DORA			= (59),		/*	ドラで放銃							*/
		HOUJU_REACHBATTLE	= (60),		/*	立直合戦で放銃						*/
		HOUJU_CHANKAN		= (61),		/*	搶カンで放銃						*/
		HOUJU_HOUTEI		= (62),		/*	河底で放銃							*/
		DAIMINKAN			= (63),		/*	大明カン							*/
		TENPAI_ONLY		= (64),		/*	一人聴牌							*/
		NOTEN_ONLY			= (65),		/*	一人不聴							*/
		TENPAI_ALL			= (66),		/*	四人聴牌							*/
		D_9SYU9HAI			= (67),		/*	九種九牌							*/
		D_4CYA_REACH			= (68),		/*	四家立直							*/
		TO_4CYA_REACH		= (69),		/*	四家立直の最後の人に				*/
		D_4KAIKAN			= (70),		/*	四開カン							*/
		D_4FURENDA			= (71),		/*	四風連打							*/
		RYANSHI			= (72),		/*	２飜縛り突入						*/
		PAREN				= (73),		/*	八連荘突入							*/
		NANNYU				= (74),		/*	南入								*/
		SYANYU				= (75),		/*	西・北・返東入						*/
		ALLLAST_NOTOP		= (76),		/*	オーラストップ目無し				*/
		GAME_TOP			= (77),		/*	半荘トップ							*/
		GAME_BOXLAS		= (78),		/*	半荘箱ラス							*/
		ALLLAST_OYASMLTOP	= (79),		/*	オーラス親僅差トップ				*/
		ALLLAST_OYABIGTOP	= (80),		/*	オーラス親大差トップ				*/
		ALLLAST_KOSMLTOP	= (81),		/*	オーラス子僅差トップ				*/
		ALLLAST_KOBIGTOP	= (82),		/*	オーラス子大差トップ				*/
		SASHIUMA_OK		= (83),		/*	プレーヤーから差し馬ＯＫ			*/
		//#if 1
		// > 2006/02/17 No.65
		MAXSIZE			= (84),		/*	最大会話数							*/
		SASHIWIN			= (200),		/*	差し馬勝ち(発声)					*/
		SASHILOST			= (201),		/*	差し馬負け(発声)					*/
		TAIKAIWIN			= (202),		/*	大会優勝(カット)					*/
		HASAN				= (203),		/*	破産								*/
		DOBON				= (204),		/*	ドボン（破産を使用する）			*/
		D_10000G				= (205),		/*	１００００Ｇ達成(カット)			*/
		PLAYER_TOP			= (206),		/*	プレーヤートップ					*/
		PLAYER_LAS			= (207),		/*	プレーヤーがラス					*/
		PLAYER_BOXLAS		= (208),		/*	プレーヤー箱ラス					*/
		BECAME_PAO			= (209),		/*	パオになった人に					*/
		NAGASHIMAN			= (210),		/*	ヤオ九振切							*/
		TO_NAGASHIMAN		= (211),		/*	ヤオ九振切に						*/
		DABURON_ATAMA		= (212),		/*	ダブロン頭ハネ						*/
		DABURON_SHIRI		= (213),		/*	ダブロン頭ハネ						*/
		TORIRON_HANEWIN	= (214),		/*	トリロン頭ハネ(カット)				*/
		TORIRON_HANELOST	= (215),		/*	トリロン頭ハネされた(カット),		*/
	}

/*************************	END	OF	FILE	*************************************	*/
}