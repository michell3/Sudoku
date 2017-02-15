using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridShake : MonoBehaviour {

	private bool isPlaying = false;
	private float timer = 0f;
	private float shakeTime = 0f;

	public float Speed = 30f;
	public float Amount = 0.05f;
	private float speed;
	private float amount;

	private Vector3 originalPosition;

	void Awake () {
		originalPosition = transform.position;
		speed = Speed;
		amount = Amount;
	}

	void Update () {
		if (isPlaying) {
			transform.position = new Vector3 (originalPosition.x + amount * Mathf.Sin (Time.time * speed),
											  originalPosition.y, originalPosition.z);
			
			timer -= Time.deltaTime;

			if (timer < 0f) {
				timer = shakeTime;
				isPlaying = false;
			}

		} else {
			transform.position = originalPosition;
		}
	}

	public void Play(int seconds) {
		isPlaying = true;
		shakeTime = (float)seconds;
		timer = shakeTime;
	}
}
