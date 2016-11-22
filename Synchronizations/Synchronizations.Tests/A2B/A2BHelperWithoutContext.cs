using System;
using System.Collections.Generic;
using System.Globalization;
using NMF.Expressions.Linq;
using NMF.Synchronizations;
using NMF.Transformations;
using NMF.Utilities;
using NMF.Models;

namespace A2BHelperWithoutContextNamespace
{
    public class A2BHelperWithoutContext : ReflectiveSynchronization
    {
    	public static InputModelContainer InputModelContainer { get; set; }
    	
    	
    	public class Model2ModelMainRule : SynchronizationRule<InputModelContainer, OutputModelContainer>
    	{
    	    public override void DeclareSynchronization()
    	    {
    	    	SynchronizeManyLeftToRightOnly(SyncRule<RuleA>(),
    	    		input => input.IN.Descendants().OfType<TypeA.IA>(),
    	    		output => new OutputModelCollection<TypeB.IA>(output.OUT.RootElements.OfType<IModelElement, TypeB.IA>()));
    	    	
    	    	SynchronizeManyLeftToRightOnly(SyncRule<RuleB>(),
    	    		input => input.IN.Descendants().OfType<TypeA.IB>(),
    	    		output => new OutputModelCollection<TypeB.IB>(output.OUT.RootElements.OfType<IModelElement, TypeB.IB>()));
    	    }
    	}
    	
    	
    	public class RuleA : SynchronizationRule<TypeA.IA, TypeB.IA>
    	{
    	    public override void DeclareSynchronization()
    	    {
    	    	SynchronizeLeftToRightOnly(
    	    		s => s.NameA + HelperExtensionMethods.NameExtension(),
    	    		t => t.Name);
    	    	
    	    	SynchronizeManyLeftToRightOnly(SyncRule<RuleB>(),
    	    		s => s.Elms,
    	    		t => t.Elms);
    	    	
    	    	SynchronizeLeftToRightOnly(
    	    		s => HelperExtensionMethods.TestNameOfAElement("ABC", s),
    	    		t => t.IsNameABC);
    	    }
    	}
    	
    	
    	public class RuleB : SynchronizationRule<TypeA.IB, TypeB.IB>
    	{
    	    public override void DeclareSynchronization()
    	    {
    	    	SynchronizeLeftToRightOnly(
    	    		s => s.NameB + HelperExtensionMethods.NameExtension(),
    	    		t => t.Name);
    	    	
    	    	SynchronizeLeftToRightOnly(SyncRule<RuleA>(),
    	    		s => HelperExtensionMethods.FirstAElement(),
    	    		t => t.FirstAElement.FirstOrDefault(),
    	    		null);
    	    	
    	    	SynchronizeLeftToRightOnly(SyncRule<RuleA>(),
    	    		s => HelperExtensionMethods.AElementWithName("ABC"),
    	    		t => t.AElementWithName.FirstOrDefault(),
    	    		null);
    	    	
    	    	SynchronizeManyLeftToRightOnly(SyncRule<RuleA>(),
    	    		s => s.Elms,
    	    		t => t.Elms);
    	    }
    	}
    	
    }
}
