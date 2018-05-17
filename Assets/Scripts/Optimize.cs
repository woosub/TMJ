using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Optimize : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        int val = other.gameObject.name.ToInt();

        StageMgr.currentOptiFront = val;

        if (LoadFile.GetBlockList.Count - 1 > val)
        {
            LoadFile.GetBlockList[val + 1].SetActive(true);

            if(LoadFile.GetObjectList[val + 1] != null)
                LoadFile.GetObjectList[val + 1].SetActive(true);
        }        
    }
}
