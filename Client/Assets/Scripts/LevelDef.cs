using System.Collections.Generic;

public class LevelDef {
	public int index;
	public string title;
	public string fen;
	public BoardDef board;

	public static LevelDef FromDictionary(Dictionary<string, object> dictionary, int index = 0) {
		LevelDef levelDef = new LevelDef();
		levelDef.index = index;
		levelDef.title = dictionary["title"] as string;
		levelDef.fen = dictionary["fen"] as string;
		levelDef.board = new BoardDef(levelDef.fen);
		return levelDef;
	}

	public override string ToString() {
		return "LevelDef {" + index + ", " + title + ", " + fen + "}";
	}
}
