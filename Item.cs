using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

public class Item : INotifyPropertyChanged
{
    private string _name;
    private string _description;
    private double _price;
    private int _id;
    private int _amountAvailable;
    private int _quantity;

    public Item(string name, string description, double price, int IdTracker, int amountAvailable)
    {
        _name = name;
        _description = description;
        _price = price;
        _id = IdTracker;
        _amountAvailable = amountAvailable;
        _quantity = 0;
    }

    // Public properties for data binding
    public string ItemName 
    { 
        get => string.IsNullOrEmpty(_name) ? "Unknown Item" : _name;
        set { _name = value; OnPropertyChanged(); }
    }
    
    public string ItemDescription 
    { 
        get => string.IsNullOrEmpty(_description) ? "No description" : _description;
        set { _description = value; OnPropertyChanged(); }
    }
    
    public double ItemPrice 
    { 
        get => _price;
        set { _price = value; OnPropertyChanged(); }
    }
    
    public int ItemId 
    { 
        get => _id;
        set { _id = value; OnPropertyChanged(); }
    }
    
    public int ItemAmountAvailable 
    { 
        get => _amountAvailable;
        set { _amountAvailable = value; OnPropertyChanged(); OnPropertyChanged(nameof(StockStatus)); OnPropertyChanged(nameof(StockStatusColor)); }
    }
    
    public int ItemQuantity 
    { 
        get => _quantity;
        set { _quantity = value; OnPropertyChanged(); }
    }
    
    // Stock status properties for UI
    public string StockStatus
    {
        get
        {
            if (_amountAvailable == 0) return "OUT OF STOCK";
            if (_amountAvailable <= 5) return "LOW STOCK";
            return "IN STOCK";
        }
    }
    
    public Color StockStatusColor
    {
        get
        {
            if (_amountAvailable == 0) return Colors.Red;
            if (_amountAvailable <= 5) return Colors.Orange;
            return Colors.Green;
        }
    }

    // Legacy getter methods for compatibility
    public string GetName() => _name;
    public string GetDescription() => _description;
    public double GetPrice() => _price;
    public int GetId() => _id;
    public int GetAmountAvailable() => _amountAvailable;
    public int GetQuantity() => _quantity;

    // Legacy setter methods for compatibility
    public void SetName(string name) 
    { 
        _name = name; 
        OnPropertyChanged(nameof(ItemName)); 
    }
    
    public void SetDescription(string description) 
    { 
        _description = description; 
        OnPropertyChanged(nameof(ItemDescription)); 
    }
    
    public void SetPrice(double price) 
    { 
        _price = price; 
        OnPropertyChanged(nameof(ItemPrice)); 
    }
    
    public void SetAmountAvailable(int amountAvailable) 
    { 
        _amountAvailable = amountAvailable; 
        OnPropertyChanged(nameof(ItemAmountAvailable));
        OnPropertyChanged(nameof(StockStatus));
        OnPropertyChanged(nameof(StockStatusColor));
    }
    
    public void SetQuantity(int quantity) 
    { 
        _quantity = quantity; 
        OnPropertyChanged(nameof(ItemQuantity)); 
    }

    // INotifyPropertyChanged implementation
    public event PropertyChangedEventHandler PropertyChanged;
    
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}