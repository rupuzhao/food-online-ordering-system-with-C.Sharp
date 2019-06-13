using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

public partial class checkOut : System.Web.UI.Page
{
    CommonClass ccObj = new CommonClass();
    DBClass dbObj = new DBClass();
    OrderClass ocObj = new OrderClass();
    UserClass ucObj = new UserClass();
    DataTable dtTable;
    Hashtable hashCar;
    string strSql;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["UserID"] == null)
            {
                //Response.Redirect("Default.aspx");
                 Response.Write("<script lanuage=javascript>alert('Sorry, you are not member！');location='default.aspx'</script>");
            }  
            //if (Session["Username"] != null)
            else
            {
                //if user already login, show their information
                DataTable dsTable = ucObj.GetUserInfo(Convert.ToInt32(Session["UserID"].ToString()));
                //this.userid.Text = Session["UserID"].ToString();   
               
 this.txtReciverName.Text = dsTable.Rows[0][1].ToString(); 
                this.txtReceiverPhone.Text = dsTable.Rows[0][6].ToString();   //phone
                this.txtReceiverEmails.Text = dsTable.Rows[0][7].ToString();  //email
                this.txtReceiverPostCode.Text = dsTable.Rows[0][9].ToString();//WeChat
                this.txtReceiverAddress.Text =dsTable.Rows[0][8].ToString();  //address

            }
            if (Session["ShopCart"] == null)
            {
               
                this.labMessage.Text = "You have not ordered anything yet！"; //show tips
                this.btnConfirm.Visible = false;  
            }
            else
            {
                hashCar = (Hashtable)Session["ShopCart"];
                if (hashCar.Count == 0)
                {
                 
                    this.labMessage.Text = "There is no food in your shopping cart！";//show tips
                    this.btnConfirm.Visible = false;              //hide "confirm" button
                }
                else
                {
                 
                    dtTable = new DataTable();
                    DataColumn column1 = new DataColumn("No");
                    DataColumn column2 = new DataColumn("mealID");  
                    DataColumn column3 = new DataColumn("mealName"); 
                    DataColumn column4 = new DataColumn("Num");
                    DataColumn column5 = new DataColumn("price"); 
                    DataColumn column6 = new DataColumn("totalPrice");
                    dtTable.Columns.Add(column1);
                    dtTable.Columns.Add(column2);
                    dtTable.Columns.Add(column3);
                    dtTable.Columns.Add(column4);
                    dtTable.Columns.Add(column5);
                    dtTable.Columns.Add(column6);
                    DataRow row;
                   
                    foreach (object key in hashCar.Keys)
                    {
                        row = dtTable.NewRow();
                        row["mealID"] = key.ToString();        //foodID
                        row["Num"] = hashCar[key].ToString();  //food quantity        
                        dtTable.Rows.Add(row);
                    }
                    //calculate total price
                    DataTable dstable;
                    int i = 1;
                    float price; 
                    int num;  
                    float totalPrice = 0;
                    int totailNum = 0; 
                    foreach (DataRow drRow in dtTable.Rows)
                    {
                        strSql = "select mealName,HotPrice from tb_mealInfo where mealID=" + Convert.ToInt32(drRow["mealID"].ToString());
                        dstable = dbObj.GetDataSetStr(strSql, "tbGI");
                        drRow["No"] = i;
                        drRow["mealName"] = dstable.Rows[0][0].ToString(); 
                        drRow["price"] = dstable.Rows[0][1].ToString();
                        price = float.Parse(dstable.Rows[0][1].ToString());  
                        num = Int32.Parse(drRow["Num"].ToString());
                        drRow["totalPrice"] =(price*num);  
                        totalPrice += price * num;     
                        totailNum += num;           
                        i++;
                    }
                    this.labTotalPrice.Text = totalPrice.ToString();  
                    this.labTotalNum.Text = totailNum.ToString(); 
                    this.gvShopCart.DataSource = dtTable.DefaultView; 
                    this.gvShopCart.DataBind();
                }
            }
        }

    }
    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        if (Page.IsValid)
        {
            //get information from users
            string strPhone; 
            string strEmail;  //Email
            string strZip;  
            float fltShipFee; 
            if (IsValidPostCode(this.txtReceiverPostCode.Text.Trim()) == true) //check if WeChat correct
            {
                strZip = this.txtReceiverPostCode.Text.Trim();
            }
            else
            {
                Response.Write(ccObj.MessageBox("Invalid！"));
                return;
            }
            if (IsValidPhone(this.txtReceiverPhone.Text.Trim()) == true)//check phone
            {
                strPhone = this.txtReceiverPhone.Text.Trim();
            }
            else
            {
                Response.Write(ccObj.MessageBox("Invalid！"));
                return;
            }
            if (IsValidEmail(this.txtReceiverEmails.Text.Trim()) == true)//check Email
            {
                strEmail = this.txtReceiverEmails.Text.Trim();
            }
            else
            {
                Response.Write(ccObj.MessageBox("Invalid！"));
                return;
            }
            if (this.ddlShipType.SelectedIndex != 0)
            { 
                fltShipFee = float.Parse(this.ddlShipType.SelectedValue.ToString());
            }
            else 
            { 
                Response.Write(ccObj.MessageBox("Please choose payment method！"));
                return;
            }
            string strName = this.txtReciverName.Text.Trim();    
            string strAddress = this.txtReceiverAddress.Text.Trim();
            string strRemark = this.txtRemark.Text.Trim(); 
            int IntTotalNum = int.Parse(this.labTotalNum.Text); 
            int userid = Convert.ToInt32(Session["UserID"].ToString());
            float fltTotalShipFee =  fltShipFee;
        
            int IntOrderID = ocObj.AddOrder(float.Parse(this.labTotalPrice.Text), fltTotalShipFee, this.ddlShipType.SelectedItem.Text, strName, strPhone, strZip, strAddress, strEmail,userid);
            int IntmealID; 
            int IntNum;  
            float fltTotalPrice;
         
            foreach (GridViewRow gvr in this.gvShopCart.Rows)
            {
                IntmealID = int.Parse(gvr.Cells[1].Text);
                IntNum = int.Parse(gvr.Cells[3].Text);
                fltTotalPrice = float.Parse(gvr.Cells[5].Text);
                ocObj.AddDetail(IntmealID, IntNum, IntOrderID, fltTotalPrice, strRemark);
            }
            //setup Session
            Session["ShopCart"] = null;  //empty shopping cart
            Response.Redirect("PayWay.aspx?OrderID=" + IntOrderID);
            //Response.Redirect("GoBank.aspx?OrderID=" + IntOrderID );
          
        
        }
    }

    public bool IsValidPostCode(string num)
    {
        return Regex.IsMatch(num, @"\d{6}");
    }
    public bool IsValidPhone(string num)
    {
        return Regex.IsMatch(num, @"(\(\d{3,4}\)|\d{3,4}-)?\d{7,8}$");
    }
    public bool IsValidEmail(string num)
    {
        return Regex.IsMatch(num, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
    }

  
}
