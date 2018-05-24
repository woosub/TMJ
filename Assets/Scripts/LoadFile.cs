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

    List<GameObject> blockPoolList = new List<GameObject>();
    List<GameObject> objectPoolList = new List<GameObject>();

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

    public static void AddObjectPool(GameObject obj)
    {
        m_this.objectPoolList.Add(obj);
    }

    public static void AddBlockPool(GameObject obj)
    {
        m_this.blockPoolList.Add(obj);
    }

    static GameObject GetNextBlock(bool isBridge)
    {
        GameObject obj = m_this.blockPoolList[Random.Range(0, m_this.blockPoolList.Count)];
        if (isBridge)
        {
            if (obj.transform.Find("BridgePos") == null)
            {
                m_this.blockPoolList.Remove(obj);
                m_this.blockPoolList.Add(obj);
                return GetNextBlock(isBridge);
            }
            else
            {
                return obj;
            }
        }

        return obj;
    }

    public static void NextBlockSetting()
    {
        GameObject pool = null;
        GameObject lastBlock = m_this.ingameBlockList[m_this.ingameBlockList.Count - 1];
        bool isBridge = false;
        Vector3 nextPos = Vector3.zero;
        int blockNum = -1;

        if (m_this.blockPoolList.Count > 0)
        {
            pool = GetNextBlock(lastBlock.transform.Find("BridgePos") == null);//m_this.blockPoolList[Random.Range(0, m_this.blockPoolList.Count)];

            isBridge = pool.transform.Find("BridgePos") == null;


            nextPos = lastBlock.transform.Find(isBridge ? "BridgePos" : "NextPos").position;

            pool.SetActive(false);
            pool.transform.position = nextPos;

            blockNum = lastBlock.name.ToInt() + 1;

            pool.name = blockNum.ToString();

            m_this.ingameBlockList.Add(pool);
            m_this.blockPoolList.Remove(pool);
        }

        if (!isBridge)
        {
            if (m_this.objectPoolList.Count > 0)
            {
                pool = m_this.objectPoolList[Random.Range(0, m_this.objectPoolList.Count)];


                pool.SetActive(false);
                pool.transform.position = nextPos;
                pool.name = blockNum.ToString();

                m_this.objectList.Add(pool);
                m_this.objectPoolList.Remove(pool);
            }
        }
        //else
        //{
        //    m_this.objectList.Add(null);
        //}
    }


    public static void LoadMap()
	{
		m_this.ingameBlockList = new List<GameObject> ();
		m_this.objectList = new List<GameObject> ();

        string data = string.Empty;

        data = Resources.Load<TextAsset>("LoadMap1").ToString();

        string[] tempData = data.Split(new string[] { "," }, System.StringSplitOptions.None);

        GameObject block = CreateBlock (0);
		GameObject objects = null;

        block.name = "0";

        block.transform.position = Vector3.zero;
        
		Vector3 nextPos = tempData[1].ToInt() == 3 ? block.transform.Find("BridgePos").position : block.transform.Find ("NextPos").position;

		m_this.ingameBlockList.Add (block);
        //m_this.objectList.Add(null);

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
                //if (i < tempData.Length - 1)
                //{
                    objects = Instantiate(m_this.objectResList[Random.Range(0, 6)]); //초반은 쉬운난이도이기때문에 6개만 사용한다.

                //}
                //else
                //{
                //    objects = Instantiate(m_this.objectResList[m_this.objectResList.Count - 1]);
                //}
                objects.transform.position = block.transform.position;

                objects.name = i.ToString();
                m_this.objectList.Add(objects);

                if (m_this.objectList.Count >= 5)
                {
                    objects.SetActive(false);
                }
            }
            //else
            //{
            //    m_this.objectList.Add(null);
            //}

			m_this.ingameBlockList.Add (block);

            if (m_this.ingameBlockList.Count >= 5)
            {
                block.SetActive(false);
            }
		}

	}

    public static void Lv2()
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject obj = Instantiate(m_this.objectResList[6]);
            obj.SetActive(false);
            m_this.objectPoolList.Add(obj);
            obj = Instantiate(m_this.objectResList[7]);
            obj.SetActive(false);
            m_this.objectPoolList.Add(obj);
        }
    }

    public static void Lv3()
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject obj = Instantiate(m_this.objectResList[8]);
            obj.SetActive(false);
            m_this.objectPoolList.Add(obj);
            obj = Instantiate(m_this.objectResList[9]);
            obj.SetActive(false);
            m_this.objectPoolList.Add(obj);
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

