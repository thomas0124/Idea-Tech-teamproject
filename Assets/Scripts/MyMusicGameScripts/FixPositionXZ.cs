using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixPositionXZ : MonoBehaviour
{
    [SerializeField] private GameObject roadOrange;
    [SerializeField] private GameObject roadBlue;
    [SerializeField] private GameObject roadGreen;
    [SerializeField] private GameObject roadPink;

    private void Start() 
    {
        roadOrange = GameObject.Find("road.orange");
        roadBlue = GameObject.Find("road.blue");
        roadGreen = GameObject.Find("road.green");
        roadPink = GameObject.Find("road.pink");
    }

    private void Update() 
    {
        switch(this.gameObject.name)
        {
            case "musicbox.orange":
                this.transform.position=  new Vector3(roadOrange.transform.position.x, this.transform.position.y, roadOrange.transform.position.z);
                break;
            case "musicbox.blue":
                this.transform.position =  new Vector3(roadBlue.transform.position.x, this.transform.position.y, roadBlue.transform.position.z);
                break;
            case "musicbox.green":
                this.transform.position = new Vector3(roadGreen.transform.position.x, this.transform.position.y, roadGreen.transform.position.z);
                break;
            case "musicbox.pink":
                this.transform.position =  new Vector3(roadPink.transform.position.x, this.transform.position.y, roadPink.transform.position.z);
                break;
        }
    }
}
