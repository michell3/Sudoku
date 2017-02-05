using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour {

	public GameObject GiraffeSprite;
	public GameObject FlamingoSprite;
	public GameObject SeahorseSprite;
	public GameObject ElephantSprite;
	public GameObject SnakeSprite;
	public GameObject SealSprite;
	public GameObject FoxSprite;
	public GameObject BearSprite;
	public GameObject TurtleSprite;

	public GameObject pointer;

	public GameObject cell;

	private GameObject[] spriteList;

	private GameObject[,] board;

	public int columns = 9;
	public int rows = 9;

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

	void Awake () {
		

		spriteList = new GameObject[9];
		spriteList [0] = GiraffeSprite;
		spriteList [1] = FlamingoSprite;
		spriteList [2] = SeahorseSprite;
		spriteList [3] = ElephantSprite;
		spriteList [4] = SnakeSprite;
		spriteList [5] = SealSprite;
		spriteList [6] = FoxSprite;
		spriteList [7] = BearSprite;
		spriteList [8] = TurtleSprite;

		board = new GameObject[rows, columns];

		spriteScale = GiraffeSprite.transform.localScale.x;
		spacing = Spacing * spriteScale;
		leftOffset = LeftOffset * spriteScale;
		topOffset = TopOffset * spriteScale;

		boardWidth = GetComponent<SpriteRenderer>().bounds.size.x;
		boardHeight = GetComponent<SpriteRenderer>().bounds.size.y;
		leftBound = leftOffset + transform.position.x - (boardWidth / 2f);
		upperBound = topOffset + transform.position.y - (boardHeight / 2f);
		spriteWidth = GiraffeSprite.GetComponent<SpriteRenderer>().bounds.size.x;
		spriteHeight = GiraffeSprite.GetComponent<SpriteRenderer>().bounds.size.y;

		DisplayBoard ();
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
		//moving the selector. 
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			Select (pointerRow - 1, pointerCol);
		}
		else if (Input.GetKeyDown (KeyCode.UpArrow)) {
			Select (pointerRow + 1, pointerCol);
		}
		else if (Input.GetKeyDown (KeyCode.LeftArrow)) {
			Select (pointerRow, pointerCol - 1);
		}
		else if (Input.GetKeyDown (KeyCode.RightArrow)) {
			Select (pointerRow, pointerCol + 1);
		}

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
