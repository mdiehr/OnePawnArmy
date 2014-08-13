// DragDrop.cs
// Allows dragging and dropping an item on the grid

using UnityEngine;
using System.Collections.Generic;

public class DragDrop : MonoBehaviour {
	
	public float _gridOffset = 0.5f;
	public float _gridScale = 1.0f;
	public float _snapTime = 0.2f;
	public float _holdScale = 1.25f;

	private Vector3 _mousePoint;
	private Vector3 _touchPoint;
	private Vector3 _delta;

	GoTweenConfig _tweenUp;
	GoTweenConfig _tweenDown;

	List<GoTween> _currentTweens;

	void Awake() {
		_currentTweens = new List<GoTween>();
		_tweenUp = new GoTweenConfig().setEaseType( GoEaseType.CubicOut ).scale(this.transform.localScale * _holdScale);
		_tweenDown = new GoTweenConfig().setEaseType( GoEaseType.CubicOut ).scale(this.transform.localScale);
	}

	// Grab the mouse position
	void OnMouseDown() {
		_mousePoint = CalcMousePoint();
		// Save the touch point
		_touchPoint = _mousePoint;
		// Set Z position
		transform.Translate(Vector3.back);
		// Complete old tweens
		FinishOldTweens();
		// Tween up
		_currentTweens.Add(Go.to(transform, _snapTime, _tweenUp));
	}

	// Translate the object based on how the mouse moved
	void OnMouseDrag() {
		_mousePoint = CalcMousePoint();
		// Move by the difference
		transform.Translate(_mousePoint - _touchPoint);
		// Save the touch point
		_touchPoint = _mousePoint;
	}

	// Drop the item into the grid
	void OnMouseUp() {
		// Set Z position
		transform.Translate(Vector3.forward);
		// Tween to the closest tile position
		GoTweenConfig tweenToGrid = new GoTweenConfig()
			.setEaseType( GoEaseType.ExpoOut )
			.position(GetClosestSnapPoint(), false);
		// Complete old tweens
		FinishOldTweens();
		// Tween down
		_currentTweens.Add(Go.to(transform, _snapTime, tweenToGrid));
		_currentTweens.Add(Go.to(transform, _snapTime, _tweenDown));
	}

	private void FinishOldTweens() {
		if (_currentTweens != null) {
			foreach (GoTween tween in _currentTweens) {
				tween.complete();
			}
			_currentTweens.Clear();
		}
	}

	// Returns the mouse position in world coordinates
	private Vector3 CalcMousePoint() {
		return Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}

	// Returns a vector that is snapped to the nearest point on the grid
	private Vector3 GetClosestSnapPoint() {
		Vector3 position = transform.localPosition;
		float x = position.x;
		float y = position.y;
		x = Mathf.Floor(x/_gridScale) + _gridOffset;
		y = Mathf.Floor(y/_gridScale) + _gridOffset;
		return new Vector3(x, y, 0f);
	}
}
