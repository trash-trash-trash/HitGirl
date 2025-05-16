using System.Collections.Generic;
using UnityEngine;

public class PatrolPaths : MonoBehaviour
{
    public PatrolPoint idlePoint;

    public List<PatrolPoint> idlePatrolPoints = new List<PatrolPoint>();
    public List<PatrolPoint> fleePatrolPoints = new List<PatrolPoint>();
}
