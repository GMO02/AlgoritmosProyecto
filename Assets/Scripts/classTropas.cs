public abstract class Tropa
{
    public int Vida { get; protected set; }
    public int Ataque { get; protected set; }
    public int Defensa { get; protected set; }
    public Jugador Due�o { get; private set; }

    public Tropa(Jugador due�o)
    {
        Due�o = due�o;
    }

    public void SetVida(int i) {
        Vida = i;
    }
    public abstract string Tipo { get; }  // Para identificar el tipo en la UI
}
public class Infanteria : Tropa
{
    public Infanteria(Jugador due�o) : base(due�o)
    {
        Vida = 100;
        Ataque = 15;
        Defensa = 10;
    }

    public override string Tipo => "Infanter�a";
}
public class Artilleria : Tropa
{
    public Artilleria(Jugador due�o) : base(due�o)
    {
        Vida = 80;
        Ataque = 25;
        Defensa = 5;
    }

    public override string Tipo => "Artiller�a";
}
public class Caballeria : Tropa
{
    public Caballeria(Jugador due�o) : base(due�o)
    {
        Vida = 120;
        Ataque = 20;
        Defensa = 15;
    }

    public override string Tipo => "Caballer�a";
}


