using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    public int level = 1;
    public int currentExp = 0;
    public int currentHp;
    public int maxExp = 100;
    public int maxHp = 100;
    public int NormalAttack = 6;
    public int HardAttack = 10;
    public float moveSpeed = 5f;

    void Start()
    {
        currentHp = maxHp;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F4))
        {
            AddExp(500);
        }
    }
    public void AddExp(int amount)
    {
        currentExp += amount;
        while (currentExp >= maxExp)
        {
            LevelUP();
        }
    }
    private void LevelUP()
    {
        currentExp -= maxExp;
        level++;
        maxExp += 50;
        maxHp += 20;
        NormalAttack += 2;
        HardAttack += 4;
        moveSpeed += 0.2f;
        currentHp = maxHp;
    }
}
