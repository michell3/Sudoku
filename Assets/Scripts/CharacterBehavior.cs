using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehavior : MonoBehaviour {

	public GameObject Enemy;
	private CharacterBehavior enemyChar;

	public GameObject Board;
	private BoardManager bm;

	public GameObject Lion;
	private Animator lionAnim;
	public GameObject Squid;
	private Animator squidAnim;

	private bool isHurt;
	public float HurtTime = 2f;
	private float hurtTime;
	private float hurtTimer = 0f;

	Animator anim;

	void Awake () {
		enemyChar = Enemy.GetComponent<CharacterBehavior> ();
		anim = GetComponent<Animator> ();

		bm = Board.GetComponent<BoardManager> ();

		lionAnim = Lion.GetComponent<Animator> ();
		squidAnim = Squid.GetComponent<Animator> ();

		hurtTime = HurtTime;
		hurtTimer = hurtTime;
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

	public void ThrowLock() {
		anim.SetTrigger ("isThrowingLock");
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
