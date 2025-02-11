
namespace Checkout;

public class Checkout(IItemRepository itemRepository,IDiscountRuleRepository discountRuleRepository)
{
    public int BasketCost(params string[] items)
    {
        // find all the rules that apply to this baslet
        var rules = discountRuleRepository.GetAllDiscountRules(items);
        
        var discountedItemsCost = 0;
        var standardItems = new List<string>(items);
        //remove any items from the main "basket"/items that we have already applied a discount to
        foreach (var rule in rules)
        {
            var discountedItems = new Stack<string>(rule.ItemsProcessed);
            while (discountedItems.TryPop(out var discountItem))
            {
                //remove exactly 1 of the discount items from our "basket"
                standardItems.Remove(discountItem);
            }

            discountedItemsCost += rule.CostToAdd;
        }
      
      
        return standardItems.Sum(itemRepository.GetCost) + discountedItemsCost;
    }
}

