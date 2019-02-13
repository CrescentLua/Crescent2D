using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    float camx, camy, camz = 0.0f;
    float camTransSpeed;
    float camStart;
    // Use this for initialization
    void Start () {
        camTransSpeed = 2.0f;
        camStart = Camera.main.orthographicSize;
}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.E) && Camera.main.orthographicSize < 4.5f)
        {
            Vector3 Zoom = new Vector3(camx, camy + 2.25f, camz);
            transform.position += Zoom * Time.deltaTime;
            Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, camStart + 10.0f, camTransSpeed * Time.deltaTime);
            // *ANOTHER WAY OF TEMPERING WITH THE CAMERA*
        }

        if (Input.GetKey(KeyCode.Q) && Camera.main.orthographicSize > 2.14f)
        {
            Vector3 Zoom = new Vector3(camx, camy - 2.25f, camz);
 
            transform.position += Zoom * Time.deltaTime;

            Camera.main.orthographicSize = Mathf.MoveTowards(Camera.main.orthographicSize, camStart - 10.0f, camTransSpeed * Time.deltaTime);
            // *ANOTHER WAY OF TEMPERING WITH THE CAMERA*
        }

    }
}
