using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string no = "E0023";
            string prefix = "E" + string.Format("{0:0000}", (int.Parse(no.Substring(1)) + 1));
            Console.WriteLine(prefix);
            Console.Read();

        }
    }
}
