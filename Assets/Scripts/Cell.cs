using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cell : MonoBehaviour
{
	private bool selected; 

	private bool locked; 
	public float lockTimer;

	private bool wasSet; 

	private int val; 

	public GameObject[] spriteList; 

	public List<GameObject> childList = new List<GameObject> ();

	public bool Selected 
	{
		get {return selected;}
		set{ selected = value;}

	}

	public int Val 
	{
		get{return val;}
		set{val = value;
			wasSet = true;}

	}

	public bool Locked
	{
		get{ return locked; }
		set{ locked = value; }
	}
		

	// Use this for initialization
	void Start ()
	{
		lockTimer = 0;
		selected = false;
		locked = false;
		if (!wasSet)
			val = -1; 
	}
		
		
	private bool spinning;

	public bool Spinning
	{
		get{ return spinning; }
		set{ spinning = value; }
	}



	// Update is called once per frame
	void Update ()
	{
		//unlocks the grid after 20 seconds
		lockTimer -= Time.deltaTime;
		if (lockTimer < 0)
			locked = false;
		//TODO: add animal/number placement
		if (val >= 0 && gameObject.transform.childCount == 0)
		{
			GameObject newSprite = Instantiate (spriteList [val], gameObject.transform);
			newSprite.transform.position = gameObject.transform.position;
			childList.Add (newSprite);
		}

		if (spinning && childList.Count > 0) {
			gameObject.GetComponentInChildren<DanceBehavior> ().Spinning = true;
			spinning = false;
		}
			
	}
}

