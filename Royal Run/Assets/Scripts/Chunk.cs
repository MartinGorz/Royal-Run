using UnityEngine;

public class Chunk : MonoBehaviour
{

    [SerializeField] GameObject fencePrefab;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnFence();

    }

    void SpawnFence()
    {
        //Vector3 spawnPosition = new Vector3();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
