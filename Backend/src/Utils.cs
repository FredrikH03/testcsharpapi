using Xunit.Sdk;

namespace WebApp;

public static class Utils
{
    private static readonly Arr badWords = ((Arr)JSON.Parse(
        File.ReadAllText(FilePath("json", "bad-words.json"))
    )).Sort((a, b) => ((string)b).Length - ((string)a).Length);
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

        public static string RemoveBadWords(string sentence, string replaceWith = "---")
    {
        //Arr badWords = JSON.Parse(File.ReadAllText(Path.Combine("json", "bad-words.json")));
        //Log(badWords);
        
        sentence = " " + sentence;
        badWords.ForEach(bad =>
        {
            string badWord = bad;
            string censorSymbolString = new string('*', badWord.Length);
            string stringcompletedCensor = " " + censorSymbolString + "$1";

            var pattern = @$" {bad}([\,\.\!\?\:\; ])";
            sentence = Regex.Replace(
                sentence, pattern, stringcompletedCensor, RegexOptions.IgnoreCase);
        });
        return sentence[1..];
    }

    public static Arr DeleteMockUsers()
    {
        var read = File.ReadAllText(Path.Combine("json", "mock-users.json"));
        Arr mockUsers = JSON.Parse(read);
        Arr successfullyDeletedUsers = Arr();
        foreach (var user in mockUsers)
        {

            var result = SQLQueryOne(@"DELETE FROM users WHERE email = $email", user);

            if (!result.HasKey("error"))
            {
                user.Delete("password");
                successfullyDeletedUsers.Push(user);
            }
        }
        return successfullyDeletedUsers;

    }
    
    public static Obj CountDomainsFromUserEmails(){
    
        Obj domainCount = Obj();
        Arr usersInDb = SQLQuery("SELECT * FROM users");
        Arr emailsInDb = usersInDb.Map(user => user.email);
        
        foreach(string email in emailsInDb){
            string domain = email.Split('@')[1];
            if(!domainCount.HasKey(domain)){
                domainCount[$"{domain}"] = 0;
            }
            if(domainCount.HasKey(domain)){
                domainCount[$"{domain}"]++;
            }
        }
        
        var domainCountArr = domainCount.GetKeys().Map(x => Obj(new{domain = x, count = domainCount[x]}));
        domainCountArr.Sort((a,b) => b.count - a.count);
        var domainCountSorted = Obj();
        domainCountArr.ForEach(x => domainCountSorted[x.domain] = x.count);
        
        return domainCountSorted;
    }
}