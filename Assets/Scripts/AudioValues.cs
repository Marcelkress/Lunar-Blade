public class AudioValues
{
    public static AudioValues instance;

    public static void CreateInstance()
    {
        if (instance == null)
        {
            instance = new AudioValues();
        }
    }
    
    public float musicVol = 10, ambienceVol = 10, CharVol = 10;
}