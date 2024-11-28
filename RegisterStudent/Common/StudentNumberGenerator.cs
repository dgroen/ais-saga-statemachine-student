﻿namespace RegisterStudent.Common
{
    public static class StudentNumberGenerator
    {
        public static string Generate()
        {
            var random = new Random();
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string randomString = new string(Enumerable.Repeat(chars, 10)
                                          .Select(s => s[random.Next(s.Length)]).ToArray());
            return randomString;
        }
    }
}
