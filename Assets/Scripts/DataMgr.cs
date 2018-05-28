using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataMgr : MonoBehaviour {


    public static List<RegionData> regionList = new List<RegionData>();
    public static List<string> bigRegionList = new List<string>();
    public static RegionData currentRegion;

    public static bool isCartoon = false;
    
    public static void LoadRegionInfo()
    {
        regionList = new List<RegionData>();
        bigRegionList = new List<string>();

        string text = Resources.Load<TextAsset>("RegionData").text;

        string tempRegion = string.Empty;

        string[] data = text.Split(new string[] { "#" }, System.StringSplitOptions.None);
        string[] temp;

        for (int i = 1; i < data.Length; i++)
        {
            data[i] = data[i].Trim();
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
                    if (temp[cnt] != tempRegion)
                    {
                        tempRegion = temp[cnt];
                        bigRegionList.Add(temp[cnt]);
                    }
                }
                else if (cnt == 2)
                {
                    rData.region2 = temp[cnt];
                }
                else
                {
                    if (temp[cnt] != "")
                    {
                        rData.nameList.Add(temp[cnt]);
                    }
                }
                cnt++;
            }

            regionList.Add(rData);
        }
    }

    // Use this for initialization
    void Start () {
        if (FindObjectOfType<DataMgr>() != null)
        {
            DestroyImmediate(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        Application.targetFrameRate = -1;

        Screen.SetResolution(480, 840, false);
        
    }
    
	
	// Update is called once per frame
	void Update () {
		
	}
}

public struct RegionData
{
    public int index;
    public string region;
    public string region2;
    public List<string> nameList;
}
