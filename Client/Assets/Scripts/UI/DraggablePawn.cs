using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class DraggablePawn : UIDragDropItem {

	public float _snapTime = 0.2f;
	public float _holdScale = 1.25f;

	// TODO: Move these to the board script
	private float _gridOffset = 64.0f;
	private float _gridScale = 128.0f;

	TweenParms _tweenUp;
	TweenParms _tweenDown;

	UIWidget _widget;
	private int _widgetDepthFront = 100;
	private int _widgetDepthOld;

	void Awake() {
		_tweenUp = new TweenParms().Prop("localScale", transform.localScale * _holdScale).Ease(EaseType.EaseOutCubic);
		_tweenDown = new TweenParms().Prop("localScale", transform.localScale).Ease(EaseType.EaseOutCubic).OnComplete(this.OnTweenedDown);
		_widget = this.GetComponent<UIWidget>();
	}

	protected override void OnDragDropStart() {
		base.OnDragDropStart();

		// Move to front
		_widgetDepthOld = _widget.depth;
		_widget.depth = _widgetDepthFront;

		// Tween up
		HOTween.To(transform, _snapTime, _tweenUp);
	}

	protected override void OnDragDropMove (Vector2 delta) {
		base.OnDragDropMove(delta);
	}
	
	protected override void OnDragDropEnd () {
		base.OnDragDropEnd();
		
		// Tween to the closest tile position
		TweenParms tweenToGrid = new TweenParms()
			.Prop( "localPosition", GetClosestSnapPoint() )
				.Ease(EaseType.EaseInOutExpo);

		// Tween down
		HOTween.To(transform, _snapTime, tweenToGrid);
		HOTween.To(transform, _snapTime, _tweenDown);
	}

	// Returns a vector that is snapped to the nearest point on the grid
	private Vector3 GetClosestSnapPoint() {
		Vector3 position = transform.localPosition;
		float x = position.x - _gridOffset;
		float y = position.y - _gridOffset;
		x = Mathf.Round(x/_gridScale) * _gridScale + _gridOffset;
		y = Mathf.Round(y/_gridScale) * _gridScale + _gridOffset;
		return new Vector3(x, y, 0f);
	}

	private void OnTweenedDown() {
		// Move the widget down
		_widget.depth = _widgetDepthOld;
	}
}
