using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class JobClick : MonoBehaviour
{
    public GameObject humanorphantom;
    public Button human;
    public Button phantom;
    public void ClickBtn()
    {
        GameObject humanorphantom = EventSystem.current.currentSelectedGameObject;
        if(humanorphantom.name == "Phantom")
        {
            phantom.GetComponent<Button>().interactable = false;
            human.GetComponent<Button>().interactable = true;
        }
        else
        {
            phantom.GetComponent<Button>().interactable = true;
            human.GetComponent<Button>().interactable = false;
        }
        DontDestroyOnLoad(humanorphantom);
    }
    
}
