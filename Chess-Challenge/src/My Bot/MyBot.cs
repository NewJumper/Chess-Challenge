﻿using System;
using ChessChallenge.API;

public class MyBot : IChessBot {
    public static string pawnPoints = "0000000045500554421331243336633344577544556886559999999999999999";
    int[] pieceValues = { 0, 1, 3, 3, 5, 9, 20 };

    public Move Think(Board board, Timer timer) {
        Move[] moves = board.GetLegalMoves();
        Move move = moves[new Random().Next(moves.Length)];
        float boardEval = Evaluation(board, board.IsWhiteToMove); // get board evaluation before the bot makes a move

        foreach (Move possibleMove in moves) {
            // make a move, if that move improves eval: confirm move
            board.MakeMove(possibleMove);
            float eval = Evaluation(board, !board.IsWhiteToMove); // get board evaluation after bot makes the possible move (it is techinically opponents turn right now, thats why we undo the move later)
            // Console.WriteLine(eval + " ? " + boardEval);
            if (eval > boardEval) {
                boardEval = eval;
                move = possibleMove;
            }
            board.UndoMove(possibleMove);
        }

        /*Console.WriteLine(move);
        Console.WriteLine("previous: " + Evaluation(board, board.IsWhiteToMove));
        board.MakeMove(move);
        Console.WriteLine("current: " + Evaluation(board, board.IsWhiteToMove));
        board.UndoMove(move);*/

        return move;
    }

    float Evaluation(Board board, bool white) {
        if (board.IsInCheckmate()) return 10000;
        float evaluation = 0;
        for (int i = 0; i < 64; i++) {
            int pieceType = (int) GetPiece(board, i).PieceType;
            int value = pieceValues[pieceType];
            if (GetPiece(board, i).IsWhite) evaluation += value;
            else evaluation -= value;
        }
        return white ? evaluation : -evaluation; // return board evaluation
    }

    Piece GetPiece(Board board, int index) {
        return board.GetPiece(new Square(index));
    }
}
