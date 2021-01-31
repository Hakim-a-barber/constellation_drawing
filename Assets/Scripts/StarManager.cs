using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public delegate void CheckBounds(Bounds bounds, StarManager manager);
public class StarManager : MonoBehaviour
{
    [SerializeField] StarNode rootStar = null;

    [SerializeField] LineRenderer lineRendererPrefab;

    [SerializeField] float maxLineDist = 10.0f;

    Camera cam;
    List<GameObject> lineRendererObjects = new List<GameObject>();

    List<StarNode> stars = new List<StarNode>();

    LineRenderer lineRenderer = null;


    public static event Notify OnStarConnection; 

    private Board board;


    void Start()
    {
        cam = Camera.main;
        rootStar.SetDrawable(true);

        board = GetComponent<Board>();
    }

    public void AddStar(StarNode star){
        stars.Add(star);
    }
    
    public void BuildConnection(StarNode starA, StarNode starB){
        starA.AddConnection(starB);
        starB.AddConnection(starA);
    }

    //goes through all of the stars and gets rid of all the connections
    public void ClearAllConnections(){
        foreach(GameObject obj in lineRendererObjects){
            Destroy(obj);
        }
        foreach (StarNode node in stars)
        {
            node.ClearConnections();
            node.SetDrawable(false);
        }
        rootStar.SetDrawable(true);
    }

    //To do: potentially change the input parts so it works w/ phones or use a input handler instead
    IEnumerator Connect(StarNode starA){
        Plane dragPlane = new Plane(cam.transform.forward, starA.gameObject.transform.position); 
        Ray mouseRay = cam.ScreenPointToRay(Input.mousePosition); 
        float dist;
        dragPlane.Raycast(mouseRay, out dist);
        lineRenderer = Instantiate(lineRendererPrefab, Vector3.zero, Quaternion.identity).GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, starA.transform.position); 
        while(!Input.GetMouseButtonUp(0)){
            mouseRay = cam.ScreenPointToRay(Input.mousePosition);
            dragPlane.Raycast(mouseRay, out dist);
            Vector3 drawPos = mouseRay.GetPoint(dist);
            if(Vector3.Distance(drawPos, lineRenderer.GetPosition(0))>maxLineDist){
                Vector3 offset = drawPos - lineRenderer.GetPosition(0);
                lineRenderer.SetPosition(1, lineRenderer.GetPosition(0)+ Vector3.ClampMagnitude(offset, maxLineDist));
            }
            else{
                lineRenderer.SetPosition(1, drawPos); 
            }
            RaycastHit2D[] hits = Physics2D.RaycastAll(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1) - lineRenderer.GetPosition(0), Vector3.Distance(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1)));
            foreach(RaycastHit2D linehit in hits){
                if(linehit.collider.gameObject.tag == "Obstacle"){
                    InterruptConnection();
                    yield break;
                }
            }
            yield return null;
        }
        RaycastHit2D hit = Physics2D.Raycast(lineRenderer.GetPosition(1), Vector2.zero);
        if(hit.collider != null){
            StarNode newConnection = hit.collider.gameObject.GetComponent<StarNode>();
            if(newConnection != null && starA.CanBuildConnection(newConnection)){
                lineRenderer.SetPosition(1, newConnection.gameObject.transform.position);
                BuildConnection(starA, newConnection);
                newConnection.SetDrawable(true);
                lineRendererObjects.Add(lineRenderer.gameObject);

                OnStarConnection?.Invoke();
                
                board.drawConnection(starA.transform.position, newConnection.transform.position);
            }
            else{
                Destroy(lineRenderer.gameObject);
            }
        }
        else{
            Destroy(lineRenderer.gameObject);
        }
        lineRenderer = null;
    }

    public void TryConnecting(StarNode starA){
        StartCoroutine(Connect(starA));
    }

   public void InterruptConnection(){
        StopAllCoroutines();
        if(lineRenderer != null){
            Destroy(lineRenderer.gameObject);
            lineRenderer = null;
        }
    }

    public StarNode GetRootStar(){
        return rootStar;
    }
}
