<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Account.master" AutoEventWireup="false"
    CodeFile="AspxTransactionPurchase.aspx.vb" Inherits="AspxPages_AspxTransactionPurchase" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../Resource/js/Chosen/chosen.css" rel="stylesheet" type="text/css" />
      <script src="../Resource/js/jquery.panzoom.js" type="text/javascript"></script>
    <script src="../Resource/js/jquery.mousewheel.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            //            $('input:text.second').focus();
            var $inp = $('.form-control');
            $inp.bind('keydown', function (e) {
                //var key = (e.keyCode ? e.keyCode : e.charCode);
                var key = e.which;
                if (key == 13) {
                    e.preventDefault();
                    var nxtIdx = $inp.index(this) + 1;
                    $(".form-control:eq(" + nxtIdx + ")").focus();
                }
            });
        });
    </script>
    <script>
        $(function () {
            $("#TxtPurchaseDate").datepicker({

                dateFormat: 'dd-M-yy',
                changeMonth: true,
                changeYear:true,
            }).val();
        });
    </script>
   <%-- <script src="../Resource/js/image_zoom/jquery.zoom.js" type="text/javascript"></script>--%>
    <script>
        $(document).ready(function () {
//            $('#ex1').zoom();
//            $('#ex2').zoom({ on: 'grab' });
//            $('#ex3').zoom({ on: 'click' });
            //            $('#ex4').zoom({ on: 'toggle' });
            $(".panzoom").draggable({
                handle: "img"
            });
            $('#remark').hide();
            $('#<%=ddlStatus.ClientID %>').change(function () {
                var val = document.getElementById('<%= ddlStatus.ClientID %>').value;
                if (val == 1) {
                    $get('<%=Hdnstatus.ClientID %>').value = "";
                    $('#remark').hide();
                }
                else {
                    $get('<%=Hdnstatus.ClientID %>').value = "Review";
                    $('#remark').show();
                }

            });
        });
    </script>
    <style>
        .heading15
        {
            float: left;
            width: 98%;
            height: auto;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 15px;
            color: #000000;
            text-transform: uppercase;
            padding: 0px 0 10px 0;
        }
    </style>
    <form id="frmAssignimage" runat="server" role="form">
    <script type="text/javascript">
        var prm = Sys.WebForms.PageRequestManager.getInstance();
        prm.add_endRequest(function () {
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

        });
    </script>
    <asp:HiddenField ID="HdnMSTaxStateNo" runat="server" />
    <asp:ScriptManager ID="SMLocationMaster" runat="server" EnablePageMethods="true" ScriptMode="Release">
    </asp:ScriptManager>
    <asp:HiddenField ID="Hdnstatus" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="HdnIDImage" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="HdnIdentity" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnIDPurchaseMaster" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnIDPurchaseMasteredit" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdnClientid" runat="server" ClientIDMode="Static" />
    <div class="heading24">
        <strong>Transaction Purchase for     </strong><label id="lblCompanyName" runat="server"></label></div>
    <div class="heading15" id="Review_List" runat="server" visible="false">
        <strong>Status Review:</strong><label class="form-control" runat="server" id="lblReview"></label></div>
    <div class="company_box">
        <div id="resultsel1" style="display: block" class="info_image">
            <span class='zoom' id='ex1'>
                <img id="imgprvw" runat="server" src='../Upload/Transaction/UploadTransaction.png'
                    alt='Daisy on the Ohoopee' />
                <p>
                    Hover for zoom.</p>
            </span>
             <asp:Literal ID="ltEmbed" runat="server" />
            <section id="pan-when-zoomed">
      <%--<div class="parent">
        <div class="panzoom">
          <img id="imgprvw" runat="server" src="../Upload/Transaction/UploadTransaction.png">
        </div>
      </div>--%>
      <div class="buttons">
        <button class="zoom-in">Zoom In</button>
        <button class="zoom-out">Zoom Out</button>
       <%-- <input type="range" class="zoom-range">--%>
        <button class="reset">Reset</button>
      </div>
      <script>
          (function () {
              var $section = $('#pan-when-zoomed');
              $section.find('.panzoom').panzoom({
                  $zoomIn: $section.find(".zoom-in"),
                  $zoomOut: $section.find(".zoom-out"),
                  $zoomRange: $section.find(".zoom-range"),
                  $reset: $section.find(".reset"),
                  panOnlyWhenZoomed: true,
                  minScale: 1
              });

          })();
         
      </script>
	  <script>
	      (function () {
	          var $section = $('#pan-when-zoomed');
	          var $panzoom = $section.find('.panzoom').panzoom();
	          $panzoom.parent().on('mousewheel.focal', function (e) {
	              e.preventDefault();
	              var delta = e.delta || e.originalEvent.wheelDelta;
	              var zoomOut = delta ? delta < 0 : e.originalEvent.deltaY > 0;
	              $panzoom.panzoom('zoom', zoomOut, {
	                  animate: false,
	                  focal: e
	              });
	          });
	      })();
      </script>
    </section>
        </div>
        <asp:Panel ID="pnlPerson" runat="server">
            <div id="resultselright" runat="server" style="display: block" class="info_box">
                <div class="heading16" style="margin: 0 0 20px 0; padding: 8px 0; text-align: center; width: 98%;">
                <div class="company_list company_list2">
                    <label id="Label5" runat="server">Request New Image if you cant read it, Click here</label>
                    <asp:Button ID="BtnRequestNewImage" runat="server" Text="Request New Image" CssClass="submit_btn" TabIndex="16" />
                </div>

                    
                </div>
                <div class="heading16" style="background-color: #00bcd5; color: #fff; margin: 0 0 20px 0;
                    padding: 8px 0; text-align: center; width: 98%;">
                    <strong>Purchase:</strong></div>
                <div class="company_list company_list2">
                    <label id="Label3" runat="server">
                        Name of Party</label>
                    <%--<input type="text" class="form-control" runat="server" id="TxtNameofParty" placeholder=" Name of Party"
                        clientidmode="Static" tabindex="1" maxlength="100" onkeypress="return IsAlphaNumeric(event);" />--%>
                        <asp:DropDownList ID="TxtNameofParty" class="chosen-select" runat="server" TabIndex="10" AutoPostBack="true" />
                </div>
                 <div class="company_list company_list2">
                    <label id="Label7" runat="server">
                        Tin No</label>
                    <asp:TextBox ID="TxtTinNo" class="form-control" runat="server" placeholder=" Tin No"
                        onblur="showtextbox();" ClientIDMode="Static" TabIndex="2" MaxLength="100" onkeypress="return IsAlphaNumeric(event);"></asp:TextBox>
                </div>
                <div class="company_list company_list2 date_box">
                    <label id="Label1" runat="server">
                        Purchase Date</label>
                    <asp:TextBox ID="TxtPurchaseDate" runat="server" ClientIDMode="Static" class="form-control"
                        placeholder="Receipt Date" TabIndex="3" MaxLength="11" />
                </div>
                <div class="company_list company_list2">
                    <label id="Label2" runat="server">
                        Bill No</label>
                    <input type="text" class="form-control" runat="server" id="TxtTransactionId" placeholder=" Bill No"
                        clientidmode="Static" tabindex="4" maxlength="100"  /> <%--onkeypress="return IsNumeric(event);"--%>
                </div>
                <div class="company_list">
                    <label id="Label31" runat="server">
                        Party Address</label>
                    <input type="text" class="form-control" runat="server" id="TxtPartyAddress" placeholder=" Party Address"
                        clientidmode="Static" tabindex="5" maxlength="100" onkeypress="return IsAlphaNumeric(event);" />
                </div>
                <div class="company_list company_list2">
                    <label id="Label4" runat="server">
                        City</label>
                    <input type="text" class="form-control" runat="server" id="TxtCity" placeholder=" City"
                        clientidmode="Static" tabindex="6" maxlength="100" onkeypress="return IsAlphaNumeric(event);" />
                </div>
                <div class="company_list company_list2">
                    <label id="Label6" runat="server">
                        Excise No/ServiceTax No</label>
                    <input type="text" class="form-control" runat="server" id="TxtExcNoSrvceTaxNo" placeholder=" Excise No/ Service Tax No"
                        clientidmode="Static" tabindex="7" maxlength="100" onkeypress="return IsAlphaNumeric(event);" />
                </div>
               
                <asp:UpdatePanel ID="updtPnlPopUp1" runat="server">
                    <ContentTemplate>
                        <fieldset>
                            <div class="company_list">
                                <label id="Label8" runat="server">
                                    Item Name</label>
                                <input type="text" class="form-control" runat="server" id="txtItemName" placeholder=" Item Name"
                                    clientidmode="Static" tabindex="9" maxlength="100" onkeypress="return IsAlphaNumeric(event);" />
                            </div>
                            <div class="company_list company_list3">
                                <label id="Label9" runat="server">
                                    Measure</label>
                                <asp:DropDownList ID="ddlMeasure" class="chosen-select" runat="server" TabIndex="10" />
                            </div>
                            <div class="company_list company_list3">
                                <label id="Label10" runat="server">
                                    Nos.</label>
                                <input type="text" class="form-control" runat="server" id="txtNos" placeholder=" Nos."
                                    clientidmode="Static" tabindex="11" maxlength="100" onkeypress="return IsNumericforDecimal(event);" />
                            </div>
                            <div class="company_list company_list3">
                                <label id="Label11" runat="server">
                                    Rate</label>
                                <input type="text" class="form-control" runat="server" id="txtRate" placeholder=" Rate"
                                    clientidmode="Static" tabindex="12" maxlength="100" onkeypress="return IsNumericforDecimal(event);" />
                            </div>
                            <div class="company_list company_list3">
                            </div>
                            <div class="company_list company_list3">
                            </div>
                            <div class="company_list company_list3">
                                <label id="Label12" runat="server">
                                    Duty Tax %
                                </label>
                                <input type="text" class="form-control" runat="server" id="txtDutyTax" placeholder=" Duty Tax %"
                                    clientidmode="Static" tabindex="13" maxlength="100" onblur="CalculateRateDiscount(this.value);"
                                    onkeypress="return IsNumericforDecimal(event);" />
                            </div>
                            <div class="company_list company_list3">
                                <label id="Label29" runat="server">
                                    Duty Tax Value
                                </label>
                                <input type="text" disabled="disabled" class="form-control" runat="server" id="txtDutyTaxValue"
                                    placeholder=" Duty Tax Value" clientidmode="Static" tabindex="14" maxlength="100"
                                    onkeypress="return IsAlphaNumeric(event);" />
                            </div>
                            <div class="company_list company_list3">
                                <label id="Label13" runat="server">
                                    Assessable Value</label>
                                <input type="text" disabled="disabled" class="form-control" runat="server" id="txtAssessableValue"
                                    placeholder=" Assessable Value" clientidmode="Static" tabindex="15" maxlength="100"
                                    onkeypress="return IsAlphaNumeric(event);" />
                            </div>
                            <div class="company_list">
                                <asp:Button ID="btnAddItem" runat="server" Text="Add Item" CssClass="submit_btn"
                                    OnClientClick="return validate()" TabIndex="16" />
                                <asp:Button ID="btnResetItem" runat="server" Text="Reset" CssClass="submit_btn" OnClientClick="return resetFields()" />
                            </div>
                            <br />
                            <br />
                        </fieldset>
                        <div class="heading16" style="border: thin">
                            <asp:GridView ID="Gvdetails" CssClass="total_list" HeaderStyle-ForeColor="Black"
                                ShowHeader="True" runat="server" AutoGenerateColumns="false">
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
                                    <asp:BoundField DataField="Nos" HeaderText="Nos">
                                        <ItemStyle VerticalAlign="Middle" Width="100px" HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Rate" HeaderText="Rate">
                                        <ItemStyle VerticalAlign="Middle" Width="60px" HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DutyTax" HeaderText="DutyTax">
                                        <ItemStyle VerticalAlign="Middle" Width="60px" HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="DutyTaxValue" HeaderText="DutyTaxValue">
                                        <ItemStyle VerticalAlign="Middle" Width="100px" HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="AssessableValue" HeaderText="AssessableValue">
                                        <ItemStyle VerticalAlign="Middle" Width="60px" HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Delete?">
                                        <ItemTemplate>
                                            <span onclick="return confirm('Are you sure to Delete the record?')">
                                                <asp:LinkButton ID="lnkB" runat="Server" Text="Delete" CommandName="Delete"></asp:LinkButton>
                                            </span>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="total_box" style="float: left; width: 100%; height: auto; margin: 20px 0px;">
                            <div class="total_list">
                                <div class="total_label">
                                    Total
                                </div>
                                <asp:TextBox ID="txtTotalValue" class="form-control" runat="server" ClientIDMode="Static"
                                    Enabled="false" MaxLength="100"></asp:TextBox>
                            </div>
                            <div class="total_list">
                                <div class="total_label">
                                    Discount %</div>
                                <asp:TextBox ID="txtDiscount" class="form-control" runat="server" onblur="CalculationSum();"
                                    onkeypress="return IsNumericforDecimal(event);" ClientIDMode="Static" TabIndex="17"
                                    MaxLength="5"></asp:TextBox>
                                <input type="text" id="txtDiscountValue" class="form-control" runat="server" readonly="readonly"
                                    clientidmode="Static" maxlength="100" style="text-align: right;" />
                                <input type="text" id="txtTotDiscountValue" class="form-control" runat="server" readonly="readonly"
                                    clientidmode="Static" maxlength="100" style="text-align: right;" />
                            </div>
                            <div class="total_list">
                                <div class="total_label">
                                    Service Tax %</div>
                                <asp:TextBox ID="txtServicetax" class="form-control" runat="server" onblur="CalculationSum();"
                                    onkeypress="return IsNumericforDecimal(event);" ClientIDMode="Static" TabIndex="18"
                                    MaxLength="5"></asp:TextBox>
                                <input type="text" class="form-control" runat="server" id="txtServicetaxValue" clientidmode="Static"
                                    readonly="readonly" maxlength="100" style="text-align: right;" />
                                <input type="text" class="form-control" runat="server" id="txtTotServicetaxValue"
                                    readonly="readonly" clientidmode="Static" maxlength="100" style="text-align: right;" />
                            </div>
                            <div class="total_list" style="display:none">
                                <div class="total_label">
                                    SBC %</div>
                                <asp:TextBox ID="txtSbc" class="form-control" runat="server" onblur="CalculationSum();" Text="0"
                                    onkeypress="return IsNumericforDecimal(event);" ClientIDMode="Static" TabIndex="19"
                                    MaxLength="5"></asp:TextBox>
                                <input type="text" class="form-control" runat="server" id="txtSbcValue" clientidmode="Static"
                                    readonly="readonly" maxlength="100" style="text-align: right;" />
                                <input type="text" class="form-control" runat="server" id="txtTotSbcValue" clientidmode="Static"
                                    readonly="readonly" maxlength="100" style="text-align: right;" />
                            </div>
                            <div class="total_list" style="display:none">
                                <div class="total_label">
                                    KKc %</div>
                                <asp:TextBox ID="txtKkc" class="form-control" runat="server" onblur="CalculationSum();" Text="0"
                                    onkeypress="return IsNumericforDecimal(event);" ClientIDMode="Static" TabIndex="20"
                                    MaxLength="5"></asp:TextBox>
                                <input type="text" class="form-control" runat="server" id="txtKkcValue" clientidmode="Static"
                                    readonly="readonly" maxlength="100" style="text-align: right;" />
                                <input type="text" class="form-control" runat="server" id="txtTotKkcValue" clientidmode="Static"
                                    readonly="readonly" maxlength="100" style="text-align: right;" />
                            </div>
                            <div class="total_list" style="display:none">
                                <div class="total_label">
                                    Excise Duty %</div>
                                <asp:TextBox ID="txtexciseduty" class="form-control" runat="server" onblur="CalculationSum();" Text="0"
                                    onkeypress="return IsNumericforDecimal(event);" ClientIDMode="Static" TabIndex="21"
                                    MaxLength="5"></asp:TextBox>
                                <input type="text" class="form-control" runat="server" id="txtexcisedutyValue" clientidmode="Static"
                                    readonly="readonly" maxlength="100" style="text-align: right;" />
                                <input type="text" class="form-control" runat="server" id="txtTotexcisedutyValue"
                                    readonly="readonly" clientidmode="Static" maxlength="100" style="text-align: right;" />
                            </div>
                            <div class="total_list" id="lblMspurchase">
                                <div class="total_label">
                                    MS Purchase %</div>
                                <asp:TextBox ID="txtMspurchase" class="form-control" runat="server" onblur="CalculationSum();"
                                    onkeypress="return IsNumericforDecimal(event);" ClientIDMode="Static" TabIndex="22"
                                    MaxLength="5"></asp:TextBox>
                                <input type="text" class="form-control" runat="server" id="txtMspurchaseValue" clientidmode="Static"
                                    readonly="readonly" maxlength="100" style="text-align: right;" />
                                <input type="text" class="form-control" runat="server" id="txtTotMspurchaseValue"
                                    readonly="readonly" clientidmode="Static" maxlength="100" style="text-align: right;" />
                            </div>
                            <div class="total_list" id="lblOmspurchasecform">
                                <div class="total_label">
                                    OMS Purchase with 'C' form %</div>
                                <asp:TextBox ID="txtOmspurchasecform" class="form-control" runat="server" onblur="CalculationSum();"
                                    onkeypress="return IsNumericforDecimal(event);" ClientIDMode="Static" TabIndex="23"
                                    MaxLength="5"></asp:TextBox>
                                <input type="text" class="form-control" runat="server" id="txtOmspurchasecformValue"
                                    readonly="readonly" clientidmode="Static" maxlength="100" style="text-align: right;" />
                                <input type="text" class="form-control" runat="server" id="txtTotOmspurchasecformValue"
                                    readonly="readonly" clientidmode="Static" maxlength="100" style="text-align: right;" />
                            </div>
                            <div class="total_list" id="lblOmspurchasefform">
                                <div class="total_label">
                                    OMS Purchase with 'F' form %</div>
                                <asp:TextBox ID="txtOmspurchasefform" class="form-control" runat="server" onblur="CalculationSum();"
                                    onkeypress="return IsNumericforDecimal(event);" ClientIDMode="Static" TabIndex="24"
                                    MaxLength="5"></asp:TextBox>
                                <input type="text" class="form-control" runat="server" id="txtOmspurchasefformValue"
                                    readonly="readonly" clientidmode="Static" maxlength="100" style="text-align: right;" />
                                <input type="text" class="form-control" runat="server" id="txtTotOmspurchasefformValue"
                                    readonly="readonly" clientidmode="Static" maxlength="100" style="text-align: right;" />
                            </div>
                            <div class="total_list" id="lblOmspurchasewocform">
                                <div class="total_label">
                                    OMS Purchase W/O 'C' form %</div>
                                <asp:TextBox ID="txtOmspurchasewocform" class="form-control" runat="server" onblur="CalculationSum();"
                                    onkeypress="return IsNumericforDecimal(event);" ClientIDMode="Static" TabIndex="25"
                                    MaxLength="5"></asp:TextBox>
                                <input type="text" class="form-control" runat="server" id="txtOmspurchasewocformValue"
                                    readonly="readonly" clientidmode="Static" maxlength="100" style="text-align: right;" />
                                <input type="text" class="form-control" runat="server" id="txtTotOmspurchasewocformValue"
                                    readonly="readonly" clientidmode="Static" maxlength="100" style="text-align: right;" />
                            </div>
                            <div class="total_list"  id="lblvatpurchase">
                                <div class="total_label">
                                    VAT Purchase %</div>
                                <asp:TextBox ID="txtVatpurchase" class="form-control" runat="server" onblur="CalculationSum();"
                                    onkeypress="return IsNumericforDecimal(event);" ClientIDMode="Static" TabIndex="26"
                                    MaxLength="5"></asp:TextBox>
                                <input type="text" class="form-control" runat="server" id="txtVatpurchaseValue" readonly="readonly"
                                    clientidmode="Static" maxlength="100" style="text-align: right;" />
                                <input type="text" class="form-control" runat="server" id="txtTotVatpurchaseValue"
                                    readonly="readonly" clientidmode="Static" maxlength="100" style="text-align: right;" />
                            </div>
                            <div class="total_list">
                                <div class="total_label">
                                    Works Contract Tax %</div>
                                <asp:TextBox ID="txtWorkscontracttax" class="form-control" runat="server" onblur="CalculationSum();"
                                    onkeypress="return IsNumericforDecimal(event);" ClientIDMode="Static" TabIndex="27"
                                    MaxLength="5"></asp:TextBox>
                                <input type="text" class="form-control" runat="server" id="txtWorkscontracttaxValue"
                                    readonly="readonly" clientidmode="Static" maxlength="100" style="text-align: right;" />
                                <input type="text" class="form-control" runat="server" id="txtTotWorkscontracttaxValue"
                                    readonly="readonly" clientidmode="Static" maxlength="100" style="text-align: right;" />
                            </div>
                            <div class="total_list">
                                <div class="total_label">
                                    Input VAT On CST %</div>
                                <asp:TextBox ID="txtInputvatoncst" class="form-control" runat="server" onblur="CalculationSum();"
                                    onkeypress="return IsNumericforDecimal(event);" ClientIDMode="Static" TabIndex="28"
                                    MaxLength="5"></asp:TextBox>
                                <input type="text" class="form-control" runat="server" id="txtInputvatoncstValue"
                                    readonly="readonly" clientidmode="Static" maxlength="100" style="text-align: right;" />
                                <input type="text" class="form-control" runat="server" id="txtTotInputvatoncstValue"
                                    readonly="readonly" clientidmode="Static" maxlength="100" style="text-align: right;" />
                            </div>
                            <div class="total_list">
                                <div class="total_label">
                                    Round Off ( + / - )</div>
                                <input type="text" class="form-control" runat="server" id="Text1" clientidmode="Static"
                                    maxlength="100" disabled="disabled" style="text-align: right; visibility: hidden" />
                                <asp:TextBox ID="txtRoundoff" class="form-control" runat="server" onblur="CalculationSum();"
                                    onkeypress="return IsNumericforRoundoff(event);" ClientIDMode="Static" TabIndex="29"
                                    MaxLength="5"></asp:TextBox>
                                <input type="text" class="form-control" runat="server" id="txtRoundoffValue" clientidmode="Static"
                                    maxlength="100" style="text-align: right; display: none" />
                                <input type="text" class="form-control" runat="server" id="txtTotRoundoffValue" readonly="readonly"
                                    clientidmode="Static" maxlength="100" style="text-align: right;" />
                            </div>
                            <div class="total_list">
                                <div class="total_label">
                                    Discount before Gross %</div>
                                <asp:TextBox ID="txtDiscountbeforegross" class="form-control" runat="server" onblur="CalculationSum();"
                                    onkeypress="return IsNumericforDecimal(event);" ClientIDMode="Static" TabIndex="30"
                                    MaxLength="5"></asp:TextBox>
                                <input type="text" class="form-control" runat="server" id="txtDiscountbeforegrossValue"
                                    readonly="readonly" clientidmode="Static" maxlength="100" style="text-align: right;" />
                                <input type="text" class="form-control" runat="server" id="txtTotDiscountbeforegrossValue"
                                    readonly="readonly" clientidmode="Static" maxlength="100" style="text-align: right;" />
                            </div>
                            <div class="total_list">
                                <div class="total_label">
                                    Gross Amount</div>
                                <input type="text" class="form-control" runat="server" id="txtGrossamountValue" clientidmode="Static"
                                    readonly="readonly" maxlength="100" style="text-align: right;" />
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div id="review" runat="server" visible="false" class="company_list">
                    <div class="company_list company_list3">
                        <label id="lblStatus" runat="server">
                            Status</label>
                        <asp:DropDownList ID="ddlStatus" runat="server" TabIndex="31">
                            <asp:ListItem Value="1">Approved</asp:ListItem>
                            <asp:ListItem Value="2">Reject</asp:ListItem>
                            <asp:ListItem Value="3">On-Hold</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div id="remark" class="company_list">
                        <label id="lblRemark" runat="server">
                            Remark</label>
                        <input type="text" id="txtRemark" runat="server" name="textfield" placeholder=" Remark"
                            clientidmode="Static" tabindex="32" maxlength="255" />
                    </div>
                </div>
                <div class="company_list">
                    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="submit_btn" OnClientClick="return validateOutSide()"
                        TabIndex="33" />
                    <asp:Button ID="btnreset" runat="server" Text="Reset" CssClass="submit_btn" OnClientClick="return resetFieldsOutside()" />
                </div>
            </div>
        </asp:Panel>
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
    <script type="text/javascript" language="javascript">
        var totval;
        var dec;
        var decplace;
        function CalculationSum() {
            totval = $get('<%=txtTotalValue.ClientID %>').value;
            if (totval != "") {
                decplace = parseInt('<%=StrDecimal %>');

                dec = '<%=StrDecimalPlace %>'
                totval = $get('<%=txtTotalValue.ClientID %>').value;
                var Discount = GetDiscount($get('<%=txtDiscount.ClientID %>').value, parseFloat(totval));
                var Discountvalue = parseFloat(Discount).toFixed(decplace);
                if (Discountvalue == "NaN") {
                    Discountvalue = "0";
                }
                $get('<%=txtDiscountValue.ClientID %>').value = Discountvalue;
                $get('<%=txtTotDiscountValue.ClientID %>').value = (parseFloat(totval) - parseFloat(Discountvalue)).toFixed(decplace);

                var Servicetax = GetDiscount($get('<%=txtServicetax.ClientID %>').value, $get('<%=txtTotDiscountValue.ClientID %>').value);
                var ServicetaxValue = parseFloat(Servicetax).toFixed(decplace);
                if (ServicetaxValue == "NaN") {
                    ServicetaxValue = "0";
                }
                $get('<%=txtServicetaxValue.ClientID %>').value = ServicetaxValue;
                $get('<%=txtTotServicetaxValue.ClientID %>').value = ((parseFloat(totval) - parseFloat(Discountvalue)) + parseFloat(ServicetaxValue)).toFixed(decplace);

                var Sbc = GetDiscount($get('<%=txtSbc.ClientID %>').value, $get('<%=txtTotServicetaxValue.ClientID %>').value);
                var SbcValue = parseFloat(Sbc).toFixed(decplace);
                if (SbcValue == "NaN") {
                    SbcValue = "0";
                }
                $get('<%=txtSbcValue.ClientID %>').value = SbcValue;
                $get('<%=txtTotSbcValue.ClientID %>').value = ((parseFloat(totval) - parseFloat(Discountvalue)) + parseFloat(ServicetaxValue) + parseFloat(SbcValue)).toFixed(decplace);

                var Kkc = GetDiscount($get('<%=txtKkc.ClientID %>').value, $get('<%=txtTotSbcValue.ClientID %>').value);
                var KkcValue = parseFloat(Kkc).toFixed(decplace);
                if (KkcValue == "NaN") {
                    KkcValue = "0";
                }
                $get('<%=txtKkcValue.ClientID %>').value = KkcValue;
                $get('<%=txtTotKkcValue.ClientID %>').value = ((parseFloat(totval) - parseFloat(Discountvalue)) + parseFloat(ServicetaxValue) + parseFloat(SbcValue) + parseFloat(KkcValue)).toFixed(decplace);

                var exciseduty = GetDiscount($get('<%=txtexciseduty.ClientID %>').value, $get('<%=txtTotKkcValue.ClientID %>').value);
                var excisedutyValue = parseFloat(exciseduty).toFixed(decplace);
                if (excisedutyValue == "NaN") {
                    excisedutyValue = "0";
                }
                $get('<%=txtexcisedutyValue.ClientID %>').value = excisedutyValue;
                $get('<%=txtTotexcisedutyValue.ClientID %>').value = ((parseFloat(totval) - parseFloat(Discountvalue)) + parseFloat(ServicetaxValue) + parseFloat(SbcValue) + parseFloat(KkcValue) + parseFloat(excisedutyValue)).toFixed(decplace);

                var Mspurchase = GetDiscount($get('<%=txtMspurchase.ClientID %>').value, ((parseFloat(totval) - parseFloat(Discountvalue)) + parseFloat(ServicetaxValue) + parseFloat(SbcValue) + parseFloat(KkcValue) + parseFloat(excisedutyValue)));
                var MspurchaseValue = parseFloat(Mspurchase).toFixed(decplace);
                if (MspurchaseValue == "NaN") {
                    MspurchaseValue = "0";
                }
                $get('<%=txtMspurchaseValue.ClientID %>').value = MspurchaseValue;
                $get('<%=txtTotMspurchaseValue.ClientID %>').value = ((parseFloat(totval) - parseFloat(Discountvalue)) + parseFloat(ServicetaxValue) + parseFloat(SbcValue) + parseFloat(KkcValue) + parseFloat(excisedutyValue) + parseFloat(MspurchaseValue)).toFixed(decplace);

                var Omspurchasecform = GetDiscount($get('<%=txtOmspurchasecform.ClientID %>').value, ((parseFloat(totval) - parseFloat(Discountvalue)) + parseFloat(ServicetaxValue) + parseFloat(SbcValue) + parseFloat(KkcValue) + parseFloat(excisedutyValue) + parseFloat(MspurchaseValue)));
                var OmspurchasecformValue = parseFloat(Omspurchasecform).toFixed(decplace);
                if (OmspurchasecformValue == "NaN") {
                    OmspurchasecformValue = "0";
                }
                $get('<%=txtOmspurchasecformValue.ClientID %>').value = OmspurchasecformValue;
                $get('<%=txtTotOmspurchasecformValue.ClientID %>').value = ((parseFloat(totval) - parseFloat(Discountvalue)) + parseFloat(ServicetaxValue) + parseFloat(SbcValue) + parseFloat(KkcValue) + parseFloat(excisedutyValue) + parseFloat(MspurchaseValue) + parseFloat(OmspurchasecformValue)).toFixed(decplace);

                var Omspurchasefform = GetDiscount($get('<%=txtOmspurchasefform.ClientID %>').value, $get('<%=txtTotOmspurchasecformValue.ClientID %>').value);
                var OmspurchasefformValue = parseFloat(Omspurchasefform).toFixed(decplace);
                if (OmspurchasefformValue == "NaN") {
                    OmspurchasefformValue = "0";
                }
                $get('<%=txtOmspurchasefformValue.ClientID %>').value = OmspurchasefformValue;
                $get('<%=txtTotOmspurchasefformValue.ClientID %>').value = ((parseFloat(totval) - parseFloat(Discountvalue)) + parseFloat(ServicetaxValue) + parseFloat(SbcValue) + parseFloat(KkcValue) + parseFloat(excisedutyValue) + parseFloat(MspurchaseValue) + parseFloat(OmspurchasecformValue) + parseFloat(OmspurchasefformValue)).toFixed(decplace);

                var Omspurchasewocform = GetDiscount($get('<%=txtOmspurchasewocform.ClientID %>').value, $get('<%=txtTotOmspurchasefformValue.ClientID %>').value);
                var OmspurchasewocformValue = parseFloat(Omspurchasewocform).toFixed(decplace);
                if (OmspurchasewocformValue == "NaN") {
                    OmspurchasewocformValue = "0";
                }
                $get('<%=txtOmspurchasewocformValue.ClientID %>').value = OmspurchasewocformValue;
                $get('<%=txtTotOmspurchasewocformValue.ClientID %>').value = ((parseFloat(totval) - parseFloat(Discountvalue)) + parseFloat(ServicetaxValue) + parseFloat(SbcValue) + parseFloat(KkcValue) + parseFloat(excisedutyValue) + parseFloat(MspurchaseValue) + parseFloat(OmspurchasecformValue) + parseFloat(OmspurchasefformValue) + parseFloat(OmspurchasewocformValue)).toFixed(decplace);

                var Vatpurchase = GetDiscount($get('<%=txtVatpurchase.ClientID %>').value, ((parseFloat(totval) - parseFloat(Discountvalue)) + parseFloat(ServicetaxValue) + parseFloat(SbcValue) + parseFloat(KkcValue) + parseFloat(excisedutyValue) + parseFloat(MspurchaseValue) + parseFloat(OmspurchasecformValue) + parseFloat(OmspurchasefformValue) + parseFloat(OmspurchasewocformValue)));
                var VatpurchaseValue = parseFloat(Vatpurchase).toFixed(decplace);
                if (VatpurchaseValue == "NaN") {
                    VatpurchaseValue = "0";
                }
                $get('<%=txtVatpurchaseValue.ClientID %>').value = VatpurchaseValue;
                $get('<%=txtTotVatpurchaseValue.ClientID %>').value = ((parseFloat(totval) - parseFloat(Discountvalue)) + parseFloat(ServicetaxValue) + parseFloat(SbcValue) + parseFloat(KkcValue) + parseFloat(excisedutyValue) + parseFloat(MspurchaseValue) + parseFloat(OmspurchasecformValue) + parseFloat(OmspurchasefformValue) + parseFloat(OmspurchasewocformValue) + parseFloat(VatpurchaseValue)).toFixed(decplace);

                var Workscontracttax = GetDiscount($get('<%=txtWorkscontracttax.ClientID %>').value, $get('<%=txtTotVatpurchaseValue.ClientID %>').value);
                var WorkscontracttaxValue = parseFloat(Workscontracttax).toFixed(decplace);
                if (WorkscontracttaxValue == "NaN") {
                    WorkscontracttaxValue = "0";
                }
                $get('<%=txtWorkscontracttaxValue.ClientID %>').value = WorkscontracttaxValue;
                $get('<%=txtTotWorkscontracttaxValue.ClientID %>').value = ((parseFloat(totval) - parseFloat(Discountvalue)) + parseFloat(ServicetaxValue) + parseFloat(SbcValue) + parseFloat(KkcValue) + parseFloat(excisedutyValue) + parseFloat(MspurchaseValue) + parseFloat(OmspurchasecformValue) + parseFloat(OmspurchasefformValue) + parseFloat(OmspurchasewocformValue) + parseFloat(VatpurchaseValue) + parseFloat(WorkscontracttaxValue)).toFixed(decplace);

                var Inputvatoncst = GetDiscount($get('<%=txtInputvatoncst.ClientID %>').value, $get('<%=txtTotWorkscontracttaxValue.ClientID %>').value);
                var InputvatoncstValue = parseFloat(Inputvatoncst).toFixed(decplace);
                if (InputvatoncstValue == "NaN") {
                    InputvatoncstValue = "0";
                }
                $get('<%=txtInputvatoncstValue.ClientID %>').value = InputvatoncstValue;
                $get('<%=txtTotInputvatoncstValue.ClientID %>').value = ((parseFloat(totval) - parseFloat(Discountvalue)) + parseFloat(ServicetaxValue) + parseFloat(SbcValue) + parseFloat(KkcValue) + parseFloat(excisedutyValue) + parseFloat(MspurchaseValue) + parseFloat(OmspurchasecformValue) + parseFloat(OmspurchasefformValue) + parseFloat(OmspurchasewocformValue) + parseFloat(VatpurchaseValue) + parseFloat(WorkscontracttaxValue) + parseFloat(InputvatoncstValue)).toFixed(decplace);

                var Roundoff = $get('<%=txtRoundoff.ClientID %>').value;
                if (Roundoff == "") {
                    Roundoff = "0";
                }
                var valstr = Roundoff.substring(0, 1);
                var RoundoffValue = parseFloat(Roundoff).toFixed(decplace);
                if (RoundoffValue == "NaN") {
                    RoundoffValue = "0";
                }
                $get('<%=txtRoundoffValue.ClientID %>').value = RoundoffValue;
                $get('<%=txtTotRoundoffValue.ClientID %>').value = ((parseFloat(totval) - parseFloat(Discountvalue)) + parseFloat(ServicetaxValue) + parseFloat(SbcValue) + parseFloat(KkcValue) + parseFloat(excisedutyValue) + parseFloat(MspurchaseValue) + parseFloat(OmspurchasecformValue) + parseFloat(OmspurchasefformValue) + parseFloat(OmspurchasewocformValue) + parseFloat(VatpurchaseValue) + parseFloat(WorkscontracttaxValue) + parseFloat(InputvatoncstValue) + parseFloat(RoundoffValue)).toFixed(decplace);

                var Discountbeforegross = GetDiscount($get('<%=txtDiscountbeforegross.ClientID %>').value, $get('<%=txtTotRoundoffValue.ClientID %>').value);
                var DiscountbeforegrossValue = parseFloat(Discountbeforegross).toFixed(decplace);
                if (DiscountbeforegrossValue == "NaN") {
                    DiscountbeforegrossValue = "0";
                }
                $get('<%=txtDiscountbeforegrossValue.ClientID %>').value = DiscountbeforegrossValue;
                $get('<%=txtTotDiscountbeforegrossValue.ClientID %>').value = ((parseFloat(totval) - parseFloat(Discountvalue) - parseFloat(DiscountbeforegrossValue)) + parseFloat(ServicetaxValue) + parseFloat(SbcValue) + parseFloat(KkcValue) + parseFloat(excisedutyValue) + parseFloat(MspurchaseValue) + parseFloat(OmspurchasecformValue) + parseFloat(OmspurchasefformValue) + parseFloat(OmspurchasewocformValue) + parseFloat(VatpurchaseValue) + parseFloat(WorkscontracttaxValue) + parseFloat(InputvatoncstValue) + parseFloat(RoundoffValue)).toFixed(decplace);

                $get('<%=txtGrossamountValue.ClientID %>').value = $get('<%=txtTotDiscountbeforegrossValue.ClientID %>').value;

            }
        }
        //    function checknull(value) {
        //        
        //        if (value == "0" || value == '') {
        //            value == dec
        //        } 
        //        return value;  
        //    }
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
            document.getElementById("<%=txtItemName.ClientID%>").value = "";
            document.getElementById("<%=txtNos.ClientID%>").value = "";
            document.getElementById("<%=txtRate.ClientID%>").value = "";
            document.getElementById("<%=txtDutyTax.ClientID%>").value = "";
            document.getElementById("<%=txtDutyTaxValue.ClientID%>").value = "";
            document.getElementById("<%=txtAssessableValue.ClientID%>").value = "";
        }
        function resetFieldsOutside() {
            document.getElementById("<%=TxtTransactionId.ClientID%>").value = "";
            document.getElementById("<%=TxtNameofParty.ClientID%>").value = "";
            document.getElementById("<%=TxtPartyAddress.ClientID%>").value = "";
            document.getElementById("<%=TxtCity.ClientID%>").value = "";
            document.getElementById("<%=TxtExcNoSrvceTaxNo.ClientID%>").value = "";
            document.getElementById("<%=TxtTinNo.ClientID%>").value = "";
            document.getElementById("<%=txtDiscount.ClientID%>").value = "";
            document.getElementById("<%=txtDiscountValue.ClientID%>").value = "0.00";
            document.getElementById("<%=txtServicetax.ClientID%>").value = "";
            document.getElementById("<%=txtServicetaxValue.ClientID%>").value = "0.00";
            document.getElementById("<%=txtSbc.ClientID%>").value = "";
            document.getElementById("<%=txtSbcValue.ClientID%>").value = "0.00";
            document.getElementById("<%=txtKkc.ClientID%>").value = "";
            document.getElementById("<%=txtKkcValue.ClientID%>").value = "0.00";
            document.getElementById("<%=txtexciseduty.ClientID%>").value = "";
            document.getElementById("<%=txtexcisedutyValue.ClientID%>").value = "0.00";
            document.getElementById("<%=txtMspurchase.ClientID%>").value = "";
            document.getElementById("<%=txtMspurchaseValue.ClientID%>").value = "0.00";
            document.getElementById("<%=txtOmspurchasecform.ClientID%>").value = "";
            document.getElementById("<%=txtOmspurchasecformValue.ClientID%>").value = "0.00";
            document.getElementById("<%=txtOmspurchasefform.ClientID%>").value = "";
            document.getElementById("<%=txtOmspurchasefformValue.ClientID%>").value = "0.00";
            document.getElementById("<%=txtOmspurchasewocform.ClientID%>").value = "";
            document.getElementById("<%=txtOmspurchasewocformValue.ClientID%>").value = "0.00";
            document.getElementById("<%=txtVatpurchase.ClientID%>").value = "";
            document.getElementById("<%=txtVatpurchaseValue.ClientID%>").value = "0.00";
            document.getElementById("<%=txtWorkscontracttax.ClientID%>").value = "";
            document.getElementById("<%=txtWorkscontracttaxValue.ClientID%>").value = "0.00";
            document.getElementById("<%=txtInputvatoncst.ClientID%>").value = "";
            document.getElementById("<%=txtInputvatoncstValue.ClientID%>").value = "0.00";
            document.getElementById("<%=txtRoundoffValue.ClientID%>").value = "0.00";
            document.getElementById("<%=txtTotRoundoffValue.ClientID%>").value = "0.00";
            document.getElementById("<%=txtDiscountbeforegross.ClientID%>").value = "";
            document.getElementById("<%=txtDiscountbeforegrossValue.ClientID%>").value = "0.00";
            document.getElementById("<%=txtGrossamountValue.ClientID%>").value = "0.00";
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

        }
        function validateOutSide() {
            var valstatus = $get('<%=Hdnstatus.ClientID %>').value;
            if (valstatus == "Review") {
                if (document.getElementById("<%=txtRemark.ClientID %>").value.trim() == "") {
                    alert("Please Provide Remark");
                    document.getElementById("<%=txtRemark.ClientID %>").focus();
                    return false;
                }
            }
            if (document.getElementById("<%=TxtNameofParty.ClientID %>").value.trim() == "") {
                alert("Please Provide Party Name");
                document.getElementById("<%=TxtNameofParty.ClientID %>").focus();
                return false;
            }

            if (document.getElementById("<%=TxtCity.ClientID %>").value.trim() == "") {
                alert("Please Provide City");
                document.getElementById("<%=TxtCity.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=TxtTransactionId.ClientID %>").value.trim() == "") {
                alert("Please Provide BillNo");
                document.getElementById("<%=TxtTransactionId.ClientID %>").focus();
                return false;
            }
//            if (document.getElementById("<%=TxtExcNoSrvceTaxNo.ClientID %>").value.trim() == "") {
//                alert("Please Provide ServiceTaxNo/ExcNo");
//                document.getElementById("<%=TxtExcNoSrvceTaxNo.ClientID %>").focus();
//                return false;
//            }
            if (document.getElementById("<%=TxtTinNo.ClientID %>").value.trim() == "") {
                alert("Please Provide TinNo");
                document.getElementById("<%=TxtTinNo.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtDiscount.ClientID %>").value.trim() == "") {
                alert("Please Provide Discount");
                document.getElementById("<%=txtDiscount.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtServicetax.ClientID %>").value.trim() == "") {
                alert("Please Provide Servicetax");
                document.getElementById("<%=txtServicetax.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtSbc.ClientID %>").value.trim() == "") {
                alert("Please Provide Sbc");
                document.getElementById("<%=txtSbc.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtKkc.ClientID %>").value.trim() == "") {
                alert("Please Provide Kkc");
                document.getElementById("<%=txtKkc.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtexciseduty.ClientID %>").value.trim() == "") {
                alert("Please Provide exciseduty");
                document.getElementById("<%=txtexciseduty.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtVatpurchase.ClientID %>").value.trim() == "") {
                alert("Please Provide Vatpurchase");
                document.getElementById("<%=txtVatpurchase.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtWorkscontracttax.ClientID %>").value.trim() == "") {
                alert("Please Provide Workscontracttax");
                document.getElementById("<%=txtWorkscontracttax.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtInputvatoncst.ClientID %>").value.trim() == "") {
                alert("Please Provide Inputvatoncst");
                document.getElementById("<%=txtInputvatoncst.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=txtDiscountbeforegross.ClientID %>").value.trim() == "") {
                alert("Please Provide Discountbeforegross");
                document.getElementById("<%=txtDiscountbeforegross.ClientID %>").focus();
                return false;
            }
        }
    </script>
    <script type="text/javascript">
        function CalculateRateDiscount(val) {
            var tax = val;

            if (tax != '') {
                var nos = $get('<%=txtNos.ClientID %>').value;
                var rate = $get('<%=txtRate.ClientID %>').value;
                var amt = (nos * rate)
                var taxamt = ((parseFloat(amt) / 100) * parseFloat(tax));
                var totamt = (parseFloat(amt) + parseFloat(taxamt));
                $get('<%=txtDutyTaxValue.ClientID %>').value = parseFloat(taxamt).toFixed(2);
                $get('<%=txtAssessableValue.ClientID %>').value = parseFloat(totamt).toFixed(2);
                //$('#<%=txtNos.ClientID %>').attr("disabled", "disabled");
                //$('#<%=txtRate.ClientID %>').attr("disabled", "disabled");
            }
            else {
                var nos = $get('<%=txtNos.ClientID %>').value;
                var rate = $get('<%=txtRate.ClientID %>').value;
                var amt = (nos * rate)
                $get('<%=txtAssessableValue.ClientID %>').value = parseFloat(amt).toFixed(2);
                //$('#<%=txtNos.ClientID %>').removeAttr("disabled");
                //$('#<%=txtRate.ClientID %>').removeAttr("disabled");
            }
        }
        function showtextbox() {
            var nos = $get('<%=TxtTinNo.ClientID %>').value;
            var text = nos;
            var dec = '<%=StrDecimalPlace %>'
            var str = $get('<%=HdnMSTaxStateNo.ClientID %>').value;
            var len = text.length;
            if (len > 1) {
                var res = text.substring(0, 2);
                if (str.indexOf(res) != -1) {
                    var ns = res;
                }

                else {
                    ns = null;
                }

            }
            else {
                ns = null;
            }
            if (ns == null) {
                $("#lblMspurchase").hide();
                $("#lblvatpurchase").hide();
                $("#lblOmspurchasewocform").show();
                $("#lblOmspurchasefform").show();
                $("#lblOmspurchasecform").show();
                $get('<%=txtMspurchase.ClientID %>').value = dec;
                $get('<%=txtMspurchaseValue.ClientID %>').value = dec;
                $get('<%=txtTotMspurchaseValue.ClientID %>').value = dec;

                
            }
            else {
                $("#lblMspurchase").show();
                $("#lblvatpurchase").show();
                $("#lblOmspurchasewocform").hide();
                $("#lblOmspurchasefform").hide();
                $("#lblOmspurchasecform").hide();
                $get('<%=txtOmspurchasewocform.ClientID %>').value = dec;
                $get('<%=txtOmspurchasewocformValue.ClientID %>').value = dec;
                $get('<%=txtTotOmspurchasewocformValue.ClientID %>').value = dec;
                $get('<%=txtOmspurchasefform.ClientID %>').value = dec;
                $get('<%=txtOmspurchasefformValue.ClientID %>').value = dec;
                $get('<%=txtTotOmspurchasefformValue.ClientID %>').value = dec;
                $get('<%=txtOmspurchasecform.ClientID %>').value = dec;
                $get('<%=txtOmspurchasecformValue.ClientID %>').value = dec;
                $get('<%=txtTotOmspurchasecformValue.ClientID %>').value = dec;
            }

        }
    </script>
</asp:Content>
