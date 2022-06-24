using UnityEngine;
using UnityEngine.Tilemaps;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public Piece activePiece { get; private set; }
    public TetrominoData[] tetrominoes;
    public Vector3Int spawnPos;
    public Vector2Int boardSize = new Vector2Int(10, 20);

    public RectInt Bounds
    { 
        get 
        { Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2); 
            return new RectInt(position, this.boardSize); 
        } 
    }

    private void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();

        for (int i = 0; i < this.tetrominoes.Length; i++)
        {
            this.tetrominoes[i].Initialize();
        }

    }

    private void Start()
    {
        SpawnPiece();
    }

    public void SpawnPiece()
    {
        int random = Random.Range(0, this.tetrominoes.Length);
        TetrominoData data = this.tetrominoes[random];

        this.activePiece.Initialize(this, this.spawnPos, data);
        Set(this.activePiece);
    }

    public void Set(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePos = piece.cells[i] + piece.pos;
            this.tilemap.SetTile(tilePos, piece.data.tile);
        }
    }

    public void Clear(Piece piece)
    {
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePos = piece.cells[i] + piece.pos;
            this.tilemap.SetTile(tilePos, null);
        }
    }

    public bool InBounds(Piece piece, Vector3Int pos)
    {
        RectInt bounds = this.Bounds;

        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + pos;

            if (!bounds.Contains((Vector2Int)tilePosition)) return false;
            if (this.tilemap.HasTile(tilePosition)) return false;

        }

        return true;
    }
}
