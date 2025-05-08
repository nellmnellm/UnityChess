using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public int width = 8;
    public int height = 8;

    public Vector3 BoardToWorldPosition(int x, int y)
    {
        float tileSize = 1.5f;
        float originX = -5.25f;
        float originY = -5.25f;

        return new Vector3(x * tileSize + originX, 0.001f, y * tileSize + originY);
    }

    public Vector3 BoardToWorldPosition(Vector2Int pos)
    {
        return BoardToWorldPosition(pos.x, pos.y);
    }


    void Start()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = BoardToWorldPosition(x, y);
                GameObject tile = Instantiate(tilePrefab, pos, Quaternion.identity, transform);
                tile.transform.rotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));
                // 색상 번갈아가며 지정
                Renderer rend = tile.GetComponent<Renderer>();
                bool isWhite = (x + y) % 2 == 0;
                rend.material.color = isWhite ? Color.white : Color.gray;
            }
        }
    }
}
