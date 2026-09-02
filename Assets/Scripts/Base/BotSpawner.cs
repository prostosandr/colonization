using UnityEngine;

public class BotSpawner : Spawner<Bot>
{
    [SerializeField] private Transform _base;

    protected override Vector3 GetSpawnPoint()
    {
        return _base.position;
    }
}
