// DragDrop.cs
// Allows dragging and dropping an item on the grid

using UnityEngine;
using System.Collections.Generic;
using Holoville.HOTween;

public class DragDrop : MonoBehaviour {
	
	public float _gridOffset = 0.0f;
	public float _gridScale = 128.0f;
	public float _snapTime = 0.2f;
	public float _holdScale = 1.25f;

	private Vector3 _mousePoint;
	private Vector3 _touchPoint;
	private Vector3 _delta;

	TweenParms _tweenUp;
	TweenParms _tweenDown;

	void Awake() {
		//_currentTweens = new List<GoTween>();
		_tweenUp = new TweenParms().Prop("localScale", transform.localScale * _holdScale).Ease(EaseType.EaseOutCubic);
		_tweenDown = new TweenParms().Prop("localScale", transform.localScale).Ease(EaseType.EaseOutCubic);
	}

	// Grab the mouse position
	void OnMouseDown() {
		_mousePoint = CalcMousePoint();
		// Save the touch point
		_touchPoint = _mousePoint;
		// Set Z position
		transform.Translate(Vector3.back);
		// Tween up
		HOTween.To(transform, _snapTime, _tweenUp);
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
		TweenParms tweenToGrid = new TweenParms()
			.Prop( "localPosition", GetClosestSnapPoint() )
			.Ease(EaseType.EaseInOutExpo);
		// Tween down
		HOTween.To(transform, _snapTime, tweenToGrid);
		HOTween.To(transform, _snapTime, _tweenDown);
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
