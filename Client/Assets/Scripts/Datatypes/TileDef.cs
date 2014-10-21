// TileDef
// Data type for the contents of one tile in the board

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

	public TileDef(TileOwner owner, TileType type) {
		this.owner = owner;
		this.type = type;
	}

	public TileOwner owner;
	public TileType type;

	// Construct by tileId
	public static TileDef FromId(char tileId) {
		switch(tileId) {
			// Black
			case 'p':	return new TileDef(TileDef.TileOwner.black, TileDef.TileType.pawn);
			case 'n':	return new TileDef(TileDef.TileOwner.black, TileDef.TileType.knight);
			case 'b':	return new TileDef(TileDef.TileOwner.black, TileDef.TileType.bishop);
			case 'r':	return new TileDef(TileDef.TileOwner.black, TileDef.TileType.rook);
			case 'q':	return new TileDef(TileDef.TileOwner.black, TileDef.TileType.queen);
			case 'k':	return new TileDef(TileDef.TileOwner.black, TileDef.TileType.king);
			// White
			case 'P':	return new TileDef(TileDef.TileOwner.white, TileDef.TileType.pawn);
			case 'N':	return new TileDef(TileDef.TileOwner.white, TileDef.TileType.knight);
			case 'B':	return new TileDef(TileDef.TileOwner.white, TileDef.TileType.bishop);
			case 'R':	return new TileDef(TileDef.TileOwner.white, TileDef.TileType.rook);
			case 'Q':	return new TileDef(TileDef.TileOwner.white, TileDef.TileType.queen);
			case 'K':	return new TileDef(TileDef.TileOwner.white, TileDef.TileType.king);
			// Blanks
			case '.':	return new TileDef(TileDef.TileOwner.nobody, TileDef.TileType.empty);
			default:	return new TileDef(TileDef.TileOwner.nobody, TileDef.TileType.empty);
		}
	}

	public override string ToString() {
		string letter = ".";
		switch(type) {
		case TileType.pawn:		letter = "p"; break;
		case TileType.knight:	letter = "n"; break;
		case TileType.bishop:	letter = "b"; break;
		case TileType.rook:		letter = "r"; break;
		case TileType.queen:	letter = "q"; break;
		case TileType.king:		letter = "k"; break;
		}
		if (owner == TileOwner.white)
			letter = letter.ToUpper();
		return letter;
	}
}
