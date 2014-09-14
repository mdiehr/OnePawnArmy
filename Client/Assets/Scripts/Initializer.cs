using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class Initializer : MonoBehaviour {

	void Awake() {
		// Initialize modules & libraries
		HOTween.Init();
		// Load the levels
		this.gameObject.AddComponent<LevelManager>();
		this.gameObject.AddComponent<BoardManager>();
		this.gameObject.AddComponent<UIManager>();
	}
}
