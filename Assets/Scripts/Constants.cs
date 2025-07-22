using UnityEngine;

public class Constants : MonoBehaviour
{
    public static float PlayerXLimit;
    public static float PlayerYLimit;
    public const float PlayerSpeed = 5.0f;

    public static float SmallEnemyXLimit;
    public static float MiddleEnemyXLimit;
    public static float LargeEnemyXLimit;

    public static float AwardXLimit;

    public static float DestroyYLimit;
    
    public const float FrameRate = 10;

    private void Awake()
    {
        if (Camera.main == null) return;
        Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        
        PlayerXLimit = screenBounds.x - 0.8f;
        PlayerYLimit = screenBounds.y - 0.8f;
        
        SmallEnemyXLimit  = screenBounds.x - 0.8f;
        MiddleEnemyXLimit = screenBounds.x - 1.0f;
        LargeEnemyXLimit  = screenBounds.x - 1.2f;
        
        AwardXLimit = screenBounds.x - 1.0f;
        
        DestroyYLimit = screenBounds.y + 2.0f;
        
        Resolution[] resolutions = Screen.resolutions;

        foreach (var res in resolutions)
        {
            var refreshRate = res.refreshRateRatio;
            Debug.Log($"{res.width} x {res.height} @ {refreshRate.numerator / (float)refreshRate.denominator}Hz");
        }
    }
}
