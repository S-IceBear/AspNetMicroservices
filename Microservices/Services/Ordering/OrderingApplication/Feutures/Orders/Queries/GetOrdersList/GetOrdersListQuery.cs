﻿using MediatR;

namespace OrderingApplication.Feutures.Orders.Queries.GetOrdersList
{
    public class GetOrdersListQuery : IRequest<List<OrderDTO>>
    {
        public string UserName { get; } = string.Empty;

        public GetOrdersListQuery(string userName)
        {
            UserName = userName;
        }
    }
}
