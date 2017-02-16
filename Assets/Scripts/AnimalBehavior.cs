using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalBehavior: MonoBehaviour {

	public GameObject Enemy;
	private CharacterBehavior enemyChar;

	public GameObject EnemyBoard;
	private BoardManager bm;

	public bool IsLion = true;
	private bool isLion;

	public AudioClip PoofAudio;
	private AudioSource poofAudio;
	public AudioClip JumpAudio;
	private AudioSource jumpAudio;

	void Start () {
		enemyChar = Enemy.GetComponent<CharacterBehavior> ();
		bm = EnemyBoard.GetComponent<BoardManager> ();
		isLion = IsLion;

		// Sounds
		poofAudio = gameObject.AddComponent<AudioSource> ();
		poofAudio.clip = PoofAudio;
		jumpAudio = gameObject.AddComponent<AudioSource> ();
		jumpAudio.clip = JumpAudio;
	}

	public void AttackBoard () {
		if (isLion) {
			bm.LionScare ();
			enemyChar.GetHurt (2f);
		} else {
			bm.SquidInk ();
			enemyChar.GetHurt (2f);
		}
	}

	public void PlayPoofSound() {
		poofAudio.Play ();
	}

	public void PlayJumpSound() {
		jumpAudio.Play ();
	}
}
