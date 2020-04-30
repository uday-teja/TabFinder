using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabFinder
{
    public class ChromeData
    {
        const int NumTopTabs = 10;

        List<int> Counts = new List<int>();
        public ArrayList AllTabsUsed = new ArrayList();
        int IndexProcesChrome = -1;
        Process[] List;

        public int Delay = 250;
        public bool IsActive { get; private set; }

        public string CurrentTab = "";

        public string[] TopUsedTabs = new string[NumTopTabs];


        public bool IsChromeOpened()
        {
            List = Process.GetProcessesByName("chrome");
            if (List.Count() == 0)
                return false;
            else
                return true;
        }

        public void StartMonitoring()
        {
            IsActive = true;
            while (IsActive)
            {
                if (!IsChromeOpened())
                {
                    StopMonitoring();
                    break;
                }
                else
                {
                    IndexProcesChrome = -1;
                    foreach (Process p in List)
                    {
                        IndexProcesChrome += 1;
                        if (p.MainWindowTitle != "")
                            break;
                    }
                    Process Proc = List[IndexProcesChrome];
                    try
                    {
                        CurrentTab = Proc.MainWindowTitle.Remove(Proc.MainWindowTitle.Length - 15);
                        if (!AllTabsUsed.Contains(Proc.MainWindowTitle))
                        {
                            AllTabsUsed.Add(Proc.MainWindowTitle);
                            Counts.Add(1);
                        }
                        else
                            Counts[AllTabsUsed.IndexOf(Proc.MainWindowTitle)] += 1;
                        OrderMostUsedTabs();
                    }
                    catch (Exception e) { };
                }
            }
        }

        private void OrderMostUsedTabs()
        {
            int[] TempCounts = Counts.ToArray();
            for (int i = 0; (i < NumTopTabs) && (i < Counts.Count); i++)
            {
                int Max = 0;
                for (int j = 0; j < Counts.Count; j++)
                    if ((TempCounts[j] > TempCounts[Max]))
                        Max = j;
                string str = AllTabsUsed[Max].ToString();
                TopUsedTabs[i] = str.Remove(str.Length - 15);
                TempCounts[Max] = -1;
            }
        }

        public void StopMonitoring()
        {
            IsActive = false;
            CurrentTab = "";
        }
    }
}
