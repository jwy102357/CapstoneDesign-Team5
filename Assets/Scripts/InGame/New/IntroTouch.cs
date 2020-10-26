using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;

public class IntroTouch : MonoBehaviour, IPointerEnterHandler
{
    public float time;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(ES3.KeyExists("Shield","Player.es3"))
        {
            Initiate.Fade("LobbyScene", Color.black, time);
        }
        else
        {
            Initiate.Fade("Opening", Color.black, time);
        }
    }

}
