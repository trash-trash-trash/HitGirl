using System;
using UnityEngine;

public class HeavyObject : MonoBehaviour
{
    public Rigidbody rb;

    public float currentSpeed;
    public float minSpeedToKill;

    public Sound sound;
    
    void Update()
    {
        currentSpeed = rb.linearVelocity.magnitude;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (rb.linearVelocity.magnitude > minSpeedToKill)
        {
            sound.EmitSound();
            Health hp = other.GetComponent<Health>();
            if(hp !=null)
                hp.ChangeHP(-1000000);
        }
    }
}
