using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] string nextSceneName = "";
    // Start is called before the first frame update
    void Start()
    {
        RelicCollection.OnAllRelicsCollected += LoadNextScene;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadNextScene(){
        if(nextSceneName == ""){
            Debug.Log("No scene set");
            return;
        }
        SceneManager.LoadScene(nextSceneName);
    }

}
