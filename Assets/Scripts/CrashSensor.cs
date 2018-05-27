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
            if (!p.isResetPlayer)
            {
                sm.LifeContol();
                p.Crash();

                SoundMgr.PlaySound(SoundType.damage);
            }
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Falling"))
        {
            if (!p.isResetPlayer)
            {
                p.Falling();
            }
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Slide"))
        {
            if (!p.GetSlideState())
            {
                if (!p.isResetPlayer)
                {
                    sm.LifeContol();
                    p.Crash();

                    SoundMgr.PlaySound(SoundType.damage);
                }
            }
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Coin"))
        {
            sm.CardGaugeUp();
            other.GetComponent<Renderer>().enabled = false;//gameObject.SetActive(false);
            other.GetComponent<Collider>().enabled = false;
            p.CoinGetEffect ();

            SoundMgr.PlaySound(SoundType.coin);
            //Destroy(other.gameObject);
        }
        //else if (other.gameObject.layer == LayerMask.NameToLayer("Card"))
        //{
        //    //other.transform.parent.gameObject.SetActive(false);

        //    other.transform.parent.SetParent(p.transform.Find("Main Camera/CardDest"));
        //    other.transform.GetComponent<CardItem>().MoveCard();

        //    p.PlayerGetCard();
        //    sm.CardCapture();
        //}
    }
    
}
