using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class Chunk : MonoBehaviour
{

    [SerializeField] GameObject fencePrefab;
    [SerializeField] float[] lanes = { -4.19f, -1.94f, 0.72f };


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnFence();

    }

    void SpawnFence()
    {
        List<int> availableLanes = new List<int> { 0, 1, 2 };
        int fencesToSpawn = Random.Range(0, 3);


        for (int i = 0; i < fencesToSpawn; i++)
        {
            int randomLaneIndex = Random.Range(0, availableLanes.Count);
            int selectedLane = availableLanes[randomLaneIndex];
            availableLanes.RemoveAt(randomLaneIndex);

            Vector3 spawnPosition = new Vector3(lanes[selectedLane], transform.position.y, transform.position.z);
            Instantiate(fencePrefab, spawnPosition, Quaternion.identity, this.transform);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
