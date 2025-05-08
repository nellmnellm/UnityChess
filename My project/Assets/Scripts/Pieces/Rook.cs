using System.Collections.Generic;
using UnityEngine;

public class Rook : Piece
{
    public override List<Vector2Int> GetAvailableMoves(Vector2Int boardPosition)
    {
        List<Vector2Int> moves = new List<Vector2Int>();

        Vector2Int[] directions = {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        foreach (var dir in directions)
        {
            for (int i = 1; i < 8; i++)
            {
                Vector2Int next = boardPosition + dir * i;
                if (IsValidPosition(next))
                    moves.Add(next);
                else
                    break; // 보드 밖이면 중단
            }
        }

        return moves;
    }
}