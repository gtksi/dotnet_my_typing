namespace RomanMatrixBuilder.Models;

public class KeyboardState
{
    /// <summary>現在ユーザーが入力中の文字位置など</summary>
    public int CurrentInputIndex { get; set; } = 0;

    /// <summary>現在正解として打つべきキー（ルート提示用）</summary>
    public string ExpectedKey { get; set; } = "";

    /// <summary>直前に打鍵エラーがあったかどうか（視覚的フィードバック用）</summary>
    public bool HasError { get; set; } = false;

    /// <summary>最後に打たれてエラーになったキー（グレー沈み込み効果用）</summary>
    public string LastErrorKey { get; set; } = "";

    /// <summary>ナビゲーション線で光らせる対象のキー（例えば子音の次に打つ母音）</summary>
    public string NavigationTargetKey { get; set; } = "";
}
