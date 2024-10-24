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
using System.DirectoryServices;

namespace FEDDEVOSS.ActiveDirectoryHelper
{
    public class ADUser
    {
        public string ObjectGUID { get; set; }
        public string ObjectSID { get; set; }
        public string EmployeeID { get; set; }
        public string EmployeeNumber { get; set; }
        public string UserAccountControl { get; set; }
        public string Domain { get; set; }
        public string FullSamAccountName { get; set; }
        public string SamAccountName { get; set; }
        public string DisplayName { get; set; }
        public string DistinguishedName { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string WorkPhone { get; set; }
        public string MobilePhone { get; set; }
        public string Title { get; set; }
        public string Office { get; set; }
        public string Department { get; set; }
        public string Company { get; set; }
        public string City { get; set; }
        public string State { get; set; }

        /// <summary>
        /// Standard Active Directory Attributes for ADUser
        /// </summary>
        /// <returns>string array of Active Directory Attributes</returns>
        public static string[] PropertiesToLoad()
        {
            return new string[] {
                Constants.ADAttributes.COMMON_NAME,
                Constants.ADAttributes.DISPLAY_NAME,
                Constants.ADAttributes.DISTINGUISHED_NAME,
                Constants.ADAttributes.OBJECT_GUID,
                Constants.ADAttributes.OBJECT_SID,
                Constants.ADAttributes.EMPLOYEE_ID,
                Constants.ADAttributes.EMPLOYEE_NUMBER,
                Constants.ADAttributes.USER_ACCOUNT_CONTROL,
                Constants.ADAttributes.USER_PRINCIPAL_NAME,
                Constants.ADAttributes.MSDS_PRINCIPAL_NAME,
                Constants.ADAttributes.EMAIL_ADDRESS,
                Constants.ADAttributes.FIRST_NAME,
                Constants.ADAttributes.LAST_NAME,
                Constants.ADAttributes.MIDDLE_NAME,
                Constants.ADAttributes.LOGIN_NAME,
                Constants.ADAttributes.TELEPHONE_NUMBER,
                Constants.ADAttributes.MOBILE_PHONE,
                Constants.ADAttributes.MIDDLE_NAME,
                Constants.ADAttributes.TITLE,
                Constants.ADAttributes.OFFICE,
                Constants.ADAttributes.DEPARTMENT,
                Constants.ADAttributes.COMPANY,
                Constants.ADAttributes.CITY,
                Constants.ADAttributes.STATE
            };
        }

        /// <summary>
        /// Cast DirectoryServices SearchResult to new ADUser
        /// </summary>
        /// <param name="entry">SearchResult to cast</param>
        /// <returns>ADUser with ADUser attributes populated</returns>
        public ADUser(SearchResult userResults)
        {
            ObjectGUID = ADHelper
                .GetAdPropertyObjectGuid(userResults, Constants.ADAttributes.OBJECT_GUID);

            ObjectSID = ADHelper
                .GetAdPropertySID(userResults, Constants.ADAttributes.OBJECT_SID);

            EmployeeID = ADHelper
                .GetAdProperty(userResults, Constants.ADAttributes.EMPLOYEE_ID);

            EmployeeNumber = ADHelper
                .GetAdProperty(userResults, Constants.ADAttributes.EMPLOYEE_NUMBER);

            UserAccountControl = ADHelper
                .GetAdProperty(userResults, Constants.ADAttributes.USER_ACCOUNT_CONTROL);

            Domain = ADHelper
                .GetAdDomain(userResults).ToUpper();

            FullSamAccountName = ADHelper
                .GetAdProperty(userResults, Constants.ADAttributes.MSDS_PRINCIPAL_NAME).ToUpper();

            SamAccountName = ADHelper
                .GetAdProperty(userResults, Constants.ADAttributes.LOGIN_NAME).ToUpper();

            DisplayName = ADHelper
                .GetAdProperty(userResults, Constants.ADAttributes.DISPLAY_NAME);

            DistinguishedName = ADHelper
                .GetAdProperty(userResults, Constants.ADAttributes.DISTINGUISHED_NAME);

            FullName = ADHelper
                .GetAdProperty(userResults, Constants.ADAttributes.COMMON_NAME);

            FirstName = ADHelper
                .GetAdProperty(userResults, Constants.ADAttributes.FIRST_NAME);

            MiddleName = ADHelper
                .GetAdProperty(userResults, Constants.ADAttributes.MIDDLE_NAME);

            LastName = ADHelper
                .GetAdProperty(userResults, Constants.ADAttributes.LAST_NAME);

            Title = ADHelper
                .GetAdProperty(userResults, Constants.ADAttributes.TITLE);

            Email = ADHelper
                .GetAdProperty(userResults, Constants.ADAttributes.EMAIL_ADDRESS);

            WorkPhone = ADHelper
                .GetAdProperty(userResults, Constants.ADAttributes.TELEPHONE_NUMBER);

            MobilePhone = ADHelper
                .GetAdProperty(userResults, Constants.ADAttributes.MOBILE_PHONE);

            Office = ADHelper
                .GetAdProperty(userResults, Constants.ADAttributes.OFFICE);

            Department = ADHelper
                .GetAdProperty(userResults, Constants.ADAttributes.DEPARTMENT);

            Company = ADHelper
                .GetAdProperty(userResults, Constants.ADAttributes.COMPANY);

            City = ADHelper
                .GetAdProperty(userResults, Constants.ADAttributes.CITY);

            State = ADHelper
                .GetAdProperty(userResults, Constants.ADAttributes.STATE);
        }
    }
}
