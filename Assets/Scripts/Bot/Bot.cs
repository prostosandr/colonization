using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BotMover))]
[RequireComponent(typeof(CrystalPicker))]
public class Bot : MonoBehaviour, IPolledObject<Bot>
{
    private BotMover _mover;
    private CrystalPicker _picker;
    private Vector3 _startPosition;
    private bool _isWork;

    public bool IsWork => _isWork;

    public event Action<Bot> Deactivated;
    public event Action<Bot, Crystal> Worked;
    public event Action<Vector3, Bot> BuildComleted;

    private void Awake()
    {
        _mover = GetComponent<BotMover>();
        _picker = GetComponent<CrystalPicker>();
    }

    public void SetWork(Crystal crystal, Vector3 startPostion)
    {
        _isWork = true;
        _startPosition = startPostion;

        StartCoroutine(Work(crystal));
    }

    public void SetBuild(Vector3 buildPosition)
    {
        StartCoroutine(Build(buildPosition));
    }

    private IEnumerator Build(Vector3 buildPosition)
    {
        while (_mover.CanMove(buildPosition))
        {
            _mover.Move(buildPosition);

            yield return null;
        }

        BuildComleted?.Invoke(buildPosition, this);
    }

    private IEnumerator Work(Crystal crystal)
    {
        while (_mover.CanMove(crystal.transform.position))
        {
            _mover.Move(crystal.transform.position);

            yield return null;
        }

        _picker.PickUpCrystal(crystal);

        while (_mover.CanMove(_startPosition))
        {
            _mover.Move(_startPosition);

            yield return null;
        }

        _isWork = false;

        crystal.InvokeDeactivated();

        Worked?.Invoke(this, crystal);
    }
}