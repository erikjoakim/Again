using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    public Terrain terrain;
    public GameObject UIScreen;
    GameObject selectedGo;

    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    public static Vector3 adjustToTerrainHeight(Vector3 pos)
    {
        return new Vector3(pos.x, instance.terrain.SampleHeight(pos), pos.z);
    }
   
    
    public static Vector3 getTerrainNormal(Vector3 pos)
    {
        //Debug.Log("Position: " + pos);
        Vector3 terrainSize = instance.terrain.terrainData.size;
        return instance.terrain.terrainData.GetInterpolatedNormal(pos.x/terrainSize.x, pos.z/terrainSize.z);
    }

    public static void setSelectedBlob(GameObject blobGo)
    {
        if (instance.selectedGo != blobGo)
        {
            //Debug.Log("Subscribing BLOB: "+blobGo);
            if (instance.selectedGo)
            {
                BlobMover bm = instance.selectedGo.GetComponent<BlobMover>();
                //Debug.Log("Unsubscribing: " + bm);
                bm.blobChangedEvent -= OnsetUIBlobHealth;
            }
            instance.selectedGo = blobGo;
            blobGo.GetComponent<BlobMover>().blobChangedEvent += OnsetUIBlobHealth;
            GameManager.OnsetUIBlobHealth(blobGo);
        }
        
    }
    
    static void OnsetUIBlobHealth(GameObject go)
    {
        BlobMover bm = go.GetComponent<BlobMover>();
        if (bm.health > 0)
        {
            instance.UIScreen.GetComponent<ScreenMenuManager>().setBlobHealth(bm.health);
        }
        else
        {
            bm.blobChangedEvent -= OnsetUIBlobHealth;
            instance.selectedGo = null;
        }
    }    
}
