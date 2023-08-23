using System;
using ChessChallenge.API;

public class MyBot : IChessBot {
    int[] pieceValues = { 0, 1, 3, 3, 5, 9, 20 };

    public Move Think(Board board, Timer timer) {
        Move[] moves = board.GetLegalMoves();
        Move move = moves[new Random().Next(moves.Length)];
        float boardEval = Evaluation(board); // get board evaluation before the bot makes a move

        foreach (Move possibleMove in moves) {
            // check if checkmate, if so: make move GGs
            board.MakeMove(possibleMove);
            bool checkmate = board.IsInCheckmate();
            board.UndoMove(possibleMove);
            if (checkmate) return possibleMove;

            // make a move, if that move improves eval: make move
            board.MakeMove(possibleMove);
            float eval = -Evaluation(board); // get board evaluation after bot makes the possible move (it is techinically opponents turn right now, thats why we undo the move later)
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
        return board.IsWhiteToMove ? evaluation : -evaluation; // return board evaluation
    }

    Piece GetPiece(Board board, int index) {
        return board.GetPiece(new Square(index));
    }
}
