using System.Collections.Generic;

namespace TaskManager;

class Program
{
    static void Main(string[] args)
    {
        List<User> ListOfUsers = new();

        if (ListOfUsers.Count > 0)
        {
            int currentMaxId = ListOfUsers.Max(x => x.Id);
            Task.SetNextId(currentMaxId+1);
            User.SetNextId(currentMaxId + 1);
        }
    }
}