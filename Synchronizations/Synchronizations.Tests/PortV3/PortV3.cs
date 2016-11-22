using System;
using System.Collections.Generic;
using System.Globalization;
using NMF.Expressions.Linq;
using NMF.Synchronizations;
using NMF.Transformations;
using NMF.Utilities;
using NMF.Models;

namespace PortV3Namespace
{
    public class PortV3 : ReflectiveSynchronization
    {
    	public static InputModelContainer InputModelContainer { get; set; }
    	
    	
    	public class Model2ModelMainRule : SynchronizationRule<InputModelContainer, OutputModelContainer>
    	{
    	    public override void DeclareSynchronization()
    	    {
    	    	SynchronizeManyLeftToRightOnly(SyncRule<BlkA2BlkB>(),
    	    		input => input.inA.Descendants().OfType<TypeA.IBlockA>(),
    	    		output => new OutputModelCollection<TypeB.IBlockB>(output.inB.RootElements.OfType<IModelElement, TypeB.IBlockB>()));
    	    	
    	    	SynchronizeManyLeftToRightOnly(SyncRule<PortA2InPortB>(),
    	    		input => input.inA.Descendants().OfType<TypeA.IPortA>().Where(x => x.PortA2InPortBFilter()),
    	    		output => new OutputModelCollection<TypeB.IInPortB>(output.inB.RootElements.OfType<IModelElement, TypeB.IInPortB>()));
    	    	
    	    	SynchronizeManyLeftToRightOnly(SyncRule<PortA2OutPortB>(),
    	    		input => input.inA.Descendants().OfType<TypeA.IPortA>().Where(x => x.PortA2OutPortBFilter()),
    	    		output => new OutputModelCollection<TypeB.IOutPortB>(output.inB.RootElements.OfType<IModelElement, TypeB.IOutPortB>()));
    	    }
    	}
    	
    	
    	public class BlkA2BlkB : SynchronizationRule<TypeA.IBlockA, TypeB.IBlockB>
    	{
    	    public override void DeclareSynchronization()
    	    {
    	    	SynchronizeManyLeftToRightOnly(SyncRule<PortA2InPortB>(),
    	    		blkA => blkA.InputPorts.Where(x => x.PortA2InPortBFilter()),
    	    		blkB => blkB.InputPorts);
    	    	
    	    	SynchronizeManyLeftToRightOnly(SyncRule<PortA2OutPortB>(),
    	    		blkA => blkA.OutputPorts.Where(x => x.PortA2OutPortBFilter()),
    	    		blkB => blkB.OutputPorts);
    	    }
    	}
    	
    	
    	public class PortA2InPortB : SynchronizationRule<TypeA.IPortA, TypeB.IInPortB>
    	{
    	    public override void DeclareSynchronization()
    	    {
    	    	SynchronizeLeftToRightOnly(
    	    		s => s.Name,
    	    		t => t.Name);
    	    }
    	}
    	
    	
    	public class PortA2OutPortB : SynchronizationRule<TypeA.IPortA, TypeB.IOutPortB>
    	{
    	    public override void DeclareSynchronization()
    	    {
    	    	SynchronizeLeftToRightOnly(
    	    		s => s.Name,
    	    		t => t.Name);
    	    }
    	}
    	
    }
}
