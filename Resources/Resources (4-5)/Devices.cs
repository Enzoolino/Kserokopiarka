using System;
using System.Numerics;

namespace ver2
{
    public interface IDevice
    {
        enum State { on, off, standby };

        void PowerOn() // uruchamia urządzenie, zmienia stan na `on`
        {
            SetState(State.on);
        }
            
        void PowerOff() // wyłącza urządzenie, zmienia stan na `off
        {
            SetState(State.off);
        }
        void StandbyOn() // Włącza w urządzeniu tryb standby
        {
            SetState(State.standby);
        }
        
        void StandbyOff() // Wyłącza w urządzeniu tryb standby
        {
            SetState(State.on);
        }
        
        State GetState(); // zwraca aktualny stan urządzenia
        abstract protected void SetState(State state);

        int Counter {get;}  // zwraca liczbę charakteryzującą eksploatację urządzenia,
                            // np. liczbę uruchomień, liczbę wydrukow, liczbę skanów, ...
    }

    public interface IPrinter : IDevice
    {
        /// <summary>
        /// Dokument jest drukowany, jeśli urządzenie włączone. W przeciwnym przypadku nic się nie wykonuje
        /// </summary>
        /// <param name="document">obiekt typu IDocument, różny od `null`</param>
        void Print(in IDocument document);
    }

    public interface IScanner : IDevice
    {
        // dokument jest skanowany, jeśli urządzenie włączone
        // w przeciwnym przypadku nic się dzieje
        void Scan(out IDocument document, IDocument.FormatType formatType);
    }

}
