<%@ Page Title="Panel użytkownika" Language="C#" MasterPageFile="~/Szablon.master" AutoEventWireup="true" CodeFile="Panel.aspx.cs" Inherits="Panel" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">
    <div class="ramka">
        <div id="r0" runat="server"> 
            <div class="naglowek">Aby dokonać zmian musisz podać aktualne hasło</div>
            <div id="blad0" runat="server"></div>
            <div class="wiersz">
                <div class="pole"><label for="HasloInput">Aktualne Hasło: </label><input id="HasloInput" name="HasloInput" type="password" runat="server" /></div>
            </div>
            <input style="display: none;" type="submit" id="hid" name="dane" value="enter" />
        </div>
    </div>
    <div class="ramka">
        <div id="r1" runat="server"> 
            <div class="naglowek">Zmiana danych osobowych</div>
            <div id="blad1" runat="server"></div>
            <div class="wiersz">
                <div class="pole"><label for="NewImieInput">Nowe imię: </label><input id="NewImieInput" name="NewImieInput" type="text" runat="server" /></div>
                <div class="pole"><label for="NewNazwiskoInput">Nowe nazwisko: </label><input id="NewNazwiskoInput" name="NewNazwiskoInput" type="text" runat="server" /></div>
            </div>
            <input type="submit" id="dane" name="dane" value="Zmień dane" />
        </div>
    </div>
    <div class="ramka">
        <div id="r2" runat="server">
            <div class="naglowek">Zmiana adresu E-mail</div>
            <div id="blad2" runat="server"></div>
            <div class="wiersz">
                <div class="pole"><label for="NewEmailInput">Adres E-mail: </label><input id="NewEmailInput" name="NewEmailInput" type="text" runat="server" /></div>
                <div class="pole"><label for="NewEmailInput2">Powtórz adres E-mail: </label><input id="NewEmailInput2" name="NewEmailInput2" type="text" runat="server" /></div>
            </div>
            <input type="submit" id="email" name="email" value="Zmień adres E-mail" />

        </div>
    </div>
    <div class="ramka">
        <div id="r3" runat="server">
            <div class="naglowek">Zmiana Hasła</div>
            <div id="blad3" runat="server"></div>
            <div class="wiersz">
                <div class="pole"><label for="NewHasloInput">Hasło: </label><input id="NewHasloInput" name="NewHasloInput" type="password" runat="server" /></div>
                <div class="pole"><label for="NewHasloInput2">Powtórz hasło: </label><input id="NewHasloInput2" name="NewHasloInput2" type="password" runat="server" /></div>
            </div>
            <input type="submit" id="pass" name="pass" value="Zmień Hasło" />

        </div>
    </div>
</asp:Content>

