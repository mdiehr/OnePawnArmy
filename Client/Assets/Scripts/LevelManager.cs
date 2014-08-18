using UnityEngine;
using System.Collections;

using MiniJSON;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

	public static LevelManager Instance;

	private List<LevelDef> levels;

	void Awake() {
		if (Instance == null)
			Instance = this;
		levels = new List<LevelDef>();
		Instance.Load();
	}

	private void Load() {
		TextAsset levelFile = Resources.Load<TextAsset>("JSON/levels");
		if (levelFile != null) {
			Dictionary<string, object> levelData = MiniJSON.Json.Deserialize(levelFile.text) as Dictionary<string, object>;
			List<object> levelList = levelData["levels"] as List<object>;
			int index = 0;
			foreach(Dictionary<string, object> obj in levelList) {
				LevelDef levelDef = LevelDef.FromDictionary(obj, index++);
				levels.Add(levelDef);
				//Debug.Log (levelDef);
			}
		} else {
			Debug.LogError("Couldn't load resource: levels.json");
		}
	}
}
