using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    public override List<Vector2Int> GetAvailableMoves(Vector2Int boardPosition)
    {
        List<Vector2Int> moves = new List<Vector2Int>();

        // ŷ�� �̵� ������ 8����
        Vector2Int[] directions = new Vector2Int[]
        {
            new Vector2Int(0, 1),   // ��
            new Vector2Int(1, 1),   // ���
            new Vector2Int(1, 0),   // ������
            new Vector2Int(1, -1),  // ����
            new Vector2Int(0, -1),  // �Ʒ�
            new Vector2Int(-1, -1), // ����
            new Vector2Int(-1, 0),  // ����
            new Vector2Int(-1, 1)   // �»�
        };

        foreach (Vector2Int dir in directions)
        {
            Vector2Int target = boardPosition + dir;

            // ���� �� && ����ų� �� �⹰�� ���� �̵�
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