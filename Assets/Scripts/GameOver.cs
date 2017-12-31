using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour {

  public void LoadScene(){
    SceneManager.LoadScene(1); //loads AndreScene which is the 2nd scene in the build project
  }
  public void BackToMenu(){
    SceneManager.LoadScene(2);
  }
}
