using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Assertions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Const;

//---------------------------------------------------------
/// <summary>
///  音声管理
/// </summary>
//---------------------------------------------------------
public class SoundManagerCtrl : MonoBehaviour {

	private enum SOUNDETYPE{
		TYPE_BGM,
		TYPE_SE,
		TYPE_VOICE,
		TYPE_MAX
	};

	[SerializeField]
	private Transform m_BGMTransform;
	
	[SerializeField]
	private Transform m_SETransform;
	
	[SerializeField]
	private Transform m_VoiceTransform;

	[SerializeField]
	private AudioSource m_audioSourceBgm;
	[SerializeField]
	private AudioSource m_audioSourceSe;
	[SerializeField]
	private AudioSource m_audioSourceVoice;

	void Start () {}
	void Update () {}




//-*todo：拾い物コピペ中
    //外部からの呼び出し用メソッド
    public void PlayBgm(string name)
    {
        StartCoroutine(LoadToAudioClipAndPlay(name, SOUNDETYPE.TYPE_BGM));
    }
	public void PlaySe(string name)
	{
        StartCoroutine(LoadToAudioClipAndPlay(name, SOUNDETYPE.TYPE_SE));
	}

    //ファイルの読み込み（ダウンロード）と再生
    IEnumerator LoadToAudioClipAndPlay(string name, SOUNDETYPE type)
    {
		String path = String.Concat(Dir.SOUND_DIRECTORY, name);
		Debug.Log("LoadToAudioClipAndPlay."+path);

        if (m_audioSourceBgm == null || string.IsNullOrEmpty(path))
            yield break;

        if (!File.Exists(path)) {
            //ここにファイルが見つからない処理
            Debug.Log("File not found.");
            yield break;
        }

        using(WWW www = new WWW("file://" + path))  //※あくまでローカルファイルとする
        {
            while (!www.isDone)
                yield return null;

            AudioClip audioClip = www.GetAudioClip(false, true);
            if (audioClip.loadState != AudioDataLoadState.Loaded)
            {
                //ここにロード失敗処理
                Debug.Log("Failed to load AudioClip.");
                yield break;
            }

            //ここにロード成功処理
			switch(type){
			case SOUNDETYPE.TYPE_BGM:
				m_audioSourceBgm.clip = audioClip;
				m_audioSourceBgm.Play();
				break;
			case SOUNDETYPE.TYPE_SE:
				m_audioSourceSe.clip = audioClip;
				m_audioSourceSe.Play();
				break;
			case SOUNDETYPE.TYPE_VOICE:
				m_audioSourceVoice.clip = audioClip;
				m_audioSourceVoice.Play();
				break;
			default:
            	Debug.LogError("Load type Miss("+type+") : " + path);
				break;
			}
            Debug.Log("Load success : " + path);
        }
    }

    // Use this for initialization
    // private void Start () {
    //     //起動時に読み込むときなど
    //     LoadAudio(path);   //※メインからの呼び出し例
    // }









}
