using System.Transactions;
using InterviewTest.Customers;
using InterviewTest.Orders;
using InterviewTest.Products;
using InterviewTest.Returns;
using Microsoft.Data.SqlClient;
namespace UnitTests;


[TestClass]
public class UnitTest1
{

    private static string connectionString = "Server=localhost,1433;Database=InterviewTest;User Id=SA;Password=StrongPassw0rd;Encrypt=False;";
    // private static SqlConnection connection = new SqlConnection(connectionString);

    [TestMethod]
    public void TestOrder()
    {
        string orderNumber = "Order1";
        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            try
            {
                OrderRepository order_repo = new(connection);
                ReturnRepository return_repo = new(connection);
                TruckAccessoriesCustomer customer = new(order_repo, return_repo);

                HitchAdapter hitch = new();
                BedLiner bed_liner = new();


                Order order = new(orderNumber, customer);
                order.AddProduct(hitch);
                order.AddProduct(bed_liner);
                customer.CreateOrder(order);

                Assert.AreEqual(customer.GetTotalProfit(), customer.GetTotalSales());
                Assert.AreEqual(customer.GetTotalProfit(), hitch.GetSellingPrice() + bed_liner.GetSellingPrice());
            }
            finally
            {
                using (SqlCommand command = new SqlCommand("DELETE FROM Orders WHERE OrderNumber = @OrderNumber", connection))
                {
                    command.Parameters.AddWithValue("@OrderNumber", orderNumber);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
    }

    [TestMethod]
    public void TestReturn()
    {
        string orderNumber = "Order1";
        string returnNumber = "Return1";

        using (var connection = new SqlConnection(connectionString))
        {
            connection.Open();
            try
            {
                OrderRepository order_repo = new(connection);
                ReturnRepository return_repo = new(connection);
                TruckAccessoriesCustomer customer = new(order_repo, return_repo);

                HitchAdapter hitch = new();
                BedLiner bed_liner = new();

                Order order = new(orderNumber, customer);
                order.AddProduct(hitch);
                order.AddProduct(bed_liner);
                customer.CreateOrder(order);

                Return rga = new(returnNumber, order);
                rga.AddProduct(order.Products.First());
                customer.CreateReturn(rga);

                Assert.AreEqual(customer.GetTotalReturns(), hitch.GetSellingPrice());
                Assert.AreEqual(customer.GetTotalProfit(), bed_liner.GetSellingPrice());
            }
            finally
            {
                using (SqlCommand command = new SqlCommand("DELETE FROM Orders WHERE OrderNumber = @OrderNumber; DELETE FROM Returns WHERE ReturnNumber = @ReturnNumber", connection))
                {
                    command.Parameters.AddWithValue("@OrderNumber", orderNumber);
                    command.Parameters.AddWithValue("@ReturnNumber", returnNumber);
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }
    }
}