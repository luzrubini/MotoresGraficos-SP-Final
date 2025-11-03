using UnityEngine;

public class FogController : MonoBehaviour
{
    [Header("Configuración de la neblina")]
    public Color fogColor = Color.gray;     
    public float minDensity = 0.2f;       
    public float maxDensity = 1f;         
    public float densityStep = 0.01f;          

    void Start()
    {
        RenderSettings.fog = true;
        RenderSettings.fogMode = FogMode.Exponential;
        RenderSettings.fogColor = fogColor;
        RenderSettings.fogDensity = minDensity;
    }

    public void IncreaseDensity()
    {
        RenderSettings.fogDensity += densityStep;
        RenderSettings.fogDensity = Mathf.Clamp(RenderSettings.fogDensity, minDensity, maxDensity);
        Debug.Log("Neblina aumentada a: " + RenderSettings.fogDensity);
    }

    public void DecreaseDensity()
    {
        RenderSettings.fogDensity -= densityStep;
        RenderSettings.fogDensity = Mathf.Clamp(RenderSettings.fogDensity, minDensity, maxDensity);
        Debug.Log("Neblina disminuida a: " + RenderSettings.fogDensity);
    }
    public void ResetFog()
    {
        RenderSettings.fogDensity = minDensity;
    }
}
