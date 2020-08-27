using System;
using UnityEngine;

[Serializable]
public class SightBlocker
{
    [SerializeField]
    private GameObject first, second;

    public void Deacvtivate()
    {
        first.SetActive(false);
        second.SetActive(false);
    }
    public void Acvtivate()
    {
        first.SetActive(true);
        second.SetActive(true);
    }

}
