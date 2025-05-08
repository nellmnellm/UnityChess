using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    public override List<Vector2Int> GetAvailableMoves(Vector2Int boardPosition)
    {
        List<Vector2Int> moves = new List<Vector2Int>();

        // 킹이 이동 가능한 8방향
        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(0, 1),   // 위
            new Vector2Int(1, 1),   // 우상
            new Vector2Int(1, 0),   // 오른쪽
            new Vector2Int(1, -1),  // 우하
            new Vector2Int(0, -1),  // 아래
            new Vector2Int(-1, -1), // 좌하
            new Vector2Int(-1, 0),  // 왼쪽
            new Vector2Int(-1, 1)   // 좌상
        };

        foreach (Vector2Int dir in directions)
        {
            Vector2Int target = boardPosition + dir;

            // 범위 내 && 비었거나 적 기물일 때만 이동
            if (GameManager.Instance.IsValidPosition(target))
            {
                if (GameManager.Instance.IsEmpty(target) || GameManager.Instance.IsEnemyAt(target, isWhite))
                {
                    moves.Add(target);
                }
            }
        }

        return moves;
    }
}