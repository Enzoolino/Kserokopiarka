using System;
using System.Collections.Generic;
using ver1;

namespace Zadanie3
{
    public class Printer : IPrinter
    {
        protected IDevice.State state = IDevice.State.off;
        public IDevice.State GetState() => state;

        //Counters
        public int PrintCounter { get; private set; }
        public int Counter { get; private set; }
        
        DateTime now = DateTime.Now;

        public void PowerOn()
        {
            if (state == IDevice.State.off)
            {
                state = IDevice.State.on;
                Console.WriteLine("Printer is on ...");
                Counter++;
            }
        }

        public void PowerOff()
        {
            if (state == IDevice.State.on)
            {
                state = IDevice.State.off;
                Console.WriteLine("... Printer is off !");
            }
        }

        public void Print(in IDocument document)
        {
            if (state == IDevice.State.on && document != null)
            {
                Console.ForegroundColor = ConsoleColor.Blue;

                Console.WriteLine($"{now.ToString()} Print: {document.GetFileName()}");
                PrintCounter++;

                Console.ForegroundColor = ConsoleColor.White;
            }
        }

    }
}
