﻿
public class BoardDef {

	private TileDef[,] tiles;
	public TileDef[,] Tiles { get { return tiles; } }

	private int w;
	private int h;

	public BoardDef(int w, int h) {
		this.tiles = new TileDef[w, h];
	}

	public BoardDef(string fen) {
		// Replace shorthand with empty spaces
		fen = fen.Replace("8", "........");
		fen = fen.Replace("7", ".......");
		fen = fen.Replace("6", "......");
		fen = fen.Replace("5", ".....");
		fen = fen.Replace("4", "....");
		fen = fen.Replace("3", "...");
		fen = fen.Replace("2", "..");
		fen = fen.Replace("1", ".");
		string[] fenLines = fen.Split('/');
		
		w = fenLines.Length;
		h = fenLines[0].Length;
		this.tiles = new TileDef[w, h];
		
		for (int x = 0; x < w; ++x) {
			for (int y = 0; y < h; ++y) {
				this.tiles[x, y] = TileDef.FromId(fenLines[x][y]);
			}
		}
	}

	public override string ToString() {
		string output = "BoardDef:\n";
		for (int x = 0; x < w; ++x) {
			for (int y = 0; y < h; ++y) {
				output += this.tiles[x, y].ToString();
			}
			output += "\n";
		}
		return output;
	}
}
