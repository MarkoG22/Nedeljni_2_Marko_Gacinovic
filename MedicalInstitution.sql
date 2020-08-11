--Creating database only if database is not created yet
IF DB_ID('MedicalInstitution') IS NULL
CREATE DATABASE MedicalInstitution
GO
USE MedicalInstitution;
​
if exists (SELECT name FROM sys.sysobjects WHERE name = 'tblPatient')
drop table tblPatient;
​
if exists (SELECT name FROM sys.sysobjects WHERE name = 'tblDoctor')
drop table tblDoctor;
​
if exists (SELECT name FROM sys.sysobjects WHERE name = 'tblHospital')
drop table tblHospital;
​
if exists (SELECT name FROM sys.sysobjects WHERE name = 'tblManager')
drop table tblManager;
​
if exists (SELECT name FROM sys.sysobjects WHERE name = 'tblMaintance')
drop table tblMaintance;
​
if exists (SELECT name FROM sys.sysobjects WHERE name = 'tblShift')
drop table tblShift;
​
if exists (SELECT name FROM sys.sysobjects WHERE name = 'tblUser')
drop table tblUser;
​
if OBJECT_ID('vwMaintance','v') IS NOT NULL DROP VIEW vwMaintance;
if OBJECT_ID('vwDoctor','v') IS NOT NULL DROP VIEW vwDoctor;
if OBJECT_ID('vwPatient','v') IS NOT NULL DROP VIEW vwPatient;
if OBJECT_ID('vwManager','v') IS NOT NULL DROP VIEW vwManager;
​
create table tblUser (
UserId int identity(1,1) primary key,
FullName nvarchar (50) not null,
IdCard nvarchar (50) not null,
Gender nvarchar(1) not null,
Birthdate date not null,
Citizenship nvarchar (50) not null,
Manager bit not null,
Username nvarchar (50) unique not null,
Pasword nvarchar (50) unique not null 
)
​
create table tblMaintance (
MaintanceID int identity(1,1) primary key,
UserID int not null,
GrowPermision bit not null,
InvalidDuty bit not null,
AmbulanceDuty bit not null
)
​
create table tblManager(
ManagerID int identity (1,1) primary key,
UserID int not null,
HospitalLevel int,
MaxDoctors int,
MinRooms int,
Erors int,
​
)
​
create table tblDoctor(
DoctorID int identity (1,1) primary key,
UserID int not null,
Department nvarchar(50),
UniqueNumber nvarchar(50),
AccountNumber nvarchar(50),
ShiftID int,
Reception bit,
ManagerID int 
)
​
create table tblPatient(
PatientID int identity(1,1) primary key,
UserID int,
CardNumber nvarchar(40),
DateExpire date,
DoctorID int
)
​
create table tblShift(
ShiftID int identity(1,1) primary key,
ShiftName nvarchar (50) not null,
)
​
create table tblHospital(
HospitalID int identity (1,1) primary key,
HospitalName nvarchar(40) unique not null,
Adress nvarchar (40) not null,
StartDate date,
Owns nvarchar(50) not null,
Flors int not null,
Levels int not null,
Balcony bit not null,
Yard bit not null,
AmbulancePoint int not null,
InvalidPoint int not null
)
​
Alter Table tblMaintance
Add foreign key (UserID) references tblUser(UserID);
​
Alter Table tblManager
Add foreign key (UserID) references tblUser(UserID);
​
Alter Table tblDoctor
Add foreign key (UserID) references tblUser(UserID);
​
Alter Table tblDoctor
Add foreign key (ManagerID) references tblManager(ManagerID);
​
Alter Table tblDoctor
Add foreign key (ShiftID) references tblShift(ShiftID);
​
Alter Table tblPatient
Add foreign key (UserID) references tblUser(UserID);
​
Alter Table tblPatient
Add foreign key (DoctorID) references tblDoctor(DoctorID);
​
Insert into tblShift values ('Morning'),('Afternoon'),('Night'),('24');
​
GO
CREATE VIEW vwMaintance AS
	SELECT	tblUser.*, 
			tblMaintance.MaintanceID, tblMaintance.AmbulanceDuty,tblMaintance.GrowPermision,tblMaintance.InvalidDuty
	FROM	tblUser, tblMaintance
	WHERE	tblUser.UserID = tblMaintance.UserID
​
GO
CREATE VIEW vwDoctor AS
	SELECT	tblUser.*, 
			tblDoctor.DoctorID,tblDoctor.AccountNumber,tblDoctor.Department,tblDoctor.ManagerID,tblDoctor.Reception,tblDoctor.ShiftID,tblDoctor.UniqueNumber
	FROM	tblUser, tblDoctor 
	WHERE	tblUser.UserID = tblDoctor.UserID
​
GO
CREATE VIEW vwPatient AS
	SELECT	tblUser.*,
			tblPatient.CardNumber,tblPatient.PatientID,tblPatient.DateExpire,tblPatient.DoctorID
	FROM	tblUser, tblPatient
	WHERE	tblUser.UserID = tblPatient.UserID
​
GO
CREATE VIEW vwManager AS
	SELECT	tblUser.*,
			tblManager.ManagerID,tblManager.HospitalLevel,tblManager.MaxDoctors,tblManager.MinRooms,tblManager.Erors
	From tblUser,tblManager
	WHERE	tblUser.UserID = tblManager.UserID
​
