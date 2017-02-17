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

	// Sound
	public AudioClip SelectAudio;
	private AudioSource selectAudio;
	public AudioClip PlaceAudio;
	private AudioSource placeAudio;
	public AudioClip CompleteAudio;
	private AudioSource completeAudio;
	public AudioClip PowerupAudio;
	private AudioSource powerupAudio;

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
	private int comebackThreshold = 7;

	public int NumAnimals
	{
		get{ return numAnimals; }
	}
		
	private bool isGameOver = false;

	private int randomIndex;

	private GameObject selectedCell;

	private int pointerCol;
	private int pointerRow;

	private int pointerNum;
	public GameObject numberBar;
	public GameObject powerupBar;
	public GameObject[] weakPowerups; 
	public GameObject[] strongPowerups;
	private List<GameObject> myPowerups;

	private int randomRow;
	private int randomCol;
	private int power;

	private float stunTime = 0;
	private bool stunned = false;

	private int lionScareCount;

	private bool P1justMovedHorizontal = false;
	private bool P1justMovedVertical = false;
	private bool P1justMovedRightTrigger = false;
	private bool P1justMovedLeftTrigger = false;

	private bool P2justMovedHorizontal = false;
	private bool P2justMovedVertical = false;
	private bool P2justMovedRightTrigger = false;
	private bool P2justMovedLeftTrigger = false;

	private Dictionary<string, KeyCode> controls;

	private int[,] answer, show;

	//Back To Menu
	private int back = 0;

	//Define different controls for different players
	Dictionary<string, KeyCode> p2Controls = 
		new Dictionary<string, KeyCode> () {
		{"up", KeyCode.UpArrow},
		{"down", KeyCode.DownArrow},
		{"left", KeyCode.LeftArrow},
		{"right", KeyCode.RightArrow},
		{"place", KeyCode.Space},
		{"chooseDown", KeyCode.Comma},
		{"chooseUp", KeyCode.Period},
		{"activate", KeyCode.RightShift},
		{"cheat", KeyCode.RightControl}
	};

	Dictionary<string, KeyCode> p1Controls = 
		new Dictionary<string, KeyCode> () {
		{"up", KeyCode.W},
		{"left", KeyCode.A},
		{"down", KeyCode.S},
		{"right", KeyCode.D},
		{"place", KeyCode.F},
		{"chooseDown", KeyCode.Z},
		{"chooseUp", KeyCode.X},
		{"activate", KeyCode.LeftShift},
		{"cheat", KeyCode.LeftControl}
	};

	void Awake () {
		
		board = new GameObject[rows, columns];
		enemyBoard = EnemyBoard;
		cb = Char.GetComponent<CharacterBehavior> ();

		myPowerups = new List<GameObject> (); 

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

		// Sound
		selectAudio = gameObject.AddComponent<AudioSource>();
		selectAudio.clip = SelectAudio;
		placeAudio = gameObject.AddComponent<AudioSource>();
		placeAudio.clip = PlaceAudio;
		completeAudio = gameObject.AddComponent<AudioSource>();
		completeAudio.clip = CompleteAudio;
		powerupAudio = gameObject.AddComponent<AudioSource>();
		powerupAudio.clip = PowerupAudio;

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

		//play sound
		if (selectAudio) {
			selectAudio.Play ();
		}
	}

	void Update() {
		//makes animals feared by lion fall
		animalDescend ();

		if (!isGameOver) {

			if (!stunned) {
				P1XBoxControls ();
				P2XBoxControls ();

			} else {
				stunTime -= Time.deltaTime;
				if (stunTime < 0)
					stunned = false;
			}
			updatePowerupBar ();
		}
	}

	public int backToMenu(){
		return back;
	}

	private void updatePowerupBar() {
	
		GameObject temp;
		for (int i = 0; i < myPowerups.Count; i++) {
			temp = powerupBar.transform.GetChild (i).gameObject;
			myPowerups [i].transform.parent = temp.transform; 
			myPowerups [i].transform.localPosition = Vector3.zero;
		}
	}
		
	private void choosePointerNum(int move) {
	
		selectSprite (false); // deselect current sprite
		pointerNum = ((rows + pointerNum + move) % rows); 
		selectSprite(true); // select new sprite

		// Play sound
		selectAudio.Play();
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
		cb.GetHurt ((float)seconds);
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
			if(animal != null)
			{
				animal.transform.Translate (0, -.1f, 0);
				if (animal.transform.position.y < -6) {
					descendList.Remove (animal);
					Destroy (animal);
				}
			}
		}
	}

	// when your power-up meter is full, select a random power
	public void CastPowerUp() {
		power = Random.Range (1, 5);
		if (power == 1)
			cb.ThrowDart ();
		else if (power == 2)
			cb.ThrowLock();
		else if (power == 3)
			cb.LionAttack();
		else if (power == 4)
			cb.SquidAttack();
	}

	public void GainPowerUp() {
		//cant have more than 4 powerups at a time
		if (myPowerups.Count >= 4)
			return;
		int powerupIndex;
		GameObject p;
		if (numAnimals + comebackThreshold < enemyBoard.GetComponent<BoardManager> ().NumAnimals) {
			powerupIndex = Random.Range (0, strongPowerups.Length); // comeback - best powerup in last index
			p = Instantiate (strongPowerups [powerupIndex], gameObject.transform);
		} else {
			powerupIndex = Random.Range (0, weakPowerups.Length);
			p = Instantiate (weakPowerups [powerupIndex], gameObject.transform);
		}
		myPowerups.Add (p);
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

			// Play sound
			placeAudio.Play();

			if (numAnimals == 81) {
				GameOver (true);
			}

			CheckComplete (pointerRow, pointerCol);
		}
		//if you try to place something in a locked grid 
		else if (selectedCell.GetComponent<Cell> ().Locked)  {
			// TODO: some interaction that lets the user know they can't do this
			Stun (2);
		}
		//if placement is incorrect and cell is unlocked
		else {
			Stun (2);
		}
	}

	private void CheckComplete(int row, int col){
		GameObject[] toCheck = new GameObject[9];
		HashSet<Cell> toSpin = new HashSet<Cell> (); 

		//check row

		for (int i = 0; i < columns; i++) {
			toCheck[i] = board[row, i];
		}
		toSpin.UnionWith (CheckCompleteHelper (toCheck));

		//Check column
		for (int i = 0; i < rows; i++) {
			toCheck[i] = board[i, col];
		}
		toSpin.UnionWith (CheckCompleteHelper (toCheck));

		//check box
		int boxStartRow = (row / 3) * 3;
		int boxStartCol = (col / 3) * 3;

		int index = 0; 

		for (int i = boxStartRow; i < boxStartRow + 3; i++){
			for (int j = boxStartCol; j < boxStartCol + 3; j++) {
				toCheck [index] = board [i, j];
				index++; 
			}
		}

		toSpin.UnionWith (CheckCompleteHelper (toCheck));

		Cell[] toSpinArray = new Cell[toSpin.Count];
		toSpin.CopyTo (toSpinArray);

		foreach (Cell c in toSpinArray){
			c.GetComponent<Cell>().Spinning = true;
			c.GetComponent<Cell>().GameOver = isGameOver;
		}

		// Play sound
		if (toSpin.Count > 0) {
			completeAudio.Play ();
		}
	}

	//returns a set of cells that are complete
	private HashSet<Cell> CheckCompleteHelper(GameObject[] toCheck){
		HashSet<Cell> tempCells = new HashSet<Cell> ();
		Cell curCell; 

		for (int i = 0; i < toCheck.Length; i++){
			curCell = toCheck [i].GetComponent<Cell>();
			if (curCell.Val < 0) {
				//not complete, return empty set
				tempCells.Clear ();
				break;
			}
			tempCells.Add (curCell);
		}
		return tempCells;
	}


	//XBOX CONTROLLER CONTROLS
	private void P1XBoxControls() {
		if (isP1) {
			//movement around the grid using Analog Stick
			if (Input.GetAxis ("J_MainHorizontal") > .5 && !P1justMovedHorizontal) {
				P1justMovedHorizontal = true;
				Select (pointerRow, pointerCol + 1);
			} else if (Input.GetAxis ("J_MainHorizontal") < -.5 && !P1justMovedHorizontal) {
				P1justMovedHorizontal = true;
				Select (pointerRow, pointerCol - 1);
			} else if (Input.GetAxis ("J_MainVertical") > .5 && !P1justMovedVertical) {
				P1justMovedVertical = true;
				Select (pointerRow + 1, pointerCol);
			} else if (Input.GetAxis ("J_MainVertical") < -.5 && !P1justMovedVertical) {
				P1justMovedVertical = true;
				Select (pointerRow - 1, pointerCol);
			}

			//reset analog stick so you can move again
			if (Input.GetAxis ("J_MainHorizontal") == 0)
				P1justMovedHorizontal = false;
			if (Input.GetAxis ("J_MainVertical") == 0)
				P1justMovedVertical = false;

			//placing the sprites
			if (Input.GetButtonDown ("A_Button")) {
				Handheld.Vibrate ();
				Place ();
			}

			//scrolling through sprites to place
			if ((Input.GetAxis ("Left_Trigger") > .8f) && !P1justMovedLeftTrigger) {
				P1justMovedLeftTrigger = true;
				choosePointerNum (-1);
			} else if ((Input.GetAxis ("Right_Trigger") > .8f) && !P1justMovedRightTrigger) {
				P1justMovedRightTrigger = true;
				choosePointerNum (1);
			}	
			//reset triggers to be able to be placed again
			if (Input.GetAxis ("Right_Trigger") == 0)
				P1justMovedRightTrigger = false;
			if (Input.GetAxis ("Left_Trigger") == 0)
				P1justMovedLeftTrigger = false;

			//test power-ups
			if (Input.GetButtonDown ("X_Button") && myPowerups.Count > 0 && !stunned)
			{
				GameObject temp = myPowerups [0];
				((Powerup)temp.GetComponent<Powerup> ()).Activate (cb); //call the activation method for powerup
				myPowerups.Remove (temp); //delete and destroy powerup
				Destroy (temp); 
			} 

			if (Input.GetButtonDown ("B_Button"))
				SceneManager.LoadScene ("Splash_Screen");

			//WASD Logic
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

			} else if (Input.GetKeyDown (controls ["activate"]) && 
				myPowerups.Count > 0 &&
				!stunned) {
				GameObject temp = myPowerups [0];
				((Powerup)temp.GetComponent<Powerup> ()).Activate (cb); //call the activation method for powerup
				myPowerups.Remove (temp); //delete and destroy powerup
				Destroy (temp); 
			}

			if (TimerBar.GetComponent<Timer> ().IsPoweredUp () == true) {
				powerupAudio.Play ();
				GainPowerUp ();
			}

			if (Input.GetKeyDown (controls ["cheat"])) {
				powerupAudio.Play ();
				GainPowerUp ();
			}
		}
	}

	private void P2XBoxControls() {
		if (!isP1) {

			//CONTROLLER INPUTS
			//movement around the grid using Analog Stick
			if (Input.GetAxis ("PC_J_MainHorizontal") > .5 && !P2justMovedHorizontal) {
				P2justMovedHorizontal = true;
				Select (pointerRow, pointerCol + 1);
			} else if (Input.GetAxis ("PC_J_MainHorizontal") < -.5 && !P2justMovedHorizontal) {
				P2justMovedHorizontal = true;
				Select (pointerRow, pointerCol - 1);
			} else if (Input.GetAxis ("PC_J_MainVertical") > .5 && !P2justMovedVertical) {
				P2justMovedVertical = true;
				Select (pointerRow + 1, pointerCol);
			} else if (Input.GetAxis ("PC_J_MainVertical") < -.5 && !P2justMovedVertical) {
				P2justMovedVertical = true;
				Select (pointerRow - 1, pointerCol);
			}

			//reset analog stick so you can move again
			if (Input.GetAxis ("PC_J_MainHorizontal") == 0)
				P2justMovedHorizontal = false;
			if (Input.GetAxis ("PC_J_MainVertical") == 0)
				P2justMovedVertical = false;

			//placing the sprites
			if (Input.GetButtonDown ("PC_A_Button")) {
				Handheld.Vibrate ();
				Place ();
			}

			//scrolling through sprites to place
			if ((Input.GetAxis ("PC_Left_Trigger") > .8f) && !P2justMovedLeftTrigger) {
				P2justMovedLeftTrigger = true;
				choosePointerNum (-1);
			} else if ((Input.GetAxis ("PC_Right_Trigger") > .8f) && !P2justMovedRightTrigger) {
				P2justMovedRightTrigger = true;
				choosePointerNum (1);
			}	
			//reset triggers to be able to be placed again
			if (Input.GetAxis ("PC_Right_Trigger") == 0)
				P2justMovedRightTrigger = false;
			if (Input.GetAxis ("PC_Left_Trigger") == 0)
				P2justMovedLeftTrigger = false;

			//test power-ups
			if (Input.GetButtonDown ("PC_X_Button") && myPowerups.Count > 0 && !stunned)
			{
				GameObject temp = myPowerups [0];
				((Powerup)temp.GetComponent<Powerup> ()).Activate (cb); //call the activation method for powerup
				myPowerups.Remove (temp); //delete and destroy powerup
				Destroy (temp); 
			} 

			if (Input.GetButtonDown ("PC_B_Button"))
				SceneManager.LoadScene ("Splash_Screen");

			//HARD CODED KEYBOARD INPUTS
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

			} else if (Input.GetKeyDown (controls ["activate"]) && 
				myPowerups.Count > 0 &&
				!stunned) {
				GameObject temp = myPowerups [0];
				((Powerup)temp.GetComponent<Powerup> ()).Activate (cb); //call the activation method for powerup
				myPowerups.Remove (temp); //delete and destroy powerup
				Destroy (temp); 
			}

			if (TimerBar.GetComponent<Timer> ().IsPoweredUp () == true) {
				powerupAudio.Play ();
				GainPowerUp ();
			}
				
			if (Input.GetKeyDown (controls ["cheat"])) {
				powerupAudio.Play ();
				GainPowerUp ();
			}
		}
	}

	public int pointerNumber() {
		return pointerNum;
	}

	public void GameOver(bool isWinner) {
		isGameOver = true;
		TimerBar.GetComponent<Timer> ().GameOver ();

		if (isWinner) {
			BoardManager enemyBM = enemyBoard.GetComponent<BoardManager> ();
			enemyBM.GameOver (false);

			//this is just a shitty way of making everything spin on a complete board
			for (int i = 0; i < rows; i++) {
				CheckComplete (i, 0);
			}

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
