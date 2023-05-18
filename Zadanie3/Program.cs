using ver1;

namespace Zadanie3
{
    class Program
    {
        public static void Main(string[] args)
        {
            //Copier
            Copier copier = new Copier();
            copier.PowerOn();

            IDocument doc1 = new PDFDocument("aaa.pdf");

            copier.Print(in doc1);
            copier.Scan(out doc1);
            
            copier.PowerOff();

            //MultiFunctionalDevice
            MultiFunctionalDevice device1 = new MultiFunctionalDevice(111111111);
            MultiFunctionalDevice device2 = new MultiFunctionalDevice(222222222);
            device1.PowerOn();
            device2.PowerOn();

            IDocument doc2 = new TextDocument("bbb.txt");
            IDocument doc3 = new ImageDocument("ccc.jpg");

            device1.Print(in doc2);
            device1.Scan(out doc1, IDocument.FormatType.PDF);

            device1.Send(doc2, 222222222);
            device1.Send(doc3, 222222222);

            device2.Receive();

            device1.PowerOff();
            device2.PowerOff();
        }
    }
}