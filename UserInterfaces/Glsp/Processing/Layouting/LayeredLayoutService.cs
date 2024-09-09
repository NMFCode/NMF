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

        /// <inheritdoc />
        protected override void ProcessLayout(GeometryGraph g)
        {
            var settings = new SugiyamaLayoutSettings
            {
                Transformation = PlaneTransformation.Rotation(Math.PI / 2),
                EdgeRoutingSettings = { EdgeRoutingMode = EdgeRoutingMode.Rectilinear }
            };
            var layout = new LayeredLayout(g, settings);
            layout.Run();
        }
    }
}
