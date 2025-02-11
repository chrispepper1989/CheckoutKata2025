namespace Checkout;

public interface IItemRepository
{
    int GetCost(string item);
}