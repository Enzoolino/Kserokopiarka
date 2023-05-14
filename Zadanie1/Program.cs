using ver1;

namespace Zadanie1
{
    class Program
    {
        public static void Main(string[] args)
        {

            //Test funkcjonalności.
            var Ksero = new Copier();
            Ksero.PowerOn();
            IDocument document1 = new TextDocument("jjj.txt");
            Ksero.Print(in document1);

            IDocument document2;
            Ksero.Scan(out document2);

            Ksero.ScanAndPrint();

            System.Console.WriteLine(Ksero.Counter);
            System.Console.WriteLine(Ksero.PrintCounter);
            System.Console.WriteLine(Ksero.ScanCounter);
        }
    }
}
