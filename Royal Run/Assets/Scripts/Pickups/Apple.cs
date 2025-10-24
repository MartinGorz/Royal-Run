using UnityEngine;

public class Apple : Pickup
{
    [SerializeField] float appleSpeedUp = 3f;
    LevelGenerator levelGenerator;


    public void Init(LevelGenerator levelGenerator)
    {
        this.levelGenerator = levelGenerator;
    }

    protected override void OnPickup()
    {
        levelGenerator.ChangeChunkMoveSpeed(appleSpeedUp);
    }
}
