(function() {
    'use strict';

    angular
        .module('checkpoint')
        .directive('basicPanel', basicPanel);
    
    function basicPanel() {
        var directive = {
            restrict: 'EA',
            scope: {
                headerTitle: '='
            },
            templateUrl: '/app/areas/common/basicPanel.html',
            transclude: true
        };
        return directive;
    }

})();