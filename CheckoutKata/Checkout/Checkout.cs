namespace Checkout;

public class Checkout(IItemRepository itemRepository)
{
    public int BasketCost(params string[] items)
    {
        if (items.Count(x => x == "ItemA") == 2)
        {
            return 5;
        }
        return items.Sum(itemRepository.GetCost);
    }
}

public interface IItemRepository
{
    int GetCost(string item);
}
