using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.Random;
using Dreamteck.Splines;
using TMPro;


public class Movement : MonoBehaviour
{
    // if (Input.GetKeyDown(KeyCode.JoystickButton))
    public PlayerInputActions playerControls;

    private InputAction interact;
    private InputAction move;
    Camera mainCamera;
    public float speed = 5f;

    //Press B
    public Camera CanvasCamera;
    private bool isList=false;

    //Press Y
    public Camera MiniMapCamera;
    private bool isMiniMap = false;

    //Press A
    public Transform ThirdView;
    public Transform FirstView;
    private bool isFirst = true;
    public LayerMask FirstLayer;
    public LayerMask ThirdLayer;
    public GameObject parent;

    //Press X for telep
    private GameObject TelepHub;
    public GameObject Hub1,Hub2,Hub3,Hub4,Hub5;
    [SerializeField] private GameObject player;
    public Camera EnlargedCamera;

    //press X for select fruit
    public GameObject[] RedFruit;
    public GameObject[] GreenFruit;
    public GameObject[] YellowFruit;
    public Text RedNumText;
    public Text GreenNumText;
    public Text YellowNumText;
    public int RedNum;
    public int GreenNum;
    public int YellowNum;

    //Fruit in Box
    public GameObject selectedFruit;

    public Transform fruitBoxTransform; // 박스의 Transform 컴포넌트
    private float spawnRadius = 0.3f; // 열매를 생성할 반경
    public Transform handTransform;
    public int ColorFlag;//0:None, 1:red, 2:green, 3:yellow

    //Gazing bird->Change path
    public GameObject redBird, greenBird, yellowBird;
    public SplineFollower redFollower, greenFollower, yellowFollower;
    private bool isMovingToRed, isMovingToGreen, isMovingToYellow;
    public Transform redZonePoint, greenZonePoint, yellowZonePoint;
    public Transform eatPosition;
    public Animator RedAnimator, GreenAnimator, YellowAnimator;


    private void Awake()
    {
        playerControls = new PlayerInputActions();
    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();

        interact = playerControls.Player.Interaction;
        interact.Enable();
        interact.performed += Interaction;
    }

    private void OnDisable()
    {
        move.Disable();
        interact.Disable();
    }

    void Start()
    {
        mainCamera = Camera.main;
        EnlargedCamera.gameObject.SetActive(false);
        CanvasCamera.gameObject.SetActive(false);
       // offset = transform.position - avatar.transform.position;
    }
    void Update()
    {
        Vector2 inputDirection = move.ReadValue<Vector2>();
        Vector3 moveDirection = mainCamera.transform.forward * inputDirection.y + mainCamera.transform.right * inputDirection.x;
        moveDirection.y = 0;
        transform.position += moveDirection.normalized * speed * Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            Debug.Log("JoystickButton1");
        }


            if (isMovingToGreen)
        {
            greenFollower.transform.LookAt(greenZonePoint.position);
            greenFollower.transform.position = Vector3.MoveTowards(greenFollower.transform.position, greenZonePoint.position, 2f * Time.deltaTime);

            // 새가 목표 위치에 도달하면 이동을 멈춥니다.
            if (greenFollower.transform.position == greenZonePoint.position)
            {
                isMovingToGreen = false;
                greenFollower.SetPercent(0f);
                greenFollower.follow = true;
                GreenAnimator.Play("Walk");
            }
        }
        if (isMovingToRed)
        {
            redFollower.transform.LookAt(redZonePoint.position);
            redFollower.transform.position = Vector3.MoveTowards(redFollower.transform.position, redZonePoint.position, 2f * Time.deltaTime);

            // 새가 목표 위치에 도달하면 이동을 멈춥니다.
            if (redFollower.transform.position == redZonePoint.position)
            {
                isMovingToRed = false;
                redFollower.SetPercent(0f);
                redFollower.follow = true;
                RedAnimator.Play("Walk");
            }
        }
        if (isMovingToYellow)
        {
            yellowFollower.transform.LookAt(yellowZonePoint.position);
            yellowFollower.transform.position = Vector3.MoveTowards(yellowFollower.transform.position, yellowZonePoint.position, 2f * Time.deltaTime);

            // 새가 목표 위치에 도달하면 이동을 멈춥니다.
            if (yellowFollower.transform.position == yellowZonePoint.position)
            {
                isMovingToYellow = false;
                yellowFollower.SetPercent(0f);
                yellowFollower.follow = true;
                YellowAnimator.Play("Walk");
            }
        }
    }

    void LateUpdate()
    {
        if (!isFirst)
        {
            mainCamera.transform.parent = parent.transform;
            player.transform.rotation = mainCamera.transform.rotation;
            mainCamera.transform.position = ThirdView.position;
            //mainCamera.transform.parent = player.transform;
        }
        else
        {
            mainCamera.transform.parent = player.transform;
        }
    }

    private void Interaction(InputAction.CallbackContext context)
    {
        if (context.control.name == "k" && context.performed)
        {
            isList = !isList;
            CanvasCamera.gameObject.SetActive(isList);
            Debug.Log("Check lsit");

        }
        else if (context.control.name == "u" && context.performed)
        {
            isMiniMap = !isMiniMap;
            MiniMapCamera.gameObject.SetActive(isMiniMap);
            Debug.Log("MiniMap");
        }
        else if (context.control.name == "l" && context.performed)
        {
            Vector3 currentRotation = transform.localEulerAngles;

            if (isFirst)
            {
                isFirst = false;
                handTransform.parent = fruitBoxTransform;
                eatPosition.parent = fruitBoxTransform;

                mainCamera.transform.position = ThirdView.position;

                //currentRotation.x += 20f;

                transform.localEulerAngles = currentRotation;
                mainCamera.cullingMask = ThirdLayer;
                Debug.Log("To Third-Person View");
            }
            else
            {
                isFirst = true;

                mainCamera.transform.position = FirstView.position;

                //currentRotation.x -= 20f;

                transform.localEulerAngles = currentRotation;
                mainCamera.cullingMask = FirstLayer;
                handTransform.parent = mainCamera.transform;
                eatPosition.parent = mainCamera.transform;
                Debug.Log("To First-Person View");
            }
        }
        else if (context.control.name == "y" && context.performed)
        {
            
            RaycastHit hitInfo;
            if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hitInfo))
            {
                Debug.Log("fruit?");
                foreach(GameObject redF in RedFruit)
                {
                    if (hitInfo.collider.gameObject == redF)
                    {
                        Debug.Log("select red fruit");
                        RedNum++;
                        AddFruitToBox(redF);
                        RedNumText.text = RedNum.ToString();
                        break;
                    }
                }
                foreach (GameObject greenF in GreenFruit)
                {
                    if (hitInfo.collider.gameObject == greenF)
                    {
                        Debug.Log("select green fruit");
                        GreenNum++;
                        AddFruitToBox(greenF);
                        GreenNumText.text = (int.Parse(GreenNumText.text) + 1).ToString();
                        break;
                    }
                }
                foreach (GameObject yellowF in YellowFruit)
                {
                    if (hitInfo.collider.gameObject == yellowF)
                    {
                        Debug.Log("select yellow fruit");
                        YellowNum++;
                        AddFruitToBox(yellowF);
                        YellowNumText.text = YellowNum.ToString();
                        break;
                    }
                }

                if(hitInfo.collider.gameObject == redBird)
                {
                    redFollower.follow = false;
                    isMovingToRed = true;
                }
                else if (hitInfo.collider.gameObject == greenBird)
                {
                    Debug.Log("green bird!!");
                    greenFollower.follow = false;
                    isMovingToGreen = true;
                    //greenFollower.transform.position = greenZone.position;
                    //SplinePoint closestPoint = splineComputer.GetClosestPoint(greenZone.position);
                    //transform.position = closestPoint.position;
                }
                else if (hitInfo.collider.gameObject == yellowBird)
                {
                    yellowFollower.follow = false;
                    isMovingToYellow = true;
                }
            }
            
            checkISTelpHub();
            selectInBox();
        }
    }

    

    public void AddFruitToBox(GameObject fruit)
    {
        // 박스 주변의 랜덤한 위치에 열매를 생성합니다.
        Vector3 randomPosition = fruitBoxTransform.position + UnityEngine.Random.insideUnitSphere * spawnRadius;
        // Y 좌표를 박스의 높이로 설정하여 열매가 박스 위에 생성되도록 합니다.
        randomPosition.y = fruitBoxTransform.position.y;

        // 열매 프리팹을 복제하여 새로운 GameObject를 생성합니다.
        GameObject newFruit = Instantiate(fruit, randomPosition, Quaternion.identity);
        // 새로운 GameObject를 박스의 자식으로 설정하여 박스 안에 담습니다.
        newFruit.transform.parent = fruitBoxTransform;
    }

    private void selectInBox()
    {


        RaycastHit hitInfo;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hitInfo))
        {
            if (ColorFlag != 0)
            {
                if(hitInfo.collider.gameObject.name == "RedFruit(Clone)" ||
                hitInfo.collider.gameObject.name == "GreenFruit(Clone)"
                || hitInfo.collider.gameObject.name == "YellowFruit(Clone)")
                Debug.Log("already selected");
                Vector3 randomPosition = fruitBoxTransform.position + UnityEngine.Random.insideUnitSphere * spawnRadius;
                randomPosition.y = fruitBoxTransform.position.y;
                selectedFruit.transform.position = randomPosition;
                selectedFruit.transform.parent = fruitBoxTransform;
                ColorFlag = 0;
            }

            selectedFruit = hitInfo.collider.gameObject;

            if (selectedFruit.name == "RedFruit(Clone)")
            {
                Debug.Log("red clone");
                ColorFlag = 1;
            }
            else if (selectedFruit.name == "GreenFruit(Clone)")
            {
                Debug.Log("green clone");
                ColorFlag = 2;
            }
            else if (selectedFruit.name == "YellowFruit(Clone)")
            {
                Debug.Log("yellow clone");
                ColorFlag = 3;
            }
            else
            {
                Debug.Log("nothing");
                return;
            }
            selectedFruit.transform.position = handTransform.position;
            selectedFruit.transform.parent = handTransform;
        }
    }

    private void checkISTelpHub()
    {
        Debug.Log("check");

        RaycastHit hitInfo;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hitInfo))
        {
            if (hitInfo.collider.gameObject == Hub1 || hitInfo.collider.gameObject == Hub2 || hitInfo.collider.gameObject == Hub3
                || hitInfo.collider.gameObject == Hub4 || hitInfo.collider.gameObject == Hub5)
            {
                Debug.Log("collide");
                mainCamera.gameObject.SetActive(false);
                move.Disable();

                MiniMapCamera.gameObject.SetActive(false);

                EnlargedCamera.gameObject.SetActive(true);

                CanvasCamera.gameObject.SetActive(false);
                isList = false;

                interact.performed -= Interaction;
                interact.performed += Teleport;

            }
        }
    }

    private void Teleport(InputAction.CallbackContext context)
    {
        Debug.Log("teleport");
        RaycastHit hitInfo;
        if (Physics.Raycast(EnlargedCamera.transform.position, EnlargedCamera.transform.forward, out hitInfo) && context.control.name == "y")
        {
            if (hitInfo.collider.gameObject == Hub1)
            {
                TelepHub = Hub1;
                Debug.Log("Hub1");
            }
            else if (hitInfo.collider.gameObject == Hub2)
            {
                TelepHub = Hub2;
                Debug.Log("Hub2");
            }
            else if (hitInfo.collider.gameObject == Hub3)
            {
                TelepHub = Hub3;
                Debug.Log("Hub3");
            }
            else if (hitInfo.collider.gameObject == Hub4)
            {
                TelepHub = Hub4;
                Debug.Log("Hub4");
            }
            else if(hitInfo.collider.gameObject == Hub5)
            {
                TelepHub = Hub5;
                Debug.Log("Hub5");
            }
            else
            {
                Debug.Log("no Match Hub");
                return;
            }

            Debug.Log("start telpo");

            Vector3 currentRotation = transform.localEulerAngles;


            transform.localEulerAngles = currentRotation;

            Debug.Log("Player->" + player.transform.position);
            Debug.Log("Hub2->" + Hub2.transform.position);
            Debug.Log("TehlpHub -> " + TelepHub.transform.position);
            player.transform.position = new Vector3(TelepHub.transform.position.x,
                                                  player.transform.position.y,
                                                  TelepHub.transform.position.z);
            mainCamera.gameObject.SetActive(true);
            MiniMapCamera.gameObject.SetActive(true);
            EnlargedCamera.gameObject.SetActive(false);
            move.Enable();
            MiniMapCamera.gameObject.SetActive(isMiniMap);
            //int layerMask = LayerMask.GetMask("MiniMapBase");
            if (!isFirst)
            {
                isFirst = true;

                mainCamera.transform.position = FirstView.position;

                //currentRotation.x -= 20f;

                transform.localEulerAngles = currentRotation;
                mainCamera.cullingMask = FirstLayer;
                handTransform.parent = mainCamera.transform;
                eatPosition.parent = mainCamera.transform;
                Debug.Log("To First-Person View");
            }

            // 마스크를 Visible inside mask로 설정합니다.
            //layerMask = 1 << LayerMask.NameToLayer("MiniMapBase");
            interact.performed += Interaction;
            interact.performed -= Teleport;
      
            Debug.Log("Complete Telpo");
        }
    }
}