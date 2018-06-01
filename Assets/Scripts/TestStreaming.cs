using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Video;

public class TestStreaming : MonoBehaviour {

    //[SerializeField]
    //VideoPlayer vp;

    Renderer mRenderer;
    Texture2D[] textures;

    [SerializeField]
    AudioSource source;

    [SerializeField]
    AudioClip m1;

    [SerializeField]
    AudioClip m2;

    [SerializeField]
    AudioClip m3;


    // Use this for initialization
    void Start () {
        //vp.url = "http://www.quirksmode.org/html5/videos/big_buck_bunny.mp4"; //Application.dataPath + "/StreamingAssets/test01.mp4";
        //vp.Play();
        // ((MovieTexture)GetComponent<Renderer>().material.mainTexture).Play();
        //Handheld.

        mRenderer = GetComponent<Renderer>();
        
    }

    void OnEnable()
    {
        int n = Random.Range(0, 3);
        textures = Resources.LoadAll<Texture2D>("movie" + (n + 1));

        if (n == 0)
        {
            source.clip = m1;
        }
        else if (n == 1)
        {
            source.clip = m2;
        }
        else
        {
            source.clip = m3;
        }
        source.loop = false;


        SoundMgr.StopBGM();
        StartCoroutine(StartFrame());
    }

    IEnumerator StartFrame()
    {
        float sec = 0.0f;
        int frame = 0;

        source.Play();

        while (true)
        {
            yield return null;

            mRenderer.sharedMaterial.mainTexture = textures[frame];

            sec += Time.deltaTime;

            if (sec >= 0.09f)
            {
                sec = 0.0f;
                if (frame < textures.Length - 1)
                {
                    frame++;
                }
                else
                {
                    break;
                }
            }
        }

        yield return new WaitForSeconds(0.2f);


        SoundMgr.PlayBGM();
        StageMgr sm = FindObjectOfType<StageMgr>();

        //if (sm.isFinish)
        //{
        if (sm.isRestart)
        {
            sm.RestartGo();
        }
        else
        {
            sm.Continue();
        }
        
        //}
        //else
        //{
        //    sm.Restart();
        //}
        //FindObjectOfType<StageMgr>().Restart();
        // Debug.Log("Finish");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
