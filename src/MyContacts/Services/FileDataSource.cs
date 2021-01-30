using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MyContacts.Interfaces;
using MyContacts.Shared.Models;
using MyContacts.Shared.Utils;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace MyContacts.Services
{
    /// <summary>
    /// This class exists mainly for isolating data during our Xamarin Test Cloud test runs, 
    /// but it can also serve as an example of how to do local storage.
    /// </summary>
    public class FileDataSource : IDataSource<Contact>
	{       
		bool isInitialized;
		List<Contact> myContacts;
        readonly string fullPath;
		public FileDataSource()
		{
            var fileName = "MyContacts.json";
            fullPath = Path.Combine(FileSystem.AppDataDirectory, fileName);
		}

		#region IDataSource implementation
		public async Task<IEnumerable<Contact>> GetItems()
		{
			await EnsureInitialized().ConfigureAwait(false);

			return await Task.FromResult(myContacts.OrderBy(x => x.LastName)).ConfigureAwait(false);
		}

		public async Task<Contact> GetItem(string id)
		{
			await EnsureInitialized().ConfigureAwait(false);

			return await Task.FromResult(myContacts.SingleOrDefault(x => x.Id == id)).ConfigureAwait(false);
		}

		public Task<bool> AddItem(Contact item)
		{
			myContacts.Add(item with {Id = Guid.NewGuid().ToString() });

            WriteFile();


            return Task.FromResult(true);
		}

		public async Task<bool> UpdateItem(Contact item)
		{
			await EnsureInitialized();

			var i = myContacts.FindIndex(a => a.Id == item.Id);

			if (i < 0)
				return false;
			
			myContacts[i] = item;

            WriteFile();

            return true;
		}

		public async Task<bool> RemoveItem(Contact item)
		{
			await EnsureInitialized();

			myContacts.RemoveAll(c => c.Id == item.Id);

            WriteFile();

            return true;
		}

        void WriteFile()
        {
            var contents = JsonConvert.SerializeObject(myContacts);
            File.WriteAllText(fullPath, contents);
            // Settings.LastUpdate = DateTime.UtcNow;
        }

		#endregion

		#region supporting methods

		void Initialize()
		{
            var contents = string.Empty;
			if (File.Exists(fullPath))
			{
                contents = File.ReadAllText(fullPath);
            }

			if (string.IsNullOrWhiteSpace(contents))
			{
				myContacts = ContactsGenerator.GenerateContacts();

                File.WriteAllText(fullPath, contents);
            }
			else
			{
				myContacts = JsonConvert.DeserializeObject<List<Contact>>(contents);
			}

			isInitialized = true;
		}

		Task EnsureInitialized()
		{
            if (!isInitialized)
                return Task.Run(() => Initialize());

            return Task.CompletedTask;
		}

		#endregion
	} 
}

