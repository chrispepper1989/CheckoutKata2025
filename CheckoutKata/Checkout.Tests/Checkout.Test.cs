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
    private IDiscountRuleRepository _mockDiscountRuleRepository;
    public CheckoutUnitTests()
    {
        //arrange itemRepo
        _mockItemRepository = A.Fake<IItemRepository>();
        A.CallTo(() => _mockItemRepository.GetCost("ItemA")).Returns(3);
        A.CallTo(() => _mockItemRepository.GetCost("ItemB")).Returns(5);
        A.CallTo(() => _mockItemRepository.GetCost("ItemC")).Returns(7);
        
        //arrange discount repo
        _mockDiscountRuleRepository = A.Fake<IDiscountRuleRepository>();
        A.CallTo(() => _mockDiscountRuleRepository.GetBestMatchingRule("ItemA", "ItemA"))
            .Returns(new DiscountRule(["ItemA", "ItemA"], 5));
        A.CallTo(() => _mockDiscountRuleRepository.GetBestMatchingRule("ItemC", "ItemC", "ItemC"))
            .Returns(new DiscountRule(["ItemC", "ItemC", "ItemC"], 20));
    }
    
    [Theory]
    [InlineData("ItemA", 3)]
    [InlineData("ItemB", 5)]
    [InlineData("ItemC", 7)]
    public void ItemCostAmount_WhenNoSpecialOffers(string item, int expectedCost)
    {
        //arrange
        var checkout = new Checkout.Checkout(_mockItemRepository, _mockDiscountRuleRepository);
        
        //act
        var cost = checkout.BasketCost(item);
        
        //assert
        Assert.Equal(expectedCost, cost);   
    }

    [Fact]
    public void ItemCostAmount_WhenThereAreTwoAs_DiscountIsApplied()
    {
        //arrange
        var checkout = new Checkout.Checkout(_mockItemRepository, _mockDiscountRuleRepository);
        
        //act
        var cost = checkout.BasketCost("ItemA", "ItemA");
        
        //assert
        Assert.Equal(5, cost);
    }
    
    [Fact]
    public void ItemCostAmount_WhenThereAreThreeCs_DiscountIsApplied()
    {
        //arrange
        var checkout = new Checkout.Checkout(_mockItemRepository, _mockDiscountRuleRepository);
        
        //act
        var cost = checkout.BasketCost("ItemC", "ItemC", "ItemC");
        
        //assert
        Assert.Equal(20, cost);
    }

    [Theory]
                //Expected Cost | For Items
    [InlineData(  8, "ItemA", "ItemB")]
    [InlineData(  10, "ItemA", "ItemA", "ItemB")]
    public void BasketCostAmount_WhenMixOfDiscountsAndItems_DiscountIsApplied(int expectedCost, params string[] items)
    {
        //arrange
        var checkout = new Checkout.Checkout(_mockItemRepository, _mockDiscountRuleRepository);
        
        //act
        var cost = checkout.BasketCost(items);
        
        //assert
        Assert.Equal(expectedCost, cost);
    }
}


