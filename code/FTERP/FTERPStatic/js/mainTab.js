function tabs(obj,ulclass,con){
	$(obj).each(function(){
		var liSelector = 'ul[class*="' + ulclass + '"] li';
		$(this).find(liSelector).each(function(){
			$(this).bind('click',function(){
				var index = $(this).index();
				var content = $(this).parent('ul').siblings('div');
				var li = $(this).parent('ul').find('li');
				// 操作标签
				li.removeClass('selectTag');
				li.eq(index).addClass('selectTag');
				// 操作内容
				content.find(con).removeClass('selectTag');
				content.find(con).eq(index).addClass('selectTag');
			});	
		});
	});		
}



/*$(document).ready(function(){
	$('.con').each(function(){
		$(this).find('ul[class*="tags"] li').each(function(){
			$(this).bind('click',function(){
				var index = $(this).index();
				var content = $(this).parent('ul').siblings('div');
				var li = $(this).parent('ul').find('li');
				// 操作标签
				li.removeClass('selectTag');
				li.eq(index).addClass('selectTag');
				// 操作内容
				content.find('.tagContentCon').removeClass('selectTag');
				content.find('.tagContentCon').eq(index).addClass('selectTag');
			});	
		});
	});	
});
*/