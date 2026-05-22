using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip villageBGM;

    [SerializeField]
    private AudioClip dungeonBGM;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource.loop = true;
            audioSource.playOnAwake = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayVillage()
    {
        if (audioSource.clip == villageBGM && audioSource.isPlaying) 
            return;

        audioSource.Stop();
        audioSource.clip = villageBGM;
        audioSource.Play();
    }

    public void PlayDungeon()
    {
        if (audioSource.clip == dungeonBGM && audioSource.isPlaying) 
            return;

        audioSource.Stop();
        audioSource.clip = dungeonBGM;
        audioSource.Play();
    }
}