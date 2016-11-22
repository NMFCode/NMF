using NMF.Models;

namespace A2BHelperWithoutContextNamespace
{
    public class OutputModelContainer
    {
        public OutputModelContainer()
        {
			OUT = new Model();
        }
        
		public Model OUT { get; private set; }
		
    }
}
