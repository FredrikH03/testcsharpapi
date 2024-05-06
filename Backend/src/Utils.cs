using Xunit.Sdk;

namespace WebApp;

public static class Utils
{
    public static int SumInts(int a, int b)
    {
        return a + b;
    }

    public static bool IsPasswordGoodEnough(string password)
    {

        string specialCharacters = "!@#$%^&*()-_+=~`[]{}|;:,.<>?/";

        if (password.Length > 7 &&
        password.Any(char.IsLower) &&
        password.Any(char.IsUpper) &&
        password.Any(specialCharacters.Contains) &&
        password.Any(char.IsDigit))
        {
            return true;
        }

        return false;
    }

    public static Arr CreateMockUsers()
    {
        var read = File.ReadAllText(Path.Combine("json", "mock-users.json"));
        Arr mockUsers = JSON.Parse(read);
        Arr successFullyWrittenUsers = Arr();
        foreach (var user in mockUsers)
        {
            user.password = "12345678";
            var result = SQLQueryOne(@"INSERT INTO users(firstName,lastName,email,password)
            VALUES($firstName, $lastName, $email, $password)", user);

            if (!result.HasKey("error"))
            {
                user.Delete("password");
                successFullyWrittenUsers.Push(user);
            }
        }
        return successFullyWrittenUsers;

    }

    public static string RemoveBadWords(string stringInput)
    {
        //partially works? idk arr doesnt work so i cant really do .Contains but idk i might be doing it wrong anyways
        string pattern = "[^a-zA-Z0-9 ]";
        string unfilteredString = stringInput;
        string filteredString = null;
        string[] splitString = unfilteredString.Split(' ');

        string read = File.ReadAllText(Path.Combine("json", "bad-words.json"));
        //Arr badWords = JSON.Parse(read);

        foreach (string word in splitString)
        {
            string removedSpecials = Regex.Replace(word, pattern, "");
            //string specials = Regex.Replace
            if (read.Contains(removedSpecials.ToLower()))
            {
                string censor = new string('*', removedSpecials.Length);
                filteredString += censor;
                filteredString += ' ';
            }
            else if (!read.Contains(removedSpecials.ToLower()))
            {
                filteredString += word;
                filteredString += ' ';
            }
            else{
                filteredString += word;
                filteredString += ' ';
            }

        }
        return filteredString;
    }

    public static Arr RemoveMockUsers()
    {

        return Arr();
    }
}