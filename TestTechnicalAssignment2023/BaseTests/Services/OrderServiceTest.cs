using System.Linq;
using System.Threading;
using Base.Data;
using Base.Entities;
using Base.Models.Dtos;
using Base.Models.Responses;
using Moq;

namespace Catalog.UnitTests.Services;

public class OrderServiceTest
{
    private readonly IOrderService _orderService;

    private readonly Mock<IOrderRepository> _orderRepository;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IDbContextWrapper<BikeStoresContext>> _dbContextWrapper;
    private readonly Mock<ILogger<OrderService>> _logger;

    private readonly Order _orderSample = new Order()
    {
        OrderId = 1,
        OrderDate = DateTime.MinValue,
        OrderStatus = 3,
        CustomerId = 1,
        RequiredDate = DateTime.MinValue,
        ShippedDate = DateTime.MinValue,
        StoreId = 1,
        StaffId = 1
    };

    public OrderServiceTest()
    {
        _orderRepository = new Mock<IOrderRepository>();
        _mapper = new Mock<IMapper>();
        _dbContextWrapper = new Mock<IDbContextWrapper<BikeStoresContext>>();
        _logger = new Mock<ILogger<OrderService>>();

        var dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper.Setup(s => s.BeginTransactionAsync(CancellationToken.None)).ReturnsAsync(dbContextTransaction.Object);

        _orderService = new OrderService(_dbContextWrapper.Object, _logger.Object, _orderRepository.Object, _mapper.Object);
    }

    [Fact]
    public async Task GetOrdersByPageAsync_Success()
    {
        // arrange
        var testPageIndex = 0;
        var testPageSize = 4;
        var testTotalCount = 12;

        var pagingPaginatedOrdersSuccess = new PaginatedItem<Order>()
        {
            Content = new List<Order>()
            {
                new Order()
                {
                    OrderId = 1
                }
            },
            Count = testTotalCount,
        };

        var orderSuccess = new Order()
        {
            OrderId = 1
        };

        var orderDtoSuccess = new OrderDto()
        {
            OrderId = 1
        };

        _orderRepository.Setup(s => s.GetOrdersByPageAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize))).ReturnsAsync(pagingPaginatedOrdersSuccess);

        _mapper.Setup(s => s.Map<OrderDto>(
            It.Is<Order>(i => i.Equals(orderSuccess)))).Returns(orderDtoSuccess);

        // act
        var result = await _orderService.GetOrdersByPageAsync(testPageSize, testPageIndex);

        // assert
        result?.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
        result?.Count.Should().Be(testTotalCount);
        result?.PageIndex.Should().Be(testPageIndex);
        result?.PageSize.Should().Be(testPageSize);
    }

    [Fact]
    public async Task GetOrdersByPageAsync_Failed()
    {
        // arrange
        var testPageIndex = 1000;
        var testPageSize = 10000;

        _orderRepository.Setup(s => s.GetOrdersByPageAsync(
            It.Is<int>(i => i == testPageIndex),
            It.Is<int>(i => i == testPageSize))).Returns((Func<PaginatedItemResponse<OrderDto>>)null!);

        // act
        var result = await _orderService.GetOrdersByPageAsync(testPageSize, testPageIndex);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetOrderByIdAsync_Success()
    {
        // arrange
        var testId = 1;
        var orderSuccess = new Order()
        {
            OrderId = 1
        };

        var orderDtoSuccess = new OrderDto()
        {
            OrderId = 1
        };

        _orderRepository.Setup(s => s.GetOrderByIdAsync(It.Is<int>(i => i == testId))).ReturnsAsync(orderSuccess);
        _mapper.Setup(s => s.Map<OrderDto>(It.Is<Order>(i => i.Equals(orderSuccess)))).Returns(orderDtoSuccess);

        // act
        var result = await _orderService.GetOrderByIdAsync(testId);

        // assert
        result.Should().NotBeNull();
        result?.OrderId.Should().Be(testId);
    }

    [Fact]
    public async Task GetOrderByIdAsync_Failed()
    {
        // arrange
        var testId = 100000;
        _orderRepository.Setup(s => s.GetOrderByIdAsync(It.Is<int>(i => i.Equals(testId)))).Returns((Func<Order>)null!);

        // act
        var result = await _orderService.GetOrderByIdAsync(testId);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task CreateOrderAsync_Success()
    {
        // arrange
        var testId = 1;

        _orderRepository.Setup(s => s.CreateOrderAsync(
            It.IsAny<int?>(),
            It.IsAny<byte>(),
            It.IsAny<DateTime>(),
            It.IsAny<DateTime>(),
            It.IsAny<DateTime?>(),
            It.IsAny<int>(),
            It.IsAny<int>())).ReturnsAsync(testId);

        // act
        var result = await _orderService.CreateOrderAsync(
            _orderSample.CustomerId,
            _orderSample.OrderStatus,
            _orderSample.OrderDate,
            _orderSample.RequiredDate,
            _orderSample.ShippedDate,
            _orderSample.StoreId,
            _orderSample.StaffId);

        // assert
        result.Should().Be(testId);
    }

    [Fact]
    public async Task CreateOrderAsync_Failed()
    {
        // arrange
        int? testValue = null;

        _orderRepository.Setup(s => s.CreateOrderAsync(
            It.IsAny<int?>(),
            It.IsAny<byte>(),
            It.IsAny<DateTime>(),
            It.IsAny<DateTime>(),
            It.IsAny<DateTime?>(),
            It.IsAny<int>(),
            It.IsAny<int>())).ReturnsAsync(testValue);

        // act
        var result = await _orderService.CreateOrderAsync(
            _orderSample.CustomerId,
            _orderSample.OrderStatus,
            _orderSample.OrderDate,
            _orderSample.RequiredDate,
            _orderSample.ShippedDate,
            _orderSample.StoreId,
            _orderSample.StaffId);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task RemoveOrderAsync_Success()
    {
        // arrange
        var testId = 1;
        _orderRepository.Setup(s => s.RemoveOrderAsync(It.Is<int>(i => i == testId))).ReturnsAsync(true);

        // act
        var result = await _orderService.RemoveOrderAsync(testId);

        // assert
        result.Should().Be(true);
    }

    [Fact]
    public async Task RemoveOrderAsync_Failed()
    {
        // arrange
        var testId = 10000;
        _orderRepository.Setup(s => s.RemoveOrderAsync(It.Is<int>(i => i.Equals(testId)))).ReturnsAsync(false);

        // act
        var result = await _orderService.RemoveOrderAsync(It.Is<int>(i => i.Equals(testId)));

        // assert
        result.Should().Be(false);
    }
}
