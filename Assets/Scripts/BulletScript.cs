using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public GameObject explosion;

    void OnCollisionEnter(Collision collision)
    {
        Instantiate(explosion, this.transform.position, this.transform.rotation);
        Destroy(this.gameObject);
    }

}
