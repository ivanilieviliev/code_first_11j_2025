using System.ComponentModel.DataAnnotations;

namespace BusinessLayer;

public class Book
{
	[Key]
	[StringLength(13, MinimumLength = 13, ErrorMessage = "ISBN must be exactly 13 symbols!")]
	public string ISBN { get; set; }

	[Required]
	[MaxLength(80)]
	public string Title { get; set; }

	[Range(5, 10_000, ErrorMessage = "Pages must be in [5;10_000]")]
	public int? Pages { get; set; }

	[Required]
	public Author Author { get; set; }

	[Required]
	public List<Genre> Genres { get; set; }

	private Book()
	{
		
	}

	public Book(string isbn, string title, Author author, List<Genre> genres, int? pages = 0)
	{
		ISBN = isbn;
		Title = title;
		Author = author;
		Genres = genres;
		Pages = pages;
	}

}