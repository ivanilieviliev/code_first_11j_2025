using BusinessLayer;
using Microsoft.EntityFrameworkCore;

namespace DataLayer;

public class GenresContext : IDb<Genre, int>
{
	private LibraryDbContext dbContext;

	public GenresContext(LibraryDbContext dbContext)
	{
		this.dbContext = dbContext;
	}

	public void Create(Genre item)
	{
		dbContext.Genres.Add(item);
		dbContext.SaveChanges();
	}
	

	public Genre Read(int key, bool useNavigationalProperties = false, bool isReadOnly = false)
	{
		IQueryable<Genre> query = dbContext.Genres;

		if (useNavigationalProperties) query = query.Include(g => g.Books);
		if (isReadOnly) query = query.AsNoTrackingWithIdentityResolution();

		Genre genre = query.FirstOrDefault(g => g.Id == key);

		if (genre is null) throw new ArgumentException($"Genre with id = {key} does not exist!");

		return genre;
	}

	public List<Genre> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
	{
		IQueryable<Genre> query = dbContext.Genres;

		if (useNavigationalProperties) query = query.Include(g => g.Books);
		if (isReadOnly) query = query.AsNoTrackingWithIdentityResolution();

		return query.ToList();
	}

	public void Update(Genre item, bool useNavigationalProperties = false)
	{
		Genre genreFromDb = Read(item.Id, useNavigationalProperties);

		// I way: (each property you type)
		// genreFromDb.Name = item.Name;

		// II way:
		dbContext.Entry<Genre>(genreFromDb).CurrentValues.SetValues(item);

		if (useNavigationalProperties)
		{
			List<Book> books = new List<Book>(item.Books.Count);
			for (int i = 0; i < item.Books.Count; i++)
			{
				Book bookFromDb = dbContext.Books.Find(item.Books[i].ISBN);

				if (bookFromDb is not null) books.Add(bookFromDb);
				else books.Add(item.Books[i]);
			}

			genreFromDb.Books = books;
		}

		dbContext.SaveChanges();
	}

	public void Delete(int key)
	{
		Genre genreFromDb = Read(key);
		dbContext.Genres.Remove(genreFromDb);
		dbContext.SaveChanges();
	}
}