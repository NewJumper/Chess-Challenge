using System;
using System.Collections.Generic;
using ChessChallenge.API;

public class MyBot : IChessBot {
    public static string pawnPoints = "0000000045500554421331243336633344577544556886559999999999999999";
    int[] pieceValues = { 0, 1, 3, 3, 5, 9, 20 };
    List<Move> moves = new(3);

    public Move Think(Board board, Timer timer) {
        minMax(board, 3, int.MinValue, int.MaxValue, board.IsWhiteToMove);

        foreach(Move move1 in moves) {
            Console.Write(move1.ToString().Replace("Move: ", "") + ", ");
        }

        return moves[0];
    }

    int minMax(Board board, int depth, int min, int max, bool white) {
        if(depth == 0 || board.IsInCheckmate()) return Evaluation(board);

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

            //eval = white ? Math.Max(eval, newEval) : Math.Min(eval, newEval);

            if (white) min = Math.Max(min, newEval);
            else max = Math.Min(max, newEval);
            board.UndoMove(possibleMove);
            if (max <= min) break;
        }

        moves.Add(move);
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
