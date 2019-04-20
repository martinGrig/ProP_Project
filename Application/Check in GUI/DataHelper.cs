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

        public int AddAStudent(int number, string name, int creditpoints)
        {   //Probably you expected a return-value of type bool:
            //true if the query was executed succesfully and false otherwise.
            //But what if you executed a delete-query? Or an update-query?
            //The return-value is teh number of records affected.

            String sql = "INSERT INTO StudentTable VALUES (@name, @number, @cp )";
            MySqlCommand command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@number", number);
            command.Parameters.AddWithValue("@cp", creditpoints);

            //On internet you also see a solution like:
            // String sql = "INSERT INTO StudentTable VALUES (" +
            //     "'" + name + "'," + number  + "," + creditpoints + ")";
            //Be aware of sql-injection!

            try
            {
                connection.Open();
                int nrOfRecordsChanged = command.ExecuteNonQuery();
                return nrOfRecordsChanged;
            }
            catch
            {
                return -1; //which means the try-block was not executed succesfully, so  . . .
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
