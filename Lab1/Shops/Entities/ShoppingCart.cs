using Shops.Exceptions;
using Shops.Models;

namespace Shops.Entities;

public class ShoppingCart
{
    private readonly List<ShoppingCartItem> _shoppingList;

    public ShoppingCart()
    {
        _shoppingList = new List<ShoppingCartItem>();
    }

    public IReadOnlyList<ShoppingCartItem> ShoppingCartItems => _shoppingList.AsReadOnly();

    public void AddItem(ShoppingCartItem shoppingCartItem)
    {
        if (_shoppingList.Contains(shoppingCartItem))
        {
            shoppingCartItem.IncreaseProductAmount(shoppingCartItem.Amount);
        }
        else
        {
            _shoppingList.Add(shoppingCartItem);
        }
    }

    public void RemoveItem(ShoppingCartItem shoppingCartItem)
    {
        if (_shoppingList != null && !_shoppingList.Contains(shoppingCartItem))
        {
            throw ShoppingCartException.ShoppingCartHasNoSuchItem(
                shoppingCartItem.Product.Name,
                shoppingCartItem.Amount);
        }

        _shoppingList?.Remove(shoppingCartItem);
    }
}