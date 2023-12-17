using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Data.SqlClient;

namespace InterviewTest.Orders
{
    public class OrderRepository
    {
        private readonly SqlConnection _connection;

        private List<IOrder> orders;
        public OrderRepository(SqlConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            orders = new List<IOrder>();
        }

        public void Add(IOrder newOrder)
        {
            // Create Order
            using (SqlCommand command = new SqlCommand("INSERT INTO Orders (OrderNumber, CustomerName) VALUES (@OrderNumber, @CustomerName)", _connection))
            {
                command.Parameters.AddWithValue("@OrderNumber", newOrder.OrderNumber);
                command.Parameters.AddWithValue("@CustomerName", newOrder.Customer.GetName());
                command.ExecuteNonQuery();
            }

            // Create Order products
            using (SqlCommand command = _connection.CreateCommand())
            {
                // Build a single SQL command with multiple parameter sets
                command.CommandText = "INSERT INTO OrderProducts (OrderNumber, ProductNumber) VALUES ";
                for (int i = 0; i < newOrder.Products.Count; i++)
                {
                    // Add parameters for each order
                    string orderNumberParam = $"@OrderNumber{i}";
                    string customerNameParam = $"@ProductNumber{i}";

                    command.CommandText += $"({orderNumberParam}, {customerNameParam}), ";

                    command.Parameters.AddWithValue(orderNumberParam, newOrder.OrderNumber);
                    command.Parameters.AddWithValue(customerNameParam, newOrder.Products[i].Product.GetProductNumber());
                }

                // Remove the trailing comma and execute the command
                command.CommandText = command.CommandText.TrimEnd(',', ' ');
                command.ExecuteNonQuery();
            }

            orders.Add(newOrder);
        }

        public void Remove(IOrder removedOrder)
        {
            using (SqlCommand command = new SqlCommand("DELETE FROM Orders WHERE OrderNumber = @OrderNumber", _connection))
            {
                command.Parameters.AddWithValue("@OrderNumber", removedOrder.OrderNumber);
                command.ExecuteNonQuery();
            }
            orders = orders.Where(o => !string.Equals(removedOrder.OrderNumber, o.OrderNumber)).ToList();
        }

        public List<IOrder> Get()
        {
            return orders;
        }
    }
}
