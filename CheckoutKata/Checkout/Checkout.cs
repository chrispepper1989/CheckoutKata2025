namespace Checkout;

public class Checkout(IItemRepository itemRepository)
{
    public int BasketCost(params string[] items)
    {
        return items.Sum(itemRepository.GetCost);
        
    }
}

public interface IItemRepository
{
    int GetCost(string item);
}
