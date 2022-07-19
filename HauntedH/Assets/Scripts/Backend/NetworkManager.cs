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
    public GameObject RespawnPanel;
    public GameObject Canvas;
    public Button connect;
    public Button Phantom;
    public Button Human;
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
    public void Connect() => PhotonNetwork.ConnectUsingSettings();

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.LocalPlayer.NickName = NickNameInput.text;
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 2 }, null);
    }
    public override void OnJoinedRoom()
    {
        
        DisconnectPanel.SetActive(false);
        Canvas.SetActive(true);
        if (isPhantom == false && isHuman == true) SpawnHuman();
        else SpawnPhantom();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && PhotonNetwork.IsConnected) PhotonNetwork.Disconnect();
        if (isHuman || isPhantom) connect.interactable = true;
    }
    public void SpawnHuman()
    {
        GameObject human = PhotonNetwork.Instantiate("HumanPlayer",new Vector3(-95,2,-180),Quaternion.identity);
        human.GetComponent<SC_HumanController>().enabled = true;
    }
    public void SpawnPhantom()
    {
        GameObject human = PhotonNetwork.Instantiate("PhantomPlayer", new Vector3(-95, 2, -180), Quaternion.identity);
        human.GetComponent<PhantomController>().enabled = true;
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        DisconnectPanel.SetActive(true);
        RespawnPanel.SetActive(false);
    }
}
