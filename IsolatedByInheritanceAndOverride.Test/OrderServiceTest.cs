using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace IsolatedByInheritanceAndOverride.Test
{
    /// <summary>
    /// OrderServiceTest 的摘要描述
    /// </summary>
    [TestFixture]
    public class OrderServiceTest
    {
        private OrderServiceForTest _Target;
        private IBookDao _BookDao;

        [SetUp]
        public void Setup()
        {
            _Target = new OrderServiceForTest();
            _BookDao = Substitute.For<IBookDao>();
            _Target.SetBookDao(_BookDao);
        }

        [Test]
        public void Test_SyncBookOrders_3_Orders_Only_2_book_order()
        {
            // hard to isolate dependency to unit test

            GivenOrders(
                new Order {Type = "Book"}, 
                new Order {Type = "CD"}, 
                new Order {Type = "Book"}
                );

            WhenSyncBookOrders();

            BookDaoShouldInsert(2);
        }

        private void WhenSyncBookOrders()
        {
            _Target.SyncBookOrders();
        }

        private void BookDaoShouldInsert(int times)
        {
            _Target._BookDao.Received(times).Insert(Arg.Is<Order>(x => x.Type == "Book"));
        }

        private void GivenOrders(params Order[] orders)
        {
            _Target.SetOrders(orders.ToList());
        }


        class OrderServiceForTest : OrderService
        {
            private List<Order> Orders;
            private IBookDao BookDao;

            public void SetBookDao(IBookDao bookDao)
            {
                BookDao = bookDao;
            }

            public override IBookDao GetBookDao()
            {
                return BookDao;
            }



            public void SetOrders(List<Order> orders)
            {
                Orders = orders;
            }

            public override List<Order> GetOrders()
            {
                return Orders;
            }


        }

    }

}