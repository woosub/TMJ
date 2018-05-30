using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    bool isLeft = false;
    bool isRight = false;
    bool isJump = false;
    bool isDown = false;
    bool isGetCard = false;

    const float gravity = -0.08f;
    const float jumpPower = 0.0225f;

    const float jumpLimit = 0.3f;

    float jumpVal = 0.0f;

    float flat = 0.0f;

    static Vector3 startPos = new Vector3(0, 0.98f, -0.59f);
    static Vector3 startRot = new Vector3(0, 0, 0);

    //[SerializeField]
    //float gamePlaySpeed = 2.0f;

    public float playSpeed = 1.5f;

    Transform control;
    SpriteRenderer character;

    [SerializeField]
    float runFrameTime = 0.05f;

    [SerializeField]
    float jumpFrameTime = 0.05f;

    [SerializeField]
    float damageFrameTime = 0.05f;

    [SerializeField]
    float slideFrameTime = 0.05f;

    const int runSpriteLimit = 8;
    int runCount = 0;
    float runTimer = 0.0f;

    [SerializeField]
    Sprite[] runSprite;

    const int jumpSpriteLimit = 4;
    int jumpCount = 0;
    float jumpTimer = 0.0f;

    [SerializeField]
    Sprite[] jumpSprite;

    [SerializeField]
    Sprite[] etcSprite;

    Transform shadow;

    const float shadowFlat = 0.125f;

    new Transform camera;

    Vector3 camCurPos;
    Vector3 camDestPos;

    Transform sky;

    public bool isCrash = false;

    const float crashTime = 1.0f;
    float crashTimer = 0.0f;

    public bool isFalling = false;

    const float fallingTime = 1.8f;
    float fallingTimer = 0.0f;

    const float lineMovelimit = 0.228f;

    bool isMoveline = false;
    int lineNum = 0;

    Vector3 destVal;
    Vector3 curVal;
    float sideMoveVal;

    StageMgr sm;

    [SerializeField]
    GameObject coinEffect;

    [SerializeField]
    float coinEffectTime = 0.1f;

    Vector3 fallingPos;

    public bool GetSlideState()
    {
        return isDown;
    }

    // Use this for initialization
    void Start() {

        SimpleGesture.On4AxisFlickSwipeDown(SwipeDown);
        SimpleGesture.On4AxisFlickSwipeUp(SwipeUp);
        SimpleGesture.On4AxisFlickSwipeLeft(SwipeLeft);
        SimpleGesture.On4AxisFlickSwipeRight(SwipeRight);

        //playSpeed = gamePlaySpeed;


        coinEffect.SetActive(false);

        sky = transform.Find("Sky");
        camera = transform.Find("Main Camera");
        control = transform.Find("Control");
        character = control.Find("Character").GetComponent<SpriteRenderer>();
        flat = control.transform.position.y;

        shadow = transform.Find("Shadow");

        sm = FindObjectOfType<StageMgr>();

    }

    public void SwipeDown()
    {
        if (!isDown && !isJump)
        {
            isDown = true;

            character.sprite = etcSprite[3];


            Invoke("ReleaseSlide", 0.3f);
        }
    }

    public void SwipeUp()
    {
        ControlJump();
    }

    public void SwipeLeft()
    {
        if (!isMoveline)
        {
            ControlLeft();
        }
    }

    public void SwipeRight()
    {
        if (!isMoveline)
        {
            ControlRight();
        }
    }


    public static GameObject LoadPlayer()
    {
        GameObject obj = Resources.Load<GameObject>("Character/Player");

        obj = Instantiate(obj);

        obj.transform.position = startPos;
        obj.transform.eulerAngles = startRot;

        return obj;
    }

    // Update is called once per frame
    void Update() {

        if (!StageMgr.isStart)
            return;

        if (isFalling)
        {
            control.position += Time.deltaTime * Vector3.up * gravity * 20;

            if (isRight) {
                control.position += Time.deltaTime * Vector3.right * 0.4f;
            }
            if (isLeft) {
                control.position += Time.deltaTime * Vector3.left * 0.4f;
            }


            fallingTimer += Time.deltaTime;

            if (fallingTimer >= fallingTime)
            {
                //sm.Finish();

                sm.LifeContol();
                if (sm.lifeCount < 3)
                {
                    sm.ResetPlayer();
                }

                //UnityEngine.SceneManagement.SceneManager.LoadScene(1);
            }

        }
        else
        {
            SkyPos();
            CameraPos();
            RunAnimation();
            ShadowAnimation();

            transform.position += Vector3.forward * Time.deltaTime * playSpeed;

            //sm.CalcMeter(Time.deltaTime * playSpeed);

            if (!isMoveline)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    ControlLeft();
                }

                if (Input.GetKeyDown(KeyCode.D))
                {
                    ControlRight();
                }
            }
            else
            {
                sideMoveVal += Time.deltaTime * 6f;
                Vector3 moveVal = Vector3.Lerp(curVal, destVal, sideMoveVal);
                if (1.0f > sideMoveVal)
                {
                    control.localPosition = new Vector3(moveVal.x, control.localPosition.y);
                }
                else
                {
                    isMoveline = false;
                    isLeft = false;
                    control.localPosition = new Vector3(destVal.x, control.localPosition.y);
                    camera.localPosition = camDestPos;
                }
            }

            if (!isDown && !isJump)
            {
                if (Input.GetKeyDown(KeyCode.S))
                {
                    isDown = true;

                    character.sprite = etcSprite[3];


                    Invoke("ReleaseSlide", 0.3f);
                }
            }

            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W))
            {
                ControlJump();
            }

            if (isJump)
            {

                JumpAnimtion();

                jumpVal += gravity * Time.deltaTime;
                control.position += new Vector3(0, jumpPower + jumpVal) * Time.deltaTime * 52;

                if (control.position.y <= flat)
                {
                    control.position = new Vector3(control.position.x, flat, control.position.z);
                    isJump = false;
                }
            }

            if (isCrash)
            {
                crashTimer += Time.deltaTime;

                if (crashTimer >= crashTime)
                {
                    isCrash = false;
                    isResetPlayer = false;

                    character.enabled = true;

                    CancelInvoke("CrashEffect");

                    //sm.Finish();
                    playSpeed = sm.currentSpeed;
                }

            }

        }

    }

    void ReleaseSlide()
    {
        isDown = false;
    }

    void SkyPos()
    {
        sky.position = new Vector3(control.position.x, sky.position.y, sky.position.z);
    }

    void CameraPos()
    {
        if (!isMoveline)
            return;

        camera.localPosition = Vector3.Lerp(camCurPos, camDestPos, sideMoveVal);
    }

    void ShadowAnimation()
    {
        shadow.position = new Vector3(control.position.x, shadowFlat, control.position.z);

        shadow.localScale = new Vector3(0.28f - (control.position.y * 0.5f), 0.28f - (control.position.y * 0.5f));
    }

    void RunAnimation()
    {
        if (isCrash || isJump || isDown || isFalling || isGetCard)
            return;

        character.sprite = runSprite[runCount];

        runTimer += Time.deltaTime;

        if (runTimer >= runFrameTime) {
            runTimer = 0.0f;
            runCount++;

            if (runCount >= runSpriteLimit)
                runCount = 0;
        }
    }

    void JumpAnimtion()
    {
        if (isCrash || isFalling || isGetCard)
            return;

        character.sprite = jumpSprite[jumpCount];

        jumpTimer += Time.deltaTime;

        if (jumpTimer >= jumpFrameTime) {
            jumpTimer = 0.0f;
            jumpCount++;

            if (jumpCount >= jumpSpriteLimit)
                jumpCount = 0;
        }
    }

    public bool GetCrashState()
    {
        return isCrash;
    }

    public void Crash()
    {
        if (isCrash)
            return;

        isResetPlayer = true;
        isCrash = true;

        character.sprite = etcSprite[0];

        crashTimer = 0.0f;

        playSpeed = 0.0f;

        if (sm.lifeCount < 3)
        {
            InvokeRepeating("CrashEffect", 0.0f, 0.1f);
        }
    }

    public void Falling()
    {
        playSpeed = 0.5f;

        isFalling = true;

        fallingTimer = 0.0f;

        character.sprite = etcSprite[1];

        shadow.gameObject.SetActive(false);


        fallingPos = control.localPosition;
    }

    public bool isResetPlayer = false;

    public void ResetPlayer(bool fall)
    {
        isResetPlayer = true;
        isFalling = false;
        isCrash = false;
        
        shadow.gameObject.SetActive(true);

        if (fall)
        {
            control.localPosition = fallingPos;
            transform.position += Vector3.forward * 0.7f;
        }

        playSpeed = 0.0f;

        if (sm.lifeCount < 3)
        {
            InvokeRepeating("CrashEffect", 0.0f, 0.1f);
            Invoke("CancelCrashEffect", 2f);
        }
    }

    bool crashEffectFlag = false;

    void CrashEffect()
    {
        playSpeed += 0.02f;

        crashEffectFlag = !crashEffectFlag;

        character.enabled = crashEffectFlag;
    }

    public void CancelCrashEffect()
    {
        character.enabled = true;
        playSpeed = sm.currentSpeed;
        isResetPlayer = false;
        CancelInvoke("CrashEffect");
    }


    void ControlLeft()
	{
        if (lineNum >= 0)
        {
            isMoveline = true;
            isLeft = true;

            camCurPos = camera.localPosition;
            camDestPos = camCurPos + Vector3.left * 0.17f;

            sideMoveVal = 0.0f;
            curVal = control.localPosition;
            destVal = control.localPosition - new Vector3(lineMovelimit, 0);
            lineNum--;
        }
    }

	void ControlRight()
	{
        if (lineNum <= 0)
        {
            isMoveline = true;
            isRight = true;

            camCurPos = camera.localPosition;
            camDestPos = camCurPos + Vector3.right * 0.17f;

            sideMoveVal = 0.0f;
            curVal = control.localPosition;
            destVal = control.localPosition + new Vector3(lineMovelimit, 0);
            lineNum++;
        }
    }

	void ControlJump()
	{
        if (isDown)
            return;

		if (isJump)
			return;

		isJump = true;

		jumpVal = 0.0f;
	}

    public void PlayerGetCard()
    {
        isGetCard = true;
        character.sprite = etcSprite[2];

        Invoke("InvokeGetcardOff", 0.35f);
    }

    void InvokeGetcardOff()
    {
        isGetCard = false;
    }

	public void CoinGetEffect()
	{
		coinEffect.SetActive (true);

		CancelInvoke ("CoinEffectOff");
		Invoke ("CoinEffectOff", coinEffectTime);
	}

	void CoinEffectOff()
	{
		coinEffect.SetActive (false);
	}
}
