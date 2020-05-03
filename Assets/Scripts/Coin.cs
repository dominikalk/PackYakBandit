using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private bool canHit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        canHit = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Character") && canHit)
        {
            canHit = false;
            FindObjectOfType<Character>().coinCollected(gameObject);
            FindObjectOfType<CoinSound>().playSound();
        }
    }
}
