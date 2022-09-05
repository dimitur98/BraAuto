(function ($) {

    /**
    * Constructor to create a new multiselect using the given select.
    *
    * @param {jQuery} select
    * @param {Object} options
    * @returns {LazyMultiselect}
    */
    function LazyMultiselect(element, options) {
        var self = this;
        var defaults = {
            buttonText: 'Items',
            class: '',
            buttonClass: '',
            noneSelectedText: null,
            noItemsText: 'No results found',
            selectAllText: 'Select All',
            selectAllClass: 'fw-bold font-weight-bold',
            maxHeight: '400px',
            filterPlaceholder: 'Search',
            filterClass: '',
            filterMatchClass: 'text-success',
            showOnlyFilterMatchesChecked: true,
            showOnlyFilterMatchesText: 'Show only search results',
            reloadOnOpen: false,
            autoLoad: true, // load the items during init() if the select has selected options
            itemsDisplayed: 2, // max items dispayed. if they are more, buttonText(n) format will be used
            listSelectedItemsFirst: true, // the selected items will be listed before the not selected items
            singleSelectMode: false, // only one checkbox can be checked. select all option is removed,
            values: [], // the default selected values
            onClose: undefined
        };

        this.$select = $(element);
        this.options = $.extend({}, defaults, options);

        this.$container = null;
        this.$btnToggle = null;
        this.$btnClearSelection = null;
        this.$menu = null;
        this.$loadingIcon = null;
        this.$noItemsMessage = null;
        this.$itemsList = null;
        this.$selectAll = null;
        this.$selectAllContainer = null;
        this.$filterContainer = null;
        this.$filter = null;
        this.$btnClearFilter = null;
        this.$showOnlyFilterMatchesContainer = null;
        this.$showOnlyFilterMatches = null;
        this.$showOnlyFilterMatchesText = null;

        this.init = function () {
            var id = this.$select.attr('id');

            // pupulate default values
            if (this.options.values && this.options.values.length) {
                this.$select.empty();

                for (var i = 0, length = this.options.values.length; i < length; i++) {
                    var val = this.options.values[i];

                    $('<option />')
                        .text(val.label || val)
                        .attr('value', val.value || val)
                        .attr('selected', 'selected')
                        .appendTo(this.$select);
                }
            }

            var selectedValues = this.$select.val();

            if (selectedValues === null) { selectedValues = []; }


            this.$select.hide();

            this.$container = $('<div class="dropdown lazy-multiselect"></div>')
                .addClass(this.options.class)
                .on('hidden.bs.dropdown', function (event) {
                    if (self.options.onClose) {
                        $.proxy(self.options.onClose, self)(event, self.$select.val(), self.areAllSelected());
                    }
                });

            this.$btnToggle = $('<a class="dropdown-toggle text-decoration-none text-truncate" data-toggle="dropdown" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false" href="#"></a>')
                .text(this.options.buttonText + (selectedValues.length > 0 ? ' (' + selectedValues.length + ')' : ''))
                .addClass(this.options.buttonClass)
                .on('click.lazymultiselect', function (event) {
                    if (!self.options.reloadOnOpen) {
                        $(this).off(event);
                    }

                    self.loadItems();
                })
                .appendTo(this.$container);

            this.$btnClearSelection = $('<a class="dropdown-clear-selection text-decoration-none position-absolute" href="#"></a>')
                .html('<i class="fas fa-times"></i>')
                .on('click.lazymultiselect', function (event) {
                    self.clearSelection();
                })
                .appendTo(this.$container);

            if (this.options.noneSelectedText && selectedValues.length <= 0) {
                this.$btnToggle.text(this.options.noneSelectedText);
            }

            this.$menu = $('<div class="dropdown-menu p-2" aria-labelledby="dropdownMenuButton"></div>')
                .css({
                    'max-height': this.options.maxHeight || 'auto',
                    'overflow-y': 'auto',
                    'overflow-x': 'auto'
                })
                .on('click', function (event) {
                    event.stopPropagation();
                })
                .appendTo(this.$container);

            this.$loadingIcon = $('<div class="text-center text-muted mx-5 my-5"><i class="fas fa-sync fa-spin fa-5x"></i></div>');

            this.$noItemsMessage = $('<div class="alert alert-danger mt-1 mb-1"></div>')
                .text(this.options.noItemsText);

            this.$itemsList = $('<ul class="pl-0 mb-0 list-unstyled"></ul>');


            // filter
            this.$filterContainer = $('<div class="input-group"></div>');

            this.$filter = $('<input type="text" class="form-control form-control-sm" />')
                .on('keydown', function () {
                    var keyCode = event.which ? event.which : event.keyCode;

                    if (event.preventDefault && keyCode == 13) {
                        event.preventDefault();

                        self.applyFilter();
                    }
                })
                .on('keyup', function (event) {
                    if (event.which == 13) { return; }

                    self.applyFilter();
                })
                .attr('placeholder', this.options.filterPlaceholder)
                .addClass(this.options.filterClass)
                .appendTo(this.$filterContainer);

            this.$btnClearFilter = $('<button class="btn btn-sm input-group-text" type="button"><i class="fa fa-times"></i></button>')
                .on('click', function () {
                    self.$filter.val('');
                    self.applyFilter();
                });

            $('<div class="input-group-append"></div>')
                .append(this.$btnClearFilter)
                .appendTo(this.$filterContainer);


            // select all
            this.$selectAllContainer = $('<label class="dropdown-item px-1 mb-0 w-100 text-nowrap"></label>')
                .addClass(this.options.selectAllClass + (self.options.singleSelectMode ? ' d-none' : ''));

            this.$selectAll = $('<input type="checkbox" class="me-2 mr-2" />')
                .on('change', function () {
                    self.selectAll();
                })
                .appendTo(this.$selectAllContainer);

            $('<span></span>')
                .text(this.options.selectAllText)
                .appendTo(this.$selectAllContainer);


            // showOnlyFilterMatches
            this.$showOnlyFilterMatchesContainer = $('<label class="dropdown-item px-1 mt-2 mb-0 w-100 text-nowrap bg-success text-white rounded"></label>');

            this.$showOnlyFilterMatches = $('<input type="checkbox" class="align-middle me-2 mr-2" />')
                .on('change', function () {
                    self.applyFilter();
                })
                .prop('checked', this.options.showOnlyFilterMatchesChecked)
                .appendTo(this.$showOnlyFilterMatchesContainer);

            this.$showOnlyFilterMatchesText = $('<span></span>')
                .appendTo(this.$showOnlyFilterMatchesContainer);

            if (id) {
                this.$container.attr('id', id + '-container');
            }

            this.$select.after(this.$container);
        };

        this.getAllCheckBoxes = function () {
            return this.$itemsList.find('input[type="checkbox"]');
        };

        this.getCheckedBoxes = function () {
            return this.$itemsList.find('input[type="checkbox"]:checked');
        };

        this.selectAll = function () {
            var checked = this.$selectAll.prop('checked');
            var allCheckBoxes = this.getAllCheckBoxes().filter(':visible');

            for (var i = 0, length = allCheckBoxes.length; i < length; i++) {
                var $cb = $(allCheckBoxes[i]);

                $cb.prop('checked', checked);
            }

            this.onChange();
        };

        this.clearSelection = function () {
            var allCheckBoxes = this.getAllCheckBoxes();

            for (var i = 0, length = allCheckBoxes.length; i < length; i++) {
                var $cb = $(allCheckBoxes[i]);

                $cb.prop('checked', false);
            }

            this.onChange();
        };

        this.onChange = function (triggerChange) {
            var $checkedBoxes = this.getCheckedBoxes();
            this.$select.empty();

            for (var i = 0, length = $checkedBoxes.length; i < length; i++) {
                var $cb = $($checkedBoxes[i]);
                var item = $cb.data('item');

                $('<option></option>')
                    .val(item.value)
                    .text(item.label)
                    .prop('selected', true)
                    .appendTo(this.$select);
            }

            if (triggerChange !== false) {
                this.$select.trigger('change');
            }


            if (this.options.noneSelectedText && $checkedBoxes.length <= 0) {
                this.$btnToggle.text(this.options.noneSelectedText);
            } else if (this.options.itemsDisplayed && $checkedBoxes.length <= this.options.itemsDisplayed) {
                this.$btnToggle.text($checkedBoxes.map(function () { return $(this).data('item').label; }).get().join(', '));
            } else {
                this.$btnToggle.text(this.options.buttonText + ($checkedBoxes.length > 0 ? ' (' + $checkedBoxes.length + ')' : ''));
            }

            if ($checkedBoxes.length > 0) {
                this.$container.addClass('has-selection');
            } else {
                this.$container.removeClass('has-selection');
            }

            this.refreshSelectAll();
        };

        this.refreshSelectAll = function () {

            var checkedBoxes = this.getCheckedBoxes();
            var allCheckBoxes = this.getAllCheckBoxes();
            var query = this.getFilterQuery();

            // if filter query is applied, select all should work only with the not hidden checkboxes 
            if (query != '') {
                checkedBoxes = checkedBoxes.filter(':visible');
                allCheckBoxes = allCheckBoxes.filter(':visible');
            }

            var areAllChecked = checkedBoxes.length == allCheckBoxes.length;

            this.$selectAll.prop('checked', areAllChecked);

            if (allCheckBoxes.length <= 0) {
                this.$selectAllContainer.hide();
            } else {
                this.$selectAllContainer.show();
            }
        };

        this.areAllSelected = function () {
            var checkedBoxes = this.getCheckedBoxes();
            var allCheckBoxes = this.getAllCheckBoxes();

            return checkedBoxes.length == allCheckBoxes.length;
        }

        this.loadItems = function () {
            var selectedValues = this.$select.val();

            if (selectedValues === null) { selectedValues = []; }

            this.$menu.css('min-width', this.$btnToggle.outerWidth() + 'px');
            this.$select.empty();
            this.$itemsList.remove().empty();
            this.$selectAll.prop('checked', false);
            this.$filterContainer.detach();
            this.$selectAllContainer.detach();
            this.$noItemsMessage.remove();
            this.$loadingIcon.appendTo(this.$menu);

            if (this.options.source) {
                this.options.source(function (items) {
                    if (items && items.length) {
                        var triggerOnChange = selectedValues.length > 0;

                        // mark items as selected if included in selectedValues
                        if (selectedValues.length > 0) {
                            for (var i = 0, length = items.length; i < length; i++) {
                                var item = items[i];

                                if (selectedValues.indexOf(item.value + '') > -1) {
                                    item.selected = true;
                                }
                            }
                        }

                        // put the selected items first
                        if (self.options.listSelectedItemsFirst) {
                            var sortedItems = [];

                            for (var i = 0, length = items.length; i < length; i++) {
                                var item = items[i];

                                if (item.selected == true) {
                                    sortedItems.push(item);
                                }
                            }

                            for (var i = 0, length = items.length; i < length; i++) {
                                var item = items[i];

                                if (item.selected != true) {
                                    sortedItems.push(item);
                                }
                            }

                            items = sortedItems;
                        }

                        for (var i = 0, length = items.length; i < length; i++) {
                            var item = items[i];
                            var $li = $('<li class="px-0"></li>');
                            var $label = $('<label class="dropdown-item px-1 mb-0 w-100 text-nowrap"></label>')
                                .appendTo($li);
                            var $cb = $('<input type="checkbox" class="me-2 me-2 mr-2 mr-2" />')
                                .val(item.value)
                                .css('margin-left', ((item.indent || 0) * 30) + 'px')
                                .on('change', function () {
                                    if (self.options.singleSelectMode) {
                                        var $this = $(this);
                                        var checked = $this.prop('checked');

                                        self.getAllCheckBoxes().prop('checked', false);
                                        $this.prop('checked', checked)
                                    }

                                    self.onChange();
                                })
                                .data('item', item)
                                .appendTo($label);
                            var $span = $('<span></span>')
                                .text(item.label)
                                .appendTo($label);

                            if (item.selected == true) {
                                $cb.prop('checked', true);
                            }

                            if (item.selected == true) {
                                triggerOnChange = true;
                            }

                            $li.appendTo(self.$itemsList);
                        }

                        self.$filterContainer.appendTo(self.$menu);
                        self.$selectAllContainer.appendTo(self.$menu);
                        self.$itemsList.appendTo(self.$menu);

                        if (triggerOnChange) {
                            self.onChange(false);
                        }
                    } else {
                        self.$noItemsMessage.appendTo(self.$menu);
                    }

                    self.$loadingIcon.remove();
                    self.$select.trigger('lazy:itemsLoaded');
                });
            }
        };

        this.getFilterQuery = function () {
            return this.$filter.val().toLowerCase().trim();
        };

        this.applyFilter = function () {
            var allCheckBoxes = this.getAllCheckBoxes();
            var query = this.getFilterQuery();
            var showOnlyFilterMatches = this.$showOnlyFilterMatches.prop('checked');
            var matchesCount = 0;

            this.$showOnlyFilterMatchesContainer.detach();
            this.$showOnlyFilterMatchesText.text(this.options.showOnlyFilterMatchesText);

            if (query != '') {
                this.$showOnlyFilterMatchesContainer.insertAfter(this.$filterContainer);
            }

            this.$itemsList.detach();

            for (var i = 0, length = allCheckBoxes.length; i < length; i++) {
                var $cb = $(allCheckBoxes[i]);
                var $label = $cb.parent();
                var $li = $label.parent();
                var item = $cb.data('item');

                $label.removeClass(this.options.filterMatchClass);
                $li.show();

                if (query == '') { continue; }

                if (item.label.toLowerCase().indexOf(query) > -1) {
                    $label.addClass(this.options.filterMatchClass);
                    matchesCount++;
                } else if (showOnlyFilterMatches) {
                    $li.hide();
                }
            }

            this.$itemsList.appendTo(self.$menu);

            this.$showOnlyFilterMatchesText.text(this.options.showOnlyFilterMatchesText + ' (' + matchesCount + ')');
            this.refreshSelectAll();
        };

        this.init();

        if (this.options.autoLoad && this.$select.val() != null && this.$select.val().length > 0) {
            if (!this.options.reloadOnOpen) {
                this.$btnToggle.off('click.lazymultiselect');
            }

            this.loadItems();
        }
    };





    $.fn.lazyMultiselect = function (options) {
        return this.each(function () {
            var obj = $(this).data('lazy-multiselect');

            if (!obj) {
                obj = new LazyMultiselect(this, options);

                $(this).data('lazy-multiselect', obj);
            } else if (typeof options === 'string') {

                switch (options) {
                    case 'show':
                        obj.$btnToggle.dropdown('show');
                        obj.loadItems();
                        break;

                    case 'hide':
                        obj.$btnToggle.dropdown('hide');
                        break;

                    case 'toggle':
                        obj.$btnToggle.dropdown('toggle');
                        break;

                    case 'clearSelection':
                        obj.clearSelection();
                        break;

                    case 'reload':
                        obj.loadItems();
                        break;

                    case 'destroy':
                        obj.$container.remove();
                        break;
                }
            }
        });
    };
})(jQuery);