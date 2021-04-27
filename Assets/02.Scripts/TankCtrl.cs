using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TankCtrl : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float rotationSpeed;

    private Transform tankTr;
    private PhotonView pv;
    // Start is called before the first frame update
    void Start()
    {
        tankTr = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();
        // 무게중심
        GetComponent<Rigidbody>().centerOfMass = new Vector3(0, -5.0f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(pv.IsMine)
        {
            float v = Input.GetAxis("Vertical");
            float h = Input.GetAxis("Horizontal");

            tankTr.Translate(Vector3.forward * Time.deltaTime * moveSpeed * v);
            tankTr.Rotate(Vector3.up * Time.deltaTime * rotationSpeed * h);
        }
    }
}
