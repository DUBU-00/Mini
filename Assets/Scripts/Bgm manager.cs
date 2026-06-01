using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip villageBGM;
    [SerializeField] private AudioClip dungeonBGM;
    [SerializeField] private AudioClip dungeon_3BGM;
    [SerializeField] private AudioClip bossBGM;

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

    public void PlayVillage() => PlayBGM(villageBGM);
    public void PlayDungeon() => PlayBGM(dungeonBGM);
    public void PlayDungeon_3() => PlayBGM(dungeon_3BGM);
    public void PlayBoss() => PlayBGM(bossBGM);

    public void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;
        if (audioSource.clip == clip && audioSource.isPlaying) 
            return;

        audioSource.Stop();
        audioSource.clip = clip;
        audioSource.Play();
    }
}