using CodinGamesRepo.State;

namespace CodinGamesRepo.Sim;

public static class GameSimulator
{
    public static void UpdateFromInput(GameState state)
    {
        string input = Console.ReadLine();
        if (string.IsNullOrEmpty(input)) return;

        var parts = input.Split(' ');
        int n = int.Parse(parts[0]);
        state.MyId = int.Parse(parts[1]);

        for (int i = 0; i < n; i++)
        {
            var posParts = Console.ReadLine().Split(' ');
            int x0 = int.Parse(posParts[0]), y0 = int.Parse(posParts[1]);
            int x1 = int.Parse(posParts[2]), y1 = int.Parse(posParts[3]);

            if (!state.Players.ContainsKey(i))
            {
                state.Players[i] = new PlayerInfo { Id = i, IsAlive = true };
            }

            var player = state.Players[i];

            if (x1 == -1)
            {
                if (player.IsAlive) KillPlayer(state, i);
                continue;
            }

            player.X = x1;
            player.Y = y1;
            player.IsAlive = true;

            // Обновление сетки
            state.Grid[x0, y0] = i;
            state.Grid[x1, y1] = i;
        }
    }

    // Удаление стен мертвых
    private static void KillPlayer(GameState state, int playerId)
    {
        state.Players[playerId].IsAlive = false;
        for (int x = 0; x < GameState.Width; x++)
        {
            for (int y = 0; y < GameState.Height; y++)
            {
                if (state.Grid[x, y] == playerId)
                {
                    state.Grid[x, y] = -1; // Очищаем хвост
                }
            }
        }
    }
}
