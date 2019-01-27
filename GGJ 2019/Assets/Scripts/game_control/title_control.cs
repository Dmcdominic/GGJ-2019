using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class title_control : MonoBehaviour {


	public int scene_index;

	void Start() {
		
	}

	private void Update() {
		if (Input.anyKeyDown && !(Input.GetAxisRaw("Cancel") > 0)) {
			start_anim();
		}
	}

	private void start_anim() {
		GetComponent<Animator>().SetTrigger("egg cracks");
		StartCoroutine(load_scene());
	}

	IEnumerator load_scene() {
		yield return new WaitForSeconds(8f);
		black_fade.black_fade_to_scene(scene_index);
	}
}
