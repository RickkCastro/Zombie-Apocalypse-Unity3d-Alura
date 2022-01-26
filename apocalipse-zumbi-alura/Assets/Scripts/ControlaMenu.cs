using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlaMenu : MonoBehaviour
{
    public GameObject PanelCredits;

    private void Start()
    {
        #if UNITY_2017_1_OR_NEWER || UNITY_EDITOR
        //Debug.Log("IS STANDALONE");
        #endif
        StopAllCoroutines();
    }

    public void PlayGame() => SceneManager.LoadScene("Game");

    public void PlayGameHellMode() => SceneManager.LoadScene("HellMode");

    public void OpenCredits() => PanelCredits.SetActive(true);

    public void CloseCredits() => PanelCredits.SetActive(false);


}
