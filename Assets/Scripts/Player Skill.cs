using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerSkill : MonoBehaviour
{
    [SerializeField] private GameObject skillPrefab;
    [SerializeField] private Transform firePoint;
    private Player player;
    private PlayerStats stats;
    private PlayerHealth health;

    private bool canInput;
    private bool _isright;
    void Start()
    {
        StartCoroutine(EnableInput());
        player = this.GetComponent<Player>();
        stats = this.GetComponent<PlayerStats>();
        health = this.GetComponent<PlayerHealth>();
    }
    IEnumerator EnableInput()
    {
        yield return null;
        canInput = true;
    }
    void Update()
    {
        if (health != null && health.IsDie())
            return;

        if (!canInput)
            return;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            _isright = player.Get_isFacingRight();
            GameObject Fireball = Instantiate(skillPrefab, firePoint.position, firePoint.rotation);

            GetComponent<PlayerAudio>().PlaySkillAttack();

            SkillFireball fireballComponent = Fireball.GetComponent<SkillFireball>();
            if (fireballComponent != null)
            {
                fireballComponent.InitSkill(_isright, firePoint.position, stats);
            }
        }
    }
}
