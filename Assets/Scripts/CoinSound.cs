using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSound : MonoBehaviour
{
    [SerializeField] private AudioClip[] coinClips;
    private AudioSource coinSource;

    // Start is called before the first frame update
    void Start()
    {
        coinSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playSound()
    {
        int random = Random.Range(0, coinClips.Length);
        coinSource.clip = coinClips[random];
        coinSource.Play();
    }
}
