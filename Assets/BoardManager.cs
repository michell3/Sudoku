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

	private GameObject[] spriteList;

	private int[,] board;

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

		board = RandomAssBoard ();

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
		int val;
		float x, y;
		for (int c = 0; c < 9; c++) {
			for (int r = 0; r < 9; r++) {
				val = board [r, c];
				if (val != 0) {
					x = leftBound + c * (spriteWidth + spacing) + (spriteWidth / 2f);
					y = upperBound + r * (spriteHeight + spacing) + (spriteHeight / 2f);
					Instantiate (spriteList [val - 1],
								 new Vector3 (x, y, -1f),
								 Quaternion.identity);
				}
			}
		}
	}

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
	}
}
