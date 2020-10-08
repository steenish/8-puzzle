using TMPro;
using UnityEngine;

public class Board : MonoBehaviour {

	public bool shuffle;
	public bool reset;
	public Transform[] knobs;
	public Transform[] pieces;
	public GameObject resetButton;
	public int shuffleSteps;
	public bool showDebugBoard;
	public GameObject debugBoard;

	private int[] board;

	void Start() {
		board = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

		shuffle = false;
		reset = false;

		debugBoard.SetActive(showDebugBoard);
    }

	private void OnValidate() {
		if (shuffle) {
			shuffle = false;
			Shuffle();
		}

		if (reset) {
			reset = false;
			board = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			UpdatePieces();
			resetButton.SetActive(true);
		}
	}

	public void Move(int piece) {
		bool moved = false;
		for (int i = 0; i < board.Length; ++i) {
			if (board[i] == piece) {
				int[] adjacentIndices = adjacentBoardIndices(i);
				if (adjacentIndices != null) {
					for (int j = 0; j < adjacentIndices.Length; ++j) {
						if (board[adjacentIndices[j]] == 9) {
							board[adjacentIndices[j]] = piece;
							board[i] = 9;
							
							MovePiece(piece, adjacentIndices[j]);
							moved = true;
							break;
						}
					}
				}
			}
			if (moved) break;
		}

		if (IsWin()) {
			resetButton.SetActive(true);
		} else {
			resetButton.SetActive(false);
		}
	}

	private void MovePiece(int piece, int toBoardIndex) {
		LogBoard();

		Vector3 knobPosition = knobs[toBoardIndex].position;
		Transform pieceTransform = pieces[piece - 1];
		pieceTransform.position = knobPosition;
	}

	private bool IsWin() {
		for (int i = 0; i < board.Length; ++i) {
			if (board[i] != i + 1) {
				return false;
			}
		}
		return true;
	}

	public void Shuffle() {
		for (int i = 0; i < shuffleSteps; ++i) {
			int piece = Random.Range(1, 9);
			Move(piece);
			Debug.Log("call Move(" + piece + ")");
		}

		UpdatePieces();
		resetButton.SetActive(false);
	}

	private void UpdatePieces() {
		for (int i = 0; i < board.Length; ++i) {
			int piece = board[i];
			if (piece == 9) continue;

			MovePiece(piece, i);
		}
	}

	private int[] adjacentBoardIndices(int index) {

		switch (index) {
			case 0:
				return new int[] { 1, 3 };
			case 1:
				return new int[] { 0, 2, 4 };
			case 2:
				return new int[] { 1, 5 };
			case 3:
				return new int[] { 0, 4, 6 };
			case 4:
				return new int[] { 1, 3, 5, 7 };
			case 5:
				return new int[] { 2, 4, 8 };
			case 6:
				return new int[] { 3, 7 };
			case 7:
				return new int[] { 4, 6, 8 };
			case 8:
				return new int[] { 5, 7 };
			default:
				return null;
		}
	}

	private void LogBoard() {
		string boardString = "{\n";
		for (int i = 0; i < 3; ++i) {
			for (int j = 0; j < 3; ++j) {
				boardString += board[i * 3 + j] + ", ";
			}
			boardString += "\n";
		}
		boardString += "}";
		debugBoard.GetComponent<TMP_Text>().text = boardString;
	}
}
