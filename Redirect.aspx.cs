using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

public partial class Redirect : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Label l1 = new Label();
        string action = (!string.IsNullOrEmpty(Request.QueryString["a"])) ? Request.QueryString["a"] : "";
        string link = (!string.IsNullOrEmpty(Request.QueryString["link"])) ? Request.QueryString["link"] : "./";

        HtmlMeta metaKey = new HtmlMeta();
        metaKey.HttpEquiv = "Refresh";
        metaKey.Content = "3; url=" + link;

        if (action == "login"){
            Page.Title = "Logowanie";
            l1.Text = "Zalogowano prawidłowo.<br />Teraz nastąpi przeniesienie do poprzedniej lokalizacji.<br /><a href=\"" + link + "\">Kliknij tutaj, jeśli nie chcesz czekać.</a>";
            Page.Header.Controls.Add(metaKey);
        }
        else if (action == "logout"){
            Page.Title = "Wyloguj";
            l1.Text = "Wylogowano prawidłowo.<br />Teraz nastąpi przeniesienie do poprzedniej lokalizacji.<br /><a href=\"" + link + "\">Kliknij tutaj, jeśli nie chcesz czekać.</a>";
            Page.Header.Controls.Add(metaKey);
        }

        
        redirect.Controls.Add(l1);
    }
}