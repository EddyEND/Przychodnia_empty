using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

public partial class _Default : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        /*string connStr = ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString;
        MySqlConnection conn = new MySqlConnection(connStr);

        if (conn.State != System.Data.ConnectionState.Open)
            try {
                conn.Open();
                MySqlCommand zapytanie = new MySqlCommand("SELECT * FROM users ORDER BY dataRejestracji;", conn);
                MySqlDataReader wynik = zapytanie.ExecuteReader();

                TrescZawartosc.Controls.Clear();

                while (wynik.Read())
                {
                    Label l1 = new Label();
                    for (int i=0; i<wynik.FieldCount; i++){
                        l1.Text += wynik.GetName(i)+ ": " + wynik[i].ToString() + "<br />";
                    }
                    TrescZawartosc.Controls.Add(l1);
                }
                wynik.Close();
            }
            catch (MySqlException ex) {
                throw (ex);
            }*/
    }
}