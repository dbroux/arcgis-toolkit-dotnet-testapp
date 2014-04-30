using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
#if NETFX_CORE
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
#else
using System.Windows;
using System.Windows.Controls;
#endif

namespace Esri.ArcGISRuntime.Toolkit.TestApp
{
    /// <summary>
    /// SampleDatasource
    /// </summary>
    public class SampleDatasource
    {
        private SampleDatasource()
        {
            Samples = Application.Current.GetType().GetTypeInfo().Assembly.ExportedTypes
                .Where(t => t.GetTypeInfo().IsSubclassOf(typeof(Page)) && t.FullName.Contains(".Samples."))
                .Select(t => new Sample { Page = t, Name = t.Name }).ToArray();
        }

        /// <summary>
        /// Samples defined in the current project
        /// </summary>
        public IEnumerable<Sample> Samples { get; private set; }

        private static SampleDatasource _current;
        /// <summary>
        /// Unique SampleDataSource instance
        /// </summary>
        public static SampleDatasource Current
        {
            get { return _current ?? (_current = new SampleDatasource()); }
        }
    }

    /// <summary>
    /// Sample
    /// </summary>
    public class Sample
    {
        /// <summary>
        /// Sample Type
        /// </summary>
        public Type Page { get; set; }

        /// <summary>
        /// Sample Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Sample Description
        /// </summary>
        public string Description { get; set; }
    }
}
