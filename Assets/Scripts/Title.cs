using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Title : MonoBehaviour {

	bool isReady = false;
	bool isShowRegionList = false;

	[SerializeField]
	GameObject regionListObj;

	[SerializeField]
	GameObject buttonObj;

	const int gap = 100;
	const int startPos = 150; 
	const int defaultCnt = 4;
	const int defaultGap = 50;


	// Use this for initialization
	IEnumerator Start () {

		yield return new WaitForSeconds (0.05f);

		isReady = false;
		isShowRegionList = false;

		LoadFile.LoadRegionInfo ();
		SetLoadRegion ();

		yield return new WaitForSeconds (0.5f);

		isReady = true;
	}

	void SetLoadRegion()
	{
		List<RegionData> list = LoadFile.GetRegionList;

		int firstPos = startPos + (Mathf.Max(defaultCnt, list.Count) - defaultCnt) * defaultGap;

		GameObject button;
		Transform tr = regionListObj.transform.Find ("Viewport").Find ("Content");

		tr.GetComponent<RectTransform>().offsetMin = 
			new Vector2(tr.GetComponent<RectTransform>().offsetMin.x
				, -(gap * (Mathf.Max(defaultCnt, list.Count) - defaultCnt)));
		 
		for (int i = 0; i<list.Count; i++) {
			button = Instantiate (buttonObj);

			button.GetComponentInChildren<Text> ().text = list [i].region;
			button.transform.SetParent(tr);
			button.GetComponent<RectTransform>().anchoredPosition3D = new Vector3 (0, firstPos - (i * gap));
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
			isShowRegionList = true;

			regionListObj.SetActive (true);
        }
	}


}
