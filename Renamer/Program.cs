
using System;
using System.Collections.Generic;
using System.Text;


namespace Renamer
{


    class Program
    {


        static void Main(string[] args)
        {
            MethodTests.FileTextReplacer.Test();

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(" --- Press any key to continue --- ");
            Console.ReadKey();
        }


    }


}
