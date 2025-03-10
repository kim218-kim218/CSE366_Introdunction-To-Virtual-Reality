using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class carMoving : MonoBehaviour
{
    public Transform pondCenter; // center of pond
    public float rotationSpeed = 10f; 
    public float moveSpeed = 5f;

    public bool isMoving; // if camera is in the car -> true / otherwise->false
    public bool isResetplace;

    [SerializeField] private GameObject player;
     public Transform firstPlayerPos;
    [SerializeField] private GameObject cart;
    [SerializeField] private GameObject cartBottom;
    private float watchingTime = 5f;
    private float myTimer_car = 0f;
    private float myTimer_feet = 0f;


    private float moveTime;

    [SerializeField] private Text timer_cart;
    [SerializeField] private Text timer_feet;

    [SerializeField] private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        moveTime = -45f;
        timer_cart.enabled = false;
        timer_feet.enabled = false;
        isMoving = false;
        isResetplace = true;
    }

    // Update is called once per frame
    void Update()
    {
        CheckTeleportInput();

        // if gazing.
        if (isMoving)
        {
            moveTime += Time.deltaTime;

            float rotationAngle = Mathf.Repeat(moveTime * rotationSpeed, 360f);
            // move car with rotationAngle.
            transform.position = movePosition(rotationAngle);

            transform.LookAt(pondCenter);

            player.transform.position = new Vector3(cart.transform.position.x, player.transform.position.y, cart.transform.position.z);
            //isMoving = true;

        }
        else
        {
            if (isResetplace)
            {
                //Debug.Log("isGazingFeet = true!" + timer_feet.enabled);
                player.transform.position = new Vector3(firstPlayerPos.position.x,firstPlayerPos.position.y,firstPlayerPos.position.z);
            }
        }

        

    }

    Vector3 movePosition(float angle)
    {
        float Xradius = 14.7f; // x축 반지름
        float Yradius = 14.7f; // y축 반지름

        float x = pondCenter.position.x + (Xradius * Mathf.Cos(Mathf.Deg2Rad * angle));
        float z = pondCenter.position.z + (Yradius * Mathf.Sin(Mathf.Deg2Rad * angle));

        return new Vector3(x, transform.position.y, z);
    }


    public void CheckTeleportInput()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hitInfo))
        {
            if (hitInfo.collider.gameObject == cart)
            {
                //show timer
                //Debug.Log("cart timer on" + myTimer_car);
                timer_cart.enabled = true;
                timer_feet.enabled = false;
                UpdateTextPosition(timer_cart);
                myTimer_car += Time.deltaTime;
                myTimer_feet = 0f;

                // start moving
                if (myTimer_car >= watchingTime)
                {
                    myTimer_car = 0f;
                    //Debug.Log("timer off");
                    timer_cart.enabled = false;
                    isMoving = true;
                    isResetplace = false;
                }
            }
            else if(hitInfo.collider.gameObject == cartBottom)
            {
                //show timer
                //Debug.Log("feet timer on" + myTimer_feet);
                timer_cart.enabled = true;
                timer_feet.enabled = true;
                UpdateTextPosition(timer_feet);
                myTimer_feet += Time.deltaTime;
                myTimer_car = 0f;

                // start moving
                if (myTimer_feet >= watchingTime)
                {
                    myTimer_feet = 0f;
                    //Debug.Log("timer off");
                    timer_feet.enabled = false;
                    isMoving = false;
                    isResetplace = true;
                }
            }
            else
            {
                // stop gazing -> reset/hide timer
                timer_cart.enabled = false;
                timer_feet.enabled = false;
                myTimer_feet = 0f;
                myTimer_car = 0f;
            }
        }
        else
        {
            timer_cart.enabled = false;
            timer_feet.enabled = false;
            myTimer_feet = 0f;
            myTimer_car = 0f;
        }

    }

    void UpdateTextPosition(Text text)
    {
        
        Vector3 playerPosition = player.transform.position;

        Camera playerCamera = player.transform.GetComponentInChildren<Camera>();

        if(text == timer_cart)
        {
            if (!isMoving)
            {
                text.transform.position = new Vector3(playerCamera.transform.position.x, playerCamera.transform.position.y, playerCamera.transform.position.z + 1f);
                text.text = myTimer_car.ToString("F1");

            }
        }
        else
        {
            if (!isResetplace)
            {
                text.transform.position = new Vector3(playerCamera.transform.position.x, playerCamera.transform.position.y-0.5f, playerCamera.transform.position.z);

                text.text = myTimer_feet.ToString("F1");
                //Debug.Log("text -> " + text.transform.position);
                //Debug.Log("player -> " + playerPosition);
            }
        }
        
    }
}
