using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    private AudioSource audioSource;

    public AudioClip normalAttackClip;
    public AudioClip hardAttackClip;
    public AudioClip dashClip;
    public AudioClip skillAttackClip;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayNormalAttack()
    {
        PlaySound(normalAttackClip);
    }
    public void PlayHardAttack()
    {
        PlaySound(hardAttackClip);
    }
    public void PlayDash()
    {
        PlaySound(dashClip);
    }
    public void PlaySkillAttack()
    {
        PlaySound(skillAttackClip);
    }
    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
