using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Banka
{
    interface IKreditnaAplikacija
    {
        List<KreditnaAplikacija> FindCreditApps(string Jmbg);
    }
}
