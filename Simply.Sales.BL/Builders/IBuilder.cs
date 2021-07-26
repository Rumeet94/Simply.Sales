namespace Simply.Sales.BLL.Builders {
	public interface IBuilder<TIn, TOut> {
		TOut Build(TIn source);
	}
}
