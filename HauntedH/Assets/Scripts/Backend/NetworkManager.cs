using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class NetworkManager : MonoBehaviourPunCallbacks
{
    public InputField NickNameInput;
    public GameObject DisconnectPanel;
    public GameObject HumanCanvas;
    public GameObject PhantomCanvas;
    public Button connect;
    public Button Phantom;
    public Button Human;
    public Camera humancam;
    public Camera phantomcam;
    public Camera MainCam;
    public PhotonView PV;
    private bool isHuman;
    private bool isPhantom;
    private void Awake()
    {
        Screen.SetResolution(960, 540, false);
        PhotonNetwork.SendRate = 60;
        PhotonNetwork.SerializationRate = 30;
    }
    public void Start()
    {
        connect.interactable = false;
        isHuman = false;
        isPhantom = false;
    }
    public void selecthhuman()
    {
        isHuman = true;
        isPhantom = false;
        Phantom.interactable = true;
        Human.interactable =false;
    }
    public void selectphantom()
    {
        isHuman = false;
        isPhantom = true;
        Phantom.interactable = false;
        Human.interactable = true;
    }
    public void Connect() { 
        MainCam.gameObject.SetActive(false);
        PhotonNetwork.ConnectUsingSettings();

    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 2 }, null);
    }
    public override void OnJoinedRoom()
    {
        DisconnectPanel.SetActive(false);
        phantomcam.enabled=false;
        humancam.enabled=false;
        //Canvas.SetActive(true);
        if (isPhantom == false && isHuman == true) {
            SpawnHuman(); 
            }
        else SpawnPhantom();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && PhotonNetwork.IsConnected) PhotonNetwork.Disconnect();
        if (isHuman || isPhantom) connect.interactable = true;
    }
    public void SpawnHuman()
    {
        GameObject human = PhotonNetwork.Instantiate("HumanPlayer",new Vector3(-95,10,-180),Quaternion.identity);
        human.GetComponent<SC_HumanController>().enabled = true;
        HumanCanvas.SetActive(true);
        humancam.enabled=true;
        //if (PV.IsMine){human.GetComponentInChildren<Camera>().enabled = true;}
        
    }
    public void SpawnPhantom()
    {
        GameObject phantom = PhotonNetwork.Instantiate("PhantomPlayer", new Vector3(-100, 50, -180), Quaternion.identity);
        phantom.GetComponent<PhantomController>().enabled = true;
        PhantomCanvas.SetActive(true);
        phantomcam.enabled=true;
        //if (PV.IsMine){phantom.GetComponentInChildren<Camera>().enabled = true;}

    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        DisconnectPanel.SetActive(true);
    }
}
