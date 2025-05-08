using System.Collections.Generic;
using UnityEngine;


public abstract class Piece : MonoBehaviour
{
    public bool isWhite;
    public Vector2Int boardPosition;

    public abstract List<Vector2Int> GetAvailableMoves(Vector2Int boardPosition);


    protected bool IsValidPosition(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < 8 && pos.y >= 0 && pos.y < 8;
    }
}

