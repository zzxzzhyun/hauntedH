using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class JobClick : MonoBehaviour
{
    [SerializeField] Button phantom;
    [SerializeField] Button human;
    // Start is called before the first frame update
    public void Phantom()
    {
        phantom.interactable = false;
        human.interactable = true;
        GameMgr.isPlayer = true;
        GameMgr.isPhantom = false;
    }
    public void Human()
    {
        phantom.interactable = true;
        human.interactable = false;
        GameMgr.isPlayer = false;
        GameMgr.isPhantom = true;
    }
    
}
