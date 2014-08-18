
public class BoardDef {

	TileDef[,] board;

	public BoardDef(int w, int h) {
		board =  new TileDef[w, h];
	}

	public TileDef Tile(int x, int y) {
		return board[x, y];
	}

}
