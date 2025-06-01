using System;
using System.Collections.Generic;

namespace ConsoleApp2
{
    class Program
    {
        public static class Logger
        {
            public static void Info(string message)
            {
                Console.WriteLine(message);
            }

            public static void InfoWithIndent(string message)
            {
                Console.WriteLine($"  {message}");
            }
        }
        public interface IOsoba
        {
            void Accept(IOsobaVisitor visitor);
        }

        public interface IOsobaVisitor
        {
            void Visit(Uczen uczen);
            void Visit(Nauczyciel nauczyciel);
            void Visit(Administrator administrator);
        }

        public class Uczen : IOsoba
        {
            public string Imie { get; }
            public List<int> Oceny { get; }

            public Uczen(string imie, List<int> oceny)
            {
                Imie = imie;
                Oceny = oceny;
            }

            public void Accept(IOsobaVisitor visitor)
            {
                visitor.Visit(this);
            }
        }

        public class Nauczyciel : IOsoba
        {
            public string Imie { get; }
            public int LiczbaWystawionychOcen { get; }

            public Nauczyciel(string imie, int liczbaWystawionychOcen)
            {
                Imie = imie;
                LiczbaWystawionychOcen = liczbaWystawionychOcen;
            }

            public void Accept(IOsobaVisitor visitor)
            {
                visitor.Visit(this);
            }
        }

        public class Administrator : IOsoba
        {
            public string Imie { get; }
            public List<string> Logi { get; }

            public Administrator(string imie)
            {
                Imie = imie;
                Logi = new List<string>();
            }

            public Administrator(string imie, List<string> logi)
            {
                Imie = imie;
                Logi = logi;
            }

            public void DodajUzytkownika()
            {
                Logi.Add("Dodano użytkownika");
            }

            public void Accept(IOsobaVisitor visitor)
            {
                visitor.Visit(this);
            }
        }

        public class RaportVisitor : IOsobaVisitor
        {
            public void Visit(Uczen uczen)
            {
                if (uczen.Oceny.Count > 0)
                {
                    double suma = 0;
                    foreach (int ocena in uczen.Oceny)
                    {
                        suma += ocena;
                    }
                    double srednia = suma / uczen.Oceny.Count;
                    Logger.Info($"Uczeń {uczen.Imie} - Średnia ocen: {srednia:F2}");
                }
                else
                {
                    Logger.Info($"Uczeń {uczen.Imie} - Brak ocen.");
                }
            }

            public void Visit(Nauczyciel nauczyciel)
            {
                Logger.Info($"Nauczyciel {nauczyciel.Imie} - Wystawił ocen: {nauczyciel.LiczbaWystawionychOcen}");
            }

            public void Visit(Administrator administrator)
            {
                Logger.Info($"Administrator {administrator.Imie} - Logi systemowe:");
                if (administrator.Logi.Count > 0)
                {
                    foreach (var log in administrator.Logi)
                    {
                        Logger.InfoWithIndent($"- {log}");
                    }
                }
                else
                {
                    Logger.InfoWithIndent("- Brak logów systemowych.");
                }
            }
        }

        public class ActionVisitor : IOsobaVisitor
        {
            private readonly Action<Administrator> _adminAction;

            public ActionVisitor(Action<Administrator> adminAction)
            {
                _adminAction = adminAction;
            }

            public void Visit(Uczen uczen) { }
            public void Visit(Nauczyciel nauczyciel) { }
            public void Visit(Administrator administrator)
            {
                _adminAction(administrator);
            }
        }

        static void Main(string[] args)
        {
            var osoby = new List<IOsoba>
            {
                new Uczen("Kasia", new List<int> { 5, 4, 3 }),
                new Uczen("Marek", new List<int>()), // brak ocen
                new Nauczyciel("Jan Kowalski", 142),
                new Administrator("Admin1", new List<string> { "Zalogowano użytkownika", "Zmieniono hasło" }), // symulacja logów 
                new Administrator("Admin2", new List<string>()), // brak logów
                new Administrator("Admin3") // pusta lista logów
            };

            osoby[5].Accept(new ActionVisitor(a => a.DodajUzytkownika()));

            var visitor = new RaportVisitor();
            foreach (var osoba in osoby)
            {
                osoba.Accept(visitor);
            }
        }
    }
}
