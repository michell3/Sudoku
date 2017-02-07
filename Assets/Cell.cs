using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour
{
	private bool selected; 
	private bool locked; 
	private bool visible;

	private int val; 

	public GameObject[] spriteList; 

	public bool Selected 
	{
		get {return selected;}
		set{ selected = value;}

	}

	public int Val 
	{
		get{return val;}
		set{val = value;}

	}

	public bool Locked
	{
		get{ return locked; }
		set{ locked = value; }
	}

	public bool Visible 
	{
		get{ return visible; }
		set{ locked = value; }
	}

	// Use this for initialization
	void Start ()
	{
		selected = false;
		locked = false;
		visible = true;
		val = -1; 
	}
		
		



	// Update is called once per frame
	void Update ()
	{
		//TODO: add animal/number placement
		if (val >= 0 && gameObject.transform.childCount == 0)
		{
			GameObject newSprite = Instantiate (spriteList [val], gameObject.transform);
			newSprite.transform.position = gameObject.transform.position;

		}
			
	}
}

