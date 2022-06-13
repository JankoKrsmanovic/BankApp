using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banka
{
    //Ubacio sam klasu kredit jer je u zadatku pisalo: "Učitati listu kreditnih aplikacija iz csv fajla.."
    //pa onda kaze: "RBC odbija kredit ako klijent ima neki aktivan (neisplaćen) kredit"
    //Dakle u CSV fajlu sa kreditnim aplikacijama nigde nemamo podatak da li je kredita aktivan ili isplacen jer su to aplikacija
    //stoga sam kreirao CSV fajl sa kreditima gde cu pri apliciranju kredita iscitavati taj CSV fajl u cilju da proverim
    //da li klijent sa tim JMBG-om ima aktivan kredit u RBC-u.
    class Kredit :IKredit
    {
        public int IdKredita { get; set; }
        public string NazivBanke { get; set; }
        public string JmbgKlijenta { get; set; }
        public float IznosKredita { get; set; }
        public float IznosRate { get; set; }
        public float RateZaOtplatu { get; set; }
        public float UkupnoRata { get; set; }
        public bool StatusKredita { get; set; }


        public List<Kredit> FindCredits(string Jmbg, string NazivBanke)
        {
            List<Kredit> nadjeniKrediti = new List<Kredit>();
            Kredit kredit = new Kredit();

            var lines = File.ReadAllLines("Krediti.csv");


            foreach (var line in lines)
            {
                var values = line.Split(',');


                if (values[2] == Jmbg && values[1] == NazivBanke)
                {
                    kredit = new Kredit()
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
                    nadjeniKrediti.Add(kredit);
                }

            }
            return nadjeniKrediti;

        }
    }
}
