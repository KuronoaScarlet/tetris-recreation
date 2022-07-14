using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class Board : MonoBehaviour
{
    public Tilemap tilemap { get; private set; }
    public Piece activePiece { get; private set; }
    public TetrominoData[] tetrominoes;
    public Vector3Int spawnPos;
    public Vector2Int boardSize = new Vector2Int(10, 20);

    public TMP_Text points;
    public TMP_Text level;

    private int pieceCount = 0;

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
        pieceCount++;
        
        int random = Random.Range(0, this.tetrominoes.Length);
        TetrominoData data = this.tetrominoes[random];

        this.activePiece.Initialize(this, this.spawnPos, data);

        if(pieceCount == 10)
        {
            pieceCount = 0;
            level.text = (int.Parse(level.text) + 1).ToString();
            this.activePiece.stepDelay -= 0.1f * this.activePiece.stepDelay;
            this.activePiece.lockDelay -= 0.05f * this.activePiece.stepDelay;
        }

        if(InBounds(this.activePiece, this.spawnPos))
        {
            Set(this.activePiece);
        }
        else
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        points.text = "0";
        level.text = "1";
        this.activePiece.stepDelay = 1f;
        this.activePiece.lockDelay = 0.5f;
        this.tilemap.ClearAllTiles();
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

    public void ClearLines()
    {
        RectInt bounds = this.Bounds;
        int row = bounds.yMin;

        while(row < bounds.yMax)
        {
            if(IsLineFull(row))
            {
                LineClear(row);
            }
            else
            {
                row++;
            }
        }
    }

    private bool IsLineFull(int row)
    {
        RectInt bounds = this.Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int pos = new Vector3Int(col, row, 0);

            if(!this.tilemap.HasTile(pos))
            {
                return false;
            }
        }

        return true;
    }

    private void LineClear(int row)
    {
        RectInt bounds = this.Bounds;

        for (int col = bounds.xMin; col < bounds.xMax; col++)
        {
            Vector3Int pos = new Vector3Int(col, row, 0);
            this.tilemap.SetTile(pos, null);
        }

        while(row < bounds.yMax)
        {
            for (int col = bounds.xMin; col < bounds.xMax; col++)
            {
                Vector3Int pos = new Vector3Int(col, row + 1, 0);
                TileBase above = this.tilemap.GetTile(pos);

                pos = new Vector3Int(col, row, 0);

                this.tilemap.SetTile(pos, above);
            }

            row++;
        }

        int currentPoints = int.Parse(points.text);
        int newPoints = currentPoints + (1000 * int.Parse(level.text));
        points.text = newPoints.ToString();
    }
}

