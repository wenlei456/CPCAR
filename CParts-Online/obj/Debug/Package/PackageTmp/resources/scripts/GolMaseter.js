function chageType(obj, boj2) 
{
    var accorion = document.getElementsByName("accorion")
    var check = document.getElementById(obj);
    if (accorion.length > 0) 
    {
        for (var i = 0; i < accorion.length; i++) 
        {
            accorion.item(i).className = "nav-top-item";
        }
        check.className = "nav-top-item current";
    }
    if (boj2 != "") 
    {
        var three = document.getElementsByName("accorion_");
        var checkthree = document.getElementById(boj2);
        if (three.length > 0) 
        {
            for (var ii = 0; ii < three.length; ii++) 
            {
                three.item(ii).className = "";
            }
            checkthree.className = "current";
        }
    }

}
//模态窗口
function showdiv(mess) {
    $("#show").html("<h3>"+mess+"</h3>");
    document.getElementById("bg").style.display = "block";
    document.getElementById("show").style.display = "block";
}
function hidediv() {
    document.getElementById("bg").style.display = 'none';
    document.getElementById("show").style.display = 'none';
}
