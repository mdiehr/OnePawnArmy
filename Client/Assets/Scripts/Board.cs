using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour {

	public int gridWidth = 8;
	public int gridHeight = 8;
	public int cellSize = 128;
	public Vector2 startPosition;

	public GameObject TilePrefab;
	public GameObject MovableTilePrefab;

	private List<Tile> _bgTiles;
	private List<Tile> _movableTiles;

	void Awake() {
		_bgTiles = new List<Tile>();
		_movableTiles = new List<Tile>();
	}

	void Start() {
		int tileDepth = 1;
		Vector3 position = Vector3.zero;
		for (int x = 0; x < gridWidth; ++x) {
			position.x = x * cellSize + startPosition.x;
			for (int y = 0; y < gridHeight; ++y) {
				position.y = y * cellSize + startPosition.y;
				// Instantiate and position the tile object
				GameObject go = NGUITools.AddChild(this.gameObject, TilePrefab);
				go.transform.parent = this.gameObject.transform;
				go.transform.localPosition = position;

				if (go != null) {
					// Set widget depth
					UIWidget widget = go.GetComponent<UIWidget>();
					if (widget != null) {
						widget.depth = tileDepth++;
					}

					// Set tile color
					Tile tile = go.GetComponent<Tile>();
					if (tile != null) {
						_bgTiles.Add(tile);
						tile.TileColor = (((x + y) % 2) == 0);
					}
				}
			}
		}
	}

	// TODO: Call this when the level manager is done loading
	void ShowLevel() {
		LevelDef level = LevelManager.Instance.GetLevel(0);

		for (int x = 0; x < level.board.w; ++x) {
			for (int y = 0; y < level.board.h; ++y) {
				TileDef tile = level.board.Tiles[x,y];
				// TODO: Create tiles
				//_movableTiles.Add(tile);
			}
		}
	}

	// Catch event when a piece is dropped on this board
	void OnDrop(GameObject go) {
		Debug.Log (go.name);
	}
}
