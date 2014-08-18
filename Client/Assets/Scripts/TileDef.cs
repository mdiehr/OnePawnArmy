public class TileDef {

	public enum TileOwner {
		nobody = 0,
		white,
		black
	}

	public enum TileType {
		empty = 0,
		pawn,
		knight,
		bishop,
		rook,
		queen,
		king
	}

	public TileOwner owner;
	public TileType type;
}
