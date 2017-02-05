using UnityEngine;
using System.Collections;

public class Cell : MonoBehaviour
{
	private bool selected; 

	private int val; 

	public GameObject[] sprites; 

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
		if (val >= 0 && !gameObject.GetComponent<SpriteRenderer>()){
			GameObject newSprite = Instantiate (sprites [val], gameObject.transform);

		}
			

	}
}

