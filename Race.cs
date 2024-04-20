using System;
using System.Collections.Generic;
using System.Threading;

namespace CarRace_in_Console
{
    internal class Race
    {
        private List<Car> cars = new List<Car>();

        //Variabel för att hålla koll på vilken bil som avslutade racet först
        private static Car firstFinisher;
        private static bool raceOver = false;
        public static int finishedCars = 0;

        public void AddCar(string name)
        {
            cars.Add(new Car(name));
            Console.WriteLine($"{name} gör sig redo för start...");
            Thread.Sleep(500);
        }

        public void StartRace()
        {
            //Lista med trådar där varje bil kör i en egen tråd
            List<Thread> carThreads = new List<Thread>();

            //Skapar en ny tråd för varje bil och exekverar DriveMetoden för varje bil
            foreach (Car car in cars)
            {
                Thread thread = new Thread(car.Drive);
                carThreads.Add(thread);
                thread.Start();
            }

            //En ny tråd för att visa statusuppdateringar under racets gång
            Thread statusThread = new Thread(DisplayStatus);
            statusThread.Start();

            //Väntar på att alla bilar ska avsluta loppet
            foreach (Thread thread in carThreads)
            {
                thread.Join();
            }

            //Sätter boolen raceOver till true när alla bilar kört i mål
            //och kör metoden DisplayFinalResults
            raceOver = true;
            Console.WriteLine($"\nRacet är slut. Grattis till {firstFinisher.Name} som VINNER!!");
            Console.ReadKey();

        }

        public static void AnnounceWinner(Car car)
        {
            //Låser med RaceLock
            lock (Car.RaceLock)
            {
                //Kollar så ingen vinnare har registrerats.
                if (firstFinisher == null)
                {
                    firstFinisher = car;
                    finishedCars++;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"\n{car.Name} är först över mållinjen och vinner!");
                    Console.ForegroundColor = ConsoleColor.White;

                }
            }
            
        }

        private void DisplayStatus()
        {

            while (!raceOver)
            {
                //Lyssnar efter att enter trycks ner.
                if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                {
                    Console.Clear();
                    Console.ForegroundColor= ConsoleColor.Blue;
                    Console.WriteLine("\t\tStatus för racet");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("----------------------------------------------------------");

                    //Sorterar bilarna efter hur långt dem kört för att hålla koll på position

                    var sortedCars = cars.OrderByDescending(c => c.Distance).ToList();
                    for (int i = 0; i < sortedCars.Count; i++)
                    {
                        Car car = sortedCars[i];
                        Console.ForegroundColor= ConsoleColor.Yellow;
                        Console.WriteLine($"Pos. {i + 1} {car.Name}\tSträcka : {car.Distance:0} meter\tHastighet: {car.Speed} km/h");
                    }
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("----------------------------------------------------------");
                }
            }
        }
      
    }
}
