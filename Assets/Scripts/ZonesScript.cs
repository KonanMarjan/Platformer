using UnityEngine;
using System.Collections;

public class ZonesScript : MonoBehaviour {
    [HideInInspector]
    public bool allowHor = false;
    [HideInInspector]
    public bool allowVer = false;
    [HideInInspector]
    public bool allowTarget = false;
    [HideInInspector]
    public Collider2D target;
    [HideInInspector]
    public bool changeSide = false;
    private int priority = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}
    void OnTriggerStay2D(Collider2D other)
    {
        
        if (other.tag == "HorFoll" && other.GetComponent<ZonePriority>().priority > priority)
        {
            priority = other.GetComponent<ZonePriority>().priority;
            allowHor = true;
            allowVer = false;
            allowTarget = false;

        }
        if (other.tag == "VerFoll" && other.GetComponent<ZonePriority>().priority > priority)
        {
            priority = other.GetComponent<ZonePriority>().priority;
            allowVer = true;
            allowHor = false;
            allowTarget = false;
        }
        if (other.tag == "Target" && other.GetComponent<ZonePriority>().priority > priority)
        {
            priority = other.GetComponent<ZonePriority>().priority;
            target = other;
            allowTarget = true;
            allowVer = false;
            allowHor = false;
            
        }

    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "HorFoll")
        {
            allowHor = false;
        }
        if (other.tag == "VerFoll")
        {
            allowVer = false;
        }
        if (other.tag == "Target")
        {
            allowTarget = false;
        }
        priority = 0;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "changeSide")
            changeSide = true;
    }
}
