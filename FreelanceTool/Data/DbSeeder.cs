using System;
using System.Linq;
using System.Threading.Tasks;
using FreelanceTool.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FreelanceTool.Data
{
	public static class DbSeeder
	{
		public static async Task Run(IServiceProvider serviceProvider)
		{
			var dbContextOptions = serviceProvider
				.GetRequiredService<DbContextOptions<ApplicationDbContext>>();
			using (var dbContext = new ApplicationDbContext(dbContextOptions))
			{
				dbContext.Database.Migrate();

				await SeedMembership(serviceProvider);

				await SeedData(dbContext);
			}
		}

		private static async Task SeedMembership(IServiceProvider serviceProvider)
		{
			var config = serviceProvider.GetRequiredService<IConfiguration>();
			var defaultPassword = config["seededUserPwd"];

			var user1Id = await EnsureUser(
				serviceProvider, defaultPassword, "fidomax07@gmail.com");
			//await EnsureRole(serviceProvider, user1Id, "admin");

			var user2Id = await EnsureUser(
				serviceProvider, defaultPassword, "andreas.mueller@livingtech.ch");
			//await EnsureRole(serviceProvider, user2Id, "admin");
		}

		private static async Task<string> EnsureUser(
			IServiceProvider serviceProvider, string defaultUserPwd, string userName)
		{
			var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
			var user = await userManager.FindByNameAsync(userName);
			if (user == null)
			{
				user = new ApplicationUser { UserName = userName };
				await userManager.CreateAsync(user, defaultUserPwd);
			}

			return user.Id;
		}

		private static async Task<IdentityResult> EnsureRole(
			IServiceProvider serviceProvider, string userId, string role)
		{
			IdentityResult IR = null;

			var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
			if (!await roleManager.RoleExistsAsync(role))
			{
				IR = await roleManager.CreateAsync(new IdentityRole(role));
			}

			var userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
			var user = await userManager.FindByIdAsync(userId);
			var userRoles = await userManager.GetRolesAsync(user);
			if (!userRoles.Contains(role))
			{
				IR = await userManager.AddToRoleAsync(user, role);
			}

			return IR;
		}

		public static async Task SeedData(ApplicationDbContext context)
		{
			if (context.Languages.Any()) return;

			var languages = new[]
			{
				new Language
				{
					Alpha1 = "D",
					Alpha2 = "de",
					NameEnglish = "German",
					NameGerman = "Deutsch",
					NameFrench = "Deutsch"
				},
				new Language
				{
					Alpha1 = "F",
					Alpha2 = "fr",
					NameEnglish = "French",
					NameGerman = "Français",
					NameFrench = "Français"
				},
				new Language
				{
					Alpha1 = "I",
					Alpha2 = "it",
					NameEnglish = "Italian",
					NameGerman = "Italiano",
					NameFrench = "Italiano"
				},
				new Language
				{
					Alpha1 = "E",
					Alpha2 = "en",
					NameEnglish = "English",
					NameGerman = "Englisch",
					NameFrench = "Anglais"
				}
			};
			context.Languages.AddRange(languages);
			await context.SaveChangesAsync();

			#region Raw query for populating nationalities

			context.Database.ExecuteSqlCommand(@"
				INSERT INTO Nationality (
					Code, Alpha2, Alpha3, NameEnglish, NameGerman, NameFrench) 
				VALUES
				(4, N'AF', N'AFG', N'Afghanistan', N'Afghanistan', N'Afghanistan'),
				(8, N'AL', N'ALB', N'Albania', N'Albanien', N'Albanie'),
				(10, N'AQ', N'ATA', N'Antarctica', N'Antarktis', N'Antarctique'),
				(12, N'DZ', N'DZA', N'Algeria', N'Algerien', N'Algérie'),
				(16, N'AS', N'ASM', N'American Samoa', N'Samoa', N'Samoa Américaines'),
				(20, N'AD', N'AND', N'Andorra', N'Andorra', N'Andorre'),
				(24, N'AO', N'AGO', N'Angola', N'Angola', N'Angola'),
				(28, N'AG', N'ATG', N'Antigua and Barbuda', N'Antigua und Barbuda', N'Antigua-et-Barbuda'),
				(31, N'AZ', N'AZE', N'Azerbaijan', N'Aserbaidschan', N'Azerbaïdjan'),
				(32, N'AR', N'ARG', N'Argentina', N'Argentinien', N'Argentine'),
				(36, N'AU', N'AUS', N'Australia', N'Australien', N'Australie'),
				(40, N'AT', N'AUT', N'Austria', N'Österreich', N'Autriche'),
				(44, N'BS', N'BHS', N'Bahamas', N'Bahamas', N'Bahamas'),
				(48, N'BH', N'BHR', N'Bahrain', N'Bahrain', N'Bahreïn'),
				(50, N'BD', N'BGD', N'Bangladesh', N'Bangladesh', N'Bangladesh'),
				(51, N'AM', N'ARM', N'Armenia', N'Armenien', N'Arménie'),
				(52, N'BB', N'BRB', N'Barbados', N'Barbados', N'Barbade'),
				(56, N'BE', N'BEL', N'Belgium', N'Belgien', N'Belgique'),
				(60, N'BM', N'BMU', N'Bermuda', N'Bermudas', N'Bermudes'),
				(64, N'BT', N'BTN', N'Bhutan', N'Bhutan', N'Bhoutan'),
				(68, N'BO', N'BOL', N'Bolivia', N'Bolivien', N'Bolivie'),
				(70, N'BA', N'BIH', N'Bosnia and Herzegovina', N'Bosnien-Herzegowina', N'Bosnie-Herzégovine'),
				(72, N'BW', N'BWA', N'Botswana', N'Botswana', N'Botswana'),
				(74, N'BV', N'BVT', N'Bouvet Island', N'Bouvet Inseln', N'Île Bouvet'),
				(76, N'BR', N'BRA', N'Brazil', N'Brasilien', N'Brésil'),
				(84, N'BZ', N'BLZ', N'Belize', N'Belize', N'Belize'),
				(86, N'IO', N'IOT', N'British Indian Ocean Territory', N'Britisch-Indischer Ozean', N'Territoire Britannique de l''Océan Indien'),
				(90, N'SB', N'SLB', N'Solomon Islands', N'Solomon Inseln', N'Îles Salomon'),
				(92, N'VG', N'VGB', N'British Virgin Islands', N'Virgin Island (Brit.)', N'Îles Vierges Britanniques'),
				(96, N'BN', N'BRN', N'Brunei Darussalam', N'Brunei', N'Brunéi Darussalam'),
				(100, N'BG', N'BGR', N'Bulgaria', N'Bulgarien', N'Bulgarie'),
				(104, N'MM', N'MMR', N'Myanmar', N'Birma', N'Myanmar'),
				(108, N'BI', N'BDI', N'Burundi', N'Burundi', N'Burundi'),
				(112, N'BY', N'BLR', N'Belarus', N'Weissrussland', N'Bélarus'),
				(116, N'KH', N'KHM', N'Cambodia', N'Kambodscha', N'Cambodge'),
				(120, N'CM', N'CMR', N'Cameroon', N'Kamerun', N'Cameroun'),
				(124, N'CA', N'CAN', N'Canada', N'Kanada', N'Canada'),
				(132, N'CV', N'CPV', N'Cape Verde', N'Kap Verde', N'Cap-vert'),
				(136, N'KY', N'CYM', N'Cayman Islands', N'Kaiman Inseln', N'Îles Caïmanes'),
				(140, N'CF', N'CAF', N'Central African', N'Zentralafrikanische Republik', N'République Centrafricaine'),
				(144, N'LK', N'LKA', N'Sri Lanka', N'Sri Lanka', N'Sri Lanka'),
				(148, N'TD', N'TCD', N'Chad', N'Tschad', N'Tchad'),
				(152, N'CL', N'CHL', N'Chile', N'Chile', N'Chili'),
				(156, N'CN', N'CHN', N'China', N'China', N'Chine'),
				(158, N'TW', N'TWN', N'Taiwan', N'Taiwan', N'Taïwan'),
				(162, N'CX', N'CXR', N'Christmas Island', N'Christmas Island', N'Île Christmas'),
				(166, N'CC', N'CCK', N'Cocos (Keeling) Islands', N'Kokosinseln', N'Îles Cocos (Keeling)'),
				(170, N'CO', N'COL', N'Colombia', N'Kolumbien', N'Colombie'),
				(174, N'KM', N'COM', N'Comoros', N'Komoren', N'Comores'),
				(175, N'YT', N'MYT', N'Mayotte', N'Mayotte', N'Mayotte'),
				(178, N'CG', N'COG', N'Republic of the Congo', N'Kongo', N'République du Congo'),
				(180, N'CD', N'COD', N'The Democratic Republic Of The Congo', N'Kongo, Demokratische Republik', N'République Démocratique du Congo'),
				(184, N'CK', N'COK', N'Cook Islands', N'Cook Inseln', N'Îles Cook'),
				(188, N'CR', N'CRI', N'Costa Rica', N'Costa Rica', N'Costa Rica'),
				(191, N'HR', N'HRV', N'Croatia', N'Kroatien', N'Croatie'),
				(192, N'CU', N'CUB', N'Cuba', N'Kuba', N'Cuba'),
				(196, N'CY', N'CYP', N'Cyprus', N'Zypern', N'Chypre'),
				(203, N'CZ', N'CZE', N'Czech Republic', N'Tschechische Republik', N'République Tchèque'),
				(204, N'BJ', N'BEN', N'Benin', N'Benin', N'Bénin'),
				(208, N'DK', N'DNK', N'Denmark', N'Dänemark', N'Danemark'),
				(212, N'DM', N'DMA', N'Dominica', N'Dominika', N'Dominique'),
				(214, N'DO', N'DOM', N'Dominican Republic', N'Dominikanische Republik', N'République Dominicaine'),
				(218, N'EC', N'ECU', N'Ecuador', N'Ecuador', N'Équateur'),
				(222, N'SV', N'SLV', N'El Salvador', N'El Salvador', N'El Salvador'),
				(226, N'GQ', N'GNQ', N'Equatorial Guinea', N'Äquatorial Guinea', N'Guinée Équatoriale'),
				(231, N'ET', N'ETH', N'Ethiopia', N'Äthiopien', N'Éthiopie'),
				(232, N'ER', N'ERI', N'Eritrea', N'Eritrea', N'Érythrée'),
				(233, N'EE', N'EST', N'Estonia', N'Estland', N'Estonie'),
				(234, N'FO', N'FRO', N'Faroe Islands', N'Färöer Inseln', N'Îles Féroé'),
				(238, N'FK', N'FLK', N'Falkland Islands', N'Falkland Inseln', N'Îles (malvinas) Falkland'),
				(239, N'GS', N'SGS', N'South Georgia and the South Sandwich Islands', N'South Georgia, South Sandwich Isl.', N'Géorgie du Sud et les Îles Sandwich du Sud'),
				(242, N'FJ', N'FJI', N'Fiji', N'Fidschi', N'Fidji'),
				(246, N'FI', N'FIN', N'Finland', N'Finnland', N'Finlande'),
				(248, N'AX', N'ALA', N'Åland Islands', N'Åland Islands', N'Îles Åland'),
				(250, N'FR', N'FRA', N'France', N'Frankreich', N'France'),
				(254, N'GF', N'GUF', N'French Guiana', N'französisch Guyana', N'Guyane Française'),
				(258, N'PF', N'PYF', N'French Polynesia', N'Französisch Polynesien', N'Polynésie Française'),
				(260, N'TF', N'ATF', N'French Southern Territories', N'Französisches Süd-Territorium', N'Terres Australes Françaises'),
				(262, N'DJ', N'DJI', N'Djibouti', N'Djibuti', N'Djibouti'),
				(266, N'GA', N'GAB', N'Gabon', N'Gabun', N'Gabon'),
				(268, N'GE', N'GEO', N'Georgia', N'Georgien', N'Géorgie'),
				(270, N'GM', N'GMB', N'Gambia', N'Gambia', N'Gambie'),
				(275, N'PS', N'PSE', N'Occupied Palestinian Territory', N'Palästina', N'Territoire Palestinien Occupé'),
				(276, N'DE', N'DEU', N'Germany', N'Deutschland', N'Allemagne'),
				(288, N'GH', N'GHA', N'Ghana', N'Ghana', N'Ghana'),
				(292, N'GI', N'GIB', N'Gibraltar', N'Gibraltar', N'Gibraltar'),
				(296, N'KI', N'KIR', N'Kiribati', N'Kiribati', N'Kiribati'),
				(300, N'GR', N'GRC', N'Greece', N'Griechenland', N'Grèce'),
				(304, N'GL', N'GRL', N'Greenland', N'GrÃ¶nland', N'Groenland'),
				(308, N'GD', N'GRD', N'Grenada', N'Grenada', N'Grenade'),
				(312, N'GP', N'GLP', N'Guadeloupe', N'Guadeloupe', N'Guadeloupe'),
				(316, N'GU', N'GUM', N'Guam', N'Guam', N'Guam'),
				(320, N'GT', N'GTM', N'Guatemala', N'Guatemala', N'Guatemala'),
				(324, N'GN', N'GIN', N'Guinea', N'Guinea', N'Guinée'),
				(328, N'GY', N'GUY', N'Guyana', N'Guyana', N'Guyana'),
				(332, N'HT', N'HTI', N'Haiti', N'Haiti', N'Haïti'),
				(334, N'HM', N'HMD', N'Heard Island and McDonald Islands', N'Heard und McDonald Islands', N'Îles Heard et Mcdonald'),
				(336, N'VA', N'VAT', N'Vatican City State', N'Vatikan', N'Saint-Siège (état de la Cité du Vatican)'),
				(340, N'HN', N'HND', N'Honduras', N'Honduras', N'Honduras'),
				(344, N'HK', N'HKG', N'Hong Kong', N'Hong Kong', N'Hong-Kong'),
				(348, N'HU', N'HUN', N'Hungary', N'Ungarn', N'Hongrie'),
				(352, N'IS', N'ISL', N'Iceland', N'Island', N'Islande'),
				(356, N'IN', N'IND', N'India', N'Indien', N'Inde'),
				(360, N'ID', N'IDN', N'Indonesia', N'Indonesien', N'Indonésie'),
				(364, N'IR', N'IRN', N'Islamic Republic of Iran', N'Iran', N'République Islamique d''Iran'),
				(368, N'IQ', N'IRQ', N'Iraq', N'Irak', N'Iraq'),
				(372, N'IE', N'IRL', N'Ireland', N'Irland', N'Irlande'),
				(376, N'IL', N'ISR', N'Israel', N'Israel', N'Israël'),
				(380, N'IT', N'ITA', N'Italy', N'Italien', N'Italie'),
				(384, N'CI', N'CIV', N'Côte d''Ivoire', N'Elfenbeinküste', N'Côte d''Ivoire'),
				(388, N'JM', N'JAM', N'Jamaica', N'Jamaika', N'Jamaïque'),
				(392, N'JP', N'JPN', N'Japan', N'Japan', N'Japon'),
				(398, N'KZ', N'KAZ', N'Kazakhstan', N'Kasachstan', N'Kazakhstan'),
				(400, N'JO', N'JOR', N'Jordan', N'Jordanien', N'Jordanie'),
				(404, N'KE', N'KEN', N'Kenya', N'Kenia', N'Kenya'),
				(408, N'KP', N'PRK', N'Democratic People''s Republic of Korea', N'Nord Korea', N'République Populaire Démocratique de Corée'),
				(410, N'KR', N'KOR', N'Republic of Korea', N'Süd Korea', N'République de Corée'),
				(414, N'KW', N'KWT', N'Kuwait', N'Kuwait', N'Koweït'),
				(417, N'KG', N'KGZ', N'Kyrgyzstan', N'Kirgisistan', N'Kirghizistan'),
				(418, N'LA', N'LAO', N'Lao People''s Democratic Republic', N'Laos', N'République Démocratique Populaire Lao'),
				(422, N'LB', N'LBN', N'Lebanon', N'Libanon', N'Liban'),
				(426, N'LS', N'LSO', N'Lesotho', N'Lesotho', N'Lesotho'),
				(428, N'LV', N'LVA', N'Latvia', N'Lettland', N'Lettonie'),
				(430, N'LR', N'LBR', N'Liberia', N'Liberia', N'Libéria'),
				(434, N'LY', N'LBY', N'Libyan Arab Jamahiriya', N'Libyen', N'Jamahiriya Arabe Libyenne'),
				(438, N'LI', N'LIE', N'Liechtenstein', N'Liechtenstein', N'Liechtenstein'),
				(440, N'LT', N'LTU', N'Lithuania', N'Litauen', N'Lituanie'),
				(442, N'LU', N'LUX', N'Luxembourg', N'Luxemburg', N'Luxembourg'),
				(446, N'MO', N'MAC', N'Macao', N'Macao', N'Macao'),
				(450, N'MG', N'MDG', N'Madagascar', N'Madagaskar', N'Madagascar'),
				(454, N'MW', N'MWI', N'Malawi', N'Malawi', N'Malawi'),
				(458, N'MY', N'MYS', N'Malaysia', N'Malaysia', N'Malaisie'),
				(462, N'MV', N'MDV', N'Maldives', N'Malediven', N'Maldives'),
				(466, N'ML', N'MLI', N'Mali', N'Mali', N'Mali'),
				(470, N'MT', N'MLT', N'Malta', N'Malta', N'Malte'),
				(474, N'MQ', N'MTQ', N'Martinique', N'Martinique', N'Martinique'),
				(478, N'MR', N'MRT', N'Mauritania', N'Mauretanien', N'Mauritanie'),
				(480, N'MU', N'MUS', N'Mauritius', N'Mauritius', N'Maurice'),
				(484, N'MX', N'MEX', N'Mexico', N'Mexiko', N'Mexique'),
				(492, N'MC', N'MCO', N'Monaco', N'Monaco', N'Monaco'),
				(496, N'MN', N'MNG', N'Mongolia', N'Mongolei', N'Mongolie'),
				(498, N'MD', N'MDA', N'Republic of Moldova', N'Moldavien', N'République de Moldova'),
				(500, N'MS', N'MSR', N'Montserrat', N'Montserrat', N'Montserrat'),
				(504, N'MA', N'MAR', N'Morocco', N'Marokko', N'Maroc'),
				(508, N'MZ', N'MOZ', N'Mozambique', N'Mocambique', N'Mozambique'),
				(512, N'OM', N'OMN', N'Oman', N'Oman', N'Oman'),
				(516, N'NA', N'NAM', N'Namibia', N'Namibia', N'Namibie'),
				(520, N'NR', N'NRU', N'Nauru', N'Nauru', N'Nauru'),
				(524, N'NP', N'NPL', N'Nepal', N'Nepal', N'Népal'),
				(528, N'NL', N'NLD', N'Netherlands', N'Niederlande', N'Countries-Bas'),
				(530, N'AN', N'ANT', N'Netherlands Antilles', N'Niederländische Antillen', N'Antilles Néerlandaises'),
				(533, N'AW', N'ABW', N'Aruba', N'Aruba', N'Aruba'),
				(540, N'NC', N'NCL', N'New Caledonia', N'Neukaledonien', N'Nouvelle-Calédonie'),
				(548, N'VU', N'VUT', N'Vanuatu', N'Vanuatu', N'Vanuatu'),
				(554, N'NZ', N'NZL', N'New Zealand', N'Neuseeland', N'Nouvelle-Zélande'),
				(558, N'NI', N'NIC', N'Nicaragua', N'Nicaragua', N'Nicaragua'),
				(562, N'NE', N'NER', N'Niger', N'Niger', N'Niger'),
				(566, N'NG', N'NGA', N'Nigeria', N'Nigeria', N'Nigéria'),
				(570, N'NU', N'NIU', N'Niue', N'Niue', N'Niué'),
				(574, N'NF', N'NFK', N'Norfolk Island', N'Norfolk Inseln', N'Île Norfolk'),
				(578, N'NO', N'NOR', N'Norway', N'Norwegen', N'Norvège'),
				(580, N'MP', N'MNP', N'Northern Mariana Islands', N'Marianen', N'Îles Mariannes du Nord'),
				(581, N'UM', N'UMI', N'United States Minor Outlying Islands', N'United States Minor Outlying Islands', N'Îles Mineures Éloignées des États-Unis'),
				(583, N'FM', N'FSM', N'Federated States of Micronesia', N'Mikronesien', N'États Fédérés de Micronésie'),
				(584, N'MH', N'MHL', N'Marshall Islands', N'Marshall Inseln', N'Îles Marshall'),
				(585, N'PW', N'PLW', N'Palau', N'Palau', N'Palaos'),
				(586, N'PK', N'PAK', N'Pakistan', N'Pakistan', N'Pakistan'),
				(591, N'PA', N'PAN', N'Panama', N'Panama', N'Panama'),
				(598, N'PG', N'PNG', N'Papua New Guinea', N'Papua Neuguinea', N'Papouasie-Nouvelle-Guinée'),
				(600, N'PY', N'PRY', N'Paraguay', N'Paraguay', N'Paraguay'),
				(604, N'PE', N'PER', N'Peru', N'Peru', N'Pérou'),
				(608, N'PH', N'PHL', N'Philippines', N'Philippinen', N'Philippines'),
				(612, N'PN', N'PCN', N'Pitcairn', N'Pitcairn', N'Pitcairn'),
				(616, N'PL', N'POL', N'Poland', N'Polen', N'Pologne'),
				(620, N'PT', N'PRT', N'Portugal', N'Portugal', N'Portugal'),
				(624, N'GW', N'GNB', N'Guinea-Bissau', N'Guinea Bissau', N'Guinée-Bissau'),
				(626, N'TL', N'TLS', N'Timor-Leste', N'Timor-Leste', N'Timor-Leste'),
				(630, N'PR', N'PRI', N'Puerto Rico', N'Puerto Rico', N'Porto Rico'),
				(634, N'QA', N'QAT', N'Qatar', N'Qatar', N'Qatar'),
				(638, N'RE', N'REU', N'Réunion', N'Reunion', N'Réunion'),
				(642, N'RO', N'ROU', N'Romania', N'Rumänien', N'Roumanie'),
				(643, N'RU', N'RUS', N'Russian Federation', N'Russland', N'Fédération de Russie'),
				(646, N'RW', N'RWA', N'Rwanda', N'Ruanda', N'Rwanda'),
				(654, N'SH', N'SHN', N'Saint Helena', N'St. Helena', N'Sainte-Hélène'),
				(659, N'KN', N'KNA', N'Saint Kitts and Nevis', N'St. Kitts Nevis Anguilla', N'Saint-Kitts-et-Nevis'),
				(660, N'AI', N'AIA', N'Anguilla', N'Anguilla', N'Anguilla'),
				(662, N'LC', N'LCA', N'Saint Lucia', N'Saint Lucia', N'Sainte-Lucie'),
				(666, N'PM', N'SPM', N'Saint-Pierre and Miquelon', N'St. Pierre und Miquelon', N'Saint-Pierre-et-Miquelon'),
				(670, N'VC', N'VCT', N'Saint Vincent and the Grenadines', N'St. Vincent', N'Saint-Vincent-et-les Grenadines'),
				(674, N'SM', N'SMR', N'San Marino', N'San Marino', N'Saint-Marin'),
				(678, N'ST', N'STP', N'Sao Tome and Principe', N'Sao Tome', N'Sao Tomé-et-Principe'),
				(682, N'SA', N'SAU', N'Saudi Arabia', N'Saudi Arabien', N'Arabie Saoudite'),
				(686, N'SN', N'SEN', N'Senegal', N'Senegal', N'Sénégal'),
				(690, N'SC', N'SYC', N'Seychelles', N'Seychellen', N'Seychelles'),
				(694, N'SL', N'SLE', N'Sierra Leone', N'Sierra Leone', N'Sierra Leone'),
				(702, N'SG', N'SGP', N'Singapore', N'Singapur', N'Singapour'),
				(703, N'SK', N'SVK', N'Slovakia', N'Slowakei -slowakische Republik-', N'Slovaquie'),
				(704, N'VN', N'VNM', N'Vietnam', N'Vietnam', N'Viet Nam'),
				(705, N'SI', N'SVN', N'Slovenia', N'Slowenien', N'Slovénie'),
				(706, N'SO', N'SOM', N'Somalia', N'Somalia', N'Somalie'),
				(710, N'ZA', N'ZAF', N'South Africa', N'Südafrika', N'Afrique du Sud'),
				(716, N'ZW', N'ZWE', N'Zimbabwe', N'Zimbabwe', N'Zimbabwe'),
				(724, N'ES', N'ESP', N'Spain', N'Spanien', N'Espagne'),
				(732, N'EH', N'ESH', N'Western Sahara', N'Westsahara', N'Sahara Occidental'),
				(736, N'SD', N'SDN', N'Sudan', N'Sudan', N'Soudan'),
				(740, N'SR', N'SUR', N'Suriname', N'Surinam', N'Suriname'),
				(744, N'SJ', N'SJM', N'Svalbard and Jan Mayen', N'Svalbard und Jan Mayen Islands', N'Svalbard etÎle Jan Mayen'),
				(748, N'SZ', N'SWZ', N'Swaziland', N'Swasiland', N'Swaziland'),
				(752, N'SE', N'SWE', N'Sweden', N'Schweden', N'Suède'),
				(756, N'CH', N'CHE', N'Switzerland', N'Schweiz', N'Suisse'),
				(760, N'SY', N'SYR', N'Syrian Arab Republic', N'Syrien', N'République Arabe Syrienne'),
				(762, N'TJ', N'TJK', N'Tajikistan', N'Tadschikistan', N'Tadjikistan'),
				(764, N'TH', N'THA', N'Thailand', N'Thailand', N'Thaïlande'),
				(768, N'TG', N'TGO', N'Togo', N'Togo', N'Togo'),
				(772, N'TK', N'TKL', N'Tokelau', N'Tokelau', N'Tokelau'),
				(776, N'TO', N'TON', N'Tonga', N'Tonga', N'Tonga'),
				(780, N'TT', N'TTO', N'Trinidad and Tobago', N'Trinidad Tobago', N'Trinité-et-Tobago'),
				(784, N'AE', N'ARE', N'United Arab Emirates', N'Vereinigte Arabische Emirate', N'Émirats Arabes Unis'),
				(788, N'TN', N'TUN', N'Tunisia', N'Tunesien', N'Tunisie'),
				(792, N'TR', N'TUR', N'Turkey', N'Türkei', N'Turquie'),
				(795, N'TM', N'TKM', N'Turkmenistan', N'Turkmenistan', N'Turkménistan'),
				(796, N'TC', N'TCA', N'Turks and Caicos Islands', N'Turks und Kaikos Inseln', N'Îles Turks et Caïques'),
				(798, N'TV', N'TUV', N'Tuvalu', N'Tuvalu', N'Tuvalu'),
				(800, N'UG', N'UGA', N'Uganda', N'Uganda', N'Ouganda'),
				(804, N'UA', N'UKR', N'Ukraine', N'Ukraine', N'Ukraine'),
				(807, N'MK', N'MKD', N'The Former Yugoslav Republic of Macedonia', N'Mazedonien', N'L''ex-République Yougoslave de Macédoine'),
				(818, N'EG', N'EGY', N'Egypt', N'Ägypten', N'Égypte'),
				(826, N'GB', N'GBR', N'United Kingdom', N'GroÃŸbritannien (UK)', N'Royaume-Uni'),
				(833, N'IM', N'IMN', N'Isle of Man', N'Isle of Man', N'Île de Man'),
				(834, N'TZ', N'TZA', N'United Republic Of Tanzania', N'Tansania', N'République-Unie de Tanzanie'),
				(840, N'US', N'USA', N'United States', N'Vereinigte Staaten von Amerika', N'États-Unis'),
				(850, N'VI', N'VIR', N'U.S. Virgin Islands', N'Virgin Island (USA)', N'Îles Vierges des États-Unis'),
				(854, N'BF', N'BFA', N'Burkina Faso', N'Burkina Faso', N'Burkina Faso'),
				(858, N'UY', N'URY', N'Uruguay', N'Uruguay', N'Uruguay'),
				(860, N'UZ', N'UZB', N'Uzbekistan', N'Usbekistan', N'Ouzbékistan'),
				(862, N'VE', N'VEN', N'Venezuela', N'Venezuela', N'Venezuela'),
				(876, N'WF', N'WLF', N'Wallis and Futuna', N'Wallis et Futuna', N'Wallis et Futuna'),
				(882, N'WS', N'WSM', N'Samoa', N'Samoa', N'Samoa'),
				(887, N'YE', N'YEM', N'Yemen', N'Jemen', N'Yémen'),
				(891, N'CS', N'SCG', N'Serbia and Montenegro', N'Serbia and Montenegro', N'Serbie-et-Monténégro'),
				(894, N'ZM', N'ZMB', N'Zambia', N'Sambia', N'Zambie')");

			#endregion
			await context.SaveChangesAsync();
		}
	}
}
