using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Timer : MonoBehaviour {

	// Timer parameters
	public float TimerSpeed = 30.0f; // to adjust the speed of countdown timer
	public float InitialValue = 0.05f;
	public float IncreaseAmount = 0.20f;
	private float timerSpeed;
	private float initialValue;
	private float increaseAmount;

	// Timer image object
	private Image image;
	public Sprite RedTimer;
	public Sprite GreenTimer;
	public Sprite YellowTimer;
	public float GreenTime = 2.0f;
	private float greenTime;
	private bool isGreen = false;

	private bool powerup = false;

	// Called by the board manager when a correct number is placed
	public void IncreaseTimer() {
		image.fillAmount = image.fillAmount + increaseAmount;
	}

	// Called by the board manager to see if a powerup occurred
	public bool IsPoweredUp() {
		bool res = powerup;
		powerup = false;
		return res;
	}

	void Awake () {
		timerSpeed = TimerSpeed;
		initialValue = InitialValue;
		increaseAmount = IncreaseAmount;
		greenTime = GreenTime;

		image = GetComponent<Image> ();
	}



	void Update () {

		if (image.fillAmount >= initialValue) {

			if (image.fillAmount >= 0.99f) {
				isGreen = true;


				if (greenTime <= 0.0f) {
					powerup = true;
					isGreen = false;
					greenTime = GreenTime;
					image.sprite = RedTimer;
					image.fillAmount = initialValue;
					//powerup = false;
					return;
				}

			} else {
				image.fillAmount += 1.0f / timerSpeed * Time.deltaTime;

				if (image.fillAmount <= 0.5f) {
					image.sprite = RedTimer;
				} else if (0.5f < image.fillAmount && image.fillAmount <= 0.9f) {
					image.sprite = YellowTimer;
					return;
				} else {
					image.sprite = GreenTimer;

				}

			}
		}


		if (isGreen) {
			greenTime -= Time.deltaTime;
		}
	}
}
