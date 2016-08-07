<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="day.aspx.cs" Inherits="CParts_Online.html.day" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
  <style>   
       .jp-page{position:relative} 
       .jp-text,.jp-label,.jp-image,.jp-barcode{position:absolute;overflow:hidden}   
       .jp-auto-stretch,.jp-barcode object,.jp-barcode embed{width: 100%; height: 100%;}   
       .jp-paper-background{position:absolute;width:100%;height:100%;} 
       .jp-comp-0{filter: ; opacity: 1;}  
       .jp-comp-1{left: 75px; top: 70px; width: 113px; height: 20px; font-size: 16px; position: absolute; z-index: 101;}   
       .jp-comp-2{left: 228px; top: 50px; width: 116px; height: 21px; font-size: 16px; z-index: 102;} 
       .jp-comp-3{left: 90px; top: 96px; width: 175px; height: 18px; font-size: 16px; position: absolute; z-index: 103;}  
       .jp-comp-4{left: 317px; top: 96px; width: 105px; height: 24px; font-size: 16px; z-index: 104;}   
       .jp-comp-5{left: 83px; top: 120px; width: 370px; height: 17px; font-size: 16px; position: absolute; z-index: 105;}   
       .jp-comp-6{left: 98px; top: 144px; width: 158px; height: 18px; font-size: 16px; position: absolute; z-index: 106;}  
       .jp-comp-7{left: 323px; top: 138px; width: 150px; height: 28px; font-size: 24px; position: absolute; z-index: 107;} 
       .jp-comp-8{left: 93px; top: 188px; width: 94px; height: 21px; font-size: 16px; position: absolute; z-index: 108;}  
       .jp-comp-9{left: 244px; top: 188px; width: 179px; height: 25px; font-size: 16px; position: absolute; z-index: 109;}   
       .jp-comp-10{left: 95px; top: 198px; width: 179px; height: 19px; font-size: 16px; position: absolute; z-index: 110;}   
       .jp-comp-11{left: 317px; top: 230px; width: 122px; height: 19px; position: absolute; z-index: 111;}   
       .jp-comp-12{left: 93px; top: 248px; width: 340px; height: 37px; font-size: 16px; position: absolute; z-index: 112;} 
       .jp-comp-13{left: 325px; top: 294px; width: 121px; height: 24px; font-size: 26px; position: absolute; z-index: 113;}

  </style>
    <script src="js/jquery-1.4.2.min.js"></script>
    <script src="js/jquery.PrintArea.js"></script>
  <script>
      function GetRequest() {

          var url = location.search; //获取url中"?"符后的字串
          var theRequest = new Object();
          if (url.indexOf("?") != -1) {
              var str = url.substr(1);
              strs = str.split("&");
              for (var i = 0; i < strs.length; i++) {
                  theRequest[strs[i].split("=")[0]] = (strs[i].split("=")[1]);
              }
          }
          return theRequest;
      }
      $(function () {
          var Request = new Object();
          Request = GetRequest();
          var uName =  decodeURI(Request["uName"]);
          var tell = decodeURI(Request["tell"]);
          var address = decodeURI(Request["address"]);
          $("#uName").html(uName);
          $("#tell").html(tell);
          $("#address").html(address);
          
      })

      //$(document).ready(function () {
      //    $("input#biuuu_button").click(function () {

      //        $("div#myPrintArea").printArea();

      //    });
      //});

     </script>
 </head>
<body>
  <%--<div> 
   <input type="button" value="打印预览..." onclick="doPrint('打印预览...')" /> 
   <input type="button" value="打印..." onclick="doPrint('打印...')" /> 
   <input type="button" value="打印" onclick="doPrint('打印')" /> 
   <span style="font-size:12px;color:gray;margin-left:20px;">本网页代码可以在IE,Firefox,Chrome,Safari 上打印.</span>
  </div>--%>

    <%--<input id="biuuu_button" type="button" value="打印"></input>--%>  
  
<div id="myPrintArea">   
  <div style="width: 231mm; height: 127mm;" id="page1" class="jp-page"> 
   <%--<img class="jp-paper-background screen-only" src="http://print.jatools.com/jatoolsPrinterUI/../backgroundImages/jp-8466360460405699295.jpg" />--%> 
   <div class="jp-label jp-comp-1">
   
   </div> 
   <div class="jp-label jp-comp-2">
     0730-8673882
   </div> 
   <div class="jp-label jp-comp-3">
     车管家Cp商城 
   </div> 
   <div class="jp-label jp-comp-4">
   </div> 
   <div class="jp-label jp-comp-5">
     湖南省岳阳市岳阳楼区天伦城金三角银座A座2317室
   </div> 
   <div class="jp-label jp-comp-6">
   </div> 
   <div class="jp-label jp-comp-7">
     4 1 4 0 00 
   </div> 
   <div class="jp-label jp-comp-8" id="uName">
   </div> 
   <div class="jp-label jp-comp-9" id="tell">
   </div> 
   <div class="jp-label jp-comp-10">
   </div> 
   <div class="jp-label jp-comp-11">
   </div> 
   <div class="jp-label jp-comp-12" id="address">
   </div> 
   <div class="jp-label jp-comp-13">
   </div>
  </div>
    </div>
  <object id="ojatoolsPrinter" codebase="jatoolsPrinter.cab#version=5,4,0,0" classid="clsid:B43D3361-D075-4BE2-87FE-057188254255" width="0" height="0"> <embed id="ejatoolsPrinter" type="application/x-vnd.jatoolsPrinter" width="0" height="0" /></object>
</body>
</html>
