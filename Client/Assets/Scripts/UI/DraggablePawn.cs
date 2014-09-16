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
	private int _widgetDepthFront = 255;
	private int _widgetDepthOld;

	private Vector3 _prevPosition;

	void Awake() {
		_tweenUp = new TweenParms().Prop("localScale", transform.localScale * _holdScale).Ease(EaseType.EaseOutCubic);
		_tweenDown = new TweenParms().Prop("localScale", transform.localScale).Ease(EaseType.EaseOutCubic).OnComplete(this.OnTweenedDown);
		_widget = this.GetComponent<UIWidget>();
	}

	protected override void OnDragDropStart() {
		base.OnDragDropStart();

		// Save old position
		_prevPosition = this.transform.localPosition;

		// Move to front
		_widgetDepthOld = _widget.depth;
		_widget.depth = _widgetDepthFront;
		_widget.SetDirty();

		// Tween up
		HOTween.To(transform, _snapTime, _tweenUp);
	}

	protected override void OnDragDropMove (Vector2 delta) {
		base.OnDragDropMove(delta);
	}

	protected override void OnDragDropEnd () {
		base.OnDragDropEnd();
		Debug.Log("[DraggablePawn] OnDragDropEnd\n");
	}

	protected override void OnDragDropRelease (GameObject surface) {
		base.OnDragDropRelease(surface);

		if (surface == null) {
			Debug.LogError ("Pawn dropped onto a null surface. Was expecting to land on something.");
			return;
		}

		Debug.Log("[DraggablePawn] OnDragDropRelease " + surface + "\n");

		if (surface != null) {
			Board board = surface.GetComponent<Board>();
			if (board != null) {
				SnapTo(GetClosestSnapPoint(board));
			} else {
				RevertPosition();
			}
		} else {
			RevertPosition();
		}
	}

	private void SnapTo(Vector3 location) {
		// Tween to the closest tile position
		TweenParms tweenToGrid = new TweenParms()
			.Prop( "localPosition", location )
				.Ease(EaseType.EaseInOutExpo);
		
		// Tween down
		HOTween.To(transform, _snapTime, tweenToGrid);
		HOTween.To(transform, _snapTime, _tweenDown);
	}

	private void RevertPosition() {
		SnapTo(_prevPosition);
	}

	// Returns a vector that is snapped to the nearest point on the grid
	private Vector3 GetClosestSnapPoint(Board board) {
		Vector3 position = transform.localPosition;
		float x = position.x - _gridOffset;
		float y = position.y - _gridOffset;
		x = Mathf.Round(x/board.cellSize) * board.cellSize + _gridOffset;
		y = Mathf.Round(y/board.cellSize) * board.cellSize + _gridOffset;
		return new Vector3(x, y, 0f);
	}

	private void OnTweenedDown() {
		// Move the widget down
		_widget.depth = _widgetDepthOld;
	}
}
