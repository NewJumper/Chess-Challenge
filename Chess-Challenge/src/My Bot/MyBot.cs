using System;
using ChessChallenge.API;
using ChessChallenge.Example;

public class MyBot : IChessBot {
    int[] pieceValues = { 0, 1, 3, 3, 5, 9, 100 };

    public Move Think(Board board, Timer timer) {
        Move[] moves = board.GetLegalMoves();
        int highestValue = 0;
        Move move = moves[0];

        Console.WriteLine(Evaluation(board));

        foreach(Move possibleMove in moves) {
            board.MakeMove(possibleMove);
            bool checkmate = board.IsInCheckmate();
            board.UndoMove(possibleMove);
            if(checkmate) return possibleMove;

            int pieceValue = pieceValues[(int) possibleMove.CapturePieceType];
            if (pieceValue > highestValue) {
                highestValue = pieceValue;
                move = possibleMove;
            }
        }
        return move;
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
