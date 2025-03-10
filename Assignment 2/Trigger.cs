using System.Collections;
using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;
using UnityEngine.UI;

public class Trigger : MonoBehaviour
{
    private bool BirdInside, playerInside = false;
    private GameObject bird;
    Movement movement;
    public GameObject person;
    private int colorFlag;
    Transform eatPosition;
    private Animator RedAnimator, GreenAnimator, YellowAnimator;
    private bool toEatingPosition;
    private bool isBirdEat;
    public Transform redZonePoint, greenZonePoint, yellowZonePoint;
    public SplineFollower redFollower, greenFollower, yellowFollower;
    private GameObject selectedFruit;
    private int redNum, greenNum, yellowNum;
    public Text RedNumText;
    public Text GreenNumText;
    public Text YellowNumText;
    private bool nothing;
    private bool samePos;

    // Start is called before the first frame update
    void Start()
    {
        Movement movement = person.GetComponent<Movement>();
        colorFlag = movement.ColorFlag;
        selectedFruit = movement.selectedFruit;
        redNum = movement.RedNum;
        greenNum = movement.GreenNum;
        yellowNum = movement.YellowNum;
        RedAnimator = movement.RedAnimator;
        GreenAnimator = movement.GreenAnimator;
        YellowAnimator = movement.YellowAnimator;
        Debug.Log("ColorFlay = " + colorFlag);
        //colorFlag = movement.ColorFlag;
        //selectedFruit = movement.selectedFruit;

    }

    // Update is called once per frame
    void Update()
    {
        Movement movement = person.GetComponent<Movement>();
        colorFlag = movement.ColorFlag;
        selectedFruit = movement.selectedFruit;
        redNum = movement.RedNum;
        greenNum = movement.GreenNum;
        yellowNum = movement.YellowNum;
        eatPosition = movement.eatPosition;
        //colorFlag = movement.ColorFlag;
        //selectedFruit = movement.selectedFruit;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bird"))
        {
            BirdInside = true;
            bird = other.gameObject;
            Debug.Log("bird in Zone");
            CheckInteraction();
        }
        else if (other.CompareTag("Me"))
        {
            playerInside = true;
            Debug.Log("Me in Zone");
            Debug.Log("ColorFlay = " + colorFlag);
            CheckInteraction();
        }
    }

    private void CheckInteraction()
    {
        if (BirdInside && playerInside)
        {
            if (bird.name == "RedBird")
            {
                Debug.Log("Not following");
                redFollower.follow = false;

            }
            else if (bird.name == "GreenBird")
            {
                Debug.Log("Not following");
                greenFollower.follow = false;

            }
            else if (bird.name == "YellowBird")
            {
                yellowFollower.follow = false;
            }
            toEatingPosition = true;
            // 두 객체가 모두 Zone 안에 있을 때 상호 작용 코드를 실행합니다.
            Debug.Log("Both bird and player are inside the interaction zone. Perform interaction.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Bird"))
        {
            Debug.Log("bird out");
            BirdInside = false;
        }
        else if (other.CompareTag("Me"))
        {
            Debug.Log("Me out");
            playerInside = false;
            toEatingPosition = false;
            if (BirdInside)
            {
                if (bird.name == "RedBird")
                {
                    redFollower.SetPercent(0f);
                    redFollower.follow = true;
                }
                else if (bird.name == "GreenBird")
                {
                    greenFollower.SetPercent(0f);
                    greenFollower.follow = true;
                }
                else if (bird.name == "YellowBird")
                {
                    yellowFollower.SetPercent(0f);
                    yellowFollower.follow = true;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (toEatingPosition)
        {
            bird.transform.position = Vector3.MoveTowards(
                            bird.transform.position,
                            eatPosition.position, 2f * Time.deltaTime);

            if (bird.name == "RedBird")
            {
                RedAnimator.Play("Fly");
                
                if (bird.transform.position == eatPosition.position)
                {
                    if (colorFlag == 1)
                    {
                        selectedFruit.transform.parent = bird.transform;
                        //colorFlag = 0;
                        Movement movement = person.GetComponent<Movement>();
                        movement.ColorFlag = 0;
                        //selectedFruit.transform.localPosition = new Vector3(0f, 0.5f, 1f);
                        //animator.SetTrigger("Eat");

                        toEatingPosition = false;
                        isBirdEat = true;
                    }
                }
            }
            else if (bird.name == "GreenBird")
            {
                GreenAnimator.Play("Fly");

                if (bird.transform.position == eatPosition.position)
                {
                    if (colorFlag == 2)
                    {
                        selectedFruit.transform.parent = bird.transform;
                        //colorFlag = 0;
                        Movement movement = person.GetComponent<Movement>();
                        movement.ColorFlag = 0;
                        //selectedFruit.transform.localPosition = new Vector3(0f, 0.5f, 1f);
                        //animator.SetTrigger("Eat");

                        toEatingPosition = false;
                        isBirdEat = true;
                    }
                }
            }
            else if (bird.name == "YellowBird")
            {
                YellowAnimator.Play("Fly");

                if (bird.transform.position == eatPosition.position)
                {
                    if (colorFlag == 3)
                    {
                        selectedFruit.transform.parent = bird.transform;
                        //colorFlag = 0;
                        Movement movement = person.GetComponent<Movement>();
                        movement.ColorFlag = 0;
                        //selectedFruit.transform.localPosition = new Vector3(0f, 0.5f, 1f);
                        //animator.SetTrigger("Eat");

                        toEatingPosition = false;
                        isBirdEat = true;
                    }
                }
            }
        }
        else if(isBirdEat)
        {
            if (bird.name == "RedBird")
            {

                Debug.Log("다시 포인터로돌아가는");
                bird.transform.position = Vector3.MoveTowards(
                        bird.transform.position,
                        redZonePoint.position, 2f * Time.deltaTime);

                if (bird.transform.position == redZonePoint.position)
                {
                    Debug.Log("in start spline");
                    redFollower.SetPercent(0f);
                    Destroy(selectedFruit);
                    RedNumText.text = (int.Parse(RedNumText.text) - 1).ToString();
                    redFollower.follow = true;
                    isBirdEat = false;
                }
            }
            else if (bird.name == "GreenBird")
            {
                Debug.Log("다시 포인터로돌아가는");
                bird.transform.position = Vector3.MoveTowards(
                        bird.transform.position,
                        greenZonePoint.position, 2f * Time.deltaTime);

                 if (bird.transform.position == greenZonePoint.position)
                 {
                    Debug.Log("in start spline");
                    greenFollower.SetPercent(0f);
                    Destroy(selectedFruit);
                    GreenNumText.text = (int.Parse(GreenNumText.text) - 1).ToString();
                    greenFollower.follow = true;
                    isBirdEat = false;
                 }
            }
            else if (bird.name == "YellowBird")
            {
                Debug.Log("다시 포인터로돌아가는");
                bird.transform.position = Vector3.MoveTowards(
                        bird.transform.position,
                        yellowZonePoint.position, 2f * Time.deltaTime);

                if (bird.transform.position == yellowZonePoint.position)
                {
                    Debug.Log("in start spline");
                    yellowFollower.SetPercent(0f);
                    Destroy(selectedFruit);
                    YellowNumText.text = (int.Parse(YellowNumText.text) - 1).ToString();
                    yellowFollower.follow = true;
                    isBirdEat = false;
                }
            }
        }
    }

}
