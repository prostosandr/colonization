using UnityEngine;

public class CrystalPicker : MonoBehaviour
{
    public void PickUpCrystal(Crystal crystal)
    {
        crystal.transform.parent = transform;

        crystal.transform.position = transform.position;
    }
}
