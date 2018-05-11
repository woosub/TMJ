using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardItem : MonoBehaviour {

    [SerializeField]
    Sprite[] CardSprite;
    SpriteRenderer sr;

    bool isActive;

    int cnt = 0;

    float timer = 0.0f;

    void OnEnable()
    {
        isActive = true;
    }

    void OnDisable()
    {
        isActive = false;
    }

	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();

        cnt = 0;
    }
	
	// Update is called once per frame
	void Update () {
        if (!isActive)
            return;

        timer += Time.deltaTime;

        if (timer >= 0.2f)
        {
            timer = 0.0f;

            cnt++;

            if (cnt > 3)
            {
                cnt = 0;
            }
        }

        sr.sprite = CardSprite[cnt];
    }
}
