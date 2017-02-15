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

	void Start () {
		enemyChar = Enemy.GetComponent<CharacterBehavior> ();
		bm = EnemyBoard.GetComponent<BoardManager> ();
		isLion = IsLion;
	}

	void AttackBoard () {
		if (isLion) {
			bm.LionScare ();
			enemyChar.GetHurt (2f);
		} else {
			bm.SquidInk ();
			enemyChar.GetHurt (2f);
		}
	}


}
