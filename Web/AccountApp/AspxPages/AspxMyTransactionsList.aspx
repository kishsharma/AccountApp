<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Account.master" AutoEventWireup="false" CodeFile="AspxMyTransactionsList.aspx.vb" Inherits="AspxPages_AspxMyTransactionsList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <link href="../Resource/js/Chosen/chosen.css" rel="stylesheet" type="text/css" />
    <%-- <script src="../Resource/ResponsiveList/media/js/jquery.js" type="text/javascript"></script>--%>
    <script src="../Resource/ResponsiveList/media/js/jquery.dataTables.js" type="text/javascript"></script>
    <link rel="stylesheet" type="text/css" href="https://cdn.datatables.net/1.10.15/css/jquery.dataTables.min.css" />
    <script type="text/javascript" language="javascript" class="init">
        $(document).ready(function () {
            $('#DataList').DataTable();

        });
    </script>
    <script>
        $(function () {
            $("#TxtTransactionDtFrm").datepicker({
                dateFormat: 'dd-M-yy',
                changeMonth: true,
                changeYear:true,
                onClose: function () {
                    $(this).trigger('blur');
                }
            }).val();
            $("#TxtTransactionDtTo").datepicker({
                dateFormat: 'dd-M-yy',
                changeMonth: true,
                changeYear:true,
                onClose: function () {
                    $(this).trigger('blur');
                }
            }).val();
        });
    </script>
    <form id="FrmSearchList" runat="server">
    <asp:HiddenField ID="HdnIdBlock" runat="server" ClientIDMode="Static" />
        <div class="filter_list_box" id="divMain" runat="server" style="margin: 40px 0 0 0">
            <div class="filter_list" style="width:95%;height: 90%;min-height:65px" >
                    <div class="filter_content" style="height:30px;min-height:30px">Transaction Type</div>
<label class="container" style="display:inline;padding-left:50px">Purchase
  <input type="radio" id="RdPUR" runat="server"  name="radio" value="6">
  <span class="checkmark"  style="left:135px"></span>
</label> 
                              <label class="container" style="display:inline;padding-left:50px">Sales
  <input type="radio"  id="RdSL" runat="server" name="radio" value="7">
  <span class="checkmark"  style="left:115px" ></span>
</label> 
        
                             <label class="container" style="display:inline;padding-left:50px">Receipt
  <input type="radio"  id="RdRCPT" runat="server"  name="radio" value="10">
  <span class="checkmark"  style="left:135px"></span>
</label> 
                              <label class="container" style="display:inline;padding-left:50px">Payment
  <input type="radio"  id="RdPAY" runat="server"  name="radio" value="9">
  <span class="checkmark" style="left:135px"></span>
</label> 
                                                   <label class="container" style="display:inline;padding-left:50px">Others
  <input type="radio"  id="RdJE" runat="server"  name="radio" value="8">
  <span class="checkmark"  style="left:135px"></span>
</label>
                
                    </div>
            
                
        <div class="company_list" id="Companylist" runat="server">
            <label>
                Company</label>
            <asp:DropDownList ID="DDLCompany" AutoPostBack="false" class="chosen-select" runat="server" TabIndex="2" />
        </div>
        <div class="company_list date_box">
            <label>
                Transaction Date From:*
            </label>
            <asp:TextBox ID="TxtTransactionDtFrm" runat="server" ClientIDMode="Static" class="form-control"
                placeholder="Transaction Date From (DD-MMM-YYYY)"
                TabIndex="3" MaxLength="11" />
        </div>
        <div class="company_list date_box">
            <label>
                Transaction Date To:*
            </label>
            <asp:TextBox ID="TxtTransactionDtTo" runat="server" ClientIDMode="Static" class="form-control"
                placeholder="Transaction Date To (DD-MMM-YYYY)"
                TabIndex="3" MaxLength="11" />
        </div>
            <div class="company_list">
            <label>
                Bill Number:
            </label>
            <asp:TextBox ID="txtBillNo" runat="server" ClientIDMode="Static" class="form-control"
                placeholder="Bill Number"     MaxLength="100" />
        </div>
            <div class="company_list">
            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="submit_btn" TabIndex="14" />
            <asp:Button ID="btnreset" runat="server" Text="Reset" CssClass="submit_btn" TabIndex="15" OnClientClick="return resetFields()" />
        </div>
    </div>
    <input id="LblScreenName" runat="server" value="Screen Name" class="LableHeader" visible="false" />
    <div class="filter_list_box">
        <asp:Label ID="LblSearchData" runat="server" />
    </div>
    <script src="../Resource/js/Chosen/chosen.jquery.js" type="text/javascript"></script>
    <script type="text/javascript">
        var config = {
            '.chosen-select': {},
            '.chosen-select-deselect': { allow_single_deselect: true },
            '.chosen-select-no-single': { disable_search_threshold: 10 },
            '.chosen-select-no-results': { no_results_text: 'Oops, nothing found!' },
            '.chosen-select-width': { width: "95%" }
        }
        for (var selector in config) {
            $(selector).chosen(config[selector]);
        }
    </script>
    </form>

</asp:Content>

