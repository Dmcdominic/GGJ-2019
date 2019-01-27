using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
	public AudioSource flap;
	public AudioSource chirp;
	public AudioSource eat;
	public AudioSource pickupTwig;
	public AudioSource smallChirp;
	public AudioSource gliding;
	public bool_var gliding_var;
	public static SoundManager instance = null;
	public float lowPitchRange;
	public float highPitchRange;
	public float smallChirpRangeMult;


	// Start is called before the first frame update
	void Awake() {
		//Check if there is already an instance of SoundManager
		if (instance == null)
			//if not, set it to this.
			instance = this;
		//If instance already exists:
		else if (instance != this)
			//Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
			Destroy(gameObject);

		//Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
		DontDestroyOnLoad(gameObject);

		StartCoroutine("repeat_chrip");
	}

	// Turn the gliding sound on/off appropriately
	private void Update() {
		if (!gliding.isPlaying) {
			gliding.Play();
			gliding.volume = 0f;
		}

		if (gliding_var.val && gliding.volume < 1f) {
			gliding.volume += Time.deltaTime * 4;
		} else if (!gliding_var.val && gliding.volume > 0.1f) {
			gliding.volume -= Time.deltaTime * 3;
		}
	}

	IEnumerator repeat_chrip() {
		while (true) {
			yield return new WaitForSeconds(Random.Range(18f, 25f));
			chirp.Play();
		}
	}

	public void playFlap() {
		flap.pitch = Random.Range(lowPitchRange, highPitchRange);
		flap.Play();
	}

	public void playEat() {
		eat.pitch = Random.Range(lowPitchRange, highPitchRange);
		eat.Play();
	}

	public void playPickupTwig() {
		pickupTwig.pitch = Random.Range(lowPitchRange, highPitchRange);
		pickupTwig.Play();
	}

	public void playSmallChirp() {
		smallChirp.pitch = Random.Range(1 - ((1- lowPitchRange)*smallChirpRangeMult), 1 + ((highPitchRange - 1)*smallChirpRangeMult));
		smallChirp.Play();
	}

	public void playSingle(AudioClip clip) {
		flap.PlayOneShot(clip);
	}

}
