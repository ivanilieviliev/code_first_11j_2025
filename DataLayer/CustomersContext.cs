using BusinessLayer;
using Microsoft.EntityFrameworkCore;

namespace DataLayer;

public class CustomersContext : IDb<Customer, int>
{
	private LibraryDbContext dbContext;

	public CustomersContext(LibraryDbContext dbContext)
	{
		this.dbContext = dbContext;
	}

	public void Create(Customer item)
	{
		dbContext.Customers.Add(item);
		dbContext.SaveChanges();
	}
	

	public Customer Read(int key, bool useNavigationalProperties = false, bool isReadOnly = false)
	{
		IQueryable<Customer> query = dbContext.Customers;

		if (useNavigationalProperties) query = query.Include(g => g.Orders);
		if (isReadOnly) query = query.AsNoTrackingWithIdentityResolution();

		Customer customerFromDb = query.FirstOrDefault(g => g.Id == key);

		if (customerFromDb is null) throw new ArgumentException($"Customer with id = {key} does not exist!");

		return customerFromDb;
	}

	public List<Customer> ReadAll(bool useNavigationalProperties = false, bool isReadOnly = false)
	{
		IQueryable<Customer> query = dbContext.Customers;

		if (useNavigationalProperties) query = query.Include(g => g.Orders);
		if (isReadOnly) query = query.AsNoTrackingWithIdentityResolution();

		return query.ToList();
	}

	public void Update(Customer item, bool useNavigationalProperties = false)
	{
		Customer customerFromDb = Read(item.Id, useNavigationalProperties);

		// I way: (each property you type)
		// customerFromDb.Name = item.Name;

		// II way:
		dbContext.Entry<Customer>(customerFromDb).CurrentValues.SetValues(item);

		if (useNavigationalProperties)
		{
			List<Order> orders = new List<Order>(item.Orders.Count);
			for (int i = 0; i < item.Orders.Count; i++)
			{
				Order orderFromDb = dbContext.Orders.Find(item.Orders[i].Id);

				if (customerFromDb is not null) orders.Add(orderFromDb);
				else orders.Add(item.Orders[i]);
			}

			customerFromDb.Orders = orders;
		}

		dbContext.SaveChanges();
	}

	public void Delete(int key)
	{
		Customer customerFromDb = Read(key);
		dbContext.Customers.Remove(customerFromDb);
		dbContext.SaveChanges();
	}
}