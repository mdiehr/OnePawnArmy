using UnityEngine;
using System.Collections;

public class ButtonPanel : MonoBehaviour {

	public UIButton prevButton;
	public UIButton nextButton;

	void Start () {
		nextButton.onClick.Add(new EventDelegate(NextLevel));
		prevButton.onClick.Add(new EventDelegate(PrevLevel));
	}

	void NextLevel() {
		LevelManager.Instance.NextLevel();
	}

	void PrevLevel() {
		LevelManager.Instance.PrevLevel();
	}

	void OnDestroy() {
		//nextButton.onClick -= LevelManager.Instance.NextLevel;
		//prevButton.onClick -= LevelManager.Instance.PrevLevel;
	}
}
