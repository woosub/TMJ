using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataMgr : MonoBehaviour {


    public static List<RegionData> regionList = new List<RegionData>();
    public static RegionData currentRegion;

    public static bool isCartoon = false;
    
    public static void LoadRegionInfo()
    {
        regionList = new List<RegionData>();

        string text = Resources.Load<TextAsset>("RegionData").text;

        string[] data = text.Split(new string[] { "\n" }, System.StringSplitOptions.None);
        string[] temp;

        for (int i = 0; i < data.Length; i++)
        {
            temp = data[i].Split(new string[] { "," }, System.StringSplitOptions.None);

            int cnt = 0;
            RegionData rData = new RegionData();
            rData.nameList = new List<string>();

            for (int j = 0; j < temp.Length; j++)
            {

                if (cnt == 0)
                {
                    rData.index = temp[cnt].ToInt();
                }
                else if (cnt == 1)
                {
                    rData.region = temp[cnt];
                }
                else
                {
                    rData.nameList.Add(temp[cnt]);
                }
                cnt++;
            }

            regionList.Add(rData);
        }
    }

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(gameObject);

        Application.targetFrameRate = -1;

        Screen.SetResolution(320, 560, false);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
