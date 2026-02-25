using RomanMatrixBuilder.Models;

namespace RomanMatrixBuilder.Services;

public class ProblemGenerator
{
    private readonly Random _random = new();

    // ステージごとの解放行定義
    private readonly Dictionary<string, (string[] Hiragana, string[] Primary, List<string>[] Allowed)> _matrixData = new()
    {
        // Stage 1: 母音のみ
        { "", (
            new[] { "あ", "い", "う", "え", "お" },
            new[] { "A", "I", "U", "E", "O" },
            new[] { new List<string> { "A" }, new List<string> { "I" }, new List<string> { "U" }, new List<string> { "E" }, new List<string> { "O" } }
        )},
        // Stage 2: か行, さ行, た行
        { "K", (
            new[] { "か", "き", "く", "け", "こ" },
            new[] { "KA", "KI", "KU", "KE", "KO" },
            new[] { new List<string> { "KA", "CA" }, new List<string> { "KI" }, new List<string> { "KU", "CU", "QU" }, new List<string> { "KE" }, new List<string> { "KO", "CO" } }
        )},
        { "S", (
            new[] { "さ", "し", "す", "せ", "そ" },
            new[] { "SA", "SHI", "SU", "SE", "SO" },
            new[] { new List<string> { "SA" }, new List<string> { "SHI", "SI", "CI" }, new List<string> { "SU" }, new List<string> { "SE", "CE" }, new List<string> { "SO" } }
        )},
        { "T", (
            new[] { "た", "ち", "つ", "て", "と" },
            new[] { "TA", "CHI", "TSU", "TE", "TO" },
            new[] { new List<string> { "TA" }, new List<string> { "CHI", "TI" }, new List<string> { "TSU", "TU" }, new List<string> { "TE" }, new List<string> { "TO" } }
        )},
        // Stage 3: な行, は行, ま行
        { "N", (
            new[] { "な", "に", "ぬ", "ね", "の" },
            new[] { "NA", "NI", "NU", "NE", "NO" },
            new[] { new List<string> { "NA" }, new List<string> { "NI" }, new List<string> { "NU" }, new List<string> { "NE" }, new List<string> { "NO" } }
        )},
        { "H", (
            new[] { "は", "ひ", "ふ", "へ", "ほ" },
            new[] { "HA", "HI", "FU", "HE", "HO" },
            new[] { new List<string> { "HA" }, new List<string> { "HI" }, new List<string> { "FU", "HU" }, new List<string> { "HE" }, new List<string> { "HO" } }
        )},
        { "M", (
            new[] { "ま", "み", "む", "め", "も" },
            new[] { "MA", "MI", "MU", "ME", "MO" },
            new[] { new List<string> { "MA" }, new List<string> { "MI" }, new List<string> { "MU" }, new List<string> { "ME" }, new List<string> { "MO" } }
        )},
        // Stage 4: や行, ら行, わ行
        { "Y", (
            new[] { "や", "ゆ", "よ" },
            new[] { "YA", "YU", "YO" },
            new[] { new List<string> { "YA" }, new List<string> { "YU" }, new List<string> { "YO" } }
        )},
        { "R", (
            new[] { "ら", "り", "る", "れ", "ろ" },
            new[] { "RA", "RI", "RU", "RE", "RO" },
            new[] { new List<string> { "RA" }, new List<string> { "RI" }, new List<string> { "RU" }, new List<string> { "RE" }, new List<string> { "RO" } }
        )},
        { "W", (
            new[] { "わ", "を", "ん" },
            new[] { "WA", "WO", "NN" },
            new[] { new List<string> { "WA" }, new List<string> { "WO" }, new List<string> { "NN", "N" } }
        )}
    };

    public TypingProblem GenerateProblem(int currentStage)
    {
        // 現在のステージで解放されている行(Key)を取得
        var availableRows = new List<string>();
        availableRows.Add(""); // Stage 1 (All)

        if (currentStage >= 2) availableRows.AddRange(new[] { "K", "S", "T" });
        if (currentStage >= 3) availableRows.AddRange(new[] { "N", "H", "M" });
        if (currentStage >= 4) availableRows.AddRange(new[] { "Y", "R", "W" });

        // ランダムに行を選ぶ
        var selectedRowKey = availableRows[_random.Next(availableRows.Count)];
        var rowData = _matrixData[selectedRowKey];

        // ランダムにその行の文字（母音）を選ぶ
        var charIndex = _random.Next(rowData.Hiragana.Length);

        return new TypingProblem
        {
            Hiragana = rowData.Hiragana[charIndex],
            PrimaryRomaji = rowData.Primary[charIndex],
            AllowedRomajiList = rowData.Allowed[charIndex],
            MatrixRow = selectedRowKey
        };
    }
}
