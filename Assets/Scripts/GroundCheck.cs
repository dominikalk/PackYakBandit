using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private Character theCharacter;
    private int floorCount;
    private bool isOnPlatform;
    private GameObject platform;

    // Start is called before the first frame update
    void Start()
    {
        theCharacter = FindObjectOfType<Character>();
        platform = GameObject.FindGameObjectsWithTag("Platform")[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            if(isOnPlatform)
            {
                platform.GetComponent<PlatformEffector2D>().rotationalOffset = 180;
            }
        }
        else
        {
            if (!isOnPlatform)
            {
                platform.GetComponent<PlatformEffector2D>().rotationalOffset = 0;
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
                platform = collision.gameObject;
                isOnPlatform = true;
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
                isOnPlatform = false;
            }
        }

        if(floorCount <= 0)
        {
            theCharacter.onGround = false;
            theCharacter.charAnim.SetBool("OnGround", false);
        }
    }
}
