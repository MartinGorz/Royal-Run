using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    [SerializeField] float checkpointTimeExtension = 5f;

    GameManager gameManager;
    const string playerString = "Player";
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();    
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerString)){
            gameManager.IncreaseTime(checkpointTimeExtension);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
