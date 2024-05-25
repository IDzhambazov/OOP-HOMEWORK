
public abstract class Hero
{
    public string Name { get; private set; }
    public int Health { get; protected set; }
    public int Damage { get; protected set; }

    public Hero(string name, int health, int damage)
    {
        Name = name;
        Health = health;
        Damage = damage;
    }

    public virtual void Attack(Hero opponent)
    {
        Console.WriteLine($"{Name} удря {opponent.Name} и му нанася {Damage} щети.");
        opponent.TakeDamage(Damage);
    }

    public virtual void TakeDamage(int damage)
    {
        Health -= damage;
        Console.WriteLine($"{Name} получава {damage} щети. Оставащо здраве: {Health}");
    }

    public virtual void Defend(int incomingDamage)
    {
        Console.WriteLine($"{Name} се защитава и получава {incomingDamage} щети.");
        Health -= incomingDamage;
        Console.WriteLine($"{Name} остава с {Health} здраве.");
    }
}


public class Knight : Hero
{
    public Knight(string name) : base(name, 120, 15)
    {
    }

    public override void Attack(Hero opponent)
    {
        base.Attack(opponent);
        Console.WriteLine($"{Name} използва своя меч върху {opponent.Name}!");
    }

    public override void Defend(int incomingDamage)
    {
        base.Defend(incomingDamage);
        Console.WriteLine($"{Name} блокира част от щетите си с щита.");
    }
}


public class Rogue : Hero
{
    public Rogue(string name) : base(name, 100, 20)
    {
    }

    public override void Attack(Hero opponent)
    {
        base.Attack(opponent);
        Console.WriteLine($"{Name} използва своя кинжал върху {opponent.Name}!");
    }

    public override void Defend(int incomingDamage)
    {
        base.Defend(incomingDamage);
        Console.WriteLine($"{Name} избягва част от щетите си.");
    }
}


public class Wizard : Hero
{
    public Wizard(string name) : base(name, 80, 25)
    {
    }

    public override void Attack(Hero opponent)
    {
        base.Attack(opponent);
        Console.WriteLine($"{Name} изпраща мощен магически удар върху {opponent.Name}!");
    }

    public override void Defend(int incomingDamage)
    {
        base.Defend(incomingDamage);
        Console.WriteLine($"{Name} използва магията си, за да намали щетите.");
    }
}


public class Barbarian : Hero
{
    public Barbarian(string name) : base(name, 150, 12)
    {
    }

    public override void Attack(Hero opponent)
    {
        base.Attack(opponent);
        Console.WriteLine($"{Name} използва своя огромен бой върху {opponent.Name}!");
    }

    public override void Defend(int incomingDamage)
    {
        base.Defend(incomingDamage);
        Console.WriteLine($"{Name} търпеливо издържа щетите.");
    }
}

public class ConsoleGameEventListener : GameEventListener
{
    public override void GameRound(Hero attacker, Hero defender, int attack)
    {
        string message = $"{attacker.Name} напада {defender.Name} и нанася {attack} щети";
        if (defender.Health <= 0)
        {
            message += $", {defender.Name} умира.";
        }
        else
        {
            message += $", но {defender.Name} остава жив.";
        }
        Console.WriteLine(message);
    }
}

public class Arena
{
    private  Hero[] heroes;
    public GameEventListener EventListener { get; set; }

    public Arena(params Hero[] heroes)
    {
        this.heroes = heroes;
    }

    public Hero Battle()
    {
        Random random = new Random();
        while (heroes.Length > 1)
        {
            
            int attackerIndex = random.Next(heroes.Length);
            int defenderIndex;
            do
            {
                defenderIndex = random.Next(heroes.Length);
            } while (defenderIndex == attackerIndex);

            Hero attacker = heroes[attackerIndex];
            Hero defender = heroes[defenderIndex];

            int attack = attacker.Damage;
            defender.TakeDamage(attack);
            EventListener?.GameRound(attacker, defender, attack);

            
            heroes = RemoveDeadHeroes(heroes);
        }

        return heroes[0];
    }

    private Hero[] RemoveDeadHeroes(Hero[] heroes)
    {
        return Array.FindAll(heroes, hero => hero.Health > 0);
    }
}

public abstract class GameEventListener
{
    public abstract void GameRound(Hero attacker, Hero defender, int attack);
}

internal class Program
{
    static void Main(string[] args)
    {
        Hero[] heroes = CreateHeroes();
        Arena arena = new Arena(heroes);
        arena.EventListener = new ConsoleGameEventListener();

        Console.WriteLine("Битката започва.");
        Hero winner = arena.Battle();
        Console.WriteLine($"Битката приключи. Победител е: {winner.Name}");
        Console.ReadLine();
    }

    static Hero[] CreateHeroes()
    {
        return new Hero[]
        {
            new Knight("Сър Джон"),
            new Rogue("Слим Шейди"),
            new Wizard("Магистър Гандалф"),
            new Barbarian("Кръвожадният Бор")
        };
    }
}
