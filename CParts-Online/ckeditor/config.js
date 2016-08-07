/*
Copyright (c) 2003-2011, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function( config ) {
	config.extraPlugins += (config.extraPlugins ? ',lineheight' : 'lineheight');
/****
	
    config.uiColor = '#F7F8F9'
    config.scayt_autoStartup = false
    config.language = 'zh-cn';
    config.width = 600;                   //初始化时的宽度
    config.skin = 'kama';                    //使用的皮肤
    config.tabSpaces = 4;
    config.resize_maxWidth = 600;             //如果设置了编辑器可以拖拽 这是可以移动的最大宽度
    config.toolbarCanCollapse = false;              //工具栏可以隐藏
    //config.toolbarLocation = 'bottom';              //可选：设置工具栏在整个编辑器的底部bottom
    config.resize_minWidth = 600;                   //如果设置了编辑器可以拖拽 这是可以移动的最小宽度
    config.resize_enabled = false;                  //如果设置了编辑器可以拖拽
    config.toolbar = [['Bold', 'Italic', 'Underline', 'Strike', 'FontSize', 'NumberedList', 'BulletedList', 'Outdent', 'Indent', 'JustifyLeft',
'JustifyCenter', 'JustifyRight', 'Link', 'Unlink', 'TextColor', 'BGColor', 'Image', 'Flash', 'Smiley', 'Table',
'RemoveFormat', 'Source']];
    config.filebrowserBrowseUrl = '/resource/ckfinder/ckfinder.html';
    config.filebrowserImageBrowseUrl = '/resource/ckfinder/ckfinder.html?Type=Images';
    config.filebrowserFlashBrowseUrl = '/resource/ckfinder/ckfinder.html?Type=Flash';
    config.filebrowserUploadUrl = '/resource/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files';
    config.filebrowserImageUploadUrl = '/resource/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images';
    config.filebrowserFlashUploadUrl = '/resource/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash';
    config.font_names = '宋体;楷体_GB2312;新宋体;黑体;隶书;幼圆;微软雅黑;Arial;Comic Sans MS;Courier New;Tahoma;Times New Roman;Verdana';
    ***/
    config.language = 'zh-cn'; //设置中文语言  

    

    config.font_names = '宋体;楷体_GB2312;新宋体;黑体;隶书;幼圆;微软雅黑;Arial;Comic Sans MS;Courier New;Tahoma;Times New Roman;Verdana';

    config.toolbar_Full = [['Source', '-', 'Preview', '-', 'Templates'], ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-', 'Print',

    'SpellChecker', 'Scayt'], ['Undo', 'Redo', '-', 'Find', 'Replace', '-', 'SelectAll', 'RemoveFormat'], ['Form', 'Checkbox', 'Radio', 'TextField',

    'Textarea', 'Select', 'Button', 'ImageButton', 'HiddenField'], '/', ['Bold', 'Italic', 'Underline', 'Strike', '-', 'Subscript', 'Superscript'],

    ['NumberedList', 'BulletedList', '-', 'Outdent', 'Indent', 'Blockquote', 'CreateDiv'], ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],

    ['Link', 'Unlink', 'Anchor'], ['Image', 'Flash', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar', 'PageBreak'], '/', ['Styles', 'Format', 'Font', 'FontSize', 'lineheight'],

    ['TextColor', 'BGColor'], ['Maximize', 'ShowBlocks', '-', 'About']];

    config.toolbar_Basic = [['Bold', 'Italic', '-', 'NumberedList', 'BulletedList', '-', 'Link', 'Unlink', '-', 'About']];

    config.width = 700;

    config.height = 300;
    //config.filebrowserBrowseUrl = '../../ckfinder/ckfinder.html';
   // config.filebrowserImageBrowseUrl = '../../ckfinder/ckfinder.html?Type=Images';
   // config.filebrowserFlashBrowseUrl = '../../ckfinder/ckfinder.html?Type=Flash';
  //  config.filebrowserUploadUrl = '../../ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files';
   // config.filebrowserImageUploadUrl = '../../ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images';
  //  config.filebrowserFlashUploadUrl = '../../ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash';

    config.filebrowserBrowseUrl = '/ckfinder/ckfinder.html';
     config.filebrowserImageBrowseUrl = '/ckfinder/ckfinder.html?Type=Images';
     config.filebrowserFlashBrowseUrl = '/ckfinder/ckfinder.html?Type=Flash';
     config.filebrowserUploadUrl = '/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files';
    config.filebrowserImageUploadUrl = '/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images';
     config.filebrowserFlashUploadUrl = '/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash';

};
