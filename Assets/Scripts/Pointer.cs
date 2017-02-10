using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour {
	public GameObject Board;

	//pointer stuff
	private SpriteRenderer spriteR;
	public Sprite sprite1;
	public Sprite sprite2;
	public Sprite sprite3;
	public Sprite sprite4;
	public Sprite sprite5;
	public Sprite sprite6;
	public Sprite sprite7;
	public Sprite sprite8;
	public Sprite sprite9;

	private int number;


	// Use this for initialization
	void Start () {
		spriteR = gameObject.GetComponent<SpriteRenderer>();
		spriteR.sprite = sprite1;
	}
	
	// Update is called once per frame
	void Update () {
		number = Board.GetComponent<BoardManager> ().pointerNumber ();
		changePointer ();
	}

	private void changePointer(){
		switch (number) {
		case 0:
			spriteR.sprite = sprite1;
			break;
		case 1:
			spriteR.sprite = sprite2;
			break;
		case 2:
			spriteR.sprite = sprite3;
			break;
		case 3:
			spriteR.sprite = sprite4;
			break;
		case 4:
			spriteR.sprite = sprite5;
			break;
		case 5:
			spriteR.sprite = sprite6;
			break;
		case 6:
			spriteR.sprite = sprite7;
			break;
		case 7:
			spriteR.sprite = sprite8;
			break;
		case 8:
			spriteR.sprite = sprite9;
			break;
		}
	}
}
