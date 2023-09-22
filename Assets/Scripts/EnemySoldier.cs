using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoldier : MonoBehaviour
{
    public float speed = 3.0f;
    public Transform Soldier_demo ;

    void Update()
    {
        transform.LookAt(Soldier_demo);

        transform.position += transform.forward * speed * Time.deltaTime;
    }
}