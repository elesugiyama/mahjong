using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



/// <summary>
/// スクリーン演出
/// フェードイン、アウトなど
/// </summary>
public class ScreenEffect : MonoBehaviour {

	private enum EFFECTSTATE {
		WAIT,
		START,
		END,
		MAX,
	};

	public enum EFFECTTYPE {
		NONE,
		FADE_IN_BLACK,
		FADE_OUT_BLACK,
		FADE_IN_WHITE,
		FADE_OUT_WHITE,
		QUAKE,
		MAX,
	};



	[SerializeField]
	private Image m_fade;

	private EFFECTSTATE m_state = EFFECTSTATE.WAIT;
	private EFFECTTYPE m_type = EFFECTTYPE.NONE;
	private List<EFFECTTYPE> m_EffectList =new List<EFFECTTYPE>();
	void Start () {}
	void Update () {}
	void Awake(){
		Init();
	}
	public void Init(){
		m_state = EFFECTSTATE.WAIT;
		m_type = EFFECTTYPE.NONE;
		m_EffectList.Clear();
	}
	public void SetEffect(EFFECTTYPE effect)
	{
		if(m_EffectList == null){
			m_EffectList = new List<EFFECTTYPE>();
		}
		m_EffectList.Add(effect);
	}
	private IEnumerator UpdateEffect(){
		bool isEndFade = false;
		while(true){
		Debug.Log("//-*UpdateEffect********************"+m_type);

			switch(m_type){
			case EFFECTTYPE.FADE_IN_BLACK:
			case EFFECTTYPE.FADE_IN_WHITE:
			case EFFECTTYPE.FADE_OUT_BLACK:
			case EFFECTTYPE.FADE_OUT_WHITE:
				if(FadeInOutProcess(m_type)){
					isEndFade = true;
				}
				break;
			case EFFECTTYPE.QUAKE:
				break;
			case EFFECTTYPE.NONE:
			default:
				break;
				// yield break;
			}
			if(isEndFade){
				EffectFinalize();
				yield break;
			}
			yield return null;
		}
	}

	private void EffectFinalize()
	{
		m_state = EFFECTSTATE.WAIT;
		m_type = EFFECTTYPE.NONE;
		m_EffectList.Clear();
		Debug.Log("//-*EffectFinalize********************");
	}
#region FADEINOUT
	//-******************
	//-*フェードインアウト
	//-******************

	private const float ONTIME_FADE = 0.01f;	//-*フェードインアウト時のアルファ値変化量
	private const float FADE_ALPHA_MAX = 1.0f;	//-*フェードインアウトのアルファ最大値
	private const float FADE_ALPHA_MIN = 0.0f;	//-*フェードインアウトのアルファ最小値
	

	/// <summary>
	/// フェードイン
	/// <param name="isWhite">白:true　黒:false</param>
	/// </summary>
	public IEnumerator FadeInStart(bool isWhite)
	{
		if(m_state != EFFECTSTATE.WAIT)yield break;
		m_state = EFFECTSTATE.START;
		if(isWhite){
			m_type = EFFECTTYPE.FADE_IN_WHITE;
			m_fade.color = Color.white;
		}else{
			m_type = EFFECTTYPE.FADE_IN_BLACK;
			m_fade.color = Color.black;
		}
		//-*初期アルファ値設定
		SetAlpha(FADE_ALPHA_MAX);

		yield return StartCoroutine("UpdateEffect");
		yield break;
	}

	/// <summary>
	/// フェードアウト
	/// </summary>
	public IEnumerator FadeOutStart(bool isWhite)
	{
		m_state = EFFECTSTATE.START;
		if(isWhite){
			m_type = EFFECTTYPE.FADE_OUT_WHITE;
			m_fade.color = Color.white;
		}else{
			m_type = EFFECTTYPE.FADE_OUT_BLACK;
			m_fade.color = Color.black;
		}
		//-*初期アルファ値設定
		SetAlpha(FADE_ALPHA_MIN);

		yield return StartCoroutine("UpdateEffect");
		yield break;
	}

	/// <summary>
	/// フェードインアウト実行中処理
	/// </summary>
	private bool FadeInOutProcess(EFFECTTYPE type){
		float setAlphaP = m_fade.color.a;
		switch(type){
		case EFFECTTYPE.FADE_IN_BLACK:
		case EFFECTTYPE.FADE_IN_WHITE:
			setAlphaP -= ONTIME_FADE;
			if(setAlphaP > FADE_ALPHA_MIN){
			//-*フェードイン中
				SetAlpha(setAlphaP);
			}else{
			//-*フェードイン終了
				SetAlpha(FADE_ALPHA_MIN);
				return true;
			}
			break;
		case EFFECTTYPE.FADE_OUT_BLACK:
		case EFFECTTYPE.FADE_OUT_WHITE:
			setAlphaP += ONTIME_FADE;
			
			if(setAlphaP < FADE_ALPHA_MAX){
			//-*フェードアウト中
				SetAlpha(setAlphaP);
			}else{
			//-*フェードアウト終了
				SetAlpha(FADE_ALPHA_MAX);
				return true;
			}
			break;
		default:
			Debug.LogError("FadeInOutProcess:typeErr:"+type);
			break;
		}
		return false;	//-*フェード中
	}

	/// <summary>
	/// アルファ値設定
	/// </summary>
	public void SetAlpha(float alpha)
	{
		var color = m_fade.color;
		color.a = alpha;
		m_fade.color = color;
	}
#endregion
}
