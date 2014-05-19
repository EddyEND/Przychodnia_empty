<%@ Page Title="Stron główna" Language="C#" MasterPageFile="~/Szablon.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ContentPlaceholderID="head" runat="server">
    <script type="text/javascript" src="./js/jssor.slider.mini.js"></script>
    <script type="text/javascript" src="./js/slider.js"></script> 
</asp:Content>
<asp:Content ContentPlaceholderID="MainContent" runat="server">
    <div id="slider1_container" style="height: 400px;">
        <div u="slides">
		    <div>
                <img u="image" src="./images/hp-hero-bg.jpg" />
			    <div class="slidecontent">
			        <div u="caption" t="L" style="position: absolute; width: 480px; height: 120px; top: 15px; left: 30px; padding: 5px; text-align: left; line-height: 60px; text-transform: uppercase; font-size: 40px; color: #FFFFFF;">
				        Przychodnia
				        <br />
				        "Nazwa Przychodni"  
			        </div>
			        <div u="caption" t="L" style="position: absolute; width: 480px; height: 120px; top: 180px; left: 30px; padding: 5px; text-align: left; line-height: 36px; font-size: 20px; color: #FFFFFF;">
				        Lorem ipsum dolor sit amet, consectetur adipiscing elit. Phasellus et risus at nulla semper rhoncus. Nunc in turpis quam. Praesent mattis quis eros non pharetra. Integer aliquam leo vel posuere tempus. Aliquam gravida lacus vitae aliquam posuere
			        </div>
			        <div u="caption" t="R" style="position: absolute; width: 480px; height: 120px; top: 20px; left: 660px; padding: 5px;">
				         <img class="zdjecia" src="./images/1.jpg" />
			        </div>
                </div>
		    </div>
		    <div>
			    <img u="image" src="./images/hp-hero-bg1.png" />
			    <div class="slidecontent">
                   <div  u="caption" t="TEST" style="position: absolute; width: 495px; height: 330px; top: 25px; left: 30px; padding: 5px;">
				        <img  class="zdjecia"  src="./images/2.jpg" />
			        </div>
			        <div u="caption" t="T" style="position: absolute; width: 480px; height: 100px; top: 15px; left: 640px; padding: 5px; text-align: center; line-height: 60px; text-transform: uppercase; font-size: 40px; color: #FFFFFF;">
				        Kontakt 
			        </div>
			        <div u="caption" t="B" style="position: absolute; width: 480px; height: 120px; top: 80px; left: 600px; padding: 5px; text-align: left; line-height: 36px; font-size: 20px; color: #FFFFFF;">
				        Telefon : 555 222 333 <br />
				        Adres : Zawszona 4 Iskrzynia<br />                   
			        </div>
                </div>            
		    </div>
		    <div>
			    <img u="image" src="./images/hp-hero-bg2.png" />
			    <div class="slidecontent">
                    <div u="caption" t="T" style="position: absolute; width: 480px; height: 100px; top: 10px; left: 425px; padding: 5px; text-align: center; line-height: 60px; font-size: 40px; color: #fff;">
				        W Naszej Ofercie
			        </div>
			        <div id="dzielenie" u="caption" t="TEST" style="position: absolute; width: 1px; height: 295px; top: 75px; left: 650px;">                 
			        </div>              
			        <div>  
				        <div u="caption" t="B" style="position: absolute; width: 480px; height: 120px; top: 95px; left: 700px; padding: 5px; text-align: left; font-size: 28px; color: #ffffff;">
					        <img src="./images/Lekarz1.jpg" />
					        Ginekolog Dr Rafał Czyż 
				        </div>
				        <div u="caption" t="B" style="position: absolute; width: 480px; height: 120px; top: 180px; left: 700px; padding: 3px; text-align: left; font-size: 20px; color: #ffffff; line-height: 45px;">
					        22 Lat doświadczenia za tym niesamowicie uzdolnionym Ginekologiem.
					        <br />
					        Największe gwiazdy polskiego kina leczyły się u niego:
					        <br />
					        Pan/Pani Grotzke 
				        </div>
				        <div u="caption" t="B" style="position: absolute; width: 480px; height: 120px; top: 95px; left: 30px; padding: 5px; text-align: left; font-size: 28px; color: #ffffff;">
					        <img src="./images/Lekarz2.bmp" />
					        Ortopeda Dr Mateusz Kijowski
				        </div>
				        <div u="caption" t="B" style="position: absolute; width: 480px; height: 140px; top: 180px; left: 30px; padding: 3px; text-align: left; font-size: 20px; color: #ffffff; line-height: 45px;">
					        15 lat jako najlepszy ortopeda Kraju zdobywca 
					        <br />
					        trzech najwiekszych osiągnięć w dziedzinie medycyny
					        <br />
					        Tytuł: Patyk roku 2012
				        </div>
			        </div>
                </div>
		    </div>
		    <div u="navigator" class="jssorb21" style="position: absolute; bottom: 10px; left:6px;">
			    <div u="prototype" style="position: absolute; width: 19px; height: 19px;"></div>
		    </div>
	    </div>   
    </div> 
    <div id="InformacjeFull">
        <div>
            <div class="InformacjePol">
                <div id="inf1" runat="server">
                    <h2>Aktualności</h2>
                    <p>Pierwsza Informacja</p>
                    <p>Pierwsza Informacja</p>
                    <p>Pierwsza Informacja</p>
                    <p>Pierwsza Informacja</p>
                    <p>Pierwsza Informacja</p>
                    <p>Pierwsza Informacja</p>
                </div>
            </div>
            <div class="InformacjePol">
                <div id="inf2" runat="server">
                    <p>Druga Informacja</p>
                    <p>Druga Informacja</p>
                    <p>Druga Informacja</p>
                    <p>Druga Informacja</p>
                </div>
            </div>
            <div class="InformacjePol">
                <div id="inf3" runat="server">
                    <p>Trzecia Informacja</p>
                    <p>Trzecia Informacja</p>
                </div>
            </div>
        </div>
    </div>
</asp:Content>