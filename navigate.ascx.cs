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

public partial class userControl_navigate : System.Web.UI.UserControl
{
    GoodsClass gcObj = new GoodsClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            gcObj.DLClassBind(this.dlClass);
            //gcObj.DLNewGoods(this.dlNewGoods);

        }
    }
    protected void dlClass_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "select")
        {
            Response.Redirect("goodsList.aspx?id=" + e.CommandArgument);
        }   
    }
    public string GetClassName(int IntClassID)
    {
       return  gcObj.GetClass(IntClassID);
    }
    protected void dlNewGoods_ItemCommand(object source, DataListCommandEventArgs e)
    {
        if (e.CommandName == "detailSee")
        {
            Session["address"] = "";
            Session["address"] = "Default.aspx";
            Response.Redirect("~/showInfo.aspx?id=" + Convert.ToInt32(e.CommandArgument.ToString()));
        }

    }
}
