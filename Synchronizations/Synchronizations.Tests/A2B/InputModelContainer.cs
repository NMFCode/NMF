using NMF.Models;

namespace A2BHelperWithoutContextNamespace
{
    public class InputModelContainer
    {
    	public InputModelContainer(Model IN) 
    	{
			this.IN = IN;
    	}
    	
		public Model IN { get; private set; }
		
    }
}
