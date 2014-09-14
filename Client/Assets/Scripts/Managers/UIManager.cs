using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

	void Update () {
#if UNITY_ANDROID
		if (Input.GetKeyDown(KeyCode.Escape)) {
			// Quit on back (Android only)
			Application.Quit(); 
		}
#endif
	}

}
