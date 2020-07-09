using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/// <summary>
/// 色々な宣言など
/// </summary>
namespace Const {
#region DIRECTORY
	/// <summary>
	/// リソース関連のディレクトリ
	/// </summary>
	public class Dir
	{
	#region ADVENTURE_FILE
	//-*****シナリオ関連
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
	#endregion //-*ADVENTURE_FILE

	#region INGAME_FILE
	//-*****麻雀関連
	
		/// <summary>
		/// 麻雀牌の基本名
		/// 使用例:IMAGE_TILE_BASE_NAME+00(牌番号)
		/// </summary>
		public const string IMAGE_TILE_BASE_NAME = "p";
	#endregion //-*INGAME_FILE

	#region SOUND_FILE
	//-*****サウンド関連
		/// <summary>
		/// BGMの基本名
		/// </summary>
		public const string SOUND_BGM_BASE_NAME = "mmg_bgm";

		/// <summary>
		/// SEの基本名
		/// </summary>
		public const string SOUND_SE_BASE_NAME = "mmg_se";
	#endregion //-*SOUND_FILE

	#region BASE
		/// <summary>
		/// リソースの場所
		/// </summary>
		public static string RESOURCES_DIRECTORY {
			get{ return String.Concat(Application.dataPath, "/Resources/");}
		} 
	#endregion //-*BASE
		
	#region ADVENTURE_DIR
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
	#endregion //-*ADVENTURE_DIR

	#region INGAME_DIR
		/// <summary>
		/// 麻雀で使用する画像の場所
		/// </summary>
		public static string MJ_DIRECTORY {
			get{ return "InGame/";}
		} 
		/// <summary>
		/// 麻雀で使用する牌画像の場所
		/// </summary>
		public static string MJ_TILE_DIRECTORY {
			// get{ return "scenario/";}
			// get{ return String.Concat(RESOURCES_DIRECTORY, MJ_DIRECTORY, "pai/");}
			get{ return String.Concat(MJ_DIRECTORY, "pai/");}
		} 
	#endregion //-*INGAME_DIR

	#region SOUND_DIR
		/// <summary>
		/// サウンドの場所
		/// </summary>
		public static string SOUND_DIRECTORY {
			// get{ return "sound/";}
			get{ return String.Concat(RESOURCES_DIRECTORY, "sound/");}
		} 
	#endregion //-*SOUND_DIR
	}
	
#endregion //-*DIRECTORY


#region ERROR
	/// <summary>
	/// todo:エラーダイアログ
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
#endregion //-*ERROR
}
