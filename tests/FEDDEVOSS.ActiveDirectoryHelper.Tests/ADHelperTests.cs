using FEDDEVOSS.ActiveDirectoryHelper.Helpers;
using System.Diagnostics;
using System.DirectoryServices;
using System.Runtime.Versioning;

namespace FEDDEVOSS.ActiveDirectoryHelper.Tests
{
    [TestClass]
    public class ADHelperTests
    {
        /// <summary>
        /// List of Users for testing
        /// </summary>
        private static List<TestUser> TestUsers
        {
            get
            {
                var testUser = new TestUser()
                {
                    FirstName = "Joe",
                    LastName = "Public",
                    Username = "XXXPUBLICJ",
                    Email = @"Joe.Public@example.com",
                    DistinguishedName = @"CN=Public\, Joe,OU=Partners,DC=oit,DC=example,DC=com"
                };

                return [testUser];
            }
        }

        /// <summary>
        /// ADSearcher instance property used by all tests
        /// </summary>
        private static ADSearcher TestADSearcher { get; set; }
        /// <summary>
        /// Create resources for Test Class
        /// see: https://msdn.microsoft.com/en-us/library/microsoft.visualstudio.testtools.unittesting.classinitializeattribute.aspx
        /// </summary>
        /// <param name="testContext"></param>
        [ClassInitialize()]
        [SupportedOSPlatform("windows")]
        public static void ClassInit(TestContext testContext)
        {
            string ldapPath = "GC://DC=example,DC=com";

#if !TESTING_USING_LDAP_PATH
            // Testing using only LDAP Path
            TestADSearcher = new ADSearcher(ldapPath);
#elif TESTING_USING_SVC_ACCOUNT
            // Testing using a Service Account
            // NOTE: To test, need to provide Service Account information.
            string username = @" "; // LDAP Service Account username
            string password = " "; // LDAP Service Account password
            TestADSearcher = new ADSearcher(ldapPath, username, password);
#elif TESTING_USING_SVC_ACCOUNT_AUTH_TYPES
            // Testing using a Service Account and AuthenticationTypes
            // NOTE: To test, need to provide Service Account information.
            string username = @" "; // LDAP Service Account username
            string password = " "; // LDAP Service Account password
            TestADSearcher = new ADSearcher(ldapPath, username, password, 3269, AuthenticationTypes.Sealing | AuthenticationTypes.Signing | AuthenticationTypes.Secure);
#endif
        }

        /// <summary>
        /// Cleans resources for Test Class
        /// </summary>
        [ClassCleanup()]
        public static void ClassCleanup()
        {
            TestADSearcher.Dispose();
        }

        [TestMethod]
        public void Test_GetADUserBySamAccountName()
        {
            try
            {
                string username = TestUsers[0].Username;

                var adUser = TestADSearcher.GetADUserBySamAccountName(username);

                Assert.IsNotNull(adUser, "Result is NULL");
                Assert.AreNotEqual(adUser.DistinguishedName, "", "adUser.DistinguishedName is IsNullOrEmpty");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Assert
                    .Fail(string
                    .Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }

        [TestMethod]
        public void Test_GetADUserManagerBySamAccountName()
        {
            try
            {
                string username = TestUsers[0].Username;

                var adUser = TestADSearcher.GetADUserManagerBySamAccountName(username);

                Assert.IsNotNull(adUser, "Result is NULL");
                Assert.AreNotEqual(adUser.DistinguishedName, "", "adUser.DistinguishedName is IsNullOrEmpty");

            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Assert
                    .Fail(string
                    .Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }

        [TestMethod]
        public void Test_GetADUserByDistinguishedName()
        {
            try
            {
                string distinguishedName = TestUsers[0].DistinguishedName;

                var adUser = TestADSearcher.GetADUserByDistinguishedName(distinguishedName);

                Assert.IsNotNull(adUser, "Result is NULL");
                Assert.AreNotEqual(adUser.DistinguishedName, "", "adUser.DistinguishedName is IsNullOrEmpty");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Assert
                    .Fail(string
                    .Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }

        [TestMethod]
        public void Test_GetSearchResultBySamAccountName()
        {
            try
            {
                string username1 = TestUsers[0].Username;
                string[] props = ["sn", "givenName", "mail"];

                var result1 = TestADSearcher.GetUserSearchResultBySamAccountName(username1, props);

                Assert.IsNotNull(result1, "Result is NULL");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Assert
                    .Fail(string
                    .Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }

        [TestMethod]
        public void Test_GetSearchResult()
        {
            try
            {
                string username = TestUsers[0].Username;
                string filter = $"(&(objectClass=user)(sAMAccountName={username}))";
                string[] props = ["sn", "givenName", "pwdLastSet", "lastLogonTimestamp", "proxyAddresses"];

                var result = TestADSearcher.GetSearchResult(filter, props);

                var pwdLastSet = ADHelper.GetAdPropertyDate(result, "pwdLastSet");
                var lastLogonTimestamp = ADHelper.GetAdPropertyDate(result, "lastLogonTimestamp");

                Assert.IsNotNull(pwdLastSet, "Result is NULL");
                Assert.IsNotNull(lastLogonTimestamp, "Result is NULL");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Assert
                    .Fail(string
                    .Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }

        [TestMethod]
        public void Test_GetSearchResult_Emails()
        {
            try
            {
                string email = TestUsers[0].Email;
                string filter = $"(&(objectClass=user)(mail={email}))";
                string[] props = ["sn", "givenName", "mail", "proxyAddresses"];

                var result = TestADSearcher.GetSearchResult(filter, props);

                string[] emails = ADHelper.GetAdPropertyEmails(result);

                Assert.IsNotNull(emails, "Result is NULL");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Assert
                    .Fail(string
                    .Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }

        [TestMethod]
        [SupportedOSPlatform("windows")]
        public void Test_GetSeachResultCollection()
        {
            try
            {
                string firstName = TestUsers[0].FirstName;
                string lastName = TestUsers[0].LastName;

                string filter = $"(&(objectClass=user)(sn={lastName.Trim()}*)(givenName={firstName.Trim()}*))";
                string[] props = ["sn", "givenName", "pwdLastSet", "lastLogonTimestamp"];

                var collectionResult = TestADSearcher.GetSeachResultCollection(filter, props);

                var date = ADHelper.GetAdPropertyDate(collectionResult[0], "pwdLastSet");

                Assert.IsNotNull(collectionResult, "Result is NULL");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Assert
                    .Fail(string
                    .Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }

        [TestMethod]
        public void Test_GetADUserByEmail()
        {
            try
            {
                string email = TestUsers[0].Email;
                var adUser = TestADSearcher.GetADUserByEmail(email);

                Assert.IsNotNull(adUser, "Result is NULL");
                Assert.AreNotEqual(adUser.DistinguishedName, "", "adUser.DistinguishedName is IsNullOrEmpty");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Assert
                    .Fail(string
                    .Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }

        [TestMethod]
        public void Test_GetADUsersByNameSearch_True()
        {
            try
            {
                string firstName = TestUsers[0].FirstName;
                string lastName = TestUsers[0].LastName;
                var adUsers = TestADSearcher.GetADUsersByNameSearch(firstName, lastName);

                Assert.AreNotEqual(0, adUsers.Count());
                Assert.AreNotEqual(adUsers.FirstOrDefault().DistinguishedName, "", "adUser.DistinguishedName is IsNullOrEmpty");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Assert
                    .Fail(string
                    .Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }

        [TestMethod]
        public void Test_GetADUsersByNameSearch_Fail()
        {
            try
            {
                string firstName = "testFirstName";
                string lastName = "testLastName";
                var adUsers = TestADSearcher.GetADUsersByNameSearch(firstName, lastName);

                Assert.AreEqual(0, adUsers.Count());
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Assert
                    .Fail(string
                    .Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }
        [TestMethod]
        public void Test_GetADUserManagerByEmail()
        {
            try
            {
                string email = TestUsers[0].Email;

                var adUser = TestADSearcher.GetADUserManagerByEmail(email);

                Assert.IsNotNull(adUser, "Result is NULL");
                Assert.AreNotEqual(adUser.DistinguishedName, "", "adUser.DistinguishedName is IsNullOrEmpty");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Assert
                    .Fail(string
                    .Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }

        [TestMethod]
        public void Test_HasGroupMembershipByUsername()
        {
            try
            {
                string username = TestUsers[0].Username;

                string groupName = "IT Web Team";

                bool results = TestADSearcher.IsUserInGroup(groupName, username);

                Assert.IsTrue(results, "Result was FALSE when expect result should be TRUE.");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Assert
                    .Fail(string
                    .Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }

        [TestMethod]
        public void Test_GetADGroupByCN()
        {
            try
            {
                string groupName = "IT Web Team";
                var adGroup = TestADSearcher.GetADGroupByGroupName(groupName);

                Assert.IsNotNull(adGroup, "Result is NULL");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Assert
                    .Fail(string
                    .Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }

        [TestMethod]
        public void Test_GetGroupSearchResultByCN()
        {
            try
            {
                string groupName = "IT Web Team";
                string[] propertiesToLoad = { "canonicalName", "cn", "distinguishedName", "groupCategory", "groupScope", "managedBy", "member", "SamAccountName" };

                var adGroup = TestADSearcher.GetGroupSearchResultByGroupName(groupName, propertiesToLoad);

                Assert.IsNotNull(adGroup, "Result is NULL");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Assert
                    .Fail(string
                    .Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }

        [TestMethod]
        public void Test_GetGroupSearchResultBySamAccountName()
        {
            try
            {
                string groupName = "IT Web Team";
                string[] propertiesToLoad = { "canonicalName", "cn", "distinguishedName", "groupCategory", "groupScope", "managedBy", "member", "SamAccountName" };

                var adGroup = TestADSearcher.GetGroupSearchResultBySamAccountName(groupName, propertiesToLoad);

                Assert.IsNotNull(adGroup, "Result is NULL");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Assert
                    .Fail(string
                    .Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }

        [TestMethod]
        public void Test_GetGroupMemberships()
        {
            try
            {
                string username = TestUsers[0].Username;
                List<string> groups = TestADSearcher.GetGroupMemberships(username).ToList();

                Assert.AreNotEqual(0, groups.Count);
                Assert.IsNotNull(groups, "Result is NULL");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Assert
                    .Fail(string
                    .Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }


        [TestMethod]
        public void Test_GetGroupMembers()
        {
            try
            {
                string groupName = "IT Web Team";
                List<string> members = TestADSearcher.GetGroupMembers(groupName).ToList();

                Assert.IsNotNull(members, "Result is NULL");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Assert
                    .Fail(string
                    .Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }

        [TestMethod]
        public void Test_GetGroupADUserMembers()
        {
            try
            {
                string groupName = "IT Web Team";
                var members = TestADSearcher.GetGroupADUserMembers(groupName).ToList();

                Assert.IsNotNull(members, "Result is NULL");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Assert
                    .Fail(string
                    .Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }

        [TestMethod]
        public void Test_GetAdComputerByCn()
        {
            try
            {
                string computerCn = "SERVER1234";
                var adComputer = TestADSearcher.GetAdComputerByCN(computerCn);

                Assert.IsNotNull(adComputer, "Result is NULL");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Assert
                    .Fail(string
                    .Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }

        [TestMethod]
        public void Test_GetAdComputerByObjectGuid()
        {
            try
            {
                string computerObjectGuid = "3199f815-a337-4e5a-9d56-944da22cf46d";
                var adComputer = TestADSearcher.GetAdComputerByObjectGuid(computerObjectGuid);

                Assert.IsNotNull(adComputer, "Result is NULL");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                Assert
                    .Fail(string
                    .Format("Unexpected exception of type {0} caught: {1}", e.GetType(), e.Message));
            }
        }
    }
}
