using System;
using System.Collections.Generic;
using System.Numerics;
using ver1;

namespace Zadanie3
{
    public class MultiFunctionalDevice : BaseDevice
    {
        //Current local Time
        DateTime now = DateTime.Now;

        //Counters - Other
        public int PrintCounter { get => printer.PrintCounter; }
        public int ScanCounter { get => scanner.ScanCounter; }
        public new int Counter { get ; private set; }

        //Counters - Fax
        public int FaxSendCounter { get => fax.FaxSendCounter;  }
        public int FaxReceivedCounter { get => fax.FaxReceivedCounter; }

        //Number of connected (controlled) Fax Device
        public BigInteger FaxNumber { get => fax.FaxNumber; }

        //Connected Devices
        private Printer printer;
        private Scanner scanner;
        private Fax fax;

        //Constructor
        public MultiFunctionalDevice(BigInteger number)
        {
            printer = new Printer();
            scanner = new Scanner();
            fax = new Fax(number, printer);
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

        public void Send(in IDocument document, BigInteger number)
        {
            if (state == IDevice.State.on)
            {
                fax.PowerOn();
                fax.Send(document, number);
                fax.PowerOff();
            }
        }

        public void Receive()
        {
            if (state == IDevice.State.on)
            {
                fax.PowerOn();
                fax.Receive();
                fax.PowerOff();
            }

        }

       
    }
}
