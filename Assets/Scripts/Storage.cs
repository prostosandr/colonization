using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    private List<Crystal> _occupiedCrystals;
    private List<Crystal> _foundCrystals;

    private void Awake()
    {
        _occupiedCrystals = new();
        _foundCrystals = new();
    }

    public void SetFoundCrystals(List<Crystal> crystals)
    {
        _foundCrystals = crystals;
    }

    public Crystal GetFreeCrystal()
    {
        Crystal freeCrystal = null;

        foreach (Crystal foundCrystal in _foundCrystals)
        {
            if (_occupiedCrystals.Contains(foundCrystal) == false || _occupiedCrystals.Count == 0)
            {
                _occupiedCrystals.Add(foundCrystal);
                freeCrystal = foundCrystal;

                break;
            }
        }

        return freeCrystal;
    }

    public void RemoveOccupiedCrystal(Bot bot, Crystal crystal)
    {
        bot.Worked -= RemoveOccupiedCrystal;
        _occupiedCrystals.Remove(crystal);
    }
}