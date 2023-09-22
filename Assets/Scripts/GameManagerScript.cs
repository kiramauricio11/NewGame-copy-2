using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject lamp;

    void Start()
    {
        // Instantiate at a position and rotation.
        Instantiate(lamp, new Vector3(5, 2.5f, -5), Quaternion.identity);
        Instantiate(lamp, new Vector3(-5, 2.5f, -5), Quaternion.identity);
    }

    void Update()
    {
    }
}
