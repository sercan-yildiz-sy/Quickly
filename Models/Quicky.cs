namespace Quicky.Models;

public class Quicky 
{
	public required int Id { get; set; }
	public required string Name { get; set; }
	public required float Quantity { get; set; }
	public required string Quantity_Type { get; set; }
	public required string Image {  get; set; }
	public required string Location {  get; set; }

}
