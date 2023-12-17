using System;
using System.Linq;
using InterviewTest.Customers;
using InterviewTest.Orders;
using InterviewTest.Products;
using InterviewTest.Returns;

namespace InterviewTest
{
    public class Program
    {
        // private static readonly OrderRepository orderRepo = new OrderRepository();
        // private static readonly ReturnRepository returnRepo = new ReturnRepository();

        static void Main(string[] args)
        {
            // ------------------------
            // Coding Challenge Requirements
            // ------------------------


            // ------------------------
            // Code Implementations
            // ------------------------
            // 1: Implement get total sales, returns, and profit in the CustomerBase class.
            // 2: Record when an item was purchased.


            // ------------------------
            // Bug fixes
            // ------------------------
            // ~~ Run the console app after implementing the Code Changes section above! ~~
            // 1: Meyer Truck Equipment's returns are not being processed.
            // 2: Ruxer Ford Lincoln, Inc.'s totals are incorrect.
            

            // ------------------------
            // Bonus
            // ------------------------
            // 1: Create unit tests for the ordering and return process.
            // 2: Create a database and refactor all repositories to save/update/pull from it.

            ProcessTruckAccessoriesExample();

            ProcessCarDealershipExample();

            // while(true);

            Console.ReadKey();
        }

        private static void ProcessTruckAccessoriesExample()
        {
            var customer = GetTruckAccessoriesCustomer();

            IOrder order = new Order("TruckAccessoriesOrder123", customer);
            order.AddProduct(new HitchAdapter());
            order.AddProduct(new BedLiner());
            customer.CreateOrder(order);

            IReturn rga = new Return("TruckAccessoriesReturn123", order);
            rga.AddProduct(order.Products.First());
            customer.CreateReturn(rga);

            ConsoleWriteLineResults(customer);
        }

        private static void ProcessCarDealershipExample()
        { 
            var customer = GetCarDealershipCustomer();

            IOrder order = new Order("CarDealerShipOrder123", customer);
            order.AddProduct(new ReplacementBumper());
            order.AddProduct(new SyntheticOil());
            customer.CreateOrder(order);

            IReturn rga = new Return("CarDealerShipReturn123", order);
            rga.AddProduct(order.Products.First());
            customer.CreateReturn(rga);

            ConsoleWriteLineResults(customer);
        }

        private static ICustomer GetTruckAccessoriesCustomer()
        {
            OrderRepository orderRepo = new OrderRepository();
            ReturnRepository returnRepo = new ReturnRepository();
            return new TruckAccessoriesCustomer(orderRepo, returnRepo);
        }

        private static ICustomer GetCarDealershipCustomer()
        {
            OrderRepository orderRepo = new OrderRepository();
            ReturnRepository returnRepo = new ReturnRepository();
            return new CarDealershipCustomer(orderRepo, returnRepo);
        }

        private static void ConsoleWriteLineResults(ICustomer customer)
        {
            Console.WriteLine(customer.GetName());

            Console.WriteLine($"Total Sales: {customer.GetTotalSales().ToString("c")}");

            Console.WriteLine($"Total Returns: {customer.GetTotalReturns().ToString("c")}");

            Console.WriteLine($"Total Profit: {customer.GetTotalProfit().ToString("c")}");

            Console.WriteLine();
        }
    }
}
