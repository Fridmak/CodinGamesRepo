namespace CodinGamesRepo.State;

public class GameState
{
    public const int Width = 30;
    public const int Height = 20;

    public int MyId { get; set; }
    
    // В сетке храним ID игрока, чья там стена
    // -1 означает пустую клетку
    public int[,] Grid { get; }
    public Dictionary<int, PlayerInfo> Players { get; }

    public GameState()
    {
        Grid = new int[Width, Height];
        for (int x = 0; x < Width; x++)
        for (int y = 0; y < Height; y++)
            Grid[x, y] = -1;
                
        Players = new Dictionary<int, PlayerInfo>();
    }

    public GameState Clone()
    {
        var clone = new GameState { MyId = this.MyId };
        Array.Copy(this.Grid, clone.Grid, this.Grid.Length);
        foreach (var kvp in this.Players)
        {
            clone.Players[kvp.Key] = new PlayerInfo
            {
                Id = kvp.Value.Id,
                X = kvp.Value.X,
                Y = kvp.Value.Y,
                IsAlive = kvp.Value.IsAlive
            };
        }
        return clone;
    }

    public bool IsValid(int x, int y) =>
        x >= 0 && x < Width && y >= 0 && y < Height && Grid[x, y] == -1;
}