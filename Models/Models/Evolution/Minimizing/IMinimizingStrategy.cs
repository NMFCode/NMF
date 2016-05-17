using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NMF.Models.Evolution.Minimizing
{
    public interface IMinimizingStrategy
    {
        List<IModelChange> Execute(List<IModelChange> changes);
    }
}
