using System;
using ChessChallenge.API;

public class MyBot : IChessBot {
    int[] pieceValues = { 0, 1, 3, 3, 5, 9, 100 };

    public Move Think(Board board, Timer timer) {
        Move[] moves = board.GetLegalMoves();
        Move move = moves[new Random().Next(moves.Length)];
        float boardEval = Evaluation(board);

        foreach (Move possibleMove in moves) {
            board.MakeMove(possibleMove);
            bool checkmate = board.IsInCheckmate();
            board.UndoMove(possibleMove);
            if (checkmate) return possibleMove;

            board.MakeMove(possibleMove);
            float eval = -Evaluation(board);
            Console.WriteLine(eval + " ? " + boardEval);
            if (eval > boardEval) {
                boardEval = eval;
                move = possibleMove;
            }
            board.UndoMove(possibleMove);
        }

        Console.WriteLine(move);

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
        return board.IsWhiteToMove ? evaluation : -evaluation;
    }

    Piece GetPiece(Board board, int index) {
        return board.GetPiece(new Square(index));
    }
}
