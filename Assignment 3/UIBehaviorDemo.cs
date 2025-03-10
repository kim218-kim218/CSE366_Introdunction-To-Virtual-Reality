using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class UIBehaviorDemo : MonoBehaviour
{
    public GameObject cube; // for cube in scene
    public GameObject sphere; // for Sphere prefab
    public ARRaycastManager raycastManager;
    public ARPlaneManager planeManager;
    public ARAnchorManager anchorManager;

    ARAnchor planeAnchor; // an anchor attached to the plane
    GameObject placedSphere; // an instantiated sphere, whose position is tied to the anchor
    bool spherePlaced = false;

    public void DropCubeOnPlane(){
        // cast a ray to the plane
        Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        if (raycastManager.Raycast(ray, hits, TrackableType.PlaneWithinPolygon)) // if it hits
        {
            Debug.Log("cube dropped");
            ARRaycastHit hitObj = hits[0];
            Pose pose = hitObj.pose;
            cube.SetActive(true); // turn on the cube
            cube.transform.position = pose.position + new Vector3(0, 1, 0); // place it 1m above the hitpoint
        }
        

    }

    // Update is called once per frame
    //void Update()
    //{
    //    // place a sphere ONCE, on touchscreen tap
    //    if (!spherePlaced){
    //        if (Input.touchCount > 0){
    //            // Check if finger is over a UI element
    //            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
    //            {
    //                Touch t = Input.GetTouch(0);
    //                if (t.phase == TouchPhase.Began){
    //                    Ray ray = Camera.main.ScreenPointToRay(t.position);
    //                    List<ARRaycastHit> hits = new List<ARRaycastHit>();
    //                    if (raycastManager.Raycast(ray, hits, TrackableType.PlaneWithinPolygon))
    //                    {
    //                        ARRaycastHit hitObj = hits[0];
    //                        Pose pose = hitObj.pose;
    //                        ARPlane arPlane = planeManager.GetPlane(hitObj.trackableId);

    //                        // you can attach anchors directly to a plane
    //                        planeAnchor = anchorManager.AttachAnchor(arPlane, pose);
                            
    //                        // You can also add arbitrary anchors with:
    //                        // yourGameObject.AddComponent<ARAnchor>();

    //                        placedSphere = Instantiate<GameObject>(sphere, pose.position, pose.rotation);

    //                        spherePlaced = true;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    else {
    //        Debug.Log("already placed");
    //        // once placed, continuously update the sphere position/rotation from anchor
    //        sphere.transform.position = planeAnchor.transform.position;
    //        sphere.transform.rotation = planeAnchor.transform.rotation;
    //    }

    //}

    void Update()
    {
        if (!spherePlaced && Input.touchCount > 0)
        {
            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    List<ARRaycastHit> hits = new List<ARRaycastHit>();
                    if (raycastManager.Raycast(ray, hits, TrackableType.PlaneWithinPolygon))
                    {
                        Pose pose = hits[0].pose;
                        placedSphere = Instantiate(sphere, pose.position, pose.rotation);
                        spherePlaced = true; 
                    }
                }
            }
        }
        else if (spherePlaced)
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    List<ARRaycastHit> hits = new List<ARRaycastHit>();
                    if (raycastManager.Raycast(ray, hits, TrackableType.PlaneWithinPolygon))
                    {
                        Pose pose = hits[0].pose;
                        placedSphere.transform.position = pose.position;
                        placedSphere.transform.rotation = pose.rotation;
                    }
                }
            }
        }
    }

}
