public abstract class Tropa
{
    public int Vida { get; protected set; }
    public int Ataque { get; protected set; }
    public int Defensa { get; protected set; }
    public Jugador Dueño { get; private set; }

    public int TurnosRestantes { get; set; }  // Turnos para producción (nuevo)

    public Tropa(Jugador dueño)
    {
        Dueño = dueño;
        TurnosRestantes = ObtenerTurnosProduccion();
    }

    public void SetVida(int i) {
        Vida = i;
    }
    public abstract int ObtenerTurnosProduccion(); // Nuevo método abstracto para turnos de producción
    public abstract string Tipo { get; }  // Para identificar el tipo en la UI
}
public class Infanteria : Tropa
{
    public Infanteria(Jugador dueño) : base(dueño)
    {
        Vida = 100;
        Ataque = 15;
        Defensa = 10;
    }

    public override string Tipo => "Infantería";
    public override int ObtenerTurnosProduccion()
    {
        return 0;  // Infantería se produce automáticamente, sin espera
    }
}
public class Artilleria : Tropa
{
    public Artilleria(Jugador dueño) : base(dueño)
    {
        Vida = 80;
        Ataque = 25;
        Defensa = 5;
    }

    public override string Tipo => "Artillería";
    public override int ObtenerTurnosProduccion() => 4;  // 4 turnos para Artillería
}
public class Caballeria : Tropa
{
    public Caballeria(Jugador dueño) : base(dueño)
    {
        Vida = 120;
        Ataque = 20;
        Defensa = 15;
    }

    public override string Tipo => "Caballería";
    public override int ObtenerTurnosProduccion() => 2;  // 2 turnos para Caballería
}


