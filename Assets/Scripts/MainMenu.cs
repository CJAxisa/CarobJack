using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

  public void LoadScene(){
    SceneManager.LoadScene("andreScene", LoadSceneMode.Single); //loads AndreScene which is the 2nd scene in the build project
  }
  public void QuitGame(){
    //Debug.Log("please work");
    Application.Quit();
  }
}
