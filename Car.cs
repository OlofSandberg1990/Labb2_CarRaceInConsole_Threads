using System;
using System.Diagnostics;
using System.Threading;

namespace CarRace_in_Console
{
    internal class Car
    {
        public string Name { get; set; }
        public double Speed { get; set; }
        public double Distance { get; set; }
        public bool HasFinished { get; set; }

        public static object RaceLock { get; set; } = new object();

        public Car(string name)
        {
            Name = name;
            Speed = 120;
            Distance = 0;
            HasFinished = false;
        }

        public void Drive()
        {
            //Startar en tidtagare för att hålla reda på tid för händelser.
            Stopwatch stopwatch = Stopwatch.StartNew();
            Console.WriteLine($"{Name} STARTAR!!!");

            //Loopar så länge bilen inte gått i mål
            while (!HasFinished)
            {
                //Simulerar en sekunds fördröjning
                Thread.Sleep(1000);

                //Uppdaterar bilens distans och konverterar från km/h till m/s.
                Distance += Speed / 3600 * 1000;

                // Utlöser slumpmässiga händelser var 30:e sekund
                if (stopwatch.Elapsed.TotalSeconds >= 30)
                {
                    HandleRandomEvent();

                    //Återställer tidtagaren efter en händelse
                    stopwatch.Restart();
                }

                // Kontrollerar om bilen har nått mållinjen
                if (Distance >= 10000)
                {
                    //RaceLock för att säkra att endast en tråd kör detta block i taget.
                    lock (RaceLock)
                    {
                        if (!HasFinished)
                        {
                            HasFinished = true;
                            if (Race.finishedCars == 0)
                            {
                                Race.AnnounceWinner(this);

                            } else
                            {
                                Console.WriteLine($"{Name} gick i mål!");

                            }
                        }
                    }
                }
            }
        }

        private void HandleRandomEvent()
        {
            Random rand = new Random();

            //Slumpar ett tal mellan 1-50.
            int eventChance = rand.Next(1, 51);

            //Varje if-sats representerar sannolikheten för en specifik händelse
            //Och låser tråden ett visst antal sekunder innan bilen kan göra vidare
            if (eventChance == 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{Name} stannar för att tanka. Detta tar 30 sekunder....");
                Console.ForegroundColor = ConsoleColor.White;
                Thread.Sleep(30000);
                Console.WriteLine($"{Name} har full tank och är nu igång igen!");
            } else if (eventChance <= 3)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{Name} fick en punktering och byter däck. Bilen står stilla i 20 sekunder....");
                Console.ForegroundColor = ConsoleColor.White;
                Thread.Sleep(20000);
                Console.WriteLine($"{Name} har bytt däck och är igång igen!");
            } else if (eventChance <= 8)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{Name} tvättar vindrutan efter en fågelträff. Bilen står stilla i 10 sekunder....");
                Console.ForegroundColor = ConsoleColor.White;
                Thread.Sleep(10000);
                Console.WriteLine($"{Name} är igång igen med en ren och fin ruta!");
            } else if (eventChance <= 18)
            {
                Speed -= 1;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{Name} fick motorproblem, hastigheten sänks till {Speed} km/h.");
                Console.ForegroundColor = ConsoleColor.White;
            }

        }
    }
}
