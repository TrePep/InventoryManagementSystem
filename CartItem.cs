using System;

public class CartItem
{
    public Item Item { get; set; }
    public int Quantity { get; set; }
    
    public CartItem(Item item, int quantity)
    {
        Item = item;
        Quantity = quantity;
    }

    public double TotalPrice
    {
        get { return Item.GetPrice() * Quantity; }
    }

    public string DisplayText
    {
        get { return $"{Item.GetName()} x{Quantity} - {TotalPrice:C}"; }
    }
}