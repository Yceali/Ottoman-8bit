using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SavedGame : MonoBehaviour
{
    [SerializeField]
    private Text dateTime;
    [SerializeField]
    private Image health;
    [SerializeField]
    private Image stamina;
    [SerializeField]
    private Image xp;
    [SerializeField]
    private Text healthText;
    [SerializeField]
    private Text staminaText;
    [SerializeField]
    private Text xpText;
    [SerializeField]
    private Text levelText;
    [SerializeField]
    private GameObject visuals;
    [SerializeField]
    private int index;

    public int MyIndex { get => index;}

    private void Awake()
    {
        visuals.SetActive(false);
    }

    public void ShowInfo(SaveData saveData)
    {
        visuals.SetActive(true);
        dateTime.text = "Tarih:" + saveData.MyDateTime.ToString("dd/MM/yy") + " - Saat:" + saveData.MyDateTime.ToString("H:mm");
        health.fillAmount = saveData.MyPlayerData.MyHealth / saveData.MyPlayerData.MyMaxHealth;
        healthText.text = saveData.MyPlayerData.MyHealth +" / "+ saveData.MyPlayerData.MyMaxHealth;

        stamina.fillAmount = saveData.MyPlayerData.MyStamina / saveData.MyPlayerData.MyMaxStamina;
        staminaText.text = saveData.MyPlayerData.MyStamina + " / " + saveData.MyPlayerData.MyMaxStamina;

        xp.fillAmount = saveData.MyPlayerData.MyExp / saveData.MyPlayerData.MyMaxExp;
        xpText.text = saveData.MyPlayerData.MyExp + " / " + saveData.MyPlayerData.MyMaxExp;

        levelText.text = saveData.MyPlayerData.MyLevel.ToString();
    }

    public void HideInfo()
    {
        visuals.SetActive(false);
    }
}
