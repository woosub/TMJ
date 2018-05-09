using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageMgr : MonoBehaviour {


	public Button button;


	public GameObject[] block;

	LoadFile loadFile;

	public static bool isStart;


	List<SpriteRenderer> bgSpriteList = new List<SpriteRenderer> ();

	GameObject player;
	//const int 

	public void Replay()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene (0);
	}

	void Awake()
	{
		Screen.SetResolution (480, 840, false);		
	}

	// Use this for initialization
	void Start () {
		//DontDestroyOnLoad (gameObject);

		button.gameObject.SetActive (false);

		isStart = false;

		bgSpriteList = new List<SpriteRenderer> ();

		loadFile = FindObjectOfType<LoadFile> ();

		//Test
		LoadData ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!isStart)
			return;
		
		SpriteSorting ();
	}

	void SpriteSorting()
	{
		for (int i = 0; i < bgSpriteList.Count; i++) {
			if (player.transform.position.z >= bgSpriteList [i].transform.position.z) {
				bgSpriteList [i].sortingOrder += 2;

				bgSpriteList [i] = null;
			}

		}

		bgSpriteList.RemoveAll (n => n == null);
	}

	public void LoadData()
	{
		//맵
		loadFile.LoadMap ();

		//플레이어 캐릭터
		player = Player.LoadPlayer ().transform.Find("Control").gameObject;
		//장애물 오브젝트



		SpriteRenderer[] allSprite = FindObjectsOfType<SpriteRenderer> ();

		for (int i = 0; i < allSprite.Length; i++) {
			if (allSprite [i].gameObject.layer != LayerMask.NameToLayer ("Water")) {
				bgSpriteList.Add (allSprite [i]);
			}
		}

		StartCoroutine (PlayStart ());
	}


	IEnumerator PlayStart()
	{
		yield return new WaitForSeconds (0.5f);

		isStart = true;
	}

	public void Finish()
	{
		button.gameObject.SetActive (true);
	}
}
