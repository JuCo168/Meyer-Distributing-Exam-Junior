using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Data.SqlClient;

namespace InterviewTest.Returns
{
    public class ReturnRepository
    {
        private readonly SqlConnection _connection;
        private List<IReturn> returns;
        public ReturnRepository(SqlConnection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
            returns = new List<IReturn>();
        }

        public void Add(IReturn newReturn)
        {
            // Create Return
            string returnNumber = newReturn.ReturnNumber;
            string returnOrder = newReturn.OriginalOrder.OrderNumber;
            using (SqlCommand command = new SqlCommand("INSERT INTO Returns (ReturnNumber, CustomerName, OriginalOrder) VALUES (@ReturnNumber, @CustomerName, @OriginalOrder)", _connection))
            {
                command.Parameters.AddWithValue("@ReturnNumber", returnNumber);
                command.Parameters.AddWithValue("@CustomerName", newReturn.OriginalOrder.Customer.GetName());
                command.Parameters.AddWithValue("@OriginalOrder", returnOrder);
                command.ExecuteNonQuery();
            }

            // Create Return products
            using (SqlCommand command = _connection.CreateCommand())
            {
                // Build a single SQL command with multiple parameter sets
                command.CommandText = "INSERT INTO ReturnProducts (OriginalOrder, ProductNumber, ReturnNumber) VALUES ";
                for (int i = 0; i < newReturn.ReturnedProducts.Count; i++)
                {
                    ReturnedProduct returnedProduct = newReturn.ReturnedProducts[i];
                    string orderNumberParam = $"@OriginalOrder{i}";
                    string productNumberParam = $"@ProductNumber{i}";
                    string returnNumberParam = $"@ReturnNumber{i}";
                    command.CommandText += $"({orderNumberParam}, {productNumberParam}, {returnNumberParam}), ";
                    // Add parameters for each order
                    command.Parameters.AddWithValue(orderNumberParam, returnOrder);
                    command.Parameters.AddWithValue(productNumberParam, returnedProduct.OrderProduct.Product.GetProductNumber());
                    command.Parameters.AddWithValue(returnNumberParam, returnNumber);
                }

                // Remove the trailing comma and execute the command
                command.CommandText = command.CommandText.TrimEnd(',', ' ');
                command.ExecuteNonQuery();
            }
            returns.Add(newReturn);
        }

        public void Remove(IReturn removedReturn)
        {
            using (SqlCommand command = new SqlCommand("DELETE FROM Returns WHERE OriginalOrder = @OriginalOrder AND ReturnNumber = @ReturnNumber", _connection))
            {
                command.Parameters.AddWithValue("@OriginalOrder", removedReturn.OriginalOrder.OrderNumber);
                command.Parameters.AddWithValue("@ReturnNumber", removedReturn.ReturnNumber);
                command.ExecuteNonQuery();
            }
            returns = returns.Where(o => !string.Equals(removedReturn.ReturnNumber, o.ReturnNumber)).ToList();
        }

        public List<IReturn> Get()
        {
            return returns;
        }
    }
}
