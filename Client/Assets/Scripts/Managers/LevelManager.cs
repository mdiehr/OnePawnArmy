using UnityEngine;
using System.Collections;

using MiniJSON;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour {

	public static LevelManager Instance;

	private List<LevelDef> levels;

	void Awake() {
		// Remmeber the static instance
		if (Instance == null && (this.GetType() == typeof(LevelManager)))
			Instance = this;

		levels = new List<LevelDef>();

		Instance.Load();
	}

	// Loads LevelDefs from a JSON file
	private void Load() {
		TextAsset levelFile = Resources.Load<TextAsset>(GameConstants.LEVELS_JSON_PATH);
		if (levelFile != null) {
			Dictionary<string, object> levelData = MiniJSON.Json.Deserialize(levelFile.text) as Dictionary<string, object>;
			List<object> levelList = levelData[GameConstants.LEVELS_JSON_PAYLOAD_KEY] as List<object>;
			int index = 0;
			foreach(Dictionary<string, object> obj in levelList) {
				LevelDef levelDef = LevelDef.FromDictionary(obj, index++);
				levels.Add(levelDef);
				Debug.Log(levelDef.board.ToString());
			}
		} else {
			Debug.LogError("Couldn't load resource: levels.json");
		}
	}
}
