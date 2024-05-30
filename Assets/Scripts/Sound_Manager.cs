using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Manager : MonoBehaviour
{
    public static Sound_Manager instance;


    public AudioSource BgSource;
    public AudioSource EffectsSource;
    public AudioSource MatchEffectsSource;


    public AudioClip buttonClick,StarSound,ClockTimer,FreezeEffect,TileClick;
    public AudioClip LevelWin,LevelLose;

    public GameObject ad_manager;




    private void Awake()
    {
        ad_manager = GameObject.Find("AdsManager");

        MuteSounds();
        BgSource.Play();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
   

    public void PlayOnshootSound(AudioClip Clip_)
    {
        if (PlayerPrefs.GetInt(GameConstants.SFX) != 0)
        EffectsSource.PlayOneShot(Clip_);
    }
    public void PlayOnshootSoundMatch(AudioClip Clip_)
    {
        if (PlayerPrefs.GetInt(GameConstants.SFX) != 0)
            MatchEffectsSource.PlayOneShot(Clip_);
    }

    public void MuteSounds()
    {
        if (PlayerPrefs.GetInt(GameConstants.Music) == 0)
            BgSource.volume = 0.5f;
        else
            BgSource.volume =0f;

    }
}
