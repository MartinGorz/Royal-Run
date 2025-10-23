using UnityEngine;

public class Apple : Pickup
{
    [SerializeField] float appleSpeedUp = 3f;
    LevelGenerator levelGenerator;


    private void Start()
    {
        levelGenerator = FindFirstObjectByType<LevelGenerator>();
    }

    protected override void OnPickup()
    {
        levelGenerator.ChangeChunkMoveSpeed(appleSpeedUp);
    }
}
