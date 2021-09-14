using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed = 1f;
    public float maxHeight = 20;
    public float minHeight = 2;
    public float rotAtMaxH = 89;
    public float rotAtMinH = 2;
    public float scrollSpeed = 100;

    public Transform followMe;
    public Vector3 followMeOffset;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (followMe == null)
        {
            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.left * speed * Time.unscaledDeltaTime);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.right * speed * Time.unscaledDeltaTime);
            }
            if (Input.GetKey(KeyCode.W))
            {
                transform.Translate(Vector3.forward * speed * Time.unscaledDeltaTime);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.Translate(Vector3.back * speed * Time.unscaledDeltaTime);
            }
        }
        else
        {
            transform.position = followMe.position + followMeOffset;
        }

        if (Input.mouseScrollDelta.sqrMagnitude > 0)
        {
            float trans = Input.mouseScrollDelta.y * scrollSpeed * Time.unscaledDeltaTime;
            if (transform.position.y + trans < maxHeight && transform.position.y + trans > minHeight)
            {
                transform.Translate(new Vector3(0, trans, 0));
                
                float curRot = (transform.position.y * (rotAtMaxH - rotAtMinH) + rotAtMinH * maxHeight - rotAtMaxH * minHeight) / (maxHeight - minHeight);
                //Debug.Log(" Rotation: " + curRot);

                Camera.main.transform.rotation = Quaternion.Euler(curRot, 0, 0);                                    
                
                //new Vector3(Mathf.Lerp(rotAtMaxH, rotAtMinH, trans / (rotAtMinH - rotAtMaxH)), 0, 0));
                //transform.rotation = Quaternion.Euler(trans, 0, 0);
                //Debug.Log("Trans " + trans);
                //Debug.Log(Mathf.Lerp(rotAtMaxH, rotAtMinH, trans / (rotAtMinH - rotAtMaxH)));
            }
            //Debug.Log(Input.mouseScrollDelta);
        }
    }

    public void setFollowTarget(Transform trf)
    {
        this.followMe = trf;
        this.followMeOffset = this.transform.position - trf.position;
    }
}
