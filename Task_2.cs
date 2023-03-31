using System;
using System.Runtime.ExceptionServices;

namespace ConsoleApp1
{
    abstract class TimeHelper
    {
        private DateTime createdAt;
        private DateTime updatedAt;
        private DateTime deletedAt;
        private bool isDelete = false;
        private const string format = "dddd, d/MM/yy, H:mm, K";

        public TimeHelper()
        {
            createdAt = DateTime.Now;
            updatedAt = DateTime.Now;
        }

        public void deleted()
        {
            isDelete = true;
            deletedAt = DateTime.Now;
        }

        protected void updated()
        {
            updatedAt = DateTime.Now;
        }

        protected bool checkUpdate()
        {
            return !createdAt.Equals(updatedAt);
        }

        public string getFormatCreatedAt()
        {
            return createdAt.ToString(format);
        }
    }

    abstract class Name
    {
        protected string name = "";

        public void setName(string name)
        {
            this.name = name;
        }

        public string getName() => this.name;

        ~Name()
        {

        }
    };

    class Price : Name {
	private int price = 0;
	public Price(string name, int price)
    {
        this.price = price;
    }

    public void setPrice(int price)
    {
        this.price = price;
    }

    public int getPrice() => this.price;

    ~Price()
    {

    }
};

class Shield : Price
    {
        protected int protect = 0;
        public Shield(int protect, string name, int price = 0) : base(name, price)
        {
            this.protect = protect;
            this.setName(name);
            this.setPrice(price);
        }
        public void setProtect(int protect)
        {
            this.protect = protect;
        }

        public int getProtect() => this.protect;


        ~Shield()
        {

        }
    };

    class Weapon : Price
    {
        protected int damage = 0;
        public Weapon(int damage, string name, int price = 0) : base(name, price)
        {
            this.setName(name);
            this.damage = damage;
            this.setPrice(price);
        }

        public void setDamage(int damage)
        {
            this.damage = damage;
        }

        public int getDamage() => this.damage;

        ~Weapon()
        {

        }
    };

    interface IUseSkill : IGetName
    {
        void use(Monster monster);
    }

    interface IDrink : IGetName
    {
        void drink(Player player);
    }

    interface IGetName
    {
        string getName();
    }

    abstract class Potion : Name
    {
        protected int size = 0;

        private string generateName()
        {
            string name = "";

            if (this.size == 1)
                name = "Little ";
            else if (this.size == 2)
                name = "Middle ";
            else
                name = "Legendary ";

            return name;
        }

        public Potion(int size)
        {
            this.size = size;
            this.name = generateName() + this.GetType().Name;
        }
    }

    class HealthPotion : Potion, IDrink
    {
        public HealthPotion(int size) : base(size)
        {
        }

        public void drink(Player player)
        {
            player.addHealth(this.size * 100);
        }
    }

    class StrenghtPotion : Potion, IDrink
    {
        public StrenghtPotion(int size) : base(size)
        {
        }
        public void drink(Player player)
        {
            int[] values = { 1, 3, 6 };

            player.addPower(values[this.size - 1]);
        }
    }

    abstract class CombatSkill : Name
    {
        public CombatSkill()
        {
            this.name = this.GetType().Name;
        }
    }

    class Hook : CombatSkill, IUseSkill
    {
        public void use(Monster mob)
        {
            mob.setHealth(mob.getHealth() - 100);
        }
    }

    class Energyball : CombatSkill, IUseSkill
    {
        public void use(Monster mob)
        {
            mob.setHealth(mob.getHealth() - 500);
        }
    }

    class Kraccbacc : CombatSkill, IUseSkill
    {
        public void use(Monster mob)
        {
            mob.setHealth(mob.getHealth() - 300);
        }
    }
    class BasePerson : TimeHelper
    {
        protected int health = 0;
        protected int healthMax = 0;
        protected int energy = 0;
        protected int level = 0;
        protected string name = "";
        protected int money = 0;

        public BasePerson(string name, int level)
        {
            this.name = name;
            this.level = level;
        }

        public void showInfo()
        {
            Console.WriteLine($"{this.name} {this.level} lvl.");

            Console.Write("\t[");
            double tmp = ((double)this.health / (double)this.healthMax) * 10;
            for (int i = 0; i < 10; i++)
            {
                if (i < tmp)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("♥");
                    Console.ResetColor();
                }
                else
                    Console.Write("♥");
            }
            Console.Write("]");
            Console.WriteLine($"\n\t {this.health} / {this.healthMax}");
        }

        public void setHealth(int health)
        { this.health = health; }

        public void setEnergy(int energy)
        { this.energy = energy; }

        public void setLevel(int level)
        { this.level = level; }

        public void setName(string name)
        { this.name = name; }
        public void setMoney(int money)
        { this.money = money; }


        public string getName() => this.name;

        public int getHealth() => this.health;

        public void addHealth(int value)
        {
            if (value + this.health > this.healthMax)
                this.health = this.healthMax;
            else
                this.health += value;
        }

        public int getEnergy() => this.energy;

        public int getLevel() => this.level;

        public int getMoney() => this.money;
    }

    class Tank : Player
    {
        public Tank(string name) : base(name, 1200)
        {
            this.power = 10;
            this.agility = 5;
            this.endurance = 20;
        }
    }

    class Rogue : Player
    {
        public Rogue(string name) : base(name, 400)
        {
            this.power = 10;
            this.agility = 20;
            this.endurance = 5;
        }
    }
    class Barbar : Player
    {
        public Barbar(string name) : base(name, 300)
        {
            this.power = 15;
            this.agility = 10;
            this.endurance = 10;
        }
    }

    class Player : BasePerson
    {
        protected int power = 0;
        protected int agility = 0;
        protected int endurance = 0;
        protected int damage = 0;
        protected int protect = 0;
        protected int wallet = 0;
        protected List<IUseSkill> skills;
        private IDrink potion = null;
        private int xp = 0;
        private Shield shield = null;
        private Weapon armament = null;
        private Random rand = null;

        public Player(string name, int hp) : base(name, 1)
        {
            this.health = hp;
            this.healthMax = hp;
            this.skills = new List<IUseSkill>();
            rand = new Random();
        }

        public void setShield(Shield shield) => this.shield = shield;
        public void setWeapon(Weapon armament) => this.armament = armament;

        public void showEquipment()
        {
            Console.WriteLine("Weapon: " + this.armament.getName() + " Value: " + this.armament.getDamage());
            Console.WriteLine("Shield: " + this.shield.getName() + " Value: " + this.shield.getProtect());
        }

        public void setWallet(int wallet) => this.wallet = wallet;

        public void setPotion(IDrink potion) => this.potion = potion;

        private bool isPotion()
        {
            if (this.potion == null)
                return false;

            return true;
        }

        public bool drinkPotion()
        {
            if (isPotion())
            {
                this.potion.drink(this);
                this.potion = null;

                return true;
            }

            return false;
        }

        public void addSkill(IUseSkill skill)
        {
            this.skills.Add(skill);
        }

        public void showSkillList()
        {
            int index = 0;

            Console.WriteLine("Choose skill: ");
            foreach (var item in this.skills)
            {
                Console.WriteLine($"{index} - {item.getName()}");
                index++;
            }
        }

        public void useSkill(int index, Monster mob)
        {
            this.skills[index].use(mob);
        }

        ~Player()
        { }

        public void setXp(int xp)
        { this.xp = xp; }

        public void addPower(int power)
        {
            this.power += power;
        }

        public void setPower(int power)
        { this.power = power; }

        public void setDamage(int damage)
        {
            if (agility % rand.Next(1, 11) == 0)
            {
                this.damage = (damage + armament.getDamage() + this.power % 100) * 2;
            }
            else
            {
                this.damage = damage + armament.getDamage() + this.power % 100;
            }
        }

        public void setProtect(int protect)
        {
            this.protect = protect + shield.getProtect() + this.endurance % 100;
        }



        public void setAgility(int agility)
        { this.agility = agility; }

        public void setEndurance(int endurance)
        { this.endurance = endurance; }

        public int getXp() => this.xp;

        public int getDamage() => this.damage;

        public int getProtect() => this.protect;

        public int getPower() => this.power;

        public int getAgility() => this.agility;

        public int getEndurance() => this.endurance;

        public int getWallet() => this.wallet;
    };

    class Monster : BasePerson
    {
        private int damage = 0;
        private int protect = 0;
        private int xpPlus = 0;

        public Monster(string name, int level) : base(name, level)
        {
            this.damage = level * 11;
            this.health = level * 120;
            this.healthMax = level * 120;
            this.protect = level * 8;
            this.xpPlus = level * 20;
        }

        public void setDamage(int damage)
        { this.damage = damage; }

        public void setProtect(int protect)
        { this.protect = protect; }

        public void setXpPlus(int xpPlus)
        { this.xpPlus = xpPlus; }

        public int getDamage() => this.damage;

        public int getProtect() => this.protect;

        public int getXpPlus() => this.xpPlus;


        ~Monster()
        { }
    };

    class Engine
    {
        private List<string> armorNames = null;
        private List<string> weaponNames = null;
        private Random rand = null;
        private string[] list = new string[] { "Сyclop", "Minotaur", "Deymos", "Troll", "Orc", "Elf", "Wizzard", "Illager", "Skeleton", "Murder" };

        public Engine(Random rand)
        {
            this.rand = rand;
            this.fillArmorNames();
        }


        private void fillArmorNames()
        {
            this.armorNames = new List<string>();

            this.armorNames.Add("package ATB");
            this.armorNames.Add("wood");
            this.armorNames.Add("stone");
            this.armorNames.Add("steel");
            this.armorNames.Add("IRIS");
            this.armorNames.Add("diamond");
            this.armorNames.Add("magic shield");
        }

        public string generateWeaponShopText()
        {
            return generateShopPhrase("Enter your new weapon", this.weaponNames);
        }

        public string generateArmorShopText()
        {
            return generateShopPhrase("Enter your new shield", this.armorNames);
        }

        private string generateShopPhrase(string basePhrase, List<string> names)
        {
            basePhrase += "(";

            for (int i = 0; i < names.Count; i++)
            {
                basePhrase += names[i];

                if (i != names.Count - 1)
                    basePhrase += ", ";
            }

            basePhrase += "): ";

            return basePhrase;
        }



        public Monster createMonster(int level)
        {
            return new Monster(this.list[rand.Next(0, list.GetLength(0))], (level + rand.Next(-1, 2)));
        }

        public Weapon createWeapon(string choose2)
        {
            Weapon gun = null;

            if (choose2 == "spear")
            {
                gun = new Weapon(3, "spear");
            }
            else if (choose2 == "lightsaber")
            {
                gun = new Weapon(5, "lightsaber");
            }
            else if (choose2 == "bow")
            {
                gun = new Weapon(7, "bow");
            }
            else if (choose2 == "fireball")
            {
                gun = new Weapon(9, "fireball");
            }
            else if (choose2 == "axe")
            {
                gun = new Weapon(11, "axe");
            }
            else if (choose2 == "AK-47")
            {
                gun = new Weapon(15, "AK-47");
            }
            else if (choose2 == "Cesium-137" || choose2 == "Cs-137" || choose2 == "Cs")
            {
                gun = new Weapon(20, "Cesium-137");
            }
            return gun;
        }

        public bool checkArmor(string choose2)
        {
            if (this.armorNames.IndexOf(choose2) == -1)
                return false;
            else
                return true;
        }

        public Shield createShield(string choose2)
        {
            int[] protectList = { 3, 5, 7, 9, 11, 15, 20 };

            int index = this.armorNames.IndexOf(choose2);

            return new Shield(protectList[index], this.armorNames[index]);
        }

        public Player createPlayer(string name, string choose)
        {
            Player player = null;

            if (choose == "barbar")
            {
                player = new Barbar(name);
            }
            else if (choose == "tank")
            {
                player = new Tank(name);
            }
            else if (choose == "rogue")
            {
                player = new Rogue(name);
            }

            player.addSkill(new Hook());
            player.addSkill(new Energyball());
            player.addSkill(new Kraccbacc());

            player.showInfo();

            return player;
        }

    }

    class Event
    {
        private int minus = 0;
        private Player player = null;
        private Monster enemy = null;
        private Random rand = null;

        private Weapon buyWeapon(int wallet, int damage, int price, string name)
        {

            if (checkWallet(price, wallet))
            {
                return new Weapon(damage, name, price);
            }

            return null;
        }

        private Shield buyShield(int wallet, int protect, int price, string name)
        {

            if (checkWallet(price, wallet))
            {
                return new Shield(protect, name, price);
            }

            return null;
        }

	    public Event (Player player, Random rand)
            {
                this.rand = rand;
                this.player = player;
            }

        public string typeName(string text)
        {
            Console.WriteLine(text);
            text = Console.ReadLine();
            return text;
        }

        public int typeNameInt(string text)
        {
            int number = 0;
            Console.WriteLine(text);
            number = Convert.ToInt32(Console.ReadLine());
            return number;
        }

        public bool checkWallet(int price, int wallet)
        {
            if (price > wallet)
            {
                Console.WriteLine("Sorry, we can't give credit! Come back when you been little mmm..... Richer!");
                return false;
            }
            else
            {
                this.player.setWallet(wallet - price);
                return true;
            }
        }


        public Weapon shopWeapon(int choose3)
        {
            Weapon gun = null;
            int wallet = player.getWallet();

            switch (choose3)
            {
                case 1:
                    return this.buyWeapon(wallet, 3, 30, "flint and stone");
                    break;
                case 2:
                    return this.buyWeapon(wallet, 5, 50, "sword");
                    break;
                case 3:
                    return this.buyWeapon(wallet, 7, 70, "bow");
                    break;
                case 4:
                    return this.buyWeapon(wallet, 9, 90, "fireball");
                    break;
                case 5:
                    return this.buyWeapon(wallet, 11, 110, "axe");
                    break;
                case 6:
                    return this.buyWeapon(wallet, 15, 150, "AK-47");
                    break;
                case 7:
                    return this.buyWeapon(wallet, 20, 200, "HIPL");
                    break;
                default:
                    Console.WriteLine("\aError: Wrong value! Try again!");
                    return null;
                    break;
            }

        }

        Shield shopShield(int choose3)
        {
            Shield target = null;
            int wallet = player.getWallet();

            switch (choose3)
            {
                case 1:
                    return this.buyShield(wallet, 3, 30, "package ATB");
                    break;
                case 2:
                    return this.buyShield(wallet, 5, 50, "wood");
                    break;
                case 3:
                    return this.buyShield(wallet, 7, 70, "stone");
                    break;
                case 4:
                    return this.buyShield(wallet, 9, 90, "steel");
                    break;
                case 5:
                    return this.buyShield(wallet, 11, 110, "IRIS");
                    break;
                case 6:
                    return this.buyShield(wallet, 15, 150, "graphene");
                    break;
                case 7:
                    return this.buyShield(wallet, 20, 200, "magic shield");
                    break;
                default:
                    Console.WriteLine("\aError: Wrong value! Try again!");
                    return null;
                    break;
            }
        }

        public void mainPage(int choose)
        {
            if (choose == 1)
            {
                this.player.setWeapon(shopWeapon(typeNameInt("Choose item: \n1-flint and stone-30$ \n2-sword-50$ \n3-bow-70$ \n4-fireball-90$ \n5-axe-110$ \n6-AK-47-150$ \n7-HIPL-200$")));
            }
            else if (choose == 2)
            {
                this.player.setShield(shopShield(typeNameInt("Choose item: \n1-package ATB-30$ \n2-wood-50$ \n3-stone-70$ \n4-steel-90$ \n5-IRIS-110$ \n6-graphene-150$ \n7-magic shield-200$(SALE -0,0000001%!!!)")));
            }
            else if (choose == 3)
            {
                Console.WriteLine("OK");
            }
            else
            {
                Console.WriteLine("\aError: Wrong value! Try again!");
                return;
            };
        }

        public void meetMonster()
        {
            int skillChoose = typeNameInt("Do you want to use skill(1-level up,2-critical damage,3-healing,4-no)");
            if (skillChoose < 1 || skillChoose > 3)
            {
                Console.WriteLine("\aError: Wrong value! Try again!");
                return;
            }
            else if (skillChoose == 1)
            {
                player.useSkill(0, enemy);
            }
            else if (skillChoose == 2)
            {
                player.useSkill(1, enemy);
            }
            else if (skillChoose == 3)
            {
                player.useSkill(2, enemy);
            }
            else
            {
                Console.WriteLine("OK!");
            }
            Engine action = new Engine(rand);
            this.enemy = action.createMonster(this.player.getLevel());


            Console.WriteLine("Monster has spawned!");

            int roundCount = 1;
            while (player.getHealth() > 0 && enemy.getHealth() > 0)
            {
                Console.WriteLine($"Round {roundCount}");
                Console.WriteLine("==============================");

                player.setHealth(player.getHealth() - enemy.getDamage());
                Console.WriteLine($"Player: {player.getHealth()}");

                enemy.setHealth(enemy.getHealth() - player.getDamage());
                Console.WriteLine($"Enemy: {enemy.getHealth()} \n");

                roundCount++;
            }

            if (player.getHealth() > 0)
            {
                Console.WriteLine("You win!");
                player.setXp(player.getXp() + enemy.getXpPlus());
            }
            else
            {
                Console.WriteLine("You lose!");
            }
        }

        public void increaseCharacter(int randChoose)
        {
            if (randChoose == 1)
            {
                Console.WriteLine("Level up! Congrats!");
            }
            else if (randChoose == 2)
            {
                player.setHealth(player.getHealth() + 150);
                Console.WriteLine("Your health bar has increased! Congrats!");
            }
            else if (randChoose == 3)
            {
                player.setEnergy(player.getEnergy() + 150);
                Console.WriteLine("Your energy bar has increased! Congrats!");
            }
            else if (randChoose == 4)
            {
                player.setAgility(player.getAgility() + 150);
                Console.WriteLine("You become agiliter! Congrats!");
            }
            else if (randChoose == 5)
            {
                player.setEndurance(player.getEndurance() + 150);
                Console.WriteLine("You become endurancer! Congrats!");
            }
            else if (randChoose == 6)
            {
                player.setPower(player.getPower() + 150);
                Console.WriteLine("Power You have more power! Congrats!");
            }
            else if (randChoose == 7)
            {
                player.setXp(player.getXp() + 150);
                Console.WriteLine("XP You become smarter! Congrats!");
            }
            else if (randChoose == 8)
            {
                int coin = rand.Next(1, 11);
                Console.WriteLine($"You got {coin} coins! Congrats!");

                player.setWallet(player.getWallet() + coin);
            }
        }

        public void nothing()
        {
            Console.Clear();
            Console.WriteLine("Nothing...");
            Console.WriteLine("    //_____ __");
            Console.WriteLine("   @ )====// .\\\\___");
            Console.WriteLine("   \\#\\_\\__(_/_\\_/");
            Console.WriteLine("     / /       \\\\  ");
            Console.WriteLine("That's a cricket");
        }

        ~Event() { }
    };


    internal class Program
    {

        static string inputText(string text)
        {
            Console.WriteLine(text);
            return Console.ReadLine();
        }

        static void Main()
        {
            Random rand = new Random();
            Engine engine = new Engine(new Random());
            Player player = engine.createPlayer(inputText("Enter you nickname: "), inputText("Enter your new character(barbar, tank, rogue): "));
            Event events = new Event(player, new Random());
            Console.Clear();


            player.setWeapon(engine.createWeapon(inputText("Enter your new weapon(AK-47, lightsaber, spear, axe, Cesium-137(Cs-137,Cs), bow): ")));
            Console.Clear();

            string choose = inputText(engine.generateArmorShopText());

            if (engine.checkArmor(choose))
            {
                player.setShield(engine.createShield(choose));
            }
            else
            {
                Console.WriteLine("Error!");
                return;
            }

            Console.Clear();
            player.showInfo();
            player.showEquipment();

            while (true)
            {
                int chance = rand.Next(1, 101);
                if (chance <= 5)
                {
                    events.mainPage(events.typeNameInt("Choose the option(1 - buy weapon, 2 - buy shield, 3 - buy potion, 4 - continue game"));
                }
                else if (chance > 5 && chance <= 30)
                {
                    events.increaseCharacter(rand.Next(1, 9));
                }
                else if (chance > 30 && chance <= 70)
                {
                    events.meetMonster();
                }
                else
                {
                    events.nothing();
                }
            }
        }
    }
}
