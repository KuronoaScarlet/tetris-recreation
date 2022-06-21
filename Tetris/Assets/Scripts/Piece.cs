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

}
