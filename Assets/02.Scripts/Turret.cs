using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Turret : MonoBehaviour
{
    private PhotonView pv;
    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        // 포톤뷰가 내 것이 아니면 아무것도 처리하지 않는다.
        this.enabled = pv.IsMine;
    }

    // Update is called once per frame
    void Update()
    {
        float r = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up * Time.deltaTime * 200.0f * r);
    }
}
