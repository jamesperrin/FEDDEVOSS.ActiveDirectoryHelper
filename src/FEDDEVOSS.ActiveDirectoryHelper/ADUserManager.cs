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
using System.Linq;

namespace FEDDEVOSS.ActiveDirectoryHelper
{
    public class ADUserManager : ADUser
    {
        public string ManagerDistinguishedName { get; set; }
        public ADUser Manager { get; set; }

        /// <summary>
        /// Standard Active Directory Attributes for ADUserManager
        /// </summary>
        /// <returns>string array of Active Directory Attributes</returns>
        public static string[] ManagerPropertiesToLoad()
        {
            return PropertiesToLoad().Concat(new string[] { Constants.ADAttributes.MANAGER }).ToArray();
        }

        public ADUserManager(SearchResult userResults) : base(userResults)
        {
            ManagerDistinguishedName = ADHelper.GetAdProperty(userResults, Constants.ADAttributes.MANAGER);
        }
    }
}
