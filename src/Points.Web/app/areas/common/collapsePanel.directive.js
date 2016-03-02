(function() {
    'use strict';

    angular
        .module('checkpoint')
        .directive('collapsePanel', collapsePanel);
    
    function collapsePanel() {
        var directive = {
            restrict: 'EA',
            scope: {
                initOpen: '=',
                headerTitle: '=',
                headerClass: '='
            },
            templateUrl: '/app/areas/common/collapsePanel.html',
            transclude: true,
            controller: "collapsePanelController",
            controllerAs: 'cpVm'
        };
        return directive;
    }

    angular
        .module('checkpoint')
        .controller('collapsePanelController', collapsePanelController);

    function collapsePanelController() {
        /* jshint validthis:true */
        var cpVm = this;

        cpVm.panelClass = (cpVm.headerClass) ? cpVm.headerClass : 'panel-default';
        cpVm.isOpen = cpVm.initOpen;

        activate();

        function activate() { }

    }

})();