using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    [SerializeField] private float cloudfrequency = default;

    [SerializeField] private GameObject[] closeClouds = default;
    [SerializeField] private GameObject[] farClouds = default;
    [SerializeField] private float closeCloudsSpeed = default;
    [SerializeField] private float farCloudsSpeed = default;

    [SerializeField] private Transform topPos = default;
    [SerializeField] private Transform bottomPos = default;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("spawnClouds");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator spawnClouds()
    {
        while (true)
        {
            yield return new WaitForSeconds(1/ cloudfrequency);
            if(Random.Range(0, 10) == 1)
            {
                float yPos = Random.Range(topPos.position.y * 10, bottomPos.position.y * 10) / 10f;
                if(Random.Range(0, 2) == 1)
                {
                    int randomCloud = Random.Range(0, farClouds.Length);
                    GameObject newCloud = Instantiate(farClouds[randomCloud], new Vector3(topPos.position.x, yPos, 2.1f), Quaternion.identity);
                    newCloud.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(farCloudsSpeed - 1.5f, farCloudsSpeed + 1.5f), 0);
                }
                else
                {
                    int randomCloud = Random.Range(0, closeClouds.Length);
                    GameObject newCloud = Instantiate(closeClouds[randomCloud], new Vector3(topPos.position.x, yPos, 2), Quaternion.identity);
                    newCloud.GetComponent<Rigidbody2D>().velocity = new Vector2(Random.Range(closeCloudsSpeed - 2, closeCloudsSpeed + 2), 0);
                }
            }

        }
    }
}
