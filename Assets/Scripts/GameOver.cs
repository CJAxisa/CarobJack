using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

  public void LoadScene(){
    SceneManager.LoadScene("andreScene", LoadSceneMode.Single); //loads AndreScene which is the 2nd scene in the build project
  }
  public void BackToMenu(){
    SceneManager.LoadScene("mainMenu", LoadSceneMode.Single);
  }
}
