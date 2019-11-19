using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MONOPOLY
{
    class Viewer
    {
        Game game;
        public void Start()
        {
            game = new Game();
            var PlayerAdditionObserver = new PlayerAdditionObserver();
            var gameoverobserver = new GameOverObserver();
            Player p1 = new Player("victor");
            Player p2 = new Player("Lucas");
            game.AddPlayers(p1);
            game.AddPlayers(p2);
            Player winner = null;
            bool end = false;
            while(end == false)
            {
                game.Turn();
                Console.WriteLine("Press enter to continue...");
                Console.ReadKey();
                if(game.Winner != null)
                {
                    end = true;
                    winner = game.Winner;
                }
            }
            Console.WriteLine(winner.Playername1 + " has won");
            Console.ReadKey();
        }
    }
}
