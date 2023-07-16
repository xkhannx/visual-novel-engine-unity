using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public int money;

    public void SaveGame()
    {
        PlayerPrefs.SetInt("money", money);
    }

    public void LoadGame()
    {
        money = PlayerPrefs.GetInt("money", 0);
    }

    public void UpdateMoney(int change)
    {
        money += change;
        SaveGame();
    }
}
