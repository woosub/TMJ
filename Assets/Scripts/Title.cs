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

    bool pressAnyKeyFlag = false;

	const int gap = 100;
	const int startPos = 200; 
	const int defaultCnt = 4;
	const int defaultGap = 50;
    
    // Use this for initialization
    IEnumerator Start () {
        
        yield return new WaitForSeconds (0.05f);

		isReady = false;
		isShowRegionList = false;

		DataMgr.LoadRegionInfo ();
		SetLoadRegion ();

		yield return new WaitForSeconds (1.0f);

		isReady = true;

        InvokeRepeating("PressAnyKey", 0.0f, 0.4f);
	}

    void PressAnyKey()
    {
        pressAnyKeyFlag = !pressAnyKeyFlag;
        pressAnyKey.SetActive(pressAnyKeyFlag);
    }


    void SetLoadRegion()
	{
		List<RegionData> list = DataMgr.regionList;

		int firstPos = startPos + (Mathf.Max(defaultCnt, list.Count) - defaultCnt) * defaultGap;

		GameObject button;
		Transform tr = regionListObj.transform.Find ("Viewport").Find ("Content");

		tr.GetComponent<RectTransform>().offsetMin = 
			new Vector2(tr.GetComponent<RectTransform>().offsetMin.x
				, -(gap * (Mathf.Max(defaultCnt, list.Count) - defaultCnt)));

        int testInt = 0;
		 
		for (int i = 0; i<list.Count; i++) {
			button = Instantiate (buttonObj);

            button.name = i.ToString();

            button.GetComponentInChildren<Text> ().text = list [i].region;
			button.transform.SetParent(tr);
            button.transform.localScale = Vector3.one * 1.5f;

            button.GetComponent<RectTransform>().anchoredPosition3D = new Vector3 (0, firstPos - (i * gap));
            
            button.GetComponent<Button>().onClick.AddListener(delegate { DataMgr.Select(testInt++); });

        }
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