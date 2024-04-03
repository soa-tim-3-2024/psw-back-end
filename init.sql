-- ******************************** USERS ********************************
-- Passwords for users:
-- dragan pass: dragan
--sima pass: sima
--somi pass: somi
--brmbrm pass: lozinka
--buki(k****) pass: lozinka
--dop pass: dop123
--author pass: author
--tourist pass- tourist
--veljko pass- veljko
--bosko pass- bosko
--mile pass- tourist 
--zora pass- tourist 
--luka pass- tourist 
INSERT INTO stakeholders."Users"(
	"Id", "Username", "Password", "Role", "IsActive","ProfilePicture")
	VALUES (-168, 'dragan', '$2a$12$wK5oRUsOmY.QZB2aNUTNweKfI3ZdYfxy8iAMcMEpa.pbaLvTXX70W', 2, 'True', 'https://www.espreso.co.rs/data/images/2023/01/17/12/1317487_sin-dragan_share.jpg'),
	(-169, 'sima', '$2a$12$gHLoc9WrjCdW0xDPAfdm0.aceGyqJaSemsk107tO.tcxAmYWeohgO', 2, 'True', 'https://media.discordapp.net/attachments/605735235476520972/1175913776986460180/1700430082262.jpg'),
	(-170, 'somi', '$2a$12$2TY7aJLFJts01ny.UyRMU.1V7NNWCbplb1UUPEx/x/Ya8yXihtDrm', 1, 'True', 'https://cdn.discordapp.com/attachments/1165638888082124852/1173981466925994095/image.png'),
	(-171, 'brmbrm', '$2a$12$0W/JBzV6WiDLyP.mU5ECn.LHZoUm6o/A7P9/oybd4wZdc4sfAt90m', 2, 'True', 'https://cdn.discordapp.com/attachments/1165638888082124852/1174057297807409222/1699987465617.jpg?ex=65663510&is=6553c010&hm=9e7d8441134deebb3d03cd901b291eb2eabccb325c856ff7ead67a8e508d74d5&'),
	(-172, 'buki(k****)', '$2a$12$0W/JBzV6WiDLyP.mU5ECn.LHZoUm6o/A7P9/oybd4wZdc4sfAt90m', 2, 'True', 'https://cdn.discordapp.com/attachments/1165638888082124852/1174057297484451890/1699987465609.jpg?ex=65663510&is=6553c010&hm=76cd42499b60ee0eeb1d21c1d7268158c9f6369dcbf1f5152e6fa8d15fb4359e&'),
    (-1, 'dop', '$2a$12$l/cp/IIoC4pj1CeCK28rH.h01LOND0TfGaQCJnJJsxwIyfMRgZunq', 0, true, ''),
    (-2, 'author', '$2a$12$lYBkk9QFF9LBNhrfsDDDeed5XSpTfMwlQmkrMZL3dpzXN2as8dx/O', 1, true, 'https://imgs.search.brave.com/n8Gm53DmrCXkPu9d7FpTq1FLO8Nj00zDwzTlFac8HH4/rs:fit:860:0:0/g:ce/aHR0cHM6Ly9pbWcu/ZnJlZXBpay5jb20v/cHJlbWl1bS1waG90/by9oYW5kc29tZS10/b3VyaXN0LW1hbi1s/b29rLW1hcC13aGls/ZS1wb2ludGluZy1m/aW5nZXItZGlyZWN0/aW9uLWRlc3RpbmF0/aW9uLXRyYXZlbC1j/b25jZXBfNTY4NTQt/Mzk4NS5qcGc_c2l6/ZT02MjYmZXh0PWpw/Zw'),
    (-3, 'tourist', '$2a$12$DVn357kN0RhFGjVWFuWx7Oxd4sHrQ3oGEnJjkMdG1BhlNx7rPq3Fu', 2, true, 'https://imgs.search.brave.com/_8gIhJxRAq9aqREpTnh_wGcNfv4JwgEssYFmzKDWow8/rs:fit:860:0:0/g:ce/aHR0cHM6Ly9tZWRp/YS5nZXR0eWltYWdl/cy5jb20vaWQvMTMw/MTAwMzg3NC9waG90/by95b3VuZy13b21h/bi1hcnJpdmluZy1h/dC1hLXRyb3BpY2Fs/LXJlc29ydC1mb3It/aGVyLXZhY2F0aW9u/LmpwZz9zPTYxMng2/MTImdz0wJms9MjAm/Yz1BT3N1eWltalZm/QkducTZZeVVhOVZ0/aXRQWE9rdWJOMDFq/TFp0R19lb1ZjPQ'),
    (-4, 'veljko', '$2y$10$2i4JzB5nXynZATPKSIE5k.R3rdXplw09TaD9d.bNaVg.xCqNM90nu', 2, true, 'https://imgs.search.brave.com/n8Gm53DmrCXkPu9d7FpTq1FLO8Nj00zDwzTlFac8HH4/rs:fit:860:0:0/g:ce/aHR0cHM6Ly9pbWcu/ZnJlZXBpay5jb20v/cHJlbWl1bS1waG90/by9oYW5kc29tZS10/b3VyaXN0LW1hbi1s/b29rLW1hcC13aGls/ZS1wb2ludGluZy1m/aW5nZXItZGlyZWN0/aW9uLWRlc3RpbmF0/aW9uLXRyYXZlbC1j/b25jZXBfNTY4NTQt/Mzk4NS5qcGc_c2l6/ZT02MjYmZXh0PWpw/Zw'),
    (-5, 'bosko', '$2y$10$BfKyogjaZczuKc/HuNm3Jeapm.4R5ycfcinL/ZwqTiSSFNXqREFjG', 2, true, 'https://imgs.search.brave.com/n8Gm53DmrCXkPu9d7FpTq1FLO8Nj00zDwzTlFac8HH4/rs:fit:860:0:0/g:ce/aHR0cHM6Ly9pbWcu/ZnJlZXBpay5jb20v/cHJlbWl1bS1waG90/by9oYW5kc29tZS10/b3VyaXN0LW1hbi1s/b29rLW1hcC13aGls/ZS1wb2ludGluZy1m/aW5nZXItZGlyZWN0/aW9uLWRlc3RpbmF0/aW9uLXRyYXZlbC1j/b25jZXBfNTY4NTQt/Mzk4NS5qcGc_c2l6/ZT02MjYmZXh0PWpw/Zw'),
    (-6, 'mile', '$2a$12$DVn357kN0RhFGjVWFuWx7Oxd4sHrQ3oGEnJjkMdG1BhlNx7rPq3Fu', 2, true, 'https://imgs.search.brave.com/n8Gm53DmrCXkPu9d7FpTq1FLO8Nj00zDwzTlFac8HH4/rs:fit:860:0:0/g:ce/aHR0cHM6Ly9pbWcu/ZnJlZXBpay5jb20v/cHJlbWl1bS1waG90/by9oYW5kc29tZS10/b3VyaXN0LW1hbi1s/b29rLW1hcC13aGls/ZS1wb2ludGluZy1m/aW5nZXItZGlyZWN0/aW9uLWRlc3RpbmF0/aW9uLXRyYXZlbC1j/b25jZXBfNTY4NTQt/Mzk4NS5qcGc_c2l6/ZT02MjYmZXh0PWpw/Zw'),
    (-7, 'zora', '$2a$12$DVn357kN0RhFGjVWFuWx7Oxd4sHrQ3oGEnJjkMdG1BhlNx7rPq3Fu', 2, true, 'https://imgs.search.brave.com/n8Gm53DmrCXkPu9d7FpTq1FLO8Nj00zDwzTlFac8HH4/rs:fit:860:0:0/g:ce/aHR0cHM6Ly9pbWcu/ZnJlZXBpay5jb20v/cHJlbWl1bS1waG90/by9oYW5kc29tZS10/b3VyaXN0LW1hbi1s/b29rLW1hcC13aGls/ZS1wb2ludGluZy1m/aW5nZXItZGlyZWN0/aW9uLWRlc3RpbmF0/aW9uLXRyYXZlbC1j/b25jZXBfNTY4NTQt/Mzk4NS5qcGc_c2l6/ZT02MjYmZXh0PWpw/Zw'),
    (-8, 'luka', '$2a$12$DVn357kN0RhFGjVWFuWx7Oxd4sHrQ3oGEnJjkMdG1BhlNx7rPq3Fu', 2, true, 'https://imgs.search.brave.com/n8Gm53DmrCXkPu9d7FpTq1FLO8Nj00zDwzTlFac8HH4/rs:fit:860:0:0/g:ce/aHR0cHM6Ly9pbWcu/ZnJlZXBpay5jb20v/cHJlbWl1bS1waG90/by9oYW5kc29tZS10/b3VyaXN0LW1hbi1s/b29rLW1hcC13aGls/ZS1wb2ludGluZy1m/aW5nZXItZGlyZWN0/aW9uLWRlc3RpbmF0/aW9uLXRyYXZlbC1j/b25jZXBfNTY4NTQt/Mzk4NS5qcGc_c2l6/ZT02MjYmZXh0PWpw/Zw');


-- ******************** SHOPPING CART *******************************
INSERT INTO payments."ShoppingCarts"(
	"Id", "TouristId", "TotalPrice", "IsPurchased")
	VALUES
		(-1, -168, 0, false),
		(-2, -169, 0, false),
		(-3, -171, 0, false),
		(-4, -172, 0, false),
		(-5, -3, 0, false);


-- ******************************** PEOPLE ********************************

INSERT INTO stakeholders."People" ("Id", "UserId", "Name", "Surname", "Email", "Bio", "Motto") VALUES (-169, -169, 'Filip', 'Simic', 'fsimic346@gmail.com', NULL, 'Brt, guglaj o.0'),
(-170, -170, 'Milos', 'Milutinovic', 'somi@gmail.com', NULL, NULL),
(-168, -168, 'Dragoslav', 'Maslac', 'gagi@gmail.com', NULL, NULL),
(-171, -171, 'Vlada', 'Devic', 'brmbrm@gmail.com', NULL, NULL),
(-172, -172, 'Mihajlo', 'Bukarica', 'buki@gmail.com', NULL, NULL),
(-1, -2, 'John', 'Johnson', 'author@gmail.com', 'I love making tours.', 'Never give up.'),
(-2, -3, 'Charles', 'Smith', 'nikolicveljko01@gmail.com', 'I love tours.', 'Stay strong.'),
(-3, -4, 'Veljko', 'Nikolić', 'nikolicveljko01@gmail.com', 'I love tours.', 'Stay strong.'),
(-4, -5, 'Boško', 'Kulušić', 'kulusicbosko@gmail.com', 'I love tours.', 'Stay strong.'),
(-5, -6, 'Mile', 'Kitic', 'mile@gmail.com', 'I love tours.', 'Stay strong.'),
(-6, -7, 'Zora', 'Belic', 'zore@gmail.com', 'I love tours.', 'Stay strong.'),
(-7, -8, 'Lukas', 'Bale', 'nikola3444@gmail.com', 'I love tours.', 'Stay strong.');

-- ***************** WALLETS **********************
INSERT INTO payments."Wallets"(
	"Id", "TouristId", "AdventureCoin")
	VALUES
		(-1, -3, 10000),
		(-2, -168, 200),
		(-3, -169, 500),
		(-4, -170, 400),
		(-5, -171, 800),
		(-6, -172, 150),
		(-7, -4, 10000),
		(-8, -5, 10000),
		(-9, -6, 10000),
		(-10, -7, 10000),
		(-11, -8, 10000),
		(-12, -4, 10000),
		(-13, -5, 10000);

-- **************************** TOUR TOKENS ************************************
INSERT INTO payments."tourTokens"(
	"Id", "TourId", "TouristId")
	VALUES  (-1, -21, -3),
	 (-2, -21, -169),
	 (-3, -21, -6),
	 (-4, -21, -7),
	 (-5, -21, -8),
	 (-6, -22, -3),
	 (-7, -22, -6),
	 (-8, -22, -7),
	 (-9, -25, -8),
	 (-10, -27, -3),
	 (-11, -27, -169),
	 (-12, -21, -4),
   (-13, -21, -5),
	 (-14, -29, -4),
	 (-15, -29, -5);

-- ******************************** FOLLOWERS ******************************

INSERT INTO stakeholders."Followers" ("Id", "UserId", "FollowedById") VALUES (-1, -170, -169),
(-2, -172, -169),
(-3, -171, -169),
(-4, -168, -169),
(-5, -169, -168),
(-6, -169, -170);

-- ******************************** BLOGS ********************************

INSERT INTO blog."Blogs" ("Id", "Title", "Description", "Date", "Status", "AuthorId", "Votes", "VisibilityPolicy") VALUES (-11, 'Off-the-Beaten-Path Travel', '<p>Discover charming villages and secret natural havens off the tourist path. Immerse yourself in the heart of local cultures and explore hidden treasures. Pack your bags for an authentic travel experience!</p><p><br></p><p><img src="https://live.staticflickr.com/65535/53244354274_f096dbce42.jpg"></p><p><br></p><p>Join us on the road less traveled!</p>', '2023-11-14 00:00:00+01', 3, -170, '[{"UserId": -168, "VoteType": 1}, {"UserId": -170, "VoteType": 1}]', 0),
(-12, 'Historical Wonders: Time Travel Edition', '<p>Step into history with ancient ruins and medieval castles. Explore the mysteries of past civilizations and witness the grandeur of medieval architecture. Join us on a captivating journey through time!</p><p><br></p><p><img src="https://live.staticflickr.com/65535/53280758885_34afd10b3a.jpg"></p><p><br></p><p>Walk the corridors of history with us!</p>', '2023-11-14 00:00:00+01', 1, -169, '[{"UserId": -168, "VoteType": 0}, {"UserId": -170, "VoteType": 0}]', 0),
(-13, 'Urban Wanderlust: City Exploration', '<p>Delve into the heartbeat of vibrant cities! Explore bustling markets, iconic landmarks, and hidden gems within the urban landscape.</p><p>Immerse yourself in the culture and energy of city life.</p><p><br></p><p><img src="https://live.staticflickr.com/65535/53308142182_84983c1cc3.jpg"></p><p><br></p><p>Experience the pulse of the city with us!</p>', '2023-11-14 00:00:00+01', 0, -170, '[]', 0),
(-14, 'Team building grupa 1 (produžeci)', '<p>Nastavak kod Dragana na svirku i drinkić</p><p><br></p><p><img src="https://cdn.discordapp.com/attachments/1165638888082124852/1174054126708064327/IMG-20231113-WA0004.jpg"></p>', '2023-11-14 00:00:00+01', 1, -170, '[]', 0),
(-15, 'Team building grupa 1', '<p>Divno veče u ambijentu još divnijih ljudi huh</p><p><br></p><p><img src="https://cdn.discordapp.com/attachments/1165638888082124852/1174053744795734026/IMG-20231113-WA0006.jpg"></p>', '2023-11-14 00:00:00+01', 1, -170, '[]', 0);


INSERT INTO blog."Comments" ("Id", "AuthorId", "BlogId", "CreatedAt", "UpdatedAt", "Text") VALUES (-1, -168, -11, '2023-11-14 13:49:10.108651+01', NULL, 'dobar blog!'),
(-2, -168, -11, '2023-11-14 13:49:23.737314+01', NULL, 'opet sam procitao, i jos je bolji...'),
(-4, -170, -14, '2023-11-14 13:49:10.108651+01', NULL, 'dobar blog!'),
(-5, -171, -14, '2023-11-14 13:58:16.459035+01', NULL, 'mene na pivo ne zovete, a?'),
(-6, -172, -14, '2023-11-14 13:49:10.108651+01', NULL, 'ni ja nisam pozvan 😔'),
(-3, -169, -14, '2023-11-14 13:58:16.459035+01', NULL, 'drugi put');

-- **************************** COUPONS ************************************

INSERT INTO payments."Coupons"(
	"Id", "Code", "Discount", "TourId", "ExpirationDate", "AllFromAuthor", "AuthorId")
	VALUES (-1, 'ZXItNZq3', 20, -21, '2023-12-29 01:00:00+01', true, -2);

-- **************************** TOUR SALES ************************************

INSERT INTO payments."TourSales"("Id","AuthorId", "Name", "StartDate","EndDate","DiscountPercentage","TourIds") VALUES (-1, -2, 'Popust', '2023-12-27', '2023-12-31', 0.2,  '[-24]')