using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoadFile : MonoBehaviour
{
	[SerializeField]
	List<GameObject> objectResList;

	[SerializeField]
	List<GameObject> mapBlockList;

	List<GameObject> ingameBlockList;

	List<GameObject> objectList;

	Vector3 startPos = Vector3.zero;

	const int maxObjectCnt = 5;
	const int maxObjectNum = 5;

	const int maxBlockNum = 5;

	public void LoadMap()
	{
		ingameBlockList = new List<GameObject> ();
		objectList = new List<GameObject> ();

		string data = string.Empty;

		data = Resources.Load<TextAsset> ("LoadMap1").ToString ();

		string[] tempData = data.Split (new string[] { "," }, System.StringSplitOptions.None);

		GameObject block = CreateBlock (0);
		GameObject objects = null;

		block.transform.position = startPos;
        
		Vector3 nextPos = tempData[1].ToInt() == 3 ? block.transform.Find("BridgePos").position : block.transform.Find ("NextPos").position;

		ingameBlockList.Add (block);

		for (int i = 1; i < tempData.Length; i++) {
			
			block = CreateBlock(tempData[i].ToInt());

			block.transform.position = nextPos;

            if (i + 1 < tempData.Length)
            {
                nextPos = tempData[i + 1].ToInt() == 3 ? block.transform.Find("BridgePos").position : block.transform.Find("NextPos").position;
            }

			if (tempData [i].ToInt () != 3) 
			{
				if (i < tempData.Length - 1) {
					objects = Instantiate (objectResList [Random.Range (0, objectResList.Count - 1)]);

				} else {
					objects = Instantiate (objectResList [objectResList.Count - 1]);
				}
				objects.transform.position = block.transform.position;
				objectList.Add (objects);
			}

            ingameBlockList.Add (block);
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

	GameObject CreateBlock(int listCnt)
	{
		return Instantiate(mapBlockList [listCnt]);

		//return Instantiate (obj);
	}

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}


}

