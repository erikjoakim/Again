using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manufacturing : MonoBehaviour
{
    public Color color;
    public GameObject spawnGO;
    public int maxNOspawns = 10;
    public Transform spawnPos;
    public float SpawnIntervall = 5f;
    public Transform guardPos;
    private IEnumerator SpawnCR;
    private int i = 0;
    private int blobIndex = 0;
    public List<GameObject> blobList;
    private float lastTime= 0;
    // Start is called before the first frame update
    void Start()
    {
        blobList = new List<GameObject>();
        gameObject.GetComponent<Renderer>().material.color = color;        
    }

    // Update is called once per frame
    void Update()
    {
        float timeSinceLastTime = Time.time - lastTime;
        if (i < maxNOspawns)
        {
            if (timeSinceLastTime > SpawnIntervall)
            {

                GameObject go = Instantiate(spawnGO, spawnPos.position, Quaternion.identity, gameObject.transform);
                go.tag = gameObject.tag;
                go.name = gameObject.tag + "_Blob_" + blobIndex;
                blobIndex++;
                go.GetComponent<Renderer>().material.color = color;
                BlobMover bm = go.GetComponent<BlobMover>();
                bm.guardPos = GameManager.adjustToTerrainHeight(new Vector3(guardPos.position.x, 0, guardPos.position.z));
                blobList.Add(go);
                i++;
                lastTime = Time.time;
            }            
        }
    }
}
