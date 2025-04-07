using BusinessLayer;
using Microsoft.EntityFrameworkCore;

namespace DataLayer;

public class AuthorsContext : IDb<Author, int>
{
	private LibraryDbContext dbContext;

	public AuthorsContext(LibraryDbContext dbContext)
	{
		this.dbContext = dbContext;
	}

	public void Create(Author item)
	{
		dbContext.Authors.Add(item);
		dbContext.SaveChanges();
	}
	

	public Author Read(int key, bool useNavigationalProperties = false, bool isReadOnly = false)
	{
		IQueryable<Author> query = dbContext.Authors;

		if (useNavigationalProperties) query = query.Include(g => g.Books);
		if (isReadOnly) query = query.AsNoTrackingWithIdentityResolution();

		Author author = query.FirstOrDefault(g => g.Id == key);

		if (author is null) throw new ArgumentException($"Author with id = {key} does not exist!");

		return author;
	}

	public List<Author> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
	{
		IQueryable<Author> query = dbContext.Authors;

		if (useNavigationalProperties) query = query.Include(g => g.Books);
		if (isReadOnly) query = query.AsNoTrackingWithIdentityResolution();

		return query.ToList();
	}

	public void Update(Author item, bool useNavigationalProperties = false)
	{
		Author authorFromDb = Read(item.Id, useNavigationalProperties);

		// I way: (each property you type)
		// authorFromDb.Name = item.Name;
		if (useNavigationalProperties)
		{
			List<Book> books = new List<Book>();
			for (int i = 0; i < item.Books.Count; i++)
			{
				Book bookFromDb = dbContext.Books.Find(item.Books[i].ISBN);

				if (bookFromDb is not null) books.Add(bookFromDb);
				else books.Add(item.Books[i]);
			}
			authorFromDb.Books = books;
		}

		dbContext.SaveChanges();
	}

	public void Delete(int key)
	{
		Author authorFromDb = Read(key);
		dbContext.Authors.Remove(authorFromDb);
		dbContext.SaveChanges();
	}
}