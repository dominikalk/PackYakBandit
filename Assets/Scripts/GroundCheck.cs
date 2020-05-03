using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private Character theCharacter;
    private int floorCount;
    private int platformCount;

    private GameObject[] platforms;

    // Start is called before the first frame update
    void Start()
    {
        theCharacter = FindObjectOfType<Character>();
        platforms = GameObject.FindGameObjectsWithTag("Platform");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            if(platformCount > 0)
            {
                foreach (GameObject thePlatform in platforms)
                {
                    thePlatform.GetComponent<PlatformEffector2D>().rotationalOffset = 180;
                }
            }
        }

        if (Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
        {
            foreach (GameObject thePlatform in platforms)
            {
                thePlatform.GetComponent<PlatformEffector2D>().rotationalOffset = 0;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Platform") || collision.CompareTag("Ground"))
        {
            floorCount += 1;
            theCharacter.onGround = true;
            theCharacter.charAnim.SetBool("OnGround", true);

            if (collision.CompareTag("Platform"))
            {
                platformCount += 1;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Platform") || collision.CompareTag("Ground"))
        {
            floorCount -= 1;

            if (collision.CompareTag("Platform"))
            {
                platformCount -= 1;
            }
        }

        if(floorCount <= 0)
        {
            theCharacter.onGround = false;
            theCharacter.charAnim.SetBool("OnGround", false);
        }

        if (platformCount <= 0)
        {
            foreach (GameObject thePlatform in platforms)
            {
                thePlatform.GetComponent<PlatformEffector2D>().rotationalOffset = 0;
            }
        }
    }
}
