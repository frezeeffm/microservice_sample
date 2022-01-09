using System.Text.Json.Serialization;

public class Product
{
    [JsonPropertyName("description")]
    public string Description;

    [JsonPropertyName("id")]
    public Id Id;

    [JsonPropertyName("name")]
    public string Name;
        
    [JsonPropertyName("price")]
    public Price Price;
        
    [JsonPropertyName("properties")]
    public Properties Properties;
        
    [JsonPropertyName("template")]
    public int Template;
}

public class Id
{
    [JsonPropertyName("inventory")]
    public string Inventory;
        
    [JsonPropertyName("primary")]
    public string Primary;
}

public class Price
{
    [JsonPropertyName("currency")]
    public string Currency;
        
    [JsonPropertyName("value")]
    public int Value;
}
        
public class Properties
{
    [JsonPropertyName("calories")]
    public string Calories;
        
    [JsonPropertyName("volume")]
    public string Volume;
        
    [JsonPropertyName("weight")]
    public string Weight;
}