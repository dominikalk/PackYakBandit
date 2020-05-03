using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Character theChar;
    private Camera theCamera;
    [SerializeField] private float smootheness;

    [SerializeField] private float jumpSize = default;
    [SerializeField] private float idleSize = default;

    [SerializeField] private float shakeDuration = default;
    [SerializeField] private float shakeMagnitude = default;

    public float stompSpeed = 4;

    // Start is called before the first frame update
    void Start()
    {
        theChar = FindObjectOfType<Character>();
        theCamera = GetComponent<Camera>();

        StartCoroutine("repeatCameraShake");
    }

    // Update is called once per frame
    void Update()
    {
        float yPos = 0;
        if(theChar.transform.position.y < -1f)
        {
            yPos = -1f;
        }
        else
        {
            yPos = theChar.transform.position.y;
        }

        float xPos = 0;
        if (theChar.transform.position.x < -2.5f || theChar.transform.position.x > 3.5f)
        {
            if (theChar.transform.position.x < -2.5f)
            {
                xPos = -2.5f;
            }
            else
            {
                xPos = 3.5f;
            }
        }
        else
        {
            xPos = theChar.transform.position.x;
        }

        Vector3 smootherPos = Vector3.Lerp(transform.position, new Vector3(xPos, yPos, transform.position.z), smootheness * Time.deltaTime);
        transform.position = smootherPos;

        if (theChar.onGround || theChar.onLadder)
        {
            float zPos = Mathf.Lerp(theCamera.orthographicSize, idleSize, (smootheness * Time.deltaTime) / 3f);
            theCamera.orthographicSize = zPos;
        }
        else
        {
            float zPos = Mathf.Lerp(theCamera.orthographicSize, jumpSize, (smootheness * Time.deltaTime)/ 3f);
            theCamera.orthographicSize = zPos;
        }
    }

    // -2.5, 3.5

    IEnumerator repeatCameraShake()
    {
        yield return new WaitForSeconds(2.5f);
        while (true)
        {
            StartCoroutine("cameraShake");
            GameObject[] lights = GameObject.FindGameObjectsWithTag("LightBulb");
            foreach(GameObject light in lights)
            {
                int random = Random.Range(0, 2);
                if(random == 1)
                {
                    light.transform.localScale = new Vector3(-light.transform.localScale.x, light.transform.localScale.y, light.transform.localScale.z);
                }
                StartCoroutine(lightDelay(light));
            }
            if (theChar.changeSpeed)
            {
                stompSpeed /= 1.5f;
                GameObject.FindGameObjectWithTag("Head").GetComponent<Animator>().SetFloat("speedMag", 1f / stompSpeed);
                theChar.changeSpeed = false;
            }
            GameObject.FindGameObjectWithTag("ThumpSound").GetComponent<CoinSound>().playSound();
            yield return new WaitForSeconds(stompSpeed);
        }
    }

    IEnumerator lightDelay(GameObject theLight)
    {
        yield return new WaitForSeconds(Random.Range(0, 10) / 20f);
        theLight.GetComponent<Animator>().SetTrigger("LightShake");
    }

    IEnumerator cameraShake()
    {
        Vector3 originalPos = transform.position;
        float elapsed = 0.0f;
        while(elapsed < shakeDuration)
        {
            float x = Random.Range(-1f, 1f) * shakeMagnitude;
            float y = Random.Range(-1f, 1f) * shakeMagnitude;

            transform.position = originalPos + new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }
        transform.position = originalPos;
    }
}
