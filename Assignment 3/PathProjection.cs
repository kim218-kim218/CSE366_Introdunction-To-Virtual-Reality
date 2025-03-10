using UnityEngine;
using UnityEngine.UI;

public class PathProjection : MonoBehaviour
{
    private LineRenderer lr;
    Rigidbody rb;
    Vector3 startPosition;
    Vector3 startVelocity;
    float InitialForce = 15;
    float InitialAngle = -45;
    Quaternion rot;
    int i = 0;
    int NumberOfPoints = 50;
    float timer = 0.1f;
    public Text debug;
    public GameObject Arrow;

    public GameObject ball;
    Ball ballAction;
    private bool isStart;

    // Start is called before the first frame update
    void Start()
    {
        lr = GetComponent<LineRenderer>();
        rb = GetComponent<Rigidbody>();
        rot = Quaternion.Euler(InitialAngle, 0, 0);

    }

    // Update is called once per frame
    void Update()
    {
        Ball ballAction = ball.GetComponent<Ball>();
        isStart = ballAction.isStart;

        if (isStart)
        {
            drawline();
        }
        else
        {
            lr.enabled = false;
            debug.text =lr.enabled.ToString()+rb.transform.position+lr.transform.position;
        }

    }

    public void OnButtonClicked()
    {
        drawline();
    }


    private void drawline()
    {
        debug.text = lr.enabled.ToString() + rb.transform.position + lr.transform.position;
    i = 0;
        lr.positionCount = NumberOfPoints;
        lr.enabled = true;
        startPosition = transform.position;
        startVelocity = -Arrow.transform.forward * 20f / rb.mass;
        lr.SetPosition(i, startPosition);
        for (float j = 0; i < lr.positionCount - 1; j += timer)
        {
            i++;
            Vector3 linePosition = startPosition + j * startVelocity;
            linePosition.y = startPosition.y + startVelocity.y * j + 0.5f * Physics.gravity.y * j * j;
            lr.SetPosition(i, linePosition);
        }
    }
}