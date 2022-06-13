using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banka
{
    interface IKredit
    {
        List<Kredit> FindCredits(string Jmbg, string NazivBanke);
    }
}
