
using System.ComponentModel.DataAnnotations;

namespace BusinessLayer;

public class Author
{
	[Key]
	public int Id { get; set; }

	[Required]
	[MaxLength(40, ErrorMessage = "Name cannot be more than 50 symbols!")]
	public string Name { get; set; }

	public List<Book> Books { get; set; }

	private Author()
	{
	    
	}

	public Author(string name)
	{
		Name = name;
		Books = new List<Book>();
	}

}
