namespace Checkout;

public class Checkout(ItemRepository itemRepository)
{
    public int BasketCost(string item)
    {
        return itemRepository.GetCost(item);
        
    }
}

public class ItemRepository
{
    public int GetCost(string item)
    {
        return item switch
        {
            "ItemA" => 3,
            "ItemB" => 5,
            "ItemC" => 7,
            _ => 0
        };
    }
}