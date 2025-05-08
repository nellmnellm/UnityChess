using System.Collections.Generic;
using UnityEngine;

public class Knight : Piece
{
    public override List<Vector2Int> GetAvailableMoves(Vector2Int boardPosition)
    {
        List<Vector2Int> moves = new List<Vector2Int>();

        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(2, 1), new Vector2Int(1, 2),
            new Vector2Int(-1, 2), new Vector2Int(-2, 1),
            new Vector2Int(-2, -1), new Vector2Int(-1, -2),
            new Vector2Int(1, -2), new Vector2Int(2, -1)
        };

        foreach (var dir in directions)
        {
            Vector2Int next = boardPosition + dir;
            moves.Add(next);
        }

        return moves;
    }
}