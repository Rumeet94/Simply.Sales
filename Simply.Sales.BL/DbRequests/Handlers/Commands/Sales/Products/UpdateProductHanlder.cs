﻿using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using MediatR;

using Simply.Sales.BLL.DbRequests.Requests.Commands.Sales.Products;
using Simply.Sales.DLL.Models.Sales;

namespace Simply.Sales.BLL.DbRequests.Handlers.Commands.Sales.Products {
	public class UpdateProductHanlder : BaseUpdateHandler, IRequestHandler<UpdateProduct> {
		private readonly IMapper _mapper;

		public UpdateProductHanlder(IServiceProvider serviceProvider, IMapper mapper)
			: base(serviceProvider) {
			Contract.Requires(mapper != null);

			_mapper = mapper;
		}

		public async Task<Unit> Handle(UpdateProduct request, CancellationToken cancellationToken) {
			var product = _mapper.Map<Product>(request.Dto);

			await Handle(product);

			return Unit.Value;
		}
	}
}
