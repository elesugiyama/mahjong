using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

/// <summary>
/// シナリオで使用する共通定義
/// todo:ココを弄れば他ゲーへ換装がある程度楽になるようにしたい
/// </summary>
namespace AdventureDefine
{
	public class AdvDefine
	{
#region	SCENARIOCMMAND
		/// <summary>
		/// シナリオコマンド種類
		/// </summary>
		public enum CMD_TYPE{
			CMD_PAGEEND,	// ページ区切り
			CMD_SPEAKER_DEL,// 発言者名:消す
			CMD_SPEAKER_MOE,// 発言者名:萌(萌日記)
			CMD_BG,			// 背景
			CMD_EV_BG,		// イベント背景
			CMD_BGM,		// BGM再生:未実装
			CMD_BGM_END,	// BGM終了:未実装
			CMD_CH,			// キャラ
			CMD_CH_FACE,	// 表情
			CMD_CH_DEL,		// キャラ消し
			CMD_SCO_END,	// シナリオ終了
			CMD_GO_BATTLE,	// InGame(麻雀)へ
			CMD_CHOICE_START,	// 選択肢開始
			CMD_CHOICE,			// 選択肢
			CMD_CHOICE_END,		// 選択肢終了
			CMD_FADE_IN_B,		// フェードイン
			CMD_FADE_OUT_B,		// フェードアウト
			CMD_GAME_CLEAR,		// ゲームクリア
			NO_CMD_SENTENCE,// シナリオ本文
			CMD_MAX,		// コマンド総数
		}

		/// <summary>
		/// シナリオコマンド※順番に注意
		/// 同文字列が含まれている場合は長い方を上に
		/// </summary>
		public static Dictionary<CMD_TYPE,string> CmdDir = new Dictionary<CMD_TYPE,string>()
		{
			{ CMD_TYPE.CMD_PAGEEND,			"/p" },				// ページ区切り
			{ CMD_TYPE.CMD_SPEAKER_DEL,		"/no" },			// 発言者名:消す
			{ CMD_TYPE.CMD_SPEAKER_MOE,		"/moe" },			// 発言者名:萌(萌日記)
			{ CMD_TYPE.CMD_BG,				"#bg_" },			// 背景
			{ CMD_TYPE.CMD_EV_BG,			"#ev_" },			// イベント背景
			{ CMD_TYPE.CMD_BGM_END,			"#bgm_e" },			// BGM終了:未実装
			{ CMD_TYPE.CMD_BGM,				"#bgm_" },			// BGM再生:未実装
			{ CMD_TYPE.CMD_CH,				"#ch_c_" },			// キャラ
			{ CMD_TYPE.CMD_CH_FACE,			"#ch_face_" },		// 表情
			{ CMD_TYPE.CMD_CH_DEL,			"#chc_c" },			// キャラ消し
			{ CMD_TYPE.CMD_GO_BATTLE,		"#battle_" },		// InGame(麻雀)へ
			{ CMD_TYPE.CMD_CHOICE_START,	"#choice_start" },	// 選択肢開始
			{ CMD_TYPE.CMD_CHOICE_END,		"#choice_end" },	// 選択肢終了
			{ CMD_TYPE.CMD_CHOICE,			"#choice_" },		// 選択肢
			{ CMD_TYPE.CMD_FADE_IN_B,		"#blackin" },		// 黒フェードイン
			{ CMD_TYPE.CMD_FADE_OUT_B,		"#blackout" },		// 黒フェードアウト
			{ CMD_TYPE.CMD_GAME_CLEAR,		"#game_clear" },	// ゲームクリア
			{ CMD_TYPE.CMD_SCO_END,			"#file_end" },		// シナリオ終了
		};

		public static char SCO_CMD_SPLIT = '_';	//-*シナリオコマンドの分割判定用文字列
#endregion	//-*SCENARIOCMMAND

#region IMAGE
		/// <summary>
		/// CGリスト< int:イベント番号, string:画像名 >
		/// 旧(eventCGFileName.dat イベントCG用ファイル名)
		/// </summary>
		public static string[] CGList = new string[]
		{
			"moema_event01",	// 0:ＣＧ１～２戦目用下段中（ソファ上膝立ち)"
			"moema_event02",	// 1:ＣＧ１～２戦目用下段右（着替え中）"
			"moema_event00",	// 2:ＣＧ１～２戦目用上段左（ソファ座り）"
			"moema_event03",	// 3:ＣＧ１～２戦目用上段右(ソファ寝そべり)"
			"moema_event04",	// 4:ＣＧ体操着
			"moema_event06",	// 5:ＣＧチャイナ服
			"moema_event08",	// 6:ＣＧバニーガール
			"moema_event05",	// 7:ＣＧ水着
			"moema_event07",	// 8:ＣＧメイド服
			"moema_event09",	// 9:５戦め後お祭り萌
			"moema_event10",	// 9:特典CG
		};
#endregion	//-*IMAGE

	}
}