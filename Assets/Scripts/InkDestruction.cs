using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkDestruction : MonoBehaviour {
	private float startTime = 12.5f;
	private float fadeDuration = 2.5f;
	private Color lerpedColor; 
	private float t; 
	void Start(){
		t = 0.0f; 

	}
	
	// Update is called once per frame
	void Update () {
		startTime -= Time.deltaTime;
		if (startTime < 0) {
			
			lerpedColor = Color.Lerp (new Color (1.0f, 1.0f, 1.0f, 1.0f), new Color (1.0f, 1.0f, 1.0f, 0.0f), t);
			gameObject.GetComponent<SpriteRenderer> ().color = lerpedColor;
			t += Time.deltaTime / fadeDuration; 
			if (t >= 1.0f)
				Destroy (this.gameObject);
		}
	}
}
