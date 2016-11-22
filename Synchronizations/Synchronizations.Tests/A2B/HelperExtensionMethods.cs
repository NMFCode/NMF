using System;
using System.Globalization;
using System.Linq;
using NMF.Expressions;
using NMF.Expressions.Linq;
using NMF.Models;

namespace A2BHelperWithoutContextNamespace
{
    public static class HelperExtensionMethods
    {
    	
    	public static string NameExtension()
    	{
    		//attribute helper
    		return "Extension";
    	}
    	
    	
    	public static TypeA.IA FirstAElement()
    	{
    		//attribute helper
    		return A2BHelperWithoutContext.InputModelContainer.IN.Descendants().OfType<TypeA.IA>().FirstOrDefault();
    	}
    	
    	
    	[ObservableProxy(typeof(Proxies), "AElementWithName")]
    	public static TypeA.IA AElementWithName(string name)
    	{
    		//functional (operational) helper
    		return A2BHelperWithoutContext.InputModelContainer.IN.Descendants().OfType<TypeA.IA>().FirstOrDefault(element => element.NameA == name);
    	}
    	
    	
    	[ObservableProxy(typeof(Proxies), "TestNameOfAElement")]
    	public static bool TestNameOfAElement(string name, TypeA.IA element)
    	{
    		//functional (operational) helper
    		return (element.NameA == name) ? 
    			(true) : (false);
    	}
    	
    	
    	private class Proxies
    	{
	    	
	    	private static ObservingFunc<string, TypeA.IA> aElementWithNameFunc = 
	    		new ObservingFunc<string, TypeA.IA>(
	    	    (name) => A2BHelperWithoutContext.InputModelContainer.IN.Descendants().OfType<TypeA.IA>().FirstOrDefault(element => element.NameA == name));
	    	
	    	public static INotifyValue<TypeA.IA> AElementWithName(INotifyValue<string> name)
	    	{
	    	    return aElementWithNameFunc.Observe(name);
	    	}
	    	
	    	
	    	private static ObservingFunc<string, TypeA.IA, bool> testNameOfAElementFunc = 
	    		new ObservingFunc<string, TypeA.IA, bool>(
	    	    (name, element) => (element.NameA == name) ? 
	    	    	(true) : (false));
	    	
	    	public static INotifyValue<bool> TestNameOfAElement(INotifyValue<string> name, INotifyValue<TypeA.IA> element)
	    	{
	    	    return testNameOfAElementFunc.Observe(name, element);
	    	}
	    	
    	}
    }
}
