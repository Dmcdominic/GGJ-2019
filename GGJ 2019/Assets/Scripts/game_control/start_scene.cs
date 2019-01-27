using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class start_scene : MonoBehaviour {

	public int scene_index;
	public bool on_any_button_press = false;
	public bool black_fade_transition = false;

	void Start() {
		if (!on_any_button_press) {
			load_scene();
		}
	}

	private void Update() {
		if (on_any_button_press && Input.anyKeyDown && !(Input.GetAxisRaw("Cancel") > 0)) {
			load_scene();
		}
	}

	private void load_scene() {
		if (black_fade_transition) {
			black_fade.black_fade_to_scene(scene_index);
		} else {
			SceneManager.LoadScene(scene_index);
		}
	}
}
