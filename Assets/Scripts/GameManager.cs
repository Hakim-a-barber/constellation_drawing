﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] string nextSceneName = "";

    [SerializeField] AudioSource starSound = null;
    [SerializeField] AudioSource relicSound = null;
    // Start is called before the first frame update
    void Start()
    {
        //potentially swap out the reliccollection events for one that uses the board if using that for implementing relic collection
        RelicCollection.OnRelicCollected += PlayRelicSound;
        RelicCollection.OnAllRelicsCollected += LoadNextScene;
        StarManager.OnStarConnection += PlayStarSound;
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
    void PlayRelicSound(){
        PlayOnce(relicSound);
    }
    void PlayStarSound(){
        PlayOnce(starSound);
    }
    public void PlayOnce(AudioSource source){
        if(source == null){
            return;
        }
        source.loop = false;
        source.Play();
    }

}