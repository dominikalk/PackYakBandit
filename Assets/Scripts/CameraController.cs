using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Character theChar;
    [SerializeField] private float smootheness;

    // Start is called before the first frame update
    void Start()
    {
        theChar = FindObjectOfType<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        float yPos = 0;
        if(theChar.transform.position.y < -1.8f)
        {
            yPos = -1.8f;
        }
        else
        {
            yPos = theChar.transform.position.y;
        }
        Vector3 smootherPos = Vector3.Lerp(transform.position, new Vector3(theChar.transform.position.x, yPos, transform.position.z), smootheness * Time.deltaTime);
        transform.position = smootherPos;
    }
}
