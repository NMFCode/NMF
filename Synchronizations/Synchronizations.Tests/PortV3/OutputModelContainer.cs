using NMF.Models;

namespace PortV3Namespace
{
    public class OutputModelContainer
    {
        public OutputModelContainer()
        {
			inB = new Model();
        }
        
		public Model inB { get; private set; }
		
    }
}
