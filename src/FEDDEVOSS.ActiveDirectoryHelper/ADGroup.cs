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
using System.Collections.Generic;
using System.DirectoryServices;

namespace FEDDEVOSS.ActiveDirectoryHelper
{
    public class ADGroup
    {
        public string CanonicalName { get; set; }
        public string CN { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public string DistinguishedName { get; set; }
        public string GroupCategory { get; set; }
        public string GroupScope { get; set; }
        public string ManagedBy { get; set; }
        public IEnumerable<string> Members { get; set; }
        public string ObjectGUID { get; set; }
        public string ObjectSID { get; set; }
        public string SamAccountName { get; set; }

        /// <summary>
        /// Standard Active Directory Attributes for ADUser
        /// </summary>
        /// <returns>string array of Active Directory Attributes</returns>
        public static string[] PropertiesToLoad()
        {
            return new string[] {
                Constants.ADAttributes.CANONICAL_NAME,
                Constants.ADAttributes.COMMON_NAME,
                Constants.ADAttributes.DESCRIPTION,
                Constants.ADAttributes.DISPLAY_NAME,
                Constants.ADAttributes.DISTINGUISHED_NAME,
                Constants.ADAttributes.GROUP_CATEGORY,
                Constants.ADAttributes.GROUP_SCOPE,
                Constants.ADAttributes.MANAGED_BY,
                Constants.ADAttributes.MEMBER,
                Constants.ADAttributes.OBJECT_GUID,
                Constants.ADAttributes.OBJECT_SID,
                Constants.ADAttributes.LOGIN_NAME
            };
        }

        /// <summary>
        /// Cast DirectoryServices SearchResult to new ADGroup
        /// </summary>
        /// <param name="entry">SearchResult to cast</param>
        /// <returns>ADGroup with ADGroup attributes populated</returns>
        public ADGroup(SearchResult groupResults)
        {
            CanonicalName = ADHelper
                .GetAdProperty(groupResults, Constants.ADAttributes.CANONICAL_NAME);

            CN = ADHelper
                .GetAdProperty(groupResults, Constants.ADAttributes.COMMON_NAME);

            Description = ADHelper
                .GetAdProperty(groupResults, Constants.ADAttributes.DESCRIPTION);

            DisplayName = ADHelper
                .GetAdProperty(groupResults, Constants.ADAttributes.DISPLAY_NAME);

            DistinguishedName = ADHelper
                .GetAdProperty(groupResults, Constants.ADAttributes.DISTINGUISHED_NAME);

            GroupCategory = ADHelper
                .GetAdProperty(groupResults, Constants.ADAttributes.GROUP_CATEGORY);

            GroupScope = ADHelper
                .GetAdProperty(groupResults, Constants.ADAttributes.GROUP_SCOPE);

            ManagedBy = ADHelper
                .GetAdProperty(groupResults, Constants.ADAttributes.MANAGED_BY);

            Members = ADHelper.GetAdPropertyCollection(groupResults, Constants.ADAttributes.MEMBER);

            ObjectGUID = ADHelper
                .GetAdPropertyObjectGuid(groupResults, Constants.ADAttributes.OBJECT_GUID);

            ObjectSID = ADHelper
                .GetAdPropertySID(groupResults, Constants.ADAttributes.OBJECT_SID);

            SamAccountName = ADHelper
                .GetAdProperty(groupResults, Constants.ADAttributes.LOGIN_NAME).ToUpper();
        }
    }
}
