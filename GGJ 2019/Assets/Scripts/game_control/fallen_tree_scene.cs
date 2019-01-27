using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallen_tree_scene : MonoBehaviour {

	private void Start() {
		SoundManager.instance.playCutDown();
		StartCoroutine(end_game_delayed());
	}

	IEnumerator end_game_delayed() {
		yield return new WaitForSeconds(25f);
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif
#if UNITY_STANDALONE
		Application.Quit();
#endif
	}
}
