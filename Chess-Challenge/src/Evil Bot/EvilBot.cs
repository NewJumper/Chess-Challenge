using System;
using System.Collections.Generic;
using ChessChallenge.API;

namespace ChessChallenge.Example {
    public class EvilBot : IChessBot {
        int[] pieceValues = { 0, 1, 3, 3, 5, 9, 20 };
        List<Move> moves = new(1);

        public Move Think(Board board, Timer timer) {
            minMax(board, 3, int.MinValue, int.MaxValue, board.IsWhiteToMove);
            return moves[0];
        }

        int minMax(Board board, int depth, int min, int max, bool white) {
            if (depth == 0 || board.IsInCheckmate()) return Evaluation(board);

            Move move = new();
            int eval = white ? int.MinValue : int.MaxValue;
            foreach (Move possibleMove in board.GetLegalMoves()) {
                board.MakeMove(possibleMove);

                int newEval = minMax(board, depth - 1, min, max, !white);
                if (white && newEval > eval) {
                    eval = newEval;
                    move = possibleMove;
                } else if (!white && newEval < eval) {
                    eval = newEval;
                    move = possibleMove;
                }

                if (white) min = Math.Max(min, newEval);
                else max = Math.Min(max, newEval);
                board.UndoMove(possibleMove);
                if (max <= min) break;
            }

            moves.Insert(0, move);
            return eval;
        }

        int Evaluation(Board board) {
            if (board.IsInCheckmate()) return board.IsWhiteToMove ? -10000 : 10000; // if its checkmate, dont do any of the following checks
            int evaluation = 0;
            for (int i = 0; i < 64; i++) { // go through all 64 squares
                int pieceType = (int) GetPiece(board, i).PieceType; // get piece type in integer form (0 for nothing, 1 for pawn, etc.)
                int value = pieceValues[pieceType]; // get the value of the piece at the square using the pieceValues array
                if (GetPiece(board, i).IsWhite) evaluation += value; // add value to eval if the piece is white
                else evaluation -= value; // subtract value from eval if the piece is black
            }
            return evaluation;
        }

        Piece GetPiece(Board board, int index) {
            return board.GetPiece(new Square(index));
        }
    }

    /*// A simple bot that can spot mate in one, and always captures the most valuable piece it can.
    // Plays randomly otherwise.
    public class EvilBot : IChessBot {
        // Piece values: null, pawn, knight, bishop, rook, queen, king
        int[] pieceValues = { 0, 100, 300, 300, 500, 900, 10000 };

        public Move Think(Board board, Timer timer) {
            Move[] allMoves = board.GetLegalMoves();

            // Pick a random move to play if nothing better is found
            Random rng = new();
            Move moveToPlay = allMoves[rng.Next(allMoves.Length)];
            int highestValueCapture = 0;

            foreach (Move move in allMoves) {
                // Always play checkmate in one
                if (MoveIsCheckmate(board, move)) {
                    moveToPlay = move;
                    break;
                }

                // Find highest value capture
                Piece capturedPiece = board.GetPiece(move.TargetSquare);
                int capturedPieceValue = pieceValues[(int) capturedPiece.PieceType];

                if (capturedPieceValue > highestValueCapture) {
                    moveToPlay = move;
                    highestValueCapture = capturedPieceValue;
                }
            }

            return moveToPlay;
        }

        // Test if this move gives checkmate
        bool MoveIsCheckmate(Board board, Move move) {
            board.MakeMove(move);
            bool isMate = board.IsInCheckmate();
            board.UndoMove(move);
            return isMate;
        }
    }*/
}