using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JinxiaocunApp.Extensions
{
    public static class StringOpExtensions
    {
        public static string GetAscNumberStr(this string ascStr)
        {
            string returnStr = "";

            bool isAll = true;
            for (int i = ascStr.Length - 1; i >= 0; i--)
            {

                if (char.IsNumber(ascStr[i]))
                {
                    returnStr = ascStr[i] + returnStr;
                }
                else
                {
                    if (string.IsNullOrEmpty(returnStr))
                    {
                        returnStr = ascStr + "1";
                    }
                    else
                    {
                        int strLeng = returnStr.Length;
                        if (strLeng > (Convert.ToInt32(returnStr)+1).ToString().Length)
                        {
                            string zs = "";
                            for (int j = 0; j < (strLeng - (Convert.ToInt32(returnStr)+1).ToString().Length); j++)
                            {
                                zs += "0";
                            }
                            returnStr = ascStr.Substring(0, (ascStr.Length - returnStr.Length)) + zs + (Convert.ToInt32(returnStr) + 1);
                        }
                        else
                        {
                            returnStr = ascStr.Substring(0, (ascStr.Length - returnStr.Length)) + (Convert.ToInt32(returnStr) + 1);
                        }
                    }
                    isAll = false;
                    break;
                }
            }
            if (isAll)
            {
                int strLeng = returnStr.Length;
                if (strLeng > (Convert.ToInt32(returnStr)+1).ToString().Length)
                {
                    string zs = "";
                    for (int j = 0; j < (strLeng - (Convert.ToInt32(returnStr)+1).ToString().Length); j++)
                    {
                        zs += "0";
                    }
                    returnStr = zs + (Convert.ToInt32(returnStr) + 1);
                }
            }
            return returnStr;
        }
    }
}