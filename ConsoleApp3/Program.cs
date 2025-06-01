using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    class Program
    {
        public interface IWasher
        {
            void Wash(string cloth);
        }

        public class WashingMachine : IWasher
        {
            public void Wash(string cloth)
            {
                string time = DateTime.Now.ToString("HH:mm:ss");
                Console.WriteLine($"[PRALKA] [{time}] Rozpoczynam pranie: {cloth}");
                Thread.Sleep(1000);
                string endTime = DateTime.Now.ToString("HH:mm:ss");
                Console.WriteLine($"[PRALKA] [{endTime}] Zakończono pranie: {cloth}");
            }
        }

        public class ManualWasher
        {
            public void ScrubWithBoard(string clothes)
            {
                Console.WriteLine("Rozpoczynam pranie ręczne.");
            }
        }

        public class ManualWasherAdapter : IWasher
        {
            private readonly ManualWasher _manualWasher;
            public ManualWasherAdapter(ManualWasher manualWasher)
            {
                _manualWasher = manualWasher;
            }
            public void Wash(string cloth)
            {
                _manualWasher.ScrubWithBoard(cloth);
            }
        }

        public class LaundryService
        {
            private readonly IWasher _washer;
            public LaundryService(IWasher washer)
            {
                _washer = washer;
            }
            public void WashAll(List<string> clothes)
            {
                foreach (var cloth in clothes)
                {
                    _washer.Wash(cloth);
                }
            }
        }

        static void Main(string[] args)
        {
            // Lista ubrań
            List<string> clothes = new List<string>
            {
                "Koszulka",
                "Spodnie dresowe",
                "Skarpetki",
                "Bluza z kapturem",
                "Czapka zimowa"
            };

            ManualWasher manualWasher = new ManualWasher();
            IWasher manualWasherAdapter = new ManualWasherAdapter(manualWasher);
            LaundryService manualLaundryService = new LaundryService(manualWasherAdapter);
            Console.WriteLine("=== Rozpoczynam pranie ręczne ===");
            manualLaundryService.WashAll(clothes);

            IWasher washingMachine = new WashingMachine();
            LaundryService autoLaundryService = new LaundryService(washingMachine);
            Console.WriteLine("\n=== Rozpoczynam pranie automatyczne ===");
            Thread thread = new Thread(() => autoLaundryService.WashAll(clothes));
            thread.Start();
            thread.Join(); // wait
        }
    }
}
