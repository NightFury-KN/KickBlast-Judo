Create table Usertable --user data
(
	userid varchar (10) primary key,
	username varchar (50) not null,
	upassword varchar (16) not null
)

create table Trainingplantb 
(
	TPlan_ID varchar (10) primary key,
	Tplan_name varchar (50) not null,
	no_of_ses int not null,
	cost decimal (10, 2)

)

create table other_services
(
OserviceID varchar (10) primary key,
Oservice_name varchar (50) not null,
cost decimal (10,2)

)

create table W_category_table
(
cat_ID varchar (10) Primary key,
cat_name varchar (100) not null,
weightlimit decimal not null

)

Create table Athelete(
AtheleteID varchar (10)primary key,
AtheleteName varchar (100) not null,
age int not null,
cweight decimal (10,1) not null

)

create table Atheleterec 
(
RecordID varchar (10) primary key,
recmonth varchar (50) not null,
AtheleteID varchar (10) foreign key references  Athelete(AtheleteID),
Trainingplan varchar (10) foreign key references Trainingplantb (TPlan_ID),
PThours int,
weight_cat varchar (10) foreign key references W_category_table (cat_ID),
no_of_comp int ,

)

