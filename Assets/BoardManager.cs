using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardManager : MonoBehaviour {
	//for the purposes of sizing
	public GameObject sampleSprite;
	public GameObject pointer;
	public GameObject cell;

	public Image image; // timer bar
	public Image image2; // powerup bar
	public float timeSpeed = 30.0f; // to adjust the speed of countdown timer
	public bool increment;
	public bool decrement;

	private GameObject[,] board;

	public int columns = 9;
	public int rows = 9;

	public bool isP1 = true;

	private bool powerup = false; // use to see if powerup bar is filled

	private float boardWidth;
	private float boardHeight;
	private float leftBound;
	private float upperBound;
	private float spriteWidth;
	private float spriteHeight;
	private float spriteScale;

	public float Spacing = .8f;
	public float LeftOffset = 1.2f;
	public float TopOffset = 1.2f;
	private float spacing;
	private float leftOffset;
	private float topOffset;

	private GameObject selectedCell;

	private int pointerCol;
	private int pointerRow;

	private int pointerNum;

	private Dictionary<string, KeyCode> controls;


	//Define different controls for different players
	Dictionary<string, KeyCode> p1Controls = 
		new Dictionary<string, KeyCode> () {
		{"up", KeyCode.UpArrow},
		{"down", KeyCode.DownArrow},
		{"left", KeyCode.LeftArrow},
		{"right", KeyCode.RightArrow},
		{"place", KeyCode.Space},
		{"chooseUp", KeyCode.N},
		{"chooseDown", KeyCode.B}
		};

	Dictionary<string, KeyCode> p2Controls = 
		new Dictionary<string, KeyCode> () {
		{"up", KeyCode.W},
		{"down", KeyCode.S},
		{"left", KeyCode.A},
		{"right", KeyCode.D},
		{"place", KeyCode.R},
		{"chooseUp", KeyCode.Y},
		{"chooseDown", KeyCode.T}
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
	}



		
	private void Select(int row, int col){
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

	void Update(){
		//if the battle starts
		Timer();

		//moving the selector. 
		if (Input.GetKeyDown (controls["down"])) {
			Select (pointerRow - 1, pointerCol);
		}
		else if (Input.GetKeyDown (controls["up"])) {
			Select (pointerRow + 1, pointerCol);
		}
		else if (Input.GetKeyDown (controls["left"])) {
			Select (pointerRow, pointerCol - 1);
		}
		else if (Input.GetKeyDown (controls["right"])) {
			Select (pointerRow, pointerCol + 1);
		}

		if (Input.GetKeyDown (controls["place"])) {
			Place ();
		} 
		else if (Input.GetKeyDown(controls["chooseDown"])) {
			//make sure to wrap around # of rows/columns, then add 1 since
			//we are 1-indexing
			pointerNum = ((rows + pointerNum - 1) % rows); 
			Debug.Log (pointerNum);
		}
		else if (Input.GetKeyDown (controls["chooseUp"])) {
			//make sure to wrap around # of rows/columns, then add 1 since
			//we are 1-indexing
			pointerNum = ((rows + pointerNum + 1) % rows); 
		}



	}
		

	private void Timer(){
		if (increment == true) {
		
			image.fillAmount -= 1.0f / timeSpeed * Time.deltaTime;

			if (image.fillAmount == 0) {
				image.fillAmount = 1;
			}
		}



	}

	private void Place(){
		//if incorrect 


		//if correct 
		selectedCell.GetComponent<Cell> ().Val = pointerNum;
		if (decrement == true) {
			image2.fillAmount = image2.fillAmount + image.fillAmount;
			if (image2.fillAmount == 1) {
				image2.fillAmount = 0;
				powerup = true;
			}
			image.fillAmount = 1;
		}
		//timer logic

	}


	/*
	int[,] RandomAssBoard () {
		int[,] newBoard = { { 0, 0, 0, 0, 0, 0, 9, 2, 6 },
							{ 2, 6, 0, 9, 1, 0, 5, 0, 0 },
							{ 0, 5, 4, 0, 3, 0, 0, 0, 0 },
							{ 6, 0, 0, 8, 0, 5, 0, 9, 7 },
							{ 8, 0, 0, 0, 0, 0, 0, 0, 1 },
							{ 5, 4, 0, 1, 0, 9, 0, 0, 2 },
							{ 0, 0, 0, 0, 2, 0, 1, 6, 0 },
							{ 0, 0, 2, 0, 9, 6, 0, 3, 5 },
							{ 3, 8, 6, 0, 0, 0, 0, 0, 0 } };
		return newBoard;
	}*/
}
