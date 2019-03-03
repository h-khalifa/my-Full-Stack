create table person (
Id int primary key identity(1,1),
Name varchar(50) not null,
Phone varchar(15),
IsMarried bit,
Gender int,
BDate Date
);
insert into person (Name, Phone, IsMarried, Gender, BDate) values
('Haitham Khalifa', '01254789672', 0, 1, '1995-02-11'),
('Hiba Khalifa', null, 1, 2, '1986-04-11'),
('Hisham Khalifa', null, 1, 1, '1987-03-29'),
('Malak Wael', null, 0, 2, '2006-11-13'),
('Menna Wael', null, 0, 2, '2008-06-04'),
('Omar Wael', null, 0, 1, '2016-08-16')