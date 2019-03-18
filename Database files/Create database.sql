CREATE TABLE Visitor (ticketNr int(10) NOT NULL, name varchar(15) NOT NULL, surname varchar(20) NOT NULL, email varchar(50) NOT NULL, password varchar(20) NOT NULL, bankAccountNr varchar(20), campSpotId int(10), isCampPayed tinyint(1) DEFAULT 0, isScanned tinyint(1) DEFAULT 0 NOT NULL, whenScanned timestamp NULL, RFIDCode int(10), balance int(10) DEFAULT 0 NOT NULL, isValid tinyint(1) NOT NULL, PRIMARY KEY (ticketNr));
CREATE TABLE `Transaction` (ticketNr int(10) NOT NULL, dateOfTrans timestamp NOT NULL, amount int(10) NOT NULL, PRIMARY KEY (ticketNr, dateOfTrans));
CREATE TABLE Purchaise (ticketNr int(10) NOT NULL, dateOfTrans timestamp NOT NULL, itemId int(10) NOT NULL, PRIMARY KEY (ticketNr, dateOfTrans));
CREATE TABLE TopUp (ticketNr int(10) NOT NULL, dateOfTrans timestamp NOT NULL, machineId int(10) NOT NULL, PRIMARY KEY (ticketNr, dateOfTrans));
CREATE TABLE Entity (VisitorticketNr int(10) NOT NULL);
CREATE TABLE Place (placeId int(10) NOT NULL AUTO_INCREMENT, name varchar(20) NOT NULL, PRIMARY KEY (placeId));
CREATE TABLE Item (itemId int(10) NOT NULL AUTO_INCREMENT, itemType varchar(100), placeId int(10) NOT NULL, price int(10) NOT NULL, name varchar(20), isPurchaisable tinyint(1) NOT NULL, PRIMARY KEY (itemId));
CREATE TABLE Review (ticketNr int(10) NOT NULL AUTO_INCREMENT, reviewText varchar(1000), PRIMARY KEY (ticketNr));
CREATE TABLE CampSpot (campSpotId int(10) NOT NULL AUTO_INCREMENT, reservedPlaces int(2) NOT NULL, PRIMARY KEY (campSpotId));
CREATE TABLE Loan (ticketNr int(10) NOT NULL, dateOfTrans timestamp NOT NULL, itemId int(10) NOT NULL, startTime timestamp NOT NULL, endTime timestamp NULL, isReturned tinyint(1) DEFAULT 0 NOT NULL, PRIMARY KEY (ticketNr, dateOfTrans));
CREATE TABLE Employee (employeeNr int(10) NOT NULL AUTO_INCREMENT, password int(10) NOT NULL, positionId int(10), PRIMARY KEY (employeeNr));
CREATE TABLE Employee_Place (EmployeeemployeeNr int(10) NOT NULL, PlaceplaceId int(10) NOT NULL, PRIMARY KEY (EmployeeemployeeNr, PlaceplaceId));
ALTER TABLE Purchaise ADD INDEX FKPurchaise639017 (ticketNr, dateOfTrans), ADD CONSTRAINT FKPurchaise639017 FOREIGN KEY (ticketNr, dateOfTrans) REFERENCES `Transaction` (ticketNr, dateOfTrans);
ALTER TABLE TopUp ADD INDEX FKTopUp87177 (ticketNr, dateOfTrans), ADD CONSTRAINT FKTopUp87177 FOREIGN KEY (ticketNr, dateOfTrans) REFERENCES `Transaction` (ticketNr, dateOfTrans);
ALTER TABLE `Transaction` ADD INDEX FKTransactio330613 (ticketNr), ADD CONSTRAINT FKTransactio330613 FOREIGN KEY (ticketNr) REFERENCES Visitor (ticketNr);
ALTER TABLE Item ADD INDEX FKItem964462 (placeId), ADD CONSTRAINT FKItem964462 FOREIGN KEY (placeId) REFERENCES Place (placeId);
ALTER TABLE Purchaise ADD INDEX FKPurchaise595715 (itemId), ADD CONSTRAINT FKPurchaise595715 FOREIGN KEY (itemId) REFERENCES Item (itemId);
ALTER TABLE Visitor ADD INDEX FKVisitor981131 (campSpotId), ADD CONSTRAINT FKVisitor981131 FOREIGN KEY (campSpotId) REFERENCES CampSpot (campSpotId);
ALTER TABLE Visitor ADD INDEX FKVisitor300069 (ticketNr), ADD CONSTRAINT FKVisitor300069 FOREIGN KEY (ticketNr) REFERENCES Review (ticketNr);
ALTER TABLE Employee_Place ADD INDEX FKEmployee_P279081 (EmployeeemployeeNr), ADD CONSTRAINT FKEmployee_P279081 FOREIGN KEY (EmployeeemployeeNr) REFERENCES Employee (employeeNr);
ALTER TABLE Employee_Place ADD INDEX FKEmployee_P158334 (PlaceplaceId), ADD CONSTRAINT FKEmployee_P158334 FOREIGN KEY (PlaceplaceId) REFERENCES Place (placeId);
ALTER TABLE Loan ADD INDEX FKLoan308597 (itemId), ADD CONSTRAINT FKLoan308597 FOREIGN KEY (itemId) REFERENCES Item (itemId);
ALTER TABLE Loan ADD INDEX FKLoan706295 (ticketNr, dateOfTrans), ADD CONSTRAINT FKLoan706295 FOREIGN KEY (ticketNr, dateOfTrans) REFERENCES `Transaction` (ticketNr, dateOfTrans);
