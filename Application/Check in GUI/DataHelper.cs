using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

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
                                    "password=kingovel1;" +
                                    "connect timeout=30;";

            connection = new MySqlConnection(connectionInfo);
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

        public List<Item> GetItems(int employeeNr)
        {   //Probably you expected a return-value of type bool:
            //true if the query was executed succesfully and false otherwise.
            //But what if you executed a delete-query? Or an update-query?
            //The return-value is teh number of records affected.

            String sql = "SELECT item.name as Name,item.price as Price,itemtype_place.StartQuantity as Stock FROM item, purchaisable,place,itemtype_place,employee_place,employee,itemtype WHERE item.itemId = purchaisable.itemId AND purchaisable.placeId = place.placeId AND itemtype_place.PlaceplaceId = place.placeId And employee_place.PlaceplaceId = place.placeId AND employee.employeeNr = employee_place.EmployeeemployeeNr AND itemtype.itemType = itemtype_place.ItemTypeitemType And purchaisable.itemType = itemtype.itemType AND employee.employeeNr = @empNr ;";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@empNr", employeeNr );

            //On internet you also see a solution like:
            // String sql = "INSERT INTO StudentTable VALUES (" +
            //     "'" + name + "'," + number  + "," + creditpoints + ")";
            //Be aware of sql-injection!

            List<Item> temp;
            temp = new List<Item>();
            try
            {
                connection.Open();
                MySqlDataReader reader = command.ExecuteReader();

                String name;
                int price;
                int stock;
                while (reader.Read())
                {
                    name = Convert.ToString(reader["Name"]);
                    price = Convert.ToInt32(reader["Price"]);
                    stock = Convert.ToInt32(reader["Stock"]);
                    temp.Add(new Item(name,price,stock, "Images/burger.ico"));
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

            String sql = "SELECT employeeName, surname, possitionId, password, employeeNr FROM employees WHERE employee.employeeNr = @empNr ;";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@empNr", employeeNr );

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
                int jobId;
                int password;
                while (reader.Read())
                {
                    name = Convert.ToString(reader["employeeName"]);
                    lastName = Convert.ToString(reader["surname"]);
                    jobId = Convert.ToInt32(reader["possitionId"]);
                    password = Convert.ToInt32(reader["password"]);
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
    }
}
