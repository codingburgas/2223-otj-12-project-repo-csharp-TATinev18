USE Ticket2GO
INSERT INTO AspNetRoles VALUES('24d083d2-adb2-4c9d-9fd5-ed3726c42d20','User', 'USER' ,'ee798def-b2d6-4f37-a7e2-55849422f7d9');
INSERT INTO AspNetRoles VALUES('e6e1a46f-4a9f-4b8a-94d2-f994856765ad','Admin','ADMIN','0ea98ce7-9831-42e4-8e8b-e4ff61e919a7');
INSERT INTO AspNetRoles VALUES('febe12b6-5fcb-4349-a4ab-92b4a6ced624','Company Manager','COMPANY MANAGER','24700fcd-65c5-4519-9dfd-ad7876cffd40');

/* Парола: CMDkurtach21# */
INSERT INTO AspNetUsers VALUES('6ab2876a-4f55-4d8e-bf71-ab821d3042be', N'Админ', N'Админов', 'adminanamina@abv.bg', 'ADMINANAMINA@ABV.BG', 'adminanamina@abv.bg', 'ADMINANAMINA@ABV.BG', '0', 'AQAAAAEAACcQAAAAEAgrnXCIwMKaPBj7eotc21b0sdU7QjvAz7oEHzM+0MRQNunUmv4mMpXqw+O0GYVPzQ==', 'EA5AGHA7JZFVJEEVHDU2QNUFU7VZ4DCG', '71ac0cee-ee98-4927-8ef0-749aa26f72d2', NULL, 0,0,NULL,1,0);

INSERT INTO AspNetUserRoles VALUES('6ab2876a-4f55-4d8e-bf71-ab821d3042be', '24d083d2-adb2-4c9d-9fd5-ed3726c42d20');
INSERT INTO AspNetUserRoles VALUES('6ab2876a-4f55-4d8e-bf71-ab821d3042be', 'e6e1a46f-4a9f-4b8a-94d2-f994856765ad');