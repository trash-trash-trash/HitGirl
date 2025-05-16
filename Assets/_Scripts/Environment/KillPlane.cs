using System;
using UnityEngine;

public class KillPlane : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        Health HP = other.GetComponent<Health>();
        if (HP!=null)
        {
            HP.ChangeHP(-1000000);
        }
    }
}
