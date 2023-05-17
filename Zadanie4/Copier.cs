using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using ver2;

namespace Zadanie4
{
    public class Copier :  IPrinter, IScanner
    {
        protected IDevice.State statePrinter = IDevice.State.off;
        protected IDevice.State stateScanner = IDevice.State.off;
        protected IDevice.State stateCopier
        {
            get
            {
                if (statePrinter == IDevice.State.off && stateScanner == IDevice.State.off)
                    return IDevice.State.off;
                else if (statePrinter == IDevice.State.standby && stateScanner == IDevice.State.standby)
                    return IDevice.State.standby;
                else
                    return IDevice.State.on;
            }
            private set
            {
                    statePrinter = value;
                    stateScanner = value;           
            }
        }
        
        public IDevice.State GetState() => stateCopier;

        public void SetState(IDevice.State state)
        {
            this.stateCopier = state;
        }

        //Counters
        public int PrintCounter { get; private set; }
        public int ScanCounter { get; private set; }
        public  int Counter { get; private set; }

        DateTime now = DateTime.Now;

        public void PowerOn()
        {
            if (stateCopier == IDevice.State.off)
            {
                statePrinter = IDevice.State.on;
                stateScanner = IDevice.State.on;
                Console.WriteLine("Copier is on ...");
                Counter++;
            }
        }

        public void PowerOff()
        {
            if (stateCopier == IDevice.State.on || stateCopier == IDevice.State.standby)
            {
                statePrinter = IDevice.State.off;
                stateScanner = IDevice.State.off;
                Console.WriteLine("... Copier is off");
            }
        }

        public void StandbyOn()
        {
            statePrinter = IDevice.State.standby;
            stateScanner = IDevice.State.standby;
            Console.WriteLine("Copier in standby mode!");
        }

        public void StandbyOff()
        {
            statePrinter = IDevice.State.on;
            stateScanner = IDevice.State.on;
            Console.WriteLine("Copier out of standby mode!");
        }


        public void Print(in IDocument document)
        {
            if ((statePrinter == IDevice.State.on || statePrinter == IDevice.State.standby) && document != null)
            {
                Console.ForegroundColor = ConsoleColor.Blue;

                if ((statePrinter == IDevice.State.standby && stateScanner == IDevice.State.on) || PrintCounter + ScanCounter == 0)
                {
                    System.Threading.Thread.Sleep(2000);
                    Console.WriteLine("Enabling printer and switching scanner to standby mode!");
                    System.Threading.Thread.Sleep(2000);
                }
                else if (statePrinter == IDevice.State.standby && stateScanner == IDevice.State.standby)
                {
                    System.Threading.Thread.Sleep(2000);
                    Console.WriteLine("Enabling printer ...");
                    System.Threading.Thread.Sleep(2000);
                }

                statePrinter = IDevice.State.on;
                stateScanner = IDevice.State.standby;

                Console.WriteLine($"{now.ToString()} Print: {document.GetFileName()}");
                PrintCounter++;

                if (PrintCounter % 3 == 0 && PrintCounter != 0)
                {
                    Console.WriteLine("Enabling standby mode for Printer. It will be ready again after a few seconds ...");
                    statePrinter = IDevice.State.standby;
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void Scan(out IDocument document, IDocument.FormatType formatType = IDocument.FormatType.JPG)
        {
            document = null;
            if (stateScanner == IDevice.State.on || stateScanner == IDevice.State.standby)
            {
                Console.ForegroundColor = ConsoleColor.Red;

                if ((stateScanner == IDevice.State.standby && statePrinter == IDevice.State.on) || ScanCounter + PrintCounter == 0)
                {
                    System.Threading.Thread.Sleep(2000);
                    Console.WriteLine("Enabling scanner and switching printer to standby mode!");
                    System.Threading.Thread.Sleep(2000);
                }
                else if (stateScanner == IDevice.State.standby && statePrinter == IDevice.State.standby)
                {
                    System.Threading.Thread.Sleep(2000);
                    Console.WriteLine("Enabling scanner ...");
                    System.Threading.Thread.Sleep(2000);
                }

                stateScanner = IDevice.State.on;
                statePrinter = IDevice.State.standby;
                
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

                if (ScanCounter % 2 == 0 && ScanCounter != 0)
                {
                    Console.WriteLine("Enabling standby mode for Scanner. It will be ready again after a few seconds ...");
                    stateScanner = IDevice.State.standby;
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void ScanAndPrint()
        {
            if (stateCopier == IDevice.State.on || stateCopier == IDevice.State.standby)
            {
                IDocument document;
                this.Scan(out document, IDocument.FormatType.JPG);
                this.Print(document);
            }
        }
    }
}



