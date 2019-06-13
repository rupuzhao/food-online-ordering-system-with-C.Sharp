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

public partial class showInfo : System.Web.UI.Page
{
    DBClass dbObj = new DBClass();
    CommonClass ccObj = new CommonClass();
    GoodsClass gcObj = new GoodsClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            GetGoodsInfo();
        }
    }
    
    public void GetGoodsInfo()
    {
        string strSql = "select * from tb_mealInfo where mealID=" + Convert.ToInt32(Request["id"].Trim());
        SqlCommand myCmd = dbObj.GetCommandStr(strSql);
        DataTable dsTable = dbObj.GetDataSetStr(strSql, "tbBI");
        this.txtCategory.Text = gcObj.GetClass(Convert.ToInt32(dsTable.Rows[0]["ClassID"].ToString()));
        this.txtName.Text = dsTable.Rows[0]["mealName"].ToString();
        this.txtAuthor.Text = dsTable.Rows[0]["Author"].ToString();
        this.txtCompany.Text = dsTable.Rows[0]["Company"].ToString();
        this.txtMarketPrice.Text = dsTable.Rows[0]["MarketPrice"].ToString();
        this.txtHotPrice.Text = dsTable.Rows[0]["HotPrice"].ToString();
        this.ImageMapPhoto.ImageUrl ="updatefile/"+ dsTable.Rows[0]["mealUrl"].ToString();
      
        this.txtShortDesc.Text = dsTable.Rows[0]["mealIntroduce"].ToString();
    }

    protected void btnBack_Click(object sender, EventArgs e)
    {
        string strUrl = Session["address"].ToString();
        Response.Redirect(strUrl);
    }
    protected void imagebtnBuy_Click(object sender, ImageClickEventArgs e)
    {
        



        /*check if login*/
        ST_check_Login();
        Hashtable hashCar;
        if (Session["ShopCart"] == null)
        {
            
            hashCar = new Hashtable();        
            hashCar.Add(Request["id"].Trim(), 1); 
            Session["ShopCart"] = hashCar;  
        }
        else
        {
    
            hashCar = (Hashtable)Session["ShopCart"];
            if (hashCar.Contains(Request["id"].Trim()))
            {
                int count = Convert.ToInt32(hashCar[Request["id"].Trim()].ToString());
                hashCar[Request["id"].Trim()] = (count + 1);
            }
            else
                hashCar.Add(Request["id"].Trim(), 1);
        }
        Response.Write(ccObj.MessageBox("Added to shopping cart", "shopCart.aspx"));
    }


    public void ST_check_Login()
    {

        if ((Session["UserName"] == null))
        {
            Response.Write("<script>alert('Please login first!');location='Default.aspx'</script>");
            Response.End();
        }
    }
}
