using System;
using System.Collections;
using System.Collections.Generic;
using MiniJSON;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	private int currentLevelIndex = 0;
	private List<LevelDef> levels;
	public List<LevelDef> Levels { get; private set; }

	// Events
	public static Action<LevelDef> OnLevelLoaded;
	private void RaiseOnLevelLoaded(LevelDef level) { if (OnLevelLoaded != null) OnLevelLoaded(level); }

	public static LevelManager Instance;
	void Awake() {
		// Remember the static instance
		if (Instance == null && (this.GetType() == typeof(LevelManager)))
			Instance = this;

		levels = new List<LevelDef>();
	}

	void Start() {
		Load();
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
				//Debug.Log(levelDef.board.ToString());
			}
			// Notify the rest of the game
			LoadLevelByIndex(currentLevelIndex);
		} else {
			Debug.LogError("[LevelManager] Couldn't load resource: levels.json");
		}
	}

	public void LoadLevelByIndex(int index) {
		RaiseOnLevelLoaded(GetLevel(index));
	}

	public void NextLevel() {
		currentLevelIndex = (currentLevelIndex + 1) % levels.Count;
		Debug.Log("[LevelManager] NextLevel " + currentLevelIndex + "\n");
		LoadLevelByIndex(currentLevelIndex);
	}

	public void PrevLevel() {
		currentLevelIndex = (levels.Count + currentLevelIndex - 1) % levels.Count;
		Debug.Log("[LevelManager] PrevLevel " + currentLevelIndex + "\n");
		LoadLevelByIndex(currentLevelIndex);
	}

	public LevelDef GetLevel(int index) {
		return levels[index];
	}
}
