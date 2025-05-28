public abstract class Tropa
{
    public int Vida { get; protected set; }
    public int Ataque { get; protected set; }
    public int Defensa { get; protected set; }
    public Jugador Due�o { get; private set; }

    public int TurnosRestantes { get; set; }  // Turnos para producci�n (nuevo)

    public Tropa(Jugador due�o)
    {
        Due�o = due�o;
        TurnosRestantes = ObtenerTurnosProduccion();
    }

    public void SetVida(int i) {
        Vida = i;
    }
    public abstract int ObtenerTurnosProduccion(); // Nuevo m�todo abstracto para turnos de producci�n
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
    public override int ObtenerTurnosProduccion()
    {
        return 0;  // Infanter�a se produce autom�ticamente, sin espera
    }
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
    public override int ObtenerTurnosProduccion() => 4;  // 4 turnos para Artiller�a
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
    public override int ObtenerTurnosProduccion() => 2;  // 2 turnos para Caballer�a
}


