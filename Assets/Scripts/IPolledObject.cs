using System;
using UnityEngine;

public interface IPolledObject<TItem> where TItem : MonoBehaviour
{
    public event Action<TItem> Deactivated;
}
