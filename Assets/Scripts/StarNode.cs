using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarNode : MonoBehaviour
{
    [SerializeField] Color drawableColor = Color.yellow;
    Color initialColor;

    SpriteRenderer sr;

    List<StarNode> connections = new List<StarNode>();
    bool drawable = false;

    StarManager starManager;
    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        initialColor = sr.color;
    }
    //To do: use tags or something else to find star manager instead
    void Start()
    {
        starManager = FindObjectOfType<StarManager>();
        starManager.AddStar(this);
    }

    public List<StarNode> GetConnections(){
        return new List<StarNode>(connections);
    }

    public void ClearConnections(){
        connections.Clear();
    }
    public void AddConnection(StarNode node){
        //no duplicates
        if(connections.Contains(node)){
            return;
        }
        connections.Add(node);
    }
    public bool CanBuildConnection(StarNode node){
        return node != this && !connections.Contains(node);
    }

    //To do: potentially change to work w/ phones or some sort of input handler
    void OnMouseDown()
    {
        if(drawable) starManager.TryConnecting(this);
    }

    public void SetDrawable(bool canDraw){
        drawable = canDraw;
        sr.color = drawable ? drawableColor : initialColor;
    }
}
