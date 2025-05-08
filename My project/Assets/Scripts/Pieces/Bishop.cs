using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    public override List<Vector2Int> GetAvailableMoves(Vector2Int boardPosition)
    {
        List<Vector2Int> moves = new List<Vector2Int>();

        Vector2Int[] directions = {
            new Vector2Int(1, 1),
            new Vector2Int(1, -1),
            new Vector2Int(-1, 1),
            new Vector2Int(-1, -1)
        };

        foreach (var dir in directions)
        {
            for (int i = 1; i < 8; i++)
            {
                Vector2Int next = boardPosition + dir * i;
                if (IsValidPosition(next))
                    moves.Add(next);
                else
                    break;
            }
        }

        return moves;
    }
}