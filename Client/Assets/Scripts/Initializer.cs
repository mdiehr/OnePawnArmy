﻿using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class Initializer : MonoBehaviour {

	void Awake() {
		// Initialize modules & libraries
		HOTween.Init();
	}
}
