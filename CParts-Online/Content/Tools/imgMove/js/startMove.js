
$(function () {
    $(".tabList img:first").css({"filter":"alpha(opacity:100","opacity":"1","-moz-opacity":"1"});
    $(".tabList img").click(function () {
        $("#imgViewM").attr("src", $(this).attr("src"));
    });
    var tabcount = $(".tabList li").length;
    //var imgWidth = 70+4;
    var imgWidth = 100 + 4;
    var init = tabcount * imgWidth + 20;

    $(".tabList").css("width", init);

    $("#nextimg").click(function () {
        //-370
        var ml = $(".tabList").css("margin-left");
        ml = parseInt(ml.replace("px", ""));
        if (ml > -(init - imgWidth*5)) {
            $(".tabList").css("margin-left", (ml - imgWidth) + "px");
        }
    })

    $("#perimg").click(function () {
        var ml = $(".tabList").css("margin-left");
        ml = parseInt(ml.replace("px", ""));
        if (ml < 0) {
            $(".tabList").css("margin-left", (ml + imgWidth) + "px");
        }
    })

    //Ôö¼õÊýÁ¿
    $("#maxNum").click(function () {
        var num = parseInt($("#txtNum").val());
        if (num < 99) {
            $("#txtNum").val(num + 1);
        }
    })
    
    $("#minNum").click(function () {
        var num = parseInt($("#txtNum").val());
        if (num >1) {
            $("#txtNum").val(num - 1);
        }
    })
    
    
})