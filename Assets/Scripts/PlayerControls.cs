using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerControls : MonoBehaviour
{
    public Rigidbody rigidBody;
    public float frontForce;
    public float sideForce;
    public float maxVelocity;

    public Transform cameraTransform;
    public Transform pointLightTransform;

    private Vector3 deltaCameraTransform;
    private Vector3 deltaPointLightTransform;

    public Text playerVelocityText;
    public Text playerPointsText;
    public int playerPoints = 0;

    public GameObject pauseCanvas;

    //TEST TOUCH PHASE
    public Text phaseText;
    private Touch theTouch;
    private float timeTouchEnded;
    private float displayTime = 0.5f;
    //TEST TOUCH PHASE

    //TEST TOUCH DIRECTION AND X-POSITION
    public Text directionText;
    public Text xTouchPositionText;
    private Vector2 touchStartPosition, touchEndPosition;
    private string directionString;
    //TEST TOUCH DIRECTION

    //TEST VELOCITY 
    public Text velocityText;
    //TEST VELOCITY

    //TEST START AND END POSITION 
    public GameObject planeFirst;
    //TEST START AND END POSITION

    public Camera camera;

    public GameObject stopPrefab;
    public GameObject pointPrefab;
    public GameObject speedPrefab;

    public int maxNumberOfElements;
    public int maxNumberOfPoints;
    public int maxNumberOfSpeeds;

    void TestTouchPhase()
    {
        if (Input.touchCount > 0)
        {
            theTouch = Input.GetTouch(0);
            if (theTouch.phase == TouchPhase.Ended)
            {
                phaseText.text = theTouch.phase.ToString();
                timeTouchEnded = Time.time;
            }
            else if (Time.time - timeTouchEnded > displayTime)
            {
                phaseText.text = theTouch.phase.ToString();
                timeTouchEnded = Time.time;
            }
        }
        else if (Time.time - timeTouchEnded > displayTime)
        {
            phaseText.text = "";
        }
    }
   
    void TestTouchDirection()
    {
        if (Input.touchCount > 0)
        {
            theTouch = Input.GetTouch(0);

            if (theTouch.phase == TouchPhase.Began)
            {
                touchStartPosition = theTouch.position;
            } 
            else
            {
                if (theTouch.phase == TouchPhase.Moved || theTouch.phase == TouchPhase.Ended)
                {
                    touchEndPosition = theTouch.position;
                    xTouchPositionText.text = "X-position: " + touchEndPosition;

                    float x = touchEndPosition.x - touchStartPosition.x;
                    float y = touchEndPosition.y - touchStartPosition.y;

                    if (Mathf.Abs(x) == 0 && Mathf.Abs(y) == 0)
                    {
                        directionString = "Tapped";
                    }
                    else
                    {
                        if (Mathf.Abs(x) > Mathf.Abs(y))
                        {
                            directionString = x > 0 ? "Right" : "Left";
                        }
                        else
                        {
                            directionString = y > 0 ? "Up" : "Down";
                        }
                    }
                }
            }
        }

        directionText.text = directionString;
    }
    
    void TestVelocityXY()
    {
        velocityText.text = "X: " + rigidBody.velocity.x + ", Y: " + rigidBody.velocity.y;
    }

    void GenerateElementsArray()
    {
        int[][] elements = new int[3][];
        elements[0] = new int[maxNumberOfElements];
        elements[1] = new int[maxNumberOfElements];
        elements[2] = new int[maxNumberOfElements];
        int numberOfPoints = 0;
        int numberOfSpeeds = 0;

        for (int i = 0; i < elements.Length; i++)
        {
            int maxNumberOfPointsOnOneLine = maxNumberOfPoints >= 3 ? (maxNumberOfPoints / 3) : (maxNumberOfPoints == 0 ? 0 : 1);
            int numberOfPointsOnOneLine = 0;
            bool isPointAvailable = (numberOfPoints < maxNumberOfPoints);

            int maxNumberOfSpeedsOnOneLine = maxNumberOfSpeeds >= 3 ? (maxNumberOfSpeeds / 3) : (maxNumberOfSpeeds == 0 ? 0 : 1);
            int numberOfSpeedsOnOneLine = 0;
            bool isSpeedAvailable = (numberOfSpeeds < maxNumberOfSpeeds);

            for (int j = 0; j < elements[i].Length; j++)
            {
                elements[i][j] = Random.Range(0, 4);

                if (elements[i][j] == 2)
                {
                    if (numberOfPointsOnOneLine >= maxNumberOfPointsOnOneLine || !isPointAvailable)
                    {
                        elements[i][j] = Random.Range(0, 2);
                    } else
                    {
                        numberOfPoints++;
                        numberOfPointsOnOneLine++;
                        if (numberOfPoints >= maxNumberOfPoints) isPointAvailable = false;
                    }
                }

                if (elements[i][j] == 3)
                {
                    if (numberOfSpeedsOnOneLine >= maxNumberOfSpeedsOnOneLine || !isSpeedAvailable)
                    {
                        elements[i][j] = Random.Range(0, 2);
                    }
                    else
                    {
                        numberOfSpeeds++;
                        numberOfSpeedsOnOneLine++;
                        if (numberOfSpeeds >= maxNumberOfSpeeds) isSpeedAvailable = false;
                    }
                }

                if (j > 2 && elements[i][j] == 0 && elements[i][j-1] == 0 && elements[i][j - 2] == 0 && elements[i][j - 3] == 0)
                {
                    elements[i][Random.Range(j - 3, j + 1)] = 1;
                }

                if (i == 1 && (elements[i][j] == 2 || elements[i][j] == 3) && (elements[i - 1][j] == 2 || elements[i - 1][j] == 3))
                {
                    elements[Random.Range(0, 2)][j] = Random.Range(0, 1);
                }

                if (i == 2 && (elements[i][j] == 2 || elements[i][j] == 3) && (elements[i - 1][j] == 2 || elements[i - 1][j] == 3))
                {
                    elements[Random.Range(1, 3)][j] = Random.Range(0, 1);
                }

                if (i == 2 && (elements[i][j] == 2 || elements[i][j] == 3) && (elements[i - 2][j] == 2 || elements[i - 2][j] == 3))
                {
                    elements[i][j] = Random.Range(0, 1);
                }

                if (j > 0 && elements[i][j] == 1 && elements[i][j - 1] == 1)
                {
                    elements[i][Random.Range(j - 1, j + 1)] = 0;
                }

                if (i == 2 && elements[i][j] == 1 && elements[i - 1][j] == 1 && elements[i - 2][j] == 1)
                {
                    elements[Random.Range(0, 3)][j] = 0;
                }
            }
        }

        GenerateElements(elements);
    }

    void GenerateElements(int [][] elements)
    {
        float planeStartPositionZ = planeFirst.GetComponent<Renderer>().bounds.min.z;
        float spaceBetween = planeFirst.GetComponent<Renderer>().bounds.size.z / (elements[0].Length + 1);

        for (int i = 0; i < elements.Length; i++)
        {
            for (int j = 0; j < elements[i].Length; j++)
            {
                if (elements[i][j] != 0)
                {
                    float positionX = i == 0 ? -3 : (i == 1 ? 0 : 3);
                    float positionZ = planeStartPositionZ + spaceBetween + j * spaceBetween;

                    Vector3 startPosition = new Vector3(positionX, planeFirst.transform.localPosition.y, positionZ);

                    switch (elements[i][j])
                    {
                        case 1:
                            Instantiate(stopPrefab, startPosition, Quaternion.identity, planeFirst.transform);
                            break;
                        case 2:
                            Instantiate(pointPrefab, startPosition, Quaternion.identity, planeFirst.transform);
                            break;
                        case 3:
                            Instantiate(speedPrefab, startPosition, Quaternion.identity, planeFirst.transform);
                            break;
                        default:
                            break;
                    }

                }
            }
        }
    }

    void Start()
    {
        GenerateElementsArray();
        deltaCameraTransform = new Vector3(transform.position.x, transform.position.y, transform.position.z) - new Vector3(cameraTransform.position.x, cameraTransform.position.y, cameraTransform.position.z);
        deltaPointLightTransform = new Vector3(transform.position.x, transform.position.y, transform.position.z) - new Vector3(cameraTransform.position.x, pointLightTransform.position.y, pointLightTransform.position.z);
    }

    void FixedUpdate()
    {
        ChangeCameraPosition();
        ChangePointLightPosition();

        AddPlayerFrontForce();

        ChangePlayerPositionByAD();
        ChangePlayerPositionByRaycasting();

        SetPlayerVelocityText();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }
    void AddPlayerFrontForce()
    {
        if (rigidBody.velocity.z <= maxVelocity) rigidBody.AddForce(Vector3.forward * frontForce * Time.fixedDeltaTime);
    }

    void ChangePlayerPosition(Vector3 position)
    {
        Vector3 newPosition = new Vector3(position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.fixedDeltaTime * sideForce);
    }

    void ChangePlayerPositionByRaycasting()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            bool e = Physics.Raycast(ray, out hit);

            if (e)
                ChangePlayerPosition(hit.point);
        }
    }

    void ChangeCameraPosition()
    {
        cameraTransform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z) - deltaCameraTransform;
    }

    void ChangePointLightPosition()
    {
        pointLightTransform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z) - deltaPointLightTransform;
    }

    void ChangePlayerPositionBySwipe()
    {
        if (rigidBody.velocity.z <= maxVelocity) rigidBody.AddForce(Vector3.forward * frontForce * Time.fixedDeltaTime);
        if (Input.touchCount > 0)
        {
            theTouch = Input.GetTouch(0);
            if (theTouch.phase == TouchPhase.Moved || theTouch.phase == TouchPhase.Ended)
            {
                touchEndPosition = theTouch.position;

                float x = touchEndPosition.x - touchStartPosition.x;
                float y = touchEndPosition.y - touchStartPosition.y;

                if (Mathf.Abs(x) != 0 && Mathf.Abs(y) != 0)
                {
                    if (Mathf.Abs(x) > Mathf.Abs(y))
                    {
                        if (x > 0) //right
                        {
                            if (rigidBody.velocity.x <= 0) rigidBody.AddForce(Vector3.right * sideForce * Time.fixedDeltaTime);
                            rigidBody.AddForce(Vector3.right * sideForce * Time.fixedDeltaTime);
                        }
                        else //left
                        {
                            if (rigidBody.velocity.x >= 0) rigidBody.velocity = new Vector3(0, rigidBody.velocity.y, rigidBody.velocity.z);
                            rigidBody.AddForce(Vector3.left * sideForce * Time.fixedDeltaTime);
                        }
                    }
                }
            }
        }
    }

    void ChangePlayerPositionByAD()
    {
        if (rigidBody.velocity.z >= maxVelocity) rigidBody.velocity = rigidBody.velocity.normalized * maxVelocity;
        if (Input.GetKey(KeyCode.D)) //rights
        {
            if (rigidBody.velocity.x <= 0) rigidBody.AddForce(Vector3.right * sideForce * Time.fixedDeltaTime);
            rigidBody.AddForce(Vector3.right * sideForce * Time.fixedDeltaTime);
            Debug.Log("D pressed!");
        }
        if (Input.GetKey(KeyCode.A)) //left
        {
            if (rigidBody.velocity.x >= 0) rigidBody.AddForce(Vector3.left * sideForce * Time.fixedDeltaTime);
            rigidBody.AddForce(Vector3.left * sideForce * Time.fixedDeltaTime);
            Debug.Log("A pressed!");
        }
    }

    void ChangePlayerPositionByTouch()
    {
        if (rigidBody.velocity.z <= maxVelocity) rigidBody.AddForce(Vector3.forward * frontForce * Time.fixedDeltaTime);
        if (Input.touchCount > 0)
        {
            touchEndPosition = theTouch.position;
            float xPlayerPosition = GetXPlayerPositionFromXTouchPosition(touchEndPosition.x);
            transform.position = Vector3.Lerp(transform.position, new Vector3(xPlayerPosition, transform.position.y, transform.position.z), Time.fixedDeltaTime * sideForce);
        }
    }

    //calculated for full hd window (1080x1920)
    /*player can move left and right
    touch size - from 0 to 1080
    way size - from -4 to 4 (from -3.5 to 3.5)
    so was calculated coefficient to lead touch position to way position 
     */
    float GetXPlayerPositionFromXTouchPosition(float xTouchPosition)
    {
        return xTouchPosition > 590 ? (xTouchPosition - 590) * 0.0059322f : (xTouchPosition * 0.0059322f) -3.5f;
    }

    void SetPlayerVelocityText() 
    {
        playerVelocityText.text = ((int) rigidBody.velocity.z).ToString();
    }

    void PauseGame()
    {
        Time.timeScale = 0;
        pauseCanvas.SetActive(true);
    }

    public void ResumeGame()
    {
        pauseCanvas.SetActive(false);
        Time.timeScale = 1;
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
}

