using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	public TileDef tileDef;

	private bool _tileColor = true;
	public bool TileColor {
		get { return _tileColor; }
		set {
			_tileColor = value;
			_sprite.color = _tileColor ? Color.gray : Color.black;
		}
	}

	private UISprite _sprite;
	void Awake() {
		_sprite = this.GetComponent<UISprite>();
	}

	public void SetTileDef(TileDef def) {
		tileDef = def;
		//def.owner == TileDef.TileOwner.black;
	}
}
