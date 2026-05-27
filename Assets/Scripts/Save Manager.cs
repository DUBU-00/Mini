using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    private string savePath;
    private void Awake()
    {
        Instance = this;
        savePath = Application.persistentDataPath + "/save.json";
    }
    private void Start()
    {
        LoadGame();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            DeleteSave();
        }
    }
    private void OnApplicationQuit()
    {
        SaveGame();
    }
    public void SaveGame()
    {
        PlayerStats player = GameManager.Instance.playerStats;
        SaveData data = new SaveData();

        data.level = player.level;
        data.currentExp = player.currentExp;
        data.maxExp = player.maxExp;

        data.maxHp = player.maxHp;
        data.currentHp = player.currentHp;

        data.attack1 = player.NormalAttack;
        data.attack2 = player.HardAttack;

        data.posX = player.transform.position.x;
        data.posY = player.transform.position.y;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);
    }

    public void LoadGame()
    {
        if(File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            PlayerStats player = GameManager.Instance.playerStats;
            player.level = data.level;
            player.currentExp = data.currentExp;
            player.maxExp = data.maxExp;

            player.maxHp = data.maxHp;
            player.currentHp = data.currentHp;

            player.NormalAttack = data.attack1;
            player.HardAttack = data.attack2;

            player.transform.position = new Vector3(data.posX, data.posY, 0);
        }
    }
    public void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
        }
        SceneManager.LoadScene(0);
    }
}
