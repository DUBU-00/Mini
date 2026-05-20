using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerSkill : MonoBehaviour
{
    [SerializeField] private GameObject skillPrefab;
    [SerializeField] private Transform firePoint;

    private bool canInput;

    void Start()
    {
        StartCoroutine(EnableInput());
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
            Instantiate(skillPrefab, firePoint.position, firePoint.rotation);
        }
    }
}
