$(document).ready(function () {

    //Sidebar Accordion Menu:

    $("#main-nav li ul").hide(); // Hide all sub menus
    $("#main-nav li a.current").parent().find("ul").slideToggle("slow"); // Slide down the current menu item's sub menu

    $("#main-nav li a.nav-top-item").click( // When a top menu item is clicked...
			function () {
			    $(this).parent().siblings().find("ul").slideUp("normal"); // Slide up all sub menus except the one clicked
			    $(this).next().slideToggle("normal"); // Slide down the clicked sub menu
			    return false;
			}
		);

    $("#main-nav li a.no-submenu").click( // When a menu item with no sub menu is clicked...
			function () {
			    window.location.href = (this.href); // Just open the link instead of a sub menu
			    return false;
			}
		);

    // Sidebar Accordion Menu Hover Effect:

    $("#main-nav li .nav-top-item").hover(
			function () {
			    $(this).stop().animate({ paddingRight: "25px" }, 200);
			},
			function () {
			    $(this).stop().animate({ paddingRight: "15px" });
			}
		);

    //Minimize Content Box

    $(".content-box-header h3").css({ "cursor": "s-resize" }); // Give the h3 in Content Box Header a different cursor
    $(".content-box-header2 h3").css({ "cursor": "s-resize" }); // Give the h3 in Content Box Header a different cursor
    $(".closed-box .content-box-content").hide(); // Hide the content of the header if it has the class "closed"
    $(".closed-box .content-box-tabs").hide(); // Hide the tabs in the header if it has the class "closed"

    $(".content-box-header h3").click( // When the h3 is clicked...
			function () {
			    $(this).parent().next().toggle(); // Toggle the Content Box
			    $(this).parent().parent().toggleClass("closed-box"); // Toggle the class "closed-box" on the content box
			    $(this).parent().find(".content-box-tabs").toggle(); // Toggle the tabs
			}
		);


    // Content box tabs:

    $('.content-box .content-box-content div.tab-content').hide(); // Hide the content divs
    $('ul.content-box-tabs li a.default-tab').addClass('current'); // Add the class "current" to the default tab
    $('.content-box-content div.default-tab').show(); // Show the div with class "default-tab"

    $('.content-box ul.content-box-tabs li a').click( // When a tab is clicked...
			function () {
			    $(this).parent().siblings().find("a").removeClass('current'); // Remove "current" class from all tabs
			    $(this).addClass('current'); // Add class "current" to clicked tab
			    var currentTab = $(this).attr('href'); // Set variable "currentTab" to the value of href of clicked tab
			    $(currentTab).siblings().hide(); // Hide all content divs
			    $(currentTab).show(); // Show the content div with the id equal to the id of clicked tab
			    return false;
			}
		);
    //Close button:

    $(".close").click(
			function () {
			    $(this).parent().fadeTo(400, 0, function () { // Links with the class "close" will close parent
			        $(this).slideUp(400);
			    });
			    return false;
			}
		);

    // Alternating table rows:

    $('tbody tr:even').addClass("alt-row"); // Add class "alt-row" to even table rows

    // Check all checkboxes when the one in a table head is checked:

    $('.check-all').click(
		function () {
			$(this).parent().parent().parent().parent().find("input[type='checkbox']").attr('checked', $(this).is(':checked'));
		}
	);

    $(".check").each(function () {
        $(this).click(function () {
            // alert($(".check :checked").length + "\n" + $(".check").length);
            if ($(".check :checked").length == $(".check").length) {

                $(".check-all").attr("checked", "checked");
            }
            else {
                $("#check-all").removeAttr("checked");
            }
        });
    });
 


    // Initialise Facebox Modal window:

    //$('a[rel*=modal]').facebox(); 

    // Initialise jQuery WYSIWYG:

    $(".wysiwyg").wysiwyg(); // Applies WYSIWYG editor to any textarea with the class "wysiwyg"
    $(".label_color_pro").click(function () {
        $(".label_color_pro").css("background-color", "");
        $(this).css("background-color", "#BCBCBC");
    });
    $(".label_color_stand").click(function () {
        $(".label_color_stand").css("background-color", "");
        $(this).css("background-color", "#BCBCBC");
    });
    $(".label_color_type").click(function () {
        $(".label_color_type").css("background-color", "");
        $(this).css("background-color", "#BCBCBC");
    });
    $("#Adserch_bt").click(function () {
        $("#Adserch").slideToggle("slow");

    });


    $("#J_TSearchTabs li").click(function () {
        $("#J_TSearchTabs li").css("background", "");
        $("#J_TSearchTabs li").children("a").css("color", "#000");
        $("#J_TSearchTabs li").children("a").css("font-weight", "normal");
        $(this).css("background", "#239CDE");
        $(this).children("a").css("color", "#FFF");
        $(this).children("a").css("font-weight", "bold");

        $("#hide1").val($(this).attr("id"));

    });

    $("#serchbutton").click(function () {

        if ($("#hide1").attr("value") == "order") {
        var coni=$("#condition").attr("value");
         var url;
            if(coni.indexOf("/")>0 || coni.indexOf("-")>0 || coni.indexOf(".")>0)
            {
               url="/WebPage/order/MachineSearch.aspx?l_2_1&&machineid=" + 
               coni.replace("-","/");
               
            }else{
            url= "/Home.aspx?l_2_1&searchType=" + $("#hide1").attr("value") + "&flag=order&search=" + $("#condition").attr("value");
            }
           //"~/WebPage/order/MachineSearch.aspx?l_2_1&&machineid=" + pro
            window.location.href = url
        } else if (($("#hide1").attr("value") == "product")) {
            var url = "/WebPage/Search/searchpro.aspx?l_3_1&flag=product&pronum=" + $("#condition").attr("value");
            window.location.href = url;
        } else if (($("#hide1").attr("value") == "stock")) {
            var url = "/WebPage/Stock/Stock.aspx?l_5_1&flag=stock&search=" + $("#condition").attr("value");
            window.location.href = url;
        } else if (($("#hide1").attr("value") == "TF")) {
            var url = "/WebPage/Search/SearchTF.aspx?l_2_2&flag=TF&TF=" + $("#condition").attr("value");
            window.location.href = url;
        } else if (($("#hide1").attr("value") == "3d")) {
            var url = "/WebPage/PartsOnline/ORDERPRODUCT.aspx?l_2_4&flag=3d&three=" + $("#condition").attr("value");
            window.location.href = url;
        } else if (($("#hide1").attr("value") == "ligift")) {
            var url = "/WebPage/Gifts/GiftsList.aspx?l_2_3&flag=ligift&giftname=" + $("#condition").attr("value");
            window.location.href = url;
        }
    });




    $("#n1 td").css("border", "thin solid #000000");
    $("#n2 td").css("border", "thin solid #000000");
    $("#n3 td").css("border", "thin solid #000000");
    $("#n4 tr td").css("background", "#FFFFFF");
    $("#divdetail").css("display", "none");
    $("#divmore").click(function () {
        $("#divdetail").slideToggle("slow");
    });

    $("#div_car").click(function () { $("#a_tab2").click() });


});



function openform() {
    window.open('ApplyForm.aspx', 'Form', 'height=500, width=400, top=0,left=0, toolbar=no, menubar=no, scrollbars=no, resizable=no,location=no, status=no');
}
