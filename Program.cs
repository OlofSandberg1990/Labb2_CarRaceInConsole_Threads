using System;

namespace CarRace_in_Console
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Race race = new Race();
            Console.WriteLine("\t====Välkommen till racebanan====");
            Console.WriteLine("\nKlicka på enter under loppets gång för att få information om loppet");
            Console.ReadKey();

            race.AddCar("Volvo");
            race.AddCar("BMW");
            race.AddCar("SAAB");
            race.AddCar("Tesla");
            race.AddCar("Ferrari");
            race.AddCar("Opel");
            race.AddCar("Lambo");
            Console.WriteLine("\nKlicka på enter för att starta racet.");
            Console.ReadKey();
            Console.WriteLine("\n\tGO GO GO!\n");
            race.StartRace();
        }
    }
}
