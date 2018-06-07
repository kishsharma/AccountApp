<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Account.master" AutoEventWireup="false"
    EnableEventValidation="false" CodeFile="AspxInvoiceForm.aspx.vb" Inherits="AspxPages_AspxInvoiceForm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style>
        table.display {
            width: 100%;
            margin: 0 auto;
            clear: both;
            border-collapse: separate;
            border-spacing: 0;
        }

            table.display tbody th {
                padding: 5px 5px;
                border: 1px solid #fff;
                background-color: #00bcd5;
                color: White;
            }

        #dvGSTdetail {
            display: inline-table;
        }

        table.display tbody td {
            background-color: White;
            padding: 5px 5px;
            border: 1px solid black;
        }

        table.display tbody th.rowDiff {
            padding: 5px 5px;
            color: Black;
            background-color: #A8A8A8;
        }

        table.display tbody tr td span a.submit_btn {
            background-color: #fd4a84;
            border-radius: 3px;
            color: #fff;
            cursor: pointer;
            float: left;
            font-family: arial;
            font-size: 15px;
            font-weight: bold;
            height: auto;
            padding: 3px 3px;
            text-indent: inherit;
            width: auto;
            text-transform: uppercase;
            text-decoration: none;
        }

        .invoicemaster {
            width: 12% !important;
        }

        .company_list2 {
            width: 20% !important;
        }

        .ui-autocomplete-loading {
            background: white url("../Resource/images/ui-anim_basic_16x16.gif") right center no-repeat;
        }
    </style>
    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">
    <link rel="stylesheet" href="/resources/demos/style.css">


    <script>
        $(document).ready(function () {
            $("#<%=txtHSNCode.ClientID %>").autocomplete({
              source: function (request, response) {
                  $.ajax({
                      url: "../AutocompleteHSN.asmx/HSNcodeAutocomplete",
                      data: "{ 'HSNCODe': '" + request.term + "', 'limit': '10' }",
                      dataType: "json",
                      type: "POST",
                      contentType: "application/json; charset=utf-8",
                      dataFilter: function (data) { return data; },
                      /*      success: function (data) {
                                
                                response(data.d)
                            }*/
                      success: function (data) {
                          response($.map(data.d, function (item) {
                              return {
                                  label: item.split('-')[0],
                                  val: item.split('-')[1],
                                  cgst: item.split('-')[2],
                                  sgst: item.split('-')[3],
                                  igst: item.split('-')[4],
                                  gstDesc: item.split('-')[5]

                              }
                          }))
                      },

                  });
              },
              minLength: 2,
              select: function (event, ui) {
                  console.log("Selected: " + ui.item.value + " aka " + ui.item.val + "cgst:" + ui.item.cgst + "sgst:" + ui.item.sgst + "igst:" + ui.item.igst);
                  $('#txtCGSTRate').val(ui.item.cgst);
                  $('#txtSGSTRate').val(ui.item.sgst);
                  $('#txtIGSTRate').val(ui.item.igst);
                  $('#txtHsnDescription').val(ui.item.gstDesc);

              },
              open: function () {
                  $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
              },
              close: function () {
                  $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
              }

          });

          /*item List*/
          $("#<%=txtItemName.ClientID %>").autocomplete({
              source: function (request, response) {
                  $.ajax({
                      url: "../AutocompleteHSN.asmx/ItemListInvoice",
                      data: "{ 'ItemName': '" + request.term + "', 'limit': '10' }",
                      dataType: "json",
                      type: "POST",
                      contentType: "application/json; charset=utf-8",
                      dataFilter: function (data) { return data; },
                      success: function (data) {
                          response($.map(data.d, function (item) {
                              return {
                                  label: item.split('::')[0],
                                  val: item.split('::')[1]


                              }
                          }))
                      },
                      //                      item:"",
                      //                      success: function (data) {
                      //                          var result = data.d;
                      //                          response($.map(data.d, function (item) {
                      //                              return {
                      //                                  label: item.HSNCODE,
                      //                                  value: item.Age
                      //                              }
                      //                          }));
                      //                      }


                  });
              },
              minLength: 2,
              select: function (event, ui) {
                  console.log("Selected: " + ui.item.value + " UOM " + ui.item.val);
                  $('#<%= txtUOM.ClientID %>').val(ui.item.val);
                },
                open: function () {
                    $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
                },
                close: function () {
                    $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
                }

            });

        });
    </script>

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script>
        $(function () {
            $("#TxtSupplyDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd-M-yy'
            }).val();
            $("#txtInvoiceDate").datepicker({
                changeMonth: true,
                changeYear: true,
                dateFormat: 'dd-M-yy'
            }).val();

        });
    </script>
    <form id="frmAssignimage" runat="server" role="form">
        <asp:HiddenField ID="hdnIDInvoiceMaster" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hdnClientid" runat="server" ClientIDMode="Static" />
        <asp:ScriptManager ID="SMLocationMaster" runat="server" EnablePageMethods="true"
            ScriptMode="Release">
        </asp:ScriptManager>
        <asp:Panel ID="pnlPerson" runat="server">
            <div class="company_box">
                <div class="company_list company_list2">
                    <a id="imgcompanyId" runat="server"></a>
                </div>
                <div class="company_list company_list2">
                    <label id="lblCompanyName" runat="server">
                    </label>
                    <label id="lblCompanyAddress" runat="server">
                    </label>
                    <label id="lblCompayNo" runat="server">
                    </label>
                    <label id="lblGstin" runat="server">
                        GSTIN: 27ABLPD8022B1Z3</label>
                    <label id="lblCompanystate" runat="server"></label>
                    <label style="display: none" id="lblCompanyGstStateCode" runat="server"></label>

                </div>
                <div class="company_list company_list2">
                    <label>
                        Company Logo</label>
                </div>
                <div class="heading16" style="background-color: #00bcd5; color: #fff; margin: 0 0 20px 0; padding: 8px 0; text-align: center; width: 100%;">
                    <strong>Tax Invoice</strong>
                </div>
                <div class="info_box" style="width: 100%; overflow: hidden;">
                    <div class="company_list invoicemaster">
                        <label>
                            Invoice date:</label>
                        <input type="text" class="form-control" runat="server" id="txtInvoiceDate" placeholder=" Invoice Date" oncopy="return false" onpaste="return false" oncut="return false"
                            clientidmode="Static" tabindex="1" maxlength="100" />
                        <%-- <asp:TextBox ID="txtInvoiceDate" runat="server" ClientIDMode="Static" class="form-control"
                    placeholder="Invoice Date" TabIndex="2" MaxLength="11" />--%>
                    </div>
                    <div class="company_list invoicemaster" style="display: ;">
                        <label>
                            Invoice No:</label>
                        <input type="text" class="form-control" runat="server" id="txtInvoiceNo" value="101000" placeholder=" Invoice No" onchange="javascript:ValidateInvoiceNo();"
                            clientidmode="Static" tabindex="2" maxlength="100" />
                    </div>
                    <div class="company_list invoicemaster">
                        <label style="width: 200px">
                            Reverse Charge (Y/N):</label>
                        <asp:DropDownList ID="DDLReverseCharge" runat="server" TabIndex="3" class="chosen-select">
                            <asp:ListItem Value="YES" Text="YES" />
                            <asp:ListItem Value="NO" Text="NO" />
                        </asp:DropDownList>
                    </div>
                    <div class="company_list invoicemaster" style="display: none;">
                        <label>
                            State:</label>
                        <input type="text" class="form-control" runat="server" id="txtInvoiceState" placeholder=" State"
                            clientidmode="Static" tabindex="4" maxlength="100" />
                    </div>
                    <div class="company_list invoicemaster">
                        <label>
                            Transport Mode:</label>
                        <input type="text" class="form-control" runat="server" id="txtTransportMode" placeholder=" Transport Mode"
                            clientidmode="Static" tabindex="4" maxlength="100" />
                    </div>
                    <div class="company_list invoicemaster" style="display: none">
                        <label>
                            Delivery Note:</label>
                        <input type="text" class="form-control" runat="server" id="txtInvoiceCode" placeholder=" Code" oncopy="return false" onpaste="return false" oncut="return false"
                            onkeypress="return IsNumericforDecimal(event);" clientidmode="Static" tabindex="5"
                            maxlength="100" />
                    </div>
                    <div class="company_list invoicemaster">
                        <label>
                            Vehicle number:</label>
                        <input type="text" class="form-control" runat="server" id="txtVehicleNo" placeholder=" Vehicle number"
                            clientidmode="Static" tabindex="5" maxlength="100" />
                    </div>
                    <div class="company_list invoicemaster">
                        <label>
                            Date of Supply:</label>
                        <input type="text" class="form-control" runat="server" id="TxtSupplyDate" placeholder=" Supply Date" oncopy="return false" onpaste="return false" oncut="return false"
                            clientidmode="Static" tabindex="6" maxlength="100" />
                        <%--<asp:TextBox ID="TxtSupplyDate" runat="server" ClientIDMode="Static" class="form-control"
                    placeholder="Supply Date" TabIndex="9" MaxLength="11" />--%>
                    </div>
                    <div class="company_list invoicemaster">
                        <label>
                            Place of Supply :</label>
                        <input type="text" class="form-control" runat="server" id="txtSupplyPlace" placeholder=" Supply Place"
                            clientidmode="Static" tabindex="7" maxlength="100" />
                    </div>
                </div>
                <div class="info_box" style="width: 50%; overflow: hidden;">
                    <div class="company_list" style="width: 90%; overflow: hidden;">
                        <label>
                            Mode / Terms of Payment:</label>
                        <%--<input type="text" class="form-control" runat="server" id="TxtModeTermsofPayment" placeholder=" Code" oncopy="return false" onpaste="return false" oncut="return false"
                        onkeypress="return IsNumericforDecimal(event);" clientidmode="Static" tabindex="5" aria-multiline="true"
                        maxlength="100" />--%>

                        <textarea class="form-control" runat="server" tabindex="9" id="TxtModeTermsofPayment" cols="40" rows="5"></textarea>

                    </div>
                </div>
                <div class="info_box" style="width: 50%; overflow: hidden;">
                    <div class="company_list" style="width: 90%; overflow: hidden;">
                        <label>
                            Terms of Delivery:</label>
                        <%--                    <input type="text" class="form-control" runat="server" id="TxtTermsofDelivery" placeholder=" Code" oncopy="return false" onpaste="return false" oncut="return false"
                        onkeypress="return IsNumericforDecimal(event);" clientidmode="Static" tabindex="5" aria-multiline="true"
                        maxlength="100" />--%>
                        <textarea class="form-control" runat="server" id="TxtTermsofDelivery" placeholder=" Code" tabindex="8" cols="40" rows="5"></textarea>

                    </div>



                </div>
            </div>

            <div class="info_box" style="width: 100%; overflow: hidden;">
                <div class="heading16" style="background-color: #00bcd5; color: #fff; margin: 0 0 20px 0; padding: 8px 0; text-align: center; width: 100%;">
                    <strong>Buyer</strong>
                </div>
                <div class="company_list company_list2">
                    <label>
                        Name:</label>
                    <input type="text" class="form-control" runat="server" id="txtBillName" placeholder=" Name"
                        clientidmode="Static" tabindex="9" maxlength="100" />
                </div>

                <div class="company_list company_list2">
                    <label>
                        Address:</label>
                    <input type="text" class="form-control" runat="server" id="txtBillAddress" placeholder=" Address"
                        clientidmode="Static" tabindex="10" maxlength="100" />
                </div>
                <div class="company_list company_list2">
                    <label>
                        State:</label>
                    <%--<input type="text" class="form-control" runat="server" id="txtBillState" placeholder=" State"
                    clientidmode="Static" tabindex="11" maxlength="100" />--%>
                    <asp:DropDownList runat="server" ClientIDMode="Static" class="form-control" TabIndex="11" ID="ddlBuyerState"></asp:DropDownList>
                </div>
                <div class="company_list company_list2">
                    <label>
                        GSTIN:</label>
                    <input type="text" class="form-control" runat="server" id="txtBillGSTIN" placeholder=" GSTIN"
                        clientidmode="Static" tabindex="12" maxlength="100" />
                </div>
                <div class="company_list company_list2" style="display: none;">
                    <label>
                        Code:</label>
                    <input type="text" class="form-control" runat="server" id="txtBillCode" placeholder=" Code" oncopy="return false" onpaste="return false" oncut="return false"
                        onkeypress="return IsNumericforDecimal(event);" clientidmode="Static" tabindex="13"
                        maxlength="100" />
                </div>
            </div>

            <div class="heading16" style="background-color: red; color: #fff; margin: 0 0 20px 0; padding: 8px 0; text-align: center; width: 100%;">
                <input type="checkbox" id="ChkAutofillPartyDetails" class="form-control" runat="server" clientidmode="Static"
                    tabindex="15" onclick="fnAutofillPartyDetails();" />
                <label class="panel_list_text" for="ChkAutofillPartyDetails">
                    <span></span><strong>Click - Buyer and Consignee are Same</strong>
                </label>
                <%--<strong>Buyer</strong>--%>
            </div>
            <div class="info_box" style="width: 100%; overflow: hidden;">
                <div class="company_list company_list2">
                    <label>
                        Name:</label>
                    <input type="text" class="form-control" runat="server" id="txtShipName" placeholder=" Name"
                        clientidmode="Static" tabindex="14" maxlength="100" />
                </div>
                <div class="company_list company_list2">
                    <label>
                        Address:</label>
                    <input type="text" class="form-control" runat="server" id="txtShipAddress" placeholder=" Address"
                        clientidmode="Static" tabindex="15" maxlength="100" />
                </div>
                <div class="company_list company_list2">
                    <label>
                        State:</label>
                    <%--<input type="text" class="form-control" runat="server" id="txtShipState" placeholder=" State"
                    clientidmode="Static" tabindex="16" maxlength="100" />--%>
                    <asp:DropDownList runat="server" class="form-control" ID="ddlConsigneeState" ClientIDMode="Static" TabIndex="16"></asp:DropDownList>
                </div>
                <div class="company_list company_list2">
                    <label>
                        GSTIN:</label>
                    <input type="text" class="form-control" runat="server" id="txtShipGSTIN" placeholder=" GSTIN"
                        clientidmode="Static" tabindex="17" maxlength="100" />
                </div>
                <div class="company_list company_list2" style="display: none;">
                    <label>
                        GST Code:</label>
                    <input type="text" class="form-control" runat="server" id="txtShipCode" placeholder=" Code" oncopy="return false" onpaste="return false" oncut="return false"
                        onkeypress="return IsNumericforDecimal(event);" clientidmode="Static" tabindex="18"
                        maxlength="100" />
                </div>
            </div>


            <div class="heading16" style="background-color: #00bcd5; color: #fff; margin: 0 0 20px 0; padding: 8px 0; text-align: center; width: 100%;">
                <strong>Invoice Details</strong>
            </div>
            <asp:UpdatePanel ID="updtPnlPopUp1" runat="server" ChildrenAsTriggers="false" UpdateMode="Conditional">
                <ContentTemplate>
                    <fieldset>
                        <div class="company_list" style="width: 10%;">
                            <label>
                                HSN CODE</label>
                            <input type="text" class="form-control" runat="server" id="txtHSNCode" placeholder=" HSN CODE"
                                clientidmode="Static" tabindex="19" maxlength="100" onkeypress="return IsAlphaNumeric(event);" />
                        </div>
                        <div class="company_list" style="width: 20%;">
                            <label>
                                Item Name</label>
                            <%--                        <asp:DropDownList ID="txtItemName" class="chosen-select" runat="server" TabIndex="22"
                            AutoPostBack="true" />--%>
                            <input type="search" class="form-control" runat="server" id="txtItemName" placeholder=" Item Name"
                                clientidmode="Static" tabindex="20" maxlength="100" onkeypress="return IsAlphaNumeric(event);" />

                        </div>

                        <div class="company_list" style="width: 10%;">
                            <label>
                                UOM</label>
                            <%--<input type="text" class="form-control" disabled="disabled" runat="server" id="txtUOM"
                            placeholder=" UOM" clientidmode="Static" tabindex="24" maxlength="100" onkeypress="return IsAlphaNumeric(event);" />--%>
                            <asp:DropDownList ID="txtUOM" class="chosen-select" runat="server" TabIndex="21" />
                        </div>
                        <div class="company_list" style="width: 10%;">
                            <label>
                                Qty.</label>
                            <input type="text" class="form-control" runat="server" id="txtNos" placeholder=" Nos."
                                clientidmode="Static" tabindex="22" maxlength="100"
                                onkeypress="return IsNumericforDecimal(event);" />
                        </div>
                        <div class="company_list" style="width: 10%;">
                            <label>
                                Rate</label>
                            <input type="text" class="form-control" runat="server" id="txtRate" placeholder=" Rate"
                                onblur="CalculateAmount(this.value);" clientidmode="Static" tabindex="23" maxlength="100"
                                onkeypress="return IsNumericforDecimal(event);" />
                        </div>
                        <div class="company_list" style="width: 10%;">
                            <label>
                                Amount</label>
                            <input type="text" disabled="disabled" class="form-control" runat="server" id="txtAmount"
                                placeholder=" Amount" clientidmode="Static" maxlength="100" onkeypress="return IsNumericforDecimal(event);" />
                        </div>
                        <div class="company_list" style="width: 10%;">
                            <label>
                                Discount Amount</label>
                            <input type="text" class="form-control" runat="server" id="txtDiscAmt" placeholder=" Discount Amt"
                                clientidmode="Static" tabindex="24" maxlength="100" onblur="CalculateRateDiscount(this.value);"
                                onkeypress="return IsNumericforDecimal(event);" />
                        </div>
                        <div class="company_list" style="width: 10%;">
                            <label>
                                Taxable Value</label>
                            <input type="text" disabled="disabled" class="form-control" runat="server" id="txtTaxValue"
                                placeholder=" Duty Tax Value" clientidmode="Static" maxlength="100" onkeypress="return IsAlphaNumeric(event);" />
                        </div>

                        <div class="company_list" style="width: 30%; overflow: hidden;">
                            <label>
                                HSN Description</label>
                            <textarea type="text" class="form-control" runat="server" id="txtHsnDescription" placeholder="Read HSN Description" readonly aria-multiline
                                clientidmode="Static" style="font-family: Courier New, Courier, monospace" cols="20" rows="2" tabindex="25" maxlength="100"></textarea>
                        </div>
                        <div class="company_list" style="width: 30%; overflow: hidden;">
                            <label>
                                Item Description</label>
                            <textarea cols="20" rows="2" type="search" class="form-control" runat="server" id="txtItemDescription" placeholder=" item Description"
                                clientidmode="Static" style="font-family: Courier New, Courier, monospace" tabindex="26" maxlength="100"></textarea>
                        </div>

                        <%--Hide below part from here once the calculation is done--%>
                        <div id="dvGSTdetail">
                            <div class="company_list" style="width: 10%;">
                                <label>
                                    IGST Rate</label>
                                <input type="text" class="form-control" runat="server" id="txtIGSTRate" placeholder=" IGST Rate"
                                    onblur="CalculateIGST(this.value);" clientidmode="Static" tabindex="28" maxlength="100"
                                    onkeypress="return IsNumericforDecimal(event);" />
                            </div>
                            <div class="company_list" style="width: 10%;">
                                <label>
                                    IGST Amount</label>
                                <input type="text" class="form-control" runat="server" id="txtIGSTAmount" placeholder=" IGST Amount"
                                    disabled="disabled" clientidmode="Static" maxlength="100" onkeypress="return IsNumericforDecimal(event);" />
                            </div>
                            <div class="company_list" style="width: 10%;">
                                <label>
                                    CGST Rate</label>
                                <input type="text" class="form-control" runat="server" id="txtCGSTRate" placeholder=" CGST Rate"
                                    onblur="CalculateCGST(this.value);" clientidmode="Static" tabindex="28" maxlength="100"
                                    onkeypress="return IsNumericforDecimal(event);" />
                            </div>
                            <div class="company_list" style="width: 10%;">
                                <label>
                                    CGST Amount</label>
                                <input type="text" class="form-control" runat="server" id="txtCGSTAmount" placeholder=" CGST Amount"
                                    disabled="disabled" clientidmode="Static" maxlength="100" onkeypress="return IsNumericforDecimal(event);" />
                            </div>
                            <div class="company_list" style="width: 10%;">
                                <label>
                                    SGST Rate</label>
                                <input type="text" class="form-control" runat="server" id="txtSGSTRate" placeholder=" SGST Rate"
                                    onblur="CalculateSGST(this.value);" clientidmode="Static" tabindex="29" maxlength="100"
                                    onkeypress="return IsNumericforDecimal(event);" />
                            </div>
                            <div class="company_list" style="width: 10%;">
                                <label>
                                    SGST Amount</label>
                                <input type="text" class="form-control" runat="server" id="txtSGSTAmount" placeholder=" SGST Amount"
                                    disabled="disabled" clientidmode="Static" maxlength="100" onkeypress="return IsNumericforDecimal(event);" />
                            </div>
                            <div class="company_list" style="width: 10%;">
                                <label>
                                    Total Value</label>
                                <input type="text" disabled="disabled" class="form-control" runat="server" id="txtTotalValue"
                                    placeholder=" Total Value" clientidmode="Static" maxlength="100" onkeypress="return IsAlphaNumeric(event);" />
                            </div>
                        </div>
                        <div class="company_list" style="padding: 14px">
                            <label></label>
                            <asp:Button ID="btnAddItem" runat="server" Text="Add Item" CssClass="submit_btn"
                                OnClientClick="return validate()" TabIndex="27" />

                            <asp:Button ID="btnResetItem" runat="server" Text="Reset" CssClass="submit_btn" OnClientClick="return resetFields()"
                                TabIndex="31" />
                        </div>
                    </fieldset>
                    <div class="heading16" style="border: thin; overflow: scroll;">
                        <asp:GridView ID="GridView1" CssClass="display" HeaderStyle-ForeColor="Black" ShowHeader="True"
                            runat="server" AutoGenerateColumns="false">
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <tr>
                                            <th colspan="9"></th>
                                            <th colspan="2">IGST Tax
                                            </th>
                                            <th colspan="2">CGST Tax
                                            </th>
                                            <th colspan="2">SGST Tax
                                            </th>
                                            <th></th>
                                        </tr>
                                    </HeaderTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="SlNo">
                                    <ItemStyle HorizontalAlign="Center" />
                                    <ItemTemplate>
                                        <%# Container.DataItemIndex + 1 %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="ItemName">
                                    <ItemTemplate>


                                        <span><%#Eval("ItemName") %> </span>
                                        <span style="font-family: Courier New, Courier, monospace"><%#Eval("ItemDescription") %> </span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--<asp:BoundField DataField="ItemName" HeaderText="ItemName" DataFormatString="">
                                <ItemStyle VerticalAlign="Middle" HorizontalAlign="left" />
                                
                            </asp:BoundField>--%>
                                <asp:BoundField DataField="HSNCode" HeaderText="HSNCode">
                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="UOM" HtmlEncode="false" HeaderText="UOM">
                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Nos" HeaderText="Nos">
                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Rate" HeaderText="Rate">
                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Amount" HeaderText="Amount">
                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Discount" HeaderText="Discount">
                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="TaxValue" HeaderText="TaxValue">
                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="IGSTRate" HeaderText="IGSTRate">
                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="IGSTAmount" HeaderText="IGSTAmount">
                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CGSTRate" HeaderText="CGSTRate">
                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CGSTAmount" HeaderText="CGSTAmount">
                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SGSTRate" HeaderText="SGSTRate">
                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="SGSTAmount" HeaderText="SGSTAmount">
                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="TotalAmount" HeaderText="TotalAmount">
                                    <ItemStyle VerticalAlign="Middle" HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Delete?">
                                    <ItemTemplate>
                                        <span onclick="return confirm('Are you sure to Delete the record?')">
                                            <asp:LinkButton ID="lnkB" CssClass="submit_btn" runat="Server" Text="Delete" CommandName="Delete"></asp:LinkButton>
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div id="divWords" runat="server" class="company_list" style="width: 68%; display: none;">
                        <label style="background-color: #00bcd5; color: #fff; margin: 0 0 0px 0; padding: 5px 0; text-align: center; width: 100%;">
                            Total Invoice amount in words</label>
                        <label id="Label3" runat="server">
                            <p id="spamtword" runat="server" style="width: 300px; font-size: medium; word-wrap: break-word;">
                            </p>
                        </label>
                        <div class="total_list">
                        </div>
                        <div class="total_list">
                        </div>
                        <div class="total_list">
                        </div>
                        <div class="total_list">
                        </div>
                        <div class="total_list">
                        </div>
                        <div class="total_list">
                        </div>
                        <div class="total_list">
                        </div>
                        <div class="total_list">
                        </div>
                        <div class="total_list">
                        </div>
                        <div class="total_list">
                        </div>
                        <div class="total_list">
                        </div>
                        <div class="total_list" style="width: 50%;">
                            Common Seal
                        </div>
                    </div>
                    <div id="divTotal" runat="server" class="company_list" style="width: 28%; display: none;">
                        <div class="total_list" style="width: 50%;">
                            <label>
                                Total Amount before Tax:
                            </label>
                        </div>
                        <div class="total_list" style="width: 50%;">
                            <input type="text" disabled="disabled" class="form-control" runat="server" id="txtTotalValuebeforeTax"
                                placeholder=" Total Value" clientidmode="Static" maxlength="100" onkeypress="return IsAlphaNumeric(event);" />
                        </div>
                        <div class="total_list" style="width: 50%;">
                            <label>
                                Add: IGST:
                            </label>
                        </div>
                        <div class="total_list" style="width: 50%;">
                            <input type="text" disabled="disabled" class="form-control" runat="server" id="txtTotalIGSTTax"
                                placeholder=" Total Value" clientidmode="Static" maxlength="100" onkeypress="return IsAlphaNumeric(event);" />
                        </div>
                        <div class="total_list" style="width: 50%;">
                            <label>
                                Add: CGST:
                            </label>
                        </div>
                        <div class="total_list" style="width: 50%;">
                            <input type="text" disabled="disabled" class="form-control" runat="server" id="txtTotalCGSTTax"
                                placeholder=" Total Value" clientidmode="Static" maxlength="100" onkeypress="return IsAlphaNumeric(event);" />
                        </div>
                        <div class="total_list" style="width: 50%;">
                            <label>
                                Add: SGST:
                            </label>
                        </div>
                        <div class="total_list" style="width: 50%;">
                            <input type="text" disabled="disabled" class="form-control" runat="server" id="txtTotalSGSTTax"
                                placeholder=" Total Value" clientidmode="Static" maxlength="100" onkeypress="return IsAlphaNumeric(event);" />
                        </div>
                        <div class="total_list" style="width: 50%;">
                            <label>
                                Total Tax Amount:
                            </label>
                        </div>
                        <div class="total_list" style="width: 50%;">
                            <input type="text" disabled="disabled" class="form-control" runat="server" id="txtTotalGSTTax"
                                placeholder=" Total Value" clientidmode="Static" maxlength="100" onkeypress="return IsAlphaNumeric(event);" />
                        </div>
                        <div class="total_list" style="width: 50%;">
                            <label>
                                Total Amount after Tax:
                            </label>
                        </div>
                        <div class="total_list" style="width: 50%;">
                            <input type="text" disabled="disabled" class="form-control" runat="server" id="txtTotalAmount"
                                placeholder=" Total Value" clientidmode="Static" maxlength="100" onkeypress="return IsAlphaNumeric(event);" />
                        </div>
                        <div class="total_list" style="width: 50%;">
                            <label>
                                GST on Reverse Charge:
                            </label>
                        </div>
                        <div class="total_list" style="width: 50%;">
                            <input type="text" disabled="disabled" class="form-control" runat="server" id="txtGSTCharge"
                                placeholder=" Total Value" clientidmode="Static" maxlength="100" onkeypress="return IsAlphaNumeric(event);" />
                        </div>
                        <div class="total_list" style="width: 100%;">
                            <label>
                                Authorised signatory:
                            </label>
                        </div>
                    </div>
                    <div class="total_list" style="width: 50%;">
                        <div class="company_list" style="width: 40%;">
                            <label>
                                Bank name:
                            </label>
                            <input type="text" class="form-control" runat="server" id="txtbankName" placeholder=" Bank A/C"
                                clientidmode="Static" tabindex="28" maxlength="100" onkeypress="return IsAlphaNumeric(event);" />
                        </div>
                        <div class="company_list" style="width: 40%;">
                            <label>
                                Branch Name A/C:
                            </label>
                            <input type="text" class="form-control" runat="server" id="txtBankBranch" placeholder=" Bank A/C"
                                clientidmode="Static" tabindex="28" maxlength="100" onkeypress="return IsAlphaNumeric(event);" />
                        </div>
                        <div class="company_list" style="width: 40%;">
                            <label>
                                Bank A/C:
                            </label>
                            <input type="text" class="form-control" runat="server" id="txtBankAc" placeholder=" Bank A/C"
                                clientidmode="Static" tabindex="28" maxlength="100" onkeypress="return IsAlphaNumeric(event);" />
                        </div>
                        <div class="company_list" style="width: 40%;">
                            <label>
                                Bank IFSC:
                            </label>
                            <input type="text" class="form-control" runat="server" id="txtifsc" placeholder=" Bank IFSC"
                                clientidmode="Static" tabindex="29" maxlength="100" onkeypress="return IsAlphaNumeric(event);" />
                        </div>
                        <div class="company_list" style="width: 40%;">
                            <label>
                                Total Amount in Word(s):
                            </label>
                            <input type="text" class="form-control" runat="server" id="TxtAmtinWord" placeholder=" Total Amount in Word(s)"
                                clientidmode="Static" tabindex="32" maxlength="100" onkeypress="return IsAlphaNumeric(event);" disabled="disabled" />
                        </div>

                    </div>
                </ContentTemplate>
                <%--            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="txtItemName" EventName="SelectedIndexChanged" />
            </Triggers>--%>
            </asp:UpdatePanel>
            <div class="company_list">
                <asp:Button ID="btnSave" runat="server" Text="Save" TabIndex="33" CssClass="submit_btn"
                    OnClientClick="return validateOutSide()" />
                <asp:Button ID="btnreset" runat="server" Text="Reset" CssClass="submit_btn" OnClientClick="return resetFieldsOutside()" />
            </div>
        </asp:Panel>
    </form>
    <script type="text/javascript" language="javascript">
        var totval;
        var dec;
        var decplace;
        function deleterow(eVal) {
            alert(eVal)
            PageMethods.DeleteRow(eVal, OnSuccess, OnFailure);
        }
        function OnSuccess(valid) {
            if (valid == "True") {

            }
        }
        function OnFailure(error) {

        }
        function GetDiscount(value, value1) {
            var amt = (value1 * value);
            var taxamt = (parseFloat(amt) / 100);
            return taxamt;
        }

        var specialKeys = new Array();
        specialKeys.push(8); //Backspace
        specialKeys.push(9); //Tab
        specialKeys.push(46); //Delete
        specialKeys.push(36); //Home
        specialKeys.push(35); //End
        specialKeys.push(37); //Left
        specialKeys.push(39); //Right

        function IsNumeric(e) {
            var keyCode = e.which ? e.which : e.keyCode
            var ret = ((keyCode >= 48 && keyCode <= 57) || (specialKeys.indexOf(keyCode) != -1 && e.charCode != e.keyCode));
            return ret;
        }
        function IsNumericforRoundoff(e) {
            var keyCode = e.which ? e.which : e.keyCode

            var ret = ((keyCode >= 48 && keyCode <= 57) || keyCode == 46 || keyCode == 45 || (specialKeys.indexOf(keyCode) != -1 && e.charCode != e.keyCode));
            return ret;
        }
        function IsNumericforDecimal(e) {
            var keyCode = e.which ? e.which : e.keyCode
            var ret = ((keyCode >= 48 && keyCode <= 57) || keyCode == 46 || (specialKeys.indexOf(keyCode) != -1 && e.charCode != e.keyCode));
            return ret;
        }
        function IsAlphaNumeric(e) {
            var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
            var ret = ((keyCode == 32) || (keyCode == 39) || (keyCode >= 48 && keyCode <= 57) || (keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122) || (specialKeys.indexOf(e.keyCode) != -1 && e.charCode != e.keyCode));
            return ret;
        }
        function resetFields() {
            document.getElementById("<%=txtItemName.ClientID%>").value = "0";
            document.getElementById("<%=txtHSNCode.ClientID%>").value = "";
            document.getElementById("<%=txtUOM.ClientID%>").value = "";
            document.getElementById("<%=txtNos.ClientID%>").value = "";
            document.getElementById("<%=txtRate.ClientID%>").value = "";
            document.getElementById("<%=txtAmount.ClientID%>").value = "";
            document.getElementById("<%=txtDiscAmt.ClientID%>").value = "";
            document.getElementById("<%=txtTaxValue.ClientID%>").value = "";
            document.getElementById("<%=txtIGSTRate.ClientID%>").value = "";
            document.getElementById("<%=txtIGSTAmount.ClientID%>").value = "";
            document.getElementById("<%=txtCGSTRate.ClientID%>").value = "";
            document.getElementById("<%=txtCGSTAmount.ClientID%>").value = "";
            document.getElementById("<%=txtSGSTRate.ClientID%>").value = "";
            document.getElementById("<%=txtSGSTAmount.ClientID%>").value = "";
            document.getElementById("<%=txtTotalValue.ClientID%>").value = "";
            document.getElementById("<%=txtBankAc.ClientID%>").value = "";
            document.getElementById("<%=txtifsc.ClientID%>").value = "";
            document.getElementById("<%=TxtAmtinWord.ClientID%>").value = "";

        }

        function validate() {

            if (document.getElementById("<%=txtItemName.ClientID %>").value.trim() == "") {
                alert("Please Provide ItemName");
                document.getElementById("<%=txtItemName.ClientID %>").focus();
                return false;
            }

            if (document.getElementById("<%=txtNos.ClientID %>").value.trim() == "") {
                alert("Please Provide Nos");
                document.getElementById("<%=txtNos.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtRate.ClientID %>").value.trim() == "") {
                alert("Please Provide Rate");
                document.getElementById("<%=txtRate.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtDiscAmt.ClientID %>").value.trim() == "") {
                document.getElementById("<%=txtDiscAmt.ClientID%>").value = "0";
            }
            if (document.getElementById("<%=txtTaxValue.ClientID %>").value.trim() == "") {
                document.getElementById("<%=txtTaxValue.ClientID%>").value = "0";
            }
            if (document.getElementById("<%=txtIGSTRate.ClientID %>").value.trim() == "") {
                document.getElementById("<%=txtIGSTRate.ClientID%>").value = "0";
            }
            if (document.getElementById("<%=txtIGSTAmount.ClientID %>").value.trim() == "") {
                document.getElementById("<%=txtIGSTAmount.ClientID%>").value = "0";
            }
            if (document.getElementById("<%=txtCGSTRate.ClientID %>").value.trim() == "") {
                document.getElementById("<%=txtCGSTRate.ClientID%>").value = "0";
            }
            if (document.getElementById("<%=txtCGSTAmount.ClientID %>").value.trim() == "") {
                document.getElementById("<%=txtCGSTAmount.ClientID%>").value = "0";
            }
            if (document.getElementById("<%=txtSGSTRate.ClientID %>").value.trim() == "") {
                document.getElementById("<%=txtSGSTRate.ClientID%>").value = "0";
            }
            if (document.getElementById("<%=txtSGSTAmount.ClientID %>").value.trim() == "") {
                document.getElementById("<%=txtSGSTAmount.ClientID%>").value = "0";
            }
            if (document.getElementById("<%=txtTotalValue.ClientID %>").value.trim() == "") {
                document.getElementById("<%=txtTotalValue.ClientID%>").value = "0";
            }
        }
        function validateOutSide() {

            if (document.getElementById("<%=txtTransportMode.ClientID %>").value.trim() == "") {
                alert("Please Provide txtTransportMode");
                document.getElementById("<%=txtTransportMode.ClientID %>").focus();
                return false;
            }

            if (document.getElementById("<%=txtVehicleNo.ClientID %>").value.trim() == "") {
                alert("Please Provide txtVehicleNo");
                document.getElementById("<%=txtVehicleNo.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=TxtSupplyDate.ClientID %>").value.trim() == "") {
                alert("Please Provide TxtSupplyDate");
                document.getElementById("<%=TxtSupplyDate.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtInvoiceNo.ClientID %>").value.trim() == "") {
                alert("Please Provide Invoice Number.");
                document.getElementById("<%=txtInvoiceNo.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtInvoiceDate.ClientID %>").value.trim() == "") {
                alert("Please Provide Invoice Date.");
                document.getElementById("<%=txtInvoiceNo.ClientID %>").focus();
                return false;
            }

        }
    </script>
    <script type="text/javascript">
        function ValidateInvoiceNo() {
            var eVal = $("#<%=txtInvoiceNo.ClientID %>").val();
            PageMethods.GetNo(eVal, OnSuccess, OnFailure);
        }
        function OnSuccess(valid) {
            if (valid == "True") {
                alert("Invoice No Already taken");
                $("#<%=txtInvoiceNo.ClientID %>").val(null);
            }
        }
        function OnFailure(error) {

        }
        function CalculateAmount(val) {
            CalculateRateDiscount($('#<%=txtDiscAmt.ClientID%>').val());
            var tax = val;
            if (tax != '') {
                var nos = $get('<%=txtNos.ClientID %>').value;
                if (nos == '') {
                    nos = "0"
                }
                var rate = $get('<%=txtRate.ClientID %>').value;
                if (rate == '') {
                    rate = "0"
                }
                var amt = (nos * rate)
                $get('<%=txtAmount.ClientID %>').value = parseFloat(amt).toFixed(2);
                
                    $('#<%=txtTaxValue.ClientID %>').val(parseFloat(amt).toFixed(2)-parseFloat(0).toFixed(2));
            }
            var firstGSTN = $('#<%=lblGstin.ClientID%>').text().substr(0, 2);

            var lblCompanystate = $('#<%=lblCompanystate.ClientID%>').text();
            var ConsigneeStateCode = $("#ddlConsigneeState").val();
            var lblCompanyGstStateCode = $('#<%=lblCompanyGstStateCode.ClientID%>').text();
            var buyerStatecode = $("#ddlBuyerState").val();

            console.log(lblCompanyGstStateCode);
            console.log(buyerStatecode);
            if ($.trim(lblCompanyGstStateCode) == $.trim(buyerStatecode)) {
                //CGST SGST calculation
                console.log('CGST SGST');
                //CalculateCGST($('#txtCGSTRate').val());

                

                var tvl = $('#<%=txtTaxValue.ClientID %>').val();
                console.log('tvl');
                console.log(tvl);
                console.log($('#txtCGSTRate').val());
                var CgstTaxAmt = ((parseFloat(tvl) / 100) * parseFloat($('#txtCGSTRate').val()));
                console.log('CGST tax');
                console.log(CgstTaxAmt);
                $get('<%=txtCGSTAmount.ClientID %>').value = parseFloat(CgstTaxAmt).toFixed(2);

                var SgstTaxAmt = ((parseFloat(tvl) / 100) * parseFloat($('#txtSGSTRate').val()));

                $get('<%=txtSGSTAmount.ClientID %>').value = parseFloat(SgstTaxAmt).toFixed(2);
                $get('<%=txtTotalValue.ClientID %>').value = Number(parseFloat(tvl).toFixed(2)) + Number(parseFloat(CgstTaxAmt).toFixed(2)) + Number(parseFloat(SgstTaxAmt).toFixed(2));

<%--                var taxamt = ((parseFloat(tvl) / 100) * parseFloat($('#txtCGSTRate').val()));
                var totamt = (parseFloat(tvl) + parseFloat(taxamt));
                console.log('CGST tax');
                console.log(taxamt);
                $get('<%=txtCGSTAmount.ClientID %>').value = parseFloat(taxamt).toFixed(2);
                    //$get('<%=txtSGSTAmount.ClientID %>').value = "0";
                $get('<%=txtTotalValue.ClientID %>').value = parseFloat(totamt).toFixed(2);

                var tvl = $get('<%=txtTaxValue.ClientID %>').value;
                var taxamt = ((parseFloat(tvl) / 100) * parseFloat($('#txtSGSTRate').val()));
                var totamt = (parseFloat(tvl) + parseFloat(taxamt));
                $get('<%=txtSGSTAmount.ClientID %>').value = parseFloat(taxamt).toFixed(2);
                $get('<%=txtTotalValue.ClientID %>').value = parseFloat(totamt).toFixed(2);--%>


            } else if (lblCompanyGstStateCode != ConsigneeStateCode) {
                //IGST 
                console.log('IGST');
                var TaxableAmt = $('#<%=txtTaxValue.ClientID %>').val();
                var TaxAmt = ((parseFloat(TaxableAmt) / 100) * parseFloat($('#txtIGSTRate').val()));
                var totamt = (parseFloat(TaxableAmt) + parseFloat(TaxAmt));
                $get('<%=txtIGSTAmount.ClientID %>').value = parseFloat(TaxAmt).toFixed(2);
                $get('<%=txtTotalValue.ClientID %>').value = parseFloat(totamt).toFixed(2);
                //CalculateIGST($('#txtIGSTRate').val());
            }


        }
        function CalculateRateDiscount(val) {
            if (val == '') {
                console.log('discount o');
                val = 0;
                console.log(val);
            }
            else {

            console.log('discount 1');
            }
            console.log(val);
            var tax = val;
            //$get('<%=txtIGSTRate.ClientID %>').value = "";
            //$get('<%=txtIGSTAmount.ClientID %>').value = "";
            //$get('<%=txtCGSTRate.ClientID %>').value = "";
            //$get('<%=txtCGSTAmount.ClientID %>').value = "";
            //$get('<%=txtSGSTRate.ClientID %>').value = "";
            //$get('<%=txtSGSTAmount.ClientID %>').value = "";
            //$get('<%=txtTotalValue.ClientID %>').value = "";
            var nos = $get('<%=txtAmount.ClientID %>').value;
            if (nos == '') {
                $get('<%=txtDiscAmt.ClientID %>').value = "";
                alert("Please Provide Rate and Quantity");
            }
            else {
                if (tax != '') {
                    var nos = $('#<%=txtNos.ClientID %>').val();
                    var rate = $('#<%=txtRate.ClientID %>').val();
                    var amt = (nos * rate)
                    var totamt = (parseFloat(amt) - parseFloat(tax));
                    $('#<%=txtTaxValue.ClientID %>').val(parseFloat(totamt).toFixed(2));

                    var lblCompanystate = $('#<%=lblCompanystate.ClientID%>').text();
                    var ConsigneeStateCode = $("#ddlConsigneeState").val();
                    var lblCompanyGstStateCode = $('#<%=lblCompanyGstStateCode.ClientID%>').text();
                    var buyerStatecode = $("#ddlBuyerState").val();

                    console.log(lblCompanyGstStateCode);
                    console.log(buyerStatecode);
                    if ($.trim(lblCompanyGstStateCode) == $.trim(buyerStatecode)) {
                        //CGST SGST calculation
                        console.log('CGST SGST');

                        var tvl = $('#<%=txtTaxValue.ClientID %>').val();
                console.log('tvl');
                console.log(tvl);
                console.log($('#txtCGSTRate').val());
                var CgstTaxAmt = ((parseFloat(tvl) / 100) * parseFloat($('#txtCGSTRate').val()));
                console.log('CGST tax');
                console.log(CgstTaxAmt);
                $get('<%=txtCGSTAmount.ClientID %>').value = parseFloat(CgstTaxAmt).toFixed(2);

                var SgstTaxAmt = ((parseFloat(tvl) / 100) * parseFloat($('#txtSGSTRate').val()));

                $get('<%=txtSGSTAmount.ClientID %>').value = parseFloat(SgstTaxAmt).toFixed(2);
                $get('<%=txtTotalValue.ClientID %>').value = Number(parseFloat(tvl).toFixed(2)) + Number(parseFloat(CgstTaxAmt).toFixed(2)) + Number(parseFloat(SgstTaxAmt).toFixed(2));

            } else if (lblCompanyGstStateCode != ConsigneeStateCode) {
                //IGST 
                console.log('IGST');
                console.log('IGST');
                var TaxableAmt = $('#<%=txtTaxValue.ClientID %>').val();
                        var TaxAmt = ((parseFloat(TaxableAmt) / 100) * parseFloat($('#txtIGSTRate').val()));
                        $get('<%=txtIGSTAmount.ClientID %>').value = parseFloat(TaxAmt).toFixed(2);
                var totamt = (parseFloat(TaxableAmt) + parseFloat(TaxAmt));
                $get('<%=txtTotalValue.ClientID %>').value = parseFloat(totamt).toFixed(2);

                    }

                    //end




                }
                else {
                    var nos = $get('<%=txtNos.ClientID %>').value;
                    var rate = $get('<%=txtRate.ClientID %>').value;
                    var amt = (nos * rate)
                    $get('<%=txtTaxValue.ClientID %>').value = parseFloat(amt).toFixed(2);

                    //CalculateAmount()
                    //$get('<%=txtTotalValue.ClientID %>').value = parseFloat($get('<%=txtTaxValue.ClientID %>').value).toFixed(2);
                }
            }
        }
        function CalculateIGST(val) {
            var igstRt = val;
            var igstAmt = $get('<%=txtIGSTAmount.ClientID %>').value;
            if (igstAmt == '') {
                igstAmt = "0"
            }
            if (igstRt != '') {
                var tvl = $get('<%=txtTotalValue.ClientID %>').value;
                if (tvl == '') {
                    //$get('<%=txtIGSTRate.ClientID %>').value = "0";
                    //$get('<%=txtCGSTRate.ClientID %>').value = "0";
                    //$get('<%=txtSGSTRate.ClientID %>').value = "0";
                    //alert("Please Provide I - GST Rate");
                }

                var taxamt = ((parseFloat(tvl) / 100) * parseFloat(igstRt));
                var totamt = (parseFloat(tvl) + parseFloat(taxamt));
                $get('<%=txtIGSTAmount.ClientID %>').value = parseFloat(taxamt).toFixed(2);
                $get('<%=txtCGSTAmount.ClientID %>').value = "0";
                $get('<%=txtSGSTAmount.ClientID %>').value = "0";
                $get('<%=txtTotalValue.ClientID %>').value = parseFloat(totamt).toFixed(2);
            }

        }
        function CalculateCGST(val) {
            var cgstRt = val;
            var igstAmt = $('#<%=txtIGSTAmount.ClientID %>').val();
            var cgstAmt = $('#<%=txtCGSTAmount.ClientID %>').val();
            console.log('IGST Amt')
            console.log(igstAmt);
            console.log(cgstAmt);
            console.log(cgstRt);
            if (cgstAmt == '') {
                cgstAmt = "0"
            }
            if (cgstRt != '') {
                var tvl = $('#<%=txtTotalValue.ClientID %>').val();
                console.log(tvl);
                if (tvl == '') {
                   // $get('<%=txtCGSTRate.ClientID %>').value = "0";
                    //alert("Please Provide C - GST Rate");
                }

                var taxamt = ((parseFloat(tvl) / 100) * parseFloat(cgstRt));
                console.log('Tax amt:' + taxamt);
                var totamt = (parseFloat(tvl) + parseFloat(taxamt));
                console.log('total amt:' + totamt);
                $get('<%=txtCGSTAmount.ClientID %>').value = parseFloat(taxamt).toFixed(2);
                $get('<%=txtSGSTAmount.ClientID %>').value = "0";
                $get('<%=txtTotalValue.ClientID %>').value = parseFloat(totamt).toFixed(2);


            }

        }
        function CalculateSGST(val) {
            var tax = val;
            var igst = $get('<%=txtIGSTAmount.ClientID %>').value;
            var cgst = $get('<%=txtCGSTAmount.ClientID %>').value;
            var sgst = $get('<%=txtSGSTRate.ClientID %>').value;
            if (sgst == '') {
                sgst = "0"
            }
            if (tax != '') {
                var tvl = $get('<%=txtTotalValue.ClientID %>').value;
                if (tvl == '') {
                    //$get('<%=txtSGSTRate.ClientID %>').value = "0";
                    //alert("Please Provide S - GST Rate");
                }

                var taxamt = ((parseFloat(tvl) / 100) * parseFloat(tax));
                var totamt = (parseFloat(tvl) + parseFloat(taxamt));
                $get('<%=txtSGSTAmount.ClientID %>').value = parseFloat(taxamt).toFixed(2);
                $get('<%=txtTotalValue.ClientID %>').value = parseFloat(totamt).toFixed(2);

            }

        }
        function calculateword() {
            var wordval = $get('<%=txtTotalAmount.ClientID %>').value;
            var words = convertNumberToWords(wordval);
            $('td.csstdword').append(words);
            $('tr #ContentPlaceHolder1_Gvdetails_spWord_0').text(words);
            $('#spamtword').text(words + ' Only');
            $get('<%=TxtAmtinWord.ClientID %>').value = words;
        }
        function convertNumberToWords(amount) {
            var words = new Array();
            words[0] = 'Zero';
            words[1] = 'One';
            words[2] = 'Two';
            words[3] = 'Three';
            words[4] = 'Four';
            words[5] = 'Five';
            words[6] = 'Six';
            words[7] = 'Seven';
            words[8] = 'Eight';
            words[9] = 'Nine';
            words[10] = 'Ten';
            words[11] = 'Eleven';
            words[12] = 'Twelve';
            words[13] = 'Thirteen';
            words[14] = 'Fourteen';
            words[15] = 'Fifteen';
            words[16] = 'Sixteen';
            words[17] = 'Seventeen';
            words[18] = 'Eighteen';
            words[19] = 'Nineteen';
            words[20] = 'Twenty';
            words[30] = 'Thirty';
            words[40] = 'Forty';
            words[50] = 'Fifty';
            words[60] = 'Sixty';
            words[70] = 'Seventy';
            words[80] = 'Eighty';
            words[90] = 'Ninety';
            amount = amount.toString();
            var atemp = amount.split(".");
            var number = atemp[0].split(",").join("");
            var n_length = number.length;
            var words_string = "";
            if (n_length <= 9) {
                var n_array = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0);
                var received_n_array = new Array();
                for (var i = 0; i < n_length; i++) {
                    received_n_array[i] = number.substr(i, 1);
                }
                for (var i = 9 - n_length, j = 0; i < 9; i++ , j++) {
                    n_array[i] = received_n_array[j];
                }
                for (var i = 0, j = 1; i < 9; i++ , j++) {
                    if (i == 0 || i == 2 || i == 4 || i == 7) {
                        if (n_array[i] == 1) {
                            n_array[j] = 10 + parseInt(n_array[j]);
                            n_array[i] = 0;
                        }
                    }
                }
                value = "";
                for (var i = 0; i < 9; i++) {
                    if (i == 0 || i == 2 || i == 4 || i == 7) {
                        value = n_array[i] * 10;
                    } else {
                        value = n_array[i];
                    }
                    if (value != 0) {
                        words_string += words[value] + " ";
                    }
                    if ((i == 1 && value != 0) || (i == 0 && value != 0 && n_array[i + 1] == 0)) {
                        words_string += "Crores ";
                    }
                    if ((i == 3 && value != 0) || (i == 2 && value != 0 && n_array[i + 1] == 0)) {
                        words_string += "Lakhs ";
                    }
                    if ((i == 5 && value != 0) || (i == 4 && value != 0 && n_array[i + 1] == 0)) {
                        words_string += "Thousand ";
                    }
                    if (i == 6 && value != 0 && (n_array[i + 1] != 0 && n_array[i + 2] != 0)) {
                        words_string += "Hundred and ";
                    } else if (i == 6 && value != 0) {
                        words_string += "Hundred ";
                    }
                }
                words_string = words_string.split("  ").join(" ");
            }
            return words_string;
        }
    </script>

    <script type="text/javascript">
        function fnAutofillPartyDetails() {
            if (document.getElementById('ChkAutofillPartyDetails').checked) {
                document.getElementById('txtShipName').value = document.getElementById('txtBillName').value;
                document.getElementById('txtShipAddress').value = document.getElementById('txtBillAddress').value;
                document.getElementById('txtShipGSTIN').value = document.getElementById('txtBillGSTIN').value;
                //document.getElementById('txtShipState').value = document.getElementById('txtBillState').value;
                $("#ddlConsigneeState").val($("#ddlBuyerState").val());
                document.getElementById('txtShipCode').value = document.getElementById('txtBillCode').value;
            } else {
                document.getElementById('txtShipName').value = '';
                document.getElementById('txtShipAddress').value = '';
                document.getElementById('txtShipGSTIN').value = '';
                //document.getElementById('txtShipState').value = '';
                $("#ddlConsigneeState").val('0');
                document.getElementById('txtShipCode').value = '';
            }
        }
    </script>
    <script>
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        prm.add_endRequest(function () {
            $("#<%=txtHSNCode.ClientID %>").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "../AutocompleteHSN.asmx/HSNcodeAutocomplete",
                data: "{ 'HSNCODe': '" + request.term + "', 'limit': '10' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                /*      success: function (data) {
                          
                          response(data.d)
                      }*/
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.split('-')[0],
                            val: item.split('-')[1],
                            cgst: item.split('-')[2],
                            sgst: item.split('-')[3],
                            igst: item.split('-')[4],
                            gstDesc: item.split('-')[5]

                        }
                    }))
                },

            });
        },
        minLength: 2,
        select: function (event, ui) {
            console.log("Selected: " + ui.item.value + " aka " + ui.item.val + "cgst:" + ui.item.cgst + "sgst:" + ui.item.sgst + "igst:" + ui.item.igst);
            $('#txtCGSTRate').val(ui.item.cgst);
            $('#txtSGSTRate').val(ui.item.sgst);
            $('#txtIGSTRate').val(ui.item.igst);
            $('#txtHsnDescription').val(ui.item.gstDesc);

        },
        open: function () {
            $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
        },
        close: function () {
            $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
        }

    });

    /*item List*/
    $("#<%=txtItemName.ClientID %>").autocomplete({
        source: function (request, response) {
            $.ajax({
                url: "../AutocompleteHSN.asmx/ItemListInvoice",
                data: "{ 'ItemName': '" + request.term + "', 'limit': '10' }",
                dataType: "json",
                type: "POST",
                contentType: "application/json; charset=utf-8",
                dataFilter: function (data) { return data; },
                success: function (data) {
                    response($.map(data.d, function (item) {
                        return {
                            label: item.split('::')[0],
                            val: item.split('::')[1]


                        }
                    }))
                },
                //                      item:"",
                //                      success: function (data) {
                //                          var result = data.d;
                //                          response($.map(data.d, function (item) {
                //                              return {
                //                                  label: item.HSNCODE,
                //                                  value: item.Age
                //                              }
                //                          }));
                //                      }


            });
        },
        minLength: 2,
        select: function (event, ui) {
            console.log("Selected: " + ui.item.value + " UOM " + ui.item.val);
            $('#<%= txtUOM.ClientID %>').val(ui.item.val);
                },
                open: function () {
                    $(this).removeClass("ui-corner-all").addClass("ui-corner-top");
                },
                close: function () {
                    $(this).removeClass("ui-corner-top").addClass("ui-corner-all");
                }

            });
        });
    </script>
</asp:Content>
