public class HelperModel
{
    public int PosX;
    public int PosZ;
    public int ForX;
    public int ForZ;
    public int ResultX;
    public int ResultZ;
    public string NoneLayer;
    public string CurrentLayer;

    public HelperModel(int posX, int posZ, string noneLayer, string currentLayer)
    {
        this.PosX = posX;
        this.PosZ = posZ;
        this.NoneLayer = noneLayer;
        this.CurrentLayer = currentLayer;
    }

    public int GetX()
    {
        return PosX + ForX;
    }

    public int GetZ()
    {
        return PosZ + ForZ;
    }

    public bool isBorder(int x, int z)
    {
        return (x * ForX > ResultX) || (z * ForZ > ResultZ);
    }
}