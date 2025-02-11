
namespace Checkout;

public class Checkout(IItemRepository itemRepository,IDiscountRuleRepository discountRuleRepository)
{
    public int BasketCost(params string[] items)
    {
        //note this is not going to work when we have multiple rules
        var rule = discountRuleRepository.GetBestMatchingRule(items);

        //remove any items from the main list that we have already applied a discount to
        var noneDiscountedItems = new List<string>(items);
        var discountedItems = new Stack<string>(rule.ItemsProcessed);
        while(discountedItems.TryPop(out var discountItem)){
            noneDiscountedItems.Remove(discountItem);
        }
        
        var discountedItemsCost = rule.CostToAdd;
      
        return noneDiscountedItems.Sum(itemRepository.GetCost) + discountedItemsCost;
    }
}

