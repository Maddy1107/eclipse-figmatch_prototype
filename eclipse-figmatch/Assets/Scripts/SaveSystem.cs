using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class SaveSystem
{
    private const string ScoreKey = "Save_Score";
    private const string TurnKey = "Save_Turns";
    private const string TimeKey = "Save_Time";
    private const string RowsKey = "Save_Rows";
    private const string ColsKey = "Save_Cols";
    private const string ComboKey = "Save_Combo";
    private const string MatchedKey = "Save_Matched";
    private const string ShuffledKey = "Save_Shuffled";

    public static void SaveProgress(
        int score,
        int turns,
        float time,
        int rows,
        int cols,
        int combo,
        List<int> matchedIDs,
        List<int> allCardIDs)
    {
        PlayerPrefs.SetInt(ScoreKey, score);
        PlayerPrefs.SetInt(TurnKey, turns);
        PlayerPrefs.SetFloat(TimeKey, time);
        PlayerPrefs.SetInt(RowsKey, rows);
        PlayerPrefs.SetInt(ColsKey, cols);
        PlayerPrefs.SetInt(ComboKey, combo);

        string matchedString = string.Join(",", matchedIDs);
        string shuffledString = string.Join(",", allCardIDs);

        PlayerPrefs.SetString(MatchedKey, matchedString);
        PlayerPrefs.SetString(ShuffledKey, shuffledString);

        PlayerPrefs.Save();
        Debug.Log("Game progress saved.");
    }

    public static int GetScore() => PlayerPrefs.GetInt(ScoreKey, 0);

    public static int GetTurns() => PlayerPrefs.GetInt(TurnKey, 0);

    public static float GetTime() => PlayerPrefs.GetFloat(TimeKey, 0f);

    public static int GetRows() => PlayerPrefs.GetInt(RowsKey, 2);

    public static int GetCols() => PlayerPrefs.GetInt(ColsKey, 2);

    public static int GetCombo() => PlayerPrefs.GetInt(ComboKey, 0);

    public static List<int> GetMatchedIDs()
    {
        string data = PlayerPrefs.GetString(MatchedKey, "");
        return string.IsNullOrEmpty(data) ? new List<int>() : data.Split(',').Select(int.Parse).ToList();
    }

    public static List<int> GetShuffledIDs()
    {
        string data = PlayerPrefs.GetString(ShuffledKey, "");
        return string.IsNullOrEmpty(data) ? new List<int>() : data.Split(',').Select(int.Parse).ToList();
    }

    public static bool HasSave() => PlayerPrefs.HasKey(ScoreKey) && PlayerPrefs.HasKey(ShuffledKey);

    public static void Clear()
    {
        PlayerPrefs.DeleteKey(ScoreKey);
        PlayerPrefs.DeleteKey(TurnKey);
        PlayerPrefs.DeleteKey(TimeKey);
        PlayerPrefs.DeleteKey(RowsKey);
        PlayerPrefs.DeleteKey(ColsKey);
        PlayerPrefs.DeleteKey(ComboKey);
        PlayerPrefs.DeleteKey(MatchedKey);
        PlayerPrefs.DeleteKey(ShuffledKey);
        Debug.Log("Save data cleared.");
    }
}
