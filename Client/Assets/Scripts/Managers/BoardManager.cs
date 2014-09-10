using UnityEngine;
using System.Collections;
using System;

public class BoardManager : MonoBehaviour {

	public static Action<BoardDef> OnBoardLoaded;
	private void RaiseOnBoardLoaded(BoardDef board) { if (OnBoardLoaded != null) OnBoardLoaded(board); }

	public static BoardManager Instance;
	void Awake() {
		// Remember the static instance
		if (Instance == null && (this.GetType() == typeof(BoardManager)))
			Instance = this;

		LevelManager.OnLevelLoaded += OnLevelLoaded;
	}

	void OnLevelLoaded(LevelDef level) {
		RaiseOnBoardLoaded(level.board);
	}
}
