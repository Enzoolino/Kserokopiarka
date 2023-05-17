using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ver2;

namespace Zadanie5
{
    public class Printer : IPrinter
    {
        public IDevice.State state = IDevice.State.off;
        public IDevice.State GetState() => state;

        public void SetState(IDevice.State state)
        {
            this.state = state;
        }

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
            if (state == IDevice.State.on)
            {
                Console.WriteLine($"{now.ToString()} Print: {document.GetFileName()}");
                PrintCounter++;

                if (PrintCounter % 3 == 0 && PrintCounter != 0)
                {
                    Console.WriteLine("Enabling standby mode for Printer. It will be ready again after a few seconds ...");
                    state = IDevice.State.standby;
                }
            }
        }


    }
}
