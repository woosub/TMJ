using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	bool isLeft = false;
	bool isRight = false;
	bool isJump = false;

	const float gravity = -0.08f;
	const float jumpPower = 0.03f;

	const float jumpLimit = 0.3f;

	float jumpVal = 0.0f;

	float flat = 0.0f;

	static Vector3 startPos = new Vector3 (0, 0.98f, -0.59f);
	static Vector3 startRot = new Vector3 (0, 0, 0);

	float playSpeed = 0.8f;

	Transform control;
	SpriteRenderer character;

	[SerializeField]
	float runFrameTime = 0.05f;

	[SerializeField]
	float jumpFrameTime = 0.05f;

	[SerializeField]
	float damageFrameTime = 0.05f;

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

	int damageCount = 0;
	float damageTimer = 0.0f;

	[SerializeField]
	Sprite[] damageSprite;

	Transform shadow;

	const float shadowFlat = 0.125f;

	Transform camera;
	Transform sky;

	bool isCrash = false;

	const float crashTime = 2.0f;
	float crashTimer = 0.0f;

	bool isFalling = false;

	const float fallingTime = 1.0f;
	float fallingTimer = 0.0f;

	// Use this for initialization
	void Start () {
		sky = transform.Find ("Sky");
		camera = transform.Find ("Main Camera");
		control = transform.Find ("Control");
		character = control.Find ("Character").GetComponent<SpriteRenderer>();
		flat = control.transform.position.y;

		shadow = transform.Find ("Shadow");
	}

	public static GameObject LoadPlayer()
	{
		GameObject obj = Resources.Load <GameObject>("Character/Player");

		obj = Instantiate (obj);

		obj.transform.position = startPos;
		obj.transform.eulerAngles = startRot;

		return obj;
	}
	
	// Update is called once per frame
	void Update () {

		if (!StageMgr.isStart)
			return;

		RunAnimation ();
		ShadowAnimation ();
		CameraPos ();
		SkyPos ();

		transform.position += Vector3.forward * Time.deltaTime * playSpeed;

		if (isLeft || Input.GetKey(KeyCode.A)) {

			if (control.position.x > -0.3f) {
				control.position += Vector3.left * Time.deltaTime; 
			}
		}

		if (isRight || Input.GetKey(KeyCode.D)) {
			if (control.position.x < 0.3f) {
				control.position += Vector3.right * Time.deltaTime;

			}
		}
		if (Input.GetKeyDown (KeyCode.Space)) {
			ControlJump ();
		}

		if (isJump) {

			JumpAnimtion ();

			jumpVal += gravity * Time.deltaTime; 
			control.position += new Vector3(0, jumpPower + jumpVal);	

			if (control.position.y <= flat) {
				control.position = new Vector3 (control.position.x, flat, control.position.z);
				isJump = false;
			}
		}

		if (isCrash) {
			crashTimer += Time.deltaTime;

			DamageAnimation ();

			if (crashTimer >= crashTime) {
				isCrash = false;

				character.enabled = true;

				CancelInvoke ("CrashEffect");

				playSpeed = 0.8f;
			}

		}

		if (isFalling) {
			control.position += Time.deltaTime * Vector3.down * gravity;

			fallingTimer += Time.deltaTime;

			if (fallingTimer >= fallingTime) {
				StageMgr.isStart = false;

				UnityEngine.SceneManagement.SceneManager.LoadScene (0);
			}

		}

	}

	void SkyPos()
	{
		sky.position = new Vector3 (control.position.x, sky.position.y, sky.position.z);
	}

	void CameraPos()
	{
		camera.position = new Vector3 (control.position.x, camera.position.y, camera.position.z);
	}

	void ShadowAnimation()
	{
		shadow.position = new Vector3(control.position.x, shadowFlat, control.position.z);

		shadow.localScale = new Vector3 (0.28f - (control.position.y * 0.5f), 0.28f - (control.position.y * 0.5f));
	}

	void RunAnimation()
	{
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
		character.sprite = jumpSprite [jumpCount];

		jumpTimer += Time.deltaTime;

		if (jumpTimer >= jumpFrameTime) {
			jumpTimer = 0.0f;
			jumpCount++;
		
			if (jumpCount >= jumpSpriteLimit)
				jumpCount = 0;
		}
	}

	void DamageAnimation()
	{
		character.sprite = damageSprite [damageCount];

		damageTimer += Time.deltaTime;

		if (damageTimer >= damageFrameTime) {
			damageTimer = 0.0f;
			damageCount++;

			if (damageCount >= damageSprite.Length)
				damageCount = 0;
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

		isCrash = true;

		crashTimer = 0.0f;

		playSpeed = 0.0f;

		damageCount = 0;
		damageTimer = 0.0f;

		InvokeRepeating ("CrashEffect", 0.0f, 0.1f);
	}

	public void Falling()
	{
		playSpeed = 0.2f;

		fallingTimer = 0.0f;
	}

	bool crashEffectFlag = false;

	void CrashEffect()
	{
		playSpeed += 0.05f;

		crashEffectFlag = !crashEffectFlag;

		character.enabled = crashEffectFlag;
	}


	void ControlLeft()
	{
		isLeft = true;
	}

	void ControlRight()
	{
		isRight = true;
	}

	void ControlJump()
	{
		if (isJump)
			return;

		isJump = true;

		jumpVal = 0.0f;
	}
}
