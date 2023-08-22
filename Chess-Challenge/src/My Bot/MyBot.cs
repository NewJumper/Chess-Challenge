using ChessChallenge.API;
using ChessChallenge.Example;

public class MyBot : IChessBot {
    int[] pieceValues = { 0, 100, 300, 300, 500, 900, 10000 };

    public Move Think(Board board, Timer timer) {
        Move[] moves = board.GetLegalMoves();

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
}
