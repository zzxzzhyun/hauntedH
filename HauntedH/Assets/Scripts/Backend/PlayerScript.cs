using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class PlayerScript : MonoBehaviourPun, IPunObservable
{
    public Rigidbody RB;
    public Animator AN;
    public SpriteRenderer SR;
    public PhotonView PV;
    public Text NicknameText;

    bool isGround;
    Vector3 curPos;
    //위치 동기화
    
    void Awake()
    {
        NicknameText.text = PV.IsMine ? PhotonNetwork.NickName : PV.Owner.NickName;
        NicknameText.color = PV.IsMine ? Color.green : Color.red;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
    }
}
