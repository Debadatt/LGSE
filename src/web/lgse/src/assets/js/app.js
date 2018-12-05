window.jQuery = window.$ = jQuery;



+function($, window){ 'use strict';

	var app = {
		name: 'wales',
		version: '2.0.0'
	};

	app.defaults = {
		menubar: {
			folded: false,
			theme: 'light',
			themes: ['light', 'dark']
		},
		navbar: {
			theme: 'primary',
			themes: ['primary', 'success', 'warning', 'danger', 'pink', 'purple', 'inverse', 'dark']
		}
	};

	// Cache DOM
	app.$body = $('body');
	app.$menubar = $('#menubar');
	app.$appMenu = app.$menubar.find('.app-menu').first();
	app.$navbar = $('#app-navbar');
	app.$main = $('#app-main');

	// Which is the original loaded layout ?
	app.defaultLayout = app.$body.hasClass('menubar-left');
	app.topbarLayout = app.$body.hasClass('menubar-top');

	// TODO
	app.settings = app.defaults;

	var appSettings = app.name+app.version+"Settings";
	app.storage = $.localStorage;

	if (app.storage.isEmpty(appSettings)) {
		app.storage.set(appSettings, app.settings);
	}
	else {
		app.settings = app.storage.get(appSettings);
	}

	app.saveSettings = function() {
		app.storage.set(appSettings, app.settings);
	};

	// initialize navbar
	app.$navbar.removeClass('primary').addClass(app.settings.navbar.theme).addClass('in');
	app.$body.removeClass('theme-primary').addClass('theme-'+app.settings.navbar.theme);

	// initialize menubar
	app.$menubar.removeClass('light').addClass(app.settings.menubar.theme).addClass('in');
	app.$body.removeClass('menubar-light').addClass('menubar-'+app.settings.menubar.theme);

	// initialize main
	app.$main.addClass('in');
	
	app.init = function() {

		$('[data-plugin]').plugins();
		$('.scrollable-container').perfectScrollbar();

		// load some needed libs listed at: LIBS.others => library.js
		var loadingLibs = loader.load(LIBS["others"]);
		
		loadingLibs.done(function(){

			$('[data-switchery]').each(function(){
				var $this = $(this),
						color = $this.attr('data-color') || '#188ae2',
						jackColor = $this.attr('data-jackColor') || '#ffffff',
						size = $this.attr('data-size') || 'default'

				new Switchery(this, {
					color: color,
					size: size,
					jackColor: jackColor
				});
			});
		});
	};

	window.app = app;
}(jQuery, window);


// menubar MODULE
// =====================

+function($, window){ 'use strict';
	// Cache DOM
	
	var $body = app.$body,
			$menubar = app.$menubar,
			$appMenu = app.$appMenu,
			$menubarFoldButton = $('#menubar-fold-btn'),
			$menubarToggleButton = $('#menubar-toggle-btn');
var $navbar_img = app.$navbar.find('.navbar-brand');
	// menubar object
	var menubar = {
		open: false,
		folded: app.settings.menubar.folded,
		scrollInitialized: false,

		init: function() {
			app.defaultLayout && this.folded && this.fold();

			this.listenForEvents();
		},

		listenForEvents: function() {
			var self = this;

			// break points code
			
			// folding and unfolding the menubar
			$(document).on('click','.menubar-fold-btn', function(e){
				!self.folded ? self.fold() : self.unFold();
				e.preventDefault();
			});

			// showing and hiding the menubar
			$(document).on('click','.menubar-toggle-btn' , function(e){
				self.open ? self.pushOut() : self.pullIn();
				e.preventDefault();
			});

			// toggling submenus when the menubar is folded
			$(document).on('mouseenter mouseleave', 'body.menubar-fold ul.app-menu > li.has-submenu', function(e){
				$(this).toggleClass('open').siblings('li').removeClass('open');
			});

			// toggling submenus in the (topbar) layout
			$(document).on('mouseenter mouseleave', 'body.menubar-top ul.app-menu li.has-submenu', self.toggleTopbarSubmneuOnHover);

			// toggling submenus on click
			$(document).on('click', 'body.menubar-unfold .app-menu .submenu-toggle, body.menubar-fold .app-menu .submenu .submenu-toggle', self.toggleSubmenuOnClick);

			// As other browsers already fire the change event,

			// readjust the scroll height on resize and orientationchange
			$(window).on('resize orientationchange', self.readjustScroll);
		},

		setDefaultLayout: function() {
			app.$body.removeClass('menubar-top').addClass('menubar-left menubar-unfold');
			return true;
		},

		setTopbarLayout: function() {
			app.$body.removeClass('menubar-left menubar-unfold menubar-fold').addClass('menubar-top');
			return true;
		},

		cloneAppUser: function() {
			var $navbarCollapse = $('.navbar .navbar-collapse');
			if ($navbarCollapse.find('.app-user').length == 0){
				$menubar.find('.app-user').clone().appendTo($navbarCollapse);
			}
			return true;
		},

		foldAppUser: function() {
			$('.app-user .avatar').addClass('dropdown').find('>a').attr('data-toggle', 'dropdown');
			$('.app-user .dropdown-menu').first().clone().appendTo('.app-user .avatar')
			return true;
		},

		reduceAppMenu: function(){
			var $appMenu = $('body.menubar-top .app-menu');
			// if the menu is already customized return true
			if ($appMenu.find('>li.more-items-li').length) return true;

			var $menuItems = $appMenu.find('> li:not(.menu-separator)');
			if ($menuItems.length > 5) {
				var $moreItemsLi = $('<li class="more-items-li has-submenu"></li>'),
						$moreItemsUl = $('<ul class="submenu"></ul>'),
						$moreItemsToggle = $('<a href="javascript:void(0)" class="submenu-toggle"></a>');
				$moreItemsToggle.append(['<i class="menu-icon zmdi zmdi-more-vert zmdi-hc-lg"></i>', '<span class="menu-text">More...</span>', '<i class="menu-caret zmdi zmdi-hc-sm zmdi-chevron-right"></i>']);

				$menuItems.each(function(i, item){
					if (i >= 5) $(item).clone().appendTo($moreItemsUl);
				});
				
				$moreItemsLi.append([$moreItemsToggle, $moreItemsUl]).insertAfter($appMenu.find('>li:nth-child(5)'));
			}

			$(document).trigger('app-menu.reduce.done');

			return true;
		},

		toggleSubmenuOnClick: function(e) {
			$(this).parent().toggleClass('open').find('> .submenu').slideToggle(500).end().siblings().removeClass('open').find('> .submenu').slideUp(500);
			e.preventDefault();
		},

		toggleTopbarSubmneuOnHover: function(e){
			var $this = $(this), total = $this.offset().left + $this.width();
			var ww = $(window).width();
			if ((ww - total) < 220) {
				$this.find('> .submenu').css({left: 'auto', right: '100%'});
			} else if ((ww - total) >= 220 && !$this.is('.app-menu > li')) {
				$this.find('> .submenu').css({left: '100%', right: 'auto'});
			}
			$(this).toggleClass('open').siblings().removeClass('open');
		},

		fold: function() {
			
			$body.removeClass('menubar-unfold').addClass('menubar-fold');
			$('#menubar-fold-btn').removeClass('is-active');
			$('.navbar-brand').find('img').hide();
			$('.navbar-brand').find('img:first-child').removeClass('hidden-lg hidden-md hidden-xs').css('display','inline');
			this.toggleScroll() && this.toggleMenuHeading() && (this.folded = true);
			$('body #menubar').find('.app-menu').first().find('li.open').removeClass('open') && $appMenu.find('.submenu').slideUp();
			return true;
		},

		unFold: function() {
		
			$body.removeClass('menubar-fold').addClass('menubar-unfold');
			$('#menubar-fold-btn').addClass('is-active');
			// initialize the scroll if it's not initialized
			$('.navbar-brand').find('img').hide();
			$('.navbar-brand').find('img:first-child').addClass('hidden-lg hidden-md hidden-xs');
			$('.navbar-brand').find('img').show();
			this.scrollInitialized || this.initScroll();
			this.toggleScroll() && this.toggleMenuHeading() && (this.folded = false);
			$('body #menubar').find('.app-menu').first().find('li.open').removeClass('open') && $appMenu.find('.submenu').slideUp();
			return true;
		},

		pullIn: function() {
			$body.addClass('menubar-in') && $('.menubar-toggle-btn').addClass('is-active') && (this.open = true);
			return true;
		},
		
		pushOut: function() {
			$body.removeClass('menubar-in') && $('.menubar-toggle-btn').removeClass('is-active') && (this.open = false);
			return true;
		},

		initScroll: function() {
			var $scrollInner = $('body.menubar-left.menubar-unfold .menubar-scroll-inner');
			if (!this.scrollInitialized) {
				$scrollInner.slimScroll({
					height: 'auto',
					position: 'right',
					size: "5px",
					color: '#98a6ad',
					wheelStep: 10,
					touchScrollStep: 50
				});
				this.scrollInitialized = true;
			}
			return true;
		},

		readjustScroll: function(e){
			if ($body.hasClass('menubar-top') || this.folded) return;

			var parentHeight = $menubar.height(),
					$targets = $('.menubar-scroll, .menubar-scroll-inner, .slimScrollDiv');
			if (Breakpoints.current().name === 'xs') {
				$targets.height(parentHeight);
			} else {
				$targets.height(parentHeight - 75);
			}
		},

		toggleScroll: function(){
			var $scrollContainer = $('.menubar-scroll-inner');
			if(!$body.hasClass('menubar-unfold')){
				$scrollContainer.css('overflow', 'inherit').parent().css('overflow', 'inherit');
				$scrollContainer.siblings('.slimScrollBar').css('visibility', 'hidden');
				$scrollContainer.parent().css('height', '100%');
				$scrollContainer.css('height', '100%');
				
			} else{
				$scrollContainer.css('overflow', 'hidden').parent().css('overflow', 'hidden');
				$scrollContainer.siblings('.slimScrollBar').css('visibility', 'visible');
			}
			return true;
		},

		toggleMenuHeading: function() {
			if ($body.hasClass("menubar-fold")) {
				$('.app-menu > li:not(.menu-separator)').each(function(i, item){
					if (!$(item).hasClass('has-submenu')) {
						$(item).addClass('has-submenu').append('<ul class="submenu"></ul>');
					}
					if(!$('has-submenu .submenu').find('li.menu-heading')) {
						var href = $(item).find('a:first-child').attr("href");
						var menuHeading = $(item).find('> a > .menu-text').text();
						$(item).find('.submenu').first().prepend('<li class="menu-heading"><a href="'+href+'">'+menuHeading+'</a></li>');
					}
					
				});
			} else {
				$appMenu.find('.menu-heading').remove();
			}

			return true;
		},

		highlightOpenLink: function() {
			var currentPageName = location.pathname.slice(location.pathname.lastIndexOf('/') + 1),
					currentPageLink = $appMenu.find('a[href="'+currentPageName+'"]').first();
			
			currentPageLink.parents('li').addClass('active');

			if ($body.hasClass('menubar-left') && !this.folded) {
				currentPageLink.parents('.has-submenu').addClass('open').find('>.submenu').slideDown(500);
			}

			return true;
		}

		
	};

	window.app.menubar = menubar;
}(jQuery, window);


// NAVBAR MODULE
// =====================

+function($, window){ 'use strict';
	
	// Cache DOM
	var $body = app.$body,
			$navbar = app.$navbar;

	var navbar = {

		init: function() {
			this.listenForEvents();
		},

		listenForEvents: function() {
			$(document).on("click", '[data-toggle="collapse"]', function(e) {
				var $trigger = $(e.target);
				$trigger.is('[data-toggle="collapse"]') || ($trigger = $trigger.parents('[data-toggle="collapse"]'));
				var $target = $($trigger.attr('data-target'));
				if ($target.attr('id') === 'navbar-search') {
					if (!$trigger.hasClass('collapsed')) {
						var $field = $target.find('input[type="search"]').focus();
						document.querySelector($field.selector).setSelectionRange(0, $field.val().length);
					} else {
						$target.find('input[type="search"]').blur();
					}
				} else if ($target.attr('id') === 'app-navbar-collapse') {
					$body.toggleClass('navbar-collapse-in', !$trigger.hasClass('collapsed'));
				}
				e.preventDefault();
			});
		}

		
	};
	window.app.navbar = navbar;
}(jQuery, window);


// initialize app
+function($, window) { 'use strict';
	window.app.init();
	window.app.menubar.init();
	window.app.navbar.init();
	window.app.customizer.init();
}(jQuery, window);

// other
+function($, window) { 'use strict';

	$(window).on('load resize orientationchange', function(){
		// readjust panels on load, resize and orientationchange
		readjustActionPanel();

		// activate bootstrap tooltips
		$('[data-toggle="tooltip"]').tooltip();
	});

	function readjustActionPanel(){
		var $actionPanel = $('.app-action-panel');
		if (!$actionPanel.length > 0) return;
		var $actionList = $actionPanel.children('.app-actions-list').first();
		$actionList.height($actionPanel.height() - $actionList.position().top);
	}

}(jQuery, window);

