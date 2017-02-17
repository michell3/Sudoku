using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusic : MonoBehaviour {

	public float AudioVolume = 0.6f;
	private float audioVolume;

	private AudioSource audio;

	private static BGMusic _instance;
	public static BGMusic instance {

		get {
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType<BGMusic> ();
				DontDestroyOnLoad (_instance.gameObject);
			}
			return _instance;
		}
	}

	void Awake () {

		audioVolume = AudioVolume;

		audio = GetComponent<AudioSource>();
		audio.volume = audioVolume;

		if (_instance == null) {
			_instance = this;
			DontDestroyOnLoad (this);
		} else {
			if (this != _instance) {
				Destroy (this.gameObject);
			}
		}
	}
}
