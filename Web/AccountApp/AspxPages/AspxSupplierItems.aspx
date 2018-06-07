<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Account.master" AutoEventWireup="false" CodeFile="AspxSupplierItems.aspx.vb" Inherits="AspxPages_AspxSupplierItems" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <link href="../Resource/js/Chosen/chosen.css" rel="stylesheet" type="text/css" />
    <script>
    $(function () {
        $("#TxtWEFDate").datepicker({
            changeMonth: true,
                changeYear:true,
            dateFormat: 'dd-M-yy'
        }).val();
    });
    </script>
    
    <form id="frmUsers" runat="server"  clientidmode="Static" role="form">
        
        <div class="total_box" style="float: left; width: 100%; height: auto; margin: 20px 0px;">
            <div class="company_list" id="Div1" runat="server">
                <label>
                    Client</label>
                <asp:DropDownList ID="DDLClient" class="chosen-select" runat="server" TabIndex="1" AutoPostBack="true" />
            </div>
            <div class="company_list" id="Companylist" runat="server">
                <label>
                    Supplier</label>
                <asp:DropDownList ID="DDLSupplier" class="chosen-select" runat="server" TabIndex="2" AutoPostBack="true"  />
            </div>
        </div>
        <div style="float: left; width: 100%; height: auto; margin: 20px 0px;">
            <div class="company_list">
                <label id="Label2" runat="server">
                    Item Name</label>
                    <asp:TextBox id="TxtItemName" runat="server"  class="form-control" placeholder="Item Name"
                    clientidmode="Static" tabindex="3" maxlength="100" onkeypress="return IsAlphaNumeric(event);" />
            </div>
            <div class="company_list">
                <label id="Label3" runat="server">
                    Unit Of Measurement</label>
                <asp:DropDownList ID="DDLMeasure" class="chosen-select" runat="server" TabIndex="4" />
            </div>
            <div class="company_list">
                <label id="Label1" runat="server">
                    HSN Code</label>
                    <asp:TextBox id="TxtHSNCode" runat="server"  class="form-control" placeholder="HSN Code"
                    clientidmode="Static" tabindex="5" maxlength="100" onkeypress="return IsAlphaNumeric(event);" />
            </div>
            <div class="company_list">
                <label id="Label4" runat="server">
                    CGST Rate</label>
                    <asp:TextBox id="TxtCGSTRate" runat="server"  class="form-control" placeholder="CGST Rate"
                    clientidmode="Static" tabindex="6" maxlength="100" onkeypress="return IsNumericforDecimal(event);"/>
            </div>
            <div class="company_list">
                <label id="Label5" runat="server">
                    SGST Rate</label>
                    <asp:TextBox id="TxtSGSTRate" runat="server"  class="form-control" placeholder="SGST Rate"
                    clientidmode="Static" tabindex="7" maxlength="100" onkeypress="return IsNumericforDecimal(event);" />
            </div>
            <div class="company_list">
                <label id="Label6" runat="server">
                    IGST Rate</label>
                    <asp:TextBox id="TxtIGSTRate" runat="server"  class="form-control" placeholder="IGST Rate"
                    clientidmode="Static" tabindex="8" maxlength="100" onkeypress="return IsNumericforDecimal(event);" />
            </div>
            <div class="company_list company_list2">
                    <label>
                        With Effect From:</label>
                    <input type="text" class="form-control" runat="server" id="TxtWEFDate" placeholder=" Date With Effect From"
                        clientidmode="Static" tabindex="9" maxlength="100" />
                    <%--<asp:TextBox ID="TxtSupplyDate" runat="server" ClientIDMode="Static" class="form-control"
                    placeholder="Supply Date" TabIndex="9" MaxLength="11" />--%>
                </div>
                
        </div>
        <div style="float: left; width: 100%; height: auto; margin: 20px 0px;">
            <div class="company_list">
                <asp:Button ID="btnAddItem" runat="server" Text="Add Item" CssClass="submit_btn"
                    OnClientClick="return validate()" TabIndex="5" />
                <asp:Button ID="btnResetItem" runat="server" Text="Reset" CssClass="submit_btn" OnClientClick="return resetFields()" />
            </div>
            <div class="heading16" style="border: thin">
                <asp:GridView ID="Gvdetails" CssClass="total_list" HeaderStyle-ForeColor="Black" ShowHeader="True" runat="server" AutoGenerateColumns="false">
                    <Columns>
                        <asp:TemplateField HeaderText="SlNo" ItemStyle-Width="37px">
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <%# Container.DataItemIndex+1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="ItemName" HeaderText="ItemName">
                            <ItemStyle VerticalAlign="Middle" HorizontalAlign="left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Measure" HtmlEncode="false" HeaderText="Measure">
                            <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="180px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="HSNCode" HtmlEncode="false" HeaderText="HSNCode">
                            <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="180px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="IGSTRATE" HtmlEncode="false" HeaderText="IGSTRATE">
                            <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="180px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="CGSTRATE" HtmlEncode="false" HeaderText="CGSTRATE">
                            <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="180px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="SGSTRATE" HtmlEncode="false" HeaderText="SGSTRATE">
                            <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="180px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="WEFDate" HtmlEncode="false" HeaderText="WEFDate">
                            <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" Width="180px" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
        
    </form>
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
    <script type="text/javascript">
        function IsNumericforDecimal(e) {
            var keyCode = e.which ? e.which : e.keyCode
            var ret = ((keyCode >= 48 && keyCode <= 57) || keyCode == 46 || (specialKeys.indexOf(keyCode) != -1 && e.charCode != e.keyCode));
            return ret;
        }
        function resetFields() {
            document.getElementById("<%=txtItemName.ClientID%>").value = "";
            document.getElementById("<%=DDLClient.ClientID%>").value = "0";
            document.getElementById("<%=DDLSupplier.ClientID%>").value = "0";
            document.getElementById("<%=DDLMeasure.ClientID%>").value = "0";
        }
    </script>
</asp:Content>

