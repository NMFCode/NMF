using System;
using System.Globalization;
using System.Linq;
using NMF.Expressions;
using NMF.Expressions.Linq;
using NMF.Models;

namespace PortV3Namespace
{
    public static class FilterExtensionMethods
    {
    	
    	[ObservableProxy(typeof(Proxies), "PortA2InPortBFilter")]
    	public static bool PortA2InPortBFilter(this TypeA.IPortA s)
    	{
    		return PortV3.InputModelContainer.inA.Descendants().OfType<TypeA.IBlockA>().Where(e => e.InputPorts.Contains(s)).Any();
    	}
    	
    	
    	[ObservableProxy(typeof(Proxies), "PortA2OutPortBFilter")]
    	public static bool PortA2OutPortBFilter(this TypeA.IPortA s)
    	{
    		return PortV3.InputModelContainer.inA.Descendants().OfType<TypeA.IBlockA>().Where(e => e.OutputPorts.Contains(s)).Any();
    	}
    	
    	
    	private class Proxies
    	{
	    	
	    	private static ObservingFunc<TypeA.IPortA, bool> portA2InPortBFilterFunc = new ObservingFunc<TypeA.IPortA, bool>(
	    	    s => PortV3.InputModelContainer.inA.Descendants().OfType<TypeA.IBlockA>().Where(e => e.InputPorts.Contains(s)).Any());
	    	
	    	public static INotifyValue<bool> PortA2InPortBFilter(INotifyValue<TypeA.IPortA> self)
	    	{
	    	    return portA2InPortBFilterFunc.Observe(self);
	    	}
	    	
	    	
	    	private static ObservingFunc<TypeA.IPortA, bool> portA2OutPortBFilterFunc = new ObservingFunc<TypeA.IPortA, bool>(
	    	    s => PortV3.InputModelContainer.inA.Descendants().OfType<TypeA.IBlockA>().Where(e => e.OutputPorts.Contains(s)).Any());
	    	
	    	public static INotifyValue<bool> PortA2OutPortBFilter(INotifyValue<TypeA.IPortA> self)
	    	{
	    	    return portA2OutPortBFilterFunc.Observe(self);
	    	}
	    	
    	}
    }
}
