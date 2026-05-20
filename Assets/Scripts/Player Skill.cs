using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerSkill : MonoBehaviour
{
    [SerializeField] private GameObject skillPrefab;
    [SerializeField] private Transform firePoint;
    private Player player;

    private bool canInput;
    private bool _isright;
    void Start()
    {
        StartCoroutine(EnableInput());
        player = this.GetComponent<Player>();
    }
    IEnumerator EnableInput()
    {
        yield return null;
        canInput = true;
    }
    void Update()
    {
        if (!canInput)
            return;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            _isright = player.Get_isFacingRight();
            GameObject Fireball = Instantiate(skillPrefab, firePoint.position, firePoint.rotation);

            
            Fireball.GetComponent<SkillFireball>().InitSkill(_isright, firePoint.position);
        }
    }
}
