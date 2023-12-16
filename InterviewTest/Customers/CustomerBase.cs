using System;
using System.Collections.Generic;
using InterviewTest.Orders;
using InterviewTest.Returns;

namespace InterviewTest.Customers
{
    public abstract class CustomerBase : ICustomer
    {
        private readonly OrderRepository _orderRepository;
        private readonly ReturnRepository _returnRepository;

        protected CustomerBase(OrderRepository orderRepo, ReturnRepository returnRepo)
        {
            _orderRepository = orderRepo;
            _returnRepository = returnRepo;
        }

        public abstract string GetName();
        
        public void CreateOrder(IOrder order)
        {
            _orderRepository.Add(order);
        }

        public List<IOrder> GetOrders()
        {
            return _orderRepository.Get();
        }

        public void CreateReturn(IReturn rga)
        {
            _returnRepository.Add(rga);
        }

        public List<IReturn> GetReturns()
        {
            return _returnRepository.Get();
        }

        public float GetTotalSales()
        {
            float total_sales = 0.0f;
            foreach (Order order in GetOrders()) {
                foreach (OrderedProduct ordered_product in order.Products) {
                    total_sales += ordered_product.Product.GetSellingPrice();
                }
            }
            return total_sales;
            // throw new NotImplementedException();
        }

        public float GetTotalReturns()
        {
            float total_returns = 0.0f;
            foreach (Return returns in GetReturns()) {
                foreach (ReturnedProduct returned_product in returns.ReturnedProducts) {
                    total_returns += returned_product.OrderProduct.Product.GetSellingPrice();
                }
            }
            return total_returns;
            // throw new NotImplementedException();
        }

        public float GetTotalProfit()
        {
            return GetTotalSales() - GetTotalReturns();
            // throw new NotImplementedException();
        }
    }
}
