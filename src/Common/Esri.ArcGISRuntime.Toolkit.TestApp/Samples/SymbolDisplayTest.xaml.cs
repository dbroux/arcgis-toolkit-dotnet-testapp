using Esri.ArcGISRuntime.Geometry;
using Esri.ArcGISRuntime.Symbology;
using System;
using System.Collections.Generic;
using Symbol = Esri.ArcGISRuntime.Symbology.Symbol;
#if NETFX_CORE
using Windows.UI.Xaml.Controls;
using Windows.UI;
#else
using System.Windows.Controls;
using System.Windows.Media;
#endif
#if WINDOWS_PHONE_APP
using Esri.ArcGISRuntime.Toolkit.TestApp.Internal;
#endif

namespace Esri.ArcGISRuntime.Toolkit.TestApp.Samples
{
    /// <summary>
    /// Interaction logic for SymbolDisplayTest.xaml
    /// </summary>
    public partial class SymbolDisplayTest : Page
    {
        private readonly Color _symbolColor = Colors.LawnGreen;
        const string PictureData = "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";

        /// <summary>
        /// Initializes a new instance of the <see cref="SymbolDisplayTest"/> class.
        /// </summary>
        public SymbolDisplayTest()
        {
            var pictureMarkerSymbolSize50 = new PictureMarkerSymbol {Height = 50, Width = 50};
            pictureMarkerSymbolSize50.SetSource(Convert.FromBase64String(PictureData));
            var pictureMarkerSymbolSize100 = new PictureMarkerSymbol { Height = 100, Width = 100 };
            pictureMarkerSymbolSize100.SetSource(Convert.FromBase64String(PictureData));
            var pictureMarkerSymbolSize500 = new PictureMarkerSymbol { Height = 500, Width = 500 };
            pictureMarkerSymbolSize500.SetSource(Convert.FromBase64String(PictureData));
            var pictureMarkerSymbolWithoutSize = new PictureMarkerSymbol();
            pictureMarkerSymbolWithoutSize.SetSource(Convert.FromBase64String(PictureData));

            var pictureMarkerSymbolFromUrl = new PictureMarkerSymbol { Width = 20, Height = 20, XOffset = 50 };
            var task = pictureMarkerSymbolFromUrl.SetSourceAsync(new Uri("http://static.arcgis.com/images/Symbols/Arrows/icon1.png"));


            SymbolInfos = new List<SymbolInfo>
            {
                new SymbolInfo
                {
                    Description = "SMS Square Size 50",
                    Symbol = new SimpleMarkerSymbol {Style = SimpleMarkerStyle.Square, Size = 50, Color = _symbolColor},
                    GeometryType = GeometryType.Point
                },
                new SymbolInfo
                {
                    Description = "SMS Diamond Size 20",
                    Symbol = new SimpleMarkerSymbol {Style = SimpleMarkerStyle.Diamond, Size = 20, Color = _symbolColor},
                    GeometryType = GeometryType.Point
                },
                new SymbolInfo
                {
                    Description = "SMS Diamond Size 10",
                    Symbol = new SimpleMarkerSymbol {Style = SimpleMarkerStyle.Diamond, Size = 10, Color = _symbolColor},
                    GeometryType = GeometryType.Unknown
                },
                new SymbolInfo
                {
                    Description = "SMS Circle Size 50 with outline",
                    Symbol = new SimpleMarkerSymbol {Style = SimpleMarkerStyle.Circle, Size = 50, Color = _symbolColor, Outline = new SimpleLineSymbol {Color= Colors.Green, Width = 2}},
                    GeometryType = GeometryType.Unknown
                },
                new SymbolInfo
                {
                    Description = "SMS Cross Size 50 with outline",
                    Symbol = new SimpleMarkerSymbol {Style = SimpleMarkerStyle.Cross, Size = 50, Color = _symbolColor, Outline = new SimpleLineSymbol {Color= Colors.Green, Width = 2}},
                    GeometryType = GeometryType.Unknown
                },
                new SymbolInfo
                {
                    Description = "SMS Triangle Size 50",
                    Symbol = new SimpleMarkerSymbol {Style = SimpleMarkerStyle.Triangle, Size = 50, Color = _symbolColor},
                    GeometryType = GeometryType.Unknown
                },
                new SymbolInfo
                {
                    Description = "SMS Triangle Size 50 XOffset=20 YOffset=30",
                    Symbol = new SimpleMarkerSymbol {Style = SimpleMarkerStyle.Triangle, Size = 50, Color = _symbolColor, XOffset = 20, YOffset = 30},
                    GeometryType = GeometryType.Unknown
                },
                new SymbolInfo
                {
                    Description = "SMS Diamond Size 5 as Line",
                    Symbol = new SimpleMarkerSymbol {Style = SimpleMarkerStyle.Diamond, Size = 5, Color = _symbolColor},
                    GeometryType = GeometryType.Polyline
                },
                new SymbolInfo
                {
                    Description = "SMS Diamond Size 5 as Polygon",
                    Symbol = new SimpleMarkerSymbol {Style = SimpleMarkerStyle.Diamond, Size = 5, Color = _symbolColor},
                    GeometryType = GeometryType.Polygon
                },
                new SymbolInfo
                {
                    Description = "PMS Size 50",
                    Symbol = pictureMarkerSymbolSize50,
                    GeometryType = GeometryType.Unknown
                },
                new SymbolInfo
                {
                    Description = "PMS Size 100",
                    Symbol = pictureMarkerSymbolSize100,
                    GeometryType = GeometryType.Unknown
                },
                new SymbolInfo
                {
                    Description = "PMS Size 500",
                    Symbol = pictureMarkerSymbolSize500,
                    GeometryType = GeometryType.Unknown
                },
                new SymbolInfo
                {
                    Description = "PMS Without Size (not working for now)",
                    Symbol = pictureMarkerSymbolWithoutSize,
                    GeometryType = GeometryType.Unknown
                },
                new SymbolInfo
                {
                    Description = "PMS From Url",
                    Symbol = pictureMarkerSymbolFromUrl,
                    GeometryType = GeometryType.Unknown
                },
                new SymbolInfo
                {
                    Description = "SLS Solid Width=5",
                    Symbol = new SimpleLineSymbol{Color = _symbolColor, Style = SimpleLineStyle.Solid, Width = 5},
                    GeometryType = GeometryType.Unknown
                },
                new SymbolInfo
                {
                    Description = "SLS DashDot Width=1",
                    Symbol = new SimpleLineSymbol{Color = _symbolColor, Style = SimpleLineStyle.DashDot, Width = 1},
                    GeometryType = GeometryType.Unknown
                },
                new SymbolInfo
                {
                    Description = "SFS With Outline",
                    Symbol = new SimpleFillSymbol{Color = _symbolColor, Outline = new SimpleLineSymbol {Color= Colors.Green, Width = 2}},
                    GeometryType = GeometryType.Unknown
                },
                new SymbolInfo
                {
                    Description = "CIM Symbol",
                    Symbol = Symbol.FromJson(@"{
                        ""type"":""CIMPointSymbol"",
                        ""symbolLayers"":[{""type"":""CIMVectorMarker"",""size"":10,""anchorPoint"":{""x"":0,""y"":0},""anchorPointUnits"":""Points"",""dominantSizeAxis3D"":""Z"",""markerGraphics"":[]},{""type"":""CIMVectorMarker"",""size"":10,""anchorPoint"":{""x"":0,""y"":0},""anchorPointUnits"":""Points"",""dominantSizeAxis3D"":""Z"",""markerGraphics"":[]},{""type"":""CIMVectorMarker"",""size"":25,""anchorPoint"":{""x"":0,""y"":0},""anchorPointUnits"":""Relative"",""scaleStrokesAndFills"":true,""dominantSizeAxis3D"":""Z"",""frame"":{""xmin"":0,""ymin"":0,""xmax"":320.003307088242,""ymax"":320.003401577482},""markerGraphics"":[{""type"":""CIMMarkerGraphic"",""geometry"":{""rings"":[[[92,92],[93.2,77.3],[96.7,63.9],[102.6,51.8],[110.6,41.5],[120.8,32.9],[132.8,26.5],[146.1,22.5],[160,21.2],[174,22.5],[187.2,26.5],[199.2,32.9],[209.4,41.5],[217.4,51.8],[223.3,63.9],[226.8,77.3],[228,92],[242.7,93.2],[256.1,96.7],[268.2,102.6],[278.5,110.6],[287.1,120.8],[293.5,132.8],[297.5,146],[298.8,160],[297.5,173.9],[293.5,187.2],[287.1,199.2],[278.5,209.4],[268.2,217.4],[256.1,223.3],[242.7,226.8],[228,228],[226.8,242.7],[223.3,256.1],[217.4,268.1],[209.4,278.5],[199.2,287.1],[187.2,293.5],[174,297.5],[160,298.8],[146.1,297.5],[132.8,293.5],[120.8,287.1],[110.6,278.5],[102.6,268.1],[96.7,256.1],[93.2,242.7],[92,228],[77.3,226.8],[63.9,223.3],[51.9,217.4],[41.5,209.4],[32.9,199.2],[26.5,187.2],[22.5,173.9],[21.2,160],[22.5,146],[26.5,132.8],[32.9,120.8],[41.5,110.6],[51.9,102.6],[63.9,96.7],[77.3,93.2],[92,92]]]},""symbol"":{""type"":""CIMPolygonSymbol"",""symbolLayers"":[{""type"":""CIMFill"",""pattern"":{""type"":""CIMSolidPattern"",""color"":[255,255,128,255]}}]}},{""type"":""CIMMarkerGraphic"",""geometry"":{""rings"":[[[92,92],[77.3,93.2],[63.9,96.7],[51.9,102.6],[41.5,110.6],[32.9,120.8],[26.5,132.8],[22.5,146],[21.2,160],[22.5,173.9],[26.5,187.2],[32.9,199.2],[41.5,209.4],[51.9,217.4],[63.9,223.3],[77.3,226.8],[92,228],[93.2,242.7],[96.7,256.1],[102.6,268.1],[110.6,278.5],[120.8,287.1],[132.8,293.5],[146.1,297.5],[160,298.8],[174,297.5],[187.2,293.5],[199.2,287.1],[209.4,278.5],[217.4,268.1],[223.3,256.1],[226.8,242.7],[228,228],[242.7,226.8],[256.1,223.3],[268.2,217.4],[278.5,209.4],[287.1,199.2],[293.5,187.2],[297.5,173.9],[298.8,160],[297.5,146],[293.5,132.8],[287.1,120.8],[278.5,110.6],[268.2,102.6],[256.1,96.7],[242.7,93.2],[228,92],[226.8,77.3],[223.3,63.9],[217.4,51.8],[209.4,41.5],[199.2,32.9],[187.2,26.5],[174,22.5],[160,21.2],[146.1,22.5],[132.8,26.5],[120.8,32.9],[110.6,41.5],[102.6,51.8],[96.7,63.9],[93.2,77.3],[92,92]]]},""symbol"":{""type"":""CIMLineSymbol"",""symbolLayers"":[{""type"":""CIMFilledStroke"",""width"":7.9943302622253727,""pattern"":{""type"":""CIMSolidPattern"",""color"":[0,0,0,255]},""capStyle"":""Butt"",""joinStyle"":""Miter"",""miterLimit"":112,""alignment"":""Center"",""lineStyle3D"":""Strip""}]}},{""type"":""CIMMarkerGraphic"",""geometry"":{""rings"":[[[173,126.5],[173.4,136.1],[174.9,142.2],[178.7,147.3],[185.7,153.9],[199.6,166.3],[207.7,175.9],[211.5,184.3],[212.8,193.1],[211.9,201.1],[209.3,208.5],[204.9,215.4],[198.8,221.9],[191.2,227.3],[182.4,231.1],[172.4,233.4],[161.2,234.2],[150.5,233.5],[140.9,231.2],[132.4,227.4],[124.9,222],[118.8,215.6],[114.3,208.5],[111.4,200.7],[110,192.3],[135.2,189.2],[139,199.8],[144.9,207.4],[152.8,211.8],[162.4,213.3],[172.1,211.9],[179.6,207.6],[184.4,201.3],[186,193.9],[185.1,188.4],[182.4,183.3],[168.1,170.5],[158.1,161.3],[152,153],[149,144.1],[148,133],[148.1,126.5],[173,126.5]],[[175.6,89.6],[175.6,117],[148.1,117],[148.1,89.6],[175.6,89.6]]]},""symbol"":{""type"":""CIMPolygonSymbol"",""symbolLayers"":[{""type"":""CIMFill"",""pattern"":{""type"":""CIMSolidPattern"",""color"":[0,0,0,255]}}]}}]}]
                    }"),
                    GeometryType = GeometryType.Unknown
                },
            };

            InitializeComponent();
            DataContext = this;
#if WINDOWS_PHONE_APP
            var navigationHelper = new NavigationHelper(this);
#endif

        }

        /// <summary>
        /// Gets the symbol infos.
        /// </summary>
        public IEnumerable<SymbolInfo> SymbolInfos { get; private set; }

#if NETFX_CORE
        private void SymbolComboBoxOnLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ((ComboBox) sender).SelectedIndex = 0;
        }
#endif
    }

    /// <summary>
    /// Info about a test symbol
    /// </summary>
    public class SymbolInfo
    {
        /// <summary>
        /// Gets or sets the symbol.
        /// </summary>
        public Symbol Symbol { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the type of the geometry.
        /// </summary>
        public GeometryType GeometryType { get; set; }
    }
}


