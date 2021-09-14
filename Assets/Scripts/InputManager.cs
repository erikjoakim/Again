using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public GameObject selectedObject;
    public GameObject pathMarker;
    public LayerMask lm;
    public Vector3 startPoint;
    Vector3 lastEndPoint;
    public List<Vector3> pathPoints;
    public List<GameObject> pathMarkerPool;
    
    // Start is called before the first frame update
    
    void Start()
    {        
        pathPoints = new List<Vector3>();
        pathMarkerPool = new List<GameObject>();
        lastEndPoint = new Vector3();
    }

    // Update is called once per frame
    
    void Update()
    {
        RaycastHit hit;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                GameObject tt = hit.transform.gameObject;
                if (tt.GetComponent<Selectable>())
                {
                    //Debug.Log("FOUND SELECTABLE");
                    //CameraController cm = Camera.main.gameObject.GetComponentInParent<CameraController>();
                    //cm.setFollowTarget(tt.GetComponent<Selectable>().transform);

                    GameManager.setSelectedBlob(tt);
                }
                if (tt.tag == "Player_1")
                {
                    selectedObject = tt;
                    startPoint = new Vector3(Mathf.Round(tt.transform.position.x),0, Mathf.Round(tt.transform.position.z));
                    //Debug.Log("SELECTED: " + tt.tag);
                }                
            }
        }
        if(Input.GetMouseButton(0))
        {
            if (selectedObject != null)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    
                    Vector3 endPoint= new Vector3(Mathf.Round(hit.point.x), 0, Mathf.Round(hit.point.z));
                    if (Vector3.Distance(endPoint, startPoint) > 1 && lastEndPoint != endPoint)
                    {
                        //Debug.Log("Hit: " + hit.point + " EndPoint: " + endPoint);
                        pathPoints = updatePathPoints(startPoint, endPoint);
                        updateMarkers(pathPoints);
                    }
                    //Debug.Log("Dist: " + Vector3.Distance(endPoint, startPoint));
                    lastEndPoint = endPoint;
                }
            }
        }

        
    }

    private void updateMarkers(List<Vector3> pathPoints)
    {
        for (int i = 0; i < pathMarkerPool.Count; i++)
        {
            pathMarkerPool[i].SetActive(false);
        }
        GameObject go;
        for (int i = 0; i < pathPoints.Count; i++)
        {
            if (i+1 > pathMarkerPool.Count)
            {
                go = Instantiate(pathMarker);
                pathMarkerPool.Add(go);
            } else
            {
                go = pathMarkerPool[i];
            }
            go.SetActive(true);
            Vector3 pos = new Vector3(pathPoints[i].x, 0, pathPoints[i].z);
            go.transform.position = GameManager.adjustToTerrainHeight(pos);

            Vector3 terrainNormal = GameManager.getTerrainNormal(pos);
            //Debug.Log("Normal: " + terrainNormal);
            go.transform.rotation = Quaternion.FromToRotation(Vector3.up, terrainNormal);
            
        }
        for (int i = 0; i < pathPoints.Count-pathMarkerPool.Count; i++)
        {
            go = Instantiate(pathMarker);
            go.SetActive(false);
            pathMarkerPool.Add(go);
        }
    }

    private List<Vector3> updatePathPoints(Vector3 start, Vector3 end)
    {
        List<Vector3> ret_val = new List<Vector3>();
        int point_x=0;
        int point_z=0;
        int dist_x = (int)Mathf.Abs(Mathf.Abs(start.x) - Mathf.Abs(end.x))+1;
        int dist_z = (int)Mathf.Abs(Mathf.Abs(start.z) - Mathf.Abs(end.z))+1;
        Debug.Log("Start:" + start + " END "+ end);
        Debug.Log("Dist x: " + dist_x + " Dist_y: " + dist_z);
        int x = 0;
        int z = 0;
        for (int i = 0; i < Mathf.Max(dist_x,dist_z);i++)
        {            
            if (x < dist_x)
            {
                if (start.x < end.x)
                {
                    point_x = (int)start.x + x;
                }
                else
                {
                    point_x = (int)start.x - x;
                }
            }
            x++;
            if(z < dist_z)
            {
                if (start.z < end.z)
                {
                    point_z = (int)start.z + z;
                }
                else
                {
                    point_z = (int)start.z - z;
                }                
            }
            z++;
            ret_val.Add(new Vector3(point_x, 0, point_z));
        }
        return ret_val;
    }
}
