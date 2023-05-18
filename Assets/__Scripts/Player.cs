using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public static class Player
{
    private static int _scoreCount;
    private static int _coinsCount;
    private static int _startLivesCount;
    private static int _weaponsCount;
    private static int _fireWavesCount;
    private static int _highScore;
    private static int _maxLevel;
    private static int _playersBestLevel;
    private static int _playersBestScore;
    private static int _playersBestLevel_2;
    private static int _playersBestScore_2;
    private static int _playersBestLevel_3;
    private static int _playersBestScore_3;
    private static string _plName;
    public static void LoadPlayersStats(string plNum)
    {
        plName = plNum;

        if (PlayerPrefs.HasKey("startLivesCountPl_" + plName))
        {
            scoreCount = 0;
            coinsCount = PlayerPrefs.GetInt("CoinsPl_" + plName);
            weaponsCount = PlayerPrefs.GetInt("ActiveWeaponsPl_" + plName);
            startLivesCount = PlayerPrefs.GetInt("startLivesCountPl_" + plName);
            fireWavesCount = PlayerPrefs.GetInt("FireWavesCountPl_" + plName);
            highScore = PlayerPrefs.GetInt("HighScore");
            maxLevel = PlayerPrefs.GetInt("MaxLevel");
            playersBestLevel = PlayerPrefs.GetInt("BestLevelPl_" + plName);
            playersBestScore = PlayerPrefs.GetInt("BestScorePl_" + plName);
        }
        else
        {
            DefaulData();
        }
    }
    public static int maxLevel
    {
        get { return _maxLevel; }
        set
        {
            _maxLevel = value;
            PlayerPrefs.SetInt("MaxLevel", maxLevel);
        }
    }
    public static int playersBestLevel
    {
        get { return _playersBestLevel; }
        set
        {
            _playersBestLevel = value;
            PlayerPrefs.SetInt("BestLevelPl_" + plName, playersBestLevel);
        }

    }
    public static int playersBestScore
    {
        get { return _playersBestScore; }
        set
        {
            _playersBestScore = value;
            PlayerPrefs.SetInt("BestScorePl_" + plName, playersBestScore);
        }
    }
    public static int scoreCount
    {
        get { return _scoreCount; }
        set
        {
            _scoreCount = value;
            PlayerPrefs.SetInt("ScorePl_" + plName, _scoreCount);
        }
    }
    public static int coinsCount
    {
        get { return _coinsCount; }
        set
        {
            _coinsCount = value;
            PlayerPrefs.SetInt("CoinsPl_" + plName, coinsCount);
        }
    }
    public static int startLivesCount
    {
        get { return _startLivesCount; }
        set
        {
            _startLivesCount = value;
            PlayerPrefs.SetInt("startLivesCountPl_" + plName, startLivesCount);
        }
    }
    public static int weaponsCount
    {
        get { return _weaponsCount; }
        set
        {
            _weaponsCount = value;
            PlayerPrefs.SetInt("ActiveWeaponsPl_" + plName, weaponsCount);
        }
    }
    public static int fireWavesCount
    {
        get { return _fireWavesCount; }
        set
        {
            _fireWavesCount = value;
            PlayerPrefs.SetInt("FireWavesCountPl_" + plName, fireWavesCount);
        }
    }
    public static int highScore
    {
        get { return _highScore; }
        set
        {
            _highScore = value;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
    }
    public static string plName
    {
        get { return _plName; }
        set { _plName = value; }
    }
    public static void DefaulData()
    {
        scoreCount = 0;
        coinsCount = 0;
        startLivesCount = 1;
        weaponsCount = 3;
        fireWavesCount = 0;
        playersBestLevel = 0;
        playersBestScore = 0;
    }
    public static void SaveGame()
    {
        PlayerPrefs.SetInt("ScorePl_" + plName, scoreCount);
        PlayerPrefs.SetInt("CoinsPl_" + plName, coinsCount);
        PlayerPrefs.SetInt("ActiveWeaponsPl_" + plName, weaponsCount);
        PlayerPrefs.SetInt("startLivesCountPl_" + plName, startLivesCount);
        PlayerPrefs.SetInt("FireWavesCountPl_" + plName, fireWavesCount);
        PlayerPrefs.SetInt("HighScore", highScore);
    }
    public static void DeleteData()
    {
        PlayerPrefs.DeleteAll();
    }
}
