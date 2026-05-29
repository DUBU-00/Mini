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
    public int potionCount = 10;
    public int maxpotionCount = 10;
    public int healAmount = 50;
    public float moveSpeed = 5f;

    void Awake()
    {
        InitDefaultStats();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F4))
        {
            AddExp(500);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Heal();
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
    public void Heal()
    {
        if (potionCount <= 0)
            return;
        if (currentHp >= maxHp)
            return;
        potionCount--;
        currentHp += healAmount;
        if(currentHp > maxHp)
        {
            currentHp = maxHp;
        }
    }
    public void FullRecovery()
    {
        currentHp = maxHp;
        potionCount = maxpotionCount;
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
    public void InitDefaultStats()
    {
        level = 1;
        currentExp = 0;
        maxExp = 100;
        maxHp = 100;
        currentHp = maxHp;
        NormalAttack = 6;
        HardAttack = 10;
        potionCount = 10;
        maxpotionCount = 10;
        healAmount = 50;
        moveSpeed = 5f;
    }
}
