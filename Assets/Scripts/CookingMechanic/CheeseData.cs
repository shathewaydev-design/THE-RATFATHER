[System.Serializable]
public class CheeseData
{
    public float finalStability;
    public string cheeseType;
    public int qualityTier;

    public CheeseData(float stability)
    {
        finalStability = stability;

        if (stability >= 90) qualityTier = 3;
        else if (stability >= 60) qualityTier = 2;
        else qualityTier = 1;
    }
}