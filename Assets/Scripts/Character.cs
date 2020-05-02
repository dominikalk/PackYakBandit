using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    private Rigidbody2D myRigid;
    [SerializeField] private float speed = default;
    [SerializeField] private float jumpForce = default;
    private Rigidbody2D checkFloor;

    [SerializeField] private GameObject[] coins = default;

    public bool onGround = true;
    public bool onLadder = false;
    public Animator charAnim;

    private int coinsCollected = 0;
    [SerializeField] private Text coinText;

    // Start is called before the first frame update
    void Start()
    {
        myRigid = GetComponent<Rigidbody2D>();
        charAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {

        }
        else
        {
            if (Input.GetKey(KeyCode.A))
            {
                myRigid.velocity = new Vector2(-speed, myRigid.velocity.y);
            }
            if (Input.GetKey(KeyCode.D))
            {
                myRigid.velocity = new Vector2(speed, myRigid.velocity.y);
            }
        }

        if(!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        { 
            myRigid.velocity = new Vector2(0, myRigid.velocity.y);
        }

        if (Input.GetKey(KeyCode.W))
        {
            if (onLadder)
            {
                myRigid.velocity = new Vector2(myRigid.velocity.x, speed);
                charAnim.SetBool("Climbing", true);
            }
            else if (onGround)
            {
                myRigid.AddForce(new Vector2(0, jumpForce));
            }
        }
        else
        {
            charAnim.SetBool("Climbing", false);
        }
    }

    public void coinCollected(GameObject hitCoin)
    {
        List<GameObject> nonVisibleCoins = new List<GameObject>();
        foreach(GameObject coin in coins)
        {
            if(coin.activeSelf == false)
            {
                nonVisibleCoins.Add(coin);
            }
        }
        int rand = Random.Range(0, nonVisibleCoins.Count);
        nonVisibleCoins[rand].SetActive(true);
        hitCoin.SetActive(false);
        coinsCollected += 1;
        coinText.text = coinsCollected.ToString();
    }
}
