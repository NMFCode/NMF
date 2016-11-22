using NMF.Models;

namespace PortV3Namespace
{
    public class InputModelContainer
    {
    	public InputModelContainer(Model inA) 
    	{
			this.inA = inA;
    	}
    	
		public Model inA { get; private set; }
		
    }
}
