using UnityEngine;

public class SpawnModel
{
    public int PosX;
    public int PosZ;
    public string None;
    public string Next;
    public string Current;
    public bool IsBlack;
    public GameObject Chip;
    public GameObject ChipParent;
    public Vector3 Position;

    public SpawnModel(GameObject chipParent, Vector3 position, int posX, int posZ, string noneLayer)
    {
        this.Position = position;
        this.ChipParent = chipParent;
        this.PosX = posX;
        this.PosZ = posZ;
        this.None = noneLayer;
    }
}