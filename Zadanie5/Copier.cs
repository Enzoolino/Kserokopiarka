using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using ver2;

namespace Zadanie5
{
    public class Copier 
    {
        protected IDevice.State statePrinter => printer.GetState();
        protected IDevice.State stateScanner => scanner.GetState();
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
        }

        private Printer printer;
        private Scanner scanner;

        public Copier()
        {
            printer = new Printer();
            scanner = new Scanner();
        }

        public IDevice.State GetState() => stateCopier;

        //Counters
        public int PrintCounter { get => printer.PrintCounter; }
        public int ScanCounter { get => scanner.ScanCounter; }
        public  int Counter { get; private set; }

        DateTime now = DateTime.Now;

        public void PowerOn()
        {
            if (stateCopier == IDevice.State.off)
            {
                printer.SetState(IDevice.State.on);
                scanner.SetState(IDevice.State.on);
                Console.WriteLine("Copier is on ...");
                Counter++;
            }
        }

        public void PowerOff()
        {
            if (stateCopier == IDevice.State.on || stateCopier == IDevice.State.standby)
            {
                printer.SetState(IDevice.State.off);
                scanner.SetState(IDevice.State.off);
                Console.WriteLine("... Copier is off");
            }
        }

        public void StandbyOn()
        {
            if (stateCopier == IDevice.State.on)
            {
                printer.SetState(IDevice.State.standby);
                scanner.SetState(IDevice.State.standby);
                Console.WriteLine("Copier in standby mode!");
            }
        }

        public void StandbyOff()
        {
            if (stateCopier == IDevice.State.standby)
            {
                printer.SetState(IDevice.State.on);
                scanner.SetState(IDevice.State.on);
                Console.WriteLine("Copier out of standby mode!");
            }
        }


        public void Print(in IDocument document)
        {
            ConsoleColor color = ConsoleColor.Blue;
            if ((statePrinter == IDevice.State.on || statePrinter == IDevice.State.standby) && document != null)
            {
                Console.ForegroundColor = color;
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

                printer.SetState(IDevice.State.on);
                scanner.SetState(IDevice.State.standby);

                printer.Print(in document);
            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        public void Scan(out IDocument document, IDocument.FormatType formatType = IDocument.FormatType.JPG)
        {
            document = null;
            ConsoleColor color = ConsoleColor.Red;

            if (stateScanner == IDevice.State.on || stateScanner == IDevice.State.standby)
            {
                Console.ForegroundColor = color;
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

                scanner.SetState(IDevice.State.on);
                printer.SetState(IDevice.State.standby);

                scanner.Scan(out document, formatType);
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



