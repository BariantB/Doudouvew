using System;
using System.Linq;
using System.Collections.Generic;

namespace Doudouvew
{
    public enum RewardType
    {
        ATTACK = 0,
        DEFENSE = 1,
        PV = 2,
    }
    abstract class Pokemon
    {
        protected int _life;
        protected string _name;
        protected string _attackName;

        public int _Life {
            get {
                return _life;
            }
            set {
                _life = value;
            }
        }
        public string _Name {
            get {
                return _name;
            }
            set {
                _name = value;
            }
        }
        public string _AttackName {
            get {
                return _attackName;
            }
            set {
                _attackName = value;
            }
        }

    }

    class Attacker : Pokemon
    {
        private int _attackPoints;

        public int _AttackPoints {
            get {
                return _attackPoints;
            }
            set {
                _attackPoints = value;
            }
        }

        public Attacker(int life, string name, string attackName, int attackPoints) {
            _life = life;
            _name = name;
            _attackName = attackName;
            _attackPoints = attackPoints;
        }

    }

    class Defender : Pokemon
    {
        private int _defensePoints;
     
        public int _DefensePoints {
            get {
                return _defensePoints;
            }
            set {
                _defensePoints = value;
            }
        }

        public Defender(int life, string name, string attackName, int defensePoints) {
            _life = life;
            _name = name;
            _attackName = attackName;
            _defensePoints = defensePoints;
        }   
    }

    abstract class Reward {
        protected string _description;

        public string _Description {
            get {
                return _description;
            }
            set {
                _description = value;
            }
        }

        public abstract void apply(Game game);
    }

    class Boost : Reward
    {
        private RewardType _type;
        private int _amount;

        public RewardType _Type {
            get {
                return _type;
            }
            set {
                _type = value;
            }
        }
        public int _Amount {
            get {
                return _amount;
            }
            set {
                _amount = value;
            }
        }

        public Boost(RewardType type, int amount, string description) {
            _type = type;
            _amount = amount;
            _description = description;
        }

        public override void apply(Game game) {
            return;
        }
    }

    class SecondPokemon : Reward
    {
        public SecondPokemon(string description) {
            _description = description;
        }

        public override void apply(Game game) {
            return;
        }
    }
    
    class GlobalBoost : Reward
    {
        public GlobalBoost(string description) {
            _description = description;
        }

        public override void apply(Game game) {
            return;
        }
    }

    class Player {
        private List<Pokemon> _pokemons;
        private List<Reward> _rewards;

        public List<Pokemon> _Pokemons {
            get {
                return _pokemons;
            }
            set {
                _pokemons = value;
            }
        }
        public List<Reward> _Rewards {
            get {
                return _rewards;
            }
            set {
                _rewards = value;
            }
        }

        public Player() {
            List<Pokemon> pokemons = new List<Pokemon>();
            Pokemon pokemon;
            pokemon = new Attacker(120, "VolcaNix", "attaque volcanique", 40);
            pokemons.Add(pokemon);
            pokemon = new Attacker(100, "TsunaMix", "attaque tsunami", 50);
            pokemons.Add(pokemon);
            pokemon = new Defender(80, "TeRex", "défense souterraine", 60);
            pokemons.Add(pokemon);
            pokemon = new Defender(90, "OceaNix", "défence aquatique", 50);
            pokemons.Add(pokemon);
            _pokemons = pokemons;
        }
    }

    class Game {
        private Player _playerOne;
        private Player _playerTwo;
        private List<Reward> _rewards;
        private int _field;

        public Player _PlayerOne {
            get {
                return _playerOne;
            }
            set {
                _playerOne = value;
            }
        }

        public Player _PlayerTwo {
            get {
                return _playerTwo;
            }
            set {
                _playerTwo = value;
            }
        }

        public List<Reward> _Rewards {
            get {
                return _rewards;
            }
            set {
                _rewards = value;
            }
        }

        public int _Field {
            get {
                return _field;
            }
            set {
                _field = value;
            }
        }

        public Game() {
            List<Reward> rewards = new List<Reward>();
            Boost reward;
            
            for (int i = 0; i != 2; i++) {
                reward = new Boost(RewardType.ATTACK, 10, "Ajouter 10% à la force d’attaque");
                rewards.Add(reward);
                reward = new Boost(RewardType.ATTACK, 15, "Ajouter 15% à la force d’attaque");
                rewards.Add(reward);
                reward = new Boost(RewardType.ATTACK, 20, "Ajouter 20% à la force d’attaque");
                rewards.Add(reward);
                reward = new Boost(RewardType.DEFENSE, 10, "Ajouter 10% à la force de défense");
                rewards.Add(reward);
                reward = new Boost(RewardType.DEFENSE, 15, "Ajouter 10% à la force de défense");
                rewards.Add(reward);
                reward = new Boost(RewardType.DEFENSE, 20, "Ajouter 10% à la force de défense");
                rewards.Add(reward);
                SecondPokemon spreward = new SecondPokemon("Ajouter un deuxième Pokémon à une bataille");
                rewards.Add(spreward);
            }

            for (int i = 0; i != 4; i++) {
                reward = new Boost(RewardType.PV, 10, "Ajouter 10 points de vie");
                rewards.Add(reward);
                reward = new Boost(RewardType.PV, 15, "Ajouter 15 points de vie");
                rewards.Add(reward);
                reward = new Boost(RewardType.PV, 20, "Ajouter 20 points de vie");
                rewards.Add(reward);
            }

            GlobalBoost gbreward = new GlobalBoost("Augmenter de 20 points tous les Pokémons d’un joueur");
            rewards.Add(gbreward);
        }
        
        public void gameLoop() {
            while (_playerOne._Pokemons.Count() > 0 && _playerTwo._Pokemons.Count() > 0) {
                printStatus();
                List<int> challengers = getChallengers();
                getField();
                getReward();
                playRewards();
                fight();
            }
            endGame();
            return;

        }
        public void printStatus() {
            for (int i = 0; i != _playerOne._Pokemons.Count(); ++i) {
                if (typeof(Attacker).IsInstanceOfType(_playerOne._Pokemons)) {
                    Attacker attacker = (Attacker)_playerOne._Pokemons[i];
                    Console.WriteLine("Pokemon n°" + (i + 1) + ": " + attacker._Name + ", a une attaque de " + attacker._AttackPoints + " et " + attacker._Life + " points de vie restants");
                }
                if (typeof(Defender).IsInstanceOfType(_playerOne._Pokemons)) {
                    Defender defender = (Defender)_playerOne._Pokemons[i];
                    Console.WriteLine("Pokemon n°" + (i + 1) + ": " + defender._Name + ", a une défense de " + defender._DefensePoints + " et " + defender._Life + " points de vie restants");
                }
            }

            Console.WriteLine("----------------------------------------------------------------");

            for (int i = 0; i != _playerTwo._Pokemons.Count(); ++i) {
                if (typeof(Attacker).IsInstanceOfType(_playerTwo._Pokemons)) {
                    Attacker attacker = (Attacker)_playerTwo._Pokemons[i];
                    Console.WriteLine("Pokemon n°" + (i + 1) + ": " + attacker._Name + ", a une attaque de " + attacker._AttackPoints + " et " + attacker._Life + " points de vie restants");
                }
                if (typeof(Defender).IsInstanceOfType(_playerTwo._Pokemons)) {
                    Defender defender = (Defender)_playerTwo._Pokemons[i];
                    Console.WriteLine("Pokemon n°" + (i + 1) + ": " + defender._Name + ", a une défense de " + defender._DefensePoints + " et " + defender._Life + " points de vie restants");
                }
            }

            Console.WriteLine("----------------------------------------------------------------");
            Console.Write("Appuyez sur une touche pour passer à la séléction des Pokémons :");
            Console.ReadKey();
            Console.Clear();
            return;
        }
        public List<int> getChallengers() {
            List<int> challengers = new List<int>();

            Console.Write("Joueur 1, à toi ! Appuie sur une touche pour séléctionner ton Pokémon :");
            Console.ReadKey();
            Console.Clear();

            bool ok = false;
            while (!ok) {
                for (int i = 0; i != _playerOne._Pokemons.Count(); ++i) {
                    if (typeof(Attacker).IsInstanceOfType(_playerOne._Pokemons)) {
                        Attacker attacker = (Attacker)_playerOne._Pokemons[i];
                        Console.WriteLine("Pokemon n°" + (i + 1) + ": " + attacker._Name + ", a une attaque de " + attacker._AttackPoints + " et " + attacker._Life + " points de vie restants");
                    }
                    if (typeof(Defender).IsInstanceOfType(_playerOne._Pokemons)) {
                        Defender defender = (Defender)_playerOne._Pokemons[i];
                        Console.WriteLine("Pokemon n°" + (i + 1) + ": " + defender._Name + ", a une défense de " + defender._DefensePoints + " et " + defender._Life + " points de vie restants");
                    }
                }

                Console.WriteLine("----------------------------------------------------------------");
                Console.Write("Choisis ton pokémon pour ce round en rentrant le numéro correspondant :");

                string tmp = Console.ReadLine();
                int challenger;
                if (!Int32.TryParse(tmp, out challenger)) {
                    Console.Write("Mauvais Input ! Appuie sur une touche pour réesayer !");
                    Console.ReadKey();
                    Console.Clear();
                } else {
                    challenger -= 1;
                    if (challenger < 0 || challenger > _playerOne._Pokemons.Count()) {
                        Console.Write("Mauvais Input ! Appuie sur une touche pour réesayer !");
                        Console.ReadKey();
                        Console.Clear();
                    } else {
                        challengers.Add(challenger);
                        ok = true;
                        Console.Write("Tu as choisi " + _playerOne._Pokemons[challenger]._Name + ". Appuie sur une touche pour passer la main au Joueur 2 !");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
            }

            Console.Write("Joueur 2, à toi ! Appuie sur une touche pour séléctionner ton Pokémon :");
            Console.ReadKey();
            Console.Clear();

            ok = false;
            while (!ok) {
                for (int i = 0; i != _playerOne._Pokemons.Count(); ++i) {
                    if (typeof(Attacker).IsInstanceOfType(_playerTwo._Pokemons)) {
                        Attacker attacker = (Attacker)_playerTwo._Pokemons[i];
                        Console.WriteLine("Pokemon n°" + (i + 1) + ": " + attacker._Name + ", a une attaque de " + attacker._AttackPoints + " et " + attacker._Life + " points de vie restants");
                    }
                    if (typeof(Defender).IsInstanceOfType(_playerTwo._Pokemons)) {
                        Defender defender = (Defender)_playerTwo._Pokemons[i];
                        Console.WriteLine("Pokemon n°" + (i + 1) + ": " + defender._Name + ", a une défense de " + defender._DefensePoints + " et " + defender._Life + " points de vie restants");
                    }
                }

                Console.WriteLine("----------------------------------------------------------------");
                Console.Write("Choisis ton pokémon pour ce round en rentrant le numéro correspondant :");

                string tmp = Console.ReadLine();
                int challenger;
                if (!Int32.TryParse(tmp, out challenger)) {
                    Console.Write("Mauvais Input ! Appuie sur une touche pour réesayer !");
                    Console.ReadKey();
                    Console.Clear();
                } else {
                    challenger -= 1;
                    if (challenger < 0 || challenger > _playerOne._Pokemons.Count()) {
                        Console.Write("Mauvais Input ! Appuie sur une touche pour réesayer !");
                        Console.ReadKey();
                        Console.Clear();
                    } else {
                        challengers.Add(challenger);
                        ok = true;
                        Console.Write("Tu as choisi " + _playerOne._Pokemons[challenger]._Name + ". Appuie sur une touche pour passer à la génération du terrain !");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
            }

            return challengers;
        }
        public void getField() {
            var rand = new Random();
            _field = rand.Next(0,2);

            if (_field == 0)
                Console.Write("La prochaine manche se déroulera sur Terre. Appuyez sur une touche pour passer à la pioche des récompenses.");
            else if (_field == 1)
                Console.Write("La prochaine manche se déroulera en Mer. Appuyez sur une touche pour passer à la pioche des récompenses.");
            Console.ReadKey();
            Console.Clear();
            return;
        }
        public void getReward() {
            var rand = new Random();

            Console.Write("Joueur 1, à toi ! Appuie sur une touche pour tirer une carte récompense :");
            Console.ReadKey();
            Console.Clear();

            int rew = rand.Next(0, _rewards.Count() + 1);
            Reward stock = _rewards[rew];
            _PlayerOne._Rewards.Add(stock);
            _rewards.RemoveAt(rew);

            Console.Write("Tu as pioché : " + stock._Description + ". Appuie sur une touche pour passer la main au Joueur 2 !");
            Console.ReadKey();
            Console.Clear();

            Console.Write("Joueur 2, à toi ! Appuie sur une touche pour tirer une carte récompense :");
            Console.ReadKey();
            Console.Clear();

            rew = rand.Next(0, _rewards.Count() + 1);
            stock = _rewards[rew];
            _PlayerTwo._Rewards.Add(stock);
            _rewards.RemoveAt(rew);

            Console.Write("Tu as pioché : " + stock._Description + ". Appuie sur une touche pour passer à l'utilisation des cartes récompences !");
            Console.ReadKey();
            Console.Clear();
        }
        public void playRewards() {
            return;
        }
        public void fight() {
            return;
        }
        public void endGame() {
            
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
