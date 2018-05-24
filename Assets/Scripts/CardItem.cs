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

    bool isMoving = false;

    Vector3 curPos;
    Vector3 destScale = new Vector3(5f, 5f, 5f);
    Vector3 curScale;

    const float moveSpeed = 2.0f;
    float moveVal = 0.0f;

    void OnEnable()
    {
        isActive = true;
		//if(GetComponent<Animation>().clip != null)
		//	GetComponent<Animation> ().Play ();
    }

    void OnDisable()
    {
        isActive = false;
		//if(GetComponent<Animation>().clip != null)
		//	GetComponent<Animation> ().Stop ();
    }

	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();

        cnt = 0;
    }

    public void MoveCard()
    {
        moveVal = 0.0f;
        isMoving = true;

        curPos = transform.parent.localPosition;
        curScale = transform.parent.localScale;

        sr.sprite = CardSprite[4];
    }

    // Update is called once per frame
    void Update() {
        if (!isActive)
            return;

        if (isMoving)
        {
            moveVal += Time.deltaTime * moveSpeed;
            transform.parent.localPosition = Vector3.Lerp(curPos, Vector3.zero, moveVal);
            transform.parent.localScale = Vector3.Lerp(curScale, destScale, moveVal);
            sr.color -= new Color(0, 0, 0, Time.deltaTime * 0.6f);

            if (moveVal >= 1.0f)
            {
                transform.parent.gameObject.SetActive(false);
                transform.parent.SetParent(null);

                sr.color = Color.white;

                isMoving = false;
            }
        }
        else
        {

            //timer += Time.deltaTime;

            //if (timer >= 0.2f)
            //{
            //    timer = 0.0f;

            //    cnt++;

            //    if (cnt > 3)
            //    {
            //        cnt = 0;
            //    }
            //}

            //sr.sprite = CardSprite[cnt];

        }
    }
}
