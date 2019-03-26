CREATE TABLE Visitor (ticketNr int(10) NOT NULL, name varchar(15) NOT NULL, surname varchar(20) NOT NULL, email varchar(50) NOT NULL, password varchar(20) NOT NULL, bankAccountNr varchar(20), campSpotId int(10), isCampPayed tinyint(1) DEFAULT 0, isScanned tinyint(1) DEFAULT 0 NOT NULL, whenScanned timestamp NULL, RFIDCode int(10), balance int(10) DEFAULT 0 NOT NULL, isValid tinyint(1) NOT NULL, PRIMARY KEY (ticketNr));
CREATE TABLE `Transaction` (ticketNr int(10) NOT NULL, dateOfTrans timestamp NOT NULL, amount int(10) NOT NULL, PRIMARY KEY (ticketNr, dateOfTrans));
CREATE TABLE Purchaise (ticketNr int(10) NOT NULL, dateOfTrans timestamp NOT NULL, itemId int(10) NOT NULL, PRIMARY KEY (ticketNr, dateOfTrans));
CREATE TABLE TopUp (ticketNr int(10) NOT NULL, dateOfTrans timestamp NOT NULL, machineId int(10) NOT NULL, PRIMARY KEY (ticketNr, dateOfTrans));
CREATE TABLE Entity (VisitorticketNr int(10) NOT NULL);
CREATE TABLE Place (placeId int(10) NOT NULL AUTO_INCREMENT, name varchar(20) NOT NULL, PRIMARY KEY (placeId));
CREATE TABLE Entity2 ();
CREATE TABLE Item (itemId int(10) NOT NULL AUTO_INCREMENT, name varchar(20), price int(10) NOT NULL, PRIMARY KEY (itemId));
CREATE TABLE Review (ticketNr int(10) NOT NULL AUTO_INCREMENT, reviewText varchar(1000), PRIMARY KEY (ticketNr));
CREATE TABLE CampSpot (campSpotId int(10) NOT NULL, reservedPlaces int(2) NOT NULL, PRIMARY KEY (campSpotId));
CREATE TABLE Loan (ticketNr int(10) NOT NULL, dateOfTrans timestamp NOT NULL, itemId int(10) NOT NULL, startTime timestamp NOT NULL, endTime timestamp NULL, isReturned tinyint(1) DEFAULT 0 NOT NULL, PRIMARY KEY (ticketNr, dateOfTrans));
CREATE TABLE Employee (employeeNr int(10) NOT NULL AUTO_INCREMENT, password int(10) NOT NULL, positionId int(10), PRIMARY KEY (employeeNr));
CREATE TABLE Employee_Place (EmployeeemployeeNr int(10) NOT NULL, PlaceplaceId int(10) NOT NULL, PRIMARY KEY (EmployeeemployeeNr, PlaceplaceId));
CREATE TABLE Loanable (itemId int(10) NOT NULL, PRIMARY KEY (itemId));
CREATE TABLE Purchaisable (itemId int(10) NOT NULL, placeId int(10) NOT NULL, itemType int(10) NOT NULL, PRIMARY KEY (itemId));
CREATE TABLE ItemType (itemType int(10) NOT NULL AUTO_INCREMENT, PRIMARY KEY (itemType));
CREATE TABLE ItemType_Place (ItemTypeitemType int(10) NOT NULL, PlaceplaceId int(10) NOT NULL, StartQuantity int(11), PRIMARY KEY (ItemTypeitemType, PlaceplaceId));
ALTER TABLE Purchaise ADD INDEX FKPurchaise639017 (ticketNr, dateOfTrans), ADD CONSTRAINT FKPurchaise639017 FOREIGN KEY (ticketNr, dateOfTrans) REFERENCES `Transaction` (ticketNr, dateOfTrans);
ALTER TABLE TopUp ADD INDEX FKTopUp87177 (ticketNr, dateOfTrans), ADD CONSTRAINT FKTopUp87177 FOREIGN KEY (ticketNr, dateOfTrans) REFERENCES `Transaction` (ticketNr, dateOfTrans);
ALTER TABLE `Transaction` ADD INDEX FKTransactio330613 (ticketNr), ADD CONSTRAINT FKTransactio330613 FOREIGN KEY (ticketNr) REFERENCES Visitor (ticketNr);
ALTER TABLE Purchaisable ADD INDEX FKPurchaisab896724 (placeId), ADD CONSTRAINT FKPurchaisab896724 FOREIGN KEY (placeId) REFERENCES Place (placeId);
ALTER TABLE Visitor ADD INDEX FKVisitor981131 (campSpotId), ADD CONSTRAINT FKVisitor981131 FOREIGN KEY (campSpotId) REFERENCES CampSpot (campSpotId);
ALTER TABLE Visitor ADD INDEX FKVisitor300069 (ticketNr), ADD CONSTRAINT FKVisitor300069 FOREIGN KEY (ticketNr) REFERENCES Review (ticketNr);
ALTER TABLE Employee_Place ADD INDEX FKEmployee_P279081 (EmployeeemployeeNr), ADD CONSTRAINT FKEmployee_P279081 FOREIGN KEY (EmployeeemployeeNr) REFERENCES Employee (employeeNr);
ALTER TABLE Employee_Place ADD INDEX FKEmployee_P158334 (PlaceplaceId), ADD CONSTRAINT FKEmployee_P158334 FOREIGN KEY (PlaceplaceId) REFERENCES Place (placeId);
ALTER TABLE Loan ADD INDEX FKLoan88157 (itemId), ADD CONSTRAINT FKLoan88157 FOREIGN KEY (itemId) REFERENCES Loanable (itemId);
ALTER TABLE Loan ADD INDEX FKLoan706295 (ticketNr, dateOfTrans), ADD CONSTRAINT FKLoan706295 FOREIGN KEY (ticketNr, dateOfTrans) REFERENCES `Transaction` (ticketNr, dateOfTrans);
ALTER TABLE Loanable ADD INDEX FKLoanable172602 (itemId), ADD CONSTRAINT FKLoanable172602 FOREIGN KEY (itemId) REFERENCES Item (itemId);
ALTER TABLE Purchaisable ADD INDEX FKPurchaisab886250 (itemId), ADD CONSTRAINT FKPurchaisab886250 FOREIGN KEY (itemId) REFERENCES Item (itemId);
ALTER TABLE CampSpot ADD INDEX FKCampSpot556628 (campSpotId), ADD CONSTRAINT FKCampSpot556628 FOREIGN KEY (campSpotId) REFERENCES Item (itemId);
ALTER TABLE Purchaise ADD INDEX FKPurchaise265472 (itemId), ADD CONSTRAINT FKPurchaise265472 FOREIGN KEY (itemId) REFERENCES Purchaisable (itemId);
ALTER TABLE Purchaise ADD INDEX FKPurchaise708351 (itemId), ADD CONSTRAINT FKPurchaise708351 FOREIGN KEY (itemId) REFERENCES CampSpot (campSpotId);
ALTER TABLE ItemType_Place ADD INDEX FKItemType_P289527 (ItemTypeitemType), ADD CONSTRAINT FKItemType_P289527 FOREIGN KEY (ItemTypeitemType) REFERENCES ItemType (itemType);
ALTER TABLE ItemType_Place ADD INDEX FKItemType_P150657 (PlaceplaceId), ADD CONSTRAINT FKItemType_P150657 FOREIGN KEY (PlaceplaceId) REFERENCES Place (placeId);
ALTER TABLE Purchaisable ADD INDEX FKPurchaisab427814 (itemType), ADD CONSTRAINT FKPurchaisab427814 FOREIGN KEY (itemType) REFERENCES ItemType (itemType);
