using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace EFaktura
{
    class Program
    {
        static void Main(string[] args)
        {
            //U svrhu testiranja

            List<string> listOfVersions = EFakturaHelper.GetEFakturaTestData();

            Tuple<List<EFakturaVersion>, LastVersionStatusEnum, List<string>> res = EFakturaHelper.ValidateAndFillEFactura(listOfVersions,false);
            
            if (res != null)
            {                
                List<EFakturaVersion> validEF = res.Item1;
                LastVersionStatusEnum statusEnum = res.Item2;
                List<string> errorMessages = res.Item3;
               
                EFakturaVersion lastVersion = validEF[0];

                for (int i = 0; i < validEF.Count; i++)
                {
                    lastVersion = EFakturaHelper.LastVersion(lastVersion, validEF[i]);
                }

                Console.WriteLine(lastVersion.FullName);
            }
        }
    }
}
