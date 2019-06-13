using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Collections;

public partial class _Default : System.Web.UI.Page 
{
    public string photos = "";
    CommonClass ccObj = new CommonClass();
    GoodsClass gcObj = new GoodsClass();
    DBClass dbObj = new DBClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            RefineBind();
            HotBind();
            DiscountBind();
           
            //ST_check_Login();
        }
    }
    public void ST_check_Login()
    {

        if ((Session["UserName"] == null))
        {
            Response.Write("<script>alert('Please login first!');location='Default.aspx'</script>");
            Response.End();
        }
    }
    //bind market price
    public string GetVarMKP(string strMarketPrice)
    {
        return ccObj.VarStr(strMarketPrice, 2);
    }
    //bind hot price
    public string GetVarHot(string strHotPrice)
    {
        return ccObj.VarStr(strHotPrice, 2);
    }
    protected void RefineBind()
    {
        
        SqlCommand myCmd = dbObj.GetCommandProc("proc_NewGoods");
        dbObj.ExecNonQuery(myCmd);
        DataTable dsTable = dbObj.GetDataSet(myCmd, "tbGoods");
        int i = 0;
        foreach (DataRow drRow in dsTable.Rows )
        {
            if (i > 3) break;
            //Response.Write(drRow["mealUrl"]+"<br>");             

             photos+="imgUrl[" + i.ToString() + "]='updatefile/" + drRow["mealUrl"] + "';imgLink[" + i.ToString() + "]='showInfo.aspx?id=" + drRow["mealid"] + "';imgtext[" + i.ToString() + "]='" + drRow["mealname"] + "';";
	 
            
            i++;
        }

       
    }
    protected void HotBind()
    {
        gcObj.DLDeplayGI(3, this.dlHot, "Hot");
    }
    protected void DiscountBind()
    {
        gcObj.DLDeplayGI(2, this.dlDiscount, "Discount");
    }
    public void AddressBack(DataListCommandEventArgs e)
    {
        Session["address"] = "";
        Session["address"] = "Default.aspx";
        Response.Redirect("~/showInfo.aspx?id=" + Convert.ToInt32(e.CommandArgument.ToString()));
    }

    protected void dLRefine_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "detailSee")
        {
            AddressBack(e);
        }
        else if (e.CommandName == "buy")
        {
            AddShopCart(e);
        }

    }
    protected void dlDiscount_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "detailSee")
        {
            AddressBack(e);
        }
        else if (e.CommandName == "buy")
        {
            AddShopCart(e);
        }

    }
    protected void dlHot_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "detailSee")
        {
            AddressBack(e);
        }
        else if (e.CommandName == "buy")
        {
            AddShopCart(e);
        }

    }
    
    public void AddShopCart(DataListCommandEventArgs e)
    {
        //if (Session["UserName"] == null)
        //{
        //    Response.Redirect("Default.aspx");
        //}
        /*check if login*/
        ST_check_Login();
        Hashtable hashCar;
        if (Session["ShopCart"] == null)
        {
            
            hashCar = new Hashtable();         
            hashCar.Add(e.CommandArgument, 1);
            Session["ShopCart"] = hashCar; 
        }
        else
        {
            //user had shopping cart
            hashCar = (Hashtable)Session["ShopCart"];//get hash form
            if (hashCar.Contains(e.CommandArgument))//if food existed, quantity +1
            {
                int count = Convert.ToInt32(hashCar[e.CommandArgument].ToString());//get number
                hashCar[e.CommandArgument] = (count + 1);
            }
            else
                hashCar.Add(e.CommandArgument, 1);
        }
        Response.Write("<script>alert('Added to shopping cart');</script>");
    }


    protected void Button1_Click(object sender, EventArgs e)
    {
        
    }
    protected void imagebtnRefine_Click(object sender, ImageClickEventArgs e)
    {

    }

    protected void dlHot_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
