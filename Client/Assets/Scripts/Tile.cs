using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	private bool _tileColor = true;
	public bool TileColor {
		get { return _tileColor; }
		set {
			_tileColor = value;
			_sprite.color = _tileColor ? Color.white : Color.black;
		}
	}

	private UISprite _sprite;
	void Awake() {
		_sprite = this.GetComponent<UISprite>();
	}
}
