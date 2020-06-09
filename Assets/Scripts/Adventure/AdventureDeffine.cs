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
			CMD_EV_BG,			// イベント背景
			CMD_BGM,		// BGM再生:未実装
			CMD_BGM_END,	// BGM終了:未実装
			CMD_CH,			// キャラ
			CMD_CH_FACE,	// 表情
			CMD_SCO_END,	// シナリオ終了
			CMD_GO_BATTLE,	// InGame(麻雀)へ
			CMD_CHOICE_START,	// 選択肢開始
			CMD_CHOICE,			// 選択肢
			CMD_CHOICE_END,		// 選択肢終了
			CMD_FADE_IN_B,		// フェードイン
			CMD_FADE_OUT_B,		// フェードアウト
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
			{ CMD_TYPE.CMD_GO_BATTLE,		"#battle_" },		// InGame(麻雀)へ
			{ CMD_TYPE.CMD_CHOICE_START,	"#choice_start" },	// 選択肢開始
			{ CMD_TYPE.CMD_CHOICE_END,		"#choice_end" },	// 選択肢終了
			{ CMD_TYPE.CMD_CHOICE,			"#choice_" },		// 選択肢
			{ CMD_TYPE.CMD_FADE_IN_B,		"#blackin" },		// 黒フェードイン
			{ CMD_TYPE.CMD_FADE_OUT_B,		"#blackout" },		// 黒フェードアウト
			{ CMD_TYPE.CMD_SCO_END,			"#file_end" },		// シナリオ終了
		};

		public static char SCO_CMD_SPLIT = '_';	//-*シナリオコマンドの分割判定用文字列
#endregion	//-*SCENARIOCMMAND
	}
}