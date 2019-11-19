using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MONOPOLY
{
    #region Observer
    public interface IGameObserver
    {
        void Update(Game game);
    }
    public class PlayerAdditionObserver : IGameObserver
    {

        public void Update(Game game)
        {
            if (game.State == 1)
            {
                Console.WriteLine(game.Players.Last<Player>().Playername1+" has been added");
            }
        }

    }
    public class GameOverObserver : IGameObserver
    {

        public void Update(Game game)
        {
            if (game.State == 2)
            {
                Console.WriteLine("The game is over");
            }
        }

    }
    public interface IGame
    {
        // Attach an observer to the subject.
        void Attach(IGameObserver observer);

        // Detach an observer from the subject.
        void Detach(IGameObserver observer);

        // Notify all observers about an event.
        void Notify();
    }
    #endregion
    public class Game
    {
        List<Player> players;
        Player winner;
        public int State { get; set; } = -0;
        public List<Player> Players { get => players; set => players = value; }
        public Player Winner { get => winner; set => winner = value; }
        public int ending = 3;

        private List<IGameObserver> _observers = new List<IGameObserver>();
        public void Attach(IGameObserver observer)
        {
            _observers.Add(observer);
        }
        public void Detach(IGameObserver observer)
        {
            _observers.Remove(observer);

        }
        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.Update(this);
            }
        }
        public Game()
        {
            players = new List<Player>();
            winner = null;
        }
        public void AddPlayers(Player player)
        {
            var moveObserver = new MoveObserver();
            player.Attach(moveObserver);
            var diceObserver = new DiceObserver();
            player.Attach(diceObserver);
            var jailObserver = new JailObserver();
            player.Attach(jailObserver);
            Players.Add(player);
        }
        public void Jailed(Player player)
        {
            int[] throws = player.Throw_dice();
            if (throws[0] == throws[1])
            {
                player.Player_unjailed();
            }
            else
            {
                player.Turn_jailed += 1;
            }
        }
        public void Turn()
        {
            foreach(Player player in Players)
            {
                if(player.Fullturn ==ending)
                {
                    Winner = player;
                    State = 2;
                    Notify();
                }
                if(player.Turn_jailed == 2)
                {
                    player.Player_unjailed();
                }
                if(player.Jailed)
                {
                    Jailed(player);
                }
                else
                {

                    int[] throws = player.Throw_dice();
                    player.Move(throws[0] + throws[1]);
                    int doublecount = 0;
                    if(throws[0] == throws[1])
                    {
                        while (throws[0] == throws[1] )
                        {
                            doublecount += 1;
                            throws = player.Throw_dice();
                            player.Move(throws[0] + throws[1]);
                            if (doublecount == 3) break;
                            if (player.Position == 30) break;
                        }
                        if (doublecount == 3)
                        {
                            player.Player_jailed();
                        }
                    }
                    
                    if(player.Position == 30)
                    {
                        player.Player_jailed();
                    }
                }
                
            }
        }
    }
}
