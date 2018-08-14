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
				(4, 'AF', 'AFG', 'Afghanistan', 'Afghanistan', 'Afghanistan'),
				(8, 'AL', 'ALB', 'Albania', 'Albanien', 'Albanie'),
				(10, 'AQ', 'ATA', 'Antarctica', 'Antarktis', 'Antarctique'),
				(12, 'DZ', 'DZA', 'Algeria', 'Algerien', 'Algérie'),
				(16, 'AS', 'ASM', 'American Samoa', 'Samoa', 'Samoa Américaines'),
				(20, 'AD', 'AND', 'Andorra', 'Andorra', 'Andorre'),
				(24, 'AO', 'AGO', 'Angola', 'Angola', 'Angola'),
				(28, 'AG', 'ATG', 'Antigua and Barbuda', 'Antigua und Barbuda', 'Antigua-et-Barbuda'),
				(31, 'AZ', 'AZE', 'Azerbaijan', 'Aserbaidschan', 'Azerbaïdjan'),
				(32, 'AR', 'ARG', 'Argentina', 'Argentinien', 'Argentine'),
				(36, 'AU', 'AUS', 'Australia', 'Australien', 'Australie'),
				(40, 'AT', 'AUT', 'Austria', 'Österreich', 'Autriche'),
				(44, 'BS', 'BHS', 'Bahamas', 'Bahamas', 'Bahamas'),
				(48, 'BH', 'BHR', 'Bahrain', 'Bahrain', 'Bahreïn'),
				(50, 'BD', 'BGD', 'Bangladesh', 'Bangladesh', 'Bangladesh'),
				(51, 'AM', 'ARM', 'Armenia', 'Armenien', 'Arménie'),
				(52, 'BB', 'BRB', 'Barbados', 'Barbados', 'Barbade'),
				(56, 'BE', 'BEL', 'Belgium', 'Belgien', 'Belgique'),
				(60, 'BM', 'BMU', 'Bermuda', 'Bermudas', 'Bermudes'),
				(64, 'BT', 'BTN', 'Bhutan', 'Bhutan', 'Bhoutan'),
				(68, 'BO', 'BOL', 'Bolivia', 'Bolivien', 'Bolivie'),
				(70, 'BA', 'BIH', 'Bosnia and Herzegovina', 'Bosnien-Herzegowina', 'Bosnie-Herzégovine'),
				(72, 'BW', 'BWA', 'Botswana', 'Botswana', 'Botswana'),
				(74, 'BV', 'BVT', 'Bouvet Island', 'Bouvet Inseln', 'Île Bouvet'),
				(76, 'BR', 'BRA', 'Brazil', 'Brasilien', 'Brésil'),
				(84, 'BZ', 'BLZ', 'Belize', 'Belize', 'Belize'),
				(86, 'IO', 'IOT', 'British Indian Ocean Territory', 'Britisch-Indischer Ozean', 'Territoire Britannique de l''Océan Indien'),
				(90, 'SB', 'SLB', 'Solomon Islands', 'Solomon Inseln', 'Îles Salomon'),
				(92, 'VG', 'VGB', 'British Virgin Islands', 'Virgin Island (Brit.)', 'Îles Vierges Britanniques'),
				(96, 'BN', 'BRN', 'Brunei Darussalam', 'Brunei', 'Brunéi Darussalam'),
				(100, 'BG', 'BGR', 'Bulgaria', 'Bulgarien', 'Bulgarie'),
				(104, 'MM', 'MMR', 'Myanmar', 'Birma', 'Myanmar'),
				(108, 'BI', 'BDI', 'Burundi', 'Burundi', 'Burundi'),
				(112, 'BY', 'BLR', 'Belarus', 'Weissrussland', 'Bélarus'),
				(116, 'KH', 'KHM', 'Cambodia', 'Kambodscha', 'Cambodge'),
				(120, 'CM', 'CMR', 'Cameroon', 'Kamerun', 'Cameroun'),
				(124, 'CA', 'CAN', 'Canada', 'Kanada', 'Canada'),
				(132, 'CV', 'CPV', 'Cape Verde', 'Kap Verde', 'Cap-vert'),
				(136, 'KY', 'CYM', 'Cayman Islands', 'Kaiman Inseln', 'Îles Caïmanes'),
				(140, 'CF', 'CAF', 'Central African', 'Zentralafrikanische Republik', 'République Centrafricaine'),
				(144, 'LK', 'LKA', 'Sri Lanka', 'Sri Lanka', 'Sri Lanka'),
				(148, 'TD', 'TCD', 'Chad', 'Tschad', 'Tchad'),
				(152, 'CL', 'CHL', 'Chile', 'Chile', 'Chili'),
				(156, 'CN', 'CHN', 'China', 'China', 'Chine'),
				(158, 'TW', 'TWN', 'Taiwan', 'Taiwan', 'Taïwan'),
				(162, 'CX', 'CXR', 'Christmas Island', 'Christmas Island', 'Île Christmas'),
				(166, 'CC', 'CCK', 'Cocos (Keeling) Islands', 'Kokosinseln', 'Îles Cocos (Keeling)'),
				(170, 'CO', 'COL', 'Colombia', 'Kolumbien', 'Colombie'),
				(174, 'KM', 'COM', 'Comoros', 'Komoren', 'Comores'),
				(175, 'YT', 'MYT', 'Mayotte', 'Mayotte'),
				(178, 'CG', 'COG', 'Republic of the Congo', 'République du Congo'),
				(180, 'CD', 'COD', 'The Democratic Republic Of The Congo', 'République Démocratique du Congo'),
				(184, 'CK', 'COK', 'Cook Islands', 'Îles Cook'),
				(188, 'CR', 'CRI', 'Costa Rica', 'Costa Rica'),
				(191, 'HR', 'HRV', 'Croatia', 'Croatie'),
				(192, 'CU', 'CUB', 'Cuba', 'Cuba'),
				(196, 'CY', 'CYP', 'Cyprus', 'Chypre'),
				(203, 'CZ', 'CZE', 'Czech Republic', 'République Tchèque'),
				(204, 'BJ', 'BEN', 'Benin', 'Bénin'),
				(208, 'DK', 'DNK', 'Denmark', 'Danemark'),
				(212, 'DM', 'DMA', 'Dominica', 'Dominique'),
				(214, 'DO', 'DOM', 'Dominican Republic', 'République Dominicaine'),
				(218, 'EC', 'ECU', 'Ecuador', 'Équateur'),
				(222, 'SV', 'SLV', 'El Salvador', 'El Salvador'),
				(226, 'GQ', 'GNQ', 'Equatorial Guinea', 'Guinée Équatoriale'),
				(231, 'ET', 'ETH', 'Ethiopia', 'Éthiopie'),
				(232, 'ER', 'ERI', 'Eritrea', 'Érythrée'),
				(233, 'EE', 'EST', 'Estonia', 'Estonie'),
				(234, 'FO', 'FRO', 'Faroe Islands', 'Îles Féroé'),
				(238, 'FK', 'FLK', 'Falkland Islands', 'Îles (malvinas) Falkland'),
				(239, 'GS', 'SGS', 'South Georgia and the South Sandwich Islands', 'Géorgie du Sud et les Îles Sandwich du Sud'),
				(242, 'FJ', 'FJI', 'Fiji', 'Fidji'),
				(246, 'FI', 'FIN', 'Finland', 'Finlande'),
				(248, 'AX', 'ALA', 'Åland Islands', 'Îles Åland'),
				(250, 'FR', 'FRA', 'France', 'France'),
				(254, 'GF', 'GUF', 'French Guiana', 'Guyane Française'),
				(258, 'PF', 'PYF', 'French Polynesia', 'Polynésie Française'),
				(260, 'TF', 'ATF', 'French Southern Territories', 'Terres Australes Françaises'),
				(262, 'DJ', 'DJI', 'Djibouti', 'Djibouti'),
				(266, 'GA', 'GAB', 'Gabon', 'Gabon'),
				(268, 'GE', 'GEO', 'Georgia', 'Géorgie'),
				(270, 'GM', 'GMB', 'Gambia', 'Gambie'),
				(275, 'PS', 'PSE', 'Occupied Palestinian Territory', 'Territoire Palestinien Occupé'),
				(276, 'DE', 'DEU', 'Germany', 'Allemagne'),
				(288, 'GH', 'GHA', 'Ghana', 'Ghana'),
				(292, 'GI', 'GIB', 'Gibraltar', 'Gibraltar'),
				(296, 'KI', 'KIR', 'Kiribati', 'Kiribati'),
				(300, 'GR', 'GRC', 'Greece', 'Grèce'),
				(304, 'GL', 'GRL', 'Greenland', 'Groenland'),
				(308, 'GD', 'GRD', 'Grenada', 'Grenade'),
				(312, 'GP', 'GLP', 'Guadeloupe', 'Guadeloupe'),
				(316, 'GU', 'GUM', 'Guam', 'Guam'),
				(320, 'GT', 'GTM', 'Guatemala', 'Guatemala'),
				(324, 'GN', 'GIN', 'Guinea', 'Guinée'),
				(328, 'GY', 'GUY', 'Guyana', 'Guyana'),
				(332, 'HT', 'HTI', 'Haiti', 'Haïti'),
				(334, 'HM', 'HMD', 'Heard Island and McDonald Islands', 'Îles Heard et Mcdonald'),
				(336, 'VA', 'VAT', 'Vatican City State', 'Saint-Siège (état de la Cité du Vatican)'),
				(340, 'HN', 'HND', 'Honduras', 'Honduras'),
				(344, 'HK', 'HKG', 'Hong Kong', 'Hong-Kong'),
				(348, 'HU', 'HUN', 'Hungary', 'Hongrie'),
				(352, 'IS', 'ISL', 'Iceland', 'Islande'),
				(356, 'IN', 'IND', 'India', 'Inde'),
				(360, 'ID', 'IDN', 'Indonesia', 'Indonésie'),
				(364, 'IR', 'IRN', 'Islamic Republic of Iran', 'République Islamique d''Iran'),
				(368, 'IQ', 'IRQ', 'Iraq', 'Iraq'),
				(372, 'IE', 'IRL', 'Ireland', 'Irlande'),
				(376, 'IL', 'ISR', 'Israel', 'Israël'),
				(380, 'IT', 'ITA', 'Italy', 'Italie'),
				(384, 'CI', 'CIV', 'Côte d''Ivoire', 'Côte d''Ivoire'),
				(388, 'JM', 'JAM', 'Jamaica', 'Jamaïque'),
				(392, 'JP', 'JPN', 'Japan', 'Japon'),
				(398, 'KZ', 'KAZ', 'Kazakhstan', 'Kazakhstan'),
				(400, 'JO', 'JOR', 'Jordan', 'Jordanie'),
				(404, 'KE', 'KEN', 'Kenya', 'Kenya'),
				(408, 'KP', 'PRK', 'Democratic People''s Republic of Korea', 'République Populaire Démocratique de Corée'),
				(410, 'KR', 'KOR', 'Republic of Korea', 'République de Corée'),
				(414, 'KW', 'KWT', 'Kuwait', 'Koweït'),
				(417, 'KG', 'KGZ', 'Kyrgyzstan', 'Kirghizistan'),
				(418, 'LA', 'LAO', 'Lao People''s Democratic Republic', 'République Démocratique Populaire Lao'),
				(422, 'LB', 'LBN', 'Lebanon', 'Liban'),
				(426, 'LS', 'LSO', 'Lesotho', 'Lesotho'),
				(428, 'LV', 'LVA', 'Latvia', 'Lettonie'),
				(430, 'LR', 'LBR', 'Liberia', 'Libéria'),
				(434, 'LY', 'LBY', 'Libyan Arab Jamahiriya', 'Jamahiriya Arabe Libyenne'),
				(438, 'LI', 'LIE', 'Liechtenstein', 'Liechtenstein'),
				(440, 'LT', 'LTU', 'Lithuania', 'Lituanie'),
				(442, 'LU', 'LUX', 'Luxembourg', 'Luxembourg'),
				(446, 'MO', 'MAC', 'Macao', 'Macao'),
				(450, 'MG', 'MDG', 'Madagascar', 'Madagascar'),
				(454, 'MW', 'MWI', 'Malawi', 'Malawi'),
				(458, 'MY', 'MYS', 'Malaysia', 'Malaisie'),
				(462, 'MV', 'MDV', 'Maldives', 'Maldives'),
				(466, 'ML', 'MLI', 'Mali', 'Mali'),
				(470, 'MT', 'MLT', 'Malta', 'Malte'),
				(474, 'MQ', 'MTQ', 'Martinique', 'Martinique'),
				(478, 'MR', 'MRT', 'Mauritania', 'Mauritanie'),
				(480, 'MU', 'MUS', 'Mauritius', 'Maurice'),
				(484, 'MX', 'MEX', 'Mexico', 'Mexique'),
				(492, 'MC', 'MCO', 'Monaco', 'Monaco'),
				(496, 'MN', 'MNG', 'Mongolia', 'Mongolie'),
				(498, 'MD', 'MDA', 'Republic of Moldova', 'République de Moldova'),
				(500, 'MS', 'MSR', 'Montserrat', 'Montserrat'),
				(504, 'MA', 'MAR', 'Morocco', 'Maroc'),
				(508, 'MZ', 'MOZ', 'Mozambique', 'Mozambique'),
				(512, 'OM', 'OMN', 'Oman', 'Oman'),
				(516, 'NA', 'NAM', 'Namibia', 'Namibie'),
				(520, 'NR', 'NRU', 'Nauru', 'Nauru'),
				(524, 'NP', 'NPL', 'Nepal', 'Népal'),
				(528, 'NL', 'NLD', 'Netherlands', 'Countries-Bas'),
				(530, 'AN', 'ANT', 'Netherlands Antilles', 'Antilles Néerlandaises'),
				(533, 'AW', 'ABW', 'Aruba', 'Aruba'),
				(540, 'NC', 'NCL', 'New Caledonia', 'Nouvelle-Calédonie'),
				(548, 'VU', 'VUT', 'Vanuatu', 'Vanuatu'),
				(554, 'NZ', 'NZL', 'New Zealand', 'Nouvelle-Zélande'),
				(558, 'NI', 'NIC', 'Nicaragua', 'Nicaragua'),
				(562, 'NE', 'NER', 'Niger', 'Niger'),
				(566, 'NG', 'NGA', 'Nigeria', 'Nigéria'),
				(570, 'NU', 'NIU', 'Niue', 'Niué'),
				(574, 'NF', 'NFK', 'Norfolk Island', 'Île Norfolk'),
				(578, 'NO', 'NOR', 'Norway', 'Norvège'),
				(580, 'MP', 'MNP', 'Northern Mariana Islands', 'Îles Mariannes du Nord'),
				(581, 'UM', 'UMI', 'United States Minor Outlying Islands', 'Îles Mineures Éloignées des États-Unis'),
				(583, 'FM', 'FSM', 'Federated States of Micronesia', 'États Fédérés de Micronésie'),
				(584, 'MH', 'MHL', 'Marshall Islands', 'Îles Marshall'),
				(585, 'PW', 'PLW', 'Palau', 'Palaos'),
				(586, 'PK', 'PAK', 'Pakistan', 'Pakistan'),
				(591, 'PA', 'PAN', 'Panama', 'Panama'),
				(598, 'PG', 'PNG', 'Papua New Guinea', 'Papouasie-Nouvelle-Guinée'),
				(600, 'PY', 'PRY', 'Paraguay', 'Paraguay'),
				(604, 'PE', 'PER', 'Peru', 'Pérou'),
				(608, 'PH', 'PHL', 'Philippines', 'Philippines'),
				(612, 'PN', 'PCN', 'Pitcairn', 'Pitcairn'),
				(616, 'PL', 'POL', 'Poland', 'Pologne'),
				(620, 'PT', 'PRT', 'Portugal', 'Portugal'),
				(624, 'GW', 'GNB', 'Guinea-Bissau', 'Guinée-Bissau'),
				(626, 'TL', 'TLS', 'Timor-Leste', 'Timor-Leste'),
				(630, 'PR', 'PRI', 'Puerto Rico', 'Porto Rico'),
				(634, 'QA', 'QAT', 'Qatar', 'Qatar'),
				(638, 'RE', 'REU', 'Réunion', 'Réunion'),
				(642, 'RO', 'ROU', 'Romania', 'Roumanie'),
				(643, 'RU', 'RUS', 'Russian Federation', 'Fédération de Russie'),
				(646, 'RW', 'RWA', 'Rwanda', 'Rwanda'),
				(654, 'SH', 'SHN', 'Saint Helena', 'Sainte-Hélène'),
				(659, 'KN', 'KNA', 'Saint Kitts and Nevis', 'Saint-Kitts-et-Nevis'),
				(660, 'AI', 'AIA', 'Anguilla', 'Anguilla'),
				(662, 'LC', 'LCA', 'Saint Lucia', 'Sainte-Lucie'),
				(666, 'PM', 'SPM', 'Saint-Pierre and Miquelon', 'Saint-Pierre-et-Miquelon'),
				(670, 'VC', 'VCT', 'Saint Vincent and the Grenadines', 'Saint-Vincent-et-les Grenadines'),
				(674, 'SM', 'SMR', 'San Marino', 'Saint-Marin'),
				(678, 'ST', 'STP', 'Sao Tome and Principe', 'Sao Tomé-et-Principe'),
				(682, 'SA', 'SAU', 'Saudi Arabia', 'Arabie Saoudite'),
				(686, 'SN', 'SEN', 'Senegal', 'Sénégal'),
				(690, 'SC', 'SYC', 'Seychelles', 'Seychelles'),
				(694, 'SL', 'SLE', 'Sierra Leone', 'Sierra Leone'),
				(702, 'SG', 'SGP', 'Singapore', 'Singapour'),
				(703, 'SK', 'SVK', 'Slovakia', 'Slovaquie'),
				(704, 'VN', 'VNM', 'Vietnam', 'Viet Nam'),
				(705, 'SI', 'SVN', 'Slovenia', 'Slovénie'),
				(706, 'SO', 'SOM', 'Somalia', 'Somalie'),
				(710, 'ZA', 'ZAF', 'South Africa', 'Afrique du Sud'),
				(716, 'ZW', 'ZWE', 'Zimbabwe', 'Zimbabwe'),
				(724, 'ES', 'ESP', 'Spain', 'Espagne'),
				(732, 'EH', 'ESH', 'Western Sahara', 'Sahara Occidental'),
				(736, 'SD', 'SDN', 'Sudan', 'Soudan'),
				(740, 'SR', 'SUR', 'Suriname', 'Suriname'),
				(744, 'SJ', 'SJM', 'Svalbard and Jan Mayen', 'Svalbard etÎle Jan Mayen'),
				(748, 'SZ', 'SWZ', 'Swaziland', 'Swaziland'),
				(752, 'SE', 'SWE', 'Sweden', 'Suède'),
				(756, 'CH', 'CHE', 'Switzerland', 'Suisse'),
				(760, 'SY', 'SYR', 'Syrian Arab Republic', 'République Arabe Syrienne'),
				(762, 'TJ', 'TJK', 'Tajikistan', 'Tadjikistan'),
				(764, 'TH', 'THA', 'Thailand', 'Thaïlande'),
				(768, 'TG', 'TGO', 'Togo', 'Togo'),
				(772, 'TK', 'TKL', 'Tokelau', 'Tokelau'),
				(776, 'TO', 'TON', 'Tonga', 'Tonga'),
				(780, 'TT', 'TTO', 'Trinidad and Tobago', 'Trinité-et-Tobago'),
				(784, 'AE', 'ARE', 'United Arab Emirates', 'Émirats Arabes Unis'),
				(788, 'TN', 'TUN', 'Tunisia', 'Tunisie'),
				(792, 'TR', 'TUR', 'Turkey', 'Turquie'),
				(795, 'TM', 'TKM', 'Turkmenistan', 'Turkménistan'),
				(796, 'TC', 'TCA', 'Turks and Caicos Islands', 'Îles Turks et Caïques'),
				(798, 'TV', 'TUV', 'Tuvalu', 'Tuvalu'),
				(800, 'UG', 'UGA', 'Uganda', 'Ouganda'),
				(804, 'UA', 'UKR', 'Ukraine', 'Ukraine'),
				(807, 'MK', 'MKD', 'The Former Yugoslav Republic of Macedonia', 'L''ex-République Yougoslave de Macédoine'),
				(818, 'EG', 'EGY', 'Egypt', 'Égypte'),
				(826, 'GB', 'GBR', 'United Kingdom', 'Royaume-Uni'),
				(833, 'IM', 'IMN', 'Isle of Man', 'Île de Man'),
				(834, 'TZ', 'TZA', 'United Republic Of Tanzania', 'République-Unie de Tanzanie'),
				(840, 'US', 'USA', 'United States', 'États-Unis'),
				(850, 'VI', 'VIR', 'U.S. Virgin Islands', 'Îles Vierges des États-Unis'),
				(854, 'BF', 'BFA', 'Burkina Faso', 'Burkina Faso'),
				(858, 'UY', 'URY', 'Uruguay', 'Uruguay'),
				(860, 'UZ', 'UZB', 'Uzbekistan', 'Ouzbékistan'),
				(862, 'VE', 'VEN', 'Venezuela', 'Venezuela'),
				(876, 'WF', 'WLF', 'Wallis and Futuna', 'Wallis et Futuna'),
				(882, 'WS', 'WSM', 'Samoa', 'Samoa'),
				(887, 'YE', 'YEM', 'Yemen', 'Yémen'),
				(891, 'CS', 'SCG', 'Serbia and Montenegro', 'Serbie-et-Monténégro'),
				(894, 'ZM', 'ZMB', 'Zambia', 'Zambie')");

			#endregion
			await context.SaveChangesAsync();
		}
	}
}
