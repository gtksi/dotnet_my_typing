namespace RomanMatrixBuilder.Models;

public class TypingProblem
{
    /// <summary>出題されるひらがな</summary>
    public string Hiragana { get; set; } = "";

    /// <summary>画面に表示される優先度の高い正解ローマ字（ヘボン式ベース）</summary>
    public string PrimaryRomaji { get; set; } = "";

    /// <summary>入力として許可されるすべてのローマ字パターンのリスト（ヘボン式・訓令式等）</summary>
    public List<string> AllowedRomajiList { get; set; } = new();

    /// <summary>この問題が属する子音行（マトリクスの管理用）</summary>
    public string MatrixRow { get; set; } = "";
}
