using Xunit.Sdk;

namespace WebApp;

public static class Utils {
    public static int SumInts(int a, int b) {
        return a + b;
    }
    
    public static bool IsPasswordGoodEnough(string password){

        string specialCharacters = "!@#$%^&*()-_+=~`[]{}|;:,.<>?/";

        if(password.Length > 7 && 
        password.Any(char.IsLower) && 
        password.Any(char.IsUpper) && 
        password.Any(specialCharacters.Contains) && 
        password.Any(char.IsDigit)){
            return true;
        }
        
        return false;
    }

    public static Arr CreateMockUsers(){
        var read = File.ReadAllText(Path.Combine("json","mock-users.json"));
        Arr mockUsers = JSON.Parse(read);
        Arr successFullyWrittenUsers = Arr();
        foreach(var user in mockUsers){
            user.password = "12345678";
            var result = SQLQueryOne(@"INSERT INTO users(firstName,lastName,email,password)
            VALUES($firstName, $lastName, $email, $password)", user);
            
            if(!result.HasKey("error")){
                user.Delete("password");
                successFullyWrittenUsers.Push(user);
            }
        }
        return successFullyWrittenUsers;

    }
    
    public static string RemoveBadWords(string stringInput){
        string unfilteredString = stringInput;
        string? filteredString = null;
        string[] splitString = unfilteredString.Split(' ');

        var read = File.ReadAllText(Path.Combine("json","bad-words.json"));
        Arr badWords = JSON.Parse(read);

        foreach(string word in splitString){
           if(badWords.Contains(word.ToLower())){
            filteredString += '*' * word.Length;
            filteredString +=  ' ';
           } 
           else if(!badWords.Contains(word.ToLower())){
            filteredString += word;
            filteredString += ' ';
           }

        }
        return filteredString;
    }
}