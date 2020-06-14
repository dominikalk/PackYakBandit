using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public static MusicController Instance { get; private set; }

    public AudioClip mainLoop;
    public AudioClip deathLoop;

    [HideInInspector] public AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.M))
        {
            if (FindObjectOfType<Camera>().GetComponent<AudioListener>())
            {
                Destroy(FindObjectOfType<Camera>().GetComponent<AudioListener>());
            }
            else
            {
                FindObjectOfType<Camera>().gameObject.AddComponent<AudioListener>();
            }
        }*/
    }

    public void died()
    {
        StartCoroutine(fadeInOutAudio(deathLoop, 0.25f));
        StartCoroutine(fadeOutAudio(GameObject.FindGameObjectWithTag("Ambient").GetComponent<AudioSource>(), 0.25f));
    }

    IEnumerator fadeOutAudio(AudioSource theAudioSource, float duration)
    {
        float timeElepsed = 0;
        while (timeElepsed < duration)
        {
            audioSource.volume = Mathf.Lerp(1, 0, timeElepsed / duration);
            timeElepsed += Time.unscaledDeltaTime;
            yield return null;
        }
        theAudioSource.Stop();
    }

    public IEnumerator fadeInOutAudio(AudioClip toAudio, float duration)
    {
        float timeElepsed = 0;
        while(timeElepsed < duration)
        {
            audioSource.volume = Mathf.Lerp(1, 0, timeElepsed / duration);
            timeElepsed += Time.unscaledDeltaTime;
            yield return null;
        }
        audioSource.clip = toAudio;
        audioSource.Play();
        timeElepsed = 0;
        while (timeElepsed < duration)
        {
            audioSource.volume = Mathf.Lerp(0, 1, timeElepsed / duration);
            timeElepsed += Time.unscaledDeltaTime;
            yield return null;
        }
    }

    void shuffleArray(AudioClip[] clips)
    {
        for (int i = 0; i < clips.Length; i++)
        {
            AudioClip temp = clips[i];
            int r = Random.Range(i, clips.Length);
            clips[i] = clips[r];
            clips[r] = temp;
        }
    }
}
