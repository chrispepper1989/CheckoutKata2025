namespace Checkout;

public class Checkout
{
    public int BasketCost(string item)
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