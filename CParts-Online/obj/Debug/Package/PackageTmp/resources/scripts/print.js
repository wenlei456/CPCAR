
// JScript 文件


//打印<!--startprint-->  与 <!--endprint--> 之间的内容

    function preview() 
    { 
        bdhtml=window.document.body.innerHTML; 
        sprnstr="<!--startprint-->"; 
        eprnstr="<!--endprint-->"; 
        prnhtml=bdhtml.substr(bdhtml.indexOf(sprnstr)+17); 
        prnhtml=prnhtml.substring(0,prnhtml.indexOf(eprnstr)); 
        window.document.body.innerHTML=prnhtml; 
        window.print(); 
    }
    
    

