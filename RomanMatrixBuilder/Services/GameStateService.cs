using Blazored.LocalStorage;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class GameStateService
{
    private readonly ISyncLocalStorageService _localStorage;

    public GameStateService(ISyncLocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public int CurrentCombo { get; private set; } = 0;
    public int MaxSessionCombo { get; private set; } = 0;

    // 現在のステージ (Stage 1, 2, 3...)
    public int CurrentStage { get; private set; } = 1;

    // 現在のセッションにおける正解問題数 (1セッション = 10問)
    public int SolvedInSession { get; private set; } = 0;

    // 現在のセッションにおけるタイピングの正確性
    public int SessionCorrectKeystrokes { get; private set; } = 0;
    public int SessionTotalKeystrokes { get; private set; } = 0;
    public double SessionAccuracy => SessionTotalKeystrokes == 0 ? 0 : (double)SessionCorrectKeystrokes / SessionTotalKeystrokes;

    public int ConsecutiveHighAccuracySessions { get; private set; } = 0;
    public Dictionary<string, int> CharacterCorrectCounts { get; private set; } = new();

    public string NextStageHint { get; private set; } = "";
    public bool StageJustCleared { get; private set; } = false;

    public void AddCombo()
    {
        CurrentCombo++;
        if (CurrentCombo > MaxSessionCombo)
        {
            MaxSessionCombo = CurrentCombo;
        }
    }

    public void ResetCombo()
    {
        CurrentCombo = 0;
    }

    public void GoalProblem(string primaryRomaji)
    {
        SolvedInSession++;

        if (!CharacterCorrectCounts.ContainsKey(primaryRomaji))
        {
            CharacterCorrectCounts[primaryRomaji] = 0;
        }
        CharacterCorrectCounts[primaryRomaji]++;

        if (IsSessionComplete)
        {
            EvaluateStageProgression();
        }
    }

    public void RegisterKeystroke(bool isCorrect)
    {
        SessionTotalKeystrokes++;
        if (isCorrect) SessionCorrectKeystrokes++;
    }

    private void EvaluateStageProgression()
    {
        StageJustCleared = false;
        NextStageHint = "";

        if (CurrentStage >= 4)
        {
            NextStageHint = "すべての行をマスターしました！";
            return;
        }

        bool highAccuracy = SessionTotalKeystrokes > 0 && SessionAccuracy >= 0.90;
        if (highAccuracy)
        {
            ConsecutiveHighAccuracySessions++;
        }
        else
        {
            ConsecutiveHighAccuracySessions = 0;
        }

        var requiredChars = GetRequiredCharsForStage(CurrentStage);
        int unmasteredCount = requiredChars.Count(c => !CharacterCorrectCounts.ContainsKey(c) || CharacterCorrectCounts[c] < 3);

        if (ConsecutiveHighAccuracySessions >= 2 && unmasteredCount == 0)
        {
            CurrentStage++;
            StageJustCleared = true;
            ConsecutiveHighAccuracySessions = 0;
            SaveData();
        }
        else
        {
            if (ConsecutiveHighAccuracySessions < 2 && unmasteredCount > 0)
            {
                NextStageHint = "正確にタイピングしつつ、色々な文字を練習していこう！";
            }
            else if (ConsecutiveHighAccuracySessions < 2)
            {
                NextStageHint = "文字の練習はバッチリ！あとは正確なタイピングを続けると次の行が解放されるかも？";
            }
            else
            {
                NextStageHint = "正確さは素晴らしい！まだ練習が足りない文字があるみたいだよ！";
            }
        }
    }

    private string[] GetRequiredCharsForStage(int stage)
    {
        var list = new List<string>();
        list.AddRange(new[] { "A", "I", "U", "E", "O" }); // Stage 1
        if (stage >= 2) list.AddRange(new[] { "KA", "KI", "KU", "KE", "KO", "SA", "SHI", "SU", "SE", "SO", "TA", "CHI", "TSU", "TE", "TO" });
        if (stage >= 3) list.AddRange(new[] { "NA", "NI", "NU", "NE", "NO", "HA", "HI", "FU", "HE", "HO", "MA", "MI", "MU", "ME", "MO" });
        if (stage >= 4) list.AddRange(new[] { "YA", "YU", "YO", "RA", "RI", "RU", "RE", "RO", "WA", "WO", "NN" });
        return list.ToArray();
    }

    public bool IsSessionComplete => SolvedInSession >= 10;

    public void ResetSession()
    {
        SolvedInSession = 0;
        CurrentCombo = 0;
        MaxSessionCombo = 0;
        SessionCorrectKeystrokes = 0;
        SessionTotalKeystrokes = 0;
        StageJustCleared = false;
        NextStageHint = "";
    }

    public void SetStage(int stage)
    {
        CurrentStage = stage;
        SaveData();
    }

    public void LoadData()
    {
        if (_localStorage.ContainKey("rmb_stage"))
        {
            CurrentStage = _localStorage.GetItem<int>("rmb_stage");
        }
        if (_localStorage.ContainKey("rmb_max_combo"))
        {
            MaxSessionCombo = _localStorage.GetItem<int>("rmb_max_combo");
        }
        if (_localStorage.ContainKey("rmb_char_counts"))
        {
            CharacterCorrectCounts = _localStorage.GetItem<Dictionary<string, int>>("rmb_char_counts") ?? new();
        }
        if (_localStorage.ContainKey("rmb_consecutive_high_acc"))
        {
            ConsecutiveHighAccuracySessions = _localStorage.GetItem<int>("rmb_consecutive_high_acc");
        }
    }

    public void SaveData()
    {
        _localStorage.SetItem("rmb_stage", CurrentStage);
        _localStorage.SetItem("rmb_max_combo", MaxSessionCombo);
        _localStorage.SetItem("rmb_char_counts", CharacterCorrectCounts);
        _localStorage.SetItem("rmb_consecutive_high_acc", ConsecutiveHighAccuracySessions);
    }
}
