using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private float _scanDistance;

    public List<Crystal> GetScanObjects()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, _scanDistance);

        List<Crystal> crystals;
        crystals = new();

        foreach (Collider hit in hits)
        {
            if (hit.TryGetComponent(out Crystal crystal))
                crystals.Add(crystal);
        }

        return crystals;
    }
}
