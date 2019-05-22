using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class SROptions
{
    public int ComPort { get { return PlayerPrefs.GetInt(nameof(ComPort), 5); } set { PlayerPrefs.SetInt(nameof(ComPort), value); } }
    public int Port { get { return PlayerPrefs.GetInt(nameof(Port), 4649); } set { PlayerPrefs.SetInt(nameof(Port), value); } }
    public float FadeTime { get { return PlayerPrefs.GetFloat(nameof(FadeTime), 1); } set { PlayerPrefs.SetFloat(nameof(FadeTime), value); } }

    public void ReloadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    public void TriggerRandom()
    {
        DMXManager.TriggerRandom();
    }

    public void TriggerClear()
    {
        DMXManager.TriggerClear();
    }
}
