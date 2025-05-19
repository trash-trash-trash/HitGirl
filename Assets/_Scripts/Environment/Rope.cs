using UnityEngine;

public class Rope : Health
{
    public Joint joint;
    public void Awake()
    {
        joint = GetComponent<Joint>();
        ChangeHP(1);
    }
    
    public override void ChangeHP(int input)
    {
        base.ChangeHP(input);
        if(CurrentHP <= 0)
            Destroy(joint);
    }
}
