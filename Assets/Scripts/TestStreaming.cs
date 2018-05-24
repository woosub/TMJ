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

	// Use this for initialization
	void Start () {
        //vp.url = "http://www.quirksmode.org/html5/videos/big_buck_bunny.mp4"; //Application.dataPath + "/StreamingAssets/test01.mp4";
        //vp.Play();
        // ((MovieTexture)GetComponent<Renderer>().material.mainTexture).Play();
        //Handheld.

        mRenderer = GetComponent<Renderer>();
        textures = Resources.LoadAll<Texture2D>("jpgs");

        
    }

    void OnEnable()
    {
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
                if (frame < 429)
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

        StageMgr sm = FindObjectOfType<StageMgr>();

        if (sm.isFinish)
        {
            sm.Continue();
        }
        else
        {
            sm.Restart();
        }
        //FindObjectOfType<StageMgr>().Restart();
       // Debug.Log("Finish");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
