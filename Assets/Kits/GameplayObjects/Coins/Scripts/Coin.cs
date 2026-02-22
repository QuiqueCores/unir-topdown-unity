using UnityEngine;

public class Coin : MonoBehaviour, IVisible2D
{
    public void SetSide(IVisible2D.Side side)
    {
        throw new System.NotImplementedException();
    }

    int IVisible2D.GetPriority()
    {
        return 0;
    }

    IVisible2D.Side IVisible2D.GetSide()
    {
        return IVisible2D.Side.Neutrals;
    }


}
