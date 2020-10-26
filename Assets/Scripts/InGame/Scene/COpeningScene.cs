using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COpeningScene : MonoBehaviour
{
    public float time = 0.5f;

    void End()
    {
        Initiate.Fade("LobbyScene", Color.black, time);
    }

    public void TouchSkip()
    {
        this.End();
    }
}
