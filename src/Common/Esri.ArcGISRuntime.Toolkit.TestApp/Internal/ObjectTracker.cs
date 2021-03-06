﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// Code coming from Callipso WinStore toolkit: https://github.com/timheuer/callisto

namespace Esri.ArcGISRuntime.Toolkit.TestApp.Internal
{
    internal static class ObjectTracker
    {
        private static readonly object Monitor = new object();
        private static readonly List<WeakReference> Objects = new List<WeakReference>();
        private static bool? _shouldTrack;

        public static void Track(object objectToTrack)
        {
            if (ShouldTrack())
            {
                lock (Monitor)
                {
                    Objects.Add(new WeakReference(objectToTrack));
                }
            }
        }

        internal static bool ShouldTrack()
        {
            if (_shouldTrack == null)
            {
                _shouldTrack = Debugger.IsAttached;
            }

            return _shouldTrack.Value;
        }

        public static IEnumerable<object> GetAllLiveTrackedObjects()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            lock (Monitor)
            {
                var toDelete = Objects.Where(o => !o.IsAlive).ToArray();
                foreach (var o in toDelete)
                    Objects.Remove(o);
                return Objects.Select(o => o.Target).ToArray();
            }
        }

        public static string GarbageCollect()
        {
            var liveObjects = ObjectTracker.GetAllLiveTrackedObjects();

            StringBuilder sbStatus = new StringBuilder();

            Debug.WriteLine("---------------------------------------------------------------------");
            if (!liveObjects.Any())
            {
                sbStatus.AppendLine("No Memory Leaks.");
            }
            else
            {
                sbStatus.AppendLine("***    Possible memory leaks in the objects below or their children.   ***");
                sbStatus.AppendLine("*** Clear memory again and see if any of the objects free from memory. ***");
            }
            foreach (object obj in liveObjects)
            {
                string strAliveObj = obj.GetType().ToString();
                sbStatus.AppendLine(strAliveObj);
            }
            if (liveObjects.Any())
                sbStatus.AppendLine(string.Format("*** #Live Objects {0}", liveObjects.Count()));
            sbStatus.AppendLine("----");
            //long lBytes = GC.GetTotalMemory(true);
            //sbStatus.AppendLine(string.Format("GC.GetTotalMemory(true): {0} Bytes, {1} MB", lBytes.ToString(), (lBytes / 1024 / 1024).ToString()));
            Debug.WriteLine(sbStatus.ToString());
            Debug.WriteLine("---------------------------------------------------------------------");
            return sbStatus.ToString();
        }
    }
}
