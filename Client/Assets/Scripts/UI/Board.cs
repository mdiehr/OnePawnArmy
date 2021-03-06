﻿// Board
// Sets up a BoardDef to display on the screen

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour {

	public int gridWidth = GameConstants.BOARD_W;
	public int gridHeight = GameConstants.BOARD_H;
	public int cellSize = 128;
	public int cellOffset = 64;
	public Vector2 startPosition;

	public GameObject TilePrefab;
	public GameObject MovableTilePrefab;

	private int tileDepth = 1;
	private const int movableTileDepthStart = 100;
	private int movableTileDepth = movableTileDepthStart;

	private List<Tile> _bgTiles;
	private List<Tile> _movableTiles;

	private Vector3 position = Vector3.zero;

	void Awake() {
		_bgTiles = new List<Tile>();
		_movableTiles = new List<Tile>();

		BoardManager.OnBoardLoaded += OnBoardLoaded;
		BoardManager.Instance.RegisterBoard(this);
	}

	void Start() {
		for (int x = 0; x < gridWidth; ++x) {
			for (int y = 0; y < gridHeight; ++y) {
				CreateEmptyTile(x, y);
			}
		}
	}

	private void CreateEmptyTile(int x, int y) {
		position.x = x * cellSize + startPosition.x;
		position.y = y * cellSize + startPosition.y;

		// Instantiate and position the tile object
		GameObject go = NGUITools.AddChild(this.gameObject, TilePrefab);
		if (go != null) {
			go.transform.parent = this.gameObject.transform;
			go.transform.localPosition = position;
			go.name = "Square:"+x+","+y;

			// Set widget depth
			UIWidget widget = go.GetComponent<UIWidget>();
			if (widget != null) {
				widget.depth = tileDepth++;
			}

			// Set tile color
			Tile tile = go.GetComponent<Tile>();
			if (tile != null) {
				_bgTiles.Add(tile);
				tile.TileColor = (((x + y) % 2) == 1);
			}
		}
	}

	private void OnBoardLoaded(BoardDef boardDef) {
		ShowLevel(boardDef);
	}
	
	public void ShowLevel(BoardDef boardDef) {
		ClearMovableTiles();
		for (int x = 0; x < boardDef.w; ++x) {
			for (int y = 0; y < boardDef.h; ++y) {
				CreateMovableTile(x, y, boardDef.GetTile(x,y));
			}
		}
	}

	private void CreateMovableTile(int x, int y, TileDef tileDef) {
		if (tileDef.type == TileDef.TileType.empty)
			return;

		position.x = x * cellSize + startPosition.x;
		position.y = y * cellSize + startPosition.y;

		GameObject go = NGUITools.AddChild(this.gameObject, MovableTilePrefab);
		if (go != null) {
			go.transform.parent = this.gameObject.transform;
			go.transform.localPosition = position;
			go.name = "Piece " + tileDef.owner.ToString() + " " + tileDef.type.ToString() + " [" + x + "," + y + "]";
			
			UIWidget widget = go.GetComponent<UIWidget>();
			if (widget != null) {
				widget.depth = movableTileDepth++;
			}
			
			Tile tile = go.GetComponent<Tile>();
			if (tile != null) {
				tile.SetTileDef(tileDef);
				_movableTiles.Add(tile);
			}
		}
	}
	
	// Delete the movable tiles
	private void ClearMovableTiles() {
		foreach(Tile tile in _movableTiles) {
			Destroy(tile.gameObject);
		}
		_movableTiles.Clear();
		movableTileDepth = movableTileDepthStart;
	}
}
