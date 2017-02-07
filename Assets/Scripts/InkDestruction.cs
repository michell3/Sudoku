using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkDestruction : MonoBehaviour {
	private float startTime = 10.0f;

	
	// Update is called once per frame
	void Update () {
		startTime -= Time.deltaTime;
		if (startTime < 0)
			Destroy (this.gameObject);
	}
}
