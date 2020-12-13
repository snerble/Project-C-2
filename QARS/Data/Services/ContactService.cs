using Microsoft.EntityFrameworkCore;

using QARS.Data;
using QARS.Data.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QARS.Data.Services
{
	public class ContactServices
	{
		private AppDbContext dbContext;

		public ContactServices(AppDbContext dbcontext)
		{
			this.dbContext = dbcontext;
		}

		public async Task<List<Contact>> getContactasync()
		{
			return await dbContext.Contacts.ToListAsync();
		}

		public async Task<Contact> addcontactasync(Contact contact)
		{
			try
			{
				dbContext.Contacts.Add(contact);
				await dbContext.SaveChangesAsync();
			}
			catch (Exception)
			{
				throw;
			}
			return contact;
		}

		public async Task<Contact> updatecontactasync(Contact contact)
		{
			try
			{
				var contactexist = dbContext.Contacts.FirstOrDefault(p => p.Id == contact.Id);
				if (contactexist != null)
				{
					dbContext.Update(contact);
					await dbContext.SaveChangesAsync();
				}
			}
			catch (Exception)
			{
				throw;
			}
			return contact;
		}

		public async Task Deletecontactsasync(Contact contact)
		{
			try
			{
				dbContext.Contacts.Remove(contact);
				await dbContext.SaveChangesAsync();
			}
			catch (Exception)
			{
				throw;
			}
		}
	}
}
