using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessLayer;

public class Order
{
	[Key]
	public int Id { get; set; }

	public DateTime Occurred { get; set; }

	public DateTime ReturnDate { get; set; }

	[Required]
	public Customer Customer { get; set; }

	[ForeignKey("Customer")]
	public int CustomerId { get; set; }

	[Required]
	public List<Book> Books { get; set; }

	private Order()
	{
		
	}

	public Order(DateTime returnDate, Customer customer, List<Book> books)
	{
		Occurred = DateTime.Now;
		ReturnDate = returnDate;
		Customer = customer;
		Books = books;
	}

}