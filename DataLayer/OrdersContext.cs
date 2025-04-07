using BusinessLayer;
using Microsoft.EntityFrameworkCore;

namespace DataLayer;

public class OrdersContext : IDb<Order, int>
{
	private LibraryDbContext dbContext;

	public OrdersContext(LibraryDbContext dbContext)
	{
		this.dbContext = dbContext;
	}

	public void Create(Order item)
	{
		Customer customerFromDb = dbContext.Customers.Find(item.Customer.Id);

		if (customerFromDb != null) item.Customer = customerFromDb;

		List<Book> books = new List<Book>(item.Books.Count);
		for (int i = 0; i < item.Books.Count; ++i)
		{
			Book bookFromDb = dbContext.Books.Find(item.Books[i].ISBN);
			if (bookFromDb != null) books.Add(bookFromDb);
			else books.Add(item.Books[i]);
		}
		item.Books = books;

		dbContext.Orders.Add(item);
		dbContext.SaveChanges();
	}
	

	public Order Read(int key, bool useNavigationalProperties = false, bool isReadOnly = false)
	{
		IQueryable<Order> query = dbContext.Orders;
		if (useNavigationalProperties) query = query.Include(o => o.Books).Include(o => o.Customer);
		if (isReadOnly) query = query.AsNoTrackingWithIdentityResolution();

		Order order = query.FirstOrDefault(o => o.Id == key);

		if (order == null) throw new ArgumentException($"Order with id = {key} does not exist!");

		return order;
	}

	public List<Order> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
	{
		IQueryable<Order> query = dbContext.Orders;
		if (useNavigationalProperties) query = query.Include(o => o.Books).Include(o => o.Customer);
		if (isReadOnly) query = query.AsNoTrackingWithIdentityResolution();

		return query.ToList();
	}

	public void Update(Order item, bool useNavigationalProperties = false)
	{
		// III way: Lazy for programmers, bad for performance!
		// Use the same logic as in Create()!
		dbContext.Orders.Update(item);
		dbContext.SaveChanges();
	}

	public void Delete(int key)
	{
		Order orderFromDb = Read(key);
		dbContext.Orders.Remove(orderFromDb);
		dbContext.SaveChanges();
	}
}
