using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField]
    private float cannonSpeed;

    public GameObject expEffect;

    public string shooter;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * cannonSpeed);
    }

    void OnCollisionEnter(Collision coll)
    {
            GameObject exp = Instantiate(expEffect, transform.position, Quaternion.identity);
            Destroy(exp, 1f);
    }
}
