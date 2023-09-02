using System;
using ChessChallenge.API;

public class MyBot : IChessBot {
    public static string pawnPoints = "0000000045500554421331243336633344577544556886559999999999999999";
    int[] pieceValues = { 0, 1, 3, 3, 5, 9, 20 };

    public Move Think(Board board, Timer timer) {
        bool white = board.IsWhiteToMove;
        Move[] moves = board.GetLegalMoves();
        Move move = moves[new Random().Next(moves.Length)];
        float boardEval = Evaluation(board); // get board evaluation before the bot makes a move

        foreach (Move possibleMove in moves) {
            // play possibleMove, then check if that move improves the bot's eval. if it does, set move = possibleMove
            board.MakeMove(possibleMove);
            float eval = Evaluation(board);
            if (white && eval > boardEval) {
                boardEval = eval;
                move = possibleMove;
            } else if (!white && eval < boardEval) {
                boardEval = eval;
                move = possibleMove;
            }
            board.UndoMove(possibleMove);
        }

        return move;
    }

    float Evaluation(Board board) {
        if (board.IsInCheckmate()) return board.IsWhiteToMove ? -10000 : 10000; // if its checkmate, dont do any of the following checks
        float evaluation = 0;
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
