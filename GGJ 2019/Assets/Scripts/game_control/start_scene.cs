using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class start_scene : MonoBehaviour {

	public int scene_index;

	void Start() {
		SceneManager.LoadScene(scene_index);
	}
}
