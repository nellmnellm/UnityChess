using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public BoardManager boardManager;

    public bool isWhiteTurn = true;  // ���� ���� ���� 
    private Piece selectedPiece;

    public GameObject whitePlayer; // �� �÷��̾�
    public GameObject blackPlayer; // �� �÷��̾�


    public Piece[,] boardState = new Piece[8, 8]; // ��ġ�� �⹰ ����

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [Header("White Object")]
    public GameObject whitePawnPrefab;
    public GameObject whiteRookPrefab;
    public GameObject whiteKnightPrefab;
    public GameObject whiteBishopPrefab;
    public GameObject whiteQueenPrefab;
    public GameObject whiteKingPrefab;
    [Header("Black Object")]
    public GameObject blackPawnPrefab;
    public GameObject blackRookPrefab;
    public GameObject blackKnightPrefab;
    public GameObject blackBishopPrefab;
    public GameObject blackQueenPrefab;
    public GameObject blackKingPrefab;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        SpawnPieces();

        SetPlayerTurn(isWhiteTurn); // ù ��° �� ����

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick();
        }
    }


    public void EndTurn()
    {
        isWhiteTurn = !isWhiteTurn;  // ���� �ٲ��ش�
        SetPlayerTurn(isWhiteTurn);  // ȭ�鿡 ���� ������ ǥ�� (�ɼ�)
    }
    private void SetPlayerTurn(bool isWhiteTurn)
    {
        if (isWhiteTurn)
        {
            whitePlayer.SetActive(true);  // �� �÷��̾ ���� ��
            blackPlayer.SetActive(false); // �� �÷��̾�� ��Ȱ��ȭ
        }
        else
        {
            whitePlayer.SetActive(false); // �� �÷��̾ ���� ��
            blackPlayer.SetActive(true);  // �� �÷��̾�� ��Ȱ��ȭ
        }
    }


    void SpawnPieces()
    {
        for (int x = 0; x < 8; x++)
        {
            Vector3 whitePos = boardManager.BoardToWorldPosition(x, 1);
            Vector3 blackPos = boardManager.BoardToWorldPosition(x, 6);

            Instantiate(whitePawnPrefab, whitePos + Vector3.up * 0.01f, Quaternion.identity);
            Instantiate(blackPawnPrefab, blackPos + Vector3.up * 0.01f, Quaternion.identity);
        }
        //�� ��ȯ
        Instantiate(whiteRookPrefab, boardManager.BoardToWorldPosition(0, 0), Quaternion.identity);
        Instantiate(whiteRookPrefab, boardManager.BoardToWorldPosition(7, 0), Quaternion.identity);
        Instantiate(blackRookPrefab, boardManager.BoardToWorldPosition(0, 7), Quaternion.identity); 
        Instantiate(blackRookPrefab, boardManager.BoardToWorldPosition(7, 7), Quaternion.identity);
        //����Ʈ ��ȯ
        Instantiate(whiteKnightPrefab, boardManager.BoardToWorldPosition(1, 0), Quaternion.identity);
        Instantiate(whiteKnightPrefab, boardManager.BoardToWorldPosition(6, 0), Quaternion.identity);
        Instantiate(blackKnightPrefab, boardManager.BoardToWorldPosition(1, 7), Quaternion.identity);
        Instantiate(blackKnightPrefab, boardManager.BoardToWorldPosition(6, 7), Quaternion.identity);
        //��� ��ȯ
        Instantiate(whiteBishopPrefab, boardManager.BoardToWorldPosition(2, 0), Quaternion.identity);
        Instantiate(whiteBishopPrefab, boardManager.BoardToWorldPosition(5, 0), Quaternion.identity);
        Instantiate(blackBishopPrefab, boardManager.BoardToWorldPosition(2, 7), Quaternion.identity);
        Instantiate(blackBishopPrefab, boardManager.BoardToWorldPosition(5, 7), Quaternion.identity);
        //�� ��ȯ
        Instantiate(whiteQueenPrefab, boardManager.BoardToWorldPosition(3, 0), Quaternion.identity);
        Instantiate(blackQueenPrefab, boardManager.BoardToWorldPosition(3, 7), Quaternion.identity);
        //ŷ ��ȯ
        Instantiate(whiteKingPrefab, boardManager.BoardToWorldPosition(4, 0), Quaternion.identity);
        Instantiate(blackKingPrefab, boardManager.BoardToWorldPosition(4, 7), Quaternion.identity);
    }

    /*public List<Vector2Int> FilterBlockedMoves(List<Vector2Int> rawMoves, Vector2Int currentPos, Piece self)
    {
        List<Vector2Int> result = new List<Vector2Int>();
        foreach (var pos in rawMoves)
        {
            if (!IsBlocked(pos, currentPos, self))
            {
                result.Add(pos);
            }
            else
            {
                // �� �⹰�̸� �� ĭ������ OK
                if (IsEnemyAt(pos, self))
                    result.Add(pos);

                // ���� ĭ�� ���� ����
                break;
            }
        }
        return result;
    }*/

    public bool IsValidPosition(Vector2Int pos)
    {
        return pos.x >= 0 && pos.x < 8 && pos.y >= 0 && pos.y < 8;
    }

    public bool IsEmpty(Vector2Int pos)
    {
        if (!IsValidPosition(pos)) return false;
        return boardState[pos.x, pos.y] == null;
    }

    public bool IsEnemyAt(Vector2Int pos, bool isWhite)
    {
        if (!IsValidPosition(pos)) return false;

        Piece piece = boardState[pos.x, pos.y];
        if (piece == null) return false;

        return piece.isWhite != isWhite;
    }

    public void PlacePiece(Piece piece, Vector2Int pos)
    {
        boardState[pos.x, pos.y] = piece;
        piece.boardPosition = pos;
    }


    void HandleClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 worldPos = hit.point;
            Vector2Int boardPos = boardManager.WorldToBoardPosition(worldPos);

            Piece clickedPiece = GetPieceAtPosition(boardPos);

            if (selectedPiece == null)
            {
                // �⹰ ����
                if (clickedPiece != null && clickedPiece.isWhite == isWhiteTurn)
                {
                    selectedPiece = clickedPiece;
                    // ���̶���Ʈ �Ǵ� UI ó�� ����
                }
            }
            else
            {
                // �� �̵� �õ�
                TryMoveTo(boardPos);
            }
        }
    }
    void TryMoveTo(Vector2Int targetPos)
    {
        var validMoves = selectedPiece.GetAvailableMoves(selectedPiece.boardPosition);
        if (!validMoves.Contains(targetPos))
        {
            selectedPiece = null;
            return;
        }

        Piece target = GetPieceAtPosition(targetPos);
        if (target != null && target.isWhite != selectedPiece.isWhite)
        {
            Destroy(target.gameObject);
        }

        // ��ġ ����
        boardState.Remove(selectedPiece.boardPosition);
        selectedPiece.boardPosition = targetPos;
        selectedPiece.transform.position = boardManager.BoardToWorldPosition(targetPos) + Vector3.up * 0.01f;
        boardState[targetPos] = selectedPiece;

        selectedPiece = null;
        isWhiteTurn = !isWhiteTurn;
    }

    public Piece GetPieceAtPosition(Vector2Int pos)
    {
        boardState.TryGetValue(pos, out Piece piece);
        return piece;
    }

}
