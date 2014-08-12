// DragDrop.cs
// Allows dragging and dropping an item on the grid

using UnityEngine;

public class DragDrop : MonoBehaviour {
	
	public float _gridOffset = 0.5f;
	public float _gridScale = 1.0f;
	public float _snapTime = 0.2f;

	private Vector3 _mousePoint;
	private Vector3 _touchPoint;
	private Vector3 _delta;

	void OnMouseDown() {
		_mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		_touchPoint = _mousePoint;
	}

	void OnMouseDrag() {
		_mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		_delta = _mousePoint - _touchPoint;
		transform.Translate(_delta);
		// Save the new touch point
		_touchPoint = _mousePoint;
	}

	void OnMouseUp() {
		// Tween to the position
		var config = new GoTweenConfig().setEaseType( GoEaseType.ExpoOut ).position( GetClosestSnapPoint(), false );
		Go.to(transform, _snapTime, config);
	}

	private Vector3 GetClosestSnapPoint() {
		Vector3 position = transform.localPosition;
		float x = position.x;
		float y = position.y;
		x = Mathf.Floor(x/_gridScale) + _gridOffset;
		y = Mathf.Floor(y/_gridScale) + _gridOffset;
		return new Vector3(x, y, 0f);
	}
}
