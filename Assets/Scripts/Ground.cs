using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Ground : MonoBehaviour
{
    private Collider _collider;
    private Bounds _bounds;

    public Bounds Bounds => _bounds;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _bounds = _collider.bounds;
    }
}
