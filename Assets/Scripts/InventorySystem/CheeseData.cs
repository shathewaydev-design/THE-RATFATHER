[System.Serializable]
public class CheeseData
{
    public float finalQuality;
    public string cheeseType;
    public int qualityTier;

    public CheeseData(float quality)
    {
        finalQuality = quality;

        if (quality >= 90) qualityTier = 3;
        else if (quality >= 60) qualityTier = 2;
        else qualityTier = 1;
    }
}