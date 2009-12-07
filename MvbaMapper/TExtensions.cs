namespace MvbaMapper
{
	public static class TExtensions
	{
		public static void MapFrom(this object destination, object source)
		{
			new SimpleMapper().Map(source, destination);
		}

		public static TDestination MapTo<TDestination>(this object source)
			where TDestination : class, new()
		{
			var destination = new TDestination();
			destination.MapFrom(source);
			return destination;
		}
	}
}