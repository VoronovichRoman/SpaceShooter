using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public GameObject exitButton;
    public GameObject newGButton;
    public GameObject storeButton;
    public GameObject controlsButton;
    public GameObject continueButton;
    public GameObject backButton;
    public GameObject plButton1;
    public GameObject plButton2;
    public GameObject plButton3;
    public GameObject warningText;
    public GameObject yesButton;
    public GameObject noButton;
    private int oneLifePrice;
    private int additionalWeaponPrice;
    private int fireWavePrice;
    private int pMenu;
    private int[] fireWavePriceAr;
    public void Awake()
    {
        fireWavePriceAr = new int[15] { 1, 2, 3, 5, 8, 13, 21, 34, 55, 89, 144, 233, 377, 610, 987 };
    }
    public void Start()
    {
        SelectPlayerMenu();
        if (!Cursor.visible)
        {
            Cursor.visible = true;
        }
    }
    public void SelectPlayerMenu()
    {
        newGButton.SetActive(false);
        storeButton.SetActive(false);
        continueButton.SetActive(false);
        controlsButton.SetActive(false);
        backButton.SetActive(false);
        warningText.SetActive(false);
        yesButton.SetActive(false);
        noButton.SetActive(false);

        plButton1.SetActive(true);
        plButton2.SetActive(true);
        plButton3.SetActive(true);
    }
    public void MainMenu()
    {
        plButton1.SetActive(false);
        plButton2.SetActive(false);
        plButton3.SetActive(false);
        warningText.SetActive(false);
        yesButton.SetActive(false);
        noButton.SetActive(false);

        newGButton.SetActive(true);
        storeButton.SetActive(true);
        controlsButton.SetActive(true);
        backButton.SetActive(true);
        exitButton.SetActive(true);

        if (Player.coinsCount > 0 || Player.startLivesCount > 1 || Player.weaponsCount > 3 || Player.fireWavesCount > 0)
        {
            continueButton.SetActive(true);
        }

        pMenu = 0;
    }
    public void BackButton()
    {
        if (pMenu == 0)
        {
            SelectPlayerMenu();
        }
        else if (pMenu == 1)
        {
            MainMenu();
        }
    }
    public void LoadPlayer(string num)
    {
        Player.LoadPlayersStats(num);
    }
    public void NewGame(bool confidence = false)
    {
        if (!confidence && (Player.coinsCount > 0 || Player.startLivesCount > 1 || Player.weaponsCount > 3 || Player.fireWavesCount > 0))
        {
            warningText.SetActive(true);
            yesButton.SetActive(true);
            noButton.SetActive(true);

            continueButton.SetActive(false);
            newGButton.SetActive(false);
            storeButton.SetActive(false);
            controlsButton.SetActive(false);
            backButton.SetActive(false);
            exitButton.SetActive(false);
        }
        else
        {
            Player.DefaulData();
            SceneManager.LoadScene("Game");
        }
    }
    public void ContinueGame()
    {
        SceneManager.LoadScene("Game");
    }
    public void StoreMenu()
    {
        continueButton.SetActive(false);
        newGButton.SetActive(false);
        storeButton.SetActive(false);
        controlsButton.SetActive(false);
        exitButton.SetActive(false);

        backButton.SetActive(true);

        pMenu = 1;

        StoreCheck();
    }
    public void StoreCheck()
    {
        GameObject livesInShopGo = GameObject.Find("BuyLivesButton");
        Text lifePriceText = GameObject.Find("LifePriceText").GetComponent<Text>();
        GameObject weaponsCountInShopGo = GameObject.Find("AddWeaponButton");
        Text addWeaponPriceText = GameObject.Find("AddWeaponPriceText").GetComponent<Text>();
        GameObject fireWaveInShopGo = GameObject.Find("BuyFireWaveButton");
        Text fireWavePriceText = GameObject.Find("FireWavePriceText").GetComponent<Text>();
        if (Player.startLivesCount == 3)
        {
            lifePriceText.text = "  Price:\n";
            livesInShopGo.GetComponentInChildren<Text>().text = "MAX";
            livesInShopGo.GetComponent<Button>().interactable = false;
        }
        else
        {
            if (Player.startLivesCount == 1)
            {
                oneLifePrice = 300;
            }
            else if (Player.startLivesCount == 2)
            {
                oneLifePrice = 1500;
            }
            lifePriceText.text = "  Price:\n" + oneLifePrice;
            livesInShopGo.GetComponentInChildren<Text>().text = "BUY";
            livesInShopGo.GetComponent<Button>().interactable = true;
            if (Player.coinsCount < oneLifePrice)
            {
                livesInShopGo.GetComponent<Button>().interactable = false;
            }
        }
        if (Player.weaponsCount == 5)
        {
            addWeaponPriceText.text = "  Price:";
            weaponsCountInShopGo.GetComponentInChildren<Text>().text = "MAX";
            weaponsCountInShopGo.GetComponent<Button>().interactable = false;
        }
        else
        {
            if (Player.weaponsCount == 3)
            {
                additionalWeaponPrice = 300;
            }
            else if (Player.weaponsCount == 4)
            {
                additionalWeaponPrice = 1500;
            }
            addWeaponPriceText.text = "  Price:\n" + additionalWeaponPrice;
            weaponsCountInShopGo.GetComponentInChildren<Text>().text = "BUY";
            weaponsCountInShopGo.GetComponent<Button>().interactable = true;
            if (Player.coinsCount < additionalWeaponPrice)
            {
                weaponsCountInShopGo.GetComponent<Button>().interactable = false;
            }
        }
        fireWaveInShopGo.GetComponent<Button>().interactable = true;
        fireWavePrice = FireWavePriceCalculation(Player.fireWavesCount);
        fireWavePriceText.text = "  Price:\n" + fireWavePrice;
        fireWaveInShopGo.GetComponentInChildren<Text>().text = "BUY";
        if (Player.coinsCount < fireWavePrice)
        {
            fireWaveInShopGo.GetComponent<Button>().interactable = false;
        }
    }
    public void BuyLife()
    {
        if (Player.coinsCount >= oneLifePrice)
        {
            Player.startLivesCount++;
            Player.coinsCount -= oneLifePrice;
        }
        StoreCheck();
    }
    public void BuyAdditionalWeapon()
    {
        if (Player.coinsCount >= additionalWeaponPrice)
        {
            Player.weaponsCount++;
            Player.coinsCount -= additionalWeaponPrice;
        }
        StoreCheck();
    }
    public void BuyFireWave()
    {
        if (Player.coinsCount >= fireWavePrice)
        {
            Player.fireWavesCount++;
            Player.coinsCount -= fireWavePrice;
        }
        StoreCheck();
    }
    public int FireWavePriceCalculation(int n)
    {
        if (n >= 14)
        {
            n = 14;
        }
        n = fireWavePriceAr[n];
        return n * 50;
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void DeleteData()
    {
        Player.DeleteData();
    }
}
