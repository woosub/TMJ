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
    List<SpriteRenderer> coinSpriteList = new List<SpriteRenderer>();

    int getCardCount = 0;

	GameObject player;
    //const int 

    [SerializeField]
    Sprite[] cardCaptureSprite;

    [SerializeField]
    Sprite[] getCardsSprite;

    [SerializeField]
    Sprite[] GameOver;

    [SerializeField]
    Sprite[] counts;

    [SerializeField]
    Image[] playTimer;

    [SerializeField]
    Image[] meter;

    [SerializeField]
    Image[] getCards;

    [SerializeField]
    Image gauge;

    [SerializeField]
    Image cardCapture;

    [SerializeField]
    GameObject FinishUI;

    [SerializeField]
    GameObject CardRes;

    [SerializeField]
    GameObject Coin;

    GameObject CardObj;

    [SerializeField]
    int CardGaugeLimit = 30;

    int cardGauge = 0;

    float playTime = 60f;

	public void Replay()
	{
		UnityEngine.SceneManagement.SceneManager.LoadScene (1);
	}

	void Awake()
	{
		Screen.SetResolution (480, 840, false);		
	}

	// Use this for initialization
	void Start () {
        //DontDestroyOnLoad (gameObject);
        goalDistance = 100f;
        getCardCount = 0;

        button.gameObject.SetActive (false);

		isStart = false;

		bgSpriteList = new List<SpriteRenderer> ();
        coinSpriteList = new List<SpriteRenderer>();

        loadFile = FindObjectOfType<LoadFile> ();

        CardObj = Instantiate(CardRes);
        CardObj.SetActive(false);

        FinishUI.SetActive(false);
        //Test
        LoadData ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!isStart)
			return;

        playTime -= Time.deltaTime;


        if (playTime > 0)
        {
            playTimer[0].sprite = counts[((int)playTime % 10)];
            playTimer[1].sprite = counts[((int)playTime / 10)];
        }
        else
        {
            playTimer[0].sprite = counts[0];
            playTimer[1].sprite = counts[0];

            TimeOver();
        }

        SpriteSorting ();
	}

    void TimeOver()
    {
        isStart = false;

        FinishUI.SetActive(true);
        FinishUI.GetComponent<Image>().sprite = GameOver[1];

        button.gameObject.SetActive(true);
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
			if (allSprite [i].gameObject.layer != LayerMask.NameToLayer ("Water") &&
                allSprite[i].gameObject.layer != LayerMask.NameToLayer("Coin")) {
				bgSpriteList.Add (allSprite [i]);
			}
		}

        for (int i = 0; i < allSprite.Length; i++)
        {
            if (allSprite[i].gameObject.layer == LayerMask.NameToLayer("Coin"))
                coinSpriteList.Add(allSprite[i]);
        }

		StartCoroutine (PlayStart ());
	}

    void CoinsActive(bool flag)
    {
        for (int i = 0; i < coinSpriteList.Count; i++)
        {
            coinSpriteList[i].gameObject.SetActive(flag);
        }
    }

	IEnumerator PlayStart()
	{
		yield return new WaitForSeconds (0.5f);

		isStart = true;
	}

	public void Finish()
	{
        FinishUI.SetActive(true);
        button.gameObject.SetActive (true);
	}

    public void CardGaugeUp()
    {
        cardGauge++;

        gauge.fillAmount = cardGauge / (float)CardGaugeLimit;

        if (CardGaugeLimit <= cardGauge)
        {
            cardGauge = 0;
            cardCapture.sprite = cardCaptureSprite[1];
            //카드 생성
            CardObj.SetActive(true);
            CardObj.transform.parent = player.transform.parent.Find("Card_" + Random.Range(0, 3));
            CardObj.transform.localPosition = Vector3.zero;
            //>

            CoinsActive(false);
        }
    }

    public void CardCapture()
    {
        CoinsActive(true);

        gauge.fillAmount = 0;
        cardCapture.sprite = cardCaptureSprite[0];

        getCards[getCardCount].sprite = getCardsSprite[1];

        getCardCount++;

        if (getCardCount > 3)
            getCardCount = 3;
    }

    float goalDistance = 100f;

    public void CalcMeter(float distance)
    {
        goalDistance -= distance;

        meter[0].sprite = counts[(int)goalDistance % 10];
        meter[1].sprite = counts[(int)goalDistance / 10 % 10];
        meter[2].sprite = counts[(int)goalDistance / 100 % 10];
        meter[3].sprite = counts[(int)goalDistance / 1000];
    }
}
