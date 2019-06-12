# food-online-ordering-system-with-C.Sharp
This is a simple food online ordering system developed with C#

Setup:
Platform: Visual Studio 2017 + Mysql Server 2008
Before running the application, we should setup the server first. 
Open Web.Config in source code and change the value after “server=” (line 8) to your sql server name (you can check in property of database). 
Then attach .mdf file (in file data) to database and we can run the application.

Instruction (How to run it):
First, open Visual Studio and select file -> open -> Web Site.... 
Then choose the source code file (food online-order system). 
Then we can see a lot of file attached in Solution Explorer on the right side. 
Right click “food online-order system”and select “View in Browser”, then you can see my web application in Browser. 
(Make sure attach .mdf file in database and connect it).

Guideline of Home Page:
As customer, on the left side you are able to login as member and register member 
(you are not able to order food or leave comments before you log in as member). 
After you log in and place order, you can go to shopping cart to check out and make payment by choosing payment method. 
You can also leave comments in comment section list on the top menu.
As administrator, you can find the entry of back-end stage in the bottom of home
page. While you click “Administrator Login ”in the bottom of page, you are able to
login as adminstrator and manage orders and the details of food. (username: admin password: admin).
