using UnityEngine;
using System.Collections;

public class Board : MonoBehaviour {

	public int gridWidth = 8;
	public int gridHeight = 8;
	public int cellSize = 128;
	public int startX = -448;
	public int startY = -448;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnDrop(GameObject go) {
		Debug.Log (go.name);
	}
}
