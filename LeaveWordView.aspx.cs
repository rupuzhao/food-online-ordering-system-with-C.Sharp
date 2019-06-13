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

public partial class LeaveWordView : System.Web.UI.Page
{
    DBClass dbObj = new DBClass();
    CommonClass ccObj = new CommonClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.dlBind();
        }
    }

    public void dlBind()
    {
        int curpage = Convert.ToInt32(labNowPage.Text);
        PagedDataSource ps = new PagedDataSource();
        string sqlStr = "SELECT * FROM tb_Reply WHERE ReplyID='" + Request["ID"].ToString() + "'";
        SqlCommand myCmd=dbObj.GetCommandStr(sqlStr);
        DataTable dsTable = dbObj.GetDataSet(myCmd, "tbReplay");
        ps.DataSource = dsTable.DefaultView;
        ps.AllowPaging = true; 
        ps.PageSize = 10; 
        ps.CurrentPageIndex = curpage - 1; //get current page
        lnkbtnPrve.Enabled = true;
        lnkbtnTop.Enabled = true;
        lnkbtnNext.Enabled = true;
        lnkbtnLast.Enabled = true;
        if (curpage == 1)
        {
            lnkbtnTop.Enabled = false;//not show 1st page button
            lnkbtnPrve.Enabled = false;//not show last page button
        }
        if (curpage == ps.PageCount)
        {
            lnkbtnNext.Enabled = false;//not show next page
            lnkbtnLast.Enabled = false;//not show end page

        }
        this.labCount.Text = Convert.ToString(ps.PageCount);
        this.DataList1.DataSource = ps;
        this.DataList1.DataKeyField = "ID";
        this.DataList1.DataBind();
    }

    protected void lnkbtnTop_Click(object sender, EventArgs e)
    {
        this.labNowPage.Text = "1";
        this.dlBind();
    }
    protected void lnkbtnPrve_Click(object sender, EventArgs e)
    {
        this.labNowPage.Text = Convert.ToString(Convert.ToInt32(this.labNowPage.Text) - 1);
        this.dlBind();
    }
    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        this.labNowPage.Text = Convert.ToString(Convert.ToInt32(this.labNowPage.Text) + 1);
        this.dlBind();
    }
    protected void lnkbtnLast_Click(object sender, EventArgs e)
    {
        this.labNowPage.Text = this.labCount.Text;
        this.dlBind();
    }
    protected void DataList1_DeleteCommand(object source, DataListCommandEventArgs e)
    {
        string strid = this.DataList1.DataKeys[e.Item.ItemIndex].ToString(); //get current DataList
        string strSql = "Delete from tb_Reply where ID='" + Convert.ToInt32(strid) + "'";
        dbObj.ExecNonQuery(dbObj.GetCommandStr(strSql));
        Page.Response.Redirect("LeaveWordView.aspx?ID=" + Request["ID"].ToString() + "");
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Page.Response.Redirect("MyWord.aspx");
    }
}
