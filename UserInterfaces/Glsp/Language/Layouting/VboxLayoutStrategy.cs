﻿using NMF.Glsp.Graph;
using NMF.Glsp.Protocol.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NMF.Glsp.Language.Layouting
{
    internal class VboxLayoutStrategy : LayoutStrategy
    {
        public static readonly VboxLayoutStrategy Instance = new VboxLayoutStrategy();

        public override void Apply(GElement container)
        {
            container.Details["layout"] = "vbox";
        }

        public override void SetPosition(GElement element, Point position)
        {
            var pos = position;
            foreach (var child in element.Parent.Children)
            {
                if (child != element)
                {
                    pos = new Point(pos.X, pos.Y + (child.Size?.Height ?? 0));
                }
                else
                {
                    break;
                }
            }
            element.Position = pos;
        }
    }
}