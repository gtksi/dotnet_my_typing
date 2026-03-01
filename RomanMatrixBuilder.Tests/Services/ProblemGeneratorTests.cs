using RomanMatrixBuilder.Services;
using RomanMatrixBuilder.Models;

namespace RomanMatrixBuilder.Tests.Services;

public class ProblemGeneratorTests
{
    private readonly ProblemGenerator _generator = new();

    [Fact]
    public void GenerateProblem_Stage1_ReturnsVowelOnly()
    {
        // Stage 1 は母音のみ出題される
        var vowels = new HashSet<string> { "A", "I", "U", "E", "O" };
        var charCounts = new Dictionary<string, int>();

        for (int i = 0; i < 50; i++)
        {
            var problem = _generator.GenerateProblem(1, charCounts);

            Assert.Contains(problem.PrimaryRomaji, vowels);
            Assert.Equal("", problem.MatrixRow); // 母音行は空文字列
        }
    }

    [Fact]
    public void GenerateProblem_Stage2_IncludesKSTRows()
    {
        // Stage 2 は母音 + K, S, T 行が出題される
        var allowedRows = new HashSet<string> { "", "K", "S", "T" };
        var foundRows = new HashSet<string>();
        var charCounts = new Dictionary<string, int>();

        for (int i = 0; i < 200; i++)
        {
            var problem = _generator.GenerateProblem(2, charCounts);
            Assert.Contains(problem.MatrixRow, allowedRows);
            foundRows.Add(problem.MatrixRow);
        }

        // 十分な回数試行すれば全行が出現するはず
        Assert.Equal(allowedRows, foundRows);
    }

    [Fact]
    public void GenerateProblem_ReturnsValidProblemStructure()
    {
        var charCounts = new Dictionary<string, int>();
        var problem = _generator.GenerateProblem(1, charCounts);

        Assert.False(string.IsNullOrEmpty(problem.Hiragana));
        Assert.False(string.IsNullOrEmpty(problem.PrimaryRomaji));
        Assert.NotEmpty(problem.AllowedRomajiList);
        Assert.Contains(problem.PrimaryRomaji, problem.AllowedRomajiList);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    public void GenerateProblem_AllStages_ReturnsNonNull(int stage)
    {
        var charCounts = new Dictionary<string, int>();
        var problem = _generator.GenerateProblem(stage, charCounts);

        Assert.NotNull(problem);
        Assert.NotNull(problem.AllowedRomajiList);
    }

    [Fact]
    public void GenerateProblem_AllowedRomajiList_ContainsPrimaryRomaji()
    {
        // すべてのステージでPrimaryRomajiがAllowedRomajiListに含まれることを確認
        for (int stage = 1; stage <= 4; stage++)
        {
            for (int i = 0; i < 50; i++)
            {
                var charCounts = new Dictionary<string, int>();
                var problem = _generator.GenerateProblem(stage, charCounts);
                Assert.Contains(problem.PrimaryRomaji, problem.AllowedRomajiList);
            }
        }
    }

    [Fact]
    public void GenerateProblem_PrioritizesUnmasteredCharacters()
    {
        // A, I, U, E は3回正解（習熟済み）とする
        var charCounts = new Dictionary<string, int>
        {
            { "A", 3 }, { "I", 3 }, { "U", 3 }, { "E", 3 }
        };

        // O だけ未習熟の場合、高確率でOが選ばれるはず（80%以上）
        int oCount = 0;
        int trials = 100;
        for (int i = 0; i < trials; i++)
        {
            var problem = _generator.GenerateProblem(1, charCounts);
            if (problem.PrimaryRomaji == "O")
            {
                oCount++;
            }
        }

        // ランダム要素（80%の確率で未習熟から選定、20%は全体から）のため、
        // Oが選ばれる確率は 0.8 * 1.0 + 0.2 * 0.2 = 0.84 程度になる。余白を持たせて60回以上ならよしとする。
        Assert.True(oCount > 60, $"Expected high frequency of O, but got {oCount}/{trials}");
    }
}
