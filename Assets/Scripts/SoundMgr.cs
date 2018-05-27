using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMgr : MonoBehaviour {

    static SoundMgr m_this;

    [SerializeField]
    AudioSource bgm;

    [SerializeField]
    AudioSource effectSound;

    [SerializeField]
    AudioClip BGM;

    [SerializeField]
    AudioClip coin;

    [SerializeField]
    AudioClip card;

    [SerializeField]
    AudioClip damage;


    // Use this for initialization
    void Start () {

        if (m_this != null)
        {
            DestroyImmediate(gameObject);
            return;
        }

        m_this = this;
        DontDestroyOnLoad(gameObject);

        PlayBGM();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void PlayBGM()
    {
        if (m_this.BGM == null)
            return;

        if (m_this.bgm.isPlaying)
            return;

        m_this.bgm.clip = m_this.BGM;
        m_this.bgm.loop = true;
        m_this.bgm.Play();
    }

    public static void StopBGM()
    {
        if (m_this.bgm.clip == null)
            return;
        
        m_this.bgm.Stop();
    }

    public static void PlaySound(SoundType type)
    {
        switch (type)
        {
            case SoundType.coin:
                m_this.effectSound.clip = m_this.coin;
                break;

            case SoundType.card:
                m_this.effectSound.clip = m_this.card;
                break;

            case SoundType.damage:
                m_this.effectSound.clip = m_this.damage;
                break;
        }

        if (m_this.effectSound.clip == null)
            return;

        m_this.effectSound.loop = false;
        m_this.effectSound.Play();
    }


}

public enum SoundType
{
    coin,
    card,
    damage,
}