using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehavior : MonoBehaviour {

	public GameObject Board;

	public GameObject Lion;
	private Animator lionAnim;
	public GameObject Squid;
	private Animator squidAnim;

	public GameObject Lock;
	private Animator lockAnim;
	public GameObject Dart;
	private Animator dartAnim;

	private bool isHurt;
	public float HurtTime = 2f;
	private float hurtTime;
	private float hurtTimer = 0f;

	// Sounds
	public AudioClip HurtAudio;
	private AudioSource hurtAudio;
	private AudioSource throwAudio;

	private Animator anim;

	void Awake () {
		
		anim = GetComponent<Animator> ();

		lionAnim = Lion.GetComponent<Animator> ();
		squidAnim = Squid.GetComponent<Animator> ();

		lockAnim = Lock.GetComponent<Animator> ();
		dartAnim = Dart.GetComponent<Animator> ();

		hurtTime = HurtTime;
		hurtTimer = hurtTime;

		// Sounds
		hurtAudio = gameObject.AddComponent<AudioSource>();
		hurtAudio.clip = HurtAudio;
		throwAudio = gameObject.AddComponent<AudioSource> ();
		throwAudio.clip = Resources.Load ("select-1.wav") as AudioClip;
	}

	void Update () {

		if (isHurt) {
			hurtTimer -= Time.deltaTime;

			if (hurtTimer < 0f) {
				hurtTimer = hurtTime;
				isHurt = false;
				anim.SetBool ("isHurt", false);
			}
		}
	}

	public void LionAttack() {
		lionAnim.SetTrigger ("hasAppeared");
	}

	public void SquidAttack() {
		squidAnim.SetTrigger ("hasAppeared");
	}

	public void ThrowDart() {
		anim.SetTrigger ("isThrowingDart");
	}

	public void ActivateDart() {
		dartAnim.SetTrigger ("isThrown");
	}

	public void ThrowLock() {
		anim.SetTrigger ("isThrowingLock");
	}

	public void ActivateLock() {
		lockAnim.SetTrigger ("isThrown");
	}

	public void Winner() {
		anim.SetBool ("isWinner", true);
	}

	public void Loser() {
		anim.SetBool ("isHurt", true);
	}

	public void GetHurt(float seconds) {
		anim.SetBool ("isHurt", true);
		isHurt = true;
		hurtTime = seconds;
		hurtTimer = hurtTime;
	}
}
