﻿using System.Numerics;
using ver1;

namespace Zadanie2
{
    class Program
    {
        public static void Main(string[] args)
        {
            //Test funkcjonalności
            var fax1 = new MultiFunctionalDevice(111111111);
            var fax2 = new MultiFunctionalDevice(222222222);

            fax1.PowerOn();
            fax2.PowerOn();

            IDocument document = new PDFDocument("sample.pdf");

            fax1.Print(in document);
            fax1.Scan(out document, IDocument.FormatType.TXT);

            fax1.Send(document, 222222222);
            fax2.Receive();
 
        }
    }
}


    
