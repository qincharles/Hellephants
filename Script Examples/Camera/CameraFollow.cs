using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CameraFollow : NetworkBehaviour {

    GameObject camera;
    public float smoothing = 5f;
    public bool secondPlayer;

    Vector3 offset;
    bool found = false;

    void Awake()
    {
        //if (!isLocalPlayer)
        //{
        //    camera.GetComponent<AudioListener>().enabled = false;
        //}
    }

    public void SetCamera(GameObject c)
    {
        Debug.Log("Camera Checking");
        //string tag;
        //if (secondPlayer)
        //{
        //    tag = "Camera2";
        //}
        //else
        //{
        //    tag = "Camera1";
        //}
        //camera = GameObject.FindGameObjectWithTag(tag);

        camera = c;
//        camera.GetComponent<AudioListener>().enabled = true;
        offset =  camera.transform.position - transform.position;
        //if (target == null)
        //{
        //    return false;
        //}
        //else
        //{
        //    found = true;
        //    GetComponent<AudioListener>().enabled = true;
        //    return true;
        //}
        found = true;
    }

    void FixedUpdate()
    {
        //if (!found)
        //{
        //    CheckForPlayer();
        //}
        if (!isLocalPlayer)
        {
            //// turn off listener
//            camera.GetComponent<AudioListener>().enabled = false;
            return;
        }
        Vector3 targetCamPos = transform.position + offset;
        camera.transform.position = Vector3.Lerp(camera.transform.position, targetCamPos, smoothing * Time.deltaTime);
    }
}
