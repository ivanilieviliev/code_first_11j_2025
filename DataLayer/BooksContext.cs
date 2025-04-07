using BusinessLayer;
using Microsoft.EntityFrameworkCore;

namespace DataLayer;
public class BooksContext : IDb<Book, string>
{
	LibraryDbContext dbContext;

	public BooksContext(LibraryDbContext dbContext)
	{
		this.dbContext = dbContext;
	}

	public void Create(Book item)
	{
		Author authorFromDb = dbContext.Authors.Find(item.Author.Id);
		if (authorFromDb != null) item.Author = authorFromDb;

		List<Genre> genres = new List<Genre>(item.Genres.Count);
		for (int i = 0; i < item.Genres.Count; ++i)
		{
			Genre genreFromDb = dbContext.Genres.Find(item.Genres[i].Id);
			if (genreFromDb != null) genres.Add(genreFromDb);
			else genres.Add(item.Genres[i]);
		}
		item.Genres = genres;

		dbContext.Books.Add(item);
		dbContext.SaveChanges();
	}


	public Book Read(string key, bool useNavigationalProperties = false, bool isReadOnly = false)
	{
		IQueryable<Book> query = dbContext.Books;
		if (useNavigationalProperties) query = query
		.Include(b => b.Author)
		.Include(b => b.Genres);

		if (isReadOnly) query = query.AsNoTrackingWithIdentityResolution();

		Book book = query.FirstOrDefault(b => b.ISBN == key);

		if (book == null) throw new ArgumentException($"Book with ISBN {key} does not exist!");

		return book;
	}

	public List<Book> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
	{
		IQueryable<Book> query = dbContext.Books;
		if (useNavigationalProperties) query = query
		.Include(b => b.Author)
		.Include(b => b.Genres);

		if (isReadOnly) query = query.AsNoTrackingWithIdentityResolution();

		return query.ToList();
	}

	public void Update(Book item, bool useNavigationalProperties = false)
	{
		Book bookFromDb = Read(item.ISBN, useNavigationalProperties);

		dbContext.Entry<Book>(bookFromDb).CurrentValues.SetValues(item);

		if (useNavigationalProperties)
		{
			Author authorFromDb = dbContext.Authors.Find(item.Author.Id);
			if (authorFromDb != null) bookFromDb.Author = authorFromDb;
			else bookFromDb.Author = item.Author;

			List<Genre> genres = new List<Genre>(item.Genres.Count);
			for (int i = 0; i < item.Genres.Count; ++i)
			{
				Genre genreFromDb = dbContext.Genres.Find(item.Genres[i].Id);
				if (genreFromDb != null) genres.Add(genreFromDb);
				else genres.Add(item.Genres[i]);
			}
			bookFromDb.Genres = genres;
		}

		dbContext.SaveChanges();
	}

	public void Delete(string key)
	{
		Book bookFromDb = Read(key);
		dbContext.Books.Remove(bookFromDb);
		dbContext.SaveChanges();
	}
}