using System;
using ChessChallenge.API;

public class MyBot : IChessBot {
    public static string pawnPoints = "0000000043300334202112023302203340433404065445605786687578999987";
    int[] pieceValues = { 0, 1, 3, 3, 5, 9, 20 };

    public Move Think(Board board, Timer timer) {
        Move[] moves = board.GetLegalMoves();
        Move move = moves[new Random().Next(moves.Length)];
        float boardEval = Evaluation(board, board.IsWhiteToMove); // get board evaluation before the bot makes a move

        foreach (Move possibleMove in moves) {
            // check if checkmate, if so: make move
            board.MakeMove(possibleMove);
            bool checkmate = board.IsInCheckmate();
            board.UndoMove(possibleMove);
            if (checkmate) return possibleMove;

            // make a move, if that move improves eval: confirm move
            board.MakeMove(possibleMove);
            float eval = Evaluation(board, !board.IsWhiteToMove); // get board evaluation after bot makes the possible move (it is techinically opponents turn right now, thats why we undo the move later)
            Console.WriteLine(eval + " ? " + boardEval);
            if (eval > boardEval) {
                boardEval = eval;
                move = possibleMove;
            }
            board.UndoMove(possibleMove);
        }

        Console.WriteLine(move);
        Console.WriteLine(Evaluation(board, board.IsWhiteToMove));
        board.MakeMove(move);
        Console.WriteLine(Evaluation(board, board.IsWhiteToMove));
        board.UndoMove(move);

        return move;
    }

    float Evaluation(Board board, bool white) {
        float evaluation = 0;
        for (int i = 0; i < 64; i++) {
            int pieceType = (int) GetPiece(board, i).PieceType;
            int value = pieceValues[pieceType];
            if (white && pieceType == 1) {
                value *= byte.Parse(pawnPoints[i] + "");
            } else if (pieceType == 1) {
                value *= byte.Parse(pawnPoints[63 - i] + "");
            }
            if (GetPiece(board, i).IsWhite) evaluation += value;
            else evaluation -= value;
        }
        return white ? evaluation : -evaluation; // return board evaluation
    }

    Piece GetPiece(Board board, int index) {
        return board.GetPiece(new Square(index));
    }
}
