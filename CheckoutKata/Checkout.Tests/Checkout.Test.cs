using Checkout;
using FakeItEasy;

namespace CheckoutKata;

// The customer has requested a supermarket checkout system
// Used to calculate basket totals
// The following specs apply:
//
// Item A cost 3
// Item B cost 5
// Item C cost 7
//
// Special discounts:
// 2 x A for 5
// 3 x C for 20
//
// 50% off all baskets on Fridays

public class CheckoutUnitTests
{
    private IItemRepository _mockItemRepository;

    public CheckoutUnitTests()
    {
        _mockItemRepository = A.Fake<IItemRepository>();
        A.CallTo(() => _mockItemRepository.GetCost("ItemA")).Returns(3);
        A.CallTo(() => _mockItemRepository.GetCost("ItemB")).Returns(5);
        A.CallTo(() => _mockItemRepository.GetCost("ItemC")).Returns(7);
    }
    
    [Theory]
    [InlineData("ItemA", 3)]
    [InlineData("ItemB", 5)]
    [InlineData("ItemC", 7)]
    public void ItemCostAmount_WhenNoSpecialOffers(string item, int expectedCost)
    {
        //arrange
        var checkout = new Checkout.Checkout(_mockItemRepository);
        
        //act
        var cost = checkout.BasketCost(item);
        
        //assert
        Assert.Equal(expectedCost, cost);   
    }

    [Fact]
    public void ItemCostAmount_WhenThereAreTwoAs_DiscountIsApplied()
    {
        //arrange
        var checkout = new Checkout.Checkout(_mockItemRepository);
        
        //act
        var cost = checkout.BasketCost("ItemA", "ItemA");
        
        //assert
        Assert.Equal(5, cost);
    }
}


