using ver2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadanie5
{
    class Program
    {
        public static void Main(string[] args)
        {
            Copier xerox = new Copier();

            xerox.PowerOn();

            IDocument doc1 = new PDFDocument("aaa.pdf");
            IDocument doc2 = new TextDocument("bbb.txt");
            IDocument doc3 = new ImageDocument("ccc.jpg");
            
            //Print test + Switch to standby after 3 Prints
            xerox.Print(in doc1);
            xerox.Print(in doc2);
            xerox.Print(in doc3);

            //Scan test + Switch to standby after 2 Scans
            xerox.Scan(out doc1, IDocument.FormatType.PDF);
            xerox.Scan(out doc2, IDocument.FormatType.TXT);
            xerox.Scan(out doc3, IDocument.FormatType.JPG);
            
            //Scan&Print test + Switch to standby
            xerox.ScanAndPrint();
            xerox.ScanAndPrint();
            xerox.ScanAndPrint();
            
            //Copier off test
            xerox.PowerOff();
            xerox.Print(in doc1);
            xerox.Scan(out doc1, IDocument.FormatType.PDF);
            xerox.ScanAndPrint();
        }
    }
}