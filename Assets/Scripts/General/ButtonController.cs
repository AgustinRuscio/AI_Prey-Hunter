using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour
{
    private string _mainScenedName = "Main";

    private string _menueSceneName = "Menue";

    public void PlaySimulation() => SceneManager.LoadScene(_mainScenedName);
    
    public void BackToMenue() => SceneManager.LoadScene(_menueSceneName);
    
    public void QuitAplication() => Application.Quit();
    
}