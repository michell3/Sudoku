using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBehavior : MonoBehaviour {

	public GameObject Enemy;
	private CharacterBehavior enemyChar;

	public GameObject EnemyBoard;
	private BoardManager bm;

	public bool IsLock = true;
	private bool isLock;

	void Start () {
		enemyChar = Enemy.GetComponent<CharacterBehavior> ();
		bm = EnemyBoard.GetComponent<BoardManager> ();
		isLock = IsLock;
	}

	void AttackBoard () {
		if (isLock) {
			bm.LockGridCell ();
			enemyChar.GetHurt (2f);
		} else {
			bm.Stun (5);
		}
	}
}
