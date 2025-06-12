using System.Collections.Generic;

namespace TaskManager;

class Program
{
    static List<User> CreateUser(List<User> ListOfUsers)
    {
        bool ContinueRequest = true;
        while (ContinueRequest)
        {
            Console.Write("\n Введите имя пользователя: ");
            string UserName = Console.ReadLine()!;

            bool UserExist = false;
            foreach (User user in ListOfUsers)
            {
                if (user.Name != null &&
                    user!.Name == UserName)
                {
                    Console.WriteLine("Пользователь с таким именем уже существует");
                    UserExist = true;
                    break;
                }
            }

            Console.Write("\n Введите электронную почту пользователя: ");
            string UserEmail = Console.ReadLine()!;

            if (!UserExist)
            {
                foreach (User user in ListOfUsers)
                {
                    if (user.Email != null &&
                        user!.Email == UserEmail)
                    {
                        Console.WriteLine("Пользователь с такой электронной почтой уже существует");
                        UserExist = true;
                        break;
                    }
                }

                if (!UserExist)
                {
                    ListOfUsers.Add(new User(UserName, UserEmail));
                    Console.WriteLine("Новый пользователь успешно добавлен");
                }
            }

            string UserAnswer;
            do
            {
                Console.WriteLine("Хотите ли вы добавить нового абонента? Y/N");
                UserAnswer = Console.ReadLine()!.ToUpper();
            } while (UserAnswer != "Y" && UserAnswer != "N");
            ContinueRequest = (UserAnswer == "Y");
        }

        Console.WriteLine();
        return ListOfUsers;
    }

    static void ShowListOfUsers(List<User> ListOfUsers)
    {
        foreach (User user in ListOfUsers)
        {
            Console.WriteLine($"{user.Id}. {user.Name}, Email: {user.Email}");
        }
    }

    static void Main(string[] args)
    {
        List<User> ListOfUsers = new();

        if (ListOfUsers.Count > 0)
        {
            int currentMaxId = ListOfUsers.Max(x => x.Id);
            Task.SetNextId(currentMaxId+1);
            User.SetNextId(currentMaxId + 1);
        }

        CreateUser(ListOfUsers);
        ShowListOfUsers(ListOfUsers);
    }
}