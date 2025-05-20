using System;
using UnityEngine;

public class HeavyObject : MonoBehaviour, ICharacter
{
    public Rigidbody rb;

    public float currentSpeed;
    public float minSpeedToKill;

    public Sound sound;

    public float shoveForce;
    
    private Collider triggerCollider;
    
    [SerializeField] private LayerMask ignoredLayers;
    
    public Health[] ropes;

    public FixedJoint fixedJoint;
    
    public bool broken = false;

    void Awake()
    {
        character = Character.Object;
        fixedJoint = GetComponentInParent<FixedJoint>();
        triggerCollider = GetComponent<Collider>();
        ropes = GetComponentsInParent<Health>();
        foreach (Health hp in ropes)
        {
            hp.AnnounceHP += RopeBroken;
        }
    }

    private void RopeBroken(int obj)
    {
        if (broken)
            return;

        if (obj == 0)
        {
            broken = true;
            Destroy(fixedJoint);
        }
    }

    void Update()
    {
        currentSpeed = rb.linearVelocity.magnitude;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & ignoredLayers) != 0)
            return;

        if (rb.linearVelocity.magnitude > minSpeedToKill)
        {  
            //failsafe for other things triggering kill (ie, ropes)
            float crushRadius = Mathf.Max(triggerCollider.bounds.extents.x, triggerCollider.bounds.extents.z);
            float distance = Vector3.Distance(triggerCollider.bounds.center, other.ClosestPoint(triggerCollider.bounds.center));
            if (distance > crushRadius)
                return; 
            
            if(sound!=null)
                sound.EmitSound(cb);
            
            Health hp = other.GetComponent<Health>();
            if (hp == null)
                hp = other.GetComponentInParent<Health>();
            if (hp != null)
            {
                hp.ChangeHP(-1000000);
                Debug.Log("Crushed "+other.gameObject.name);
            }

            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb == null)
                rb = other.GetComponentInParent<Rigidbody>();
            if (rb != null)
            {
                Vector3 direction = (rb.position - transform.position).normalized;
                Vector3 shoveVelocity = direction * shoveForce;
                rb.linearVelocity = shoveVelocity;
            }
        }
    }

    public Character character;
    public CharacterBase cb;
    
    public Character ReturnCharacter()
    {
        return character;
    }

    public CharacterBase ReturnCharacterBase()
    {
        return cb;
    }
}
