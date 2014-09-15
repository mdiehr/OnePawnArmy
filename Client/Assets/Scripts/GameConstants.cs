using UnityEngine;
using System.Collections;

public static class GameConstants {

	public const int BOARD_W = 8;
	public const int BOARD_H = 8;

	public const string LEVELS_JSON_PATH = "JSON/levels";
	public const string LEVELS_JSON_PAYLOAD_KEY = "levels";

	public static readonly Color32 colorBeige = new Color32(255, 225, 159, 255);
	public static readonly Color32 colorBlue  = new Color32(48, 129, 223, 255);
	public static readonly Color32 colorRed   = new Color32(165, 40, 52, 255);
}
