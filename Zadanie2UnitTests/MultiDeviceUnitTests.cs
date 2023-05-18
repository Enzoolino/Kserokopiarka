
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ver1;
using System;
using System.IO;
using Zadanie2;
using System.Numerics;
using System.Data;

namespace Zadanie2UnitTests
{

    public class ConsoleRedirectionToStringWriter : IDisposable
    {
        private StringWriter stringWriter;
        private TextWriter originalOutput;

        public ConsoleRedirectionToStringWriter()
        {
            stringWriter = new StringWriter();
            originalOutput = Console.Out;
            Console.SetOut(stringWriter);
        }

        public string GetOutput()
        {
            return stringWriter.ToString();
        }

        public void Dispose()
        {
            Console.SetOut(originalOutput);
            stringWriter.Dispose();
        }
    }

    [TestClass]
    public class UnitTestMultiFunctionalDevice
    {
        //Cyszczenie zbioru dostępnych faxów (inaczej wyskoczy błąd duplikatu):
        public void ClearFaxBook()
        {
            //receivedFaxDocumentsByNumber.Clear();
            MultiFunctionalDevice.receivedFaxDocumentsByNumber.Clear();
        }

        [TestMethod]
        public void MultiFunctionalDevice_GetState_StateOff()
        {
            ClearFaxBook();
            var multiDevice = new MultiFunctionalDevice(111111111);
            multiDevice.PowerOff();

            Assert.AreEqual(IDevice.State.off, multiDevice.GetState());
        }

        [TestMethod]
        public void MultiFunctionalDevice_GetState_StateOn()
        {
            ClearFaxBook();
            var multiDevice = new MultiFunctionalDevice(111111111);
            multiDevice.PowerOn();

            Assert.AreEqual(IDevice.State.on, multiDevice.GetState());
        }


        // weryfikacja, czy po wywołaniu metody `Print` i włączonym urządzeniu w napisie pojawia się słowo `Print`
        // wymagane przekierowanie konsoli do strumienia StringWriter
        [TestMethod]
        public void MultiFunctionalDevice_Print_DeviceOn()
        {
            ClearFaxBook();
            var multiDevice = new MultiFunctionalDevice(111111111);
            multiDevice.PowerOn();

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                IDocument doc1 = new PDFDocument("aaa.pdf");
                multiDevice.Print(in doc1);
                Assert.IsTrue(consoleOutput.GetOutput().Contains("Print"));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }

        // weryfikacja, czy po wywołaniu metody `Print` i wyłączonym urządzeniu w napisie NIE pojawia się słowo `Print`
        // wymagane przekierowanie konsoli do strumienia StringWriter
        [TestMethod]
        public void MultiFunctionalDevice_Print_DeviceOff()
        {
            ClearFaxBook();
            var multiDevice = new MultiFunctionalDevice(111111111);
            multiDevice.PowerOff();

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                IDocument doc1 = new PDFDocument("aaa.pdf");
                multiDevice.Print(in doc1);
                Assert.IsFalse(consoleOutput.GetOutput().Contains("Print"));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }

        // weryfikacja, czy po wywołaniu metody `Scan` i wyłączonym urządzeniu w napisie NIE pojawia się słowo `Scan`
        // wymagane przekierowanie konsoli do strumienia StringWriter
        [TestMethod]
        public void MultiFunctionalDevice_Scan_DeviceOff()
        {
            ClearFaxBook();
            var multiDevice = new MultiFunctionalDevice(111111111);
            multiDevice.PowerOff();

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                IDocument doc1;
                multiDevice.Scan(out doc1);
                Assert.IsFalse(consoleOutput.GetOutput().Contains("Scan"));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }

        // weryfikacja, czy po wywołaniu metody `Scan` i włączonym urządzeniu w napisie pojawia się słowo `Scan`
        // wymagane przekierowanie konsoli do strumienia StringWriter
        [TestMethod]
        public void MultiFunctionalDevice_Scan_DeviceOn()
        {
            ClearFaxBook();
            var multiDevice = new MultiFunctionalDevice(111111111);
            multiDevice.PowerOn();

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                IDocument doc1;
                multiDevice.Scan(out doc1);
                Assert.IsTrue(consoleOutput.GetOutput().Contains("Scan"));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }

        // weryfikacja, czy po wywołaniu metody `Send` i wyłączonym urządzeniu w napisie pojawia się słowo `Fax`
        // wymagane przekierowanie konsoli do strumienia StringWriter
        [TestMethod]
        public void MultiFunctionalDevice_Send_DeviceOff()
        {
            ClearFaxBook();
            var multiDevice1  = new MultiFunctionalDevice(111111111);
            var multiDevice2 = new MultiFunctionalDevice(222222222);
            multiDevice1.PowerOff();

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                IDocument doc1 = new PDFDocument("aaa.pdf");
                multiDevice1.Send(in doc1, 222222222);
                Assert.IsFalse(consoleOutput.GetOutput().Contains("Fax"));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }

        // weryfikacja, czy po wywołaniu metody `Send` i włączonym urządzeniu w napisie pojawia się słowo `Fax`
        // wymagane przekierowanie konsoli do strumienia StringWriter
        public void MultiFunctionalDevice_Send_DeviceOn()
        {
            ClearFaxBook();
            var multiDevice1 = new MultiFunctionalDevice(111111111);
            var multiDevice2 = new MultiFunctionalDevice(222222222);

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                IDocument doc1 = new PDFDocument("aaa.pdf");
                multiDevice1.Send(in doc1, 222222222);
                Assert.IsTrue(consoleOutput.GetOutput().Contains("Fax"));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }

        // weryfikacja, czy po wywołaniu metody `Receive` i wyłączonym urządzeniu w napisie pojawia się słowo `receive`
        // wymagane przekierowanie konsoli do strumienia StringWriter
        public void MultiFunctionalDevice_Receive_DeviceOff()
        {
            ClearFaxBook();
            var multiDevice1 = new MultiFunctionalDevice(111111111);
            var multiDevice2 = new MultiFunctionalDevice(222222222);
            multiDevice2.PowerOff();

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                IDocument doc1 = new PDFDocument("aaa.pdf");
                multiDevice1.Send(in doc1, 222222222);
                multiDevice2.Receive();
                Assert.IsFalse(consoleOutput.GetOutput().Contains("received"));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }

        // weryfikacja, czy po wywołaniu metody `Receive` i włączonym urządzeniu w napisie pojawia się słowo `receive`
        // wymagane przekierowanie konsoli do strumienia StringWriter
        public void MultiFunctionalDevice_Receive_DeviceOn()
        {
            ClearFaxBook();
            var multiDevice1 = new MultiFunctionalDevice(111111111);
            var multiDevice2 = new MultiFunctionalDevice(222222222);

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                IDocument doc1 = new PDFDocument("aaa.pdf");
                multiDevice1.Send(in doc1, 222222222);
                multiDevice2.Receive();
                Assert.IsTrue(consoleOutput.GetOutput().Contains("received"));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }


        // weryfikacja, czy wywołanie metody `Scan` z parametrem określającym format dokumentu
        // zawiera odpowiednie rozszerzenie (`.jpg`, `.txt`, `.pdf`)
        [TestMethod]
        public void MultiFunctionalDevice_Scan_FormatTypeDocument()
        {
            ClearFaxBook();
            var multiDevice = new MultiFunctionalDevice(111111111);
            multiDevice.PowerOn();

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                IDocument doc1;
                multiDevice.Scan(out doc1, formatType: IDocument.FormatType.JPG);
                Assert.IsTrue(consoleOutput.GetOutput().Contains("Scan"));
                Assert.IsTrue(consoleOutput.GetOutput().Contains(".jpg"));

                multiDevice.Scan(out doc1, formatType: IDocument.FormatType.TXT);
                Assert.IsTrue(consoleOutput.GetOutput().Contains("Scan"));
                Assert.IsTrue(consoleOutput.GetOutput().Contains(".txt"));

                multiDevice.Scan(out doc1, formatType: IDocument.FormatType.PDF);
                Assert.IsTrue(consoleOutput.GetOutput().Contains("Scan"));
                Assert.IsTrue(consoleOutput.GetOutput().Contains(".pdf"));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }

        //Weryfikacja czy numer faxu jest poprawnie przypisywany do urządzenia
        [TestMethod]
        public void MultiFunctionalDevice_FaxNumber()
        {
            ClearFaxBook();
            var multiDevice = new MultiFunctionalDevice(111111111);

            Assert.AreEqual(111111111, multiDevice.FaxNumber);
        }

        //Weryfikacja czy numer faxu poprawnie zapisuje się w 'książce' faxów
        [TestMethod]
        public void MultiFunctionalDevice_InFaxBook()
        {
            ClearFaxBook();
            var multiDevice1 = new MultiFunctionalDevice(111111111);
            var multiDevice2 = new MultiFunctionalDevice(222222222);

            Assert.IsTrue(MultiFunctionalDevice.receivedFaxDocumentsByNumber.ContainsKey(multiDevice1.FaxNumber));
            Assert.IsTrue(MultiFunctionalDevice.receivedFaxDocumentsByNumber.ContainsKey(multiDevice2.FaxNumber));
        }

        #region Counters

        [TestMethod]
        public void MultiFunctionalDevice_PrintCounter()
        {
            ClearFaxBook();
            var multiDevice = new MultiFunctionalDevice(111111111);
            multiDevice.PowerOn();

            IDocument doc1 = new PDFDocument("aaa.pdf");
            multiDevice.Print(in doc1);
            IDocument doc2 = new TextDocument("aaa.txt");
            multiDevice.Print(in doc2);
            IDocument doc3 = new ImageDocument("aaa.jpg");
            multiDevice.Print(in doc3);

            multiDevice.PowerOff();
            multiDevice.Print(in doc3);
            multiDevice.Scan(out doc1);
            multiDevice.PowerOn();

            // 3 wydruków, gdy urządzenie włączone
            Assert.AreEqual(3, multiDevice.PrintCounter);
        }

        [TestMethod]
        public void MultiFunctionalDevice_PrintCounterAfterFax()
        {
            ClearFaxBook();
            var multiDevice1 = new MultiFunctionalDevice(111111111);
            var multiDevice2 = new MultiFunctionalDevice(222222222);
            multiDevice1.PowerOn();
            multiDevice2.PowerOn();

            IDocument doc1 = new PDFDocument("aaa.pdf");
            multiDevice1.Send(in doc1, 222222222);
            IDocument doc2 = new TextDocument("bbb.txt");
            multiDevice1.Send(in doc2, 222222222);

            IDocument doc3 = new ImageDocument("ccc.jpg");
            multiDevice2.Print(in doc3);

            multiDevice2.Receive();

            Assert.AreEqual(3, multiDevice2.PrintCounter);
        }

        [TestMethod]
        public void MultiFunctionalDevice_ScanCounter()
        {
            ClearFaxBook();
            var multiDevice = new MultiFunctionalDevice(111111111);
            multiDevice.PowerOn();

            IDocument doc1;
            multiDevice.Scan(out doc1);
            IDocument doc2;
            multiDevice.Scan(out doc2);

            IDocument doc3 = new ImageDocument("aaa.jpg");
            multiDevice.Print(in doc3);

            multiDevice.PowerOff();
            multiDevice.Print(in doc3);
            multiDevice.Scan(out doc1);
            multiDevice.PowerOn();

            // 2 skany, gdy urządzenie włączone
            Assert.AreEqual(2, multiDevice.ScanCounter);
        }

        [TestMethod]
        public void MultiFunctionalDevice_PowerOnCounter()
        {
            ClearFaxBook();
            var multiDevice = new MultiFunctionalDevice(111111111);
            multiDevice.PowerOn();
            multiDevice.PowerOn();
            multiDevice.PowerOn();

            IDocument doc1;
            multiDevice.Scan(out doc1);
            IDocument doc2;
            multiDevice.Scan(out doc2);

            multiDevice.PowerOff();
            multiDevice.PowerOff();
            multiDevice.PowerOff();
            multiDevice.PowerOn();

            IDocument doc3 = new ImageDocument("aaa.jpg");
            multiDevice.Print(in doc3);

            multiDevice.PowerOff();
            multiDevice.Print(in doc3);
            multiDevice.Scan(out doc1);
            multiDevice.PowerOn();

            // 3 włączenia
            Assert.AreEqual(3, multiDevice.Counter);
        }

        [TestMethod]
        public void MultiFunctionalDevice_FaxSendCounter()
        {
            ClearFaxBook();
            var multiDevice1 = new MultiFunctionalDevice(111111111);
            var multiDevice2 = new MultiFunctionalDevice(222222222);
            multiDevice1.PowerOn();

            IDocument doc1 = new PDFDocument("aaa.pdf");
            multiDevice1.Send(in doc1, 222222222);
            IDocument doc2 = new TextDocument("bbb.txt");
            multiDevice1.Send(in doc2, 222222222);

            IDocument doc3 = new ImageDocument("ccc.jpg");
            multiDevice1.Print(in doc3);

            multiDevice1.PowerOff();
            multiDevice1.Send(in doc3, 222222222);
            multiDevice1.Scan(out doc1);
            multiDevice1.PowerOn();

            Assert.AreEqual(2, multiDevice1.FaxSendCounter);
        }

        [TestMethod]
        public void MultiFunctionalDevice_FaxReceiveCounter()
        {
            ClearFaxBook();
            var multiDevice1 = new MultiFunctionalDevice(111111111);
            var multiDevice2 = new MultiFunctionalDevice(222222222);
            multiDevice1.PowerOn();

            IDocument doc1 = new PDFDocument("aaa.pdf");
            multiDevice1.Send(in doc1, 222222222);
            IDocument doc2 = new TextDocument("bbb.txt");
            multiDevice1.Send(in doc2, 222222222);

            IDocument doc3 = new ImageDocument("ccc.jpg");
            multiDevice1.Print(in doc3);

            multiDevice1.PowerOff();
            multiDevice1.Send(in doc3, 222222222);
            multiDevice1.Scan(out doc1);

            multiDevice2.PowerOn();
            multiDevice2.Receive();

            multiDevice1.PowerOn();
            multiDevice2.PowerOff();
            multiDevice1.Send(in doc3, 222222222);
            multiDevice2.Receive();

            Assert.AreEqual(2, multiDevice2.FaxReceivedCounter);
        }
        #endregion

        #region Errors & Exceptions

        [DataTestMethod]
        [DataRow(111111)]
        [DataRow(11111111)]
        [DataRow(1111111111)]
        [DataRow(2222222222222)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void MultiFunctionalDevice_Constructor_BadNumberError(long input)
        {
            ClearFaxBook();
            BigInteger converted = (BigInteger)input;
            var multiDevice = new MultiFunctionalDevice(input);
        }

        [TestMethod]
        [ExpectedException(typeof(DuplicateNameException))]
        public void MultiFunctionalDevice_Constructor_Duplicate()
        {
            ClearFaxBook();
            var multiDevice1 = new MultiFunctionalDevice(111111111);
            var multiDevice2 = new MultiFunctionalDevice(111111111);
        }

        [TestMethod]
        public void MultiFunctionalDevice_Send_SendToSelfError()
        {
            ClearFaxBook();
            var multiDevice1 = new MultiFunctionalDevice(111111111);
            multiDevice1.PowerOn();

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                IDocument doc1 = new PDFDocument("aaa.pdf");
                multiDevice1.Send(in doc1, 111111111);
                Assert.IsTrue(consoleOutput.GetOutput().Contains("Fax sending failed."+" You can't send Fax to the same device you are sending it from!"));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }

        [TestMethod]
        public void MultiFunctionalDevice_Send_NotInBookError()
        {
            ClearFaxBook();
            var multiDevice1 = new MultiFunctionalDevice(111111111);
            multiDevice1.PowerOn();

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                IDocument doc1 = new PDFDocument("aaa.pdf");
                multiDevice1.Send(in doc1, 222222222);
                Assert.IsTrue(consoleOutput.GetOutput().Contains("Fax sending failed."+" The specified device does not exist!"));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }

        [TestMethod]
        public void MultiFunctionalDevice_Receive_NoFaxes()
        {
            ClearFaxBook();
            var multiDevice1 = new MultiFunctionalDevice(111111111);
            multiDevice1.PowerOn();

            var currentConsoleOut = Console.Out;
            currentConsoleOut.Flush();
            using (var consoleOutput = new ConsoleRedirectionToStringWriter())
            {
                multiDevice1.Receive();
                Assert.IsTrue(consoleOutput.GetOutput().Contains("No new faxes received."));
            }
            Assert.AreEqual(currentConsoleOut, Console.Out);
        }
        #endregion
    }
}
