// /******************************************************************************
// * The CC0 1.0 Universal License
// * Copyright (c) 2024 FED DEV Open Source Software
// *
// * The person who associated a work with this deed has dedicated the work to
// * the public domain by waiving all of his or her rights to the work worldwide
// * under copyright law, including all related and neighboring rights, to the
// * extent allowed by law.
// *
// * You can copy, modify, distribute and perform the work, even for commercial
// * purposes, all without asking permission.
// *
// * The above copyright notice and this permission notice shall be included in
// * all copies or substantial portions of the Software.
// *
// * THE SOFTWARE IS PROVIDED AS IS, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// * SOFTWARE.
// *******************************************************************************/

using FEDDEVOSS.ActiveDirectoryHelper.Helpers;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Threading.Tasks;

namespace FEDDEVOSS.ActiveDirectoryHelper
{
    public class ADSearcher : IDisposable
    {
        /// <summary>
        /// DirectoryEntry Search Root Property
        /// This property is creating a new instance DirectoryEntry
        /// </summary>
        private readonly DirectoryEntry _entryRoot;
        private bool _disposedValue = false;

        /// <summary>
        /// Validates and Returns a LDAP Path
        /// </summary>
        /// <param name="ldapPath">LDAP Path</param>
        /// <returns>string LDAP PATH</returns>
        private static string GetLdapPath(string ldapPath)
        {
            if (string.IsNullOrEmpty(ldapPath))
            {
                throw new ArgumentNullException(nameof(ldapPath));
            }

            return ldapPath.Trim();
        }

        /// <summary>
        /// ADSearcher Constructor using only LDAP Path
        /// AuthenticationType set to ReadonlyServer, Sealing, Signing, and Secure
        /// </summary>
        /// <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.directoryservices"/>
        /// <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.directoryservices.authenticationtypes"/>
        /// <param name="ldapPath">LDAP Path</param>
        public ADSearcher(string ldapPath)
        {
            _entryRoot = new DirectoryEntry(GetLdapPath(ldapPath))
            {
                AuthenticationType = AuthenticationTypes.ReadonlyServer
                | AuthenticationTypes.Sealing
                | AuthenticationTypes.Signing
                | AuthenticationTypes.Secure
            };
        }

        /// <summary>
        /// ADSearcher Constructor using a Service Account Credentials
        /// AuthenticationType set to ReadonlyServer, Sealing, Signing, and Secure
        /// </summary>
        /// <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.directoryservices"/>
        /// <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.directoryservices.authenticationtypes"/>
        /// <param name="ldapPath">LDAP Path</param>
        /// <param name="username">Service Account Username</param>
        /// <param name="password">Service Account Password</param>
        public ADSearcher(string ldapPath, string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("Service Account Username or Password missing.");
            }

            _entryRoot = new DirectoryEntry(GetLdapPath(ldapPath), username, password)
            {
                AuthenticationType = AuthenticationTypes.ReadonlyServer
                | AuthenticationTypes.Sealing
                | AuthenticationTypes.Signing
                | AuthenticationTypes.Secure
            };
        }

        /// <summary>
        /// ADSearcher Constructor using a Service Account Credentials
        /// Allows setting PasswordPort and AuthenticationTypes
        /// </summary>
        /// <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.directoryservices"/>
        /// <see cref="https://docs.microsoft.com/en-us/dotnet/api/system.directoryservices.authenticationtypes"/>
        /// <param name="ldapPath">LDAP Path</param>
        /// <param name="username">Service Account SamAccountName</param>
        /// <param name="password">Service Account Password</param>
        /// <param name="sslPort">SSL Port - Common SSL ports 636 or 6269</param>
        /// <param name="authenticationTypes">bitwise combination of AuthenticationTypes</param>
        public ADSearcher(string ldapPath, string username, string password, int sslPort, AuthenticationTypes authenticationTypes)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("Service Account SamAccountName or Password missing.");
            }

            if (sslPort == 0)
            {
                throw new ArgumentNullException("SSL Port number missing.");
            }

            _entryRoot = new DirectoryEntry(GetLdapPath(ldapPath), username, password, authenticationTypes);

            if (sslPort > 0)
            {
                _entryRoot.Options.PasswordPort = sslPort;
            }
        }

        // NON_STATIC_METHODS
        #region NON_STATIC_METHODS

        /// <summary>
        /// Retrieve raw Active Directory Search Result information
        /// </summary>
        /// <param name="directorySearchFilter">Directory Search Filter</param>
        /// <param name="propertiesToLoad">The props to load - i.e. 'mail','surname'</param>
        /// <param name="sizeLimit">Maximum number objects to returns in a search</param>
        /// <param name="pageSize">Page size in a paged search</param>
        /// <returns>SearchResult</returns>
        public SearchResult GetSearchResult(string directorySearchFilter, string[] propertiesToLoad, int sizeLimit = 0, int pageSize = 0)
        {
            if (string.IsNullOrEmpty(directorySearchFilter)
                || !RegExHelper.IsValidAlphaNumeric(directorySearchFilter.Trim()))
            {
                return null;
            }

            using (var adSearch = new DirectorySearcher(_entryRoot))
            {
                try
                {
                    adSearch.Filter = directorySearchFilter.Trim();
                    adSearch.SizeLimit = sizeLimit;
                    adSearch.PageSize = pageSize;
                    adSearch.PropertiesToLoad.AddRange(propertiesToLoad);

                    return adSearch.FindOne();
                }
                catch (DirectoryServicesCOMException comex)
                {
                    string message = $"{Constants.Error.ActiveDirectory_Connection_Message}\n\n{comex.Message}";
                    throw new Exception(message);
                }
                catch (Exception ex)
                {
                    string message = $"{Constants.Error.ActiveDirectory_Connection_Message}\n\n{ex.Message}";
                    throw new Exception(message);
                }
            }
        }

        /// <summary>
        /// Retrieve raw Active Directory Search Result information
        /// </summary>
        /// <param name="directorySearchFilter">Directory Search Filter</param>
        /// <param name="propertiesToLoad">The props to load - i.e. 'mail','surname'</param>
        /// <param name="sizeLimit">Maximum number objects to returns in a search</param>
        /// <param name="pageSize">Page size in a paged search</param>
        /// <returns>SearchResult</returns>
        public async Task<SearchResult> GetSearchResultAsync(string directorySearchFilter, string[] propertiesToLoad, int sizeLimit = 0, int pageSize = 0)
        {
            return await Task.Run(() =>
            {
                return GetSearchResult(directorySearchFilter, propertiesToLoad, sizeLimit, pageSize);
            });
        }

        /// <summary>
        /// Retrieve raw Active Directory Seach Result Collection information
        /// </summary>
        /// <param name="directorySearchFilter">Directory Search Filter</param>
        /// <param name="propertiesToLoad">The props to load - i.e. 'mail','surname'</param>
        /// <param name="sizeLimit">Maximum number objects to returns in a search</param>
        /// <param name="pageSize">Page size in a paged search</param>
        /// <returns>SearchResultCollection</returns>
        public SearchResultCollection GetSeachResultCollection(string directorySearchFilter, string[] propertiesToLoad = null, int sizeLimit = 0, int pageSize = 0)
        {
            if (string.IsNullOrEmpty(directorySearchFilter)
                || !RegExHelper.IsValidAlphaNumeric(directorySearchFilter.Trim()))
            {
                return null;
            }

            using (var adSearch = new DirectorySearcher(_entryRoot))
            {
                try
                {
                    adSearch.Filter = directorySearchFilter.Trim();
                    adSearch.SizeLimit = sizeLimit;
                    adSearch.PageSize = pageSize;

                    if (propertiesToLoad != null)
                    {
                        adSearch.PropertiesToLoad.AddRange(propertiesToLoad);
                    }

                    return adSearch.FindAll();
                }
                catch (DirectoryServicesCOMException comex)
                {
                    string message = $"{Constants.Error.ActiveDirectory_Connection_Message}\n\n{comex.Message}";
                    throw new Exception(message);
                }
                catch (Exception ex)
                {
                    string message = $"{Constants.Error.ActiveDirectory_Connection_Message}\n\n{ex.Message}";
                    throw new Exception(message);
                }
            }
        }

        /// <summary>
        /// Retrieve raw Active Directory Seach Result Collection information
        /// </summary>
        /// <param name="directorySearchFilter">Directory Search Filter</param>
        /// <param name="propertiesToLoad">The props to load - i.e. 'mail','surname'</param>
        /// <param name="sizeLimit">Maximum number objects to returns in a search</param>
        /// <param name="pageSize">Page size in a paged search</param>
        /// <returns>SearchResultCollection</returns>
        public async Task<SearchResultCollection> GetSeachResultCollectionAsync(string directorySearchFilter, string[] propertiesToLoad = null, int sizeLimit = 0, int pageSize = 0)
        {
            return await Task.Run(() =>
            {
                return GetSeachResultCollection(directorySearchFilter, propertiesToLoad, sizeLimit, pageSize);
            });
        }

        /// <summary>
        /// Retrieve raw Active Directory information for a single User by an SamAccountName
        /// </summary>
        /// <param name="samAccountName">Active Diretory SamAccountName</param>
        /// <param name="propertiesToLoad">The props to load - i.e. 'mail','surname'</param>
        /// <param name="sizeLimit">Maximum number objects to returns in a search</param>
        /// <param name="pageSize">Page size in a paged search</param>
        /// <returns>SearchResult</returns>
        public SearchResult GetUserSearchResultBySamAccountName(string samAccountName, string[] propertiesToLoad, int sizeLimit = 0, int pageSize = 0)
        {
            if (string.IsNullOrEmpty(samAccountName)
                || !RegExHelper.IsValidAlphaNumeric(samAccountName.Trim()))
            {
                throw new Exception("Please provide a valid SamAccountName");
            }

            string filter = $"(&(objectClass=user)(sAMAccountName={samAccountName.Trim()}))";

            return GetSearchResult(filter, propertiesToLoad, sizeLimit, pageSize);
        }

        /// <summary>
        /// Retrieve raw Active Directory information for a single User by an SamAccountName
        /// </summary>
        /// <param name="samAccountName">Active Diretory SamAccountName</param>
        /// <param name="propertiesToLoad">The props to load - i.e. 'mail','surname'</param>
        /// <param name="sizeLimit">Maximum number objects to returns in a search</param>
        /// <param name="pageSize">Page size in a paged search</param>
        /// <returns>SearchResult</returns>
        public async Task<SearchResult> GetUserSearchResultBySamAccountNameAsync(string samAccountName, string[] propertiesToLoad, int sizeLimit = 0, int pageSize = 0)
        {
            return await Task.Run(() =>
            {
                return GetUserSearchResultBySamAccountName(samAccountName, propertiesToLoad, sizeLimit, pageSize);
            });
        }

        /// <summary>
        /// Get an ADUser by an SamAccountName
        /// This function will return ADUser.
        /// This takes SamAccountName as input parameter.
        /// </summary>
        /// <param name="samAccountName">Active Diretory SamAccountName</param>
        /// <param name="sizeLimit">Maximum number objects to returns in a search</param>
        /// <param name="pageSize">Page size in a paged search</param>
        /// <returns>ADUser</returns>
        public ADUser GetADUserBySamAccountName(string samAccountName, int sizeLimit = 0, int pageSize = 0)
        {
            if (string.IsNullOrEmpty(samAccountName)
                || !RegExHelper.IsValidAlphaNumeric(samAccountName.Trim()))
            {
                throw new Exception("Please provide a valid SamAccountName");
            }

            var results = GetUserSearchResultBySamAccountName(samAccountName.Trim(), ADUser.PropertiesToLoad(), sizeLimit, pageSize);

            if (results != null)
            {
                return new ADUser(results);
            }

            return null;
        }

        /// <summary>
        /// Get an ADGroup by an Active Directory DistinguishedName.
        /// </summary>
        /// <param name="distinguishedName">Active Directory DistinguishedName</param>
        /// <param name="sizeLimit">Maximum number objects to returns in a search</param>
        /// <param name="pageSize">Page size in a paged search</param>
        /// <returns>ADGroup</returns>
        public ADGroup GetADGroupByDistinguishedName(string distinguishedName, int sizeLimit = 0, int pageSize = 0)
        {
            if (string.IsNullOrEmpty(distinguishedName) || !RegExHelper.IsValidAlphaNumeric(distinguishedName.Trim()))
            {
                throw new Exception("Please provide a valid DistinguishedName");
            }

            string filter = $"(&(objectClass=group)(distinguishedName={distinguishedName.Trim()}))";

            var results = GetSearchResult(filter, ADGroup.PropertiesToLoad(), sizeLimit, pageSize);

            if (results != null)
            {
                return new ADGroup(results);
            }

            return null;
        }

        /// <summary>
        /// Get an ADGroup by an Active Directory DistinguishedName.
        /// </summary>
        /// <param name="distinguishedName">Active Directory DistinguishedName</param>
        /// <param name="sizeLimit">Maximum number objects to returns in a search</param>
        /// <param name="pageSize">Page size in a paged search</param>
        /// <returns>ADGroup</returns>
        public async Task<ADGroup> GetADGroupByDistinguishedNameAsync(string distinguishedName, int sizeLimit = 0, int pageSize = 0)
        {
            return await Task.Run(() =>
            {
                return GetADGroupByDistinguishedName(distinguishedName, sizeLimit, pageSize);
            });
        }

        /// <summary>
        /// Get an ADGroup by Active Directory group name (AKA CN)
        /// This function will return ADGroup.
        /// This takes Group Name as input parameter.
        /// </summary>
        /// <param name="groupName">Active Diretory Group Name (AKA CN)</param>
        /// <param name="sizeLimit">Maximum number objects to returns in a search</param>
        /// <param name="pageSize">Page size in a paged search</param>
        /// <returns>ADGroup</returns>
        public ADGroup GetADGroupByGroupName(string groupName, int sizeLimit = 0, int pageSize = 0)
        {
            if (string.IsNullOrEmpty(groupName)
                || !RegExHelper.IsValidAlphaNumeric(groupName.Trim()))
            {
                throw new Exception("Please provide a valid group name.");
            }

            string filter = $"(&(objectClass=group)(cn={groupName.Trim()}))";

            var results = GetSearchResult(filter, ADGroup.PropertiesToLoad(), sizeLimit, pageSize);

            if (results != null)
            {
                return new ADGroup(results);
            }

            return null;
        }

        /// <summary>
        /// Get an ADGroup by Active Directory group name (AKA CN)
        /// This function will return ADGroup.
        /// This takes Group Name as input parameter.
        /// </summary>
        /// <param name="groupName">Active Diretory Group Name (AKA CN)</param>
        /// <param name="sizeLimit">Maximum number objects to returns in a search</param>
        /// <param name="pageSize">Page size in a paged search</param>
        /// <returns>ADGroup</returns>
        public async Task<ADGroup> GetADGroupByGroupNameAsync(string groupName, int sizeLimit = 0, int pageSize = 0)
        {
            return await Task.Run(() =>
            {
                return GetADGroupByGroupName(groupName, sizeLimit, pageSize);
            });
        }

        /// <summary>
        /// Get an ADGroup by Active Directory group by SamAccountName
        /// This function will return ADGroup.
        /// This takes SamAccountName as input parameter.
        /// </summary>
        /// <param name="groupSamAccountName">Active Diretory Group SamAccountName</param>
        /// <param name="sizeLimit">Maximum number objects to returns in a search</param>
        /// <param name="pageSize">Page size in a paged search</param>
        /// <returns>ADGroup</returns>
        public ADGroup GetADGroupBySamAccountName(string groupSamAccountName, int sizeLimit = 0, int pageSize = 0)
        {
            if (string.IsNullOrEmpty(groupSamAccountName)
                || !RegExHelper.IsValidAlphaNumeric(groupSamAccountName.Trim()))
            {
                throw new Exception("Please provide a valid group SamAccountName");
            }

            string filter = $"(&(objectClass=group)(sAMAccountName={groupSamAccountName.Trim()}))";

            var results = GetSearchResult(filter, ADGroup.PropertiesToLoad(), sizeLimit, pageSize);

            if (results != null)
            {
                return new ADGroup(results);
            }

            return null;
        }

        /// <summary>
        /// Get an ADGroup by Active Directory group by SamAccountName
        /// This function will return ADGroup.
        /// This takes SamAccountName as input parameter.
        /// </summary>
        /// <param name="groupSamAccountName">Active Diretory Group SamAccountName</param>
        /// <param name="sizeLimit">Maximum number objects to returns in a search</param>
        /// <param name="pageSize">Page size in a paged search</param>
        /// <returns>ADGroup</returns>
        public async Task<ADGroup> GetADGroupBySamAccountNameAsync(string groupSamAccountName, int sizeLimit = 0, int pageSize = 0)
        {
            return await Task.Run(() =>
            {
                return GetADGroupBySamAccountName(groupSamAccountName, sizeLimit, pageSize);
            });
        }

        /// <summary>
        /// Retrieve raw Active Directory information for a single Group by group name (AKA CN)
        /// </summary>
        /// <param name="groupName">Active Diretory SamAccountName</param>
        /// <param name="propertiesToLoad">The props to load - i.e. 'mail','surname'</param>
        /// <param name="sizeLimit">Maximum number objects to returns in a search</param>
        /// <param name="pageSize">Page size in a paged search</param>
        /// <returns>SearchResult</returns>
        public SearchResult GetGroupSearchResultByGroupName(string groupName, string[] propertiesToLoad, int sizeLimit = 0, int pageSize = 0)
        {
            if (string.IsNullOrEmpty(groupName)
                || !RegExHelper.IsValidAlphaNumeric(groupName.Trim()))
            {
                throw new Exception("Please provide a valid group name");
            }

            string filter = $"(&(objectClass=group)(cn={groupName.Trim()}))";

            return GetSearchResult(filter, propertiesToLoad, sizeLimit, pageSize);
        }

        /// <summary>
        /// Retrieve raw Active Directory information for a single Group by SamAccountName
        /// </summary>
        /// <param name="groupSamAccountName">Active Diretory SamAccountName</param>
        /// <param name="propertiesToLoad">The props to load - i.e. 'mail','surname'</param>
        /// <param name="sizeLimit">Maximum number objects to returns in a search</param>
        /// <param name="pageSize">Page size in a paged search</param>
        /// <returns>SearchResult</returns>
        public SearchResult GetGroupSearchResultBySamAccountName(string groupSamAccountName, string[] propertiesToLoad, int sizeLimit = 0, int pageSize = 0)
        {
            if (string.IsNullOrEmpty(groupSamAccountName)
                || !RegExHelper.IsValidAlphaNumeric(groupSamAccountName.Trim()))
            {
                throw new Exception("Please provide a valid group SamAccountName");
            }

            string filter = $"(&(objectClass=group)(sAMAccountName={groupSamAccountName.Trim()}))";

            return GetSearchResult(filter, propertiesToLoad, sizeLimit, pageSize);
        }

        /// <summary>
        /// Get an ADUser by an SamAccountName
        /// This function will return ADUser.
        /// This takes SamAccountName as input parameter.
        /// </summary>
        /// <param name="samAccountName">Active Diretory SamAccountName</param>
        /// <param name="sizeLimit">Maximum number objects to returns in a search</param>
        /// <param name="pageSize">Page size in a paged search</param>
        /// <returns>ADUser</returns>
        public async Task<ADUser> GetADUserBySamAccountNameAsync(string samAccountName, int sizeLimit = 0, int pageSize = 0)
        {
            return await Task.Run(() =>
            {
                return GetADUserBySamAccountName(samAccountName, sizeLimit, pageSize);
            });
        }

        /// <summary>
        /// Get an ADUser by an email address.
        /// This takes an email address as input parameter.
        /// This function will return an ADUser.
        /// </summary>
        /// <param name="emailAddress">Email address</param>
        /// <param name="sizeLimit">Maximum number objects to returns in a search</param>
        /// <param name="pageSize">Page size in a paged search</param>
        /// <returns>ADUser</returns>
        public ADUser GetADUserByEmail(string emailAddress, int sizeLimit = 0, int pageSize = 0)
        {
            if (string.IsNullOrEmpty(emailAddress)
                || !RegExHelper.IsValidAlphaNumeric(emailAddress.Trim())
                || !RegExHelper.IsValidEmail(emailAddress.Trim()))
            {
                throw new Exception("Please provide a valid email address");
            }

            //string filter = $"(&(objectClass=user)(mail={emailAddress.Trim()}))";
            string filter = $"(&(objectClass=user)(proxyAddresses=smtp:{emailAddress.Trim()}))";

            var results = GetSearchResult(filter, ADUser.PropertiesToLoad(), sizeLimit, pageSize);

            if (results != null)
            {
                return new ADUser(results);
            }

            return null;
        }

        /// <summary>
        /// Get an ADUser by an email address.
        /// This takes an email address as input parameter.
        /// This function will return an ADUser.
        /// </summary>
        /// <param name="emailAddress">Email address</param>
        /// <param name="sizeLimit">Maximum number objects to returns in a search</param>
        /// <param name="pageSize">Page size in a paged search</param>
        /// <returns>ADUser</returns>
        public async Task<ADUser> GetADUserByEmailAsync(string emailAddress, int sizeLimit = 0, int pageSize = 0)
        {
            return await Task.Run(() =>
            {
                return GetADUserByEmail(emailAddress, sizeLimit, pageSize);
            });
        }

        /// <summary>
        /// Get an ADUser with Manager information by an SamAccountName
        /// This takes SamAccountName as input parameter.
        /// This function will return ADUserManager.
        /// </summary>
        /// <param name="samAccountName">Active Diretory SamAccountName</param>
        /// <param name="sizeLimit">Maximum number objects to returns in a search</param>
        /// <param name="pageSize">Page size in a paged search</param>
        /// <returns>ADUser with Manager</returns>
        public ADUserManager GetADUserManagerBySamAccountName(string samAccountName, int sizeLimit = 0, int pageSize = 0)
        {
            if (string.IsNullOrEmpty(samAccountName)
                || !RegExHelper.IsValidAlphaNumeric(samAccountName.Trim()))
            {
                throw new Exception("Please provide a valid SamAccountName");
            }

            var results = GetUserSearchResultBySamAccountName(samAccountName.Trim(), ADUserManager.ManagerPropertiesToLoad(), sizeLimit, pageSize);

            if (results != null)
            {
                var user = new ADUserManager(results);

                if (!string.IsNullOrEmpty(user.ManagerDistinguishedName))
                {
                    user.Manager = GetADUserByDistinguishedName(user.ManagerDistinguishedName);
                }

                return user;
            }

            return null;
        }

        /// <summary>
        /// Get an ADUser with Manager information by an SamAccountName
        /// This takes SamAccountName as input parameter.
        /// This function will return ADUserManager.
        /// </summary>
        /// <param name="samAccountName">Active Diretory SamAccountName</param>
        /// <param name="sizeLimit">Maximum number objects to returns in a search</param>
        /// <param name="pageSize">Page size in a paged search</param>
        /// <returns>ADUser with Manager</returns>
        public async Task<ADUserManager> GetADUserManagerBySamAccountNameAsync(string samAccountName, int sizeLimit = 0, int pageSize = 0)
        {
            return await Task.Run(() =>
            {
                return GetADUserManagerBySamAccountName(samAccountName, sizeLimit, pageSize);
            });
        }

        /// <summary>
        /// Get an ADUser with Manager information by an email address.
        /// This takes an email address as input parameter.
        /// This function will return ADUserManager.
        /// </summary>
        /// <param name="emailAddress">Email Address</param>
        /// <param name="sizeLimit">Maximum number objects to returns in a search</param>
        /// <param name="pageSize">Page size in a paged search</param>
        /// <returns>ADUser</returns>
        public ADUserManager GetADUserManagerByEmail(string emailAddress, int sizeLimit = 0, int pageSize = 0)
        {
            if (string.IsNullOrEmpty(emailAddress)
                || !RegExHelper.IsValidAlphaNumeric(emailAddress.Trim())
                || !RegExHelper.IsValidEmail(emailAddress.Trim()))
            {
                throw new Exception("Please provide a valid email address");
            }

            string filter = $"(&(objectClass=user)(proxyAddresses=smtp:{emailAddress.Trim()}))";

            var results = GetSearchResult(filter, ADUserManager.ManagerPropertiesToLoad(), sizeLimit, pageSize);

            if (results != null)
            {
                var ADUser = new ADUserManager(results);

                if (!string.IsNullOrEmpty(ADUser.ManagerDistinguishedName))
                {
                    ADUser.Manager = GetADUserByDistinguishedName(ADUser.ManagerDistinguishedName);
                }

                return ADUser;
            }

            return null;
        }

        /// <summary>
        /// Get an ADUser with Manager information by an email address.
        /// This takes an email address as input parameter.
        /// This function will return ADUserManager.
        /// </summary>
        /// <param name="emailAddress">Email Address</param>
        /// <param name="sizeLimit">Maximum number objects to returns in a search</param>
        /// <param name="pageSize">Page size in a paged search</param>
        /// <returns>ADUser</returns>
        public async Task<ADUserManager> GetADUserManagerByEmailAsync(string emailAddress, int sizeLimit = 0, int pageSize = 0)
        {
            return await Task.Run(() =>
            {
                return GetADUserManagerByEmail(emailAddress, sizeLimit, pageSize);
            });
        }

        /// <summary>
        /// Get an ADUser by an Active Directory DistinguishedName.
        /// </summary>
        /// <param name="distinguishedName">Active Directory DistinguishedName</param>
        /// <param name="sizeLimit">Maximum number objects to returns in a search</param>
        /// <param name="pageSize">Page size in a paged search</param>
        /// <returns>ADUser</returns>
        public ADUser GetADUserByDistinguishedName(string distinguishedName, int sizeLimit = 0, int pageSize = 0)
        {
            if (string.IsNullOrEmpty(distinguishedName) || !RegExHelper.IsValidAlphaNumeric(distinguishedName.Trim()))
            {
                throw new Exception("Please provide a valid DistinguishedName");
            }

            string filter = $"(&(objectClass=user)(distinguishedName={distinguishedName.Trim()}))";

            var results = GetSearchResult(filter, ADUser.PropertiesToLoad(), sizeLimit, pageSize);

            if (results != null)
            {
                return new ADUser(results);
            }

            return null;
        }

        /// <summary>
        /// Get an ADUser by an Active Directory DistinguishedName.
        /// </summary>
        /// <param name="distinguishedName">Active Directory DistinguishedName</param>
        /// <param name="sizeLimit">Maximum number objects to returns in a search</param>
        /// <param name="pageSize">Page size in a paged search</param>
        /// <returns>ADUser</returns>
        public async Task<ADUser> GetADUserByDistinguishedNameAsync(string distinguishedName, int sizeLimit = 0, int pageSize = 0)
        {
            return await Task.Run(() =>
            {
                return GetADUserByDistinguishedName(distinguishedName, sizeLimit, pageSize);
            });
        }

        /// <summary>
        /// Get a List of ADUsers by First Name and Last Name.
        /// This takes First Name and Last Name as input parameters.
        /// This function will return a List of ADUsers.
        /// </summary>
        /// <param name="firstName">First Name</param>
        /// <param name="lastName">Last Name</param>
        /// <param name="sizeLimit">Maximum number objects to returns in a search</param>
        /// <param name="pageSize">Page size in a paged search</param>
        /// <returns>List of ADUsers</returns>
        public IEnumerable<ADUser> GetADUsersByNameSearch(string firstName, string lastName, int sizeLimit = 0, int pageSize = 0)
        {
            if (string.IsNullOrEmpty(firstName))
            {
                throw new ArgumentNullException(nameof(firstName));
            }

            if (string.IsNullOrEmpty(lastName))
            {
                throw new ArgumentNullException(nameof(lastName));
            }

            var ADUsersList = new List<ADUser>();
            lastName = lastName.Trim();
            firstName = firstName.Trim();

            if (RegExHelper.IsValidAlphaNumeric(firstName) && RegExHelper.IsValidAlphaNumeric(lastName))
            {
                string filter = $"(&(objectClass=user)(sn={lastName}*)(givenName={firstName}*))";

                var results = GetSeachResultCollection(filter, ADUser.PropertiesToLoad(), sizeLimit, pageSize);

                if (results.Count > 0)
                {
                    foreach (SearchResult userSearchResult in results)
                    {
                        ADUsersList.Add(new ADUser(userSearchResult));
                    }
                }
            }

            return ADUsersList;
        }

        /// <summary>
        /// Get a List of ADUsers by First Name and Last Name.
        /// This takes First Name and Last Name as input parameters.
        /// This function will return a List of ADUsers.
        /// </summary>
        /// <param name="firstName">First Name</param>
        /// <param name="lastName">Last Name</param>
        /// <param name="sizeLimit">Maximum number objects to returns in a search</param>
        /// <param name="pageSize">Page size in a paged search</param>
        /// <returns>List of ADUsers</returns>
        public async Task<IEnumerable<ADUser>> GetADUsersByNameSearchAsync(string firstName, string lastName, int sizeLimit = 0, int pageSize = 0)
        {
            return await Task.Run(() =>
            {
                return GetADUsersByNameSearch(firstName, lastName, sizeLimit, pageSize);
            });
        }

        /// <summary>
        /// Returns a list of Active Directory group memberships' names by an User's SamAccountName.
        /// </summary>
        /// <param name="samAccountName">Active Directory SamAccountName</param>
        /// <param name="sizeLimit">Maximum number objects to returns in a search</param>
        /// <param name="pageSize">Page size in a paged search</param>
        /// <returns>List string of Active Directory group names</returns>
        public IEnumerable<string> GetGroupMembershipNames(string samAccountName, int sizeLimit = 0, int pageSize = 0)
        {
            if (string.IsNullOrEmpty(samAccountName)
                || !RegExHelper.IsValidAlphaNumeric(samAccountName.Trim()))
            {
                throw new Exception("Please provide a valid SamAccountName");
            }

            string filter = $"(&(objectClass=user)(sAMAccountName={samAccountName.Trim()}))";
            string[] propertiesToLoad = { "memberOf" };

            var result = GetSearchResult(filter, propertiesToLoad, sizeLimit, pageSize);

            List<string> groupNames = new List<string>();

            foreach (var group in result.Properties["memberOf"])
            {
                string groupDN = group.ToString();
                string groupName = ADHelper.GetCommonName(groupDN);
                groupNames.Add(groupName);
            }

            return groupNames;
        }

        /// <summary>
        /// Returns a list of Active Directory group memberships' names by an User's SamAccountName.
        /// </summary>
        /// <param name="samAccountName">Active Directory SamAccountName</param>
        /// <param name="sizeLimit">Maximum number objects to returns in a search</param>
        /// <param name="pageSize">Page size in a paged search</param>
        /// <returns>List string of Active Directory group names</returns>
        public async Task<IEnumerable<string>> GetGroupMembershipNamesAsync(string samAccountName, int sizeLimit = 0, int pageSize = 0)
        {
            return await Task.Run(() =>
            {
                return GetGroupMembershipNames(samAccountName, sizeLimit, pageSize);
            });
        }

        /// <summary>
        /// Returns a list of Active Directory Group's Members' Common Names (CNs).
        /// </summary>
        /// <param name="groupSamAccountName">Active Directory Group SamAccountName</param>
        /// <param name="sizeLimit">Maximum number objects to returns in a search</param>
        /// <param name="pageSize">Page size in a paged search</param>
        /// <returns>List string of Active Directory Group Members' Common Names (CNs)</returns>
        public IEnumerable<string> GetGroupMemberships(string groupSamAccountName, int sizeLimit = 0, int pageSize = 0)
        {
            if (string.IsNullOrEmpty(groupSamAccountName)
                || !RegExHelper.IsValidAlphaNumeric(groupSamAccountName.Trim()))
            {
                throw new Exception("Please provide a valid SamAccountName");
            }

            string filter = $"(&(objectClass=user)(sAMAccountName={groupSamAccountName.Trim()}))";
            string[] propertiesToLoad = { "memberOf" };

            var result = GetSearchResult(filter, propertiesToLoad, sizeLimit, pageSize);

            List<string> groupNames = new List<string>();

            if (result != null)
            {
                foreach (var member in result.Properties["memberOf"])
                {
                    string memberDN = member.ToString();
                    groupNames.Add(memberDN);
                }
            }

            return groupNames;
        }

        /// <summary>
        /// Returns a list of Active Directory Group's Members' Common Names (CNs).
        /// </summary>
        /// <param name="groupSamAccountName">Active Directory Group SamAccountName</param>
        /// <param name="sizeLimit">Maximum number objects to returns in a search</param>
        /// <param name="pageSize">Page size in a paged search</param>
        /// <returns>List string of Active Directory Group Members' Common Names (CNs)</returns>
        public async Task<IEnumerable<string>> GetGroupMembershipsAsync(string groupSamAccountName, int sizeLimit = 0, int pageSize = 0)
        {
            return await Task.Run(() =>
            {
                return GetGroupMemberships(groupSamAccountName, sizeLimit, pageSize);
            });
        }

        /// <summary>
        /// Returns a list of Active Directory Group's Members' Common Names (CNs).
        /// </summary>
        /// <param name="groupName">Active Directory Group Name</param>
        /// <param name="sizeLimit">Maximum number objects to returns in a search</param>
        /// <param name="pageSize">Page size in a paged search</param>
        /// <returns>List string of Active Directory Group Members' Common Names (CNs)</returns>
        public IEnumerable<string> GetGroupMembers(string groupName, int sizeLimit = 0, int pageSize = 0)
        {
            if (string.IsNullOrEmpty(groupName)
                || !RegExHelper.IsValidAlphaNumeric(groupName.Trim()))
            {
                throw new Exception("Please provide a valid group name");
            }

            string filter = $"(&(objectClass=group)(cn={groupName.Trim()}))";

            string[] propertiesToLoad = { "member" };

            var result = GetSearchResult(filter, propertiesToLoad, sizeLimit, pageSize);

            List<string> groupMembers = new List<string>();

            if (result != null && result.Properties.Contains("member"))
            {
                foreach (var member in result.Properties["member"])
                {
                    string memberDN = member.ToString();
                    groupMembers.Add(memberDN);
                }
            }

            return groupMembers;
        }

        /// <summary>
        /// Returns a list of Active Directory Group's Members' Common Names (CNs).
        /// </summary>
        /// <param name="groupName">Active Directory Group Name</param>
        /// <param name="sizeLimit">Maximum number objects to returns in a search</param>
        /// <param name="pageSize">Page size in a paged search</param>
        /// <returns>List string of Active Directory Group Members' Common Names (CNs)</returns>
        public async Task<IEnumerable<string>> GetGroupMembersAsync(string groupName, int sizeLimit = 0, int pageSize = 0)
        {
            return await Task.Run(() =>
            {
                return GetGroupMembers(groupName, sizeLimit, pageSize);
            });
        }

        /// <summary>
        /// Returns a list of Active Directory Group's Members' Names (LastName, FirstName).
        /// </summary>
        /// <param name="groupName">Active Directory Group Name</param>
        /// <param name="sizeLimit">Maximum number objects to returns in a search</param>
        /// <param name="pageSize">Page size in a paged search</param>
        /// <returns>List string of Active Directory Group Members' Names (LastName, FirstName)</returns>
        public IEnumerable<string> GetGroupMembersNames(string groupName, int sizeLimit = 0, int pageSize = 0)
        {
            if (string.IsNullOrEmpty(groupName)
                || !RegExHelper.IsValidAlphaNumeric(groupName.Trim()))
            {
                throw new Exception("Please provide a valid group name");
            }

            string filter = $"(&(objectClass=group)(cn={groupName.Trim()}))";

            string[] propertiesToLoad = { "member" };

            var result = GetSearchResult(filter, propertiesToLoad, sizeLimit, pageSize);

            List<string> groupMembersNames = new List<string>();

            if (result != null && result.Properties.Contains("member"))
            {
                foreach (var member in result.Properties["member"])
                {
                    string memberDN = member.ToString();
                    string memberName = ADHelper.GetNameFromCN(memberDN);
                    groupMembersNames.Add(memberName);
                }
            }

            return groupMembersNames;
        }

        /// <summary>
        /// Returns a list of Active Directory Group's Members' Names (LastName, FirstName).
        /// </summary>
        /// <param name="groupName">Active Directory Group Name</param>
        /// <param name="sizeLimit">Maximum number objects to returns in a search</param>
        /// <param name="pageSize">Page size in a paged search</param>
        /// <returns>List string of Active Directory Group Members' Names (LastName, FirstName)</returns>
        public async Task<IEnumerable<string>> GetGroupMembersNamesAsync(string groupName, int sizeLimit = 0, int pageSize = 0)
        {
            return await Task.Run(() =>
            {
                return GetGroupMembersNames(groupName, sizeLimit, pageSize);
            });
        }

        /// <summary>
        /// Returns a list of ADUsers for an Active Directory Group's Members
        /// </summary>
        /// <param name="groupName">Active Directory Group Name</param>
        /// <param name="sizeLimit">Maximum number objects to returns in a search</param>
        /// <param name="pageSize">Page size in a paged search</param>
        /// <returns>List of ADUsers for an Active Directory Group's Members</returns>
        /// <exception cref="Exception"></exception>
        public IEnumerable<ADUser> GetGroupADUserMembers(string groupName, int sizeLimit = 0, int pageSize = 0)
        {
            if (string.IsNullOrEmpty(groupName)
                || !RegExHelper.IsValidAlphaNumeric(groupName.Trim()))
            {
                throw new Exception("Please provide a valid group name");
            }

            string filter = $"(&(objectClass=group)(cn={groupName.Trim()}))";

            string[] propertiesToLoad = { "member" };

            var results = GetSearchResult(filter, propertiesToLoad, sizeLimit, pageSize);

            var groupMembers = new List<ADUser>();

            if (results != null && results.Properties.Contains("member"))
            {
                foreach (var member in results.Properties["member"])
                {
                    var adUser = GetADUserByDistinguishedName(member.ToString());
                    groupMembers.Add(adUser);
                }
            }

            return groupMembers;
        }

        /// <summary>
        /// Returns a list of ADUsers for an Active Directory Group's Members
        /// </summary>
        /// <param name="groupName">Active Directory Group Name</param>
        /// <param name="sizeLimit">Maximum number objects to returns in a search</param>
        /// <param name="pageSize">Page size in a paged search</param>
        /// <returns>List of ADUsers for an Active Directory Group's Members</returns>
        /// <returns></returns>
        public async Task<IEnumerable<ADUser>> GetGroupADUserMembersAsync(string groupName, int sizeLimit = 0, int pageSize = 0)
        {
            return await Task.Run(() =>
            {
                return GetGroupADUserMembers(groupName, sizeLimit, pageSize);
            });
        }

        /// <summary>
        /// Return a flag indicating a User has an Active Directory group membership
        /// </summary>
        /// <param name="groupSamAccountName">Active Directory Group SamAccountName</param>
        /// <param name="userSamAccountName">Active Directory Usre SamAccountName</param>
        /// <returns>bool</returns>
        /// <exception cref="Exception"></exception>
        public bool IsUserInGroup(string groupSamAccountName, string userSamAccountName)
        {
            if (string.IsNullOrEmpty(groupSamAccountName)
                || string.IsNullOrEmpty(userSamAccountName)
                || !RegExHelper.IsValidAlphaNumeric(groupSamAccountName.Trim())
                || !RegExHelper.IsValidAlphaNumeric(userSamAccountName.Trim()))
            {
                throw new Exception("Please provide a valid Group SamAccountName and User SamAccountName");
            }

            bool isMember = false;

            using (var adSearcher = new DirectorySearcher(_entryRoot))
            {
                adSearcher.Filter = $"(&(objectClass=group)(sAMAccountName={groupSamAccountName}))";
                adSearcher.PropertiesToLoad.Add("distinguishedName");
                var result = adSearcher.FindOne();

                if (result != null)
                {
                    string groupDN = result.Properties["distinguishedName"][0].ToString();

                    adSearcher.Filter = $"(&(memberOf:1.2.840.113556.1.4.1941:={groupDN})(objectCategory=person)(objectClass=user)(sAMAccountName={userSamAccountName}))";

                    var results = adSearcher.FindAll();

                    if (results != null && results.Count > 0)
                    {
                        isMember = true;
                    }
                }
            }

            return isMember;
        }

        /// <summary>
        /// Return a flag indicating a User has an Active Directory group membership
        /// </summary>
        /// <param name="groupSamAccountName">Active Directory Group SamAccountName</param>
        /// <param name="userSamAccountName">Active Directory Usre SamAccountName</param>
        /// <returns>bool</returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> IsUserInGroupAsync(string groupSamAccountName, string userSamAccountName)
        {
            return await Task.Run(() =>
            {
                return IsUserInGroup(groupSamAccountName, userSamAccountName);
            });
        }

        /// <summary>
        /// Retrieve raw Active Directory information for a single Computer
        /// by an Username
        /// </summary>
        /// <param name="commonName">Active Diretory Computer Common Name</param>
        /// <param name="propertiesToLoad">The props to load - i.e. 'CN','DistinguishedName','OperatingSystem'</param>
        /// <param name="sizeLimit">Maximum number objects to returns in a search</param>
        /// <param name="pageSize">Page size in a paged search</param>
        /// <returns>SearchResult</returns>
        public SearchResult GetComputerSearchResultByCN(string commonName, string[] propertiesToLoad, int sizeLimit = 0, int pageSize = 0)
        {
            if (string.IsNullOrEmpty(commonName) || !RegExHelper.IsValidAlphaNumeric(commonName))
            {
                throw new ArgumentException("Please provide a valid computer name.");
            }

            string filter = $"(&(objectClass=computer)(cn={commonName.Trim()}))";

            return GetSearchResult(filter, propertiesToLoad, sizeLimit, pageSize);
        }

        /// <summary>
        /// Retrieve raw Active Directory information for a single Computer
        /// by an Username
        /// </summary>
        /// <param name="commonName">Active Diretory Computer Common Name</param>
        /// <param name="propertiesToLoad">The props to load - i.e. 'CN','DistinguishedName','OperatingSystem'</param>
        /// <param name="sizeLimit">Maximum number objects to returns in a search</param>
        /// <param name="pageSize">Page size in a paged search</param>
        /// <returns>SearchResult</returns>
        public async Task<SearchResult> GetComputerSearchResultByCNAsync(string commonName, string[] propertiesToLoad, int sizeLimit = 0, int pageSize = 0)
        {
            return await Task.Run(() =>
            {
                return GetComputerSearchResultByCN(commonName, propertiesToLoad, sizeLimit, pageSize);
            });
        }

        /// <summary>
        /// Retrieve raw Active Directory information for a single Computer
        /// by an Active Directory Object GUID
        /// </summary>
        /// <param name="objectGuid">Active Directory Object GUID</param>
        /// <param name="propertiesToLoad">The props to load - i.e. 'CN','DistinguishedName','OperatingSystem'</param>
        /// <param name="sizeLimit">Maximum number objects to returns in a search</param>
        /// <param name="pageSize">Page size in a paged search</param>
        /// <returns>SearchResult</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public SearchResult GetComputerSearchResultByObjectGuid(string objectGuid, string[] propertiesToLoad, int sizeLimit = 0, int pageSize = 0)
        {
            if (string.IsNullOrEmpty(objectGuid))
            {
                throw new ArgumentNullException(nameof(objectGuid));
            }

            string octetString = ADHelper.ConvertGuidToOctetString(objectGuid.Trim());
            string filter = $"(&(objectClass=computer)(objectGUID={octetString}))";

            return GetSearchResult(filter, propertiesToLoad, sizeLimit, pageSize);
        }

        /// <summary>
        /// Retrieve raw Active Directory information for a single Computer
        /// by an Active Directory Object GUID
        /// </summary>
        /// <param name="objectGuid">Active Directory Object GUID</param>
        /// <param name="propertiesToLoad">The props to load - i.e. 'CN','DistinguishedName','OperatingSystem'</param>
        /// <param name="sizeLimit">Maximum number objects to returns in a search</param>
        /// <param name="pageSize">Page size in a paged search</param>
        /// <returns>SearchResult</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<SearchResult> GetComputerSearchResultByObjectGuidAsync(string objectGuid, string[] propertiesToLoad, int sizeLimit = 0, int pageSize = 0)
        {
            return await Task.Run(() =>
            {
                return GetComputerSearchResultByObjectGuid(objectGuid, propertiesToLoad, sizeLimit, pageSize);
            });
        }

        /// <summary>
        /// Get an AD Computer by an Active Directory Common Name
        /// </summary>
        /// <param name="commonName">Common Name</param>
        /// <returns>ADComputer</returns>
        public ADComputer GetAdComputerByCN(string commonName)
        {
            if (string.IsNullOrEmpty(commonName) || !RegExHelper.IsValidAlphaNumeric(commonName))
            {
                throw new ArgumentException("Please provide a valid computer name.");
            }

            string filter = $"(&(objectClass=computer)(cn={commonName.Trim()}))";
            string[] propertiesToLoad = { "cn" };

            var findOneResult = GetSearchResult(filter, propertiesToLoad);

            if (findOneResult != null)
            {
                string ldapPath = ADHelper.GetLdapPath(findOneResult.Path);

                using (var deReentry = new DirectoryEntry(ldapPath))
                {
                    using (var adResearch = new DirectorySearcher(deReentry))
                    {
                        adResearch.Filter = $"(&(objectClass=computer)(cn={commonName}))";
                        adResearch.PropertiesToLoad.AddRange(ADComputer.PropertiesToLoad());

                        if (adResearch != null)
                        {
                            findOneResult = adResearch.FindOne();
                        }
                    }
                }
            }

            return new ADComputer(findOneResult);
        }

        /// <summary>
        /// Get an AD Computer by an Active Directory Common Name
        /// </summary>
        /// <param name="commonName">Common Name</param>
        /// <returns>ADComputer</returns>
        public async Task<ADComputer> GetAdComputerByCNAsync(string commonName)
        {
            return await Task.Run(() =>
            {
                return GetAdComputerByCN(commonName);
            });
        }

        /// <summary>
        /// Get an Active Directory Computer by an Active Directory Object GUID
        /// </summary>
        /// <param name="objectGuid">ObjectGUID</param>
        /// <returns>ADComputer</returns>
        public ADComputer GetAdComputerByObjectGuid(string objectGuid)
        {
            if (string.IsNullOrEmpty(objectGuid))
            {
                throw new ArgumentNullException(nameof(objectGuid));
            }

            string octetString = ADHelper.ConvertGuidToOctetString(objectGuid.Trim());
            string filter = $"(&(objectClass=computer)(objectGUID={octetString}))";
            string[] propertiesToLoad = { "objectGUID" };

            var findOneResult = GetSearchResult(filter, propertiesToLoad);

            if (findOneResult != null)
            {
                string ldapPath = ADHelper.GetLdapPath(findOneResult.Path);

                using (var deReentry = new DirectoryEntry(ldapPath))
                {
                    using (var adResearch = new DirectorySearcher(deReentry))
                    {
                        adResearch.Filter = $"(&(objectClass=computer)(objectGUID={octetString}))";
                        adResearch.PropertiesToLoad.AddRange(ADComputer.PropertiesToLoad());

                        if (adResearch != null)
                        {
                            findOneResult = adResearch.FindOne();
                        }
                    }
                }
            }

            return new ADComputer(findOneResult);
        }

        /// <summary>
        /// Get an Active Directory Computer by an Active Directory Object GUID
        /// </summary>
        /// <param name="objectGuid">ObjectGUID</param>
        /// <returns>ADComputer</returns>
        public async Task<ADComputer> GetAdComputerByObjectGuidAsync(string objectGuid)
        {
            return await Task.Run(() =>
            {
                return GetAdComputerByObjectGuid(objectGuid);
            });
        }

        #endregion
        // END-NON_STATIC_METHODS

        #region IDisposable Support
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    // dispose managed state (managed objects)
                    _entryRoot.Dispose();
                }

                // free unmanaged resources (unmanaged objects) and override finalizer
                // set large fields to null
                _disposedValue = true;
            }
        }

        // // override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~ADSearcher()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
