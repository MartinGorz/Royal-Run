using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    [SerializeField] GameObject chunkPrefab;
    [SerializeField] int startingChunksAmount = 12;
    [SerializeField] Transform chunkParent;
    [SerializeField] float chunkLength = 10f;
    private void Start()
    {
        for (int i = 0; i < startingChunksAmount; i++)
        {
            Vector3 chunkPosition = transform.position + new Vector3(0,0, i * chunkLength);
            Instantiate(chunkPrefab, chunkPosition, Quaternion.identity, chunkParent);
        }

    }
}
