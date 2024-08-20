using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectileLife = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, projectileLife);
    }

    private void OnCollisionEnter(Collision Collision)
    {
        Destroy(gameObject);
    }
}