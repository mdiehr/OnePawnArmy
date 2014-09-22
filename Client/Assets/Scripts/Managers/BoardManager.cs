using UnityEngine;
using System.Collections;
using System;

public class BoardManager : MonoBehaviour {

	public static Action<BoardDef> OnBoardLoaded;
	private void RaiseOnBoardLoaded(BoardDef board) { if (OnBoardLoaded != null) OnBoardLoaded(board); }

	private Board _board;

	public static BoardManager Instance;
	void Awake() {
		// Remember the static instance
		if (Instance == null && (this.GetType() == typeof(BoardManager)))
			Instance = this;

		LevelManager.OnLevelLoaded += OnLevelLoaded;
	}

	void OnLevelLoaded(LevelDef level) {
		// Raise an event for the board inside this level. This is a useful indirection because the undo system will use boards.
		RaiseOnBoardLoaded(level.board);
	}

	public void RegisterBoard(Board board) {
		_board = board;
	}

	public bool CanDropHere(Vector3 position) {
		Vector2 coords = GetBoardCoordinates(position);
		if (coords.x < 0 || coords.y < 0 || coords.x >= _board.gridWidth || coords.y >= _board.gridHeight) {
			// Out of bounds
			return false;
		}
		return true;
	}
	
	public Vector2 GetBoardCoordinates(Vector3 position) {
		// Get info from board
		float cellSize = _board.cellSize;
		float cellOffset = _board.cellOffset;
		// Calculate game logic coordinates
		float x = position.x - cellOffset;
		float y = position.y - cellOffset;
		x = Mathf.Round(x/cellSize) + _board.gridWidth/2;
		y = Mathf.Round(y/cellSize) + _board.gridHeight;
		return new Vector2(x, y);
	}

	public Vector3 GetSnapPoint(Vector3 position) {
		// Get info from board
		float cellSize = _board.cellSize;
		float cellOffset = _board.cellOffset;
		// Calculate game logic coordinates
		float x = position.x - cellOffset;
		float y = position.y - cellOffset;
		x = Mathf.Round(x/cellSize) * cellSize + cellOffset;
		y = Mathf.Round(y/cellSize) * cellSize + cellOffset;
		return new Vector3(x, y, 0f);
	}

	public void PieceDropped(DraggablePawn pawn) {
		Vector2 coords = GetBoardCoordinates(pawn.transform.localPosition);
		Debug.Log("<color=green>[BoardManager]</color> Piece dropped: " + pawn.gameObject.name + " at " + coords.x + ", " + coords.y + "\n");
	}
}
