using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;

public partial class admin_MasterAdmin : System.Web.UI.MasterPage
{
    protected bool sprAdmin(int id)
    {
        string connStr = ConfigurationManager.ConnectionStrings["MySQLConnStr"].ConnectionString;
        MySqlConnection conn = new MySqlConnection(connStr);

        try
        {
            conn.Open();

            string sql = "SELECT typ FROM users WHERE id=@Id;";
            MySqlCommand zapytanie = new MySqlCommand(sql, conn);

            zapytanie.Parameters.Add(new MySqlParameter("@Id", Session["id"].ToString()));

            object wynik = zapytanie.ExecuteScalar();

            if (wynik != null && wynik.ToString() == "A")
            {
                conn.Close();
                return true;
            }

            conn.Close();
        }
        catch (MySqlException ex)
        {
            //Blad.InnerHtml = ex.ToString();
        }

        return false;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack && Request.UrlReferrer != null)
        {
            Session["PrevPage"] = Request.UrlReferrer.ToString();
        }
        
        if (Session["zalogowany"] != null)
        {
            if (!sprAdmin(Convert.ToInt32(Session["id"])))
                Server.Transfer("../Redirect.aspx?a=adminperm&link=admin", true);

            NavbarRejestracja.InnerHtml = "";

            List<String> linki = new List<string>();

            linki.Add("<div>Witaj, " + Session["nazwa"] + ".</div>");
            linki.Add("<div><a href=\"./Default.aspx?action=ustawienia\">Ustawienia ogólne</a></div>");
            linki.Add("<div><a href=\"./Default.aspx?action=aktualnosci\">Aktualności</a></div>");
            linki.Add("<div><a href=\"./Default.aspx?action=uzytkownicy\">Użytkownicy</a></div>");
            linki.Add("<div><a href=\"./Default.aspx?action=wizyty\">Wizyty</a></div>");


            NavbarDol.InnerHtml = "<div>" + String.Join("", linki) + "</div>";
        }
        else {
            Server.Transfer("../Redirect.aspx?a=permission&link=admin", true);
        }

        
    }
}
