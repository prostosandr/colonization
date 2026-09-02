using System;
using UnityEngine;

public class Crystal : MonoBehaviour, IPolledObject<Crystal>
{
    public event Action<Crystal> Deactivated;

    public void InvokeDeactivated()
    {
        Deactivated?.Invoke(this);
    }
}