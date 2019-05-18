using System;
using System.Collections.Generic;
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

        public DataHelper()
        {
            String connectionInfo = "server=studmysql01.fhict.local;" +//the hera-server
                                    "database=dbi410102;" +
                                    "user id=dbi410102;" +
                                    "password=prop17;" +
                                    "connect timeout=30;";

            connection = new MySqlConnection(connectionInfo);
        }

        public  bool IsServerConnected()
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
        {   //Probably you expected a return-value of type bool:
            //true if the query was executed succesfully and false otherwise.
            //But what if you executed a delete-query? Or an update-query?
            //The return-value is teh number of records affected.

            String sql = "SELECT i.name as Name,i.price as Price, p.StartQuantity as Stock, p.IsFood as Food, i.itemId as Id FROM item i JOIN purchaisable p ON i.itemId = p.itemId JOIN place pl On p.placeId = pl.placeId JOIN employee_place ep ON pl.placeId = ep.PlaceplaceId JOIN employee e ON ep.EmployeeemployeeNr = e.employeeNr WHERE e.employeeNr = @empNr;";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@empNr", employeeNr);

            //On internet you also see a solution like:
            // String sql = "INSERT INTO StudentTable VALUES (" +
            //     "'" + name + "'," + number  + "," + creditpoints + ")";
            //Be aware of sql-injection!

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
                    temp.Add(new ShopItem(name, price, stock, $"Images/{name}.png",id, isFood));
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

        public int AddEmployee(string firstName, string lastName, string jobId, string password)
        {   //Probably you expected a return-value of type bool:
            //true if the query was executed succesfully and false otherwise.
            //But what if you executed a delete-query? Or an update-query?
            //The return-value is teh number of records affected.
            int nrOfRecordsChanged;
            String sql = "INSERT INTO employee (employeeName, surname, positionId, password) VALUES (@employeeName, @surname, @possitionId, @password )";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@employeeName", firstName);
            command.Parameters.AddWithValue("@surname", lastName);
            command.Parameters.AddWithValue("@possitionId", jobId);
            command.Parameters.AddWithValue("@password", password);

            //On internet you also see a solution like:
            // String sql = "INSERT INTO StudentTable VALUES (" +
            //     "'" + name + "'," + number  + "," + creditpoints + ")";
            //Be aware of sql-injection!

            try
            {
                connection.Open();
                nrOfRecordsChanged = command.ExecuteNonQuery();
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

        public CampingSpot GetCampingSpotByRFID(string rfid)
        {
            String sql = "SELECT c.reservedPlaces as places, a.name as groupleader, c.campSpotId as spotId, v.isCampPayed as paymentStatus FROM campspot c JOIN visitor v ON c.campSpotId = v.campSpotId JOIN account a ON v.accountEmail = a.email WHERE v.RFIDCode = @rfidCode";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@rfidCode", rfid);
            CampingSpot spot = null;
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                int amountOfParticipants;
                string groupLeader;
                string spotId;
                bool paymentStatus;

                while (reader.Read())
                {
                    groupLeader = Convert.ToString(reader["groupleader"]);
                    spotId = Convert.ToString(reader["spotId"]);
                    paymentStatus = Convert.ToBoolean(reader["paymentStatus"]);
                    amountOfParticipants = Convert.ToInt32(reader["places"]);

                    spot = new CampingSpot(groupLeader, spotId, amountOfParticipants, paymentStatus);
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
            return spot;
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
                    temp.Add(new Shop(id,name));
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
    }
}

