using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyData : MonoBehaviour {

	// Use this for initialization
	void Start () {}
	
	// Update is called once per frame
	void Update () {}
	void Awake() {
		// シーンまたぎに登録
		int numMusicPlayers = FindObjectsOfType<DontDestroyData>().Length;
		if (numMusicPlayers > 1)
		{
			Destroy(this);
		}
		else
		{
	        DontDestroyOnLoad(this);
		} 
	}

#region MOEMAHJONG
/// 萌え日記麻雀関連

	/// <summary>
	/// シナリオ：現在のシナリオ番号
	/// </summary>
	private int m_advScoNo;
	public int AdvScoNo {
		get { return m_advScoNo; }
		set { m_advScoNo = value; }
	}

	/// <summary>
	/// シナリオ：次シナリオ番号
	/// </summary>
	private int m_advNextScoNo;
	public int AdvNextScoNo {
		get { return m_advNextScoNo; }
		set { m_advNextScoNo = value; }
	}

	/// <summary>
	/// シナリオ：現在の背景番号
	/// </summary>
	private string m_advBgNo;
	public string AdvBgNo {
		get { return m_advBgNo; }
		set { m_advBgNo = value; }
	}

	/// <summary>
	/// 麻雀：ステージ番号
	/// </summary>
	private int m_BattleStage;
	public int BattleStage {
		get { return m_BattleStage; }
		set { m_BattleStage = value; }
	}

	/// <summary>
	/// 麻雀：ルール
	/// </summary>
	private int m_BattleRule;
	public int BattleRule {
		get { return m_BattleRule; }
		set { m_BattleRule = value; }
	}

	/// <summary>
	/// サウンド
	/// </summary>
	[SerializeField]
	private SoundManagerCtrl m_SoundCtl;
	public SoundManagerCtrl SoundCtl {
		get { return m_SoundCtl; }
	}
#endregion	//-*MAHJONG

}
