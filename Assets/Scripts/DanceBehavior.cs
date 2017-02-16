using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceBehavior : MonoBehaviour {

	public float RotateAmount = 4f;
	public float TimeInterval = 0.3f;
	public float SpinInterval = 1.0f; 
	private float rotateAmount;
	private float timeInterval;

	private float timer;
	private float tempTimer;
	private float direction;

	private bool spinning; 
	public bool Spinning 
	{
		get{ return spinning; }
		set{ 
			spinning = value;
			tempTimer = timer;
			timer = SpinInterval;
		}
	}

	void Start () {
		rotateAmount = RotateAmount;
		timeInterval = TimeInterval;

		timer = timeInterval;
		direction = 1;

		transform.Rotate (0f, 0f, direction * rotateAmount);
		direction *= -1;
	}

	void Update () {
		
		timer -= Time.deltaTime;

		if (spinning) {
			transform.RotateAround(transform.position, Vector3.forward, 360 * Time.deltaTime / SpinInterval);

			//once done spinning 
			if (timer < 0f) {
				spinning = false; 
				transform.rotation = Quaternion.identity;
				timer = timeInterval;
			}
		}
		else if (timer < 0f) {
			transform.Rotate (0f, 0f, 2f * direction * rotateAmount);
			timer = timeInterval;
			direction *= -1f;
		}
	}
}
