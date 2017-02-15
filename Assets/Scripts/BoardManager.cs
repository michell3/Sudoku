using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BoardManager : MonoBehaviour {
	
	// For the purposes of sizing
	public GameObject Char;
	private CharacterBehavior cb;
	public GameObject sampleSprite;
	public GameObject pointer;
	public GameObject cell;
	public GameObject lockPrefab;
	public GameObject inkPrefab;
	public List<GameObject> lockList = new List<GameObject>();
	private List<GameObject> descendList = new List<GameObject> ();

	// Timer gameobject
	public GameObject TimerBar;

	// Board gameobjects
	private GameObject[,] board;
	public GameObject EnemyBoard;
	private GameObject enemyBoard;

	// Display gameobjects
	public GameObject LoseBoard;
	public GameObject WinBoard;

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
	private float boardScale;

	// Display parameters
	public float Spacing = .8f;
	public float LeftOffset = 1.2f;
	public float TopOffset = 1.2f;
	private float spacing;
	private float leftOffset;
	private float topOffset;

	// Game state variables
	public bool isP1 = true;
	private int numAnimals = 0;
	private bool isGameOver = false;

	private int randomIndex;

	private GameObject selectedCell;

	private int pointerCol;
	private int pointerRow;

	private int pointerNum;
	public GameObject numberBar;
	public GameObject powerupBar;
	public GameObject[] powerupSprites; 
	private List<GameObject> powerups;

	private int randomRow;
	private int randomCol;
	private int power;

	private float stunTime = 0;
	private bool stunned = false;

	private int lionScareCount;

	private bool justMovedHorizontal = false;
	private bool justMovedVertical = false;

	private bool justMovedRightTrigger = false;
	private bool justMovedLeftTrigger = false;

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
		{"lock", KeyCode.LeftShift},
		{"activate", KeyCode.M}
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
		{"lock", KeyCode.RightShift},
		{"activate", KeyCode.U}
	};

	void Awake () {
		
		board = new GameObject[rows, columns];
		enemyBoard = EnemyBoard;
		cb = Char.GetComponent<CharacterBehavior> ();

		powerups = new List<GameObject> (); 

		// Variables for placing animals on grid
		spriteScale = sampleSprite.transform.localScale.x;
		boardScale = GetComponent<SpriteRenderer> ().transform.localScale.x;
		spacing = Spacing * spriteScale * boardScale;
		leftOffset = LeftOffset * spriteScale * boardScale;
		topOffset = TopOffset * spriteScale * boardScale;

		boardWidth = GetComponent<SpriteRenderer>().bounds.size.x;
		boardHeight = GetComponent<SpriteRenderer>().bounds.size.y;
		leftBound = leftOffset + transform.position.x - (boardWidth / 2f);
		upperBound = topOffset + transform.position.y - (boardHeight / 2f);
		spriteWidth = sampleSprite.GetComponent<SpriteRenderer>().bounds.size.x;
		spriteHeight = sampleSprite.GetComponent<SpriteRenderer>().bounds.size.y;

		DisplayBoard ();

		// Player controls
		if (isP1)
			controls = p1Controls;
		else
			controls = p2Controls;
	}

	void DisplayBoard () {

		// Create cells that contain all animal sprites
		float x, y;
		for (int c = 0; c < 9; c++) {
			for (int r = 0; r < 9; r++) {
				x = leftBound + c * (spriteWidth + spacing) + (spriteWidth / 2f);
				y = upperBound + r * (spriteHeight + spacing) + (spriteHeight / 2f);
				GameObject instance = Instantiate (cell,
					new Vector3 (x, y, -1f),
					Quaternion.identity);
				instance.transform.parent = transform;
				board [r, c] = instance;
			}
		}

		Select (8, 0);  // select upper left cell
		pointerNum = 0; // the number users put in the cell

		// choose a random puzzle
		randomIndex = (int)Mathf.Floor (Random.value * 100f + 1f);
		answer = RefBoard.getAnswerBoard(randomIndex);
		show = RefBoard.getShowBoard (randomIndex);

		// update the sprites on the board
		for (int c = 0; c < 9; c++) {
			for (int r = 0; r < 9; r++) {
				if (show [r, c] > 0) {
					board [r, c].GetComponent<Cell> ().Val = answer [r, c] - 1;
					numAnimals++;
				}
			}
		}
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
		//makes animals feared by lion fall
		animalDescend ();

		if (!isGameOver) {

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

				if (Input.GetKeyDown (controls ["place"])) {
					Place ();
				} else if (Input.GetKeyDown (controls ["chooseDown"])) {
					//make sure to wrap around # of rows/columns, then add 1 since
					//we are 1-indexing
					choosePointerNum (-1);

				} else if (Input.GetKeyDown (controls ["chooseUp"])) {
					//make sure to wrap around # of rows/columns, then add 1 since
					//we are 1-indexing
					choosePointerNum (1);

				} else if (Input.GetKeyDown (controls ["activate"]) && powerups.Count > 0) {
					GameObject temp = powerups [0];
					((Powerup)temp.GetComponent<Powerup> ()).Activate (); //call the activation method for powerup
					powerups.Remove (temp); //delete and destroy powerup
					Destroy (temp); 
				}

				if (TimerBar.GetComponent<Timer> ().IsPoweredUp () == true) {
					if (isP1)
						CastPowerUp ();
				}

				//REMEMBER TO DELETE THIS
				if (Input.GetKeyDown (KeyCode.G)) {
					cb.LionAttack ();
//					cb.SquidAttack();
//					Stun (2);
//					Restart ();
//					GameOver (true);
//					if (isP1)
//						GainPowerUp ();
				}

				if (Input.GetKeyDown (controls ["lock"])) {
					//LockGridCell ();
					GainPowerUp ();
				} 

				P1XBoxControls ();

			} else {
				stunTime -= Time.deltaTime;
				if (stunTime < 0)
					stunned = false;
			}

			updatePowerupBar ();

		} else {
			// GameOver UI controls should be implemented here
		}
	}

	private void updatePowerupBar() {
	
		GameObject temp;
		for (int i = 0; i < powerups.Count; i++) {
			temp = powerupBar.transform.GetChild (i).gameObject;
			powerups [i].transform.parent = temp.transform; 
			powerups [i].transform.localPosition = Vector3.zero;
		}
	}
		
	private void choosePointerNum(int move) {
	
		selectSprite (false); // deselect current sprite
		pointerNum = ((rows + pointerNum + move) % rows); 
		selectSprite(true); // select new sprite
	
	}

	private void selectSprite(bool select) {
	
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

	//returns how many cells are empty and not locked
	private int openGrid() {
	
		int openCells = 0;
		for(int r = 0; r < 9; r++) {
			for(int c = 0; c < 9; c++) {
				if(board[r,c].GetComponent<Cell>().Val == -1 &&
				   board[r,c].GetComponent<Cell>().Locked == false)
					openCells += 1;
			}
		}
		return openCells;
	}

	//counts how many animals are placed on the board
	private int animalCount() {
	
		int animals = 0;
		for(int r = 0; r < 9; r++) {
			for(int c = 0; c < 9; c++) {
				if(board[r,c].GetComponent<Cell>().Val > -1)
					animals += 1;
			}
		}
		return animals;
	}

	// locks the grid cell that is selected
	public void LockGridCell() {
	
		if (openGrid () != 0) {
			randomRow = Random.Range (0, 9);
			randomCol = Random.Range (0, 9);
			while (board [randomRow, randomCol].GetComponent<Cell> ().Locked ||
			       board [randomRow, randomCol].GetComponent<Cell> ().Val != -1) {
				randomRow = Random.Range (0, 9);
				randomCol = Random.Range (0, 9);
			}
		}
		if (board [randomRow, randomCol].GetComponent<Cell> ().Locked == false) {
		
			board [randomRow, randomCol].GetComponent<Cell> ().Locked = true;
			board [randomRow, randomCol].GetComponent<Cell> ().lockTimer = 20;
			GameObject gridLock = Instantiate (lockPrefab);
			gridLock.transform.position = (board [randomRow,
				randomCol].GetComponent<Cell> ().transform.position);
			lockList.Add (gridLock);
		}
	}

	// stuns a player, either yourself if you choose an incorrect cell or the enemy if you ge tthe stun power-up
	public void Stun(int seconds) {

		GridShake gridShake = GetComponent<GridShake> ();
		gridShake.Play (seconds);
		stunned = true;
		stunTime = seconds;
	}

	// makes a cell not visible when opponent gets squid ink ability
	public void SquidInk()
	{
		randomRow = Random.Range (1, 7);
		randomCol = Random.Range (1, 7);
		GameObject squidInk = Instantiate (inkPrefab);
		squidInk.transform.position = board [randomRow,
			randomCol].GetComponent<Cell> ().transform.position;
	}

	// a lion runs across a certain row and scares off all the animals from that row
	//MAKE IT SO THAT THE SPRITES ON THE POSITIONS ARE DESTROYED
	public void LionScare() {
	
		if (animalCount() < 5)
			lionScareCount = animalCount ();
		else
			lionScareCount = 5;

		numAnimals -= lionScareCount;

		if (animalCount () != 0) {
		
			while (lionScareCount > 0) {
				randomRow = Random.Range (0, 9);
				randomCol = Random.Range (0, 9);
				while (board [randomRow, randomCol].GetComponent<Cell> ().Val == -1) {
					randomRow = Random.Range (0, 9);
					randomCol = Random.Range (0, 9);
				}
				board [randomRow, randomCol].GetComponent<Cell> ().Val = -1;
				foreach (GameObject sprite in board[randomRow,
						 	randomCol].GetComponent<Cell>().childList) {
					//Destroy (sprite);
					descendList.Add(sprite);
				}
				lionScareCount -= 1;
			}
		}
	}

	// makes the animals fall when they are scared by the lion, then destroys them
	private void animalDescend() {

		List<GameObject> copyList = new List<GameObject> (descendList);
	
		foreach (GameObject animal in copyList) {
				
			animal.transform.Translate (0, -.1f, 0);
			if (animal.transform.position.y < -6) {
				descendList.Remove (animal);
				Destroy (animal);
			}
		}
	}

	// when your power-up meter is full, select a random power
	public void CastPowerUp() {
	
		power = Random.Range (1, 5);
		if (power == 1)
			EnemyBoard.GetComponent<BoardManager>().Stun (5);
		else if (power == 2)
			//LockGridCell ();TimerBar.GetComponent<Timer>().IncreaseTimer();
			EnemyBoard.GetComponent<BoardManager>().LockGridCell();
		else if (power == 3)
			//LionScare ();
			EnemyBoard.GetComponent<BoardManager>().LionScare();
		else if (power == 4)
			EnemyBoard.GetComponent<BoardManager>().SquidInk ();
	}

	public void GainPowerUp() {
		//cant have more than 4 powerups at a time
		if (powerups.Count >= 4)
			return;
		GameObject p = Instantiate (powerupSprites [0], gameObject.transform);
		powerups.Add (p);
	}

	private bool Check() {
		return (answer [pointerRow, pointerCol] - 1 == pointerNum);
	}

	private void Place() {
	
		//if placement is correct and cell isn't locked 
		if (!selectedCell.GetComponent<Cell> ().Locked &&
			selectedCell.GetComponent<Cell> ().Val < 0 && Check()) {

			selectedCell.GetComponent<Cell> ().Val = pointerNum;

			// Call timer bar object
			TimerBar.GetComponent<Timer>().IncreaseTimer();

			numAnimals++;

			if (numAnimals == 81) {
				GameOver (true);
			}
		}
		//if you try to place something in a locked grid 
		else if (selectedCell.GetComponent<Cell> ().Locked)  {
			// TODO: some interaction that lets the user know they can't do this
		}
		//if placement is incorrect and cell is unlocked
		else {
			Stun (2);
		}
	}


	//XBOX CONTROLLER CONTROLS
	private void P1XBoxControls() {

		if (isP1) {
			//movement around the grid using Analog Stick
			if (Input.GetAxis ("J_MainHorizontal") > .5 && !justMovedHorizontal) {
				justMovedHorizontal = true;
				Select (pointerRow, pointerCol + 1);
			} else if (Input.GetAxis ("J_MainHorizontal") < -.5 && !justMovedHorizontal) {
				justMovedHorizontal = true;
				Select (pointerRow, pointerCol - 1);
			} else if (Input.GetAxis ("J_MainVertical") > .5 && !justMovedVertical) {
				justMovedVertical = true;
				Select (pointerRow + 1, pointerCol);
			} else if (Input.GetAxis ("J_MainVertical") < -.5 && !justMovedVertical) {
				justMovedVertical = true;
				Select (pointerRow - 1, pointerCol);
			}

			//reset analog stick so you can move again
			if (Input.GetAxis ("J_MainHorizontal") == 0)
				justMovedHorizontal = false;
			if (Input.GetAxis ("J_MainVertical") == 0)
				justMovedVertical = false;

			//placing the sprites
			if (Input.GetButtonDown ("A_Button"))
				Place ();

			//scrolling through sprites to place
			if (Input.GetAxis ("Left_Trigger") > .8f && !justMovedLeftTrigger) {
				justMovedLeftTrigger = true;
				choosePointerNum (-1);
			} else if (Input.GetAxis ("Right_Trigger") > .8f && !justMovedRightTrigger) {
				justMovedRightTrigger = true;
				choosePointerNum (1);
			}	
			//reset triggers to be able to be placed again
			if (Input.GetAxis ("Right_Trigger") < .2f)
				justMovedRightTrigger = false;
			if (Input.GetAxis ("Left_Trigger") < .2f)
				justMovedLeftTrigger = false;
		
		} else {
			//test power-ups
			if (Input.GetButtonDown ("Y_Button"))
				SquidInk ();
			if (Input.GetButtonDown ("B_Button"))
				LionScare ();
		}
	}

	public int pointerNumber() {
		return pointerNum;
	}

	public void GameOver(bool isWinner) {
		
		isGameOver = true;

		if (isWinner) {
			BoardManager enemyBM = enemyBoard.GetComponent<BoardManager> ();
			enemyBM.GameOver (false);

			Instantiate (WinBoard);
			cb.Winner ();
		} else {
			Instantiate (LoseBoard);
			cb.Loser ();
		}
	}

	private void Restart() {
		SceneManager.LoadScene ("Main");
	}
}
