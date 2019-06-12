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

public partial class OrderList : System.Web.UI.Page
{
    CommonClass ccObj = new CommonClass();
    DBClass dbObj = new DBClass();
    OrderClass ocObj = new OrderClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            /*check if login*/
            ST_check_Login();
            pageBind(); //bind orders information
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
    //bind food amount
    public string GetVarGF(string strGoodsFee)
    {
        return ccObj.VarStr(strGoodsFee, 2);
    }
    //bind delivery fee
    public string GetVarSF(string strShipFee)
    {
        return ccObj.VarStr(strShipFee,2);
    }
    //bind total amount
    public string GetVarTP(string strTotalPrice)
    {
        return ccObj.VarStr(strTotalPrice,2);
    }
    public string GetStatus(int IntOrderID)
    {
        string strSql = "select (case IsConfirm when '0' then 'NotCon' when '1' then 'Confirmed' end ) as IsConfirm";
        strSql +=",(case IsSend when '0' then 'NotShi' when '1' then 'Shipped' end ) as IsSend";
        strSql +=",(case IsEnd when '0' then 'NotRec' when '1' then 'Recorded' end ) as IsEnd ";
        strSql +="  from tb_OrderInfo where OrderID="+IntOrderID;
        DataTable dsTable = dbObj.GetDataSetStr(strSql, "tbOI");
        return (dsTable.Rows[0][0].ToString() + "<Br>" + dsTable.Rows[0][1].ToString() + "<Br>" + dsTable.Rows[0][2].ToString());
    }
    public string GetAdminName(int IntOrderID)
    {
        string strSql = "select AdminName from tb_Admin ";
        strSql += "where AdminID=(select AdminID from tb_OrderInfo";
        strSql += " where OrderID='"+IntOrderID+"')";
        SqlCommand myCmd=dbObj.GetCommandStr(strSql);
        string strAdminName=(dbObj.ExecScalar(myCmd).ToString());
        if(strAdminName =="")
        {
            return "none";
        }
        else 
        {
            return strAdminName;
        }
    }
    
    string strSql;
    public void pageBind()
    {
        strSql ="select * from tb_OrderInfo where ";
        //get use ID
        int userid = Convert.ToInt32(Session["UserID"].ToString());
        
        strSql +=" userid="+userid+" order by OrderDate Desc";
        
        DataTable dsTable = dbObj.GetDataSetStr(strSql, "tbOI");
        this.gvOrderList.DataSource = dsTable.DefaultView;
        this.gvOrderList.DataKeyNames = new string[] { "OrderID"};
        this.gvOrderList.DataBind();
    }
   

    protected void gvOrderList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvOrderList.PageIndex = e.NewPageIndex;
        pageBind();

    }
    
}
