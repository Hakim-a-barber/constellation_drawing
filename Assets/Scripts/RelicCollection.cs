using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public delegate void Notify();
//put script on player to collect relics or just ignore/delete if using grid to check if player is over a relic
public class RelicCollection : MonoBehaviour
{
    //events for sound/switching scenes or w/e else
    public static event Notify OnAllRelicsCollected;
    public static event Notify OnRelicCollected;
    HashSet<GameObject> relics;
    
    void Start()
    {
        relics = new HashSet<GameObject>(GameObject.FindGameObjectsWithTag("Relic"));
    }
    //replace w/ OnColliderEnter2D or w/e if neither the player or relic are a trigger
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "Relic"){
            relics.Remove(other.gameObject);
            other.gameObject.SetActive(false);
            OnRelicCollected?.Invoke();
            if(relics.Count == 0){
                OnAllRelicsCollected?.Invoke();
            }
        }
    }
}
