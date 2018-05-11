using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrashSensor : MonoBehaviour
{

    Player p;
    StageMgr sm;
    // Use this for initialization
    void Start()
    {
        p = transform.GetComponentInParent<Player>();
        sm = FindObjectOfType<StageMgr>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Crash"))
        {
            p.Crash();
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Falling"))
        {
            p.Falling();
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Slide"))
        {
            if (!p.GetSlideState())
            {
                p.Crash();
            }
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Finish"))
        {
            StageMgr.isStart = false;
            Invoke("Finish", 0.5f);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Coin"))
        {
            sm.CardGaugeUp();
            other.gameObject.SetActive(false);
            //Destroy(other.gameObject);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Card"))
        {
            other.gameObject.SetActive(false);

            sm.CardCapture();
        }
    }

    void Finish()
    {
       sm.Finish();
    }
}
