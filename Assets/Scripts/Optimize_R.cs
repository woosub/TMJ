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
        string val = other.gameObject.name;

        //StageMgr.currentOptiRear = val;

        other.gameObject.SetActive(false);

        LoadFile.GetBlockList.Remove(other.gameObject);
        LoadFile.AddBlockPool(other.gameObject);

        GameObject obj = LoadFile.GetObjectList.Find(n => n.name == val);

        if (obj != null)
        {
            obj.SetActive(false);

            LoadFile.GetObjectList.Remove(obj);
            LoadFile.AddObjectPool(obj);
        }
        
    }
}
