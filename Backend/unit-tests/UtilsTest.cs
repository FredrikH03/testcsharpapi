using System.Data.Common;
using System.Diagnostics.Contracts;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using Xunit;
using Xunit.Sdk;
namespace WebApp;

public class UtilsTest
{

    //[Fact]

    /*public void TestSumInt(){
        //Assert.Equal()
    }*/

    [Fact]
    public void TestCreateMockUsers()
    {
        //Read all mock users from the json file
        var read = File.ReadAllText(Path.Combine("json", "mock-users.json"));
        Arr mockUsers = JSON.Parse(read);
        Arr usersInDb = SQLQuery("Select * FROM users");
        Arr emailsInDb = usersInDb.Map(user => user.email);
        //only keep the mock users not already in db
        Arr mockUserNotInDb = mockUsers.Filter(
            mockUser => !emailsInDb.Contains(mockUser.email));

        var result = Utils.CreateMockUsers();

        Assert.Equal(mockUserNotInDb.Length, result.Length);


    }

    [Theory]
    [InlineData("Aa1!")]     // Too short so should be false]
    [InlineData("Aa1aaaaa")] // Missing special characters so should be false
    [InlineData("Aa!aaaaa")] // Missing digit so should be false]
    [InlineData("AA1!AAAA")] // Missing lower character so should be false
    [InlineData("aa1!aaaa")] // Missing upper character so should be false]
    public static void TestIsPasswordDenied(string toTest)
    {
        Assert.False(Utils.IsPasswordGoodEnough(toTest));
    }

    [Theory]
    [InlineData("GoodPassword123!")] //should be correctomundo
    [InlineData("ReallyGoodPassword123123!")]
    public static void TestIsPasswordAllowed(string toTest)
    {
        Assert.True(Utils.IsPasswordGoodEnough(toTest));
    }

    [Fact]
    public void TestRemoveBadWords()
    {
        string unfilteredSentence = "chink!, i fuck!! shell, hell?";
        string filteredSentence = Utils.RemoveBadWords(unfilteredSentence);
        Assert.Equal("*****!, i ****!! shell, ****?", filteredSentence);
        Log("filtered sentence: " + filteredSentence);

    }

    [Fact]
    public void TestCountDomainsFromUserEmails()
    {
        Obj emailDomainsCountedAndSorted = Utils.CountDomainsFromUserEmails();
        //Log(emailDomainsCountedAndSorted);
        var query = SQLQuery(@"SELECT SUBSTR(email, INSTR(email, '@') + 1) AS domain,
        COUNT(*) AS count FROM users GROUP BY domain ORDER BY count DESC;");

        //Log(query);

        var dbDomainCounts = new Obj();
        foreach (var email in query)
        {
            //Console.WriteLine(email);
            dbDomainCounts[$"{email.domain}"] = email.count;
        }
        //Log(dbDomainCounts);

        foreach (var domain in emailDomainsCountedAndSorted.GetKeys())
        {
            //Log("domain from function " + domain + " " + emailDomainsCountedAndSorted[domain]);
            //Log("domain from DB " + domain + " " + dbDomainCounts[domain]);

            string compareFunctionDomain = domain + " " + emailDomainsCountedAndSorted[domain];
            string compareDbDomain = domain + " " + dbDomainCounts[domain];
            //Console.WriteLine(compareFunctionDomain);
            //Console.WriteLine(compareDbDomain);

            Assert.Equal(compareFunctionDomain, compareDbDomain);
        }

    }


/* 
    [Fact]
    public void TestRemoveMockUsers()
    {
        var read = File.ReadAllText(Path.Combine("json", "mock-users.json"));
        Arr mockUsers = JSON.Parse(read);
        Arr usersInDb = SQLQuery("Select * FROM users");
        Arr emailsInDb = usersInDb.Map(user => user.email);
        Arr mockUserInDb = mockUsers.Filter(
            mockUser => emailsInDb.Contains(mockUser.email));

        var result = Utils.DeleteMockUsers();

        Assert.Equal(mockUserInDb.Length, result.Length);
    } */
}

