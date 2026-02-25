using Blazored.LocalStorage;
using System.Threading.Tasks;

namespace RomanMatrixBuilder.Services;

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

    public void GoalProblem()
    {
        SolvedInSession++;
    }

    public bool IsSessionComplete => SolvedInSession >= 10;

    public void ResetSession()
    {
        SolvedInSession = 0;
        CurrentCombo = 0;
        MaxSessionCombo = 0;
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
    }

    public void SaveData()
    {
        _localStorage.SetItem("rmb_stage", CurrentStage);
        _localStorage.SetItem("rmb_max_combo", MaxSessionCombo);
    }
}
