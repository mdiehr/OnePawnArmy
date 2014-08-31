using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour {

	public int gridWidth = 8;
	public int gridHeight = 8;
	public int cellSize = 128;
	public Vector2 startPosition;

	public GameObject TilePrefab;

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
						tile.TileColor = (((x + y) % 2) == 0);
					}
				}
			}
		}
	}

	void OnDrop(GameObject go) {
		Debug.Log (go.name);
	}
}
