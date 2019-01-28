using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class black_fade : MonoBehaviour {

	public Image black_panel;

	public static black_fade instance = null;
	public static bool currently_fading_to_black = false;
	public static bool currently_fading_from_black = false;
	public static float rate;


	// Static instance setup and initialization
	void Awake() {
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);

		//SceneManager.activeSceneChanged += onSceneChanged;
	}

	private void Update() {
		if (currently_fading_to_black) {
			black_panel.color = new Color(0, 0, 0, black_panel.color.a + Time.deltaTime * rate);
			if (black_panel.color.a >= 1f) {
				currently_fading_to_black = false;
			}
		}
		if (currently_fading_from_black) {
			black_panel.color = new Color(0, 0, 0, black_panel.color.a - Time.deltaTime * rate);
			if (black_panel.color.a <= 0f) {
				currently_fading_from_black = false;
			}
		}
	}

	public static void fade_to_black(float duration) {
		currently_fading_to_black = true;
		currently_fading_from_black = false;
		rate = 1f / duration;
		movement.bird_instance.set_movement_enabled(false);
	}

	public static void fade_from_black(float duration) {
		currently_fading_to_black = false;
		currently_fading_from_black = true;
		rate = 1f / duration;
		movement.bird_instance.set_movement_enabled(true);
	}

	public static void black_fade_to_scene(int build_index) {
		instance.StartCoroutine(instance.black_fade_to_scene_coroutine(build_index, 1f));
	}

	IEnumerator black_fade_to_scene_coroutine(int build_index, float duration) {
		fade_to_black(duration);
		yield return new WaitForSeconds(duration);
		SceneManager.LoadScene(build_index);
		yield return new WaitUntil(() => SceneManager.GetActiveScene().buildIndex == build_index);
		if (build_index == 5) {
			yield return new WaitForSeconds(8f);
			fade_from_black(3f);
		} else {
			fade_from_black(duration);
		}
	}
}
