using System;
using System.IO;

public interface IMediator
{
    void RealizujOperacje(IOperacjaFinansowa operacja);
}

public interface IOperacjaFinansowa
{
    void Realizuj();
}

public interface IWplacalne { }
public interface IWyplacalne { }

public interface IPodatekStrategia
{
    decimal ObliczPodatek(decimal kwota);
}

public class Bank : IMediator
{
    private const string PlikOperacji = "operacje.txt";

    public void RealizujOperacje(IOperacjaFinansowa operacja)
    {
        operacja.Realizuj();
        ZapiszDoPliku(operacja.GetType().Name);
    }

    public void ZapiszDoPliku(string operacja)
    {
        if (!File.Exists(PlikOperacji))
        {
            File.Create(PlikOperacji).Close();
        }
        File.AppendAllText(PlikOperacji, $"{DateTime.Now}: Wykonano operację {operacja}\n");
    }
}

public class Wplata : IOperacjaFinansowa, IWplacalne
{
    private readonly IMediator _mediator;

    public Wplata(IMediator mediator)
    {
        _mediator = mediator;
    }

    public void Realizuj()
    {
        Console.WriteLine("Wykonano operację wpłaty.");
    }
}

public class Wyplata : IOperacjaFinansowa, IWyplacalne
{
    private readonly IMediator _mediator;

    public Wyplata(IMediator mediator)
    {
        _mediator = mediator;
    }

    public void Realizuj()
    {
        Console.WriteLine("Wykonano operację wypłaty.");
    }
}

public class PodatekPolska : IPodatekStrategia
{
    public decimal ObliczPodatek(decimal kwota)
    {
        return kwota * 0.23m;
    }
}

public class PodatekNiemcy : IPodatekStrategia
{
    public decimal ObliczPodatek(decimal kwota)
    {
        return kwota * 0.19m;
    }
}
public class KalkulatorPodatku
{
    private readonly IPodatekStrategia _strategia;

    public KalkulatorPodatku(IPodatekStrategia strategia)
    {
        _strategia = strategia;
    }

    public decimal Oblicz(decimal kwota)
    {
        return _strategia.ObliczPodatek(kwota);
    }
}
class Program
{
    static void Main(string[] args)
    {
        Bank bank = new Bank();

        IOperacjaFinansowa wplata = new Wplata(bank);
        IOperacjaFinansowa wyplata = new Wyplata(bank);

        bank.RealizujOperacje(wplata);
        bank.RealizujOperacje(wyplata);

        Console.WriteLine();

        KalkulatorPodatku kalkulator = new KalkulatorPodatku(new PodatekPolska());
        decimal podatekPolska = kalkulator.Oblicz(1000);
        Console.WriteLine($"Podatek w Polsce od 1000 zł: {podatekPolska} zł");

        kalkulator = new KalkulatorPodatku(new PodatekNiemcy());
        decimal podatekNiemcy = kalkulator.Oblicz(1000);
        Console.WriteLine($"Podatek w Niemczech od 1000 €: {podatekNiemcy} €");
    }
}