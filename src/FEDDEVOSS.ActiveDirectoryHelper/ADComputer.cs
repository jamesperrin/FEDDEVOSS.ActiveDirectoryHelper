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
using System.Net;

namespace FEDDEVOSS.ActiveDirectoryHelper
{
    public class ADComputer
    {
        public string CN { get; set; }
        public string DnsHostName { get; set; }
        public string DistinguishedName { get; set; }
        public string ObjectGuid { get; set; }
        public string Description { get; set; }
        public string OperatingSystem { get; set; }
        public string OperatingSystemVersion { get; set; }
        public string ServerIP { get; set; }

        /// <summary>
        /// Standard Active Directory Attributes for ADUser
        /// </summary>
        /// <returns>string array of Active Directory Attributes</returns>
        public static string[] PropertiesToLoad()
        {
            return new string[] {
                Constants.ADAttributes.COMMON_NAME,
                Constants.ADAttributes.DNS_HOST_NAME,
                Constants.ADAttributes.DISTINGUISHED_NAME,
                Constants.ADAttributes.OBJECT_GUID,
                Constants.ADAttributes.DESCRIPTION,
                Constants.ADAttributes.OPERATING_SYSTEM,
                Constants.ADAttributes.OPERATING_SYSTEM_VERSION,
            };
        }

        public ADComputer(SearchResult userResults)
        {
            CN = ADHelper.GetAdProperty(userResults, Constants.ADAttributes.COMMON_NAME);
            DnsHostName = ADHelper.GetAdProperty(userResults, Constants.ADAttributes.DNS_HOST_NAME);
            DistinguishedName = ADHelper.GetAdProperty(userResults, Constants.ADAttributes.DISTINGUISHED_NAME);
            ObjectGuid = ADHelper.GetAdPropertyObjectGuid(userResults, Constants.ADAttributes.OBJECT_GUID);
            Description = ADHelper.GetAdProperty(userResults, Constants.ADAttributes.DESCRIPTION);
            OperatingSystem = ADHelper.GetAdProperty(userResults, Constants.ADAttributes.OPERATING_SYSTEM);
            OperatingSystemVersion = ADHelper.GetAdProperty(userResults, Constants.ADAttributes.OPERATING_SYSTEM_VERSION);
            ServerIP = Dns.GetHostEntry(DnsHostName).AddressList[0].ToString();
        }
    }
}
