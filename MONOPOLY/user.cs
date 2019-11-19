using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MONOPOLY
{
    #region Controller
    public interface IPlayerObserver
    {
        void Update(Player player);
    }
    public class MoveObserver : IPlayerObserver
    {

        public void Update(Player player)
        {
            if(player.State == 1)
            {
                Console.WriteLine(player.Playername1 + " has moved to " + player.Position);
            }
        }
        
    }
    public class JailObserver : IPlayerObserver
    {

        public void Update(Player player)
        {
            
            if (player.State == 3)
            {
                Console.WriteLine(player.Playername1 + " has been put in jail");
            }
            if (player.State == 4)
            {
                Console.WriteLine(player.Playername1 + " has left jail");
            }
        }

    }
    public class DiceObserver : IPlayerObserver
    {

        public void Update(Player player)
        {

            if (player.State == 2)
            {
                Console.WriteLine(player.Playername1 + " got a "+player.T1+" and a "+player.T2);
            }
        }

    }
    #endregion
    public interface IPlayer
    {
        // Attach an observer to the subject.
        void Attach(IPlayerObserver observer);

        // Detach an observer from the subject.
        void Detach(IPlayerObserver observer);

        // Notify all observers about an event.
        void Notify();
    }
    public class Player:IPlayer
    {
        public int State { get; set; } = -0;
        Random seed;
        // List of subscribers. In real life, the list of subscribers can be
        // stored more comprehensively (categorized by event type, etc.).
        private List<IPlayerObserver> _observers = new List<IPlayerObserver>();
        public void Attach(IPlayerObserver observer)
        {
            _observers.Add(observer);
        }
        public void Detach(IPlayerObserver observer)
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
        string Playername;        
        bool jailed;
        int turn_jailed;
        int position;
        int fullturn;
        int t1, t2;

        public bool Jailed { get => jailed; set => jailed = value; }
        public int Turn_jailed { get => turn_jailed; set => turn_jailed = value; }
        public string Playername1 { get => Playername; set => Playername = value; }
        public int Position { get => position; set => position = value; }
        public int T1 { get => t1; set => t1 = value; }
        public int T2 { get => t2; set => t2 = value; }
        public int Fullturn { get => fullturn; set => fullturn = value; }

        public Player(string Playername)
        {
            this.Playername1 = Playername;
            Jailed = false;
            Turn_jailed = -1;
            Position = 0;
            Fullturn = 0;
            seed = new Random(Playername.GetHashCode());
        }
        public int[] Throw_dice()
        {
            T1 = seed.Next(1, 7);
            T2 = seed.Next(1, 7);
            State = 2;
            Notify();
            return new int[2]{ T1, T2};
        }
        public void Player_jailed()
        {
            Jailed = true;
            Turn_jailed = 0;
            Position = 10;
            State = 3;
            Notify();
        }
        public void Player_unjailed()
        {
            Jailed = false;
            Turn_jailed = -1;
            State = 4;
            Notify();
        }
        public int Move(int nb)
        {
            if(Position+nb > 39)
            {
                Position = (Position + nb) % 39;
                Fullturn += 1;
            }
            else
            {
                Position += nb;
            }
            State = 1;
            Notify();
            return Position;
        }
    }
}
