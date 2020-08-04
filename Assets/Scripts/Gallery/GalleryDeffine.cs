using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

/// <summary>
/// シナリオで使用する共通定義
/// todo:ココを弄れば他ゲーへ換装がある程度楽になるようにしたい
/// </summary>
namespace GalleryDeffine
{
	public class GalDef
	{
		public enum IMG_TYPE {
			THUMBNAIL = 0,
			EVENTCG,
			MAX,
		}
		public const string IMAGE_THUMBNAIL_CLOSE = "a";
		public const string IMAGE_THUMBNAIL_CONNECT = "_";
		public const int EVENT_CG_NUM_MAX = 11;
		public const int THUMBNAIL_CONTAIN_NUM = 4;	//-*1枚絵内に含まれているサムネイル数

	#region IMAGE
		/// <summary>
		/// CGリスト< サムネイル画像名, CG画像名 >
		/// </summary>
		public static string[][] CGList = new string[][]
		{
			new string[]{"moema_gallery01","moema_event00"}, // 2:ＣＧ１～２戦目用上段左（ソファ座り）"
			new string[]{"moema_gallery01","moema_event01"}, // 0:ＣＧ１～２戦目用下段中（ソファ上膝立ち)"
			new string[]{"moema_gallery01","moema_event02"}, // 1:ＣＧ１～２戦目用下段右（着替え中）"
			new string[]{"moema_gallery01","moema_event03"}, // 3:ＣＧ１～２戦目用上段右(ソファ寝そべり)"
			new string[]{"moema_gallery02","moema_event04"}, // 4:ＣＧ体操着
			new string[]{"moema_gallery02","moema_event05"}, // 7:ＣＧ水着
			new string[]{"moema_gallery02","moema_event06"}, // 5:ＣＧチャイナ服
			new string[]{"moema_gallery02","moema_event07"}, // 8:ＣＧメイド服
			new string[]{"moema_gallery03","moema_event08"}, // 6:ＣＧバニーガール
			new string[]{"moema_gallery03","moema_event09"}, // 9:５戦め後お祭り萌
			new string[]{"moema_gallery03","moema_event10"}, //10:特典CG(嫁衣装)
		};
	#endregion	//-*IMAGE



	}
}