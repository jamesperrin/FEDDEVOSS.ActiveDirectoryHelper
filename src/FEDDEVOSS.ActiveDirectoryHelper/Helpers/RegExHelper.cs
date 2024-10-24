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

using System.Text.RegularExpressions;

namespace FEDDEVOSS.ActiveDirectoryHelper.Helpers
{
    internal static class RegExHelper
    {
        /// <summary>
        /// Determines if is a valid alpha value
        /// </summary>
        /// <param name="value">Value to be checked</param>
        /// <returns>Boolean</returns>
        internal static bool IsValidAlpha(string value)
        {
            string pattern = @"[a-zA-ZáÁàÀȧȦâäǎăāãåąⱥấầắằǡǻǟẫẵảȁȃẩẳạḁậặÂÄǍĂĀÃÅĄȺẤẦẮẰǠǺǞẪẴẢȀȂẨẲẠḀẬẶæÆǽǼǣǢḃƀɓḅḇƃḂɃƁḄḆƂćċĉčçȼḉƈĆĊĈČÇȻḈƇḋďḑđƌɗḍḓḏðǳǆḊĎḐĐƋƊḌḒḎÐǱǲǄǅéèėêëěĕēẽęȩɇếềḗḕễḝẻȅȇểẹḙḛệÉÈĖÊËĚĔĒẼĘȨɆẾỀḖḔỄḜẺȄȆỂẸḘḚỆḟƒƑḞǵġĝǧğḡģǥɠǴĠĜǦĞḠĢǤƓḣĥḧȟḩħḥḫⱨḢĤḦȞḨĦḤḪⱧíìıîïǐĭīĩįɨḯỉȉȋịḭĳÍÌİÎÏǏĬĪĨĮƗḮỈȈȊỊḬĲĵǰɉĴɈḱǩķƙḳḵⱪḰǨĶƘḲḴⱩĺŀľⱡļƚłḷḽḻḹǉĹĿĽⱠĻȽŁḶḼḺḸǇǈḿṁṃḾṀṂńǹṅňñņɲƞṇṋṉǌŋŃǸṄŇÑŅƝȠṆṊṈǊǋŊóòȯôöǒŏōõǫőốồøṓṑȱṍȫỗṏǿȭǭỏȍȏơổọớờỡộƣởợœÓÒȮÔÖǑŎŌÕǪŐỐỒØṒṐȰṌȪỖṎǾȬǬỎȌȎƠỔỌỚỜỠỘƢỞỢŒṕṗᵽƥṔṖⱣƤɋɊŕṙřŗɍɽȑȓṛṟṝŔṘŘŖɌⱤȐȒṚṞṜśṡŝšşṥṧṣșṩßŚṠŜŠŞṤṦṢȘṨẞṫẗťţƭṭʈțṱṯⱦþŧṪŤŢƬṬƮȚṰṮȾÞŦúùûüǔŭūũůųűʉǘǜǚṹǖṻủȕȗưụṳứừṷṵữửựÚÙÛÜǓŬŪŨŮŲŰɄǗǛǙṸǕṺỦȔȖƯỤṲỨỪṶṴỮỬỰṽṿʋṼṾƲẃẁẇŵẅẘẉⱳẂẀẆŴẄẈⱲẋẍẊẌýỳẏŷÿȳỹẙɏỷƴỵÝỲẎŶŸȲỸɎỶƳỴźżẑžƶȥẓẕⱬŹŻẐŽƵȤẒẔⱫ]";

            var regex = new Regex(pattern);

            return !string.IsNullOrEmpty(value) && regex.IsMatch(value);
        }

        /// <summary>
        /// Determines if is a valid alpha numeric value
        /// </summary>
        /// <param name="value">Value to be checked</param>
        /// <returns>Boolean</returns>
        internal static bool IsValidAlphaNumeric(string value)
        {
            string pattern = @"[a-zA-Z0-9áÁàÀȧȦâäǎăāãåąⱥấầắằǡǻǟẫẵảȁȃẩẳạḁậặÂÄǍĂĀÃÅĄȺẤẦẮẰǠǺǞẪẴẢȀȂẨẲẠḀẬẶæÆǽǼǣǢḃƀɓḅḇƃḂɃƁḄḆƂćċĉčçȼḉƈĆĊĈČÇȻḈƇḋďḑđƌɗḍḓḏðǳǆḊĎḐĐƋƊḌḒḎÐǱǲǄǅéèėêëěĕēẽęȩɇếềḗḕễḝẻȅȇểẹḙḛệÉÈĖÊËĚĔĒẼĘȨɆẾỀḖḔỄḜẺȄȆỂẸḘḚỆḟƒƑḞǵġĝǧğḡģǥɠǴĠĜǦĞḠĢǤƓḣĥḧȟḩħḥḫⱨḢĤḦȞḨĦḤḪⱧíìıîïǐĭīĩįɨḯỉȉȋịḭĳÍÌİÎÏǏĬĪĨĮƗḮỈȈȊỊḬĲĵǰɉĴɈḱǩķƙḳḵⱪḰǨĶƘḲḴⱩĺŀľⱡļƚłḷḽḻḹǉĹĿĽⱠĻȽŁḶḼḺḸǇǈḿṁṃḾṀṂńǹṅňñņɲƞṇṋṉǌŋŃǸṄŇÑŅƝȠṆṊṈǊǋŊóòȯôöǒŏōõǫőốồøṓṑȱṍȫỗṏǿȭǭỏȍȏơổọớờỡộƣởợœÓÒȮÔÖǑŎŌÕǪŐỐỒØṒṐȰṌȪỖṎǾȬǬỎȌȎƠỔỌỚỜỠỘƢỞỢŒṕṗᵽƥṔṖⱣƤɋɊŕṙřŗɍɽȑȓṛṟṝŔṘŘŖɌⱤȐȒṚṞṜśṡŝšşṥṧṣșṩßŚṠŜŠŞṤṦṢȘṨẞṫẗťţƭṭʈțṱṯⱦþŧṪŤŢƬṬƮȚṰṮȾÞŦúùûüǔŭūũůųűʉǘǜǚṹǖṻủȕȗưụṳứừṷṵữửựÚÙÛÜǓŬŪŨŮŲŰɄǗǛǙṸǕṺỦȔȖƯỤṲỨỪṶṴỮỬỰṽṿʋṼṾƲẃẁẇŵẅẘẉⱳẂẀẆŴẄẈⱲẋẍẊẌýỳẏŷÿȳỹẙɏỷƴỵÝỲẎŶŸȲỸɎỶƳỴźżẑžƶȥẓẕⱬŹŻẐŽƵȤẒẔⱫ]";

            var regex = new Regex(pattern);

            return !string.IsNullOrEmpty(value) && regex.IsMatch(value);
        }

        /// <summary>
        /// Determines if is a name value
        /// </summary>
        /// <param name="value">Value to be checked</param>
        /// <returns>Boolean</returns>
        internal static bool IsValidName(string value)
        {
            string pattern = @"[a-zA-Z0-9 ,.'-áÁàÀȧȦâäǎăāãåąⱥấầắằǡǻǟẫẵảȁȃẩẳạḁậặÂÄǍĂĀÃÅĄȺẤẦẮẰǠǺǞẪẴẢȀȂẨẲẠḀẬẶæÆǽǼǣǢḃƀɓḅḇƃḂɃƁḄḆƂćċĉčçȼḉƈĆĊĈČÇȻḈƇḋďḑđƌɗḍḓḏðǳǆḊĎḐĐƋƊḌḒḎÐǱǲǄǅéèėêëěĕēẽęȩɇếềḗḕễḝẻȅȇểẹḙḛệÉÈĖÊËĚĔĒẼĘȨɆẾỀḖḔỄḜẺȄȆỂẸḘḚỆḟƒƑḞǵġĝǧğḡģǥɠǴĠĜǦĞḠĢǤƓḣĥḧȟḩħḥḫⱨḢĤḦȞḨĦḤḪⱧíìıîïǐĭīĩįɨḯỉȉȋịḭĳÍÌİÎÏǏĬĪĨĮƗḮỈȈȊỊḬĲĵǰɉĴɈḱǩķƙḳḵⱪḰǨĶƘḲḴⱩĺŀľⱡļƚłḷḽḻḹǉĹĿĽⱠĻȽŁḶḼḺḸǇǈḿṁṃḾṀṂńǹṅňñņɲƞṇṋṉǌŋŃǸṄŇÑŅƝȠṆṊṈǊǋŊóòȯôöǒŏōõǫőốồøṓṑȱṍȫỗṏǿȭǭỏȍȏơổọớờỡộƣởợœÓÒȮÔÖǑŎŌÕǪŐỐỒØṒṐȰṌȪỖṎǾȬǬỎȌȎƠỔỌỚỜỠỘƢỞỢŒṕṗᵽƥṔṖⱣƤɋɊŕṙřŗɍɽȑȓṛṟṝŔṘŘŖɌⱤȐȒṚṞṜśṡŝšşṥṧṣșṩßŚṠŜŠŞṤṦṢȘṨẞṫẗťţƭṭʈțṱṯⱦþŧṪŤŢƬṬƮȚṰṮȾÞŦúùûüǔŭūũůųűʉǘǜǚṹǖṻủȕȗưụṳứừṷṵữửựÚÙÛÜǓŬŪŨŮŲŰɄǗǛǙṸǕṺỦȔȖƯỤṲỨỪṶṴỮỬỰṽṿʋṼṾƲẃẁẇŵẅẘẉⱳẂẀẆŴẄẈⱲẋẍẊẌýỳẏŷÿȳỹẙɏỷƴỵÝỲẎŶŸȲỸɎỶƳỴźżẑžƶȥẓẕⱬŹŻẐŽƵȤẒẔⱫ]";
            var regex = new Regex(pattern);

            return !string.IsNullOrEmpty(value) && regex.IsMatch(value);
        }

        /// <summary>
        /// Determines if is a valid email value
        /// </summary>
        /// <param name="value">Value to be checked</param>
        /// <returns>Boolean</returns>
        internal static bool IsValidEmail(string value)
        {
            string pattern = @"[a-zA-Z0-9'_.-áÁàÀȧȦâäǎăāãåąⱥấầắằǡǻǟẫẵảȁȃẩẳạḁậặÂÄǍĂĀÃÅĄȺẤẦẮẰǠǺǞẪẴẢȀȂẨẲẠḀẬẶæÆǽǼǣǢḃƀɓḅḇƃḂɃƁḄḆƂćċĉčçȼḉƈĆĊĈČÇȻḈƇḋďḑđƌɗḍḓḏðǳǆḊĎḐĐƋƊḌḒḎÐǱǲǄǅéèėêëěĕēẽęȩɇếềḗḕễḝẻȅȇểẹḙḛệÉÈĖÊËĚĔĒẼĘȨɆẾỀḖḔỄḜẺȄȆỂẸḘḚỆḟƒƑḞǵġĝǧğḡģǥɠǴĠĜǦĞḠĢǤƓḣĥḧȟḩħḥḫⱨḢĤḦȞḨĦḤḪⱧíìıîïǐĭīĩįɨḯỉȉȋịḭĳÍÌİÎÏǏĬĪĨĮƗḮỈȈȊỊḬĲĵǰɉĴɈḱǩķƙḳḵⱪḰǨĶƘḲḴⱩĺŀľⱡļƚłḷḽḻḹǉĹĿĽⱠĻȽŁḶḼḺḸǇǈḿṁṃḾṀṂńǹṅňñņɲƞṇṋṉǌŋŃǸṄŇÑŅƝȠṆṊṈǊǋŊóòȯôöǒŏōõǫőốồøṓṑȱṍȫỗṏǿȭǭỏȍȏơổọớờỡộƣởợœÓÒȮÔÖǑŎŌÕǪŐỐỒØṒṐȰṌȪỖṎǾȬǬỎȌȎƠỔỌỚỜỠỘƢỞỢŒṕṗᵽƥṔṖⱣƤɋɊŕṙřŗɍɽȑȓṛṟṝŔṘŘŖɌⱤȐȒṚṞṜśṡŝšşṥṧṣșṩßŚṠŜŠŞṤṦṢȘṨẞṫẗťţƭṭʈțṱṯⱦþŧṪŤŢƬṬƮȚṰṮȾÞŦúùûüǔŭūũůųűʉǘǜǚṹǖṻủȕȗưụṳứừṷṵữửựÚÙÛÜǓŬŪŨŮŲŰɄǗǛǙṸǕṺỦȔȖƯỤṲỨỪṶṴỮỬỰṽṿʋṼṾƲẃẁẇŵẅẘẉⱳẂẀẆŴẄẈⱲẋẍẊẌýỳẏŷÿȳỹẙɏỷƴỵÝỲẎŶŸȲỸɎỶƳỴźżẑžƶȥẓẕⱬŹŻẐŽƵȤẒẔⱫ]+@[a-zA-Z0-9'_.-áÁàÀȧȦâäǎăāãåąⱥấầắằǡǻǟẫẵảȁȃẩẳạḁậặÂÄǍĂĀÃÅĄȺẤẦẮẰǠǺǞẪẴẢȀȂẨẲẠḀẬẶæÆǽǼǣǢḃƀɓḅḇƃḂɃƁḄḆƂćċĉčçȼḉƈĆĊĈČÇȻḈƇḋďḑđƌɗḍḓḏðǳǆḊĎḐĐƋƊḌḒḎÐǱǲǄǅéèėêëěĕēẽęȩɇếềḗḕễḝẻȅȇểẹḙḛệÉÈĖÊËĚĔĒẼĘȨɆẾỀḖḔỄḜẺȄȆỂẸḘḚỆḟƒƑḞǵġĝǧğḡģǥɠǴĠĜǦĞḠĢǤƓḣĥḧȟḩħḥḫⱨḢĤḦȞḨĦḤḪⱧíìıîïǐĭīĩįɨḯỉȉȋịḭĳÍÌİÎÏǏĬĪĨĮƗḮỈȈȊỊḬĲĵǰɉĴɈḱǩķƙḳḵⱪḰǨĶƘḲḴⱩĺŀľⱡļƚłḷḽḻḹǉĹĿĽⱠĻȽŁḶḼḺḸǇǈḿṁṃḾṀṂńǹṅňñņɲƞṇṋṉǌŋŃǸṄŇÑŅƝȠṆṊṈǊǋŊóòȯôöǒŏōõǫőốồøṓṑȱṍȫỗṏǿȭǭỏȍȏơổọớờỡộƣởợœÓÒȮÔÖǑŎŌÕǪŐỐỒØṒṐȰṌȪỖṎǾȬǬỎȌȎƠỔỌỚỜỠỘƢỞỢŒṕṗᵽƥṔṖⱣƤɋɊŕṙřŗɍɽȑȓṛṟṝŔṘŘŖɌⱤȐȒṚṞṜśṡŝšşṥṧṣșṩßŚṠŜŠŞṤṦṢȘṨẞṫẗťţƭṭʈțṱṯⱦþŧṪŤŢƬṬƮȚṰṮȾÞŦúùûüǔŭūũůųűʉǘǜǚṹǖṻủȕȗưụṳứừṷṵữửựÚÙÛÜǓŬŪŨŮŲŰɄǗǛǙṸǕṺỦȔȖƯỤṲỨỪṶṴỮỬỰṽṿʋṼṾƲẃẁẇŵẅẘẉⱳẂẀẆŴẄẈⱲẋẍẊẌýỳẏŷÿȳỹẙɏỷƴỵÝỲẎŶŸȲỸɎỶƳỴźżẑžƶȥẓẕⱬŹŻẐŽƵȤẒẔⱫ]+\.[a-zA-Z0-9áÁàÀȧȦâäǎăāãåąⱥấầắằǡǻǟẫẵảȁȃẩẳạḁậặÂÄǍĂĀÃÅĄȺẤẦẮẰǠǺǞẪẴẢȀȂẨẲẠḀẬẶæÆǽǼǣǢḃƀɓḅḇƃḂɃƁḄḆƂćċĉčçȼḉƈĆĊĈČÇȻḈƇḋďḑđƌɗḍḓḏðǳǆḊĎḐĐƋƊḌḒḎÐǱǲǄǅéèėêëěĕēẽęȩɇếềḗḕễḝẻȅȇểẹḙḛệÉÈĖÊËĚĔĒẼĘȨɆẾỀḖḔỄḜẺȄȆỂẸḘḚỆḟƒƑḞǵġĝǧğḡģǥɠǴĠĜǦĞḠĢǤƓḣĥḧȟḩħḥḫⱨḢĤḦȞḨĦḤḪⱧíìıîïǐĭīĩįɨḯỉȉȋịḭĳÍÌİÎÏǏĬĪĨĮƗḮỈȈȊỊḬĲĵǰɉĴɈḱǩķƙḳḵⱪḰǨĶƘḲḴⱩĺŀľⱡļƚłḷḽḻḹǉĹĿĽⱠĻȽŁḶḼḺḸǇǈḿṁṃḾṀṂńǹṅňñņɲƞṇṋṉǌŋŃǸṄŇÑŅƝȠṆṊṈǊǋŊóòȯôöǒŏōõǫőốồøṓṑȱṍȫỗṏǿȭǭỏȍȏơổọớờỡộƣởợœÓÒȮÔÖǑŎŌÕǪŐỐỒØṒṐȰṌȪỖṎǾȬǬỎȌȎƠỔỌỚỜỠỘƢỞỢŒṕṗᵽƥṔṖⱣƤɋɊŕṙřŗɍɽȑȓṛṟṝŔṘŘŖɌⱤȐȒṚṞṜśṡŝšşṥṧṣșṩßŚṠŜŠŞṤṦṢȘṨẞṫẗťţƭṭʈțṱṯⱦþŧṪŤŢƬṬƮȚṰṮȾÞŦúùûüǔŭūũůųűʉǘǜǚṹǖṻủȕȗưụṳứừṷṵữửựÚÙÛÜǓŬŪŨŮŲŰɄǗǛǙṸǕṺỦȔȖƯỤṲỨỪṶṴỮỬỰṽṿʋṼṾƲẃẁẇŵẅẘẉⱳẂẀẆŴẄẈⱲẋẍẊẌýỳẏŷÿȳỹẙɏỷƴỵÝỲẎŶŸȲỸɎỶƳỴźżẑžƶȥẓẕⱬŹŻẐŽƵȤẒẔⱫ]{2,4}";
            var regex = new Regex(pattern);

            return !string.IsNullOrEmpty(value) && regex.IsMatch(value);
        }
    }
}

