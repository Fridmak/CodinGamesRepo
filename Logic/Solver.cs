using CodinGamesRepo.State;

namespace CodinGamesRepo.Logic;

public static class Solver
{
    private static readonly (int dx, int dy, string dir)[] Directions = 
    {
        (0, -1, "UP"), (0, 1, "DOWN"), (-1, 0, "LEFT"), (1, 0, "RIGHT")
    };

    public static string FindBestMove(GameState state)
    {
        var myPlayer = state.Players[state.MyId];
        var candidates = new List<(string dir, int score)>();

        foreach (var (dx, dy, dir) in Directions)
        {
            int nx = myPlayer.X + dx;
            int ny = myPlayer.Y + dy;

            if (!state.IsValid(nx, ny)) continue;

            // Делаем копию состояния, чтобы симулировать наш потенциальный ход
            var simulatedState = state.Clone();
            simulatedState.Grid[nx, ny] = state.MyId;
            simulatedState.Players[state.MyId].X = nx;
            simulatedState.Players[state.MyId].Y = ny;
            
            // Оценка позиции с помощью диаграммы вороного
            int score = CalculateVoronoiTerritory(simulatedState, state.MyId);
            candidates.Add((dir, score));
        }
        
        if (candidates.Count == 0) return "UP"; // Смерть

        return candidates.OrderByDescending(c => c.score).First().dir;
    }

    // Мульти-стартовый BFS для расчета контролируемой территории
    private static int CalculateVoronoiTerritory(GameState state, int myId)
    {
        var queue = new Queue<(int x, int y, int owner)>();
        var visited = new int[GameState.Width, GameState.Height];
        
        for (int x = 0; x < GameState.Width; x++)
            for (int y = 0; y < GameState.Height; y++)
                visited[x, y] = -1;

        foreach (var player in state.Players.Values.Where(p => p.IsAlive))
        {
            queue.Enqueue((player.X, player.Y, player.Id));
            visited[player.X, player.Y] = player.Id;
        }

        int myTerritorySize = 0;

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();

            if (current.owner == myId)
            {
                myTerritorySize++;
            }

            foreach (var (dx, dy, _) in Directions)
            {
                int nx = current.x + dx;
                int ny = current.y + dy;

                if (state.IsValid(nx, ny) && visited[nx, ny] == -1)
                {
                    visited[nx, ny] = current.owner;
                    queue.Enqueue((nx, ny, current.owner));
                }
            }
        }

        return myTerritorySize;
    }
}