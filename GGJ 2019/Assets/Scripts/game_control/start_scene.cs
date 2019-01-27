using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class start_scene : MonoBehaviour {

	public int scene_index;
	public bool on_any_button_press = false;

	void Start() {
		if (!on_any_button_press) {
			SceneManager.LoadScene(scene_index);
		}
	}

	private void Update() {
		if (on_any_button_press && Input.anyKeyDown && !(Input.GetAxisRaw("Cancel") > 0)) {
			SceneManager.LoadScene(scene_index);
		}
	}
}
