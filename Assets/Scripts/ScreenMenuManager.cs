using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ScreenMenuManager : MonoBehaviour
{
    public Label blobHealth;
    // Start is called before the first frame update
    void Start()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        blobHealth = root.Q<Label>("health-text");
    }

    public void setBlobHealth(float health)
    {
        //Debug.Log("UPDATING HEALTH");
        blobHealth.text = health.ToString();
    }
}
