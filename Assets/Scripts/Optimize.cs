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
        LoadFile.NextBlockSetting();

        int val = other.gameObject.name.ToInt();

        StageMgr.currentOptiFront = val;

        //Debug.Log(val + 1);

        LoadFile.GetBlockList.Find(n => n.name.ToInt() == val + 1).SetActive(true);

        GameObject obj = LoadFile.GetObjectList.Find(n => n.name.ToInt() == val + 1);

        if (obj != null)
        {
            obj.SetActive(true);

            Transform[] trs = obj.GetComponentsInChildren<Transform>();
            for (int j = 0; j < trs.Length; j++)
            {
                if (trs[j].gameObject.layer == LayerMask.NameToLayer("Coin"))
                {
                    trs[j].GetComponent<Renderer>().enabled = true;
                    trs[j].GetComponent<Collider>().enabled = true;
                }
            }
        }
    }
}
