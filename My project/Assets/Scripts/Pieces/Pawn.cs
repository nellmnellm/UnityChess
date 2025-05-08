using System.Collections.Generic;
using UnityEngine;


public class Pawn : Piece
{
    public override List<Vector2Int> GetAvailableMoves(Vector2Int boardPosition)
    {
        List<Vector2Int> moves = new List<Vector2Int>();
        int direction = isWhite ? 1 : -1;

        Vector2Int forwardOne = boardPosition + new Vector2Int(0, direction);

        if (GameManager.Instance.IsEmpty(forwardOne))
            moves.Add(forwardOne);

        Vector2Int forwardTwo = boardPosition + new Vector2Int(0, direction * 2);
        if ((isWhite && boardPosition.y == 1 || !isWhite && boardPosition.y == 6) &&
            GameManager.Instance.IsEmpty(forwardOne) && GameManager.Instance.IsEmpty(forwardTwo))
        {
            moves.Add(forwardTwo);
        }

        Vector2Int[] diagonals = {
        new Vector2Int(1, direction),
        new Vector2Int(-1, direction)
        };

        foreach (var diag in diagonals)
        {
            Vector2Int target = boardPosition + diag;
            if (GameManager.Instance.IsEnemyAt(target, isWhite))
                moves.Add(target);
        }

        return moves;
    }
}
