namespace Checkout;

public class Checkout(IItemRepository itemRepository)
{
    public int BasketCost(string item)
    {
        return itemRepository.GetCost(item);
        
    }
}

public interface IItemRepository
{
    int GetCost(string item);
}
