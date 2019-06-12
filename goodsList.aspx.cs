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

public partial class goodsList : System.Web.UI.Page
{
    CommonClass ccObj = new CommonClass();
    GoodsClass gcObj = new GoodsClass();
    DBClass dbObj = new DBClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.Label1.Visible = false;
            this.Image1.Visible = false;
            dlBind();     
            deplayTitle(); 


        }
    }
   
   
    public void dlBind()
    {
        if (this.Request["var"]=="1")
        {
            if (this.Request["id"] == "5")
            {
                //Response.Write("<script>alert('"+Convert.ToInt32(Session["UsertjID"].ToString())+"');</script>");
                
                
                dlBindPage(Convert.ToInt32(Request["id"].ToString()), Convert.ToInt32(Session["UsertjID"].ToString()));
            }
            else
            {
                dlBindPage(Convert.ToInt32(Request["id"].ToString()), 0);
            }
        }
        else
        {
            dlBindPage(0, Convert.ToInt32(Request["id"].ToString()));
        }
    
    }
    
    public void deplayTitle()
    {
        if (this.Request["var"] == "1")
        {
            
            switch (this.Request["id"])
            { 
                case "1":
                    this.labTitle.Text = "Home/New";
                    break;
                case "2":
                    this.labTitle.Text = "Home/Recommandation";
                    break;
                case "3":
                    this.labTitle.Text = "Home/Special";
                    break;
                case "4":
                    this.labTitle.Text = "Home/Hot";
                    break;
                case "5":
                    this.labTitle.Text = "Home/Intelligent Recommandation";
                    break;
            }

        }
        else
        {
           
            string strClassName = gcObj.GetClass(Convert.ToInt32(this.Request["id"].ToString()));
            this.labTitle.Text = "Home/Category/" + strClassName; 
        }
    
    }





    public void gvSearchBind()
    {
       
    }

    public void dlBindPage(int IntDeplay,int IntClass)
    {
      

        SqlCommand myCmd = dbObj.GetCommandProc("proc_GIList");
      
        SqlParameter Deplay = new SqlParameter("@Deplay", SqlDbType.Int, 4);
        Deplay.Value = IntDeplay;
        myCmd.Parameters.Add(Deplay);


        SqlParameter Classid = new SqlParameter("@ClassID", SqlDbType.Int, 4);
        Classid.Value = IntClass;
        myCmd.Parameters.Add(Classid);
        
        
        SqlParameter px = new SqlParameter("@px", SqlDbType.Int, 4);
        px.Value = this.DropDownList1.SelectedValue;
        myCmd.Parameters.Add(px);
        
        



        //search
        if (this.txtKey.Text!="")
        {
                    SqlParameter search = new SqlParameter("@search", SqlDbType.Int, 4);
                    search.Value =1;
            myCmd.Parameters.Add(search);
      
        SqlParameter key = new SqlParameter("@key", SqlDbType.VarChar, 50);
        key.Value = this.txtKey.Text.Trim();
        myCmd.Parameters.Add(key);


        }
        else
        {
   
        SqlParameter search = new SqlParameter("@search", SqlDbType.Int, 4);
        search.Value = 0;
        myCmd.Parameters.Add(search);
      
        SqlParameter key = new SqlParameter("@key", SqlDbType.VarChar, 50);
        key.Value = this.txtKey.Text.Trim();
        myCmd.Parameters.Add(key);
       }
      
        
        
        dbObj.ExecNonQuery(myCmd);
        DataTable dsTable = dbObj.GetDataSet(myCmd, "tbGI");
        int curpage = Convert.ToInt32(this.labPage.Text);
       
        
        
        PagedDataSource ps = new PagedDataSource();
      
        
        ps.DataSource = dsTable.DefaultView;
        ps.AllowPaging = true; 
        ps.PageSize = 15; 
        ps.CurrentPageIndex = curpage - 1;
        
        
        
        this.lnkbtnUp.Enabled = true;
        this.lnkbtnNext.Enabled = true;
        this.lnkbtnBack.Enabled = true;
        this.lnkbtnOne.Enabled = true;
        if (curpage == 1)
        {
            this.lnkbtnOne.Enabled = false;
            this.lnkbtnUp.Enabled = false;
        }
        if (curpage == ps.PageCount)
        {
            this.lnkbtnNext.Enabled = false;
            this.lnkbtnBack.Enabled = false;
        }
        this.labBackPage.Text = Convert.ToString(ps.PageCount);





        this.dLGoodsList.DataSource = ps;
        this.dLGoodsList.DataKeyField ="mealID";
        this.dLGoodsList.DataBind();

   
        if (  this.dLGoodsList.Items.Count == 0)
        {
         this.Image1.Visible = true;
        this.Label1.Visible = true;
        } 
        
    }
    protected void lnkbtnOne_Click(object sender, EventArgs e)
    {
        this.labPage.Text = "1";
        this.dlBind();
    }
    protected void lnkbtnUp_Click(object sender, EventArgs e)
    {
        this.labPage.Text = Convert.ToString(Convert.ToInt32(this.labPage.Text) - 1);
        this.dlBind();
    }
    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        this.labPage.Text = Convert.ToString(Convert.ToInt32(this.labPage.Text) + 1);
        this.dlBind();
    }
    protected void lnkbtnBack_Click(object sender, EventArgs e)
    {
        this.labPage.Text = this.labBackPage.Text;
        this.dlBind();
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
    public void AddressBack(DataListCommandEventArgs e)
    {
        Session["address"] = "";
        Session["address"] = "Default.aspx";
        Response.Redirect("~/showInfo.aspx?id=" + Convert.ToInt32(e.CommandArgument.ToString()));
    }
    protected void dLGoodsList_ItemCommand(object source, DataListCommandEventArgs e)
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
        /*check if login*/
        ST_check_Login();
        Hashtable hashCar;
        if (Session["ShopCart"] == null)
        {
            //if user don't have shopping cart
            hashCar = new Hashtable();         //create new one
            hashCar.Add(e.CommandArgument, 1); //add food
            Session["ShopCart"] = hashCar;     
        }
        else
        {
            //if user has shopping cart
            hashCar = (Hashtable)Session["ShopCart"];
            if (hashCar.Contains(e.CommandArgument))
            {
                int count = Convert.ToInt32(hashCar[e.CommandArgument].ToString());
                hashCar[e.CommandArgument] = (count + 1);
            }
            else
                hashCar.Add(e.CommandArgument, 1);
        }
        Response.Write(ccObj.MessageBox("Added to shopping cate successfully!", "shopCart.aspx"));

    }
    public void ST_check_Login()
    {

        if ((Session["Username"] == null))
        {
            Response.Write("<script>alert('Sorry, you are not member, please login first！');location='Default.aspx'</script>");
            Response.End();
        }
    }
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        dlBindPage(0,0);
    }


    protected void dLGoodsList_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
