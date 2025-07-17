using Microsoft.Msagl.Core.Geometry.Curves;
using Microsoft.Msagl.Core.Layout;
using Microsoft.Msagl.Core.Routing;
using Microsoft.Msagl.Layout.Layered;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Processing.Layouting
{
    /// <summary>
    /// Denotes a layered layout calculation
    /// </summary>
    public class LayeredLayoutService : AglLayoutService
    {
        /// <summary>
        /// The default instance
        /// </summary>
        public static readonly LayeredLayoutService Instance = new LayeredLayoutService();

        private static SugiyamaLayoutSettings DefaultSettings => new SugiyamaLayoutSettings
        {
            Transformation = PlaneTransformation.Rotation(Math.PI / 2),
            EdgeRoutingSettings = { EdgeRoutingMode = EdgeRoutingMode.Rectilinear }
        };

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public LayeredLayoutService() : this(null) { }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        /// <param name="settings">layout settings</param>
        public LayeredLayoutService(SugiyamaLayoutSettings settings)
        {
            Settings = settings ?? DefaultSettings;
        }

        /// <summary>
        /// Gets the layout settings for this layout service
        /// </summary>
        public SugiyamaLayoutSettings Settings { get; }

        /// <inheritdoc />
        protected override void ProcessLayout(GeometryGraph g)
        {
            var layout = new LayeredLayout(g, Settings);
            layout.Run();
        }
    }
}
