using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class StageMgr : MonoBehaviour {


	public Button button;  //replay
    public Button button2; //opencard
    public Button button3; //공약보기
    public Button button4; //Go to Main
    public Button button5; //Go to Hidden


    public GameObject[] block;

	public static bool isStart;


	List<SpriteRenderer> bgSpriteList = new List<SpriteRenderer> ();
    List<SpriteRenderer> coinSpriteList = new List<SpriteRenderer>();

    int getCardCount = 0;

	GameObject player;
    //const int 

    [SerializeField]
    bool isHidden;

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
	GameObject cardEffect;

    [SerializeField]
    GameObject FinishUI;

    [SerializeField]
    GameObject CardRes;

    [SerializeField]
    GameObject Coin;

    GameObject CardObj;

    [SerializeField]
    int CardGaugeLimit = 30;

    [SerializeField]
    GameObject Background;

    [SerializeField]
    VideoPlayer vp;

    [SerializeField]
    GameObject viewCard;

    [SerializeField]
    Transform createCardPos;

    [SerializeField]
    Transform[] cardMovePos_2;

    [SerializeField]
    Transform[] cardMovePos_3;

    [SerializeField]
    GameObject ready;

    [SerializeField]
    GameObject go;

    int cardGauge = 0;

    float playTime = 60f;
    
	public void Replay()
	{
        //UnityEngine.SceneManagement.SceneManager.LoadScene (1);
        vp.enabled = true;

        FinishUI.SetActive(false);
        button.gameObject.SetActive(false);

        Background.SetActive(true);

        vp.loopPointReached += Vp_loopPointReached;

        vp.Play();

    }

    private void Vp_loopPointReached(VideoPlayer source)
    {
        Invoke("Restart", 0.5f);        
    }

    void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    void Awake()
	{
        Application.targetFrameRate = -1;

        Screen.SetResolution (320, 560, false);		
	}

	// Use this for initialization
	IEnumerator Start () {
        //DontDestroyOnLoad (gameObject);

		CardEffectOff ();
        goalDistance = 100f;
        getCardCount = 0;

        button.gameObject.SetActive (false);
        button2.gameObject.SetActive(false);
        button3.gameObject.SetActive(false);
        button4.gameObject.SetActive(false);
        button5.gameObject.SetActive(false);

        isStart = false;

		bgSpriteList = new List<SpriteRenderer> ();
        coinSpriteList = new List<SpriteRenderer>();

        CardObj = Instantiate(CardRes);
        CardObj.SetActive(false);

        FinishUI.SetActive(false);

		yield return new WaitForSeconds (0.05f);
        //Test
        LoadData ();

        //StartCardAnimation();

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

            TimeOverFunc();
        }

        SpriteSorting ();
	}

    void TimeOverFunc()
    {
        isStart = false;

        FinishUI.SetActive(true);
        FinishUI.GetComponent<Image>().sprite = GameOver[1];

        button.gameObject.SetActive(true);
    }

    public void GameOverFunc()
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
		LoadFile.LoadMap ();

		//플레이어 캐릭터
		player = Player.LoadPlayer ().transform.Find("Control").gameObject;
        
        vp.enabled = false;
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
        int cardCnt = isHidden ? 1 : DataMgr.currentRegion.nameList.Count;
        getCards[0].gameObject.SetActive(0 < cardCnt);
        getCards[1].gameObject.SetActive(1 < cardCnt);
        getCards[2].gameObject.SetActive(2 < cardCnt);
        getCards[3].gameObject.SetActive(3 < cardCnt);


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
        ready.SetActive(true);

        yield return new WaitForSeconds(1.0f);
        ready.SetActive(false);
        go.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        go.SetActive(false);
        isStart = true;
	}

	public void Finish()
	{
        FinishUI.SetActive(true);
        if ((isHidden ? 1 : DataMgr.currentRegion.nameList.Count) > getCardCount)
        {
            FinishUI.GetComponent<Image>().sprite = GameOver[2];
            button.gameObject.SetActive(true);
        }
        else
        {
            button2.gameObject.SetActive(true);
        }
	}

    public void CardGaugeUp()
    {
        cardGauge++;

        gauge.fillAmount = cardGauge / (float)CardGaugeLimit;

        if (CardGaugeLimit <= cardGauge)
        {
            cardGauge = 0;
            cardCapture.sprite = cardCaptureSprite[1];

			cardEffect.SetActive (true);
			Invoke ("CardEffectOff", 0.2f);

            //카드 생성
            CardObj.SetActive(true);
            CardObj.transform.parent = player.transform.parent.Find("Card_" + Random.Range(0, 3));
			CardObj.transform.localPosition = Vector3.forward * 2;
            CardObj.transform.localScale = Vector3.one;
            //>

            CardObj.GetComponentInChildren<Collider>().enabled = false;

            StartCoroutine (MovingCard ());

            CoinsActive(false);
        }
    }

	IEnumerator MovingCard()
	{
		float f = 0.0f;
		while (true) {
			yield return null;
			f += Time.deltaTime;
			CardObj.transform.localPosition = Vector3.Lerp (Vector3.forward * 2, Vector3.zero, f);

			if (f >= 1.0f)
				break;
		}

        CardObj.GetComponentInChildren<Collider>().enabled = true;
        CardObj.transform.localPosition = Vector3.zero;
	}

	void CardEffectOff()
	{
		cardEffect.SetActive (false);
	}

    public void CardCapture()
    {
        CoinsActive(true);

        gauge.fillAmount = 0;
        cardCapture.sprite = cardCaptureSprite[0];

        StartCoroutine(CardCaptureCoroutine());
    }

    IEnumerator CardCaptureCoroutine()
    {
        if (getCardCount > 3)
            yield break;

        yield return new WaitForSeconds(0.5f);
        getCards[getCardCount].sprite = getCardsSprite[1];
        getCards[getCardCount].color = Color.white * 2;

        yield return new WaitForSeconds(0.2f);
        getCards[getCardCount].color = Color.white;

        getCardCount++;
    }

    float goalDistance = 100f;

    public void CalcMeter(float distance)
    {
        goalDistance -= distance;

        if (goalDistance >= 0.0f)
        {
            meter[0].sprite = counts[(int)goalDistance % 10];
            meter[1].sprite = counts[(int)goalDistance / 10 % 10];
            meter[2].sprite = counts[(int)goalDistance / 100 % 10];
            meter[3].sprite = counts[(int)goalDistance / 1000];
        }
    }

    int getCardCounting = 0;

    public void StartCardAnimation()
    {
        FinishUI.SetActive(false);
        button2.gameObject.SetActive(false);
        Background.SetActive(true);

        //카드 등장 및 오픈 연출
        getCardCounting = 0;

        StartCoroutine(downloadTexture());

        //>

    }
    
    Sprite hiddenTex;
    GameObject vCard;


    IEnumerator downloadTexture()
    {
        WWW www = new WWW(isHidden ? "http://theminjoo.einvention.kr/test/test2_f.jpg" : "http://theminjoo.einvention.kr/test/test1_f.jpg");
        
        yield return www;

        WWW www2 = new WWW("http://theminjoo.einvention.kr/test/test1_b.jpg");

        yield return www2;

        if (isHidden)
        {
            WWW www3 = new WWW("http://theminjoo.einvention.kr/test/test2_b.jpg");

            yield return www3;

            Texture2D tempTex = www3.texture;
            hiddenTex = Sprite.Create(tempTex, new Rect(0, 0, tempTex.width, tempTex.height), new Vector2(0.5f, 0.5f));

            www3.Dispose();
        }

        if (www.error == null && www2.error == null)
        {
            Texture2D tex = www.texture;
            Texture2D tex2 = www2.texture;
            vCard = Instantiate(viewCard);

            vCard.transform.SetParent(createCardPos);
            vCard.transform.localScale = Vector3.one;
            vCard.transform.position = createCardPos.position;
            vCard.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0);

            vCard.GetComponent<Image>().sprite = Sprite.Create(tex2, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));

            yield return new WaitForSeconds(0.5f);

            float val = 0.0f;

            while (true)
            {
                yield return null;
                val += Time.deltaTime * 2;
                
                vCard.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, val);

                if (val >= 1.0f)
                {
                    vCard.GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                    break;
                }
            }

            while (true)
            {
                yield return null;
                val -= Time.deltaTime * 2;

                vCard.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, val);

                if (val <= 0.0f)
                {
                    break;
                }
            }

            if (!isHidden)
            {
                yield return new WaitForSeconds(0.5f);

                val = 0.0f;
                Vector3 currentCardPos = vCard.transform.position;
                Vector3 destCardScale = Vector3.one * 0.5f;

                while (true)
                {
                    yield return null;
                    val += Time.deltaTime * 3;

                    vCard.transform.position = Vector3.Lerp(currentCardPos, cardMovePos_2[getCardCounting].position, val);
                    vCard.transform.localScale = Vector3.Lerp(Vector3.one, destCardScale, val);

                    if (val >= 1.0f)
                    {
                        break;
                    }
                }
            }


        }

        www.Dispose();
        www2.Dispose();

        getCardCounting++;

        if (!isHidden)
        {
            if (DataMgr.currentRegion.nameList.Count > getCardCounting)
            {
                yield return StartCoroutine(downloadTexture());
            }
        }

        yield return new WaitForSeconds(0.8f);

        if (isHidden)
        {
            button3.gameObject.SetActive(true);
        }
        else
        {
            button5.gameObject.SetActive(true);
        }

    }

    public void OpenHiddenTexture()
    {
        button3.gameObject.SetActive(false);
        StartCoroutine(OpenHiddenTextureCoroutine());
    }

    IEnumerator OpenHiddenTextureCoroutine()
    {
        float val = 0.0f;

        while (true)
        {
            yield return null;
            val += Time.deltaTime * 2;

            vCard.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, val);

            if (val >= 1.0f)
            {
                vCard.GetComponent<Image>().sprite = hiddenTex;
                break;
            }
        }

        while (true)
        {
            yield return null;
            val -= Time.deltaTime * 2;

            vCard.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, val);

            if (val <= 0.0f)
            {
                break;
            }
        }

        yield return new WaitForSeconds(0.5f);

        button4.gameObject.SetActive(true);
    }

    public void GoToHidden()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(2);
    }

    public void GoToMain()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
