using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoaDonDienTu
{
    public class Cls_EnCrypting
    {
        private byte[] a;

        public Cls_EnCrypting()
        {
            a = Encoding.Unicode.GetBytes("#effect@dotnet.$");
        }

        public Cls_EnCrypting(string str)
        {
            a = Encoding.Unicode.GetBytes(str);
        }

        public string EnCrypt(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }
            byte[] bytes = Encoding.Unicode.GetBytes(str);
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] ^= a[i % a.Length];
            }
            return Encoding.Unicode.GetString(bytes);
        }

        public byte[] EnCryptToByte(string str)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(str);
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] ^= a[i % a.Length];
            }
            return bytes;
        }

        public byte[] EnCryptToByte(byte[] str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                str[i] ^= a[i % a.Length];
            }
            return str;
        }

        public string EnCryptToString(byte[] st)
        {
            for (int i = 0; i < st.Length; i++)
            {
                st[i] ^= a[i % a.Length];
            }
            return Encoding.Unicode.GetString(st);
        }
    }
}
