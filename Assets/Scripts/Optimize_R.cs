using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Optimize_R : MonoBehaviour {

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        int val = other.gameObject.name.ToInt();

        StageMgr.currentOptiRear = val;

        LoadFile.GetBlockList[val].SetActive(false);
        if (LoadFile.GetObjectList[val] != null)
            LoadFile.GetObjectList[val].SetActive(false);
    }
}
