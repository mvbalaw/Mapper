using NHibernate.Proxy.DynamicProxy;

namespace MvbaMapper.Tests.TestClasses
{
	internal class DynamicDestinationContext
	{
		public DynamicOutputClass Destination;
		public OutputClass Expected;
		public InputClass Source;
	}
}