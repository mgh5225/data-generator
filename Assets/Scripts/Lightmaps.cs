using UnityEditor;
using UnityEngine;

[System.Serializable]
public class Lightmaps
{
    [SerializeField]
    private Texture2D[] _lightmaps;
    [SerializeField]
    private LightProbes _lightProbes;

    [HideInInspector]
    public bool isActive = false;

    public void ChangeLightMap()
    {
        LightmapSettings.lightmaps = _convertTexturesToLightmapData(_lightmaps);
        LightmapSettings.lightProbes = _lightProbes;
        isActive = true;
    }
    private LightmapData[] _convertTexturesToLightmapData(Texture2D[] textures)
    {
        LightmapData[] lightmapDataArray = new LightmapData[textures.Length];
        for (int i = 0; i < textures.Length; i++)
        {
            lightmapDataArray[i] = new LightmapData();
            lightmapDataArray[i].lightmapColor = textures[i];
        }
        return lightmapDataArray;
    }
}