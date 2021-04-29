using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityStandardAssets.Utility;
using TMPro;

public class TankCtrl : MonoBehaviour, IPunObservable
{
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private float rotationSpeed;
    [SerializeField]
    private float cannonRoatationSpeed;

    // 필요 컴포넌트
    private Transform tankTr;
    private PhotonView pv;
    public GameObject cannonPrefab;

    [SerializeField]
    private Transform firePos;

    public Transform cannonMesh;
    
    private new AudioSource audio;
    public AudioClip cannonFireSfx;

    public TMPro.TMP_Text userIdText;

    // Start is called before the first frame update
    void Start()
    {
        tankTr = GetComponent<Transform>();
        pv = GetComponent<PhotonView>();
        audio = GetComponent<AudioSource>();

        userIdText.text = pv.Owner.NickName;

        // 무게중심
        if(pv.IsMine)
        {
            Camera.main.GetComponent<SmoothFollow>().target = transform.Find("CamPivot").transform;
            GetComponent<Rigidbody>().centerOfMass = new Vector3(0, -5.0f, 0);
        }
        else
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(pv.IsMine)
        {
            float v = Input.GetAxis("Vertical");
            float h = Input.GetAxis("Horizontal");

            //이동 및 회전
            tankTr.Translate(Vector3.forward * Time.deltaTime * moveSpeed * v);
            tankTr.Rotate(Vector3.up * Time.deltaTime * rotationSpeed * h);

            // 포탄 발사 로직
            if(Input.GetMouseButtonDown(0))
            {
                pv.RPC("Fire", RpcTarget.AllViaServer, pv.Owner.NickName);
            }

            // 포신 회전
            float r = Input.GetAxis("Mouse ScrollWheel");
            cannonMesh.Rotate(Vector3.right * Time.deltaTime * r * cannonRoatationSpeed);
        }
        else
        {
            if((tankTr.position - receivePos).sqrMagnitude > 3.0f * 3.0f)
            {
                tankTr.position = receivePos;
            }
            else
            {
                tankTr.position = Vector3.Lerp(tankTr.position, receivePos, Time.deltaTime * 10f);
            }
            tankTr.rotation = Quaternion.Slerp(tankTr.rotation, receiveRot, Time.deltaTime * 10f);
        }
    }


    [PunRPC]
    void Fire(string shooterName)
    {
        GameObject _cannon =  Instantiate(cannonPrefab, firePos.position, firePos.rotation);
        audio?.PlayOneShot(cannonFireSfx); // 혹은 Awake영역으로 올릴 것
        _cannon.GetComponent<Cannon>().shooter = shooterName;
    }

    Vector3 receivePos = Vector3.zero;
    Quaternion receiveRot = Quaternion.identity; 

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting) // PhotonView.IsMine == true;
        {
            stream.SendNext(tankTr.position);   // 위치
            stream.SendNext(tankTr.rotation);   // 회전값
        }
        else
        {
            // 두번 보내서 두번 받음
            receivePos = (Vector3)stream.ReceiveNext();
            receiveRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
