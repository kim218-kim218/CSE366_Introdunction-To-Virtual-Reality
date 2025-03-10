using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildModeManager : MonoBehaviour
{
    //public ARAnchorManager anchorManager;
    //public ARPlaneManager planeManager;
    public ARRaycastManager raycastManager; 
    public GameObject transparentCubePrefab; 
    public Transform canvas; 

    private GameObject transparentCube; 
    private bool isPlacing= false; 

    public GameObject B1Button;
    public GameObject B2Button;
    public GameObject Building;
    public GameObject Car;
    public GameObject Tree;
    private GameObject SelectedObj;
    private bool BuildMode2 = false;
    private bool BuildMode1 = false;
    private bool isSelected = false;

    ARAnchor planeAnchor;
    public GameObject golfBall;
    private bool PlayMode = false;
    GameObject placedSphere;
    private bool ballPlaced = false;
    public Transform startBallPos;
    public GameObject resetButton;

    private float moveSpeed = 0.05f;

    public GameObject ModeChose;

    public void PlacedCube()
    {
        isPlacing = false;
        isSelected = false;
    }

    public void DeleteObj()
    {
        if (isPlacing)
        {
            if (isSelected)
            {
                Destroy(SelectedObj);
                isSelected = false;
            }
            else
            {
                Destroy(transparentCube);
            }
            isPlacing = false;

        }
    }

    public void MoveToB2()
    {
        B1Button.SetActive(false);
        B2Button.SetActive(true);
        BuildMode2 = true;
        BuildMode1 = false;
        isPlacing = false;
        isSelected = false;
    }

    public void ChooseMode()
    {
        ModeChose.SetActive(true);
        B1Button.SetActive(false);
        B2Button.SetActive(false);
        BuildMode2 = false;
        BuildMode1 = false;
    }

    public void BuildModeChose()
    {
        ModeChose.SetActive(false);
        B1Button.SetActive(true);
        BuildMode1 = true;
    }

    public void PlayModeChose()
    {
        ModeChose.SetActive(false);
        B2Button.SetActive(false);
        //PlayButton.SetActive(true);
        //BuildMode2 = false;
        isPlacing = false;
        isSelected = false;
        PlayMode = true;
        BuildMode1 = false;
        resetButton.SetActive(true);

        golfBall.SetActive(true);
        golfBall.transform.position = startBallPos.position;
    }

    public void Reset()
    {
        golfBall.transform.position = startBallPos.position;
    }

    public void OnFrontButtonClicked()
    {
        if (isPlacing)
        {
            MoveCube(Camera.main.transform.forward);
        }
    }

    public void OnBackButtonClicked()
    {
        if (isPlacing)
        {
            MoveCube(-Camera.main.transform.forward);
        }
    }

    public void OnLeftButtonClicked()
    {
        if (isPlacing)
        {
            MoveCube(-Camera.main.transform.right);
        }
    }

    public void OnRightButtonClicked()
    {
        if (isPlacing)
        {
            MoveCube(Camera.main.transform.right);
        }
    }

    private void MoveCube(Vector3 direction)
    {
        SelectedObj.transform.Translate(direction * moveSpeed);
    }

    public void SelectBuilding()
    {
        if (!isSelected)
        {
            Debug.Log("Building");
            SelectedObj = Building;
            isSelected = true;
        }
    }

    public void SelectCar()
    {
        if (!isSelected)
        {
            Debug.Log("Car");
            SelectedObj = Car;
            isSelected = true;

        }
    }

    public void SelectTree()
    {
        if (!isSelected)
        {
            Debug.Log("Tree");
            SelectedObj = Tree;
            isSelected = true;
        }
    }

    public void RotateRight()
    {
        if (isSelected)
        {
            SelectedObj.transform.Rotate(0, 10, 0);

        }
    }

    public void RotateLeft()
    {
        if (isSelected)
        {
            SelectedObj.transform.Rotate(0, -10, 0);

        }
    }

    public void ScaleUpCube()
    {
        transparentCube.transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);

        Vector3 newPosition = transparentCube.transform.position;
        newPosition.y += 0.005f;
        transparentCube.transform.position = newPosition;

    }

    public void ScaleDownCube()
    {
        transparentCube.transform.localScale -= new Vector3(0.01f, 0.01f, 0.01f);

        Vector3 newPosition = transparentCube.transform.position;
        newPosition.y -= 0.005f;
        transparentCube.transform.position = newPosition;

    }

    public void RotateDown()
    {
        if (isPlacing)
        {
            transparentCube.transform.Rotate(0, 0, 10);
        }
    }

    public void RotateUp()
    {
        if (isPlacing)
        {
            transparentCube.transform.Rotate(0, 0, -10);
        }
    }


    void Update()
    {


        if (BuildMode1)
        {
            if (Input.touchCount > 0 && !isPlacing)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    //List<ARRaycastHit> hits = new List<ARRaycastHit>();
                    //if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                    //{
                    //    Pose pose = hits[0].pose;
                    //    Vector3 cubePosition = pose.position;
                    //    cubePosition.y += 0.1f;

                    //    transparentCube = Instantiate(transparentCubePrefab, cubePosition, pose.rotation);

                    //    isPlacing = true;
                    //}
                    if (!IsPointerOverUIObject(touch.position)) // UI 버튼 위에 레이캐스트된지 확인
                    {
                        List<ARRaycastHit> hits = new List<ARRaycastHit>();
                        if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                        {
                            Pose pose = hits[0].pose;
                            Vector3 cubePosition = pose.position;
                            cubePosition.y += 0.1f;

                            transparentCube = Instantiate(transparentCubePrefab, cubePosition, pose.rotation);
                            isPlacing = true;
                        }
                    }
                }
            }
        }
        else if(BuildMode2)
        {
            if (isSelected)
            {
                if (Input.touchCount > 0 && !isPlacing)
                {
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began)
                    {
                        //List<ARRaycastHit> hits = new List<ARRaycastHit>();
                        //if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                        //{
                        //    Pose pose = hits[0].pose;
                        //    Vector3 cubePosition = pose.position;
                        //    //cubePosition.y += 0.1f;

                        //    SelectedObj = Instantiate(SelectedObj, cubePosition, pose.rotation);

                        //    isPlacing = true;
                        //}
                        if (!IsPointerOverUIObject(touch.position)) // UI 버튼 위에 레이캐스트된지 확인
                        {
                            List<ARRaycastHit> hits = new List<ARRaycastHit>();
                            if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                            {
                                Pose pose = hits[0].pose;
                                Vector3 objPosition = pose.position;

                                SelectedObj = Instantiate(SelectedObj, objPosition, pose.rotation);
                                isPlacing = true;
                            }
                        }
                    }
                }
            }
        }

    }

    bool IsPointerOverUIObject(Vector2 touchPosition)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(touchPosition.x, touchPosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.GetComponent<Button>() != null && result.gameObject.transform.IsChildOf(canvas.transform))
            {
                return true;
            }
        }

        return false;
    }
}
