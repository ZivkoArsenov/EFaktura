using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EFaktura
{
    public class EFakturaHelper
    {
        /// <summary>
        /// Method that validates data and prepares a list of valid objects for further processing.
        /// </summary>
        /// <param name="eFakture"></param>
        /// <param name="onlyStable"></param>
        /// <returns></returns>
        public static Tuple<List<EFakturaVersion>, LastVersionStatusEnum, List<string>> ValidateAndFillEFactura(List<string> eFakture, bool onlyStable)
        {
            List<EFakturaVersion> eFacture = new List<EFakturaVersion>();
            LastVersionStatusEnum status = LastVersionStatusEnum.success;
            List<string> errorList = new List<string>();
            string errorMessage = string.Empty;

            foreach (string item in eFakture)
            {
                try
                {
                    Regex validationRegex = new Regex(@"^[^@\s]+[_]+[^@\s]+[_]+[^@\s]+$");
                    bool isValid = validationRegex.IsMatch(item);
                    if (!isValid) { status = LastVersionStatusEnum.attention; errorMessage = $"Naziv verzije {item} nije u zahtevanom formatu!"; errorList.Add(errorMessage); continue; }
                    else
                    {
                        string firstString = item.Substring(0, item.IndexOf("_"));
                        if (string.IsNullOrEmpty(firstString)) { status = LastVersionStatusEnum.attention; errorMessage = $"Naziv verzije {item} nije u zahtevanom formatu!"; errorList.Add(errorMessage); continue; }
                       
                        if (firstString != LastVersionEnum.EFaktura.ToString()) { status = LastVersionStatusEnum.attention; errorMessage = $"Naziv verzije {item} nije u zahtevanom formatu!"; errorList.Add(errorMessage); continue; }                       
                        else
                        {
                            string a2 = item.Substring(item.IndexOf("_") + 1);
                            if (string.IsNullOrEmpty(a2)) { status = LastVersionStatusEnum.attention; errorMessage = $"Naziv verzije {item} nije u zahtevanom formatu!"; errorList.Add(errorMessage); continue; }
                            
                            string num = a2.Substring(0, a2.IndexOf("_"));
                            if (string.IsNullOrEmpty(num)) { status = LastVersionStatusEnum.attention; errorMessage = $"Naziv verzije {item} nije u zahtevanom formatu!"; errorList.Add(errorMessage); continue; }
                           
                            string secondString = a2.Substring(a2.IndexOf("_") + 1);
                            if (secondString != LastVersionEnum.stabile.ToString() && secondString != LastVersionEnum.beta.ToString())
                            { status = LastVersionStatusEnum.attention; errorMessage = $"Naziv verzije {item} nije u zahtevanom formatu!"; errorList.Add(errorMessage); continue; }


                            bool numIsFirstDigit = Char.IsNumber(num, 0);
                            if (!numIsFirstDigit) { status = LastVersionStatusEnum.attention; errorMessage = $"Naziv verzije {item} nije u zahtevanom formatu!"; errorList.Add(errorMessage); continue; }
                            
                            EFakturaVersion efacturaVersion = new EFakturaVersion
                            {
                                FullName = item,
                                Name = firstString,
                                VersionNum = num,
                                IsBeta = secondString == LastVersionEnum.beta.ToString() ? true : false
                            };
                            eFacture.Add(efacturaVersion);
                        }
                    }
                }
                catch (Exception ex)
                {
                    status = LastVersionStatusEnum.error;
                    errorMessage = "Naziv nije u zahtevanom formatu!";
                    errorList.Add(errorMessage);
                    Tuple<List<EFakturaVersion>, LastVersionStatusEnum, List<string>> resError = Tuple.Create(eFacture, status, errorList);
                    return resError;
                }
            }
            eFacture = onlyStable ? eFacture.Where(x => !x.IsBeta).ToList() : eFacture;
            Tuple<List<EFakturaVersion>, LastVersionStatusEnum, List<string>> res = Tuple.Create(eFacture, status, errorList);
            return res;
        }
        /// <summary>
        /// Method that returns the latest version.
        /// </summary>
        /// <param name="efactura1"></param>
        /// <param name="efaktura2"></param>
        /// <returns></returns>
        public static EFakturaVersion LastVersion(EFakturaVersion efactura1, EFakturaVersion efaktura2)
        {
            if (efactura1 != null && efaktura2 != null)
            {
                try
                {
                    
                    string v1 = efactura1.VersionNum;
                    string v2 = efaktura2.VersionNum;

                    Version? version1 = new Version();
                    Version? version2 = new Version();

                    bool isSucces1 =  Version.TryParse(v1, out version1);
                    bool isSucces2 = Version.TryParse(v2, out version2);

                    if (isSucces1 && isSucces2)
                    {
                        var result = version1.CompareTo(version2);
                        if (result > 0)
                            return efactura1;
                        else if (result < 0)
                            return efaktura2;
                        else
                            return efaktura2;
                    }
                   
                }
                catch (Exception ex)
                {
                    //Moze da se desi da je negde u verziji umesto broja slovo.
                    //ne zelim da se u tom slucaju prekine poredjene i izvrsavanje
                    //koda, vec da poslednja verzija ostane i da se poredi sa sledecom.
                    //To je vec sa TryParse pokriveno, ali ne zelim ex.
                    //throw ex;
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        /// Method that returns data for the test.
        /// </summary>
        /// <returns></returns>
        public static List<string> GetEFakturaTestData()
        {
            List<string> listOfVersions = new List<string>();

            string a = "EFaktura_10.00.00.01_stabile";
            string b = "EFaktura_10.00.22.01_beta";
            string c = "EFaktura_10.10.00.02_stabile";
            string d = "EFaktura_10.00.20.00_stabile";
            string e = "EFaktura_10.00.00.04_stabile";
            string f = "EFaktura_10.00.00.10_stabile";
            string g = "EFaktura_10.00.00.03_stabile";
            string h = "EFaktura_10.00.22.08_beta";
            string j = "EFaktura_10.00.00.10_stabile";
            string k = "EFaktura_12.00.00.00_stabile";
            string l = "EFaktura_10.00.00.114_beta";
            string z = "EFaktura_10.00.20.10_stabile";

            listOfVersions.Add(a);
            listOfVersions.Add(b);
            listOfVersions.Add(c);
            listOfVersions.Add(d);
            listOfVersions.Add(f);
            listOfVersions.Add(e);
            listOfVersions.Add(g);
            listOfVersions.Add(h);
            listOfVersions.Add(j);
            listOfVersions.Add(k);
            listOfVersions.Add(l);
            listOfVersions.Add(z);

            return listOfVersions;
        }
    }
}
