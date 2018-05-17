using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadFile : MonoBehaviour
{
	static LoadFile m_this;

	[SerializeField]
	List<GameObject> objectResList;

	[SerializeField]
	List<GameObject> mapBlockList;

	List<GameObject> ingameBlockList;

	List<GameObject> objectList;

	//Vector3 startPos = Vector3.zero;

	const int maxObjectCnt = 5;
	const int maxObjectNum = 5;

	const int maxBlockNum = 5;

    public static List<GameObject> GetBlockList
    {
        get
        {
            return m_this.ingameBlockList;
        }
    }

    public static List<GameObject> GetObjectList
    {
        get
        {
            return m_this.objectList;
        }
    }


    public static void LoadMap()
	{
		m_this.ingameBlockList = new List<GameObject> ();
		m_this.objectList = new List<GameObject> ();

		string data = string.Empty;

		data = Resources.Load<TextAsset> ("LoadMap1").ToString ();

		string[] tempData = data.Split (new string[] { "," }, System.StringSplitOptions.None);

		GameObject block = CreateBlock (0);
		GameObject objects = null;

        block.name = "0";

        block.transform.position = Vector3.zero;
        
		Vector3 nextPos = tempData[1].ToInt() == 3 ? block.transform.Find("BridgePos").position : block.transform.Find ("NextPos").position;

		m_this.ingameBlockList.Add (block);
        m_this.objectList.Add(null);

        for (int i = 1; i < tempData.Length; i++) {
			
			block = CreateBlock(tempData[i].ToInt());

            block.name = i.ToString();

            block.transform.position = nextPos;

            if (i + 1 < tempData.Length)
            {
                nextPos = tempData[i + 1].ToInt() == 3 ? block.transform.Find("BridgePos").position : block.transform.Find("NextPos").position;
            }

            if (tempData[i].ToInt() != 3)
            {
                if (i < tempData.Length - 1)
                {
                    objects = Instantiate(m_this.objectResList[Random.Range(0, m_this.objectResList.Count - 1)]);

                }
                else
                {
                    objects = Instantiate(m_this.objectResList[m_this.objectResList.Count - 1]);
                }
                objects.transform.position = block.transform.position;
                m_this.objectList.Add(objects);

                if (m_this.objectList.Count >= 5)
                {
                    objects.SetActive(false);
                }
            }
            else
            {
                m_this.objectList.Add(null);
            }

			m_this.ingameBlockList.Add (block);

            if (m_this.ingameBlockList.Count >= 5)
            {
                block.SetActive(false);
            }
		}

	}


//	public void LoadObjects()
//	{
//		GameObject block;
//
//		for (int i = 0; i < ingameBlockList.Count; i++) {
//			block = ingameBlockList [i];
//
//			Transform[] trs = block.transform.Find ("Objects").GetComponents<Transform> ();
//
//			int[] indexArrary = GetRandomIndex (trs);
//
//			for (int j = 0; j < indexArrary.Length; j++) {
//
//				GameObject obj = Instantiate(objectResList[Random.Range(0, maxObjectNum)]);
//				obj.transform.position = trs [indexArrary [j]].transform.position;
//				
//			}
//		}
//	}

	static GameObject CreateBlock(int listCnt)
	{
		return Instantiate(m_this.mapBlockList [listCnt]);

		//return Instantiate (obj);
	}

	// Use this for initialization
	void Start ()
	{
		m_this = this;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}


}

public struct RegionData
{
	public int index;
	public string region;
	public List<string> nameList;
}

