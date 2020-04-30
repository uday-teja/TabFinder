using System;
using System.Threading;

namespace TabFinder
{
    class Program
    {
        public static ChromeData chromeData;

        public static void Main(string[] args)
        {
            chromeData = new ChromeData();
            Timer t = new Timer(TimerCallback, null, 0, 3000);
            if (chromeData.IsChromeOpened())
            {
                chromeData.StartMonitoring();
            }
        }

        private static void TimerCallback(object obj)
        {
            Console.WriteLine($"Active Tab at {DateTime.Now} is {chromeData.CurrentTab}");
            //Console.WriteLine("Top Used Tabs");
            //foreach (var item in chromeData.TopUsedTabs)
            //{
            //    Console.WriteLine($"{item}");
            //}
            //Console.WriteLine("Top Used Tabs");
            //foreach (var item in chromeData.AllTabsUsed)
            //{
            //    Console.WriteLine($"{item}");
            //}
            //Console.WriteLine("----------------------------");
        }
    }
}
