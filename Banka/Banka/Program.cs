using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banka
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int pom = -1;

            while (pom != 0)
            {
                Console.WriteLine("======================================");
                Console.WriteLine("======|1. Informacije o kreditu|======");
                Console.WriteLine("======|2. Placanje rate kredita|======");
                Console.WriteLine("======|3. Apliciranje za kredit|======");
                Console.WriteLine("======|4. Izvrsi apliciranje za|======");
                Console.WriteLine("======|   sve aplikac.u sistemu|======");
                Console.WriteLine("======================================");
                Console.WriteLine("\nIzaberite jednu od ponudjenih opcija");
                pom = int.Parse(Console.ReadLine());

                //Na osnovu unosa ulazi u odredjeni case
                switch (pom)
                {
                    case 1:
                        Console.WriteLine("=============================");
                        Console.WriteLine("====Informacije o kreditu====");
                        Console.WriteLine("=============================");
                        InformacijaOKreditu();
                        break;
                    case 2:
                        Console.WriteLine("=============================");
                        Console.WriteLine("====Placanje rate kredita====");
                        Console.WriteLine("=============================");
                        PlatiRatu();
                        break;
                    case 3:
                        Console.WriteLine("=============================");
                        Console.WriteLine("====Apliciranje za kredit====");
                        Console.WriteLine("=============================");
                        OdobriKredit();
                        break;
                    case 4:
                        Console.WriteLine("===============================================");
                        Console.WriteLine("====Apliciranje za sve aplikacije u sistemu====");
                        Console.WriteLine("===============================================");
                        AplicirajSveAplikacije();
                        break;
                    default:
                        Console.WriteLine("Unesite broj od 1 do 4");
                        break;
                }
                Console.WriteLine("\nIzlaz: 0");
                Console.WriteLine("Pokreni ponovo aplikaciju: 1");
                pom = int.Parse(Console.ReadLine());

                if (pom == 0)
                {
                    //Ukoliko unese 0 izlazi iz cele aplikacije
                    return;
                }
            }


            Console.ReadLine();
        }

        static void AplicirajSveAplikacije()
        {
            #region Iscitavanje i prikaz kreditnih aplikacija iz CSV-a

            List<string> lines = File.ReadAllLines("KreditneAplikacije.csv").ToList();
              
            List<KreditnaAplikacija> aplikacije = new List<KreditnaAplikacija>();
            Kredit kredit = new Kredit();
            IKredit kr = kredit;


            //U ovom foreach-u iscitavam aplikacije iz csv-a i ubacujem u listu KreditnihAplikacija
            foreach (var line in lines)
            {
                string[] values = line.Split(',');
                KreditnaAplikacija apl = new KreditnaAplikacija() 
                { 
                    IdKredita=int.Parse(values[0]),
                    NazivBanke = values[1],
                    ImeKlijenta=values[2],
                    JmbgKlijenta=values[3], 
                    MesecnaPrimanja=float.Parse(values[4]),
                    RadniStaz=float.Parse(values[5]),
                    IznosKredita=float.Parse(values[6]),
                    BrojMesecnihRata = int.Parse(values[7]) 
                };
                aplikacije.Add(apl);
            }
            Console.WriteLine("==============================");
            Console.WriteLine("====Spisak svih aplikacija====\n\n");
            Console.WriteLine("{0,-3} {1,-13} {2, -10} {3, -16} {4, -10} {5,15} {6, 20} {7,20}","#", "Banka", "Ime", "Jmbg", "Mesecna primanja", "Radni staz", "Iznos kredita", "Broj mesecnih rata");
            Console.WriteLine("--------------------------------------------------------------------------------------------------------------------");
            int i = 1;
            foreach(KreditnaAplikacija x in aplikacije)
            {
                Console.WriteLine("{0,-3} {1,-13} {2, -10} {3, 10} {4, 14} {5,15} {6, 20} {7,20}",i, x.NazivBanke, x.ImeKlijenta, x.JmbgKlijenta, x.MesecnaPrimanja, x.RadniStaz, x.IznosKredita, x.BrojMesecnihRata);
                i++;
            }
            Console.WriteLine("\n\n");

            #endregion Iscitavanje i prikaz kreditnih aplikacija iz CSV-a

            #region Proces apliciranja svih aplikacija

            int m = 1;

            StringBuilder sb = new StringBuilder();

            foreach (KreditnaAplikacija ka in aplikacije)
            {
                //KreditniUslovi, metoda koja proverava da li aplikacija ispunjava uslov za kredit
                Console.WriteLine(m + ". " + KreditniUslovi(ka)+"\n");
                m++;
            }

            #endregion Proces apliciranja svih aplikacija
        }


        static void InformacijaOKreditu()
        {
            #region Provera da li kredit postoji

            Console.WriteLine("Unesite jmbg klijenta:");
            string Jmbg = Console.ReadLine();

            Console.WriteLine("\nUnesite naziv banke:");
            string NazivBanke = Console.ReadLine();

            Kredit kredit = new Kredit();
            IKredit kr = kredit;

            //FindCredits metoda koja vraca listu kredita koji odgovaraju prenetim parametrima
            List<Kredit> krediti = kr.FindCredits(Jmbg, NazivBanke);

            if (krediti.Count == 0)
            {
                Console.WriteLine("Nema podataka o trazenom kreditu");
                return;
            }

            #endregion Provera da li kredit postoji

            #region Ispis informacija o kreditima

            Console.WriteLine("\n\n*****************************");
            Console.WriteLine("** Jmbg klijenta: " + krediti[0].JmbgKlijenta);

            // S obzirom da nam je StatusKredita tipa bool a za ispis ne zelimo true false
            // nego ako je true da ispisuje Aktivan ako je statusKredita==false ispisuje isplacen
            foreach (Kredit credit in krediti)
            {
                string statusKredita;

                if (credit.StatusKredita == true)
                {
                    statusKredita = "Aktivan";
                }
                else
                {
                    statusKredita = "Isplacen";
                }

                Console.WriteLine("\n***************************");
                Console.WriteLine("** Informacije o kreditu **");

                Console.WriteLine("** Naziv banke: " + credit.NazivBanke);
                Console.WriteLine("** Iznos kredita: " + credit.IznosKredita);
                Console.WriteLine("** Iznos rate " + credit.IznosRate);
                Console.WriteLine("** Rate za otplatu: " + credit.RateZaOtplatu);
                Console.WriteLine("** Ukupno rata: " + credit.UkupnoRata);
                Console.WriteLine("** Status kredita: " + statusKredita);
                Console.WriteLine("***************************");
            }

            #endregion Ispis informacija o kreditima
        }

        static void OdobriKredit()
        {

            #region Provera da li aplikacija postoji

            Console.WriteLine("Unesite jmbg klijenta:");
            string Jmbg = Console.ReadLine();

            KreditnaAplikacija apl = new KreditnaAplikacija();
            IKreditnaAplikacija ap = apl;
            
            //FindCreditApps vraca listu kreditnih aplikacija
            //koje odgovaraju prenetom parametru JMBG
            List<KreditnaAplikacija> aplikacije = ap.FindCreditApps(Jmbg);


            if (aplikacije.Count == 0)
            {
                Console.WriteLine("Nema podataka o trazenoj aplikaciji");
                return;
            }

            #endregion Provera da li aplikacija postoji

            #region Ispis podataka o nadjenim aplikacijama

            Console.WriteLine("\n\n** Klijent:              ");
            Console.WriteLine("** Ime: " + aplikacije[0].ImeKlijenta);
            Console.WriteLine("** Jmbg: " + aplikacije[0].JmbgKlijenta);
            Console.WriteLine("** Mesecna primanja: " + aplikacije[0].MesecnaPrimanja);
            Console.WriteLine("** Godine radnog staza: " + aplikacije[0].RadniStaz);

            Console.WriteLine("\n\nAplikacije za kredit klijenta " + aplikacije[0].ImeKlijenta + ":");
            Console.WriteLine("    |Banka|\t|Iznos kredita|\t|Rate za otplatu|\t");
            int brojKredita = 1;

            foreach (KreditnaAplikacija k in aplikacije)
            {
                Console.WriteLine(brojKredita + ".   " + k.NazivBanke + "\t\t" + k.IznosKredita + "\t\t" + k.BrojMesecnihRata);
                brojKredita++;
            }

            #endregion Ispis podataka o nadjenim aplikacijama


            #region Odabir aplikacije
            KreditnaAplikacija izabranaApl = new KreditnaAplikacija();

            // Ukoliko klijent ima vise aplikacija za kredit prikazuju se sve njegove aplikacije i bira se jedna
            if (aplikacije.Count > 1)
            {
                Console.WriteLine("Za koji kredit zelite da izvrsite aplikaciju?(Izaberite redni broj kredita)");

                int redniBrojKredita = int.Parse(Console.ReadLine());

                while (redniBrojKredita > brojKredita - 1)
                {
                    int i = brojKredita - 1;
                    Console.WriteLine("Unesite validan redni broj kredita, od 1 do " + i);

                    redniBrojKredita = int.Parse(Console.ReadLine());
                }
                //Dekrementuje je se redniBrojKredita zbog toga sto nam indeksiranje liste krece od 0
                redniBrojKredita--;

                izabranaApl = aplikacije[redniBrojKredita];
            }
            else
            {
                //Ukoliko je pronadjena samo jedna aplikacija
                izabranaApl = aplikacije[0];
            }

            #endregion Odabir aplikacije

            #region Proces odobravanja kredita

            //KreditniUslovi, metoda koja proverava da li aplikacija ispunjava uslov za kredit
            Console.WriteLine(KreditniUslovi(izabranaApl));

            #endregion Proces odobravanja kredita


        }

        // Metoda koja proverava da li aplikacija ispunjava uslov za kredit
        // Ukoliko je kredit odobren ili odbijem vraca se string sa odgovarajucom porukom
        static string KreditniUslovi(KreditnaAplikacija ka)
        {
            Kredit kredit = new Kredit();
            IKredit kr = kredit;

            float rataKredita = ka.IznosKredita / ka.BrojMesecnihRata;
            List<Kredit> krediti = kr.FindCredits(ka.JmbgKlijenta, ka.NazivBanke);

            switch (ka.NazivBanke)
            {
                case "RBC":
                    bool AktivanKredit = krediti.Any(k => k.StatusKredita == true);
                    if (AktivanKredit == false && rataKredita * 2 <= ka.MesecnaPrimanja)
                    {
                        return "Kredit je odobren";
                    }
                    else
                    {
                        return "Klijent ne ispunjava uslove, kredit je odbijen";
                    }
                    break;
                case "Santander":

                    float target = 70 * ka.MesecnaPrimanja / 100;
                    if (ka.RadniStaz > 5 && rataKredita < target)
                    {
                        return "Kredit je odobren";
                    }
                    else
                    {
                        return "Klijent ne ispunjava uslove, kredit je odbijen";
                    }
                    break;
                case "Wells fargo":
                    if (ka.MesecnaPrimanja > rataKredita)
                    {
                        return "Kredit je odobren";
                    }
                    else
                    {
                        return "Klijent ne ispunjava uslove, kredit je odbijen";
                    }

                    break;
                default:
                    return "Banka ne postoji u sistemu";
                    break;
            }
        }

        //Placanje rate kredita. Proverava se CSV fajl sa kreditima, na osnovu parametra se nalaze krediti koji se nalaze
        // u bazi, ukoliko ima vise kredita korisnik izabira samo jedan kredit za koji se placa rata. Kada se placanje izvrsi
        // azurira se baza i klijentu se smanjuje broj rata za placanje
        static void PlatiRatu()
        {
            #region Provera da li kredit postoji

            Console.WriteLine("Unesite jmbg klijenta:");
            string Jmbg = Console.ReadLine();

            Console.WriteLine("\nUnesite naziv banke:");
            string NazivBanke = Console.ReadLine();

            Kredit k = new Kredit();
            IKredit kr = k;
            
            List<Kredit> trazeniKrediti = kr.FindCredits(Jmbg, NazivBanke);

            if (trazeniKrediti.Count == 0)
            {
                Console.WriteLine("Nema podataka o trazenom klijentu");
                return;
            }

            #endregion Provera da li kredit postoji

            #region Biranje kredita za placanje

            //rataKredit je kredit za koji se placa rata
            Kredit rataKredit = new Kredit();

            // Ukoliko ima vise kredita, ispisuju se svi krediti tog klijenta i vrsi se odabir jednog
            if (trazeniKrediti.Count() > 1)
            {
                Console.Write("\n\nJmbg klijenta " + trazeniKrediti[0].JmbgKlijenta);
                Console.WriteLine("    \n\n    |Naziv banke|   |Iznos kredita|\t|Rate za otplatu|    |Ukupno rata| |Iznos rate|");
                
                int brojKredita = 1;

                foreach (Kredit kredit in trazeniKrediti)
                {
                    Console.WriteLine(brojKredita + ".   " + kredit.NazivBanke + "\t\t" + kredit.IznosKredita + "$\t\t" + kredit.RateZaOtplatu+"\t\t\t"+kredit.UkupnoRata+"\t\t"+kredit.IznosRate+ "$");
                    brojKredita++;
                }
                Console.WriteLine("\nZa koji kredit zelite da platite ratu? (Unesite redni broj kredita)");

                int redniBrojKredita = int.Parse(Console.ReadLine());

                while (redniBrojKredita > brojKredita - 1)
                {
                    int i = brojKredita - 1;
                    Console.WriteLine("Unesite validan redni broj kredita, od 1 do " + i);

                    redniBrojKredita = int.Parse(Console.ReadLine());
                }

                redniBrojKredita--;
                rataKredit = trazeniKrediti[redniBrojKredita];
            }
            else
            {
                rataKredit = trazeniKrediti[0];
            }

            //Za kredite koji su isplaceni ne placa se rata
            if(rataKredit.StatusKredita==false)
            {
                Console.WriteLine("Izabrani kredit je isplacen");
                return;
            }

            #endregion Biranje kredita za placanje


            #region Azuriranje broja rata klijentu
            //Ideja je da iscitam sve podatke iz CVS fajla ubacim u List-u, obradim ih i te obradjene podatke vratim u csv

            List<Kredit> krediti = new List<Kredit>();

            List<string> lines = File.ReadAllLines("Krediti.csv").ToList();

            foreach (var line in lines)
            {
                string[] values = line.Split(',');

                Kredit kredit = new Kredit()
                {
                    IdKredita = int.Parse(values[0]),
                    NazivBanke = values[1],
                    JmbgKlijenta = values[2],
                    IznosKredita = float.Parse(values[3]),
                    IznosRate = float.Parse(values[4]),
                    RateZaOtplatu = float.Parse(values[5]),
                    UkupnoRata = float.Parse(values[6]),
                    StatusKredita = bool.Parse(values[7])
                };
                krediti.Add(kredit);
            }

            //Vracam u CSV obradjene podatke
            List<string> output = new List<string>();

            foreach (Kredit kredit in krediti)
            {
                if (kredit.IdKredita == rataKredit.IdKredita)
                {
                    if (kredit.RateZaOtplatu > 0)
                    {
                        kredit.RateZaOtplatu--;
                        if (kredit.RateZaOtplatu == 0)
                        {
                            kredit.StatusKredita = false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("\n------------------------------------------\n");
                        Console.WriteLine("Otplacene su sve rate kredita");
                    }
                }

                output.Add($"{kredit.IdKredita},{kredit.NazivBanke},{kredit.JmbgKlijenta},{kredit.IznosKredita},{kredit.IznosRate},{kredit.RateZaOtplatu},{kredit.UkupnoRata},{kredit.StatusKredita}");
            }

            Console.WriteLine("\n------------------------------------------\n");

            File.WriteAllLines("Krediti.csv", output);

            //End ** Vracam u CSV obradjene podatke

            Console.WriteLine("Uspesno placena rata za kredit, klijentu je ostalo jos " + --rataKredit.RateZaOtplatu + " rata");

            #endregion Azuriranje broja rata klijentu
        }
    }
}
