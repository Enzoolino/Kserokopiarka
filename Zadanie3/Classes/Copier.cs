using System;
using System.Collections.Generic;
using ver1;

namespace Zadanie3
{
    public class Copier : BaseDevice
    {
        //Counters
        public int PrintCounter { get => printer.PrintCounter; }
        public int ScanCounter { get => scanner.ScanCounter; }
        public new int Counter { get; private set; }

        //Current local time
        DateTime now = DateTime.Now;

        //Connected (controlled) devices
        private Printer printer;
        private Scanner scanner;

        public Copier()
        {
            printer = new Printer();
            scanner = new Scanner();
        }

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
            if (state == IDevice.State.on)
            {
                printer.PowerOn();
                printer.Print(in document);
                printer.PowerOff();
            }
        }

        public void Scan(out IDocument document, IDocument.FormatType formatType = IDocument.FormatType.JPG)
        {
            document = null;

            if (state == IDevice.State.on)
            {
                scanner.PowerOn();
                scanner.Scan(out document, formatType);
                scanner.PowerOff();
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
