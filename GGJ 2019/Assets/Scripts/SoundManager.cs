using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour {
	public AudioSource flap;
	public AudioSource chirp;
	public AudioSource eat;
	public AudioSource pickupTwig;
	public AudioSource smallChirp;
	public AudioSource gliding;
	public AudioSource updraft;
	public AudioSource titleTrack;
	public AudioSource lvlTrack;

	public bool_var gliding_var;
	public bool_var updraft_var;
	public static SoundManager instance = null;
	public float lowPitchRange;
	public float highPitchRange;
	public float smallChirpRangeMult;


	// Static instance setup and initialization
	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);
			return;
		}
		
		DontDestroyOnLoad(gameObject);

		StartCoroutine("repeat_chrip");

		SceneManager.activeSceneChanged += onSceneChanged;
	}

	private void onSceneChanged(Scene current, Scene next) {
		if (next.buildIndex < 2 && !titleTrack.isPlaying) {
			titleTrack.Play();
			lvlTrack.Stop();
		} else if (next.buildIndex >= 2 && !lvlTrack.isPlaying) {
			titleTrack.Stop();
			lvlTrack.Play();
		}
	}

	// Turn the gliding and updraft sounds on/off appropriately
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

		if (!updraft.isPlaying) {
			updraft.Play();
			updraft.volume = 0f;
		}

		if (updraft_var.val && gliding.volume < 1f) {
			updraft.volume += Time.deltaTime * 4;
		} else if (!updraft_var.val && gliding.volume > 0f) {
			updraft.volume -= Time.deltaTime * 3;
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
