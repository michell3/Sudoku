﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour {
	
	// For the purposes of sizing
	public GameObject sampleSprite;
	public GameObject pointer;
	public GameObject cell;
	public GameObject lockPrefab;
	public GameObject inkPrefab;
	public List<GameObject> lockList = new List<GameObject>();

	// Timer gameobject
	public GameObject TimerBar;

	// Board gameobject
	private GameObject[,] board;

	// Grid variables
	public int columns = 9;
	public int rows = 9;

	private float boardWidth;
	private float boardHeight;
	private float leftBound;
	private float upperBound;
	private float spriteWidth;
	private float spriteHeight;
	private float spriteScale;

	// Display parameters
	public float Spacing = .8f;
	public float LeftOffset = 1.2f;
	public float TopOffset = 1.2f;
	private float spacing;
	private float leftOffset;
	private float topOffset;

	// Game state variables
	public bool isP1 = true;

	private GameObject selectedCell;

	private int pointerCol;
	private int pointerRow;

	private int pointerNum;
	public GameObject numberBar;

	private int randomRow;
	private int randomCol;
	private int power;

	private float stunTime = 0;
	private bool stunned = false;

	private int lionScareCount;

	private bool unlockPress = false;

	private Dictionary<string, KeyCode> controls;

	private int[,] answer, show;

	//Define different controls for different players
	Dictionary<string, KeyCode> p1Controls = 
		new Dictionary<string, KeyCode> () {
		{"up", KeyCode.UpArrow},
		{"down", KeyCode.DownArrow},
		{"left", KeyCode.LeftArrow},
		{"right", KeyCode.RightArrow},
		{"place", KeyCode.Space},
		{"chooseUp", KeyCode.N},
		{"chooseDown", KeyCode.B},
		{"lock",KeyCode.T}
	};

	Dictionary<string, KeyCode> p2Controls = 
		new Dictionary<string, KeyCode> () {
		{"up", KeyCode.W},
		{"left", KeyCode.A},
		{"down", KeyCode.S},
		{"right", KeyCode.D},
		{"place", KeyCode.R},
		{"chooseUp", KeyCode.Y},
		{"chooseDown", KeyCode.T},
		{"lock",KeyCode.Y}
	};

	void Awake () {
		board = new GameObject[rows, columns];

		spriteScale = sampleSprite.transform.localScale.x;
		spacing = Spacing * spriteScale;
		leftOffset = LeftOffset * spriteScale;
		topOffset = TopOffset * spriteScale;

		boardWidth = GetComponent<SpriteRenderer>().bounds.size.x;
		boardHeight = GetComponent<SpriteRenderer>().bounds.size.y;
		leftBound = leftOffset + transform.position.x - (boardWidth / 2f);
		upperBound = topOffset + transform.position.y - (boardHeight / 2f);
		spriteWidth = sampleSprite.GetComponent<SpriteRenderer>().bounds.size.x;
		spriteHeight = sampleSprite.GetComponent<SpriteRenderer>().bounds.size.y;

		DisplayBoard ();

		if (isP1)
			controls = p1Controls;
		else
			controls = p2Controls;
	}

	void DisplayBoard () {
		float x, y;
		for (int c = 0; c < 9; c++) {
			for (int r = 0; r < 9; r++) {
				x = leftBound + c * (spriteWidth + spacing) + (spriteWidth / 2f);
				y = upperBound + r * (spriteHeight + spacing) + (spriteHeight / 2f);
				GameObject instance = Instantiate (cell,
					new Vector3 (x, y, -1f),
					Quaternion.identity);
				board [r, c] = instance;
			}
		}
		Select (0, 0);
		pointerNum = 0;

		// Hardcoded to only get dummy board, can change later 
		answer = RefBoard.getAnswerBoard(1);
		show = RefBoard.getShowBoard (1);

		for (int c = 0; c < 9; c++) {
			for (int r = 0; r < 9; r++) {
				if (show [r, c] == 1) {
					board [r, c].GetComponent<Cell> ().Val = answer [r, c] - 1;
				}
			}
		}
		Debug.Log (board [0, 0].GetComponent<Cell> ().Val);
	}

	private void Select(int row, int col) {
		if (row < 0 || row >= rows || col < 0 || col >= columns)
			return;

		//deselect current cell
		if (selectedCell)
			selectedCell.GetComponent<Cell>().Selected = false; 

		//keep track of selected cell in board manager
		selectedCell = board [row, col];

		//set selected flag for sprite display purposes
		selectedCell.GetComponent<Cell>().Selected = true; 

		//reset location of the pointer
		pointer.transform.position = selectedCell.transform.position;
		pointerCol = col;
		pointerRow = row;
	}

	void Update() {

		if (!stunned) {
			//moving the selector. 
			if (Input.GetKeyDown (controls ["down"])) {
				Select (pointerRow - 1, pointerCol);
			} else if (Input.GetKeyDown (controls ["up"])) {
				Select (pointerRow + 1, pointerCol);
			} else if (Input.GetKeyDown (controls ["left"])) {
				Select (pointerRow, pointerCol - 1);
			} else if (Input.GetKeyDown (controls ["right"])) {
				Select (pointerRow, pointerCol + 1);
			}

			if (Input.GetKeyDown (controls ["place"]) && !unlockPress) {
				Place ();
			} else if (Input.GetKeyDown (controls ["place"]) && unlockPress) {
				UnlockGridCell ();
				unlockPress = false;
			} else if (Input.GetKeyDown (controls ["chooseDown"])) {
				//make sure to wrap around # of rows/columns, then add 1 since
				//we are 1-indexing
				choosePointerNum(-1);

			} else if (Input.GetKeyDown (controls ["chooseUp"])) {
				//make sure to wrap around # of rows/columns, then add 1 since
				//we are 1-indexing
				choosePointerNum(1);
			}

			if (Input.GetKeyDown (controls ["lock"])) {
				LockGridCell ();
			}
				
			//REMEMBER TO DELETE THIS
			if (Input.GetKeyDown (KeyCode.G)) {
				//PowerUp ();
				LockGridCell();
				//SquidInk();
			}
		} else {
			stunTime -= Time.deltaTime;
			if (stunTime < 0)
				stunned = false;
		}
	}
		
	private void choosePointerNum(int move){


		selectSprite (false); // deselect current sprite
		pointerNum = ((rows + pointerNum + move) % rows); 
		selectSprite(true); // select new sprite
	
	}

	private void selectSprite(bool select){
		GameObject temp; 
		Color newColor;

		if (select)
			newColor = Color.white;
		else
			newColor = new Color (0.24f, 0.21f, 0.18f, 0.42f);

		temp = numberBar.transform.GetChild(pointerNum).gameObject;
		temp.GetComponent<SpriteRenderer> ().color = newColor;

		//get the selected box child of the sprite and then activate/deactivate it
		temp.transform.GetChild(0).gameObject.SetActive(select);
			
		
		
	}

	//checks to see whether all of the cells are locked/filled with an animal or not
<<<<<<< HEAD
	private int openGrid()
	{
=======
	private bool openGrid() {
>>>>>>> 12001057384a4bb0fbb65a9273bf3c60ea91afd2
		int openCells = 0;
		for(int r = 0; r < 9; r++){
			for(int c = 0; c < 9; c++){
				if(board[r,c].GetComponent<Cell>().Val == -1 &&
				   board[r,c].GetComponent<Cell>().Locked == false)
					openCells += 1;
			}
		}
		return openCells;
	}

	// locks the grid cell that is selected
<<<<<<< HEAD
	private void LockGridCell()
	{
		if (openGrid () != 0) {
=======
	private void LockGridCell() {
		if (openGrid ()) {
>>>>>>> 12001057384a4bb0fbb65a9273bf3c60ea91afd2
			randomRow = Random.Range (0, 9);
			randomCol = Random.Range (0, 9);
			while (board [randomRow, randomCol].GetComponent<Cell> ().Locked ||
			       board [randomRow, randomCol].GetComponent<Cell> ().Val != -1) {
				randomRow = Random.Range (0, 9);
				randomCol = Random.Range (0, 9);
			}
		}
		board [randomRow, randomCol].GetComponent<Cell> ().Locked = true;
		GameObject gridLock = Instantiate (lockPrefab);
		gridLock.transform.position = (board [randomRow, randomCol].GetComponent<Cell> ().transform.position);
		lockList.Add (gridLock);
	}

	// unlocks the grid cell that is selected if it is locked
	private void UnlockGridCell() {
		print ("Unlock Grid");
		selectedCell.GetComponent<Cell> ().Locked = false;
		foreach( GameObject Lock in lockList)
		{
			if (Lock.transform.position == selectedCell.GetComponent<Cell> ().transform.position)
			{
				lockList.Remove (Lock);
				Destroy (Lock);
			}
		}
	}

	// stuns a player, either yourself if you choose an incorrct cell or the enemy if you ge tthe stun power-up
	private void Stun(int seconds) {
		print ("Stun");
		stunned = true;
		stunTime = seconds;
	}

	// makes a cell not visible when opponent gets squid ink ability
	private void SquidInk() {
		randomRow = Random.Range (0, 7);
		randomCol = Random.Range (0, 7);
		for (int r = randomRow; r < randomRow + 3; r++) {
			for (int c = randomCol; c <  randomCol + 3; c++) {
				GameObject squidInk = Instantiate (inkPrefab);
				squidInk.transform.position = board [r, c].GetComponent<Cell> ().transform.position;
			}
		}
	}



	// a lion runs across a certain row and scares off all the animals from that row
	//MAKE IT SO THAT THE SPRITES ON THE POSITIONS ARE DESTROYED
<<<<<<< HEAD
	private void LionScare()
	{
		if (openGrid () < 5)
			lionScareCount = openGrid ();
		else
			lionScareCount = 5;
		while (lionScareCount > 1) 
		{
			randomRow = Random.Range (0, 9);
			randomCol = Random.Range (0, 9);
			while (board [randomRow, randomCol].GetComponent<Cell> ().Val == -1) {
				randomRow = Random.Range (0, 9);
				randomCol = Random.Range (0, 9);
			}
			board [randomRow, randomCol].GetComponent<Cell> ().Val = -1;
			foreach (GameObject sprite in board[randomRow,randomCol].GetComponent<Cell>().childList) {
				//Destroy (sprite);
				board [randomRow, randomCol].GetComponent<Cell> ().childList.Remove (sprite);
=======
	private void LionScare() {
		print ("Lion Scare");
		randomRow = Random.Range (0, 9);
		for (int c = 0; c < 9; c++) {
			board [randomRow, c].GetComponent<Cell> ().Val = -1;
			foreach (GameObject sprite in board[randomRow,c].GetComponent<Cell>().childList)
			{
>>>>>>> 12001057384a4bb0fbb65a9273bf3c60ea91afd2
				Destroy (sprite);
			}
			lionScareCount -= 1;
		}




	}


	// when your power-up meter is full, select a random power
	public void PowerUp() {
		power = Random.Range (1, 6);
		if (power == 1)
			Stun (5);
		else if (power == 2)
			LockGridCell ();
		else if (power == 3) 
			unlockPress = true;
		else if (power == 4)
			LionScare ();
		else if (power == 5)
			SquidInk ();
	}


	private void Place() {
		//if placement is correct and cell isn't locked 
		if (!selectedCell.GetComponent<Cell> ().Locked &&
			 selectedCell.GetComponent<Cell> ().Val < 0)
		{
			selectedCell.GetComponent<Cell> ().Val = pointerNum;

			// Call timer bar object
			TimerBar.GetComponent<Timer>().IncreaseTimer();
		}
		//if you try to place something in a locked grid 
		else if (selectedCell.GetComponent<Cell> ().Locked) 
		{
			// TODO: some interaction that lets the user know they can't do this
		}
		//if placement is incorrect and cell is unlocked
		else
		{
			Stun (2);
		}
	}
}