using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlobMover : Selectable 
{    
    public enum BlobStatus
    {
        OnWay, Guarding, Attacking, Mining
    }    

    [SerializeField]
    private Vector3 moveTargetPos;
    [SerializeField]
    private Vector3 attackTargetPos;
    public GameObject targetGo;
    public Dictionary<int,BlobMover> attackers;
    public List<GameObject> targets;
    public float health = 10;
    public float attack = 1;
    public float defence = 0.2f;
    public GameObject bulletGo;
    public Vector3 guardPos;
    public float speed = 1;
    private int bulletId = 0;
    public float lastTime=0;

    #region Blob Changed Event
    public delegate void blobStatsChangedEventHandler(GameObject blobGo);
    public event blobStatsChangedEventHandler blobChangedEvent;
    protected virtual void OnBlobChanged(GameObject go)
    {
        blobChangedEvent?.Invoke(go);
    }
    #endregion

    [SerializeField]
    private BlobStatus _status;
    public BlobStatus Status
    {
        get
        {
            return _status;
        }
        set
        {
            if (value != _status)
            {
                switch (value)
                {
                    case BlobStatus.OnWay:
                        _status = value;
                        speed = 1;
                        break;
                    case BlobStatus.Guarding:
                        _status = value;
                        speed = 0.5f;
                        break;
                    case BlobStatus.Attacking:
                        _status = value;
                        speed = 0;
                        break;
                    case BlobStatus.Mining:
                        _status = value;
                        break;
                    default:
                        break;
                }
            }
        }
    }
   
    // Start is called before the first frame update

    void Start()
    {
        //currentPos = transform.position;
        Status = BlobStatus.OnWay;
        moveTargetPos = guardPos;
        attackers = new Dictionary<int, BlobMover>();
        targets = new List<GameObject>();
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (Status)
        {
            case BlobStatus.OnWay:
                if (Vector3.Distance(transform.position, moveTargetPos) < 0.1f)
                {
                    Status = BlobStatus.Guarding;
                    speed = 0.5f;
                    OnBlobChanged(gameObject);
                }
                else
                {
                    //transform.position = GameManager.adjustToTerrainHeight(Vector3.MoveTowards(transform.position, moveTargetPos, Time.deltaTime * speed));
                    transform.position = Vector3.MoveTowards(transform.position, moveTargetPos, Time.deltaTime * speed);
                }
                break;

            case BlobStatus.Guarding:
                if (Vector3.Distance(transform.position, moveTargetPos) < 0.1f)
                {
                    moveTargetPos = guardPos + new Vector3(Random.Range(-9, 9), 0, Random.Range(-9, 9));
                    moveTargetPos = GameManager.adjustToTerrainHeight(moveTargetPos);
                    moveTargetPos.x = Mathf.Clamp(moveTargetPos.x, 0.1f, 10000);
                    moveTargetPos.z = Mathf.Clamp(moveTargetPos.z, 0.1f, 10000);                    
                }
                else
                {
                    //transform.position = GameManager.adjustToTerrainHeight(Vector3.MoveTowards(transform.position, moveTargetPos, Time.deltaTime * speed));
                    transform.position = Vector3.MoveTowards(transform.position, moveTargetPos, Time.deltaTime * speed);
                }
                break;

            case BlobStatus.Attacking:
                
                if (targetGo != null)
                {
                    float timeSinceLastTime = Time.time - lastTime;
                    float attackIntervall = Random.Range(1, 4);
                    if (targetGo.activeSelf)
                    {
                        if (timeSinceLastTime > attackIntervall)
                        {
                            //Debug.Log(gameObject.name + ": ATTACKING " + targetGo.name);
                            //Debug.Log(gameObject.name + ": Active");
                            GameObject go = Instantiate(bulletGo, transform.position, Quaternion.identity, transform);
                            go.GetComponent<Bullet>().targetPos = targetGo.transform.position;
                            go.tag = gameObject.tag;
                            go.name = gameObject.name + "_Bullet_" + bulletId;
                            bulletId++;
                            lastTime = Time.time;
                        }
                    }
                    else
                    {
                        //Debug.Log(gameObject.name + " STOP ATTACKING");
                        targetGo = null;
                        Status = BlobStatus.Guarding;
                        OnBlobChanged(gameObject);
                    }                    
                }
                else
                {
                    Status = BlobStatus.Guarding;
                    OnBlobChanged(gameObject);
                }
                break;
            case BlobStatus.Mining:
                break;
            default:
                break;
        }                        
    }

    

    

    public void addAttacker(GameObject go)
    {
        attackers.Add(go.GetInstanceID(), go.GetComponent<BlobMover>());
    }
    public void removeTarget(GameObject go)
    {
        if (go == targetGo)
        {
            targetGo = null;
        }
    }
    internal void takeDamage(float damage)
    {
        //Debug.Log(gameObject.name + ": TAKING DAMAGE");
        health -= (damage - defence);
        //Debug.Log(gameObject.name + ": Health: " + health);
        if (health < 0)
        {
            //Debug.Log(gameObject.name + ": KILLED");
            foreach( BlobMover bm in this.attackers.Values)
            {
                bm.removeTarget(bm.gameObject);
            }
            gameObject.SetActive(false);            
        }
        OnBlobChanged(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.activeSelf )
        {
            if (Status != BlobStatus.Attacking && other.gameObject.tag != "Terrain" && other.gameObject.tag != gameObject.tag)
            {
                //Debug.Log(gameObject.name + ": TRIGGER COLLISION");
                Status = BlobStatus.Attacking;                
                attackTargetPos = other.transform.position;
                targetGo = other.gameObject;
                OnBlobChanged(gameObject);
            }
        }
    }
}
