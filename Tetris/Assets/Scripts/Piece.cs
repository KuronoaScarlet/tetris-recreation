using UnityEngine;

public class Piece : MonoBehaviour
{
    public Board board { get; private set; }
    public Vector3Int pos { get; private set; }
    public TetrominoData data { get; private set; }
    public Vector3Int[] cells { get; private set; }

    public void Initialize(Board board, Vector3Int pos, TetrominoData data)
    {
        this.board = board;
        this.pos = pos;
        this.data = data;

        if(this.cells == null)
        {
            this.cells = new Vector3Int[data.cells.Length];
        }

        for (int i = 0; i < data.cells.Length; i++)
        {
            this.cells[i] = (Vector3Int)data.cells[i];
        }

    }

    private void Update()
    {
        this.board.Clear(this);

        if(Input.GetKeyDown(KeyCode.A))
        {
            Move(Vector2Int.left);
        }
        else if(Input.GetKeyDown(KeyCode.D))
        {
            Move(Vector2Int.right);
        }

        if(Input.GetKeyDown(KeyCode.S))
        {
            Move(Vector2Int.down);
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            HardDrop();
        }

        this.board.Set(this);
    }

    private void HardDrop()
    {
        while(Move(Vector2Int.down))
        {
            continue;
        }
    }

    private bool Move(Vector2Int translation)
    {
        Vector3Int newPos = this.pos;

        newPos.x += translation.x;
        newPos.y += translation.y;

        bool valid = this.board.InBounds(this, newPos);

        if(valid)
        {
            this.pos = newPos;
        }

        return valid;
    }
}
