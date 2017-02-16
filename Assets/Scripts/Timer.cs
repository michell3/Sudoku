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

	// Sound
	public AudioClip RainbowAudio;
	private AudioSource rainbowAudio;
	private bool isPlayingAudio = false;

	// Timer image object
	public Sprite RedTimer;
	public Sprite GreenTimer;
	public Sprite YellowTimer;
	public Sprite WhiteTimer; 
	public float GreenTime = 2.0f;
	private float greenTime;
	private bool isGreen = false;

	private SpriteRenderer childSprite;

	private bool powerup = false;

	private Color[] rainbow;
	private int colorIndex; 

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

		rainbowAudio = gameObject.AddComponent<AudioSource> ();
		rainbowAudio.clip = RainbowAudio;

		childSprite = child.GetComponent<SpriteRenderer>();

		UpdateTimer();

		//initialize the colors
		rainbow = new Color[8];
		rainbow[0] = new Color(121.0f/255 , 194.0f/255, 103.0f/255, 1.0f);
		rainbow[1] = new Color(197.0f/255 , 214.0f/255,  71.0f/255, 1.0f);
		rainbow[2] = new Color(245.0f/255 , 214.0f/255,  61.0f/255, 1.0f);
		rainbow[3] = new Color(242.0f/255 , 140.0f/255,  51.0f/255, 1.0f);
		rainbow[4] = new Color(232.0f/255 , 104.0f/255, 162.0f/255, 1.0f);
		rainbow[5] = new Color(191.0f/255 ,  98.0f/255, 166.0f/255, 1.0f);
		rainbow[6] = new Color(120.0f/255 , 197.0f/255, 214.0f/255, 1.0f);
		rainbow[7] = new Color( 79.0f/255 , 155.0f/255, 168.0f/255, 1.0f);

		colorIndex = 0;
	}

	private void UpdateTimer () {
		float x = fillAmount;
		transform.localScale = new Vector3 (x, y, 1);
	}

	void Update () {

		if (fillAmount >= initialValue) {

			if (fillAmount >= 0.99f) {
				childSprite.sprite = WhiteTimer;
				isGreen = true;

				// Play sound
				if (!isPlayingAudio) {
					rainbowAudio.Play ();
					isPlayingAudio = true;
				}

				//change the color of the bar each time
				childSprite.color = rainbow [colorIndex/8];
				colorIndex = (colorIndex + 1) % (8*rainbow.Length);

				if (greenTime <= 0.0f) {
					powerup = true;
					isGreen = false;
					greenTime = GreenTime;
					childSprite.color = new Color (1.0f, 1.0f, 1.0f, 1.0f); //reset to white
					childSprite.sprite = RedTimer;
					fillAmount = initialValue;
					isPlayingAudio = false;
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
