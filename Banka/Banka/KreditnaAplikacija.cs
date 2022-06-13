using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banka
{
    public class KreditnaAplikacija:IKreditnaAplikacija
    {
        // Dodao sam polje IdKredita jer kada se placa rata za kredit
        // moze se desiti da je isti klijent podigao dva kredita u istoj banci
        // i da se igrom slucaja pogodi da je iznos kredita i broj rata isti

        // (Nisam toliko upucen u bankarske sisteme pa nisam siguran da li je to moguce
        // kroz zadatak vidim da samo RBC ne izdaje kredit ako prethodni nije otplacen
        // sto znaci da ce druge banke odobriti kredit ukoliko klijent ispunjava ostale uslove)

        // da bi mogao da razlikujem te dve aplikacije kredita dodao sam polje IdKredita

        public int IdKredita { get; set; }
        public string NazivBanke { get; set; }
        public string ImeKlijenta { get; set; }
        public string JmbgKlijenta { get; set; }
        public float MesecnaPrimanja { get; set; }
        public float RadniStaz { get; set; }
        public float IznosKredita { get; set; }
        public int BrojMesecnihRata { get; set; }

        public List<KreditnaAplikacija> FindCreditApps(string Jmbg)
        {

            List<KreditnaAplikacija> nadjeneApl = new List<KreditnaAplikacija>();
            KreditnaAplikacija apl = new KreditnaAplikacija();

            var lines = File.ReadAllLines("KreditneAplikacije.csv");

            
            foreach (var line in lines)
            {
                var values = line.Split(',');

                if (values[3] == Jmbg)
                {
                    apl = new KreditnaAplikacija()
                    {
                        IdKredita=int.Parse(values[0]),
                        NazivBanke = values[1],
                        ImeKlijenta = values[2],
                        JmbgKlijenta = values[3],
                        MesecnaPrimanja = float.Parse(values[4]),
                        RadniStaz = float.Parse(values[5]),
                        IznosKredita = float.Parse(values[6]),
                        BrojMesecnihRata = int.Parse(values[7])
                    };
                    nadjeneApl.Add(apl);
                }
                
            }
            return nadjeneApl;
            
        }
    }
}
