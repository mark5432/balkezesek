using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;


namespace balkezesek
{
    class Jatekos
    {
        public string Nev { get; set; }
        public DateTime ElsoMeccs { get; set; }
        public DateTime UtolsoMeccs { get; set; }
        public int Suly { get; set; }
        public int MagassagInch { get; set; }
    }
    
    class JatekosInfo
    {
        public string Nev { get; set; }
        public double MagassagCm { get; set; }
    }


    class Program
    {
        static void Main()
        {
            List<Jatekos> baseballPlayers = AdatokatBeolvas("balkezesek.csv");

            if (baseballPlayers.Count == 0)
            {
                Console.WriteLine("Nincsenek adatok a CSV fájlban.");
                return;
            }

            Console.WriteLine("1. feladat:");
            Console.WriteLine($"Az adatsorok száma: {baseballPlayers.Count}");

            Console.WriteLine("2. feladat:");
            List<JatekosInfo> oktober1999Jatekosok = GetOktober1999Jatekosok(baseballPlayers);

            if (oktober1999Jatekosok.Count == 0)
            {
                Console.WriteLine("Nincsenek olyan játékosok, akik utoljára 1999 októberében játszottak.");
            }
            else
            {
                Console.WriteLine("Olyan játékosok, akik utoljára 1999 októberében játszottak:");
                foreach (var jatekos in oktober1999Jatekosok)
                {
                    Console.WriteLine($"Név: {jatekos.Nev}, Magasság (cm): {jatekos.MagassagCm:F1}");
                }
            }

            Console.WriteLine("3. feladat:");
            int ev;
            do
            {
                Console.Write("Kérlek, adj meg egy évet (1990 - 1999 között): ");
            } while (!int.TryParse(Console.ReadLine(), out ev) || ev < 1990 || ev > 1999);

            List<string> evbenJatszoJatekosok = GetJatekosokAdottEvben(baseballPlayers, ev);

            if (evbenJatszoJatekosok.Count == 0)
            {
                Console.WriteLine($"Nincsenek olyan játékosok, akik játszottak {ev}-ben.");
            }
            else
            {
                Console.WriteLine($"Olyan játékosok, akik játszottak {ev}-ben:");
                foreach (var jatekosNev in evbenJatszoJatekosok)
                {
                    Console.WriteLine(jatekosNev);
                }
            }

            Console.ReadLine();  
        }

        static List<Jatekos> AdatokatBeolvas(string fajlNev)
        {
            List<Jatekos> jatekosok = new List<Jatekos>();

            try
            {
                string[] sorok = File.ReadAllLines(fajlNev).Skip(1).ToArray(); 

                foreach (string sor in sorok)
                {
                    string[] darabok = sor.Split(',');
                    if (darabok.Length == 5)
                    {
                        Jatekos jatekos = new Jatekos
                        {
                            Nev = darabok[0].Trim(),
                            ElsoMeccs = DateTime.ParseExact(darabok[1].Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture),
                            UtolsoMeccs = DateTime.ParseExact(darabok[2].Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture),
                            Suly = int.Parse(darabok[3].Trim()),
                            MagassagInch = int.Parse(darabok[4].Trim())
                        };
                        jatekosok.Add(jatekos);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hiba a CSV fájl beolvasásakor: {ex.Message}");
            }

            return jatekosok;
        }

        static List<JatekosInfo> GetOktober1999Jatekosok(List<Jatekos> jatekosok)
        {
            List<JatekosInfo> oktober1999Jatekosok = jatekosok
                .Where(jatekos => jatekos.UtolsoMeccs.Year == 1999 && jatekos.UtolsoMeccs.Month == 10)
                .Select(jatekos => new JatekosInfo
                {
                    Nev = jatekos.Nev,
                    MagassagCm = jatekos.MagassagInch * 2.54
                })
                .ToList();

            return oktober1999Jatekosok;
        }

        static List<string> GetJatekosokAdottEvben(List<Jatekos> jatekosok, int ev)
        {
            List<string> jatekosokEvben = jatekosok
                .Where(jatekos => jatekos.ElsoMeccs.Year <= ev && jatekos.UtolsoMeccs.Year >= ev)
                .Select(jatekos => jatekos.Nev)
                .ToList();

            return jatekosokEvben;
        }

    }

}