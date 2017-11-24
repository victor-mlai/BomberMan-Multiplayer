using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUIBehaviour : MonoBehaviour
{
    public Slider slider;
    public string player1Name;
    public string player2Name;

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

    public void LoadScene(string mapName)
    {
        StartCoroutine(LoadSceneInBackground(mapName));
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator LoadSceneInBackground(string mapName)
    {
        AsyncOperation sceneLoadOperation = SceneManager.LoadSceneAsync(mapName);

        while (! sceneLoadOperation.isDone)
        {
            slider.value = Mathf.Clamp01(sceneLoadOperation.progress * 1.1f);
            
            yield return null;
        }
    }

    public void SetPlayer1Name(string name)
    {
        player1Name = name;
    }

    public void SetPlayer2Name(string name)
    {
        player2Name = name;
    }
}