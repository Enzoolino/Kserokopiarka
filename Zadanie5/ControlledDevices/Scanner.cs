﻿using System;
using System.Collections.Generic;
using ver2;

namespace Zadanie5
{
    public class Scanner  : IScanner
    {
        protected IDevice.State state = IDevice.State.off;
        public IDevice.State GetState() => state;

        public void SetState(IDevice.State state)
        {
            this.state = state;
        }

        public int ScanCounter { get; private set; }
        public int Counter { get; private set; }

        DateTime now = DateTime.Now;

        public void PowerOn()
        {
            if (state == IDevice.State.off)
            {
                state = IDevice.State.on;
                Console.WriteLine("Scanner is on ...");
                Counter++;
            }
        }

        public void PowerOff()
        {
            if (state == IDevice.State.on)
            {
                state = IDevice.State.off;
                Console.WriteLine("... Scanner is off !");
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

                if (ScanCounter % 2 == 0 && ScanCounter != 0)
                {
                    Console.WriteLine("Enabling standby mode for Scanner. It will be ready again after a few seconds ...");
                    state = IDevice.State.standby;
                }
            }
        }

    }
}
