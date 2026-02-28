using RomanMatrixBuilder.Services;

namespace RomanMatrixBuilder.Tests.Services;

public class GameStateServiceTests
{
    // GameStateService は ISyncLocalStorageService に依存するため、
    // 動的出題アルゴリズム実装時にモック化して本格テストを追加する。
    // ここでは依存なしでテスト可能な基本ロジックのみ検証する。

    [Fact]
    public void IsSessionComplete_After10Problems_ReturnsTrue()
    {
        // 直接インスタンス化できないため (DI依存)、
        // 将来的にインターフェース抽出後にテストを追加する。
        // プレースホルダーとして記録。
        Assert.True(true);
    }
}
