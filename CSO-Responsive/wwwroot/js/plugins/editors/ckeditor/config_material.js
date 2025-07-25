/**
 * @license Copyright (c) 2003-2017, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {

	// Define changes to default configuration here.
	// For complete reference see:
	// http://docs.ckeditor.com/#!/api/CKEDITOR.config


	// Setup
	// ------------------------------

    // RTL version uses different config file
    if (document.dir == "rtl") {
		config.skin = 'material_rtl';
	    config.contentsLangDirection = 'rtl';
	    config.dialog_buttonsOrder = 'rtl';
	    config.language = 'ar';
	    config.defaultLanguage = 'en';
    }
    else {
		config.skin = 'material';
    }

    // Load content styles
	config.contentsCss = '../../../../js/plugins/editors/ckeditor/skins/' + config.skin + '/contents.css';


	// Toolbar
	// ------------------------------

	// The toolbar groups arrangement, optimized for two toolbar rows.
	config.toolbarGroups = [
		{ name: 'clipboard',   groups: [ 'clipboard', 'undo' ] },
		{ name: 'editing',     groups: [ 'find', 'selection', 'spellchecker' ] },
		{ name: 'links' },
		{ name: 'insert' },
		{ name: 'forms' },
		{ name: 'tools' },
		{ name: 'document',	   groups: [ 'mode', 'document', 'doctools' ] },
		{ name: 'others' },
		'/',
		{ name: 'basicstyles', groups: [ 'basicstyles', 'cleanup' ] },
		{ name: 'paragraph',   groups: [ 'list', 'indent', 'blocks', 'align', 'bidi' ] },
		{ name: 'styles' },
		{ name: 'colors' },
		{ name: 'about' }
	];


	// Extra config
	// ------------------------------

	// Set the most common block elements.
	config.format_tags = 'p;h1;h2;h3;h4;h5;h6;pre';

	// Simplify the dialog windows.
	config.removeDialogTabs = 'image:advanced;link:advanced';

	// Allow content rules
	config.allowedContent = true;

	// Allow any class and any inline style
	config.extraAllowedContent = '*(*);*{*}';
};
