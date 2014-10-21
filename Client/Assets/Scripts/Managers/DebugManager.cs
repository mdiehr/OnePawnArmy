using UnityEngine;
using System.Collections;

public class DebugManager : MonoBehaviour {

	public static DebugManager Instance;
	void Awake() {
		// Remember the static instance
		if (Instance == null && (this.GetType() == typeof(DebugManager)))
			Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("space")) {
			
		}

		if (Input.GetKeyDown("q")) {
			
		}
		
//		if( gameState == GameState.Black ) {
//			return;	// Don't allow key presses during the opponent's turn
//		} else if( String.fromCharCode(key) == "M" || key == 27 ) {
//			// Escape to menu
//			GoToLevel(0);
//		} else if( String.fromCharCode(key) == "R" ) {
//			// Reset
//			ResetLevel();
//		} else if( String.fromCharCode(key) == "U" ) {
//			UndoLastMove();
//		} else if( String.fromCharCode(key) == "\\" || key == 39 ) {
//			DumpLevelFormat(LEVELS[GAME_level]);
//		} else if( String.fromCharCode(key) == "[" ) {
//			// Cheat: Previous Level
//			PrevLevel();
//		} else if( String.fromCharCode(key) == "]" ) {
//			// Cheat: Next Level
//			NextLevel();
//		} else if( gameState == GameState.Won ) {
//			// Progress to next level
//			NextLevel();
//		}
	}
}
