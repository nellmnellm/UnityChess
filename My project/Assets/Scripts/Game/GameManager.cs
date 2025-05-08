using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public BoardManager boardManager;

    public bool isWhiteTurn = true;  // 백이 먼저 시작 
    private Piece selectedPiece;

    public GameObject whitePlayer; // 백 플레이어
    public GameObject blackPlayer; // 흑 플레이어


    public Piece[,] boardState = new Piece[8, 8]; // 위치별 기물 참조

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

        SetPlayerTurn(isWhiteTurn); // 첫 번째 턴 설정

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
        isWhiteTurn = !isWhiteTurn;  // 턴을 바꿔준다
        SetPlayerTurn(isWhiteTurn);  // 화면에 누구 턴인지 표시 (옵션)
    }
    private void SetPlayerTurn(bool isWhiteTurn)
    {
        if (isWhiteTurn)
        {
            whitePlayer.SetActive(true);  // 백 플레이어가 현재 턴
            blackPlayer.SetActive(false); // 흑 플레이어는 비활성화
        }
        else
        {
            whitePlayer.SetActive(false); // 흑 플레이어가 현재 턴
            blackPlayer.SetActive(true);  // 백 플레이어는 비활성화
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
        //룩 소환
        Instantiate(whiteRookPrefab, boardManager.BoardToWorldPosition(0, 0), Quaternion.identity);
        Instantiate(whiteRookPrefab, boardManager.BoardToWorldPosition(7, 0), Quaternion.identity);
        Instantiate(blackRookPrefab, boardManager.BoardToWorldPosition(0, 7), Quaternion.identity); 
        Instantiate(blackRookPrefab, boardManager.BoardToWorldPosition(7, 7), Quaternion.identity);
        //나이트 소환
        Instantiate(whiteKnightPrefab, boardManager.BoardToWorldPosition(1, 0), Quaternion.identity);
        Instantiate(whiteKnightPrefab, boardManager.BoardToWorldPosition(6, 0), Quaternion.identity);
        Instantiate(blackKnightPrefab, boardManager.BoardToWorldPosition(1, 7), Quaternion.identity);
        Instantiate(blackKnightPrefab, boardManager.BoardToWorldPosition(6, 7), Quaternion.identity);
        //비숍 소환
        Instantiate(whiteBishopPrefab, boardManager.BoardToWorldPosition(2, 0), Quaternion.identity);
        Instantiate(whiteBishopPrefab, boardManager.BoardToWorldPosition(5, 0), Quaternion.identity);
        Instantiate(blackBishopPrefab, boardManager.BoardToWorldPosition(2, 7), Quaternion.identity);
        Instantiate(blackBishopPrefab, boardManager.BoardToWorldPosition(5, 7), Quaternion.identity);
        //퀸 소환
        Instantiate(whiteQueenPrefab, boardManager.BoardToWorldPosition(3, 0), Quaternion.identity);
        Instantiate(blackQueenPrefab, boardManager.BoardToWorldPosition(3, 7), Quaternion.identity);
        //킹 소환
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
                // 적 기물이면 이 칸까지는 OK
                if (IsEnemyAt(pos, self))
                    result.Add(pos);

                // 이후 칸은 전부 막힘
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
                // 기물 선택
                if (clickedPiece != null && clickedPiece.isWhite == isWhiteTurn)
                {
                    selectedPiece = clickedPiece;
                    // 하이라이트 또는 UI 처리 가능
                }
            }
            else
            {
                // 말 이동 시도
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

        // 위치 갱신
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
