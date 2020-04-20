using System;
using System.Runtime.InteropServices;
using System.Threading;
using UIAutomationClient;

namespace TabFinder
{
    class Program
    {
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        static bool TryGetMSEdgeUrlAndTitle(IntPtr edgeWindow, out string url, out string title)
        {
            const int UIA_NamePropertyId = 30005;
            const int UIA_ClassNamePropertyId = 30012;
            const int UIA_NativeWindowHandlePropertyId = 30020;

            url = "";
            title = "";

            IUIAutomation uiA = new CUIAutomation();
            IUIAutomationElement rootElement = uiA.GetRootElement();

            IUIAutomationCacheRequest cacheRequest = uiA.CreateCacheRequest();
            cacheRequest.AddProperty(UIA_NamePropertyId);

            IUIAutomationCondition windowCondition = uiA.CreatePropertyCondition(UIA_NativeWindowHandlePropertyId, GetForegroundWindow());
            IUIAutomationElement windowElement = rootElement.FindFirstBuildCache(TreeScope.TreeScope_Descendants, windowCondition, cacheRequest);
            if (windowElement == null)
                return false;

            IUIAutomationCondition edgeCondition = uiA.CreatePropertyCondition(UIA_NamePropertyId, "Microsoft Edge");
            IUIAutomationElement edgeElement = windowElement.FindFirstBuildCache(TreeScope.TreeScope_Subtree, edgeCondition, cacheRequest);
            if (edgeElement == null)
                return false;

            IUIAutomationCondition tabCondition = uiA.CreatePropertyCondition(UIA_ClassNamePropertyId, "TabWindowClass");
            IUIAutomationElement tabElement = edgeElement.FindFirstBuildCache(TreeScope.TreeScope_Descendants, tabCondition, cacheRequest);
            if (tabElement == null)
                return false;

            IUIAutomationCondition ieCondition = uiA.CreatePropertyCondition(UIA_ClassNamePropertyId, "Internet Explorer_Server");
            IUIAutomationElement ieElement = tabElement.FindFirstBuildCache(TreeScope.TreeScope_Descendants, ieCondition, cacheRequest);
            if (ieElement == null)
                return false;

            url = ieElement.CachedName;
            title = tabElement.CachedName;

            return true;
        }

        static void Main(string[] args)
        {
            string oldUrl = "";
            string oldTitle = "";

            while (true)
            {
                string url = "";
                string title = "";

                if (TryGetMSEdgeUrlAndTitle(GetForegroundWindow(), out url, out title))
                {
                    if ((url != oldUrl) || (title != oldTitle))
                    {
                        Console.WriteLine(String.Format("Page title: {0} \r\nURL: {1}", title, url));

                        oldUrl = url;
                        oldTitle = title;
                    }
                }

                Thread.Sleep(250);
            }
        }
    }
}
