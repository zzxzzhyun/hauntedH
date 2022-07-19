using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class ToLobby : MonoBehaviour
{
    [SerializeField] Button phantom;
    [SerializeField] Button human;
    [SerializeField] TextMeshProUGUI alerttext;
    [SerializeField] Button start;
   
    public void Lobby()
    {
        if (alerttext.text.Contains("Welcome"))
        {
            SceneManager.LoadScene("SampleScreen");
        }
    }
}
