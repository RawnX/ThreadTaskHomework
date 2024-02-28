﻿using System;
using System.Security.Cryptography;
using System.Text;


namespace ThreadTaskHomework.Helper
{
    public class EncrytionHelper
    {
        public static string sha256(string randomString)
        {
            var crypt = new SHA256Managed();
            string hash = String.Empty;
            byte[] crypto = crypt.ComputeHash(Encoding.ASCII.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash += theByte.ToString("x2");
            }
            return hash;
        }
    }
}
