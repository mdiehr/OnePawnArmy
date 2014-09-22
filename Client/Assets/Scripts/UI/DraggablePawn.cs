using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class DraggablePawn : UIDragDropItem {

	public float _snapTime = 0.2f;
	public float _holdScale = 1.25f;

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

	protected override void OnDragDropRelease (GameObject surface) {
		base.OnDragDropRelease(surface);

		if (BoardManager.Instance.CanDropHere(transform.localPosition)) {
			Vector3 snapPoint = BoardManager.Instance.GetSnapPoint(transform.localPosition);
			SnapTo(snapPoint);
			BoardManager.Instance.PieceDropped(this);
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

	private void OnTweenedDown() {
		// Move the widget down
		_widget.depth = _widgetDepthOld;
	}
}
