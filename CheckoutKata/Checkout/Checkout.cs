namespace Checkout;

public class Checkout(IItemRepository itemRepository)
{
    public int BasketCost(params string[] item)
    {
        return itemRepository.GetCost(item.First());
        
    }
}

public interface IItemRepository
{
    int GetCost(string item);
}
