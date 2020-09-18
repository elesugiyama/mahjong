using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//-********
using MahjongDeffine;
// using MJDefsHeader;
// using GameDefsHeader;
// using MJDialogHeader;
using GalleryDeffine;
using Const;
//-********
public class Gallery : SceneBase {

//-----------------------------------------------
// ギャラリーメニューの定義
//-----------------------------------------------
	private enum GALLERYMODE{ // menu処理全体
		mMODE_INIT,			//-*初期化
		mMODE_THUMBNAIL,	//-*サムネイル一覧
		mMODE_EVENTCG,		//-*サムネイル一覧
		tMODE_NEXT_SCENE,	//-*次シーンへ
#region KEY_TEST
		tMODE_OPTION,
#endregion //-*KEY_TEST
		mMODE_MAX,		//*最大数
	};

	[SerializeField]
	private GameObject m_scrollViewBox;
	[SerializeField]
	private GameObject[] m_thumbnailButton;

	[SerializeField]
	private GameObject m_ScrollViewBase;

	[SerializeField]
	private GameObject m_eventCgBox;
	[SerializeField]
	private GameObject m_eventCg;

	[SerializeField]
	private ButtonCtl m_BtnTitleBack = null;

	private GALLERYMODE m_Mode = GALLERYMODE.mMODE_INIT;	//-*モード
	private bool m_isThumbnailSelect = false;
	private bool m_isBack = false;
	private int m_selThumbnailNo = -1;
	
#region GAME_PAD
	[Header("ゲームパッド関連")]
	[SerializeField]
	private GameObject m_galleryCursol=null;
	[SerializeField]
	private List<GameObject> m_thumbnailObj = new List<GameObject>();		//-*TITLE_SELECTと連動させること
	[SerializeField]
	private GameObject m_TitleBackObj = null;
	[SerializeField]
	private ScrollRect ScrollRect;
	private int m_thumbnailInFrameNo = 0;

	private GALLERYMODE m_ModeTemp = GALLERYMODE.mMODE_INIT;	//-*モード

#endregion //-*GAME_PAD
	// Use this for initialization
	protected override void Start () {
		m_Mode = GALLERYMODE.mMODE_INIT;
	}
	private void  Init(){

        GameObject obj = (GameObject)Resources.Load("Prefabs/gallery_thumbnail_button");

		m_thumbnailButton = new GameObject[GalDef.EVENT_CG_NUM_MAX];

        // プレハブを元にオブジェクトを生成する
		for(int i = 0;i < GalDef.EVENT_CG_NUM_MAX;i++){
			m_thumbnailButton[i] = (GameObject)Instantiate(obj,
								Vector3.zero,
								Quaternion.identity);
			//-*スクロールビューに登録
			m_thumbnailButton[i].transform.SetParent(m_ScrollViewBase.transform, false);
			//-*ボタン設定
			var BCtl = m_thumbnailButton[i].GetComponents<GalleryButton>();
			if(BCtl != null){
				BCtl[0].SetOnPointerClickCallback(ButtonSelectThumbnail);
				string imageName = string.Concat(Dir.GAL_IMAGE_DIRECTORY,GalDef.CGList[i][(int)GalDef.IMG_TYPE.THUMBNAIL]);
				var spriteImage = Resources.LoadAll<Sprite>(imageName);
				int imgNo = i%GalDef.THUMBNAIL_CONTAIN_NUM;
				BCtl[0].SetImage(i,spriteImage[imgNo]);
			}
#region GAME_PAD
			m_thumbnailObj.Add(m_thumbnailButton[i].transform.gameObject);
#endregion //-*GAME_PAD
    	}
		if(m_BtnTitleBack != null) m_BtnTitleBack.SetOnPointerClickCallback(ButtonTitleBack);

#region GAME_PAD
		m_selThumbnailNo = 0;
		m_thumbnailInFrameNo = 0;
		m_galleryCursol.SetActive(true);
		m_galleryCursol.transform.SetParent(m_thumbnailObj[m_selThumbnailNo].transform);
		m_galleryCursol.transform.localPosition = new Vector3(0.0f,400.0f,0.0f);
#endregion //-*GAME_PAD
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update();
#region KEY_TEST
		if(m_keepData.IsOptionOpen){
			if(m_Mode != GALLERYMODE.tMODE_OPTION){
				m_ModeTemp = m_Mode;
			}
			m_Mode = GALLERYMODE.tMODE_OPTION;
		}
#endregion //-*KEY_TEST
		switch(m_Mode){
		case GALLERYMODE.mMODE_INIT:
			DebLog("//-*mMODE_INIT");
			Init();
			m_Mode = ModeSet(GALLERYMODE.mMODE_THUMBNAIL);
			break;
		case GALLERYMODE.mMODE_THUMBNAIL:
			m_Mode = UpdateSelectThumbnail();
			break;
		case GALLERYMODE.mMODE_EVENTCG:
			m_Mode = UpdateEventCG();
			break;
		case GALLERYMODE.tMODE_NEXT_SCENE:
			UpdateNextScene();
			break;
#region KEY_TEST
		case GALLERYMODE.tMODE_OPTION:
			if(!m_keepData.IsOptionOpen){
				m_Mode = m_ModeTemp;
			}
			break;
#endregion //-*KEY_TEST
		default:
			DebLog("//-*Err:"+m_Mode);

			break;
		}
	}

	private void InitThumbnail(){
		m_isBack = false;
		m_isThumbnailSelect = false;
		if(m_eventCgBox.activeSelf){
		//-*todo:CG→サムネ一覧
		}
		m_eventCgBox.SetActive(false);
		m_scrollViewBox.SetActive(true);
	}
	private GALLERYMODE UpdateSelectThumbnail()
	{
#region GAME_PAD
		if(IsKeyAxisButton(KEY_NAME.LEFT)){
			m_selThumbnailNo--;
			if(m_selThumbnailNo < 0){
				m_selThumbnailNo = 0;
			}else if(m_selThumbnailNo >= m_thumbnailObj.Count){
				m_selThumbnailNo = (m_thumbnailObj.Count-1);
			}
//-**********
			m_thumbnailInFrameNo--;
			if(m_selThumbnailNo < (m_thumbnailObj.Count-GalDef.THUMBNAI_IN_SCREEN_NUM) && m_thumbnailInFrameNo < 0){
				var scrollPos = 1f/( (float)m_thumbnailObj.Count - (float)GalDef.THUMBNAI_IN_SCREEN_NUM);
				ScrollRect.horizontalNormalizedPosition = ScrollRect.horizontalNormalizedPosition - scrollPos ;
			}
			if(m_thumbnailInFrameNo < 0)m_thumbnailInFrameNo = 0;
//-**********
			//-*カーソル移動
			m_galleryCursol.transform.SetParent(m_thumbnailObj[m_selThumbnailNo].transform);
			m_galleryCursol.transform.localPosition = GalDef.CURSOL_POS;
		}else
		if(IsKeyAxisButton(KEY_NAME.RIGHT)){
			m_selThumbnailNo++;
			if(m_selThumbnailNo < 0){
				m_selThumbnailNo = 0;
			}else if(m_selThumbnailNo >= m_thumbnailObj.Count){
				m_selThumbnailNo = (m_thumbnailObj.Count-1);
			}
//-**********			
			m_thumbnailInFrameNo++;
			if(m_selThumbnailNo > 2 && m_thumbnailInFrameNo > 2){
				var scrollPos = 1f/( (float)m_thumbnailObj.Count - 3f);
				ScrollRect.horizontalNormalizedPosition = ScrollRect.horizontalNormalizedPosition + scrollPos ;
			}
			if(m_thumbnailInFrameNo >= 3)m_thumbnailInFrameNo = 2;
			//-*カーソル移動
			m_galleryCursol.transform.SetParent(m_thumbnailObj[m_selThumbnailNo].transform);
			m_galleryCursol.transform.localPosition = GalDef.CURSOL_POS;
		}
		else if(IsKeyBtnPress(KEY_NAME.SELECT,true)){
			m_isThumbnailSelect = true;
		}
		else if(IsKeyBtnPress(KEY_NAME.BACK,true)){
			m_isBack = true;
			ButtonTitleBack();
		}
#endregion //-*GAME_PAD


		if(m_isThumbnailSelect){
			InitEventCG();
			return GALLERYMODE.mMODE_EVENTCG;
		}
		else if(m_isBack){
			return GALLERYMODE.tMODE_NEXT_SCENE;
		}
		return GALLERYMODE.mMODE_THUMBNAIL;
	}

	private void InitEventCG(){
		m_isBack = false;
		m_isThumbnailSelect = false;
		m_eventCgBox.SetActive(true);
		m_scrollViewBox.SetActive(false);
		if(m_eventCg == null || m_selThumbnailNo < 0 ) return;
		String imageName = null;
		imageName = String.Concat(Dir.ADV_IMAGE_DIRECTORY, GalDef.CGList[m_selThumbnailNo][(int)GalDef.IMG_TYPE.EVENTCG]);
		Debug.Log("//-*BGImage(Ev):"+imageName);
		var spriteImage = Resources.Load<Sprite>(imageName);
		if(spriteImage == null) return;
		m_eventCg.GetComponent<Image>().sprite = spriteImage;
	}
	private GALLERYMODE UpdateEventCG()
	{
#region //-*GAME_PAD
		if(IsKeyBtnPress(KEY_NAME.SELECT,true) || IsKeyBtnPress(KEY_NAME.BACK,true))
		{
			m_isBack = true;
			ButtonTitleBack();
		}
#endregion //-*GAME_PAD
		if(m_isBack){
			// m_selThumbnailNo = -1;	//-初期化
			InitThumbnail();
			return GALLERYMODE.mMODE_THUMBNAIL;
		}

		return GALLERYMODE.mMODE_EVENTCG;
	}
	private void UpdateNextScene()
	{
		SceneChange();
	}

	// //---------------------------------------------------------
	// /// <summary>
	// /// クリック
	// /// </summary>
	// //---------------------------------------------------------
	public void ButtonSelectThumbnail(int a)
	{
		// DebLog("//-*ButtonTest:"+a);
		// m_keepData.AdvNextScoNo = a;
		// m_nextSceneName = SceneNameDic[SCENE_NAME.INGAME];
		// m_isStageSelect = true;
		// SceneChange("InGame");
		m_isThumbnailSelect = true;
		m_selThumbnailNo = a;
	}
	public void ButtonTitleBack()
	{
		switch(m_Mode){
		case GALLERYMODE.mMODE_EVENTCG:
			m_isBack = true;
			break;
		case GALLERYMODE.mMODE_THUMBNAIL:
		default:
			m_nextSceneName = SceneNameDic[SCENE_NAME.TITLE];
			m_isBack = true;
			break;
		}
	}
}
