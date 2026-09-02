using UnityEngine;

public class BotMover : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _minDistance;

    public void Move(Vector3 target)
    {
        transform.LookAt(target);
        transform.position = Vector3.MoveTowards(transform.position, target, _speed * Time.deltaTime);
    }

    public bool CanMove(Vector3 target)
    {
        return (Vector3.Distance(transform.position, target) > _minDistance);
    }
}