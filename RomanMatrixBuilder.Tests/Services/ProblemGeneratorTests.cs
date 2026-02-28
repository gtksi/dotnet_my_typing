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

        for (int i = 0; i < 50; i++)
        {
            var problem = _generator.GenerateProblem(1);

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

        for (int i = 0; i < 200; i++)
        {
            var problem = _generator.GenerateProblem(2);
            Assert.Contains(problem.MatrixRow, allowedRows);
            foundRows.Add(problem.MatrixRow);
        }

        // 十分な回数試行すれば全行が出現するはず
        Assert.Equal(allowedRows, foundRows);
    }

    [Fact]
    public void GenerateProblem_ReturnsValidProblemStructure()
    {
        var problem = _generator.GenerateProblem(1);

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
        var problem = _generator.GenerateProblem(stage);

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
                var problem = _generator.GenerateProblem(stage);
                Assert.Contains(problem.PrimaryRomaji, problem.AllowedRomajiList);
            }
        }
    }
}
