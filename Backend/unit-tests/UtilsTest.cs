using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Xunit;
using Xunit.Sdk;
namespace WebApp;

public class UtilsTest{

    //[Fact]

    /*public void TestSumInt(){
        //Assert.Equal()
    }*/
    
    /*[Fact]
    public void TestCreateMockUsers(){
        //Read all mock users from the json file
        var read = File.ReadAllText(Path.Combine("json","mock-users.json"));
        Arr mockUsers = JSON.Parse(read);
        Arr usersInDb = SQLQuery("Select * FROM users");
        Arr emailsInDb = usersInDb.Map(user => user.email);
        //only keep the mock users not already in db
        Arr mockUserNotInDb = mockUsers.Filter( 
            mockUser => !emailsInDb.Contains(mockUser.email));
        
        var result = Utils.CreateMockUsers();

        //Log("calculated the test", mockUserNotInDb);
        //Log("reoirted by method", result);

        // assert the same length
        Assert.Equal(mockUserNotInDb.Length, result.Length);
        
        for(var i = 0; i < result.Length; i++){
            //Log("results", result[i]);
            //Log("users not in db", mockUserNotInDb[i]);
            Assert.Equal(
                JSON.Stringify(result[i]),
                JSON.Stringify(mockUserNotInDb[i]));
        }
     
    }*/
    /*
    [Theory]
    [InlineData("Aa1!")]     // Too short so should be false]
    [InlineData("Aa1aaaaa")] // Missing special characters so should be false
    [InlineData("Aa!aaaaa")] // Missing digit so should be false]
    [InlineData("AA1!AAAA")] // Missing lower character so should be false
    [InlineData("aa1!aaaa")] // Missing upper character so should be false]
    public static void TestIsPasswordDenied(string toTest){
        Assert.False(Utils.IsPasswordGoodEnough(toTest));
    }
    
    [Theory]
    [InlineData("GoodPassword123!")] //should be correctomundo
    [InlineData("ReallyGoodPassword123123!")]
    public static void TestIsPasswordAllowed(string toTest){
        Assert.True(Utils.IsPasswordGoodEnough(toTest));
    }*/
    
    [Fact]
    public void TestRemoveBadWords(){
        string unfilteredWord = "i fuck fuck fuck fuck, hello !!! zamnnn!!! damn, shit, hello, test, fuck!!!";
        string filteredWord = Utils.RemoveBadWords(unfilteredWord);
        Assert.NotEqual(unfilteredWord, filteredWord);
        Console.WriteLine(filteredWord);
        
    }
    
    [Fact]
    public void TestRemoveMockUsers(){

    }
}
