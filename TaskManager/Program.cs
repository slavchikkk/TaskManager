using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace TaskManager;

class Program
{
    static void Main(string[] args)
    {
        List<User> ListOfUsers = new();
        List<Task> ListOfTasks = new();

        if (ListOfUsers.Any())
        {
            int currentMaxIdUsers = ListOfUsers.Max(x => x.Id);
            User.SetNextId(currentMaxIdUsers + 1);
        }

        if (ListOfTasks.Any())
        {
            int currentMaxIdTasks = ListOfTasks.Max(x => x.Id);
            Task.SetNextId(currentMaxIdTasks + 1);
        }
    }
}