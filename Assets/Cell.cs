using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour
{
	private bool selected; 
	private bool locked; 

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

	// Use this for initialization
	void Start ()
	{
		selected = false;
		val = -1; 
	}
	
	// Update is called once per frame
	void Update ()
	{
		//TODO: add animal/number placement
		if (val >= 0 && gameObject.transform.childCount == 0){
			GameObject newSprite = Instantiate (spriteList [val], gameObject.transform);
			newSprite.transform.position = gameObject.transform.position;

		}
			

	}
}

