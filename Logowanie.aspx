<%@ Page Title="Logowanie" Language="C#" MasterPageFile="~/Szablon.master" AutoEventWireup="true" CodeFile="Logowanie.aspx.cs" Inherits="Logowanie" %>

<asp:Content ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ContentPlaceHolderID="MainContent" runat="server">
    <div id="Tresc">
        <div>
            <div id="Logowanie">
                <div>
                    <div class="naglowek">Logowanie</div>
                    <asp:Label runat="server" id="Blad"></asp:Label>
                    <div class="wiersz">
                        <div class="pole"><label for="NazwaInput">Nazwa użytkownika: </label><input id="NazwaInput" name="NazwaInput" type="text" runat="server" /></div>
                    </div>
                    <div class="wiersz">
                        <div class="pole"><label for="HasloInput">Hasło: </label><input id="HasloInput" name="HasloInput" type="password" runat="server" /></div>
                    </div>
                    <div><input id="Submit1" type="submit" value="Zaloguj" /></div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

