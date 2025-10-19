using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CityGenerator : MonoBehaviour
{
    // Keeps a list of created objects to clear later
    private readonly List<GameObject> _spawned = new();

    /// <summary>
    /// Generate a simple grid-based city using provided settings.
    /// </summary>
    public void Generate(CityGenSettings settings, Transform parent)
    {
        Clear(); // ensure clean state

        if (settings == null || settings.buildingPrefab == null)
        {
            Debug.LogWarning("CityGenerator: Missing settings or building prefab.");
            return;
        }

        if (settings.randomizeSeed)
            settings.seed = Random.Range(int.MinValue, int.MaxValue);

        var prng = new System.Random(settings.seed);

        float cell = settings.blockSize + settings.streetWidth;
        Vector3 origin = Vector3.zero;

        for (int r = 0; r < settings.rows; r++)
        {
            for (int c = 0; c < settings.cols; c++)
            {
                // Center position of current block
                Vector3 blockCenter = origin + new Vector3(c * cell, 0f, r * cell);

                // Spawn buildings around the block perimeter (simple ring)
                for (int i = 0; i < settings.buildingsPerBlock; i++)
                {
                    float t = (float)i / settings.buildingsPerBlock;
                    float angle = t * Mathf.PI * 2f;
                    float radius = settings.blockSize * 0.5f;
                    Vector3 pos = blockCenter + new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;

                    // Random scale
                    float s = Mathf.Lerp(settings.buildingScaleRange.x, settings.buildingScaleRange.y,
                                         (float)prng.NextDouble());

                    GameObject go;
#if UNITY_EDITOR
                    // Proper prefab instantiation in Editor for undo support
                    go = (GameObject)PrefabUtility.InstantiatePrefab(settings.buildingPrefab);
                    if (go == null) go = Instantiate(settings.buildingPrefab);
#else
                    go = Instantiate(settings.buildingPrefab);
#endif
                    go.transform.SetPositionAndRotation(pos, Quaternion.Euler(0f, prng.Next(0, 360), 0f));
                    go.transform.localScale = Vector3.one * s;
                    if (parent) go.transform.SetParent(parent, true);

                    go.name = $"B_{r}_{c}_{i}";
                    _spawned.Add(go);
                }
            }
        }
    }

    /// <summary>Remove previously generated objects.</summary>
    public void Clear()
    {
        for (int i = _spawned.Count - 1; i >= 0; i--)
        {
            var go = _spawned[i];
            if (!go) continue;
#if UNITY_EDITOR
            Undo.DestroyObjectImmediate(go);
#else
            Destroy(go);
#endif
        }
        _spawned.Clear();
    }
}
