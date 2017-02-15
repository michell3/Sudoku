using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CageRattle : MonoBehaviour {
	
	int direction = 1;
	float rotate = 0;
	float time = 30;
	int bound = 5;
	int flip = 0;

	void Update () {
		time -= Time.deltaTime;
		if (time > 0) {
			if (time > 15 && time < 25)
				rotate = .3f;
			else if (time > 5 && time < 15)
			{
				rotate = .6f;
				bound = 6;
			}
			else if (time < 5)
			{
				rotate = 1;
				bound = 7;
			}
			transform.Rotate (0, 0, rotate * direction);
			flip += 1;
			if (flip == bound) {
				direction = -direction;
				flip = 0;
			}
		} 
		else
			Destroy (this.gameObject);
	}
}
