using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Const {
	/// <summary>
	/// リソース関連のディレクトリ
	/// </summary>
	public class Dir
	{
		/// <summary>
		/// シナリオの基本名
		/// </summary>
		public const string SCENARIO_BASE_NAME = "sco";
		/// <summary>
		/// シナリオの基本名
		/// </summary>
		public const string SCENARIO_EXTENSION = ".json";

		/// <summary>
		/// 背景画像の基本名
		/// </summary>
		public const string IMAGE_BG_BASE_NAME = "moema_bg";

		/// <summary>
		/// 背景画像の基本名
		/// </summary>
		public const string IMAGE_BGEV_BASE_NAME = "moema_event";
		/// <summary>
		/// キャラ画像の基本名
		/// </summary>
		public const string IMAGE_CHARA_BASE_NAME = "moema_chr";

		/// <summary>
		/// 顔画像の基本名
		/// 使用例:IMAGE_CHARA_BASE+(キャラ番号)+IMAGE_CHARA_FACE_BASE
		/// </summary>
		public const string IMAGE_CHARA_FACE_BASE_NAME = "a";

		/// <summary>
		/// BGMの基本名
		/// </summary>
		public const string SOUND_BGM_BASE_NAME = "mmg_bgm";

		/// <summary>
		/// SEの基本名
		/// </summary>
		public const string SOUND_SE_BASE_NAME = "mmg_se";


		/// <summary>
		/// リソースの場所
		/// </summary>
		public static string RESOURCES_DIRECTORY {
			get{ return String.Concat(Application.dataPath, "/Resources/");}
		} 
		
		/// <summary>
		/// シナリオで使用する画像の格納場所
		/// </summary>
		public static string ADV_IMAGE_DIRECTORY {
			get{ return "adventure/";}
			// get{ return String.Concat(RESOURCES_DIRECTORY, "adventure/");}
		}

		/// <summary>
		/// シナリオの場所
		/// </summary>
		public static string ADV_SCENARIO_DIRECTORY {
			// get{ return "scenario/";}
			get{ return String.Concat(RESOURCES_DIRECTORY, "scenario/");}
		} 

		/// <summary>
		/// サウンドの場所
		/// </summary>
		public static string SOUND_DIRECTORY {
			// get{ return "sound/";}
			get{ return String.Concat(RESOURCES_DIRECTORY, "sound/");}
		} 
	}

	/// <summary>
	/// リソース関連のディレクトリ
	/// </summary>
	public class Err
	{
		public enum ERRTYPE{
			ERR_NOT_ERR,		//-*正常(todo：コレでなくなったらエラーダイアログを出すようにしたい)
			ERR_SCO_CMD,		//-*不正なシナリオコマンド
			ERR_SCO_LINE_OVER,	//-*シナリオ行オーバー
			ERR_MAX,
		};
	}
}
