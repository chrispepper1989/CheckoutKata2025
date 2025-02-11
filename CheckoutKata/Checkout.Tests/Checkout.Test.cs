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

/*  Chris Notes:
 *  ------------
 *  This is fun so I will probably contine, some thoughts on what would happen next
 *  - 50% off all baskets on Fridays: This will expand the IDiscountRuleRespository a little
 *  -  as we dont have a "percentage" rule. We could fudge this in the first test, but it will start to break with the mixed discount tests
 * - The question is does the 50% "compound" i.e. to we apply it to all products, included already discounted ones, or just the none discounted
 *    -- i.e. is ItemA, ItemA now 2.5 or 1.5
 *    -- equally how does the system handle decimals, does it round to the nearest pound. round down etc :) 
 *   
 */
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
        A.CallTo(() => _mockDiscountRuleRepository.GetAllDiscountRules("ItemA", "ItemA"))
            .Returns([ new DiscountRule(["ItemA", "ItemA"], 5)]);
        
        A.CallTo(() => _mockDiscountRuleRepository.GetAllDiscountRules("ItemC", "ItemC", "ItemC"))
            .Returns([new DiscountRule(["ItemC", "ItemC", "ItemC"], 20)]);
        
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
    [InlineData(  15, "ItemA", "ItemB", "ItemC")] 
    [InlineData(  27, "ItemB", "ItemC", "ItemC", "ItemB", "ItemA")] 
    public void BasketCostAmount_WhenMixItems_SumIsCorrect(int expectedCost, params string[] items)
    {
        //arrange
        var checkout = new Checkout.Checkout(_mockItemRepository, _mockDiscountRuleRepository);
        
        //act
        var cost = checkout.BasketCost(items);
        
        //assert
        Assert.Equal(expectedCost, cost);
    }
    
          
    
    // Item A cost 3
    // Item B cost 5
    // Item C cost 7
    //
    // Special discounts:
    // 2 x A for 5

    [Theory]
    //Expected Cost | For Items
    [InlineData(  10, "ItemA", "ItemA", "ItemB")] //itemA discount + ItemB
    [InlineData(  22, "ItemA", "ItemA", "ItemB", "ItemB", "ItemC")] 
    [InlineData(  8, "ItemA", "ItemA", "ItemA")] // itemA discount with an extra A
    public void BasketCostAmount_WhenMixOfItemADiscountAndItems_DiscountIsAppliedAndAddedToSum(int expectedCost, params string[] items)
    {
        //arrange
        var checkout = new Checkout.Checkout(_mockItemRepository, _mockDiscountRuleRepository);
        
        //special rules (note we could use matching aboeat some point)
        A.CallTo(() => _mockDiscountRuleRepository.GetAllDiscountRules(items))
            .Returns([new DiscountRule(["ItemA", "ItemA"], 5)]);
        
        //act
        var cost = checkout.BasketCost(items);
        
        //assert
        Assert.Equal(expectedCost, cost);
    }
    
    [Theory]
    //Expected Cost | For Items
    [InlineData(  25, "ItemA", "ItemA", "ItemC", "ItemC", "ItemC")] 
    public void BasketCostAmount_WhenBothItemAAndItemCDiscountIsPresent_AddsBoth(int expectedCost, params string[] items)
    {
        //arrange
        var checkout = new Checkout.Checkout(_mockItemRepository, _mockDiscountRuleRepository);
        
        //special rules (note we could use matching at some some point)
        A.CallTo(() => _mockDiscountRuleRepository.GetAllDiscountRules(items))
            .Returns(
                [new DiscountRule(["ItemA", "ItemA"], 5),
                new DiscountRule(["ItemC", "ItemC", "ItemC"], 20)]);
     
        //act
        var cost = checkout.BasketCost(items);
        
        //assert
        Assert.Equal(expectedCost, cost);
    }
}


