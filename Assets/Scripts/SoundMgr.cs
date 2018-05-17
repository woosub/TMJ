using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMgr : MonoBehaviour {

    

    [SerializeField]
    AudioClip bgm;

    [SerializeField]
    AudioClip coin;

    [SerializeField]
    AudioClip card;

    [SerializeField]
    AudioClip damage;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void PlaySound()
    {

    }


}

public enum SoundType
{
    coin,
    card,
    damage,
}