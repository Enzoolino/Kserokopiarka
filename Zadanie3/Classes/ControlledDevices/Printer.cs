using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ver1;
using static ver1.IDevice;

namespace Zadanie3
{
    public class Printer : IPrinter
    {
        protected IDevice.State state = IDevice.State.off;
        public IDevice.State GetState() => state;

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

        public  void PowerOff()
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
                Console.WriteLine($"{now.ToString()} Print: {document.GetFileName()}");
                PrintCounter++;
            }
        }

    }
}
