using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFaktura
{
    public class EFakturaVersion
    {
        public string FullName { get; set; }
        public string Name { get; set; }
        public string VersionNum { get; set; }
        public bool IsBeta { get; set; }
    }
}
