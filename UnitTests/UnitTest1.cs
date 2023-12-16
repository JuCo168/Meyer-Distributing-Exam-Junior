using InterviewTest.Customers;
using InterviewTest.Orders;
using InterviewTest.Products;
using InterviewTest.Returns;
namespace UnitTests;


[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestOrder()
    {
        OrderRepository order_repo = new();
        ReturnRepository return_repo = new();
        TruckAccessoriesCustomer customer = new(order_repo, return_repo);

        HitchAdapter hitch = new();
        BedLiner bed_liner = new();

        Order order = new("Order1", customer);
        order.AddProduct(hitch);
        order.AddProduct(bed_liner);
        customer.CreateOrder(order);

        Assert.AreEqual(customer.GetTotalProfit(), customer.GetTotalSales());
        Assert.AreEqual(customer.GetTotalProfit(), hitch.GetSellingPrice() + bed_liner.GetSellingPrice());
    }

    [TestMethod]
    public void TestReturn()
    {
        OrderRepository order_repo = new();
        ReturnRepository return_repo = new();
        TruckAccessoriesCustomer customer = new(order_repo, return_repo);

        HitchAdapter hitch = new();
        BedLiner bed_liner = new();

        Order order = new("Order1", customer);
        order.AddProduct(hitch);
        order.AddProduct(bed_liner);
        customer.CreateOrder(order);

        Return rga = new("Return1", order);
        rga.AddProduct(order.Products.First());
        customer.CreateReturn(rga);

        Assert.AreEqual(customer.GetTotalReturns(), hitch.GetSellingPrice());
        Assert.AreEqual(customer.GetTotalProfit(), bed_liner.GetSellingPrice());
    }
}