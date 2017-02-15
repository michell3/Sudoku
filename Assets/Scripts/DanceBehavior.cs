using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceBehavior : MonoBehaviour {

	public float RotateAmount = 4f;
	public float TimeInterval = 0.3f;
	private float rotateAmount;
	private float timeInterval;

	private float timer;
	private float direction;

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

		if (timer < 0f) {
			transform.Rotate (0f, 0f, 2f * direction * rotateAmount);
			timer = timeInterval;
			direction *= -1f;
		}
	}
}
