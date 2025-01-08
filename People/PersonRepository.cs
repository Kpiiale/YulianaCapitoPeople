using SQLite;
using People.Models;

namespace People;

public class PersonRepository
{
    string _dbPath;

    public string StatusMessage { get; set; }

    private SQLiteAsyncConnection connYC;

    private async Task Init()
    {
        if (connYC != null)
            return;
        connYC = new SQLiteAsyncConnection(_dbPath);
        await connYC.CreateTableAsync<Person>();
    }

    public PersonRepository(string dbPath)
    {
        _dbPath = dbPath;
    }

    public async Task AddNewPerson(string name)
    {
        int result = 0;
        try
        {
            await Init();

            // basic validation to ensure a name was entered
            if (string.IsNullOrEmpty(name))
                throw new Exception("Valid name required");

            // TODO: Insert the new person into the database
            result = await connYC.InsertAsync(new Person { Name = name });

            StatusMessage = string.Format("{0} record(s) added (Name: {1})", result, name);
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to add {0}. Error: {1}", name, ex.Message);
        }

    }

    public async Task<List<Person>> GetAllPeople()
    {
        // TODO: Init then retrieve a list of Person objects from the database into a list
        try
        {
            await Init();
            return await connYC.Table<Person>().ToListAsync();
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
        }

        return new List<Person>();
    }
    public async Task DeletePerson(Person person)
    {
        try
        {
            await Init();
            if (person == null)
                throw new Exception("Ingresar persona válida");
            int result = await connYC.DeleteAsync(person);
            StatusMessage = string.Format("{0} record(s) deleted (ID: {1})", result, person.Id);
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("No se pudo borrar", ex.Message);
        }
    }
}