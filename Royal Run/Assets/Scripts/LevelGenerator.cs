using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{

    [SerializeField] GameObject chunkPrefab;
    [SerializeField] int startingChunksAmount = 12;
    [SerializeField] Transform chunkParent;
    [SerializeField] float chunkLength = 10f;
    [SerializeField] float moveSpeed = 8f;


    //The List that contains all 12 chunks
    List<GameObject> chunks = new List<GameObject>();
    private void Start()
    {
        SpawnChunks();

    }


    private void Update()
    {
        MoveChunks();
    }
    // The main method that spawns chunks in the game.
    void SpawnChunks()
    {
        for (int i = 0; i < startingChunksAmount; i++)
        {
            Vector3 chunkPosition = CalculateZPosition(i);
            GameObject newChunk = Instantiate(chunkPrefab, chunkPosition, Quaternion.identity, chunkParent);
            chunks.Add(newChunk);
        }
    }

    //To calcuate Z position for the chunks, so we can line them up infront of the player.
    Vector3 CalculateZPosition(int i)
    {
        return transform.position + new Vector3(0, 0, i * chunkLength);
    }

    void MoveChunks()
    {
        for (int i = 0; i < chunks.Count; i++)
        {
            GameObject chunk = chunks[i];
            chunks[i].transform.Translate(0, 0, -1 * (moveSpeed * Time.deltaTime));
            if (chunk.transform.position.z <= Camera.main.transform.position.z - chunkLength)
            {
                chunks.Remove(chunk);
                Destroy(chunk);
            }
        }
    }
}
