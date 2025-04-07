using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Versioning;

namespace BusinessLayer;

public class Customer
{
	//[DatabaseGenerated(DatabaseGeneratedOption.None)]
	public int Id { get; set; }

	[Required]
	public string Name { get; set; }

	public int? Age { get; set; }

	public List<Order> Orders { get; set; }

	private Customer()
	{
		
	}

	public Customer(string name, int? age = null)
	{
		Age = age;
		Name = name;
		Orders = new List<Order>();
	}

}