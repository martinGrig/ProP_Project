using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using EventManager.Objects;
using MySql.Data;
using MySql.Data.MySqlClient;

namespace EventManager
{
    public class DataHelper
    {
        public MySqlConnection connection;
        public MySqlConnection connectionForTopUp;
        public MySqlConnection connectionForLoanable;

        public DataHelper()
        {
            String connectionInfo = "server=studmysql01.fhict.local;" +//the hera-server
                                    "database=dbi410102;" +
                                    "user id=dbi410102;" +
                                    "password=prop17;" +
                                    "connect timeout=30;";

            connection = new MySqlConnection(connectionInfo);
            connectionForTopUp = new MySqlConnection(connectionInfo);
            connectionForLoanable = new MySqlConnection(connectionInfo);
        }

        public bool IsServerConnected()
        {
            using (connection)
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public string Login(int _employeeNumber, string password)
        {
            String sql = "SELECT employeeName FROM employee Where employeeNr = @nr AND password = @pass;";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@nr", _employeeNumber);
            command.Parameters.AddWithValue("@pass", password);

            string empNr = null;

            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    empNr = Convert.ToString(reader["employeeName"]);
                }
            }
            catch
            {
                MessageBox.Show("error while loading the students");
            }
            finally
            {
                connection.Close();
            }
            return empNr;
        }

        public List<ShopItem> GetItems(int employeeNr)
        {
            String sql = "SELECT i.name as Name,i.price as Price, p.StartQuantity as Stock, p.IsFood as Food, i.itemId as Id FROM item i JOIN purchaisable p ON i.itemId = p.itemId JOIN place pl On p.placeId = pl.placeId JOIN employee_place ep ON pl.placeId = ep.PlaceplaceId JOIN employee e ON ep.EmployeeemployeeNr = e.employeeNr WHERE e.employeeNr = @empNr;";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@empNr", employeeNr);

            List<ShopItem> temp = new List<ShopItem>();
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                String name;
                int price;
                int stock;
                bool isFood;
                int id;
                while (reader.Read())
                {
                    name = Convert.ToString(reader["Name"]);
                    price = Convert.ToInt32(reader["Price"]);
                    stock = Convert.ToInt32(reader["Stock"]);
                    isFood = Convert.ToBoolean(reader["Food"]);
                    id = Convert.ToInt32(reader["Id"]);
                    temp.Add(new ShopItem(name, price, stock, $"Images/{name}.png", id, isFood));
                }
            }
            catch
            {
                MessageBox.Show("error while loading the students");
            }
            finally
            {
                connection.Close();
            }
            return temp;
        }

        public List<LoanStandItem> GetLoanStandItems(int employeeNr, int placeId)
        {
            String sql = "SELECT li.name AS Name, li.dailyRate AS Price, li.loanitemId AS Id, la.stock AS Stock FROM loanitem li JOIN loanable la On la.loanitemId = li.loanitemId JOIN loanstand ls On ls.loanStandId = la.loanStandId JOIN employee_loanstand el ON el.loanStandId = la.loanStandId JOIN employee emp On emp.employeeNr = el.employeeNr AND emp.employeeNr = @empNr AND la.loanStandId = @placeId";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@empNr", employeeNr);
            command.Parameters.AddWithValue("@placeId", placeId);

            List<LoanStandItem> temp = new List<LoanStandItem>();
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                String name;
                int price;
                int stock;
                int id;
                while (reader.Read())
                {
                    name = Convert.ToString(reader["Name"]);
                    price = Convert.ToInt32(reader["Price"]);
                    stock = Convert.ToInt32(reader["Stock"]);
                    id = Convert.ToInt32(reader["Id"]);
                    temp.Add(new LoanStandItem(name, price, stock, $"Images/{name}.png", id));
                }
            }
            catch
            {
                MessageBox.Show("error while loading the students");
            }
            finally
            {
                connection.Close();
            }
            return temp;
        }

        public List<Employee> GetEmployees()
        {
            String sql = "SELECT  employeeName, employeeNr, surname, positionId, password FROM employee ";
            MySqlCommand command = new MySqlCommand(sql, connection);

            List<Employee> emps = new List<Employee>();
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                String name;
                string lastName;
                string jobId;
                string password;
                int employeeNr;

                while (reader.Read())
                {
                    name = Convert.ToString(reader["employeeName"]);
                    lastName = Convert.ToString(reader["surname"]);
                    jobId = Convert.ToString(reader["positionId"]);
                    password = Convert.ToString(reader["password"]);
                    employeeNr = Convert.ToInt32(reader["employeeNr"]);

                    emps.Add(new Employee(name, lastName, jobId, password, employeeNr));
                }



            }
            catch
            {
                MessageBox.Show("error while loading the students");
            }
            finally
            {
                connection.Close();
            }
            return emps;
        }

        public Employee GetEmployee(int employeeNr)
        {

            String sql = "SELECT employeeName, surname, positionId, password FROM employee WHERE employee.employeeNr = @empNr ;";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@empNr", employeeNr);

            //On internet you also see a solution like:
            // String sql = "INSERT INTO StudentTable VALUES (" +
            //     "'" + name + "'," + number  + "," + creditpoints + ")";
            //Be aware of sql-injection!
            Employee emp = null;
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                String name;
                string lastName;
                string jobId;
                string password;

                while (reader.Read())
                {
                    name = Convert.ToString(reader["employeeName"]);
                    lastName = Convert.ToString(reader["surname"]);
                    jobId = Convert.ToString(reader["positionId"]);
                    password = Convert.ToString(reader["password"]);

                    emp = new Employee(name, lastName, jobId, password, employeeNr);
                }



            }
            catch
            {
                MessageBox.Show("error while loading the students");
            }
            finally
            {
                connection.Close();
            }
            return emp;
        }

        public int AddEmployee(string firstName, string lastName, string jobId, Shop shop, LoanStand loanStand)
        {

            int nrOfRecordsChanged;
            String sql = "INSERT INTO employee (employeeName, surname, positionId, password) VALUES (@employeeName, @surname, @possitionId, @password )";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@employeeName", firstName);
            command.Parameters.AddWithValue("@surname", lastName);
            command.Parameters.AddWithValue("@possitionId", jobId);
            command.Parameters.AddWithValue("@password", GetRandomAlphanumericString(6));



            try
            {
                connection.Open();
                nrOfRecordsChanged = command.ExecuteNonQuery();

                sql = "SELECT MAX(employee.employeeNr) as EmpNr FROM employee where employeeName = @employeeName";
                command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@employeeName", firstName);
                int empNr = (int)command.ExecuteScalar();

                if (jobId.Contains("s"))
                {
                    sql = "INSERT INTO employee_place (EmployeeemployeeNr, PlaceplaceId) VALUES (@empNr, @placeId)";
                    command = new MySqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@empNr", empNr);
                    command.Parameters.AddWithValue("@placeId", shop.ID);
                    command.ExecuteNonQuery();
                }
                if (jobId.Contains("l"))
                {
                    sql = "INSERT INTO employee_loanstand (employeeNr, loanStandId) VALUES (@empNr, @loanStandId);";
                    command = new MySqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@empNr", empNr);
                    command.Parameters.AddWithValue("@loanStandId", loanStand.ID);
                    command.ExecuteNonQuery();
                }


            }
            catch
            {
                nrOfRecordsChanged = -1; //which means the try-block was not executed succesfully, so  . . .
            }
            finally
            {
                connection.Close();

            }
            return nrOfRecordsChanged;
        }
        public Visitor CreateTemporaryAccount(string _email)
        {
            Visitor vis = null;
            String sql = "INSERT INTO account (name, surname, email, password, bankAccountNr) VALUES (@name, @surname, @email, @password, @bankAccount )";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@name", "temp");
            command.Parameters.AddWithValue("@surname", "temp");
            command.Parameters.AddWithValue("@email", _email);
            command.Parameters.AddWithValue("@password", GetRandomAlphanumericString(6));
            command.Parameters.AddWithValue("@bankAccount", "temp");

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
                sql = "INSERT INTO visitor (accountEmail) VALUES (@email)";
                command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@email", _email);
                command.ExecuteNonQuery();
                sql = "SELECT account.name as Name, account.surname as Surname, account.email as Email, visitor.ticketNr as Ticket,visitor.balance as Balance, visitor.isScanned as IsScanned FROM account, visitor WHERE account.email = visitor.accountEmail AND account.email = @email";
                command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@email", _email);
                MySqlDataReader reader = command.ExecuteReader();

                String name;
                string lastName;
                int ticketNr;
                int balance;
                bool isScanned;

                while (reader.Read())
                {
                    name = Convert.ToString(reader["Name"]);
                    lastName = Convert.ToString(reader["Surname"]);
                    balance = Convert.ToInt32(reader["Balance"]);
                    ticketNr = Convert.ToInt32(reader["Ticket"]);
                    isScanned = Convert.ToBoolean(reader["IsScanned"]);


                    vis = new Visitor(name, lastName, ticketNr, _email, balance, isScanned);
                }
            }
            catch
            {
                throw new Exception("There is already a account linked to this account");
            }
            finally
            {
                connection.Close();

            }


            return vis;
        }

        public int SetRfidTag(string tag, int ticketNr)
        {
            String sql = "UPDATE visitor SET isScanned = 1, RFIDCode = @rfidCode, whenScanned = CURRENT_TIMESTAMP WHERE visitor.ticketNr = @ticketNr AND RFIDCode is NULL";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@ticketNr", ticketNr);
            command.Parameters.AddWithValue("@rfidCode", tag);
            int rowsAffected;
            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch
            {
                throw new Exception("There is already a account linked to this rfid code");
            }
            finally
            {
                connection.Close();

            }


            return rowsAffected;
        }
        public Visitor GetVisitor(int ticketNr)
        {

            String sql = "SELECT account.name as Name, account.surname as Surname, account.email as Email, visitor.balance as Balance, visitor.isScanned as IsScanned FROM account, visitor WHERE account.email = visitor.accountEmail AND visitor.ticketNr = @ticketNr ;";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@ticketNr", ticketNr);

            //On internet you also see a solution like:
            // String sql = "INSERT INTO StudentTable VALUES (" +
            //     "'" + name + "'," + number  + "," + creditpoints + ")";
            //Be aware of sql-injection!
            Visitor vis = null;
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                String name;
                string lastName;
                string email;
                int balance;
                bool isScanned;

                while (reader.Read())
                {
                    name = Convert.ToString(reader["Name"]);
                    lastName = Convert.ToString(reader["Surname"]);
                    email = Convert.ToString(reader["Email"]);
                    balance = Convert.ToInt32(reader["Balance"]);
                    isScanned = Convert.ToBoolean(reader["IsScanned"]);


                    vis = new Visitor(name, lastName, ticketNr, email, balance, isScanned);

                }



            }
            catch
            {
                MessageBox.Show("error while loading the students");
            }
            finally
            {
                connection.Close();
            }
            return vis;
        }
        public Visitor GetVisitor(string rfidCode)
        {

            String sql = "SELECT account.name as Name, account.surname as Surname, account.email as Email, visitor.balance as Balance,visitor.ticketNr as TicketNr, visitor.isScanned as IsScanned FROM account, visitor WHERE account.email = visitor.accountEmail AND visitor.RFIDCode = @rfidCode ;";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@rfidCode", rfidCode);

            //On internet you also see a solution like:
            // String sql = "INSERT INTO StudentTable VALUES (" +
            //     "'" + name + "'," + number  + "," + creditpoints + ")";
            //Be aware of sql-injection!
            Visitor vis = null;
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                String name;
                string lastName;
                string email;
                int balance;
                bool isScanned;
                int ticketNr;

                while (reader.Read())
                {
                    name = Convert.ToString(reader["Name"]);
                    lastName = Convert.ToString(reader["Surname"]);
                    email = Convert.ToString(reader["Email"]);
                    balance = Convert.ToInt32(reader["Balance"]);
                    isScanned = Convert.ToBoolean(reader["IsScanned"]);
                    ticketNr = Convert.ToInt32(reader["TicketNr"]);


                    vis = new Visitor(name, lastName, ticketNr, email, balance, isScanned);
                }



            }
            catch
            {
                throw new Exception("Couldnt load visitor");
            }
            finally
            {
                connection.Close();
            }
            return vis;
        }

        public int GetAllVisitors()
        {
            int allVisitors = 0;
            String sql = "Select count(*) as total From visitor";
            MySqlCommand command = new MySqlCommand(sql, connection);

            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                int total = 0;
                while (reader.Read())
                {
                    total = Convert.ToInt32(reader["total"]);
                }
                allVisitors = total;
            }
            catch
            {
                MessageBox.Show("error while loading the visitors");
            }
            finally
            {
                connection.Close();
            }
            return allVisitors;
        }

        public int SumOfAllVisitorBalance()
        {
            int sumOfBalance = 0;
            String sql = "Select sum(balance) as total From visitor";
            MySqlCommand command = new MySqlCommand(sql, connection);

            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                int total = 0;
                while (reader.Read())
                {
                    total = Convert.ToInt32(reader["total"]);
                }
                sumOfBalance = total;
            }
            catch
            {
                MessageBox.Show("error while loading the visitors");
            }
            finally
            {
                connection.Close();
            }
            return sumOfBalance;
        }

        public int TotalMoneyEarned()
        {
            int sumOfEarnedMoney = 0;
            String sql = "Select sum(balance) as total From visitor";
            MySqlCommand command = new MySqlCommand(sql, connection);

            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                int total = 0;
                while (reader.Read())
                {
                    total = Convert.ToInt32(reader["total"]);
                }
                sumOfEarnedMoney = total;
            }
            catch
            {
                MessageBox.Show("error while loading the sumOfEarnedMoney");
            }
            finally
            {
                connection.Close();
            }
            return sumOfEarnedMoney;
        }

        public int TotalMoneySpentByVisitor()
        {
            int sumOfSpentMoney = 0;
            String sql = "SELECT count(v.ticketNr) * 55 + sum(p.amount) + sum(l.amount) as total FROM purchaise p join visitor v on v.ticketNr = p.ticketNr join loan l on l.ticketNr = v.ticketNr";
            MySqlCommand command = new MySqlCommand(sql, connection);

            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                int total = 0;
                while (reader.Read())
                {
                    total = Convert.ToInt32(reader["total"]);
                }
                sumOfSpentMoney = total;
            }
            catch
            {
                MessageBox.Show("error while loading the sumOfEarnedMoney");
            }
            finally
            {
                connection.Close();
            }
            return sumOfSpentMoney;
        }

        public int AmountOfBookedCampingSpots()
        {
            int amountOfBookedCampingSpots = 0;
            String sql = "Select sum(reservedPlaces) as total From campspot";
            MySqlCommand command = new MySqlCommand(sql, connection);

            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                int total = 0;
                while (reader.Read())
                {
                    total = Convert.ToInt32(reader["total"]);
                }
                amountOfBookedCampingSpots = total;
            }
            catch
            {
                MessageBox.Show("error while loading the campspots");
            }
            finally
            {
                connection.Close();
            }
            return amountOfBookedCampingSpots;
        }

        public int AmountOfFreeCampSpaces()
        {
            int amountOfFreeCampingSpots = 0;
            String sql = "Select sum(6 - reservedPlaces) as total From campspot";
            MySqlCommand command = new MySqlCommand(sql, connection);

            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                int total = 0;
                while (reader.Read())
                {
                    total = Convert.ToInt32(reader["total"]);
                }
                amountOfFreeCampingSpots = total;
            }
            catch
            {
                MessageBox.Show("error while loading the campspots");
            }
            finally
            {
                connection.Close();
            }
            return amountOfFreeCampingSpots;

        }

        public int AmountEarnedPerShop(int placeId)
        {
            String sql = "SELECT SUM(p.qauntity*i.price) as total FROM purchase_item p JOIN purchaisable c On p.itemId = c.itemId JOIN item i ON i.itemId = c.itemId And c.placeId = @placeId ;";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@placeId", placeId);

            int amount = 0;
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    amount = Convert.ToInt32(reader["total"]);
                }
            }
            catch
            {
                amount = 0;
            }
            finally
            {
                connection.Close();
            }
            return amount;
        }

        public int AmountEarnedPerLoanStand(int loanStandId)
        {
            String sql = "SELECT SUM(amount) as total FROM loan WHERE loanStandId = @loanStandId ;";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@loanStandId", loanStandId);

            int amount = 0;
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    amount = Convert.ToInt32(reader["total"]);
                }
            }
            catch
            {
                amount = 0;
            }
            finally
            {
                connection.Close();
            }
            return amount;
        }

        public int AmountEarnedPerItem(int itemId)
        {
            String sql = "SELECT SUM(p.qauntity*i.price) as total FROM purchase_item p JOIN item i ON i.itemId = p.itemId And p.itemId = @itemId;";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@itemId", itemId);

            int amount = 0;
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    amount = Convert.ToInt32(reader["total"]);
                }
            }
            catch
            {
                amount = 0;
            }
            finally
            {
                connection.Close();
            }
            return amount;
        }
        public int AmountEarnedPerLoanable(int loanitemId)
        {
            String sql = "SELECT SUM(amount) as total FROM loan WHERE loanitemId = @loanitemId;";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@loanitemId", loanitemId);

            int amount = 0;
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    amount = Convert.ToInt32(reader["total"]);
                }
            }
            catch
            {
                amount = 0;
            }
            finally
            {
                connection.Close();
            }
            return amount;
        }
        public List<string> GetTransactions(int ticketNr)
        {
            String sql = "SELECT date_format(p.dateOfTrans, '%a') as Date, p.amount as Amount, pl.name as Place " +
                "FROM purchaise p JOIN visitor v " +
                "On v.ticketNr = p.ticketNr " +
                "JOIN purchase_item b On p.purchaseNr = b.purchaseNr " +
                "JOIN purchaisable c On c.itemId = b.itemId " +
                "JOIN place pl On pl.placeId = c.placeId " +
                "AND v.ticketNr = @ticketNr";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@ticketNr", ticketNr);

            List<string> transactions = new List<string>();
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                string date;
                string Amount;
                string Place;
                while (reader.Read())
                {
                    date = Convert.ToString(reader["Date"]);
                    Amount = Convert.ToString(reader["Amount"]);
                    Place = Convert.ToString(reader["Place"]);

                    transactions.Add($"{date} spent {Amount} at {Place}");
                }
                connectionForTopUp.Open();
                sql = "SELECT date_format(tu.dateOfTrans, '%a') as Date, tu.amount as Amount FROM topup tu WHERE tu.ticketNr = @ticketNr";
                command = new MySqlCommand(sql, connectionForTopUp);
                command.Parameters.AddWithValue("@ticketNr", ticketNr);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    date = Convert.ToString(reader["Date"]);
                    Amount = Convert.ToString(reader["Amount"]);

                    transactions.Add($"{date} spent {Amount}(topUp)");
                }
                connectionForLoanable.Open();
                sql = "SELECT date_format(l.dateOfTrans, '%a') as Date, l.amount as Amount, ls.name as Place FROM loan l join loanstand ls on l.loanStandId = ls.loanStandId WHERE l.ticketNr = @ticketNr";
                command = new MySqlCommand(sql, connectionForLoanable);
                command.Parameters.AddWithValue("@ticketNr", ticketNr);
                reader = command.ExecuteReader();
                while (reader.Read())
                {
                    date = Convert.ToString(reader["Date"]);
                    Amount = Convert.ToString(reader["Amount"]);
                    Place = Convert.ToString(reader["Place"]);

                    transactions.Add($"{date} spent {Amount} at {Place}");
                }
            }
            catch
            {
                MessageBox.Show("");
            }
            finally
            {
                connection.Close();
                connectionForTopUp.Close();
                connectionForLoanable.Close();
            }
            return transactions;
        }

        public CampingSpot GetCampingSpotByRFID(string rfid)
        {
            String sql = "SELECT c.reservedPlaces as places, a.name as groupleader, c.name as Name, c.campSpotId as spotId, v.isCampPayed as paymentStatus FROM campspot c JOIN visitor v ON c.campSpotId = v.campSpotId JOIN account a ON v.accountEmail = a.email WHERE v.RFIDCode = @rfidCode";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@rfidCode", rfid);
            CampingSpot spot = null;
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                int amountOfParticipants;
                string groupLeader;
                int spotId;
                string name;
                bool paymentStatus;

                while (reader.Read())
                {
                    groupLeader = Convert.ToString(reader["groupleader"]);
                    spotId = Convert.ToInt32(reader["spotId"]);
                    name = Convert.ToString(reader["Name"]);
                    paymentStatus = Convert.ToBoolean(reader["paymentStatus"]);
                    amountOfParticipants = Convert.ToInt32(reader["places"]);

                    spot = new CampingSpot(groupLeader, spotId, amountOfParticipants, paymentStatus, name);
                }



            }
            catch
            {
                throw new Exception("Something went wrong with database, camping spot");

            }
            finally
            {
                connection.Close();
            }
            return spot;
        }
        public int PayForCampingSpot(Visitor visitor, CampingSpot campingSpot)
        {
            int total = (campingSpot.AmountOfParticpants * 20) + 20;
            int purchaseNr = 0;
            int check = 0;
            if (visitor.Balance >= total)
            {
                String sql = "UPDATE visitor SET balance = balance - @total WHERE ticketNr = @ticketNr";
                MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@total", total);
                command.Parameters.AddWithValue("@ticketNr", visitor.TicketNr);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();

                    sql = "INSERT INTO purchaise (ticketNr, dateOfTrans, purchaseNr, amount) VALUES (@ticketNr, CURRENT_TIMESTAMP, NULL, @total)";
                    command = new MySqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@ticketNr", visitor.TicketNr);
                    command.Parameters.AddWithValue("@total", total);
                    check = command.ExecuteNonQuery();

                    sql = "SELECT MAX(purchaise.purchaseNr) as purchaseNr FROM purchaise WHERE ticketNr = @ticketNr";
                    command = new MySqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@ticketNr", visitor.TicketNr);
                    purchaseNr = (int)command.ExecuteScalar();


                    sql = "UPDATE visitor SET isCampPayed = 1 WHERE visitor.ticketNr = @ticketNr";
                    command = new MySqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@ticketNr", visitor.TicketNr);
                    command.ExecuteNonQuery();

                    sql = "INSERT INTO purchase_campspot (campSpotId, purchaseNr) VALUES (@campSpotId, @purchaseNr)";
                    command = new MySqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@campSpotId", campingSpot.Spot);
                    command.Parameters.AddWithValue("@purchaseNr", purchaseNr);

                    command.ExecuteNonQuery();
                }
                catch
                {
                    //TODO make exception
                    throw new Exception("Something went wrong with updating camping tables");
                }
                finally
                {
                    connection.Close();
                }
            }

            else
            {
                //There is not enough money
                throw new Exception("Insufficient balance");
            }
            return check;
        }
        public int DeleteEmployee(int employeeNr)
        {
            int check = 1;
            String sql = "DELETE FROM employee_place WHERE employee_place.EmployeeemployeeNr = @empNr";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@empNr", employeeNr);

            //On internet you also see a solution like:
            // String sql = "INSERT INTO StudentTable VALUES (" +
            //     "'" + name + "'," + number  + "," + creditpoints + ")";
            //Be aware of sql-injection!
            try
            {
                connection.Open();
                command.ExecuteNonQuery();

                sql = "DELETE FROM employee_loanstand WHERE employee_loanstand.employeeNr = @empNr";
                command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@empNr", employeeNr);
                command.ExecuteNonQuery();

                sql = "DELETE FROM employee WHERE employeeNr = @empNr";
                command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@empNr", employeeNr);
                command.ExecuteNonQuery();


            }
            catch
            {
                MessageBox.Show("error while loading the students");
                check = 0;
            }
            finally
            {
                connection.Close();
            }
            return check;
        }

        public int ChangeJobId(Job job, int employeeNr)
        {
            int check = 1;
            String sql = "UPDATE employee SET positionId = @jobId WHERE employee.employeeNr = @empNr";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@jobId", job.Id);
            command.Parameters.AddWithValue("@empNr", employeeNr);

            //On internet you also see a solution like:
            // String sql = "INSERT INTO StudentTable VALUES (" +
            //     "'" + name + "'," + number  + "," + creditpoints + ")";
            //Be aware of sql-injection!
            try
            {
                connection.Open();
                command.ExecuteNonQuery();

            }
            catch
            {
                check = 0;
            }
            finally
            {
                connection.Close();
            }
            return check;
        }

        public List<Shop> GetShops()
        {
            String sql = "Select * From place";
            MySqlCommand command = new MySqlCommand(sql, connection);

            List<Shop> temp = new List<Shop>();
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                String name;
                int id;
                while (reader.Read())
                {
                    name = Convert.ToString(reader["name"]);
                    id = Convert.ToInt32(reader["placeId"]);
                    temp.Add(new Shop(id, name));
                }
            }
            catch
            {
                MessageBox.Show("error while loading the students");
            }
            finally
            {
                connection.Close();
            }
            return temp;
        }

        public List<LoanStand> GetLoanStands()
        {
            String sql = "Select * From loanstand";
            MySqlCommand command = new MySqlCommand(sql, connection);

            List<LoanStand> temp = new List<LoanStand>();
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                String name;
                int id;
                while (reader.Read())
                {
                    name = Convert.ToString(reader["name"]);
                    id = Convert.ToInt32(reader["loanStandId"]);
                    temp.Add(new LoanStand(id, name));
                }
            }
            catch
            {
                MessageBox.Show("error while loading the students");
            }
            finally
            {
                connection.Close();
            }
            return temp;
        }

        public int StartLoan(Visitor visitor, List<LoanStandItem> loanStandItems, int placeId, int days)
        {
            double total = (int)loanStandItems.Sum(x => x.SubTotal * days);
            int check = 0;
            if (visitor.Balance >= total)
            {
                String sql = "UPDATE visitor SET balance = balance - @total WHERE ticketNr = @ticketNr";
                MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@total", total);
                command.Parameters.AddWithValue("@ticketNr", visitor.TicketNr);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();

                    foreach (LoanStandItem li in loanStandItems)
                    {
                        sql = "UPDATE loanable SET stock = stock - @amount WHERE loanitemId = @Id AND loanStandId = @place";
                        command = new MySqlCommand(sql, connection);
                        command.Parameters.AddWithValue("@amount", li.Quantity);
                        command.Parameters.AddWithValue("@Id", li.ID);
                        command.Parameters.AddWithValue("@place", placeId);
                        command.ExecuteNonQuery();

                        sql = "INSERT INTO loan (ticketNr, dateOfTrans, loanitemId, loanStandId, startTime, endTime, isReturned, amount, qauntity) VALUES (@ticketNr, CURRENT_TIMESTAMP, @Id, @place, CURRENT_TIMESTAMP , now() + interval @day day, '0', @amount, @qauntity);";
                        command = new MySqlCommand(sql, connection);
                        command.Parameters.AddWithValue("@amount", li.SubTotal * days);
                        command.Parameters.AddWithValue("@Id", li.ID);
                        command.Parameters.AddWithValue("@place", placeId);
                        command.Parameters.AddWithValue("@day", days);
                        command.Parameters.AddWithValue("@qauntity", li.Quantity);
                        command.Parameters.AddWithValue("@ticketNr", visitor.TicketNr);
                        command.ExecuteNonQuery();
                    }



                }
                catch
                {
                    //TODO make exception
                    check = -1;
                }
                finally
                {
                    connection.Close();
                }
            }
            else
            {
                //There is not enough money
                throw new Exception("Insufficient balance");
            }
            return check;
        }

        public List<Loan> GetLoans(Visitor visitor, int loanStand)

        {
            String sql = "SELECT l.LoanNr AS Id, li.name AS Name, li.loanitemId As ItemId, l.loanStandId AS LoanStand, ls.name As LoanstandName, l.startTime As StartDate, l.endTime AS EndDate, l.amount As Total, l.qauntity As Qauntity FROM loan l JOIN visitor v ON l.ticketNr = v.ticketNr JOIN loanitem li ON li.loanitemId = l.loanitemId JOIN loanstand ls ON ls.loanStandId = l.loanStandId WHERE isReturned = 0 AND v.ticketNr = @ticketNr AND l.loanStandId = @loanStandId";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@ticketNr", visitor.TicketNr);
            command.Parameters.AddWithValue("@loanStandId", loanStand);

            List<Loan> Loans = new List<Loan>();
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                int id;
                int loanStandId;
                String name;
                string loanStandName;
                DateTime start;
                DateTime end;
                double total;
                int qauntity;
                int itemId;

                while (reader.Read())
                {
                    id = Convert.ToInt32(reader["Id"]);
                    loanStandId = Convert.ToInt32(reader["LoanStand"]);
                    name = Convert.ToString(reader["Name"]);
                    loanStandName = Convert.ToString(reader["LoanstandName"]);
                    start = DateTime.Parse(Convert.ToString(reader["StartDate"]));
                    end = DateTime.Parse(Convert.ToString(reader["EndDate"]));
                    total = Convert.ToDouble(reader["Total"]);
                    qauntity = Convert.ToInt32(reader["Qauntity"]);
                    itemId = Convert.ToInt32(reader["ItemId"]);

                    Loans.Add(new Loan(id, loanStandId, name, loanStandName, start, end, total, qauntity, itemId));
                }



            }
            catch
            {
                throw new Exception("Couldnt load visitor");
            }
            finally
            {
                connection.Close();
            }
            return Loans;
        }

        public List<Loan> GetLoans(Visitor visitor)

        {
            String sql = "SELECT l.LoanNr AS Id, li.name AS Name, li.loanitemId As ItemId, l.loanStandId AS LoanStand, ls.name As LoanstandName, l.startTime As StartDate, l.endTime AS EndDate, l.amount As Total, l.qauntity As Qauntity FROM loan l JOIN visitor v ON l.ticketNr = v.ticketNr JOIN loanitem li ON li.loanitemId = l.loanitemId JOIN loanstand ls ON ls.loanStandId = l.loanStandId WHERE isReturned = 0 AND v.ticketNr = @ticketNr";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@ticketNr", visitor.TicketNr);

            List<Loan> Loans = new List<Loan>();
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                int id;
                int loanStandId;
                String name;
                string loanStandName;
                DateTime start;
                DateTime end;
                double total;
                int qauntity;
                int itemId;

                while (reader.Read())
                {
                    id = Convert.ToInt32(reader["Id"]);
                    loanStandId = Convert.ToInt32(reader["LoanStand"]);
                    name = Convert.ToString(reader["Name"]);
                    loanStandName = Convert.ToString(reader["LoanstandName"]);
                    start = DateTime.Parse(Convert.ToString(reader["StartDate"]));
                    end = DateTime.Parse(Convert.ToString(reader["EndDate"]));
                    total = Convert.ToDouble(reader["Total"]);
                    qauntity = Convert.ToInt32(reader["Qauntity"]);
                    itemId = Convert.ToInt32(reader["ItemId"]);

                    Loans.Add(new Loan(id, loanStandId, name, loanStandName, start, end, total, qauntity, itemId));
                }



            }
            catch
            {
                throw new Exception("Couldnt load visitor");
            }
            finally
            {
                connection.Close();
            }
            return Loans;
        }

        public void ReturnLoanedItems(List<Loan> loans, Visitor visitor)
        {
            String sql;
            MySqlCommand command;
            try
            {
                connection.Open();
                foreach (Loan loan in loans)
                {
                    if (loan.IsOverdue)
                    {
                        int days = ((TimeSpan)(loan.EndDate - loan.StartDate)).Days;
                        double pricePerDay = (loan.Total / loan.Qauntity) / days;
                        int daysOverdue = ((TimeSpan)(DateTime.Now - loan.EndDate)).Days;
                        double extra = pricePerDay * daysOverdue;

                        if (visitor.Balance >= extra)
                        {
                            sql = "UPDATE visitor SET balance = balance - @total WHERE ticketNr = @ticketNr";
                            command = new MySqlCommand(sql, connection);
                            command.Parameters.AddWithValue("@total", extra);
                            command.Parameters.AddWithValue("@ticketNr", visitor.TicketNr);
                            command.ExecuteNonQuery();

                            sql = "UPDATE loanable SET stock = stock + @amount WHERE loanitemId = @Id AND loanStandId = @place";
                            command = new MySqlCommand(sql, connection);
                            command.Parameters.AddWithValue("@amount", loan.Qauntity);
                            command.Parameters.AddWithValue("@Id", loan.ItemId);
                            command.Parameters.AddWithValue("@place", loan.LoanStandID);
                            command.ExecuteNonQuery();

                            sql = "UPDATE loan SET isReturned = 1, amount = amount + @amount  WHERE loan.LoanNr = @loanNr;";
                            command = new MySqlCommand(sql, connection);
                            command.Parameters.AddWithValue("@loanNr", loan.ID);
                            command.Parameters.AddWithValue("@amount", extra);
                            command.ExecuteNonQuery();
                        }
                        else
                        {
                            throw new Exception("Insufficient balance");
                        }


                    }
                    else
                    {

                        sql = "UPDATE loanable SET stock = stock + @amount WHERE loanitemId = @Id AND loanStandId = @place";
                        command = new MySqlCommand(sql, connection);
                        command.Parameters.AddWithValue("@amount", loan.Qauntity);
                        command.Parameters.AddWithValue("@Id", loan.ItemId);
                        command.Parameters.AddWithValue("@place", loan.LoanStandID);
                        command.ExecuteNonQuery();

                        sql = "UPDATE loan SET isReturned = 1 WHERE loan.LoanNr = @loanNr;";
                        command = new MySqlCommand(sql, connection);
                        command.Parameters.AddWithValue("@loanNr", loan.ID);
                        command.ExecuteNonQuery();
                    }

                }
            }
            catch
            {
                //TODO make exception
                throw new Exception("Something went wrong");
            }
            finally
            {
                connection.Close();
            }


        }

        public int CreatePurchase(Visitor visitor, List<ShopItem> shopItems, int placeId)
        {
            int total = (int)shopItems.Sum(x => x.SubTotal);
            int purchaseNr = 0;
            int check = 0;
            if (visitor.Balance >= total)
            {
                String sql = "UPDATE visitor SET balance = balance - @total WHERE ticketNr = @ticketNr";
                MySqlCommand command = new MySqlCommand(sql, connection);
                command.Parameters.AddWithValue("@total", total);
                command.Parameters.AddWithValue("@ticketNr", visitor.TicketNr);
                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();

                    sql = "INSERT INTO purchaise (ticketNr, dateOfTrans, purchaseNr, amount) VALUES (@ticketNr, CURRENT_TIMESTAMP, NULL, @total)";
                    command = new MySqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@ticketNr", visitor.TicketNr);
                    command.Parameters.AddWithValue("@total", total);
                    check = command.ExecuteNonQuery();

                    sql = "SELECT MAX(purchaise.purchaseNr) as purchaseNr FROM purchaise WHERE ticketNr = @ticketNr";
                    command = new MySqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@ticketNr", visitor.TicketNr);
                    purchaseNr = (int)command.ExecuteScalar();

                    foreach (ShopItem s in shopItems)
                    {
                        sql = "UPDATE purchaisable SET StartQuantity = StartQuantity - @amount WHERE itemId = @Id AND placeId = @place";
                        command = new MySqlCommand(sql, connection);
                        command.Parameters.AddWithValue("@amount", s.Quantity);
                        command.Parameters.AddWithValue("@Id", s.ID);
                        command.Parameters.AddWithValue("@place", placeId);
                        command.ExecuteNonQuery();

                        sql = "INSERT INTO purchase_item (itemId, purchaseNr, qauntity) VALUES (@itemId, @purchaseNr, @amount)";
                        command = new MySqlCommand(sql, connection);
                        command.Parameters.AddWithValue("@itemId", s.ID);
                        command.Parameters.AddWithValue("@purchaseNr", purchaseNr);
                        command.Parameters.AddWithValue("@amount", s.Quantity);

                        command.ExecuteNonQuery();
                    }



                }
                catch
                {
                    //TODO make exception
                    check = -1;
                }
                finally
                {
                    connection.Close();
                }
            }
            else
            {
                //There is not enough money
                throw new Exception("Insufficient balance");
            }
            return check;
        }

        #region Converter
        public int CreateTopUps(List<LogLine> logLines)
        {
            int nrOfRecordsChanged = 0;
            String sql;
            MySqlCommand command;
            try
            {
                connection.Open();
                foreach (LogLine line in logLines)
                {
                    string[] words = line.Line.Split(' ');
                    int ticketNr = Convert.ToInt32(words[0]);
                    double amount = Convert.ToDouble(words[1]);
                    sql = "INSERT INTO topup (topUpNr, ticketNr, dateOfTrans, amount) VALUES (NULL, @ticketNr, CURRENT_TIMESTAMP, @amount)";
                    command = new MySqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@ticketNr", ticketNr);
                    command.Parameters.AddWithValue("@amount", amount);
                    command.ExecuteNonQuery();


                    sql = "UPDATE visitor SET balance = balance + @total WHERE ticketNr = @ticketNr";
                    command = new MySqlCommand(sql, connection);
                    command.Parameters.AddWithValue("@total", amount);
                    command.Parameters.AddWithValue("@ticketNr", ticketNr);
                    command.ExecuteNonQuery();
                }
            }
            catch
            {
                throw new Exception("Something went wrong with database");
            }
            finally
            {
                connection.Close();

            }
            return nrOfRecordsChanged;
        }
        #endregion 

        #region Password Generation
        public static string GetRandomAlphanumericString(int length)
        {
            const string alphanumericCharacters =
                //"ABCDEFGHJKMNPQRSTUVWXYZ" +
                "abcdefghjkmnpqrstuvwxyz" +
                "23456789";
            return GetRandomString(length, alphanumericCharacters);
        }

        public static string GetRandomString(int length, IEnumerable<char> characterSet)
        {
            if (length < 0)
                throw new ArgumentException("length must not be negative", "length");
            if (length > int.MaxValue / 8) // 250 million chars ought to be enough for anybody
                throw new ArgumentException("length is too big", "length");
            if (characterSet == null)
                throw new ArgumentNullException("characterSet");
            var characterArray = characterSet.Distinct().ToArray();
            if (characterArray.Length == 0)
                throw new ArgumentException("characterSet must not be empty", "characterSet");

            var bytes = new byte[length * 8];
            new RNGCryptoServiceProvider().GetBytes(bytes);
            var result = new char[length];
            for (int i = 0; i < length; i++)
            {
                ulong value = BitConverter.ToUInt64(bytes, i * 8);
                result[i] = characterArray[value % (uint)characterArray.Length];
            }
            return new string(result);
        }

        #endregion
    }

}

