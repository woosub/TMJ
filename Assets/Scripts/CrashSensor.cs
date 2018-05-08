using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		} else {
			p.Falling ();
		}

	}
}
