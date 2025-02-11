namespace Checkout;

public record DiscountRule(string[] ItemsProcessed, int CostToAdd);

public interface IDiscountRuleRepository
{
    DiscountRule GetBestMatchingRule(params string[] items);
}