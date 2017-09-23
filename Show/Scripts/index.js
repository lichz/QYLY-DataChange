$(function(){
    // 轮播参数控制 --注释部分解除即可开始轮播
    jQuery(".slideBox").slide({mainCell:".bd ul",effect:"leftLoop",delayTime:1000
        //,autoPlay:true
    });
    // 楼宇展示参数控制
    jQuery(".picScroll-left").slide({titCell:".hd ul",mainCell:".bd ul",autoPage:true,effect:"leftLoop",autoPlay:true,vis:4,delayTime:1000});
    // 左侧导航
    $('.first_nav li').click(function(event) {
        $('.senc_nav').hide()
        $(this).find('.senc_nav').show();
    });
    $('.senc_nav li').click(function(event) {
        $(this).addClass('on').siblings().removeClass('on');
    });
    // 搜索框效果
    $('.diy_search .key_word1').click(function (event) {
       $('.key_list').slideUp(100);
       $(this).next('.key_list').slideToggle(100);
    });
    $('.key_list li').click(function (event) {
        $(this).parents('.key_list').hide();
        var keyword = $(this).text();
        var index = $(this).index();
        $(this).parents('.diy_search').find('.key_word1').text(keyword);
        $(this).parents('.diy_search').find('select').find("option").eq(index).attr("selected", true);
    });
    // 查询结果样式辅助控制
    function fzkz(){
        var lilength=$('.result_list li').length/3;
        for (var i =  0; i < lilength; i++) {
            $('.result_list li').eq(3*i+2).css('margin-right', '0px');
        };
    }
    // 导航跳转高亮
    var url = window.location.pathname;
    url2 = url.substr(1, url.lastIndexOf("/"));
    if (url2 == "Home/" || url2 == "") {
        $('.nav_list>li').eq(0).find('a').addClass('on');
    } else if (url2 == "SpecialQY/") {
        $('.nav_list>li').eq(1).find('a').addClass('on');
    } else if (url2 == "BuildingInformation/" || url2 == "BuildingInformation/Details/") {
        $('.nav_list>li').eq(2).find('a').addClass('on');
    } else if (url2 == "Buildings/" || url2=="Buildings/Details/") {
        $('.nav_list>li').eq(3).find('a').addClass('on');
    } else if (url2 == "Rental/" || url2 == "Rental/Details/") {
        $('.nav_list>li').eq(4).find('a').addClass('on');
    } else if (url2 == "Map/") {
        $('.nav_list>li').eq(5).find('a').addClass('on');
    }

    fzkz();
    //左侧一二级导航菜单
    var info1=$('.senc_nav li').eq(0).find("a").text();
    if (info1 == "投资导向" || info1 == "工作动态") {
        $('.senc_nav li').css('padding-left', '0');
    }
    // 租售结果样式辅助控制
     function fzkz2(){
        var lilength=$('.result2_ul li').length/2;
        for (var i =  0; i < lilength; i++) {
            $('.result2_ul li').eq(2*i+1).css('float', 'right');
        };
    }
    fzkz2();
})