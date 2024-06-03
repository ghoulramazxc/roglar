using System;
using System.Collections.Generic;

namespace RoguelikeGame
{
    // Класс Оружие
    public class Weapon
    {
        public string Name { get; set; }
        public int Damage { get; set; }
        public int Durability { get; set; }

        public Weapon(string name, int damage, int durability)
        {
            Name = name;
            Damage = damage;
            Durability = durability;
        }

        public void Use()
        {
            Durability--;
        }

        public bool IsBroken()
        {
            return Durability <= 0;
        }
    }

    // Класс Аптечка
    public class Aid
    {
        public string Name { get; set; }
        public int HealAmount { get; set; }

        public Aid(string name, int healAmount)
        {
            Name = name;
            HealAmount = healAmount;
        }
    }

    // Класс Противник
    public class Enemy
    {
        public string Name { get; set; }
        public int CurrentHealth { get; set; }
        public int MaxHealth { get; set; }
        public Weapon Weapon { get; set; }

        public Enemy(string name, int maxHealth, Weapon weapon)
        {
            Name = name;
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
            Weapon = weapon;
        }

        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;
            if (CurrentHealth < 0)
            {
                CurrentHealth = 0;
            }
        }

        public bool IsDead()
        {
            return CurrentHealth <= 0;
        }

        public int Attack()
        {
            if (!Weapon.IsBroken())
            {
                Weapon.Use();
                return Weapon.Damage;
            }
            else
            {
                return 1; // минимальный урон, если оружие сломано
            }
        }
    }

    // Класс Игрок
    public class Player
    {
        public string Name { get; set; }
        public int CurrentHealth { get; set; }
        public int MaxHealth { get; set; }
        public Aid AidKit { get; set; }
        public Weapon Weapon { get; set; }
        public int Score { get; set; }

        public Player(string name, int maxHealth, Aid aidKit, Weapon weapon)
        {
            Name = name;
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
            AidKit = aidKit;
            Weapon = weapon;
            Score = 0;
        }

        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;
            if (CurrentHealth < 0)
            {
                CurrentHealth = 0;
            }
        }

        public void Heal()
        {
            CurrentHealth += AidKit.HealAmount;
            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
        }

        public bool IsDead()
        {
            return CurrentHealth <= 0;
        }

        public int Attack()
        {
            if (!Weapon.IsBroken())
            {
                Weapon.Use();
                return Weapon.Damage;
            }
            else
            {
                return 1; // минимальный урон, если оружие сломано
            }
        }

        public void IncreaseScore(int points)
        {
            Score += points;
        }
    }

    class Program
    {
        static Random random = new Random();

        static void Main(string[] args)
        {
            Console.WriteLine("Добро пожаловать, воин!");
            Console.WriteLine("Назови себя:");
            string playerName = Console.ReadLine();

            Weapon playerWeapon = new Weapon("Фламберг", 20, 10);
            Aid playerAidKit = new Aid("Средняя аптечка", 10);
            Player player = new Player(playerName, 100, playerAidKit, playerWeapon);

            Console.WriteLine($"Ваше имя {playerName}!");
            Console.WriteLine($"Вам был ниспослан меч {playerWeapon.Name} ({playerWeapon.Damage}), а также {playerAidKit.Name} ({playerAidKit.HealAmount}hp).");
            Console.WriteLine($"У вас {player.CurrentHealth}hp.");

            while (!player.IsDead())
            {
                Enemy enemy = GenerateRandomEnemy();
                Console.WriteLine($"{player.Name} встречает врага {enemy.Name} ({enemy.CurrentHealth}hp), у врага на поясе сияет оружие {enemy.Weapon.Name} ({enemy.Weapon.Damage})");
                bool playerTurn = true;

                while (!enemy.IsDead() && !player.IsDead())
                {
                    Console.WriteLine("Что вы будете делать?");
                    Console.WriteLine("1. Ударить");
                    Console.WriteLine("2. Пропустить ход");
                    Console.WriteLine("3. Использовать аптечку");

                    string choice = Console.ReadLine();

                    if (choice == "1" && playerTurn)
                    {
                        int damage = player.Attack();
                        enemy.TakeDamage(damage);
                        Console.WriteLine($"{player.Name} ударил противника {enemy.Name}");
                        Console.WriteLine($"У противника {enemy.CurrentHealth}hp, у вас {player.CurrentHealth}hp");
                        playerTurn = false;
                    }
                    else if (choice == "2" && playerTurn)
                    {
                        Console.WriteLine($"{player.Name} пропустил ход");
                        playerTurn = false;
                    }
                    else if (choice == "3" && playerTurn)
                    {
                        player.Heal();
                        Console.WriteLine($"{player.Name} использовал аптечку");
                        Console.WriteLine($"У противника {enemy.CurrentHealth}hp, у вас {player.CurrentHealth}hp");
                        playerTurn = false;
                    }

                    if (!playerTurn && !enemy.IsDead())
                    {
                        int damage = enemy.Attack();
                        player.TakeDamage(damage);
                        Console.WriteLine($"Противник {enemy.Name} ударил вас!");
                        Console.WriteLine($"У противника {enemy.CurrentHealth}hp, у вас {player.CurrentHealth}hp");
                        playerTurn = true;
                    }
                }

                if (player.IsDead())
                {
                    Console.WriteLine("Вы погибли!");
                }
                else if (enemy.IsDead())
                {
                    player.IncreaseScore(10);
                    Console.WriteLine($"Вы победили {enemy.Name} и получили 10 очков!");
                    Console.WriteLine($"Ваши текущие очки: {player.Score}");
                }
            }

            Console.WriteLine($"Игра окончена! Ваш итоговый счет: {player.Score}");
        }

        static Enemy GenerateRandomEnemy()
        {
            string[] enemyNames = { "Варвар", "Гоблин", "Огр", "Тролль" };
            string[] weaponNames = { "Экскалибур", "Боевой топор", "Короткий меч", "Длинный лук" };
            int[] weaponDamages = { 10, 15, 5, 8 };
            int[] weaponDurabilities = { 10, 8, 15, 12 };

            int randomIndex = random.Next(enemyNames.Length);
            string enemyName = enemyNames[randomIndex];
            int enemyHealth = random.Next(30, 61);
            string weaponName = weaponNames[randomIndex];
            int weaponDamage = weaponDamages[randomIndex];
            int weaponDurability = weaponDurabilities[randomIndex];

            Weapon enemyWeapon = new Weapon(weaponName, weaponDamage, weaponDurability);
            return new Enemy(enemyName, enemyHealth, enemyWeapon);
        }
    }
}