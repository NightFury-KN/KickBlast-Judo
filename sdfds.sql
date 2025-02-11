create table Atheleterec 
(
RecordID varchar (10) primary key,
recmonth varchar (50) not null,
AtheleteID varchar (10) foreign key references  Athelete(AtheleteID),
Trainingplan varchar (10) foreign key references Trainingplantb (TPlan_ID),
Noofplans int,
PThours int,
weight_cat varchar (10) foreign key references W_category_table (cat_ID),
no_of_comp int ,

)