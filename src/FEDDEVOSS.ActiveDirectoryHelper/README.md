# FEDDEVOSS.ActiveDirectoryHelper

**Version: v1.0.0**

Description: A .NET Standard C# class library exposing Active Directory data. This class library can be used in any Windows .NET Standard 2.0+, .NET Framework 4.6.1+, or .NET/.NET Core 2.0+ based projects.  

## Dependencies

- [.Net Stanard 2.0](https://learn.microsoft.com/en-us/dotnet/standard/net-standard?WT.mc_id=dotnet-35129-website&tabs=net-standard-2-0 ".NET Standard versions")
- [System.DirectoryServices](https://www.nuget.org/packages/System.DirectoryServices/ "NuGet package System.DirectoryServices")

   
## Code Examples

### Retrieving Active Directory information for a Person by Username

```csharp
using (var adSearcher = new ADSearcher("GC://DC=example,DC=com"))
{
    string username = "XXXDOEJ";
    var adUser = adSearcher.GetADUserBySamAccountName(username);

    Console.WriteLine(adUser.DisplayName);
}
```

### Retrieving Active Directory information for a Person by Email addreass

```csharp
using (var adSearcher = new ADSearcher("GC://DC=example,DC=com"))
{
    string email = "John.Doe@example.com";
    var adUser = adSearcher.GetADUserByEmail(email);

    Console.WriteLine(adUser.DisplayName);
}
```

### Retrieving Active Directory information for a Person with Supervisor information

```csharp
using (var adSearcher = new ADSearcher("GC://DC=example,DC=com"))
{
    string username = "XXXDOEJ";
    var adUser = adSearcher.GetADUserManagerBySamAccountName(username);

    Console.WriteLine(adUser.DisplayName);
    Console.WriteLine(adUser.Manager.DisplayName);
}
```

### Retrieving Active Directory information for a Person using Partial First and Last Name search

```csharp
using (var adSearcher = new ADSearcher("GC://DC=example,DC=com"))
{
    string firstName = "jan";
    string lastName = "doe";

    var adUsers = adSearcher.GetADUsersByNameSearch(firstName, lastName).ToList();

    ADUser firstPerson = adUsers[0];

    Console.WriteLine(firstPerson.DisplayName);
}
```

### Retrieving custom Active Directory information for a Person using Partial First and Last Name search

```csharp
using (var adSearcher = new ADSearcher("GC://DC=example,DC=com"))
{
    string firstName = "jane";
    string lastName = "doe";

    string filter = $"(&(objectClass=user)(sn={lastName.Trim()}*)(givenName={firstName.Trim()}*))";
    string[] propertiesToLoad = ["distinguishedName", "mail", "displayName"];

    var results = adSearcher.GetSeachResultCollection(filter, propertiesToLoad);

    // string firstDisplayName = results[0].Properties["displayName"][0].ToString();
    string firstDisplayName = ADHelper.GetAdProperty(results, "displayName");

    Console.WriteLine(firstDisplayName);
}
```
