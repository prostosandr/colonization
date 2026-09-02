using System.Collections;
using UnityEngine;

public class CrystalSpawner : Spawner<Crystal>
{
    [SerializeField] private Ground _ground;
    [SerializeField] private float _xOffset;
    [SerializeField] private float _yOffset;
    [SerializeField] private float _zOffset;
    [SerializeField] private int _delay;

    private  void Start()
    {
        var wait = new WaitForSeconds(_delay);
        StartCoroutine(GenerateItem(wait));
    }

    protected override Vector3 GetSpawnPoint()
    {
        float randomX = Random.Range(_ground.Bounds.min.x + _xOffset, _ground.Bounds.max.x - _xOffset);
        float randomZ = Random.Range(_ground.Bounds.min.z + _zOffset, _ground.Bounds.max.z - _zOffset);

        return new Vector3(randomX, _ground.transform.position.y + _yOffset, randomZ);
    }

    private IEnumerator GenerateItem(WaitForSeconds wait)
    {
        while (enabled)
        {
            Spawn();

            yield return wait;
        }
    }
}
