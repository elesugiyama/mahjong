using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D;

public class spriteUINumS : MonoBehaviour {
	[SerializeField]
	private SpriteAtlas _uiNumSAtlas = null;

	public void init(string file_name)
	{
		// 画像設定
		SpriteAtlas chip_atlas = _uiNumSAtlas;//-*Resources.Load<SpriteAtlas>("Resources/UINumS");
		this.GetComponent<SpriteRenderer>().sprite = chip_atlas.GetSprite(file_name);

		// ソート順
		this.GetComponent<SpriteRenderer>().sortingOrder = 1;
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
