using UnityEngine;
using System.Collections;

public static class GameConstants {

	public const int BOARD_W = 8;
	public const int BOARD_H = 8;

	public const string LEVELS_JSON_PATH = "JSON/levels";
	public const string LEVELS_JSON_PAYLOAD_KEY = "levels";

	public static readonly Color32 colorNeutral = new Color32(255, 225, 159, 255);
	public static readonly Color32 colorWhite   = new Color32(255, 255, 255, 255);
	public static readonly Color32 colorBlack   = new Color32(  0,   0,   0, 255);
	public static readonly Color32 colorLight   = new Color32(255, 255, 255,  25);
	public static readonly Color32 colorDark    = new Color32(  0,   0,   0,  25);
}
