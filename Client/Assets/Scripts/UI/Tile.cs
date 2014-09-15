using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {

	public TileDef tileDef;

	private bool _tileColor = true;
	public bool TileColor {
		get {
			return _tileColor;
		}
		set {
			_tileColor = value;
			_sprite.color = _tileColor ? Color.gray : Color.black;
		}
	}

	private UISprite _sprite;
	void Awake() {
		_sprite = this.GetComponent<UISprite>();
	}

	public void SetTileDef(TileDef tileDef) {
		this.tileDef = tileDef;

		// Set sprite
		string baseName = tileDef.type.ToString().ToLower();
		//if (tileDef.owner == TileDef.TileOwner.black)
		//	baseName += "-outline";
		_sprite.spriteName = baseName;

		// Set tile color
		UIWidget widget = GetComponent<UIWidget>();
		if (widget != null) {
			if (tileDef.owner == TileDef.TileOwner.white) {
				widget.color = GameConstants.colorBlue;
			} else if (tileDef.owner == TileDef.TileOwner.black) {
				widget.color = GameConstants.colorRed;
			} else {
				widget.color = GameConstants.colorBeige;
			}
		}
	}
}
