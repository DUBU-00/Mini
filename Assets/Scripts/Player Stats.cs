using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private int expReward = 0;

    public int level = 1;
    public int currentExp = 0;
    public int currentHp;
    public int maxExp = 100;
    public int maxHp = 100;
    public int attack1 = 6;
    public int attack2 = 10;

    void Start()
    {
        currentHp = maxHp;
    }
    private void Die()
    {
        PlayerStats player = FindAnyObjectByType<PlayerStats>();
        if (player != null)
        {
            player.AddExp(expReward);
        }
        Destroy(gameObject);
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
        attack1 += 2;
        attack2 += 4;
    }
}
