using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform spawnTransform;
    public float force = 500;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            GameObject newProjectile = Instantiate(projectilePrefab, spawnTransform.position, spawnTransform.rotation);
            newProjectile.GetComponent<Rigidbody>().AddForce(newProjectile.transform.forward * force);
        }
    }
}
