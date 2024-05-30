using UnityEngine;
using Lofelt.NiceVibrations;
public class HapticManager : MonoBehaviour
{
    public static HapticManager instance;
    //   For click/pick any object
    float Amplitude = 0.3f;
    float Duration = 0.02f;
    //Type = Light Impact

    //For collection/park/place etc
    float C_Amplitude = 0.5f;
    float C_Duration = 0.017f;
    //Type = Light Impact.
    float frequency = 1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


        
    }
    public void click()
    {
        if (PlayerPrefs.GetInt(GameConstants.Vibration) == 1)
        {
            HapticController.fallbackPreset = HapticPatterns.PresetType.LightImpact;
            HapticPatterns.PlayConstant(Amplitude, frequency, Duration);
        }
    }
    public void Connect()
    {
        if (PlayerPrefs.GetInt(GameConstants.Vibration) == 1)
        {
            HapticController.fallbackPreset = HapticPatterns.PresetType.LightImpact;
            HapticPatterns.PlayConstant(C_Amplitude, frequency, C_Duration);
        }
    }

    //SoundManager.instance?.TriggerHapticFeedback(0.5f, 1f, 0.017f); //On Item Drop
}
