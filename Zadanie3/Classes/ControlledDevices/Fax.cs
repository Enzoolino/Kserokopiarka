using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ver1;

namespace Zadanie3
{
    public class Fax : IFax
    {
        protected IDevice.State state = IDevice.State.off;
        public IDevice.State GetState() => state;

        //Current local Time
        DateTime now = DateTime.Now;

        //All received documents and Accesible faxes
        public static Dictionary<BigInteger, List<IDocument>> receivedFaxDocumentsByNumber = new Dictionary<BigInteger, List<IDocument>>();

        //Counters - Fax
        public int FaxSendCounter { get; private set; }
        public int FaxReceivedCounter { get; private set; }
        public int Counter { get; private set; }

        //Number of this specific fax device
        public BigInteger FaxNumber { get; private set; }

        //This fax received documents
        public List<IDocument> receivedDocuments;

        //Connected printer
        public Printer printer;


        public Fax(BigInteger number, Printer connectedPrinter)
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

            printer = connectedPrinter;
        }


        public void PowerOn()
        {
            if (state == IDevice.State.off)
            {
                state = IDevice.State.on;
                Console.WriteLine("Fax is on ...");
                Counter++;
            }
        }

        public void PowerOff()
        {
            if (state == IDevice.State.on)
            {
                state = IDevice.State.off;
                Console.WriteLine("... Fax is off !");
            }
        }

        public void Send(in IDocument document, BigInteger number)
        {
            if (state == IDevice.State.on)
            {
                if (number == FaxNumber)
                {
                    Console.WriteLine("Fax sending failed." + " You can't send Fax to the same device you are sending it from!");
                }
                else if (!receivedFaxDocumentsByNumber.ContainsKey(number))
                {
                    Console.WriteLine("Fax sending failed." + " The specified device does not exist!");
                }
                else if (document != null)
                {
                    Console.WriteLine($"{now.ToString()} Fax: {document.GetFileName()} Sent to: {number}");
                    FaxSendCounter++;

                    receivedFaxDocumentsByNumber[number].Add(document);

                }
            }
        }

        public void Receive()
        {
            if (state == IDevice.State.on)
            {
                receivedDocuments = receivedFaxDocumentsByNumber[FaxNumber];

                if (receivedDocuments.Count == 0)
                {
                    Console.WriteLine("No new faxes received.");
                }
                else
                {
                    Console.WriteLine($"{now.ToString()} Fax: {receivedDocuments.Count} new faxes received:");

                    printer.PowerOn();
                    foreach (var document in receivedDocuments)
                    {
                        printer.Print(in document);
                    }
                    printer.PowerOff();
                    FaxReceivedCounter += receivedDocuments.Count;
                    receivedDocuments.Clear();
                }
            }
        }

        public void ClearFaxBook()
        {
            receivedFaxDocumentsByNumber.Clear();
        }

    }
}
