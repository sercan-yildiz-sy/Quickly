using SQLite;

namespace Quicky.Models;

public class Inventory
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }
    public int ItemId { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
    public float Quantity { get; set; }
    public string Quantity_Type { get; set; }
    public string Category { get; set; }
    public string Location { get; set; }

}