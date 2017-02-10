using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	// Timer parameters
	public GameObject Child;
	public float TimerSpeed = 30.0f; // to adjust the speed of countdown timer
	public float InitialValue = 0.05f;
	public float IncreaseAmount = 0.20f;
	private GameObject child;
	private float timerSpeed;
	private float initialValue;
	private float increaseAmount;
	private float fillAmount;
	private float y;

	// Timer image object
	public Sprite RedTimer;
	public Sprite GreenTimer;
	public Sprite YellowTimer;
	public float GreenTime = 2.0f;
	private float greenTime;
	private bool isGreen = false;

	private SpriteRenderer childSprite;

	private bool powerup = false;

	// Called by the board manager when a correct number is placed
	public void IncreaseTimer() {
		float newAmount = fillAmount + increaseAmount;
		if (newAmount > 1.0f) {
			newAmount = 1.0f;
		}
		fillAmount = newAmount;
		UpdateTimer ();
	}

	// Called by the board manager to see if a powerup occurred
	public bool IsPoweredUp() {
		bool res = powerup;
		powerup = false;
		return res;
	}

	void Start () {
		child = Child;
		timerSpeed = TimerSpeed;
		initialValue = InitialValue;
		increaseAmount = IncreaseAmount;
		fillAmount = initialValue;
		y = transform.localScale.y;
		greenTime = GreenTime;

		childSprite = child.GetComponent<SpriteRenderer>();

		UpdateTimer();
	}

	private void UpdateTimer () {
		float x = fillAmount;
		transform.localScale = new Vector3 (x, y, 1);
	}

	void Update () {

		if (fillAmount >= initialValue) {

			if (fillAmount >= 0.99f) {
				isGreen = true;

				if (greenTime <= 0.0f) {
					powerup = true;
					isGreen = false;
					greenTime = GreenTime;
					childSprite.sprite = RedTimer;
					fillAmount = initialValue;
					//powerup = false;
					return;
				}

			} else {
				fillAmount += 1.0f / timerSpeed * Time.deltaTime;
				UpdateTimer ();

				if (fillAmount <= 0.5f) {
					childSprite.sprite = RedTimer;
				} else if (0.5f < fillAmount && fillAmount <= 0.9f) {
					childSprite.sprite = YellowTimer;
					return;
				} else {
					childSprite.sprite = GreenTimer;
				}
			}
		}

		if (isGreen) {
			greenTime -= Time.deltaTime;
		}
	}
}
