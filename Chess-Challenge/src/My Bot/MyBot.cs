using System;
using ChessChallenge.API;
using ChessChallenge.Example;

public class MyBot : IChessBot {
    int[] pieceValues = { 0, 1, 3, 3, 5, 9, 100 };

    public Move Think(Board board, Timer timer) {
        Move[] moves = board.GetLegalMoves();

        Console.WriteLine(Evaluation(board));

        foreach(Move move in moves) {
            board.MakeMove(move);
            bool checkmate = board.IsInCheckmate();
            board.UndoMove(move);
            if(checkmate) return move;

            PieceType piece = move.CapturePieceType;
            if (piece != 0) return move;
        }
        return moves[0];
    }

    float Evaluation(Board board) {
        float evaluation = 0;
        for (int i = 0; i < 64; i++) {
            int pieceType = (int) GetPiece(board, i).PieceType;
            int value = pieceValues[pieceType];
            if (GetPiece(board, i).IsWhite) evaluation += value;
            else evaluation -= value;
        }
        return evaluation;
    }

    Piece GetPiece(Board board, int index) {
        return board.GetPiece(new Square(index));
    }
}
