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

using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Security.Principal;
using System.Text;

namespace FEDDEVOSS.ActiveDirectoryHelper.Helpers
{
    public static class ADHelper
    {
        /// <summary>
        /// Converts GUID string to OctetString data type
        /// </summary>
        /// <param name="objectGuid">ObjectGUID</param>
        /// <returns>OctetString</returns>
        public static string ConvertGuidToOctetString(string objectGuid)
        {
            byte[] byteGuid = new Guid(objectGuid).ToByteArray();

            var guidStringBuilder = new StringBuilder();

            foreach (byte b in byteGuid)
            {
                guidStringBuilder.Append($@"\{b:x2}");
            }

            return guidStringBuilder.ToString();
        }

        /// <summary>
        /// Get Active Directory Domain Name from msDS-PrincipalName
        /// </summary>
        /// <param name="entry">DirectoryServies DirectoryEntry</param>
        /// <returns>String of Active Directory Domain Name</returns>
        public static string GetAdDomain(DirectoryEntry entry)
        {
            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            return entry.Properties.Contains(Constants.ADAttributes.MSDS_PRINCIPAL_NAME)
                       ? entry.Properties[Constants.ADAttributes.MSDS_PRINCIPAL_NAME][0]
                       .ToString().Split('\\')[0]
                       : string.Empty;
        }

        /// <summary>
        /// Get Active Directory Domain Name from msDS-PrincipalName
        /// </summary>
        /// <param name="searchResult">DirectoryServies SearchResult</param>
        /// <returns>String of Active Directory Domain Name</returns>
        public static string GetAdDomain(SearchResult searchResult)
        {
            if (searchResult == null)
            {
                throw new ArgumentNullException(nameof(searchResult));
            }

            return searchResult.Properties.Contains(Constants.ADAttributes.MSDS_PRINCIPAL_NAME)
                ? searchResult.Properties[Constants.ADAttributes.MSDS_PRINCIPAL_NAME][0].ToString().Split('\\')[0]
                : string.Empty;
        }

        /// <summary>
        /// Get Property value from a DirectoryServices DirectoryEntry object
        /// </summary>
        /// <param name="entry">DirectoryServies DirectoryEntry</param>
        /// <param name="propertyName">AD Property Name - i.e. 'mail','surname'</param>
        /// <returns>AD Property Value</returns>
        public static string GetAdProperty(DirectoryEntry entry, string propertyName)
        {
            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            return entry.Properties.Contains(propertyName)
                       ? entry.Properties[propertyName][0].ToString()
                       : string.Empty;
        }

        /// <summary>
        /// Gets a property out of a DirectoryServices SearchResult object
        /// </summary>
        /// <param name="searchResult">DirectoryServices SearchResult</param>
        /// <param name="propertyName">AD Property Name -  - i.e. 'mail','surname'</param>
        /// <returns>AD Property Value</returns>
        public static string GetAdProperty(SearchResult searchResult, string propertyName)
        {
            if (searchResult == null)
            {
                throw new ArgumentNullException(nameof(searchResult));
            }

            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            return !string.IsNullOrWhiteSpace(propertyName)
                       && searchResult.Properties.Contains(propertyName)
                       ? searchResult.Properties[propertyName][0].ToString()
                       : string.Empty;
        }

        /// <summary>
        /// Gets a property collection out of a DirectoryServices SearchResult object
        /// </summary>
        /// <param name="searchResult">DirectoryServices SearchResult</param>
        /// <param name="propertyName">AD Property Name -  - i.e. 'member'</param>
        /// <returns>AD Property Value Collection</returns>
        public static IEnumerable<string> GetAdPropertyCollection(SearchResult searchResult, string propertyName)
        {
            if (searchResult == null)
            {
                throw new ArgumentNullException(nameof(searchResult));
            }

            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            List<string> dataCollection = new List<string>();

            if (searchResult != null && searchResult.Properties.Contains(propertyName))
            {
                foreach (var item in searchResult.Properties[propertyName])
                {
                    string dataItem = item.ToString();
                    dataCollection.Add(dataItem);
                }
            }

            return dataCollection;
        }

        /// <summary>
        /// Get Property Date value from a DirectoryServices DirectoryEntry object
        /// </summary>
        /// <param name="entry">DirectoryServices DirectoryEntry</param>
        /// <param name="propertyName">AD Property Name -  - i.e. pwdLastSet, lastLogonTimestamp</param>
        /// <returns>AD Property Date Value</returns>
        public static string GetAdPropertyDate(DirectoryEntry entry, string propertyName)
        {
            if (entry == null)
            {
                throw new ArgumentNullException(nameof(entry));
            }

            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (entry.Properties[propertyName].Value != null)
            {
                return null;
            }

            if (!entry.Properties[propertyName].Value.GetType().IsCOMObject)
            {
                return entry.Properties[propertyName].Value.ToString();
            }

            int highPart = (int)entry.Properties[propertyName]
                .Value.GetType()
                .InvokeMember("HighPart", System.Reflection.BindingFlags.GetProperty, null,
                entry.Properties[propertyName].Value, null);

            int lowPart = (int)entry.Properties[propertyName]
                .Value.GetType()
                .InvokeMember("LowPart", System.Reflection.BindingFlags.GetProperty, null,
                entry.Properties[propertyName].Value, null);

            long value = highPart * ((long)uint.MaxValue + 1) + lowPart;

            var dateLastSet = DateTime.FromFileTimeUtc(value);

            return dateLastSet.ToLocalTime().ToString();
        }

        /// <summary>
        /// Get Property Date value from DirectoryServices SearchResult object
        /// </summary>
        /// <param name="searchResult">DirectoryServices SearchResult</param>
        /// <param name="propertyName">AD Property Name -  - i.e. pwdLastSet, lastLogonTimestamp</param>
        /// <returns>AD Property Date Value</returns>
        public static string GetAdPropertyDate(SearchResult searchResult, string propertyName)
        {
            if (searchResult == null)
            {
                throw new ArgumentNullException(nameof(searchResult));
            }

            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (!searchResult.Properties.Contains(propertyName))
            {
                return null;
            }

            if (long.TryParse(searchResult.Properties[propertyName][0].ToString(), out long number))
            {
                var dateLastSet = DateTime.FromFileTimeUtc(number);

                return dateLastSet.ToLocalTime().ToString();
            }

            return null;
        }

        /// <summary>
        /// Get Property Date value from DirectoryServices SearchResult object
        /// </summary>
        /// <param name="searchResult">DirectoryServices SearchResult</param>
        /// <param name="propertyName">AD Property Name -  - i.e. pwdLastSet, lastLogonTimestamp</param>
        /// <returns>AD Property Date Value</returns>
        public static string[] GetAdPropertyEmails(SearchResult searchResult)
        {
            if (searchResult == null)
            {
                throw new ArgumentNullException(nameof(searchResult));
            }

            if (searchResult.Properties.Contains("proxyAddresses"))
            {
                List<string> emails = new List<string>();
                var properties = searchResult.Properties["proxyAddresses"];

                foreach (object item in properties)
                {
                    if (item.ToString().ToLower().Contains("smtp:"))
                    {
                        emails.Add(item.ToString().ToLower().Replace("smtp:", ""));
                    }
                }

                return emails.ToArray();
            }

            return new string[] { };
        }

        /// <summary>
        /// Get Property String value from DirectoryServices SearchResult Object GUID
        /// </summary>
        /// <param name="searchResult">DirectoryServices SearchResult</param>
        /// <param name="propertyName">AD Property Name -  - i.e. pwdLastSet, lastLogonTimestamp</param>
        /// <returns>AD Property Object GUID Value</returns>
        public static string GetAdPropertyObjectGuid(SearchResult searchResult, string propertyName)
        {
            if (searchResult == null)
            {
                throw new ArgumentNullException(nameof(searchResult));
            }

            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (searchResult.Properties.Contains(propertyName))
            {
                return new Guid((byte[])searchResult.Properties[propertyName][0]).ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// Get Property String value from DirectoryServices SearchResult ObjectSID
        ///  https://docs.microsoft.com/en-us/previous-versions/windows/it-pro/windows-2000-server/cc961625(v=technet.10)?redirectedfrom=MSDN
        /// </summary>
        /// <param name="searchResult">DirectoryServices SearchResult</param>
        /// <param name="propertyName">AD Property Name -  - i.e. pwdLastSet, lastLogonTimestamp</param>
        /// <returns>string of value of AD Property ObjectSID Value</returns>
        public static string GetAdPropertySID(SearchResult searchResult, string propertyName)
        {
            if (searchResult == null)
            {
                throw new ArgumentNullException(nameof(searchResult));
            }

            if (string.IsNullOrWhiteSpace(propertyName))
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (searchResult.Properties.Contains(propertyName))
            {
                byte[] propertyAsBytes = (byte[])searchResult.Properties[propertyName][0];
                return new SecurityIdentifier(propertyAsBytes, 0).Value.ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// Get Common Name from DistinguishedName
        /// </summary>
        /// <param name="distinguishedName">Active Directory DistinguishedName</param>
        /// <returns>String</returns>
        public static string GetCommonName(string distinguishedName)
        {
            string[] parts = distinguishedName.Split(',');

            foreach (string part in parts)
            {
                if (part.StartsWith("CN=", StringComparison.OrdinalIgnoreCase))
                {
                    return part.Substring(3);
                }
            }

            return null;
        }

        /// <summary>
        /// Return a 'DC=oit,DC=example,DC=com' type string from a 'oit.example.com' formatted String
        /// </summary>
        /// <param name="domain">The domain with period separated DC values</param>
        /// <returns>String Distinguished Name</returns>
        public static string GetDCsFromADDomain(string domain)
        {
            if (string.IsNullOrEmpty(domain))
            {
                throw new ArgumentNullException(nameof(domain));
            }

            var ldapStringBuilder = new StringBuilder();

            foreach (var token in domain.Split('.'))
            {
                ldapStringBuilder.Append($"DC={token},");
            }

            string ldapString = ldapStringBuilder.ToString();

            return ldapString.Substring(0, ldapString.Length - 1);
        }

        /// <summary>
        /// Return a 'DC=oit,DC=example,DC=com' type string from a 'CN=Public\, Joe,OU=Partners,DC=oit,DC=example,DC=com' formatted String
        /// </summary>
        /// <param name="domain">The domain with comma separated DC values</param>
        /// <returns>String of Domain Controllers</returns>
        public static string GetDCsFromDistinguishedName(string domain)
        {
            if (string.IsNullOrEmpty(domain))
            {
                throw new ArgumentNullException(nameof(domain));
            }

            var ldapStringBuilder = new StringBuilder();

            foreach (string token in domain.Split(','))
            {
                if (token.StartsWith("DC"))
                {
                    ldapStringBuilder.Append($"{token},");
                }
            }

            string ldapString = ldapStringBuilder.ToString();

            return ldapString.Substring(0, ldapString.Length - 1);
        }

        /// <summary>
        /// Return a LDAP path of 'LDAP://DC=oit,DC=example,DC=com' type string from a 'CN=Public\, Joe,OU=Partners,DC=oit,DC=example,DC=com' formatted String
        /// </summary>
        /// <param name="distinguishedName">"Friendly" name used by other applications</param>
        /// <returns>String of Domain Controllers</returns>
        public static string GetLdapPath(string distinguishedName)
        {
            if (string.IsNullOrWhiteSpace(distinguishedName))
            {
                throw new ArgumentNullException(nameof(distinguishedName));
            }

            string ldapString = GetDCsFromDistinguishedName(distinguishedName);

            return $"LDAP://{ldapString}";
        }

        /// <summary>
        /// Get Last and First Name from Active Directory Common Name
        /// </summary>
        /// <param name="adCommonName">CN=Public\, Joe,OU=Partners,DC=oit,DC=example,DC=com</param>
        /// <returns>LastName, FirstName</returns>
        public static string GetNameFromCN(string adCommonName)
        {
            if (string.IsNullOrWhiteSpace(adCommonName))
            {
                throw new ArgumentNullException(nameof(adCommonName));
            }

            string[] stringArray = adCommonName.Split(',');

            return $"{stringArray[0].Replace("CN=", "").Replace("\\", "")}, {stringArray[1]}";
        }
    }
}
