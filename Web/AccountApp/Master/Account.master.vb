Imports System.Data

Partial Class Master_Account
    Inherits System.Web.UI.MasterPage
    Dim allMenus As DataTable = New DataTable

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        Page.Title = System.Configuration.ConfigurationManager.AppSettings("PageTitle").ToString()
        If Not IsPostBack Then
            If IsNothing(Session("UserFullName")) Then
                Response.Redirect("~/Default.aspx", False)
                Exit Sub
            End If
            lblLoginName.Text = Session("UserFullName").ToString
            lblUserName.Text = Session("UserName")
            ''lblSiteMap.Text = "Home"
            If Not IsNothing(Session("UserName")) = False Then
                Response.Redirect("~/Default.aspx", False)
                Exit Sub
            End If
            If Not IsNothing(Session("DatMainMenu")) Then
                Dim DatsMenu As New DataSet
                DatsMenu = Session("DatMainMenu")
                allMenus = DatsMenu.Tables(0)
            End If
            If Not IsNothing(Session("DatSubMenu")) Then
                Dim DatsMenu As New DataSet
                DatsMenu = Session("DatSubMenu")
                rptMenus.DataSource = DatsMenu.Tables(0)
                rptMenus.DataBind()
            End If
            If IsNothing(Session("DatMainMenu")) = True And IsNothing(Session("DatSubMenu")) = True Then
                LoadMenu()
            End If
        End If
    End Sub

    Private Sub LoadMenu()
        allMenus = GetAllMenus()
        If Not IsNothing(allMenus) Then
            If allMenus.Rows.Count > 0 Then
                rptMenus.DataSource = GetMenus()
                rptMenus.DataBind()
            Else
                Response.Redirect("~/Default.aspx", False)
            End If
        Else
            Response.Redirect("~/Default.aspx", False)
        End If
    End Sub

    Private Function GetAllMenus() As DataTable
        Dim selectCommand As New StringBuilder
        selectCommand.Append(" SELECT tbl_Menudetail.idMenuDtl, tbl_Menudetail.description MenuName, tbl_Menudetail.idmainmenu ParentCategoryID, ")
        selectCommand.Append(" case when tbl_Menudetail.uri = '' then '#' else isnull(tbl_Menudetail.uri,'#') end uri, ")
        selectCommand.Append(" tbl_Menudetail.ImagePath, ")
        selectCommand.Append(" tbl_Menudetail.menuorder ")
        selectCommand.Append(" from tbl_Menudetail ")
        selectCommand.Append(" left outer join tbl_MainMenu on tbl_Menudetail.idMainMenu=tbl_MainMenu.idMainMenu ")
        If Session("IDRole") > 1 Then ' Avoid Role Check for Super and Application Admin User
            selectCommand.Append(" left outer join tbl_roles on tbl_Menudetail.idMenuDtl=tbl_roles.idMenuDtl ")
        End If
        selectCommand.Append(" where tbl_Menudetail.deleteflag=0 ")
        If Session("IDRole") > 1 Then ' Avoid Role Check for Super and Application Admin User
            selectCommand.Append(" and RoleId = " & Session("IDRole"))
        End If
        If Session("IDUser") <> 1 Then ' Avoid Role Check for Super Application User
            selectCommand.Append(" and tbl_Menudetail.idmainmenu <> 1 ")
        End If

        'If Session("idrole") > 2 Then
        '    selectCommand.Append(" and tbl_Menudetail.idMenuDtl in(" & Session("IDMenuDtl") & ") ")
        'End If
        selectCommand.Append(" order by tbl_Menudetail.idmainmenu, tbl_Menudetail.menuorder ")

        Dim dt As DataTable = New DataTable
        Dim Objparent As New clsData
        Dim DatsMenu As New DataSet
        DatsMenu = Objparent.fnDataSet(selectCommand.ToString, CommandType.Text)
        If Not IsNothing(DatsMenu) Then
            If DatsMenu.Tables.Count > 0 Then
                If DatsMenu.Tables(0).Rows.Count > 0 Then
                    dt = DatsMenu.Tables(0)
                    If dt.Rows.Count > 0 Then
                        Session("DatMainMenu") = DatsMenu
                        Dim StrMainMenuId As New StringBuilder
                        Dim IntCnt As Integer = 0
                        For IntCnt = 0 To dt.Rows.Count - 1
                            If StrMainMenuId.ToString.Contains(dt.Rows(IntCnt)("ParentCategoryID")) = False Then
                                StrMainMenuId.Append(dt.Rows(IntCnt)("ParentCategoryID") & ",")
                            End If
                        Next
                        Session("IDMainMenu") = Mid(StrMainMenuId.ToString, 1, StrMainMenuId.ToString.Length - 1)
                        dt = DatsMenu.Tables(0)
                    End If
                End If
            End If
        End If
        Return dt
    End Function

    Private Function GetMenus() As DataTable
        Dim selectCommand As New StringBuilder
        selectCommand.Append(" Select * from (   ")
        selectCommand.Append(" select idMainMenu, Description as MenuName, isnull(ImagePath,'') ImagePath,   case when uri = '' then '#' else isnull(uri,'#') end uri, MenuOrder, 'Img'+ CAST(idMainMenu as varchar(10)) as ImageId  from tbl_MainMenu  where Deleteflag=0 ")
        If UCase(Session("IDMainMenu")).ToString <> String.Empty Then
            selectCommand.Append(" and tbl_MainMenu.idMainMenu in(" & Session("IDMainMenu") & ") ")
        End If
        If Session("IDUser") <> 1 Then
            selectCommand.Append(" and tbl_MainMenu.idMainMenu not in(1) ")
        End If
        'selectCommand.Append(" Union all ")
        'selectCommand.Append(" select 999, replace(isnull(UserFullName,''),'  ',' '),  case when isnull(ImageName,'') <>'' then '../'+ImageName else '../Resource/Images/Menu/User/user.png' end ImagePath, '#' uri, 9999 as 'MenuOrder',  'Img'+'9999' as ImageId from tbl_user where iduser = " & Session("IDUser") & " ")
        selectCommand.Append(" ) MainMenu   ")
        If UCase(Session("UserName")).ToString <> "SUPER" Then
            selectCommand.Append(" Where idMainMenu not in(1)")
        End If
        selectCommand.Append(" order by idMainMenu, MenuOrder ")

        Dim dt As New DataTable
        Dim ObjParent As New clsData
        Dim DatsMenu As New DataSet
        DatsMenu = ObjParent.fnDataSet(selectCommand.ToString, CommandType.Text)
        If Not IsNothing(DatsMenu) Then
            If DatsMenu.Tables.Count > 0 Then
                If DatsMenu.Tables(0).Rows.Count > 0 Then
                    dt = DatsMenu.Tables(0)
                    Session("DatSubMenu") = DatsMenu
                Else
                    Response.Redirect("AspxLostSessions.aspx", False)
                End If
            Else
                Response.Redirect("AspxLostSessions.aspx", False)
            End If
        Else
            Response.Redirect("AspxLostSessions.aspx", False)
        End If
        Return dt
    End Function

    Protected Sub rptMenu_ItemDataBound(ByVal sender As Object, ByVal e As RepeaterItemEventArgs)
        If ((e.Item.ItemType = ListItemType.Item) _
                    OrElse (e.Item.ItemType = ListItemType.AlternatingItem)) Then
            If (Not (allMenus) Is Nothing) Then
                Dim drv As DataRowView = CType(e.Item.DataItem, DataRowView)
                Dim idMainMenu As String = drv("idMainMenu").ToString
                If idMainMenu < 999 Then
                    Dim rows() As DataRow = allMenus.Select(("ParentCategoryID=" + idMainMenu), "MenuOrder")
                    If (rows.Length > 0) Then
                        Dim sb As StringBuilder = New StringBuilder
                        For Each item As DataRow In rows
                            sb.Append("<li><a href=" + item("uri") + ">" + item("MenuName") + "</a></li>")
                        Next
                        CType(e.Item.FindControl("ltrlSubMenu"), Literal).Text = sb.ToString
                    End If
                Else

                    Dim StrLinkBtnChngPWD As String = "<a href=""../AspxPages/AspxChangePassword.aspx""><span class=""urlspan-submenu"">Change Password</span><span class=""iconspan-submenu""><img src=""../Resource/Images/Login/SignOut.png"" name=""ImgSignOut"" border=""0"" height=""20"" width=""20"" alt="""" /></span></a>"
                    ''CType(e.Item.FindControl("ltrlSubMenu"), Literal).Text = StrLinkBtnChngPWD
                    Dim StrLinkBtn As String = "<a href=""../Default.aspx"" onclick=""javascript:CallServerCodeLogout();""><span class=""urlspan-submenu"">Sign Out</span><span class=""iconspan-submenu""><img src=""../Resource/Images/Login/SignOut.png"" name=""ImgSignOut"" border=""0"" height=""20"" width=""20"" alt="""" /></span></a>"
                    StrLinkBtn = StrLinkBtnChngPWD & StrLinkBtn
                    CType(e.Item.FindControl("ltrlSubMenu"), Literal).Text = StrLinkBtn
                End If
            End If
        End If
    End Sub
End Class

