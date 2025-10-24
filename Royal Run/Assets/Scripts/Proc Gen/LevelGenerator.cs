using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour
{

    [Header("Refrences")]
    [SerializeField] CameraController cameraController;
    [SerializeField] GameObject chunkPrefab;
    [SerializeField] Transform chunkParent;
    [SerializeField] ScoreManager scoreManager;

    [Header("Level Settings")]
    [SerializeField] int startingChunksAmount = 12;
    [Tooltip("Do not change chunklength value unless chunk prefab size reflects change.")]
    [SerializeField] float chunkLength = 10f;
    [SerializeField] float moveSpeed = 8f;
    [SerializeField] float minMoveSpeed = 2f;
    [SerializeField] float maxMoveSpeed = 20f;
    [SerializeField] float minGravityZ = -22f;
    [SerializeField] float maxGravityZ = -2f;

    //The List that contains all 12 chunks
    List<GameObject> chunks = new List<GameObject>();
    private void Start()
    {
        SpawnStartingChunks();

    }


    private void Update()
    {
        MoveChunks();
    }

    public void ChangeChunkMoveSpeed(float speedAmount)
    {
        float newMoveSpeed = moveSpeed + speedAmount;
            newMoveSpeed = Mathf.Clamp(newMoveSpeed, minMoveSpeed, maxMoveSpeed);
      

        if (newMoveSpeed != moveSpeed)
        {
            moveSpeed = newMoveSpeed;
            float newGravityZ = Physics.gravity.z - speedAmount;
            newGravityZ = Mathf.Clamp(newGravityZ, minGravityZ, maxGravityZ);
            Physics.gravity = new Vector3 (Physics.gravity.x, Physics.gravity.y, newGravityZ);
            cameraController.ChangeCameraFOV(speedAmount);
        
        }
    }


    // The main method that spawns chunks in the game.
    void SpawnStartingChunks()
    {
        for (int i = 0; i < startingChunksAmount; i++)
        {
            SpawnChunksAlgorithm(i);
        }
    }

    private void SpawnChunksAlgorithm(int i)
    {
        Vector3 chunkPosition = CalculateZPosition(i);
        GameObject newChunkGO = Instantiate(chunkPrefab, chunkPosition, Quaternion.identity, chunkParent);

        chunks.Add(newChunkGO);
        Chunk newChunk = newChunkGO.GetComponent<Chunk>();
        newChunk.Init(this, scoreManager);
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
            RemoveChunksBehind(chunk);
            if (chunks.Count < startingChunksAmount)
            {
                SpawnChunksAlgorithm(chunks.Count-1);
            }
        }
    }

    private void RemoveChunksBehind(GameObject chunk)
    {
        if (chunk.transform.position.z <= Camera.main.transform.position.z - chunkLength)
        {
            chunks.Remove(chunk);
            Destroy(chunk);

        }
    }
}
