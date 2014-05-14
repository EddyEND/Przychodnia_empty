<%@ Page Title="Rejestrcja" Language="C#" MasterPageFile="~/Szablon.Master" AutoEventWireup="true" CodeFile="Rejestracja.aspx.cs" Inherits="Rejestracja" %>

<asp:Content ContentPlaceholderID="head" runat="server"></asp:Content>
<asp:Content ContentPlaceholderID="MainContent" runat="server">
    <div id="Tresc">
        <div>
            <div id="Rejestracja_" runat="server">
                <div>
                    <div class="naglowek">Rejestracja nowego użytkownika</div>
                    <div runat="server" id="Blad"></div>
                    <div class="wiersz">
                        <div class="pole"><label for="ImieInput">Imię: <span class="gwiazdka">*</span></label><input id="ImieInput" name="ImieInput" type="text" runat="server" /></div>
                        <div class="pole"><label for="NazwiskoInput">Nazwisko: <span class="gwiazdka">*</span></label><input id="NazwiskoInput" name="NazwiskoInput" type="text" runat="server" /></div>
                    </div>
                    <div class="wiersz">
                        <div class="pole"><label for="NazwaInput">Nazwa użytkownika: <span class="gwiazdka">*</span></label><input id="NazwaInput" name="NazwaInput" type="text" runat="server" /></div>
                    </div>
                    <div class="wiersz">
                        <div class="pole"><label for="HasloInput">Hasło: <span class="gwiazdka">*</span></label><input id="HasloInput" name="HasloInput" type="password" runat="server" /></div>
                        <div class="pole"><label for="HasloInput2">Powtórz hasło: <span class="gwiazdka">*</span></label><input id="HasloInput2" name="HasloInput2" type="password" runat="server" /></div>
                    </div>
                    <div class="wiersz">
                        <div class="pole"><label for="EmailInput">Adres E-mail: <span class="gwiazdka">*</span></label><input id="EmailInput" name="EmailInput" type="text" runat="server" /></div>
                        <div class="pole"><label for="EmailInput2">Powtórz adres E-mail: <span class="gwiazdka">*</span></label><input id="EmailInput2" name="EmailInput2" type="text" runat="server" /></div>
                    </div>
                    <div><input id="Submit1" type="submit" value="Zarejestruj" /></div>
                    <div class="infoMale">Wypełnienie pól oznaczonych <span class="gwiazdka">*</span> jest obowiązkowe.</div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>