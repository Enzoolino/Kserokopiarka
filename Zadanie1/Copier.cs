using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using ver1;

namespace Zadanie1
{
    public class Copier :BaseDevice, IPrinter, IScanner
    {
        public int PrintCounter { get; private set; }
        public int ScanCounter { get; private set; }
        public new int Counter { get; private set; }

        DateTime now = DateTime.Now;

        public new void PowerOn()
        {
            if (state == IDevice.State.off)
            {
                base.PowerOn();
                Counter++;
            }
        }

        public new void PowerOff()
        {
            if (state == IDevice.State.on)
            {
                base.PowerOff();
            }
        }


        public void Print(in IDocument document)
        {
            if (state == IDevice.State.on && document != null)
            {
                Console.WriteLine($"{now.ToString()} Print: {document.GetFileName()}");
                PrintCounter++;
            }
        }

        public void Scan(out IDocument document, IDocument.FormatType formatType = IDocument.FormatType.JPG)
        {
            document = null;

            if (state == IDevice.State.on)
            {
                if (formatType == IDocument.FormatType.JPG)
                {
                    document = new ImageDocument($"ImageScan{ScanCounter}.jpg");
                    Console.WriteLine($"{now.ToString()} Scan: {document.GetFileName()}");
                    ScanCounter++;
                }
                else if (formatType == IDocument.FormatType.PDF)
                {
                    document = new PDFDocument($"PDFScan{ScanCounter}.pdf");
                    Console.WriteLine($"{now.ToString()} Scan: {document.GetFileName()}");
                    ScanCounter++;
                }
                else if (formatType == IDocument.FormatType.TXT)
                {
                    document = new TextDocument($"TextScan{ScanCounter}.txt");
                    Console.WriteLine($"{now.ToString()} Scan: {document.GetFileName()}");
                    ScanCounter++;
                }
            } 
        }

        public void ScanAndPrint()
        {
            if (state == IDevice.State.on)
            {
                IDocument document;
                this.Scan(out document, IDocument.FormatType.JPG);
                this.Print(document);
            }
        }
    }
}
