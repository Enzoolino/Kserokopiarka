using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ver1;
using Zadanie1;

namespace Zadanie2
{
    public class MultiFunctionalDevice : BaseDevice, IScanner, IPrinter, IFax
    {
        //Current local Time
        DateTime now = DateTime.Now;

        //All received documents and Accesible faxes
        public static Dictionary<BigInteger, List<IDocument>> receivedFaxDocumentsByNumber = new Dictionary<BigInteger, List<IDocument>>();

        //Counters - Other
        public int PrintCounter { get; private set; }
        public int ScanCounter { get; private set; }
        public new int Counter { get; private set; }

        //Counters - Fax
        public int FaxSendCounter { get; private set; }
        public int FaxReceivedCounter { get; private set; }

        //Number of this specific fax device
        public BigInteger FaxNumber { get; private set; }

        //Constructor
        public MultiFunctionalDevice(BigInteger number)
        {
            if (number.ToString().Length != 9)
            {
                throw new ArgumentOutOfRangeException(nameof(number));
            }
            else if (receivedFaxDocumentsByNumber.ContainsKey(number))
            {
                throw new DuplicateNameException("Fax with this number already exists!");
            }
            else
            {
                FaxNumber = number;
                receivedFaxDocumentsByNumber.Add(number, new List<IDocument>());
            }      
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
            if (state == IDevice.State.on && document != null)
            {
                Console.ForegroundColor = ConsoleColor.Blue;

                Console.WriteLine($"{now.ToString()} Print: {document.GetFileName()}");
                PrintCounter++;

                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public void Scan(out IDocument document, IDocument.FormatType formatType = IDocument.FormatType.JPG)
        {
            document = null;

            if (state == IDevice.State.on)
            {
                Console.ForegroundColor = ConsoleColor.Red;

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

                Console.ForegroundColor = ConsoleColor.White;
            }
        }


        public void Send(in IDocument document, BigInteger number)
        {
            if (state == IDevice.State.on)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;

                if (number == FaxNumber)
                {
                    Console.WriteLine("Fax sending failed."+" You can't send Fax to the same device you are sending it from!");
                }
                else if (!receivedFaxDocumentsByNumber.ContainsKey(number))
                {
                    Console.WriteLine("Fax sending failed."+" The specified device does not exist!");
                }
                else if (document != null)
                {
                    Console.WriteLine($"{now.ToString()} Fax{$"({FaxNumber})"}: {document.GetFileName()} Sent to: {number}");
                    FaxSendCounter++;

                    receivedFaxDocumentsByNumber[number].Add(document);
                }

                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public void Receive()
        {
            if (state == IDevice.State.on)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;

                List<IDocument> receivedDocuments = receivedFaxDocumentsByNumber[FaxNumber];

                if (receivedDocuments.Count == 0)
                {
                    Console.WriteLine("No new faxes received.");
                }
                else
                {
                    Console.WriteLine($"{now.ToString()} Fax{$"({FaxNumber})"}: {receivedDocuments.Count} new faxes received:");
                    foreach (var document in receivedDocuments)
                    {
                        Print(in document);
                    }
                    FaxReceivedCounter += receivedDocuments.Count;
                    receivedDocuments.Clear();
                }

                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public static void ClearFaxBook()
        {
            receivedFaxDocumentsByNumber.Clear();
        }

    }

    



}
