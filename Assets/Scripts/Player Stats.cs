using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int level = 1;
    public int currentExp = 0;
    public int currentHp;
    public int currentMp;
    public int maxExp = 100;
    public int maxHp = 100;
    public int maxMp = 100;
    public int NormalAttack = 6;
    public int HardAttack = 10;
    public int hpPotionCount = 10;
    public int maxHpPotionCount = 10;
    public int hpHealAmount = 50;
    public int mpPotionCount = 10;
    public int maxMpPotionCount = 10;
    public int mpHealAmount = 20;
    public float moveSpeed = 5f;
    public int skillPoints = 0;
    public int fireballLevel = 1;
    public const int MAX_FIREBALL_LEVEL = 20;

    public int FireballDamage
    {
        get
        {
            float damageRatio = Mathf.Lerp(0.5f, 1.5f, (MAX_FIREBALL_LEVEL - 1) / 19f);
            return Mathf.FloorToInt(NormalAttack * damageRatio);
        }
    }
    void Awake()
    {
        InitDefaultStats();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F4))
        {
            AddExp(1000);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            HealHp();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            HealMp();
        }
    }
    public void AddExp(int amount)
    {
        currentExp += amount;
        while (currentExp >= maxExp)
        {
            LevelUP();
        }
        if (LogManager.Instance != null)
        {
            LogManager.Instance.ShowExpLog(amount);
        }
    }
    public void HealHp()
    {
        if (hpPotionCount <= 0)
            return;
        if (currentHp >= maxHp)
            return;
        hpPotionCount--;
        currentHp += hpHealAmount;
        if(currentHp > maxHp)
        {
            currentHp = maxHp;
        }
    }
    public void HealMp()
    {
        if (mpPotionCount <= 0)
            return;
        if (currentMp >= maxMp)
            return;
        mpPotionCount--;
        currentMp += mpHealAmount;
        if (currentMp > maxMp)
        {
            currentMp = maxMp;
        }
    }
    public void FullRecovery()
    {
        currentHp = maxHp;
        currentMp = maxMp;
        hpPotionCount = maxHpPotionCount;
        mpPotionCount = maxMpPotionCount;
    }
    private void LevelUP()
    {
        currentExp -= maxExp;
        level++;
        skillPoints += 1;
        maxExp += 50;
        maxHp += 20;
        maxMp += 10;
        NormalAttack += 2;
        HardAttack += 4;
        moveSpeed += 0.2f;
        currentHp = maxHp;
        currentMp = maxMp;
        SkillUI skillUI = FindFirstObjectByType<SkillUI>();
        if (skillUI != null)
        {
            skillUI.UpdateSkillUI();
        }
    }
    public bool UpgradeFireball()
    {
        if (skillPoints > 0 && fireballLevel < MAX_FIREBALL_LEVEL)
        {
            skillPoints--;
            fireballLevel++;
            Debug.Log($"파이어볼 레벨업! 현재 레벨: {fireballLevel}, 데미지: {FireballDamage}");
            return true;
        }
        return false;
    }
    public void InitDefaultStats()
    {
        level = 1;
        currentExp = 0;
        maxExp = 100;
        maxHp = 100;
        currentHp = maxHp;
        maxMp = 100;
        currentMp = maxMp;
        NormalAttack = 6;
        HardAttack = 10;
        hpPotionCount = 10;
        maxHpPotionCount = 10;
        hpHealAmount = 50;
        mpPotionCount = 10;
        maxMpPotionCount = 10;
        mpHealAmount = 20;
        moveSpeed = 5f;
        skillPoints = 0;
        fireballLevel = 1;
    }
}
