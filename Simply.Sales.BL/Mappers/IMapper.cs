namespace Simply.Sales.BLL.Mappers {
	public interface IMapper<Tin, Tout> {
		Tout Map(Tin source);

		Tin BackMap(Tout source);
	}
}
