using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint.Client;
using System.Net;
using System.Security.Cryptography;
using System.IO;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("hello");
            RemoveToken("region=TAP&country=US&lang=en&mpn=PSA72U-0FZ02F&sn=4H012345Z&bpn=G71C000BE510&bsn=B8100379APW&n=1453927594&tok=AECA01B9F6FABE1A5CC1EBC0BA9BA450F555E6A2");
            Test();
            return;
            //string·str·=·"n=1453141209&mpn=PTSE3U-06N00G&sn=7H137673A&bpn=G71C000123E00&bsn=EC0001234ABY&region=TAIS&country=US&lang=en";
            //byte[]·array·=·Encoding.ASCII.GetBytes(str);
            //byte[] array = Convert.FromBase64String("5F6A9B261CA9082DF109DC804A635ABE43046C0029E0180ABFB2B8FE44FCEDE954770E22B7E3BB655DEE67A136B24E5448E775D1A332108638D8492F24A56051");
            //string·input·=·Convert.ToBase64String(Encoding.ASCII.GetBytes("5F6A9B261CA9082DF109DC804A635ABE43046C0029E0180ABFB2B8FE44FCEDE954770E22B7E3BB655DEE67A136B24E5448E775D1A332108638D8492F24A56051"));
            //    Console.Write(input);····
//·Loop·through·contents·of·the·array.···
string output = "";

            /*string key = "5F6A9B261CA9082DF109DC804A635ABE43046C0029E0180ABFB2B8FE44FCEDE954770E22B7E3BB655DEE67A136B24E5448E775D1A332108638D8492F24A56051";
            byte array = Convert.ToByte(key, 8);
            foreach (byte element in array)
            {
                output += String.Format("{0:X}", Convert.ToInt32(element));
            }*/

            string key128 = "5F6A9B261CA9082DF109DC804A635ABE43046C0029E0180ABFB2B8FE44FCEDE954770E22B7E3BB655DEE67A136B24E5448E775D1A332108638D8492F24A56051";
            char[] s = key128.ToCharArray();

            char o;
            string[] key64 = new string[64];
            string key64str = "";
            int index = 0;
            string bytestr = "";
            byte[] bytearr = new byte[64];
            for (int i = 0; i < s.Length;)
            {
                string k = s[i].ToString() + s[i + 1].ToString();
                bytearr[index] = Convert.ToByte(k, 16);
           /*o = Convert.ToChar(Convert.ToUInt32(k, 16));
                byte[] b = BitConverter.GetBytes(o);
                key64.SetValue(o.ToString(), index);
                key64str += o.ToString();
                //byte b = Convert.ToByte((o.ToString());*/
                i += 2;
                index++;
            }
            //byte[] array = Encoding.ASCII.GetBytes(key64str);
            string input = "n=1453141209&mpn=PTSE3U-06N00G&sn=7H137673A&bpn=G71C000123E00&bsn=EC0001234ABY&region=TAIS&country=US&lang=en";
            string hmac = Encode(input, bytearr);
            /*foreach (byte element in hmac)
            {
                output += String.Format("{0:X}", Convert.ToInt32(element));
            }*/

            Console.Write(hmac);

        }

        public static void Test()
        {
            string key128 = "5F6A9B261CA9082DF109DC804A635ABE43046C0029E0180ABFB2B8FE44FCEDE954770E22B7E3BB655DEE67A136B24E5448E775D1A332108638D8492F24A56051";
            char[] s = key128.ToCharArray();


            int index = 0;

            byte[] bytearr = new byte[64];
            for (int i = 0; i < s.Length;)
            {
                string k = s[i].ToString() + s[i + 1].ToString();
                bytearr[index] = Convert.ToByte(k, 16);
                i += 2;
                index++;
            }

            string input = "region=TAP&country=US&lang=en&mpn=PSA72U-0FZ02F&sn=4H012345Z&bpn=G71C000BE510&bsn=B8100379APW&n=1453927594";// "n =1453141209&mpn=PTSE3U-06N00G&sn=7H137673A&bpn=G71C000123E00&bsn=EC0001234ABY&region=TAIS&country=US&lang=en";
            string hmac = Encode(input, bytearr);
   
            Console.Write(hmac);
        }

        public static string Encode(string input, byte[] key)
        {
            HMACSHA1 myhmacsha1 = new HMACSHA1(key);
            byte[] byteArray = Encoding.ASCII.GetBytes(input);
            MemoryStream stream = new MemoryStream(byteArray);
            /*string ret = "";
            byte[] bArr = myhmacsha1.ComputeHash(byteArray);
            foreach (byte b in bArr)
            {
                string str = String.Format("{0:X2}", b);
                if (str.Length == 1)
                    str = "0" + str;
                ret += str;
            }
            return ret;
            */
            return myhmacsha1.ComputeHash(stream).Aggregate("", (s, e) => s + String.Format("{0:X2}", e), s => s);
        }

        private static string RemoveToken(string input)
        {
            string[] tokens = input.Split('&');
            foreach (string token in tokens)
            {
                string[] keyvalue = token.Split('=');
                if (keyvalue[0].Equals("tok"))
                {
                    input = input.Replace(keyvalue[0]+"=", "");
                    input = input.Replace(keyvalue[1], "");
                }
            }
            return input;
        }
    }
}
