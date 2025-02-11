namespace Checkout;

public class Checkout(IItemRepository itemRepository,IDiscountRuleRepository discountRuleRepository)
{
    public int BasketCost(params string[] items)
    {
        var rule = discountRuleRepository.GetBestMatchingRule(items);
        var nonDiscountItems = items.Except(rule.ItemsProcessed);
        var discountedItemsCost = rule.CostToAdd;
      
        return nonDiscountItems.Sum(itemRepository.GetCost) + discountedItemsCost;
    }
}

public interface IDiscountRuleRepository
{
    IDiscountRule GetBestMatchingRule(params string[] items);
}

public interface IDiscountRule
{
    string[] ItemsProcessed { get; }
    int CostToAdd { get; }
}

public interface IItemRepository
{
    int GetCost(string item);
}
