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
        protected int _fieldBoost;
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

        public abstract void boostPokemon(RewardType rewardType, int amount);
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

        public Attacker(int life, int fieldBoost, string name, string attackName, int attackPoints) {
            _life = life;
            _fieldBoost = fieldBoost;
            _name = name;
            _attackName = attackName;
            _attackPoints = attackPoints;
        }
        public override void boostPokemon(RewardType rewardType, int amount){
            if (rewardType == RewardType.ATTACK)
                _attackPoints += _attackPoints * amount / 100;
            else
                _life += amount;
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

        public Defender(int life, int fieldBoost, string name, string attackName, int defensePoints) {
            _life = life;
            _fieldBoost = fieldBoost;
            _name = name;
            _attackName = attackName;
            _defensePoints = defensePoints;
        }
        public override void boostPokemon(RewardType rewardType, int amount){
            if (rewardType == RewardType.DEFENSE)
                _defensePoints += _defensePoints * amount / 100;
            else
                _life += amount;
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

        public abstract void apply(Player player);
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

        public override void apply(Player player) {
            Console.Write("Appuie pour afficher tes pokémons disponibles :");
            Console.ReadKey();
            Console.Clear();
            bool ok = false;
            while(!ok) {
                for (int i = 0; i != player._Pokemons.Count(); ++i) {
                    if (_type != RewardType.DEFENSE) {
                        if (typeof(Attacker).IsInstanceOfType(player._Pokemons[i].Item2)) {
                            Attacker attacker = (Attacker)player._Pokemons[i].Item2;
                            Console.WriteLine("Pokemon n°" + (i + 1) + ": " + attacker._Name + ", a une attaque de " + attacker._AttackPoints + " et " + attacker._Life + " points de vie restants");
                        }
                    }
                    if (_type != RewardType.ATTACK) {
                        if (typeof(Defender).IsInstanceOfType(player._Pokemons[i].Item2)) {
                            Defender defender = (Defender)player._Pokemons[i].Item2;
                            Console.WriteLine("Pokemon n°" + (i + 1) + ": " + defender._Name + ", a une défense de " + defender._DefensePoints + " et " + defender._Life + " points de vie restants");
                        }
                    }
                }
                Console.WriteLine("----------------------------------------------------------------");
                Console.Write("Choisis le pokémon à booster :");

                string tmp = Console.ReadLine();
                int challenger;
                if (!Int32.TryParse(tmp, out challenger)) {
                    Console.Write("Mauvais Input ! Appuie sur une touche pour réessayer !");
                    Console.ReadKey();
                    Console.Clear();
                } else {
                    challenger -= 1;
                    if (challenger < 0 || challenger > player._Pokemons.Count()) {
                        Console.Write("Mauvais Input ! Appuie sur une touche pour réessayer !");
                        Console.ReadKey();
                        Console.Clear();
                    } else {
                        player._Pokemons[challenger].Item2.boostPokemon(_type, _amount);
                        ok = true;
                        Console.Write("Tu as choisi de booster " + player._Pokemons[challenger].Item2._Name + ". Appuie pour revenir à l'écran des récompenses !");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
            }
            return;
        }
    }

    class SecondPokemon : Reward
    {
        public SecondPokemon(string description) {
            _description = description;
        }

        public override void apply(Player player) {
            if (player._Pokemons.Count() == 1) {
                Console.WriteLine("Un seul pokémon restant. Carte récompense invalide !");
            } else {
                Console.Write("Appuie pour afficher tes pokémons disponibles :");
                Console.ReadKey();
                Console.Clear();
                bool ok = false;
                while(!ok) {
                    for (int i = 0; i != player._Pokemons.Count(); ++i) {
                        if (player._Pokemons[i].Item1 == 0) {
                            if (typeof(Attacker).IsInstanceOfType(player._Pokemons[i].Item2)) {
                                Attacker attacker = (Attacker)player._Pokemons[i].Item2;
                                Console.WriteLine("Pokemon n°" + (i + 1) + ": " + attacker._Name + ", a une attaque de " + attacker._AttackPoints + " et " + attacker._Life + " points de vie restants");
                            }
                            if (typeof(Defender).IsInstanceOfType(player._Pokemons[i].Item2)) {
                                Defender defender = (Defender)player._Pokemons[i].Item2;
                                Console.WriteLine("Pokemon n°" + (i + 1) + ": " + defender._Name + ", a une défense de " + defender._DefensePoints + " et " + defender._Life + " points de vie restants");
                            }
                        }
                    }
                    Console.WriteLine("----------------------------------------------------------------");
                    Console.Write("Choisis ton pokémon pour ce round en rentrant le numéro correspondant :");

                    string tmp = Console.ReadLine();
                    int challenger;
                    if (!Int32.TryParse(tmp, out challenger)) {
                        Console.Write("Mauvais Input ! Appuie sur une touche pour réessayer !");
                        Console.ReadKey();
                        Console.Clear();
                    } else {
                        challenger -= 1;
                        if (challenger < 0 || challenger > player._Pokemons.Count()) {
                            Console.Write("Mauvais Input ! Appuie sur une touche pour réessayer !");
                            Console.ReadKey();
                            Console.Clear();
                        } else {
                            player._Pokemons[challenger] = new Tuple<int, Pokemon>(1, player._Pokemons[challenger].Item2);
                            ok = true;
                            Console.Write("Tu as choisi " + player._Pokemons[challenger].Item2._Name + ". Appuie pour revenir à l'écran des récompenses !");
                            Console.ReadKey();
                            Console.Clear();
                        }
                    }
                }
            }
            return;
        }
    }
    
    class GlobalBoost : Reward
    {
        public GlobalBoost(string description) {
            _description = description;
        }

        public override void apply(Player player) {
            for (int i = 0; i != player._Pokemons.Count(); ++i)
                player._Pokemons[i].Item2.boostPokemon(RewardType.PV, 20);
            Console.Write("20 points de vie ajoutés à toute ton équipe ! Appuie sur une touche pour revenir à l'écran des récompenses !");
            Console.ReadKey();
            Console.Clear();
            return;
        }
    }

    class Player {
        private List<Tuple<int, Pokemon>> _pokemons;
        private List<Reward> _rewards;

        public List<Tuple<int, Pokemon>> _Pokemons {
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
            List<Tuple<int, Pokemon>> pokemons = new List<Tuple<int, Pokemon>>();
            _rewards = new List<Reward>();
            Pokemon pokemon;
            pokemon = new Attacker(120, 0, "VolcaNix", "attaque volcanique", 40);
            Tuple<int, Pokemon> add = new Tuple<int, Pokemon>(0, pokemon);
            pokemons.Add(add);
            pokemon = new Attacker(100, 1, "TsunaMix", "attaque tsunami", 50);
            add = new Tuple<int, Pokemon>(0, pokemon);
            pokemons.Add(add);
            pokemon = new Defender(80, 0, "TeRex", "défense souterraine", 60);
            add = new Tuple<int, Pokemon>(0, pokemon);
            pokemons.Add(add);
            pokemon = new Defender(90, 1, "OceaNix", "défence aquatique", 50);
            add = new Tuple<int, Pokemon>(0, pokemon);
            pokemons.Add(add);
            _pokemons = pokemons;
        }
        public void addToList(Reward reward) {
            _rewards.Add(reward);
            return;
        }
        public void removeFromList (int pos) {
            _rewards.RemoveAt(pos);
            return;
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
            Boost reward;
            
            for (int i = 0; i != 2; i++) {
                _rewards = new List<Reward>();
                reward = new Boost(RewardType.ATTACK, 10, "Ajouter 10% à la force d’attaque");
                _rewards.Add(reward);
                reward = new Boost(RewardType.ATTACK, 15, "Ajouter 15% à la force d’attaque");
                _rewards.Add(reward);
                reward = new Boost(RewardType.ATTACK, 20, "Ajouter 20% à la force d’attaque");
                _rewards.Add(reward);
                reward = new Boost(RewardType.DEFENSE, 10, "Ajouter 10% à la force de défense");
                _rewards.Add(reward);
                reward = new Boost(RewardType.DEFENSE, 15, "Ajouter 10% à la force de défense");
                _rewards.Add(reward);
                reward = new Boost(RewardType.DEFENSE, 20, "Ajouter 10% à la force de défense");
                _rewards.Add(reward);
                SecondPokemon spreward = new SecondPokemon("Ajouter un deuxième Pokémon à une bataille");
                _rewards.Add(spreward);
            }

            for (int i = 0; i != 4; i++) {
                reward = new Boost(RewardType.PV, 10, "Ajouter 10 points de vie");
                _rewards.Add(reward);
                reward = new Boost(RewardType.PV, 15, "Ajouter 15 points de vie");
                _rewards.Add(reward);
                reward = new Boost(RewardType.PV, 20, "Ajouter 20 points de vie");
                _rewards.Add(reward);
            }

            GlobalBoost gbreward = new GlobalBoost("Augmenter de 20 points tous les Pokémons d’un joueur");
            _rewards.Add(gbreward);

            _playerOne = new Player();
            _playerTwo = new Player();
        }
        
        public void gameLoop() {
            while (_playerOne._Pokemons.Count() > 0 && _playerTwo._Pokemons.Count() > 0) {
                printStatus();
                getChallengers();
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
                if (typeof(Attacker).IsInstanceOfType(_playerOne._Pokemons[i].Item2)) {
                    Attacker attacker = (Attacker)_playerOne._Pokemons[i].Item2;
                    Console.WriteLine("Pokemon n°" + (i + 1) + ": " + attacker._Name + ", a une attaque de " + attacker._AttackPoints + " et " + attacker._Life + " points de vie restants");
                }
                if (typeof(Defender).IsInstanceOfType(_playerOne._Pokemons[i].Item2)) {
                    Defender defender = (Defender)_playerOne._Pokemons[i].Item2;
                    Console.WriteLine("Pokemon n°" + (i + 1) + ": " + defender._Name + ", a une défense de " + defender._DefensePoints + " et " + defender._Life + " points de vie restants");
                }
            }

            Console.WriteLine("----------------------------------------------------------------");

            for (int i = 0; i != _playerTwo._Pokemons.Count(); ++i) {
                if (typeof(Attacker).IsInstanceOfType(_playerTwo._Pokemons[i].Item2)) {
                    Attacker attacker = (Attacker)_playerTwo._Pokemons[i].Item2;
                    Console.WriteLine("Pokemon n°" + (i + 1) + ": " + attacker._Name + ", a une attaque de " + attacker._AttackPoints + " et " + attacker._Life + " points de vie restants");
                }
                if (typeof(Defender).IsInstanceOfType(_playerTwo._Pokemons[i].Item2)) {
                    Defender defender = (Defender)_playerTwo._Pokemons[i].Item2;
                    Console.WriteLine("Pokemon n°" + (i + 1) + ": " + defender._Name + ", a une défense de " + defender._DefensePoints + " et " + defender._Life + " points de vie restants");
                }
            }

            Console.WriteLine("----------------------------------------------------------------");
            Console.Write("Appuyez sur une touche pour passer à la séléction des Pokémons :");
            Console.ReadKey();
            Console.Clear();
            return;
        }
        public void getChallengers() {

            Console.Write("Joueur 1, à toi ! Appuie sur une touche pour séléctionner ton Pokémon :");
            Console.ReadKey();
            Console.Clear();

            bool ok = false;
            while (!ok) {
                for (int i = 0; i != _playerOne._Pokemons.Count(); ++i) {
                    if (typeof(Attacker).IsInstanceOfType(_playerOne._Pokemons[i].Item2)) {
                        Attacker attacker = (Attacker)_playerOne._Pokemons[i].Item2;
                        Console.WriteLine("Pokemon n°" + (i + 1) + ": " + attacker._Name + ", a une attaque de " + attacker._AttackPoints + " et " + attacker._Life + " points de vie restants");
                    }
                    if (typeof(Defender).IsInstanceOfType(_playerOne._Pokemons[i].Item2)) {
                        Defender defender = (Defender)_playerOne._Pokemons[i].Item2;
                        Console.WriteLine("Pokemon n°" + (i + 1) + ": " + defender._Name + ", a une défense de " + defender._DefensePoints + " et " + defender._Life + " points de vie restants");
                    }
                }

                Console.WriteLine("----------------------------------------------------------------");
                Console.Write("Choisis ton pokémon pour ce round en rentrant le numéro correspondant : ");

                string tmp = Console.ReadLine();
                int challenger;
                if (!Int32.TryParse(tmp, out challenger)) {
                    Console.Write("Mauvais Input ! Appuie sur une touche pour réessayer !");
                    Console.ReadKey();
                    Console.Clear();
                } else {
                    challenger -= 1;
                    if (challenger < 0 || challenger > _playerOne._Pokemons.Count()) {
                        Console.Write("Mauvais Input ! Appuie sur une touche pour réessayer !");
                        Console.ReadKey();
                        Console.Clear();
                    } else {
                        _playerOne._Pokemons[challenger] = new Tuple<int, Pokemon>(1, _playerOne._Pokemons[challenger].Item2);
                        ok = true;
                        Console.Write("Tu as choisi " + _playerOne._Pokemons[challenger].Item2._Name + ". Appuie sur une touche pour passer la main au Joueur 2 !");
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
                for (int i = 0; i != _playerTwo._Pokemons.Count(); ++i) {
                    if (typeof(Attacker).IsInstanceOfType(_playerTwo._Pokemons[i].Item2)) {
                        Attacker attacker = (Attacker)_playerTwo._Pokemons[i].Item2;
                        Console.WriteLine("Pokemon n°" + (i + 1) + ": " + attacker._Name + ", a une attaque de " + attacker._AttackPoints + " et " + attacker._Life + " points de vie restants");
                    }
                    if (typeof(Defender).IsInstanceOfType(_playerTwo._Pokemons[i].Item2)) {
                        Defender defender = (Defender)_playerTwo._Pokemons[i].Item2;
                        Console.WriteLine("Pokemon n°" + (i + 1) + ": " + defender._Name + ", a une défense de " + defender._DefensePoints + " et " + defender._Life + " points de vie restants");
                    }
                }

                Console.WriteLine("----------------------------------------------------------------");
                Console.Write("Choisis ton pokémon pour ce round en rentrant le numéro correspondant : ");

                string tmp = Console.ReadLine();
                int challenger;
                if (!Int32.TryParse(tmp, out challenger)) {
                    Console.Write("Mauvais Input ! Appuie sur une touche pour réessayer !");
                    Console.ReadKey();
                    Console.Clear();
                } else {
                    challenger -= 1;
                    if (challenger < 0 || challenger > _playerTwo._Pokemons.Count()) {
                        Console.Write("Mauvais Input ! Appuie sur une touche pour réessayer !");
                        Console.ReadKey();
                        Console.Clear();
                    } else {
                        _playerTwo._Pokemons[challenger] = new Tuple<int, Pokemon>(1, _playerTwo._Pokemons[challenger].Item2);
                        ok = true;
                        Console.Write("Tu as choisi " + _playerTwo._Pokemons[challenger].Item2._Name + ". Appuie sur une touche pour passer à la génération du terrain !");
                        Console.ReadKey();
                        Console.Clear();
                    }
                }
            }

            return;
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

            int rew = rand.Next(0, _rewards.Count());
            Reward stock = _rewards[rew];
            _playerOne.addToList(stock);
            _rewards.RemoveAt(rew);

            Console.Write("Tu as pioché : " + stock._Description + ". Appuie sur une touche pour passer la main au Joueur 2 !");
            Console.ReadKey();
            Console.Clear();

            Console.Write("Joueur 2, à toi ! Appuie sur une touche pour tirer une carte récompense :");
            Console.ReadKey();
            Console.Clear();

            rand = new Random();
            rew = rand.Next(0, _rewards.Count());
            stock = _rewards[rew];
            _playerTwo.addToList(stock);
            _rewards.RemoveAt(rew);

            Console.Write("Tu as pioché : " + stock._Description + ". Appuie sur une touche pour passer à l'utilisation des cartes récompenses !");
            Console.ReadKey();
            Console.Clear();
        }
        public void playRewards() {
            Console.Write("Joueur 1, à toi ! Appuie sur une touche pour afficher tes cartes récompenses :");
            Console.ReadKey();
            Console.Clear();

            int reward;
            string tmp;
            bool ok = false;
            while (!ok) {
                for (int i = 0; i != _playerOne._Rewards.Count(); ++i)
                    Console.WriteLine((i + 1) + ": " + _playerOne._Rewards[i]._Description);
                Console.WriteLine("----------------------------------------------------------------");
                Console.Write("Choisis une carte récompense à jouer en rentrant le numéro correspondant, ou rentre 0 pour n'en jouer aucune :");
                tmp = Console.ReadLine();
                if (!Int32.TryParse(tmp, out reward)) {
                    Console.Write("Mauvais Input ! Appuie sur une touche pour réessayer !");
                    Console.ReadKey();
                    Console.Clear();
                } else {
                    reward -= 1;
                    if (reward < -1 || reward > _playerOne._Rewards.Count()) {
                        Console.Write("Mauvais Input ! Appuie sur une touche pour réessayer !");
                        Console.ReadKey();
                        Console.Clear();
                    } else if (reward == -1) {
                        ok = true;
                        Console.Write("Vous avez fini avec vos récompenses. Appuie sur une touche pour passer au joueur 2 !");
                        Console.ReadKey();
                        Console.Clear();
                    } else {
                        _playerOne._Rewards[reward].apply(_playerOne);
                        _rewards.Add(_playerOne._Rewards[reward]);
                        _playerOne.removeFromList(reward);
                    }
                }
                if (_playerOne._Rewards.Count() == 0) {
                    ok = true;
                    Console.Write("Vous avez fini avec vos récompenses. Appuie sur une touche pour passer au joueur 2 !");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            Console.Write("Joueur 2, à toi ! Appuie sur une touche pour afficher tes cartes récompenses :");
            Console.ReadKey();
            Console.Clear();
            ok = false;
            while (!ok) {
                for (int i = 0; i != _playerTwo._Rewards.Count(); ++i)
                    Console.WriteLine((i + 1) + ": " + _playerTwo._Rewards[i]._Description);
                Console.WriteLine("----------------------------------------------------------------");
                Console.Write("Choisis une carte récompense à jouer en rentrant le numéro correspondant, ou rentre 0 pour n'en jouer aucune :");
                tmp = Console.ReadLine();
                if (!Int32.TryParse(tmp, out reward)) {
                    Console.Write("Mauvais Input ! Appuie sur une touche pour réessayer !");
                    Console.ReadKey();
                    Console.Clear();
                } else {
                    reward -= 1;
                    if (reward < -1 || reward > _playerTwo._Rewards.Count()) {
                        Console.Write("Mauvais Input ! Appuie sur une touche pour réessayer !");
                        Console.ReadKey();
                        Console.Clear();
                    } else if (reward == -1) {
                        ok = true;
                        Console.Write("Vous avez fini avec vos récompenses. Appuie pour passer au combat !");
                        Console.ReadKey();
                        Console.Clear();
                    } else {
                        _playerTwo._Rewards[reward].apply(_playerTwo);
                        _rewards.Add(_playerTwo._Rewards[reward]);
                        _playerTwo.removeFromList(reward);
                    }
                }
                if (_playerTwo._Rewards.Count() == 0) {
                    ok = true;
                    Console.Write("Vous avez fini avec vos récompenses. Appuie pour passer au combat !");
                    Console.ReadKey();
                    Console.Clear();
                }
            }
            return;
        }
        public List<int> chooseTarget(List<int> currentPlayer, List<int> opponent, bool playerOne) {
            List<int> damagedChallengers = new List<int>();
            bool ok = false;
            if (playerOne)
                Console.WriteLine("Joueur 1, appuie sur une touche pour passer à l'attaque !");
            else
                Console.WriteLine("Joueur 2, appuie sur une touche pour passer à l'attaque !");
            Console.ReadKey();
            Console.Clear();
            for (int i = 0; i != currentPlayer.Count(); ++i) {
                while (!ok) {
                    if (opponent.Count() == 1) {
                        damagedChallengers.Add(opponent[0]);
                        ok = true;
                    } else {
                        Console.WriteLine("Quel Pokémon souhaites-tu attaquer ?");
                        for (int j = 0; j != opponent.Count(); ++j) {
                            if (playerOne)
                                Console.Write((j + 1) + ": " + _playerTwo._Pokemons[j].Item2._Name);
                            else
                                Console.Write((j + 1) + ": " + _playerOne._Pokemons[j].Item2._Name);
                            Console.Write(" ");
                        }
                        string tmp = Console.ReadLine();
                        int number;
                        if (!Int32.TryParse(tmp, out number)) {
                            Console.Write("Mauvais Input ! Appuie sur une touche pour réessayer !");
                            Console.ReadKey();
                            Console.Clear();
                        } else {
                            if (number < 1 || number > (opponent.Count() + 1)) {
                                Console.Write("Mauvais Input ! Appuie sur une touche pour réessayer !");
                                Console.ReadKey();
                                Console.Clear();
                            } else {
                                damagedChallengers.Add(opponent[number - 1]);
                                ok = true;
                            }
                        }
                    }
                }
                ok = false;
            }
            return damagedChallengers;
        }
        public void damageStep(List<int> firstAttack, List<int> secondAttack, List<int> playerOneChallengers, List<int> playerTwoChallengers) {
            for (int i = 0; i != playerOneChallengers.Count(); ++i) {
                if (typeof(Attacker).IsInstanceOfType(_playerOne._Pokemons[playerOneChallengers[i]].Item2)) {
                    Attacker firstAttacker = (Attacker)_playerOne._Pokemons[playerOneChallengers[i]].Item2;
                    if (typeof(Attacker).IsInstanceOfType(_playerTwo._Pokemons[firstAttack[i]].Item2)) {
                        Attacker secondAttacker = (Attacker)_playerTwo._Pokemons[firstAttack[i]].Item2;
                        int diff = firstAttacker._AttackPoints - secondAttacker._AttackPoints;
                        if (diff > 0) {
                            _playerTwo._Pokemons[firstAttack[i]].Item2._Life -= 10;
                            Console.WriteLine(_playerOne._Pokemons[playerOneChallengers[i]].Item2._Name + " a infligé 10 dégâts à " + _playerTwo._Pokemons[firstAttack[i]].Item2._Name + " grâce à " + _playerOne._Pokemons[playerOneChallengers[i]].Item2._AttackName);
                        } else if (diff < 0) {
                            _playerOne._Pokemons[playerOneChallengers[i]].Item2._Life -= 10;
                            Console.WriteLine(_playerTwo._Pokemons[firstAttack[i]].Item2._Name + " a infligé 10 dégâts à " + _playerOne._Pokemons[playerOneChallengers[i]].Item2._Name + " grâce à " + _playerTwo._Pokemons[firstAttack[i]].Item2._AttackName);
                        } else {
                            Console.WriteLine("L'attaque des deux combattants étant égales, personne n'a perdu de point de vie.");
                        }
                    }
                    if (typeof(Defender).IsInstanceOfType(_playerTwo._Pokemons[firstAttack[i]].Item2)) {
                        Defender secondDefender = (Defender)_playerTwo._Pokemons[firstAttack[i]].Item2;
                        int diff = firstAttacker._AttackPoints - secondDefender._DefensePoints;
                        if (diff > 0) {
                            _playerTwo._Pokemons[firstAttack[i]].Item2._Life -= diff;
                            Console.WriteLine(_playerOne._Pokemons[playerOneChallengers[i]].Item2._Name + " a infligé " + Math.Abs(diff) + " dégâts à " + _playerTwo._Pokemons[firstAttack[i]].Item2._Name + " grâce à " + _playerOne._Pokemons[playerOneChallengers[i]].Item2._AttackName);
                        } else if (diff < 0) {
                            _playerOne._Pokemons[playerOneChallengers[i]].Item2._Life -= Math.Abs(diff);
                            Console.WriteLine(_playerTwo._Pokemons[firstAttack[i]].Item2._Name + " a infligé " + Math.Abs(diff) + " dégâts à " + _playerOne._Pokemons[playerOneChallengers[i]].Item2._Name + " grâce à " + _playerTwo._Pokemons[firstAttack[i]].Item2._AttackName);
                        } else {
                            Console.WriteLine("L'attaque et la défense des deux combattants étant égales, personne n'a perdu de point de vie.");
                        }
                    }
                }
                if (typeof(Defender).IsInstanceOfType(_playerOne._Pokemons[playerOneChallengers[i]].Item2)) {
                    Defender firstDefender = (Defender)_playerOne._Pokemons[playerOneChallengers[i]].Item2;
                    if (typeof(Attacker).IsInstanceOfType(_playerTwo._Pokemons[firstAttack[i]].Item2)) {
                        Attacker secondAttacker = (Attacker)_playerTwo._Pokemons[firstAttack[i]].Item2;
                        int diff = firstDefender._DefensePoints - secondAttacker._AttackPoints;
                        if (diff > 0) {
                            _playerTwo._Pokemons[firstAttack[i]].Item2._Life -= diff;
                            Console.WriteLine(_playerOne._Pokemons[playerOneChallengers[i]].Item2._Name + " a infligé " + Math.Abs(diff) + " dégâts à " + _playerTwo._Pokemons[firstAttack[i]].Item2._Name + " grâce à " + _playerOne._Pokemons[playerOneChallengers[i]].Item2._AttackName);
                        } else if (diff < 0) {
                            _playerOne._Pokemons[playerOneChallengers[i]].Item2._Life -= Math.Abs(diff);
                            Console.WriteLine(_playerTwo._Pokemons[firstAttack[i]].Item2._Name + " a infligé " + Math.Abs(diff) + " dégâts à " + _playerOne._Pokemons[playerOneChallengers[i]].Item2._Name + " grâce à " + _playerTwo._Pokemons[firstAttack[i]].Item2._AttackName);
                        } else {
                            Console.WriteLine("L'attaque et la défense des deux combattants étant égales, personne n'a perdu de point de vie.");
                        }
                    }
                    if (typeof(Defender).IsInstanceOfType(_playerTwo._Pokemons[firstAttack[i]].Item2)) {
                        Defender secondDefender = (Defender)_playerTwo._Pokemons[firstAttack[i]].Item2;
                        int diff = firstDefender._DefensePoints - secondDefender._DefensePoints;
                        if (diff > 0) {
                            _playerTwo._Pokemons[firstAttack[i]].Item2._Life -= 10;
                            Console.WriteLine(_playerOne._Pokemons[playerOneChallengers[i]].Item2._Name + " a infligé 10 dégâts à " + _playerTwo._Pokemons[firstAttack[i]].Item2._Name + " grâce à " + _playerOne._Pokemons[playerOneChallengers[i]].Item2._AttackName);
                        } else if (diff < 0) {
                            _playerOne._Pokemons[playerOneChallengers[i]].Item2._Life -= 10;
                            Console.WriteLine(_playerTwo._Pokemons[firstAttack[i]].Item2._Name + " a infligé 10 dégâts à " + _playerOne._Pokemons[playerOneChallengers[i]].Item2._Name + " grâce à " + _playerTwo._Pokemons[firstAttack[i]].Item2._AttackName);
                        } else {
                            Console.WriteLine("La défense des deux combattants étant égales, personne n'a perdu de point de vie.");
                        }
                    }
                }
                Console.Write("Appuyez sur une touche pour découvrir le combat suivant");
                Console.ReadKey();
                Console.Clear();
            }
            for (int i = 0; i != playerTwoChallengers.Count(); ++i) {
                if (typeof(Attacker).IsInstanceOfType(_playerTwo._Pokemons[playerTwoChallengers[i]].Item2)) {
                    Attacker firstAttacker = (Attacker)_playerTwo._Pokemons[playerTwoChallengers[i]].Item2;
                    if (typeof(Attacker).IsInstanceOfType(_playerOne._Pokemons[secondAttack[i]].Item2)) {
                        Attacker secondAttacker = (Attacker)_playerOne._Pokemons[secondAttack[i]].Item2;
                        int diff = firstAttacker._AttackPoints - secondAttacker._AttackPoints;
                        if (diff > 0) {
                            _playerOne._Pokemons[secondAttack[i]].Item2._Life -= 10;
                            Console.WriteLine(_playerTwo._Pokemons[playerTwoChallengers[i]].Item2._Name + " a infligé 10 dégâts à " + _playerOne._Pokemons[secondAttack[i]].Item2._Name + " grâce à " + _playerTwo._Pokemons[playerTwoChallengers[i]].Item2._AttackName);
                        } else if (diff < 0) {
                            _playerTwo._Pokemons[playerTwoChallengers[i]].Item2._Life -= 10;
                            Console.WriteLine(_playerOne._Pokemons[secondAttack[i]].Item2._Name + " a infligé 10 dégâts à " + _playerTwo._Pokemons[playerTwoChallengers[i]].Item2._Name + " grâce à " + _playerOne._Pokemons[secondAttack[i]].Item2._AttackName);
                        } else {
                            Console.WriteLine("L'attaque des deux combattants étant égales, personne n'a perdu de point de vie.");
                        }
                    }
                    if (typeof(Defender).IsInstanceOfType(_playerOne._Pokemons[secondAttack[i]].Item2)) {
                        Defender secondDefender = (Defender)_playerOne._Pokemons[secondAttack[i]].Item2;
                        int diff = firstAttacker._AttackPoints - secondDefender._DefensePoints;
                        if (diff > 0) {
                            _playerOne._Pokemons[secondAttack[i]].Item2._Life -= diff;
                            Console.WriteLine(_playerTwo._Pokemons[playerTwoChallengers[i]].Item2._Name + " a infligé " + Math.Abs(diff) + " dégâts à " + _playerOne._Pokemons[secondAttack[i]].Item2._Name + " grâce à " + _playerTwo._Pokemons[playerTwoChallengers[i]].Item2._AttackName);
                        } else if (diff < 0) {
                            _playerTwo._Pokemons[playerTwoChallengers[i]].Item2._Life -= Math.Abs(diff);
                            Console.WriteLine(_playerOne._Pokemons[secondAttack[i]].Item2._Name + " a infligé " + Math.Abs(diff) + " dégâts à " + _playerTwo._Pokemons[playerTwoChallengers[i]].Item2._Name + " grâce à " + _playerOne._Pokemons[secondAttack[i]].Item2._AttackName);
                        } else {
                            Console.WriteLine("L'attaque et la défense des deux combattants étant égales, personne n'a perdu de point de vie.");
                        }
                    }
                }
                if (typeof(Defender).IsInstanceOfType(_playerTwo._Pokemons[playerTwoChallengers[i]].Item2)) {
                    Defender firstDefender = (Defender)_playerTwo._Pokemons[playerTwoChallengers[i]].Item2;
                    if (typeof(Attacker).IsInstanceOfType(_playerOne._Pokemons[secondAttack[i]].Item2)) {
                        Attacker secondAttacker = (Attacker)_playerOne._Pokemons[secondAttack[i]].Item2;
                        int diff = firstDefender._DefensePoints - secondAttacker._AttackPoints;
                        if (diff > 0) {
                            _playerOne._Pokemons[secondAttack[i]].Item2._Life -= diff;
                            Console.WriteLine(_playerTwo._Pokemons[playerTwoChallengers[i]].Item2._Name + " a infligé " + Math.Abs(diff) + " dégâts à " + _playerOne._Pokemons[secondAttack[i]].Item2._Name + " grâce à " + _playerTwo._Pokemons[playerTwoChallengers[i]].Item2._AttackName);
                        } else if (diff < 0) {
                            _playerTwo._Pokemons[playerTwoChallengers[i]].Item2._Life -= Math.Abs(diff);
                            Console.WriteLine(_playerOne._Pokemons[secondAttack[i]].Item2._Name + " a infligé " + Math.Abs(diff) + " dégâts à " + _playerTwo._Pokemons[playerTwoChallengers[i]].Item2._Name + " grâce à " + _playerOne._Pokemons[secondAttack[i]].Item2._AttackName);
                        } else {
                            Console.WriteLine("L'attaque et la défense des deux combattants étant égales, personne n'a perdu de point de vie.");
                        }
                    }
                    if (typeof(Defender).IsInstanceOfType(_playerOne._Pokemons[secondAttack[i]].Item2)) {
                        Defender secondDefender = (Defender)_playerOne._Pokemons[secondAttack[i]].Item2;
                        int diff = firstDefender._DefensePoints - secondDefender._DefensePoints;
                        if (diff > 0) {
                            _playerOne._Pokemons[secondAttack[i]].Item2._Life -= 10;
                            Console.WriteLine(_playerTwo._Pokemons[playerTwoChallengers[i]].Item2._Name + " a infligé 10 dégâts à " + _playerOne._Pokemons[secondAttack[i]].Item2._Name + " grâce à " + _playerTwo._Pokemons[playerTwoChallengers[i]].Item2._AttackName);
                        } else if (diff < 0) {
                            _playerTwo._Pokemons[playerTwoChallengers[i]].Item2._Life -= 10;
                            Console.WriteLine(_playerOne._Pokemons[secondAttack[i]].Item2._Name + " a infligé 10 dégâts à " + _playerTwo._Pokemons[playerTwoChallengers[i]].Item2._Name + " grâce à " + _playerOne._Pokemons[secondAttack[i]].Item2._AttackName);
                        } else {
                            Console.WriteLine("La défense des deux combattants étant égales, personne n'a perdu de point de vie.");
                        }
                    }
                }
                Console.Write("Appuyez sur une touche pour découvrir le combat suivant");
                Console.ReadKey();
                Console.Clear();
            }
        }
        public void fight() {
            Console.Write("C'est l'heure du combat ! Appuyez sur une touche pour voir les combattants !");
            Console.ReadKey();
            Console.Clear();
            List<int> playerOneChallengers = new List<int>();
            List<int> playerTwoChallengers = new List<int>();
            for (int i = 0; i != _playerOne._Pokemons.Count(); ++i) {
                if (_playerOne._Pokemons[i].Item1 == 1)
                    playerOneChallengers.Add(i);
            }
            for (int i = 0; i != _playerTwo._Pokemons.Count(); ++i) {
                if (_playerTwo._Pokemons[i].Item1 == 1)
                    playerTwoChallengers.Add(i);
            }
            if (playerOneChallengers.Count() == 1)
                Console.WriteLine("Le challenger du joueur 1 est :");
            else
                Console.WriteLine("Les challengers du joueur 1 sont :");
            foreach (int challenger in playerOneChallengers) {
                if (typeof(Attacker).IsInstanceOfType(_playerOne._Pokemons[challenger].Item2)) {
                    Attacker attacker = (Attacker)_playerOne._Pokemons[challenger].Item2;
                    Console.WriteLine("-" + attacker._Name + ", a une attaque de " + attacker._AttackPoints + " et " + attacker._Life + " points de vie restants");
                }
                if (typeof(Defender).IsInstanceOfType(_playerOne._Pokemons[challenger].Item2)) {
                    Defender defender = (Defender)_playerOne._Pokemons[challenger].Item2;
                    Console.WriteLine("-" + defender._Name + ", a une défense de " + defender._DefensePoints + " et " + defender._Life + " points de vie restants");
                }
            }
            Console.WriteLine("----------------------------------------------------------------");
            if (playerTwoChallengers.Count() == 1)
                Console.WriteLine("Le challenger du joueur 2 est :");
            else
                Console.WriteLine("Les challengers du joueur 2 sont :");
            foreach (int challenger in playerTwoChallengers) {
                if (typeof(Attacker).IsInstanceOfType(_playerTwo._Pokemons[challenger].Item2)) {
                    Attacker attacker = (Attacker)_playerTwo._Pokemons[challenger].Item2;
                    Console.WriteLine("-" + attacker._Name + ", a une attaque de " + attacker._AttackPoints + " et " + attacker._Life + " points de vie restants");
                }
                if (typeof(Defender).IsInstanceOfType(_playerTwo._Pokemons[challenger].Item2)) {
                    Defender defender = (Defender)_playerTwo._Pokemons[challenger].Item2;
                    Console.WriteLine("-" + defender._Name + ", a une défense de " + defender._DefensePoints + " et " + defender._Life + " points de vie restants");
                }
            }
            Console.WriteLine("----------------------------------------------------------------");
            Console.Write("Appuyez sur une touche pour passer au combat et à l'application des bonus de terrain !");
            Console.ReadKey();
            Console.Clear();
            List<int> firstAttack = chooseTarget(playerOneChallengers, playerTwoChallengers, true);
            List<int> secondattack = chooseTarget(playerTwoChallengers, playerOneChallengers, false);
            damageStep(firstAttack, secondattack, playerOneChallengers, playerTwoChallengers);
            foreach (int pokemon in playerOneChallengers) {
                if (_playerOne._Pokemons[pokemon].Item2._Life <= 0) {
                    Console.WriteLine(_playerOne._Pokemons[pokemon].Item2._Name + " a été tué par le Joueur 2 !");
                    _playerOne._Pokemons.RemoveAt(pokemon);
                }
            }
            foreach (int pokemon in playerTwoChallengers) {
                if (_playerTwo._Pokemons[pokemon].Item2._Life <= 0) {
                    Console.WriteLine(_playerTwo._Pokemons[pokemon].Item2._Name + " a été tué par le Joueur 1 !");
                    _playerTwo._Pokemons.RemoveAt(pokemon);
                }
            }
            Console.Write("Combat terminé ! Appuyez sur une touche pour passer au round suivant !");
            Console.ReadKey();
            Console.Clear();
            for (int i = 0; i != _playerOne._Pokemons.Count(); ++i)
                _playerOne._Pokemons[i] = new Tuple<int, Pokemon>(0, _playerOne._Pokemons[i].Item2);
            for (int i = 0; i != _playerTwo._Pokemons.Count(); ++i)
                _playerTwo._Pokemons[i]= new Tuple<int, Pokemon>(0, _playerTwo._Pokemons[i].Item2);
            return;
        }

        
        public void endGame() {
            if (_playerOne._Pokemons.Count() == 0)
                Console.WriteLine("Victoire écrasante du Joueur 2 !");
            else
                Console.WriteLine("Victoire écrasante du Joueur 1 !");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game();
            game.gameLoop();
            return;
        }
    }
}
