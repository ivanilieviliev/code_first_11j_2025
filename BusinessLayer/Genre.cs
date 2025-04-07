using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessLayer;

public class Genre
{
	[Key]
	public int Id { get; set; }

	[Required]
	[MaxLength(20, ErrorMessage = "Name must be max 20 symbols!")]
	public string Name { get; set; }

	public List<Book> Books { get; set; }

	private Genre()
	{
		
	}

	public Genre(string name)
	{
		Name = name;
		Books = new List<Book>();
	}

}
