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


public partial class shopCart : System.Web.UI.Page
{
    CommonClass ccObj = new CommonClass();
    DBClass dbObj = new DBClass();
    string strSql;
    DataTable dtTable;
    Hashtable hashCar;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            /*check if login*/
            ST_check_Login();
            if (Session["ShopCart"] == null)
            {
                //if haven't ordered yet, hide the button
                this.labMessage.Text = "You have not ordered anything yet！";
                this.labMessage.Visible = true;        //show tips
                this.lnkbtnCheck.Visible = false;      
                this.lnkbtnClear.Visible = false;    
                this.lnkbtnContinue.Visible = false;  

            }
            else
            {
                hashCar = (Hashtable)Session["ShopCart"]; 
                if (hashCar.Count == 0)
                {
                
                    this.labMessage.Text = "There is no food in your shopping cart！";
                    this.labMessage.Visible = true;      
                    this.lnkbtnCheck.Visible = false; 
                    this.lnkbtnClear.Visible = false; 
                    this.lnkbtnContinue.Visible = false; 

                }
                else
                {
               
                    dtTable = new DataTable();
                    DataColumn column1 = new DataColumn("No");        //No
                    DataColumn column2 = new DataColumn("mealID");    //food ID
                    DataColumn column3 = new DataColumn("mealName");  //food name
                    DataColumn column4 = new DataColumn("Num");       //quantity
                    DataColumn column5 = new DataColumn("price");     //unit-price
                    DataColumn column6 = new DataColumn("totalPrice");//total-price
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
                        row["mealID"] = key.ToString();
                        row["Num"] = hashCar[key].ToString();
                        dtTable.Rows.Add(row);
                    }
                    //calculate price
                    DataTable dstable;
                    int i = 1;
                    float price;
                    int count;
                    float totalPrice = 0;
                    foreach (DataRow drRow in dtTable.Rows)
                    {
                        strSql = "select mealName,HotPrice from tb_mealInfo where mealID=" + Convert.ToInt32(drRow["mealID"].ToString());
                        dstable = dbObj.GetDataSetStr(strSql, "tbGI");
                        drRow["No"] = i;
                        drRow["mealName"] = dstable.Rows[0][0].ToString();
                        drRow["price"] = (dstable.Rows[0][1].ToString());
                        price = float.Parse(dstable.Rows[0][1].ToString());
                        count = Int32.Parse(drRow["Num"].ToString());
                        drRow["totalPrice"] = price * count; 
                        totalPrice += price * count; 
                        i++;
                    }
                    this.labTotalPrice.Text = "总价：" + totalPrice.ToString(); 
                    this.gvShopCart.DataSource = dtTable.DefaultView; 
                    this.gvShopCart.DataKeyNames = new string[] { "mealID" };
                    this.gvShopCart.DataBind();
                }
            }
        
        }
       
    }
    public void ST_check_Login()
    {

        if ((Session["Username"] == null))
        {
            Response.Write("<script>alert('Please login first!');location='Default.aspx'</script>");
            Response.End();
        }
    }
    public void bind()
    {
        if (Session["ShopCart"] == null)
        {
         
            this.labMessage.Text = "You have not ordered anything yet！";
            this.labMessage.Visible = true; 
            this.lnkbtnCheck.Visible = false; 
            this.lnkbtnClear.Visible = false;  
            this.lnkbtnContinue.Visible = false; 
            
        }
        else
        {
            hashCar = (Hashtable)Session["ShopCart"];
            if (hashCar.Count == 0)
            {

                this.labMessage.Text = "There is no food in your shopping cart！";
                this.labMessage.Visible = true;  
                this.lnkbtnCheck.Visible = false; 
                this.lnkbtnClear.Visible = false; 
                this.lnkbtnContinue.Visible = false; 
               
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
                    row["mealID"] = key.ToString();
                    row["Num"] = hashCar[key].ToString();
                    dtTable.Rows.Add(row);
                }
             
                DataTable dstable;
                int i = 1;
                float price;
                int count;
                float totalPrice = 0; 
                foreach (DataRow drRow in dtTable.Rows)
                {
                    strSql = "select mealName,HotPrice from tb_mealInfo where mealID=" + Convert.ToInt32(drRow["mealID"].ToString());
                    dstable = dbObj.GetDataSetStr(strSql, "tbGI");
                    drRow["No"] = i;
                    drRow["mealName"] = dstable.Rows[0][0].ToString();
                    drRow["price"] = (dstable.Rows[0][1].ToString());
                    price = float.Parse(dstable.Rows[0][1].ToString());
                    count = Int32.Parse(drRow["Num"].ToString());
                    drRow["totalPrice"] = price * count; 
                    totalPrice += price * count; 
                    i++;
                }
                this.labTotalPrice.Text = "总价：" + totalPrice.ToString(); 
                this.gvShopCart.DataSource = dtTable.DefaultView; 
                this.gvShopCart.DataKeyNames=new string[] {"mealID"};
                this.gvShopCart.DataBind();
            }
        }
            
    
    
    }
    protected void lnkbtnUpdate_Click(object sender, EventArgs e)
    {
        hashCar = (Hashtable)Session["ShopCart"]; 
   
        foreach (GridViewRow gvr in this.gvShopCart.Rows)
        {
            TextBox otb = (TextBox)gvr.FindControl("txtNum"); 
            int count = Int32.Parse(otb.Text);
            string mealID = gvr.Cells[1].Text;
            hashCar[mealID] = count;
        }
        Session["ShopCart"] = hashCar;
        Response.Redirect("shopCart.aspx");
    }
    protected void lnkbtnDelete_Command(object sender, CommandEventArgs e)
    {
        hashCar = (Hashtable)Session["ShopCart"];
    
        hashCar.Remove(e.CommandArgument);
        Session["ShopCart"] = hashCar;
        Response.Redirect("shopCart.aspx");
    }
    protected void lnkbtnClear_Click(object sender, EventArgs e)
    {
        Session["ShopCart"] =null;
        Response.Redirect("shopCart.aspx");
    }
    protected void lnkbtnContinue_Click(object sender, EventArgs e)
    {
        Response.Redirect("Default.aspx");
    }
    protected void lnkbtnCheck_Click(object sender, EventArgs e)
    {
        Response.Redirect("checkOut.aspx");
    }
    protected void gvShopCart_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvShopCart.PageIndex = e.NewPageIndex;
        bind();

    }
    protected void txtNum_TextChanged(object sender, EventArgs e)
    {
        hashCar = (Hashtable)Session["ShopCart"];
        foreach (GridViewRow gvr in this.gvShopCart.Rows)
        {

            TextBox otb = (TextBox)gvr.FindControl("txtNum");
            int count = Int32.Parse(otb.Text);
            string mealID = gvr.Cells[1].Text;
            hashCar[mealID] = count;

        }
        Session["ShopCart"] = hashCar;
        bind();

    }
}
