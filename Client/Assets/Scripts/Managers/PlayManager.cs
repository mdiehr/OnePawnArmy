using UnityEngine;
using System.Collections;

public class PlayManager : MonoBehaviour {

	public enum GameState {
		White,
		Black,
		Won,
		Lost
	};
	private GameState gameState = GameState.White;

	// Legacy game states
	
	// Input variables
	bool GAME_CLICK_LATCH = false;
	TileDef GAME_CLICK_DATA;
	int GAME_CLICK_X = 0;
	int GAME_CLICK_Y = 0;
	int GAME_MOUSE_X = 0;
	int GAME_MOUSE_Y = 0;
	// Program state
	int GAME_ENEMY_MOVE_PROGRESS = 0;
	bool GAME_ENEMY_MOVE_PROGRESS_PROCESSED = false;
	bool GAME_ENEMY_THREATEN = false;
	int GAME_ENEMY_THREATEN_X = 0;
	int GAME_ENEMY_THREATEN_Y = 0;
	int GAME_ENEMY_SOLDIER_X = 0;
	int GAME_ENEMY_SOLDIER_Y = 0;
	// Misc
	int GAME_level = 0;

	public static PlayManager Instance;
	void Awake() {
		// Remember the static instance
		if (Instance == null && (this.GetType() == typeof(PlayManager)))
			Instance = this;
	}
	
	void Start() {
		StartCoroutine(Tick());
	}

	// Can we pick up a piece, or click on some UI?
	void Click(int x, int y, TileDef data) {
		if( gameState == GameState.Black ) {
			// The enemy is moving, not you
		} else if( GAME_CLICK_LATCH == false && data.owner == TileDef.TileOwner.white ) {
			// Clicked somewhere to start a move
			ProcessBetweenTurns();
			BoardManager.Instance.PutTileData(x, y, GetEmptyPiece());
			GAME_CLICK_LATCH = true;
			GAME_CLICK_DATA = data;
			GAME_CLICK_X = x;
			GAME_CLICK_Y = y;
			HighlightDrag(x, y, true);
		}
	}

	// Released a piece
	void Release(int x, int y, TileDef data) {
		if( GAME_CLICK_LATCH == true ) {
			GAME_CLICK_LATCH = false;
			bool allowMove = (data.type == TileDef.TileType.empty || data.owner == TileDef.TileOwner.black) && (BoardManager.Instance.GetIsHighlighted(x, y));
			
			ClearHighlights();
			
			if( allowMove ) {
				if( data.type != TileDef.TileType.empty ) {
					GAME_CLICK_DATA.type = data.type;	// Assume this piece's identity

					if( GAME_CLICK_DATA.type == TileDef.TileType.king ) {
						//TODO: Save manager
//						SAVE.CompleteLevel(GAME_level);
						gameState = GameState.Won;
						AudioPlay("GAME_SOUND_WIN");
						StatusText("Nice work. Press any key to continue.");
					}
				}
				
				BoardManager.Instance.PutTileData(GAME_CLICK_X, GAME_CLICK_Y, GetEmptyPiece());
				BoardManager.Instance.PutTileData(x, y, GAME_CLICK_DATA);
				HighlightDrag(x, y, false);
							
				// Time for the opponent to move
				if( gameState != GameState.Won ) {
					MakeEnemyMove();
				}
			} else {
				Unlatch(x, y, data);
			}
		}
	}

	// Mouse over to highlight moves
	void Enter(int x, int y, TileDef data) {
		GAME_MOUSE_X = x;
		GAME_MOUSE_Y = y;
		
		if( gameState == GameState.Black || gameState == GameState.Won ) {
			// The enemy is moving, not you
		} else if( GAME_CLICK_LATCH == true ) {
			DrawPieceAt(x, y, GAME_CLICK_DATA);
			DrawPieceHighlights(GAME_CLICK_X, GAME_CLICK_Y, GAME_CLICK_DATA);
			HighlightDrag(x, y, true);
		} else {
			DrawPieceHighlights(x, y, data);
		}
	}

	// Mouse out to unhighlight things
	void Leave(int x, int y, int data) {
		if( gameState == GameState.Black || gameState == GameState.Won ) {
			// The enemy is moving, not you
		} else if( GAME_CLICK_LATCH == true ) {
			HighlightDrag(x, y, false);
		} else {
			ClearHighlights();
		}
	}

	// Clock
	IEnumerator Tick() {
		while(true) {
			if( gameState == GameState.Black ) {
				ProcessEnemyMove();
			}
			yield return new WaitForSeconds(0.1f);
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////
	// Unsorted gameplay functions
	////////////////////////////////////////////////////////////////////////////////
	
	void WriteCurrentLevelStatus() {
		StatusText("Level " + (GAME_level));
	}
	
	// See what floor tile was at x,y and redraw it
	void RedrawTileAt(int x, int y) {
		var piece = BoardManager.Instance.GetTile(x, y);
		DrawPieceAt(x, y, piece);
	}
	
	void DrawPieceAt(int x, int y, TileDef piece) {
//		PS.BeadGlyph(x, y, piece.glyph);
//		PS.BeadGlyphColor(x, y, piece.color);
//		
//		if( ((x+y)%2) == 0 ) {
//			PS.BeadColor(x, y, GAME_COLOR_DARK);
//		} else {
//			PS.BeadColor(x, y, GAME_COLOR_LIGHT);
//		}
	}
	
	void HighlightPotentialMove(TileDef piece, int x, int y) {
		if( IsInBounds(x, y) ) {
			var targetPiece = BoardManager.Instance.GetTile(x, y);
			if (piece.owner == TileDef.TileOwner.black) {
				HighlightEnemyThreaten(x, y);
				if( targetPiece.owner ==  TileDef.TileOwner.white ) {
					GAME_ENEMY_THREATEN = true;
					GAME_ENEMY_THREATEN_X = x;
					GAME_ENEMY_THREATEN_Y = y;
				}
			} else if( targetPiece.owner != TileDef.TileOwner.white ) {
				if( targetPiece.owner == TileDef.TileOwner.black ) {
					HighlightCapture(x, y);
				} else {
					HighlightMove(x, y);
				}
			}
		}
	}
	
	void HighlightMove(int x, int y) {
		if( IsInBounds(x, y) ) {
//			PS.BeadBorderWidth(x, y, 3);
//			PS.BeadBorderColor(x, y, GAME_COLOR_HIGHLIGHT_MOVE);
		}
	}
	
	void HighlightEnemyThreaten(int x, int y) {
		if( IsInBounds(x, y) ) {
//			PS.BeadBorderWidth(x, y, 3);
//			PS.BeadBorderColor(x, y, GAME_COLOR_HIGHLIGHT_ENEMY_THREATEN);
		}
	}
	
	void HighlightCapture(int x, int y) {
		if( IsInBounds(x, y) ) {
//			PS.BeadBorderWidth(x, y, 3);
//			PS.BeadBorderColor(x, y, GAME_COLOR_HIGHLIGHT_CAPTURE);
		}
	}
	
	void HighlightDrag(int x, int y, bool on) {
//		if (on) {
//			if (PS.BeadBorderWidth (x, y) != 0) {
//				PS.BeadBorderWidth (x, y, 6);
//			}
//
//			if (BoardManager.Instance.GetTile (x, y).owner == TileDef.TileOwner.black) {
//				PS.BeadBorderColor (x, y, GAME_COLOR_HIGHLIGHT_CAPTURE);
//			} else {
//				PS.BeadBorderColor (x, y, GAME_COLOR_HIGHLIGHT_MOVE);
//			}
//		} else {
//			PS.BeadBorderWidth (x, y, 0);
//		}
	}
	
	
	void Unlatch(int x, int y, TileDef data) {
//		DrawPieceHighlights(x, y, data);
//		PutTileData(GAME_CLICK_X, GAME_CLICK_Y, GAME_CLICK_DATA);
//		GAME_CLICK_LATCH = false;
//		// Cancel the undo state for this move
		BoardManager.Instance.PopBoardStateForUndo();
//		GAME_MOVE_STACK.pop();
	}

	// Draws a highlight for a bishop, rook, or queen
	void DrawPieceHighlightTravel(TileDef piece, int x, int y, int dx, int dy) {
		for(int i = 0; i < 8; i++) {
			x += dx;
			y += dy;

			if( !IsInBounds(x, y) ) {
				break;
			}

			// Hit a friendly piece
			TileDef targetPiece = BoardManager.Instance.GetTile(x, y);
			if( piece.owner == TileDef.TileOwner.white && targetPiece.owner == TileDef.TileOwner.white ) {
				break;
			}
			
			HighlightPotentialMove(piece, x, y);

			// Hit any other piece
			if( targetPiece.type != TileDef.TileType.empty ) {
				break;
			}
		}
	}
	
	void DrawPieceHighlights(int x, int y, TileDef piece) {
		Debug.LogWarning("type: " + piece.type + "\n");
		bool me = (piece.owner == TileDef.TileOwner.white);
		TileDef.TileType type = piece.type;
		int dir = me ? 1 : -1;
		int pawnRow = me ? GameConstants.BOARD_H - 2 : 1;
	
		// Pawn
		if( type == TileDef.TileType.pawn ) {
			if( me ) {
				// Move forward
				if( IsClear(x, y-1*dir) ) {
					HighlightPotentialMove(piece, x, y-1*dir);
					// Double move on starting row
					if( y == pawnRow ) {
						if( IsClear(x, y-2*dir) ) {
							HighlightPotentialMove(piece, x, y-2*dir);	// Double move on first row
						}
					}
				}
				// Capture right
				if( !IsClear(x+1, y-1*dir) ) {
					HighlightPotentialMove(piece, x+1, y-1*dir);
				}
				// Capture left
				if( !IsClear(x-1, y-1*dir) ) {
					HighlightPotentialMove(piece, x-1, y-1*dir);
				}
			} else {
				HighlightPotentialMove(piece, x+1, y-1*dir);
				HighlightPotentialMove(piece, x-1, y-1*dir);
			}
		}
		
		// Knight
		if( type == TileDef.TileType.knight ) {
			HighlightPotentialMove(piece, x+1, y-2*dir);
			HighlightPotentialMove(piece, x+1, y+2*dir);
			HighlightPotentialMove(piece, x-1, y-2*dir);
			HighlightPotentialMove(piece, x-1, y+2*dir);
			HighlightPotentialMove(piece, x+2, y-1*dir);
			HighlightPotentialMove(piece, x+2, y+1*dir);
			HighlightPotentialMove(piece, x-2, y-1*dir);
			HighlightPotentialMove(piece, x-2, y+1*dir);
		}
		
		if( type == TileDef.TileType.bishop || type == TileDef.TileType.queen )
		{
			DrawPieceHighlightTravel(piece, x, y,  1,  1);
			DrawPieceHighlightTravel(piece, x, y,  1, -1);
			DrawPieceHighlightTravel(piece, x, y, -1,  1);
			DrawPieceHighlightTravel(piece, x, y, -1, -1);
		}
		
		if( type == TileDef.TileType.rook || type == TileDef.TileType.queen ) {
			for(var i = 1; i < 8; i++) {
				DrawPieceHighlightTravel(piece, x, y,  1,  0);
				DrawPieceHighlightTravel(piece, x, y, -1,  0);
				DrawPieceHighlightTravel(piece, x, y,  0,  1);
				DrawPieceHighlightTravel(piece, x, y,  0, -1);
			}
		}
		
		if( type == TileDef.TileType.king ) {
			HighlightPotentialMove(piece, x+1, y+1);
			HighlightPotentialMove(piece, x+0, y+1);
			HighlightPotentialMove(piece, x-1, y+1);
			
			HighlightPotentialMove(piece, x+1, y+0);
			HighlightPotentialMove(piece, x-1, y+0);
			
			HighlightPotentialMove(piece, x+1, y-1);
			HighlightPotentialMove(piece, x+0, y-1);
			HighlightPotentialMove(piece, x-1, y-1);
		}
	}
	
	void ClearHighlights() {
//		PS.BeadBorderWidth(PS.ALL, PS.ALL, 0);
	}
	
	bool RunTheGauntlet() {
		TileDef.TileType[] pieceTypes = new TileDef.TileType[]
			{
				TileDef.TileType.pawn,
				TileDef.TileType.knight,
				TileDef.TileType.bishop,
				TileDef.TileType.rook,
				TileDef.TileType.queen,
				TileDef.TileType.king
			};
		
		// Loop over the piece tyeps
		for( int i = 0; i < pieceTypes.Length; i++ ) {
			TileDef.TileType pieceType = pieceTypes[i];
			
			// Loop over the board
			for( int y = 0; y < GameConstants.BOARD_H; y++ ) {
				for( int x = 0; x < GameConstants.BOARD_W; x++ ) {
					// Inspect the piece there
					TileDef piece = BoardManager.Instance.GetTile(x, y);
					if( piece.owner == TileDef.TileOwner.black && piece.type == pieceType ) {
						DrawPieceHighlights(x, y, piece);
						
						if( GAME_ENEMY_THREATEN == true ) {
							// Clear others and redraw for special fx
							ClearHighlights();
							DrawPieceHighlights(x, y, piece);
							
							GAME_ENEMY_SOLDIER_X = x;
							GAME_ENEMY_SOLDIER_Y = y;
							return true;
						}
					}
				}
			}
		}
		
		// Don't let the player see the mess we made
		ClearHighlights();
		
		return false;
	}
	
	void MakeEnemyMove() {
		ClearHighlights();
		gameState = GameState.Black;
		GAME_ENEMY_THREATEN = false;
		GAME_ENEMY_MOVE_PROGRESS = 50;
	}
	
	void ProcessEnemyMove() {
		if( GAME_ENEMY_MOVE_PROGRESS_PROCESSED == false ) {
			GAME_ENEMY_MOVE_PROGRESS_PROCESSED = true;
			if( RunTheGauntlet() == false ) {
				GAME_ENEMY_MOVE_PROGRESS = 0;
				AudioPlay("GAME_SOUND_MOVE");
			} else {
				AudioPlay("GAME_SOUND_SPOTTED");
			}
		} else if (GAME_ENEMY_MOVE_PROGRESS == 15) {
			if (GAME_ENEMY_THREATEN == true) {
				// Capture the player's piece
				TileDef enemyPiece = BoardManager.Instance.GetTile(GAME_ENEMY_SOLDIER_X, GAME_ENEMY_SOLDIER_Y);
//				TileDef myPiece = BoardManager.Instance.GetTile(GAME_ENEMY_THREATEN_X, GAME_ENEMY_THREATEN_Y);
				BoardManager.Instance.PutTileData(GAME_ENEMY_THREATEN_X, GAME_ENEMY_THREATEN_Y, enemyPiece);
				BoardManager.Instance.PutTileData(GAME_ENEMY_SOLDIER_X, GAME_ENEMY_SOLDIER_Y, GetEmptyPiece());
				
				AudioPlay("GAME_SOUND_CAPTURED");
				
				GAME_ENEMY_THREATEN = false;
			} else {
				// Something bad happened
				Debug.LogWarning("Something bad happened\n");
			}
		}	
		
		if( GAME_ENEMY_MOVE_PROGRESS <= 0 ) {
			ClearHighlights();
			gameState = GameState.White;
			GAME_ENEMY_MOVE_PROGRESS_PROCESSED = false;
			
			CheckForLossCondition();
			
			// Re-highlight wherever the user's mouse was
			Enter(GAME_MOUSE_X, GAME_MOUSE_Y, BoardManager.Instance.GetTile(GAME_MOUSE_X, GAME_MOUSE_Y));
		}
		
		GAME_ENEMY_MOVE_PROGRESS--;
	}
	
	void ProcessBetweenTurns() {
		BoardManager.Instance.SaveBoardStateForUndo();
	}
	
	bool PiecesLeft(TileDef.TileOwner owner) {
		for( int y = 0; y < GameConstants.BOARD_H; y++ ) {
			for( int x = 0; x < GameConstants.BOARD_W; x++ ) {
				TileDef piece = BoardManager.Instance.GetTile(x, y);
				if( piece.owner == owner ) {
					return true;
				}
			}
		}
		return false;
	}
	
	void CheckForLossCondition() {
		if( PiecesLeft(TileDef.TileOwner.white) == false ) {
			StatusText("Out of materiel. Press R to restart.");
			AudioPlay("GAME_SOUND_LOSE");
			gameState = GameState.Lost;
		}
	}
	
	////////////////////////////////////////////////////////////////////////////////
	// Shim methods
	////////////////////////////////////////////////////////////////////////////////
	
	void StatusText(string message) {
		Debug.Log("Status: " + message);
	}

	void AudioPlay(string audio) {
		Debug.Log("Audio: " + audio);
	}

	TileDef GetEmptyPiece() {
		return new TileDef(TileDef.TileOwner.nobody, TileDef.TileType.empty);
	}
	
	bool IsInBounds(int x, int y) {
		return (x >= 0 && x < GameConstants.BOARD_W && y >= 0 && y < GameConstants.BOARD_H);
	}
	
	bool IsClear(int x, int y) {
		return IsInBounds(x, y) && BoardManager.Instance.GetTile(x, y).type == TileDef.TileType.empty;
	}
	
	bool IsLastLevel() {
		//return ( GAME_level === (LEVELS.length - 1) );
		return false;
	}
	
	bool IsMenuLevel() {
		//return ( GAME_level === 0 );
		return false;
	}
	
}
