using UnityEngine;

[CreateAssetMenu(fileName = "CityGenSettings", menuName = "Gorz/City Gen Settings")]
public class CityGenSettings : ScriptableObject
{
    [Header("Grid")]
    public int rows = 4;
    public int cols = 4;
    public float blockSize = 20f;      // size of each block
    public float streetWidth = 4f;     // gap between blocks for roads

    [Header("Buildings")]
    public GameObject buildingPrefab;
    public Vector2 buildingScaleRange = new Vector2(0.8f, 1.4f);
    public int buildingsPerBlock = 6;

    [Header("Randomness")]
    public int seed = 12345;
    public bool randomizeSeed = true;

    [Header("NavMesh (optional)")]
    public bool autoBuildNavMesh = false;
}
