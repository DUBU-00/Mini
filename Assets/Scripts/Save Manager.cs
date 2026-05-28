using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    private string savePath;
    public static SaveData loadedData;
    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        savePath = Application.persistentDataPath + "/save.json";
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
        if (GameManager.Instance == null || GameManager.Instance.playerStats == null)
            return;

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
            loadedData = JsonUtility.FromJson<SaveData>(json);
        }
    }
    public void ApplyLoadedGame()
    {
        LoadGame();

        SaveData data = loadedData;

        if (loadedData == null)
        {
            Debug.LogWarning("세이브 파일이 존재하지 않습니다. 새 게임용 기본 데이터를 생성합니다.");
            loadedData = new SaveData();

            loadedData.level = 1;
            loadedData.maxHp = 100;
            loadedData.currentHp = 0;
            loadedData.maxExp = 100;
            loadedData.currentExp = 0;
            loadedData.posX = -3.43f;
            loadedData.posY = -2.77f;
            loadedData.attack1 = 4;
            loadedData.attack2 = 6;
        }

        if (GameManager.Instance == null || GameManager.Instance.playerStats == null)
        {
            Debug.LogError("GameManager나 playerStats가 씬에 존재하지 않아 데이터를 적용할 수 없습니다.");
            return;
        }

        PlayerStats player = GameManager.Instance.playerStats;
        player.level = loadedData.level;
        player.currentExp = loadedData.currentExp;
        player.maxExp = loadedData.maxExp;

        player.maxHp = loadedData.maxHp;
        player.currentHp = loadedData.currentHp;

        player.NormalAttack = loadedData.attack1;
        player.HardAttack = loadedData.attack2;

        player.transform.position = new Vector3(loadedData.posX, loadedData.posY, 0);
        Physics2D.SyncTransforms();
        Camera.main.GetComponent<CameraFollow>().MoveInstant();
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
