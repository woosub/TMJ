using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Title : MonoBehaviour {

    bool isReady = false;
    bool isShowRegionList = false;

    [SerializeField]
    GameObject regionListObj2;

    [SerializeField]
    GameObject regionListObj;

    [SerializeField]
    GameObject buttonObj;

    [SerializeField]
    GameObject pressAnyKey;

    [SerializeField]
    Image cartoon;

    [SerializeField]
    GameObject cartoonBtn;

    [SerializeField]
    Sprite[] cartoonPage;

    [SerializeField]
    GameObject backButton;

    [SerializeField]
    Scrollbar scrollbar;

    [SerializeField]
    GameObject skipButton;

    int cartoonCnt = 0;

    bool pressAnyKeyFlag = false;

    const int gap = 100;
    const int startPos = 200;
    const int defaultCnt = 4;
    const int defaultGap = 50;

    // Use this for initialization
    IEnumerator Start() {
        cartoon.gameObject.SetActive(false);
        cartoonBtn.SetActive(false);

        cartoonCnt = 0;

        yield return new WaitForSeconds(0.05f);

        isReady = false;
        isShowRegionList = false;

        DataMgr.LoadRegionInfo();
        SetLoadBigRegion();

        yield return new WaitForSeconds(1.0f);

        isReady = true;

        InvokeRepeating("PressAnyKey", 0.0f, 0.4f);
    }

    void PressAnyKey()
    {
        pressAnyKeyFlag = !pressAnyKeyFlag;
        pressAnyKey.SetActive(pressAnyKeyFlag);
    }


    public void SetLoadBigRegion()
    {
        backButton.SetActive(false);
        Transform tr = regionListObj.transform.Find("Viewport").Find("Content");

        Button[] btns = tr.GetComponentsInChildren<Button>();
        for (int i = 0; i < btns.Length; i++)
        {
            DestroyImmediate(btns[i].gameObject);
        }

        tr.GetComponent<RectTransform>().offsetMin = new Vector2(tr.GetComponent<RectTransform>().offsetMin.x, 260.9599f);
        tr.GetComponent<RectTransform>().offsetMax = new Vector2(tr.GetComponent<RectTransform>().offsetMax.x, -22.95993f);

        scrollbar.value = 0.0f;
        scrollbar.size = 1.0f;

        List<string> list = DataMgr.bigRegionList;

        int firstPos = startPos + (Mathf.Max(defaultCnt, list.Count) - defaultCnt) * defaultGap;

        GameObject button;

        tr.GetComponent<RectTransform>().offsetMin =
            new Vector2(tr.GetComponent<RectTransform>().offsetMin.x
                , -(gap * (Mathf.Max(defaultCnt, list.Count) - defaultCnt)));

        int testInt = 0;
        
        for (int i = 0; i < list.Count; i++)
        {
            button = Instantiate(buttonObj);

            button.name = i.ToString();

            button.GetComponentInChildren<Text>().text = list[i];
            button.transform.SetParent(tr);
            button.transform.localScale = Vector3.one * 1.5f;

            button.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, firstPos - (i * gap));

			int v = testInt++;

            button.GetComponent<Button>().onClick.AddListener(delegate {
                //str = ;
				SetLoadRegion(v); });

        }
    }

    public void SetLoadRegion(int idx)
    {
        backButton.SetActive(true);
        Transform tr = regionListObj.transform.Find("Viewport").Find("Content");

        Button[] btns = tr.GetComponentsInChildren<Button>();
        for (int i = 0; i < btns.Length; i++)
        {
            DestroyImmediate(btns[i].gameObject);
        }

        tr.GetComponent<RectTransform>().offsetMin = new Vector2(tr.GetComponent<RectTransform>().offsetMin.x, 260.9599f);
        tr.GetComponent<RectTransform>().offsetMax = new Vector2(tr.GetComponent<RectTransform>().offsetMax.x, -22.95993f);

        scrollbar.value = 0.0f;
        scrollbar.size = 1.0f;

        List<RegionData> list = DataMgr.regionList.FindAll(n=>n.region == DataMgr.bigRegionList[idx]);

		int firstPos = startPos + (Mathf.Max(defaultCnt, list.Count) - defaultCnt) * defaultGap;

		GameObject button;

		tr.GetComponent<RectTransform>().offsetMin = 
			new Vector2(tr.GetComponent<RectTransform>().offsetMin.x
				, -(gap * (Mathf.Max(defaultCnt, list.Count) - defaultCnt)));

        int testInt = list[0].index;
		 
		for (int i = 0; i<list.Count; i++) {
			button = Instantiate (buttonObj);

            button.name = i.ToString();

            button.GetComponentInChildren<Text> ().text = list [i].region2;
			button.transform.SetParent(tr);
            button.transform.localScale = Vector3.one * 1.5f;

            button.GetComponent<RectTransform>().anchoredPosition3D = new Vector3 (0, firstPos - (i * gap));

			int v = testInt++;
            
            button.GetComponent<Button>().onClick.AddListener(delegate { Select(v); });

        }
	}


    public void Select(int idx)
    {
        DataMgr.currentRegion = DataMgr.regionList[idx];

        if (DataMgr.isCartoon)
        {
            Invoke("NextScene", 0.5f);
        }
        else
        {
            Invoke("PlayCartoon", 0.5f);
        }
    }

    public void NextScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    void PlayCartoon()
    {
        cartoon.gameObject.SetActive(true);
        backButton.SetActive(false);
        
        NextPage();
    }

    public void NextPage()
    {
        if (cartoonCnt >= cartoonPage.Length)
        {
            DataMgr.isCartoon = true;
            Invoke("NextScene", 0.5f);
            return;
        }

        cartoonBtn.SetActive(false);
        cartoon.sprite = cartoonPage[cartoonCnt];

        cartoonCnt++;

        Invoke("NextButton", 1.5f);
    }

    void NextButton()
    {
        skipButton.SetActive(true);
        cartoonBtn.SetActive(true);
    }


    // Update is called once per frame
    void Update () {
		if (!isReady)
			return;

		if (isShowRegionList)
			return;
        
        if (Input.anyKey)
        {
            //UnityEngine.SceneManagement.SceneManager.LoadScene(1);
            CancelInvoke("PressAnyKey");
            pressAnyKey.SetActive(false);

            isShowRegionList = true;

			regionListObj.SetActive (true);
            regionListObj2.SetActive(true);
        }
	}


}