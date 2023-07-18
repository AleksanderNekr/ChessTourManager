public class DrawResultTests
{
    [Fact]
    public void Success_CreatesSuccessDrawResult()
    {
        // Arrange
        ReadOnlySpan<char> message = "Success";

        // Act
        DrawResult drawResult = DrawResult.Success(message);

        // Assert
        Assert.Equal(DrawResult.ResultType.Success, drawResult.Result);
        Assert.Equal(message.ToString(),            drawResult.Message.ToString());
    }

    [Fact]
    public void NotEnoughPlayers_CreatesNotEnoughPlayersDrawResult()
    {
        // Arrange
        ReadOnlySpan<char> message = "Not enough players";

        // Act
        DrawResult drawResult = DrawResult.NotEnoughPlayers(message);

        // Assert
        Assert.Equal(DrawResult.ResultType.NotEnoughPlayers, drawResult.Result);
        Assert.Equal(message.ToString(),                     drawResult.Message.ToString());
    }

    [Fact]
    public void TournamentIsOver_CreatesTournamentIsOverDrawResult()
    {
        // Arrange
        ReadOnlySpan<char> message = "Tournament is over";

        // Act
        DrawResult drawResult = DrawResult.TournamentIsOver(message);

        // Assert
        Assert.Equal(DrawResult.ResultType.TournamentIsOver, drawResult.Result);
        Assert.Equal(message.ToString(),                     drawResult.Message.ToString());
    }

    [Fact]
    public void Fail_CreatesFailDrawResult()
    {
        // Arrange
        ReadOnlySpan<char> message = "Failed to draw";

        // Act
        DrawResult drawResult = DrawResult.Fail(message);

        // Assert
        Assert.Equal(DrawResult.ResultType.Fail, drawResult.Result);
        Assert.Equal(message.ToString(),         drawResult.Message.ToString());
    }
}
