using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrashSensor : MonoBehaviour {

	Player p;
	// Use this for initialization
	void Start () {
		p = transform.GetComponentInParent<Player> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.layer == LayerMask.NameToLayer ("Crash")) {
			p.Crash ();
		} else if (other.gameObject.layer == LayerMask.NameToLayer ("Falling")) {
			p.Falling ();
		} else if (other.gameObject.layer == LayerMask.NameToLayer ("Slide")) {
			if (!p.GetSlideState ()) {
				p.Crash ();
			}
		} else {
			StageMgr.isStart = false;
			Invoke ("Finish", 0.5f);
		}

	}

	void Finish()
	{
		FindObjectOfType< StageMgr>().Finish ();
	}
}
