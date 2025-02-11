using Checkout;

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
    [Theory]
    [InlineData("ItemA", 3)]
    [InlineData("ItemB", 5)]
    [InlineData("ItemC", 7)]
    public void ItemCostAmount(string item, int expectedCost)
    {
        //arrange
        var checkout = new Checkout.Checkout(new ItemRepository());
        
        //act
        var cost = checkout.BasketCost(item);
        
        //assert
        Assert.Equal(expectedCost, cost);   
    }
}


