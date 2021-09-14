using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector3 targetPos;
    public float speed = 1;
    public float damage = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, targetPos) < 0.01f)
        {
            //Debug.Log("DESTROYING BULLET: " + gameObject.name);
            //this.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * speed);
        }           
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject != null || collision.gameObject.activeSelf)
        {
            //Debug.Log(gameObject.name + ": Bullet OnCollision");
            if (collision.gameObject.tag != gameObject.tag)
            {

                //Debug.Log("Bullet Collision");
                if (collision.gameObject.GetComponent<BlobMover>())
                {
                    //Debug.Log("Bullet Take Damage");
                    collision.gameObject.GetComponent<BlobMover>().takeDamage(damage);
                }

            }
        }
    }
}
